using System;
using System.Collections.Generic;
using MES.Models.Workshop;
using MES.Models.Analytics;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 设备管理业务逻辑接口
    /// 提供设备状态监控和管理的核心业务功能
    /// </summary>
    public interface IEquipmentBLL
    {
        #region 基本CRUD操作

        /// <summary>
        /// 获取所有设备信息
        /// </summary>
        /// <returns>设备信息列表</returns>
        List<EquipmentStatusInfo> GetAllEquipments();

        /// <summary>
        /// 根据ID获取设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>设备信息</returns>
        EquipmentStatusInfo GetEquipmentById(int id);

        /// <summary>
        /// 根据设备编码获取信息
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备信息</returns>
        EquipmentStatusInfo GetEquipmentByCode(string equipmentCode);

        /// <summary>
        /// 添加设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否成功</returns>
        bool AddEquipment(EquipmentStatusInfo equipment);

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否成功</returns>
        bool UpdateEquipment(EquipmentStatusInfo equipment);

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>是否成功</returns>
        bool DeleteEquipment(int id);

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
        List<EquipmentStatusInfo> SearchEquipments(int? workshopId = null, int? status = null, 
            int? equipmentType = null, string keyword = null);

        /// <summary>
        /// 根据车间获取设备列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>设备信息列表</returns>
        List<EquipmentStatusInfo> GetEquipmentsByWorkshop(int workshopId);

        /// <summary>
        /// 根据状态获取设备列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>设备信息列表</returns>
        List<EquipmentStatusInfo> GetEquipmentsByStatus(int status);

        /// <summary>
        /// 根据设备类型获取设备列表
        /// </summary>
        /// <param name="equipmentType">设备类型</param>
        /// <returns>设备信息列表</returns>
        List<EquipmentStatusInfo> GetEquipmentsByType(int equipmentType);

        /// <summary>
        /// 获取需要维护的设备列表
        /// </summary>
        /// <returns>设备信息列表</returns>
        List<EquipmentStatusInfo> GetEquipmentsNeedMaintenance();

        /// <summary>
        /// 获取设备健康摘要
        /// </summary>
        /// <param name="referenceTime">参考时间</param>
        /// <param name="dueSoonDays">即将到期天数</param>
        /// <param name="top">返回风险设备数量</param>
        /// <returns>设备健康摘要</returns>
        EquipmentHealthSummary GetEquipmentHealthSummary(DateTime? referenceTime = null, int dueSoonDays = 7, int top = 5);

        #endregion

        #region 设备控制操作

        /// <summary>
        /// 启动设备
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        bool StartEquipment(string equipmentCode, string operatorName = null);

        /// <summary>
        /// 停止设备
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        bool StopEquipment(string equipmentCode, string operatorName = null);

        /// <summary>
        /// 设备进入维护状态
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        bool MaintenanceEquipment(string equipmentCode, string operatorName = null);

        /// <summary>
        /// 设备故障报告
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="faultDescription">故障描述</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        bool ReportFault(string equipmentCode, string faultDescription, string operatorName = null);

        /// <summary>
        /// 批量更新设备状态
        /// </summary>
        /// <param name="equipmentCodes">设备编码列表</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>成功更新的数量</returns>
        int BatchUpdateStatus(List<string> equipmentCodes, int newStatus, string operatorName = null);

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
        bool UpdateEquipmentParameters(string equipmentCode, decimal? efficiency = null, 
            decimal? temperature = null, decimal? pressure = null, decimal? speed = null, 
            string updateBy = null);

        /// <summary>
        /// 获取设备实时参数
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备参数</returns>
        MES.Models.Workshop.EquipmentParameters GetEquipmentParameters(string equipmentCode);

        /// <summary>
        /// 批量更新设备参数（用于实时监控）
        /// </summary>
        /// <param name="parameterUpdates">参数更新列表</param>
        /// <returns>成功更新的数量</returns>
        int BatchUpdateParameters(List<MES.Models.Workshop.EquipmentParameterUpdate> parameterUpdates);

        #endregion

        #region 统计和报表

        /// <summary>
        /// 获取设备统计信息
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <returns>统计信息</returns>
        MES.Models.Workshop.EquipmentStatistics GetEquipmentStatistics(int? workshopId = null);

        /// <summary>
        /// 获取车间设备分布
        /// </summary>
        /// <returns>车间分布统计</returns>
        List<MES.Models.Workshop.WorkshopEquipmentDistribution> GetWorkshopEquipmentDistribution();

        /// <summary>
        /// 获取设备状态分布统计
        /// </summary>
        /// <returns>状态分布统计</returns>
        List<MES.Models.Workshop.EquipmentStatusDistribution> GetStatusDistribution();

        /// <summary>
        /// 获取设备效率统计
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>效率统计</returns>
        List<MES.Models.Workshop.EquipmentEfficiencyReport> GetEfficiencyReport(DateTime? startDate = null, DateTime? endDate = null);

        #endregion

        #region 业务验证

        /// <summary>
        /// 验证设备编码是否唯一
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        bool IsEquipmentCodeUnique(string equipmentCode, int excludeId = 0);

        /// <summary>
        /// 验证设备是否可以删除
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>是否可以删除</returns>
        bool CanDeleteEquipment(int id);

        /// <summary>
        /// 验证设备是否可以启动
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>是否可以启动</returns>
        bool CanStartEquipment(string equipmentCode);

        /// <summary>
        /// 验证设备是否可以停止
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>是否可以停止</returns>
        bool CanStopEquipment(string equipmentCode);

        #endregion
    }


}
