using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Models.Material;
using MES.BLL.Material;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 工艺路线配置窗体
    /// 用于管理产品的工艺路线和工艺步骤
    /// </summary>
    public partial class ProcessRouteConfigForm : Form
    {
        #region 私有字段

        private List<ProcessRoute> _processRoutes;
        private List<ProcessStep> _currentSteps;
        private ProcessRoute _selectedRoute;
        private readonly ProcessRouteService _processRouteService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化工艺路线配置窗体
        /// </summary>
        public ProcessRouteConfigForm()
        {
            InitializeComponent();
            _processRouteService = new ProcessRouteService();
            _processRoutes = new List<ProcessRoute>();
            _currentSteps = new List<ProcessStep>();
            
            InitializeForm();
        }

        #endregion

        #region 窗体初始化

        /// <summary>
        /// 初始化窗体设置
        /// </summary>
        private void InitializeForm()
        {
            // 设置窗体属性
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 640);
            
            // 初始化控件
            InitializeControls();
            InitializeDataGridViews();
            InitializeComboBoxes();
            
            // 绑定事件
            BindEvents();
            
            // 加载数据
            LoadData();
        }

        /// <summary>
        /// 初始化控件设置
        /// </summary>
        private void InitializeControls()
        {
            // 设置分割容器
            splitContainer.SplitterWidth = 5;
            splitContainer.BackColor = Color.FromArgb(233, 236, 239);
            
            // 设置面板边距
            panelLeft.Padding = new Padding(10);
            panelRight.Padding = new Padding(10);
        }

        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitializeDataGridViews()
        {
            // 配置工艺路线表格
            ConfigureRoutesDataGridView();
            
            // 配置工艺步骤表格
            ConfigureStepsDataGridView();
        }

        /// <summary>
        /// 配置工艺路线表格
        /// </summary>
        private void ConfigureRoutesDataGridView()
        {
            dgvRoutes.AutoGenerateColumns = false;
            dgvRoutes.AllowUserToAddRows = false;
            dgvRoutes.AllowUserToDeleteRows = false;
            dgvRoutes.ReadOnly = true;
            dgvRoutes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoutes.MultiSelect = false;
            
            // 添加列
            dgvRoutes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "ID",
                DataPropertyName = "Id",
                Width = 60,
                ReadOnly = true,
                Visible = false // 隐藏ID列
            });

            dgvRoutes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RouteCode",
                HeaderText = "路线编码",
                DataPropertyName = "RouteCode",
                Width = 120,
                ReadOnly = true
            });
            
            dgvRoutes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RouteName",
                HeaderText = "路线名称",
                DataPropertyName = "RouteName",
                Width = 150,
                ReadOnly = true
            });
            
            dgvRoutes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 120,
                ReadOnly = true
            });
            
            dgvRoutes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Version",
                HeaderText = "版本",
                DataPropertyName = "Version",
                Width = 80,
                ReadOnly = true
            });
            
            dgvRoutes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "StatusText",
                Width = 80,
                ReadOnly = true
            });
            
            dgvRoutes.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreateTime",
                HeaderText = "创建时间",
                DataPropertyName = "CreateTime",
                Width = 140,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm" }
            });
        }

        /// <summary>
        /// 配置工艺步骤表格
        /// </summary>
        private void ConfigureStepsDataGridView()
        {
            dgvSteps.AutoGenerateColumns = false;
            dgvSteps.AllowUserToAddRows = false;
            dgvSteps.AllowUserToDeleteRows = false;
            dgvSteps.ReadOnly = true;
            dgvSteps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSteps.MultiSelect = false;
            
            // 添加列
            dgvSteps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StepNumber",
                HeaderText = "步骤序号",
                DataPropertyName = "StepNumber",
                Width = 80,
                ReadOnly = true
            });
            
            dgvSteps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StepName",
                HeaderText = "步骤名称",
                DataPropertyName = "StepName",
                Width = 150,
                ReadOnly = true
            });
            
            dgvSteps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkstationName",
                HeaderText = "工作站",
                DataPropertyName = "WorkstationName",
                Width = 120,
                ReadOnly = true
            });
            
            dgvSteps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StandardTime",
                HeaderText = "标准工时(分钟)",
                DataPropertyName = "StandardTime",
                Width = 120,
                ReadOnly = true
            });
            
            dgvSteps.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "步骤描述",
                DataPropertyName = "Description",
                Width = 200,
                ReadOnly = true
            });
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitializeComboBoxes()
        {
            // 产品下拉框
            cmbProduct.Items.Clear();
            cmbProduct.Items.Add("全部产品");
            cmbProduct.SelectedIndex = 0;
            
            // 状态下拉框
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("全部状态");
            cmbStatus.Items.Add("草稿");
            cmbStatus.Items.Add("启用");
            cmbStatus.Items.Add("停用");
            cmbStatus.SelectedIndex = 0;
        }

        #endregion

        #region 事件绑定

        /// <summary>
        /// 绑定控件事件
        /// </summary>
        private void BindEvents()
        {
            // 搜索相关事件
            btnSearch.Click += BtnSearch_Click;
            txtSearch.KeyPress += TxtSearch_KeyPress;
            cmbProduct.SelectedIndexChanged += SearchCondition_Changed;
            cmbStatus.SelectedIndexChanged += SearchCondition_Changed;
            
            // 工艺路线操作按钮事件
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnCopy.Click += BtnCopy_Click;
            btnRefresh.Click += BtnRefresh_Click;
            
            // 工艺步骤操作按钮事件
            btnAddStep.Click += BtnAddStep_Click;
            btnEditStep.Click += BtnEditStep_Click;
            btnDeleteStep.Click += BtnDeleteStep_Click;
            btnStepUp.Click += BtnStepUp_Click;
            btnStepDown.Click += BtnStepDown_Click;
            
            // 表格选择事件
            dgvRoutes.SelectionChanged += DgvRoutes_SelectionChanged;
            dgvSteps.SelectionChanged += DgvSteps_SelectionChanged;
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                LoadProcessRoutes();
                UpdateButtonStates();
                UpdateStatusLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载工艺路线数据
        /// </summary>
        private void LoadProcessRoutes()
        {
            // 模拟数据加载
            _processRoutes = GenerateSampleData();
            
            // 应用筛选条件
            var filteredRoutes = ApplyFilters(_processRoutes);
            
            // 绑定到表格
            dgvRoutes.DataSource = filteredRoutes;
            
            // 清空工艺步骤
            _currentSteps.Clear();
            dgvSteps.DataSource = null;
            _selectedRoute = null;
        }

        /// <summary>
        /// 生成示例数据
        /// </summary>
        private List<ProcessRoute> GenerateSampleData()
        {
            return new List<ProcessRoute>
            {
                new ProcessRoute
                {
                    Id = 1,
                    RouteCode = "PR001",
                    RouteName = "手机主板生产工艺",
                    ProductName = "智能手机主板",
                    Version = "V1.0",
                    Status = ProcessRouteStatus.Active,
                    CreateTime = DateTime.Now.AddDays(-10),
                    Steps = new List<ProcessStep>
                    {
                        new ProcessStep { StepNumber = 1, StepName = "SMT贴片", WorkstationName = "SMT生产线", StandardTime = 30, Description = "表面贴装技术" },
                        new ProcessStep { StepNumber = 2, StepName = "回流焊接", WorkstationName = "回流焊炉", StandardTime = 15, Description = "高温焊接工艺" },
                        new ProcessStep { StepNumber = 3, StepName = "功能测试", WorkstationName = "测试工位", StandardTime = 20, Description = "电路功能检测" }
                    }
                },
                new ProcessRoute
                {
                    Id = 2,
                    RouteCode = "PR002",
                    RouteName = "电池组装工艺",
                    ProductName = "锂电池组",
                    Version = "V2.1",
                    Status = ProcessRouteStatus.Draft,
                    CreateTime = DateTime.Now.AddDays(-5),
                    Steps = new List<ProcessStep>
                    {
                        new ProcessStep { StepNumber = 1, StepName = "电芯检测", WorkstationName = "检测工位", StandardTime = 10, Description = "电芯质量检测" },
                        new ProcessStep { StepNumber = 2, StepName = "组装焊接", WorkstationName = "焊接工位", StandardTime = 25, Description = "电芯组装焊接" }
                    }
                }
            };
        }

        #endregion

        #region 筛选和搜索

        /// <summary>
        /// 应用筛选条件
        /// </summary>
        private List<ProcessRoute> ApplyFilters(List<ProcessRoute> routes)
        {
            var filtered = routes.AsEnumerable();

            // 产品筛选
            if (cmbProduct.SelectedIndex > 0)
            {
                var selectedProduct = cmbProduct.Text;
                filtered = filtered.Where(r => r.ProductName.Contains(selectedProduct));
            }

            // 状态筛选
            if (cmbStatus.SelectedIndex > 0)
            {
                var selectedStatus = GetStatusFromText(cmbStatus.Text);
                filtered = filtered.Where(r => r.Status == selectedStatus);
            }

            // 关键字搜索
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var keyword = txtSearch.Text.Trim();
                filtered = filtered.Where(r =>
                    r.RouteCode.Contains(keyword) ||
                    r.RouteName.Contains(keyword) ||
                    r.ProductName.Contains(keyword));
            }

            return filtered.ToList();
        }

        /// <summary>
        /// 从文本获取状态枚举
        /// </summary>
        private ProcessRouteStatus GetStatusFromText(string statusText)
        {
            switch (statusText)
            {
                case "草稿": return ProcessRouteStatus.Draft;
                case "启用": return ProcessRouteStatus.Active;
                case "停用": return ProcessRouteStatus.Inactive;
                default: return ProcessRouteStatus.Draft;
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadProcessRoutes();
        }

        /// <summary>
        /// 搜索框回车事件
        /// </summary>
        private void TxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LoadProcessRoutes();
            }
        }

        /// <summary>
        /// 搜索条件改变事件
        /// </summary>
        private void SearchCondition_Changed(object sender, EventArgs e)
        {
            LoadProcessRoutes();
        }

        /// <summary>
        /// 新增工艺路线
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var editForm = new ProcessRouteEditForm())
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadProcessRoutes();
                        MessageBox.Show("新增工艺路线成功", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("新增失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑工艺路线
        /// </summary>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedRoute == null)
            {
                MessageBox.Show("请先选择要编辑的工艺路线", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var editForm = new ProcessRouteEditForm(_selectedRoute))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadProcessRoutes();
                        MessageBox.Show("编辑工艺路线成功", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("编辑失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除工艺路线
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedRoute == null)
            {
                MessageBox.Show("请先选择要删除的工艺路线", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(string.Format("确定要删除工艺路线 '{0}' 吗？", _selectedRoute.RouteName),
                "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // TODO: 实现删除逻辑
                    MessageBox.Show("删除成功", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProcessRoutes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("删除失败：{0}", ex.Message), "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 复制工艺路线
        /// </summary>
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (_selectedRoute == null)
            {
                MessageBox.Show("请先选择要复制的工艺路线", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: 实现复制逻辑
                MessageBox.Show(string.Format("复制工艺路线：{0}", _selectedRoute.RouteName), "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("复制失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 工艺路线选择改变事件
        /// </summary>
        private void DgvRoutes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRoutes.SelectedRows.Count > 0)
            {
                var selectedRow = dgvRoutes.SelectedRows[0];
                var routeId = Convert.ToInt32(selectedRow.Cells["Id"].Value ?? 0);
                _selectedRoute = _processRoutes.FirstOrDefault(r => r.Id == routeId);

                if (_selectedRoute != null)
                {
                    LoadProcessSteps(_selectedRoute.Steps);
                }
            }
            else
            {
                _selectedRoute = null;
                _currentSteps.Clear();
                dgvSteps.DataSource = null;
            }

            UpdateButtonStates();
        }

        /// <summary>
        /// 加载工艺步骤
        /// </summary>
        private void LoadProcessSteps(List<ProcessStep> steps)
        {
            _currentSteps = steps != null ? steps.OrderBy(s => s.StepNumber).ToList() : new List<ProcessStep>();
            dgvSteps.DataSource = _currentSteps;
        }

        /// <summary>
        /// 工艺步骤选择改变事件
        /// </summary>
        private void DgvSteps_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStepButtonStates();
        }

        #endregion

        #region 工艺步骤操作

        /// <summary>
        /// 新增工艺步骤
        /// </summary>
        private void BtnAddStep_Click(object sender, EventArgs e)
        {
            if (_selectedRoute == null)
            {
                MessageBox.Show("请先选择工艺路线", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: 打开新增工艺步骤对话框
                MessageBox.Show("新增工艺步骤功能开发中...", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("新增步骤失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑工艺步骤
        /// </summary>
        private void BtnEditStep_Click(object sender, EventArgs e)
        {
            if (dgvSteps.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要编辑的工艺步骤", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: 打开编辑工艺步骤对话框
                MessageBox.Show("编辑工艺步骤功能开发中...", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("编辑步骤失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除工艺步骤
        /// </summary>
        private void BtnDeleteStep_Click(object sender, EventArgs e)
        {
            if (dgvSteps.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要删除的工艺步骤", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("确定要删除选中的工艺步骤吗？",
                "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // TODO: 实现删除步骤逻辑
                    MessageBox.Show("删除成功", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("删除步骤失败：{0}", ex.Message), "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 工艺步骤上移
        /// </summary>
        private void BtnStepUp_Click(object sender, EventArgs e)
        {
            if (dgvSteps.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要移动的工艺步骤", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: 实现步骤上移逻辑
                MessageBox.Show("步骤上移功能开发中...", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("移动失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 工艺步骤下移
        /// </summary>
        private void BtnStepDown_Click(object sender, EventArgs e)
        {
            if (dgvSteps.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要移动的工艺步骤", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // TODO: 实现步骤下移逻辑
                MessageBox.Show("步骤下移功能开发中...", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("移动失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 界面状态更新

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            bool hasSelection = _selectedRoute != null;

            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
            btnCopy.Enabled = hasSelection;

            UpdateStepButtonStates();
        }

        /// <summary>
        /// 更新工艺步骤按钮状态
        /// </summary>
        private void UpdateStepButtonStates()
        {
            bool hasRoute = _selectedRoute != null;
            bool hasStepSelection = dgvSteps.SelectedRows.Count > 0;

            btnAddStep.Enabled = hasRoute;
            btnEditStep.Enabled = hasStepSelection;
            btnDeleteStep.Enabled = hasStepSelection;
            btnStepUp.Enabled = hasStepSelection;
            btnStepDown.Enabled = hasStepSelection;
        }

        /// <summary>
        /// 更新状态标签
        /// </summary>
        private void UpdateStatusLabel()
        {
            var totalCount = dgvRoutes.Rows.Count;
            lblTotal.Text = string.Format("共 {0} 条记录", totalCount);
        }

        #endregion
    }
}
