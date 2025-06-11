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
        /// 订单编号（唯一标识）- BLL层兼容属性
        /// </summary>
        public string OrderNumber
        {
            get { return OrderNo; }
            set { OrderNo = value; }
        }

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
        /// 计划生产数量（对应数据库planned_quantity字段）
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 计划生产数量 - BLL层兼容属性
        /// </summary>
        public decimal PlannedQuantity
        {
            get { return Quantity; }
            set { Quantity = value; }
        }

        /// <summary>
        /// 实际完成数量（对应数据库actual_quantity字段）
        /// </summary>
        public decimal ActualQuantity { get; set; }

        /// <summary>
        /// 已完成数量 - BLL层兼容属性
        /// </summary>
        public decimal CompletedQuantity
        {
            get { return ActualQuantity; }
            set { ActualQuantity = value; }
        }

        /// <summary>
        /// 进度文本 - BLL层兼容属性
        /// </summary>
        public string ProgressText
        {
            get
            {
                if (Quantity > 0)
                {
                    var percent = (double)ActualQuantity / (double)Quantity * 100;
                    return string.Format("{0}/{1} ({2:F1}%)", ActualQuantity, Quantity, percent);
                }
                return "0/0 (0%)";
            }
        }

        /// <summary>
        /// 单位（对应数据库unit字段）
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划开始时间 - BLL层兼容属性
        /// </summary>
        public DateTime PlannedStartTime
        {
            get { return PlanStartTime; }
            set { PlanStartTime = value; }
        }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 计划完成时间 - BLL层兼容属性
        /// </summary>
        public DateTime PlannedEndTime
        {
            get { return PlanEndTime; }
            set { PlanEndTime = value; }
        }

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
        /// 优先级（对应数据库priority字段）
        /// 普通、重要、紧急等
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// 负责车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 车间名称（对应数据库workshop_name字段）
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 车间名称 - BLL层兼容属性
        /// </summary>
        public string Workshop
        {
            get { return WorkshopName; }
            set { WorkshopName = value; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// 操作员 - BLL层兼容属性
        /// </summary>
        public string Operator
        {
            get { return ResponsiblePerson; }
            set { ResponsiblePerson = value; }
        }

        /// <summary>
        /// 客户名称（对应数据库customer字段）
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 销售订单号（对应数据库sales_order_number字段）
        /// </summary>
        public string SalesOrderNumber { get; set; }



        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }



        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionOrderInfo()
        {
            Status = "待开始"; // 默认状态为待开始
            Priority = "普通"; // 默认优先级为普通
            ActualQuantity = 0; // 默认实际完成数量为0
            Unit = "个"; // 默认单位
            IsDeleted = false;
        }

        /// <summary>
        /// 克隆对象 - C# 5.0兼容实现
        /// </summary>
        public ProductionOrderInfo Clone()
        {
            return new ProductionOrderInfo
            {
                Id = this.Id,
                OrderNo = this.OrderNo,
                MaterialId = this.MaterialId,
                ProductCode = this.ProductCode,
                ProductName = this.ProductName,
                Quantity = this.Quantity,
                ActualQuantity = this.ActualQuantity,
                Unit = this.Unit,
                PlanStartTime = this.PlanStartTime,
                PlanEndTime = this.PlanEndTime,
                ActualStartTime = this.ActualStartTime,
                ActualEndTime = this.ActualEndTime,
                Status = this.Status,
                Priority = this.Priority,
                WorkshopId = this.WorkshopId,
                WorkshopName = this.WorkshopName,
                ResponsiblePerson = this.ResponsiblePerson,
                CustomerName = this.CustomerName,
                SalesOrderNumber = this.SalesOrderNumber,
                Remarks = this.Remarks,
                CreateTime = this.CreateTime,
                UpdateTime = this.UpdateTime,
                Version = this.Version
            };
        }
    }
}
