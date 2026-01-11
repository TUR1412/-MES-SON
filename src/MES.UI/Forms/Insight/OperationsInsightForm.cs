// 运营洞察界面，展示关键指标与风险清单。
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MES.BLL.Analytics;
using MES.Models.Analytics;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;
using MES.UI.Framework.Utilities;

namespace MES.UI.Forms.Insight
{
    /// <summary>
    /// 运营洞察中心
    /// </summary>
    public class OperationsInsightForm : ThemedForm
    {
        private readonly InsightBLL _insightBLL;
        private FlowLayoutPanel _kpiPanel;
        private EnhancedDataGridView _alertGrid;
        private Label _generatedLabel;
        private LolActionButton _refreshButton;
        private Timer _autoRefreshTimer;

        public OperationsInsightForm()
        {
            _insightBLL = new InsightBLL();
            InitializeLayout();
            LoadSnapshot();
            StartAutoRefresh();
        }

        private void InitializeLayout()
        {
            Text = "运营洞察中心";
            BackColor = UIThemeManager.Colors.Background;
            ForeColor = UIThemeManager.Colors.Text;
            Size = new Size(1100, 720);
            UIHelper.SetMinimumSize(this, 980, 640);

            var root = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 20, 24, 20),
                BackColor = Color.Transparent
            };
            Controls.Add(root);

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.Transparent
            };
            root.Controls.Add(header);

            var title = new Label
            {
                AutoSize = false,
                Text = "运营洞察 · Command Center",
                Font = UIThemeManager.GetTitleFont(18f),
                ForeColor = UIThemeManager.Colors.Text,
                Dock = DockStyle.Top,
                Height = 36,
                TextAlign = ContentAlignment.MiddleLeft
            };
            header.Controls.Add(title);

            _generatedLabel = new Label
            {
                AutoSize = false,
                Text = "最近更新时间：--",
                Font = UIThemeManager.GetFont(9f),
                ForeColor = UIThemeManager.Colors.TextSecondary,
                Dock = DockStyle.Bottom,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft
            };
            header.Controls.Add(_generatedLabel);

            _refreshButton = new LolActionButton
            {
                Text = "刷新数据",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(header.Width - 120, 10)
            };
            _refreshButton.Click += (s, e) => LoadSnapshot();
            header.Controls.Add(_refreshButton);

            header.SizeChanged += (s, e) =>
            {
                _refreshButton.Location = new Point(header.Width - _refreshButton.Width - 4, 10);
            };

            _kpiPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 260,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(4, 6, 4, 6),
                BackColor = Color.Transparent
            };
            EnableDoubleBuffering(_kpiPanel);
            root.Controls.Add(_kpiPanel);

            var alertTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = "风险提醒",
                Font = UIThemeManager.GetTitleFont(12f),
                ForeColor = UIThemeManager.Colors.Text,
                TextAlign = ContentAlignment.MiddleLeft
            };
            root.Controls.Add(alertTitle);

            _alertGrid = new EnhancedDataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            UIHelper.ConfigureStandardDataGrid(_alertGrid);
            root.Controls.Add(_alertGrid);
        }

        private void LoadSnapshot()
        {
            try
            {
                var snapshot = _insightBLL.GetOperationalSnapshot();
                if (snapshot == null)
                {
                    return;
                }

                _generatedLabel.Text = string.Format("最近更新时间：{0:yyyy-MM-dd HH:mm:ss}", snapshot.GeneratedAt);
                BuildKpiCards(snapshot);
                BindAlerts(snapshot);
            }
            catch
            {
                // ignore
            }
        }

        private void BuildKpiCards(OperationalInsightSnapshot snapshot)
        {
            _kpiPanel.SuspendLayout();
            _kpiPanel.Controls.Clear();

            var colors = UIThemeManager.Colors;

            _kpiPanel.Controls.Add(CreateKpiCard(
                "生产交付风险",
                string.Format("{0}/{1}", snapshot.ProductionRisk.HighRiskCount, snapshot.ProductionRisk.TotalActive),
                "高风险订单 / 活跃订单",
                colors.Error));

            _kpiPanel.Controls.Add(CreateKpiCard(
                "在制品老化",
                snapshot.WipAging.AgingCount.ToString(),
                string.Format("瓶颈车间：{0}", snapshot.WipAging.BottleneckWorkshop ?? "暂无"),
                colors.Warning));

            _kpiPanel.Controls.Add(CreateKpiCard(
                "设备健康",
                snapshot.EquipmentHealth.AverageHealthScore.ToString("F0"),
                string.Format("逾期维护：{0}", snapshot.EquipmentHealth.OverdueMaintenanceCount),
                colors.Secondary));

            _kpiPanel.Controls.Add(CreateKpiCard(
                "物料库存告警",
                snapshot.MaterialStock.BelowMinCount.ToString(),
                string.Format("低于安全库存：{0}", snapshot.MaterialStock.BelowSafetyCount),
                colors.Primary));

            _kpiPanel.Controls.Add(CreateKpiCard(
                "质量良率",
                snapshot.Quality.QualifiedRate.ToString("F1") + "%",
                string.Format("缺陷焦点：{0}", snapshot.Quality.TopDefectReason ?? "暂无"),
                colors.Success));

            _kpiPanel.Controls.Add(CreateKpiCard(
                "批次良率",
                (snapshot.BatchYield.AverageYieldRate * 100).ToString("F1") + "%",
                string.Format("低良率批次：{0}", snapshot.BatchYield.LowYieldCount),
                colors.Primary));

            _kpiPanel.ResumeLayout();
        }

        private void BindAlerts(OperationalInsightSnapshot snapshot)
        {
            var table = new DataTable();
            table.Columns.Add("等级");
            table.Columns.Add("模块");
            table.Columns.Add("标题");
            table.Columns.Add("描述");

            var alerts = snapshot.Alerts ?? new System.Collections.Generic.List<InsightAlertItem>();
            foreach (var alert in alerts)
            {
                var row = table.NewRow();
                row["等级"] = alert.Level.ToString();
                row["模块"] = alert.Module;
                row["标题"] = alert.Title;
                row["描述"] = alert.Description;
                table.Rows.Add(row);
            }

            _alertGrid.DataSource = table;
        }

        private Panel CreateKpiCard(string title, string value, string subtitle, Color accent)
        {
            var panel = new Panel
            {
                Width = 250,
                Height = 110,
                Margin = new Padding(10),
                BackColor = Color.Transparent
            };
            EnableDoubleBuffering(panel);

            panel.Paint += (s, e) =>
            {
                var rect = panel.ClientRectangle;
                rect.Width -= 1;
                rect.Height -= 1;

                var colors = UIThemeManager.Colors;
                using (var brush = new LinearGradientBrush(rect, colors.Surface, colors.Background, 90f))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }

                using (var pen = new Pen(Color.FromArgb(140, accent), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            var titleLabel = new Label
            {
                AutoSize = false,
                Text = title,
                Font = UIThemeManager.GetTitleFont(11f),
                ForeColor = UIThemeManager.Colors.Text,
                Location = new Point(14, 10),
                Size = new Size(220, 22),
                BackColor = Color.Transparent
            };

            var valueLabel = new Label
            {
                AutoSize = false,
                Text = value,
                Font = UIThemeManager.GetTitleFont(20f),
                ForeColor = accent,
                Location = new Point(14, 36),
                Size = new Size(220, 32),
                BackColor = Color.Transparent
            };

            var subtitleLabel = new Label
            {
                AutoSize = false,
                Text = subtitle,
                Font = UIThemeManager.GetFont(9f),
                ForeColor = UIThemeManager.Colors.TextSecondary,
                Location = new Point(14, 72),
                Size = new Size(220, 20),
                BackColor = Color.Transparent
            };

            panel.Controls.Add(titleLabel);
            panel.Controls.Add(valueLabel);
            panel.Controls.Add(subtitleLabel);

            return panel;
        }

        private void StartAutoRefresh()
        {
            _autoRefreshTimer = new Timer { Interval = 60000 };
            _autoRefreshTimer.Tick += (s, e) => LoadSnapshot();
            _autoRefreshTimer.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_autoRefreshTimer != null)
            {
                _autoRefreshTimer.Stop();
                _autoRefreshTimer.Dispose();
                _autoRefreshTimer = null;
            }
            base.OnFormClosing(e);
        }

        private static void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;
            try
            {
                typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    .SetValue(control, true, null);
            }
            catch
            {
                // ignore
            }
        }
    }
}

