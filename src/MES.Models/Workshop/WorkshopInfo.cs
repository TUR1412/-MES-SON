using System;
using MES.Models.Base;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 车间信息模型 - S成员负责
    /// </summary>
    public class WorkshopInfo : BaseModel
    {
        /// <summary>
        /// 车间编码
        /// </summary>
        public string WorkshopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 负责人ID (关联操作员/用户)
        /// </summary>
        public int? ManagerId { get; set; }

        /// <summary>
        /// 生产能力(件/天)
        /// </summary>
        public int? Capacity { get; set; }

        /// <summary>
        /// 状态：1-启用，0-禁用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 位置ID (对应设备表的位置字段)
        /// </summary>
        public string LocationId { get; set; }

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
        /// 车间类型 (如：装配车间、测试车间、包装车间)
        /// </summary>
        public string WorkshopType { get; set; }

        /// <summary>
        /// 部门 (所属部门)
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 设备列表 (逗号分隔的设备编码)
        /// </summary>
        public string EquipmentList { get; set; }

        /// <summary>
        /// 创建用户名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 更新用户名
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkshopInfo()
        {
            Status = true;
            IsDeleted = false;
            CreateTime = DateTime.Now;
        }
    }
}
