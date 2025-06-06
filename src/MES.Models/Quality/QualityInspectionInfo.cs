using System;
using MES.Models.Base;

namespace MES.Models.Quality
{
    /// <summary>
    /// 质量检验信息模型
    /// 用于质量管理，记录产品质量检验结果和数据
    /// </summary>
    public class QualityInspectionInfo : BaseModel
    {
        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionNumber { get; set; }

        /// <summary>
        /// 生产订单ID
        /// </summary>
        public int ProductionOrderId { get; set; }

        /// <summary>
        /// 生产订单号
        /// </summary>
        public string ProductionOrderNumber { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 检验类型：1-进料检验，2-过程检验，3-成品检验，4-出货检验
        /// </summary>
        public int InspectionType { get; set; }

        /// <summary>
        /// 检验阶段
        /// </summary>
        public string InspectionStage { get; set; }

        /// <summary>
        /// 检验数量
        /// </summary>
        public decimal InspectionQuantity { get; set; }

        /// <summary>
        /// 抽样数量
        /// </summary>
        public decimal SampleQuantity { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal QualifiedQuantity { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal UnqualifiedQuantity { get; set; }

        /// <summary>
        /// 检验结果：1-合格，2-不合格，3-让步接收
        /// </summary>
        public int InspectionResult { get; set; }

        /// <summary>
        /// 检验标准
        /// </summary>
        public string InspectionStandard { get; set; }

        /// <summary>
        /// 检验项目（JSON格式存储）
        /// </summary>
        public string InspectionItems { get; set; }

        /// <summary>
        /// 检验数据（JSON格式存储）
        /// </summary>
        public string InspectionData { get; set; }

        /// <summary>
        /// 不合格原因
        /// </summary>
        public string UnqualifiedReason { get; set; }

        /// <summary>
        /// 处理措施
        /// </summary>
        public string TreatmentMeasure { get; set; }

        /// <summary>
        /// 检验员ID
        /// </summary>
        public int InspectorId { get; set; }

        /// <summary>
        /// 检验员姓名
        /// </summary>
        public string InspectorName { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime InspectionTime { get; set; }

        /// <summary>
        /// 审核员ID
        /// </summary>
        public int ReviewerId { get; set; }

        /// <summary>
        /// 审核员姓名
        /// </summary>
        public string ReviewerName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ReviewTime { get; set; }

        /// <summary>
        /// 审核状态：1-待审核，2-已审核，3-审核不通过
        /// </summary>
        public int ReviewStatus { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string ReviewComments { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualityInspectionInfo()
        {
            InspectionNumber = string.Empty;
            ProductionOrderId = 0;
            ProductionOrderNumber = string.Empty;
            ProductCode = string.Empty;
            ProductName = string.Empty;
            InspectionType = 1; // 默认进料检验
            InspectionStage = string.Empty;
            InspectionQuantity = 0;
            SampleQuantity = 0;
            QualifiedQuantity = 0;
            UnqualifiedQuantity = 0;
            InspectionResult = 1; // 默认合格
            InspectionStandard = string.Empty;
            InspectionItems = string.Empty;
            InspectionData = string.Empty;
            UnqualifiedReason = string.Empty;
            TreatmentMeasure = string.Empty;
            InspectorId = 0;
            InspectorName = string.Empty;
            InspectionTime = DateTime.Now;
            ReviewerId = 0;
            ReviewerName = string.Empty;
            ReviewStatus = 1; // 默认待审核
            ReviewComments = string.Empty;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="inspectionNumber">检验单号</param>
        /// <param name="productionOrderId">生产订单ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="inspectionType">检验类型</param>
        public QualityInspectionInfo(string inspectionNumber, int productionOrderId, string productCode, int inspectionType)
        {
            InspectionNumber = inspectionNumber ?? string.Empty;
            ProductionOrderId = productionOrderId;
            ProductionOrderNumber = string.Empty;
            ProductCode = productCode ?? string.Empty;
            ProductName = string.Empty;
            InspectionType = inspectionType;
            InspectionStage = string.Empty;
            InspectionQuantity = 0;
            SampleQuantity = 0;
            QualifiedQuantity = 0;
            UnqualifiedQuantity = 0;
            InspectionResult = 1;
            InspectionStandard = string.Empty;
            InspectionItems = string.Empty;
            InspectionData = string.Empty;
            UnqualifiedReason = string.Empty;
            TreatmentMeasure = string.Empty;
            InspectorId = 0;
            InspectorName = string.Empty;
            InspectionTime = DateTime.Now;
            ReviewerId = 0;
            ReviewerName = string.Empty;
            ReviewStatus = 1;
            ReviewComments = string.Empty;
        }

        /// <summary>
        /// 获取检验类型显示文本
        /// </summary>
        /// <returns>检验类型文本</returns>
        public string GetInspectionTypeText()
        {
            switch (InspectionType)
            {
                case 1:
                    return "进料检验";
                case 2:
                    return "过程检验";
                case 3:
                    return "成品检验";
                case 4:
                    return "出货检验";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// 获取检验结果显示文本
        /// </summary>
        /// <returns>检验结果文本</returns>
        public string GetInspectionResultText()
        {
            switch (InspectionResult)
            {
                case 1:
                    return "合格";
                case 2:
                    return "不合格";
                case 3:
                    return "让步接收";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// 获取审核状态显示文本
        /// </summary>
        /// <returns>审核状态文本</returns>
        public string GetReviewStatusText()
        {
            switch (ReviewStatus)
            {
                case 1:
                    return "待审核";
                case 2:
                    return "已审核";
                case 3:
                    return "审核不通过";
                default:
                    return "未知";
            }
        }

        /// <summary>
        /// 计算合格率
        /// </summary>
        /// <returns>合格率（百分比）</returns>
        public decimal GetQualifiedRate()
        {
            if (InspectionQuantity > 0)
            {
                return Math.Round((QualifiedQuantity / InspectionQuantity) * 100, 2);
            }
            return 0;
        }

        /// <summary>
        /// 检查是否合格
        /// </summary>
        /// <returns>是否合格</returns>
        public bool IsQualified()
        {
            return InspectionResult == 1;
        }

        /// <summary>
        /// 检查是否已审核
        /// </summary>
        /// <returns>是否已审核</returns>
        public bool IsReviewed()
        {
            return ReviewStatus == 2;
        }

        /// <summary>
        /// 验证检验信息是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(InspectionNumber) && 
                   !string.IsNullOrEmpty(ProductCode) && 
                   InspectionQuantity > 0 &&
                   InspectorId > 0;
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>检验信息字符串</returns>
        public override string ToString()
        {
            return $"{InspectionNumber} - {ProductName} ({GetInspectionResultText()})";
        }
    }
}
