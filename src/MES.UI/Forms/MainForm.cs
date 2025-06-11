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
using MES.UI.Framework.Themes;

namespace MES.UI.Forms
{
    /// <summary>
    /// MES系统主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        private Timer statusTimer;
        private Timer animationTimer;
        private ToolStripStatusLabel currentUserLabel;
        private ToolStripStatusLabel systemStatusLabel;
        private float animationProgress = 0f;
        private LeagueAnimationManager animationManager;

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
                // 【英雄联盟主题应用】- 主界面美化
                ApplyLeagueThemeToMainForm();

                // 设置窗体属性
                this.WindowState = FormWindowState.Maximized;
                this.Text = string.Format("{0} v{1} - 英雄联盟风格版", ConfigManager.SystemTitle, ConfigManager.SystemVersion);
                this.Icon = SystemIcons.Application;

                // 启用自定义绘制
                EnableLeagueCustomPainting();

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

                // 初始化卡片特效事件
                InitializeCardEffectEvents();

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
                var materialNode = CreateTreeNode("物料管理 (L成员)",
                    Color.FromArgb(40, 167, 69),
                    new Font("微软雅黑", 10, FontStyle.Bold),
                    "物料信息管理、BOM清单、工艺路线配置");

                // 添加物料管理子节点
                var materialInfoNode = CreateTreeNode("物料信息管理",
                    Color.FromArgb(60, 180, 85),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "管理物料基础信息、规格参数");
                materialNode.Nodes.Add(materialInfoNode);

                var bomNode = CreateTreeNode("BOM物料清单",
                    Color.FromArgb(60, 180, 85),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "产品物料清单管理");
                materialNode.Nodes.Add(bomNode);



                var processRouteNode = CreateTreeNode("工艺路线配置",
                    Color.FromArgb(60, 180, 85),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "生产工艺流程配置");
                materialNode.Nodes.Add(processRouteNode);

                materialNode.ExpandAll();
                treeViewModules.Nodes.Add(materialNode);

                // H成员 - 生产管理模块
                var productionNode = new TreeNode("生产管理 (H成员)")
                {
                    ForeColor = Color.FromArgb(0, 123, 255),
                    NodeFont = new Font("微软雅黑", 10, FontStyle.Bold),
                    ToolTipText = "生产订单管理、执行控制"
                };

