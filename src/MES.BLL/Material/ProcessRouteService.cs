using System;
using System.Collections.Generic;
using System.Linq;
using MES.Models.Material;

namespace MES.BLL.Material
{
    /// <summary>
    /// 工艺路线服务类
    /// 提供工艺路线和工艺步骤的业务逻辑处理
    /// </summary>
    public class ProcessRouteService
    {
        #region 私有字段

        // 模拟数据存储（实际项目中应该使用数据访问层）
        private static List<ProcessRoute> _processRoutes = new List<ProcessRoute>();
        private static int _nextId = 1;

        #endregion

        #region 构造函数

        /// <summary>
        /// 静态构造函数，初始化示例数据
        /// </summary>
        static ProcessRouteService()
        {
            InitializeSampleData();
        }

        /// <summary>
        /// 实例构造函数
        /// </summary>
        public ProcessRouteService()
        {
        }

        #endregion

        #region 工艺路线管理

        /// <summary>
        /// 获取所有工艺路线
        /// </summary>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetAllProcessRoutes()
        {
            return _processRoutes.Where(r => !r.IsDeleted).ToList();
        }

        /// <summary>
        /// 根据ID获取工艺路线
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>工艺路线</returns>
        public ProcessRoute GetProcessRouteById(int id)
        {
            return _processRoutes.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
        }

        /// <summary>
        /// 根据产品ID获取工艺路线
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetProcessRoutesByProductId(int productId)
        {
            return _processRoutes.Where(r => r.ProductId == productId && !r.IsDeleted).ToList();
        }

        /// <summary>
        /// 根据状态获取工艺路线
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetProcessRoutesByStatus(ProcessRouteStatus status)
        {
            return _processRoutes.Where(r => r.Status == status && !r.IsDeleted).ToList();
        }

        /// <summary>
        /// 搜索工艺路线
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="productId">产品ID（可选）</param>
        /// <param name="status">状态（可选）</param>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> SearchProcessRoutes(string keyword = null, int? productId = null, ProcessRouteStatus? status = null)
        {
            var query = _processRoutes.Where(r => !r.IsDeleted);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(r => 
                    r.RouteCode.Contains(keyword) || 
                    r.RouteName.Contains(keyword) ||
                    r.ProductName.Contains(keyword));
            }

