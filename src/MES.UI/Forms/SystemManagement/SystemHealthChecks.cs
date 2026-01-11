using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using MES.Common.Configuration;
using MES.Common.Logging;
using MES.UI.Framework.Themes;
using MySql.Data.MySqlClient;

namespace MES.UI.Forms.SystemManagement
{
    public enum HealthCheckSeverity
    {
        Info,
        Ok,
        Warning,
        Error
    }

    public sealed class HealthCheckResult
    {
        public string Item { get; set; }
        public string Status { get; set; }
        public string Detail { get; set; }
        public HealthCheckSeverity Severity { get; set; }
    }

    public sealed class HealthCheckOptions
    {
        public bool IncludeDatabaseConnectivity { get; set; }
        public int DatabaseConnectionTimeoutSeconds { get; set; }
        public bool IncludeRecentCrashIndicator { get; set; }
    }

    public static class SystemHealthChecks
    {
        public static List<HealthCheckResult> Collect(HealthCheckOptions options)
        {
            if (options == null) options = new HealthCheckOptions();
            if (options.DatabaseConnectionTimeoutSeconds <= 0) options.DatabaseConnectionTimeoutSeconds = 3;

            var list = new List<HealthCheckResult>();

            SafeAddInfo(list, "应用版本", string.Format("{0} ({1})", ConfigManager.SystemTitle, ConfigManager.SystemVersion));
            SafeAddInfo(list, "主题", SafeGetThemeName());

            string logDir = SafeGetLogDirectory(list);
            SafeCheckDirectory(list, "CrashReports 目录", string.IsNullOrWhiteSpace(logDir) ? string.Empty : Path.Combine(logDir, "CrashReports"));
            SafeCheckDirectory(list, "SupportBundles 目录", string.IsNullOrWhiteSpace(logDir) ? string.Empty : Path.Combine(logDir, "SupportBundles"));
            SafeCheckDisk(list, logDir);

            if (options.IncludeRecentCrashIndicator)
            {
                SafeCheckRecentCrash(list, logDir);
            }

            if (options.IncludeDatabaseConnectivity)
            {
                SafeCheckDatabase(list, options.DatabaseConnectionTimeoutSeconds);
            }
            else
            {
                SafeCheckDatabasePresence(list);
            }

            return list;
        }

