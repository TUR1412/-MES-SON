using System;
using System.Collections.Generic;
using MES.BLL.Production;
using MES.DAL.Production;
using MES.Models.Production;
using MES.Common.Logging;
using MES.Common.Exceptions;
using MES.Common.Configuration;
using System.Linq;

namespace MES.BLL.Production
{
    /// <summary>
    /// 生产订单业务逻辑实现类
    /// 实现生产订单管理的核心业务逻辑
    /// </summary>
    public class ProductionOrderBLL : IProductionOrderBLL
    {
        private static readonly object InMemoryLock = new object();
        private static readonly List<ProductionOrderInfo> InMemoryOrders = new List<ProductionOrderInfo>();
        private static int InMemoryNextId = 1;

        private readonly ProductionOrderDAL _productionOrderDAL;
        private readonly bool _useInMemory;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionOrderBLL()
        {
            _productionOrderDAL = new ProductionOrderDAL();
            _useInMemory = ConfigManager.UseInMemoryData;

            if (_useInMemory)
            {
                EnsureInMemorySeedData();
            }
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
                    throw new ArgumentNullException("productionOrder");
                }

                // 业务规则验证
                string validationResult = ValidateProductionOrder(productionOrder);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    throw new MESException(validationResult);
                }

                // 检查订单号是否已存在
                if (!string.IsNullOrEmpty(productionOrder.OrderNumber))
                {
                    if (IsOrderNumberExists(productionOrder.OrderNumber))
                        throw new MESException(string.Format("添加生产订单失败：订单号 {0} 已存在", productionOrder.OrderNumber));
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

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var entity = CloneOrder(productionOrder);
                        entity.Id = InMemoryNextId++;
                        entity.CreateTime = DateTime.Now;
                        entity.UpdateTime = DateTime.Now;
                        entity.IsDeleted = false;
                        InMemoryOrders.Add(entity);
                        LogManager.Info(string.Format("成功添加生产订单（InMemory）：{0}", entity.OrderNumber));
                        return true;
                    }
                }

                // 调用DAL层添加
                bool result = _productionOrderDAL.Add(productionOrder);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功添加生产订单：{0}", productionOrder.OrderNumber));
                }
                else
                {
                    LogManager.Error(string.Format("添加生产订单失败：{0}", productionOrder.OrderNumber));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加生产订单异常：{0}", ex.Message), ex);
                if (ex is MESException) throw;
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
                    throw new MESException("删除生产订单失败：ID无效");
                }

                // 检查订单是否存在
                var existingOrder = GetProductionOrderById(id);
                if (existingOrder == null)
                {
                    throw new MESException(string.Format("删除生产订单失败：ID为 {0} 的订单不存在", id));
                }

                // 检查订单状态，进行中的订单不能删除
                if (existingOrder.Status == "进行中")
                {
                    throw new MESException(string.Format("删除生产订单失败：订单 {0} 正在进行中，不能删除", existingOrder.OrderNumber));
                }

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var entity = InMemoryOrders.FirstOrDefault(o => !o.IsDeleted && o.Id == id);
                        if (entity == null) return false;

                        entity.SetDeleteInfo(0, "system");
                        LogManager.Info(string.Format("成功删除生产订单（InMemory）：ID={0}, 订单号={1}", id, existingOrder.OrderNumber));
                        return true;
                    }
                }

                bool result = _productionOrderDAL.Delete(id);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功删除生产订单：ID={0}, 订单号={1}", id, existingOrder.OrderNumber));
                }
                else
                {
                    LogManager.Error(string.Format("删除生产订单失败：ID={0}", id));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除生产订单异常：{0}", ex.Message), ex);
                if (ex is MESException) throw;
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
                    throw new MESException("更新生产订单失败：生产订单信息无效");
                }

                // 验证业务规则
                string validationResult = ValidateProductionOrder(productionOrder);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    throw new MESException(string.Format("更新生产订单失败：{0}", validationResult));
                }

                // 检查订单是否存在
                var existingOrder = GetProductionOrderById(productionOrder.Id);
                if (existingOrder == null)
                {
                    throw new MESException(string.Format("更新生产订单失败：ID为 {0} 的订单不存在", productionOrder.Id));
                }

                // 订单号唯一性校验（更新时排除自身）
                if (IsOrderNumberExists(productionOrder.OrderNumber, productionOrder.Id))
                    throw new MESException(string.Format("更新生产订单失败：订单号 {0} 已存在", productionOrder.OrderNumber));

                // 更新时间
                productionOrder.UpdateTime = DateTime.Now;

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var entity = InMemoryOrders.FirstOrDefault(o => !o.IsDeleted && o.Id == productionOrder.Id);
                        if (entity == null) return false;

                        entity.OrderNo = productionOrder.OrderNo;
                        entity.MaterialId = productionOrder.MaterialId;
                        entity.ProductCode = productionOrder.ProductCode;
                        entity.ProductName = productionOrder.ProductName;
                        entity.Quantity = productionOrder.Quantity;
                        entity.ActualQuantity = productionOrder.ActualQuantity;
                        entity.Unit = productionOrder.Unit;
                        entity.PlanStartTime = productionOrder.PlanStartTime;
                        entity.PlanEndTime = productionOrder.PlanEndTime;
                        entity.ActualStartTime = productionOrder.ActualStartTime;
                        entity.ActualEndTime = productionOrder.ActualEndTime;
                        entity.Status = productionOrder.Status;
                        entity.Priority = productionOrder.Priority;
                        entity.WorkshopId = productionOrder.WorkshopId;
                        entity.WorkshopName = productionOrder.WorkshopName;
                        entity.ResponsiblePerson = productionOrder.ResponsiblePerson;
                        entity.CustomerName = productionOrder.CustomerName;
                        entity.SalesOrderNumber = productionOrder.SalesOrderNumber;
                        entity.Remarks = productionOrder.Remarks;
                        entity.UpdateTime = DateTime.Now;
                        entity.UpdateUserName = productionOrder.UpdateUserName;

                        LogManager.Info(string.Format("成功更新生产订单（InMemory）：{0}", entity.OrderNumber));
                        return true;
                    }
                }

                bool result = _productionOrderDAL.Update(productionOrder);

                if (result)
                {
                    LogManager.Info(string.Format("成功更新生产订单：{0}", productionOrder.OrderNumber));
                }
                else
                {
                    LogManager.Error(string.Format("更新生产订单失败：{0}", productionOrder.OrderNumber));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新生产订单异常：{0}", ex.Message), ex);
                if (ex is MESException) throw;
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
                    return null;
                }

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var order = InMemoryOrders.FirstOrDefault(o => !o.IsDeleted && o.Id == id);
                        return order != null ? CloneOrder(order) : null;
                    }
                }

                return _productionOrderDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取生产订单异常：{0}", ex.Message), ex);
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
                    return null;
                }

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var order = InMemoryOrders.FirstOrDefault(o =>
                            !o.IsDeleted &&
                            !string.IsNullOrEmpty(o.OrderNumber) &&
                            o.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase));
                        return order != null ? CloneOrder(order) : null;
                    }
                }

                return _productionOrderDAL.GetByOrderNumber(orderNumber);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据订单号获取生产订单异常：{0}", ex.Message), ex);
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
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryOrders.Where(o => !o.IsDeleted).Select(CloneOrder).ToList();
                    }
                }

                return _productionOrderDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取所有生产订单异常：{0}", ex.Message), ex);
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
                    return new List<ProductionOrderInfo>();
                }

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryOrders
                            .Where(o => !o.IsDeleted && string.Equals(o.Status, status, StringComparison.OrdinalIgnoreCase))
                            .Select(CloneOrder)
                            .ToList();
                    }
                }

                return _productionOrderDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取生产订单异常：{0}", ex.Message), ex);
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
                    return new List<ProductionOrderInfo>();
                }

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryOrders
                            .Where(o => !o.IsDeleted && string.Equals(o.ProductCode, productCode, StringComparison.OrdinalIgnoreCase))
                            .Select(CloneOrder)
                            .ToList();
                    }
                }

                return _productionOrderDAL.GetByProductCode(productCode);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据产品编码获取生产订单异常：{0}", ex.Message), ex);
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
                    totalCount = 0;
                    return new List<ProductionOrderInfo>();
                }

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var source = InMemoryOrders
                            .Where(o => !o.IsDeleted)
                            .OrderByDescending(o => o.CreateTime)
                            .ToList();

                        totalCount = source.Count;
                        return source.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(CloneOrder).ToList();
                    }
                }

                return _productionOrderDAL.GetByPage(pageIndex, pageSize, out totalCount);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("分页获取生产订单异常：{0}", ex.Message), ex);
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

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var kw = keyword.Trim();
                        return InMemoryOrders
                            .Where(o => !o.IsDeleted && (
                                (!string.IsNullOrEmpty(o.OrderNumber) && o.OrderNumber.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                (!string.IsNullOrEmpty(o.ProductCode) && o.ProductCode.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                (!string.IsNullOrEmpty(o.ProductName) && o.ProductName.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                (!string.IsNullOrEmpty(o.WorkshopName) && o.WorkshopName.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                (!string.IsNullOrEmpty(o.ResponsiblePerson) && o.ResponsiblePerson.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                            ))
                            .Select(CloneOrder)
                            .ToList();
                    }
                }

                return _productionOrderDAL.Search(keyword);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索生产订单异常：{0}", ex.Message), ex);
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
                var order = GetProductionOrderById(id);
                if (order == null)
                {
                    LogManager.Error(string.Format("启动生产订单失败：ID为 {0} 的订单不存在", id));
                    return false;
                }

                if (order.Status != "待开始")
                {
                    LogManager.Error(string.Format("启动生产订单失败：订单 {0} 状态不是待开始", order.OrderNumber));
                    return false;
                }

                order.Status = "进行中";
                order.ActualStartTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;

                if (_useInMemory)
                {
                    return UpdateProductionOrder(order);
                }

                bool result = _productionOrderDAL.Update(order);

                if (result)
                {
                    LogManager.Info(string.Format("成功启动生产订单：{0}", order.OrderNumber));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("启动生产订单异常：{0}", ex.Message), ex);
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
                var order = GetProductionOrderById(id);
                if (order == null)
                {
                    LogManager.Error(string.Format("完成生产订单失败：ID为 {0} 的订单不存在", id));
                    return false;
                }

                if (order.Status != "进行中")
                {
                    LogManager.Error(string.Format("完成生产订单失败：订单 {0} 状态不是进行中", order.OrderNumber));
                    return false;
                }

                if (actualQuantity <= 0)
                {
                    LogManager.Error("完成生产订单失败：实际完成数量必须大于0");
                    return false;
                }

                order.Status = "已完成";
                order.ActualQuantity = actualQuantity;
                order.ActualEndTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;

                if (_useInMemory)
                {
                    return UpdateProductionOrder(order);
                }

                bool result = _productionOrderDAL.Update(order);

                if (result)
                {
                    LogManager.Info(string.Format("成功完成生产订单：{0}，实际完成数量：{1}", order.OrderNumber, actualQuantity));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("完成生产订单异常：{0}", ex.Message), ex);
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
                var order = GetProductionOrderById(id);
                if (order == null)
                {
                    LogManager.Error(string.Format("暂停生产订单失败：ID为 {0} 的订单不存在", id));
                    return false;
                }

                if (order.Status != "进行中")
                {
                    LogManager.Error(string.Format("暂停生产订单失败：订单 {0} 状态不是进行中", order.OrderNumber));
                    return false;
                }

                order.Status = "已暂停";
                order.Remarks = string.IsNullOrEmpty(order.Remarks) ? string.Format("暂停原因：{0}", reason) : string.Format("{0}；暂停原因：{1}", order.Remarks, reason);
                order.UpdateTime = DateTime.Now;

                if (_useInMemory)
                {
                    return UpdateProductionOrder(order);
                }

                bool result = _productionOrderDAL.Update(order);

                if (result)
                {
                    LogManager.Info(string.Format("成功暂停生产订单：{0}，原因：{1}", order.OrderNumber, reason));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("暂停生产订单异常：{0}", ex.Message), ex);
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
                var order = GetProductionOrderById(id);
                if (order == null)
                {
                    LogManager.Error(string.Format("取消生产订单失败：ID为 {0} 的订单不存在", id));
                    return false;
                }

                if (order.Status == "已完成" || order.Status == "已取消")
                {
                    LogManager.Error(string.Format("取消生产订单失败：订单 {0} 已经是最终状态", order.OrderNumber));
                    return false;
                }

                order.Status = "已取消";
                order.Remarks = string.IsNullOrEmpty(order.Remarks) ? string.Format("取消原因：{0}", reason) : string.Format("{0}；取消原因：{1}", order.Remarks, reason);
                order.UpdateTime = DateTime.Now;

                if (_useInMemory)
                {
                    return UpdateProductionOrder(order);
                }

                bool result = _productionOrderDAL.Update(order);

                if (result)
                {
                    LogManager.Info(string.Format("成功取消生产订单：{0}，原因：{1}", order.OrderNumber, reason));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("取消生产订单异常：{0}", ex.Message), ex);
                throw new MESException("取消生产订单时发生异常", ex);
            }
        }

        private bool IsOrderNumberExists(string orderNumber, int excludeId = 0)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                return false;

            if (_useInMemory)
            {
                lock (InMemoryLock)
                {
                    return InMemoryOrders.Any(o =>
                        !o.IsDeleted &&
                        !string.IsNullOrEmpty(o.OrderNumber) &&
                        o.OrderNumber.Equals(orderNumber, StringComparison.OrdinalIgnoreCase) &&
                        (excludeId <= 0 || o.Id != excludeId));
                }
            }

            var existing = _productionOrderDAL.GetByOrderNumber(orderNumber);
            if (existing == null) return false;
            return excludeId <= 0 || existing.Id != excludeId;
        }

        private static ProductionOrderInfo CloneOrder(ProductionOrderInfo order)
        {
            if (order == null) return null;

            return new ProductionOrderInfo
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                MaterialId = order.MaterialId,
                ProductCode = order.ProductCode,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                ActualQuantity = order.ActualQuantity,
                Unit = order.Unit,
                PlanStartTime = order.PlanStartTime,
                PlanEndTime = order.PlanEndTime,
                ActualStartTime = order.ActualStartTime,
                ActualEndTime = order.ActualEndTime,
                Status = order.Status,
                Priority = order.Priority,
                WorkshopId = order.WorkshopId,
                WorkshopName = order.WorkshopName,
                ResponsiblePerson = order.ResponsiblePerson,
                CustomerName = order.CustomerName,
                SalesOrderNumber = order.SalesOrderNumber,
                Remarks = order.Remarks,
                CreateTime = order.CreateTime,
                CreateUserId = order.CreateUserId,
                CreateUserName = order.CreateUserName,
                UpdateTime = order.UpdateTime,
                UpdateUserId = order.UpdateUserId,
                UpdateUserName = order.UpdateUserName,
                IsDeleted = order.IsDeleted,
                DeleteTime = order.DeleteTime,
                DeleteUserId = order.DeleteUserId,
                DeleteUserName = order.DeleteUserName,
                Remark = order.Remark,
                Version = order.Version
            };
        }

        private static void EnsureInMemorySeedData()
        {
            lock (InMemoryLock)
            {
                if (InMemoryOrders.Count > 0) return;

                var now = DateTime.Now;

                InMemoryOrders.Add(new ProductionOrderInfo
                {
                    Id = InMemoryNextId++,
                    OrderNo = "PO-SEED-0001",
                    ProductCode = "P001",
                    ProductName = "钢制支架",
                    Quantity = 100,
                    ActualQuantity = 85,
                    Unit = "个",
                    Status = "进行中",
                    Priority = "重要",
                    WorkshopName = "机械加工车间",
                    ResponsiblePerson = "张工",
                    PlanStartTime = now.AddDays(-5),
                    PlanEndTime = now.AddDays(2),
                    ActualStartTime = now.AddDays(-5),
                    CreateTime = now.AddDays(-6),
                    UpdateTime = now.AddDays(-1),
                    IsDeleted = false,
                    CreateUserName = "seed"
                });

                InMemoryOrders.Add(new ProductionOrderInfo
                {
                    Id = InMemoryNextId++,
                    OrderNo = "PO-SEED-0002",
                    ProductCode = "P002",
                    ProductName = "铝合金外壳",
                    Quantity = 200,
                    ActualQuantity = 0,
                    Unit = "个",
                    Status = "待开始",
                    Priority = "普通",
                    WorkshopName = "冲压车间",
                    ResponsiblePerson = "李师傅",
                    PlanStartTime = now.AddDays(1),
                    PlanEndTime = now.AddDays(7),
                    CreateTime = now.AddDays(-1),
                    UpdateTime = now.AddDays(-1),
                    IsDeleted = false,
                    CreateUserName = "seed"
                });

                InMemoryOrders.Add(new ProductionOrderInfo
                {
                    Id = InMemoryNextId++,
                    OrderNo = "PO-SEED-0003",
                    ProductCode = "P003",
                    ProductName = "精密齿轮",
                    Quantity = 50,
                    ActualQuantity = 50,
                    Unit = "个",
                    Status = "已完成",
                    Priority = "紧急",
                    WorkshopName = "机械加工车间",
                    ResponsiblePerson = "王工",
                    PlanStartTime = now.AddDays(-10),
                    PlanEndTime = now.AddDays(-5),
                    ActualStartTime = now.AddDays(-10),
                    ActualEndTime = now.AddDays(-6),
                    CreateTime = now.AddDays(-11),
                    UpdateTime = now.AddDays(-6),
                    IsDeleted = false,
                    CreateUserName = "seed"
                });

                InMemoryOrders.Add(new ProductionOrderInfo
                {
                    Id = InMemoryNextId++,
                    OrderNo = "PO-SEED-0004",
                    ProductCode = "P004",
                    ProductName = "电机外壳",
                    Quantity = 150,
                    ActualQuantity = 45,
                    Unit = "个",
                    Status = "已暂停",
                    Priority = "普通",
                    WorkshopName = "机械加工车间",
                    ResponsiblePerson = "赵师傅",
                    PlanStartTime = now.AddDays(-3),
                    PlanEndTime = now.AddDays(3),
                    ActualStartTime = now.AddDays(-3),
                    CreateTime = now.AddDays(-4),
                    UpdateTime = now.AddHours(-2),
                    IsDeleted = false,
                    Remarks = "暂停原因：等待来料",
                    CreateUserName = "seed"
                });
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
