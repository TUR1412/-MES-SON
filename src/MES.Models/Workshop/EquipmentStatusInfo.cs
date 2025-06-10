using System;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 设备状态信息模型
    /// 用于设备状态监控和管理
    /// </summary>
    public class EquipmentStatusInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int Id { get; set; }

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
        /// 设备类型ID (1:加工设备, 2:装配设备, 3:检测设备, 4:包装设备, 5:运输设备)
        /// </summary>
        public int EquipmentTypeId { get; set; }

        /// <summary>
        /// 所属车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 所属车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 设备位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 设备状态 (0:停止, 1:运行, 2:故障, 3:维护)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 设备状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0: return "停止";
                    case 1: return "运行";
                    case 2: return "故障";
                    case 3: return "维护";
                    default: return "未知";
                }
            }
        }

        /// <summary>
        /// 运行效率百分比
        /// </summary>
        public decimal Efficiency { get; set; }

        /// <summary>
        /// 温度(°C)
        /// </summary>
        public decimal Temperature { get; set; }

        /// <summary>
        /// 压力(MPa)
        /// </summary>
        public decimal Pressure { get; set; }

        /// <summary>
        /// 转速(rpm)
        /// </summary>
        public decimal Speed { get; set; }

        /// <summary>
        /// 功率(kW)
        /// </summary>
        public decimal Power { get; set; }

        /// <summary>
        /// 振动值
        /// </summary>
        public decimal Vibration { get; set; }

        /// <summary>
        /// 上次维护时间
        /// </summary>
        public DateTime LastMaintenance { get; set; }

        /// <summary>
        /// 下次维护时间
        /// </summary>
        public DateTime NextMaintenance { get; set; }

        /// <summary>
        /// 维护周期(天)
        /// </summary>
        public int MaintenanceCycle { get; set; }

        /// <summary>
        /// 当前操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int? OperatorId { get; set; }

        /// <summary>
        /// 设备制造商
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
        /// 保修期至
        /// </summary>
        public DateTime? WarrantyUntil { get; set; }

        /// <summary>
        /// 设备价值
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentStatusInfo()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsDeleted = false;
            IsEnabled = true;
            Status = 0; // 默认停止状态
            Efficiency = 0;
            MaintenanceCycle = 30; // 默认30天维护周期
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        public EquipmentStatusInfo Clone()
        {
            return new EquipmentStatusInfo
            {
                Id = this.Id,
                EquipmentCode = this.EquipmentCode,
                EquipmentName = this.EquipmentName,
                EquipmentType = this.EquipmentType,
                EquipmentTypeId = this.EquipmentTypeId,
                WorkshopId = this.WorkshopId,
                WorkshopName = this.WorkshopName,
                Location = this.Location,
                Status = this.Status,
                Efficiency = this.Efficiency,
                Temperature = this.Temperature,
                Pressure = this.Pressure,
                Speed = this.Speed,
                Power = this.Power,
                Vibration = this.Vibration,
                LastMaintenance = this.LastMaintenance,
                NextMaintenance = this.NextMaintenance,
                MaintenanceCycle = this.MaintenanceCycle,
                Operator = this.Operator,
                OperatorId = this.OperatorId,
                Manufacturer = this.Manufacturer,
                Model = this.Model,
                PurchaseDate = this.PurchaseDate,
                WarrantyUntil = this.WarrantyUntil,
                Value = this.Value,
                IsEnabled = this.IsEnabled,
                Remarks = this.Remarks,
                CreateTime = this.CreateTime,
                UpdateTime = this.UpdateTime,
                CreateBy = this.CreateBy,
                UpdateBy = this.UpdateBy,
                IsDeleted = this.IsDeleted
            };
        }

        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        public void UpdateStatus(int newStatus, string updateBy = null)
        {
            Status = newStatus;
            UpdateTime = DateTime.Now;
            UpdateBy = updateBy;

            // 如果状态变为维护，更新维护时间
            if (newStatus == 3)
            {
                LastMaintenance = DateTime.Now;
                NextMaintenance = DateTime.Now.AddDays(MaintenanceCycle);
            }
        }

        /// <summary>
        /// 更新设备参数
        /// </summary>
        /// <param name="efficiency">效率</param>
        /// <param name="temperature">温度</param>
        /// <param name="pressure">压力</param>
        /// <param name="speed">转速</param>
        /// <param name="updateBy">更新人</param>
        public void UpdateParameters(decimal? efficiency = null, decimal? temperature = null, 
            decimal? pressure = null, decimal? speed = null, string updateBy = null)
        {
            if (efficiency.HasValue) Efficiency = efficiency.Value;
            if (temperature.HasValue) Temperature = temperature.Value;
            if (pressure.HasValue) Pressure = pressure.Value;
            if (speed.HasValue) Speed = speed.Value;
            
            UpdateTime = DateTime.Now;
            UpdateBy = updateBy;
        }

        /// <summary>
        /// 检查是否需要维护
        /// </summary>
        /// <returns>是否需要维护</returns>
        public bool NeedsMaintenance()
        {
            return DateTime.Now >= NextMaintenance;
        }

        /// <summary>
        /// 获取设备健康状态
        /// </summary>
        /// <returns>健康状态描述</returns>
        public string GetHealthStatus()
        {
            if (Status == 2) return "故障";
            if (Status == 3) return "维护中";
            if (NeedsMaintenance()) return "需要维护";
            if (Efficiency < 70) return "效率偏低";
            if (Efficiency >= 90) return "运行良好";
            return "正常";
        }
    }
}
