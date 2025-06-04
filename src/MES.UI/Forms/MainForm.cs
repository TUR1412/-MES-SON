using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Common.Configuration;

namespace MES.UI.Forms
{
    /// <summary>
    /// MES系统主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        private Timer statusTimer;
        private ToolStripStatusLabel currentUserLabel;
        private ToolStripStatusLabel systemStatusLabel;

        public MainForm()
        {
            InitializeComponent();
            InitializeMainForm();
        }

        /// <summary>
        /// 初始化主窗体
        /// </summary>
        private void InitializeMainForm()
        {
            try
            {
                // 设置窗体属性
                this.WindowState = FormWindowState.Maximized;
                this.Text = $"{ConfigManager.SystemTitle} v{ConfigManager.SystemVersion} - 2025年6月4日";
                this.Icon = SystemIcons.Application;

                // 设置窗体样式
                this.BackColor = Color.FromArgb(240, 240, 240);

                // 初始化状态栏
                InitializeStatusBar();

                // 初始化菜单
                InitializeMenu();

                // 初始化工具栏
                InitializeToolBar();

                // 初始化主面板
                InitializeMainPanel();

                // 显示欢迎信息
                ShowWelcomeMessage();

                LogManager.Info("主窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("主窗体初始化失败", ex);
                MessageBox.Show($"主窗体初始化失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化状态栏
        /// </summary>
        private void InitializeStatusBar()
        {
            statusStrip1.Items.Clear();

            // 系统状态标签
            systemStatusLabel = new ToolStripStatusLabel("系统就绪")
            {
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.Green
            };
            statusStrip1.Items.Add(systemStatusLabel);

            // 当前用户标签
            currentUserLabel = new ToolStripStatusLabel("当前用户: 管理员")
            {
                ForeColor = Color.Blue
            };
            statusStrip1.Items.Add(currentUserLabel);

            // 分隔符
            statusStrip1.Items.Add(new ToolStripSeparator());

            // 时间标签
            var timeLabel = new ToolStripStatusLabel(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            statusStrip1.Items.Add(timeLabel);

            // 启动定时器更新时间
            statusTimer = new Timer { Interval = 1000 };
            statusTimer.Tick += (s, e) => timeLabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            statusTimer.Start();
        }

        /// <summary>
        /// 初始化工具栏
        /// </summary>
        private void InitializeToolBar()
        {
            var toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                ImageScalingSize = new Size(32, 32)
            };

            // 物料管理工具按钮
            var materialBtn = new ToolStripButton("物料管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Information.ToBitmap(),
                ToolTipText = "物料信息管理 (L成员负责)"
            };
            materialBtn.Click += (s, e) => OpenMaterialForm();
            toolStrip.Items.Add(materialBtn);

            // 生产管理工具按钮
            var productionBtn = new ToolStripButton("生产管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Application.ToBitmap(),
                ToolTipText = "生产订单管理 (H成员负责)"
            };
            productionBtn.Click += (s, e) => OpenProductionOrderForm();
            toolStrip.Items.Add(productionBtn);

            // 车间管理工具按钮
            var workshopBtn = new ToolStripButton("车间管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Shield.ToBitmap(),
                ToolTipText = "车间作业管理 (S成员负责)"
            };
            workshopBtn.Click += (s, e) => OpenWorkshopOperationForm();
            toolStrip.Items.Add(workshopBtn);

            this.Controls.Add(toolStrip);
        }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        private void InitializeMenu()
        {
            // 物料管理菜单 - L成员负责
            var materialMenu = new ToolStripMenuItem("物料管理(&M)");
            materialMenu.DropDownItems.Add("物料信息", null, (s, e) => OpenMaterialForm());
            materialMenu.DropDownItems.Add("BOM管理", null, (s, e) => OpenBOMForm());
            materialMenu.DropDownItems.Add("工艺路线", null, (s, e) => OpenProcessRouteForm());
            menuStrip1.Items.Add(materialMenu);

            // 生产管理菜单 - H成员负责
            var productionMenu = new ToolStripMenuItem("生产管理(&P)");
            productionMenu.DropDownItems.Add("生产订单", null, (s, e) => OpenProductionOrderForm());
            productionMenu.DropDownItems.Add("生产执行", null, (s, e) => OpenProductionExecutionForm());
            productionMenu.DropDownItems.Add("用户权限", null, (s, e) => OpenUserPermissionForm());
            menuStrip1.Items.Add(productionMenu);

            // 车间管理菜单 - S成员负责
            var workshopMenu = new ToolStripMenuItem("车间管理(&W)");
            workshopMenu.DropDownItems.Add("车间作业", null, (s, e) => OpenWorkshopOperationForm());
            workshopMenu.DropDownItems.Add("在制品管理", null, (s, e) => OpenWIPForm());
            workshopMenu.DropDownItems.Add("设备管理", null, (s, e) => OpenEquipmentForm());
            menuStrip1.Items.Add(workshopMenu);

            // 系统管理菜单
            var systemMenu = new ToolStripMenuItem("系统管理(&S)");
            systemMenu.DropDownItems.Add("系统配置", null, (s, e) => OpenSystemConfigForm());
            systemMenu.DropDownItems.Add("关于系统", null, (s, e) => ShowAbout());
            menuStrip1.Items.Add(systemMenu);
        }

        /// <summary>
        /// 初始化主面板
        /// </summary>
        private void InitializeMainPanel()
        {
            // 设置主面板属性
            panelMain.BackColor = Color.White;
            panelMain.Padding = new Padding(20);

            // 清空现有控件
            panelMain.Controls.Clear();

            // 创建欢迎面板
            var welcomePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // 添加系统标题
            var titleLabel = new Label
            {
                Text = "MES制造执行系统",
                Font = new Font("微软雅黑", 28, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 122, 183),
                AutoSize = true,
                Location = new Point(50, 50)
            };
            welcomePanel.Controls.Add(titleLabel);

            // 添加版本信息
            var versionLabel = new Label
            {
                Text = $"版本 {ConfigManager.SystemVersion} - 基础框架已完成",
                Font = new Font("微软雅黑", 12),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(50, 100)
            };
            welcomePanel.Controls.Add(versionLabel);

            // 添加功能模块说明
            var modulePanel = CreateModuleInfoPanel();
            modulePanel.Location = new Point(50, 150);
            welcomePanel.Controls.Add(modulePanel);

            panelMain.Controls.Add(welcomePanel);
        }

        /// <summary>
        /// 创建模块信息面板
        /// </summary>
        private Panel CreateModuleInfoPanel()
        {
            var panel = new Panel
            {
                Size = new Size(800, 400),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            var titleLabel = new Label
            {
                Text = "系统模块分工",
                Font = new Font("微软雅黑", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            panel.Controls.Add(titleLabel);

            // L成员模块
            var lLabel = new Label
            {
                Text = "L成员 - 物料管理模块\n• 物料信息管理\n• BOM物料清单\n• 工艺路线配置",
                Font = new Font("微软雅黑", 11),
                Location = new Point(30, 60),
                Size = new Size(200, 80),
                ForeColor = Color.FromArgb(40, 167, 69)
            };
            panel.Controls.Add(lLabel);

            // H成员模块
            var hLabel = new Label
            {
                Text = "H成员 - 生产管理模块\n• 生产订单管理\n• 生产执行控制\n• 用户权限管理",
                Font = new Font("微软雅黑", 11),
                Location = new Point(280, 60),
                Size = new Size(200, 80),
                ForeColor = Color.FromArgb(0, 123, 255)
            };
            panel.Controls.Add(hLabel);

            // S成员模块
            var sLabel = new Label
            {
                Text = "S成员 - 车间管理模块\n• 车间作业管理\n• 在制品管理\n• 设备状态管理",
                Font = new Font("微软雅黑", 11),
                Location = new Point(530, 60),
                Size = new Size(200, 80),
                ForeColor = Color.FromArgb(220, 53, 69)
            };
            panel.Controls.Add(sLabel);

            // 状态信息
            var statusLabel = new Label
            {
                Text = "当前状态：基础框架已完成，各模块可以开始并行开发",
                Font = new Font("微软雅黑", 12, FontStyle.Bold),
                Location = new Point(30, 160),
                Size = new Size(700, 30),
                ForeColor = Color.FromArgb(25, 135, 84)
            };
            panel.Controls.Add(statusLabel);

            // 技术信息
            var techLabel = new Label
            {
                Text = "技术架构：C# .NET Framework 4.8 + WinForms + MySQL 8.0\n开发模式：三层架构 (UI/BLL/DAL/Models/Common)\n版本控制：Git + GitHub (main/develop分支)",
                Font = new Font("微软雅黑", 10),
                Location = new Point(30, 200),
                Size = new Size(700, 60),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            panel.Controls.Add(techLabel);

            return panel;
        }

        /// <summary>
        /// 显示欢迎消息
        /// </summary>
        private void ShowWelcomeMessage()
        {
            systemStatusLabel.Text = "MES系统启动成功 - 基础框架就绪";
            systemStatusLabel.ForeColor = Color.Green;
        }

        #region 菜单事件处理方法 - 待各模块负责人实现

        // L成员负责实现的物料管理模块
        private void OpenMaterialForm() => ShowNotImplemented("物料信息管理");
        private void OpenBOMForm() => ShowNotImplemented("BOM管理");
        private void OpenProcessRouteForm() => ShowNotImplemented("工艺路线管理");

        // H成员负责实现的生产管理模块
        private void OpenProductionOrderForm() => ShowNotImplemented("生产订单管理");
        private void OpenProductionExecutionForm() => ShowNotImplemented("生产执行管理");
        private void OpenUserPermissionForm() => ShowNotImplemented("用户权限管理");

        // S成员负责实现的车间管理模块
        private void OpenWorkshopOperationForm() => ShowNotImplemented("车间作业管理");
        private void OpenWIPForm() => ShowNotImplemented("在制品管理");
        private void OpenEquipmentForm() => ShowNotImplemented("设备管理");

        // 系统管理模块
        private void OpenSystemConfigForm() => ShowNotImplemented("系统配置");

        #endregion

        /// <summary>
        /// 显示功能未实现提示
        /// </summary>
        private void ShowNotImplemented(string functionName)
        {
            MessageBox.Show($"{functionName}功能正在开发中，敬请期待！", "提示", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示关于对话框
        /// </summary>
        private void ShowAbout()
        {
            MessageBox.Show("MES制造执行系统 v1.0\n\n开发团队：天帝、L、H、S\n技术架构：C# + WinForms + MySQL", 
                "关于系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (MessageBox.Show("确定要退出MES系统吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            
            LogManager.Info("用户退出系统");
            base.OnFormClosing(e);
        }
    }
}
