// --- START OF FILE ProcessRouteDAL.cs ---

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using MES.Models.Material;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;
using MES.Models.Production;

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
                string sql = "SELECT pr.*, p.product_name FROM process_route pr LEFT JOIN product_info p ON pr.product_id = p.id WHERE pr.is_deleted = 0 ORDER BY pr.create_time DESC";
                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                var routes = ConvertDataTableToProcessRouteList(dataTable);
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
                string sql = "SELECT pr.*, p.product_name FROM process_route pr LEFT JOIN product_info p ON pr.product_id = p.id WHERE pr.id = @id AND pr.is_deleted = 0";
                var parameters = new[] { DatabaseHelper.CreateParameter("@id", id) };
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
            using (var connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    string sql = @"
                        INSERT INTO process_route (route_code, route_name, product_id, version, status, description, create_user_id, create_user_name, create_time, is_deleted) 
                        VALUES (@routeCode, @routeName, @productId, @version, @status, @description, @createUserId, @createUserName, @createTime, 0);
                        SELECT LAST_INSERT_ID();";

                    var cmd = new MySqlCommand(sql, connection, transaction);
                    cmd.Parameters.AddRange(new[]
                    {
                        DatabaseHelper.CreateParameter("@routeCode", processRoute.RouteCode),
                        DatabaseHelper.CreateParameter("@routeName", processRoute.RouteName),
                        DatabaseHelper.CreateParameter("@productId", processRoute.ProductId),
                        DatabaseHelper.CreateParameter("@version", processRoute.Version),
                        DatabaseHelper.CreateParameter("@status", (int)processRoute.Status),
                        DatabaseHelper.CreateParameter("@description", processRoute.Description),
                        DatabaseHelper.CreateParameter("@createUserId", processRoute.CreateUserId),
                        DatabaseHelper.CreateParameter("@createUserName", processRoute.CreateUserName),
                        DatabaseHelper.CreateParameter("@createTime", processRoute.CreateTime)
                    });

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        processRoute.Id = Convert.ToInt32(result);
                        if (processRoute.Steps != null)
                        {
                            foreach (var step in processRoute.Steps)
                            {
                                step.ProcessRouteId = processRoute.Id;
                                AddProcessStepInTransaction(step, connection, transaction);
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    transaction.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Error(string.Format("添加工艺路线失败：编码={0}", processRoute?.RouteCode), ex);
                    throw new MESException("数据库事务操作失败", ex);
                }
            }
        }

        /// <summary>
        /// 更新工艺路线
        /// </summary>
        /// <param name="processRoute">工艺路线</param>
        /// <returns>是否成功</returns>
        public bool UpdateProcessRoute(ProcessRoute processRoute)
        {
            // 因为此方法现在只更新主表，不再需要事务
            try
            {
                const string sql = @"
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
                    WHERE id = @id";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@routeCode", processRoute.RouteCode),
                    DatabaseHelper.CreateParameter("@routeName", processRoute.RouteName),
                    DatabaseHelper.CreateParameter("@productId", processRoute.ProductId),
                    DatabaseHelper.CreateParameter("@version", processRoute.Version),
                    DatabaseHelper.CreateParameter("@status", (int)processRoute.Status),
                    DatabaseHelper.CreateParameter("@description", processRoute.Description),
                    DatabaseHelper.CreateParameter("@updateUserId", processRoute.UpdateUserId),
                    DatabaseHelper.CreateParameter("@updateUserName", processRoute.UpdateUserName),
                    DatabaseHelper.CreateParameter("@updateTime", processRoute.UpdateTime),
                    DatabaseHelper.CreateParameter("@id", processRoute.Id)
                };

                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新工艺路线失败：ID={processRoute?.Id}", ex);
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
                // 使用物理删除，真实从数据库中移除记录
                return PhysicalDeleteProcessRoute(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除工艺路线失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 逻辑删除工艺路线（软删除，可恢复）
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否删除成功</returns>
        public bool SoftDeleteProcessRoute(int id)
        {
            try
            {
                string sql = "UPDATE process_route SET is_deleted = 1, update_time = @updateTime WHERE id = @id";
                return DatabaseHelper.ExecuteNonQuery(sql, new[] { DatabaseHelper.CreateParameter("@updateTime", DateTime.Now), DatabaseHelper.CreateParameter("@id", id) }) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("逻辑删除工艺路线失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 物理删除工艺路线（真实删除，不可恢复）
        /// </summary>
        /// <param name="id">工艺路线ID</param>
        /// <returns>是否删除成功</returns>
        public bool PhysicalDeleteProcessRoute(int id)
        {
            try
            {
                // 先删除相关的工艺步骤
                string deleteStepsSQL = "DELETE FROM process_step WHERE process_route_id = @id";
                DatabaseHelper.ExecuteNonQuery(deleteStepsSQL, new[] { DatabaseHelper.CreateParameter("@id", id) });

                // 再删除工艺路线
                string deleteRouteSQL = "DELETE FROM process_route WHERE id = @id";
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(deleteRouteSQL, new[] { DatabaseHelper.CreateParameter("@id", id) });

                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info(string.Format("物理删除ProcessRoute成功，ID: {0}", id));
                }

                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("物理删除ProcessRoute失败，ID: {0}", id), ex);
                throw new MESException("物理删除工艺路线失败", ex);
            }
        }


        public List<ProductionInfo> GetAllProducts()
        {
            try
            {
                string sql = "SELECT p.id, p.product_code, p.product_name, p.product_type, p.detail_type, p.package_type, p.unit, p.description, p.status, p.create_time, p.update_time FROM product_info p WHERE p.is_deleted = 0";
                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                var products = new List<ProductionInfo>();

                foreach (DataRow row in dataTable.Rows)
                {
                    var product = new ProductionInfo
                    {
                        ProductId = Convert.ToInt32(row["id"]),
                        ProductNum = row["product_code"].ToString(),
                        ProductName = row["product_name"].ToString(),
                        ProductType = row["product_type"].ToString(),
                        DetailType = row["detail_type"].ToString(),
                        PackageType = row["package_type"].ToString(),
                        Unit = row["unit"].ToString(),
                        Description = row["description"].ToString(),
                        Status = Convert.ToInt32(row["status"]),
                        CreateTime = Convert.ToDateTime(row["create_time"]),
                        UpdateTime = row["update_time"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["update_time"]) : null
                    };
                    products.Add(product);
                }

                return products;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取产品列表失败", ex);
                throw new MESException("获取产品列表时发生异常", ex);
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

        /// <summary>
        /// 根据工艺流程ID获取工艺路线列表
        /// </summary>
        /// <param name="flowId">工艺流程ID</param>
        /// <returns>工艺路线列表</returns>
        public List<ProcessRoute> GetProcessRoutesByFlowId(int flowId)
        {
            try
            {
                string sql = @"
                    SELECT pr.*, p.product_name
                    FROM process_route pr
                    LEFT JOIN product_info p ON pr.product_id = p.id
                    WHERE pr.is_deleted = 0 AND pr.process_flow_id = @flowId
                    ORDER BY pr.create_time DESC";

                var parameters = new[] { DatabaseHelper.CreateParameter("@flowId", flowId) };
                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
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
                LogManager.Error(string.Format("根据工艺流程ID获取工艺路线失败：流程ID={0}", flowId), ex);
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
                string sql = "SELECT ps.*, w.workstation_name FROM process_step ps LEFT JOIN workstation_info w ON ps.workstation_id = w.id WHERE ps.process_route_id = @routeId AND ps.is_deleted = 0 ORDER BY ps.step_number";
                var dataTable = DatabaseHelper.ExecuteQuery(sql, new[] { DatabaseHelper.CreateParameter("@routeId", routeId) });
                return ConvertDataTableToProcessStepList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取工艺步骤失败：路线ID={0}", routeId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// [新增] 根据ID获取单个工艺步骤
        /// </summary>
        public ProcessStep GetProcessStepById(int stepId)
        {
            try
            {
                string sql = "SELECT ps.*, w.workstation_name FROM process_step ps LEFT JOIN workstation_info w ON ps.workstation_id = w.id WHERE ps.id = @id AND ps.is_deleted = 0";
                var dataTable = DatabaseHelper.ExecuteQuery(sql, new[] { DatabaseHelper.CreateParameter("@id", stepId) });
                var steps = ConvertDataTableToProcessStepList(dataTable);
                return steps.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取单个工艺步骤失败：步骤ID={0}", stepId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// [新增] 添加单个工艺步骤
        /// </summary>
        public bool AddProcessStep(ProcessStep step)
        {
            try
            {
                string sql = @"
            INSERT INTO process_step (
                process_route_id, step_number, step_name, step_type, workstation_id, port_number,
                standard_time, setup_time, wait_time, description, operation_instructions,
                quality_requirements, safety_notes, required_skill_level, is_critical,
                requires_inspection, status, create_time, is_deleted
            ) VALUES (
                @processRouteId, @stepNumber, @stepName, @stepType, @workstationId, @portNumber,
                @standardTime, @setupTime, @waitTime, @description, @operationInstructions,
                @qualityRequirements, @safetyNotes, @requiredSkillLevel, @isCritical,
                @requiresInspection, @status, @createTime, 0
            )";
                var parameters = new[] {
            DatabaseHelper.CreateParameter("@processRouteId", step.ProcessRouteId),
            DatabaseHelper.CreateParameter("@stepNumber", step.StepNumber),
            DatabaseHelper.CreateParameter("@stepName", step.StepName),
            DatabaseHelper.CreateParameter("@stepType", (int)step.StepType),
            DatabaseHelper.CreateParameter("@workstationId", step.WorkstationId),
            DatabaseHelper.CreateParameter("@portNumber", step.PortNumber),
            DatabaseHelper.CreateParameter("@standardTime", step.StandardTime),
            DatabaseHelper.CreateParameter("@setupTime", step.SetupTime),
            DatabaseHelper.CreateParameter("@waitTime", step.WaitTime),
            DatabaseHelper.CreateParameter("@description", step.Description),
            DatabaseHelper.CreateParameter("@operationInstructions", step.OperationInstructions),
            DatabaseHelper.CreateParameter("@qualityRequirements", step.QualityRequirements),
            DatabaseHelper.CreateParameter("@safetyNotes", step.SafetyNotes),
            DatabaseHelper.CreateParameter("@requiredSkillLevel", step.RequiredSkillLevel),
            DatabaseHelper.CreateParameter("@isCritical", step.IsCritical),
            DatabaseHelper.CreateParameter("@requiresInspection", step.RequiresInspection),
            DatabaseHelper.CreateParameter("@status", (int)step.Status),
            DatabaseHelper.CreateParameter("@createTime", step.CreateTime)
        };
                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error("添加工艺步骤失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// [修改] 更新单个工艺步骤的完整信息
        /// </summary>
        public bool UpdateProcessStep(ProcessStep step)
        {
            try
            {
                string sql = @"
            UPDATE process_step SET
                step_number = @stepNumber, step_name = @stepName, step_type = @stepType,
                workstation_id = @workstationId, port_number = @portNumber, standard_time = @standardTime, setup_time = @setupTime,
                wait_time = @waitTime, description = @description, operation_instructions = @operationInstructions,
                quality_requirements = @qualityRequirements, safety_notes = @safetyNotes,
                required_skill_level = @requiredSkillLevel, is_critical = @isCritical,
                requires_inspection = @requiresInspection, status = @status, update_time = @updateTime
            WHERE id = @id";

                var parameters = new[]
                {
            DatabaseHelper.CreateParameter("@stepNumber", step.StepNumber),
            DatabaseHelper.CreateParameter("@stepName", step.StepName),
            DatabaseHelper.CreateParameter("@stepType", (int)step.StepType),
            DatabaseHelper.CreateParameter("@workstationId", step.WorkstationId),
            DatabaseHelper.CreateParameter("@portNumber", step.PortNumber),
            DatabaseHelper.CreateParameter("@standardTime", step.StandardTime),
            DatabaseHelper.CreateParameter("@setupTime", step.SetupTime),
            DatabaseHelper.CreateParameter("@waitTime", step.WaitTime),
            DatabaseHelper.CreateParameter("@description", step.Description),
            DatabaseHelper.CreateParameter("@operationInstructions", step.OperationInstructions),
            DatabaseHelper.CreateParameter("@qualityRequirements", step.QualityRequirements),
            DatabaseHelper.CreateParameter("@safetyNotes", step.SafetyNotes),
            DatabaseHelper.CreateParameter("@requiredSkillLevel", step.RequiredSkillLevel),
            DatabaseHelper.CreateParameter("@isCritical", step.IsCritical),
            DatabaseHelper.CreateParameter("@requiresInspection", step.RequiresInspection),
            DatabaseHelper.CreateParameter("@status", (int)step.Status),
            DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
            DatabaseHelper.CreateParameter("@id", step.Id)
        };
                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新工艺步骤失败：步骤ID={0}", step.Id), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// [新增] 逻辑删除单个工艺步骤
        /// </summary>
        public bool DeleteProcessStep(int stepId)
        {
            try
            {
                string sql = "UPDATE process_step SET is_deleted = 1, update_time = @updateTime WHERE id = @id";
                var parameters = new[] {
            DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
            DatabaseHelper.CreateParameter("@id", stepId)
        };
                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除工艺步骤失败：步骤ID={0}", stepId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// [新增] 在事务中批量更新步骤序号
        /// </summary>
        public bool UpdateStepNumbers(List<ProcessStep> stepsToUpdate)
        {
            if (stepsToUpdate == null || !stepsToUpdate.Any())
            {
                LogManager.Info("批量更新步骤序号：没有需要更新的步骤");
                return true;
            }

            LogManager.Info(string.Format("开始批量更新步骤序号，共 {0} 个步骤", stepsToUpdate.Count));

            using (var connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    string sql = "UPDATE process_step SET step_number = @stepNumber, update_time = @updateTime WHERE id = @id";
                    int updatedCount = 0;

                    foreach (var step in stepsToUpdate)
                    {
                        if (step.Id <= 0)
                        {
                            LogManager.Warning(string.Format("跳过无效的步骤ID：{0}", step.Id));
                            continue;
                        }

                        var cmd = new MySqlCommand(sql, connection, transaction);
                        cmd.Parameters.AddRange(new[]
                        {
                            DatabaseHelper.CreateParameter("@stepNumber", step.StepNumber),
                            DatabaseHelper.CreateParameter("@updateTime", step.UpdateTime ?? DateTime.Now),
                            DatabaseHelper.CreateParameter("@id", step.Id)
                        });

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            updatedCount++;
                            LogManager.Info(string.Format("更新步骤序号成功：ID={0}，新序号={1}", step.Id, step.StepNumber));
                        }
                        else
                        {
                            LogManager.Warning(string.Format("更新步骤序号失败：ID={0}，可能步骤不存在", step.Id));
                        }
                    }

                    transaction.Commit();
                    LogManager.Info(string.Format("批量更新步骤序号完成，成功更新 {0} 个步骤", updatedCount));
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Error("批量更新步骤序号失败，事务已回滚", ex);
                    throw new MESException("数据库事务操作失败", ex);
                }
            }
        }


        /// <summary>
        /// 添加工艺步骤（在事务中调用）
        /// </summary>
        private void AddProcessStepInTransaction(ProcessStep step, MySqlConnection conn, MySqlTransaction trans)
        {
            string sql = @"
                INSERT INTO process_step (
                    process_route_id, step_number, step_name, step_type, workstation_id, port_number,
                    standard_time, setup_time, wait_time, description, operation_instructions,
                    quality_requirements, safety_notes, required_skill_level, is_critical,
                    requires_inspection, status, create_time, is_deleted
                ) VALUES (
                    @processRouteId, @stepNumber, @stepName, @stepType, @workstationId, @portNumber,
                    @standardTime, @setupTime, @waitTime, @description, @operationInstructions,
                    @qualityRequirements, @safetyNotes, @requiredSkillLevel, @isCritical,
                    @requiresInspection, @status, @createTime, 0
                )";
            var cmd = new MySqlCommand(sql, conn, trans);
            cmd.Parameters.AddRange(new[]
            {
                DatabaseHelper.CreateParameter("@processRouteId", step.ProcessRouteId),
                DatabaseHelper.CreateParameter("@stepNumber", step.StepNumber),
                DatabaseHelper.CreateParameter("@stepName", step.StepName),
                DatabaseHelper.CreateParameter("@stepType", (int)step.StepType),
                DatabaseHelper.CreateParameter("@workstationId", step.WorkstationId),
                DatabaseHelper.CreateParameter("@portNumber", step.PortNumber),
                DatabaseHelper.CreateParameter("@standardTime", step.StandardTime),
                DatabaseHelper.CreateParameter("@setupTime", step.SetupTime),
                DatabaseHelper.CreateParameter("@waitTime", step.WaitTime),
                DatabaseHelper.CreateParameter("@description", step.Description),
                DatabaseHelper.CreateParameter("@operationInstructions", step.OperationInstructions),
                DatabaseHelper.CreateParameter("@qualityRequirements", step.QualityRequirements),
                DatabaseHelper.CreateParameter("@safetyNotes", step.SafetyNotes),
                DatabaseHelper.CreateParameter("@requiredSkillLevel", step.RequiredSkillLevel),
                DatabaseHelper.CreateParameter("@isCritical", step.IsCritical),
                DatabaseHelper.CreateParameter("@requiresInspection", step.RequiresInspection),
                DatabaseHelper.CreateParameter("@status", (int)step.Status),
                DatabaseHelper.CreateParameter("@createTime", step.CreateTime)
            });
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 软删除指定工艺路线的所有步骤（在事务中使用）
        /// </summary>
        private void DeleteAllProcessStepsByRouteIdInTransaction(int routeId, MySqlConnection conn, MySqlTransaction trans)
        {
            string sql = "UPDATE process_step SET is_deleted = 1, update_time = @updateTime WHERE process_route_id = @routeId AND is_deleted = 0";
            var cmd = new MySqlCommand(sql, conn, trans);
            cmd.Parameters.Add(DatabaseHelper.CreateParameter("@updateTime", DateTime.Now));
            cmd.Parameters.Add(DatabaseHelper.CreateParameter("@routeId", routeId));
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 将DataTable转换为工艺步骤列表
        /// </summary>
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
                    PortNumber = row["port_number"] != DBNull.Value ? row["port_number"].ToString() : null,
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