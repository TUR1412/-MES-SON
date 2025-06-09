using System;
using MES.Models.Base;

namespace MES.Models.WorkOrder
{
    /// <summary>
    /// 工单
    /// </summary>
    public class WorkOrderInfo : BaseModel
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNum { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public string WorkOrderType { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 工艺流程ID
        /// </summary>
        public int FlowId { get; set; }

        /// <summary>
        /// BOM ID
        /// </summary>
        public int BOMId { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlannedQuantity { get; set; }

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal InputQuantity { get; set; }

        /// <summary>
        /// 产出数量
        /// </summary>
        public decimal OutputQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQuantity { get; set; }

        /// <summary>
        /// 工单状态(0:未开始,1:进行中,2:已完成,3:已关闭)
        /// </summary>
        public int WorkOrderStatus { get; set; }

        /// <summary>
        /// 工艺状态
        /// </summary>
        public string ProcessStatus { get; set; }

        /// <summary>
        /// 锁定状态(0:未锁定,1:已锁定)
        /// </summary>
        public int LockStatus { get; set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public int FactoryId { get; set; }

        /// <summary>
        /// Hot类型
        /// </summary>
        public string HotType { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlannedStartTime { get; set; }

        /// <summary>
        /// 计划到期日
        /// </summary>
        public DateTime? PlannedDueDate { get; set; }

        /// <summary>
        /// 创建时间（隐藏基类成员）
        /// </summary>
        public new DateTime CreateTime { get; set; }

        /// <summary>
        /// 投产时间
        /// </summary>
        public DateTime? ProductionStartTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompletionTime { get; set; }

        /// <summary>
        /// 关闭时间
        /// </summary>
        public DateTime? CloseTime { get; set; }

        /// <summary>
        /// 工单版本
        /// </summary>
        public string WorkOrderVersion { get; set; }

        /// <summary>
        /// 父工单版本
        /// </summary>
        public string ParentWorkOrderVersion { get; set; }

        /// <summary>
        /// 产品订单号
        /// </summary>
        public string ProductOrderNo { get; set; }

        /// <summary>
        /// 产品订单版本
        /// </summary>
        public string ProductOrderVersion { get; set; }

        /// <summary>
        /// 销售单号
        /// </summary>
        public string SalesOrderNo { get; set; }

        /// <summary>
        /// 主批次号
        /// </summary>
        public string MainBatchNo { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }
}

