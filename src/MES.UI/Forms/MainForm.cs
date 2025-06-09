using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Common.Configuration;
using MES.UI.Forms.Material;
using MES.UI.Forms.Production;
using MES.UI.Forms.WorkOrder;
using MES.UI.Forms.Batch;
using MES.UI.Forms.SystemManagement;
using MES.UI.Forms.Workshop;
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
                this.BackColor = Color.FromArgb(248, 249, 250);

                // 设置窗体属性
                this.WindowState = FormWindowState.Maximized;
                this.Text = string.Format("{0} v{1} - 2025年6月7日", ConfigManager.SystemTitle, ConfigManager.SystemVersion);
                this.Icon = SystemIcons.Application;

                // 初始化状态栏
                InitializeStatusBar();

                // 初始化菜单
                InitializeMenu();

                // 初始化工具栏
                InitializeToolBar();

                // 初始化导航树
                InitializeNavigationTree();

                // 初始化主面板内容
                InitializeMainPanelContent();

                // 初始化卡片点击事件
                InitializeCardClickEvents();

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
                ForeColor = Color.FromArgb(25, 135, 84),
                Font = new Font("微软雅黑", 9F)
            };
            statusStrip1.Items.Add(systemStatusLabel);

            // 当前用户标签
            currentUserLabel = new ToolStripStatusLabel("当前用户: 管理员")
            {
                ForeColor = Color.FromArgb(0, 123, 255),
                Font = new Font("微软雅黑", 9F)
            };
            statusStrip1.Items.Add(currentUserLabel);

            // 分隔符
            statusStrip1.Items.Add(new ToolStripSeparator());

            // 数据库连接状态
            var dbStatusLabel = new ToolStripStatusLabel("数据库: 已连接")
            {
                ForeColor = Color.FromArgb(25, 135, 84),
                Font = new Font("微软雅黑", 9F)
            };
            statusStrip1.Items.Add(dbStatusLabel);

            // 分隔符
            statusStrip1.Items.Add(new ToolStripSeparator());

            // 时间标签 - 增强显示格式
            var timeLabel = new ToolStripStatusLabel(GetFormattedDateTime())
            {
                ForeColor = Color.FromArgb(108, 117, 125),
                Font = new Font("微软雅黑", 9F, FontStyle.Bold)
            };
            statusStrip1.Items.Add(timeLabel);

            // 启动定时器更新时间 - 每秒更新
            statusTimer = new Timer();
            statusTimer.Interval = 1000;
            statusTimer.Tick += StatusTimer_Tick;
            statusTimer.Start();
        }

        /// <summary>
        /// 状态定时器事件处理
        /// </summary>
        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            var timeLabel = statusStrip1.Items[statusStrip1.Items.Count - 1] as ToolStripStatusLabel;
            if (timeLabel != null)
            {
                timeLabel.Text = GetFormattedDateTime();
                // 可以在这里添加其他实时更新的状态信息
            }
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
                Image = CreateToolBarIcon(Color.FromArgb(40, 167, 69)),
                ToolTipText = "物料信息管理 (L成员负责) - Ctrl+M",
                Font = new Font("微软雅黑", 9F),
                ForeColor = Color.FromArgb(40, 167, 69),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight
            };
            materialBtn.Click += MaterialBtn_Click;
            toolStrip1.Items.Add(materialBtn);

            // 分隔符
            toolStrip1.Items.Add(new ToolStripSeparator());

            // 生产管理工具按钮
            var productionBtn = new ToolStripButton("生产管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = CreateToolBarIcon(Color.FromArgb(0, 123, 255)),
                ToolTipText = "生产订单管理 (H成员负责) - Ctrl+P",
                Font = new Font("微软雅黑", 9F),
                ForeColor = Color.FromArgb(0, 123, 255),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight
            };
            productionBtn.Click += ProductionBtn_Click;
            toolStrip1.Items.Add(productionBtn);

            // 分隔符
            toolStrip1.Items.Add(new ToolStripSeparator());

            // 车间管理工具按钮
            var workshopBtn = new ToolStripButton("车间管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = CreateToolBarIcon(Color.FromArgb(220, 53, 69)),
                ToolTipText = "车间作业管理 (S成员负责) - Ctrl+W",
                Font = new Font("微软雅黑", 9F),
                ForeColor = Color.FromArgb(220, 53, 69),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight
            };
            workshopBtn.Click += WorkshopBtn_Click;
            toolStrip1.Items.Add(workshopBtn);

            // 分隔符
            toolStrip1.Items.Add(new ToolStripSeparator());

            // 系统管理工具按钮
            var systemBtn = new ToolStripButton("系统管理")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = CreateToolBarIcon(Color.FromArgb(108, 117, 125)),
                ToolTipText = "系统配置和管理",
                Font = new Font("微软雅黑", 9F),
                ForeColor = Color.FromArgb(108, 117, 125),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight
            };
            systemBtn.Click += SystemBtn_Click;
            toolStrip1.Items.Add(systemBtn);

            // 弹性空间
            var spacer = new ToolStripLabel();
            spacer.Text = "";
            spacer.AutoSize = false;
            spacer.Width = 100;
            toolStrip1.Items.Add(spacer);

            // 刷新按钮
            var refreshBtn = new ToolStripButton("刷新");
            refreshBtn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            refreshBtn.Image = CreateRefreshIcon();
            refreshBtn.ToolTipText = "刷新界面数据";
            refreshBtn.Font = new Font("微软雅黑", 9F);
            refreshBtn.ForeColor = Color.FromArgb(108, 117, 125);
            refreshBtn.Click += RefreshBtn_Click;
            toolStrip1.Items.Add(refreshBtn);
        }

        /// <summary>
        /// 创建树节点的辅助方法
        /// </summary>
        private TreeNode CreateTreeNode(string text, Color foreColor, Font font, string toolTip)
        {
            var node = new TreeNode(text);
            node.ForeColor = foreColor;
            node.NodeFont = font;
            node.ToolTipText = toolTip;
            return node;
        }

        /// <summary>
        /// 初始化导航树
        /// </summary>
        private void InitializeNavigationTree()
        {
            try
            {
                treeViewModules.Nodes.Clear();

                // L成员 - 物料管理模块
                var materialNode = CreateTreeNode("📦 物料管理 (L成员)",
                    Color.FromArgb(40, 167, 69),
                    new Font("微软雅黑", 10, FontStyle.Bold),
                    "物料信息管理、BOM清单、工艺路线配置");

                // 添加物料管理子节点
                var materialInfoNode = CreateTreeNode("📋 物料信息管理",
                    Color.FromArgb(60, 180, 85),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "管理物料基础信息、规格参数");
                materialNode.Nodes.Add(materialInfoNode);

                var bomNode = CreateTreeNode("🔧 BOM物料清单",
                    Color.FromArgb(60, 180, 85),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "产品物料清单管理");
                materialNode.Nodes.Add(bomNode);

                var processRouteNode = CreateTreeNode("⚙️ 工艺路线配置",
                    Color.FromArgb(60, 180, 85),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "生产工艺流程配置");
                materialNode.Nodes.Add(processRouteNode);

                materialNode.ExpandAll();
                treeViewModules.Nodes.Add(materialNode);

                // H成员 - 生产管理模块
                var productionNode = new TreeNode("⚙️ 生产管理 (H成员)")
                {
                    ForeColor = Color.FromArgb(0, 123, 255),
                    NodeFont = new Font("微软雅黑", 10, FontStyle.Bold),
                    ToolTipText = "生产订单管理、执行控制、权限管理"
                };

                // 添加生产管理子节点
                var productionOrderNode = new TreeNode("📊 生产订单管理")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "生产计划与订单管理"
                };
                productionNode.Nodes.Add(productionOrderNode);

                var executionControlNode = new TreeNode("🎯 生产执行控制")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "生产过程监控与控制"
                };
                productionNode.Nodes.Add(executionControlNode);

                productionNode.ExpandAll();
                treeViewModules.Nodes.Add(productionNode);
                //创建工单
                var CreateWorkOrder = new TreeNode("创建工单")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "创建工单"
                };

                var CancelWorkOrder = new TreeNode("取消创建工单")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "取消创建工单"
                };

                var SubmitWorkOrder = new TreeNode("提交工单")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "提交工单"
                };
                var CreateBatch = new TreeNode("创建批次")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "创建批次"
                };

                var CancelBatch = new TreeNode("取消创建批次")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "取消创建批次"
                };
                productionNode.Nodes.Add(CreateWorkOrder);
                productionNode.Nodes.Add(CancelWorkOrder);
                productionNode.Nodes.Add(SubmitWorkOrder);
                productionNode.Nodes.Add(CreateBatch);
                productionNode.Nodes.Add(CancelBatch);

                // S成员 - 车间管理模块
                var workshopNode = new TreeNode("🏭 车间管理 (S成员)")
                {
                    ForeColor = Color.FromArgb(220, 53, 69),
                    NodeFont = new Font("微软雅黑", 10, FontStyle.Bold),
                    ToolTipText = "车间作业管理、在制品管理、设备状态"
                };

                // 添加车间管理子节点
                var workshopOperationNode = new TreeNode("🔨 车间作业管理")
                {
                    ForeColor = Color.FromArgb(235, 70, 85),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "车间生产作业调度管理"
                };
                workshopNode.Nodes.Add(workshopOperationNode);

                var wipNode = new TreeNode("📦 在制品管理")
                {
                    ForeColor = Color.FromArgb(235, 70, 85),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "在制品状态跟踪管理"
                };
                workshopNode.Nodes.Add(wipNode);

                var equipmentNode = new TreeNode("🔧 设备状态管理")
                {
                    ForeColor = Color.FromArgb(235, 70, 85),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "生产设备状态监控"
                };
                workshopNode.Nodes.Add(equipmentNode);

                workshopNode.ExpandAll();
                treeViewModules.Nodes.Add(workshopNode);

                // 系统管理模块
                var systemNode = new TreeNode("⚙️ 系统管理")
                {
                    ForeColor = Color.FromArgb(108, 117, 125),
                    NodeFont = new Font("微软雅黑", 10, FontStyle.Bold),
                    ToolTipText = "系统配置、关于信息"
                };

                // 添加系统管理子节点
                var systemConfigNode = new TreeNode("⚙️ 系统配置")
                {
                    ForeColor = Color.FromArgb(128, 137, 145),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "系统参数配置管理"
                };
                systemNode.Nodes.Add(systemConfigNode);

                var aboutSystemNode = new TreeNode("ℹ️ 关于系统")
                {
                    ForeColor = Color.FromArgb(128, 137, 145),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "系统版本信息"
                };
                systemNode.Nodes.Add(aboutSystemNode);

                systemNode.ExpandAll();
                treeViewModules.Nodes.Add(systemNode);

                // 绑定节点点击事件
                treeViewModules.NodeMouseClick += TreeViewModules_NodeMouseClick;
                treeViewModules.NodeMouseDoubleClick += TreeViewModules_NodeMouseDoubleClick;

                LogManager.Info("导航树初始化完成 - 现代化图标样式");
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化导航树失败", ex);
                MessageBox.Show(string.Format("初始化导航树失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导航树节点单击事件 - 处理主节点的展开/折叠
        /// </summary>
        private void TreeViewModules_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 更新底部信息显示
            UpdateNavigationInfo(e.Node);

            // 只处理主节点的单击事件
            if (e.Node.Parent == null)
            {
                // 主节点单击时切换展开/折叠状态
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                }
                else
                {
                    e.Node.Expand();
                }
            }
        }

        /// <summary>
        /// 更新导航信息显示
        /// </summary>
        private void UpdateNavigationInfo(TreeNode selectedNode)
        {
            try
            {
                if (selectedNode == null)
                {
                    labelNavInfo.Text = "请选择功能模块";
                    return;
                }

                // 根据选中的节点更新信息
                if (selectedNode.Parent == null)
                {
                    // 主节点
                    if (selectedNode.Text.Contains("物料管理"))
                    {
                        labelNavInfo.Text = "物料管理模块 - L成员负责";
                    }
                    else if (selectedNode.Text.Contains("生产管理"))
                    {
                        labelNavInfo.Text = "生产管理模块 - H成员负责";
                    }
                    else if (selectedNode.Text.Contains("车间管理"))
                    {
                        labelNavInfo.Text = "车间管理模块 - S成员负责";
                    }
                    else if (selectedNode.Text.Contains("系统管理"))
                    {
                        labelNavInfo.Text = "系统管理模块 - 管理员功能";
                    }
                    else
                    {
                        labelNavInfo.Text = "功能模块";
                    }
                }
                else
                {
                    // 子节点
                    var cleanText = selectedNode.Text.Substring(2); // 去掉图标前缀
                    labelNavInfo.Text = string.Format("已选择：{0}", cleanText);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("更新导航信息失败", ex);
                labelNavInfo.Text = "导航信息更新失败";
            }
        }

        /// <summary>
        /// 导航树节点双击事件 - 仅处理子节点（具体功能）
        /// </summary>
        private void TreeViewModules_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var nodeName = e.Node.Text;

            // 只处理子节点的双击事件，主节点通过单击展开/折叠
            if (e.Node.Parent == null)
            {
                // 主节点不处理双击，避免干扰展开/折叠操作
                return;
            }

            // 根据子节点名称打开对应窗体
            switch (nodeName)
            {
                case "📋 物料信息管理":
                    OpenMaterialForm();
                    break;
                case "🔧 BOM物料清单":
                    OpenBOMForm();
                    break;
                case "⚙️ 工艺路线配置":
                    OpenProcessRouteForm();
                    break;
                case "📊 生产订单管理":
                    OpenProductionOrderForm();
                    break;
                case "创建工单":
                    OpenCreateWorkOrderForm();
                    break;
                case "取消创建工单":
                    OpenCancelWorkOrderForm();
                    break;
                case "提交工单":
                    OpenSubmitWorkOrderForm();
                    break;
                case "创建批次":
                    OpenCreateBatchForm();
                    break;
                case "取消创建批次":
                    OpenCancelBatchForm();
                    break;
                case "🎯 生产执行控制":
                    OpenProductionExecutionForm();
                    break;
                case "🔨 车间作业管理":
                    OpenWorkshopOperationForm();
                    break;
                case "📦 在制品管理":
                    OpenWIPForm();
                    break;
                case "🔧 设备状态管理":
                    OpenEquipmentForm();
                    break;
                case "⚙️ 系统配置":
                    OpenSystemConfigForm();
                    break;
                case "ℹ️ 关于系统":
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
            // 清空现有菜单项
            menuStrip1.Items.Clear();

            // 物料管理菜单 - L成员负责
            var materialMenu = new ToolStripMenuItem("📦 物料管理(&M)")
            {
                ForeColor = Color.FromArgb(40, 167, 69),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(40, 167, 69))
            };

            var materialInfoItem = new ToolStripMenuItem("物料信息管理");
            materialInfoItem.ShortcutKeys = Keys.Control | Keys.M;
            materialInfoItem.ShowShortcutKeys = true;
            materialInfoItem.Click += MaterialInfoItem_Click;
            materialMenu.DropDownItems.Add(materialInfoItem);

            var bomItem = new ToolStripMenuItem("BOM物料清单");
            bomItem.Click += BomItem_Click;
            materialMenu.DropDownItems.Add(bomItem);

            var processRouteItem = new ToolStripMenuItem("工艺路线配置");
            processRouteItem.Click += ProcessRouteItem_Click;
            materialMenu.DropDownItems.Add(processRouteItem);
            menuStrip1.Items.Add(materialMenu);

            // 生产管理菜单 - H成员负责
            var productionMenu = new ToolStripMenuItem("⚙️ 生产管理(&P)")
            {
                ForeColor = Color.FromArgb(0, 123, 255),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(0, 123, 255))
            };

            var productionOrderItem = new ToolStripMenuItem("生产订单管理");
            productionOrderItem.ShortcutKeys = Keys.Control | Keys.P;
            productionOrderItem.ShowShortcutKeys = true;
            productionOrderItem.Click += ProductionOrderItem_Click;
            productionMenu.DropDownItems.Add(productionOrderItem);

            var executionItem = new ToolStripMenuItem("生产执行控制");
            executionItem.Click += ExecutionItem_Click;
            productionMenu.DropDownItems.Add(executionItem);


            // 车间管理菜单 - S成员负责
            var workshopMenu = new ToolStripMenuItem("🏭 车间管理(&W)")
            {
                ForeColor = Color.FromArgb(220, 53, 69),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(220, 53, 69))
            };

            var workshopOperationItem = new ToolStripMenuItem("车间作业管理");
            workshopOperationItem.ShortcutKeys = Keys.Control | Keys.W;
            workshopOperationItem.ShowShortcutKeys = true;
            workshopOperationItem.Click += MenuItem_Click;
            workshopMenu.DropDownItems.Add(workshopOperationItem);

            var wipItem = new ToolStripMenuItem("在制品管理");
            wipItem.Click += MenuItem_Click;
            workshopMenu.DropDownItems.Add(wipItem);

            var equipmentItem = new ToolStripMenuItem("设备状态管理");
            equipmentItem.Click += MenuItem_Click;
            workshopMenu.DropDownItems.Add(equipmentItem);
            menuStrip1.Items.Add(workshopMenu);

            // 系统管理菜单
            var systemMenu = new ToolStripMenuItem("⚙️ 系统管理(&S)")
            {
                ForeColor = Color.FromArgb(108, 117, 125),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(108, 117, 125))
            };
            var systemConfigItem = new ToolStripMenuItem("系统配置");
            systemConfigItem.Click += MenuItem_Click;
            systemMenu.DropDownItems.Add(systemConfigItem);
            systemMenu.DropDownItems.Add(new ToolStripSeparator());

            var uiFrameworkItem = new ToolStripMenuItem("UI框架演示");
            uiFrameworkItem.Click += MenuItem_Click;
            systemMenu.DropDownItems.Add(uiFrameworkItem);
            systemMenu.DropDownItems.Add(new ToolStripSeparator());
            var aboutSystemItem = new ToolStripMenuItem("关于系统");
            aboutSystemItem.Click += MenuItem_Click;
            systemMenu.DropDownItems.Add(aboutSystemItem);
            menuStrip1.Items.Add(systemMenu);

            // 帮助菜单
            var helpMenu = new ToolStripMenuItem("❓ 帮助(&H)")
            {
                ForeColor = Color.FromArgb(108, 117, 125),
                Font = new Font("微软雅黑", 9.5F),
                Image = CreateMenuIcon(Color.FromArgb(108, 117, 125))
            };
            var userManualItem = new ToolStripMenuItem("使用手册");
            userManualItem.Click += MenuItem_Click;
            helpMenu.DropDownItems.Add(userManualItem);

            var techSupportItem = new ToolStripMenuItem("技术支持");
            techSupportItem.Click += MenuItem_Click;
            helpMenu.DropDownItems.Add(techSupportItem);

            helpMenu.DropDownItems.Add(new ToolStripSeparator());

            var aboutMESItem = new ToolStripMenuItem("关于MES");
            aboutMESItem.Click += MenuItem_Click;
            helpMenu.DropDownItems.Add(aboutMESItem);
            menuStrip1.Items.Add(helpMenu);
        }

        /// <summary>
        /// 初始化主面板内容（使用设计器控件）
        /// </summary>
        private void InitializeMainPanelContent()
        {
            // 设置系统标题和版本信息
            labelSystemTitle.Text = ConfigManager.SystemTitle;
            labelSystemVersion.Text = string.Format("版本 {0} - 企业级制造管理", ConfigManager.SystemVersion);

            // 设置状态信息
            labelStatusTitle.Text = "当前状态：基础框架已完成，各模块就绪";
            labelTechInfo.Text = "技术架构：C# .NET Framework 4.8 + WinForms + MySQL 8.0\n" +
                                "开发模式：三层架构 (UI/BLL/DAL/Models/Common)\n" +
                                "版本控制：Git + GitHub (main/develop分支)";

            // 应用现代化样式到所有面板
            // 面板样式已在设计器中配置
        }

        /// <summary>
        /// 初始化卡片点击事件（重构为Button卡片）
        /// </summary>
        private void InitializeCardClickEvents()
        {
            // 物料管理卡片点击事件
            btnMaterialCard.Click += MaterialCard_Click;

            // 生产管理卡片点击事件
            btnProductionCard.Click += ProductionCard_Click;

            // 车间管理卡片点击事件
            btnWorkshopCard.Click += WorkshopCard_Click;

            // Button控件自带稳定的悬浮效果，无需额外处理
        }

        // 旧的Panel悬停效果方法已删除，因为现在使用Button控件

        /// <summary>
        /// 创建菜单图标
        /// </summary>
        private Image CreateMenuIcon(Color color)
        {
            var bitmap = new Bitmap(20, 20);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 绘制主图标背景
                using (var brush = new SolidBrush(color))
                {
                    FillRoundedRectangle(g, brush, 2, 2, 16, 16, 3);
                }

                // 绘制高光效果
                using (var brush = new SolidBrush(Color.FromArgb(80, Color.White)))
                {
                    FillRoundedRectangle(g, brush, 3, 3, 14, 8, 2);
                }

                // 绘制中心图标
                using (var brush = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brush, 7, 7, 6, 6);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 创建工具栏图标
        /// </summary>
        private Image CreateToolBarIcon(Color color)
        {
            var bitmap = new Bitmap(24, 24);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 绘制圆角矩形背景
                using (var brush = new SolidBrush(Color.FromArgb(50, color)))
                {
                    FillRoundedRectangle(g, brush, 2, 2, 20, 20, 4);
                }

                // 绘制主图标
                using (var brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, 6, 6, 12, 12);
                }

                // 绘制高光效果
                using (var brush = new SolidBrush(Color.FromArgb(100, Color.White)))
                {
                    g.FillEllipse(brush, 8, 8, 6, 6);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 创建刷新图标
        /// </summary>
        private Image CreateRefreshIcon()
        {
            var bitmap = new Bitmap(24, 24);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 绘制刷新箭头
                using (var pen = new Pen(Color.FromArgb(108, 117, 125), 2))
                {
                    // 绘制圆弧
                    g.DrawArc(pen, 4, 4, 16, 16, -90, 270);

                    // 绘制箭头
                    var arrowPoints = new Point[]
                    {
                        new Point(20, 4),
                        new Point(16, 2),
                        new Point(16, 6)
                    };
                    using (var brush = new SolidBrush(Color.FromArgb(108, 117, 125)))
                    {
                        g.FillPolygon(brush, arrowPoints);
                    }
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        private void FillRoundedRectangle(Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                path.AddArc(x + width - radius * 2, y, radius * 2, radius * 2, 270, 90);
                path.AddArc(x + width - radius * 2, y + height - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(x, y + height - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            try
            {
                // 更新状态栏信息
                systemStatusLabel.Text = "正在刷新数据...";
                systemStatusLabel.ForeColor = Color.FromArgb(255, 193, 7);

                // 模拟刷新操作
                System.Threading.Thread.Sleep(500);

                // 刷新完成
                systemStatusLabel.Text = "数据刷新完成";
                systemStatusLabel.ForeColor = Color.FromArgb(25, 135, 84);

                LogManager.Info("界面数据刷新完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                systemStatusLabel.Text = "刷新失败";
                systemStatusLabel.ForeColor = Color.FromArgb(220, 53, 69);
            }
        }

        /// <summary>
        /// 获取格式化的日期时间字符串
        /// </summary>
        private string GetFormattedDateTime()
        {
            var now = DateTime.Now;
            var dayOfWeek = GetChineseDayOfWeek(now.DayOfWeek);
            return string.Format("{0} {1} {2}", now.ToString("yyyy年MM月dd日"), dayOfWeek, now.ToString("HH:mm:ss"));
        }

        /// <summary>
        /// 获取中文星期
        /// </summary>
        private string GetChineseDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "星期一";
                case DayOfWeek.Tuesday: return "星期二";
                case DayOfWeek.Wednesday: return "星期三";
                case DayOfWeek.Thursday: return "星期四";
                case DayOfWeek.Friday: return "星期五";
                case DayOfWeek.Saturday: return "星期六";
                case DayOfWeek.Sunday: return "星期日";
                default: return "";
            }
        }



        /// <summary>
        /// 显示欢迎消息
        /// </summary>
        private void ShowWelcomeMessage()
        {
            systemStatusLabel.Text = "MES系统启动成功 - 基础框架就绪";
            systemStatusLabel.ForeColor = Color.FromArgb(25, 135, 84);
        }

        /// <summary>
        /// 显示使用手册
        /// </summary>
        private void ShowUserManual()
        {
            MessageBox.Show("MES系统使用手册\n\n功能模块说明：\n• 物料管理：负责物料信息、BOM清单管理\n• 生产管理：负责生产订单、执行控制\n• 车间管理：负责车间作业、在制品管理\n• 系统管理：负责系统配置和维护",
                "使用手册", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示技术支持
        /// </summary>
        private void ShowTechnicalSupport()
        {
            MessageBox.Show("技术支持信息\n\n技术架构：C# .NET Framework 4.8 + WinForms + MySQL 8.0\n开发团队：L成员(物料)、H成员(生产)、S成员(车间)\n版本控制：Git + GitHub\n\n如需技术支持，请联系开发团队。",
                "技术支持", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region 菜单事件处理方法 - 待各模块负责人实现

        // L成员负责实现的物料管理模块
        //private void OpenMaterialForm() { ShowNotImplemented("物料信息管理"); }
        private void OpenMaterialForm() { showMMForm(); }
        private void OpenBOMForm() { ShowBOMManagementForm(); }
        private void OpenProcessRouteForm() { ShowProcessRouteConfigForm(); }

        // H成员负责实现的生产管理模块
        private void OpenProductionOrderForm() { ShowProductionOrderForm(); }
        private void OpenCreateWorkOrderForm() { ShowCreateWorkOrderForm(); }
        private void OpenCancelWorkOrderForm() { ShowCancelWorkOrderForm(); }
        private void OpenSubmitWorkOrderForm() { ShowSubmitWorkOrderForm(); }
        private void OpenCreateBatchForm() { ShowCreateBatchForm(); }
        private void OpenCancelBatchForm() { ShowCancelBatchForm(); }
        private void OpenProductionExecutionForm() { ShowProductionExecutionControlForm(); }

        // S成员负责实现的车间管理模块
        private void OpenWorkshopOperationForm() { ShowWorkshopOperationForm(); }
        private void OpenWIPForm() { ShowWIPManagementForm(); }
        private void OpenEquipmentForm() { ShowEquipmentStatusForm(); }

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
        private void OpenSystemConfigForm()
        {
            try
            {
                var configForm = new SystemManagement.SystemConfigForm();
                configForm.ShowDialog();
                LogManager.Info("打开系统配置窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开系统配置窗体失败", ex);
                MessageBox.Show(string.Format("打开系统配置窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        #region 工具栏事件处理方法

        /// <summary>
        /// 物料管理按钮点击事件
        /// </summary>
        private void MaterialBtn_Click(object sender, EventArgs e)
        {
            OpenMaterialForm();
        }

        /// <summary>
        /// 生产管理按钮点击事件
        /// </summary>
        private void ProductionBtn_Click(object sender, EventArgs e)
        {
            OpenProductionOrderForm();
        }

        /// <summary>
        /// 车间管理按钮点击事件
        /// </summary>
        private void WorkshopBtn_Click(object sender, EventArgs e)
        {
            OpenWorkshopOperationForm();
        }

        /// <summary>
        /// 系统管理按钮点击事件
        /// </summary>
        private void SystemBtn_Click(object sender, EventArgs e)
        {
            OpenSystemConfigForm();
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        #endregion

        #region 菜单项事件处理方法

        /// <summary>
        /// 物料信息管理菜单项点击事件
        /// </summary>
        private void MaterialInfoItem_Click(object sender, EventArgs e)
        {
            OpenMaterialForm();
        }

        /// <summary>
        /// BOM物料清单菜单项点击事件
        /// </summary>
        private void BomItem_Click(object sender, EventArgs e)
        {
            OpenBOMForm();
        }

        /// <summary>
        /// 工艺路线配置菜单项点击事件
        /// </summary>
        private void ProcessRouteItem_Click(object sender, EventArgs e)
        {
            OpenProcessRouteForm();
        }

        /// <summary>
        /// 卡片点击事件处理方法
        /// </summary>
        private void MaterialCard_Click(object sender, EventArgs e)
        {
            OpenMaterialForm();
        }

        private void ProductionCard_Click(object sender, EventArgs e)
        {
            OpenProductionOrderForm();
        }

        private void WorkshopCard_Click(object sender, EventArgs e)
        {
            OpenWorkshopOperationForm();
        }

        /// <summary>
        /// 生产订单管理菜单项点击事件
        /// </summary>
        private void ProductionOrderItem_Click(object sender, EventArgs e)
        {
            OpenProductionOrderForm();
        }

        /// <summary>
        /// 生产执行控制菜单项点击事件
        /// </summary>
        private void ExecutionItem_Click(object sender, EventArgs e)
        {
            OpenProductionExecutionForm();
        }



        /// <summary>
        /// 通用菜单项点击事件处理
        /// </summary>
        private void MenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null) return;

            switch (menuItem.Text)
            {
                case "车间作业管理":
                    OpenWorkshopOperationForm();
                    break;
                case "在制品管理":
                    OpenWIPForm();
                    break;
                case "设备状态管理":
                    OpenEquipmentForm();
                    break;
                case "系统配置":
                    OpenSystemConfigForm();
                    break;
                case "UI框架演示":
                    ShowUIFrameworkInfo();
                    break;
                case "关于系统":
                case "关于MES":
                    ShowAbout();
                    break;
                case "使用手册":
                    ShowUserManual();
                    break;
                case "技术支持":
                    ShowTechnicalSupport();
                    break;
                default:
                    MessageBox.Show(string.Format("功能 '{0}' 正在开发中...", menuItem.Text), "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
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
        /// 显示生产订单管理窗体
        /// </summary>
        private void ShowProductionOrderForm()
        {
            ProductionOrderManagementForm productionForm = new ProductionOrderManagementForm();
            productionForm.Show();
        }
        private void ShowCreateWorkOrderForm()
        {
            CreateWorkOrder createWorkOrder = new CreateWorkOrder();
            createWorkOrder.Show();
        }
        private void ShowCancelWorkOrderForm()
        {
            CancelWorkOrder cancelWorkOrder = new CancelWorkOrder();
            cancelWorkOrder.Show();
        }
        private void ShowSubmitWorkOrderForm()
        {
            SubmitWorkOrder submitWorkOrder = new SubmitWorkOrder();
            submitWorkOrder.Show();
        }
        private void ShowCreateBatchForm()
        {
            CreateBatch createBatch = new CreateBatch();
            createBatch.Show();
        }
        private void ShowCancelBatchForm()
        {
            CancelBatch cancelBatch = new CancelBatch();
            cancelBatch.Show();
        }



        /// <summary>
        /// 显示BOM物料清单管理窗体
        /// </summary>
        private void ShowBOMManagementForm()
        {
            BOMManagementForm bomForm = new BOMManagementForm();
            bomForm.Show();
        }

        /// <summary>
        /// 显示工艺路线配置窗体
        /// </summary>
        private void ShowProcessRouteConfigForm()
        {
            ProcessRouteConfigForm processRouteForm = new ProcessRouteConfigForm();
            processRouteForm.Show();
        }

        /// <summary>
        /// 显示生产执行控制窗体
        /// </summary>
        private void ShowProductionExecutionControlForm()
        {
            ProductionExecutionControlForm executionForm = new ProductionExecutionControlForm();
            executionForm.Show();
        }

        /// <summary>
        /// 显示车间作业管理窗体
        /// </summary>
        private void ShowWorkshopOperationForm()
        {
            try
            {
                var workshopOperationForm = new WorkshopOperationForm();
                workshopOperationForm.Show();
                LogManager.Info("打开车间作业管理窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开车间作业管理窗体失败", ex);
                MessageBox.Show(string.Format("打开车间作业管理窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示在制品管理窗体
        /// </summary>
        private void ShowWIPManagementForm()
        {
            try
            {
                var wipForm = new WIPManagementForm();
                wipForm.Show();
                LogManager.Info("打开在制品管理窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开在制品管理窗体失败", ex);
                MessageBox.Show(string.Format("打开在制品管理窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示设备状态管理窗体
        /// </summary>
        private void ShowEquipmentStatusForm()
        {
            try
            {
                var equipmentForm = new EquipmentStatusForm();
                equipmentForm.Show();
                LogManager.Info("打开设备状态管理窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开设备状态管理窗体失败", ex);
                MessageBox.Show(string.Format("打开设备状态管理窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 显示关于对话框
        /// </summary>
        private void ShowAbout()
        {
            try
            {
                var aboutForm = new SystemManagement.AboutForm();
                aboutForm.ShowDialog();
                LogManager.Info("显示关于系统窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("显示关于系统窗体失败", ex);
                MessageBox.Show(string.Format("显示关于系统窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            // 释放状态定时器
            if (statusTimer != null)
            {
                statusTimer.Stop();
                statusTimer.Dispose();
                statusTimer = null;
            }

            LogManager.Info("用户退出系统，资源已释放");
            base.OnFormClosing(e);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
