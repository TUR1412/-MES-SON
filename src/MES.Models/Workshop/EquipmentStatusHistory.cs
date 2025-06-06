using System;
using MES.Models.Base;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 设备状态变更历史模型 - S成员负责
    /// </summary>
    public class EquipmentStatusHistory : BaseModel
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public int EquipmentId { get; set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public int OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public int NewStatus { get; set; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; }

        /// <summary>
        /// 变更用户ID
        /// </summary>
        public int ChangeUserId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

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
        public EquipmentStatusHistory()
        {
            ChangeTime = DateTime.Now;
            IsDeleted = false;
            CreateTime = DateTime.Now;
        }
    }
}