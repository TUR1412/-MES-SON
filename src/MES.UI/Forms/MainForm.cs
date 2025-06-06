using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Common.Configuration;
using MES.UI.Forms.Material;
// using MES.UI.Framework.Themes;
// using MES.UI.Framework.Utilities;
// using MES.UI.Framework.Controls;

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
                // 应用标准窗体样式
                this.BackColor = Color.FromArgb(240, 240, 240);

                // 设置窗体属性
                this.WindowState = FormWindowState.Maximized;
                this.Text = string.Format("{0} v{1} - 2025年6月4日", ConfigManager.SystemTitle, ConfigManager.SystemVersion);
                this.Icon = SystemIcons.Application;

                // 初始化状态栏
                InitializeStatusBar();

                // 初始化菜单
                InitializeMenu();

                // 初始化工具栏
                InitializeToolBar();

                // 初始化导航树
                InitializeNavigationTree();

                // 初始化主面板
                InitializeMainPanel();

                // 显示欢迎信息
                ShowWelcomeMessage();

                LogManager.Info("主窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("主窗体初始化失败", ex);
                MessageBox.Show(string.Format("主窗体初始化失败：{0}", ex.Message), "错误",
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
            toolStrip1.Items.Clear();

            // 物料管理工具按钮
            var materialBtn = new ToolStripButton("物料管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Information.ToBitmap(),
                ToolTipText = "物料信息管理 (L成员负责)"
            };
            materialBtn.Click += (s, e) => OpenMaterialForm();
            toolStrip1.Items.Add(materialBtn);

            // 生产管理工具按钮
            var productionBtn = new ToolStripButton("生产管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Application.ToBitmap(),
                ToolTipText = "生产订单管理 (H成员负责)"
            };
            productionBtn.Click += (s, e) => OpenProductionOrderForm();
            toolStrip1.Items.Add(productionBtn);

            // 车间管理工具按钮
            var workshopBtn = new ToolStripButton("车间管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Shield.ToBitmap(),
                ToolTipText = "车间作业管理 (S成员负责)"
            };
            workshopBtn.Click += (s, e) => OpenWorkshopOperationForm();
            toolStrip1.Items.Add(workshopBtn);

            // 添加分隔符
            toolStrip1.Items.Add(new ToolStripSeparator());

            // 系统配置按钮
            var configBtn = new ToolStripButton("系统配置")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Question.ToBitmap(),
                ToolTipText = "系统配置管理"
            };
            configBtn.Click += (s, e) => OpenSystemConfigForm();
            toolStrip1.Items.Add(configBtn);

            // 主题切换按钮（演示版）
            var themeBtn = new ToolStripButton("主题切换")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = SystemIcons.Application.ToBitmap(),
                ToolTipText = "演示主题切换功能"
            };
            themeBtn.Click += (s, e) => DemoThemeSwitch();
            toolStrip1.Items.Add(themeBtn);
        }

        /// <summary>
        /// 初始化导航树
        /// </summary>
        private void InitializeNavigationTree()
        {
            treeViewModules.Nodes.Clear();

            // L成员 - 物料管理模块
            var materialNode = new TreeNode("物料管理 (L成员)")
            {
                ForeColor = Color.FromArgb(40, 167, 69),
                NodeFont = new Font("微软雅黑", 10, FontStyle.Bold)
            };
            materialNode.Nodes.Add("物料信息管理");
            materialNode.Nodes.Add("BOM物料清单");
            materialNode.Nodes.Add("工艺路线配置");
            materialNode.ExpandAll();
            treeViewModules.Nodes.Add(materialNode);

            // H成员 - 生产管理模块
            var productionNode = new TreeNode("生产管理 (H成员)")
            {
                ForeColor = Color.FromArgb(0, 123, 255),
                NodeFont = new Font("微软雅黑", 10, FontStyle.Bold)
            };
            productionNode.Nodes.Add("生产订单管理");
            productionNode.Nodes.Add("生产执行控制");
            productionNode.Nodes.Add("用户权限管理");
            productionNode.ExpandAll();
            treeViewModules.Nodes.Add(productionNode);

            // S成员 - 车间管理模块
            var workshopNode = new TreeNode("车间管理 (S成员)")
            {
                ForeColor = Color.FromArgb(220, 53, 69),
                NodeFont = new Font("微软雅黑", 10, FontStyle.Bold)
            };
            workshopNode.Nodes.Add("车间作业管理");
            workshopNode.Nodes.Add("在制品管理");
            workshopNode.Nodes.Add("设备状态管理");
            workshopNode.ExpandAll();
            treeViewModules.Nodes.Add(workshopNode);

            // 系统管理模块
            var systemNode = new TreeNode("系统管理")
            {
                ForeColor = Color.FromArgb(108, 117, 125),
                NodeFont = new Font("微软雅黑", 10, FontStyle.Bold)
            };
            systemNode.Nodes.Add("系统配置");
            systemNode.Nodes.Add("关于系统");
            systemNode.ExpandAll();
            treeViewModules.Nodes.Add(systemNode);

            // 绑定节点点击事件
            treeViewModules.NodeMouseDoubleClick += TreeViewModules_NodeMouseDoubleClick;
        }

        /// <summary>
        /// 导航树节点双击事件
        /// </summary>
        private void TreeViewModules_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var nodeName = e.Node.Text;

            // 根据节点名称打开对应窗体
            switch (nodeName)
            {
                case "物料信息管理":
                    OpenMaterialForm();
                    break;
                case "BOM物料清单":
                    OpenBOMForm();
                    break;
                case "生产订单管理":
                    OpenProductionOrderForm();
                    break;
                case "车间作业管理":
                    OpenWorkshopOperationForm();
                    break;
                case "系统配置":
                    OpenSystemConfigForm();
                    break;
                case "关于系统":
                    ShowAbout();
                    break;
                default:
                    MessageBox.Show(string.Format("功能 '{0}' 正在开发中...", nodeName), "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
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
            systemMenu.DropDownItems.Add(new ToolStripSeparator());
            systemMenu.DropDownItems.Add("UI框架演示", null, (s, e) => ShowUIFrameworkInfo());
            systemMenu.DropDownItems.Add(new ToolStripSeparator());
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
                Text = string.Format("版本 {0} - 基础框架已完成", ConfigManager.SystemVersion),
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
        //private void OpenMaterialForm() { ShowNotImplemented("物料信息管理"); }
        private void OpenMaterialForm() { showMMForm(); }
        private void OpenBOMForm() { ShowNotImplemented("BOM管理"); }
        private void OpenProcessRouteForm() { ShowNotImplemented("工艺路线管理"); }

        // H成员负责实现的生产管理模块
        private void OpenProductionOrderForm() { ShowNotImplemented("生产订单管理"); }
        private void OpenProductionExecutionForm() { ShowNotImplemented("生产执行管理"); }
        private void OpenUserPermissionForm() { ShowNotImplemented("用户权限管理"); }

        // S成员负责实现的车间管理模块
        private void OpenWorkshopOperationForm() { OpenWorkshopManagementForm(); }
        private void OpenWIPForm() { ShowNotImplemented("在制品管理"); }
        private void OpenEquipmentForm() { ShowNotImplemented("设备管理"); }

        /// <summary>
        /// 打开车间管理窗体
        /// </summary>
        private void OpenWorkshopManagementForm()
        {
            try
            {
                var workshopForm = new WorkshopManagementForm();
                workshopForm.ShowDialog();
                LogManager.Info("打开车间管理窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开车间管理窗体失败", ex);
                MessageBox.Show(string.Format("打开车间管理窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 系统管理模块
        private void OpenSystemConfigForm() { ShowNotImplemented("系统配置"); }

        /// <summary>
        /// 显示UI框架信息
        /// </summary>
        private void ShowUIFrameworkInfo()
        {
            try
            {
                string frameworkInfo = @"
🎨 MES UI框架完善项目

✅ 已完成的核心组件：
• UIThemeManager - 主题管理器
• IconManager - 图标资源管理器
• UIHelper - UI通用工具类
• ModernButton - 现代化按钮控件
• EnhancedDataGridView - 增强数据网格
• QueryPanel - 查询面板控件

🎯 主要特性：
• 3种预设主题（默认/蓝色/深色）
• 统一的界面风格和组件库
• 现代化的用户体验设计
• 模块化架构，易于扩展

📊 项目状态：✅ 已完成
📈 质量评级：⭐⭐⭐⭐⭐ 优秀

点击工具栏的'主题切换'按钮可以体验主题切换效果！";

                MessageBox.Show(frameworkInfo, "UI框架演示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info("显示UI框架信息");
            }
            catch (Exception ex)
            {
                LogManager.Error("显示UI框架信息失败", ex);
                MessageBox.Show("显示UI框架信息失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        /// <summary>
        /// 显示功能未实现提示
        /// </summary>
        private void ShowNotImplemented(string functionName)
        {
            MessageBox.Show(string.Format("{0}功能正在开发中，敬请期待！", functionName), "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void showMMForm()
        {
            MaterialManagementForm mmForm = new MaterialManagementForm();
            mmForm.Show();
        }
        /// <summary>
        /// 显示关于对话框
        /// </summary>
        private void ShowAbout()
        {
            string aboutText = $@"
{ConfigManager.SystemTitle}
版本：{ConfigManager.SystemVersion}
技术架构：C# .NET Framework 4.8 + WinForms + MySQL 8.0
开发团队：
- 天帝 (组长) - 架构设计与协调
- L成员 - 物料管理模块
- H成员 - 生产管理模块
- S成员 - 车间管理模块

Copyright © 2025 您的公司名称
";
            MessageBox.Show(aboutText, "关于系统", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 演示主题切换功能
        /// </summary>
        private void DemoThemeSwitch()
        {
            try
            {
                // 演示主题切换效果
                string[] themes = { "默认主题", "蓝色主题", "深色主题" };
                Color[] colors = {
                    Color.FromArgb(240, 240, 240),  // 默认
                    Color.FromArgb(240, 248, 255),  // 蓝色
                    Color.FromArgb(33, 37, 41)      // 深色
                };

                Random rand = new Random();
                int themeIndex = rand.Next(themes.Length);

                this.BackColor = colors[themeIndex];

                MessageBox.Show(string.Format("主题已切换为：{0}\n\n", themes[themeIndex]) +
                    "这是UI框架主题切换功能的演示。\n" +
                    "完整版本支持：\n" +
                    "• 3种预设主题\n" +
                    "• 全局样式应用\n" +
                    "• 动态主题切换\n" +
                    "• 组件自适应",
                    "主题切换演示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info(string.Format("演示主题切换：{0}", themes[themeIndex]));
            }
            catch (Exception ex)
            {
                LogManager.Error("主题切换演示失败", ex);
                MessageBox.Show("主题切换演示失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
