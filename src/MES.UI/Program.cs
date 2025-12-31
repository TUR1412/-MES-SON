using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Common.Configuration;
using MES.UI.Forms;
using MES.UI.Framework.Themes;
using MySql.Data.MySqlClient;

namespace MES.UI
{
    /// <summary>
    /// MES系统主程序入口
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // 启用应用程序视觉样式
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 初始化日志系统
                LogManager.Initialize();
                LogManager.Info("MES系统启动");

                // 设置全局异常处理
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                // 初始化主题（以 App.config 为准：DefaultTheme = Default/Blue/Dark/Lol）
                // 说明：此前为了“主题记忆”曾读取用户级配置，但这会导致 LOL 主题看起来“没生效”。
                // 目前按你的诉求：以 2025-06 终版 LOL 风格为主，避免被用户级配置覆盖。
                try
                {
                    var themeSetting = ConfigManager.GetAppSetting("DefaultTheme", "Lol");
                    UIThemeManager.ThemeType theme;
                    if (!Enum.TryParse(themeSetting, true, out theme))
                    {
                        theme = UIThemeManager.ThemeType.Lol;
                    }
                    UIThemeManager.CurrentTheme = theme;
                }
                catch (Exception ex)
                {
                    LogManager.Error("读取主题配置失败，已回落默认主题", ex);
                    UIThemeManager.CurrentTheme = UIThemeManager.ThemeType.Lol;
                }

                // 启动前数据库预检：避免用户一进业务窗体就被“Unknown database”等异常轰炸
                // 注意：这里不做“静默修复”，只有在检测到缺库时才会询问是否自动初始化。
                EnsureDatabaseReadyOrNotify();

                // 启动主窗体（LoL 客户端风格 V2 / 全新大厅式）
                var startupModule = TryParseStartupModuleFromArgs(Environment.GetCommandLineArgs());
                var mainForm = new MainFormLolV2(startupModule);
                Application.Run(mainForm);