        public static string RenderText(IEnumerable<HealthCheckResult> results)
        {
            var sb = new StringBuilder();
            sb.AppendLine("MES System Health Check");
            sb.AppendLine(string.Format("GeneratedAt: {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            try { sb.AppendLine(string.Format("Theme: {0}", SafeGetThemeName())); } catch { }
            sb.AppendLine();

            if (results != null)
            {
                foreach (var r in results)
                {
                    if (r == null) continue;
                    sb.AppendLine(string.Format("- {0} [{1}] {2}",
                        r.Item ?? string.Empty,
                        string.IsNullOrWhiteSpace(r.Status) ? " " : r.Status,
                        r.Detail ?? string.Empty));
                }
            }

            return sb.ToString();
        }

        private static void SafeAddInfo(List<HealthCheckResult> list, string item, string detail)
        {
            try
            {
                list.Add(new HealthCheckResult
                {
                    Item = item,
                    Status = "ℹ",
                    Detail = detail ?? string.Empty,
                    Severity = HealthCheckSeverity.Info
                });
            }
            catch
            {
                // ignore
            }
        }

        private static string SafeGetThemeName()
        {
            try
            {
                return UIThemeManager.CurrentTheme.ToString();
            }
            catch
            {
                return "unknown";
            }
        }

        private static string SafeGetLogDirectory(List<HealthCheckResult> list)
        {
            try
            {
                var logDir = LogManager.LogDirectory;
                string error;
                if (TryEnsureDirectoryWritable(logDir, out error))
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = "日志目录可写",
                        Status = "✓",
                        Detail = logDir,
                        Severity = HealthCheckSeverity.Ok
                    });
                }
                else
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = "日志目录可写",
                        Status = "✗",
                        Detail = string.IsNullOrWhiteSpace(error) ? logDir : (logDir + " (" + error + ")"),
                        Severity = HealthCheckSeverity.Error
                    });
                }

                return logDir;
            }
            catch (Exception ex)
            {
                list.Add(new HealthCheckResult
                {
                    Item = "日志目录可写",
                    Status = "✗",
                    Detail = ex.Message,
                    Severity = HealthCheckSeverity.Error
                });
                return string.Empty;
            }
        }

        private static void SafeCheckDirectory(List<HealthCheckResult> list, string name, string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = name,
                        Status = "⚠",
                        Detail = "(empty)",
                        Severity = HealthCheckSeverity.Warning
                    });
                    return;
                }

                string error;
                if (TryEnsureDirectoryWritable(path, out error))
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = name,
                        Status = "✓",
                        Detail = path,
                        Severity = HealthCheckSeverity.Ok
                    });
                }
                else
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = name,
                        Status = "⚠",
                        Detail = string.IsNullOrWhiteSpace(error) ? path : (path + " (" + error + ")"),
                        Severity = HealthCheckSeverity.Warning
                    });
                }
            }
            catch (Exception ex)
            {
                list.Add(new HealthCheckResult
                {
                    Item = name,
                    Status = "⚠",
                    Detail = ex.Message,
                    Severity = HealthCheckSeverity.Warning
                });
            }
        }

        private static void SafeCheckDisk(List<HealthCheckResult> list, string logDir)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(logDir)) return;

                var root = Path.GetPathRoot(logDir);
                if (string.IsNullOrWhiteSpace(root)) return;

                var drive = new DriveInfo(root);
                var freeGb = drive.AvailableFreeSpace / 1024d / 1024d / 1024d;
                var ok = freeGb >= 2;

                list.Add(new HealthCheckResult
                {
                    Item = "磁盘剩余空间",
                    Status = ok ? "✓" : "⚠",
                    Detail = string.Format("{0:0.00} GB（{1}）", freeGb, root),
                    Severity = ok ? HealthCheckSeverity.Ok : HealthCheckSeverity.Warning
                });
            }
            catch (Exception ex)
            {
                list.Add(new HealthCheckResult
                {
                    Item = "磁盘剩余空间",
                    Status = "⚠",
                    Detail = ex.Message,
                    Severity = HealthCheckSeverity.Warning
                });
            }
        }

        private static void SafeCheckRecentCrash(List<HealthCheckResult> list, string logDir)
        {
            try
            {
                var crashDir = string.IsNullOrWhiteSpace(logDir) ? string.Empty : Path.Combine(logDir, "CrashReports");
                if (string.IsNullOrWhiteSpace(crashDir) || !Directory.Exists(crashDir))
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = "最近崩溃报告",
                        Status = "✓",
                        Detail = "CrashReports 目录不存在或为空",
                        Severity = HealthCheckSeverity.Ok
                    });
                    return;
                }

                var info = new DirectoryInfo(crashDir);
                var files = info.GetFiles("MES_Crash_*.txt");
                if (files == null || files.Length == 0)
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = "最近崩溃报告",
                        Status = "✓",
                        Detail = "未检测到崩溃报告（CrashReports 为空）",
                        Severity = HealthCheckSeverity.Ok
                    });
                    return;
                }

                Array.Sort(files, (a, b) => b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc));
                list.Add(new HealthCheckResult
                {
                    Item = "最近崩溃报告",
                    Status = "⚠",
                    Detail = string.Format("{0} ({1:yyyy-MM-dd HH:mm:ss})", files[0].Name, files[0].LastWriteTime),
                    Severity = HealthCheckSeverity.Warning
                });
            }
            catch (Exception ex)
            {
                list.Add(new HealthCheckResult
                {
                    Item = "最近崩溃报告",
                    Status = "⚠",
                    Detail = ex.Message,
                    Severity = HealthCheckSeverity.Warning
                });
            }
        }

        private static void SafeCheckDatabasePresence(List<HealthCheckResult> list)
        {
            try
            {
                var connectionString = ConfigManager.GetCurrentConnectionString();
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = "数据库连接",
                        Status = "⚠",
                        Detail = "未配置连接字符串（推荐使用环境变量 MES_CONNECTION_STRING）",
                        Severity = HealthCheckSeverity.Warning
                    });
                }
                else
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = "数据库连接",
                        Status = "ℹ",
                        Detail = "已配置连接字符串（默认脱敏；可在健康检查中执行联通性测试）",
                        Severity = HealthCheckSeverity.Info
                    });
                }
            }
            catch (Exception ex)
            {
                list.Add(new HealthCheckResult
                {
                    Item = "数据库连接",
                    Status = "⚠",
                    Detail = ex.Message,
                    Severity = HealthCheckSeverity.Warning
                });
            }
        }

        private static void SafeCheckDatabase(List<HealthCheckResult> list, int timeoutSeconds)
        {
            try
            {
                var raw = ConfigManager.GetCurrentConnectionString();
                if (string.IsNullOrWhiteSpace(raw))
                {
                    list.Add(new HealthCheckResult
                    {
                        Item = "数据库连接",
                        Status = "⚠",
                        Detail = "未配置连接字符串（推荐使用环境变量 MES_CONNECTION_STRING）",
                        Severity = HealthCheckSeverity.Warning
                    });
                    return;
                }

                var effective = ConnectionStringHelper.EnsureAllowPublicKeyRetrieval(raw);
                var masked = ConnectionStringHelper.MaskSecrets(effective);

                var builder = new MySqlConnectionStringBuilder(effective);
                try
                {
                    var t = timeoutSeconds <= 0 ? 3 : timeoutSeconds;
                    builder.ConnectionTimeout = (uint)Math.Max(1, t);
                }
                catch
                {
                    // ignore
                }

                using (var connection = new MySqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    list.Add(new HealthCheckResult
                    {
                        Item = "数据库连接",
                        Status = "✓",
                        Detail = string.Format("连接成功：{0} / {1} / {2}", connection.ServerVersion, connection.Database, masked),
                        Severity = HealthCheckSeverity.Ok
                    });
                }
            }
            catch (Exception ex)
            {
                list.Add(new HealthCheckResult
                {
                    Item = "数据库连接",
                    Status = "✗",
                    Detail = ex.Message,
                    Severity = HealthCheckSeverity.Error
                });
            }
        }

        private static bool TryEnsureDirectoryWritable(string directory, out string error)
        {
            error = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(directory))
                {
                    error = "empty directory";
                    return false;
                }

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var probe = Path.Combine(directory, ".mes_health_probe_" + Guid.NewGuid().ToString("N") + ".tmp");
                File.WriteAllText(probe, "ok");
                try { File.Delete(probe); } catch { }

                return true;
            }
            catch (Exception ex)
            {
                try { error = ex.Message; } catch { error = string.Empty; }
                return false;
            }
        }
    }
}

