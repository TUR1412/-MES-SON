using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;
using MES.UI.Framework.Utilities;
using MES.Common.Logging;

namespace MES.UI.Forms
{
    /// <summary>
    /// UI框架演示窗体 - 展示新UI组件的功能和效果
    /// </summary>
    public partial class UIFrameworkDemoForm : Form
    {
        #region 私有字段

        private QueryPanel _queryPanel;
        private EnhancedDataGridView _dataGridView;
        private ModernButton _btnPrimary;
        private ModernButton _btnSecondary;
        private ModernButton _btnSuccess;
        private ModernButton _btnWarning;
        private ModernButton _btnDanger;
        private ModernButton _btnOutline;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化UI框架演示窗体
        /// </summary>
        public UIFrameworkDemoForm()
        {
            InitializeComponent();
            InitializeDemoForm();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化演示窗体
        /// </summary>
        private void InitializeDemoForm()
        {
            try
            {
                // 应用标准窗体样式
                UIHelper.ApplyStandardFormStyle(this);
                
                // 设置窗体属性
                Text = "UI框架组件演示";
                Size = new Size(1000, 700);
                StartPosition = FormStartPosition.CenterParent;
                
                // 创建演示组件
                CreateDemoComponents();
                
                // 初始化演示数据
                InitializeDemoData();
                
                LogManager.Info("UI框架演示窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("UI框架演示窗体初始化失败", ex);
                UIHelper.ShowError($"窗体初始化失败：{ex.Message}", "错误", this);
            }
        }

        /// <summary>
        /// 创建演示组件
        /// </summary>
        private void CreateDemoComponents()
        {
            SuspendLayout();
            
            // 创建主面板
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            Controls.Add(mainPanel);
            
            // 创建标题标签
            var titleLabel = new Label
            {
                Text = "MES UI框架组件演示",
                Font = UIThemeManager.GetTitleFont(16f),
                ForeColor = UIThemeManager.Colors.Primary,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            mainPanel.Controls.Add(titleLabel);
            
            // 创建按钮演示区域
            CreateButtonDemoArea(mainPanel);
            
            // 创建查询面板演示区域
            CreateQueryPanelDemoArea(mainPanel);
            
            // 创建数据网格演示区域
            CreateDataGridDemoArea(mainPanel);
            
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// 创建按钮演示区域
        /// </summary>
        /// <param name="parent">父容器</param>
        private void CreateButtonDemoArea(Control parent)
        {
            var buttonPanel = new Panel
            {
                Location = new Point(10, 50),
                Size = new Size(960, 80),
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(buttonPanel);
            
            var buttonLabel = new Label
            {
                Text = "现代化按钮组件演示：",
                Font = UIThemeManager.GetTitleFont(12f),
                Location = new Point(10, 10),
                AutoSize = true
            };
            buttonPanel.Controls.Add(buttonLabel);
            
            // 创建不同样式的按钮
            _btnPrimary = new ModernButton
            {
                Text = "主要按钮",
                Style = ModernButton.ButtonStyle.Primary,
                Location = new Point(10, 40),
                Size = new Size(100, 30)
            };
            _btnPrimary.Click += (s, e) => UIHelper.ShowInfo("点击了主要按钮", "按钮演示", this);
            buttonPanel.Controls.Add(_btnPrimary);
            
            _btnSecondary = new ModernButton
            {
                Text = "次要按钮",
                Style = ModernButton.ButtonStyle.Secondary,
                Location = new Point(120, 40),
                Size = new Size(100, 30)
            };
            _btnSecondary.Click += (s, e) => UIHelper.ShowInfo("点击了次要按钮", "按钮演示", this);
            buttonPanel.Controls.Add(_btnSecondary);
            
            _btnSuccess = new ModernButton
            {
                Text = "成功按钮",
                Style = ModernButton.ButtonStyle.Success,
                Location = new Point(230, 40),
                Size = new Size(100, 30)
            };
            _btnSuccess.Click += (s, e) => UIHelper.ShowInfo("点击了成功按钮", "按钮演示", this);
            buttonPanel.Controls.Add(_btnSuccess);
            
            _btnWarning = new ModernButton
            {
                Text = "警告按钮",
                Style = ModernButton.ButtonStyle.Warning,
                Location = new Point(340, 40),
                Size = new Size(100, 30)
            };
            _btnWarning.Click += (s, e) => UIHelper.ShowWarning("这是警告按钮的演示", "按钮演示", this);
            buttonPanel.Controls.Add(_btnWarning);
            
            _btnDanger = new ModernButton
            {
                Text = "危险按钮",
                Style = ModernButton.ButtonStyle.Danger,
                Location = new Point(450, 40),
                Size = new Size(100, 30)
            };
            _btnDanger.Click += (s, e) => 
            {
                if (UIHelper.ShowConfirm("确定要执行危险操作吗？", "确认", this))
                {
                    UIHelper.ShowInfo("已执行危险操作", "按钮演示", this);
                }
            };
            buttonPanel.Controls.Add(_btnDanger);
            
            _btnOutline = new ModernButton
            {
                Text = "轮廓按钮",
                Style = ModernButton.ButtonStyle.Outline,
                Location = new Point(560, 40),
                Size = new Size(100, 30)
            };
            _btnOutline.Click += (s, e) => UIHelper.ShowInfo("点击了轮廓按钮", "按钮演示", this);
            buttonPanel.Controls.Add(_btnOutline);
        }

        /// <summary>
        /// 创建查询面板演示区域
        /// </summary>
        /// <param name="parent">父容器</param>
        private void CreateQueryPanelDemoArea(Control parent)
        {
            _queryPanel = new QueryPanel
            {
                Location = new Point(10, 140),
                Size = new Size(960, 120)
            };
            parent.Controls.Add(_queryPanel);
            
            // 添加查询字段
            _queryPanel.AddQueryField(new QueryPanel.QueryField
            {
                Name = "MaterialCode",
                DisplayName = "物料编码",
                FieldType = QueryPanel.QueryFieldType.Text,
                IsRequired = false
            });
            
            _queryPanel.AddQueryField(new QueryPanel.QueryField
            {
                Name = "MaterialName",
                DisplayName = "物料名称",
                FieldType = QueryPanel.QueryFieldType.Text,
                IsRequired = false
            });
            
            _queryPanel.AddQueryField(new QueryPanel.QueryField
            {
                Name = "Category",
                DisplayName = "物料类别",
                FieldType = QueryPanel.QueryFieldType.ComboBox,
                ComboBoxItems = new[] { "原材料", "半成品", "成品", "辅料" },
                IsRequired = false
            });
            
            _queryPanel.AddQueryField(new QueryPanel.QueryField
            {
                Name = "CreateDate",
                DisplayName = "创建日期",
                FieldType = QueryPanel.QueryFieldType.Date,
                DefaultValue = DateTime.Today,
                IsRequired = false
            });
            
            // 订阅查询事件
            _queryPanel.SearchClicked += OnQueryPanelSearch;
            _queryPanel.ResetClicked += OnQueryPanelReset;
        }

        /// <summary>
        /// 创建数据网格演示区域
        /// </summary>
        /// <param name="parent">父容器</param>
        private void CreateDataGridDemoArea(Control parent)
        {
            var gridLabel = new Label
            {
                Text = "增强数据网格组件演示：",
                Font = UIThemeManager.GetTitleFont(12f),
                Location = new Point(10, 270),
                AutoSize = true
            };
            parent.Controls.Add(gridLabel);
            
            _dataGridView = new EnhancedDataGridView
            {
                Location = new Point(10, 300),
                Size = new Size(960, 350),
                ShowRowNumbers = true,
                AlternateRowColors = true,
                EmptyDataText = "暂无物料数据，请点击查询按钮加载数据"
            };
            parent.Controls.Add(_dataGridView);
            
            // 配置标准样式
            UIHelper.ConfigureStandardDataGrid(_dataGridView);
        }

        /// <summary>
        /// 初始化演示数据
        /// </summary>
        private void InitializeDemoData()
        {
            // 创建演示数据表
            var dataTable = new DataTable();
            dataTable.Columns.Add("物料编码", typeof(string));
            dataTable.Columns.Add("物料名称", typeof(string));
            dataTable.Columns.Add("物料类别", typeof(string));
            dataTable.Columns.Add("规格型号", typeof(string));
            dataTable.Columns.Add("单位", typeof(string));
            dataTable.Columns.Add("库存数量", typeof(int));
            dataTable.Columns.Add("单价", typeof(decimal));
            dataTable.Columns.Add("创建时间", typeof(DateTime));
            
            // 添加示例数据
            dataTable.Rows.Add("M001", "钢板A", "原材料", "1000*2000*5mm", "张", 150, 1250.50m, DateTime.Now.AddDays(-10));
            dataTable.Rows.Add("M002", "螺栓B", "辅料", "M8*25", "个", 5000, 0.85m, DateTime.Now.AddDays(-8));
            dataTable.Rows.Add("M003", "电机C", "成品", "380V/3KW", "台", 25, 2800.00m, DateTime.Now.AddDays(-5));
            dataTable.Rows.Add("M004", "轴承D", "原材料", "6205-2RS", "个", 200, 45.60m, DateTime.Now.AddDays(-3));
            dataTable.Rows.Add("M005", "齿轮E", "半成品", "模数2.5", "个", 80, 125.30m, DateTime.Now.AddDays(-1));
            
            _dataGridView.DataSource = dataTable;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 查询面板搜索事件
        /// </summary>
        private void OnQueryPanelSearch(object sender, QueryPanel.QueryEventArgs e)
        {
            try
            {
                string message = "执行查询，参数：\n";
                foreach (var param in e.QueryParameters)
                {
                    message += $"{param.Key}: {param.Value}\n";
                }
                
                UIHelper.ShowInfo(message, "查询演示", this);
                
                // 模拟刷新数据
                _dataGridView.RefreshData();
                
                LogManager.Info("查询面板搜索事件被触发");
            }
            catch (Exception ex)
            {
                LogManager.Error("处理查询事件失败", ex);
                UIHelper.ShowError("查询失败：" + ex.Message, "错误", this);
            }
        }

        /// <summary>
        /// 查询面板重置事件
        /// </summary>
        private void OnQueryPanelReset(object sender, EventArgs e)
        {
            UIHelper.ShowInfo("查询条件已重置", "重置演示", this);
            LogManager.Info("查询面板重置事件被触发");
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UIFrameworkDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Name = "UIFrameworkDemoForm";
            this.Text = "UI框架组件演示";
            this.ResumeLayout(false);
        }

        #endregion
    }
}
