using System;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI.Forms
{
    /// <summary>
    /// MES系统主窗体
    /// </summary>
    public partial class MainForm : Form
    {
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
                this.Text = "MES制造执行系统 v1.0";
                
                // 初始化状态栏
                InitializeStatusBar();
                
                // 初始化菜单
                InitializeMenu();
                
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
            
            // 添加状态标签
            var statusLabel = new ToolStripStatusLabel("系统就绪")
            {
                Spring = true,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            };
            statusStrip1.Items.Add(statusLabel);
            
            // 添加时间标签
            var timeLabel = new ToolStripStatusLabel(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            statusStrip1.Items.Add(timeLabel);
            
            // 启动定时器更新时间
            var timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) => timeLabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            timer.Start();
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
