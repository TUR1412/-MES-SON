// 运营洞察业务实现，聚合多域数据并生成洞察快照。
using System;
using System.Collections.Generic;
using MES.BLL.Material;
using MES.BLL.Production;
using MES.BLL.Quality;
using MES.BLL.Workshop;
using MES.Common.Exceptions;
using MES.Common.Logging;
using MES.Models.Analytics;

namespace MES.BLL.Analytics
{
    /// <summary>
    /// 运营洞察聚合服务
    /// </summary>
    public class InsightBLL : IInsightBLL
    {
        private static readonly object CacheLock = new object();
        private static OperationalInsightSnapshot _cachedSnapshot;
        private static DateTime _cacheTime = DateTime.MinValue;
        private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);

        private readonly ProductionOrderBLL _productionOrderBLL;
        private readonly WIPBLL _wipBLL;
        private readonly EquipmentBLL _equipmentBLL;
        private readonly MaterialBLL _materialBLL;
        private readonly QualityInspectionBLL _qualityBLL;
        private readonly BatchBLL _batchBLL;

        public InsightBLL()
        {
            _productionOrderBLL = new ProductionOrderBLL();
            _wipBLL = new WIPBLL();
            _equipmentBLL = new EquipmentBLL();
            _materialBLL = new MaterialBLL();
            _qualityBLL = new QualityInspectionBLL();
            _batchBLL = new BatchBLL();
        }

        /// <summary>
        /// 获取运营洞察快照
        /// </summary>
        /// <param name="referenceTime">参考时间</param>
        /// <returns>运营洞察快照</returns>
        public OperationalInsightSnapshot GetOperationalSnapshot(DateTime? referenceTime = null)
        {
            var now = referenceTime ?? DateTime.Now;
            if (!referenceTime.HasValue)
            {
                var cached = TryGetCachedSnapshot();
                if (cached != null)
                {
                    return cached;
                }
            }

            try
            {
                var snapshot = new OperationalInsightSnapshot
                {
                    GeneratedAt = now,
                    ProductionRisk = _productionOrderBLL.GetProductionOrderRiskSummary(now, 5),
                    WipAging = _wipBLL.GetWIPAgingSummary(now, 48, 5),
                    EquipmentHealth = _equipmentBLL.GetEquipmentHealthSummary(now, 7, 5),
                    MaterialStock = _materialBLL.GetMaterialStockAlertSummary(5),
                    Quality = _qualityBLL.GetQualityDefectSummary(now.AddDays(-30), now, 5),
                    BatchYield = _batchBLL.GetBatchYieldSummary(0.9m, 5)
                };

                snapshot.Alerts = BuildAlerts(snapshot);

                CacheSnapshot(snapshot);
                return snapshot;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取运营洞察快照失败", ex);
                throw new MESException("获取运营洞察快照失败", ex);
            }
        }

        private static OperationalInsightSnapshot TryGetCachedSnapshot()
        {
            lock (CacheLock)
            {
                if (_cachedSnapshot == null)
                {
                    return null;
                }

                if (DateTime.Now - _cacheTime > CacheTtl)
                {
                    return null;
                }

                return _cachedSnapshot;
            }
        }

        private static void CacheSnapshot(OperationalInsightSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return;
            }

            lock (CacheLock)
            {
                _cachedSnapshot = snapshot;
                _cacheTime = DateTime.Now;
            }
        }

        private static List<InsightAlertItem> BuildAlerts(OperationalInsightSnapshot snapshot)
        {
            var alerts = new List<InsightAlertItem>();
            if (snapshot == null)
            {
                return alerts;
            }

            if (snapshot.ProductionRisk != null && snapshot.ProductionRisk.HighRiskCount > 0)
            {
                alerts.Add(new InsightAlertItem
                {
                    Title = "生产订单交付风险",
                    Description = string.Format("高风险订单 {0} 单，需要优先干预。", snapshot.ProductionRisk.HighRiskCount),
                    Level = RiskLevel.High,
                    Module = "生产管理"
                });
            }

            if (snapshot.WipAging != null && snapshot.WipAging.AgingCount > 0)
            {
                alerts.Add(new InsightAlertItem
                {
                    Title = "在制品老化预警",
                    Description = string.Format("超过阈值的在制品 {0} 条，关注瓶颈：{1}", snapshot.WipAging.AgingCount, snapshot.WipAging.BottleneckWorkshop ?? "未知"),
                    Level = RiskLevel.Medium,
                    Module = "车间管理"
                });
            }

            if (snapshot.EquipmentHealth != null && snapshot.EquipmentHealth.OverdueMaintenanceCount > 0)
            {
                alerts.Add(new InsightAlertItem
                {
                    Title = "设备维护到期",
                    Description = string.Format("逾期设备 {0} 台，请安排维护。", snapshot.EquipmentHealth.OverdueMaintenanceCount),
                    Level = RiskLevel.High,
                    Module = "设备管理"
                });
            }

            if (snapshot.MaterialStock != null && snapshot.MaterialStock.BelowMinCount > 0)
            {
                alerts.Add(new InsightAlertItem
                {
                    Title = "物料库存告急",
                    Description = string.Format("低于最小库存物料 {0} 项。", snapshot.MaterialStock.BelowMinCount),
                    Level = RiskLevel.High,
                    Module = "物料管理"
                });
            }

            if (snapshot.Quality != null && snapshot.Quality.QualifiedRate < 95)
            {
                alerts.Add(new InsightAlertItem
                {
                    Title = "质量良率波动",
                    Description = string.Format("当前合格率 {0}% ，关注缺陷原因：{1}", snapshot.Quality.QualifiedRate, snapshot.Quality.TopDefectReason ?? "未分类"),
                    Level = RiskLevel.Medium,
                    Module = "质量管理"
                });
            }

            if (snapshot.BatchYield != null && snapshot.BatchYield.LowYieldCount > 0)
            {
                alerts.Add(new InsightAlertItem
                {
                    Title = "批次良率偏低",
                    Description = string.Format("低良率批次 {0} 个，建议复盘。", snapshot.BatchYield.LowYieldCount),
                    Level = RiskLevel.Medium,
                    Module = "批次管理"
                });
            }

            return alerts;
        }
    }
}

