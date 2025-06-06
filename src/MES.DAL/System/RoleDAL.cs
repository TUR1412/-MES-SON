using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.DAL.Base;
using MES.Models.System;
using MES.Common.Logging;

namespace MES.DAL.System
{
    /// <summary>
    /// 角色信息数据访问类
    /// 提供角色管理的数据库操作功能
    /// </summary>
    public class RoleDAL : BaseDAL<RoleInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName => "sys_role";

        /// <summary>
        /// 主键字段名
        /// </summary>
        protected override string PrimaryKey => "id";

        /// <summary>
        /// 将DataRow转换为RoleInfo对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>角色信息对象</returns>
        protected override RoleInfo MapRowToEntity(DataRow row)
        {
            return new RoleInfo
            {
                Id = Convert.ToInt32(row["id"]),
                RoleCode = row["role_code"]?.ToString() ?? string.Empty,
                RoleName = row["role_name"]?.ToString() ?? string.Empty,
                Description = row["description"]?.ToString() ?? string.Empty,
                Status = Convert.ToInt32(row["status"]),
                Permissions = row["permissions"]?.ToString() ?? string.Empty,
                SortOrder = Convert.ToInt32(row["sort_order"]),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserId = row["create_user_id"] != DBNull.Value ? Convert.ToInt32(row["create_user_id"]) : 0,
                CreateUserName = row["create_user_name"]?.ToString() ?? string.Empty,
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                UpdateUserId = row["update_user_id"] != DBNull.Value ? Convert.ToInt32(row["update_user_id"]) : 0,
                UpdateUserName = row["update_user_name"]?.ToString() ?? string.Empty,
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        /// <summary>
        /// 获取插入SQL语句
        /// </summary>
        /// <returns>插入SQL</returns>
        protected override string GetInsertSql()
        {
            return @"INSERT INTO sys_role 
                    (role_code, role_name, description, status, permissions, sort_order, 
                     create_time, create_user_id, create_user_name, is_deleted) 
                    VALUES 
                    (@RoleCode, @RoleName, @Description, @Status, @Permissions, @SortOrder, 
                     @CreateTime, @CreateUserId, @CreateUserName, @IsDeleted)";
        }

        /// <summary>
        /// 获取更新SQL语句
        /// </summary>
        /// <returns>更新SQL</returns>
        protected override string GetUpdateSql()
        {
            return @"UPDATE sys_role SET 
                    role_code = @RoleCode, 
                    role_name = @RoleName, 
                    description = @Description, 
                    status = @Status, 
                    permissions = @Permissions, 
                    sort_order = @SortOrder, 
                    update_time = @UpdateTime, 
                    update_user_id = @UpdateUserId, 
                    update_user_name = @UpdateUserName 
                    WHERE id = @Id";
        }

        /// <summary>
        /// 设置插入参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected override void SetInsertParameters(MySqlCommand cmd, RoleInfo entity)
        {
            cmd.Parameters.AddWithValue("@RoleCode", entity.RoleCode);
            cmd.Parameters.AddWithValue("@RoleName", entity.RoleName);
            cmd.Parameters.AddWithValue("@Description", entity.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@Status", entity.Status);
            cmd.Parameters.AddWithValue("@Permissions", entity.Permissions ?? string.Empty);
            cmd.Parameters.AddWithValue("@SortOrder", entity.SortOrder);
            cmd.Parameters.AddWithValue("@CreateTime", entity.CreateTime);
            cmd.Parameters.AddWithValue("@CreateUserId", entity.CreateUserId);
            cmd.Parameters.AddWithValue("@CreateUserName", entity.CreateUserName ?? string.Empty);
            cmd.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted);
        }

        /// <summary>
        /// 设置更新参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected override void SetUpdateParameters(MySqlCommand cmd, RoleInfo entity)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@RoleCode", entity.RoleCode);
            cmd.Parameters.AddWithValue("@RoleName", entity.RoleName);
            cmd.Parameters.AddWithValue("@Description", entity.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@Status", entity.Status);
            cmd.Parameters.AddWithValue("@Permissions", entity.Permissions ?? string.Empty);
            cmd.Parameters.AddWithValue("@SortOrder", entity.SortOrder);
            cmd.Parameters.AddWithValue("@UpdateTime", entity.UpdateTime);
            cmd.Parameters.AddWithValue("@UpdateUserId", entity.UpdateUserId);
            cmd.Parameters.AddWithValue("@UpdateUserName", entity.UpdateUserName ?? string.Empty);
        }

        /// <summary>
        /// 根据角色编码获取角色信息
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>角色信息</returns>
        public RoleInfo GetByRoleCode(string roleCode)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE role_code = @RoleCode AND is_deleted = 0";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@RoleCode", roleCode);
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            if (dataTable.Rows.Count > 0)
                            {
                                return MapRowToEntity(dataTable.Rows[0]);
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据角色编码获取角色信息失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 根据状态获取角色列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>角色列表</returns>
        public List<RoleInfo> GetByStatus(int status)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE status = @Status AND is_deleted = 0 ORDER BY sort_order, create_time";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            var roles = new List<RoleInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                roles.Add(MapRowToEntity(row));
                            }
                            
                            return roles;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据状态获取角色列表失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 搜索角色
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>角色列表</returns>
        public List<RoleInfo> Search(string keyword)
        {
            try
            {
                string sql = $@"SELECT * FROM {TableName} 
                               WHERE (role_code LIKE @Keyword OR role_name LIKE @Keyword OR description LIKE @Keyword) 
                               AND is_deleted = 0 
                               ORDER BY sort_order, create_time";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            var roles = new List<RoleInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                roles.Add(MapRowToEntity(row));
                            }
                            
                            return roles;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"搜索角色失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 检查角色编码是否存在
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="excludeId">排除的ID</param>
        /// <returns>是否存在</returns>
        public bool IsRoleCodeExists(string roleCode, int excludeId = 0)
        {
            try
            {
                string sql = $"SELECT COUNT(1) FROM {TableName} WHERE role_code = @RoleCode AND is_deleted = 0";
                if (excludeId > 0)
                {
                    sql += " AND id != @ExcludeId";
                }
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@RoleCode", roleCode);
                        if (excludeId > 0)
                        {
                            cmd.Parameters.AddWithValue("@ExcludeId", excludeId);
                        }
                        
                        var count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"检查角色编码是否存在失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 获取所有角色（按排序号排序）
        /// </summary>
        /// <returns>角色列表</returns>
        public override List<RoleInfo> GetAll()
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE is_deleted = 0 ORDER BY sort_order, create_time";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            var roles = new List<RoleInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                roles.Add(MapRowToEntity(row));
                            }
                            
                            return roles;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取所有角色失败：{ex.Message}", ex);
                throw;
            }
        }
    }
}
