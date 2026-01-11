// 运营洞察快照与指标模型，描述风险等级与核心摘要字段。
using System;
using System.Collections.Generic;

namespace MES.Models.Analytics
{
    /// <summary>
    /// 运营洞察快照
    /// </summary>
    public class OperationalInsightSnapshot
    {
        /// <summary>
        /// 快照生成时间
        /// </summary>
        public DateTime GeneratedAt { get; set; }

        /// <summary>
        /// 生产订单风险摘要
        /// </summary>
        public ProductionOrderRiskSummary ProductionRisk { get; set; }

        /// <summary>
        /// 在制品老化摘要
        /// </summary>
        public WIPAgingSummary WipAging { get; set; }

        /// <summary>
        /// 设备健康摘要
        /// </summary>
        public EquipmentHealthSummary EquipmentHealth { get; set; }

        /// <summary>
        /// 物料库存告警摘要
        /// </summary>
        public MaterialStockAlertSummary MaterialStock { get; set; }

        /// <summary>
        /// 质量缺陷摘要
        /// </summary>
        public QualityDefectSummary Quality { get; set; }

        /// <summary>
        /// 批次良率摘要
        /// </summary>
        public BatchYieldSummary BatchYield { get; set; }

        /// <summary>
        /// 关键提醒
        /// </summary>
        public List<InsightAlertItem> Alerts { get; set; }
    }

    /// <summary>
    /// 风险等级
    /// </summary>
    public enum RiskLevel
    {
        /// <summary>
        /// 低风险
        /// </summary>
        Low = 0,
        /// <summary>
        /// 中风险
        /// </summary>
        Medium = 1,
        /// <summary>
        /// 高风险
        /// </summary>
        High = 2
    }

    /// <summary>
    /// 生产订单风险摘要
    /// </summary>
    public class ProductionOrderRiskSummary
    {
        public int TotalActive { get; set; }
        public int OverdueCount { get; set; }
        public int HighRiskCount { get; set; }
        public int MediumRiskCount { get; set; }
        public int OnTrackCount { get; set; }
        public double AverageProgress { get; set; }
        public List<ProductionOrderRiskItem> TopRisks { get; set; }
    }

    /// <summary>
    /// 生产订单风险明细
    /// </summary>
    public class ProductionOrderRiskItem
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ProductName { get; set; }
        public string WorkshopName { get; set; }
        public DateTime PlannedEndTime { get; set; }
        public double ProgressRate { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public double RemainingHours { get; set; }
    }

    /// <summary>
    /// 在制品老化摘要
    /// </summary>
    public class WIPAgingSummary
    {
        public int TotalCount { get; set; }
        public int AgingCount { get; set; }
        public double AverageAgingHours { get; set; }
        public string BottleneckWorkshop { get; set; }
        public List<WIPAgingItem> TopAgingItems { get; set; }
    }

    /// <summary>
    /// 在制品老化明细
    /// </summary>
    public class WIPAgingItem
    {
        public string WipId { get; set; }
        public string ProductName { get; set; }
        public string WorkshopName { get; set; }
        public double AgingHours { get; set; }
        public int Priority { get; set; }
    }

    /// <summary>
    /// 设备健康摘要
    /// </summary>
    public class EquipmentHealthSummary
    {
        public int TotalCount { get; set; }
        public int OverdueMaintenanceCount { get; set; }
        public int DueSoonCount { get; set; }
        public double AverageHealthScore { get; set; }
        public List<EquipmentHealthItem> TopRisks { get; set; }
    }

    /// <summary>
    /// 设备健康明细
    /// </summary>
    public class EquipmentHealthItem
    {
        public string EquipmentCode { get; set; }
        public string EquipmentName { get; set; }
        public string WorkshopName { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public int Status { get; set; }
        public double HealthScore { get; set; }
    }

    /// <summary>
    /// 物料库存告警摘要
    /// </summary>
    public class MaterialStockAlertSummary
    {
        public int TotalMaterials { get; set; }
        public int BelowSafetyCount { get; set; }
        public int BelowMinCount { get; set; }
        public List<MaterialStockAlertItem> TopAlerts { get; set; }
    }

    /// <summary>
    /// 物料库存告警明细
    /// </summary>
    public class MaterialStockAlertItem
    {
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal? SafetyStock { get; set; }
        public decimal? MinStock { get; set; }
        public RiskLevel RiskLevel { get; set; }
    }

    /// <summary>
    /// 质量缺陷摘要
    /// </summary>
    public class QualityDefectSummary
    {
        public int TotalInspections { get; set; }
        public decimal QualifiedRate { get; set; }
        public string TopDefectReason { get; set; }
        public List<QualityDefectItem> TopDefects { get; set; }
    }

    /// <summary>
    /// 质量缺陷明细
    /// </summary>
    public class QualityDefectItem
    {
        public string DefectReason { get; set; }
        public int Count { get; set; }
        public decimal Rate { get; set; }
    }

    /// <summary>
    /// 批次良率摘要
    /// </summary>
    public class BatchYieldSummary
    {
        public int TotalBatches { get; set; }
        public int LowYieldCount { get; set; }
        public decimal AverageYieldRate { get; set; }
        public List<BatchYieldItem> LowYieldBatches { get; set; }
    }

    /// <summary>
    /// 批次良率明细
    /// </summary>
    public class BatchYieldItem
    {
        public string BatchNumber { get; set; }
        public string ProductName { get; set; }
        public decimal PlannedQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal YieldRate { get; set; }
    }

    /// <summary>
    /// 洞察提醒项
    /// </summary>
    public class InsightAlertItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public RiskLevel Level { get; set; }
        public string Module { get; set; }
    }
}

