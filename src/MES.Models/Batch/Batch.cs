using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Models.Batch
{
    /// <summary>
    /// 批次
    /// </summary>
    internal class Batch
    {
        /// <summary>
        /// 批次ID
        /// </summary>
        public int BatchId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 批次类型
        /// </summary>
        public string BatchType { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal BatchQuantity { get; set; }

        /// <summary>
        /// 子产品数量
        /// </summary>
        public decimal SubProductQuantity { get; set; }

        /// <summary>
        /// 在制品数量
        /// </summary>
        public decimal WIPQuantity { get; set; }

        /// <summary>
        /// 锁定状态(0:未锁定,1:已锁定)
        /// </summary>
        public int LockStatus { get; set; }

        /// <summary>
        /// 工艺流程ID
        /// </summary>
        public int FlowId { get; set; }

        /// <summary>
        /// 当前工站ID
        /// </summary>
        public int? CurrentOperId { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Hot类型
        /// </summary>
        public string HotType { get; set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public int FactoryId { get; set; }

        /// <summary>
        /// 子单位
        /// </summary>
        public string SubUnit { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public string CreateUser { get; set; }
    }
}

