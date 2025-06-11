using System;
using MES.Models.Base;

namespace MES.Models.WorkOrder
{
    /// <summary>
    /// 工单信息
    /// </summary>
    public class WorkOrderInfo : BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 工单ID（兼容旧系统）
        /// </summary>
        public int WorkOrderId { get; set; } = 0;

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
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

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
        public decimal PlannedQuantity { get; set; } = 0.0000m;

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal InputQuantity { get; set; } = 0.0000m;

        /// <summary>
        /// 产出数量
        /// </summary>
        public decimal OutputQuantity { get; set; } = 0.0000m;

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQuantity { get; set; } = 0.0000m;

        /// <summary>
        /// 工单状态(0:未开始,1:进行中,2:已完成,3:已关闭)
        /// </summary>
        public int WorkOrderStatus { get; set; } = 0;

        /// <summary>
        /// 工艺状态
        /// </summary>
        public string ProcessStatus { get; set; }

        /// <summary>
        /// 锁定状态(0:未锁定,1:已锁定)
        /// </summary>
        public int LockStatus { get; set; } = 0;

        /// <summary>
        /// 工厂ID
        /// </summary>
        public int FactoryId { get; set; } = 0;

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

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int? CreateUserId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 最后修改人ID
        /// </summary>
        public int? UpdateUserId { get; set; }

        /// <summary>
        /// 最后修改人姓名
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 是否删除（软删除标记）
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }

        /// <summary>
        /// 删除人ID
        /// </summary>
        public int? DeleteUserId { get; set; }

        /// <summary>
        /// 删除人姓名
        /// </summary>
        public string DeleteUserName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 版本号（用于乐观锁）
        /// </summary>
        public int Version { get; set; } = 1;
    }
}