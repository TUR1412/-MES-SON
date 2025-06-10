using System;
using System.Collections.Generic;
using MES.Models.Workshop;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 在制品业务逻辑接口
    /// 提供在制品管理的核心业务功能
    /// </summary>
    public interface IWIPBLL
    {
        #region 基本CRUD操作

        /// <summary>
        /// 获取所有在制品信息
        /// </summary>
        /// <returns>在制品信息列表</returns>
        List<WIPInfo> GetAllWIPs();

        /// <summary>
        /// 根据ID获取在制品信息
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>在制品信息</returns>
        WIPInfo GetWIPById(int id);

        /// <summary>
        /// 根据在制品编号获取信息
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <returns>在制品信息</returns>
        WIPInfo GetWIPByWIPId(string wipId);

        /// <summary>
        /// 添加在制品信息
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        /// <returns>是否成功</returns>
        bool AddWIP(WIPInfo wipInfo);

        /// <summary>
        /// 更新在制品信息
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        /// <returns>是否成功</returns>
        bool UpdateWIP(WIPInfo wipInfo);

        /// <summary>
        /// 删除在制品信息
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>是否成功</returns>
        bool DeleteWIP(int id);

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
        List<WIPInfo> SearchWIPs(int? workshopId = null, int? status = null, 
            DateTime? startDate = null, DateTime? endDate = null, string keyword = null);

        /// <summary>
        /// 根据批次号获取在制品列表
        /// </summary>
        /// <param name="batchNumber">批次号</param>
        /// <returns>在制品信息列表</returns>
        List<WIPInfo> GetWIPsByBatchNumber(string batchNumber);

        /// <summary>
        /// 根据工单号获取在制品列表
        /// </summary>
        /// <param name="workOrderNumber">工单号</param>
        /// <returns>在制品信息列表</returns>
        List<WIPInfo> GetWIPsByWorkOrderNumber(string workOrderNumber);

        /// <summary>
        /// 根据车间获取在制品列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>在制品信息列表</returns>
        List<WIPInfo> GetWIPsByWorkshop(int workshopId);

        /// <summary>
        /// 根据状态获取在制品列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>在制品信息列表</returns>
        List<WIPInfo> GetWIPsByStatus(int status);

        #endregion

        #region 状态管理

        /// <summary>
        /// 更新在制品状态
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        bool UpdateWIPStatus(string wipId, int newStatus, string updateBy = null);

        /// <summary>
        /// 批量更新在制品状态
        /// </summary>
        /// <param name="wipIds">在制品编号列表</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>成功更新的数量</returns>
        int BatchUpdateWIPStatus(List<string> wipIds, int newStatus, string updateBy = null);

        /// <summary>
        /// 更新在制品进度
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="completedQuantity">已完成数量</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        bool UpdateWIPProgress(string wipId, int completedQuantity, string updateBy = null);

        #endregion

        #region 转移和流转

        /// <summary>
        /// 转移在制品到新车间
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="newWorkshopId">新车间ID</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        bool TransferWIPToWorkshop(string wipId, int newWorkshopId, string updateBy = null);

        /// <summary>
        /// 批量转移在制品
        /// </summary>
        /// <param name="wipIds">在制品编号列表</param>
        /// <param name="newWorkshopId">新车间ID</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>成功转移的数量</returns>
        int BatchTransferWIPs(List<string> wipIds, int newWorkshopId, string updateBy = null);

        #endregion

        #region 统计和报表

        /// <summary>
        /// 获取在制品统计信息
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <param name="startDate">开始日期（可选）</param>
        /// <param name="endDate">结束日期（可选）</param>
        /// <returns>统计信息</returns>
        WIPStatistics GetWIPStatistics(int? workshopId = null, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// 获取车间在制品分布
        /// </summary>
        /// <returns>车间分布统计</returns>
        List<WorkshopWIPDistribution> GetWorkshopWIPDistribution();

        /// <summary>
        /// 获取状态分布统计
        /// </summary>
        /// <returns>状态分布统计</returns>
        List<StatusDistribution> GetStatusDistribution();

        #endregion

        #region 业务验证

        /// <summary>
        /// 验证在制品编号是否唯一
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        bool IsWIPIdUnique(string wipId, int excludeId = 0);

        /// <summary>
        /// 验证在制品是否可以删除
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>是否可以删除</returns>
        bool CanDeleteWIP(int id);

        /// <summary>
        /// 验证在制品是否可以转移
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="targetWorkshopId">目标车间ID</param>
        /// <returns>是否可以转移</returns>
        bool CanTransferWIP(string wipId, int targetWorkshopId);

        #endregion
    }

    #region 统计相关模型

    /// <summary>
    /// 在制品统计信息
    /// </summary>
    public class WIPStatistics
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总价值
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// 待开始数量
        /// </summary>
        public int PendingCount { get; set; }

        /// <summary>
        /// 生产中数量
        /// </summary>
        public int InProgressCount { get; set; }

        /// <summary>
        /// 质检中数量
        /// </summary>
        public int InQCCount { get; set; }

        /// <summary>
        /// 暂停数量
        /// </summary>
        public int PausedCount { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public int CompletedCount { get; set; }

        /// <summary>
        /// 平均完成进度
        /// </summary>
        public decimal AverageProgress { get; set; }
    }

    /// <summary>
    /// 车间在制品分布
    /// </summary>
    public class WorkshopWIPDistribution
    {
        /// <summary>
        /// 车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 在制品数量
        /// </summary>
        public int WIPCount { get; set; }

        /// <summary>
        /// 总价值
        /// </summary>
        public decimal TotalValue { get; set; }
    }

    /// <summary>
    /// 状态分布统计
    /// </summary>
    public class StatusDistribution
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public decimal Percentage { get; set; }
    }

    #endregion
}