                LogManager.Info("MES系统正常退出");
            }
            catch (Exception ex)
            {
                LogManager.Error("系统启动失败", ex);
                MessageBox.Show(string.Format("系统启动失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogManager.Error("UI线程异常", e.Exception);
            MessageBox.Show(string.Format("系统发生错误：{0}", e.Exception.Message), "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 处理非UI线程异常
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                LogManager.Error("应用程序域异常", ex);
                MessageBox.Show(string.Format("系统发生严重错误：{0}", ex.Message), "严重错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void EnsureDatabaseReadyOrNotify()
        {
            string connectionString = null;

            try
            {
                connectionString = GetEffectiveConnectionString();
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    MessageBox.Show(
                        "未配置数据库连接字符串。\n\n" +
                        "请通过以下方式之一配置：\n" +
                        "1) 设置环境变量：MES_CONNECTION_STRING（推荐，避免在仓库中保存真实密码）\n" +
                        "2) 修改 src/MES.UI/App.config 的 connectionStrings（仅用于本机开发/测试）",
                        "数据库配置缺失",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                }
            }
            catch (MySqlException ex)
            {
                LogManager.Error("数据库预检失败", ex);

                // 1) 缺库：最常见导致“业务不通”的根因
                string databaseName;
                if (IsUnknownDatabase(ex, out databaseName))
                {
                    var scriptPath = TryFindDatabaseInitScriptPath();
                    var scriptHint = string.IsNullOrWhiteSpace(scriptPath) ?
                        "未找到初始化脚本（database/create_mes_database.sql）。" :
                        string.Format("已检测到初始化脚本：{0}", scriptPath);

                    var prompt = string.Format(
                        "检测到数据库不存在：{0}\n\n{1}\n\n是否现在自动创建数据库并导入示例数据？\n（仅在库不存在时执行；会创建表结构并写入示例数据）",
                        databaseName,
                        scriptHint);

                    if (string.IsNullOrWhiteSpace(scriptPath))
                    {
                        MessageBox.Show(
                            prompt + "\n\n你也可以手动：先 CREATE DATABASE，再在该库中执行 create_mes_database.sql。",
                            "数据库未初始化",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    var result = MessageBox.Show(prompt, "数据库未初始化", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result != DialogResult.Yes)
                    {
                        return;
                    }

                    string initError;
                    if (TryInitializeDatabaseFromScript(connectionString, databaseName, scriptPath, out initError))
                    {
                        MessageBox.Show("数据库初始化完成！现在可以正常进入各业务模块。", "初始化成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    MessageBox.Show(
                        string.Format("数据库初始化失败：{0}\n\n建议打开【系统管理 -> 数据库诊断】查看详细原因，或检查连接串/权限。", initError),
                        "初始化失败",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // 2) MySQL 8/9 默认认证 + 非SSL 常见坑
                if (ex.Message != null && ex.Message.IndexOf("Retrieval of the RSA public key", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    MessageBox.Show(
                        "数据库连接失败：MySQL 需要允许在非 SSL 连接下获取 RSA 公钥。\n\n" +
                        "请在连接串中加入：AllowPublicKeyRetrieval=true；或启用 SSL。\n" +
                        "提示：程序会在检测到 SslMode=none 时自动补充该参数；也可直接在环境变量连接串中加入。",
                        "数据库连接配置提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // 3) 其他异常：给出可行动提示
                MessageBox.Show(
                    string.Format(
                        "数据库连接失败：{0}\n\n建议：\n1) 确认 MySQL 服务已启动（例如 MySQL95）\n2) 检查连接字符串配置：环境变量 MES_CONNECTION_STRING（推荐）或 src/MES.UI/App.config 的 MESConnectionString\n3) 打开【系统管理 -> 数据库诊断】进一步排查",
                        ex.Message),
                    "数据库连接失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogManager.Error("数据库预检异常", ex);
                // 预检异常不阻塞启动：避免“因为提示框又提示框”导致无法进入界面
            }
        }

        private static string GetEffectiveConnectionString()
        {
            var connectionString = ConfigManager.GetCurrentConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return string.Empty;
            }

            // 兼容 MySQL 8/9 caching_sha2_password 的 RSA Key 获取限制
            if (connectionString.IndexOf("AllowPublicKeyRetrieval", StringComparison.OrdinalIgnoreCase) < 0 &&
                connectionString.IndexOf("SslMode=none", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (!connectionString.TrimEnd().EndsWith(";"))
                {
                    connectionString += ";";
                }
                connectionString += "AllowPublicKeyRetrieval=true;";
            }

            return connectionString;
        }

        private static bool IsUnknownDatabase(MySqlException ex, out string databaseName)
        {
            databaseName = "mes_db";
            if (ex == null)
            {
                return false;
            }

            // 1049: ER_BAD_DB_ERROR (Unknown database)
            if (ex.Number == 1049)
            {
                databaseName = TryExtractDatabaseName(ex.Message) ?? databaseName;
                return true;
            }

            if (ex.Message != null && ex.Message.IndexOf("Unknown database", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                databaseName = TryExtractDatabaseName(ex.Message) ?? databaseName;
                return true;
            }

            return false;
        }

        private static string TryExtractDatabaseName(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            // 常见格式：Unknown database 'mes_db'
            var token = "Unknown database";
            var index = message.IndexOf(token, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return null;
            }

            var firstQuote = message.IndexOf('\'', index);
            if (firstQuote < 0)
            {
                return null;
            }

            var secondQuote = message.IndexOf('\'', firstQuote + 1);
            if (secondQuote < 0)
            {
                return null;
            }

            var db = message.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
            return string.IsNullOrWhiteSpace(db) ? null : db.Trim();
        }

        private static string TryFindDatabaseInitScriptPath()
        {
            const string relativePath = "database\\create_mes_database.sql";

            // 1) 允许通过 App.config 覆盖
            var configuredPath = ConfigManager.GetAppSetting("DatabaseInitScript", "");

            // 2) 常规：与 exe 同目录发布（若未来做打包）
            var baseDir = AppDomain.CurrentDomain.BaseDirectory ?? "";
            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                // 支持绝对路径/相对路径（相对路径优先按 BaseDirectory 解析）
                if (File.Exists(configuredPath))
                {
                    return configuredPath;
                }

                try
                {
                    var configuredFromBase = Path.Combine(baseDir, configuredPath);
                    if (File.Exists(configuredFromBase))
                    {
                        return configuredFromBase;
                    }
                }
                catch
                {
                    // ignore
                }
            }

            var direct = Path.Combine(baseDir, relativePath);
            if (File.Exists(direct))
            {
                return direct;
            }

            // 3) 开发态：从 bin\Release 向上回溯查找仓库根目录
            try
            {
                var dir = new DirectoryInfo(baseDir);
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    if (!string.IsNullOrWhiteSpace(configuredPath))
                    {
                        var configuredCandidate = Path.Combine(dir.FullName, configuredPath);
                        if (File.Exists(configuredCandidate))
                        {
                            return configuredCandidate;
                        }
                    }

                    var candidate = Path.Combine(dir.FullName, relativePath);
                    if (File.Exists(candidate))
                    {
                        return candidate;
                    }
                    dir = dir.Parent;
                }
            }
            catch
            {
                // ignore
            }

            return null;
        }

        private static string TryParseStartupModuleFromArgs(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    return null;
                }

                for (int i = 0; i < args.Length; i++)
                {
                    var a = args[i];
                    if (string.IsNullOrWhiteSpace(a))
                    {
                        continue;
                    }

                    a = a.Trim();

                    // 支持：--module=material / --open=material / /module:material
                    if (a.StartsWith("--module=", StringComparison.OrdinalIgnoreCase) ||
                        a.StartsWith("--open=", StringComparison.OrdinalIgnoreCase))
                    {
                        var idx = a.IndexOf('=');
                        if (idx > 0 && idx < a.Length - 1)
                        {
                            return a.Substring(idx + 1).Trim();
                        }
                    }

                    if (a.StartsWith("/module:", StringComparison.OrdinalIgnoreCase) ||
                        a.StartsWith("/open:", StringComparison.OrdinalIgnoreCase))
                    {
                        var idx = a.IndexOf(':');
                        if (idx > 0 && idx < a.Length - 1)
                        {
                            return a.Substring(idx + 1).Trim();
                        }
                    }

                    // 支持：--module material / --open material
                    if (string.Equals(a, "--module", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(a, "--open", StringComparison.OrdinalIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            return (args[i + 1] ?? string.Empty).Trim();
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }

            return null;
        }

        private static bool TryInitializeDatabaseFromScript(string connectionString, string databaseName, string scriptPath, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    errorMessage = "连接字符串为空";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(databaseName))
                {
                    databaseName = "mes_db";
                }

                if (string.IsNullOrWhiteSpace(scriptPath) || !File.Exists(scriptPath))
                {
                    errorMessage = "初始化脚本不存在";
                    return false;
                }

                // 连接到一个“必定存在”的库，以便执行 CREATE DATABASE
                var serverBuilder = new MySqlConnectionStringBuilder(connectionString);
                serverBuilder.Database = "information_schema";
                var serverConnectionString = serverBuilder.ConnectionString;

                using (var serverConn = new MySqlConnection(serverConnectionString))
                {
                    serverConn.Open();
                    using (var cmd = serverConn.CreateCommand())
                    {
                        cmd.CommandTimeout = 60;
                        cmd.CommandText = string.Format(
                            "CREATE DATABASE IF NOT EXISTS `{0}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;",
                            databaseName.Replace("`", "``"));
                        cmd.ExecuteNonQuery();
                    }
                }

                // 再连接到目标库执行 Navicat 导出的脚本（包含建表+示例数据）
                var dbBuilder = new MySqlConnectionStringBuilder(connectionString);
                dbBuilder.Database = databaseName;

                using (var dbConn = new MySqlConnection(dbBuilder.ConnectionString))
                {
                    dbConn.Open();

                    var sql = File.ReadAllText(scriptPath, Encoding.UTF8);
                    var script = new MySqlScript(dbConn, sql);
                    script.Execute();
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}



