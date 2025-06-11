// --- START OF FILE ProcessFlowDAL.cs ---

using MES.DAL.Base;
using MES.Models.Material;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace MES.DAL.Material
{
    /// <summary>
    /// 工艺流程数据访问层
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public class ProcessFlowDAL : BaseDAL
    {
        /// <summary>
        /// 获取所有工艺流程列表
        /// </summary>
        /// <returns>工艺流程数据表</returns>
        public DataTable GetAllProcessFlows()
        {
            string sql = @"
                SELECT pf.*, pp.package_name AS process_package_name
                FROM process_flow pf
                LEFT JOIN process_package pp ON pf.process_package_id = pp.id
                WHERE pf.is_deleted = 0
                ORDER BY pf.process_package_id, pf.flow_number";

            return ExecuteQuery(sql);
        }

        /// <summary>
        /// 根据工艺包ID获取工艺流程列表
        /// </summary>
        /// <param name="packageId">工艺包ID</param>
        /// <returns>工艺流程数据表</returns>
        public DataTable GetProcessFlowsByPackageId(int packageId)
        {
            string sql = @"
                SELECT pf.*, pp.package_name AS process_package_name
                FROM process_flow pf
                LEFT JOIN process_package pp ON pf.process_package_id = pp.id
                WHERE pf.is_deleted = 0 AND pf.process_package_id = @PackageId
                ORDER BY pf.flow_number";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@PackageId", packageId)
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 根据ID获取工艺流程详情
        /// </summary>
        /// <param name="id">工艺流程ID</param>
        /// <returns>工艺流程数据表</returns>
        public DataTable GetProcessFlowById(int id)
        {
            string sql = @"
                SELECT pf.*, pp.package_name AS process_package_name
                FROM process_flow pf
                LEFT JOIN process_package pp ON pf.process_package_id = pp.id
                WHERE pf.is_deleted = 0 AND pf.id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id)
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 添加工艺流程
        /// </summary>
        /// <param name="processFlow">工艺流程信息</param>
        /// <returns>新增记录的ID</returns>
        public int AddProcessFlow(ProcessFlow processFlow)
        {
            string sql = @"
                INSERT INTO process_flow 
                (flow_code, flow_name, process_package_id, flow_number, version, status, description, 
                 create_time, create_user_id, create_user_name, is_deleted)
                VALUES 
                (@FlowCode, @FlowName, @ProcessPackageId, @FlowNumber, @Version, @Status, @Description,
                 @CreateTime, @CreateUserId, @CreateUserName, @IsDeleted);
                SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@FlowCode", processFlow.FlowCode),
                new MySqlParameter("@FlowName", processFlow.FlowName),
                new MySqlParameter("@ProcessPackageId", processFlow.ProcessPackageId),
                new MySqlParameter("@FlowNumber", processFlow.FlowNumber),
                new MySqlParameter("@Version", processFlow.Version),
                new MySqlParameter("@Status", (int)processFlow.Status),
                new MySqlParameter("@Description", processFlow.Description ?? ""),
                new MySqlParameter("@CreateTime", processFlow.CreateTime),
                new MySqlParameter("@CreateUserId", processFlow.CreateUserId),
                new MySqlParameter("@CreateUserName", processFlow.CreateUserName ?? "系统"),
                new MySqlParameter("@IsDeleted", processFlow.IsDeleted)
            };

            object result = ExecuteScalar(sql, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// 更新工艺流程
        /// </summary>
        /// <param name="processFlow">工艺流程信息</param>
        /// <returns>是否成功</returns>
        public bool UpdateProcessFlow(ProcessFlow processFlow)
        {
            string sql = @"
                UPDATE process_flow SET
                    flow_code = @FlowCode,
                    flow_name = @FlowName,
                    process_package_id = @ProcessPackageId,
                    flow_number = @FlowNumber,
                    version = @Version,
                    status = @Status,
                    description = @Description,
                    update_time = @UpdateTime,
                    update_user_id = @UpdateUserId,
                    update_user_name = @UpdateUserName
                WHERE id = @Id AND is_deleted = 0";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", processFlow.Id),
                new MySqlParameter("@FlowCode", processFlow.FlowCode),
                new MySqlParameter("@FlowName", processFlow.FlowName),
                new MySqlParameter("@ProcessPackageId", processFlow.ProcessPackageId),
                new MySqlParameter("@FlowNumber", processFlow.FlowNumber),
                new MySqlParameter("@Version", processFlow.Version),
                new MySqlParameter("@Status", (int)processFlow.Status),
                new MySqlParameter("@Description", processFlow.Description ?? ""),
                new MySqlParameter("@UpdateTime", processFlow.UpdateTime),
                new MySqlParameter("@UpdateUserId", processFlow.UpdateUserId),
                new MySqlParameter("@UpdateUserName", processFlow.UpdateUserName ?? "")
            };

            int rowsAffected = ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 物理删除工艺流程
        /// </summary>
        /// <param name="id">工艺流程ID</param>
        /// <returns>是否成功</returns>
        public bool PhysicalDeleteProcessFlow(int id)
        {
            string sql = "DELETE FROM process_flow WHERE id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id)
            };

            int rowsAffected = ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 检查工艺流程编码是否存在
        /// </summary>
        /// <param name="flowCode">工艺流程编码</param>
        /// <param name="excludeId">排除的ID</param>
        /// <returns>是否存在</returns>
        public bool IsFlowCodeExists(string flowCode, int excludeId = 0)
        {
            string sql = @"
                SELECT COUNT(1) FROM process_flow 
                WHERE flow_code = @FlowCode AND is_deleted = 0";

            if (excludeId > 0)
            {
                sql += " AND id != @ExcludeId";
            }

            var parameters = excludeId > 0 
                ? new MySqlParameter[]
                {
                    new MySqlParameter("@FlowCode", flowCode),
                    new MySqlParameter("@ExcludeId", excludeId)
                }
                : new MySqlParameter[]
                {
                    new MySqlParameter("@FlowCode", flowCode)
                };

            object result = ExecuteScalar(sql, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 获取工艺流程统计信息
        /// </summary>
        /// <returns>统计信息数据表</returns>
        public DataTable GetProcessFlowStatistics()
        {
            string sql = @"
                SELECT 
                    COUNT(*) AS TotalCount,
                    SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) AS ActiveCount,
                    SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) AS InactiveCount,
                    SUM(CASE WHEN status = 0 THEN 1 ELSE 0 END) AS DraftCount,
                    COALESCE(AVG(route_counts.route_count), 0) AS AverageRouteCount,
                    COALESCE(AVG(route_counts.total_time), 0) AS AverageTotalTime
                FROM process_flow pf
                LEFT JOIN (
                    SELECT 
                        pr.process_flow_id,
                        COUNT(pr.id) AS route_count,
                        COALESCE(SUM(step_stats.total_time), 0) AS total_time
                    FROM process_route pr
                    LEFT JOIN (
                        SELECT 
                            ps.process_route_id,
                            COALESCE(SUM(ps.standard_time), 0) AS total_time
                        FROM process_step ps
                        WHERE ps.is_deleted = 0
                        GROUP BY ps.process_route_id
                    ) step_stats ON pr.id = step_stats.process_route_id
                    WHERE pr.is_deleted = 0
                    GROUP BY pr.process_flow_id
                ) route_counts ON pf.id = route_counts.process_flow_id
                WHERE pf.is_deleted = 0";

            return ExecuteQuery(sql);
        }

        /// <summary>
        /// 根据状态获取工艺流程列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>工艺流程数据表</returns>
        public DataTable GetProcessFlowsByStatus(ProcessFlowStatus status)
        {
            string sql = @"
                SELECT pf.*, pp.package_name AS process_package_name
                FROM process_flow pf
                LEFT JOIN process_package pp ON pf.process_package_id = pp.id
                WHERE pf.is_deleted = 0 AND pf.status = @Status
                ORDER BY pf.process_package_id, pf.flow_number";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Status", (int)status)
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 搜索工艺流程
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>工艺流程数据表</returns>
        public DataTable SearchProcessFlows(string keyword)
        {
            string sql = @"
                SELECT pf.*, pp.package_name AS process_package_name
                FROM process_flow pf
                LEFT JOIN process_package pp ON pf.process_package_id = pp.id
                WHERE pf.is_deleted = 0 
                AND (pf.flow_code LIKE @Keyword 
                     OR pf.flow_name LIKE @Keyword 
                     OR pp.package_name LIKE @Keyword)
                ORDER BY pf.process_package_id, pf.flow_number";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Keyword", "%" + keyword + "%")
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 获取工艺包流程分布统计
        /// </summary>
        /// <returns>分布统计数据表</returns>
        public DataTable GetPackageFlowDistribution()
        {
            string sql = @"
                SELECT 
                    pp.id AS ProcessPackageId,
                    pp.package_name AS ProcessPackageName,
                    COUNT(pf.id) AS FlowCount,
                    SUM(CASE WHEN pf.status = 1 THEN 1 ELSE 0 END) AS ActiveFlowCount
                FROM process_package pp
                LEFT JOIN process_flow pf ON pp.id = pf.process_package_id AND pf.is_deleted = 0
                WHERE pp.is_deleted = 0
                GROUP BY pp.id, pp.package_name
                HAVING FlowCount > 0
                ORDER BY FlowCount DESC";

            return ExecuteQuery(sql);
        }

        /// <summary>
        /// 获取工艺流程的最大序号
        /// </summary>
        /// <param name="packageId">工艺包ID</param>
        /// <returns>最大序号</returns>
        public int GetMaxFlowNumber(int packageId)
        {
            string sql = @"
                SELECT COALESCE(MAX(flow_number), 0) 
                FROM process_flow 
                WHERE process_package_id = @PackageId AND is_deleted = 0";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@PackageId", packageId)
            };

            object result = ExecuteScalar(sql, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}

// --- END OF FILE ProcessFlowDAL.cs ---
