using System;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 在制品信息模型
    /// 用于跟踪生产过程中的在制品状态和位置
    /// </summary>
    public class WIPInfo
    {
        /// <summary>
        /// 在制品ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 在制品编号
        /// </summary>
        public string WIPId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNumber { get; set; }

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
        /// 当前车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 当前车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 当前工位ID
        /// </summary>
        public int? WorkstationId { get; set; }

        /// <summary>
        /// 当前工位名称
        /// </summary>
        public string WorkstationName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public int CompletedQuantity { get; set; }

        /// <summary>
        /// 状态 (0:待开始, 1:生产中, 2:质检中, 3:暂停, 4:已完成)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0: return "待开始";
                    case 1: return "生产中";
                    case 2: return "质检中";
                    case 3: return "暂停";
                    case 4: return "已完成";
                    default: return "未知";
                }
            }
        }

        /// <summary>
        /// 完成进度百分比
        /// </summary>
        public decimal Progress
        {
            get
            {
                if (Quantity <= 0) return 0;
                return Math.Round((decimal)CompletedQuantity / Quantity * 100, 2);
            }
        }

        /// <summary>
        /// 优先级 (1:低, 2:普通, 3:高, 4:紧急)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 优先级文本
        /// </summary>
        public string PriorityText
        {
            get
            {
                switch (Priority)
                {
                    case 1: return "低";
                    case 2: return "普通";
                    case 3: return "高";
                    case 4: return "紧急";
                    default: return "普通";
                }
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 预计完成时间
        /// </summary>
        public DateTime EstimatedEndTime { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualEndTime { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 总价值
        /// </summary>
        public decimal TotalValue
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }

        /// <summary>
        /// 质量等级 (A:优秀, B:良好, C:合格, D:不合格)
        /// </summary>
        public string QualityGrade { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WIPInfo()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsDeleted = false;
            Priority = 2; // 默认普通优先级
            Status = 0; // 默认待开始
            QualityGrade = "C"; // 默认合格
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        public WIPInfo Clone()
        {
            return new WIPInfo
            {
                Id = this.Id,
                WIPId = this.WIPId,
                BatchNumber = this.BatchNumber,
                WorkOrderNumber = this.WorkOrderNumber,
                ProductId = this.ProductId,
                ProductCode = this.ProductCode,
                ProductName = this.ProductName,
                WorkshopId = this.WorkshopId,
                WorkshopName = this.WorkshopName,
                WorkstationId = this.WorkstationId,
                WorkstationName = this.WorkstationName,
                Quantity = this.Quantity,
                CompletedQuantity = this.CompletedQuantity,
                Status = this.Status,
                Priority = this.Priority,
                StartTime = this.StartTime,
                EstimatedEndTime = this.EstimatedEndTime,
                ActualEndTime = this.ActualEndTime,
                UnitPrice = this.UnitPrice,
                QualityGrade = this.QualityGrade,
                ResponsiblePerson = this.ResponsiblePerson,
                Remarks = this.Remarks,
                CreateTime = this.CreateTime,
                UpdateTime = this.UpdateTime,
                CreateBy = this.CreateBy,
                UpdateBy = this.UpdateBy,
                IsDeleted = this.IsDeleted
            };
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        public void UpdateStatus(int newStatus, string updateBy = null)
        {
            Status = newStatus;
            UpdateTime = DateTime.Now;
            UpdateBy = updateBy;

            // 如果状态变为已完成，设置实际完成时间和完成数量
            if (newStatus == 4)
            {
                ActualEndTime = DateTime.Now;
                CompletedQuantity = Quantity;
            }
        }

        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="completedQuantity">已完成数量</param>
        /// <param name="updateBy">更新人</param>
        public void UpdateProgress(int completedQuantity, string updateBy = null)
        {
            CompletedQuantity = Math.Min(completedQuantity, Quantity);
            UpdateTime = DateTime.Now;
            UpdateBy = updateBy;

            // 如果完成数量达到总数量，自动设置为已完成状态
            if (CompletedQuantity >= Quantity && Status != 4)
            {
                UpdateStatus(4, updateBy);
            }
        }

        /// <summary>
        /// 转移到新车间
        /// </summary>
        /// <param name="newWorkshopId">新车间ID</param>
        /// <param name="newWorkshopName">新车间名称</param>
        /// <param name="updateBy">更新人</param>
        public void TransferToWorkshop(int newWorkshopId, string newWorkshopName, string updateBy = null)
        {
            WorkshopId = newWorkshopId;
            WorkshopName = newWorkshopName;
            UpdateTime = DateTime.Now;
            UpdateBy = updateBy;
        }
    }
}
