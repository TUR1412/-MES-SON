// --- START OF FILE ProcessRouteBLL.cs ---

using System;
using System.Collections.Generic;
using System.Linq;
using MES.Models.Material;
using MES.DAL.Material;
using MES.Common.Logging;
using MES.Common.Exceptions;
using MES.Models.Production;

namespace MES.BLL.Material
{
    /// <summary>
    /// 工艺路线业务逻辑实现
    /// 提供工艺路线管理的核心业务功能
    /// </summary>
    public class ProcessRouteBLL : IProcessRouteBLL
    {
        private readonly ProcessRouteDAL _processRouteDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessRouteBLL()
        {
            _processRouteDAL = new ProcessRouteDAL();
        }

        #region 基本CRUD操作

        /// <summary>
        /// 获取所有工艺路线
        /// </summary>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetAllProcessRoutes()
        {
            try
            {
                var routes = _processRouteDAL.GetAllProcessRoutes();
                LogManager.Info(string.Format("获取所有工艺路线成功，共 {0} 条记录", routes != null ? routes.Count : 0));
                return routes ?? new List<ProcessRoute>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有工艺路线失败", ex);
                throw new MESException("获取工艺路线时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID获取工艺路线
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>工艺路线</returns>
        public ProcessRoute GetProcessRouteById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("工艺路线ID必须大于0", "id");
                }

                var route = _processRouteDAL.GetProcessRouteById(id);
                LogManager.Info(string.Format("根据ID获取工艺路线：ID={0}, 结果={1}", id, route != null ? "成功" : "未找到"));
                return route;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取工艺路线失败：ID={0}", id), ex);
                throw new MESException("获取工艺路线时发生异常", ex);
            }
        }

