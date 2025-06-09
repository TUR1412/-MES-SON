using System;
using MES.Models.Base;

namespace MES.Models.Factory
{
    /// <summary>
    /// 工厂
    /// </summary>
    public class FactoryInfo : BaseModel
    {
        /// <summary>
        /// 工厂ID
        /// </summary>
        public int FortoryId { get; set; }

        /// <summary>
        /// 工厂编号
        /// </summary>
        public string FactoryNum { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 工厂地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 工艺流程ID
        /// </summary>
        public int FlowId { get; set; }

        /// <summary>
        /// 工站ID
        /// </summary>
        public int OperId { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 状态(0:禁用,1:启用)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间（隐藏基类成员）
        /// </summary>
        public new DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间（隐藏基类成员）
        /// </summary>
        public new DateTime? UpdateTime { get; set; }
    }
}

