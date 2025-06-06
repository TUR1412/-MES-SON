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
        /// 变更备注
        /// </summary>
        public string ChangeRemark { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentStatusHistory()
        {
            ChangeTime = DateTime.Now;
        }
    }
}