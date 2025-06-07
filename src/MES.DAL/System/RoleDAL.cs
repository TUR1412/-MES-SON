using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.DAL.Base;
using MES.DAL.Core;
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
        protected override string TableName
        {
            get { return "sys_role"; }
        }

        /// <summary>
        /// 主键字段名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "id"; }
        }

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
                RoleCode = row["role_code"] != DBNull.Value ? row["role_code"].ToString() : string.Empty,
                RoleName = row["role_name"] != DBNull.Value ? row["role_name"].ToString() : string.Empty,
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : string.Empty,
                Status = Convert.ToInt32(row["status"]),
                Permissions = row["permissions"] != DBNull.Value ? row["permissions"].ToString() : string.Empty,
                SortOrder = Convert.ToInt32(row["sort_order"]),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserId = row["create_user_id"] != DBNull.Value ? Convert.ToInt32(row["create_user_id"]) : 0,
                CreateUserName = row["create_user_name"] != DBNull.Value ? row["create_user_name"].ToString() : string.Empty,
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                UpdateUserId = row["update_user_id"] != DBNull.Value ? Convert.ToInt32(row["update_user_id"]) : 0,
                UpdateUserName = row["update_user_name"] != DBNull.Value ? row["update_user_name"].ToString() : string.Empty,
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">角色实体</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected override bool BuildInsertSql(RoleInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO sys_role
                          (role_code, role_name, description, status, permissions, sort_order,
                           create_time, create_user_id, create_user_name, is_deleted)
                          VALUES
                          (@roleCode, @roleName, @description, @status, @permissions, @sortOrder,
                           @createTime, @createUserId, @createUserName, @isDeleted)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@roleCode", entity.RoleCode),
                DatabaseHelper.CreateParameter("@roleName", entity.RoleName),
                DatabaseHelper.CreateParameter("@description", entity.Description ?? string.Empty),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@permissions", entity.Permissions ?? string.Empty),
                DatabaseHelper.CreateParameter("@sortOrder", entity.SortOrder),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserId", entity.CreateUserId),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName ?? string.Empty),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">角色实体</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected override bool BuildUpdateSql(RoleInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE sys_role SET
                          role_code = @roleCode, role_name = @roleName, description = @description,
                          status = @status, permissions = @permissions, sort_order = @sortOrder,
                          update_time = @updateTime, update_user_id = @updateUserId,
                          update_user_name = @updateUserName
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@roleCode", entity.RoleCode),
                DatabaseHelper.CreateParameter("@roleName", entity.RoleName),
                DatabaseHelper.CreateParameter("@description", entity.Description ?? string.Empty),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@permissions", entity.Permissions ?? string.Empty),
                DatabaseHelper.CreateParameter("@sortOrder", entity.SortOrder),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserId", entity.UpdateUserId),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName ?? string.Empty),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
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
                string sql = string.Format("SELECT * FROM {0} WHERE role_code = @RoleCode AND is_deleted = 0", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
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
                LogManager.Error(string.Format("根据角色编码获取角色信息失败：{0}", ex.Message), ex);
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
                string sql = string.Format("SELECT * FROM {0} WHERE status = @Status AND is_deleted = 0 ORDER BY sort_order, create_time", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
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
                LogManager.Error(string.Format("根据状态获取角色列表失败：{0}", ex.Message), ex);
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
                string sql = string.Format(@"SELECT * FROM {0}
                               WHERE (role_code LIKE @Keyword OR role_name LIKE @Keyword OR description LIKE @Keyword)
                               AND is_deleted = 0
                               ORDER BY sort_order, create_time", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", string.Format("%{0}%", keyword));

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
                LogManager.Error(string.Format("搜索角色失败：{0}", ex.Message), ex);
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
                string sql = string.Format("SELECT COUNT(1) FROM {0} WHERE role_code = @RoleCode AND is_deleted = 0", TableName);
                if (excludeId > 0)
                {
                    sql += " AND id != @ExcludeId";
                }
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
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
                LogManager.Error(string.Format("检查角色编码是否存在失败：{0}", ex.Message), ex);
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
                string sql = string.Format("SELECT * FROM {0} WHERE is_deleted = 0 ORDER BY sort_order, create_time", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
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
                LogManager.Error(string.Format("获取所有角色失败：{0}", ex.Message), ex);
                throw;
            }
        }
    }
}
