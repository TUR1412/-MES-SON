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
    /// 数据字典信息数据访问类
    /// 提供数据字典管理的数据库操作功能
    /// </summary>
    public class DictionaryDAL : BaseDAL<DictionaryInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName => "sys_dictionary";

        /// <summary>
        /// 主键字段名
        /// </summary>
        protected override string PrimaryKey => "id";

        /// <summary>
        /// 将DataRow转换为DictionaryInfo对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>字典信息对象</returns>
        protected override DictionaryInfo MapRowToEntity(DataRow row)
        {
            return new DictionaryInfo
            {
                Id = Convert.ToInt32(row["id"]),
                DictType = row["dict_type"]?.ToString() ?? string.Empty,
                DictTypeName = row["dict_type_name"]?.ToString() ?? string.Empty,
                DictCode = row["dict_code"]?.ToString() ?? string.Empty,
                DictName = row["dict_name"]?.ToString() ?? string.Empty,
                DictValue = row["dict_value"]?.ToString() ?? string.Empty,
                ParentId = row["parent_id"] != DBNull.Value ? Convert.ToInt32(row["parent_id"]) : 0,
                SortOrder = row["sort_order"] != DBNull.Value ? Convert.ToInt32(row["sort_order"]) : 0,
                Status = Convert.ToInt32(row["status"]),
                IsSystem = row["is_system"] != DBNull.Value ? Convert.ToInt32(row["is_system"]) : 0,
                Description = row["description"]?.ToString() ?? string.Empty,
                ExtendField1 = row["extend_field1"]?.ToString() ?? string.Empty,
                ExtendField2 = row["extend_field2"]?.ToString() ?? string.Empty,
                ExtendField3 = row["extend_field3"]?.ToString() ?? string.Empty,
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
            return @"INSERT INTO sys_dictionary 
                    (dict_type, dict_type_name, dict_code, dict_name, dict_value, parent_id, sort_order, 
                     status, is_system, description, extend_field1, extend_field2, extend_field3,
                     create_time, create_user_id, create_user_name, is_deleted) 
                    VALUES 
                    (@DictType, @DictTypeName, @DictCode, @DictName, @DictValue, @ParentId, @SortOrder,
                     @Status, @IsSystem, @Description, @ExtendField1, @ExtendField2, @ExtendField3,
                     @CreateTime, @CreateUserId, @CreateUserName, @IsDeleted)";
        }

        /// <summary>
        /// 获取更新SQL语句
        /// </summary>
        /// <returns>更新SQL</returns>
        protected override string GetUpdateSql()
        {
            return @"UPDATE sys_dictionary SET 
                    dict_type = @DictType, 
                    dict_type_name = @DictTypeName, 
                    dict_code = @DictCode, 
                    dict_name = @DictName, 
                    dict_value = @DictValue, 
                    parent_id = @ParentId, 
                    sort_order = @SortOrder,
                    status = @Status,
                    is_system = @IsSystem,
                    description = @Description,
                    extend_field1 = @ExtendField1,
                    extend_field2 = @ExtendField2,
                    extend_field3 = @ExtendField3,
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
        protected override void SetInsertParameters(MySqlCommand cmd, DictionaryInfo entity)
        {
            cmd.Parameters.AddWithValue("@DictType", entity.DictType);
            cmd.Parameters.AddWithValue("@DictTypeName", entity.DictTypeName ?? string.Empty);
            cmd.Parameters.AddWithValue("@DictCode", entity.DictCode);
            cmd.Parameters.AddWithValue("@DictName", entity.DictName);
            cmd.Parameters.AddWithValue("@DictValue", entity.DictValue ?? string.Empty);
            cmd.Parameters.AddWithValue("@ParentId", entity.ParentId);
            cmd.Parameters.AddWithValue("@SortOrder", entity.SortOrder);
            cmd.Parameters.AddWithValue("@Status", entity.Status);
            cmd.Parameters.AddWithValue("@IsSystem", entity.IsSystem);
            cmd.Parameters.AddWithValue("@Description", entity.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@ExtendField1", entity.ExtendField1 ?? string.Empty);
            cmd.Parameters.AddWithValue("@ExtendField2", entity.ExtendField2 ?? string.Empty);
            cmd.Parameters.AddWithValue("@ExtendField3", entity.ExtendField3 ?? string.Empty);
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
        protected override void SetUpdateParameters(MySqlCommand cmd, DictionaryInfo entity)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@DictType", entity.DictType);
            cmd.Parameters.AddWithValue("@DictTypeName", entity.DictTypeName ?? string.Empty);
            cmd.Parameters.AddWithValue("@DictCode", entity.DictCode);
            cmd.Parameters.AddWithValue("@DictName", entity.DictName);
            cmd.Parameters.AddWithValue("@DictValue", entity.DictValue ?? string.Empty);
            cmd.Parameters.AddWithValue("@ParentId", entity.ParentId);
            cmd.Parameters.AddWithValue("@SortOrder", entity.SortOrder);
            cmd.Parameters.AddWithValue("@Status", entity.Status);
            cmd.Parameters.AddWithValue("@IsSystem", entity.IsSystem);
            cmd.Parameters.AddWithValue("@Description", entity.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@ExtendField1", entity.ExtendField1 ?? string.Empty);
            cmd.Parameters.AddWithValue("@ExtendField2", entity.ExtendField2 ?? string.Empty);
            cmd.Parameters.AddWithValue("@ExtendField3", entity.ExtendField3 ?? string.Empty);
            cmd.Parameters.AddWithValue("@UpdateTime", entity.UpdateTime);
            cmd.Parameters.AddWithValue("@UpdateUserId", entity.UpdateUserId);
            cmd.Parameters.AddWithValue("@UpdateUserName", entity.UpdateUserName ?? string.Empty);
        }

        /// <summary>
        /// 根据字典类型获取字典列表
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <returns>字典列表</returns>
        public List<DictionaryInfo> GetByDictType(string dictType)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE dict_type = @DictType AND status = 1 AND is_deleted = 0 ORDER BY sort_order, dict_code";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@DictType", dictType);
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            var dictionaries = new List<DictionaryInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                dictionaries.Add(MapRowToEntity(row));
                            }
                            
                            return dictionaries;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据字典类型获取字典列表失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 根据字典类型和编码获取字典项
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictCode">字典编码</param>
        /// <returns>字典信息</returns>
        public DictionaryInfo GetByTypeAndCode(string dictType, string dictCode)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE dict_type = @DictType AND dict_code = @DictCode AND is_deleted = 0";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@DictType", dictType);
                        cmd.Parameters.AddWithValue("@DictCode", dictCode);
                        
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
                LogManager.Error($"根据字典类型和编码获取字典项失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 根据父级ID获取子字典列表
        /// </summary>
        /// <param name="parentId">父级ID</param>
        /// <returns>字典列表</returns>
        public List<DictionaryInfo> GetByParentId(int parentId)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE parent_id = @ParentId AND status = 1 AND is_deleted = 0 ORDER BY sort_order, dict_code";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ParentId", parentId);
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            var dictionaries = new List<DictionaryInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                dictionaries.Add(MapRowToEntity(row));
                            }
                            
                            return dictionaries;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据父级ID获取子字典列表失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 获取所有字典类型
        /// </summary>
        /// <returns>字典类型列表</returns>
        public List<string> GetAllDictTypes()
        {
            try
            {
                string sql = $"SELECT DISTINCT dict_type FROM {TableName} WHERE is_deleted = 0 ORDER BY dict_type";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            var dictTypes = new List<string>();
                            while (reader.Read())
                            {
                                dictTypes.Add(reader["dict_type"].ToString());
                            }
                            
                            return dictTypes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取所有字典类型失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 搜索字典项
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>字典列表</returns>
        public List<DictionaryInfo> Search(string keyword)
        {
            try
            {
                string sql = $@"SELECT * FROM {TableName} 
                               WHERE (dict_type LIKE @Keyword OR dict_code LIKE @Keyword 
                                      OR dict_name LIKE @Keyword OR dict_value LIKE @Keyword 
                                      OR description LIKE @Keyword) 
                               AND is_deleted = 0 
                               ORDER BY dict_type, sort_order, dict_code";
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            var dictionaries = new List<DictionaryInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                dictionaries.Add(MapRowToEntity(row));
                            }
                            
                            return dictionaries;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"搜索字典项失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 检查字典项是否存在
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictCode">字典编码</param>
        /// <param name="excludeId">排除的ID</param>
        /// <returns>是否存在</returns>
        public bool IsDictExists(string dictType, string dictCode, int excludeId = 0)
        {
            try
            {
                string sql = $"SELECT COUNT(1) FROM {TableName} WHERE dict_type = @DictType AND dict_code = @DictCode AND is_deleted = 0";
                if (excludeId > 0)
                {
                    sql += " AND id != @ExcludeId";
                }
                
                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@DictType", dictType);
                        cmd.Parameters.AddWithValue("@DictCode", dictCode);
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
                LogManager.Error($"检查字典项是否存在失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 批量插入字典项
        /// </summary>
        /// <param name="dictionaries">字典列表</param>
        /// <returns>操作是否成功</returns>
        public bool BatchInsert(List<DictionaryInfo> dictionaries)
        {
            try
            {
                if (dictionaries == null || dictionaries.Count == 0)
                {
                    return true;
                }

                using (var connection = DatabaseHelper.GetConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var dict in dictionaries)
                            {
                                using (var cmd = new MySqlCommand(GetInsertSql(), connection, transaction))
                                {
                                    SetInsertParameters(cmd, dict);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            
                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"批量插入字典项失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictCode">字典编码</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典值</returns>
        public string GetDictValue(string dictType, string dictCode, string defaultValue = "")
        {
            try
            {
                var dict = GetByTypeAndCode(dictType, dictCode);
                if (dict != null && dict.Status == 1)
                {
                    return !string.IsNullOrEmpty(dict.DictValue) ? dict.DictValue : dict.DictName;
                }
                
                return defaultValue;
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取字典值失败：{ex.Message}", ex);
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取字典名称
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictCode">字典编码</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字典名称</returns>
        public string GetDictName(string dictType, string dictCode, string defaultValue = "")
        {
            try
            {
                var dict = GetByTypeAndCode(dictType, dictCode);
                if (dict != null && dict.Status == 1)
                {
                    return dict.DictName;
                }
                
                return defaultValue;
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取字典名称失败：{ex.Message}", ex);
                return defaultValue;
            }
        }
    }
}
