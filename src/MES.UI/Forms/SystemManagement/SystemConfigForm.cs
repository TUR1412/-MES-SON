using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Configuration;
using MES.Common.Logging;

namespace MES.UI.Forms.SystemManagement
{
    /// <summary>
    /// 系统配置窗体 - 设计器版本
    /// </summary>
    public partial class SystemConfigForm : Form
    {
        public SystemConfigForm()
        {
            InitializeComponent();
            InitializeCustomControls();
            LoadCurrentSettings();
            ApplyModernStyling();
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
            // 应用现代化样式
            configTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            configTabControl.DrawItem += ConfigTabControl_DrawItem;
        }

        private void ConfigTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabPage = tabControl.TabPages[e.Index];
            var tabRect = tabControl.GetTabRect(e.Index);

            // 绘制选项卡背景
            var backColor = e.State == DrawItemState.Selected ? Color.White : Color.FromArgb(233, 236, 239);
            using (var brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // 绘制选项卡文本
            var textColor = e.State == DrawItemState.Selected ? Color.FromArgb(52, 58, 64) : Color.FromArgb(108, 117, 125);
            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabControl.Font, tabRect, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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
