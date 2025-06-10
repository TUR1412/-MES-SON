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
using MES.Common.Logging;

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
        private readonly IProcessRouteBLL _processRouteBLL;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化工艺路线配置窗体
        /// </summary>
        public ProcessRouteConfigForm()
        {
            InitializeComponent();
            _processRouteBLL = new ProcessRouteBLL();
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
            try
            {
                // 从数据库加载真实工艺路线数据
                string keyword = txtSearch.Text.Trim();
                int? productId = null;
                ProcessRouteStatus? status = null;

                // 解析产品筛选条件
                if (cmbProduct.SelectedIndex > 0)
                {
                    // 根据产品名称获取产品ID
                    productId = GetProductIdByName(cmbProduct.Text);
                }

                // 解析状态筛选条件
                if (cmbStatus.SelectedIndex > 0)
                {
                    status = GetStatusFromText(cmbStatus.Text);
                }

                // 使用BLL进行数据库查询
                _processRoutes = _processRouteBLL.SearchProcessRoutes(
                    string.IsNullOrEmpty(keyword) ? null : keyword,
                    productId,
                    status);

                // 绑定到表格
                dgvRoutes.DataSource = _processRoutes;

                // 清空工艺步骤
                _currentSteps.Clear();
                dgvSteps.DataSource = null;
                _selectedRoute = null;

                UpdateStatusLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载工艺路线数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 如果数据库加载失败，使用空列表避免程序崩溃
                _processRoutes = new List<ProcessRoute>();
                dgvRoutes.DataSource = _processRoutes;
                _currentSteps.Clear();
                dgvSteps.DataSource = null;
                _selectedRoute = null;
            }
        }



        #endregion

        #region 筛选和搜索

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

        /// <summary>
        /// 根据产品名称获取产品ID
        /// </summary>
        private int? GetProductIdByName(string productName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productName) || productName == "全部产品")
                {
                    return null;
                }

                // 简化实现：从产品下拉框的数据源中查找
                // 实际项目中应该调用产品BLL获取产品ID
                var products = new Dictionary<string, int>
                {
                    { "智能手机主板", 1 },
                    { "锂电池组", 2 },
                    { "显示屏模组", 3 },
                    { "摄像头模组", 4 },
                    { "充电器", 5 }
                };

                return products.ContainsKey(productName) ? products[productName] : (int?)null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取产品ID失败，产品名称：{0}", productName), ex);
                return null;
            }
        }

        /// <summary>
        /// 获取下一个步骤编号
        /// </summary>
        private int GetNextStepNumber()
        {
            try
            {
                if (_currentSteps == null || _currentSteps.Count == 0)
                {
                    return 1;
                }

                return _currentSteps.Max(s => s.StepNumber) + 1;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取下一个步骤编号失败", ex);
                return 1;
            }
        }

        /// <summary>
        /// 获取选中的工艺步骤
        /// </summary>
        private ProcessStep GetSelectedProcessStep()
        {
            try
            {
                if (dgvSteps.SelectedRows.Count == 0)
                {
                    return null;
                }

                var selectedRow = dgvSteps.SelectedRows[0];
                var stepId = Convert.ToInt32(selectedRow.Cells["Id"].Value ?? 0);
                return _currentSteps.FirstOrDefault(s => s.Id == stepId);
            }
            catch (Exception ex)
            {
                LogManager.Error("获取选中的工艺步骤失败", ex);
                return null;
            }
        }

        /// <summary>
        /// 显示输入对话框
        /// </summary>
        private string ShowInputDialog(string prompt, string title, string defaultValue = "")
        {
            try
            {
                Form inputForm = new Form();
                inputForm.Text = title;
                inputForm.Size = new Size(400, 150);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                Label label = new Label();
                label.Text = prompt;
                label.Location = new Point(10, 20);
                label.Size = new Size(360, 20);

                TextBox textBox = new TextBox();
                textBox.Text = defaultValue;
                textBox.Location = new Point(10, 45);
                textBox.Size = new Size(360, 20);

                Button okButton = new Button();
                okButton.Text = "确定";
                okButton.Location = new Point(215, 80);
                okButton.Size = new Size(75, 25);
                okButton.DialogResult = DialogResult.OK;

                Button cancelButton = new Button();
                cancelButton.Text = "取消";
                cancelButton.Location = new Point(295, 80);
                cancelButton.Size = new Size(75, 25);
                cancelButton.DialogResult = DialogResult.Cancel;

                inputForm.Controls.Add(label);
                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(okButton);
                inputForm.Controls.Add(cancelButton);

                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = cancelButton;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return textBox.Text;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error("显示输入对话框失败", ex);
                return null;
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
                    // 使用真实的删除操作
                    bool deleteResult = _processRouteBLL.DeleteProcessRoute(_selectedRoute.Id);

                    if (deleteResult)
                    {
                        MessageBox.Show("删除成功", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProcessRoutes();
                    }
                    else
                    {
                        MessageBox.Show("删除失败", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                // 生成新的编码和名称
                string newRouteCode = _selectedRoute.RouteCode + "_Copy";
                string newRouteName = _selectedRoute.RouteName + "_副本";

                // 确保编码唯一
                int copyIndex = 1;
                while (!_processRouteBLL.IsRouteCodeUnique(newRouteCode))
                {
                    newRouteCode = string.Format("{0}_Copy_{1}", _selectedRoute.RouteCode, copyIndex);
                    copyIndex++;
                }

                // 使用真实的复制操作
                bool copyResult = _processRouteBLL.CopyProcessRoute(_selectedRoute.Id, newRouteCode, newRouteName);

                if (copyResult)
                {
                    MessageBox.Show(string.Format("复制工艺路线成功：{0}", newRouteName), "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProcessRoutes();
                }
                else
                {
                    MessageBox.Show("复制工艺路线失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            try
            {
                if (_selectedRoute != null)
                {
                    // 从数据库重新加载工艺步骤
                    _currentSteps = _processRouteBLL.GetProcessSteps(_selectedRoute.Id);
                }
                else
                {
                    _currentSteps = steps != null ? steps.OrderBy(s => s.StepNumber).ToList() : new List<ProcessStep>();
                }

                dgvSteps.DataSource = _currentSteps;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载工艺步骤失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _currentSteps = new List<ProcessStep>();
                dgvSteps.DataSource = _currentSteps;
            }
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
                // 创建新的工艺步骤
                var newStep = new ProcessStep
                {
                    ProcessRouteId = _selectedRoute.Id,
                    StepNumber = GetNextStepNumber(),
                    StepName = "新工艺步骤",
                    Description = "",
                    StandardTime = 0,
                    OperationInstructions = "",
                    QualityRequirements = "",
                    Status = ProcessStepStatus.Active,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                // 使用BLL添加工艺步骤
                bool result = _processRouteBLL.AddProcessStep(_selectedRoute.Id, newStep);

                if (result)
                {
                    LoadProcessSteps(_selectedRoute.Steps);
                    MessageBox.Show("新增工艺步骤成功", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("新增工艺步骤失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                // 获取选中的工艺步骤
                var selectedStep = GetSelectedProcessStep();
                if (selectedStep == null)
                {
                    MessageBox.Show("请先选择要编辑的工艺步骤", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 简化的编辑实现：弹出输入框修改步骤名称
                string newStepName = ShowInputDialog("请输入新的步骤名称：", "编辑工艺步骤", selectedStep.StepName);

                if (!string.IsNullOrWhiteSpace(newStepName) && newStepName != selectedStep.StepName)
                {
                    selectedStep.StepName = newStepName;
                    selectedStep.UpdateTime = DateTime.Now;

                    // 使用BLL更新工艺步骤
                    bool result = _processRouteBLL.UpdateProcessStep(selectedStep);

                    if (result)
                    {
                        LoadProcessSteps(_selectedRoute.Steps);
                        MessageBox.Show("编辑工艺步骤成功", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("编辑工艺步骤失败", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
                    // 实现删除步骤逻辑
                    var selectedStep = GetSelectedProcessStep();
                    if (selectedStep != null)
                    {
                        bool deleteResult = _processRouteBLL.DeleteProcessStep(selectedStep.Id);
                        if (deleteResult)
                        {
                            LoadProcessSteps(_selectedRoute.Steps);
                            MessageBox.Show("删除成功", "提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请先选择要删除的工艺步骤", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
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
                // 实现步骤上移逻辑
                var selectedStep = GetSelectedProcessStep();
                if (selectedStep == null)
                {
                    MessageBox.Show("请先选择要移动的工艺步骤", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否已经是第一个步骤
                if (selectedStep.StepNumber <= 1)
                {
                    MessageBox.Show("该步骤已经是第一个步骤，无法上移", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 使用BLL实现步骤上移
                bool result = _processRouteBLL.MoveProcessStep(_selectedRoute.Id, selectedStep.Id, true);

                if (result)
                {
                    LoadProcessSteps(_selectedRoute.Steps);
                    MessageBox.Show("步骤上移成功", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("步骤上移失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                // 实现步骤下移逻辑
                var selectedStep = GetSelectedProcessStep();
                if (selectedStep == null)
                {
                    MessageBox.Show("请先选择要移动的工艺步骤", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 检查是否已经是最后一个步骤
                int maxStepNumber = _currentSteps.Count > 0 ? _currentSteps.Max(s => s.StepNumber) : 0;
                if (selectedStep.StepNumber >= maxStepNumber)
                {
                    MessageBox.Show("该步骤已经是最后一个步骤，无法下移", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 使用BLL实现步骤下移
                bool result = _processRouteBLL.MoveProcessStep(_selectedRoute.Id, selectedStep.Id, false);

                if (result)
                {
                    LoadProcessSteps(_selectedRoute.Steps);
                    MessageBox.Show("步骤下移成功", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("步骤下移失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