                // 添加生产管理子节点
                var productionOrderNode = new TreeNode("生产订单管理")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "生产计划与订单管理"
                };
                productionNode.Nodes.Add(productionOrderNode);

                var executionControlNode = new TreeNode("生产执行控制")
                {
                    ForeColor = Color.FromArgb(20, 140, 255),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "生产过程监控与控制"
                };
                productionNode.Nodes.Add(executionControlNode);

                // 添加工单管理子节点
                var workOrderManagementNode = CreateTreeNode("工单管理",
                    Color.FromArgb(20, 140, 255),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "工单创建、提交、取消等管理");
                productionNode.Nodes.Add(workOrderManagementNode);

                // 添加批次管理子节点
                var batchManagementNode = CreateTreeNode("批次管理",
                    Color.FromArgb(20, 140, 255),
                    new Font("微软雅黑", 9, FontStyle.Regular),
                    "批次创建、取消等管理");
                productionNode.Nodes.Add(batchManagementNode);

                productionNode.ExpandAll();
                treeViewModules.Nodes.Add(productionNode);

                // S成员 - 车间管理模块
                var workshopNode = new TreeNode("车间管理 (S成员)")
                {
                    ForeColor = Color.FromArgb(220, 53, 69),
                    NodeFont = new Font("微软雅黑", 10, FontStyle.Bold),
                    ToolTipText = "车间作业管理、在制品管理、设备状态"
                };

                // 添加车间管理子节点
                var workshopOperationNode = new TreeNode("车间作业管理")
                {
                    ForeColor = Color.FromArgb(235, 70, 85),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "车间生产作业调度管理"
                };
                workshopNode.Nodes.Add(workshopOperationNode);

                var wipNode = new TreeNode("在制品管理")
                {
                    ForeColor = Color.FromArgb(235, 70, 85),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "在制品状态跟踪管理"
                };
                workshopNode.Nodes.Add(wipNode);

                var equipmentNode = new TreeNode("设备状态管理")
                {
                    ForeColor = Color.FromArgb(235, 70, 85),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "生产设备状态监控"
                };
                workshopNode.Nodes.Add(equipmentNode);

                workshopNode.ExpandAll();
                treeViewModules.Nodes.Add(workshopNode);

                // 系统管理模块
                var systemNode = new TreeNode("系统管理")
                {
                    ForeColor = Color.FromArgb(108, 117, 125),
                    NodeFont = new Font("微软雅黑", 10, FontStyle.Bold),
                    ToolTipText = "系统配置、关于信息"
                };

                // 添加系统管理子节点
                var systemConfigNode = new TreeNode("系统配置")
                {
                    ForeColor = Color.FromArgb(128, 137, 145),
                    NodeFont = new Font("微软雅黑", 9, FontStyle.Regular),
                    ToolTipText = "系统参数配置管理"
                };
                systemNode.Nodes.Add(systemConfigNode);

                var aboutSystemNode = new TreeNode("关于系统")
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

            // 添加调试日志
            LogManager.Info(string.Format("TreeView节点双击: '{0}' (长度: {1})", nodeName, nodeName.Length));

            // 直接根据节点文本进行精确匹配，移除emoji前缀
            var cleanText = nodeName;
            if (cleanText.Length > 2 && (cleanText[0] > 127 || cleanText[1] == ' '))
            {
                // 移除emoji和空格前缀
                var spaceIndex = cleanText.IndexOf(' ');
                if (spaceIndex > 0)
                {
                    cleanText = cleanText.Substring(spaceIndex + 1);
                }
            }

            switch (cleanText)
            {
                case "物料信息管理":
                    OpenMaterialForm();
                    break;
                case "BOM物料清单":
                    OpenBOMForm();
                    break;
                case "工艺路线配置":
                    OpenProcessRouteForm();
                    break;
                case "生产订单管理":
                    OpenProductionOrderForm();
                    break;
                case "工单管理":
                    OpenWorkOrderManagementForm();
                    break;
                case "批次管理":
                    OpenBatchManagementForm();
                    break;
                case "生产执行控制":
                    OpenProductionExecutionForm();
                    break;
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
                case "数据库诊断":
                    OpenDatabaseDiagnosticForm();
                    break;
                case "关于系统":
                    ShowAbout();
                    break;
                default:
                    MessageBox.Show(string.Format("功能 '{0}' 暂未配置具体操作", cleanText), "提示",
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
            var materialMenu = new ToolStripMenuItem("物料管理(&M)")
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
            var productionMenu = new ToolStripMenuItem("生产管理(&P)")
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
            var workshopMenu = new ToolStripMenuItem("车间管理(&W)")
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
            var systemMenu = new ToolStripMenuItem("系统管理(&S)")
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
        private void OpenWorkOrderManagementForm() { ShowWorkOrderManagementForm(); }
        private void OpenBatchManagementForm() { ShowBatchManagementForm(); }
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

主要特性：
• 3种预设主题（默认/蓝色/深色）
• 统一的界面风格和组件库
• 现代化的用户体验设计
• 模块化架构，易于扩展

项目状态：已完成
质量评级：优秀

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
                    MessageBox.Show(string.Format("功能 '{0}' 暂未配置具体操作", menuItem.Text), "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 显示功能未配置提示
        /// </summary>
        private void ShowNotImplemented(string functionName)
        {
            MessageBox.Show(string.Format("{0}功能暂未配置具体操作", functionName), "提示",
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
            try
            {
                ProcessRouteConfigForm processRouteForm = new ProcessRouteConfigForm();
                processRouteForm.Show();
                LogManager.Info("打开工艺路线配置窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开工艺路线配置窗体失败", ex);
                MessageBox.Show(string.Format("打开工艺路线配置窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// 显示工单管理窗体
        /// </summary>
        private void ShowWorkOrderManagementForm()
        {
            try
            {
                var workOrderForm = new WorkOrder.WorkOrderManagementForm();
                workOrderForm.Show();
                LogManager.Info("打开工单管理统一窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开工单管理窗体失败", ex);
                MessageBox.Show(string.Format("打开工单管理窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示批次管理窗体
        /// </summary>
        private void ShowBatchManagementForm()
        {
            try
            {
                var batchForm = new Batch.BatchManagementForm();
                batchForm.Show();
                LogManager.Info("打开批次管理统一窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开批次管理窗体失败", ex);
                MessageBox.Show(string.Format("打开批次管理窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// 打开数据库诊断工具
        /// </summary>
        private void OpenDatabaseDiagnosticForm()
        {
            try
            {
                var diagnosticForm = new MES.UI.Forms.SystemManagement.DatabaseDiagnosticForm();
                diagnosticForm.ShowDialog();
                LogManager.Info("打开数据库诊断工具");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开数据库诊断工具失败", ex);
                MessageBox.Show(string.Format("打开数据库诊断工具失败：{0}", ex.Message), "错误",
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
            // 添加英雄联盟主题测试按钮到工具栏
            AddLeagueThemeTestButton();
        }

        /// <summary>
        /// 添加英雄联盟主题测试按钮
        /// </summary>
        private void AddLeagueThemeTestButton()
        {
            try
            {
                // 在工具栏添加测试按钮
                var testButton = new ToolStripButton
                {
                    Text = "英雄联盟主题测试",
                    ToolTipText = "打开英雄联盟主题特效测试窗体",
                    Font = new Font("微软雅黑", 9F, FontStyle.Bold)
                };
                testButton.Click += (s, e) => OpenLeagueThemeTestForm();

                // 添加到工具栏
                if (this.Controls.Find("toolStrip1", true).Length > 0)
                {
                    var toolStrip = this.Controls.Find("toolStrip1", true)[0] as ToolStrip;
                    if (toolStrip != null)
                    {
                        toolStrip.Items.Add(testButton);
                    }
                }

                LogManager.Info("英雄联盟主题测试按钮已添加到工具栏");
            }
            catch (Exception ex)
            {
                LogManager.Error("添加英雄联盟主题测试按钮失败", ex);
            }
        }

        /// <summary>
        /// 打开英雄联盟主题测试窗体
        /// </summary>
        private void OpenLeagueThemeTestForm()
        {
            try
            {
                var testForm = new SystemManagement.LeagueThemeTestForm();
                testForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                LogManager.Error("打开英雄联盟主题测试窗体失败", ex);
                MessageBox.Show("打开测试窗体失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 【英雄联盟主题应用】- 将英雄联盟风格应用到主界面
        /// </summary>
        private void ApplyLeagueThemeToMainForm()
        {
            try
            {
                // 获取英雄联盟主题配色
                var leagueTheme = UIThemeManager.GetLeagueTheme();

                // 应用主窗体背景色 - 英雄联盟深色背景
                this.BackColor = leagueTheme.Background;
                this.ForeColor = leagueTheme.Text;

                // 【重点改造】应用英雄联盟布局风格
                ApplyLeagueLayoutStyle();

                // 递归应用英雄联盟主题到所有控件
                ApplyLeagueThemeToControls(this.Controls, leagueTheme);

                LogManager.Info("英雄联盟主题已成功应用到主界面");
            }
            catch (Exception ex)
            {
                LogManager.Error("应用英雄联盟主题到主界面失败", ex);
                // 如果主题应用失败，保持原有样式，不影响系统功能
                this.BackColor = Color.FromArgb(248, 249, 250);
            }
        }

        /// <summary>
        /// 应用英雄联盟布局风格 - 核心布局改造
        /// </summary>
        private void ApplyLeagueLayoutStyle()
        {
            try
            {
                // 1. 改造左侧导航面板为LOL风格
                TransformNavigationPanelToLeagueStyle();

                // 2. 改造主面板为LOL卡片布局
                TransformMainPanelToLeagueStyle();

                // 3. 改造菜单栏和工具栏
                TransformMenuAndToolbarToLeagueStyle();

                // 4. 添加LOL特色装饰元素
                AddLeagueDecorationElements();

                LogManager.Info("英雄联盟布局风格应用完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("应用英雄联盟布局风格失败", ex);
            }
        }

        /// <summary>
        /// 递归应用英雄联盟主题到控件集合
        /// </summary>
        private void ApplyLeagueThemeToControls(Control.ControlCollection controls, UIThemeManager.ThemeColors colors)
        {
            foreach (Control control in controls)
            {
                ApplyLeagueThemeToControl(control, colors);

                // 递归处理子控件
                if (control.HasChildren)
                {
                    ApplyLeagueThemeToControls(control.Controls, colors);
                }
            }
        }

        /// <summary>
        /// 应用英雄联盟主题到单个控件 - 升级版
        /// </summary>
        private void ApplyLeagueThemeToControl(Control control, UIThemeManager.ThemeColors colors)
        {
            if (control == null) return;

            try
            {
                // 根据控件类型应用英雄联盟风格 - C# 5.0兼容语法
                if (control is MenuStrip)
                {
                    var menuStrip = (MenuStrip)control;
                    ConvertToLeagueMenuStrip(menuStrip, colors);
                }
                else if (control is StatusStrip)
                {
                    var statusStrip = (StatusStrip)control;
                    ConvertToLeagueStatusStrip(statusStrip, colors);
                }
                else if (control is ToolStrip)
                {
                    var toolStrip = (ToolStrip)control;
                    ConvertToLeagueToolStrip(toolStrip, colors);
                }
                else if (control is Panel)
                {
                    var panel = (Panel)control;
                    ConvertToLeaguePanel(panel, colors);
                }
                else if (control is Button)
                {
                    var button = (Button)control;
                    ConvertToLeagueButton(button, colors);
                }
                else if (control is TextBox)
                {
                    var textBox = (TextBox)control;
                    ConvertToLeagueTextBox(textBox, colors);
                }
                else if (control is Label)
                {
                    var label = (Label)control;
                    ConvertToLeagueLabel(label, colors);
                }
                else if (control is TreeView)
                {
                    var treeView = (TreeView)control;
                    ConvertToLeagueTreeView(treeView, colors);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("应用英雄联盟主题到控件失败: {0}", control.GetType().Name), ex);
            }
        }

        /// <summary>
        /// 应用英雄联盟主题到菜单项
        /// </summary>
        private void ApplyLeagueThemeToMenuItems(ToolStripItemCollection items, UIThemeManager.ThemeColors colors)
        {
            foreach (ToolStripItem item in items)
            {
                item.BackColor = colors.Surface;
                item.ForeColor = colors.Text;

                // C# 5.0兼容语法
                if (item is ToolStripMenuItem)
                {
                    var menuItem = (ToolStripMenuItem)item;
                    if (menuItem.HasDropDownItems)
                    {
                        ApplyLeagueThemeToMenuItems(menuItem.DropDownItems, colors);
                    }
                }
            }
        }

        #region 英雄联盟风格控件转换方法

        /// <summary>
        /// 转换菜单栏为英雄联盟风格
        /// </summary>
        private void ConvertToLeagueMenuStrip(MenuStrip menuStrip, UIThemeManager.ThemeColors colors)
        {
            menuStrip.BackColor = Color.Transparent;
            menuStrip.ForeColor = colors.Text;
            menuStrip.Paint += (s, e) => LeagueVisualEffects.DrawLeagueMenuBar(e.Graphics, menuStrip.ClientRectangle);
            ApplyLeagueThemeToMenuItems(menuStrip.Items, colors);
        }

        /// <summary>
        /// 转换状态栏为英雄联盟风格 - 增强版
        /// </summary>
        private void ConvertToLeagueStatusStrip(StatusStrip statusStrip, UIThemeManager.ThemeColors colors)
        {
            statusStrip.BackColor = Color.Transparent;
            statusStrip.ForeColor = LeagueColors.TextPrimary;

            // 自定义绘制状态栏
            statusStrip.Paint += (s, e) =>
            {
                var bounds = statusStrip.ClientRectangle;

                // 绘制渐变背景
                using (var brush = new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(15, 20, 30),
                    Color.FromArgb(25, 30, 40),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                }

                // 绘制顶部双金色边框
                using (var pen = new Pen(LeagueColors.PrimaryGold, 2))
                {
                    e.Graphics.DrawLine(pen, 0, 0, statusStrip.Width, 0);
                }
                using (var pen = new Pen(Color.FromArgb(120, LeagueColors.PrimaryGoldLight), 1))
                {
                    e.Graphics.DrawLine(pen, 0, 2, statusStrip.Width, 2);
                }

                // 添加角落装饰
                var accentSize = 6;
                using (var brush = new SolidBrush(Color.FromArgb(150, LeagueColors.PrimaryGold)))
                {
                    // 左下角
                    var leftBottom = new Point[] {
                        new Point(0, bounds.Bottom - accentSize),
                        new Point(0, bounds.Bottom),
                        new Point(accentSize, bounds.Bottom)
                    };
                    e.Graphics.FillPolygon(brush, leftBottom);

                    // 右下角
                    var rightBottom = new Point[] {
                        new Point(bounds.Right - accentSize, bounds.Bottom),
                        new Point(bounds.Right, bounds.Bottom),
                        new Point(bounds.Right, bounds.Bottom - accentSize)
                    };
                    e.Graphics.FillPolygon(brush, rightBottom);
                }

                // 添加中央装饰线
                var centerY = bounds.Height / 2;
                using (var pen = new Pen(Color.FromArgb(60, LeagueColors.PrimaryGold), 1))
                {
                    e.Graphics.DrawLine(pen, 20, centerY, bounds.Width - 20, centerY);
                }
            };
        }

        /// <summary>
        /// 转换工具栏为英雄联盟风格
        /// </summary>
        private void ConvertToLeagueToolStrip(ToolStrip toolStrip, UIThemeManager.ThemeColors colors)
        {
            toolStrip.BackColor = Color.Transparent;
            toolStrip.ForeColor = colors.Text;
            toolStrip.Paint += (s, e) =>
            {
                // 绘制渐变背景
                using (var brush = new LinearGradientBrush(
                    toolStrip.ClientRectangle,
                    LeagueColors.DarkSurface,
                    LeagueColors.DarkBackground,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, toolStrip.ClientRectangle);
                }

                // 绘制底部分隔线
                using (var pen = new Pen(LeagueColors.PrimaryGold, 1))
                {
                    e.Graphics.DrawLine(pen, 0, toolStrip.Height - 1, toolStrip.Width, toolStrip.Height - 1);
                }
            };
        }

        /// <summary>
        /// 转换面板为英雄联盟风格
        /// </summary>
        private void ConvertToLeaguePanel(Panel panel, UIThemeManager.ThemeColors colors)
        {
            panel.BackColor = Color.Transparent;
            panel.Paint += (s, e) => LeagueVisualEffects.DrawLeaguePanel(e.Graphics, panel.ClientRectangle);
        }

        /// <summary>
        /// 转换按钮为英雄联盟风格
        /// </summary>
        private void ConvertToLeagueButton(Button button, UIThemeManager.ThemeColors colors)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.Transparent;
            button.ForeColor = LeagueColors.TextPrimary;
            button.Font = new Font("微软雅黑", button.Font.Size, FontStyle.Bold);

            // 添加英雄联盟风格绘制
            button.Paint += (s, e) =>
            {
                var btn = s as Button;
                if (btn == null) return;

                var isHovered = btn.ClientRectangle.Contains(btn.PointToClient(Cursor.Position));
                var isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

                LeagueVisualEffects.DrawLeagueButton(e.Graphics, btn.ClientRectangle, isHovered, isPressed, btn.Text, btn.Font);
            };

            // 添加重绘事件
            button.MouseEnter += (s, e) => button.Invalidate();
            button.MouseLeave += (s, e) => button.Invalidate();
            button.MouseDown += (s, e) => button.Invalidate();
            button.MouseUp += (s, e) => button.Invalidate();
        }

        /// <summary>
        /// 转换文本框为英雄联盟风格
        /// </summary>
        private void ConvertToLeagueTextBox(TextBox textBox, UIThemeManager.ThemeColors colors)
        {
            textBox.BackColor = LeagueColors.DarkSurface;
            textBox.ForeColor = colors.Text;
            textBox.BorderStyle = BorderStyle.None;

            // 添加自定义边框
            textBox.Paint += (s, e) =>
            {
                using (var pen = new Pen(LeagueColors.PrimaryGold, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, textBox.Width - 1, textBox.Height - 1);
                }
            };
        }

        /// <summary>
        /// 转换标签为英雄联盟风格
        /// </summary>
        private void ConvertToLeagueLabel(Label label, UIThemeManager.ThemeColors colors)
        {
            label.ForeColor = colors.Text;
            label.BackColor = Color.Transparent;

            // 为重要标签添加金色
            if (label.Font.Bold || label.Text.Contains("MES") || label.Text.Contains("系统"))
            {
                label.ForeColor = LeagueColors.TextGold;
            }
        }

        /// <summary>
        /// 转换树视图为英雄联盟风格 - 增强版
        /// </summary>
        private void ConvertToLeagueTreeView(TreeView treeView, UIThemeManager.ThemeColors colors)
        {
            treeView.BackColor = LeagueColors.DarkSurface;
            treeView.ForeColor = LeagueColors.TextPrimary;
            treeView.BorderStyle = BorderStyle.None;
            treeView.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            treeView.ItemHeight = 28; // 增加行高
            treeView.HideSelection = false;
            treeView.FullRowSelect = true;
            treeView.ShowLines = false;
            treeView.ShowPlusMinus = true;
            treeView.ShowRootLines = false;

            // 自定义绘制
            treeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            treeView.DrawNode += TreeView_DrawNode;

            // 添加自定义边框和背景
            treeView.Paint += (s, e) =>
            {
                // 绘制渐变背景
                using (var brush = new LinearGradientBrush(
                    treeView.ClientRectangle,
                    LeagueColors.DarkBackground,
                    LeagueColors.DarkSurface,
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, treeView.ClientRectangle);
                }

                // 绘制金色边框
                using (var pen = new Pen(LeagueColors.PrimaryGold, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, treeView.Width - 1, treeView.Height - 1);
                }

                // 绘制内边框
                using (var pen = new Pen(Color.FromArgb(50, LeagueColors.PrimaryGoldLight), 1))
                {
                    e.Graphics.DrawRectangle(pen, 1, 1, treeView.Width - 3, treeView.Height - 3);
                }
            };
        }

        /// <summary>
        /// 树视图节点自定义绘制
        /// </summary>
        private void TreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var treeView = sender as TreeView;
            if (treeView == null) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 确定节点状态
            bool isSelected = (e.State & TreeNodeStates.Selected) != 0;
            bool isHovered = (e.State & TreeNodeStates.Hot) != 0;
            bool isExpanded = e.Node.IsExpanded;

            // 绘制节点背景
            DrawTreeNodeBackground(e.Graphics, e.Bounds, isSelected, isHovered);

            // 绘制展开/折叠图标
            if (e.Node.Nodes.Count > 0)
            {
                DrawTreeNodeExpandIcon(e.Graphics, e.Bounds, isExpanded);
            }

            // 绘制节点图标（如果有）
            DrawTreeNodeIcon(e.Graphics, e.Bounds, e.Node);

            // 绘制节点文字
            DrawTreeNodeText(e.Graphics, e.Bounds, e.Node.Text, isSelected, isHovered);
        }

        /// <summary>
        /// 绘制树节点背景 - 精细化版本
        /// </summary>
        private void DrawTreeNodeBackground(Graphics g, Rectangle bounds, bool isSelected, bool isHovered)
        {
            if (isSelected)
            {
                // 选中状态 - 精致的金色效果
                using (var brush = new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(40, LeagueColors.PrimaryGold),
                    Color.FromArgb(20, LeagueColors.PrimaryGoldDark),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, bounds);
                }

                // 精致的左侧金色边框
                using (var brush = new LinearGradientBrush(
                    new Rectangle(bounds.X, bounds.Y, 3, bounds.Height),
                    LeagueColors.TextGold,
                    LeagueColors.PrimaryGoldDark,
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, bounds.X, bounds.Y, 3, bounds.Height);
                }

                // 右侧细线装饰
                using (var pen = new Pen(Color.FromArgb(80, LeagueColors.PrimaryGold), 1))
                {
                    g.DrawLine(pen, bounds.Right - 1, bounds.Y + 2, bounds.Right - 1, bounds.Bottom - 2);
                }

                // 顶部和底部的锐利装饰线
                using (var pen = new Pen(Color.FromArgb(120, LeagueColors.TextGold), 1))
                {
                    g.DrawLine(pen, bounds.X + 3, bounds.Y, bounds.Right - 10, bounds.Y);
                    g.DrawLine(pen, bounds.X + 3, bounds.Bottom - 1, bounds.Right - 10, bounds.Bottom - 1);
                }
            }
            else if (isHovered)
            {
                // 悬停状态 - 微妙的反馈效果
                using (var brush = new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(15, LeagueColors.PrimaryGold),
                    Color.FromArgb(5, LeagueColors.PrimaryGoldDark),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, bounds);
                }

                // 左侧细线提示
                using (var pen = new Pen(Color.FromArgb(120, LeagueColors.PrimaryGold), 2))
                {
                    g.DrawLine(pen, bounds.X, bounds.Y + 4, bounds.X, bounds.Bottom - 4);
                }

                // 右侧微光效果
                using (var pen = new Pen(Color.FromArgb(40, LeagueColors.TextGold), 1))
                {
                    g.DrawLine(pen, bounds.Right - 1, bounds.Y + bounds.Height / 4, bounds.Right - 1, bounds.Bottom - bounds.Height / 4);
                }
            }
        }

        /// <summary>
        /// 绘制树节点展开图标 - 精细化版本
        /// </summary>
        private void DrawTreeNodeExpandIcon(Graphics g, Rectangle bounds, bool isExpanded)
        {
            var iconRect = new Rectangle(bounds.X + 5, bounds.Y + bounds.Height / 2 - 6, 12, 12);

            // 绘制渐变背景
            using (var brush = new LinearGradientBrush(
                iconRect,
                LeagueColors.PrimaryGold,
                LeagueColors.PrimaryGoldDark,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, iconRect);
            }

            // 添加高光
            var highlightRect = new Rectangle(iconRect.X + 1, iconRect.Y + 1, iconRect.Width - 2, iconRect.Height / 2);
            using (var brush = new LinearGradientBrush(
                highlightRect,
                Color.FromArgb(80, Color.White),
                Color.FromArgb(20, Color.White),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, highlightRect);
            }

            // 绘制双层边框
            using (var pen = new Pen(LeagueColors.TextGold, 2))
            {
                g.DrawRectangle(pen, iconRect.X, iconRect.Y, iconRect.Width - 1, iconRect.Height - 1);
            }
            using (var pen = new Pen(Color.FromArgb(120, Color.White), 1))
            {
                g.DrawRectangle(pen, iconRect.X + 1, iconRect.Y + 1, iconRect.Width - 3, iconRect.Height - 3);
            }

            // 绘制精细化的 + 或 - 符号
            using (var pen = new Pen(Color.FromArgb(200, Color.Black), 2))
            {
                // 水平线
                g.DrawLine(pen, iconRect.X + 3, iconRect.Y + 6, iconRect.X + 9, iconRect.Y + 6);

                // 垂直线（只在折叠状态显示）
                if (!isExpanded)
                {
                    g.DrawLine(pen, iconRect.X + 6, iconRect.Y + 3, iconRect.X + 6, iconRect.Y + 9);
                }
            }

            // 添加符号阴影
            using (var pen = new Pen(Color.FromArgb(100, Color.White), 1))
            {
                // 水平线阴影
                g.DrawLine(pen, iconRect.X + 3, iconRect.Y + 7, iconRect.X + 9, iconRect.Y + 7);

                // 垂直线阴影（只在折叠状态显示）
                if (!isExpanded)
                {
                    g.DrawLine(pen, iconRect.X + 7, iconRect.Y + 3, iconRect.X + 7, iconRect.Y + 9);
                }
            }
        }

        /// <summary>
        /// 绘制树节点图标 - 精细化版本
        /// </summary>
        private void DrawTreeNodeIcon(Graphics g, Rectangle bounds, TreeNode node)
        {
            var iconRect = new Rectangle(bounds.X + 25, bounds.Y + bounds.Height / 2 - 8, 16, 16);

            // 根据节点类型绘制不同图标
            Color iconColor = LeagueColors.AccentBlue;
            Color iconSecondary = Color.FromArgb(100, LeagueColors.AccentBlue);

            if (node.Text.Contains("物料") || node.Text.Contains("Material"))
            {
                iconColor = LeagueColors.SuccessGreen;
                iconSecondary = Color.FromArgb(100, LeagueColors.SuccessGreen);
            }
            else if (node.Text.Contains("生产") || node.Text.Contains("Production"))
            {
                iconColor = LeagueColors.WarningOrange;
                iconSecondary = Color.FromArgb(100, LeagueColors.WarningOrange);
            }
            else if (node.Text.Contains("车间") || node.Text.Contains("Workshop"))
            {
                iconColor = LeagueColors.ErrorRed;
                iconSecondary = Color.FromArgb(100, LeagueColors.ErrorRed);
            }
            else if (node.Text.Contains("系统") || node.Text.Contains("System"))
            {
                iconColor = LeagueColors.TextGold;
                iconSecondary = Color.FromArgb(100, LeagueColors.TextGold);
            }

            // 绘制英雄联盟风格的六边形图标
            var centerX = iconRect.X + iconRect.Width / 2;
            var centerY = iconRect.Y + iconRect.Height / 2;
            var radius = Math.Min(iconRect.Width, iconRect.Height) / 2 - 2;

            // 创建六边形路径
            var hexPoints = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                var angle = i * Math.PI / 3;
                hexPoints[i] = new PointF(
                    centerX + (float)(radius * Math.Cos(angle)),
                    centerY + (float)(radius * Math.Sin(angle))
                );
            }

            // 绘制六边形背景
            using (var brush = new LinearGradientBrush(
                iconRect,
                Color.FromArgb(120, iconColor),
                Color.FromArgb(40, iconColor),
                LinearGradientMode.Vertical))
            {
                g.FillPolygon(brush, hexPoints);
            }

            // 绘制六边形边框
            using (var pen = new Pen(iconColor, 2))
            {
                g.DrawPolygon(pen, hexPoints);
            }

            // 内部高光六边形
            var innerRadius = radius - 2;
            var innerHexPoints = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                var angle = i * Math.PI / 3;
                innerHexPoints[i] = new PointF(
                    centerX + (float)(innerRadius * Math.Cos(angle)),
                    centerY + (float)(innerRadius * Math.Sin(angle))
                );
            }

            using (var pen = new Pen(Color.FromArgb(80, Color.White), 1))
            {
                g.DrawPolygon(pen, innerHexPoints);
            }

            // 中心装饰菱形
            var diamondSize = 4;
            var diamondPoints = new PointF[]
            {
                new PointF(centerX, centerY - diamondSize),
                new PointF(centerX + diamondSize, centerY),
                new PointF(centerX, centerY + diamondSize),
                new PointF(centerX - diamondSize, centerY)
            };

            using (var brush = new SolidBrush(Color.FromArgb(180, Color.White)))
            {
                g.FillPolygon(brush, diamondPoints);
            }
        }

        /// <summary>
        /// 绘制树节点文字 - 精细化版本
        /// </summary>
        private void DrawTreeNodeText(Graphics g, Rectangle bounds, string text, bool isSelected, bool isHovered)
        {
            var textRect = new Rectangle(bounds.X + 50, bounds.Y, bounds.Width - 55, bounds.Height);

            Color textColor = isSelected ? LeagueColors.TextGold :
                             isHovered ? LeagueColors.TextPrimary :
                             LeagueColors.TextSecondary;

            var font = new Font("微软雅黑", 9F, isSelected ? FontStyle.Bold : FontStyle.Regular);

            var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };

            if (isSelected)
            {
                // 选中状态 - 精致的文字效果
                using (var shadowBrush = new SolidBrush(Color.FromArgb(80, Color.Black)))
                {
                    var shadowRect = new Rectangle(textRect.X + 1, textRect.Y + 1, textRect.Width, textRect.Height);
                    g.DrawString(text, font, shadowBrush, shadowRect, sf);
                }

                // 轻微的外发光
                using (var glowBrush = new SolidBrush(Color.FromArgb(20, LeagueColors.PrimaryGoldLight)))
                {
                    var glowRect1 = new Rectangle(textRect.X - 1, textRect.Y, textRect.Width, textRect.Height);
                    var glowRect2 = new Rectangle(textRect.X + 1, textRect.Y, textRect.Width, textRect.Height);
                    g.DrawString(text, font, glowBrush, glowRect1, sf);
                    g.DrawString(text, font, glowBrush, glowRect2, sf);
                }

                // 主文字
                using (var brush = new SolidBrush(textColor))
                {
                    g.DrawString(text, font, brush, textRect, sf);
                }
            }
            else if (isHovered)
            {
                // 悬停状态 - 轻微发光
                using (var glowBrush = new SolidBrush(Color.FromArgb(20, LeagueColors.PrimaryGold)))
                {
                    var glowRect1 = new Rectangle(textRect.X - 1, textRect.Y, textRect.Width, textRect.Height);
                    var glowRect2 = new Rectangle(textRect.X + 1, textRect.Y, textRect.Width, textRect.Height);
                    g.DrawString(text, font, glowBrush, glowRect1, sf);
                    g.DrawString(text, font, glowBrush, glowRect2, sf);
                }

                using (var brush = new SolidBrush(textColor))
                {
                    g.DrawString(text, font, brush, textRect, sf);
                }
            }
            else
            {
                // 普通状态
                using (var brush = new SolidBrush(textColor))
                {
                    g.DrawString(text, font, brush, textRect, sf);
                }
            }
        }

        #endregion

        #region 英雄联盟主题布局

        /// <summary>
        /// 启用英雄联盟自定义绘制
        /// </summary>
        private void EnableLeagueCustomPainting()
        {
            try
            {
                // 为主要面板启用自定义绘制
                this.panelLeft.Paint += PanelLeft_Paint;
                this.panelMain.Paint += PanelMain_Paint;
                this.panelWelcome.Paint += PanelWelcome_Paint;
                this.panelModuleCards.Paint += PanelModuleCards_Paint;
                this.panelStatusInfo.Paint += PanelStatusInfo_Paint;

                // 为卡片按钮启用自定义绘制
                this.btnMaterialCard.Paint += BtnCard_Paint;
                this.btnProductionCard.Paint += BtnCard_Paint;
                this.btnWorkshopCard.Paint += BtnCard_Paint;

                // 为菜单栏和工具栏启用自定义绘制
                this.menuStrip1.Renderer = new LeagueMenuRenderer();
                this.toolStrip1.Renderer = new LeagueToolStripRenderer();

                // 启用双缓冲以减少闪烁
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

                // 启动动画定时器
                StartAnimationTimer();

                // 初始化动画管理器
                InitializeAnimationManager();

                LogManager.Info("英雄联盟自定义绘制已启用");
            }
            catch (Exception ex)
            {
                LogManager.Error("启用英雄联盟自定义绘制失败", ex);
            }
        }

        /// <summary>
        /// 左侧面板自定义绘制
        /// </summary>
        private void PanelLeft_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            // 绘制英雄联盟风格面板
            LeagueVisualEffects.DrawLeaguePanel(e.Graphics, panel.ClientRectangle);
        }

        /// <summary>
        /// 主面板自定义绘制
        /// </summary>
        private void PanelMain_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            // 绘制深色背景渐变
            using (var brush = new LinearGradientBrush(
                panel.ClientRectangle,
                Color.FromArgb(15, 20, 30),
                Color.FromArgb(25, 30, 40),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, panel.ClientRectangle);
            }
        }

        /// <summary>
        /// 欢迎面板自定义绘制
        /// </summary>
        private void PanelWelcome_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            // 绘制增强的英雄联盟风格面板（包含粒子效果）
            LeagueVisualEffects.DrawEnhancedLeaguePanel(e.Graphics, panel.ClientRectangle, panel);
        }

        /// <summary>
        /// 模块卡片面板自定义绘制 - 英雄联盟风格增强版
        /// </summary>
        private void PanelModuleCards_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            // 绘制背景装饰和六边形元素
            LeagueVisualEffects.DrawHexagonDecorations(e.Graphics, panel.ClientRectangle);

            // 添加更多英雄联盟风格装饰
            if (panel.Width > 400 && panel.Height > 300)
            {
                LeagueVisualEffects.DrawLargeHexagonBackground(e.Graphics, panel.ClientRectangle);
            }
        }

        /// <summary>
        /// 状态信息面板自定义绘制
        /// </summary>
        private void PanelStatusInfo_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            // 绘制增强的英雄联盟风格面板（包含粒子效果）
            LeagueVisualEffects.DrawEnhancedLeaguePanel(e.Graphics, panel.ClientRectangle, panel);
        }

        /// <summary>
        /// 卡片按钮自定义绘制
        /// </summary>
        private void BtnCard_Paint(object sender, PaintEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            // 判断按钮状态
            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 绘制英雄联盟风格按钮
            LeagueVisualEffects.DrawLeagueButton(
                e.Graphics,
                button.ClientRectangle,
                isHovered,
                isPressed,
                button.Text,
                button.Font);
        }

        /// <summary>
        /// 启动动画定时器
        /// </summary>
        private void StartAnimationTimer()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 50; // 20 FPS
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        /// <summary>
        /// 动画定时器事件
        /// </summary>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // 更新动画进度
            animationProgress += 0.02f;
            if (animationProgress > 1.0f)
                animationProgress = 0f;

            // 刷新需要动画的控件
            this.panelWelcome.Invalidate();
            this.panelStatusInfo.Invalidate();
        }

        /// <summary>
        /// 初始化动画管理器
        /// </summary>
        private void InitializeAnimationManager()
        {
            try
            {
                // 获取动画管理器实例
                animationManager = LeagueAnimationManager.Instance;

                // 为主要面板启用粒子效果
                LeagueVisualEffects.EnableParticleEffects(this.panelWelcome);
                LeagueVisualEffects.EnableParticleEffects(this.panelStatusInfo);

                // 注册控件到动画管理器
                animationManager.RegisterControl(this.panelWelcome);
                animationManager.RegisterControl(this.panelStatusInfo);
                animationManager.RegisterControl(this.btnMaterialCard);
                animationManager.RegisterControl(this.btnProductionCard);
                animationManager.RegisterControl(this.btnWorkshopCard);

                // 启动入场动画
                StartEntranceAnimations();

                LogManager.Info("动画管理器初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化动画管理器失败", ex);
            }
        }

        /// <summary>
        /// 启动入场动画
        /// </summary>
        private void StartEntranceAnimations()
        {
            // 延迟启动各个面板的入场动画
            var timer = new Timer();
            timer.Interval = 100;
            int step = 0;

            timer.Tick += (s, e) =>
            {
                switch (step)
                {
                    case 0:
                        animationManager.FadeIn(this.panelWelcome, 800);
                        break;
                    case 2:
                        animationManager.FadeIn(this.btnMaterialCard, 600);
                        break;
                    case 3:
                        animationManager.FadeIn(this.btnProductionCard, 600);
                        break;
                    case 4:
                        animationManager.FadeIn(this.btnWorkshopCard, 600);
                        break;
                    case 6:
                        animationManager.FadeIn(this.panelStatusInfo, 800);
                        timer.Stop();
                        timer.Dispose();
                        break;
                }
                step++;
            };

            timer.Start();
        }

        /// <summary>
        /// 初始化卡片特效事件
        /// </summary>
        private void InitializeCardEffectEvents()
        {
            try
            {
                // 为卡片按钮添加鼠标事件
                this.btnMaterialCard.MouseEnter += BtnCard_MouseEnter;
                this.btnMaterialCard.MouseLeave += BtnCard_MouseLeave;
                this.btnMaterialCard.MouseClick += BtnCard_MouseClick;

                this.btnProductionCard.MouseEnter += BtnCard_MouseEnter;
                this.btnProductionCard.MouseLeave += BtnCard_MouseLeave;
                this.btnProductionCard.MouseClick += BtnCard_MouseClick;

                this.btnWorkshopCard.MouseEnter += BtnCard_MouseEnter;
                this.btnWorkshopCard.MouseLeave += BtnCard_MouseLeave;
                this.btnWorkshopCard.MouseClick += BtnCard_MouseClick;

                LogManager.Info("卡片特效事件初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化卡片特效事件失败", ex);
            }
        }

        /// <summary>
        /// 卡片鼠标进入事件
        /// </summary>
        private void BtnCard_MouseEnter(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null && animationManager != null)
            {
                // 启动发光动画
                animationManager.Glow(button, 1500);

                // 启动脉冲动画
                animationManager.Pulse(button, 2000);
            }
        }

        /// <summary>
        /// 卡片鼠标离开事件
        /// </summary>
        private void BtnCard_MouseLeave(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                // 动画会自然结束，这里可以添加淡出效果
                button.Invalidate();
            }
        }

        /// <summary>
        /// 卡片鼠标点击事件
        /// </summary>
        private void BtnCard_MouseClick(object sender, MouseEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                // 在点击位置触发粒子爆发
                var clickPoint = new Point(e.X, e.Y);
                LeagueVisualEffects.TriggerParticleBurst(button, clickPoint, 25);

                // 触发额外的视觉反馈
                button.Invalidate();
            }
        }

        #endregion

        #region 英雄联盟布局改造方法

        /// <summary>
        /// 改造左侧导航面板为LOL风格 - 强制应用版
        /// </summary>
        private void TransformNavigationPanelToLeagueStyle()
        {
            // 强制设置导航面板的LOL风格背景
            panelLeft.BackColor = LeagueColors.DarkBackground;
            panelLeft.Width = 350; // 显著增加宽度以体现LOL风格

            // 强制改造导航内容面板
            panelNavContent.BackColor = LeagueColors.DarkBackground;

            // 强制改造导航头部
            panelNavHeader.BackColor = LeagueColors.DarkSurface;
            panelNavHeader.Height = 90; // 增加高度
            labelNavTitle.Text = "⚔️ MES 指挥中心";
            labelNavTitle.Font = new Font("微软雅黑", 16, FontStyle.Bold);
            labelNavTitle.ForeColor = LeagueColors.TextGold;
            labelNavTitle.TextAlign = ContentAlignment.MiddleCenter;
            labelNavTitle.Dock = DockStyle.Fill; // 填充整个头部

            // 强制改造TreeView为LOL风格
            treeViewModules.BackColor = LeagueColors.DarkBackground;
            treeViewModules.ForeColor = LeagueColors.TextPrimary;
            treeViewModules.BorderStyle = BorderStyle.None;
            treeViewModules.ShowLines = false;
            treeViewModules.ShowPlusMinus = true; // 保留展开按钮但自定义样式
            treeViewModules.ShowRootLines = false;
            treeViewModules.ItemHeight = 50; // 显著增加行高
            treeViewModules.Font = new Font("微软雅黑", 12, FontStyle.Regular);
            treeViewModules.FullRowSelect = true;
            treeViewModules.HideSelection = false;

            // 强制改造导航底部
            panelNavFooter.BackColor = LeagueColors.DarkSurface;
            panelNavFooter.Height = 70; // 增加高度
            labelNavInfo.ForeColor = LeagueColors.TextSecondary;
            labelNavInfo.Font = new Font("微软雅黑", 10, FontStyle.Regular);
            labelNavInfo.TextAlign = ContentAlignment.MiddleCenter;
            labelNavInfo.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 改造主面板为LOL卡片布局 - 强制应用版
        /// </summary>
        private void TransformMainPanelToLeagueStyle()
        {
            // 强制设置主面板背景
            panelMain.BackColor = LeagueColors.DarkBackground;

            // 强制改造欢迎面板
            panelWelcome.BackColor = Color.Transparent; // 透明以显示自定义绘制
            panelWelcome.Height = 140; // 增加高度

            // 强制设置系统标题样式
            labelSystemTitle.Font = new Font("微软雅黑", 28, FontStyle.Bold);
            labelSystemTitle.ForeColor = LeagueColors.TextGold;
            labelSystemTitle.TextAlign = ContentAlignment.MiddleLeft; // 左对齐更符合LOL风格
            labelSystemTitle.Text = "⚔️ MES 制造执行系统";

            labelSystemVersion.Font = new Font("微软雅黑", 14, FontStyle.Regular);
            labelSystemVersion.ForeColor = LeagueColors.TextSecondary;
            labelSystemVersion.TextAlign = ContentAlignment.MiddleLeft;
            labelSystemVersion.Text = "版本 1.0.0 - 英雄联盟风格企业级制造管理";

            // 强制改造模块卡片面板
            panelModuleCards.BackColor = Color.Transparent; // 透明以显示自定义绘制
            panelModuleCards.Padding = new Padding(30);
            panelModuleCards.Height = 220; // 增加高度

            // 强制改造卡片按钮为LOL风格
            TransformCardButtonsToLeagueStyle();

            // 强制改造状态信息面板
            panelStatusInfo.BackColor = Color.Transparent; // 透明以显示自定义绘制
            labelStatusTitle.ForeColor = LeagueColors.TextGold;
            labelStatusTitle.Font = new Font("微软雅黑", 16, FontStyle.Bold);
            labelTechInfo.ForeColor = LeagueColors.TextSecondary;
            labelTechInfo.Font = new Font("微软雅黑", 11, FontStyle.Regular);
        }

        /// <summary>
        /// 改造卡片按钮为LOL风格
        /// </summary>
        private void TransformCardButtonsToLeagueStyle()
        {
            // 改造物料管理卡片
            btnMaterialCard.BackColor = Color.Transparent;
            btnMaterialCard.ForeColor = LeagueColors.TextGold;
            btnMaterialCard.Font = new Font("微软雅黑", 14, FontStyle.Bold);
            btnMaterialCard.Text = "🛡️ 物料管理 (L成员)\n\n• 物料信息管理\n• BOM物料清单\n• 工艺路线配置";
            btnMaterialCard.Size = new Size(300, 180);

            // 改造生产管理卡片
            btnProductionCard.BackColor = Color.Transparent;
            btnProductionCard.ForeColor = LeagueColors.AccentBlue;
            btnProductionCard.Font = new Font("微软雅黑", 14, FontStyle.Bold);
            btnProductionCard.Text = "⚔️ 生产管理 (H成员)\n\n• 生产订单管理\n• 生产执行控制\n• 批次管理";
            btnProductionCard.Size = new Size(300, 180);

            // 改造车间管理卡片
            btnWorkshopCard.BackColor = Color.Transparent;
            btnWorkshopCard.ForeColor = LeagueColors.ErrorRed;
            btnWorkshopCard.Font = new Font("微软雅黑", 14, FontStyle.Bold);
            btnWorkshopCard.Text = "🏭 车间管理 (S成员)\n\n• 车间作业管理\n• 在制品管理\n• 设备状态管理";
            btnWorkshopCard.Size = new Size(300, 180);
        }

        /// <summary>
        /// 改造菜单栏和工具栏为LOL风格
        /// </summary>
        private void TransformMenuAndToolbarToLeagueStyle()
        {
            // 改造菜单栏
            menuStrip1.BackColor = LeagueColors.DarkSurface;
            menuStrip1.ForeColor = LeagueColors.TextPrimary;
            menuStrip1.Font = new Font("微软雅黑", 10, FontStyle.Regular);

            // 改造工具栏
            toolStrip1.BackColor = LeagueColors.DarkSurface;
            toolStrip1.ForeColor = LeagueColors.TextPrimary;
            toolStrip1.Font = new Font("微软雅黑", 9, FontStyle.Regular);

            // 改造状态栏
            statusStrip1.BackColor = LeagueColors.DarkSurface;
            statusStrip1.ForeColor = LeagueColors.TextSecondary;

            // 改造分隔条
            splitter1.BackColor = LeagueColors.PrimaryGold;
            splitter1.Width = 3;
        }

        /// <summary>
        /// 添加LOL特色装饰元素
        /// </summary>
        private void AddLeagueDecorationElements()
        {
            // 为主要面板添加自定义绘制事件
            panelNavHeader.Paint += PanelNavHeader_Paint;
            panelWelcome.Paint += PanelWelcome_Paint;
            panelModuleCards.Paint += PanelModuleCards_Paint;
            panelStatusInfo.Paint += PanelStatusInfo_Paint;
        }

        #endregion

        #region 英雄联盟风格绘制事件

        /// <summary>
        /// 导航头部面板绘制事件
        /// </summary>
        private void PanelNavHeader_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            LeagueVisualEffects.DrawLeaguePanel(e.Graphics, panel.ClientRectangle);

            // 添加六边形装饰
            LeagueVisualEffects.DrawHexagonDecorations(e.Graphics, panel.ClientRectangle);
        }



        #endregion
    }
}
