using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.Common.Configuration;
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
        private const int AutoRefreshIntervalMs = 2500;
        private const int HighlightMaxMatchesPerToken = 200;
        private const string LogTitleBase = "日志文件（MES_yyyyMMdd.log）";     
        private const string CrashTitleBase = "崩溃报告（MES_Crash_*.txt）";    

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

        private readonly Timer _autoRefreshTimer = new Timer();
        private readonly ThemedTabControl _tabs = new ThemedTabControl();

        // Logs tab
        private readonly Label _logListTitle = new Label();
        private readonly ThemedTextBox _logFilter = new ThemedTextBox();        
        private readonly ThemedListBox _logFiles = new ThemedListBox();
        private readonly ThemedRichTextBox _logText = new ThemedRichTextBox();  
        private readonly ThemedTextBox _logSearch = new ThemedTextBox();
        private readonly ModernButton _logSearchPrev = new ModernButton();
        private readonly ModernButton _logSearchNext = new ModernButton();
        private readonly ModernButton _logSearchCase = new ModernButton();
        private readonly ModernButton _logJumpLatestIssue = new ModernButton();
        private readonly Label _logSearchCount = new Label();
        private readonly NumericUpDown _logTailLines = new NumericUpDown();     
        private readonly ModernButton _logRefresh = new ModernButton();
        private readonly ModernButton _logOpenFolder = new ModernButton();      
        private readonly ModernButton _logOpenExternal = new ModernButton();    
        private readonly ModernButton _logCopyAll = new ModernButton();
        private readonly ModernButton _logExportBundle = new ModernButton();    
        private readonly ModernButton _logFollowTail = new ModernButton();
        private readonly ModernButton _logHighlight = new ModernButton();
        private readonly Label _logMeta = new Label();
        private readonly List<FileEntry> _logAllEntries = new List<FileEntry>();
        private int _logLoadVersion = 0;
        private bool _logFollowTailEnabled = false;
        private bool _logHighlightEnabled = true;
        private bool _logSearchCaseSensitive = false;
        private int _logSearchCountVersion = 0;
        private string _logLastLoadedPath = string.Empty;
        private long _logLastLoadedWriteTicks = 0;
        private long _logLastLoadedLengthBytes = -1;
        private int _logLastLoadedTailLines = 0;

        // CrashReports tab
        private readonly Label _crashListTitle = new Label();
        private readonly ThemedTextBox _crashFilter = new ThemedTextBox();      
        private readonly ThemedListBox _crashFiles = new ThemedListBox();       
        private readonly ThemedRichTextBox _crashText = new ThemedRichTextBox();
        private readonly ThemedTextBox _crashSearch = new ThemedTextBox();
        private readonly ModernButton _crashSearchPrev = new ModernButton();
        private readonly ModernButton _crashSearchNext = new ModernButton();
        private readonly ModernButton _crashSearchCase = new ModernButton();
        private readonly ModernButton _crashJumpLatestIssue = new ModernButton();
        private readonly Label _crashSearchCount = new Label();
        private readonly NumericUpDown _crashTailLines = new NumericUpDown();   
        private readonly ModernButton _crashRefresh = new ModernButton();       
        private readonly ModernButton _crashOpenFolder = new ModernButton();    
        private readonly ModernButton _crashOpenExternal = new ModernButton();  
        private readonly ModernButton _crashCopyAll = new ModernButton();       
        private readonly ModernButton _crashExportBundle = new ModernButton();  
        private readonly ModernButton _crashFollowTail = new ModernButton();
        private readonly ModernButton _crashHighlight = new ModernButton();
        private readonly Label _crashMeta = new Label();
        private readonly List<FileEntry> _crashAllEntries = new List<FileEntry>();
        private int _crashLoadVersion = 0;
        private bool _crashFollowTailEnabled = false;
        private bool _crashHighlightEnabled = true;
        private bool _crashSearchCaseSensitive = false;
        private int _crashSearchCountVersion = 0;
        private string _crashLastLoadedPath = string.Empty;
        private long _crashLastLoadedWriteTicks = 0;
        private long _crashLastLoadedLengthBytes = -1;
        private int _crashLastLoadedTailLines = 0;

        public TroubleshootingCenterForm()
        {
            Text = "故障排查中心 / Troubleshooting Center";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimizeBox = false;
            ShowIcon = false;
            Width = 980;
            Height = 680;
            KeyPreview = true;

            BuildLayout();
            WireEvents();
            ConfigureAutoRefresh();
            UpdateToggleButtons();
            UpdateLogSearchCountAsync();
            UpdateCrashSearchCountAsync();

            UIThemeManager.OnThemeChanged += HandleThemeChanged;
            ApplyLocalTheme();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            RefreshAll();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == (Keys.Control | Keys.F))
                {
                    FocusSearchForActiveTab();
                    return true;
                }

                if (keyData == Keys.F3)
                {
                    if (_tabs.SelectedIndex == 0) FindLogNext();
                    else if (_tabs.SelectedIndex == 1) FindCrashNext();
                    return true;
                }

                if (keyData == (Keys.Shift | Keys.F3))
                {
                    if (_tabs.SelectedIndex == 0) FindLogPrevious();
                    else if (_tabs.SelectedIndex == 1) FindCrashPrevious();
                    return true;
                }

                if (keyData == (Keys.Control | Keys.G))
                {
                    if (_tabs.SelectedIndex == 0) JumpLogLatestIssue();
                    else if (_tabs.SelectedIndex == 1) JumpCrashLatestIssue();
                    return true;
                }
            }
            catch
            {
                // ignore
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FocusSearchForActiveTab()
        {
            try
            {
                if (_tabs.SelectedIndex == 0)
                {
                    _logSearch.Focus();
                    _logSearch.SelectAll();
                }
                else if (_tabs.SelectedIndex == 1)
                {
                    _crashSearch.Focus();
                    _crashSearch.SelectAll();
                }
            }
            catch
            {
                // ignore
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { UIThemeManager.OnThemeChanged -= HandleThemeChanged; } catch { }
                try
                {
                    _autoRefreshTimer.Stop();
                    _autoRefreshTimer.Dispose();
                }
                catch
                {
                    // ignore
                }
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
            left.RowCount = 3;
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            _logListTitle.Dock = DockStyle.Fill;
            _logListTitle.Text = LogTitleBase;
            _logListTitle.TextAlign = ContentAlignment.MiddleLeft;
            left.Controls.Add(_logListTitle, 0, 0);

            _logFilter.Dock = DockStyle.Fill;
            _logFilter.Margin = new Padding(0, 4, 0, 4);
            _logFilter.PlaceholderText = "筛选 / Filter...";
            left.Controls.Add(_logFilter, 0, 1);

            _logFiles.Dock = DockStyle.Fill;
            _logFiles.IntegralHeight = false;
            _logFiles.BorderStyle = BorderStyle.FixedSingle;
            left.Controls.Add(_logFiles, 0, 2);

            root.Controls.Add(left, 0, 0);

            // Right: toolbar + viewer
            var right = new TableLayoutPanel();
            right.Dock = DockStyle.Fill;
            right.ColumnCount = 1;
            right.RowCount = 4;
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));

            right.Controls.Add(BuildLogsToolbar(), 0, 0);
            right.Controls.Add(BuildLogSearchBar(), 0, 1);

            _logText.Dock = DockStyle.Fill;
            _logText.ReadOnly = true;
            _logText.WordWrap = false;
            _logText.ScrollBars = RichTextBoxScrollBars.Vertical;
            right.Controls.Add(_logText, 0, 2);

            _logMeta.Dock = DockStyle.Fill;
            _logMeta.TextAlign = ContentAlignment.MiddleLeft;
            _logMeta.Text = "提示：默认只加载尾部内容；如需完整文件请使用“外部打开”。";
            right.Controls.Add(_logMeta, 0, 3);

            root.Controls.Add(right, 1, 0);

            return root;
        }

        private Control BuildLogsToolbar()
        {
            var bar = new TableLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.ColumnCount = 10;
            bar.RowCount = 1;

            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // refresh
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open folder
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open external
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // copy all
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // export
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); // follow tail
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // highlight
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));  // label
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // numeric
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

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

            _logCopyAll.Text = "复制尾部";
            _logCopyAll.Style = ModernButton.ButtonStyle.Outline;
            _logCopyAll.Dock = DockStyle.Fill;
            bar.Controls.Add(_logCopyAll, 3, 0);

            _logExportBundle.Text = "导出诊断包";
            _logExportBundle.Style = ModernButton.ButtonStyle.Outline;
            _logExportBundle.Dock = DockStyle.Fill;
            bar.Controls.Add(_logExportBundle, 4, 0);

            _logFollowTail.Text = "跟随: 关";
            _logFollowTail.Style = ModernButton.ButtonStyle.Outline;
            _logFollowTail.Dock = DockStyle.Fill;
            bar.Controls.Add(_logFollowTail, 5, 0);

            _logHighlight.Text = "高亮: 开";
            _logHighlight.Style = ModernButton.ButtonStyle.Secondary;
            _logHighlight.Dock = DockStyle.Fill;
            bar.Controls.Add(_logHighlight, 6, 0);

            var tailLabel = new Label();
            tailLabel.Dock = DockStyle.Fill;
            tailLabel.TextAlign = ContentAlignment.MiddleRight;
            tailLabel.Text = "尾部行数";
            bar.Controls.Add(tailLabel, 7, 0);

            _logTailLines.Dock = DockStyle.Fill;
            _logTailLines.Minimum = 200;
            _logTailLines.Maximum = 20000;
            _logTailLines.Increment = 200;
            _logTailLines.Value = 2000;
            bar.Controls.Add(_logTailLines, 8, 0);

            return bar;
        }

        private Control BuildLogSearchBar()
        {
            var bar = new TableLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.ColumnCount = 6;
            bar.RowCount = 1;

            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74)); // prev
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74)); // next
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70)); // case
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90)); // latest
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90)); // count

            _logSearch.Dock = DockStyle.Fill;
            _logSearch.Margin = new Padding(0, 4, 6, 4);
            _logSearch.PlaceholderText = "搜索文本 / Search...";
            bar.Controls.Add(_logSearch, 0, 0);

            _logSearchPrev.Text = "上一个";
            _logSearchPrev.Style = ModernButton.ButtonStyle.Outline;
            _logSearchPrev.Dock = DockStyle.Fill;
            bar.Controls.Add(_logSearchPrev, 1, 0);

            _logSearchNext.Text = "下一个";
            _logSearchNext.Style = ModernButton.ButtonStyle.Outline;
            _logSearchNext.Dock = DockStyle.Fill;
            bar.Controls.Add(_logSearchNext, 2, 0);

            _logSearchCase.Text = "Aa: 关";
            _logSearchCase.Style = ModernButton.ButtonStyle.Outline;
            _logSearchCase.Dock = DockStyle.Fill;
            bar.Controls.Add(_logSearchCase, 3, 0);

            _logJumpLatestIssue.Text = "最新错误";
            _logJumpLatestIssue.Style = ModernButton.ButtonStyle.Outline;
            _logJumpLatestIssue.Dock = DockStyle.Fill;
            bar.Controls.Add(_logJumpLatestIssue, 4, 0);

            _logSearchCount.Dock = DockStyle.Fill;
            _logSearchCount.TextAlign = ContentAlignment.MiddleRight;
            _logSearchCount.Text = string.Empty;
            bar.Controls.Add(_logSearchCount, 5, 0);

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
            left.RowCount = 3;
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            _crashListTitle.Dock = DockStyle.Fill;
            _crashListTitle.Text = CrashTitleBase;
            _crashListTitle.TextAlign = ContentAlignment.MiddleLeft;
            left.Controls.Add(_crashListTitle, 0, 0);

            _crashFilter.Dock = DockStyle.Fill;
            _crashFilter.Margin = new Padding(0, 4, 0, 4);
            _crashFilter.PlaceholderText = "筛选 / Filter...";
            left.Controls.Add(_crashFilter, 0, 1);

            _crashFiles.Dock = DockStyle.Fill;
            _crashFiles.IntegralHeight = false;
            _crashFiles.BorderStyle = BorderStyle.FixedSingle;
            left.Controls.Add(_crashFiles, 0, 2);

            root.Controls.Add(left, 0, 0);

            // Right: toolbar + viewer
            var right = new TableLayoutPanel();
            right.Dock = DockStyle.Fill;
            right.ColumnCount = 1;
            right.RowCount = 4;
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));

            right.Controls.Add(BuildCrashToolbar(), 0, 0);
            right.Controls.Add(BuildCrashSearchBar(), 0, 1);

            _crashText.Dock = DockStyle.Fill;
            _crashText.ReadOnly = true;
            _crashText.WordWrap = false;
            _crashText.ScrollBars = RichTextBoxScrollBars.Vertical;
            right.Controls.Add(_crashText, 0, 2);

            _crashMeta.Dock = DockStyle.Fill;
            _crashMeta.TextAlign = ContentAlignment.MiddleLeft;
            _crashMeta.Text = "提示：崩溃报告默认写入日志目录下的 CrashReports/。";
            right.Controls.Add(_crashMeta, 0, 3);

            root.Controls.Add(right, 1, 0);

            return root;
        }

        private Control BuildCrashToolbar()
        {
            var bar = new TableLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.ColumnCount = 10;
            bar.RowCount = 1;

            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // refresh
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open folder
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // open external
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // copy all
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // export
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); // follow tail
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // highlight
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));  // label
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));  // numeric
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

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

            _crashCopyAll.Text = "复制尾部";
            _crashCopyAll.Style = ModernButton.ButtonStyle.Outline;
            _crashCopyAll.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashCopyAll, 3, 0);

            _crashExportBundle.Text = "导出诊断包";
            _crashExportBundle.Style = ModernButton.ButtonStyle.Outline;        
            _crashExportBundle.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashExportBundle, 4, 0);

            _crashFollowTail.Text = "跟随: 关";
            _crashFollowTail.Style = ModernButton.ButtonStyle.Outline;
            _crashFollowTail.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashFollowTail, 5, 0);

            _crashHighlight.Text = "高亮: 开";
            _crashHighlight.Style = ModernButton.ButtonStyle.Secondary;
            _crashHighlight.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashHighlight, 6, 0);

            var tailLabel = new Label();
            tailLabel.Dock = DockStyle.Fill;
            tailLabel.TextAlign = ContentAlignment.MiddleRight;
            tailLabel.Text = "尾部行数";
            bar.Controls.Add(tailLabel, 7, 0);

            _crashTailLines.Dock = DockStyle.Fill;
            _crashTailLines.Minimum = 200;
            _crashTailLines.Maximum = 20000;
            _crashTailLines.Increment = 200;
            _crashTailLines.Value = 4000;
            bar.Controls.Add(_crashTailLines, 8, 0);

            return bar;
        }

        private Control BuildCrashSearchBar()
        {
            var bar = new TableLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.ColumnCount = 6;
            bar.RowCount = 1;

            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74)); // prev
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74)); // next
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70)); // case
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90)); // latest
            bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90)); // count

            _crashSearch.Dock = DockStyle.Fill;
            _crashSearch.Margin = new Padding(0, 4, 6, 4);
            _crashSearch.PlaceholderText = "搜索文本 / Search...";
            bar.Controls.Add(_crashSearch, 0, 0);

            _crashSearchPrev.Text = "上一个";
            _crashSearchPrev.Style = ModernButton.ButtonStyle.Outline;
            _crashSearchPrev.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashSearchPrev, 1, 0);

            _crashSearchNext.Text = "下一个";
            _crashSearchNext.Style = ModernButton.ButtonStyle.Outline;
            _crashSearchNext.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashSearchNext, 2, 0);

            _crashSearchCase.Text = "Aa: 关";
            _crashSearchCase.Style = ModernButton.ButtonStyle.Outline;
            _crashSearchCase.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashSearchCase, 3, 0);

            _crashJumpLatestIssue.Text = "最新错误";
            _crashJumpLatestIssue.Style = ModernButton.ButtonStyle.Outline;
            _crashJumpLatestIssue.Dock = DockStyle.Fill;
            bar.Controls.Add(_crashJumpLatestIssue, 4, 0);

            _crashSearchCount.Dock = DockStyle.Fill;
            _crashSearchCount.TextAlign = ContentAlignment.MiddleRight;
            _crashSearchCount.Text = string.Empty;
            bar.Controls.Add(_crashSearchCount, 5, 0);

            return bar;
        }

        private void WireEvents()
        {
            _logRefresh.Click += (s, e) => ReloadLogs();
            _logOpenFolder.Click += (s, e) => OpenFolderSafe(LogManager.LogDirectory);
            _logOpenExternal.Click += (s, e) => OpenSelectedFileExternal(_logFiles);
            _logCopyAll.Click += (s, e) => CopyTextSafe(_logText.Text, "日志内容已复制。");
            _logExportBundle.Click += (s, e) => ExportSupportBundle();
            _logFilter.TextChanged += (s, e) => ApplyLogFilter(null);
            _logFollowTail.Click += (s, e) => ToggleLogFollowTail();
            _logHighlight.Click += (s, e) => ToggleLogHighlight();
            _logSearch.TextChanged += (s, e) => UpdateLogSearchCountAsync();
            _logSearchPrev.Click += (s, e) => FindLogPrevious();
            _logSearchNext.Click += (s, e) => FindLogNext();
            _logSearchCase.Click += (s, e) => ToggleLogSearchCase();
            _logJumpLatestIssue.Click += (s, e) => JumpLogLatestIssue();
            _logSearch.KeyDown += LogSearch_KeyDown;
            _logTailLines.ValueChanged += (s, e) => LoadSelectedLog();
            _logFiles.SelectedIndexChanged += (s, e) => LoadSelectedLog();

            _crashRefresh.Click += (s, e) => ReloadCrashReports();
            _crashOpenFolder.Click += (s, e) => OpenFolderSafe(GetCrashReportsDirectory());
            _crashOpenExternal.Click += (s, e) => OpenSelectedFileExternal(_crashFiles);
            _crashCopyAll.Click += (s, e) => CopyTextSafe(_crashText.Text, "崩溃报告已复制。");
            _crashExportBundle.Click += (s, e) => ExportSupportBundle();
            _crashFilter.TextChanged += (s, e) => ApplyCrashFilter(null);
            _crashFollowTail.Click += (s, e) => ToggleCrashFollowTail();
            _crashHighlight.Click += (s, e) => ToggleCrashHighlight();
            _crashSearch.TextChanged += (s, e) => UpdateCrashSearchCountAsync();
            _crashSearchPrev.Click += (s, e) => FindCrashPrevious();
            _crashSearchNext.Click += (s, e) => FindCrashNext();
            _crashSearchCase.Click += (s, e) => ToggleCrashSearchCase();
            _crashJumpLatestIssue.Click += (s, e) => JumpCrashLatestIssue();
            _crashSearch.KeyDown += CrashSearch_KeyDown;
            _crashTailLines.ValueChanged += (s, e) => LoadSelectedCrashReport();
            _crashFiles.SelectedIndexChanged += (s, e) => LoadSelectedCrashReport();
        }

        private void ConfigureAutoRefresh()
        {
            try
            {
                _autoRefreshTimer.Interval = AutoRefreshIntervalMs;
                _autoRefreshTimer.Tick += (s, e) => HandleAutoRefreshTick();
                UpdateAutoRefreshTimerState();
            }
            catch
            {
                // ignore
            }
        }

        private void HandleAutoRefreshTick()
        {
            try
            {
                if (IsDisposed) return;

                // 仅刷新当前页，避免后台不断读盘占用 IO
                if (_tabs.SelectedIndex == 0)
                {
                    if (_logFollowTailEnabled) AutoRefreshLogIfChanged();
                }
                else if (_tabs.SelectedIndex == 1)
                {
                    if (_crashFollowTailEnabled) AutoRefreshCrashIfChanged();
                }
            }
            catch
            {
                // ignore
            }
        }

        private void AutoRefreshLogIfChanged()
        {
            try
            {
                var entry = _logFiles.SelectedItem as FileEntry;
                if (entry == null || string.IsNullOrWhiteSpace(entry.FullPath)) return;

                int tailLines = SafeGetTailLines(_logTailLines, 2000);

                long writeTicks;
                long lengthBytes;
                if (!TryGetFileSignature(entry.FullPath, out writeTicks, out lengthBytes))
                {
                    LoadSelectedLog();
                    return;
                }

                bool same =
                    string.Equals(_logLastLoadedPath, entry.FullPath, StringComparison.OrdinalIgnoreCase) &&
                    _logLastLoadedWriteTicks == writeTicks &&
                    _logLastLoadedLengthBytes == lengthBytes &&
                    _logLastLoadedTailLines == tailLines;
                if (same) return;

                LoadSelectedLog();
            }
            catch
            {
                // ignore
            }
        }

        private void AutoRefreshCrashIfChanged()
        {
            try
            {
                var entry = _crashFiles.SelectedItem as FileEntry;
                if (entry == null || string.IsNullOrWhiteSpace(entry.FullPath)) return;

                int tailLines = SafeGetTailLines(_crashTailLines, 4000);

                long writeTicks;
                long lengthBytes;
                if (!TryGetFileSignature(entry.FullPath, out writeTicks, out lengthBytes))
                {
                    LoadSelectedCrashReport();
                    return;
                }

                bool same =
                    string.Equals(_crashLastLoadedPath, entry.FullPath, StringComparison.OrdinalIgnoreCase) &&
                    _crashLastLoadedWriteTicks == writeTicks &&
                    _crashLastLoadedLengthBytes == lengthBytes &&
                    _crashLastLoadedTailLines == tailLines;
                if (same) return;

                LoadSelectedCrashReport();
            }
            catch
            {
                // ignore
            }
        }

        private static bool TryGetFileSignature(string fullPath, out long lastWriteUtcTicks, out long lengthBytes)
        {
            lastWriteUtcTicks = 0;
            lengthBytes = -1;

            if (string.IsNullOrWhiteSpace(fullPath)) return false;

            try
            {
                var fi = new FileInfo(fullPath);
                if (!fi.Exists) return false;

                lastWriteUtcTicks = fi.LastWriteTimeUtc.Ticks;
                lengthBytes = fi.Length;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateAutoRefreshTimerState()
        {
            try
            {
                bool anyEnabled = _logFollowTailEnabled || _crashFollowTailEnabled;
                if (anyEnabled)
                {
                    if (!_autoRefreshTimer.Enabled) _autoRefreshTimer.Start();
                }
                else
                {
                    if (_autoRefreshTimer.Enabled) _autoRefreshTimer.Stop();
                }
            }
            catch
            {
                // ignore
            }
        }

        private void UpdateToggleButtons()
        {
            try
            {
                _logFollowTail.Text = _logFollowTailEnabled ? "跟随: 开" : "跟随: 关";
                _logFollowTail.Style = _logFollowTailEnabled
                    ? ModernButton.ButtonStyle.Secondary
                    : ModernButton.ButtonStyle.Outline;

                _crashFollowTail.Text = _crashFollowTailEnabled ? "跟随: 开" : "跟随: 关";
                _crashFollowTail.Style = _crashFollowTailEnabled
                    ? ModernButton.ButtonStyle.Secondary
                    : ModernButton.ButtonStyle.Outline;

                _logHighlight.Text = _logHighlightEnabled ? "高亮: 开" : "高亮: 关";
                _logHighlight.Style = _logHighlightEnabled
                    ? ModernButton.ButtonStyle.Secondary
                    : ModernButton.ButtonStyle.Outline;

                _crashHighlight.Text = _crashHighlightEnabled ? "高亮: 开" : "高亮: 关";
                _crashHighlight.Style = _crashHighlightEnabled
                    ? ModernButton.ButtonStyle.Secondary
                    : ModernButton.ButtonStyle.Outline;

                _logSearchCase.Text = _logSearchCaseSensitive ? "Aa: 开" : "Aa: 关";
                _logSearchCase.Style = _logSearchCaseSensitive
                    ? ModernButton.ButtonStyle.Secondary
                    : ModernButton.ButtonStyle.Outline;

                _crashSearchCase.Text = _crashSearchCaseSensitive ? "Aa: 开" : "Aa: 关";
                _crashSearchCase.Style = _crashSearchCaseSensitive
                    ? ModernButton.ButtonStyle.Secondary
                    : ModernButton.ButtonStyle.Outline;
            }
            catch
            {
                // ignore
            }
        }

        private void ToggleLogFollowTail()
        {
            try
            {
                _logFollowTailEnabled = !_logFollowTailEnabled;
                UpdateToggleButtons();
                UpdateAutoRefreshTimerState();
                try { _logMeta.Text = UpdateMetaFollowText(_logMeta.Text, _logFollowTailEnabled); } catch { }
                if (_logFollowTailEnabled) LoadSelectedLog();
            }
            catch
            {
                // ignore
            }
        }

        private void ToggleCrashFollowTail()
        {
            try
            {
                _crashFollowTailEnabled = !_crashFollowTailEnabled;
                UpdateToggleButtons();
                UpdateAutoRefreshTimerState();
                try { _crashMeta.Text = UpdateMetaFollowText(_crashMeta.Text, _crashFollowTailEnabled); } catch { }
                if (_crashFollowTailEnabled) LoadSelectedCrashReport();
            }
            catch
            {
                // ignore
            }
        }

        private void ToggleLogHighlight()
        {
            try
            {
                _logHighlightEnabled = !_logHighlightEnabled;
                UpdateToggleButtons();
                LoadSelectedLog();
            }
            catch
            {
                // ignore
            }
        }

        private void ToggleCrashHighlight()
        {
            try
            {
                _crashHighlightEnabled = !_crashHighlightEnabled;
                UpdateToggleButtons();
                LoadSelectedCrashReport();
            }
            catch
            {
                // ignore
            }
        }

        private void ToggleLogSearchCase()
        {
            try
            {
                _logSearchCaseSensitive = !_logSearchCaseSensitive;
                UpdateToggleButtons();
                UpdateLogSearchCountAsync();
            }
            catch
            {
                // ignore
            }
        }

        private void ToggleCrashSearchCase()
        {
            try
            {
                _crashSearchCaseSensitive = !_crashSearchCaseSensitive;
                UpdateToggleButtons();
                UpdateCrashSearchCountAsync();
            }
            catch
            {
                // ignore
            }
        }

        private void LogSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e == null) return;

                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    if (e.Shift) FindLogPrevious();
                    else FindLogNext();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    try { _logSearch.Text = string.Empty; } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void CrashSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e == null) return;

                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    if (e.Shift) FindCrashPrevious();
                    else FindCrashNext();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    try { _crashSearch.Text = string.Empty; } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void FindLogNext()
        {
            try
            {
                if (!TryFindInViewer(_logText, _logSearch.Text, _logSearchCaseSensitive, true))
                {
                    try { System.Media.SystemSounds.Beep.Play(); } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void FindLogPrevious()
        {
            try
            {
                if (!TryFindInViewer(_logText, _logSearch.Text, _logSearchCaseSensitive, false))
                {
                    try { System.Media.SystemSounds.Beep.Play(); } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void FindCrashNext()
        {
            try
            {
                if (!TryFindInViewer(_crashText, _crashSearch.Text, _crashSearchCaseSensitive, true))
                {
                    try { System.Media.SystemSounds.Beep.Play(); } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void FindCrashPrevious()
        {
            try
            {
                if (!TryFindInViewer(_crashText, _crashSearch.Text, _crashSearchCaseSensitive, false))
                {
                    try { System.Media.SystemSounds.Beep.Play(); } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void JumpLogLatestIssue()
        {
            TryJumpLatestIssue(_logText);
        }

        private void JumpCrashLatestIssue()
        {
            TryJumpLatestIssue(_crashText);
        }

        private void UpdateLogSearchCountAsync()
        {
            try
            {
                string q = _logSearch.Text != null ? _logSearch.Text.Trim() : string.Empty;
                bool hasQuery = !string.IsNullOrWhiteSpace(q);

                _logSearchPrev.Enabled = hasQuery;
                _logSearchNext.Enabled = hasQuery;
                _logJumpLatestIssue.Enabled = true;

                if (!hasQuery)
                {
                    _logSearchCount.Text = string.Empty;
                    return;
                }

                string text = _logText.Text ?? string.Empty;
                bool caseSensitive = _logSearchCaseSensitive;

                _logSearchCountVersion++;
                int currentVersion = _logSearchCountVersion;

                Task.Run(() =>
                {
                    return CountOccurrences(text, q, caseSensitive, 9999);
                }).ContinueWith(t =>
                {
                    try
                    {
                        if (IsDisposed) return;
                        if (currentVersion != _logSearchCountVersion) return;

                        int count = 0;
                        if (t != null && t.Status == TaskStatus.RanToCompletion)
                        {
                            count = t.Result;
                        }

                        BeginInvoke(new Action(() =>
                        {
                            try { _logSearchCount.Text = string.Format("匹配: {0}", count); } catch { }
                        }));
                    }
                    catch
                    {
                        // ignore
                    }
                });
            }
            catch
            {
                // ignore
            }
        }

        private void UpdateCrashSearchCountAsync()
        {
            try
            {
                string q = _crashSearch.Text != null ? _crashSearch.Text.Trim() : string.Empty;
                bool hasQuery = !string.IsNullOrWhiteSpace(q);

                _crashSearchPrev.Enabled = hasQuery;
                _crashSearchNext.Enabled = hasQuery;
                _crashJumpLatestIssue.Enabled = true;

                if (!hasQuery)
                {
                    _crashSearchCount.Text = string.Empty;
                    return;
                }

                string text = _crashText.Text ?? string.Empty;
                bool caseSensitive = _crashSearchCaseSensitive;

                _crashSearchCountVersion++;
                int currentVersion = _crashSearchCountVersion;

                Task.Run(() =>
                {
                    return CountOccurrences(text, q, caseSensitive, 9999);
                }).ContinueWith(t =>
                {
                    try
                    {
                        if (IsDisposed) return;
                        if (currentVersion != _crashSearchCountVersion) return;

                        int count = 0;
                        if (t != null && t.Status == TaskStatus.RanToCompletion)
                        {
                            count = t.Result;
                        }

                        BeginInvoke(new Action(() =>
                        {
                            try { _crashSearchCount.Text = string.Format("匹配: {0}", count); } catch { }
                        }));
                    }
                    catch
                    {
                        // ignore
                    }
                });
            }
            catch
            {
                // ignore
            }
        }

        private static int CountOccurrences(string text, string token, bool caseSensitive, int maxCount)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            if (string.IsNullOrEmpty(token)) return 0;
            if (maxCount <= 0) maxCount = int.MaxValue;

            var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            int count = 0;
            int index = 0;
            while (index < text.Length)
            {
                index = text.IndexOf(token, index, comparison);
                if (index < 0) break;
                count++;
                if (count >= maxCount) break;
                index += token.Length;
            }

            return count;
        }

        private static bool TryFindInViewer(RichTextBox viewer, string query, bool caseSensitive, bool forward)
        {
            if (viewer == null) return false;

            string q = query != null ? query.Trim() : string.Empty;
            if (string.IsNullOrEmpty(q)) return false;

            string text;
            try { text = viewer.Text; }
            catch { return false; }

            if (string.IsNullOrEmpty(text)) return false;

            var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            int start;
            try
            {
                if (forward)
                {
                    start = viewer.SelectionStart + viewer.SelectionLength;
                }
                else
                {
                    start = viewer.SelectionStart - 1;
                }
            }
            catch
            {
                start = forward ? 0 : text.Length - 1;
            }

            if (start < 0) start = 0;
            if (start >= text.Length) start = text.Length - 1;

            int index;
            if (forward)
            {
                index = text.IndexOf(q, start, comparison);
                if (index < 0)
                {
                    // wrap-around
                    index = text.IndexOf(q, 0, comparison);
                }
            }
            else
            {
                index = text.LastIndexOf(q, start, comparison);
                if (index < 0)
                {
                    // wrap-around
                    index = text.LastIndexOf(q, text.Length - 1, comparison);
                }
            }

            if (index < 0) return false;

            try
            {
                viewer.SelectionStart = index;
                viewer.SelectionLength = q.Length;
                viewer.ScrollToCaret();
                viewer.Focus();
            }
            catch
            {
                // ignore
            }

            return true;
        }

        private static void TryJumpLatestIssue(RichTextBox viewer)
        {
            if (viewer == null) return;

            try
            {
                string text = viewer.Text;
                if (string.IsNullOrEmpty(text))
                {
                    try { System.Media.SystemSounds.Beep.Play(); } catch { }
                    return;
                }

                string matched;
                int index = FindLastIndexOfAny(text, new[] { "FATAL", "ERROR", "EXCEPTION", "Exception", "WARNING", "WARN" }, out matched);
                if (index < 0 || string.IsNullOrEmpty(matched))
                {
                    try { System.Media.SystemSounds.Beep.Play(); } catch { }
                    return;
                }

                viewer.SelectionStart = index;
                viewer.SelectionLength = matched.Length;
                viewer.ScrollToCaret();
                viewer.Focus();
            }
            catch
            {
                // ignore
            }
        }

        private static int FindLastIndexOfAny(string text, string[] tokens, out string matchedToken)
        {
            matchedToken = string.Empty;
            if (string.IsNullOrEmpty(text)) return -1;
            if (tokens == null || tokens.Length == 0) return -1;

            int bestIndex = -1;
            string bestToken = string.Empty;

            foreach (var token in tokens)
            {
                if (string.IsNullOrEmpty(token)) continue;
                int idx = text.LastIndexOf(token, StringComparison.OrdinalIgnoreCase);
                if (idx > bestIndex)
                {
                    bestIndex = idx;
                    bestToken = token;
                }
            }

            matchedToken = bestToken;
            return bestIndex;
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
                    _logAllEntries.Clear();
                    _logFiles.Items.Clear();
                    UpdateLogListTitle(0, 0);
                    _logText.Text = "未找到日志目录。";
                    return;
                }

                var selected = GetSelectedFilePath(_logFiles);

                _logAllEntries.Clear();

                var info = new DirectoryInfo(dir);
                var files = info.GetFiles("MES_*.log");
                Array.Sort(files, (a, b) => b.LastWriteTime.CompareTo(a.LastWriteTime));

                foreach (var f in files)
                {
                    _logAllEntries.Add(new FileEntry
                    {
                        FullPath = f.FullName,
                        LastWriteTime = f.LastWriteTime,
                        LengthBytes = f.Length,
                        DisplayName = string.Format("{0}  ({1:MM-dd HH:mm}, {2} KB)", f.Name, f.LastWriteTime, Math.Max(1, f.Length / 1024))
                    });
                }

                ApplyLogFilter(selected);
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
                    _crashAllEntries.Clear();
                    _crashFiles.Items.Clear();
                    UpdateCrashListTitle(0, 0);
                    _crashText.Text = "未找到崩溃报告目录。";
                    return;
                }

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var selected = GetSelectedFilePath(_crashFiles);

                _crashAllEntries.Clear();

                var info = new DirectoryInfo(dir);
                var files = info.GetFiles("MES_Crash_*.txt");
                Array.Sort(files, (a, b) => b.LastWriteTime.CompareTo(a.LastWriteTime));

                foreach (var f in files)
                {
                    _crashAllEntries.Add(new FileEntry
                    {
                        FullPath = f.FullName,
                        LastWriteTime = f.LastWriteTime,
                        LengthBytes = f.Length,
                        DisplayName = string.Format("{0}  ({1:MM-dd HH:mm}, {2} KB)", f.Name, f.LastWriteTime, Math.Max(1, f.Length / 1024))
                    });
                }

                ApplyCrashFilter(selected);
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新崩溃报告列表失败", ex);
                _crashText.Text = string.Format("刷新崩溃报告列表失败：{0}", ex.Message);
            }
        }

        private void ApplyLogFilter(string preferredSelectionPath)
        {
            try
            {
                string selected = !string.IsNullOrWhiteSpace(preferredSelectionPath)
                    ? preferredSelectionPath
                    : GetSelectedFilePath(_logFiles);

                string filterText = _logFilter != null ? _logFilter.Text : string.Empty;

                int shownCount = 0;
                _logFiles.BeginUpdate();
                try
                {
                    _logFiles.Items.Clear();
                    foreach (var entry in _logAllEntries)
                    {
                        if (!MatchesFilter(entry, filterText)) continue;
                        _logFiles.Items.Add(entry);
                        shownCount++;
                    }
                }
                finally
                {
                    _logFiles.EndUpdate();
                }

                UpdateLogListTitle(shownCount, _logAllEntries.Count);

                if (!TryReselect(_logFiles, selected))
                {
                    TrySelectDefaultLog(_logFiles);
                }

                if (_logFiles.Items.Count == 0)
                {
                    _logText.Text = _logAllEntries.Count == 0
                        ? "暂无日志文件。"
                        : "未找到匹配的日志文件。";

                    try
                    {
                        _logMeta.Text = "提示：可调整筛选条件或点击“刷新”。";
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void ApplyCrashFilter(string preferredSelectionPath)
        {
            try
            {
                string selected = !string.IsNullOrWhiteSpace(preferredSelectionPath)
                    ? preferredSelectionPath
                    : GetSelectedFilePath(_crashFiles);

                string filterText = _crashFilter != null ? _crashFilter.Text : string.Empty;

                int shownCount = 0;
                _crashFiles.BeginUpdate();
                try
                {
                    _crashFiles.Items.Clear();
                    foreach (var entry in _crashAllEntries)
                    {
                        if (!MatchesFilter(entry, filterText)) continue;
                        _crashFiles.Items.Add(entry);
                        shownCount++;
                    }
                }
                finally
                {
                    _crashFiles.EndUpdate();
                }

                UpdateCrashListTitle(shownCount, _crashAllEntries.Count);

                if (!TryReselect(_crashFiles, selected))
                {
                    if (_crashFiles.Items.Count > 0)
                    {
                        _crashFiles.SelectedIndex = 0;
                    }
                    else
                    {
                        _crashText.Text = _crashAllEntries.Count == 0
                            ? "暂无崩溃报告（CrashReports 为空）。"
                            : "未找到匹配的崩溃报告。";

                        try
                        {
                            _crashMeta.Text = "提示：可调整筛选条件或点击“刷新”。";
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void UpdateLogListTitle(int shownCount, int totalCount)
        {
            try
            {
                if (totalCount <= 0)
                {
                    _logListTitle.Text = LogTitleBase;
                }
                else
                {
                    _logListTitle.Text = string.Format("{0}  ({1}/{2})", LogTitleBase, shownCount, totalCount);
                }
            }
            catch
            {
                // ignore
            }
        }

        private void UpdateCrashListTitle(int shownCount, int totalCount)
        {
            try
            {
                if (totalCount <= 0)
                {
                    _crashListTitle.Text = CrashTitleBase;
                }
                else
                {
                    _crashListTitle.Text = string.Format("{0}  ({1}/{2})", CrashTitleBase, shownCount, totalCount);
                }
            }
            catch
            {
                // ignore
            }
        }

        private static bool MatchesFilter(FileEntry entry, string filterText)
        {
            if (entry == null) return false;
            if (string.IsNullOrWhiteSpace(filterText)) return true;

            try
            {
                string[] tokens = filterText.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens == null || tokens.Length == 0) return true;

                string name = string.Empty;
                try { name = Path.GetFileName(entry.FullPath) ?? string.Empty; } catch { }

                string display = entry.DisplayName ?? string.Empty;
                string haystack = name + " " + display;

                foreach (var token in tokens)
                {
                    if (string.IsNullOrWhiteSpace(token)) continue;
                    if (haystack.IndexOf(token, StringComparison.OrdinalIgnoreCase) < 0) return false;
                }

                return true;
            }
            catch
            {
                return true;
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
            LoadFileTailAsync(entry, _logText, _logMeta, tailLines, "日志",
                _logFollowTailEnabled, _logHighlightEnabled);
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
            LoadFileTailAsync(entry, _crashText, _crashMeta, tailLines, "崩溃报告",
                _crashFollowTailEnabled, _crashHighlightEnabled);
        }

        private void LoadFileTailAsync(
            FileEntry entry,
            ThemedRichTextBox viewer,
            Label metaLabel,
            int tailLines,
            string kind,
            bool followTail,
            bool highlight)
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
                DateTime lastWriteTime = entry.LastWriteTime;
                long lengthBytes = entry.LengthBytes;
                long lastWriteUtcTicks = 0;
                try
                {
                    var fi = new FileInfo(entry.FullPath);
                    if (fi.Exists)
                    {
                        lastWriteTime = fi.LastWriteTime;
                        lastWriteUtcTicks = fi.LastWriteTimeUtc.Ticks;
                        lengthBytes = fi.Length;
                    }
                }
                catch
                {
                    // ignore
                }

                string text = TextFileTailReader.ReadTailText(entry.FullPath, tailLines, maxBytes);
                if (string.IsNullOrWhiteSpace(text))
                {
                    text = "(空)";
                }

                string followText = GetFollowTailMetaSuffix(followTail);
                string meta = string.Format(
                    "{0}：{1} · {2:yyyy-MM-dd HH:mm:ss} · {3} KB · 显示尾部 {4} 行{5}",
                    kind,
                    Path.GetFileName(entry.FullPath),
                    lastWriteTime,
                    Math.Max(1, lengthBytes / 1024),
                    tailLines,
                    followText);

                return new object[] { text, meta, lastWriteUtcTicks, lengthBytes };
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

                    long lastWriteUtcTicks = 0;
                    long lengthBytes = -1;
                    try { if (result.Length >= 3 && result[2] is long) lastWriteUtcTicks = (long)result[2]; } catch { }
                    try { if (result.Length >= 4 && result[3] is long) lengthBytes = (long)result[3]; } catch { }

                    BeginInvoke(new Action(() =>
                    {
                        bool outdatedOnUi = false;
                        if (viewer == _logText) outdatedOnUi = version != _logLoadVersion;
                        else outdatedOnUi = version != _crashLoadVersion;
                        if (outdatedOnUi) return;

                        viewer.Text = result[0] as string ?? string.Empty;
                        if (metaLabel != null) metaLabel.Text = result[1] as string ?? string.Empty;

                        try
                        {
                            if (viewer == _logText)
                            {
                                _logLastLoadedPath = entry.FullPath ?? string.Empty;
                                _logLastLoadedWriteTicks = lastWriteUtcTicks;
                                _logLastLoadedLengthBytes = lengthBytes;
                                _logLastLoadedTailLines = tailLines;
                            }
                            else if (viewer == _crashText)
                            {
                                _crashLastLoadedPath = entry.FullPath ?? string.Empty;
                                _crashLastLoadedWriteTicks = lastWriteUtcTicks;
                                _crashLastLoadedLengthBytes = lengthBytes;
                                _crashLastLoadedTailLines = tailLines;
                            }
                        }
                        catch
                        {
                            // ignore
                        }

                        if (highlight)
                        {
                            ApplyHighlights(viewer);
                        }

                        if (followTail)
                        {
                            ScrollToEnd(viewer);
                        }

                        try
                        {
                            if (viewer == _logText) UpdateLogSearchCountAsync();
                            else if (viewer == _crashText) UpdateCrashSearchCountAsync();
                        }
                        catch
                        {
                            // ignore
                        }
                    }));
                }
                catch
                {
                    // ignore
                }
            });
        }

        private static string GetFollowTailMetaSuffix(bool followTail)
        {
            double intervalSeconds = Math.Max(0.1, AutoRefreshIntervalMs / 1000.0);
            return followTail
                ? string.Format(" · 跟随: 开({0:0.#}s)", intervalSeconds)
                : " · 跟随: 关";
        }

        private static string UpdateMetaFollowText(string meta, bool followTail)
        {
            if (string.IsNullOrWhiteSpace(meta)) return meta;

            int idx = meta.LastIndexOf(" · 跟随:", StringComparison.Ordinal);
            if (idx < 0) return meta;

            return meta.Substring(0, idx) + GetFollowTailMetaSuffix(followTail);
        }

        private void ApplyHighlights(ThemedRichTextBox viewer)
        {
            if (viewer == null) return;

            try
            {
                var colors = UIThemeManager.Colors;

                int selStart = 0;
                int selLength = 0;
                bool hideSelection = true;

                try
                {
                    selStart = viewer.SelectionStart;
                    selLength = viewer.SelectionLength;
                    hideSelection = viewer.HideSelection;
                }
                catch
                {
                    // ignore
                }

                try { viewer.HideSelection = true; } catch { }

                viewer.SuspendLayout();
                try
                {
                    // 先把整段恢复为默认色，再做关键字着色
                    try
                    {
                        viewer.SelectAll();
                        viewer.SelectionColor = colors.Text;
                        viewer.SelectionLength = 0;
                    }
                    catch
                    {
                        // ignore
                    }

                    HighlightToken(viewer, "[perf]", colors.Primary);

                    HighlightToken(viewer, "FATAL", colors.Error);
                    HighlightToken(viewer, "ERROR", colors.Error);
                    HighlightToken(viewer, "EXCEPTION", colors.Error);
                    HighlightToken(viewer, "Exception", colors.Error);

                    HighlightToken(viewer, "WARNING", colors.Warning);
                    HighlightToken(viewer, "WARN", colors.Warning);
                }
                finally
                {
                    viewer.ResumeLayout();
                }

                try
                {
                    viewer.SelectionStart = selStart;
                    viewer.SelectionLength = selLength;
                }
                catch
                {
                    // ignore
                }

                try { viewer.HideSelection = hideSelection; } catch { }
            }
            catch
            {
                // ignore
            }
        }

        private static void HighlightToken(ThemedRichTextBox viewer, string token, Color color)
        {
            if (viewer == null) return;
            if (string.IsNullOrWhiteSpace(token)) return;

            string text;
            try { text = viewer.Text; }
            catch { return; }

            if (string.IsNullOrEmpty(text)) return;

            int index = 0;
            int matches = 0;
            while (index < text.Length)
            {
                index = text.IndexOf(token, index, StringComparison.OrdinalIgnoreCase);
                if (index < 0) break;

                try
                {
                    viewer.Select(index, token.Length);
                    viewer.SelectionColor = color;
                }
                catch
                {
                    break;
                }

                index += token.Length;
                matches++;
                if (matches >= HighlightMaxMatchesPerToken) break;
            }
        }

        private static void ScrollToEnd(RichTextBox viewer)
        {
            if (viewer == null) return;

            try
            {
                viewer.SelectionStart = viewer.TextLength;
                viewer.SelectionLength = 0;
                viewer.ScrollToCaret();
            }
            catch
            {
                // ignore
            }
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

                // Support Bundle 默认脱敏：避免导出日志/崩溃报告时误携带真实密码
                string copiedLog = CopyFileIfExists(selectedLog, bundleDir, true);
                string copiedCrash = CopyFileIfExists(selectedCrash, bundleDir, true);

                var logTailLines = SafeGetTailLines(_logTailLines, 2000);
                var crashTailLines = SafeGetTailLines(_crashTailLines, 4000);

                WriteTailIfExists(selectedLog, bundleDir, "log_tail.txt", logTailLines, true);
                WriteTailIfExists(selectedCrash, bundleDir, "crash_tail.txt", crashTailLines, true);

                WriteBundleSummary(bundleDir, copiedLog, copiedCrash, logTailLines, crashTailLines);

                string zipPath = TryCreateZipBundle(bundleDir);

                try { Process.Start("explorer.exe", bundleDir); } catch { }

                var message = string.IsNullOrWhiteSpace(zipPath)
                    ? string.Format("诊断包已导出：{0}", bundleDir)
                    : string.Format("诊断包已导出：{0}{1}Zip：{2}", bundleDir, Environment.NewLine, zipPath);
                MessageBox.Show(message, "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error("导出诊断包失败", ex);
                MessageBox.Show(string.Format("导出诊断包失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string CopyFileIfExists(string sourcePath, string bundleDir, bool redact)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourcePath)) return string.Empty; 
                if (!File.Exists(sourcePath)) return string.Empty;
                if (string.IsNullOrWhiteSpace(bundleDir)) return string.Empty;

                var name = Path.GetFileName(sourcePath);
                if (string.IsNullOrWhiteSpace(name)) return string.Empty;

                var dest = Path.Combine(bundleDir, name);
                if (!redact)
                {
                    File.Copy(sourcePath, dest, true);
                    return dest;
                }

                // 小文件直接整体脱敏；大文件逐行脱敏，避免一次性占用过多内存
                try
                {
                    var info = new FileInfo(sourcePath);
                    const long MaxReadAllBytes = 5L * 1024L * 1024L;
                    if (info.Exists && info.Length > 0 && info.Length <= MaxReadAllBytes)
                    {
                        var text = File.ReadAllText(sourcePath, System.Text.Encoding.UTF8);
                        text = MaskSensitiveText(text);
                        File.WriteAllText(dest, text, System.Text.Encoding.UTF8);
                        return File.Exists(dest) ? dest : string.Empty;
                    }

                    using (var reader = new StreamReader(sourcePath, System.Text.Encoding.UTF8, true))
                    using (var writer = new StreamWriter(dest, false, System.Text.Encoding.UTF8))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(MaskSensitiveText(line));
                        }
                    }

                    return File.Exists(dest) ? dest : string.Empty;
                }
                catch
                {
                    // 脱敏失败时回退原样复制（保持功能可用性）
                    try { File.Copy(sourcePath, dest, true); } catch { }
                    return File.Exists(dest) ? dest : string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void WriteTailIfExists(string sourcePath, string bundleDir, string fileName, int tailLines, bool redact)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourcePath)) return;
                if (!File.Exists(sourcePath)) return;
                if (string.IsNullOrWhiteSpace(bundleDir)) return;
                if (string.IsNullOrWhiteSpace(fileName)) return;

                var text = TextFileTailReader.ReadTailText(sourcePath, tailLines);
                if (redact)
                {
                    text = MaskSensitiveText(text);
                }
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

        private static string TryCreateZipBundle(string bundleDir)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bundleDir)) return string.Empty;
                if (!Directory.Exists(bundleDir)) return string.Empty;

                var zipPath = bundleDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + ".zip";
                if (File.Exists(zipPath))
                {
                    try { File.Delete(zipPath); } catch { }
                }

                ZipFile.CreateFromDirectory(bundleDir, zipPath, CompressionLevel.Fastest, true);
                return zipPath;
            }
            catch (Exception ex)
            {
                try { LogManager.Error("创建诊断包压缩文件失败", ex); } catch { }
                return string.Empty;
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

