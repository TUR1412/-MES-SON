using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Core
{
    /// <summary>
    /// 数据库操作助手类
    /// 提供统一的数据库连接管理、参数化查询、事务处理等功能
    ///
    /// 数据库：MySQL 8.0
    /// 使用MySqlConnection、MySqlParameter等MySQL专用API
    /// </summary>
    public static class DatabaseHelper
    {
        #region 连接字符串管理

        private const string AllowPublicKeyRetrievalKey = "AllowPublicKeyRetrieval";

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns>数据库连接字符串</returns>
        private static string GetConnectionString()
        {
            try
            {
                // 从配置文件获取连接字符串
                string connectionString = ConfigurationManager.ConnectionStrings["MESConnectionString"] != null ?
                    ConfigurationManager.ConnectionStrings["MESConnectionString"].ConnectionString : null;

                if (string.IsNullOrEmpty(connectionString))
                {
                    // 默认连接字符串（MySQL）
                    connectionString = "Server=localhost;Database=mes_db;Uid=root;Pwd=Qwe.123;CharSet=utf8mb4;SslMode=none;AllowPublicKeyRetrieval=true;";
                    LogManager.Warning("未找到配置的数据库连接字符串，使用默认MySQL连接字符串");
                }

                // 兼容 MySQL 8 默认认证（caching_sha2_password）+ 非 SSL 连接
                // 典型报错：Retrieval of the RSA public key is not enabled for insecure connections
                connectionString = EnsureAllowPublicKeyRetrieval(connectionString);

                return connectionString;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取数据库连接字符串失败", ex);
                throw new MESException("数据库连接配置错误", ex);
            }
        }

        private static string EnsureAllowPublicKeyRetrieval(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return connectionString;
            }

            if (connectionString.IndexOf(AllowPublicKeyRetrievalKey, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return connectionString;
            }

            // 仅在显式禁用 SSL 时追加（避免无意改变其他安全策略）
            if (connectionString.IndexOf("SslMode=none", StringComparison.OrdinalIgnoreCase) < 0 &&
                connectionString.IndexOf("Ssl Mode=none", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return connectionString;
            }

            if (!connectionString.TrimEnd().EndsWith(";"))
            {
                connectionString += ";";
            }
            connectionString += "AllowPublicKeyRetrieval=true;";
            return connectionString;
        }

        #endregion

        #region 连接管理

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        public static MySqlConnection CreateConnection()
        {
            try
            {
                var connection = new MySqlConnection(GetConnectionString());
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

        /// <summary>
        /// 测试数据库连接并返回详细信息
        /// </summary>
        /// <returns>连接测试结果信息</returns>
        public static string TestConnectionWithDetails()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        var serverVersion = connection.ServerVersion;
                        var database = connection.Database;
                        var connectionString = GetConnectionString();
                        return string.Format("连接成功！\n服务器版本: {0}\n数据库: {1}\n连接字符串: {2}",
                            serverVersion, database, connectionString);
                    }
                    else
                    {
                        return string.Format("连接失败！连接状态: {0}", connection.State);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("数据库连接测试失败", ex);
                var connectionString = GetConnectionString();
                return string.Format("连接失败！错误信息: {0}\n连接字符串: {1}", ex.Message, connectionString);
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
        public static DataTable ExecuteQuery(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        // 设置命令超时时间
                        command.CommandTimeout = 60;

                        // 添加参数
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            LogManager.Info(string.Format("执行查询成功，返回 {0} 行数据", dataTable.Rows.Count));
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("执行查询失败，SQL: {0}", sql), ex);
                throw new MESException("数据库查询执行失败", ex);
            }
        }

        /// <summary>
        /// 执行非查询操作（INSERT、UPDATE、DELETE）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 60;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        int rowsAffected = command.ExecuteNonQuery();
                        LogManager.Info(string.Format("执行非查询操作成功，影响 {0} 行", rowsAffected));
                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("执行非查询操作失败，SQL: {0}", sql), ex);
                throw new MESException("数据库操作执行失败", ex);
            }
        }

        /// <summary>
        /// 执行标量查询，返回单个值
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>查询结果的第一行第一列值</returns>
        public static object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
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
                LogManager.Error(string.Format("执行标量查询失败，SQL: {0}", sql), ex);
                throw new MESException("数据库标量查询执行失败", ex);
            }
        }

        /// <summary>
        /// 创建MySQL参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>MySQL参数</returns>
        public static MySqlParameter CreateParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value ?? DBNull.Value);
        }

        #endregion
    }
}
