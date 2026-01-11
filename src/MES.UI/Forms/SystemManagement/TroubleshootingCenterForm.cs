using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.Common.IO;
using MES.Common.Logging;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.SystemManagement
{
    /// <summary>
    /// 故障排查中心：
    /// - 内置日志查看（尾部读取，避免大文件卡顿）
    /// - 内置崩溃报告（CrashReports）查看与复制
    /// </summary>
    public class TroubleshootingCenterForm : ThemedForm
    {
        private class FileEntry
        {
            public string DisplayName { get; set; }
            public string FullPath { get; set; }
            public DateTime LastWriteTime { get; set; }
            public long LengthBytes { get; set; }

            public override string ToString()
            {
                return DisplayName ?? string.Empty;
            }
        }

        private readonly TabControl _tabs = new TabControl();

        // Logs tab
        private readonly ThemedListBox _logFiles = new ThemedListBox();
        private readonly ThemedRichTextBox _logText = new ThemedRichTextBox();
        private readonly NumericUpDown _logTailLines = new NumericUpDown();
        private readonly ModernButton _logRefresh = new ModernButton();
        private readonly ModernButton _logOpenFolder = new ModernButton();
        private readonly ModernButton _logOpenExternal = new ModernButton();
        private readonly ModernButton _logCopyAll = new ModernButton();
        private readonly ModernButton _logExportBundle = new ModernButton();
        private readonly Label _logMeta = new Label();
        private int _logLoadVersion = 0;

        // CrashReports tab
        private readonly ThemedListBox _crashFiles = new ThemedListBox();
        private readonly ThemedRichTextBox _crashText = new ThemedRichTextBox();
        private readonly NumericUpDown _crashTailLines = new NumericUpDown();
        private readonly ModernButton _crashRefresh = new ModernButton();
        private readonly ModernButton _crashOpenFolder = new ModernButton();
        private readonly ModernButton _crashOpenExternal = new ModernButton();
        private readonly ModernButton _crashCopyAll = new ModernButton();
        private readonly ModernButton _crashExportBundle = new ModernButton();
        private readonly Label _crashMeta = new Label();
        private int _crashLoadVersion = 0;

        public TroubleshootingCenterForm()
        {
            Text = "故障排查中心 / Troubleshooting Center";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            ShowIcon = false;
            Width = 980;
            Height = 680;

            BuildLayout();
            WireEvents();

            UIThemeManager.OnThemeChanged += HandleThemeChanged;
            ApplyLocalTheme();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            RefreshAll();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { UIThemeManager.OnThemeChanged -= HandleThemeChanged; } catch { }
            }

            base.Dispose(disposing);
        }

        private void BuildLayout()
        {
            _tabs.Dock = DockStyle.Fill;
            Controls.Add(_tabs);

            var logsTab = new TabPage("日志 / Logs");
            logsTab.Padding = new Padding(12);
            _tabs.TabPages.Add(logsTab);

            logsTab.Controls.Add(BuildLogsTab());

            var crashTab = new TabPage("崩溃报告 / CrashReports");
            crashTab.Padding = new Padding(12);
            _tabs.TabPages.Add(crashTab);

            crashTab.Controls.Add(BuildCrashTab());
        }

        private Control BuildLogsTab()
        {
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 2;
            root.RowCount = 1;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Left: file list
            var left = new TableLayoutPanel();
            left.Dock = DockStyle.Fill;
            left.ColumnCount = 1;
            left.RowCount = 2;
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var leftTitle = new Label();
            leftTitle.Dock = DockStyle.Fill;
            leftTitle.Text = "日志文件（MES_yyyyMMdd.log）";
            leftTitle.TextAlign = ContentAlignment.MiddleLeft;
            left.Controls.Add(leftTitle, 0, 0);

            _logFiles.Dock = DockStyle.Fill;
            _logFiles.IntegralHeight = false;
            _logFiles.BorderStyle = BorderStyle.FixedSingle;
            left.Controls.Add(_logFiles, 0, 1);

            root.Controls.Add(left, 0, 0);

            // Right: toolbar + viewer
            var right = new TableLayoutPanel();
            right.Dock = DockStyle.Fill;
            right.ColumnCount = 1;
            right.RowCount = 3;
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));

            right.Controls.Add(BuildLogsToolbar(), 0, 0);

            _logText.Dock = DockStyle.Fill;
            _logText.ReadOnly = true;
            _logText.WordWrap = false;
            _logText.ScrollBars = RichTextBoxScrollBars.Vertical;
            right.Controls.Add(_logText, 0, 1);

            _logMeta.Dock = DockStyle.Fill;
            _logMeta.TextAlign = ContentAlignment.MiddleLeft;
            _logMeta.Text = "提示：默认只加载尾部内容；如需完整文件请使用“外部打开”。";
            right.Controls.Add(_logMeta, 0, 2);

            root.Controls.Add(right, 1, 0);

            return root;
        }

        private Control BuildLogsToolbar()
        {
            var bar = new TableLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.ColumnCount = 11;
            bar.RowCount = 1;

            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // refresh
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open folder
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open external
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // copy all
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // export
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));  // label
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // numeric
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));

            _logRefresh.Text = "刷新";
            _logRefresh.Style = ModernButton.ButtonStyle.Secondary;
            _logRefresh.Dock = DockStyle.Fill;
            bar.Controls.Add(_logRefresh, 0, 0);

            _logOpenFolder.Text = "打开目录";
            _logOpenFolder.Style = ModernButton.ButtonStyle.Outline;
            _logOpenFolder.Dock = DockStyle.Fill;
            bar.Controls.Add(_logOpenFolder, 1, 0);

            _logOpenExternal.Text = "外部打开";
            _logOpenExternal.Style = ModernButton.ButtonStyle.Outline;
            _logOpenExternal.Dock = DockStyle.Fill;
            bar.Controls.Add(_logOpenExternal, 2, 0);

            _logCopyAll.Text = "复制";
            _logCopyAll.Style = ModernButton.ButtonStyle.Outline;
            _logCopyAll.Dock = DockStyle.Fill;
            bar.Controls.Add(_logCopyAll, 3, 0);

            _logExportBundle.Text = "导出诊断包";
            _logExportBundle.Style = ModernButton.ButtonStyle.Outline;
            _logExportBundle.Dock = DockStyle.Fill;
            bar.Controls.Add(_logExportBundle, 4, 0);

            var tailLabel = new Label();
            tailLabel.Dock = DockStyle.Fill;
            tailLabel.TextAlign = ContentAlignment.MiddleRight;
            tailLabel.Text = "尾部行数";
            bar.Controls.Add(tailLabel, 5, 0);

            _logTailLines.Dock = DockStyle.Fill;
            _logTailLines.Minimum = 200;
            _logTailLines.Maximum = 20000;
            _logTailLines.Increment = 200;
            _logTailLines.Value = 2000;
            bar.Controls.Add(_logTailLines, 6, 0);

            return bar;
        }

        private Control BuildCrashTab()
        {
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 2;
            root.RowCount = 1;
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Left: file list
            var left = new TableLayoutPanel();
            left.Dock = DockStyle.Fill;
            left.ColumnCount = 1;
            left.RowCount = 2;
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var leftTitle = new Label();
            leftTitle.Dock = DockStyle.Fill;
            leftTitle.Text = "崩溃报告（MES_Crash_*.txt）";
            leftTitle.TextAlign = ContentAlignment.MiddleLeft;
            left.Controls.Add(leftTitle, 0, 0);

            _crashFiles.Dock = DockStyle.Fill;
            _crashFiles.IntegralHeight = false;
            _crashFiles.BorderStyle = BorderStyle.FixedSingle;
            left.Controls.Add(_crashFiles, 0, 1);

            root.Controls.Add(left, 0, 0);

            // Right: toolbar + viewer
            var right = new TableLayoutPanel();
            right.Dock = DockStyle.Fill;
            right.ColumnCount = 1;
            right.RowCount = 3;
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));

            right.Controls.Add(BuildCrashToolbar(), 0, 0);

            _crashText.Dock = DockStyle.Fill;
            _crashText.ReadOnly = true;
            _crashText.WordWrap = false;
            _crashText.ScrollBars = RichTextBoxScrollBars.Vertical;
            right.Controls.Add(_crashText, 0, 1);

            _crashMeta.Dock = DockStyle.Fill;
            _crashMeta.TextAlign = ContentAlignment.MiddleLeft;
            _crashMeta.Text = "提示：崩溃报告默认写入日志目录下的 CrashReports/。";
            right.Controls.Add(_crashMeta, 0, 2);

            root.Controls.Add(right, 1, 0);

            return root;
        }

        private Control BuildCrashToolbar()
        {
            var bar = new TableLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.ColumnCount = 11;
            bar.RowCount = 1;

            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // refresh
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open folder
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open external
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // copy all
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // export
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));  // label
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // numeric
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));

            _crashRefresh.Text = "刷新";
            _crashRefresh.Style = ModernButton.ButtonStyle.Secondary;
            _crashRefresh.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashRefresh, 0, 0);

            _crashOpenFolder.Text = "打开目录";
            _crashOpenFolder.Style = ModernButton.ButtonStyle.Outline;
            _crashOpenFolder.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashOpenFolder, 1, 0);

            _crashOpenExternal.Text = "外部打开";
            _crashOpenExternal.Style = ModernButton.ButtonStyle.Outline;
            _crashOpenExternal.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashOpenExternal, 2, 0);

            _crashCopyAll.Text = "复制";
            _crashCopyAll.Style = ModernButton.ButtonStyle.Outline;
            _crashCopyAll.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashCopyAll, 3, 0);

            _crashExportBundle.Text = "导出诊断包";
            _crashExportBundle.Style = ModernButton.ButtonStyle.Outline;
            _crashExportBundle.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashExportBundle, 4, 0);

            var tailLabel = new Label();
            tailLabel.Dock = DockStyle.Fill;
            tailLabel.TextAlign = ContentAlignment.MiddleRight;
            tailLabel.Text = "尾部行数";
            bar.Controls.Add(tailLabel, 5, 0);

            _crashTailLines.Dock = DockStyle.Fill;
            _crashTailLines.Minimum = 200;
            _crashTailLines.Maximum = 20000;
            _crashTailLines.Increment = 200;
            _crashTailLines.Value = 4000;
            bar.Controls.Add(_crashTailLines, 6, 0);

            return bar;
        }

        private void WireEvents()
        {
            _logRefresh.Click += (s, e) => ReloadLogs();
            _logOpenFolder.Click += (s, e) => OpenFolderSafe(LogManager.LogDirectory);
            _logOpenExternal.Click += (s, e) => OpenSelectedFileExternal(_logFiles);
            _logCopyAll.Click += (s, e) => CopyTextSafe(_logText.Text, "日志内容已复制。");
            _logExportBundle.Click += (s, e) => ExportSupportBundle();
            _logTailLines.ValueChanged += (s, e) => LoadSelectedLog();
            _logFiles.SelectedIndexChanged += (s, e) => LoadSelectedLog();

            _crashRefresh.Click += (s, e) => ReloadCrashReports();
            _crashOpenFolder.Click += (s, e) => OpenFolderSafe(GetCrashReportsDirectory());
            _crashOpenExternal.Click += (s, e) => OpenSelectedFileExternal(_crashFiles);
            _crashCopyAll.Click += (s, e) => CopyTextSafe(_crashText.Text, "崩溃报告已复制。");
            _crashExportBundle.Click += (s, e) => ExportSupportBundle();
            _crashTailLines.ValueChanged += (s, e) => LoadSelectedCrashReport();
            _crashFiles.SelectedIndexChanged += (s, e) => LoadSelectedCrashReport();
        }

        private void HandleThemeChanged()
        {
            try
            {
                if (IsDisposed) return;
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(ApplyLocalTheme));
                    return;
                }

                ApplyLocalTheme();
            }
            catch
            {
                // ignore
            }
        }

        private void ApplyLocalTheme()
        {
            var colors = UIThemeManager.Colors;

            BackColor = colors.Background;
            ForeColor = colors.Text;

            try
            {
                // 让日志/报告更接近“终端/报告”阅读体验
                _logText.Font = DesignTokens.Typography.CreateCodeFont(9f);
                _crashText.Font = DesignTokens.Typography.CreateCodeFont(9f);
            }
            catch
            {
                // ignore
            }

            try { Invalidate(true); } catch { }
        }

        private void RefreshAll()
        {
            ReloadLogs();
            ReloadCrashReports();
        }

        private void ReloadLogs()
        {
            try
            {
                var dir = LogManager.LogDirectory;
                if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                {
                    _logFiles.Items.Clear();
                    _logText.Text = "未找到日志目录。";
                    return;
                }

                var selected = GetSelectedFilePath(_logFiles);

                _logFiles.BeginUpdate();
                try
                {
                    _logFiles.Items.Clear();

                    var info = new DirectoryInfo(dir);
                    var files = info.GetFiles("MES_*.log");
                    Array.Sort(files, (a, b) => b.LastWriteTime.CompareTo(a.LastWriteTime));

                    foreach (var f in files)
                    {
                        _logFiles.Items.Add(new FileEntry
                        {
                            FullPath = f.FullName,
                            LastWriteTime = f.LastWriteTime,
                            LengthBytes = f.Length,
                            DisplayName = string.Format("{0}  ({1:MM-dd HH:mm}, {2} KB)", f.Name, f.LastWriteTime, Math.Max(1, f.Length / 1024))
                        });
                    }
                }
                finally
                {
                    _logFiles.EndUpdate();
                }

                if (!TryReselect(_logFiles, selected))
                {
                    TrySelectDefaultLog(_logFiles);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新日志列表失败", ex);
                _logText.Text = string.Format("刷新日志列表失败：{0}", ex.Message);
            }
        }

        private void ReloadCrashReports()
        {
            try
            {
                var dir = GetCrashReportsDirectory();
                if (string.IsNullOrWhiteSpace(dir))
                {
                    _crashFiles.Items.Clear();
                    _crashText.Text = "未找到崩溃报告目录。";
                    return;
                }

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var selected = GetSelectedFilePath(_crashFiles);

                _crashFiles.BeginUpdate();
                try
                {
                    _crashFiles.Items.Clear();

                    var info = new DirectoryInfo(dir);
                    var files = info.GetFiles("MES_Crash_*.txt");
                    Array.Sort(files, (a, b) => b.LastWriteTime.CompareTo(a.LastWriteTime));

                    foreach (var f in files)
                    {
                        _crashFiles.Items.Add(new FileEntry
                        {
                            FullPath = f.FullName,
                            LastWriteTime = f.LastWriteTime,
                            LengthBytes = f.Length,
                            DisplayName = string.Format("{0}  ({1:MM-dd HH:mm}, {2} KB)", f.Name, f.LastWriteTime, Math.Max(1, f.Length / 1024))
                        });
                    }
                }
                finally
                {
                    _crashFiles.EndUpdate();
                }

                if (!TryReselect(_crashFiles, selected))
                {
                    if (_crashFiles.Items.Count > 0)
                    {
                        _crashFiles.SelectedIndex = 0;
                    }
                    else
                    {
                        _crashText.Text = "暂无崩溃报告（CrashReports 为空）。";
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新崩溃报告列表失败", ex);
                _crashText.Text = string.Format("刷新崩溃报告列表失败：{0}", ex.Message);
            }
        }

        private void LoadSelectedLog()
        {
            var entry = _logFiles.SelectedItem as FileEntry;
            if (entry == null || string.IsNullOrWhiteSpace(entry.FullPath))
            {
                _logText.Text = string.Empty;
                return;
            }

            int tailLines = SafeGetTailLines(_logTailLines, 2000);
            LoadFileTailAsync(entry, _logText, _logMeta, tailLines, "日志");
        }

        private void LoadSelectedCrashReport()
        {
            var entry = _crashFiles.SelectedItem as FileEntry;
            if (entry == null || string.IsNullOrWhiteSpace(entry.FullPath))
            {
                _crashText.Text = string.Empty;
                return;
            }

            int tailLines = SafeGetTailLines(_crashTailLines, 4000);
            LoadFileTailAsync(entry, _crashText, _crashMeta, tailLines, "崩溃报告");
        }

        private void LoadFileTailAsync(FileEntry entry, ThemedRichTextBox viewer, Label metaLabel, int tailLines, string kind)
        {
            if (entry == null || viewer == null) return;

            int maxBytes = EstimateMaxBytes(tailLines);
            int version;
            if (viewer == _logText)
            {
                _logLoadVersion++;
                version = _logLoadVersion;
            }
            else
            {
                _crashLoadVersion++;
                version = _crashLoadVersion;
            }

            try
            {
                viewer.Text = "加载中...";
            }
            catch
            {
                // ignore
            }

            Task.Run(() =>
            {
                string text = TextFileTailReader.ReadTailText(entry.FullPath, tailLines, maxBytes);
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = "(空)";
                }

                string meta = string.Format(
                    "{0}：{1} · {2:yyyy-MM-dd HH:mm:ss} · {3} KB · 显示尾部 {4} 行",
                    kind,
                    Path.GetFileName(entry.FullPath),
                    entry.LastWriteTime,
                    Math.Max(1, entry.LengthBytes / 1024),
                    tailLines);

                return new object[] { text, meta };
            }).ContinueWith(t =>
            {
                try
                {
                    if (IsDisposed) return;

                    bool outdated = false;
                    if (viewer == _logText) outdated = version != _logLoadVersion;
                    else outdated = version != _crashLoadVersion;
                    if (outdated) return;

                    if (t.IsFaulted)
                    {
                        var msg = t.Exception != null ? t.Exception.GetBaseException().Message : "未知错误";
                        BeginInvoke(new Action(() =>
                        {
                            viewer.Text = string.Format("读取失败：{0}", msg);
                        }));
                        return;
                    }

                    var result = t.Result as object[];
                    if (result == null || result.Length < 2) return;

                    BeginInvoke(new Action(() =>
                    {
                        viewer.Text = result[0] as string ?? string.Empty;
                        if (metaLabel != null) metaLabel.Text = result[1] as string ?? string.Empty;
                    }));
                }
                catch
                {
                    // ignore
                }
            });
        }

        private static int EstimateMaxBytes(int tailLines)
        {
            // 粗略估算：每行 256 bytes（含中文/异常堆栈），上限 1MB；下限 256KB
            int bytes = tailLines * 256;
            if (bytes < 256 * 1024) bytes = 256 * 1024;
            if (bytes > 1024 * 1024) bytes = 1024 * 1024;
            return bytes;
        }

        private static int SafeGetTailLines(NumericUpDown control, int fallback)
        {
            try
            {
                if (control == null) return fallback;
                return (int)control.Value;
            }
            catch
            {
                return fallback;
            }
        }

        private static string GetSelectedFilePath(ListBox listBox)
        {
            try
            {
                var entry = listBox != null ? (listBox.SelectedItem as FileEntry) : null;
                return entry != null ? entry.FullPath : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static bool TryReselect(ListBox listBox, string selectedPath)
        {
            if (listBox == null) return false;
            if (string.IsNullOrWhiteSpace(selectedPath)) return false;

            try
            {
                for (int i = 0; i < listBox.Items.Count; i++)
                {
                    var entry = listBox.Items[i] as FileEntry;
                    if (entry != null && string.Equals(entry.FullPath, selectedPath, StringComparison.OrdinalIgnoreCase))
                    {
                        listBox.SelectedIndex = i;
                        return true;
                    }
                }
            }
            catch
            {
                // ignore
            }

            return false;
        }

        private static void TrySelectDefaultLog(ListBox logList)
        {
            if (logList == null) return;

            try
            {
                var todayName = string.Format("MES_{0:yyyyMMdd}.log", DateTime.Now);
                for (int i = 0; i < logList.Items.Count; i++)
                {
                    var entry = logList.Items[i] as FileEntry;
                    if (entry != null && string.Equals(Path.GetFileName(entry.FullPath), todayName, StringComparison.OrdinalIgnoreCase))
                    {
                        logList.SelectedIndex = i;
                        return;
                    }
                }

                if (logList.Items.Count > 0)
                {
                    logList.SelectedIndex = 0;
                }
            }
            catch
            {
                // ignore
            }
        }

        private static void OpenSelectedFileExternal(ListBox listBox)
        {
            try
            {
                var entry = listBox != null ? (listBox.SelectedItem as FileEntry) : null;
                if (entry == null || string.IsNullOrWhiteSpace(entry.FullPath))
                {
                    MessageBox.Show("请先选择文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!File.Exists(entry.FullPath))
                {
                    MessageBox.Show(string.Format("文件不存在：{0}", entry.FullPath), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Process.Start(entry.FullPath);
            }
            catch (Exception ex)
            {
                LogManager.Error("外部打开文件失败", ex);
                MessageBox.Show(string.Format("外部打开失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void OpenFolderSafe(string folderPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    MessageBox.Show("未找到目录路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                LogManager.Error("打开目录失败", ex);
                MessageBox.Show(string.Format("打开目录失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyTextSafe(string text, string okMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    MessageBox.Show("没有可复制的内容。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Clipboard.SetText(text);
                try
                {
                    if (_tabs.SelectedIndex == 0) _logMeta.Text = okMessage;
                    else _crashMeta.Text = okMessage;
                }
                catch
                {
                    // ignore
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("复制到剪贴板失败", ex);
                MessageBox.Show(string.Format("复制失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportSupportBundle()
        {
            try
            {
                var logDir = LogManager.LogDirectory;
                if (string.IsNullOrWhiteSpace(logDir))
                {
                    MessageBox.Show("未找到日志目录，无法导出诊断包。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }

                var bundlesRoot = Path.Combine(logDir, "SupportBundles");
                if (!Directory.Exists(bundlesRoot))
                {
                    Directory.CreateDirectory(bundlesRoot);
                }

                var now = DateTime.Now;
                var bundleName = string.Format("MES_Support_{0:yyyyMMdd_HHmmss}", now);
                var bundleDir = Path.Combine(bundlesRoot, bundleName);
                Directory.CreateDirectory(bundleDir);

                var selectedLog = GetSelectedFilePath(_logFiles);
                var selectedCrash = GetSelectedFilePath(_crashFiles);

                if (string.IsNullOrWhiteSpace(selectedLog))
                {
                    var today = LogManager.GetTodayLogFilePath();
                    if (!string.IsNullOrWhiteSpace(today) && File.Exists(today))
                    {
                        selectedLog = today;
                    }
                }

                if (string.IsNullOrWhiteSpace(selectedCrash) && _crashFiles.Items.Count > 0)
                {
                    try
                    {
                        var first = _crashFiles.Items[0] as FileEntry;
                        if (first != null && !string.IsNullOrWhiteSpace(first.FullPath) && File.Exists(first.FullPath))
                        {
                            selectedCrash = first.FullPath;
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }

                string copiedLog = CopyFileIfExists(selectedLog, bundleDir);
                string copiedCrash = CopyFileIfExists(selectedCrash, bundleDir);

                var logTailLines = SafeGetTailLines(_logTailLines, 2000);
                var crashTailLines = SafeGetTailLines(_crashTailLines, 4000);

                WriteTailIfExists(selectedLog, bundleDir, "log_tail.txt", logTailLines);
                WriteTailIfExists(selectedCrash, bundleDir, "crash_tail.txt", crashTailLines);

                WriteBundleSummary(bundleDir, copiedLog, copiedCrash, logTailLines, crashTailLines);

                try { Process.Start("explorer.exe", bundleDir); } catch { }

                MessageBox.Show(string.Format("诊断包已导出：{0}", bundleDir), "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error("导出诊断包失败", ex);
                MessageBox.Show(string.Format("导出诊断包失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string CopyFileIfExists(string sourcePath, string bundleDir)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourcePath)) return string.Empty;
                if (!File.Exists(sourcePath)) return string.Empty;
                if (string.IsNullOrWhiteSpace(bundleDir)) return string.Empty;

                var name = Path.GetFileName(sourcePath);
                if (string.IsNullOrWhiteSpace(name)) return string.Empty;

                var dest = Path.Combine(bundleDir, name);
                File.Copy(sourcePath, dest, true);
                return dest;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void WriteTailIfExists(string sourcePath, string bundleDir, string fileName, int tailLines)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourcePath)) return;
                if (!File.Exists(sourcePath)) return;
                if (string.IsNullOrWhiteSpace(bundleDir)) return;
                if (string.IsNullOrWhiteSpace(fileName)) return;

                var text = TextFileTailReader.ReadTailText(sourcePath, tailLines);
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = "(空)";
                }

                var path = Path.Combine(bundleDir, fileName);
                File.WriteAllText(path, text, System.Text.Encoding.UTF8);
            }
            catch
            {
                // ignore
            }
        }

        private void WriteBundleSummary(string bundleDir, string copiedLog, string copiedCrash, int logTailLines, int crashTailLines)
        {
            try
            {
                var path = Path.Combine(bundleDir, "bundle_summary.txt");
                var sb = new System.Text.StringBuilder();

                sb.AppendLine("MES Support Bundle");
                sb.AppendLine(string.Format("GeneratedAt: {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                sb.AppendLine();

                sb.AppendLine("Environment");
                sb.AppendLine(string.Format("MachineName: {0}", Environment.MachineName));
                sb.AppendLine(string.Format("OS: {0}", Environment.OSVersion));
                sb.AppendLine(string.Format("Is64BitProcess: {0}", Environment.Is64BitProcess));
                sb.AppendLine(string.Format("CLR: {0}", Environment.Version));
                sb.AppendLine(string.Format("Theme: {0}", UIThemeManager.CurrentTheme));
                sb.AppendLine();

                sb.AppendLine("Paths");
                sb.AppendLine(string.Format("LogDirectory: {0}", LogManager.LogDirectory));
                sb.AppendLine(string.Format("CrashReportsDirectory: {0}", GetCrashReportsDirectory()));
                sb.AppendLine(string.Format("BundleDirectory: {0}", bundleDir));
                sb.AppendLine();

                sb.AppendLine("Files");
                sb.AppendLine(string.Format("CopiedLog: {0}", string.IsNullOrWhiteSpace(copiedLog) ? "(none)" : copiedLog));
                sb.AppendLine(string.Format("CopiedCrashReport: {0}", string.IsNullOrWhiteSpace(copiedCrash) ? "(none)" : copiedCrash));
                sb.AppendLine(string.Format("LogTailLines: {0}", logTailLines));
                sb.AppendLine(string.Format("CrashTailLines: {0}", crashTailLines));
                sb.AppendLine();

                sb.AppendLine("Env Vars (presence only, values not exported)");
                sb.AppendLine(string.Format("MES_CONNECTION_STRING: {0}", string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MES_CONNECTION_STRING")) ? "not set" : "set"));
                sb.AppendLine(string.Format("MES_TEST_CONNECTION_STRING: {0}", string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MES_TEST_CONNECTION_STRING")) ? "not set" : "set"));
                sb.AppendLine(string.Format("MES_PROD_CONNECTION_STRING: {0}", string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MES_PROD_CONNECTION_STRING")) ? "not set" : "set"));

                File.WriteAllText(path, sb.ToString(), System.Text.Encoding.UTF8);
            }
            catch
            {
                // ignore
            }
        }

        private static string GetCrashReportsDirectory()
        {
            try
            {
                var baseDir = LogManager.LogDirectory;
                if (string.IsNullOrWhiteSpace(baseDir)) return string.Empty;
                return Path.Combine(baseDir, "CrashReports");
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

