# 批量修复字符串插值语法脚本
# 将 C# 6.0 的 $"" 语法转换为 .NET Framework 4.8 兼容的 string.Format() 语法

Write-Host "开始批量修复字符串插值语法..." -ForegroundColor Green

# 获取所有需要修复的 .cs 文件
$files = Get-ChildItem -Path "src" -Recurse -Filter "*.cs" | Where-Object { 
    $_.FullName -notlike "*\bin\*" -and 
    $_.FullName -notlike "*\obj\*" 
}

Write-Host "找到 $($files.Count) 个 C# 文件需要检查" -ForegroundColor Yellow

$totalFixed = 0
$filesFixed = 0

foreach ($file in $files) {
    Write-Host "检查文件: $($file.Name)" -ForegroundColor Cyan
    
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    $originalContent = $content
    $fileFixed = 0
    
    # 修复简单的字符串插值模式
    # 模式1: $"{var}" -> string.Format("{0}", var)
    $content = $content -replace '\$"([^"]*)\{([^}]+)\}([^"]*)"', 'string.Format("$1{0}$3", $2)'
    
    # 模式2: $"{var1} - {var2}" -> string.Format("{0} - {1}", var1, var2)
    $content = $content -replace '\$"([^"]*)\{([^}]+)\}([^"]*)\{([^}]+)\}([^"]*)"', 'string.Format("$1{0}$3{1}$5", $2, $4)'
    
    # 模式3: $"{var1} - {var2} ({var3})" -> string.Format("{0} - {1} ({2})", var1, var2, var3)
    $content = $content -replace '\$"([^"]*)\{([^}]+)\}([^"]*)\{([^}]+)\}([^"]*)\{([^}]+)\}([^"]*)"', 'string.Format("$1{0}$3{1}$5{2}$7", $2, $4, $6)'
    
    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8
        $filesFixed++
        Write-Host "  ✓ 已修复" -ForegroundColor Green
    } else {
        Write-Host "  - 无需修复" -ForegroundColor Gray
    }
}

Write-Host "`n修复完成!" -ForegroundColor Green
Write-Host "修复的文件数: $filesFixed" -ForegroundColor Yellow
Write-Host "请重新编译项目验证修复结果" -ForegroundColor Cyan
