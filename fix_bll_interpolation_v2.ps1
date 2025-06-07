# MES.BLL层字符串插值修复脚本 v2
# 简化版本，逐行处理

Write-Host "开始修复MES.BLL层字符串插值..." -ForegroundColor Green

$files = @(
    "src\MES.BLL\Equipment\EquipmentBLL.cs",
    "src\MES.BLL\Production\ProductionOrderBLL.cs", 
    "src\MES.BLL\Quality\QualityInspectionBLL.cs",
    "src\MES.BLL\System\RoleBLL.cs",
    "src\MES.BLL\Workshop\BatchBLL.cs",
    "src\MES.BLL\Workshop\WorkshopBLL.cs"
)

$totalReplacements = 0

foreach ($filePath in $files) {
    if (Test-Path $filePath) {
        Write-Host "处理文件: $filePath" -ForegroundColor Yellow
        
        $lines = Get-Content $filePath -Encoding UTF8
        $modified = $false
        $fileReplacements = 0
        
        for ($i = 0; $i -lt $lines.Count; $i++) {
            $line = $lines[$i]
            $originalLine = $line
            
            # 简单的字符串插值替换模式
            # 处理常见的日志记录模式
            
            # 模式1: LogManager.Error($"message: {variable}")
            if ($line -match 'LogManager\.(Error|Info|Warn|Debug)\(\$"([^"]*)"') {
                $method = $matches[1]
                $message = $matches[2]
                
                # 提取变量
                $variables = @()
                $formatStr = $message
                $index = 0
                
                while ($formatStr -match '\{([^}]+)\}') {
                    $var = $matches[1]
                    $variables += $var
                    $formatStr = $formatStr -replace [regex]::Escape("{$var}"), "{$index}"
                    $index++
                }
                
                if ($variables.Count -gt 0) {
                    $varsStr = $variables -join ", "
                    $newLine = $line -replace '\$"[^"]*"', "string.Format(`"$formatStr`", $varsStr)"
                } else {
                    $newLine = $line -replace '\$"([^"]*)"', '`"$1`"'
                }
                
                $lines[$i] = $newLine
                $modified = $true
                $fileReplacements++
            }
            
            # 模式2: throw new MESException($"message: {variable}")
            elseif ($line -match 'throw new MESException\(\$"([^"]*)"') {
                $message = $matches[1]
                
                $variables = @()
                $formatStr = $message
                $index = 0
                
                while ($formatStr -match '\{([^}]+)\}') {
                    $var = $matches[1]
                    $variables += $var
                    $formatStr = $formatStr -replace [regex]::Escape("{$var}"), "{$index}"
                    $index++
                }
                
                if ($variables.Count -gt 0) {
                    $varsStr = $variables -join ", "
                    $newLine = $line -replace '\$"[^"]*"', "string.Format(`"$formatStr`", $varsStr)"
                } else {
                    $newLine = $line -replace '\$"([^"]*)"', '`"$1`"'
                }
                
                $lines[$i] = $newLine
                $modified = $true
                $fileReplacements++
            }
            
            # 模式3: 其他简单的字符串插值
            elseif ($line -match '\$"([^"]*)"') {
                $message = $matches[1]
                
                $variables = @()
                $formatStr = $message
                $index = 0
                
                while ($formatStr -match '\{([^}]+)\}') {
                    $var = $matches[1]
                    $variables += $var
                    $formatStr = $formatStr -replace [regex]::Escape("{$var}"), "{$index}"
                    $index++
                }
                
                if ($variables.Count -gt 0) {
                    $varsStr = $variables -join ", "
                    $newLine = $line -replace '\$"[^"]*"', "string.Format(`"$formatStr`", $varsStr)"
                } else {
                    $newLine = $line -replace '\$"([^"]*)"', '`"$1`"'
                }
                
                $lines[$i] = $newLine
                $modified = $true
                $fileReplacements++
            }
        }
        
        if ($modified) {
            Set-Content -Path $filePath -Value $lines -Encoding UTF8
            Write-Host "  修复了 $fileReplacements 个字符串插值" -ForegroundColor Green
            $totalReplacements += $fileReplacements
        } else {
            Write-Host "  无需修复" -ForegroundColor Gray
        }
    }
}

Write-Host "`n修复完成! 总计修复: $totalReplacements 个字符串插值" -ForegroundColor Green

# 验证结果
$remaining = Get-ChildItem -Path "src\MES.BLL" -Recurse -Filter "*.cs" | Select-String -Pattern '\$".*"'
if ($remaining.Count -eq 0) {
    Write-Host "✅ 所有字符串插值已修复!" -ForegroundColor Green
} else {
    Write-Host "⚠️ 还有 $($remaining.Count) 个未修复" -ForegroundColor Yellow
}
