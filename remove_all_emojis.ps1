# 批量删除所有emoji的PowerShell脚本
# 严格遵循C# 5.0语法规范

Write-Host "开始批量删除所有emoji..."

# 定义emoji列表
$emojis = @(
    "📦", "📋", "📊", "🔧", "⚙️", "🎮", "✅", "❌", "🚨", "🔍", 
    "🎯", "🛡️", "📈", "📉", "💡", "🔄", "⭐", "🌟", "💎", "🎨",
    "🎪", "🎭", "🎲", "🎳", "🎸", "🎹", "🎺", "🎻", "🎼", "🎵",
    "🎶", "🎤", "🎧", "📻", "📺", "📷", "📹", "📼", "💿", "📀",
    "💽", "💾", "💻", "📱", "☎️", "📞", "📟", "📠", "📡", "📢",
    "📣", "📯", "🔔", "🔕", "📩", "📨", "📧", "📮", "📪", "📫",
    "📬", "📭", "📰", "📑", "📄", "📃", "📌", "📍", "📎", "📏",
    "📐", "✂️", "🔒", "🔓", "🔏", "🔐", "🔑", "🗝️", "🔨", "⛏️",
    "⚒️", "🛠️", "🗡️", "⚔️", "🔫", "🏹", "🔩", "🗜️", "⚖️", "🔗",
    "⛓️", "🧰", "🧲", "⚗️", "🧪", "🧫", "🧬", "🔬", "🔭", "💉",
    "💊", "🩹", "🩺", "🚪", "🛏️", "🛋️", "🚽", "🚿", "🛁", "🧴",
    "🧷", "🧹", "🧺", "🧻", "🧼", "🧽", "🧯", "🛒", "🚬", "⚰️",
    "⚱️", "🗿", "🏧", "🚮", "🚰", "♿", "🚹", "🚺", "🚻", "🚼",
    "🚾", "🛂", "🛃", "🛄", "🛅", "⚠️", "🚸", "⛔", "🚫", "🚳",
    "🚭", "🚯", "🚱", "🚷", "📵", "🔞", "☢️", "☣️", "⬆️", "↗️",
    "➡️", "↘️", "⬇️", "↙️", "⬅️", "↖️", "↕️", "↔️", "↩️", "↪️",
    "⤴️", "⤵️", "🔃", "🔙", "🔚", "🔛", "🔜", "🔝", "💾"
)

# 获取所有.cs和.Designer.cs文件
$files = Get-ChildItem -Path "D:\source\-MES-SON\src\MES.UI" -Recurse -Include "*.cs" | Where-Object { $_.Name -notlike "*.resx" }

$totalFiles = $files.Count
$processedFiles = 0
$totalReplacements = 0

foreach ($file in $files) {
    $processedFiles++
    Write-Progress -Activity "处理文件" -Status "正在处理: $($file.Name)" -PercentComplete (($processedFiles / $totalFiles) * 100)
    
    try {
        $content = Get-Content -Path $file.FullName -Encoding UTF8 -Raw
        $originalContent = $content
        $fileReplacements = 0
        
        # 替换每个emoji
        foreach ($emoji in $emojis) {
            $pattern = [regex]::Escape($emoji)
            $matches = [regex]::Matches($content, $pattern)
            if ($matches.Count -gt 0) {
                $content = $content -replace $pattern, ""
                $fileReplacements += $matches.Count
            }
        }
        
        # 如果有修改，保存文件
        if ($fileReplacements -gt 0) {
            Set-Content -Path $file.FullName -Value $content -Encoding UTF8
            Write-Host "已处理文件: $($file.FullName) - 删除了 $fileReplacements 个emoji"
            $totalReplacements += $fileReplacements
        }
    }
    catch {
        Write-Warning "处理文件失败: $($file.FullName) - 错误: $($_.Exception.Message)"
    }
}

Write-Host ""
Write-Host "批量emoji删除完成！"
Write-Host "处理文件总数: $totalFiles"
Write-Host "删除emoji总数: $totalReplacements"
Write-Host ""