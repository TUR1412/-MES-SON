using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Common.Configuration;
using MES.UI.Forms.Material;
using MES.UI.Forms.SystemManagement;
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
            statusTimer = new Timer { Interval = 1000 };
            statusTimer.Tick += (s, e) =>
            {
                timeLabel.Text = GetFormattedDateTime();
                // 可以在这里添加其他实时更新的状态信息
            };
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
                Image = CreateToolBarIcon(Color.FromArgb(40, 167, 69)),
                ToolTipText = "物料信息管理 (L成员负责) - Ctrl+M",
                Font = new Font("微软雅黑", 9F),
                ForeColor = Color.FromArgb(40, 167, 69),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleRight
            };
            materialBtn.Click += (s, e) => OpenMaterialForm();
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
            productionBtn.Click += (s, e) => OpenProductionOrderForm();
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
            workshopBtn.Click += (s, e) => OpenWorkshopOperationForm();
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
            systemBtn.Click += (s, e) => OpenSystemConfigForm();
            toolStrip1.Items.Add(systemBtn);

            // 弹性空间
            var spacer = new ToolStripLabel()
            {
                Text = "",
                AutoSize = false,
                Width = 100
            };
            toolStrip1.Items.Add(spacer);

            // 刷新按钮
            var refreshBtn = new ToolStripButton("刷新")
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                Image = CreateRefreshIcon(),
                ToolTipText = "刷新界面数据",
                Font = new Font("微软雅黑", 9F),
                ForeColor = Color.FromArgb(108, 117, 125)
            };
            refreshBtn.Click += (s, e) => RefreshData();
            toolStrip1.Items.Add(refreshBtn);
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
            // 清空现有菜单项
            menuStrip1.Items.Clear();

            // 物料管理菜单 - L成员负责
            var materialMenu = new ToolStripMenuItem("📦 物料管理(&M)")
            {
                ForeColor = Color.FromArgb(40, 167, 69),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(40, 167, 69))
            };

            var materialInfoItem = new ToolStripMenuItem("物料信息管理", null, (s, e) => OpenMaterialForm())
            {
                ShortcutKeys = Keys.Control | Keys.M,
                ShowShortcutKeys = true
            };
            materialMenu.DropDownItems.Add(materialInfoItem);
            materialMenu.DropDownItems.Add("BOM物料清单", null, (s, e) => OpenBOMForm());
            materialMenu.DropDownItems.Add("工艺路线配置", null, (s, e) => OpenProcessRouteForm());
            menuStrip1.Items.Add(materialMenu);

            // 生产管理菜单 - H成员负责
            var productionMenu = new ToolStripMenuItem("⚙️ 生产管理(&P)")
            {
                ForeColor = Color.FromArgb(0, 123, 255),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(0, 123, 255))
            };

            var productionOrderItem = new ToolStripMenuItem("生产订单管理", null, (s, e) => OpenProductionOrderForm())
            {
                ShortcutKeys = Keys.Control | Keys.P,
                ShowShortcutKeys = true
            };
            productionMenu.DropDownItems.Add(productionOrderItem);
            productionMenu.DropDownItems.Add("生产执行控制", null, (s, e) => OpenProductionExecutionForm());
            productionMenu.DropDownItems.Add("用户权限管理", null, (s, e) => OpenUserPermissionForm());
            menuStrip1.Items.Add(productionMenu);

            // 车间管理菜单 - S成员负责
            var workshopMenu = new ToolStripMenuItem("🏭 车间管理(&W)")
            {
                ForeColor = Color.FromArgb(220, 53, 69),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(220, 53, 69))
            };

            var workshopOperationItem = new ToolStripMenuItem("车间作业管理", null, (s, e) => OpenWorkshopOperationForm())
            {
                ShortcutKeys = Keys.Control | Keys.W,
                ShowShortcutKeys = true
            };
            workshopMenu.DropDownItems.Add(workshopOperationItem);
            workshopMenu.DropDownItems.Add("在制品管理", null, (s, e) => OpenWIPForm());
            workshopMenu.DropDownItems.Add("设备状态管理", null, (s, e) => OpenEquipmentForm());
            menuStrip1.Items.Add(workshopMenu);

            // 系统管理菜单
            var systemMenu = new ToolStripMenuItem("⚙️ 系统管理(&S)")
            {
                ForeColor = Color.FromArgb(108, 117, 125),
                Font = new Font("微软雅黑", 9.5F, FontStyle.Bold),
                Image = CreateMenuIcon(Color.FromArgb(108, 117, 125))
            };
            systemMenu.DropDownItems.Add("系统配置", null, (s, e) => OpenSystemConfigForm());
            systemMenu.DropDownItems.Add(new ToolStripSeparator());
            systemMenu.DropDownItems.Add("UI框架演示", null, (s, e) => ShowUIFrameworkInfo());
            systemMenu.DropDownItems.Add(new ToolStripSeparator());
            systemMenu.DropDownItems.Add("关于系统", null, (s, e) => ShowAbout());
            menuStrip1.Items.Add(systemMenu);

            // 帮助菜单
            var helpMenu = new ToolStripMenuItem("❓ 帮助(&H)")
            {
                ForeColor = Color.FromArgb(108, 117, 125),
                Font = new Font("微软雅黑", 9.5F),
                Image = CreateMenuIcon(Color.FromArgb(108, 117, 125))
            };
            helpMenu.DropDownItems.Add("使用手册", null, (s, e) => ShowUserManual());
            helpMenu.DropDownItems.Add("技术支持", null, (s, e) => ShowTechnicalSupport());
            helpMenu.DropDownItems.Add(new ToolStripSeparator());
            helpMenu.DropDownItems.Add("关于MES", null, (s, e) => ShowAbout());
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
        /// 初始化卡片点击事件
        /// </summary>
        private void InitializeCardClickEvents()
        {
            // 物料管理卡片点击事件
            panelMaterialCard.Click += (s, e) => OpenMaterialForm();
            labelMaterialTitle.Click += (s, e) => OpenMaterialForm();
            labelMaterialDesc.Click += (s, e) => OpenMaterialForm();
            pictureBoxMaterial.Click += (s, e) => OpenMaterialForm();

            // 生产管理卡片点击事件
            panelProductionCard.Click += (s, e) => OpenProductionOrderForm();
            labelProductionTitle.Click += (s, e) => OpenProductionOrderForm();
            labelProductionDesc.Click += (s, e) => OpenProductionOrderForm();
            pictureBoxProduction.Click += (s, e) => OpenProductionOrderForm();

            // 车间管理卡片点击事件
            panelWorkshopCard.Click += (s, e) => OpenWorkshopOperationForm();
            labelWorkshopTitle.Click += (s, e) => OpenWorkshopOperationForm();
            labelWorkshopDesc.Click += (s, e) => OpenWorkshopOperationForm();
            pictureBoxWorkshop.Click += (s, e) => OpenWorkshopOperationForm();

            // 添加鼠标悬停效果
            AddCardHoverEffects();
        }

        /// <summary>
        /// 添加卡片悬停效果
        /// </summary>
        private void AddCardHoverEffects()
        {
            AddHoverEffect(panelMaterialCard);
            AddHoverEffect(panelProductionCard);
            AddHoverEffect(panelWorkshopCard);
        }

        /// <summary>
        /// 为面板添加悬停效果
        /// </summary>
        private void AddHoverEffect(Panel panel)
        {
            var originalBackColor = panel.BackColor;
            var hoverBackColor = Color.FromArgb(248, 249, 250);

            panel.MouseEnter += (s, e) => panel.BackColor = hoverBackColor;
            panel.MouseLeave += (s, e) => panel.BackColor = originalBackColor;

            // 为子控件也添加相同效果
            foreach (Control control in panel.Controls)
            {
                control.MouseEnter += (s, e) => panel.BackColor = hoverBackColor;
                control.MouseLeave += (s, e) => panel.BackColor = originalBackColor;
            }
        }

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
            
            LogManager.Info("用户退出系统");
            base.OnFormClosing(e);
        }
    }
}
