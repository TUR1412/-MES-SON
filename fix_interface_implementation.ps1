# 快速修复BLL接口实现问题
Write-Host "开始修复BLL接口实现问题..." -ForegroundColor Green

# 修复EquipmentBLL.cs的方法名
$equipmentFile = "src\MES.BLL\Equipment\EquipmentBLL.cs"
if (Test-Path $equipmentFile) {
    Write-Host "修复EquipmentBLL.cs..." -ForegroundColor Yellow
    
    $content = Get-Content $equipmentFile -Raw -Encoding UTF8
    
    # 修复方法名
    $content = $content -replace 'public EquipmentInfo GetById\(', 'public EquipmentInfo GetEquipmentById('
    $content = $content -replace 'public EquipmentInfo GetByEquipmentCode\(', 'public EquipmentInfo GetEquipmentByCode('
    $content = $content -replace 'public List<EquipmentInfo> GetAll\(\)', 'public List<EquipmentInfo> GetAllEquipments()'
    $content = $content -replace 'public List<EquipmentInfo> GetByWorkshopId\(', 'public List<EquipmentInfo> GetEquipmentsByWorkshopId('
    $content = $content -replace 'public List<EquipmentInfo> GetByStatus\(', 'public List<EquipmentInfo> GetEquipmentsByStatus('
    $content = $content -replace 'public List<EquipmentInfo> GetMaintenanceRequired\(\)', 'public List<EquipmentInfo> GetMaintenanceRequiredEquipments()'
    $content = $content -replace 'public List<EquipmentInfo> Search\(', 'public List<EquipmentInfo> SearchEquipments('
    $content = $content -replace 'public bool Add\(EquipmentInfo', 'public bool AddEquipment(EquipmentInfo'
    $content = $content -replace 'public bool Update\(EquipmentInfo', 'public bool UpdateEquipment(EquipmentInfo'
    $content = $content -replace 'public bool Delete\(int id\)', 'public bool DeleteEquipment(int id)'
    $content = $content -replace 'private void ValidateEquipment\(', 'public string ValidateEquipment('
    
    # 修复ValidateEquipment方法返回值
    $content = $content -replace 'throw new ArgumentException\("设备编码不能为空"\);', 'return "设备编码不能为空";'
    $content = $content -replace 'throw new ArgumentException\("设备名称不能为空"\);', 'return "设备名称不能为空";'
    $content = $content -replace 'throw new ArgumentException\("设备编码长度不能超过50个字符"\);', 'return "设备编码长度不能超过50个字符";'
    $content = $content -replace 'throw new ArgumentException\("设备名称长度不能超过100个字符"\);', 'return "设备名称长度不能超过100个字符";'
    
    # 在ValidateEquipment方法末尾添加return语句
    $content = $content -replace '(\s+if \(equipment\.EquipmentName\.Length > 100\)\s+\{\s+return "设备名称长度不能超过100个字符";\s+\})\s+\}', '$1' + "`n`n            return string.Empty; // 验证通过`n        }"
    
    # 修复方法调用
    $content = $content -replace 'return GetAll\(\);', 'return GetAllEquipments();'
    $content = $content -replace 'ValidateEquipment\(equipment\);', 'string validationResult = ValidateEquipment(equipment); if (!string.IsNullOrEmpty(validationResult)) throw new ArgumentException(validationResult);'
    
    Set-Content -Path $equipmentFile -Value $content -Encoding UTF8
    Write-Host "✅ EquipmentBLL.cs 修复完成" -ForegroundColor Green
}

Write-Host "`n修复完成！" -ForegroundColor Green
