using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.BLL.Workshop;
using MES.Models.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.UI.Forms.Workshop
{
    /// <summary>
    /// 车间作业管理窗体
    /// 提供车间生产作业的调度、监控和管理功能 - S成员负责
    /// </summary>
    public partial class WorkshopOperationForm : Form
    {
        private readonly IWorkshopBLL _workshopBLL;
        private readonly IBatchBLL _batchBLL;
        private List<WorkshopOperationInfo> _currentOperations;
        private List<WorkshopInfo> _workshops;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkshopOperationForm()
        {
            InitializeComponent();
            _workshopBLL = new WorkshopBLL();
            _batchBLL = new BatchBLL();
            _currentOperations = new List<WorkshopOperationInfo>();
            _workshops = new List<WorkshopInfo>();
            
            InitializeForm();
            LoadData();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                this.Text = "车间作业管理";
                this.Size = new Size(1200, 640);
                this.StartPosition = FormStartPosition.CenterScreen;

                // 设置DataGridView列
                SetupDataGridView();
                
                // 初始化下拉框
                InitializeComboBoxes();
                
                // 绑定事件
                BindEvents();

                LogManager.Info("车间作业管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("车间作业管理窗体初始化失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("窗体初始化失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置DataGridView列
        /// </summary>
        private void SetupDataGridView()
        {
            dgvOperations.AutoGenerateColumns = false;
            dgvOperations.AllowUserToAddRows = false;
            dgvOperations.AllowUserToDeleteRows = false;
            dgvOperations.ReadOnly = true;
            dgvOperations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOperations.MultiSelect = false;

            // 添加列
            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OperationId",
                HeaderText = "作业编号",
                DataPropertyName = "OperationId",
                Width = 120
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopName",
                HeaderText = "车间名称",
                DataPropertyName = "WorkshopName",
                Width = 120
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BatchNumber",
                HeaderText = "批次号",
                DataPropertyName = "BatchNumber",
                Width = 100
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductCode",
                HeaderText = "产品编码",
                DataPropertyName = "ProductCode",
                Width = 120
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "数量",
                DataPropertyName = "Quantity",
                Width = 80
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "StatusText",
                Width = 100
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartTime",
                HeaderText = "开始时间",
                DataPropertyName = "StartTime",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Progress",
                HeaderText = "进度(%)",
                DataPropertyName = "Progress",
                Width = 80
            });

            dgvOperations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Operator",
                HeaderText = "操作员",
                DataPropertyName = "Operator",
                Width = 100
            });
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitializeComboBoxes()
        {
            try
            {
                // 状态下拉框
                cmbStatus.Items.Clear();
                cmbStatus.Items.Add(new { Text = "全部", Value = "" });
                cmbStatus.Items.Add(new { Text = "待开始", Value = "0" });
                cmbStatus.Items.Add(new { Text = "进行中", Value = "1" });
                cmbStatus.Items.Add(new { Text = "已暂停", Value = "2" });
                cmbStatus.Items.Add(new { Text = "已完成", Value = "3" });
                cmbStatus.Items.Add(new { Text = "已停止", Value = "4" });
                cmbStatus.DisplayMember = "Text";
                cmbStatus.ValueMember = "Value";
                cmbStatus.SelectedIndex = 0;

                // 车间下拉框
                LoadWorkshops();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("初始化下拉框失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("初始化下拉框失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载车间数据
        /// </summary>
        private void LoadWorkshops()
        {
            try
            {
                _workshops = _workshopBLL.GetAllWorkshops();
                
                cmbWorkshop.Items.Clear();
                cmbWorkshop.Items.Add(new { Text = "全部车间", Value = 0 });
                
                foreach (var workshop in _workshops)
                {
                    cmbWorkshop.Items.Add(new { Text = workshop.WorkshopName, Value = workshop.Id });
                }
                
                cmbWorkshop.DisplayMember = "Text";
                cmbWorkshop.ValueMember = "Value";
                cmbWorkshop.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载车间数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载车间数据失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            btnStart.Click += BtnStart_Click;
            btnPause.Click += BtnPause_Click;
            btnStop.Click += BtnStop_Click;
            btnComplete.Click += BtnComplete_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnSearch.Click += BtnSearch_Click;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            dgvOperations.SelectionChanged += DgvOperations_SelectionChanged;
            cmbWorkshop.SelectedIndexChanged += CmbWorkshop_SelectedIndexChanged;
            cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                LoadOperations();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载数据失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载作业数据
        /// </summary>
        private void LoadOperations()
        {
            try
            {
                // 模拟作业数据 - 实际应该从数据库加载
                _currentOperations = GenerateSimulatedOperations();
                
                dgvOperations.DataSource = _currentOperations;
                
                lblTotal.Text = string.Format("共 {0} 条记录", _currentOperations.Count);
                
                LogManager.Info(string.Format("加载车间作业数据完成，共 {0} 条记录", _currentOperations.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载作业数据失败：{0}", ex.Message), ex);
                throw new MESException("加载作业数据时发生异常", ex);
            }
        }

        /// <summary>
        /// 生成模拟作业数据
        /// </summary>
        private List<WorkshopOperationInfo> GenerateSimulatedOperations()
        {
            var operations = new List<WorkshopOperationInfo>();
            var random = new Random();
            var statuses = new[] { "待开始", "进行中", "已暂停", "已完成", "已停止" };
            var operators = new[] { "张三", "李四", "王五", "赵六", "钱七" };
            
            for (int i = 1; i <= 20; i++)
            {
                var operation = new WorkshopOperationInfo
                {
                    OperationId = string.Format("OP{0:D6}", i),
                    WorkshopName = _workshops.Count > 0 ? _workshops[random.Next(_workshops.Count)].WorkshopName : "车间A",
                    BatchNumber = string.Format("B{0:D4}", random.Next(1, 100)),
                    ProductCode = string.Format("P{0:D3}", random.Next(1, 50)),
                    Quantity = random.Next(10, 1000),
                    Status = random.Next(0, 5),
                    StatusText = statuses[random.Next(statuses.Length)],
                    StartTime = DateTime.Now.AddHours(-random.Next(1, 48)),
                    Progress = random.Next(0, 101),
                    Operator = operators[random.Next(operators.Length)]
                };
                
                operations.Add(operation);
            }
            
            return operations;
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            bool hasSelection = dgvOperations.SelectedRows.Count > 0;
            var selectedOperation = GetSelectedOperation();
            
            if (hasSelection && selectedOperation != null)
            {
                btnStart.Enabled = selectedOperation.Status == 0 || selectedOperation.Status == 2; // 待开始或已暂停
                btnPause.Enabled = selectedOperation.Status == 1; // 进行中
                btnStop.Enabled = selectedOperation.Status == 1 || selectedOperation.Status == 2; // 进行中或已暂停
                btnComplete.Enabled = selectedOperation.Status == 1; // 进行中
            }
            else
            {
                btnStart.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
                btnComplete.Enabled = false;
            }
        }

        /// <summary>
        /// 获取选中的作业
        /// </summary>
        private WorkshopOperationInfo GetSelectedOperation()
        {
            if (dgvOperations.SelectedRows.Count == 0)
                return null;

            int selectedIndex = dgvOperations.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < _currentOperations.Count)
            {
                return _currentOperations[selectedIndex];
            }

            return null;
        }

        #region 事件处理方法

        /// <summary>
        /// 开始作业
        /// </summary>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedOperation = GetSelectedOperation();
                if (selectedOperation == null)
                {
                    MessageBox.Show("请选择要开始的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedOperation.Status != 0 && selectedOperation.Status != 2)
                {
                    MessageBox.Show("只能开始待开始或已暂停的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要开始作业 '{0}' 吗？", selectedOperation.OperationId),
                    "确认开始", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    selectedOperation.Status = 1;
                    selectedOperation.StatusText = "进行中";
                    selectedOperation.StartTime = DateTime.Now;

                    dgvOperations.Refresh();
                    UpdateButtonStates();

                    MessageBox.Show("作业已开始", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogManager.Info(string.Format("开始作业：{0}", selectedOperation.OperationId));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("开始作业失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("开始作业失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 暂停作业
        /// </summary>
        private void BtnPause_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedOperation = GetSelectedOperation();
                if (selectedOperation == null)
                {
                    MessageBox.Show("请选择要暂停的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedOperation.Status != 1)
                {
                    MessageBox.Show("只能暂停进行中的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要暂停作业 '{0}' 吗？", selectedOperation.OperationId),
                    "确认暂停", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    selectedOperation.Status = 2;
                    selectedOperation.StatusText = "已暂停";

                    dgvOperations.Refresh();
                    UpdateButtonStates();

                    MessageBox.Show("作业已暂停", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogManager.Info(string.Format("暂停作业：{0}", selectedOperation.OperationId));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("暂停作业失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("暂停作业失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 停止作业
        /// </summary>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedOperation = GetSelectedOperation();
                if (selectedOperation == null)
                {
                    MessageBox.Show("请选择要停止的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedOperation.Status != 1 && selectedOperation.Status != 2)
                {
                    MessageBox.Show("只能停止进行中或已暂停的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要停止作业 '{0}' 吗？\n停止后将无法继续执行。", selectedOperation.OperationId),
                    "确认停止", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    selectedOperation.Status = 4;
                    selectedOperation.StatusText = "已停止";

                    dgvOperations.Refresh();
                    UpdateButtonStates();

                    MessageBox.Show("作业已停止", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogManager.Info(string.Format("停止作业：{0}", selectedOperation.OperationId));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("停止作业失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("停止作业失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 完成作业
        /// </summary>
        private void BtnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedOperation = GetSelectedOperation();
                if (selectedOperation == null)
                {
                    MessageBox.Show("请选择要完成的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedOperation.Status != 1)
                {
                    MessageBox.Show("只能完成进行中的作业", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要完成作业 '{0}' 吗？", selectedOperation.OperationId),
                    "确认完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    selectedOperation.Status = 3;
                    selectedOperation.StatusText = "已完成";
                    selectedOperation.Progress = 100;

                    dgvOperations.Refresh();
                    UpdateButtonStates();

                    MessageBox.Show("作业已完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogManager.Info(string.Format("完成作业：{0}", selectedOperation.OperationId));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("完成作业失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("完成作业失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// 搜索按钮点击
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchOperations();
        }

        /// <summary>
        /// 搜索框回车
        /// </summary>
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchOperations();
            }
        }

        /// <summary>
        /// 选择变化
        /// </summary>
        private void DgvOperations_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        /// <summary>
        /// 车间选择变化
        /// </summary>
        private void CmbWorkshop_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchOperations();
        }

        /// <summary>
        /// 状态选择变化
        /// </summary>
        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchOperations();
        }

        #endregion

        /// <summary>
        /// 搜索作业
        /// </summary>
        private void SearchOperations()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                string statusFilter = "";
                int workshopFilter = 0;

                if (cmbStatus.SelectedItem != null)
                {
                    statusFilter = ((dynamic)cmbStatus.SelectedItem).Value;
                }

                if (cmbWorkshop.SelectedItem != null)
                {
                    workshopFilter = ((dynamic)cmbWorkshop.SelectedItem).Value;
                }

                var filteredOperations = _currentOperations.AsEnumerable();

                // 关键字过滤
                if (!string.IsNullOrEmpty(keyword))
                {
                    filteredOperations = filteredOperations.Where(o =>
                        o.OperationId.Contains(keyword) ||
                        o.BatchNumber.Contains(keyword) ||
                        o.ProductCode.Contains(keyword) ||
                        o.Operator.Contains(keyword));
                }

                // 状态过滤
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    int status = int.Parse(statusFilter);
                    filteredOperations = filteredOperations.Where(o => o.Status == status);
                }

                // 车间过滤
                if (workshopFilter > 0)
                {
                    var selectedWorkshop = _workshops.FirstOrDefault(w => w.Id == workshopFilter);
                    if (selectedWorkshop != null)
                    {
                        filteredOperations = filteredOperations.Where(o => o.WorkshopName == selectedWorkshop.WorkshopName);
                    }
                }

                var result = filteredOperations.ToList();
                dgvOperations.DataSource = result;
                lblTotal.Text = string.Format("共 {0} 条记录", result.Count);

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索作业失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("搜索作业失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// 车间作业信息模型
    /// </summary>
    public class WorkshopOperationInfo
    {
        public string OperationId { get; set; }
        public string WorkshopName { get; set; }
        public string BatchNumber { get; set; }
        public string ProductCode { get; set; }
        public decimal Quantity { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Progress { get; set; }
        public string Operator { get; set; }
    }
}
