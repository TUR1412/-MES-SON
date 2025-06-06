using System;
using MES.Models.Base;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 设备信息模型 - S成员负责 (对应文档3的T_Equipment表)
    /// </summary>
    public class EquipmentInfo : BaseModel
    {
        /// <summary>
        /// 设备唯一编号 (对应equipment_id)
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备类型 (如：CNC,注塑机,测试台)
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 所属车间ID
        /// </summary>
        public int? WorkshopId { get; set; }

        /// <summary>
        /// 位置ID (车间内的具体位置)
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// 状态：1-运行，2-空闲，3-故障，4-维护中，5-停用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 采购日期
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 最后维护日期
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// 下次维护日期
        /// </summary>
        public DateTime? NextMaintenanceDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否删除：1-已删除，0-未删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentInfo()
        {
            Status = 1; // 默认运行状态
            IsDeleted = false;
            CreateTime = DateTime.Now;
        }
    }
}