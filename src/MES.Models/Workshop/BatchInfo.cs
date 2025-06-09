    using System;
    using MES.Models.Base;

    namespace MES.Models.Workshop
    {
        /// <summary>
        /// 批次信息模型 - S成员负责 (对应文档3的T_Batch表)
        /// </summary>
        public class BatchInfo : BaseModel
        {
            /// <summary>
            /// 批次唯一编号 (对应batch_id)
            /// </summary>
            public string BatchId { get; set; }

            /// <summary>
            /// 批次号（显示用）
            /// </summary>
            public string BatchNumber { get; set; }

            /// <summary>
            /// 所属工单ID (外键关联T_Work_Order)
            /// </summary>
            public string WorkOrderId { get; set; }

            /// <summary>
            /// 产品物料ID (外键关联T_Material)
            /// </summary>
            public string ProductMaterialId { get; set; }

            /// <summary>
            /// 产品编码
            /// </summary>
            public string ProductCode { get; set; }

            /// <summary>
            /// 产品名称
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// 生产订单号
            /// </summary>
            public string ProductionOrderNumber { get; set; }

            /// <summary>
            /// 批次数量（计划数量）
            /// </summary>
            public decimal Quantity { get; set; }

            /// <summary>
            /// 计划数量
            /// </summary>
            public decimal PlannedQuantity { get; set; }

            /// <summary>
            /// 实际数量
            /// </summary>
            public decimal ActualQuantity { get; set; }

            /// <summary>
            /// 批次状态：1-创建，2-待产，3-生产中，4-预留，5-完成，6-取消
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 车间ID
            /// </summary>
            public int WorkshopId { get; set; }

            /// <summary>
            /// 车间名称
            /// </summary>
            public string WorkshopName { get; set; }

            /// <summary>
            /// 生产订单ID
            /// </summary>
            public int ProductionOrderId { get; set; }

            /// <summary>
            /// 单位
            /// </summary>
            public string Unit { get; set; }

            /// <summary>
            /// 批次状态文本
            /// </summary>
            public string BatchStatus { get; set; }

            /// <summary>
            /// 操作员ID
            /// </summary>
            public int OperatorId { get; set; }

            /// <summary>
            /// 操作员姓名
            /// </summary>
            public string OperatorName { get; set; }

            /// <summary>
            /// 备注信息
            /// </summary>
            public string Remarks { get; set; }

            /// <summary>
            /// 当前所在工站ID
            /// </summary>
            public string CurrentStationId { get; set; }

            /// <summary>
            /// 生产开始时间
            /// </summary>
            public DateTime? ProductionStartTime { get; set; }

            /// <summary>
            /// 生产完成时间
            /// </summary>
            public DateTime? ProductionEndTime { get; set; }

            /// <summary>
            /// 结束时间（别名）
            /// </summary>
            public DateTime? EndTime
            {
                get { return ProductionEndTime; }
                set { ProductionEndTime = value; }
            }

            /// <summary>
            /// 当前载具ID (外键关联T_Carrier)
            /// </summary>
            public string CarrierId { get; set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            public BatchInfo()
            {
                Status = 1; // 默认创建状态
                BatchStatus = "待开始";
                PlannedQuantity = 0;
                ActualQuantity = 0;
                Quantity = 0;
                WorkshopId = 0;
                ProductionOrderId = 0;
                OperatorId = 0;
                Unit = "个";
            }
        }
    }
