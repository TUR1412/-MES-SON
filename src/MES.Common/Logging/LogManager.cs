using System;
using System.IO;
using System.Configuration;
using MES.Common.Configuration;

namespace MES.Common.Logging
{
    /// <summary>
    /// 日志管理器 - 提供统一的日志记录功能
    /// </summary>
    public static class LogManager
    {
        private static readonly object _lockObject = new object();
        private static string _logPath;
        private static LogLevel _logLevel;
        private static bool _isInitialized = false;

        // 🚀 调试输出捕获 - 自动化调试支持
        private static StreamWriter _debugWriter = null;

        /// <summary>
        /// 初始化日志管理器
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            lock (_lockObject)
            {
                if (_isInitialized) return;

                try
                {
                    // 获取日志配置
                    _logPath = ConfigManager.GetAppSetting("LogPath", "Logs");
                    var logLevelStr = ConfigManager.GetAppSetting("LogLevel", "Info");
                    
                    if (!Enum.TryParse(logLevelStr, true, out _logLevel))
                    {
                        _logLevel = LogLevel.Info;
                    }

                    // 确保日志目录存在
                    if (!Path.IsPathRooted(_logPath))
                    {
                        _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logPath);
                    }

                    if (!Directory.Exists(_logPath))
                    {
                        Directory.CreateDirectory(_logPath);
                    }

                    // 🚀 初始化调试输出捕获
                    InitializeDebugCapture();

                    _isInitialized = true;
                    Info("日志管理器初始化完成");
                }
                catch (Exception ex)
                {
                    // 如果日志初始化失败，至少要能记录到事件日志或控制台
                    Console.WriteLine(string.Format("日志管理器初始化失败: {0}", ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        public static void Debug(string message, Exception exception = null)
        {
            WriteLog(LogLevel.Debug, message, exception);
        }

        /// <summary>
        /// 记录一般信息
        /// </summary>
        public static void Info(string message, Exception exception = null)
        {
            WriteLog(LogLevel.Info, message, exception);
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        public static void Warning(string message, Exception exception = null)
        {
            WriteLog(LogLevel.Warning, message, exception);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        public static void Error(string message, Exception exception = null)
        {
            WriteLog(LogLevel.Error, message, exception);
        }

        /// <summary>
        /// 记录致命错误信息
        /// </summary>
        public static void Fatal(string message, Exception exception = null)
        {
            WriteLog(LogLevel.Fatal, message, exception);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        private static void WriteLog(LogLevel level, string message, Exception exception)
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            // 检查日志级别
            if (level < _logLevel) return;

            try
            {
                lock (_lockObject)
                {
                    var logFileName = string.Format("MES_{0:yyyyMMdd}.log", DateTime.Now);
                    var logFilePath = Path.Combine(_logPath, logFileName);

                    var logEntry = FormatLogEntry(level, message, exception);

                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

                    // 🚀 同时输出到调试捕获文件（所有级别）
                    WriteToDebugCapture(logEntry);

                    // 如果是错误或致命错误，同时输出到控制台
                    if (level >= LogLevel.Error)
                    {
                        Console.WriteLine(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                // 日志写入失败时的备用处理
                Console.WriteLine(string.Format("日志写入失败: {0}", ex.Message));
                Console.WriteLine(string.Format("原始日志: [{0}] {1}", level, message));
                if (exception != null)
                {
                    Console.WriteLine(string.Format("异常信息: {0}", exception));
                }
            }
        }

        /// <summary>
        /// 格式化日志条目
        /// </summary>
        private static string FormatLogEntry(LogLevel level, string message, Exception exception)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            
            var logEntry = string.Format("[{0}] [{1}] [Thread-{2}] {3}", timestamp, level.ToString().ToUpper(), threadId, message);

            if (exception != null)
            {
                logEntry += Environment.NewLine + string.Format("异常详情: {0}", exception);
            }

            return logEntry;
        }

        /// <summary>
        /// 🚀 初始化调试输出捕获
        /// </summary>
        private static void InitializeDebugCapture()
        {
            try
            {
                var debugLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug_logmanager.log");

                // 创建独立的LogManager调试输出文件（覆盖模式，避免混乱）
                var fileStream = new FileStream(debugLogPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                _debugWriter = new StreamWriter(fileStream);
                _debugWriter.AutoFlush = true;

                // 写入LogManager启动标记
                _debugWriter.WriteLine("");
                _debugWriter.WriteLine("🚀 LogManager调试捕获已启动！");
                _debugWriter.WriteLine("=".PadRight(80, '='));
            }
            catch (Exception ex)
            {
                // 如果调试捕获初始化失败，不影响正常日志功能
                Console.WriteLine(string.Format("调试捕获初始化失败: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 🚀 写入到调试捕获文件
        /// </summary>
        private static void WriteToDebugCapture(string logEntry)
        {
            try
            {
                if (_debugWriter != null)
                {
                    _debugWriter.WriteLine(logEntry);
                }
            }
            catch (Exception)
            {
                // 调试捕获失败不影响正常日志功能，静默处理
            }
        }

        /// <summary>
        /// 清理过期日志文件
        /// </summary>
        public static void CleanupOldLogs(int keepDays = 30)
        {
            try
            {
                if (!Directory.Exists(_logPath)) return;

                var cutoffDate = DateTime.Now.AddDays(-keepDays);
                var logFiles = Directory.GetFiles(_logPath, "MES_*.log");

                foreach (var logFile in logFiles)
                {
                    var fileInfo = new FileInfo(logFile);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        File.Delete(logFile);
                        Info(string.Format("已删除过期日志文件: {0}", Path.GetFileName(logFile)));
                    }
                }
            }
            catch (Exception ex)
            {
                Error("清理过期日志文件失败", ex);
            }
        }
    }
}
