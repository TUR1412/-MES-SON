using System;
using MES.Models.Base;

namespace MES.Models.Equipment
{
    /// <summary>
    /// 设备信息模型
    /// 用于设备管理，包含设备基本信息、状态和维护记录
    /// </summary>
    public class EquipmentInfo : BaseModel
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 所属车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 所属车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 设备状态：1-正常，2-维护中，3-故障，4-停用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 设备规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 安装日期
        /// </summary>
        public DateTime? InstallDate { get; set; }

        /// <summary>
        /// 启用日期
        /// </summary>
        public DateTime? EnableDate { get; set; }

        /// <summary>
        /// 最后维护日期
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// 下次维护日期
        /// </summary>
        public DateTime? NextMaintenanceDate { get; set; }

        /// <summary>
        /// 维护周期（天）
        /// </summary>
        public int MaintenanceCycle { get; set; }

        /// <summary>
        /// 设备位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        public int ResponsiblePersonId { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ResponsiblePersonName { get; set; }

        /// <summary>
        /// 设备描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentInfo()
        {
            EquipmentCode = string.Empty;
            EquipmentName = string.Empty;
            EquipmentType = string.Empty;
            WorkshopId = 0;
            WorkshopName = string.Empty;
            Status = 1; // 默认正常
            Specification = string.Empty;
            Manufacturer = string.Empty;
            Model = string.Empty;
            MaintenanceCycle = 30; // 默认30天维护周期
            Location = string.Empty;
            ResponsiblePersonId = 0;
            ResponsiblePersonName = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="equipmentName">设备名称</param>
        /// <param name="equipmentType">设备类型</param>
        /// <param name="workshopId">所属车间ID</param>
        public EquipmentInfo(string equipmentCode, string equipmentName, string equipmentType, int workshopId)
        {
            EquipmentCode = equipmentCode ?? string.Empty;
            EquipmentName = equipmentName ?? string.Empty;
            EquipmentType = equipmentType ?? string.Empty;
            WorkshopId = workshopId;
            WorkshopName = string.Empty;
            Status = 1;
            Specification = string.Empty;
            Manufacturer = string.Empty;
            Model = string.Empty;
            MaintenanceCycle = 30;
            Location = string.Empty;
            ResponsiblePersonId = 0;
            ResponsiblePersonName = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// 获取设备状态显示文本
        /// </summary>
        /// <returns>状态文本</returns>
        public string GetStatusText()
        {
            return Status switch
            {
                1 => "正常",
                2 => "维护中",
                3 => "故障",
                4 => "停用",
                _ => "未知"
            };
        }

        /// <summary>
        /// 检查设备是否可用
        /// </summary>
        /// <returns>是否可用</returns>
        public bool IsAvailable()
        {
            return Status == 1; // 只有正常状态才可用
        }

        /// <summary>
        /// 检查是否需要维护
        /// </summary>
        /// <returns>是否需要维护</returns>
        public bool NeedsMaintenance()
        {
            if (NextMaintenanceDate.HasValue)
            {
                return NextMaintenanceDate.Value <= DateTime.Now;
            }
            return false;
        }

        /// <summary>
        /// 计算下次维护日期
        /// </summary>
        public void CalculateNextMaintenanceDate()
        {
            if (LastMaintenanceDate.HasValue && MaintenanceCycle > 0)
            {
                NextMaintenanceDate = LastMaintenanceDate.Value.AddDays(MaintenanceCycle);
            }
            else if (EnableDate.HasValue && MaintenanceCycle > 0)
            {
                NextMaintenanceDate = EnableDate.Value.AddDays(MaintenanceCycle);
            }
        }

        /// <summary>
        /// 验证设备信息是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(EquipmentCode) && 
                   !string.IsNullOrEmpty(EquipmentName) && 
                   WorkshopId > 0;
        }

        /// <summary>
        /// 获取设备使用年限
        /// </summary>
        /// <returns>使用年限（年）</returns>
        public double GetUsageYears()
        {
            if (EnableDate.HasValue)
            {
                return (DateTime.Now - EnableDate.Value).TotalDays / 365.25;
            }
            return 0;
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>设备信息字符串</returns>
        public override string ToString()
        {
            return $"{EquipmentCode} - {EquipmentName} ({GetStatusText()})";
        }
    }
}
