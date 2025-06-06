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
        protected virtual string PrimaryKeyField
        {
            get { return "id"; }
        }

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
                string sql = string.Format("SELECT * FROM {0} WHERE {1} = @id AND is_deleted = 0", TableName, PrimaryKeyField);
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
                LogManager.Error(string.Format("根据ID获取{0}失败，ID: {1}", typeof(T).Name, id), ex);
                throw new MESException(string.Format("获取{0}数据失败", typeof(T).Name), ex);
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
                string sql = string.Format("SELECT * FROM {0} WHERE is_deleted = 0 ORDER BY {1}", TableName, PrimaryKeyField);
                DataTable dataTable = DatabaseHelper.ExecuteQuery(sql);
                
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取所有{0}失败", typeof(T).Name), ex);
                throw new MESException(string.Format("获取{0}列表失败", typeof(T).Name), ex);
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
                string sql = string.Format("SELECT * FROM {0} WHERE is_deleted = 0", TableName);

                if (!string.IsNullOrEmpty(whereClause))
                {
                    sql += string.Format(" AND ({0})", whereClause);
                }

                sql += string.Format(" ORDER BY {0}", PrimaryKeyField);
                
                DataTable dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据条件查询{0}失败，条件: {1}", typeof(T).Name, whereClause), ex);
                throw new MESException(string.Format("查询{0}数据失败", typeof(T).Name), ex);
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
                    throw new ArgumentNullException("entity");
                }

                // 设置创建时间
                entity.CreateTime = DateTime.Now;
                entity.IsDeleted = false;

                string sql;
                MySqlParameter[] parameters;
                if (!BuildInsertSql(entity, out sql, out parameters))
                {
                    throw new MESException(string.Format("构建{0}插入SQL失败", typeof(T).Name));
                }
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info(string.Format("添加{0}成功", typeof(T).Name));
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加{0}失败", typeof(T).Name), ex);
                throw new MESException(string.Format("添加{0}失败", typeof(T).Name), ex);
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
                    throw new ArgumentNullException("entity");
                }

                // 设置更新时间
                entity.UpdateTime = DateTime.Now;

                string sql;
                MySqlParameter[] parameters;
                if (!BuildUpdateSql(entity, out sql, out parameters))
                {
                    throw new MESException(string.Format("构建{0}更新SQL失败", typeof(T).Name));
                }
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info(string.Format("更新{0}成功，ID: {1}", typeof(T).Name, entity.Id));
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新{0}失败，ID: {1}", typeof(T).Name, entity.Id), ex);
                throw new MESException(string.Format("更新{0}失败", typeof(T).Name), ex);
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
                string sql = string.Format("UPDATE {0} SET is_deleted = 1, update_time = @updateTime WHERE {1} = @id", TableName, PrimaryKeyField);
                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@id", id)
                };
                
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info(string.Format("删除{0}成功，ID: {1}", typeof(T).Name, id));
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除{0}失败，ID: {1}", typeof(T).Name, id), ex);
                throw new MESException(string.Format("删除{0}失败", typeof(T).Name), ex);
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
                string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE is_deleted = 0", TableName);

                if (!string.IsNullOrEmpty(whereClause))
                {
                    sql += string.Format(" AND ({0})", whereClause);
                }
                
                object result = DatabaseHelper.ExecuteScalar(sql, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取{0}记录数失败", typeof(T).Name), ex);
                throw new MESException(string.Format("获取{0}记录数失败", typeof(T).Name), ex);
            }
        }

        #endregion

        #region 抽象方法（子类必须实现）

        /// <summary>
        /// 将DataRow转换为实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>实体对象</returns>
        protected abstract T MapRowToEntity(DataRow row);

        /// <summary>
        /// 构建INSERT SQL语句（子类必须实现）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected virtual bool BuildInsertSql(T entity, out string sql, out MySqlParameter[] parameters)
        {
            try
            {
                sql = GetInsertSql();

                using (var cmd = new MySqlCommand())
                {
                    SetInsertParameters(cmd, entity);
                    parameters = cmd.Parameters.Cast<MySqlParameter>().ToArray();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("构建{0}插入SQL失败", typeof(T).Name), ex);
                sql = null;
                parameters = null;
                return false;
            }
        }

        /// <summary>
        /// 构建UPDATE SQL语句（子类必须实现）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected virtual bool BuildUpdateSql(T entity, out string sql, out MySqlParameter[] parameters)
        {
            try
            {
                sql = GetUpdateSql();

                using (var cmd = new MySqlCommand())
                {
                    SetUpdateParameters(cmd, entity);
                    parameters = cmd.Parameters.Cast<MySqlParameter>().ToArray();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("构建{0}更新SQL失败", typeof(T).Name), ex);
                sql = null;
                parameters = null;
                return false;
            }
        }

        /// <summary>
        /// 获取插入SQL语句（兼容旧模式）
        /// </summary>
        /// <returns>插入SQL</returns>
        protected virtual string GetInsertSql()
        {
            throw new NotImplementedException("子类必须实现GetInsertSql方法或重写BuildInsertSql方法");
        }

        /// <summary>
        /// 获取更新SQL语句（兼容旧模式）
        /// </summary>
        /// <returns>更新SQL</returns>
        protected virtual string GetUpdateSql()
        {
            throw new NotImplementedException("子类必须实现GetUpdateSql方法或重写BuildUpdateSql方法");
        }

        /// <summary>
        /// 设置插入参数（兼容旧模式）
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected virtual void SetInsertParameters(MySqlCommand cmd, T entity)
        {
            throw new NotImplementedException("子类必须实现SetInsertParameters方法或重写BuildInsertSql方法");
        }

        /// <summary>
        /// 设置更新参数（兼容旧模式）
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected virtual void SetUpdateParameters(MySqlCommand cmd, T entity)
        {
            throw new NotImplementedException("子类必须实现SetUpdateParameters方法或重写BuildUpdateSql方法");
        }

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
    }
}
