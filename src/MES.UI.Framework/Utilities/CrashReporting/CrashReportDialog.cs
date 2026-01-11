using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Utilities.CrashReporting
{
    internal sealed class CrashReportDialog : ThemedForm
    {
        private readonly string _title;
        private readonly Exception _exception;
        private readonly string _reportPath;
        private readonly bool _isTerminating;

        private readonly TextBox _details = new TextBox();
        private readonly Label _summary = new Label();
        private readonly Label _path = new Label();

        public CrashReportDialog(string title, Exception exception, string reportPath, bool isTerminating)
        {
            _title = title ?? "发生异常";
            _exception = exception;
            _reportPath = reportPath ?? string.Empty;
            _isTerminating = isTerminating;

            InitializeWindow();
            BuildLayout();
            ApplyTheme();
        }

        private void InitializeWindow()
        {
            Text = "错误边界";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            Width = 860;
            Height = 560;
            KeyPreview = true;
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(16);
            layout.ColumnCount = 1;
            layout.RowCount = 5;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            Controls.Add(layout);

            var titleLabel = new Label();
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Text = _title;
            titleLabel.Font = UIThemeManager.GetTitleFont(12f);
            layout.Controls.Add(titleLabel, 0, 0);

            _summary.Dock = DockStyle.Fill;
            _summary.TextAlign = ContentAlignment.MiddleLeft;
            _summary.Text = BuildSummaryText();
            _summary.Padding = new Padding(12, 8, 12, 8);
            _summary.AutoEllipsis = true;
            layout.Controls.Add(_summary, 0, 1);

            _path.Dock = DockStyle.Fill;
            _path.TextAlign = ContentAlignment.MiddleLeft;
            _path.Padding = new Padding(4, 0, 4, 0);
            _path.Text = string.IsNullOrWhiteSpace(_reportPath)
                ? "崩溃报告：未生成（请查看日志）"
                : string.Format("崩溃报告：{0}", _reportPath);
            layout.Controls.Add(_path, 0, 2);

            _details.Dock = DockStyle.Fill;
            _details.Multiline = true;
            _details.ReadOnly = true;
            _details.ScrollBars = ScrollBars.Both;
            _details.WordWrap = false;
            _details.Font = UIThemeManager.GetFont(9f);
            _details.Text = _exception != null ? _exception.ToString() : "(no exception)";
            layout.Controls.Add(_details, 0, 3);

            var buttons = new FlowLayoutPanel();
            buttons.Dock = DockStyle.Fill;
            buttons.FlowDirection = FlowDirection.RightToLeft;
            buttons.WrapContents = false;
            buttons.Padding = new Padding(0, 8, 0, 0);
            layout.Controls.Add(buttons, 0, 4);

            var btnExit = new ModernButton();
            btnExit.Text = _isTerminating ? "退出" : "退出应用";
            btnExit.Width = 120;
            btnExit.Height = 32;
            btnExit.Style = ModernButton.ButtonStyle.Danger;
            btnExit.Click += (s, e) => { DialogResult = DialogResult.Abort; Close(); };
            buttons.Controls.Add(btnExit);

            var btnContinue = new ModernButton();
            btnContinue.Text = "继续";
            btnContinue.Width = 120;
            btnContinue.Height = 32;
            btnContinue.Enabled = !_isTerminating;
            btnContinue.Style = ModernButton.ButtonStyle.Secondary;
            btnContinue.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            buttons.Controls.Add(btnContinue);

            var btnOpenLogDir = new ModernButton();
            btnOpenLogDir.Text = "打开日志目录";
            btnOpenLogDir.Width = 140;
            btnOpenLogDir.Height = 32;
            btnOpenLogDir.Style = ModernButton.ButtonStyle.Outline;
            btnOpenLogDir.Click += (s, e) => TryOpenLogDirectory();
            buttons.Controls.Add(btnOpenLogDir);

            var btnOpenReport = new ModernButton();
            btnOpenReport.Text = "打开报告";
            btnOpenReport.Width = 120;
            btnOpenReport.Height = 32;
            btnOpenReport.Enabled = !string.IsNullOrWhiteSpace(_reportPath);
            btnOpenReport.Style = ModernButton.ButtonStyle.Outline;
            btnOpenReport.Click += (s, e) => TryOpenReport();
            buttons.Controls.Add(btnOpenReport);

            var btnCopy = new ModernButton();
            btnCopy.Text = "复制详情";
            btnCopy.Width = 120;
            btnCopy.Height = 32;
            btnCopy.Style = ModernButton.ButtonStyle.Primary;
            btnCopy.Click += (s, e) => TryCopyDetails();
            buttons.Controls.Add(btnCopy);

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = _isTerminating ? DialogResult.Abort : DialogResult.OK;
                    Close();
                }
            };
        }

        private void ApplyTheme()
        {
            try
            {
                var colors = UIThemeManager.Colors;
                BackColor = colors.Background;
                ForeColor = colors.Text;
                Font = UIThemeManager.GetFont(9f);

                _summary.BackColor = colors.Surface;
                _summary.ForeColor = colors.Text;

                _path.ForeColor = colors.TextSecondary;

                _details.BackColor = colors.Surface;
                _details.ForeColor = colors.Text;

                UIThemeManager.ApplyTheme(this);
            }
            catch
            {
                // ignore
            }
        }

        private string BuildSummaryText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("应用遇到未处理异常。");
            if (_isTerminating)
            {
                sb.Append("该错误可能导致程序退出。");
            }
            else
            {
                sb.Append("你可以选择继续（可能存在不稳定风险）。");
            }

            if (_exception != null && !string.IsNullOrWhiteSpace(_exception.Message))
            {
                sb.AppendLine();
                sb.AppendLine();
                sb.Append("原因：");
                sb.Append(_exception.Message);
            }

            return sb.ToString();
        }

        private void TryCopyDetails()
        {
            try
            {
                Clipboard.SetText(_details.Text ?? string.Empty);
            }
            catch (Exception ex)
            {
                try { LogManager.Error("复制异常详情失败", ex); } catch { }
            }
        }

        private void TryOpenReport()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_reportPath)) return;
                if (!File.Exists(_reportPath)) return;
                Process.Start(_reportPath);
            }
            catch (Exception ex)
            {
                try { LogManager.Error("打开崩溃报告失败", ex); } catch { }
            }
        }

        private void TryOpenLogDirectory()
        {
            try
            {
                var dir = LogManager.LogDirectory;
                if (string.IsNullOrWhiteSpace(dir)) return;
                Process.Start("explorer.exe", dir);
            }
            catch (Exception ex)
            {
                try { LogManager.Error("打开日志目录失败", ex); } catch { }
            }
        }
    }
}
