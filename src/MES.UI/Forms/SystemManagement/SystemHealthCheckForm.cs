using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.BLL.SystemManagement;
using MES.Common.Configuration;
using MES.Common.Logging;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.SystemManagement
{
    /// <summary>
    /// 系统健康检查：
    /// - 日志目录/CrashReports/SupportBundles 可用性
    /// - 磁盘空间
    /// - 数据库连接（默认脱敏）
    /// 设计目标：键盘优先、一键排查、无阻塞刷新。
    /// </summary>
    public class SystemHealthCheckForm : ThemedForm
    {
        private class CheckResult
        {
            public string Item { get; set; }
            public string Status { get; set; }
            public string Detail { get; set; }
            public RowKind Kind { get; set; }
        }

        private enum RowKind
        {
            Info,
            Ok,
            Warning,
            Error
        }

        private readonly Label _title = new Label();
        private readonly Label _meta = new Label();
        private readonly DataGridView _grid = new DataGridView();
        private readonly ModernButton _refresh = new ModernButton();
        private readonly ModernButton _copy = new ModernButton();
        private readonly ModernButton _openLogs = new ModernButton();

        private List<CheckResult> _lastResults = new List<CheckResult>();
        private bool _isRunning;

        public SystemHealthCheckForm()
        {
            InitializeWindow();
            BuildLayout();
            WireEvents();
            ApplyThemeStyles();
        }

        private void InitializeWindow()
        {
            Text = "系统健康检查";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            Width = 920;
            Height = 560;
        }

        private void BuildLayout()
        {
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.Padding = new Padding(16);
            root.ColumnCount = 1;
            root.RowCount = 3;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            Controls.Add(root);

            var header = new Panel();
            header.Dock = DockStyle.Fill;
            root.Controls.Add(header, 0, 0);

            _title.AutoSize = false;
            _title.Dock = DockStyle.Top;
            _title.Height = 30;
            _title.Text = "System Health Check";
            _title.Font = DesignTokens.Typography.CreateTitleFont();
            header.Controls.Add(_title);

            _meta.AutoSize = false;
            _meta.Dock = DockStyle.Bottom;
            _meta.Height = 22;
            _meta.TextAlign = ContentAlignment.MiddleLeft;
            _meta.Font = DesignTokens.Typography.CreateBaseFont();
            header.Controls.Add(_meta);

            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AllowUserToResizeRows = false;
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.EnableHeadersVisualStyles = false;
            _grid.MultiSelect = false;
            _grid.BackgroundColor = Color.White;
            _grid.BorderStyle = BorderStyle.FixedSingle;
            _grid.DefaultCellStyle.Font = DesignTokens.Typography.CreateBaseFont();
            _grid.ColumnHeadersDefaultCellStyle.Font = DesignTokens.Typography.CreateBaseFont();

            _grid.Columns.Clear();
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "检查项",
                Name = "colItem",
                FillWeight = 28
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "状态",
                Name = "colStatus",
                FillWeight = 12
            });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "详情",
                Name = "colDetail",
                FillWeight = 60
            });

            root.Controls.Add(_grid, 0, 1);

            var footer = new FlowLayoutPanel();
            footer.Dock = DockStyle.Fill;
            footer.FlowDirection = FlowDirection.RightToLeft;
            footer.WrapContents = false;
            footer.Padding = new Padding(0, 10, 0, 0);
            root.Controls.Add(footer, 0, 2);

            _refresh.Text = "刷新";
            _refresh.Width = 100;
            footer.Controls.Add(_refresh);

            _copy.Text = "复制摘要";
            _copy.Width = 120;
            footer.Controls.Add(_copy);

            _openLogs.Text = "打开日志目录";
            _openLogs.Width = 140;
            footer.Controls.Add(_openLogs);
        }

        private void WireEvents()
        {
            Load += (s, e) => RunChecksAsync();
            _refresh.Click += (s, e) => RunChecksAsync();
            _copy.Click += (s, e) => CopySummaryToClipboard();
            _openLogs.Click += (s, e) => OpenLogsDirectory();
        }

        private void ApplyThemeStyles()
        {
            try
            {
                var colors = UIThemeManager.Colors;
                if (colors == null) return;

                BackColor = colors.Background;
                _title.ForeColor = colors.Text;
                _meta.ForeColor = colors.TextSecondary;

                _grid.BackgroundColor = colors.Surface;
                _grid.GridColor = colors.Border;
                _grid.ColumnHeadersDefaultCellStyle.BackColor = colors.Surface;
                _grid.ColumnHeadersDefaultCellStyle.ForeColor = colors.Text;
                _grid.DefaultCellStyle.BackColor = colors.Surface;
                _grid.DefaultCellStyle.ForeColor = colors.Text;
                _grid.DefaultCellStyle.SelectionBackColor = colors.Selected;
                _grid.DefaultCellStyle.SelectionForeColor = colors.Text;
            }
            catch
            {
                // ignore
            }
        }

        private async void RunChecksAsync()
        {
            if (_isRunning) return;
            _isRunning = true;

            try
            {
                SetUiRunning(true, "正在检查… / Running checks…");

                var results = await Task.Run(() => CollectChecks());
                if (IsDisposed) return;

                _lastResults = results ?? new List<CheckResult>();
                RenderResults(_lastResults);

                SetUiRunning(false, string.Format(
                    "完成：{0:yyyy-MM-dd HH:mm:ss}（主题：{1}）",
                    DateTime.Now,
                    UIThemeManager.CurrentTheme));
            }
            catch (Exception ex)
            {
                try { LogManager.Error("系统健康检查失败", ex); } catch { }
                SetUiRunning(false, string.Format("检查失败：{0}", ex.Message));
            }
            finally
            {
                _isRunning = false;
            }
        }

        private void SetUiRunning(bool running, string message)
        {
            try
            {
                _refresh.Enabled = !running;
                _copy.Enabled = !running;
                _openLogs.Enabled = !running;
                _meta.Text = message ?? string.Empty;
            }
            catch
            {
                // ignore
            }
        }

        private void RenderResults(List<CheckResult> results)
        {
            try
            {
                _grid.Rows.Clear();
                if (results == null) return;

                var colors = UIThemeManager.Colors;
                foreach (var r in results)
                {
                    if (r == null) continue;

                    var rowIndex = _grid.Rows.Add();
                    var row = _grid.Rows[rowIndex];
                    row.Cells[0].Value = r.Item ?? string.Empty;
                    row.Cells[1].Value = r.Status ?? string.Empty;
                    row.Cells[2].Value = r.Detail ?? string.Empty;

                    if (colors != null)
                    {
                        switch (r.Kind)
                        {
                            case RowKind.Ok:
                                row.DefaultCellStyle.ForeColor = colors.Success;
                                break;
                            case RowKind.Warning:
                                row.DefaultCellStyle.ForeColor = colors.Warning;
                                break;
                            case RowKind.Error:
                                row.DefaultCellStyle.ForeColor = colors.Error;
                                break;
                            default:
                                row.DefaultCellStyle.ForeColor = colors.TextSecondary;
                                break;
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        private static List<CheckResult> CollectChecks()
        {
            var list = new List<CheckResult>();

            try
            {
                list.Add(new CheckResult
                {
                    Item = "应用版本",
                    Status = "ℹ",
                    Detail = string.Format("{0} ({1})", ConfigManager.SystemTitle, ConfigManager.SystemVersion),
                    Kind = RowKind.Info
                });
            }
            catch
            {
                // ignore
            }

            try
            {
                list.Add(new CheckResult
                {
                    Item = "主题",
                    Status = "ℹ",
                    Detail = UIThemeManager.CurrentTheme.ToString(),
                    Kind = RowKind.Info
                });
            }
            catch
            {
                // ignore
            }

            // 日志目录可用性
            string logDir = string.Empty;
            try
            {
                logDir = LogManager.LogDirectory;
                string error;
                if (TryEnsureDirectoryWritable(logDir, out error))
                {
                    list.Add(new CheckResult
                    {
                        Item = "日志目录可写",
                        Status = "✓",
                        Detail = logDir,
                        Kind = RowKind.Ok
                    });
                }
                else
                {
                    list.Add(new CheckResult
                    {
                        Item = "日志目录可写",
                        Status = "✗",
                        Detail = string.IsNullOrWhiteSpace(error) ? logDir : (logDir + " (" + error + ")"),
                        Kind = RowKind.Error
                    });
                }
            }
            catch (Exception ex)
            {
                list.Add(new CheckResult
                {
                    Item = "日志目录可写",
                    Status = "✗",
                    Detail = ex.Message,
                    Kind = RowKind.Error
                });
            }

            // CrashReports / SupportBundles 目录
            try
            {
                var crashDir = string.IsNullOrWhiteSpace(logDir) ? string.Empty : Path.Combine(logDir, "CrashReports");
                string error;
                if (TryEnsureDirectoryWritable(crashDir, out error))
                {
                    list.Add(new CheckResult
                    {
                        Item = "CrashReports 目录",
                        Status = "✓",
                        Detail = crashDir,
                        Kind = RowKind.Ok
                    });
                }
                else
                {
                    list.Add(new CheckResult
                    {
                        Item = "CrashReports 目录",
                        Status = "⚠",
                        Detail = string.IsNullOrWhiteSpace(error) ? crashDir : (crashDir + " (" + error + ")"),
                        Kind = RowKind.Warning
                    });
                }
            }
            catch (Exception ex)
            {
                list.Add(new CheckResult
                {
                    Item = "CrashReports 目录",
                    Status = "⚠",
                    Detail = ex.Message,
                    Kind = RowKind.Warning
                });
            }

            try
            {
                var bundleDir = string.IsNullOrWhiteSpace(logDir) ? string.Empty : Path.Combine(logDir, "SupportBundles");
                string error;
                if (TryEnsureDirectoryWritable(bundleDir, out error))
                {
                    list.Add(new CheckResult
                    {
                        Item = "SupportBundles 目录",
                        Status = "✓",
                        Detail = bundleDir,
                        Kind = RowKind.Ok
                    });
                }
                else
                {
                    list.Add(new CheckResult
                    {
                        Item = "SupportBundles 目录",
                        Status = "⚠",
                        Detail = string.IsNullOrWhiteSpace(error) ? bundleDir : (bundleDir + " (" + error + ")"),
                        Kind = RowKind.Warning
                    });
                }
            }
            catch (Exception ex)
            {
                list.Add(new CheckResult
                {
                    Item = "SupportBundles 目录",
                    Status = "⚠",
                    Detail = ex.Message,
                    Kind = RowKind.Warning
                });
            }

            // 磁盘空间（以日志目录为准）
            try
            {
                if (!string.IsNullOrWhiteSpace(logDir))
                {
                    var root = Path.GetPathRoot(logDir);
                    if (!string.IsNullOrWhiteSpace(root))
                    {
                        var drive = new DriveInfo(root);
                        var freeGb = drive.AvailableFreeSpace / 1024d / 1024d / 1024d;
                        list.Add(new CheckResult
                        {
                            Item = "磁盘剩余空间",
                            Status = freeGb >= 2 ? "✓" : "⚠",
                            Detail = string.Format("{0:0.00} GB（{1}）", freeGb, root),
                            Kind = freeGb >= 2 ? RowKind.Ok : RowKind.Warning
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                list.Add(new CheckResult
                {
                    Item = "磁盘剩余空间",
                    Status = "⚠",
                    Detail = ex.Message,
                    Kind = RowKind.Warning
                });
            }

            // 最近崩溃报告
            try
            {
                var crashDir = string.IsNullOrWhiteSpace(logDir) ? string.Empty : Path.Combine(logDir, "CrashReports");
                if (!string.IsNullOrWhiteSpace(crashDir) && Directory.Exists(crashDir))
                {
                    var info = new DirectoryInfo(crashDir);
                    var files = info.GetFiles("MES_Crash_*.txt");
                    if (files != null && files.Length > 0)
                    {
                        Array.Sort(files, (a, b) => b.LastWriteTimeUtc.CompareTo(a.LastWriteTimeUtc));
                        list.Add(new CheckResult
                        {
                            Item = "最近崩溃报告",
                            Status = "⚠",
                            Detail = string.Format("{0} ({1:yyyy-MM-dd HH:mm:ss})", files[0].Name, files[0].LastWriteTime),
                            Kind = RowKind.Warning
                        });
                    }
                    else
                    {
                        list.Add(new CheckResult
                        {
                            Item = "最近崩溃报告",
                            Status = "✓",
                            Detail = "未检测到崩溃报告（CrashReports 为空）",
                            Kind = RowKind.Ok
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                list.Add(new CheckResult
                {
                    Item = "最近崩溃报告",
                    Status = "⚠",
                    Detail = ex.Message,
                    Kind = RowKind.Warning
                });
            }

            // 数据库连接（默认脱敏）
            try
            {
                var connectionString = ConfigManager.GetCurrentConnectionString();
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    list.Add(new CheckResult
                    {
                        Item = "数据库连接",
                        Status = "⚠",
                        Detail = "未配置连接字符串（推荐使用环境变量 MES_CONNECTION_STRING）",
                        Kind = RowKind.Warning
                    });
                }
                else
                {
                    var bll = new DatabaseDiagnosticBLL();
                    var details = bll.TestConnectionWithDetails(connectionString);
                    var ok = details != null && details.IndexOf("连接成功", StringComparison.OrdinalIgnoreCase) >= 0;
                    list.Add(new CheckResult
                    {
                        Item = "数据库连接",
                        Status = ok ? "✓" : "✗",
                        Detail = string.IsNullOrWhiteSpace(details) ? "(empty)" : details.Replace("\r\n", " ").Replace("\n", " ").Trim(),
                        Kind = ok ? RowKind.Ok : RowKind.Error
                    });
                }
            }
            catch (Exception ex)
            {
                list.Add(new CheckResult
                {
                    Item = "数据库连接",
                    Status = "✗",
                    Detail = ex.Message,
                    Kind = RowKind.Error
                });
            }

            return list;
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

        private void CopySummaryToClipboard()
        {
            try
            {
                if (_lastResults == null || _lastResults.Count == 0)
                {
                    MessageBox.Show("暂无可复制的内容。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var sb = new StringBuilder();
                sb.AppendLine("MES System Health Check");
                sb.AppendLine(string.Format("GeneratedAt: {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                sb.AppendLine(string.Format("Theme: {0}", UIThemeManager.CurrentTheme));
                sb.AppendLine();

                foreach (var r in _lastResults)
                {
                    if (r == null) continue;
                    sb.AppendLine(string.Format("- {0} [{1}] {2}",
                        r.Item ?? string.Empty,
                        r.Status ?? string.Empty,
                        r.Detail ?? string.Empty));
                }

                Clipboard.SetText(sb.ToString());
                _meta.Text = "已复制到剪贴板。";
            }
            catch (Exception ex)
            {
                try { LogManager.Error("复制健康检查摘要失败", ex); } catch { }
                MessageBox.Show(string.Format("复制失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenLogsDirectory()
        {
            try
            {
                var dir = LogManager.LogDirectory;
                if (string.IsNullOrWhiteSpace(dir))
                {
                    MessageBox.Show("未找到日志目录。", "日志", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                try { Process.Start("explorer.exe", dir); } catch { }
            }
            catch (Exception ex)
            {
                try { LogManager.Error("打开日志目录失败", ex); } catch { }
                MessageBox.Show(string.Format("打开日志目录失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
