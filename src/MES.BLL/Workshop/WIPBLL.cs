using System;
using System.Collections.Generic;
using System.Linq;
using MES.Models.Workshop;
using MES.Models.Analytics;
using MES.DAL.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 在制品业务逻辑实现
    /// 提供在制品管理的核心业务功能
    /// </summary>
    public class WIPBLL : IWIPBLL
    {
        private readonly WIPDAL _wipDAL;
        private readonly WorkshopDAL _workshopDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WIPBLL()
        {
            _wipDAL = new WIPDAL();
            _workshopDAL = new WorkshopDAL();
        }

        #region 基本CRUD操作

        /// <summary>
        /// 获取所有在制品信息
        /// </summary>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetAllWIPs()
        {
            try
            {
                var wips = _wipDAL.GetAllWIPs();
                LogManager.Info(string.Format("获取所有在制品信息成功，共 {0} 条记录", wips != null ? wips.Count : 0));
                return wips ?? new List<WIPInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有在制品信息失败", ex);
                throw new MESException("获取在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID获取在制品信息
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>在制品信息</returns>
        public WIPInfo GetWIPById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("在制品ID必须大于0", "id");
                }

                var wip = _wipDAL.GetWIPById(id);
                LogManager.Info(string.Format("根据ID获取在制品信息：ID={0}, 结果={1}", id, wip != null ? "成功" : "未找到"));
                return wip;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取在制品信息失败：ID={0}", id), ex);
                throw new MESException("获取在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据在制品编号获取信息
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <returns>在制品信息</returns>
        public WIPInfo GetWIPByWIPId(string wipId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(wipId))
                {
                    throw new ArgumentException("在制品编号不能为空", "wipId");
                }

                var wip = _wipDAL.GetWIPByWIPId(wipId);
                LogManager.Info(string.Format("根据编号获取在制品信息：编号={0}, 结果={1}", wipId, wip != null ? "成功" : "未找到"));
                return wip;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据编号获取在制品信息失败：编号={0}", wipId), ex);
                throw new MESException("获取在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 添加在制品信息
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        /// <returns>是否成功</returns>
        public bool AddWIP(WIPInfo wipInfo)
        {
            try
            {
                // 验证输入参数
                ValidateWIPInfo(wipInfo);

                // 验证在制品编号唯一性
                if (!IsWIPIdUnique(wipInfo.WIPId))
                {
                    throw new MESException(string.Format("在制品编号 '{0}' 已存在", wipInfo.WIPId));
                }

                // 设置创建时间
                wipInfo.CreateTime = DateTime.Now;
                wipInfo.UpdateTime = DateTime.Now;

                var result = _wipDAL.AddWIP(wipInfo);
                if (result)
                {
                    LogManager.Info(string.Format("添加在制品信息成功：编号={0}, 产品={1}", wipInfo.WIPId, wipInfo.ProductName));
                }
                else
                {
                    LogManager.Warning(string.Format("添加在制品信息失败：编号={0}", wipInfo.WIPId));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加在制品信息失败：编号={0}", wipInfo != null ? wipInfo.WIPId : ""), ex);
                throw new MESException("添加在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新在制品信息
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        /// <returns>是否成功</returns>
        public bool UpdateWIP(WIPInfo wipInfo)
        {
            try
            {
                // 验证输入参数
                ValidateWIPInfo(wipInfo);

                // 验证在制品是否存在
                var existingWIP = GetWIPById(wipInfo.Id);
                if (existingWIP == null)
                {
                    throw new MESException(string.Format("在制品不存在：ID={0}", wipInfo.Id));
                }

                // 验证在制品编号唯一性（排除自身）
                if (!IsWIPIdUnique(wipInfo.WIPId, wipInfo.Id))
                {
                    throw new MESException(string.Format("在制品编号 '{0}' 已存在", wipInfo.WIPId));
                }

                // 设置更新时间
                wipInfo.UpdateTime = DateTime.Now;

                var result = _wipDAL.UpdateWIP(wipInfo);
                if (result)
                {
                    LogManager.Info(string.Format("更新在制品信息成功：编号={0}, 产品={1}", wipInfo.WIPId, wipInfo.ProductName));
                }
                else
                {
                    LogManager.Warning(string.Format("更新在制品信息失败：编号={0}", wipInfo.WIPId));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新在制品信息失败：编号={0}", wipInfo != null ? wipInfo.WIPId : ""), ex);
                throw new MESException("更新在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 删除在制品信息
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteWIP(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("在制品ID必须大于0", "id");
                }

                // 验证是否可以删除
                if (!CanDeleteWIP(id))
                {
                    throw new MESException("该在制品不能删除，可能正在生产中或已有关联数据");
                }

                var existingWIP = GetWIPById(id);
                if (existingWIP == null)
                {
                    throw new MESException(string.Format("在制品不存在：ID={0}", id));
                }

                var result = _wipDAL.DeleteWIP(id);
                if (result)
                {
                    LogManager.Info(string.Format("删除在制品信息成功：ID={0}, 编号={1}", id, existingWIP.WIPId));
                }
                else
                {
                    LogManager.Warning(string.Format("删除在制品信息失败：ID={0}", id));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除在制品信息失败：ID={0}", id), ex);
                throw new MESException("删除在制品信息时发生异常", ex);
            }
        }

        #endregion

        #region 查询和筛选

        /// <summary>
        /// 根据条件查询在制品
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <param name="status">状态（可选）</param>
        /// <param name="startDate">开始日期（可选）</param>
        /// <param name="endDate">结束日期（可选）</param>
        /// <param name="keyword">关键词（可选）</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> SearchWIPs(int? workshopId = null, int? status = null, 
            DateTime? startDate = null, DateTime? endDate = null, string keyword = null)
        {
            try
            {
                var wips = _wipDAL.SearchWIPs(workshopId, status, startDate, endDate, keyword);
                LogManager.Info(string.Format("查询在制品信息成功，条件：车间={0}, 状态={1}, 关键词={2}, 结果数量={3}",
                    workshopId, status, keyword, wips != null ? wips.Count : 0));
                return wips ?? new List<WIPInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error("查询在制品信息失败", ex);
                throw new MESException("查询在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据批次号获取在制品列表
        /// </summary>
        /// <param name="batchNumber">批次号</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByBatchNumber(string batchNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(batchNumber))
                {
                    throw new ArgumentException("批次号不能为空", "batchNumber");
                }

                var wips = _wipDAL.GetWIPsByBatchNumber(batchNumber);
                LogManager.Info(string.Format("根据批次号获取在制品信息：批次号={0}, 结果数量={1}", batchNumber, wips != null ? wips.Count : 0));
                return wips ?? new List<WIPInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据批次号获取在制品信息失败：批次号={0}", batchNumber), ex);
                throw new MESException("获取在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据工单号获取在制品列表
        /// </summary>
        /// <param name="workOrderNumber">工单号</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByWorkOrderNumber(string workOrderNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workOrderNumber))
                {
                    throw new ArgumentException("工单号不能为空", "workOrderNumber");
                }

                var wips = _wipDAL.GetWIPsByWorkOrderNumber(workOrderNumber);
                LogManager.Info(string.Format("根据工单号获取在制品信息：工单号={0}, 结果数量={1}", workOrderNumber, wips != null ? wips.Count : 0));
                return wips ?? new List<WIPInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工单号获取在制品信息失败：工单号={0}", workOrderNumber), ex);
                throw new MESException("获取在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据车间获取在制品列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByWorkshop(int workshopId)
        {
            try
            {
                if (workshopId <= 0)
                {
                    throw new ArgumentException("车间ID必须大于0", "workshopId");
                }

                var wips = _wipDAL.GetWIPsByWorkshop(workshopId);
                LogManager.Info(string.Format("根据车间获取在制品信息：车间ID={0}, 结果数量={1}", workshopId, wips != null ? wips.Count : 0));
                return wips ?? new List<WIPInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间获取在制品信息失败：车间ID={0}", workshopId), ex);
                throw new MESException("获取在制品信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据状态获取在制品列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByStatus(int status)
        {
            try
            {
                var wips = _wipDAL.GetWIPsByStatus(status);
                LogManager.Info(string.Format("根据状态获取在制品信息：状态={0}, 结果数量={1}", status, wips != null ? wips.Count : 0));
                return wips ?? new List<WIPInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取在制品信息失败：状态={0}", status), ex);
                throw new MESException("获取在制品信息时发生异常", ex);
            }
        }

        #endregion

        #region 状态管理

        /// <summary>
        /// 更新在制品状态
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool UpdateWIPStatus(string wipId, int newStatus, string updateBy = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(wipId))
                {
                    throw new ArgumentException("在制品编号不能为空", "wipId");
                }

                if (newStatus < 0 || newStatus > 4)
                {
                    throw new ArgumentException("状态值必须在0-4之间", "newStatus");
                }

                var result = _wipDAL.UpdateWIPStatus(wipId, newStatus, updateBy);
                if (result)
                {
                    LogManager.Info(string.Format("更新在制品状态成功：编号={0}, 新状态={1}, 更新人={2}", wipId, newStatus, updateBy));
                }
                else
                {
                    LogManager.Warning(string.Format("更新在制品状态失败：编号={0}", wipId));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新在制品状态失败：编号={0}", wipId), ex);
                throw new MESException("更新在制品状态时发生异常", ex);
            }
        }

        /// <summary>
        /// 批量更新在制品状态
        /// </summary>
        /// <param name="wipIds">在制品编号列表</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>成功更新的数量</returns>
        public int BatchUpdateWIPStatus(List<string> wipIds, int newStatus, string updateBy = null)
        {
            try
            {
                if (wipIds == null || wipIds.Count == 0)
                {
                    throw new ArgumentException("在制品编号列表不能为空", "wipIds");
                }

                if (newStatus < 0 || newStatus > 4)
                {
                    throw new ArgumentException("状态值必须在0-4之间", "newStatus");
                }

                var successCount = _wipDAL.BatchUpdateWIPStatus(wipIds, newStatus, updateBy);
                LogManager.Info(string.Format("批量更新在制品状态：总数={0}, 成功={1}, 新状态={2}", wipIds.Count, successCount, newStatus));
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量更新在制品状态失败", ex);
                throw new MESException("批量更新在制品状态时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新在制品进度
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="completedQuantity">已完成数量</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool UpdateWIPProgress(string wipId, int completedQuantity, string updateBy = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(wipId))
                {
                    throw new ArgumentException("在制品编号不能为空", "wipId");
                }

                if (completedQuantity < 0)
                {
                    throw new ArgumentException("已完成数量不能小于0", "completedQuantity");
                }

                var result = _wipDAL.UpdateWIPProgress(wipId, completedQuantity, updateBy);
                if (result)
                {
                    LogManager.Info(string.Format("更新在制品进度成功：编号={0}, 已完成数量={1}, 更新人={2}", wipId, completedQuantity, updateBy));
                }
                else
                {
                    LogManager.Warning(string.Format("更新在制品进度失败：编号={0}", wipId));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新在制品进度失败：编号={0}", wipId), ex);
                throw new MESException("更新在制品进度时发生异常", ex);
            }
        }

        #endregion

        #region 转移和流转

        /// <summary>
        /// 转移在制品到新车间
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="newWorkshopId">新车间ID</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool TransferWIPToWorkshop(string wipId, int newWorkshopId, string updateBy = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(wipId))
                {
                    throw new ArgumentException("在制品编号不能为空", "wipId");
                }

                if (newWorkshopId <= 0)
                {
                    throw new ArgumentException("车间ID必须大于0", "newWorkshopId");
                }

                // 验证是否可以转移
                if (!CanTransferWIP(wipId, newWorkshopId))
                {
                    throw new MESException("该在制品不能转移到指定车间");
                }

                var result = _wipDAL.TransferWIPToWorkshop(wipId, newWorkshopId, updateBy);
                if (result)
                {
                    LogManager.Info(string.Format("转移在制品成功：编号={0}, 新车间ID={1}, 更新人={2}", wipId, newWorkshopId, updateBy));
                }
                else
                {
                    LogManager.Warning(string.Format("转移在制品失败：编号={0}", wipId));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("转移在制品失败：编号={0}", wipId), ex);
                throw new MESException("转移在制品时发生异常", ex);
            }
        }

        /// <summary>
        /// 批量转移在制品
        /// </summary>
        /// <param name="wipIds">在制品编号列表</param>
        /// <param name="newWorkshopId">新车间ID</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>成功转移的数量</returns>
        public int BatchTransferWIPs(List<string> wipIds, int newWorkshopId, string updateBy = null)
        {
            try
            {
                if (wipIds == null || wipIds.Count == 0)
                {
                    throw new ArgumentException("在制品编号列表不能为空", "wipIds");
                }

                if (newWorkshopId <= 0)
                {
                    throw new ArgumentException("车间ID必须大于0", "newWorkshopId");
                }

                var successCount = _wipDAL.BatchTransferWIPs(wipIds, newWorkshopId, updateBy);
                LogManager.Info(string.Format("批量转移在制品：总数={0}, 成功={1}, 新车间ID={2}", wipIds.Count, successCount, newWorkshopId));
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量转移在制品失败", ex);
                throw new MESException("批量转移在制品时发生异常", ex);
            }
        }

        #endregion

        #region 统计和报表

        /// <summary>
        /// 获取在制品统计信息
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <param name="startDate">开始日期（可选）</param>
        /// <param name="endDate">结束日期（可选）</param>
        /// <returns>统计信息</returns>
        public WIPStatistics GetWIPStatistics(int? workshopId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var statisticsData = _wipDAL.GetWIPStatistics(workshopId, startDate, endDate);
                LogManager.Info(string.Format("获取在制品统计信息成功：车间ID={0}", workshopId));

                if (statisticsData == null)
                {
                    return new WIPStatistics();
                }

                // 将Dictionary转换为WIPStatistics对象
                var statistics = new WIPStatistics();
                if (statisticsData.ContainsKey("TotalCount"))
                    statistics.TotalCount = Convert.ToInt32(statisticsData["TotalCount"]);
                if (statisticsData.ContainsKey("InProgressCount"))
                    statistics.InProgressCount = Convert.ToInt32(statisticsData["InProgressCount"]);
                if (statisticsData.ContainsKey("CompletedCount"))
                    statistics.CompletedCount = Convert.ToInt32(statisticsData["CompletedCount"]);
                if (statisticsData.ContainsKey("PendingCount"))
                    statistics.PendingCount = Convert.ToInt32(statisticsData["PendingCount"]);

                return statistics;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取在制品统计信息失败", ex);
                throw new MESException("获取在制品统计信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取车间在制品分布
        /// </summary>
        /// <returns>车间分布统计</returns>
        public List<WorkshopWIPDistribution> GetWorkshopWIPDistribution()
        {
            try
            {
                var distributionData = _wipDAL.GetWorkshopWIPDistribution();
                LogManager.Info(string.Format("获取车间在制品分布成功，共 {0} 个车间", distributionData != null ? distributionData.Count : 0));

                if (distributionData == null)
                {
                    return new List<WorkshopWIPDistribution>();
                }

                // 将Dictionary列表转换为WorkshopWIPDistribution对象列表
                var distribution = new List<WorkshopWIPDistribution>();
                foreach (var item in distributionData)
                {
                    var dist = new WorkshopWIPDistribution();
                    if (item.ContainsKey("WorkshopId"))
                        dist.WorkshopId = Convert.ToInt32(item["WorkshopId"]);
                    if (item.ContainsKey("WorkshopName"))
                        dist.WorkshopName = item["WorkshopName"].ToString();
                    if (item.ContainsKey("WIPCount"))
                        dist.WIPCount = Convert.ToInt32(item["WIPCount"]);
                    distribution.Add(dist);
                }

                return distribution;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取车间在制品分布失败", ex);
                throw new MESException("获取车间在制品分布时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取状态分布统计
        /// </summary>
        /// <returns>状态分布统计</returns>
        public List<StatusDistribution> GetStatusDistribution()
        {
            try
            {
                var distributionData = _wipDAL.GetStatusDistribution();
                LogManager.Info(string.Format("获取状态分布统计成功，共 {0} 种状态", distributionData != null ? distributionData.Count : 0));

                if (distributionData == null)
                {
                    return new List<StatusDistribution>();
                }

                // 将Dictionary列表转换为StatusDistribution对象列表
                var distribution = new List<StatusDistribution>();
                foreach (var item in distributionData)
                {
                    var dist = new StatusDistribution();
                    if (item.ContainsKey("Status"))
                        dist.Status = Convert.ToInt32(item["Status"]);
                    if (item.ContainsKey("StatusName"))
                        dist.StatusName = item["StatusName"].ToString();
                    if (item.ContainsKey("Count"))
                        dist.Count = Convert.ToInt32(item["Count"]);
                    distribution.Add(dist);
                }

                return distribution;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取状态分布统计失败", ex);
                throw new MESException("获取状态分布统计时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取在制品老化摘要
        /// </summary>
        /// <param name="referenceTime">参考时间</param>
        /// <param name="agingThresholdHours">老化阈值（小时）</param>
        /// <param name="top">返回老化列表数量</param>
        /// <returns>老化摘要</returns>
        public WIPAgingSummary GetWIPAgingSummary(DateTime? referenceTime = null, double agingThresholdHours = 48, int top = 5)
        {
            try
            {
                var now = referenceTime ?? DateTime.Now;
                var wips = _wipDAL.GetActiveWIPs() ?? new List<WIPInfo>();

                double totalAging = 0;
                int agingCount = 0;
                var items = new List<WIPAgingItem>();

                foreach (var wip in wips)
                {
                    var aging = (now - wip.StartTime).TotalHours;
                    totalAging += aging;
                    if (aging >= agingThresholdHours)
                    {
                        agingCount++;
                    }

                    items.Add(new WIPAgingItem
                    {
                        WipId = wip.WIPId,
                        ProductName = wip.ProductName,
                        WorkshopName = wip.WorkshopName,
                        AgingHours = Math.Round(aging, 1),
                        Priority = wip.Priority
                    });
                }

                var bottleneckWorkshop = items
                    .Where(item => !string.IsNullOrEmpty(item.WorkshopName))
                    .GroupBy(item => item.WorkshopName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key)
                    .FirstOrDefault();

                var topItems = items
                    .OrderByDescending(item => item.AgingHours)
                    .Take(Math.Max(1, top))
                    .ToList();

                return new WIPAgingSummary
                {
                    TotalCount = wips.Count,
                    AgingCount = agingCount,
                    AverageAgingHours = wips.Count > 0 ? Math.Round(totalAging / wips.Count, 1) : 0,
                    BottleneckWorkshop = bottleneckWorkshop,
                    TopAgingItems = topItems
                };
            }
            catch (Exception ex)
            {
                LogManager.Error("获取在制品老化摘要失败", ex);
                throw new MESException("获取在制品老化摘要失败", ex);
            }
        }

        #endregion

        #region 业务验证

        /// <summary>
        /// 验证在制品编号是否唯一
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        public bool IsWIPIdUnique(string wipId, int excludeId = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(wipId))
                {
                    return false;
                }

                return _wipDAL.IsWIPIdUnique(wipId, excludeId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证在制品编号唯一性失败：编号={0}", wipId), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证在制品是否可以删除
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>是否可以删除</returns>
        public bool CanDeleteWIP(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                return _wipDAL.CanDeleteWIP(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证在制品是否可删除失败：ID={0}", id), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证在制品是否可以转移
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="targetWorkshopId">目标车间ID</param>
        /// <returns>是否可以转移</returns>
        public bool CanTransferWIP(string wipId, int targetWorkshopId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(wipId) || targetWorkshopId <= 0)
                {
                    return false;
                }

                return _wipDAL.CanTransferWIP(wipId, targetWorkshopId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证在制品是否可转移失败：编号={0}", wipId), ex);
                return false;
            }
        }

        #endregion

        #region 私有验证方法

        /// <summary>
        /// 验证在制品信息
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        private void ValidateWIPInfo(WIPInfo wipInfo)
        {
            if (wipInfo == null)
            {
                throw new ArgumentNullException("wipInfo", "在制品信息不能为空");
            }

            if (string.IsNullOrWhiteSpace(wipInfo.WIPId))
            {
                throw new ArgumentException("在制品编号不能为空", "wipInfo.WIPId");
            }

            if (string.IsNullOrWhiteSpace(wipInfo.ProductCode))
            {
                throw new ArgumentException("产品编码不能为空", "wipInfo.ProductCode");
            }

            if (string.IsNullOrWhiteSpace(wipInfo.ProductName))
            {
                throw new ArgumentException("产品名称不能为空", "wipInfo.ProductName");
            }

            if (wipInfo.Quantity <= 0)
            {
                throw new ArgumentException("数量必须大于0", "wipInfo.Quantity");
            }

            if (wipInfo.WorkshopId <= 0)
            {
                throw new ArgumentException("车间ID必须大于0", "wipInfo.WorkshopId");
            }
        }

        #endregion
    }
}
