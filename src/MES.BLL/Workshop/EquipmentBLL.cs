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
    /// 设备管理业务逻辑实现
    /// 提供设备状态监控和管理的核心业务功能
    /// </summary>
    public class EquipmentBLL : IEquipmentBLL
    {
        private readonly EquipmentDAL _equipmentDAL;
        private readonly WorkshopDAL _workshopDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentBLL()
        {
            _equipmentDAL = new EquipmentDAL();
            _workshopDAL = new WorkshopDAL();
        }

        #region 基本CRUD操作

        /// <summary>
        /// 获取所有设备信息
        /// </summary>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetAllEquipments()
        {
            try
            {
                var equipments = _equipmentDAL.GetAllEquipments();
                LogManager.Info(string.Format("获取所有设备信息成功，共 {0} 条记录", equipments != null ? equipments.Count : 0));
                return equipments ?? new List<EquipmentStatusInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有设备信息失败", ex);
                throw new MESException("获取设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID获取设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>设备信息</returns>
        public EquipmentStatusInfo GetEquipmentById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("设备ID必须大于0", "id");
                }

                var equipment = _equipmentDAL.GetEquipmentById(id);
                LogManager.Info(string.Format("根据ID获取设备信息：ID={0}, 结果={1}", id, equipment != null ? "成功" : "未找到"));
                return equipment;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取设备信息失败：ID={0}", id), ex);
                throw new MESException("获取设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据设备编码获取信息
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备信息</returns>
        public EquipmentStatusInfo GetEquipmentByCode(string equipmentCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", "equipmentCode");
                }

                var equipment = _equipmentDAL.GetEquipmentByCode(equipmentCode);
                LogManager.Info(string.Format("根据编码获取设备信息：编码={0}, 结果={1}", equipmentCode, equipment != null ? "成功" : "未找到"));
                return equipment;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据编码获取设备信息失败：编码={0}", equipmentCode), ex);
                throw new MESException("获取设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 添加设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否成功</returns>
        public bool AddEquipment(EquipmentStatusInfo equipment)
        {
            try
            {
                // 验证输入参数
                ValidateEquipmentInfo(equipment);

                // 验证设备编码唯一性
                if (!IsEquipmentCodeUnique(equipment.EquipmentCode))
                {
                    throw new MESException(string.Format("设备编码 '{0}' 已存在", equipment.EquipmentCode));
                }

                // 设置创建时间
                equipment.CreateTime = DateTime.Now;
                equipment.UpdateTime = DateTime.Now;

                var result = _equipmentDAL.AddEquipment(equipment);
                if (result)
                {
                    LogManager.Info(string.Format("添加设备信息成功：编码={0}, 名称={1}", equipment.EquipmentCode, equipment.EquipmentName));
                }
                else
                {
                    LogManager.Warning(string.Format("添加设备信息失败：编码={0}", equipment.EquipmentCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加设备信息失败：编码={0}", equipment != null ? equipment.EquipmentCode : ""), ex);
                throw new MESException("添加设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否成功</returns>
        public bool UpdateEquipment(EquipmentStatusInfo equipment)
        {
            try
            {
                // 验证输入参数
                ValidateEquipmentInfo(equipment);

                // 验证设备是否存在
                var existingEquipment = GetEquipmentById(equipment.Id);
                if (existingEquipment == null)
                {
                    throw new MESException(string.Format("设备不存在：ID={0}", equipment.Id));
                }

                // 验证设备编码唯一性（排除自身）
                if (!IsEquipmentCodeUnique(equipment.EquipmentCode, equipment.Id))
                {
                    throw new MESException(string.Format("设备编码 '{0}' 已存在", equipment.EquipmentCode));
                }

                // 设置更新时间
                equipment.UpdateTime = DateTime.Now;

                var result = _equipmentDAL.UpdateEquipment(equipment);
                if (result)
                {
                    LogManager.Info(string.Format("更新设备信息成功：编码={0}, 名称={1}", equipment.EquipmentCode, equipment.EquipmentName));
                }
                else
                {
                    LogManager.Warning(string.Format("更新设备信息失败：编码={0}", equipment.EquipmentCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新设备信息失败：编码={0}", equipment != null ? equipment.EquipmentCode : ""), ex);
                throw new MESException("更新设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteEquipment(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("设备ID必须大于0", "id");
                }

                // 验证是否可以删除
                if (!CanDeleteEquipment(id))
                {
                    throw new MESException("该设备不能删除，可能正在使用中或已有关联数据");
                }

                var existingEquipment = GetEquipmentById(id);
                if (existingEquipment == null)
                {
                    throw new MESException(string.Format("设备不存在：ID={0}", id));
                }

                var result = _equipmentDAL.DeleteEquipment(id);
                if (result)
                {
                    LogManager.Info(string.Format("删除设备信息成功：ID={0}, 编码={1}", id, existingEquipment.EquipmentCode));
                }
                else
                {
                    LogManager.Warning(string.Format("删除设备信息失败：ID={0}", id));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除设备信息失败：ID={0}", id), ex);
                throw new MESException("删除设备信息时发生异常", ex);
            }
        }

        #endregion

        #region 查询和筛选

        /// <summary>
        /// 根据条件查询设备
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <param name="status">状态（可选）</param>
        /// <param name="equipmentType">设备类型（可选）</param>
        /// <param name="keyword">关键词（可选）</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> SearchEquipments(int? workshopId = null, int? status = null, 
            int? equipmentType = null, string keyword = null)
        {
            try
            {
                var equipments = _equipmentDAL.SearchEquipments(workshopId, status, equipmentType, keyword);
                LogManager.Info(string.Format("查询设备信息成功，条件：车间={0}, 状态={1}, 类型={2}, 关键词={3}, 结果数量={4}",
                    workshopId, status, equipmentType, keyword, equipments != null ? equipments.Count : 0));
                return equipments ?? new List<EquipmentStatusInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error("查询设备信息失败", ex);
                throw new MESException("查询设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据车间获取设备列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsByWorkshop(int workshopId)
        {
            try
            {
                if (workshopId <= 0)
                {
                    throw new ArgumentException("车间ID必须大于0", "workshopId");
                }

                var equipments = _equipmentDAL.GetEquipmentsByWorkshop(workshopId);
                LogManager.Info(string.Format("根据车间获取设备信息：车间ID={0}, 结果数量={1}", workshopId, equipments != null ? equipments.Count : 0));
                return equipments ?? new List<EquipmentStatusInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间获取设备信息失败：车间ID={0}", workshopId), ex);
                throw new MESException("获取设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据状态获取设备列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsByStatus(int status)
        {
            try
            {
                var equipments = _equipmentDAL.GetEquipmentsByStatus(status);
                LogManager.Info(string.Format("根据状态获取设备信息：状态={0}, 结果数量={1}", status, equipments != null ? equipments.Count : 0));
                return equipments ?? new List<EquipmentStatusInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取设备信息失败：状态={0}", status), ex);
                throw new MESException("获取设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据设备类型获取设备列表
        /// </summary>
        /// <param name="equipmentType">设备类型</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsByType(int equipmentType)
        {
            try
            {
                var equipments = _equipmentDAL.GetEquipmentsByType(equipmentType);
                LogManager.Info(string.Format("根据类型获取设备信息：类型={0}, 结果数量={1}", equipmentType, equipments != null ? equipments.Count : 0));
                return equipments ?? new List<EquipmentStatusInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据类型获取设备信息失败：类型={0}", equipmentType), ex);
                throw new MESException("获取设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取需要维护的设备列表
        /// </summary>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsNeedMaintenance()
        {
            try
            {
                var equipments = _equipmentDAL.GetEquipmentsNeedMaintenance();
                LogManager.Info(string.Format("获取需要维护的设备信息：结果数量={0}", equipments != null ? equipments.Count : 0));
                return equipments ?? new List<EquipmentStatusInfo>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取需要维护的设备信息失败", ex);
                throw new MESException("获取设备信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取设备健康摘要
        /// </summary>
        /// <param name="referenceTime">参考时间</param>
        /// <param name="dueSoonDays">即将到期天数</param>
        /// <param name="top">返回风险设备数量</param>
        /// <returns>设备健康摘要</returns>
        public EquipmentHealthSummary GetEquipmentHealthSummary(DateTime? referenceTime = null, int dueSoonDays = 7, int top = 5)
        {
            try
            {
                var now = referenceTime ?? DateTime.Now;
                var equipments = _equipmentDAL.GetAllEquipments() ?? new List<EquipmentStatusInfo>();

                int overdue = 0;
                int dueSoon = 0;
                double totalScore = 0;
                var items = new List<EquipmentHealthItem>();

                foreach (var eq in equipments)
                {
                    var nextMaintenance = eq.NextMaintenance;
                    if (nextMaintenance <= now)
                    {
                        overdue++;
                    }
                    else if (nextMaintenance <= now.AddDays(Math.Max(1, dueSoonDays)))
                    {
                        dueSoon++;
                    }

                    var score = CalculateHealthScore(eq, now);
                    totalScore += score;

                    items.Add(new EquipmentHealthItem
                    {
                        EquipmentCode = eq.EquipmentCode,
                        EquipmentName = eq.EquipmentName,
                        WorkshopName = eq.WorkshopName,
                        NextMaintenanceDate = eq.NextMaintenance,
                        Status = eq.Status,
                        HealthScore = Math.Round(score, 1)
                    });
                }

                var topRisks = items
                    .OrderBy(item => item.HealthScore)
                    .Take(Math.Max(1, top))
                    .ToList();

                return new EquipmentHealthSummary
                {
                    TotalCount = equipments.Count,
                    OverdueMaintenanceCount = overdue,
                    DueSoonCount = dueSoon,
                    AverageHealthScore = equipments.Count > 0 ? Math.Round(totalScore / equipments.Count, 1) : 0,
                    TopRisks = topRisks
                };
            }
            catch (Exception ex)
            {
                LogManager.Error("获取设备健康摘要失败", ex);
                throw new MESException("获取设备健康摘要失败", ex);
            }
        }

        #endregion

        #region 设备控制操作

        /// <summary>
        /// 启动设备
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        public bool StartEquipment(string equipmentCode, string operatorName = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", "equipmentCode");
                }

                // 验证是否可以启动
                if (!CanStartEquipment(equipmentCode))
                {
                    throw new MESException("该设备当前状态不允许启动");
                }

                var result = _equipmentDAL.UpdateEquipmentStatus(equipmentCode, 1, operatorName);
                if (result)
                {
                    LogManager.Info(string.Format("启动设备成功：编码={0}, 操作员={1}", equipmentCode, operatorName));
                }
                else
                {
                    LogManager.Warning(string.Format("启动设备失败：编码={0}", equipmentCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("启动设备失败：编码={0}", equipmentCode), ex);
                throw new MESException("启动设备时发生异常", ex);
            }
        }

        /// <summary>
        /// 停止设备
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        public bool StopEquipment(string equipmentCode, string operatorName = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", "equipmentCode");
                }

                // 验证是否可以停止
                if (!CanStopEquipment(equipmentCode))
                {
                    throw new MESException("该设备当前状态不允许停止");
                }

                var result = _equipmentDAL.UpdateEquipmentStatus(equipmentCode, 0, operatorName);
                if (result)
                {
                    LogManager.Info(string.Format("停止设备成功：编码={0}, 操作员={1}", equipmentCode, operatorName));
                }
                else
                {
                    LogManager.Warning(string.Format("停止设备失败：编码={0}", equipmentCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("停止设备失败：编码={0}", equipmentCode), ex);
                throw new MESException("停止设备时发生异常", ex);
            }
        }

        /// <summary>
        /// 设备进入维护状态
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        public bool MaintenanceEquipment(string equipmentCode, string operatorName = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", "equipmentCode");
                }

                var result = _equipmentDAL.UpdateEquipmentStatus(equipmentCode, 3, operatorName);
                if (result)
                {
                    // 同时更新维护时间
                    _equipmentDAL.UpdateMaintenanceTime(equipmentCode, DateTime.Now);
                    LogManager.Info(string.Format("设备进入维护状态成功：编码={0}, 操作员={1}", equipmentCode, operatorName));
                }
                else
                {
                    LogManager.Warning(string.Format("设备进入维护状态失败：编码={0}", equipmentCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设备进入维护状态失败：编码={0}", equipmentCode), ex);
                throw new MESException("设备维护操作时发生异常", ex);
            }
        }

        /// <summary>
        /// 设备故障报告
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="faultDescription">故障描述</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        public bool ReportFault(string equipmentCode, string faultDescription, string operatorName = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", "equipmentCode");
                }

                var result = _equipmentDAL.UpdateEquipmentStatus(equipmentCode, 2, operatorName);
                if (result)
                {
                    // 记录故障信息
                    _equipmentDAL.RecordFault(equipmentCode, faultDescription, operatorName);
                    LogManager.Info(string.Format("设备故障报告成功：编码={0}, 故障描述={1}, 操作员={2}", equipmentCode, faultDescription, operatorName));
                }
                else
                {
                    LogManager.Warning(string.Format("设备故障报告失败：编码={0}", equipmentCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设备故障报告失败：编码={0}", equipmentCode), ex);
                throw new MESException("设备故障报告时发生异常", ex);
            }
        }

        /// <summary>
        /// 批量更新设备状态
        /// </summary>
        /// <param name="equipmentCodes">设备编码列表</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>成功更新的数量</returns>
        public int BatchUpdateStatus(List<string> equipmentCodes, int newStatus, string operatorName = null)
        {
            try
            {
                if (equipmentCodes == null || equipmentCodes.Count == 0)
                {
                    throw new ArgumentException("设备编码列表不能为空", "equipmentCodes");
                }

                if (newStatus < 0 || newStatus > 3)
                {
                    throw new ArgumentException("状态值必须在0-3之间", "newStatus");
                }

                var successCount = _equipmentDAL.BatchUpdateStatus(equipmentCodes, newStatus, operatorName);
                LogManager.Info(string.Format("批量更新设备状态：总数={0}, 成功={1}, 新状态={2}", equipmentCodes.Count, successCount, newStatus));
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量更新设备状态失败", ex);
                throw new MESException("批量更新设备状态时发生异常", ex);
            }
        }

        #endregion

        #region 参数监控

        /// <summary>
        /// 更新设备运行参数
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="efficiency">效率</param>
        /// <param name="temperature">温度</param>
        /// <param name="pressure">压力</param>
        /// <param name="speed">转速</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool UpdateEquipmentParameters(string equipmentCode, decimal? efficiency = null,
            decimal? temperature = null, decimal? pressure = null, decimal? speed = null,
            string updateBy = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", "equipmentCode");
                }

                var result = _equipmentDAL.UpdateEquipmentParameters(equipmentCode, efficiency, temperature, pressure, speed, updateBy);
                if (result)
                {
                    LogManager.Info(string.Format("更新设备参数成功：编码={0}, 更新人={1}", equipmentCode, updateBy));
                }
                else
                {
                    LogManager.Warning(string.Format("更新设备参数失败：编码={0}", equipmentCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新设备参数失败：编码={0}", equipmentCode), ex);
                throw new MESException("更新设备参数时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取设备实时参数
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备参数</returns>
        public EquipmentParameters GetEquipmentParameters(string equipmentCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", "equipmentCode");
                }

                var parameters = _equipmentDAL.GetEquipmentParameters(equipmentCode);
                LogManager.Info(string.Format("获取设备参数：编码={0}, 结果={1}", equipmentCode, parameters != null ? "成功" : "未找到"));
                return parameters;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取设备参数失败：编码={0}", equipmentCode), ex);
                throw new MESException("获取设备参数时发生异常", ex);
            }
        }

        /// <summary>
        /// 批量更新设备参数（用于实时监控）
        /// </summary>
        /// <param name="parameterUpdates">参数更新列表</param>
        /// <returns>成功更新的数量</returns>
        public int BatchUpdateParameters(List<EquipmentParameterUpdate> parameterUpdates)
        {
            try
            {
                if (parameterUpdates == null || parameterUpdates.Count == 0)
                {
                    throw new ArgumentException("参数更新列表不能为空", "parameterUpdates");
                }

                var successCount = _equipmentDAL.BatchUpdateParameters(parameterUpdates);
                LogManager.Info(string.Format("批量更新设备参数：总数={0}, 成功={1}", parameterUpdates.Count, successCount));
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量更新设备参数失败", ex);
                throw new MESException("批量更新设备参数时发生异常", ex);
            }
        }

        #endregion

        #region 统计和报表

        /// <summary>
        /// 获取设备统计信息
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <returns>统计信息</returns>
        public EquipmentStatistics GetEquipmentStatistics(int? workshopId = null)
        {
            try
            {
                var statistics = _equipmentDAL.GetEquipmentStatistics(workshopId);
                LogManager.Info(string.Format("获取设备统计信息成功：车间ID={0}", workshopId));
                return statistics != null ? statistics : new EquipmentStatistics();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取设备统计信息失败", ex);
                throw new MESException("获取设备统计信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取车间设备分布
        /// </summary>
        /// <returns>车间分布统计</returns>
        public List<WorkshopEquipmentDistribution> GetWorkshopEquipmentDistribution()
        {
            try
            {
                var distribution = _equipmentDAL.GetWorkshopEquipmentDistribution();
                LogManager.Info(string.Format("获取车间设备分布成功，共 {0} 个车间", distribution != null ? distribution.Count : 0));
                return distribution != null ? distribution : new List<WorkshopEquipmentDistribution>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取车间设备分布失败", ex);
                throw new MESException("获取车间设备分布时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取设备状态分布统计
        /// </summary>
        /// <returns>状态分布统计</returns>
        public List<EquipmentStatusDistribution> GetStatusDistribution()
        {
            try
            {
                var distribution = _equipmentDAL.GetStatusDistribution();
                LogManager.Info(string.Format("获取设备状态分布成功，共 {0} 种状态", distribution != null ? distribution.Count : 0));
                return distribution != null ? distribution : new List<EquipmentStatusDistribution>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取设备状态分布失败", ex);
                throw new MESException("获取设备状态分布时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取设备效率统计
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>效率统计</returns>
        public List<EquipmentEfficiencyReport> GetEfficiencyReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var report = _equipmentDAL.GetEfficiencyReport(startDate, endDate);
                LogManager.Info(string.Format("获取设备效率统计成功，共 {0} 条记录", report != null ? report.Count : 0));
                return report != null ? report : new List<EquipmentEfficiencyReport>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取设备效率统计失败", ex);
                throw new MESException("获取设备效率统计时发生异常", ex);
            }
        }

        #endregion

        #region 业务验证

        /// <summary>
        /// 验证设备编码是否唯一
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        public bool IsEquipmentCodeUnique(string equipmentCode, int excludeId = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    return false;
                }

                return _equipmentDAL.IsEquipmentCodeUnique(equipmentCode, excludeId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证设备编码唯一性失败：编码={0}", equipmentCode), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证设备是否可以删除
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>是否可以删除</returns>
        public bool CanDeleteEquipment(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                return _equipmentDAL.CanDeleteEquipment(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证设备是否可删除失败：ID={0}", id), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证设备是否可以启动
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>是否可以启动</returns>
        public bool CanStartEquipment(string equipmentCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    return false;
                }

                var equipment = GetEquipmentByCode(equipmentCode);
                if (equipment == null)
                {
                    return false;
                }

                // 只有停止(0)或维护(3)状态的设备可以启动
                return equipment.Status == 0 || equipment.Status == 3;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证设备是否可启动失败：编码={0}", equipmentCode), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证设备是否可以停止
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>是否可以停止</returns>
        public bool CanStopEquipment(string equipmentCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(equipmentCode))
                {
                    return false;
                }

                var equipment = GetEquipmentByCode(equipmentCode);
                if (equipment == null)
                {
                    return false;
                }

                // 只有运行(1)状态的设备可以停止
                return equipment.Status == 1;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证设备是否可停止失败：编码={0}", equipmentCode), ex);
                return false;
            }
        }

        #endregion

        #region 私有验证方法

        /// <summary>
        /// 验证设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        private void ValidateEquipmentInfo(EquipmentStatusInfo equipment)
        {
            if (equipment == null)
            {
                throw new ArgumentNullException("equipment", "设备信息不能为空");
            }

            if (string.IsNullOrWhiteSpace(equipment.EquipmentCode))
            {
                throw new ArgumentException("设备编码不能为空", "equipment.EquipmentCode");
            }

            if (string.IsNullOrWhiteSpace(equipment.EquipmentName))
            {
                throw new ArgumentException("设备名称不能为空", "equipment.EquipmentName");
            }

            if (equipment.WorkshopId <= 0)
            {
                throw new ArgumentException("车间ID必须大于0", "equipment.WorkshopId");
            }

            if (equipment.EquipmentTypeId <= 0)
            {
                throw new ArgumentException("设备类型ID必须大于0", "equipment.EquipmentTypeId");
            }
        }

        private static double CalculateHealthScore(EquipmentStatusInfo equipment, DateTime now)
        {
            if (equipment == null)
            {
                return 0;
            }

            double score = 100;

            if (equipment.Status == 2)
            {
                score -= 45;
            }
            else if (equipment.Status == 3)
            {
                score -= 25;
            }
            else if (equipment.Status == 0)
            {
                score -= 10;
            }

            if (equipment.NextMaintenance <= now)
            {
                score -= 25;
            }
            else if (equipment.NextMaintenance <= now.AddDays(7))
            {
                score -= 12;
            }

            if (equipment.Efficiency > 0)
            {
                if (equipment.Efficiency < 60)
                {
                    score -= 20;
                }
                else if (equipment.Efficiency < 75)
                {
                    score -= 10;
                }
            }

            if (score < 0) score = 0;
            if (score > 100) score = 100;
            return score;
        }

        #endregion
    }
}
