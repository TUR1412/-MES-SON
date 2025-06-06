# MES.BLL层字符串插值批量修复脚本
# 将所有 $"..." 语法转换为 string.Format(...) 语法以兼容.NET Framework 4.8

Write-Host "开始修复MES.BLL层字符串插值语法..." -ForegroundColor Green

# 定义转换函数
function Convert-StringInterpolation {
    param([string]$interpolatedString)

    $formatString = $interpolatedString
    $arguments = @()
    $argIndex = 0

    # 查找 {expression} 模式并替换
    while ($formatString -match '\{([^}]+)\}') {
        $expression = $matches[1]
        $arguments += $expression
        $formatString = $formatString -replace [regex]::Escape("{$expression}"), "{$argIndex}"
        $argIndex++
    }

    if ($arguments.Count -gt 0) {
        $argsStr = $arguments -join ', '
        return "string.Format(`"$formatString`", $argsStr)"
    } else {
        return "`"$formatString`""
    }
}

# 获取所有需要修复的C#文件
$files = Get-ChildItem -Path "src\MES.BLL" -Recurse -Filter "*.cs"
$totalFiles = 0
$totalReplacements = 0

foreach ($file in $files) {
    Write-Host "处理文件: $($file.Name)" -ForegroundColor Yellow

    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    $fileReplacements = 0

    # 使用简单的正则替换
    $pattern = '\$"([^"]*)"'
    $content = [regex]::Replace($content, $pattern, {
        param($match)
        $interpolatedString = $match.Groups[1].Value
        $script:fileReplacements++
        return Convert-StringInterpolation $interpolatedString
    })

    if ($fileReplacements -gt 0) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8
        Write-Host "  完成替换 $fileReplacements 个字符串插值" -ForegroundColor Green
        $totalFiles++
        $totalReplacements += $fileReplacements
    }
}

Write-Host "`n修复完成!" -ForegroundColor Green
Write-Host "处理文件数: $totalFiles" -ForegroundColor White
Write-Host "总替换数: $totalReplacements" -ForegroundColor White
