using System;
using System.Collections.Generic;
using MES.Models.Material;

namespace MES.BLL.Material
{
    /// <summary>
    /// 工艺路线业务逻辑接口
    /// 提供工艺路线管理的核心业务功能
    /// </summary>
    public interface IProcessRouteBLL
    {
        #region 基本CRUD操作

        /// <summary>
        /// 获取所有工艺路线
        /// </summary>
        /// <returns>工艺路线列表</returns>
        List<ProcessRoute> GetAllProcessRoutes();

        /// <summary>
        /// 根据ID获取工艺路线
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>工艺路线</returns>
        ProcessRoute GetProcessRouteById(int id);

        /// <summary>
        /// 添加工艺路线
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>是否成功</returns>
        bool AddProcessRoute(ProcessRoute processRoute);

        /// <summary>
        /// 更新工艺路线
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>是否成功</returns>
        bool UpdateProcessRoute(ProcessRoute processRoute);

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否成功</returns>
        bool DeleteProcessRoute(int id);

        /// <summary>
        /// 复制工艺路线
        /// </summary>
        /// <param name="id">源工艺路线ID</param>
        /// <param name="newRouteCode">新工艺路线编码</param>
        /// <param name="newRouteName">新工艺路线名称</param>
        /// <returns>是否成功</returns>
        bool CopyProcessRoute(int id, string newRouteCode, string newRouteName);

        #endregion

        #region 查询和筛选

        /// <summary>
        /// 根据条件搜索工艺路线
        /// </summary>
        /// <param name="keyword">关键词（可选）</param>
        /// <param name="productId">产品ID（可选）</param>
        /// <param name="status">状态（可选）</param>
        /// <returns>工艺路线列表</returns>
        List<ProcessRoute> SearchProcessRoutes(string keyword = null, int? productId = null, ProcessRouteStatus? status = null);

        /// <summary>
        /// 根据产品ID获取工艺路线
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺路线列表</returns>
        List<ProcessRoute> GetProcessRoutesByProductId(int productId);

        /// <summary>
        /// 根据状态获取工艺路线
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>工艺路线列表</returns>
        List<ProcessRoute> GetProcessRoutesByStatus(ProcessRouteStatus status);

        #endregion

        #region 工艺步骤管理

        /// <summary>
        /// 获取工艺路线的步骤列表
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <returns>工艺步骤列表</returns>
        List<ProcessStep> GetProcessSteps(int routeId);

        /// <summary>
        /// 添加工艺步骤
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <param name="step">工艺步骤</param>
        /// <returns>是否成功</returns>
        bool AddProcessStep(int routeId, ProcessStep step);

        /// <summary>
        /// 更新工艺步骤
        /// </summary>
        /// <param name="step">工艺步骤</param>
        /// <returns>是否成功</returns>
        bool UpdateProcessStep(ProcessStep step);

        /// <summary>
        /// 删除工艺步骤
        /// </summary>
        /// <param name="stepId">工艺步骤ID</param>
        /// <returns>是否成功</returns>
        bool DeleteProcessStep(int stepId);

        /// <summary>
        /// 移动工艺步骤（上移/下移）
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <param name="stepId">工艺步骤ID</param>
        /// <param name="moveUp">是否上移（true上移，false下移）</param>
        /// <returns>是否成功</returns>
        bool MoveProcessStep(int routeId, int stepId, bool moveUp);

        #endregion

        #region 业务验证

        /// <summary>
        /// 验证工艺路线编码是否唯一
        /// </summary>
        /// <param name="routeCode">工艺路线编码</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        bool IsRouteCodeUnique(string routeCode, int excludeId = 0);

        /// <summary>
        /// 验证工艺路线是否可以删除
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否可以删除</returns>
        bool CanDeleteProcessRoute(int id);

        /// <summary>
        /// 验证工艺路线数据
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>验证结果</returns>
        ValidationResult ValidateProcessRoute(ProcessRoute processRoute);

        /// <summary>
        /// 验证工艺步骤数据
        /// </summary>
        /// <param name="step">工艺步骤</param>
        /// <returns>验证结果</returns>
        ValidationResult ValidateProcessStep(ProcessStep step);

        #endregion

        #region 统计和报表

        /// <summary>
        /// 获取工艺路线统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        ProcessRouteStatistics GetProcessRouteStatistics();

        /// <summary>
        /// 获取产品工艺路线分布
        /// </summary>
        /// <returns>分布统计</returns>
        List<ProductRouteDistribution> GetProductRouteDistribution();

        #endregion
    }

    #region 相关数据模型

    /// <summary>
    /// 工艺路线统计信息
    /// </summary>
    public class ProcessRouteStatistics
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 启用数量
        /// </summary>
        public int ActiveCount { get; set; }

        /// <summary>
        /// 停用数量
        /// </summary>
        public int InactiveCount { get; set; }

        /// <summary>
        /// 草稿数量
        /// </summary>
        public int DraftCount { get; set; }

        /// <summary>
        /// 平均步骤数
        /// </summary>
        public decimal AverageStepCount { get; set; }

        /// <summary>
        /// 平均总工时
        /// </summary>
        public decimal AverageTotalTime { get; set; }
    }

    /// <summary>
    /// 产品工艺路线分布
    /// </summary>
    public class ProductRouteDistribution
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工艺路线数量
        /// </summary>
        public int RouteCount { get; set; }

        /// <summary>
        /// 启用路线数量
        /// </summary>
        public int ActiveRouteCount { get; set; }
    }

    #endregion
}
