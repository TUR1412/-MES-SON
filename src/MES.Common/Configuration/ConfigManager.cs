using System;
using System.Configuration;

namespace MES.Common.Configuration
{
    /// <summary>
    /// 配置管理器 - 提供统一的配置访问接口
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// 数据访问模式
        /// MySql: 使用数据库（默认）
        /// InMemory: 使用内存数据（离线模式，便于在未搭建数据库时跑通业务流程）
        /// </summary>
        public static string DataAccessMode
        {
            get { return GetAppSetting("DataAccessMode", "MySql"); }
        }

        /// <summary>
        /// 是否使用内存数据模式（离线）
        /// </summary>
        public static bool UseInMemoryData
        {
            get
            {
                var mode = DataAccessMode ?? string.Empty;
                return mode.Equals("InMemory", StringComparison.OrdinalIgnoreCase) ||
                       mode.Equals("Memory", StringComparison.OrdinalIgnoreCase) ||
                       mode.Equals("Offline", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// 获取应用程序设置值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static string GetAppSetting(string key, string defaultValue = "")
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取应用程序设置值并转换为指定类型
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换后的配置值</returns>
        public static T GetAppSetting<T>(string key, T defaultValue = default(T))
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrEmpty(value))
                    return defaultValue;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="name">连接字符串名称</param>
        /// <returns>连接字符串</returns>
        public static string GetConnectionString(string name)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[name];
                return connectionString != null ? connectionString.ConnectionString : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取当前环境的数据库连接字符串
        /// </summary>
        /// <returns>当前环境的连接字符串</returns>
        public static string GetCurrentConnectionString()
        {
            var environment = GetAppSetting("Environment", "Development");
            
            switch (environment.ToLower())
            {
                case "development":
                    return GetConnectionString("MESConnectionString");
                case "test":
                    return GetConnectionString("MESTestConnectionString");
                case "production":
                    return GetConnectionString("MESProductionConnectionString");
                default:
                    return GetConnectionString("MESConnectionString");
            }
        }

        /// <summary>
        /// 获取系统标题
        /// </summary>
        public static string SystemTitle
        {
            get { return GetAppSetting("SystemTitle", "MES制造执行系统"); }
        }

        /// <summary>
        /// 获取系统版本
        /// </summary>
        public static string SystemVersion
        {
            get { return GetAppSetting("SystemVersion", "1.0.0"); }
        }

        /// <summary>
        /// 获取公司名称
        /// </summary>
        public static string CompanyName
        {
            get { return GetAppSetting("CompanyName", "您的公司名称"); }
        }

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        public static bool IsDebugMode
        {
            get { return GetAppSetting<bool>("EnableDebugMode", false); }
        }

        /// <summary>
        /// 获取页面大小
        /// </summary>
        public static int PageSize
        {
            get { return GetAppSetting<int>("PageSize", 20); }
        }

        /// <summary>
        /// 获取会话超时时间（分钟）
        /// </summary>
        public static int SessionTimeout
        {
            get { return GetAppSetting<int>("SessionTimeout", 30); }
        }

        /// <summary>
        /// 获取最大登录尝试次数
        /// </summary>
        public static int MaxLoginAttempts
        {
            get { return GetAppSetting<int>("MaxLoginAttempts", 5); }
        }

        /// <summary>
        /// 获取密码最小长度
        /// </summary>
        public static int PasswordMinLength
        {
            get { return GetAppSetting<int>("PasswordMinLength", 6); }
        }

        /// <summary>
        /// 获取最大导出记录数
        /// </summary>
        public static int MaxExportRecords
        {
            get { return GetAppSetting<int>("MaxExportRecords", 10000); }
        }
    }
}
