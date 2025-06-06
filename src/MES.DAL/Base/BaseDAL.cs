using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
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

        /// <summary>
        /// 主键属性名（为了兼容子类实现）
        /// </summary>
        protected abstract string PrimaryKey { get; }

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
                    return MapRowToEntity(dataTable.Rows[0]);
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
        public virtual List<T> GetByCondition(string whereClause, params MySqlParameter[] parameters)
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
        public virtual int GetCount(string whereClause = null, params MySqlParameter[] parameters)
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

        #region 抽象方法（子类必须实现）

        /// <summary>
        /// 获取插入SQL语句
        /// </summary>
        /// <returns>插入SQL</returns>
        protected abstract string GetInsertSql();

        /// <summary>
        /// 获取更新SQL语句
        /// </summary>
        /// <returns>更新SQL</returns>
        protected abstract string GetUpdateSql();

        /// <summary>
        /// 设置插入参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected abstract void SetInsertParameters(MySqlCommand cmd, T entity);

        /// <summary>
        /// 设置更新参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected abstract void SetUpdateParameters(MySqlCommand cmd, T entity);

        /// <summary>
        /// 将DataRow转换为实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>实体对象</returns>
        protected abstract T MapRowToEntity(DataRow row);

        #endregion

        #region 数据转换方法

        /// <summary>
        /// 将DataRow转换为实体对象（通用实现，子类可重写）
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>实体对象</returns>
        protected virtual T ConvertDataRowToEntity(DataRow row)
        {
            // 调用子类实现的MapRowToEntity方法
            return MapRowToEntity(row);
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
                list.Add(MapRowToEntity(row));
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
        protected virtual (string sql, MySqlParameter[] parameters) BuildInsertSql(T entity)
        {
            try
            {
                string sql = GetInsertSql();

                using (var cmd = new MySqlCommand())
                {
                    SetInsertParameters(cmd, entity);
                    return (sql, cmd.Parameters.Cast<MySqlParameter>().ToArray());
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"构建{typeof(T).Name}插入SQL失败", ex);
                throw new MESException($"构建插入SQL失败", ex);
            }
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>SQL语句和参数</returns>
        protected virtual (string sql, MySqlParameter[] parameters) BuildUpdateSql(T entity)
        {
            try
            {
                string sql = GetUpdateSql();

                using (var cmd = new MySqlCommand())
                {
                    SetUpdateParameters(cmd, entity);
                    return (sql, cmd.Parameters.Cast<MySqlParameter>().ToArray());
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"构建{typeof(T).Name}更新SQL失败", ex);
                throw new MESException($"构建更新SQL失败", ex);
            }
        }

        #endregion
    }
}
