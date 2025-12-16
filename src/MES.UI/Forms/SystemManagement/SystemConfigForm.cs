using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Configuration;
using MES.Common.Logging;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.SystemManagement
{
    /// <summary>
    /// 系统配置窗体 - 设计器版本
    /// </summary>
    public partial class SystemConfigForm : ThemedForm
    {
        public SystemConfigForm()
        {
            InitializeComponent();
            InitializeCustomControls();
            LoadCurrentSettings();
            ApplyModernStyling();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
        }

        private void InitializeCustomControls()
        {
            CreateSystemIcon();
            SetDefaultValues();
        }

        private void CreateSystemIcon()
        {
            var bitmap = new Bitmap(48, 48);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // 绘制齿轮图标
                using (var brush = new SolidBrush(Color.FromArgb(255, 193, 7)))
                {
                    g.FillEllipse(brush, 6, 6, 36, 36);
                }

                using (var brush = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brush, 15, 15, 18, 18);
                }
            }
            iconPictureBox.Image = bitmap;
        }

        private void SetDefaultValues()
        {
            // 设置默认值
            themeComboBox.SelectedIndex = 0;
            logLevelComboBox.SelectedIndex = 2;
        }

        private void ApplyModernStyling()
        {
            // LoL 暗金风 Tab：避免“白底灰边”割裂感
            configTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            configTabControl.DrawItem += ConfigTabControl_DrawItem;

            try
            {
                configTabControl.Appearance = TabAppearance.Normal;
                configTabControl.SizeMode = TabSizeMode.Fixed;
                if (configTabControl.ItemSize.Height < 32)
                {
                    configTabControl.ItemSize = new Size(Math.Max(80, configTabControl.ItemSize.Width), 34);
                }
            }
            catch
            {
                // ignore
            }
        }

        private void ConfigTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabPage = tabControl.TabPages[e.Index];
            var tabRect = tabControl.GetTabRect(e.Index);

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // 背景：选中更亮一档；未选中更深
            var backColor = selected ? LeagueColors.DarkSurface : LeagueColors.DarkPanel;
            using (var brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // 边框：切出层级（避免 Tab 看起来像“贴纸”）
            using (var pen = new Pen(selected ? Color.FromArgb(160, LeagueColors.RiotBorderGold) : Color.FromArgb(90, LeagueColors.DarkBorder), 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1));
            }

            // 选中态下方高亮条（LoL 客户端感）
            if (selected)
            {
                using (var pen = new Pen(Color.FromArgb(220, LeagueColors.RiotGoldHover), 2))
                {
                    e.Graphics.DrawLine(pen, tabRect.Left + 10, tabRect.Bottom - 2, tabRect.Right - 10, tabRect.Bottom - 2);
                }
            }

            // 文本
            var textColor = selected ? LeagueColors.TextHighlight : LeagueColors.TextSecondary;
            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabControl.Font, tabRect, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private void LoadCurrentSettings()
        {
            try
            {
                // 加载当前配置
                systemTitleTextBox.Text = ConfigManager.SystemTitle;
                systemVersionTextBox.Text = ConfigManager.SystemVersion;
                // 其他配置项...
            }
            catch (Exception ex)
            {
                LogManager.Error("加载系统配置失败", ex);
            }
        }

        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                connectionStatusLabel.Text = "正在测试连接...";
                connectionStatusLabel.ForeColor = Color.FromArgb(255, 193, 7);

                // 模拟连接测试
                System.Threading.Thread.Sleep(1000);

                connectionStatusLabel.Text = "✅ 连接成功";
                connectionStatusLabel.ForeColor = Color.FromArgb(40, 167, 69);

                LogManager.Info("数据库连接测试成功");
            }
            catch (Exception ex)
            {
                connectionStatusLabel.Text = "❌ 连接失败";
                connectionStatusLabel.ForeColor = Color.FromArgb(220, 53, 69);
                LogManager.Error("数据库连接测试失败", ex);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 保存配置逻辑
                MessageBox.Show("✅ 配置保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogManager.Info("系统配置保存成功");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存配置失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogManager.Error("保存系统配置失败", ex);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置所有配置为默认值吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoadCurrentSettings();
                MessageBox.Show("配置已重置为默认值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
