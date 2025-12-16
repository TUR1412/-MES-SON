param(
    # 读取连接串的配置文件路径（默认使用项目的 UI 配置）
    [string]$ConfigPath = "src/MES.UI/App.config",

    # 连接串名称
    [string]$ConnectionName = "MESConnectionString",

    # Navicat 导出的数据库脚本
    [string]$ScriptPath = "database/create_mes_database.sql",

    # 若数据库已存在，默认拒绝执行（避免 DROP TABLE 造成数据丢失）；加 -Force 才允许重置导入
    [switch]$Force
)

$ErrorActionPreference = "Stop"

function Write-Info([string]$Message) {
    Write-Host ("[信息] " + $Message)
}

function Write-Warn([string]$Message) {
    Write-Host ("[警告] " + $Message)
}

function Write-Err([string]$Message) {
    Write-Host ("[错误] " + $Message)
}

function Resolve-WorkspacePath([string]$Path) {
    if ([string]::IsNullOrWhiteSpace($Path)) { return $null }
    if ([System.IO.Path]::IsPathRooted($Path)) { return $Path }
    return (Join-Path (Get-Location) $Path)
}

function Find-MySqlDataDll() {
    $candidates = @(
        "packages/MySql.Data.*/lib/net48/MySql.Data.dll",
        "src/MES.UI/bin/Release/MySql.Data.dll",
        "src/MES.UI/bin/Debug/MySql.Data.dll"
    )

    foreach ($pattern in $candidates) {
        $hits = Get-ChildItem -Path $pattern -ErrorAction SilentlyContinue
        if ($hits) {
            return ($hits | Sort-Object FullName -Descending | Select-Object -First 1).FullName
        }
    }

    return $null
}

function Ensure-AllowPublicKeyRetrieval([string]$ConnectionString) {
    if ([string]::IsNullOrWhiteSpace($ConnectionString)) { return $ConnectionString }

    if ($ConnectionString -match "(?i)AllowPublicKeyRetrieval\s*=") {
        return $ConnectionString
    }

    if ($ConnectionString -notmatch "(?i)SslMode\s*=\s*none") {
        return $ConnectionString
    }

    if (-not $ConnectionString.TrimEnd().EndsWith(";")) {
        $ConnectionString += ";"
    }
    return ($ConnectionString + "AllowPublicKeyRetrieval=true;")
}

try {
    $configFullPath = Resolve-WorkspacePath $ConfigPath
    $scriptFullPath = Resolve-WorkspacePath $ScriptPath

    if (-not (Test-Path $configFullPath)) {
        throw "未找到配置文件：$configFullPath"
    }
    if (-not (Test-Path $scriptFullPath)) {
        throw "未找到数据库脚本：$scriptFullPath"
    }

    $dllPath = Find-MySqlDataDll
    if (-not $dllPath -or -not (Test-Path $dllPath)) {
        throw "未找到 MySql.Data.dll（请先还原 NuGet 包或执行一次构建）。"
    }

    Write-Info "加载 MySql.Data.dll：$dllPath"
    Add-Type -Path $dllPath

    [xml]$xml = Get-Content -Path $configFullPath -Encoding UTF8
    $connNode = $xml.configuration.connectionStrings.add | Where-Object { $_.name -eq $ConnectionName } | Select-Object -First 1
    if (-not $connNode) {
        throw "在 $configFullPath 中未找到连接串：$ConnectionName"
    }

    $connectionString = [string]$connNode.connectionString
    $connectionString = Ensure-AllowPublicKeyRetrieval $connectionString

    $builder = New-Object MySql.Data.MySqlClient.MySqlConnectionStringBuilder($connectionString)
    $databaseName = [string]$builder.Database
    if ([string]::IsNullOrWhiteSpace($databaseName)) {
        $databaseName = "mes_db"
    }

    Write-Info "目标数据库：$databaseName"
    Write-Info "脚本文件：$scriptFullPath"

    # 连接到 information_schema（必定存在）以便检查/创建数据库
    $serverBuilder = New-Object MySql.Data.MySqlClient.MySqlConnectionStringBuilder($connectionString)
    $serverBuilder.Database = "information_schema"

    $serverConn = New-Object MySql.Data.MySqlClient.MySqlConnection($serverBuilder.ConnectionString)
    $serverConn.Open()
    try {
        # 检查数据库是否存在
        $checkCmd = $serverConn.CreateCommand()
        $checkCmd.CommandTimeout = 60
        $checkCmd.CommandText = "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @db;"
        [void]$checkCmd.Parameters.AddWithValue("@db", $databaseName)
        $exists = $checkCmd.ExecuteScalar()

        if ($exists) {
            if (-not $Force) {
                Write-Warn "数据库已存在：$databaseName"
                Write-Warn "由于脚本包含 DROP TABLE，默认不会继续执行，避免误删你的真实数据。"
                Write-Warn "如确认要重置导入，请加参数：-Force"
                exit 2
            }
            Write-Warn "已启用 -Force，将对现有库执行重置导入（包含 DROP TABLE）。"
        }

        $createCmd = $serverConn.CreateCommand()
        $createCmd.CommandTimeout = 60
        $safeDb = $databaseName.Replace("`", "``")
        # 注意：PowerShell 的反引号是转义符，这里用 `` 输出 SQL 的反引号字符
        $createCmd.CommandText = ("CREATE DATABASE IF NOT EXISTS ``{0}`` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;" -f $safeDb)
        [void]$createCmd.ExecuteNonQuery()
        Write-Info "数据库已确保存在：$databaseName"
    }
    finally {
        $serverConn.Close()
        $serverConn.Dispose()
    }

    # 连接到目标库执行脚本
    $dbBuilder = New-Object MySql.Data.MySqlClient.MySqlConnectionStringBuilder($connectionString)
    $dbBuilder.Database = $databaseName

    $dbConn = New-Object MySql.Data.MySqlClient.MySqlConnection($dbBuilder.ConnectionString)
    $dbConn.Open()
    try {
        Write-Info "开始导入脚本（可能需要 10-60 秒，取决于机器与数据量）..."
        $sql = Get-Content -Path $scriptFullPath -Raw -Encoding UTF8
        $script = New-Object MySql.Data.MySqlClient.MySqlScript($dbConn, $sql)
        $count = $script.Execute()
        Write-Info ("导入完成，执行语句数（MySqlScript 统计）：{0}" -f $count)
    }
    finally {
        $dbConn.Close()
        $dbConn.Dispose()
    }

    Write-Info "OK：MES 数据库已初始化完成。"
    exit 0
}
catch {
    Write-Err $_.Exception.Message
    exit 1
}
