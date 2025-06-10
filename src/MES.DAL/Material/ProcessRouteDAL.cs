using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Material;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Material
{
    /// <summary>
    /// 工艺路线数据访问层
    /// 提供工艺路线信息的数据库操作功能
    /// </summary>
    public class ProcessRouteDAL
    {
        #region 基本CRUD操作

        /// <summary>
        /// 获取所有工艺路线
        /// </summary>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetAllProcessRoutes()
        {
            try
            {
                string sql = @"
                    SELECT pr.*, p.product_name 
                    FROM process_route pr
                    LEFT JOIN product_info p ON pr.product_id = p.id
                    WHERE pr.is_deleted = 0
                    ORDER BY pr.create_time DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                var routes = ConvertDataTableToProcessRouteList(dataTable);

                // 加载每个路线的工艺步骤
                foreach (var route in routes)
                {
                    route.Steps = GetProcessStepsByRouteId(route.Id);
                }

                return routes;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有工艺路线失败", ex);
                throw new MESException("数据库操作失败", ex);
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
                string sql = @"
                    SELECT pr.*, p.product_name 
                    FROM process_route pr
                    LEFT JOIN product_info p ON pr.product_id = p.id
                    WHERE pr.id = @id AND pr.is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                var routes = ConvertDataTableToProcessRouteList(dataTable);
                
                if (routes.Count > 0)
                {
                    var route = routes[0];
                    route.Steps = GetProcessStepsByRouteId(route.Id);
                    return route;
                }

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取工艺路线失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
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
                string sql = @"
                    INSERT INTO process_route (
                        route_code, route_name, product_id, version, status, description,
                        create_user_id, create_user_name, create_time, is_deleted
                    ) VALUES (
                        @routeCode, @routeName, @productId, @version, @status, @description,
                        @createUserId, @createUserName, @createTime, @isDeleted
                    );
                    SELECT LAST_INSERT_ID();";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@routeCode", processRoute.RouteCode),
                    DatabaseHelper.CreateParameter("@routeName", processRoute.RouteName),
                    DatabaseHelper.CreateParameter("@productId", processRoute.ProductId),
                    DatabaseHelper.CreateParameter("@version", processRoute.Version),
                    DatabaseHelper.CreateParameter("@status", (int)processRoute.Status),
                    DatabaseHelper.CreateParameter("@description", processRoute.Description ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@createUserId", processRoute.CreateUserId),
                    DatabaseHelper.CreateParameter("@createUserName", processRoute.CreateUserName ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@createTime", processRoute.CreateTime),
                    DatabaseHelper.CreateParameter("@isDeleted", processRoute.IsDeleted)
                };

                var result = DatabaseHelper.ExecuteScalar(sql, parameters);
                if (result != null)
                {
                    processRoute.Id = Convert.ToInt32(result);
                    
                    // 添加工艺步骤
                    if (processRoute.Steps != null && processRoute.Steps.Count > 0)
                    {
                        foreach (var step in processRoute.Steps)
                        {
                            step.ProcessRouteId = processRoute.Id;
                            AddProcessStep(step);
                        }
                    }
                    
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加工艺路线失败：编码={0}", processRoute?.RouteCode), ex);
                throw new MESException("数据库操作失败", ex);
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
                string sql = @"
                    UPDATE process_route SET
                        route_code = @routeCode,
                        route_name = @routeName,
                        product_id = @productId,
                        version = @version,
                        status = @status,
                        description = @description,
                        update_user_id = @updateUserId,
                        update_user_name = @updateUserName,
                        update_time = @updateTime
                    WHERE id = @id AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", processRoute.Id),
                    DatabaseHelper.CreateParameter("@routeCode", processRoute.RouteCode),
                    DatabaseHelper.CreateParameter("@routeName", processRoute.RouteName),
                    DatabaseHelper.CreateParameter("@productId", processRoute.ProductId),
                    DatabaseHelper.CreateParameter("@version", processRoute.Version),
                    DatabaseHelper.CreateParameter("@status", (int)processRoute.Status),
                    DatabaseHelper.CreateParameter("@description", processRoute.Description ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateUserId", processRoute.UpdateUserId ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateUserName", processRoute.UpdateUserName ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateTime", processRoute.UpdateTime ?? DateTime.Now)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                if (result > 0)
                {
                    // 删除原有步骤
                    DeleteProcessStepsByRouteId(processRoute.Id);
                    
                    // 添加新步骤
                    if (processRoute.Steps != null && processRoute.Steps.Count > 0)
                    {
                        foreach (var step in processRoute.Steps)
                        {
                            step.ProcessRouteId = processRoute.Id;
                            AddProcessStep(step);
                        }
                    }
                }
                
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新工艺路线失败：编码={0}", processRoute?.RouteCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 删除工艺路线（逻辑删除）
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteProcessRoute(int id)
        {
            try
            {
                string sql = @"
                    UPDATE process_route SET 
                        is_deleted = 1,
                        update_time = @updateTime
                    WHERE id = @id";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                if (result > 0)
                {
                    // 同时删除相关的工艺步骤
                    DeleteProcessStepsByRouteId(id);
                }
                
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除工艺路线失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
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
                string sql = @"
                    SELECT pr.*, p.product_name 
                    FROM process_route pr
                    LEFT JOIN product_info p ON pr.product_id = p.id
                    WHERE pr.is_deleted = 0";

                var parameters = new List<MySqlParameter>();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    sql += " AND (pr.route_code LIKE @keyword OR pr.route_name LIKE @keyword OR p.product_name LIKE @keyword)";
                    parameters.Add(DatabaseHelper.CreateParameter("@keyword", string.Format("%{0}%", keyword)));
                }

                if (productId.HasValue && productId.Value > 0)
                {
                    sql += " AND pr.product_id = @productId";
                    parameters.Add(DatabaseHelper.CreateParameter("@productId", productId.Value));
                }

                if (status.HasValue)
                {
                    sql += " AND pr.status = @status";
                    parameters.Add(DatabaseHelper.CreateParameter("@status", (int)status.Value));
                }

                sql += " ORDER BY pr.create_time DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());
                var routes = ConvertDataTableToProcessRouteList(dataTable);

                // 加载每个路线的工艺步骤
                foreach (var route in routes)
                {
                    route.Steps = GetProcessStepsByRouteId(route.Id);
                }

                return routes;
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索工艺路线失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        #endregion

        #region 工艺步骤操作

        /// <summary>
        /// 根据工艺路线ID获取工艺步骤列表
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <returns>工艺步骤列表</returns>
        public List<ProcessStep> GetProcessStepsByRouteId(int routeId)
        {
            try
            {
                string sql = @"
                    SELECT ps.*, w.workstation_name
                    FROM process_step ps
                    LEFT JOIN workstation_info w ON ps.workstation_id = w.id
                    WHERE ps.process_route_id = @routeId AND ps.is_deleted = 0
                    ORDER BY ps.step_number";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@routeId", routeId)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToProcessStepList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取工艺步骤失败：路线ID={0}", routeId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 添加工艺步骤
        /// </summary>
        /// <param name="step">工艺步骤</param>
        /// <returns>是否成功</returns>
        public bool AddProcessStep(ProcessStep step)
        {
            try
            {
                string sql = @"
                    INSERT INTO process_step (
                        process_route_id, step_number, step_name, step_type, workstation_id,
                        standard_time, setup_time, wait_time, description, operation_instructions,
                        quality_requirements, safety_notes, required_skill_level, is_critical,
                        requires_inspection, status, create_time, is_deleted
                    ) VALUES (
                        @processRouteId, @stepNumber, @stepName, @stepType, @workstationId,
                        @standardTime, @setupTime, @waitTime, @description, @operationInstructions,
                        @qualityRequirements, @safetyNotes, @requiredSkillLevel, @isCritical,
                        @requiresInspection, @status, @createTime, @isDeleted
                    )";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@processRouteId", step.ProcessRouteId),
                    DatabaseHelper.CreateParameter("@stepNumber", step.StepNumber),
                    DatabaseHelper.CreateParameter("@stepName", step.StepName),
                    DatabaseHelper.CreateParameter("@stepType", (int)step.StepType),
                    DatabaseHelper.CreateParameter("@workstationId", step.WorkstationId),
                    DatabaseHelper.CreateParameter("@standardTime", step.StandardTime),
                    DatabaseHelper.CreateParameter("@setupTime", step.SetupTime),
                    DatabaseHelper.CreateParameter("@waitTime", step.WaitTime),
                    DatabaseHelper.CreateParameter("@description", step.Description ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@operationInstructions", step.OperationInstructions ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@qualityRequirements", step.QualityRequirements ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@safetyNotes", step.SafetyNotes ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@requiredSkillLevel", step.RequiredSkillLevel),
                    DatabaseHelper.CreateParameter("@isCritical", step.IsCritical),
                    DatabaseHelper.CreateParameter("@requiresInspection", step.RequiresInspection),
                    DatabaseHelper.CreateParameter("@status", (int)step.Status),
                    DatabaseHelper.CreateParameter("@createTime", step.CreateTime),
                    DatabaseHelper.CreateParameter("@isDeleted", step.IsDeleted)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加工艺步骤失败：步骤名称={0}", step?.StepName), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 删除工艺路线的所有工艺步骤
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteProcessStepsByRouteId(int routeId)
        {
            try
            {
                string sql = @"
                    UPDATE process_step SET
                        is_deleted = 1,
                        update_time = @updateTime
                    WHERE process_route_id = @routeId";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@routeId", routeId),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result >= 0; // 可能没有步骤，所以>=0就算成功
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除工艺步骤失败：路线ID={0}", routeId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 将DataTable转换为工艺步骤列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>工艺步骤列表</returns>
        private List<ProcessStep> ConvertDataTableToProcessStepList(DataTable dataTable)
        {
            var steps = new List<ProcessStep>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return steps;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var step = new ProcessStep
                {
                    Id = Convert.ToInt32(row["id"]),
                    ProcessRouteId = Convert.ToInt32(row["process_route_id"]),
                    StepNumber = Convert.ToInt32(row["step_number"]),
                    StepName = row["step_name"].ToString(),
                    StepType = (ProcessStepType)Convert.ToInt32(row["step_type"]),
                    WorkstationId = Convert.ToInt32(row["workstation_id"]),
                    WorkstationName = row["workstation_name"] != DBNull.Value ? row["workstation_name"].ToString() : "",
                    StandardTime = Convert.ToDecimal(row["standard_time"]),
                    SetupTime = Convert.ToDecimal(row["setup_time"]),
                    WaitTime = Convert.ToDecimal(row["wait_time"]),
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    OperationInstructions = row["operation_instructions"] != DBNull.Value ? row["operation_instructions"].ToString() : null,
                    QualityRequirements = row["quality_requirements"] != DBNull.Value ? row["quality_requirements"].ToString() : null,
                    SafetyNotes = row["safety_notes"] != DBNull.Value ? row["safety_notes"].ToString() : null,
                    RequiredSkillLevel = Convert.ToInt32(row["required_skill_level"]),
                    IsCritical = Convert.ToBoolean(row["is_critical"]),
                    RequiresInspection = Convert.ToBoolean(row["requires_inspection"]),
                    Status = (ProcessStepStatus)Convert.ToInt32(row["status"]),
                    CreateTime = Convert.ToDateTime(row["create_time"]),
                    UpdateTime = row["update_time"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["update_time"]) : null,
                    IsDeleted = Convert.ToBoolean(row["is_deleted"])
                };

                steps.Add(step);
            }

            return steps;
        }

        #endregion

        #region 验证方法

        /// <summary>
        /// 验证工艺路线编码是否唯一
        /// </summary>
        /// <param name="routeCode">工艺路线编码</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        public bool IsRouteCodeUnique(string routeCode, int excludeId = 0)
        {
            try
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM process_route
                    WHERE route_code = @routeCode AND is_deleted = 0";

                var parameters = new List<MySqlParameter>
                {
                    DatabaseHelper.CreateParameter("@routeCode", routeCode)
                };

                if (excludeId > 0)
                {
                    sql += " AND id != @excludeId";
                    parameters.Add(DatabaseHelper.CreateParameter("@excludeId", excludeId));
                }

                var result = DatabaseHelper.ExecuteScalar(sql, parameters.ToArray());
                return Convert.ToInt32(result) == 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证工艺路线编码唯一性失败：编码={0}", routeCode), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证工艺路线是否可以删除
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否可以删除</returns>
        public bool CanDeleteProcessRoute(int id)
        {
            try
            {
                // 检查工艺路线状态，启用状态的不能删除
                string sql = @"
                    SELECT status
                    FROM process_route
                    WHERE id = @id AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id)
                };

                var result = DatabaseHelper.ExecuteScalar(sql, parameters);
                if (result != null)
                {
                    var status = (ProcessRouteStatus)Convert.ToInt32(result);
                    // 启用状态的工艺路线不能删除
                    return status != ProcessRouteStatus.Active;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证工艺路线是否可删除失败：ID={0}", id), ex);
                return false;
            }
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 将DataTable转换为工艺路线列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>工艺路线列表</returns>
        private List<ProcessRoute> ConvertDataTableToProcessRouteList(DataTable dataTable)
        {
            var routes = new List<ProcessRoute>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return routes;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var route = new ProcessRoute
                {
                    Id = Convert.ToInt32(row["id"]),
                    RouteCode = row["route_code"].ToString(),
                    RouteName = row["route_name"].ToString(),
                    ProductId = Convert.ToInt32(row["product_id"]),
                    ProductName = row["product_name"] != DBNull.Value ? row["product_name"].ToString() : "",
                    Version = row["version"].ToString(),
                    Status = (ProcessRouteStatus)Convert.ToInt32(row["status"]),
                    Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                    CreateUserId = Convert.ToInt32(row["create_user_id"]),
                    CreateUserName = row["create_user_name"] != DBNull.Value ? row["create_user_name"].ToString() : null,
                    CreateTime = Convert.ToDateTime(row["create_time"]),
                    UpdateUserId = row["update_user_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["update_user_id"]) : null,
                    UpdateUserName = row["update_user_name"] != DBNull.Value ? row["update_user_name"].ToString() : null,
                    UpdateTime = row["update_time"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["update_time"]) : null,
                    IsDeleted = Convert.ToBoolean(row["is_deleted"])
                };

                routes.Add(route);
            }

            return routes;
        }

        #endregion
    }
}
