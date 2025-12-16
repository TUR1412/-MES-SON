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
    /// 关于系统窗体 - 超高度美化版本
    /// </summary>
    public partial class AboutForm : ThemedForm
    {
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel footerPanel;
        private PictureBox logoBox;
        private Label titleLabel;
        private Label versionLabel;
        private Label copyrightLabel;
        private RichTextBox infoRichTextBox;
        private Button okButton;
        private Timer animationTimer;
        private int animationStep = 0;

        public AboutForm()
        {
            InitializeComponent();
            InitializeCustomControls();
            SetupAnimation();
            ApplyModernStyling();

            // 统一套用 LoL 主题（V2）：让所有窗体视觉一致
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 窗体基本设置
            this.Text = "关于 MES 制造执行系统";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = LeagueColors.DarkBackground;
            this.Font = new Font("微软雅黑", 9F);

            this.ResumeLayout(false);
        }

        private void InitializeCustomControls()
        {
            CreateHeaderPanel();
            CreateContentPanel();
            CreateFooterPanel();
        }

        private void CreateHeaderPanel()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = LeagueColors.DarkPanel
            };

            // 系统Logo
            logoBox = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(50, 35),
                Image = CreateSystemLogo(),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // 系统标题
            titleLabel = new Label
            {
                Text = "MES 制造执行系统",
                Font = new Font("微软雅黑", 20F, FontStyle.Bold),
                ForeColor = LeagueColors.TextHighlight,
                Location = new Point(150, 30),
                Size = new Size(400, 40),
                BackColor = Color.Transparent
            };

            // 版本信息
            versionLabel = new Label
            {
                Text = string.Format("Version {0} - Enterprise Edition", ConfigManager.SystemVersion),
                Font = new Font("微软雅黑", 11F),
                ForeColor = LeagueColors.TextPrimary,
                Location = new Point(150, 75),
                Size = new Size(400, 25),
                BackColor = Color.Transparent
            };

            // 版权信息
            copyrightLabel = new Label
            {
                Text = "© 2024 MES Development Team. All Rights Reserved.",
                Font = new Font("微软雅黑", 9F),
                ForeColor = LeagueColors.TextSecondary,
                Location = new Point(150, 105),
                Size = new Size(400, 20),
                BackColor = Color.Transparent
            };

            headerPanel.Controls.AddRange(new Control[] { logoBox, titleLabel, versionLabel, copyrightLabel });
            this.Controls.Add(headerPanel);
        }

        private void CreateContentPanel()
        {
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = LeagueColors.DarkSurface,
                Padding = new Padding(30, 20, 30, 20)
            };

            // 系统信息文本框
            infoRichTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = LeagueColors.InputBackground,
                BorderStyle = BorderStyle.None,
                Font = new Font("微软雅黑", 9.5F),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            SetSystemInfo();
            contentPanel.Controls.Add(infoRichTextBox);
            this.Controls.Add(contentPanel);
        }

        private void CreateFooterPanel()
        {
            footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = LeagueColors.DarkSurface,
                Padding = new Padding(30, 15, 30, 15)
            };

            // 确定按钮
            okButton = new Button
            {
                Text = "确定",
                Size = new Size(120, 40),
                Location = new Point(footerPanel.Width - 150, 15),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Font = new Font("微软雅黑", 10F, FontStyle.Bold),
                BackColor = LeagueColors.DarkSurface,
                ForeColor = LeagueColors.TextHighlight,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            okButton.FlatAppearance.BorderSize = 0;
            okButton.Click += OkButton_Click;

            footerPanel.Controls.Add(okButton);
            this.Controls.Add(footerPanel);
        }

        private void SetSystemInfo()
        {
            var info = @"系统详细信息

产品概述
MES 制造执行系统是一套专为制造企业设计的综合性生产管理解决方案，致力于提升生产效率、优化资源配置、确保产品质量。

技术架构
• 开发框架：Microsoft .NET Framework 4.8
• 用户界面：Windows Forms (WinForms)
• 数据库：MySQL 8.0
• 架构模式：三层架构 (UI / BLL / DAL / Models / Common)
• 开发语言：C# 5.0

核心功能模块
• 物料管理：物料信息管理、BOM 清单、工艺路线配置
• 生产管理：生产订单管理、生产执行控制、工单管理
• 车间管理：车间作业管理、在制品管理、设备状态管理
• 系统管理：系统配置、数据库诊断

开发团队
• 天帝：架构设计与项目协调
• L 成员：物料管理模块开发
• H 成员：生产管理模块开发
• S 成员：车间管理模块开发

项目状态
• 开发进度：基础框架完成
• 质量等级：企业级
• 版本控制：Git + GitHub
• 部署状态：开发环境就绪

安全特性
• 用户身份验证
• 角色权限控制
• 操作日志记录
• 数据加密传输

技术支持
如需技术支持或有任何问题，请联系开发团队。";

            infoRichTextBox.Text = info;

            // 设置富文本格式
            FormatRichText();
        }

        private void FormatRichText()
        {
            // LoL 暗金风：暗底下要保证高对比与层级（避免“深底深字/浅底浅字”）
            SetTextStyle("系统详细信息", 14, FontStyle.Bold, LeagueColors.TextHighlight);

            // 各个部分标题
            SetTextStyle("产品概述", 12, FontStyle.Bold, LeagueColors.TextHighlight);
            SetTextStyle("技术架构", 12, FontStyle.Bold, LeagueColors.SpecialCyan);
            SetTextStyle("核心功能模块", 12, FontStyle.Bold, LeagueColors.RiotGoldHover);
            SetTextStyle("开发团队", 12, FontStyle.Bold, LeagueColors.TextPrimary);
            SetTextStyle("项目状态", 12, FontStyle.Bold, LeagueColors.TextSecondary);
            SetTextStyle("安全特性", 12, FontStyle.Bold, LeagueColors.AccentBlueLight);
            SetTextStyle("技术支持", 12, FontStyle.Bold, LeagueColors.WarningOrange);
        }

        private void SetTextStyle(string text, int fontSize, FontStyle style, Color color)
        {
            int start = infoRichTextBox.Text.IndexOf(text);
            if (start >= 0)
            {
                infoRichTextBox.Select(start, text.Length);
                infoRichTextBox.SelectionFont = new Font("微软雅黑", fontSize, style);
                infoRichTextBox.SelectionColor = color;
            }
        }

        private Image CreateSystemLogo()
        {
            var bitmap = new Bitmap(80, 80);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // 绘制渐变背景
                using (var brush = new LinearGradientBrush(
                    new Point(0, 0),
                    new Point(80, 80),
                    LeagueColors.RiotGoldDark,
                    LeagueColors.RiotGold))
                {
                    g.FillEllipse(brush, 5, 5, 70, 70);
                }

                // 绘制内圆
                using (var brush = new SolidBrush(LeagueColors.DarkBackground))
                {
                    g.FillEllipse(brush, 15, 15, 50, 50);
                }

                // 绘制MES文字
                using (var brush = new SolidBrush(LeagueColors.TextHighlight))
                {
                    var font = new Font("微软雅黑", 12F, FontStyle.Bold);
                    var textSize = g.MeasureString("MES", font);
                    var x = (80 - textSize.Width) / 2;
                    var y = (80 - textSize.Height) / 2;
                    g.DrawString("MES", font, brush, x, y);
                }
            }
            return bitmap;
        }

        private void SetupAnimation()
        {
            animationTimer = new Timer
            {
                Interval = 50,
                Enabled = true
            };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            animationStep++;
            
            // 创建呼吸效果
            var alpha = (int)(30 + 25 * Math.Sin(animationStep * 0.1));
            logoBox.BackColor = Color.FromArgb(alpha, LeagueColors.RiotGold);

            // 限制动画步数避免溢出
            if (animationStep > 1000) animationStep = 0;
        }

        private void ApplyModernStyling()
        {
            // 添加阴影效果
            this.Paint += AboutForm_Paint;
        }

        private void AboutForm_Paint(object sender, PaintEventArgs e)
        {
            // 绘制窗体边框阴影效果
            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (var pen = new Pen(Color.FromArgb(100, LeagueColors.RiotBorderGold), 1))
            {
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (animationTimer != null)
            {
                animationTimer.Stop();
                animationTimer.Dispose();
            }
            base.OnFormClosed(e);
        }
    }
}
