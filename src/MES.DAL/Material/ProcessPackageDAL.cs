// --- START OF FILE ProcessPackageDAL.cs ---

using MES.DAL.Base;
using MES.Models.Material;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace MES.DAL.Material
{
    /// <summary>
    /// 工艺包数据访问层
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public class ProcessPackageDAL : BaseDAL
    {
        /// <summary>
        /// 获取所有工艺包列表
        /// </summary>
        /// <returns>工艺包数据表</returns>
        public DataTable GetAllProcessPackages()
        {
            string sql = @"
                SELECT pp.*, mi.material_name AS product_name
                FROM process_package pp
                LEFT JOIN material_info mi ON pp.product_id = mi.id
                WHERE pp.is_deleted = 0
                ORDER BY pp.create_time DESC";

            return ExecuteQuery(sql);
        }

        /// <summary>
        /// 根据产品ID获取工艺包列表
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺包数据表</returns>
        public DataTable GetProcessPackagesByProductId(int productId)
        {
            string sql = @"
                SELECT pp.*, mi.material_name AS product_name
                FROM process_package pp
                LEFT JOIN material_info mi ON pp.product_id = mi.id
                WHERE pp.is_deleted = 0 AND pp.product_id = @ProductId
                ORDER BY pp.create_time DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@ProductId", productId)
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 根据ID获取工艺包详情
        /// </summary>
        /// <param name="id">工艺包ID</param>
        /// <returns>工艺包数据表</returns>
        public DataTable GetProcessPackageById(int id)
        {
            string sql = @"
                SELECT pp.*, mi.material_name AS product_name
                FROM process_package pp
                LEFT JOIN material_info mi ON pp.product_id = mi.id
                WHERE pp.is_deleted = 0 AND pp.id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id)
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 添加工艺包
        /// </summary>
        /// <param name="processPackage">工艺包信息</param>
        /// <returns>新增记录的ID</returns>
        public int AddProcessPackage(ProcessPackage processPackage)
        {
            string sql = @"
                INSERT INTO process_package 
                (package_code, package_name, product_id, version, status, description, 
                 create_time, create_user_id, create_user_name, is_deleted)
                VALUES 
                (@PackageCode, @PackageName, @ProductId, @Version, @Status, @Description,
                 @CreateTime, @CreateUserId, @CreateUserName, @IsDeleted);
                SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@PackageCode", processPackage.PackageCode),
                new MySqlParameter("@PackageName", processPackage.PackageName),
                new MySqlParameter("@ProductId", processPackage.ProductId),
                new MySqlParameter("@Version", processPackage.Version),
                new MySqlParameter("@Status", (int)processPackage.Status),
                new MySqlParameter("@Description", processPackage.Description ?? ""),
                new MySqlParameter("@CreateTime", processPackage.CreateTime),
                new MySqlParameter("@CreateUserId", processPackage.CreateUserId),
                new MySqlParameter("@CreateUserName", processPackage.CreateUserName ?? "系统"),
                new MySqlParameter("@IsDeleted", processPackage.IsDeleted)
            };

            object result = ExecuteScalar(sql, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// 更新工艺包
        /// </summary>
        /// <param name="processPackage">工艺包信息</param>
        /// <returns>是否成功</returns>
        public bool UpdateProcessPackage(ProcessPackage processPackage)
        {
            string sql = @"
                UPDATE process_package SET
                    package_code = @PackageCode,
                    package_name = @PackageName,
                    product_id = @ProductId,
                    version = @Version,
                    status = @Status,
                    description = @Description,
                    update_time = @UpdateTime,
                    update_user_id = @UpdateUserId,
                    update_user_name = @UpdateUserName
                WHERE id = @Id AND is_deleted = 0";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", processPackage.Id),
                new MySqlParameter("@PackageCode", processPackage.PackageCode),
                new MySqlParameter("@PackageName", processPackage.PackageName),
                new MySqlParameter("@ProductId", processPackage.ProductId),
                new MySqlParameter("@Version", processPackage.Version),
                new MySqlParameter("@Status", (int)processPackage.Status),
                new MySqlParameter("@Description", processPackage.Description ?? ""),
                new MySqlParameter("@UpdateTime", processPackage.UpdateTime),
                new MySqlParameter("@UpdateUserId", processPackage.UpdateUserId),
                new MySqlParameter("@UpdateUserName", processPackage.UpdateUserName ?? "")
            };

            int rowsAffected = ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 物理删除工艺包
        /// </summary>
        /// <param name="id">工艺包ID</param>
        /// <returns>是否成功</returns>
        public bool PhysicalDeleteProcessPackage(int id)
        {
            string sql = "DELETE FROM process_package WHERE id = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id)
            };

            int rowsAffected = ExecuteNonQuery(sql, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 检查工艺包编码是否存在
        /// </summary>
        /// <param name="packageCode">工艺包编码</param>
        /// <param name="excludeId">排除的ID</param>
        /// <returns>是否存在</returns>
        public bool IsPackageCodeExists(string packageCode, int excludeId = 0)
        {
            string sql = @"
                SELECT COUNT(1) FROM process_package 
                WHERE package_code = @PackageCode AND is_deleted = 0";

            if (excludeId > 0)
            {
                sql += " AND id != @ExcludeId";
            }

            var parameters = excludeId > 0 
                ? new MySqlParameter[]
                {
                    new MySqlParameter("@PackageCode", packageCode),
                    new MySqlParameter("@ExcludeId", excludeId)
                }
                : new MySqlParameter[]
                {
                    new MySqlParameter("@PackageCode", packageCode)
                };

            object result = ExecuteScalar(sql, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 获取工艺包统计信息
        /// </summary>
        /// <returns>统计信息数据表</returns>
        public DataTable GetProcessPackageStatistics()
        {
            string sql = @"
                SELECT 
                    COUNT(*) AS TotalCount,
                    SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) AS ActiveCount,
                    SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) AS InactiveCount,
                    SUM(CASE WHEN status = 0 THEN 1 ELSE 0 END) AS DraftCount,
                    COALESCE(AVG(flow_counts.flow_count), 0) AS AverageFlowCount,
                    COALESCE(AVG(flow_counts.total_time), 0) AS AverageTotalTime
                FROM process_package pp
                LEFT JOIN (
                    SELECT 
                        pf.process_package_id,
                        COUNT(pf.id) AS flow_count,
                        COALESCE(SUM(route_stats.total_time), 0) AS total_time
                    FROM process_flow pf
                    LEFT JOIN (
                        SELECT 
                            pr.process_flow_id,
                            COALESCE(SUM(ps.standard_time), 0) AS total_time
                        FROM process_route pr
                        LEFT JOIN process_step ps ON pr.id = ps.process_route_id AND ps.is_deleted = 0
                        WHERE pr.is_deleted = 0
                        GROUP BY pr.process_flow_id
                    ) route_stats ON pf.id = route_stats.process_flow_id
                    WHERE pf.is_deleted = 0
                    GROUP BY pf.process_package_id
                ) flow_counts ON pp.id = flow_counts.process_package_id
                WHERE pp.is_deleted = 0";

            return ExecuteQuery(sql);
        }

        /// <summary>
        /// 根据状态获取工艺包列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>工艺包数据表</returns>
        public DataTable GetProcessPackagesByStatus(ProcessPackageStatus status)
        {
            string sql = @"
                SELECT pp.*, mi.material_name AS product_name
                FROM process_package pp
                LEFT JOIN material_info mi ON pp.product_id = mi.id
                WHERE pp.is_deleted = 0 AND pp.status = @Status
                ORDER BY pp.create_time DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Status", (int)status)
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 搜索工艺包
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>工艺包数据表</returns>
        public DataTable SearchProcessPackages(string keyword)
        {
            string sql = @"
                SELECT pp.*, mi.material_name AS product_name
                FROM process_package pp
                LEFT JOIN material_info mi ON pp.product_id = mi.id
                WHERE pp.is_deleted = 0 
                AND (pp.package_code LIKE @Keyword 
                     OR pp.package_name LIKE @Keyword 
                     OR mi.material_name LIKE @Keyword)
                ORDER BY pp.create_time DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Keyword", "%" + keyword + "%")
            };

            return ExecuteQuery(sql, parameters);
        }

        /// <summary>
        /// 获取产品工艺包分布统计
        /// </summary>
        /// <returns>分布统计数据表</returns>
        public DataTable GetProductPackageDistribution()
        {
            string sql = @"
                SELECT 
                    mi.id AS ProductId,
                    mi.material_name AS ProductName,
                    COUNT(pp.id) AS PackageCount,
                    SUM(CASE WHEN pp.status = 1 THEN 1 ELSE 0 END) AS ActivePackageCount
                FROM material_info mi
                LEFT JOIN process_package pp ON mi.id = pp.product_id AND pp.is_deleted = 0
                WHERE mi.is_deleted = 0
                GROUP BY mi.id, mi.material_name
                HAVING PackageCount > 0
                ORDER BY PackageCount DESC";

            return ExecuteQuery(sql);
        }
    }
}

// --- END OF FILE ProcessPackageDAL.cs ---