            if (productId.HasValue)
            {
                query = query.Where(r => r.ProductId == productId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            return query.ToList();
        }

        /// <summary>
        /// 创建工艺路线
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>创建结果</returns>
        public OperationResult<ProcessRoute> CreateProcessRoute(ProcessRoute processRoute)
        {
            try
            {
                // 验证数据
                var validation = processRoute.Validate();
                if (!validation.IsValid)
                {
                    return OperationResult<ProcessRoute>.Failure(validation.GetErrorsString());
                }

                // 检查编码是否重复
                if (_processRoutes.Any(r => r.RouteCode == processRoute.RouteCode && !r.IsDeleted))
                {
                    return OperationResult<ProcessRoute>.Failure("工艺路线编码已存在");
                }

                // 设置ID和创建时间
                processRoute.Id = _nextId++;
                processRoute.CreateTime = DateTime.Now;

                // 设置步骤的路线ID
                if (processRoute.Steps != null)
                {
                    foreach (var step in processRoute.Steps)
                    {
                        step.ProcessRouteId = processRoute.Id;
                    }
                }

                _processRoutes.Add(processRoute);

                return OperationResult<ProcessRoute>.Success(processRoute);
            }
            catch (Exception ex)
            {
                return OperationResult<ProcessRoute>.Failure(string.Format("创建工艺路线失败：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 更新工艺路线
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>更新结果</returns>
        public OperationResult<ProcessRoute> UpdateProcessRoute(ProcessRoute processRoute)
        {
            try
            {
                // 验证数据
                var validation = processRoute.Validate();
                if (!validation.IsValid)
                {
                    return OperationResult<ProcessRoute>.Failure(validation.GetErrorsString());
                }

                var existingRoute = GetProcessRouteById(processRoute.Id);
                if (existingRoute == null)
                {
                    return OperationResult<ProcessRoute>.Failure("工艺路线不存在");
                }

                // 检查编码是否重复（排除自己）
                if (_processRoutes.Any(r => r.RouteCode == processRoute.RouteCode && r.Id != processRoute.Id && !r.IsDeleted))
                {
                    return OperationResult<ProcessRoute>.Failure("工艺路线编码已存在");
                }

                // 更新数据
                existingRoute.RouteCode = processRoute.RouteCode;
                existingRoute.RouteName = processRoute.RouteName;
                existingRoute.ProductId = processRoute.ProductId;
                existingRoute.ProductName = processRoute.ProductName;
                existingRoute.Version = processRoute.Version;
                existingRoute.Status = processRoute.Status;
                existingRoute.Description = processRoute.Description;
                existingRoute.UpdateTime = DateTime.Now;

                // 更新步骤
                existingRoute.Steps = processRoute.Steps ?? new List<ProcessStep>();
                if (existingRoute.Steps != null)
                {
                    foreach (var step in existingRoute.Steps)
                    {
                        step.ProcessRouteId = existingRoute.Id;
                    }
                }

                return OperationResult<ProcessRoute>.Success(existingRoute);
            }
            catch (Exception ex)
            {
                return OperationResult<ProcessRoute>.Failure(string.Format("更新工艺路线失败：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>删除结果</returns>
        public OperationResult DeleteProcessRoute(int id)
        {
            try
            {
                var processRoute = GetProcessRouteById(id);
                if (processRoute == null)
                {
                    return OperationResult.Failure("工艺路线不存在");
                }

                // 检查是否可以删除（例如：是否有关联的生产订单）
                if (processRoute.Status == ProcessRouteStatus.Active)
                {
                    return OperationResult.Failure("启用状态的工艺路线不能删除，请先停用");
                }

                // 软删除
                processRoute.IsDeleted = true;
                processRoute.UpdateTime = DateTime.Now;

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(string.Format("删除工艺路线失败：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 复制工艺路线
        /// </summary>
        /// <param name="id">源工艺路线ID</param>
        /// <returns>复制结果</returns>
        public OperationResult<ProcessRoute> CopyProcessRoute(int id)
        {
            try
            {
                var sourceRoute = GetProcessRouteById(id);
                if (sourceRoute == null)
                {
                    return OperationResult<ProcessRoute>.Failure("源工艺路线不存在");
                }

                var copiedRoute = sourceRoute.Clone();
                
                // 确保编码唯一
                var baseCode = copiedRoute.RouteCode;
                int copyIndex = 1;
                while (_processRoutes.Any(r => r.RouteCode == copiedRoute.RouteCode && !r.IsDeleted))
                {
                    copiedRoute.RouteCode = string.Format("{0}_{1}", baseCode, copyIndex);
                    copyIndex++;
                }

                return CreateProcessRoute(copiedRoute);
            }
            catch (Exception ex)
            {
                return OperationResult<ProcessRoute>.Failure(string.Format("复制工艺路线失败：{0}", ex.Message));
            }
        }

        #endregion

        #region 工艺步骤管理

        /// <summary>
        /// 获取工艺路线的步骤列表
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <returns>工艺步骤列表</returns>
        public List<ProcessStep> GetProcessSteps(int routeId)
        {
            var route = GetProcessRouteById(routeId);
            return route != null && route.Steps != null ? route.Steps.OrderBy(s => s.StepNumber).ToList() : new List<ProcessStep>();
        }

        /// <summary>
        /// 添加工艺步骤
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <param name="step">工艺步骤</param>
        /// <returns>添加结果</returns>
        public OperationResult<ProcessStep> AddProcessStep(int routeId, ProcessStep step)
        {
            try
            {
                var route = GetProcessRouteById(routeId);
                if (route == null)
                {
                    return OperationResult<ProcessStep>.Failure("工艺路线不存在");
                }

                // 验证步骤数据
                var validation = step.Validate();
                if (!validation.IsValid)
                {
                    return OperationResult<ProcessStep>.Failure(validation.GetErrorsString());
                }

                step.ProcessRouteId = routeId;
                step.CreateTime = DateTime.Now;

                if (route.Steps == null)
                {
                    route.Steps = new List<ProcessStep>();
                }

                route.Steps.Add(step);

                // 重新排序步骤序号
                ReorderSteps(route.Steps);

                return OperationResult<ProcessStep>.Success(step);
            }
            catch (Exception ex)
            {
                return OperationResult<ProcessStep>.Failure(string.Format("添加工艺步骤失败：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 重新排序步骤序号
        /// </summary>
        /// <param name="steps">步骤列表</param>
        private void ReorderSteps(List<ProcessStep> steps)
        {
            if (steps != null)
            {
                var orderedSteps = steps.OrderBy(s => s.StepNumber).ToList();
                for (int i = 0; i < orderedSteps.Count; i++)
                {
                    orderedSteps[i].StepNumber = i + 1;
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化示例数据
        /// </summary>
        private static void InitializeSampleData()
        {
            // 这里可以添加一些示例数据用于测试
            // 实际项目中应该从数据库加载
        }

        #endregion
    }

    /// <summary>
    /// 操作结果类
    /// </summary>
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static OperationResult Success(string message = "操作成功")
        {
            return new OperationResult { IsSuccess = true, Message = message };
        }

        public static OperationResult Failure(string message)
        {
            return new OperationResult { IsSuccess = false, Message = message };
        }
    }

    /// <summary>
    /// 泛型操作结果类
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public static OperationResult<T> Success(T data, string message = "操作成功")
        {
            return new OperationResult<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static new OperationResult<T> Failure(string message)
        {
            return new OperationResult<T> { IsSuccess = false, Message = message };
        }
    }
}
