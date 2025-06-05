using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Core
{
    /// <summary>
    /// 数据库操作助手类
    /// 提供统一的数据库连接管理、参数化查询、事务处理等功能
    ///
    /// 注意：当前使用SQL Server作为示例实现
    /// 要使用MySQL，请：
    /// 1. 安装MySql.Data NuGet包
    /// 2. 将SqlConnection替换为MySqlConnection
    /// 3. 将SqlParameter替换为MySqlParameter
    /// 4. 修改连接字符串格式
    /// </summary>
    public static class DatabaseHelper
    {
        #region 连接字符串管理

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns>数据库连接字符串</returns>
        private static string GetConnectionString()
        {
            try
            {
                // 从配置文件获取连接字符串
                string connectionString = ConfigurationManager.ConnectionStrings["MESDatabase"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    // 默认连接字符串（SQL Server示例）
                    connectionString = "Server=localhost;Database=mes_db;Integrated Security=true;";
                    LogManager.Warning("未找到配置的数据库连接字符串，使用默认连接字符串");
                }

                return connectionString;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取数据库连接字符串失败", ex);
                throw new MESException("数据库连接配置错误", ex);
            }
        }

        #endregion

        #region 连接管理

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        public static SqlConnection CreateConnection()
        {
            try
            {
                var connection = new SqlConnection(GetConnectionString());
                return connection;
            }
            catch (Exception ex)
            {
                LogManager.Error("创建数据库连接失败", ex);
                throw new MESException("无法创建数据库连接", ex);
            }
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns>连接是否成功</returns>
        public static bool TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    LogManager.Info("数据库连接测试成功");
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("数据库连接测试失败", ex);
                return false;
            }
        }

        #endregion

        #region 参数化查询执行

        /// <summary>
        /// 执行查询，返回DataTable
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>查询结果DataTable</returns>
        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        // 设置命令超时时间
                        command.CommandTimeout = 60;

                        // 添加参数
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (var adapter = new SqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            LogManager.Info($"执行查询成功，返回 {dataTable.Rows.Count} 行数据");
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"执行查询失败，SQL: {sql}", ex);
                throw new MESException("数据库查询执行失败", ex);
            }
        }

        /// <summary>
        /// 执行非查询操作（INSERT、UPDATE、DELETE）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        int rowsAffected = command.ExecuteNonQuery();
                        LogManager.Info($"执行非查询操作成功，影响 {rowsAffected} 行");
                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"执行非查询操作失败，SQL: {sql}", ex);
                throw new MESException("数据库操作执行失败", ex);
            }
        }

        /// <summary>
        /// 执行标量查询，返回单个值
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>查询结果的第一行第一列值</returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        object result = command.ExecuteScalar();
                        LogManager.Info("执行标量查询成功");
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"执行标量查询失败，SQL: {sql}", ex);
                throw new MESException("数据库标量查询执行失败", ex);
            }
        }

        /// <summary>
        /// 创建SQL参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>SQL参数</returns>
        public static SqlParameter CreateParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value ?? DBNull.Value);
        }

        #endregion
    }
}
