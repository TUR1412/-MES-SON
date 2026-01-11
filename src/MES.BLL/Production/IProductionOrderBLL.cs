using System;
using System.Collections.Generic;
using MES.Models.Production;
using MES.Models.Analytics;

namespace MES.BLL.Production
{
    /// <summary>
    /// 生产订单业务逻辑接口
    /// 定义生产订单管理的核心业务操作
    /// </summary>
    public interface IProductionOrderBLL
    {
        /// <summary>
        /// 添加生产订单
        /// </summary>
        /// <param name="productionOrder">生产订单信息</param>
        /// <returns>操作是否成功</returns>
        bool AddProductionOrder(ProductionOrderInfo productionOrder);

        /// <summary>
        /// 根据ID删除生产订单（逻辑删除）
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <returns>操作是否成功</returns>
        bool DeleteProductionOrder(int id);

        /// <summary>
        /// 更新生产订单信息
        /// </summary>
        /// <param name="productionOrder">生产订单信息</param>
        /// <returns>操作是否成功</returns>
        bool UpdateProductionOrder(ProductionOrderInfo productionOrder);

        /// <summary>
        /// 根据ID获取生产订单信息
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <returns>生产订单信息，未找到返回null</returns>
        ProductionOrderInfo GetProductionOrderById(int id);

        /// <summary>
        /// 根据订单号获取生产订单信息
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <returns>生产订单信息，未找到返回null</returns>
        ProductionOrderInfo GetProductionOrderByNumber(string orderNumber);

        /// <summary>
        /// 获取所有生产订单列表
        /// </summary>
        /// <returns>生产订单列表</returns>
        List<ProductionOrderInfo> GetAllProductionOrders();

        /// <summary>
        /// 根据状态获取生产订单列表
        /// </summary>
        /// <param name="status">订单状态</param>
        /// <returns>指定状态的生产订单列表</returns>
        List<ProductionOrderInfo> GetProductionOrdersByStatus(string status);

        /// <summary>
        /// 根据产品编码获取生产订单列表
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>指定产品的生产订单列表</returns>
        List<ProductionOrderInfo> GetProductionOrdersByProduct(string productCode);

        /// <summary>
        /// 分页获取生产订单列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的生产订单列表</returns>
        List<ProductionOrderInfo> GetProductionOrdersByPage(int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 根据条件搜索生产订单
        /// </summary>
        /// <param name="keyword">搜索关键词（订单号、产品名称等）</param>
        /// <returns>匹配的生产订单列表</returns>
        List<ProductionOrderInfo> SearchProductionOrders(string keyword);

        /// <summary>
        /// 启动生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <returns>操作是否成功</returns>
        bool StartProductionOrder(int id);

        /// <summary>
        /// 完成生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <param name="actualQuantity">实际完成数量</param>
        /// <returns>操作是否成功</returns>
        bool CompleteProductionOrder(int id, decimal actualQuantity);

        /// <summary>
        /// 暂停生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <param name="reason">暂停原因</param>
        /// <returns>操作是否成功</returns>
        bool PauseProductionOrder(int id, string reason);

        /// <summary>
        /// 取消生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <param name="reason">取消原因</param>
        /// <returns>操作是否成功</returns>
        bool CancelProductionOrder(int id, string reason);

        /// <summary>
        /// 验证生产订单数据
        /// </summary>
        /// <param name="productionOrder">生产订单信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        string ValidateProductionOrder(ProductionOrderInfo productionOrder);

        /// <summary>
        /// 获取生产订单风险摘要
        /// </summary>
        /// <param name="referenceTime">参考时间，默认当前时间</param>
        /// <param name="top">返回高风险列表数量</param>
        /// <returns>风险摘要</returns>
        ProductionOrderRiskSummary GetProductionOrderRiskSummary(DateTime? referenceTime = null, int top = 5);
    }
}