        /// <summary>
        /// 添加工艺路线
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>是否成功</returns>
        public bool AddProcessRoute(ProcessRoute processRoute)
        {
            try
            {
                // 验证输入参数
                var validation = ValidateProcessRoute(processRoute);
                if (!validation.IsValid)
                {
                    throw new MESException(validation.GetErrorsString());
                }

                // 验证工艺路线编码唯一性
                if (!IsRouteCodeUnique(processRoute.RouteCode))
                {
                    throw new MESException(string.Format("工艺路线编码 '{0}' 已存在", processRoute.RouteCode));
                }

                // 设置创建时间
                processRoute.CreateTime = DateTime.Now;
                processRoute.CreateUserId = 1; // TODO: 从当前用户获取
                processRoute.CreateUserName = "系统";

                // 验证工艺步骤
                if (processRoute.Steps != null && processRoute.Steps.Count > 0)
                {
                    for (int i = 0; i < processRoute.Steps.Count; i++)
                    {
                        var step = processRoute.Steps[i];
                        step.StepNumber = i + 1;
                        step.CreateTime = DateTime.Now;

                        var stepValidation = ValidateProcessStep(step);
                        if (!stepValidation.IsValid)
                        {
                            throw new MESException(string.Format("工艺步骤 {0} 验证失败：{1}", i + 1, stepValidation.GetErrorsString()));
                        }
                    }
                }

                var result = _processRouteDAL.AddProcessRoute(processRoute);
                if (result)
                {
                    LogManager.Info(string.Format("添加工艺路线成功：编码={0}, 名称={1}", processRoute.RouteCode, processRoute.RouteName));
                }
                else
                {
                    LogManager.Warning(string.Format("添加工艺路线失败：编码={0}", processRoute.RouteCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加工艺路线失败：编码={0}", processRoute != null ? processRoute.RouteCode : ""), ex);
                throw new MESException("添加工艺路线时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新工艺路线
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>是否成功</returns>
        public bool UpdateProcessRoute(ProcessRoute processRoute)
        {
            try
            {
                // 验证输入参数
                var validation = ValidateProcessRoute(processRoute);
                if (!validation.IsValid)
                {
                    throw new MESException(validation.GetErrorsString());
                }

                // 验证工艺路线是否存在
                var existingRoute = GetProcessRouteById(processRoute.Id);
                if (existingRoute == null)
                {
                    throw new MESException(string.Format("工艺路线不存在：ID={0}", processRoute.Id));
                }

                // 验证工艺路线编码唯一性（排除自身）
                if (!IsRouteCodeUnique(processRoute.RouteCode, processRoute.Id))
                {
                    throw new MESException(string.Format("工艺路线编码 '{0}' 已存在", processRoute.RouteCode));
                }

                // 设置更新时间
                processRoute.UpdateTime = DateTime.Now;
                processRoute.UpdateUserId = 1; // TODO: 从当前用户获取
                processRoute.UpdateUserName = "系统";

                // 验证工艺步骤
                if (processRoute.Steps != null && processRoute.Steps.Count > 0)
                {
                    for (int i = 0; i < processRoute.Steps.Count; i++)
                    {
                        var step = processRoute.Steps[i];
                        step.StepNumber = i + 1;
                        step.ProcessRouteId = processRoute.Id;

                        var stepValidation = ValidateProcessStep(step);
                        if (!stepValidation.IsValid)
                        {
                            throw new MESException(string.Format("工艺步骤 {0} 验证失败：{1}", i + 1, stepValidation.GetErrorsString()));
                        }
                    }
                }

                var result = _processRouteDAL.UpdateProcessRoute(processRoute);
                if (result)
                {
                    LogManager.Info(string.Format("更新工艺路线成功：编码={0}, 名称={1}", processRoute.RouteCode, processRoute.RouteName));
                }
                else
                {
                    LogManager.Warning(string.Format("更新工艺路线失败：编码={0}", processRoute.RouteCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新工艺路线失败：编码={0}", processRoute != null ? processRoute.RouteCode : ""), ex);
                throw new MESException("更新工艺路线时发生异常", ex);
            }
        }

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteProcessRoute(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("工艺路线ID必须大于0", "id");
                }

                // 验证是否可以删除
                if (!CanDeleteProcessRoute(id))
                {
                    throw new MESException("该工艺路线不能删除，可能正在使用中或状态不允许删除");
                }

                var existingRoute = GetProcessRouteById(id);
                if (existingRoute == null)
                {
                    throw new MESException(string.Format("工艺路线不存在：ID={0}", id));
                }

                var result = _processRouteDAL.DeleteProcessRoute(id);
                if (result)
                {
                    LogManager.Info(string.Format("删除工艺路线成功：ID={0}, 编码={1}", id, existingRoute.RouteCode));
                }
                else
                {
                    LogManager.Warning(string.Format("删除工艺路线失败：ID={0}", id));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除工艺路线失败：ID={0}", id), ex);
                throw new MESException("删除工艺路线时发生异常", ex);
            }
        }

        /// <summary>
        /// 复制工艺路线
        /// </summary>
        /// <param name="id">源工艺路线ID</param>
        /// <param name="newRouteCode">新工艺路线编码</param>
        /// <param name="newRouteName">新工艺路线名称</param>
        /// <returns>是否成功</returns>
        public bool CopyProcessRoute(int id, string newRouteCode, string newRouteName)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("源工艺路线ID必须大于0", "id");
                }

                if (string.IsNullOrWhiteSpace(newRouteCode))
                {
                    throw new ArgumentException("新工艺路线编码不能为空", "newRouteCode");
                }

                if (string.IsNullOrWhiteSpace(newRouteName))
                {
                    throw new ArgumentException("新工艺路线名称不能为空", "newRouteName");
                }

                var sourceRoute = GetProcessRouteById(id);
                if (sourceRoute == null)
                {
                    throw new MESException(string.Format("源工艺路线不存在：ID={0}", id));
                }

                // 验证新编码唯一性
                if (!IsRouteCodeUnique(newRouteCode))
                {
                    throw new MESException(string.Format("工艺路线编码 '{0}' 已存在", newRouteCode));
                }

                // 创建副本
                var copiedRoute = sourceRoute.Clone();
                copiedRoute.Id = 0; // 重置ID
                copiedRoute.RouteCode = newRouteCode;
                copiedRoute.RouteName = newRouteName;
                copiedRoute.Status = ProcessRouteStatus.Draft; // 复制的路线默认为草稿状态

                // 重置步骤ID
                if (copiedRoute.Steps != null)
                {
                    foreach (var step in copiedRoute.Steps)
                    {
                        step.Id = 0;
                        step.ProcessRouteId = 0;
                    }
                }

                var result = AddProcessRoute(copiedRoute);
                if (result)
                {
                    LogManager.Info(string.Format("复制工艺路线成功：源ID={0}, 新编码={1}", id, newRouteCode));
                }
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("复制工艺路线失败：源ID={0}", id), ex);
                throw new MESException("复制工艺路线时发生异常", ex);
            }
        }

        #endregion

        #region 查询和筛选

        /// <summary>
        /// 根据条件搜索工艺路线
        /// </summary>
        /// <param name="keyword">关键词（可选）</param>
        /// <param name="productId">产品ID（可选）</param>
        /// <param name="status">状态（可选）</param>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> SearchProcessRoutes(string keyword = null, int? productId = null, ProcessRouteStatus? status = null)
        {
            try
            {
                var routes = _processRouteDAL.SearchProcessRoutes(keyword, productId, status);
                LogManager.Info(string.Format("搜索工艺路线成功，条件：关键词={0}, 产品ID={1}, 状态={2}, 结果数量={3}",
                    keyword, productId, status, routes != null ? routes.Count : 0));
                return routes ?? new List<ProcessRoute>();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索工艺路线失败", ex);
                throw new MESException("搜索工艺路线时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据产品ID获取工艺路线
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetProcessRoutesByProductId(int productId)
        {
            try
            {
                if (productId <= 0)
                {
                    throw new ArgumentException("产品ID必须大于0", "productId");
                }

                return SearchProcessRoutes(null, productId, null);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据产品ID获取工艺路线失败：产品ID={0}", productId), ex);
                throw new MESException("获取工艺路线时发生异常", ex);
            }
        }


        public List<ProductionInfo> GetAllProducts()
        {
            try
            {
                return _processRouteDAL.GetAllProducts();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取产品列表失败", ex);
                throw new MESException("获取产品列表时发生异常", ex);
            }
        }


        /// <summary>
        /// 根据状态获取工艺路线
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetProcessRoutesByStatus(ProcessRouteStatus status)
        {
            try
            {
                return SearchProcessRoutes(null, null, status);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取工艺路线失败：状态={0}", status), ex);
                throw new MESException("获取工艺路线时发生异常", ex);
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
            if (routeId <= 0) throw new ArgumentException("工艺路线ID必须大于0", "routeId");
            return _processRouteDAL.GetProcessStepsByRouteId(routeId);
        }

        /// <summary>
        /// 添加工艺步骤
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <param name="step">工艺步骤</param>
        /// <returns>是否成功</returns>
        public bool AddProcessStep(int routeId, ProcessStep step)
        {
            if (routeId <= 0) throw new ArgumentException("工艺路线ID必须大于0", "routeId");
            var validation = ValidateProcessStep(step);
            if (!validation.IsValid) throw new MESException(validation.GetErrorsString());

            // 业务逻辑：自动设置新步骤的序号为当前数量+1
            step.StepNumber = GetProcessSteps(routeId).Count + 1;
            step.ProcessRouteId = routeId;
            step.CreateTime = DateTime.Now;

            return _processRouteDAL.AddProcessStep(step);
        }

        /// <summary>
        /// 更新工艺步骤
        /// </summary>
        /// <param name="step">工艺步骤</param>
        /// <returns>是否成功</returns>
        public bool UpdateProcessStep(ProcessStep step)
        {
            var validation = ValidateProcessStep(step);
            if (!validation.IsValid) throw new MESException(validation.GetErrorsString());
            step.UpdateTime = DateTime.Now;
            return _processRouteDAL.UpdateProcessStep(step);
        }

        /// <summary>
        /// 删除工艺步骤(包含核心业务逻辑：重新排序)
        /// </summary>
        /// <param name="stepId">工艺步骤ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteProcessStep(int stepId)
        {
            if (stepId <= 0) throw new ArgumentException("工艺步骤ID必须大于0", "stepId");

            // 1. 获取要删除的步骤，以便知道其所属的路线
            var stepToDelete = _processRouteDAL.GetProcessStepById(stepId);
            if (stepToDelete == null) throw new MESException("要删除的工艺步骤不存在。");

            // 2. 逻辑删除该步骤
            if (!_processRouteDAL.DeleteProcessStep(stepId)) return false;

            // 3. 业务逻辑核心：获取剩余步骤并重新排序
            var remainingSteps = _processRouteDAL.GetProcessStepsByRouteId(stepToDelete.ProcessRouteId);
            var stepsToUpdate = new List<ProcessStep>();
            for (int i = 0; i < remainingSteps.Count; i++)
            {
                if (remainingSteps[i].StepNumber != i + 1)
                {
                    remainingSteps[i].StepNumber = i + 1;
                    stepsToUpdate.Add(remainingSteps[i]);
                }
            }

            // 4. 批量更新需要调整序号的步骤
            return stepsToUpdate.Any() ? _processRouteDAL.UpdateStepNumbers(stepsToUpdate) : true;
        }

        /// <summary>
        /// 移动工艺步骤（上移/下移）
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <param name="stepId">工艺步骤ID</param>
        /// <param name="moveUp">是否上移（true上移，false下移）</param>
        /// <returns>是否成功</returns>
        public bool MoveProcessStep(int routeId, int stepId, bool moveUp)
        {
            var steps = _processRouteDAL.GetProcessStepsByRouteId(routeId);
            var stepToMove = steps.FirstOrDefault(s => s.Id == stepId);
            if (stepToMove == null) throw new MESException("未找到要移动的步骤。");

            int oldIndex = steps.FindIndex(s => s.Id == stepId);
            int newIndex = moveUp ? oldIndex - 1 : oldIndex + 1;

            if (newIndex < 0 || newIndex >= steps.Count) return true; // 已在顶部或底部

            var stepToSwap = steps[newIndex];

            // ---核心修改：使用 C# 5.0 兼容的临时变量法交换序号 ---
    // 1. 定义一个临时变量存储要移动步骤的序号
            int tempStepNumber = stepToMove.StepNumber;

            // 2. 将被交换步骤的序号赋值给要移动的步骤
            stepToMove.StepNumber = stepToSwap.StepNumber;

            // 3. 将临时变量中保存的原始序号赋值给被交换的步骤
            stepToSwap.StepNumber = tempStepNumber;
            // --- 修改结束 ---

            // 调用DAL一次性更新两个步骤
            return _processRouteDAL.UpdateStepNumbers(new List<ProcessStep> { stepToMove, stepToSwap });
        }

        #endregion

        #region 业务验证

        /// <summary>
        /// 验证工艺路线编码是否唯一
        /// </summary>
        /// <param name="routeCode">工艺路线编码</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        public bool IsRouteCodeUnique(string routeCode, int excludeId = 0)
        {
            return _processRouteDAL.IsRouteCodeUnique(routeCode, excludeId);
        }

        /// <summary>
        /// 验证工艺路线是否可以删除
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否可以删除</returns>
        public bool CanDeleteProcessRoute(int id)
        {
            return _processRouteDAL.CanDeleteProcessRoute(id);
        }

        /// <summary>
        /// 验证工艺路线数据
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>验证结果</returns>
        public ValidationResult ValidateProcessRoute(ProcessRoute route)
        {
            var result = new ValidationResult();
            if (route == null) { result.AddError("工艺路线对象不能为空。"); return result; }
            if (string.IsNullOrWhiteSpace(route.RouteCode)) result.AddError("工艺路线编码不能为空。");
            if (string.IsNullOrWhiteSpace(route.RouteName)) result.AddError("工艺路线名称不能为空。");
            if (route.ProductId <= 0) result.AddError("必须关联一个产品。");

            // 在编辑工艺路线时，步骤可以为空，因为步骤是单独管理的
            // if (route.Steps == null || route.Steps.Count == 0) result.AddError("工艺路线必须至少包含一个步骤。");

            // 验证步骤
            if (route.Steps != null)
            {
                foreach (var step in route.Steps)
                {
                    var stepValidation = ValidateProcessStep(step);
                    if (!stepValidation.IsValid)
                    {
                        result.AddErrors(stepValidation.Errors);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 验证工艺步骤数据
        /// </summary>
        /// <param name="step">工艺步骤</param>
        /// <returns>验证结果</returns>
        public ValidationResult ValidateProcessStep(ProcessStep step)
        {
            var result = new ValidationResult();
            if (step == null) { result.AddError("工艺步骤对象不能为空。"); return result; }
            if (string.IsNullOrWhiteSpace(step.StepName)) result.AddError("步骤名称不能为空。");
            if (step.WorkstationId <= 0) result.AddError("必须指定工作站。");
            if (step.StandardTime < 0) result.AddError("标准工时不能为负数。");
            return result;
        }

        #endregion

        #region 统计和报表

        /// <summary>
        /// 获取工艺路线统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public ProcessRouteStatistics GetProcessRouteStatistics()
        {
            try
            {
                var allRoutes = GetAllProcessRoutes();

                var statistics = new ProcessRouteStatistics
                {
                    TotalCount = allRoutes.Count,
                    ActiveCount = allRoutes.Count(r => r.Status == ProcessRouteStatus.Active),
                    InactiveCount = allRoutes.Count(r => r.Status == ProcessRouteStatus.Inactive),
                    DraftCount = allRoutes.Count(r => r.Status == ProcessRouteStatus.Draft),
                    AverageStepCount = allRoutes.Count > 0 ? (decimal)allRoutes.Average(r => r.StepCount) : 0,
                    AverageTotalTime = allRoutes.Count > 0 ? allRoutes.Average(r => r.TotalStandardTime) : 0
                };

                LogManager.Info("获取工艺路线统计信息成功");
                return statistics;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取工艺路线统计信息失败", ex);
                throw new MESException("获取统计信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取产品工艺路线分布
        /// </summary>
        /// <returns>分布统计</returns>
        public List<ProductRouteDistribution> GetProductRouteDistribution()
        {
            try
            {
                var allRoutes = GetAllProcessRoutes();

                var distribution = allRoutes
                    .GroupBy(r => new { r.ProductId, r.ProductName })
                    .Select(g => new ProductRouteDistribution
                    {
                        ProductId = g.Key.ProductId,
                        ProductName = g.Key.ProductName,
                        RouteCount = g.Count(),
                        ActiveRouteCount = g.Count(r => r.Status == ProcessRouteStatus.Active)
                    })
                    .OrderByDescending(d => d.RouteCount)
                    .ToList();

                LogManager.Info("获取产品工艺路线分布成功");
                return distribution;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取产品工艺路线分布失败", ex);
                throw new MESException("获取分布统计时发生异常", ex);
            }
        }

        #endregion
    }
}