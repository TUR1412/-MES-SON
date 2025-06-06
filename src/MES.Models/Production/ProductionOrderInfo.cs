using System;
using MES.Models.Base;

namespace MES.Models.Production
{
    /// <summary>
    /// 生产订单信息模型
    /// 用于管理生产订单的基本信息和状态
    /// </summary>
    public class ProductionOrderInfo : BaseModel
    {
        /// <summary>
        /// 订单编号（唯一标识）
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 物料ID（关联物料表）
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 计划生产数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public int CompletedQuantity { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartTime { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualEndTime { get; set; }

        /// <summary>
        /// 订单状态
        /// 0-待开始，1-进行中，2-已完成，3-已暂停，4-已取消
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 优先级
        /// 1-低，2-中，3-高，4-紧急
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 负责车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 交货日期
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// 订单来源
        /// 1-销售订单，2-库存补充，3-紧急生产
        /// </summary>
        public int OrderSource { get; set; }

        /// <summary>
        /// 质量要求
        /// </summary>
        public string QualityRequirement { get; set; }



        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionOrderInfo()
        {
            Status = "0"; // 默认状态为待开始
            Priority = 2; // 默认优先级为中等
            OrderSource = 1; // 默认来源为销售订单
            CompletedQuantity = 0; // 默认完成数量为0
        }
    }
}
