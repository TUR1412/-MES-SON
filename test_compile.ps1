# MES项目编译测试脚本
Write-Host "开始编译测试..." -ForegroundColor Green

# 查找MSBuild
$msbuildPaths = @(
    "C:\Program Files\Microsoft Visual Studio\2022\*\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files (x86)\Microsoft Visual Studio\2022\*\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2019\*\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files (x86)\Microsoft Visual Studio\2019\*\MSBuild\Current\Bin\MSBuild.exe"
)

$msbuild = $null
foreach ($path in $msbuildPaths) {
    $found = Get-ChildItem -Path $path -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) {
        $msbuild = $found.FullName
        break
    }
}

if (-not $msbuild) {
    Write-Host "错误：未找到MSBuild.exe" -ForegroundColor Red
    Write-Host "请安装Visual Studio 2019/2022" -ForegroundColor Yellow
    exit 1
}

Write-Host "找到MSBuild: $msbuild" -ForegroundColor Cyan

# 编译项目
$projects = @(
    "src\MES.Models\MES.Models.csproj",
    "src\MES.Common\MES.Common.csproj", 
    "src\MES.DAL\MES.DAL.csproj",
    "src\MES.BLL\MES.BLL.csproj"
)

$success = $true

foreach ($project in $projects) {
    $projectName = Split-Path $project -Leaf
    Write-Host "`n正在编译 $projectName..." -ForegroundColor Yellow
    
    $result = & $msbuild $project /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:minimal /nologo
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ $projectName 编译成功" -ForegroundColor Green
    } else {
        Write-Host "❌ $projectName 编译失败" -ForegroundColor Red
        Write-Host "详细错误信息：" -ForegroundColor Yellow
        & $msbuild $project /p:Configuration=Debug /p:Platform="Any CPU" /verbosity:normal
        $success = $false
        break
    }
}

if ($success) {
    Write-Host "`n🎉 所有项目编译成功！" -ForegroundColor Green
} else {
    Write-Host "`n❌ 编译失败，请检查错误信息" -ForegroundColor Red
    exit 1
}
