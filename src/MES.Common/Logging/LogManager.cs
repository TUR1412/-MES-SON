using System;
using System.IO;
using System.Configuration;
using System.Globalization;
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
        private static long _logMaxFileBytes = 0;
        private static int _logMaxFiles = 0;
        private static bool _isInitialized = false;

        /// <summary>
        /// 当前日志目录（会触发延迟初始化）
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }

                return _logPath;
            }
        }

        /// <summary>
        /// 获取指定日期的日志文件路径（会触发延迟初始化）
        /// </summary>
        public static string GetLogFilePath(DateTime date)
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            var logFileName = string.Format("MES_{0:yyyyMMdd}.log", date);
            return Path.Combine(_logPath, logFileName);
        }

        /// <summary>
        /// 获取当天日志文件路径（会触发延迟初始化）
        /// </summary>
        public static string GetTodayLogFilePath()
        {
            return GetLogFilePath(DateTime.Now);
        }

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
                    var maxFileSizeStr = ConfigManager.GetAppSetting("LogMaxFileSize", string.Empty);
                    _logMaxFileBytes = ParseSizeToBytes(maxFileSizeStr, 0);
                    _logMaxFiles = ConfigManager.GetAppSetting<int>("LogMaxFiles", 0);
                    
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

                    _isInitialized = true;
                    Info("日志管理器初始化完成");
                    if (_logMaxFiles > 0) CleanupOldLogsByCount(_logMaxFiles);
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

                    TryRotateActiveLogFileIfNeeded(logFilePath);
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

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

        private static long ParseSizeToBytes(string rawValue, long defaultBytes)
        {
            if (string.IsNullOrWhiteSpace(rawValue)) return defaultBytes;

            try
            {
                var s = rawValue.Trim();
                if (s.Length == 0) return defaultBytes;

                s = s.Replace(" ", string.Empty);
                var upper = s.ToUpperInvariant();

                long multiplier = 1;
                string numericPart = upper;

                if (upper.EndsWith("KB", StringComparison.Ordinal))
                {
                    multiplier = 1024L;
                    numericPart = upper.Substring(0, upper.Length - 2);
                }
                else if (upper.EndsWith("K", StringComparison.Ordinal))
                {
                    multiplier = 1024L;
                    numericPart = upper.Substring(0, upper.Length - 1);
                }
                else if (upper.EndsWith("MB", StringComparison.Ordinal))
                {
                    multiplier = 1024L * 1024L;
                    numericPart = upper.Substring(0, upper.Length - 2);
                }
                else if (upper.EndsWith("M", StringComparison.Ordinal))
                {
                    multiplier = 1024L * 1024L;
                    numericPart = upper.Substring(0, upper.Length - 1);
                }
                else if (upper.EndsWith("GB", StringComparison.Ordinal))
                {
                    multiplier = 1024L * 1024L * 1024L;
                    numericPart = upper.Substring(0, upper.Length - 2);
                }
                else if (upper.EndsWith("G", StringComparison.Ordinal))
                {
                    multiplier = 1024L * 1024L * 1024L;
                    numericPart = upper.Substring(0, upper.Length - 1);
                }
                else if (upper.EndsWith("B", StringComparison.Ordinal))
                {
                    multiplier = 1;
                    numericPart = upper.Substring(0, upper.Length - 1);
                }

                double number;
                if (!double.TryParse(numericPart, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out number))
                {
                    return defaultBytes;
                }

                if (number <= 0) return 0;

                double bytes = number * multiplier;
                if (bytes > long.MaxValue) return long.MaxValue;
                return (long)bytes;
            }
            catch
            {
                return defaultBytes;
            }
        }

        private static void TryRotateActiveLogFileIfNeeded(string activeLogFilePath)
        {
            try
            {
                if (_logMaxFileBytes <= 0) return;
                if (string.IsNullOrWhiteSpace(activeLogFilePath)) return;
                if (!File.Exists(activeLogFilePath)) return;

                var fi = new FileInfo(activeLogFilePath);
                if (!fi.Exists) return;
                if (fi.Length < _logMaxFileBytes) return;

                var directory = fi.DirectoryName;
                if (string.IsNullOrWhiteSpace(directory)) return;

                var baseName = Path.GetFileNameWithoutExtension(activeLogFilePath);
                var ext = Path.GetExtension(activeLogFilePath);
                if (string.IsNullOrWhiteSpace(baseName)) return;
                if (string.IsNullOrWhiteSpace(ext)) ext = ".log";

                for (int i = 1; i <= 999; i++)
                {
                    var rotated = Path.Combine(directory, string.Format("{0}_{1:000}{2}", baseName, i, ext));
                    if (File.Exists(rotated)) continue;

                    try { File.Move(activeLogFilePath, rotated); }
                    catch { }
                    break;
                }
            }
            catch
            {
                // ignore
            }
        }

        private static void CleanupOldLogsByCount(int maxFiles)
        {
            try
            {
                if (maxFiles <= 0) return;
                if (string.IsNullOrWhiteSpace(_logPath)) return;
                if (!Directory.Exists(_logPath)) return;

                var info = new DirectoryInfo(_logPath);
                var files = info.GetFiles("MES_*.log");
                Array.Sort(files, (a, b) => b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc));

                if (files.Length <= maxFiles) return;

                int deleted = 0;
                for (int i = maxFiles; i < files.Length; i++)
                {
                    try
                    {
                        files[i].Delete();
                        deleted++;
                    }
                    catch
                    {
                        // ignore
                    }
                }

                if (deleted > 0)
                {
                    Info(string.Format("已按数量清理过期日志文件: {0} 个（保留最新 {1} 个）", deleted, maxFiles));
                }
            }
            catch (Exception ex)
            {
                Error("按数量清理日志文件失败", ex);
            }
        }

        /// <summary>
        /// 清理过期日志文件
        /// </summary>
        public static void CleanupOldLogs(int keepDays = 30)
        {
            try
            {
                if (!_isInitialized)
                {
                    Initialize();
                }

                if (keepDays <= 0) return;
                if (string.IsNullOrWhiteSpace(_logPath)) return;
                if (!Directory.Exists(_logPath)) return;

                var cutoffUtc = DateTime.UtcNow.AddDays(-keepDays);
                var logFiles = Directory.GetFiles(_logPath, "MES_*.log");
                int deleted = 0;

                foreach (var logFile in logFiles)
                {
                    try
                    {
                        var fileInfo = new FileInfo(logFile);
                        if (fileInfo.LastWriteTimeUtc < cutoffUtc)
                        {
                            File.Delete(logFile);
                            deleted++;
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }

                if (deleted > 0)
                {
                    Info(string.Format("已清理过期日志文件: {0} 个（保留 {1} 天）", deleted, keepDays));
                }
            }
            catch (Exception ex)
            {
                Error("清理过期日志文件失败", ex);
            }
        }
    }
}
