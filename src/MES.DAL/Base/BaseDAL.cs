using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using MES.Models.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Base
{
    /// <summary>
    /// 数据访问层基类
    /// 提供通用的CRUD操作，所有具体DAL类应继承此基类
    /// </summary>
    /// <typeparam name="T">实体类型，必须继承BaseModel</typeparam>
    public abstract class BaseDAL<T> where T : BaseModel, new()
    {
        #region 抽象属性

        /// <summary>
        /// 表名，由子类实现
        /// </summary>
        protected abstract string TableName { get; }

        /// <summary>
        /// 主键字段名，默认为"id"
        /// </summary>
        protected virtual string PrimaryKeyField => "id";

        #endregion

        #region 基础CRUD操作

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体对象，不存在时返回null</returns>
        public virtual T GetById(int id)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE {PrimaryKeyField} = @id AND is_deleted = 0";
                var parameters = new[] { DatabaseHelper.CreateParameter("@id", id) };
                
                DataTable dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                
                if (dataTable.Rows.Count > 0)
                {
                    return ConvertDataRowToEntity(dataTable.Rows[0]);
                }
                
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据ID获取{typeof(T).Name}失败，ID: {id}", ex);
                throw new MESException($"获取{typeof(T).Name}数据失败", ex);
            }
        }

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public virtual List<T> GetAll()
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE is_deleted = 0 ORDER BY {PrimaryKeyField}";
                DataTable dataTable = DatabaseHelper.ExecuteQuery(sql);
                
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取所有{typeof(T).Name}失败", ex);
                throw new MESException($"获取{typeof(T).Name}列表失败", ex);
            }
        }

        /// <summary>
        /// 根据条件查询实体列表
        /// </summary>
        /// <param name="whereClause">WHERE条件子句（不包含WHERE关键字）</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>实体列表</returns>
        public virtual List<T> GetByCondition(string whereClause, params SqlParameter[] parameters)
        {
            try
            {
                string sql = $"SELECT * FROM {TableName} WHERE is_deleted = 0";
                
                if (!string.IsNullOrEmpty(whereClause))
                {
                    sql += $" AND ({whereClause})";
                }
                
                sql += $" ORDER BY {PrimaryKeyField}";
                
                DataTable dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据条件查询{typeof(T).Name}失败，条件: {whereClause}", ex);
                throw new MESException($"查询{typeof(T).Name}数据失败", ex);
            }
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>是否添加成功</returns>
        public virtual bool Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                // 设置创建时间
                entity.CreateTime = DateTime.Now;
                entity.IsDeleted = false;

                var (sql, parameters) = BuildInsertSql(entity);
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info($"添加{typeof(T).Name}成功");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error($"添加{typeof(T).Name}失败", ex);
                throw new MESException($"添加{typeof(T).Name}失败", ex);
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>是否更新成功</returns>
        public virtual bool Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                // 设置更新时间
                entity.UpdateTime = DateTime.Now;

                var (sql, parameters) = BuildUpdateSql(entity);
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info($"更新{typeof(T).Name}成功，ID: {entity.Id}");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新{typeof(T).Name}失败，ID: {entity.Id}", ex);
                throw new MESException($"更新{typeof(T).Name}失败", ex);
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="id">要删除的实体ID</param>
        /// <returns>是否删除成功</returns>
        public virtual bool Delete(int id)
        {
            try
            {
                string sql = $"UPDATE {TableName} SET is_deleted = 1, update_time = @updateTime WHERE {PrimaryKeyField} = @id";
                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@id", id)
                };
                
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info($"删除{typeof(T).Name}成功，ID: {id}");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error($"删除{typeof(T).Name}失败，ID: {id}", ex);
                throw new MESException($"删除{typeof(T).Name}失败", ex);
            }
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="whereClause">WHERE条件子句（不包含WHERE关键字）</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>记录总数</returns>
        public virtual int GetCount(string whereClause = null, params SqlParameter[] parameters)
        {
            try
            {
                string sql = $"SELECT COUNT(*) FROM {TableName} WHERE is_deleted = 0";
                
                if (!string.IsNullOrEmpty(whereClause))
                {
                    sql += $" AND ({whereClause})";
                }
                
                object result = DatabaseHelper.ExecuteScalar(sql, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取{typeof(T).Name}记录数失败", ex);
                throw new MESException($"获取{typeof(T).Name}记录数失败", ex);
            }
        }

        #endregion

        #region 数据转换方法

        /// <summary>
        /// 将DataRow转换为实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>实体对象</returns>
        protected virtual T ConvertDataRowToEntity(DataRow row)
        {
            try
            {
                T entity = new T();
                Type entityType = typeof(T);
                
                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo property = entityType.GetProperty(ConvertColumnNameToPropertyName(column.ColumnName));
                    
                    if (property != null && property.CanWrite)
                    {
                        object value = row[column.ColumnName];
                        
                        if (value != DBNull.Value)
                        {
                            // 处理类型转换
                            if (property.PropertyType != value.GetType())
                            {
                                value = Convert.ChangeType(value, property.PropertyType);
                            }
                            
                            property.SetValue(entity, value);
                        }
                    }
                }
                
                return entity;
            }
            catch (Exception ex)
            {
                LogManager.Error($"转换DataRow到{typeof(T).Name}失败", ex);
                throw new MESException($"数据转换失败", ex);
            }
        }

        /// <summary>
        /// 将DataTable转换为实体列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>实体列表</returns>
        protected virtual List<T> ConvertDataTableToList(DataTable dataTable)
        {
            List<T> list = new List<T>();
            
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(ConvertDataRowToEntity(row));
            }
            
            return list;
        }

        /// <summary>
        /// 将数据库列名转换为属性名
        /// 默认实现：下划线转驼峰命名
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns>属性名</returns>
        protected virtual string ConvertColumnNameToPropertyName(string columnName)
        {
            // 简单的下划线转驼峰实现
            if (string.IsNullOrEmpty(columnName))
                return columnName;
                
            string[] parts = columnName.Split('_');
            string result = parts[0];
            
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                {
                    result += char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }
            
            // 首字母大写
            if (result.Length > 0)
            {
                result = char.ToUpper(result[0]) + result.Substring(1);
            }
            
            return result;
        }

        #endregion

        #region SQL构建方法

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>SQL语句和参数</returns>
        protected virtual (string sql, SqlParameter[] parameters) BuildInsertSql(T entity)
        {
            // 子类可以重写此方法以自定义INSERT逻辑
            throw new NotImplementedException("子类必须实现BuildInsertSql方法");
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>SQL语句和参数</returns>
        protected virtual (string sql, SqlParameter[] parameters) BuildUpdateSql(T entity)
        {
            // 子类可以重写此方法以自定义UPDATE逻辑
            throw new NotImplementedException("子类必须实现BuildUpdateSql方法");
        }

        #endregion
    }
}
