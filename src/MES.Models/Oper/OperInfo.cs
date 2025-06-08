using System;
using MES.Models.Base;

namespace MES.Models.Oper
{
    /// <summary>
    /// 工站
    /// </summary>
    public class OperInfo : BaseModel
    {
        /// <summary>
        /// 工站ID
        /// </summary>
        public int OperId { get; set; }

        /// <summary>
        /// 工站编号
        /// </summary>
        public string OperNum { get; set; }

        /// <summary>
        /// 工站名称
        /// </summary>
        public string OperName { get; set; }

        /// <summary>
        /// 所属工厂ID
        /// </summary>
        public int FortoryId { get; set; }
        /// <summary>

        /// 所属工艺流程ID
        /// </summary>
        public int FlowId { get; set; }

        /// <summary>
        /// 工站顺序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 工站版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态(0:禁用,1:启用)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
