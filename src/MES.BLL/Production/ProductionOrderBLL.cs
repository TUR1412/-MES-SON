using System;
using System.Collections.Generic;
using MES.BLL.Production;
using MES.DAL.Production;
using MES.Models.Production;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.Production
{
    /// <summary>
    /// 生产订单业务逻辑实现类
    /// 实现生产订单管理的核心业务逻辑
    /// </summary>
    public class ProductionOrderBLL : IProductionOrderBLL
    {
        private readonly ProductionOrderDAL _productionOrderDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionOrderBLL()
        {
            _productionOrderDAL = new ProductionOrderDAL();
        }

        /// <summary>
        /// 添加生产订单
        /// </summary>
        /// <param name="productionOrder">生产订单信息</param>
        /// <returns>操作是否成功</returns>
        public bool AddProductionOrder(ProductionOrderInfo productionOrder)
        {
            try
            {
                // 验证输入参数
                if (productionOrder == null)
                {
                    LogManager.Error("添加生产订单失败：生产订单信息不能为空");
                    return false;
                }

                // 业务规则验证
                string validationResult = ValidateProductionOrder(productionOrder);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error(string.Format("添加生产订单失败：{0}", validationResult));
                    return false;
                }

                // 检查订单号是否已存在
                if (!string.IsNullOrEmpty(productionOrder.OrderNumber))
                {
                    var existingOrder = _productionOrderDAL.GetByOrderNumber(productionOrder.OrderNumber);
                    if (existingOrder != null)
                    {
                        LogManager.Error($"添加生产订单失败：订单号 {productionOrder.OrderNumber} 已存在");
                        return false;
                    }
                }

                // 设置默认值
                productionOrder.CreateTime = DateTime.Now;
                productionOrder.UpdateTime = DateTime.Now;
                productionOrder.IsDeleted = false;

                // 如果未设置状态，默认为待开始
                if (string.IsNullOrEmpty(productionOrder.Status))
                {
                    productionOrder.Status = "待开始";
                }

                // 调用DAL层添加
                bool result = _productionOrderDAL.Add(productionOrder);
                
                if (result)
                {
                    LogManager.Info($"成功添加生产订单：{productionOrder.OrderNumber}");
                }
                else
                {
                    LogManager.Error($"添加生产订单失败：{productionOrder.OrderNumber}");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error($"添加生产订单异常：{ex.Message}", ex);
                throw new MESException("添加生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID删除生产订单（逻辑删除）
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <returns>操作是否成功</returns>
        public bool DeleteProductionOrder(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("删除生产订单失败：ID无效");
                    return false;
                }

                // 检查订单是否存在
                var existingOrder = _productionOrderDAL.GetById(id);
                if (existingOrder == null)
                {
                    LogManager.Error($"删除生产订单失败：ID为 {id} 的订单不存在");
                    return false;
                }

                // 检查订单状态，进行中的订单不能删除
                if (existingOrder.Status == "进行中")
                {
                    LogManager.Error($"删除生产订单失败：订单 {existingOrder.OrderNumber} 正在进行中，不能删除");
                    return false;
                }

                bool result = _productionOrderDAL.Delete(id);
                
                if (result)
                {
                    LogManager.Info($"成功删除生产订单：ID={id}, 订单号={existingOrder.OrderNumber}");
                }
                else
                {
                    LogManager.Error($"删除生产订单失败：ID={id}");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error($"删除生产订单异常：{ex.Message}", ex);
                throw new MESException("删除生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新生产订单信息
        /// </summary>
        /// <param name="productionOrder">生产订单信息</param>
        /// <returns>操作是否成功</returns>
        public bool UpdateProductionOrder(ProductionOrderInfo productionOrder)
        {
            try
            {
                if (productionOrder == null || productionOrder.Id <= 0)
                {
                    LogManager.Error("更新生产订单失败：生产订单信息无效");
                    return false;
                }

                // 验证业务规则
                string validationResult = ValidateProductionOrder(productionOrder);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error($"更新生产订单失败：{validationResult}");
                    return false;
                }

                // 检查订单是否存在
                var existingOrder = _productionOrderDAL.GetById(productionOrder.Id);
                if (existingOrder == null)
                {
                    LogManager.Error($"更新生产订单失败：ID为 {productionOrder.Id} 的订单不存在");
                    return false;
                }

                // 更新时间
                productionOrder.UpdateTime = DateTime.Now;

                bool result = _productionOrderDAL.Update(productionOrder);
                
                if (result)
                {
                    LogManager.Info($"成功更新生产订单：{productionOrder.OrderNumber}");
                }
                else
                {
                    LogManager.Error($"更新生产订单失败：{productionOrder.OrderNumber}");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新生产订单异常：{ex.Message}", ex);
                throw new MESException("更新生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID获取生产订单信息
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <returns>生产订单信息，未找到返回null</returns>
        public ProductionOrderInfo GetProductionOrderById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("获取生产订单失败：ID无效");
                    return null;
                }

                return _productionOrderDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取生产订单异常：{ex.Message}", ex);
                throw new MESException("获取生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据订单号获取生产订单信息
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <returns>生产订单信息，未找到返回null</returns>
        public ProductionOrderInfo GetProductionOrderByNumber(string orderNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNumber))
                {
                    LogManager.Error("获取生产订单失败：订单号不能为空");
                    return null;
                }

                return _productionOrderDAL.GetByOrderNumber(orderNumber);
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据订单号获取生产订单异常：{ex.Message}", ex);
                throw new MESException("根据订单号获取生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取所有生产订单列表
        /// </summary>
        /// <returns>生产订单列表</returns>
        public List<ProductionOrderInfo> GetAllProductionOrders()
        {
            try
            {
                return _productionOrderDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取所有生产订单异常：{ex.Message}", ex);
                throw new MESException("获取所有生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据状态获取生产订单列表
        /// </summary>
        /// <param name="status">订单状态</param>
        /// <returns>指定状态的生产订单列表</returns>
        public List<ProductionOrderInfo> GetProductionOrdersByStatus(string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    LogManager.Error("根据状态获取生产订单失败：状态不能为空");
                    return new List<ProductionOrderInfo>();
                }

                return _productionOrderDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据状态获取生产订单异常：{ex.Message}", ex);
                throw new MESException("根据状态获取生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据产品编码获取生产订单列表
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>指定产品的生产订单列表</returns>
        public List<ProductionOrderInfo> GetProductionOrdersByProduct(string productCode)
        {
            try
            {
                if (string.IsNullOrEmpty(productCode))
                {
                    LogManager.Error("根据产品编码获取生产订单失败：产品编码不能为空");
                    return new List<ProductionOrderInfo>();
                }

                return _productionOrderDAL.GetByProductCode(productCode);
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据产品编码获取生产订单异常：{ex.Message}", ex);
                throw new MESException("根据产品编码获取生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 分页获取生产订单列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的生产订单列表</returns>
        public List<ProductionOrderInfo> GetProductionOrdersByPage(int pageIndex, int pageSize, out int totalCount)
        {
            try
            {
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    LogManager.Error("分页获取生产订单失败：页码和每页记录数必须大于0");
                    totalCount = 0;
                    return new List<ProductionOrderInfo>();
                }

                return _productionOrderDAL.GetByPage(pageIndex, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                LogManager.Error($"分页获取生产订单异常：{ex.Message}", ex);
                totalCount = 0;
                throw new MESException("分页获取生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据条件搜索生产订单
        /// </summary>
        /// <param name="keyword">搜索关键词（订单号、产品名称等）</param>
        /// <returns>匹配的生产订单列表</returns>
        public List<ProductionOrderInfo> SearchProductionOrders(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return GetAllProductionOrders();
                }

                return _productionOrderDAL.Search(keyword);
            }
            catch (Exception ex)
            {
                LogManager.Error($"搜索生产订单异常：{ex.Message}", ex);
                throw new MESException("搜索生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 启动生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <returns>操作是否成功</returns>
        public bool StartProductionOrder(int id)
        {
            try
            {
                var order = _productionOrderDAL.GetById(id);
                if (order == null)
                {
                    LogManager.Error($"启动生产订单失败：ID为 {id} 的订单不存在");
                    return false;
                }

                if (order.Status != "待开始")
                {
                    LogManager.Error($"启动生产订单失败：订单 {order.OrderNumber} 状态不是待开始");
                    return false;
                }

                order.Status = "进行中";
                order.ActualStartTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;

                bool result = _productionOrderDAL.Update(order);
                
                if (result)
                {
                    LogManager.Info($"成功启动生产订单：{order.OrderNumber}");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error($"启动生产订单异常：{ex.Message}", ex);
                throw new MESException("启动生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 完成生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <param name="actualQuantity">实际完成数量</param>
        /// <returns>操作是否成功</returns>
        public bool CompleteProductionOrder(int id, decimal actualQuantity)
        {
            try
            {
                var order = _productionOrderDAL.GetById(id);
                if (order == null)
                {
                    LogManager.Error($"完成生产订单失败：ID为 {id} 的订单不存在");
                    return false;
                }

                if (order.Status != "进行中")
                {
                    LogManager.Error($"完成生产订单失败：订单 {order.OrderNumber} 状态不是进行中");
                    return false;
                }

                if (actualQuantity <= 0)
                {
                    LogManager.Error($"完成生产订单失败：实际完成数量必须大于0");
                    return false;
                }

                order.Status = "已完成";
                order.ActualQuantity = actualQuantity;
                order.ActualEndTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;

                bool result = _productionOrderDAL.Update(order);
                
                if (result)
                {
                    LogManager.Info($"成功完成生产订单：{order.OrderNumber}，实际完成数量：{actualQuantity}");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error($"完成生产订单异常：{ex.Message}", ex);
                throw new MESException("完成生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 暂停生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <param name="reason">暂停原因</param>
        /// <returns>操作是否成功</returns>
        public bool PauseProductionOrder(int id, string reason)
        {
            try
            {
                var order = _productionOrderDAL.GetById(id);
                if (order == null)
                {
                    LogManager.Error($"暂停生产订单失败：ID为 {id} 的订单不存在");
                    return false;
                }

                if (order.Status != "进行中")
                {
                    LogManager.Error($"暂停生产订单失败：订单 {order.OrderNumber} 状态不是进行中");
                    return false;
                }

                order.Status = "已暂停";
                order.Remarks = string.IsNullOrEmpty(order.Remarks) ? $"暂停原因：{reason}" : $"{order.Remarks}；暂停原因：{reason}";
                order.UpdateTime = DateTime.Now;

                bool result = _productionOrderDAL.Update(order);
                
                if (result)
                {
                    LogManager.Info($"成功暂停生产订单：{order.OrderNumber}，原因：{reason}");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error($"暂停生产订单异常：{ex.Message}", ex);
                throw new MESException("暂停生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 取消生产订单
        /// </summary>
        /// <param name="id">生产订单ID</param>
        /// <param name="reason">取消原因</param>
        /// <returns>操作是否成功</returns>
        public bool CancelProductionOrder(int id, string reason)
        {
            try
            {
                var order = _productionOrderDAL.GetById(id);
                if (order == null)
                {
                    LogManager.Error($"取消生产订单失败：ID为 {id} 的订单不存在");
                    return false;
                }

                if (order.Status == "已完成" || order.Status == "已取消")
                {
                    LogManager.Error($"取消生产订单失败：订单 {order.OrderNumber} 已经是最终状态");
                    return false;
                }

                order.Status = "已取消";
                order.Remarks = string.IsNullOrEmpty(order.Remarks) ? $"取消原因：{reason}" : $"{order.Remarks}；取消原因：{reason}";
                order.UpdateTime = DateTime.Now;

                bool result = _productionOrderDAL.Update(order);
                
                if (result)
                {
                    LogManager.Info($"成功取消生产订单：{order.OrderNumber}，原因：{reason}");
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error($"取消生产订单异常：{ex.Message}", ex);
                throw new MESException("取消生产订单时发生异常", ex);
            }
        }

        /// <summary>
        /// 验证生产订单数据
        /// </summary>
        /// <param name="productionOrder">生产订单信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        public string ValidateProductionOrder(ProductionOrderInfo productionOrder)
        {
            if (productionOrder == null)
            {
                return "生产订单信息不能为空";
            }

            if (string.IsNullOrEmpty(productionOrder.OrderNumber))
            {
                return "订单号不能为空";
            }

            if (string.IsNullOrEmpty(productionOrder.ProductCode))
            {
                return "产品编码不能为空";
            }

            if (string.IsNullOrEmpty(productionOrder.ProductName))
            {
                return "产品名称不能为空";
            }

            if (productionOrder.PlannedQuantity <= 0)
            {
                return "计划数量必须大于0";
            }

            if (productionOrder.PlannedStartTime >= productionOrder.PlannedEndTime)
            {
                return "计划开始时间必须早于计划结束时间";
            }

            return string.Empty;
        }
    }
}
