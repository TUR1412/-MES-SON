using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using MES.Common.Configuration;
using MES.Common.IO;
using MES.Common.Logging;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Utilities.CrashReporting
{
    internal static class CrashReportWriter
    {
        public static string Write(Exception exception, string source, bool isTerminating)
        {
            try
            {
                if (exception == null)
                {
                    return string.Empty;
                }

                var report = BuildReport(exception, source, isTerminating);
                var directory = GetCrashReportDirectory();
                if (string.IsNullOrWhiteSpace(directory))
                {
                    return string.Empty;
                }

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fileName = string.Format("MES_Crash_{0:yyyyMMdd_HHmmss_fff}.txt", report.Timestamp);
                var path = Path.Combine(directory, fileName);
                File.WriteAllText(path, Render(report), Encoding.UTF8);
                return path;
            }
            catch (Exception ex)
            {
                try { LogManager.Error("写入崩溃报告失败", ex); } catch { }
                return string.Empty;
            }
        }

        private static CrashReport BuildReport(Exception exception, string source, bool isTerminating)
        {
            string logPath;
            string logTail = TryGetRecentLogTail(exception, out logPath);

            string environmentInfo = MaskSensitiveText(GetEnvironmentInfo());
            string exceptionText = MaskSensitiveText(exception.ToString());

            return new CrashReport
            {
                Timestamp = DateTime.Now,
                Source = source ?? string.Empty,
                IsTerminating = isTerminating,
                ApplicationName = GetApplicationName(),
                ApplicationVersion = GetApplicationVersion(),
                EnvironmentInfo = environmentInfo,
                RecentLogPath = logPath,
                RecentLogTail = logTail,
                ExceptionText = exceptionText
            };
        }

        private static string TryGetRecentLogTail(Exception exception, out string logPath)
        {
            logPath = string.Empty;
            if (exception == null) return string.Empty;

            try
            {
                logPath = LogManager.GetTodayLogFilePath();
            }
            catch
            {
                logPath = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(logPath)) return string.Empty;

            try
            {
                // 固定行数 + 固定最大字节数，避免崩溃报告过大
                var tail = TextFileTailReader.ReadTailText(logPath, 200, 256 * 1024);
                return MaskSensitiveText(tail);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string MaskSensitiveText(string text)
        {
            try
            {
                return ConnectionStringHelper.MaskSecretsInText(text);
            }
            catch
            {
                return string.IsNullOrEmpty(text) ? string.Empty : text;
            }
        }

        private static string GetCrashReportDirectory()
        {
            try
            {
                var baseDir = LogManager.LogDirectory;
                if (string.IsNullOrWhiteSpace(baseDir))
                {
                    baseDir = AppDomain.CurrentDomain.BaseDirectory;
                }
                return Path.Combine(baseDir, "CrashReports");
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetApplicationName()
        {
            try
            {
                var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                return assembly.GetName().Name ?? "MES";
            }
            catch
            {
                return "MES";
            }
        }

        private static string GetApplicationVersion()
        {
            try
            {
                var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return version != null ? version.ToString() : "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        private static string GetEnvironmentInfo()
        {
            var sb = new StringBuilder();
            try
            {
                sb.AppendLine(string.Format("OS: {0}", Environment.OSVersion));
                sb.AppendLine(string.Format(".NET: {0}", Environment.Version));
                sb.AppendLine(string.Format("Is64BitProcess: {0}", Environment.Is64BitProcess));
                sb.AppendLine(string.Format("Is64BitOS: {0}", Environment.Is64BitOperatingSystem));
                sb.AppendLine(string.Format("MachineName: {0}", Environment.MachineName));
                sb.AppendLine(string.Format("UserName: {0}", Environment.UserName));
                sb.AppendLine(string.Format("CurrentDirectory: {0}", Environment.CurrentDirectory));
                sb.AppendLine(string.Format("BaseDirectory: {0}", AppDomain.CurrentDomain.BaseDirectory));
                sb.AppendLine(string.Format("CommandLine: {0}", Environment.CommandLine));
            }
            catch
            {
                // ignore
            }

            try
            {
                sb.AppendLine(string.Format("Theme: {0}", UIThemeManager.CurrentTheme));
            }
            catch
            {
                // ignore
            }

            try
            {
                sb.AppendLine(string.Format("Process: {0}", Process.GetCurrentProcess().ProcessName));
            }
            catch
            {
                // ignore
            }

            return sb.ToString().Trim();
        }

        private static string Render(CrashReport report)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== MES Crash Report ===");
            sb.AppendLine(string.Format("Timestamp: {0:yyyy-MM-dd HH:mm:ss.fff}", report.Timestamp));
            sb.AppendLine(string.Format("Source: {0}", report.Source));
            sb.AppendLine(string.Format("IsTerminating: {0}", report.IsTerminating));
            sb.AppendLine(string.Format("Application: {0}", report.ApplicationName));
            sb.AppendLine(string.Format("Version: {0}", report.ApplicationVersion));
            sb.AppendLine();
            sb.AppendLine("=== Environment ===");
            sb.AppendLine(report.EnvironmentInfo ?? string.Empty);
            sb.AppendLine();
            sb.AppendLine("=== Recent Log Tail (masked) ===");
            if (!string.IsNullOrWhiteSpace(report.RecentLogPath))
            {
                sb.AppendLine(string.Format("LogFile: {0}", report.RecentLogPath));
            }

            sb.AppendLine(string.IsNullOrWhiteSpace(report.RecentLogTail)
                ? "(empty)"
                : report.RecentLogTail);
            sb.AppendLine();
            sb.AppendLine("=== Exception ===");
            sb.AppendLine(report.ExceptionText ?? string.Empty);
            sb.AppendLine();
            sb.AppendLine("=== End ===");
            return sb.ToString();
        }
    }
}

