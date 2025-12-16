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
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Workshop
{
    /// <summary>
    /// 在制品管理窗体
    /// 提供在制品状态跟踪和管理功能 - S成员负责
    /// </summary>
    public partial class WIPManagementForm : ThemedForm
    {
        private readonly IWorkshopBLL _workshopBLL;
        private readonly IBatchBLL _batchBLL;
        private readonly IWIPBLL _wipBLL;
        private List<WIPInfo> _currentWIPs;
        private List<WorkshopInfo> _workshops;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WIPManagementForm()
        {
            InitializeComponent();
            _workshopBLL = new WorkshopBLL();
            _batchBLL = new BatchBLL();
            _wipBLL = new WIPBLL();
            _currentWIPs = new List<WIPInfo>();
            _workshops = new List<WorkshopInfo>();

            InitializeForm();
            LoadData();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                this.Text = "在制品管理";
                this.Size = new Size(1200, 640);
                this.StartPosition = FormStartPosition.CenterScreen;

                // 设置DataGridView列
                SetupDataGridView();
                
                // 初始化下拉框
                InitializeComboBoxes();
                
                // 初始化日期选择器
                InitializeDatePickers();
                
                // 绑定事件
                BindEvents();

                LogManager.Info("在制品管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("在制品管理窗体初始化失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("窗体初始化失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置DataGridView列
        /// </summary>
        private void SetupDataGridView()
        {
            dgvWIP.AutoGenerateColumns = false;
            dgvWIP.AllowUserToAddRows = false;
            dgvWIP.AllowUserToDeleteRows = false;
            dgvWIP.ReadOnly = true;
            dgvWIP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWIP.MultiSelect = false;

            // 添加列
            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WIPId",
                HeaderText = "在制品编号",
                DataPropertyName = "WIPId",
                Width = 120
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BatchNumber",
                HeaderText = "批次号",
                DataPropertyName = "BatchNumber",
                Width = 100
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductCode",
                HeaderText = "产品编码",
                DataPropertyName = "ProductCode",
                Width = 120
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 150
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopName",
                HeaderText = "当前车间",
                DataPropertyName = "WorkshopName",
                Width = 120
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "数量",
                DataPropertyName = "Quantity",
                Width = 80
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "StatusText",
                Width = 100
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Progress",
                HeaderText = "完成进度(%)",
                DataPropertyName = "Progress",
                Width = 100
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartTime",
                HeaderText = "开始时间",
                DataPropertyName = "StartTime",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EstimatedEndTime",
                HeaderText = "预计完成时间",
                DataPropertyName = "EstimatedEndTime",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UnitPrice",
                HeaderText = "单价",
                DataPropertyName = "UnitPrice",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvWIP.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalValue",
                HeaderText = "总价值",
                DataPropertyName = "TotalValue",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
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
                cmbStatus.Items.Add(new { Text = "生产中", Value = "1" });
                cmbStatus.Items.Add(new { Text = "质检中", Value = "2" });
                cmbStatus.Items.Add(new { Text = "暂停", Value = "3" });
                cmbStatus.Items.Add(new { Text = "已完成", Value = "4" });
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
        /// 初始化日期选择器
        /// </summary>
        private void InitializeDatePickers()
        {
            dtpStartDate.Value = DateTime.Now.AddDays(-30);
            dtpEndDate.Value = DateTime.Now;
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
            btnTrack.Click += BtnTrack_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnExport.Click += BtnExport_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnSearch.Click += BtnSearch_Click;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            dgvWIP.SelectionChanged += DgvWIP_SelectionChanged;
            cmbWorkshop.SelectedIndexChanged += CmbWorkshop_SelectedIndexChanged;
            cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
            dtpStartDate.ValueChanged += DatePicker_ValueChanged;
            dtpEndDate.ValueChanged += DatePicker_ValueChanged;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                LoadWIPs();
                UpdateButtonStates();
                UpdateSummary();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载数据失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载在制品数据
        /// </summary>
        private void LoadWIPs()
        {
            try
            {
                // 从数据库加载真实在制品数据
                _currentWIPs = _wipBLL.GetAllWIPs();

                dgvWIP.DataSource = _currentWIPs;

                lblTotal.Text = string.Format("共 {0} 条记录", _currentWIPs.Count);

                LogManager.Info(string.Format("加载在制品数据完成，共 {0} 条记录", _currentWIPs.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载在制品数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载在制品数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 如果数据库加载失败，使用空列表避免程序崩溃
                _currentWIPs = new List<WIPInfo>();
                dgvWIP.DataSource = _currentWIPs;
                lblTotal.Text = "共 0 条记录";
            }
        }



        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            bool hasSelection = dgvWIP.SelectedRows.Count > 0;
            btnTrack.Enabled = hasSelection;
            btnUpdate.Enabled = hasSelection;
        }

        /// <summary>
        /// 更新汇总信息
        /// </summary>
        private void UpdateSummary()
        {
            try
            {
                if (_currentWIPs == null || _currentWIPs.Count == 0)
                {
                    lblSummary.Text = "在制品总数量：0 | 总价值：￥0.00";
                    return;
                }

                decimal totalQuantity = _currentWIPs.Sum(w => w.Quantity);
                decimal totalValue = _currentWIPs.Sum(w => w.TotalValue);

                lblSummary.Text = string.Format("在制品总数量：{0:N0} | 总价值：{1:C2}", totalQuantity, totalValue);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新汇总信息失败：{0}", ex.Message), ex);
                lblSummary.Text = "汇总信息计算失败";
            }
        }

        /// <summary>
        /// 获取选中的在制品
        /// </summary>
        private WIPInfo GetSelectedWIP()
        {
            if (dgvWIP.SelectedRows.Count == 0)
                return null;

            int selectedIndex = dgvWIP.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < _currentWIPs.Count)
            {
                return _currentWIPs[selectedIndex];
            }

            return null;
        }

        #region 事件处理方法

        /// <summary>
        /// 跟踪在制品
        /// </summary>
        private void BtnTrack_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedWIP = GetSelectedWIP();
                if (selectedWIP == null)
                {
                    MessageBox.Show("请选择要跟踪的在制品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string trackingInfo = string.Format(
                    "在制品跟踪信息\n\n" +
                    "编号：{0}\n" +
                    "批次号：{1}\n" +
                    "产品：{2} - {3}\n" +
                    "当前车间：{4}\n" +
                    "数量：{5:N0}\n" +
                    "状态：{6}\n" +
                    "完成进度：{7:F1}%\n" +
                    "开始时间：{8:yyyy-MM-dd HH:mm:ss}\n" +
                    "预计完成：{9:yyyy-MM-dd HH:mm:ss}\n" +
                    "总价值：{10:C2}",
                    selectedWIP.WIPId,
                    selectedWIP.BatchNumber,
                    selectedWIP.ProductCode,
                    selectedWIP.ProductName,
                    selectedWIP.WorkshopName,
                    selectedWIP.Quantity,
                    selectedWIP.StatusText,
                    selectedWIP.Progress,
                    selectedWIP.StartTime,
                    selectedWIP.EstimatedEndTime,
                    selectedWIP.TotalValue);

                MessageBox.Show(trackingInfo, "在制品跟踪", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogManager.Info(string.Format("查看在制品跟踪信息：{0}", selectedWIP.WIPId));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("跟踪在制品失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("跟踪在制品失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新在制品状态
        /// </summary>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedWIP = GetSelectedWIP();
                if (selectedWIP == null)
                {
                    MessageBox.Show("请选择要更新的在制品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 模拟更新状态对话框
                string message = string.Format("请选择在制品 '{0}' 的新状态：\n\n" +
                    "1. 待开始\n" +
                    "2. 生产中\n" +
                    "3. 质检中\n" +
                    "4. 暂停\n" +
                    "5. 已完成\n\n" +
                    "当前状态：{1}",
                    selectedWIP.WIPId, selectedWIP.StatusText);

                var result = MessageBox.Show(message + "\n\n点击确定模拟更新状态", "更新在制品状态",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    // 使用真实的状态更新对话框
                    using (var statusDialog = new WIPStatusUpdateDialog(selectedWIP))
                    {
                        if (statusDialog.ShowDialog() == DialogResult.OK)
                        {
                            bool updateSuccess = true;

                            // 更新数据库中的状态
                            if (statusDialog.NewStatus != selectedWIP.Status)
                            {
                                updateSuccess = _wipBLL.UpdateWIPStatus(selectedWIP.WIPId, statusDialog.NewStatus, "当前用户");
                            }

                            // 更新已完成数量
                            if (updateSuccess && statusDialog.CompletedQuantity != selectedWIP.CompletedQuantity)
                            {
                                updateSuccess = _wipBLL.UpdateWIPProgress(selectedWIP.WIPId, statusDialog.CompletedQuantity, "当前用户");
                            }

                            if (updateSuccess)
                            {
                                // 刷新数据显示
                                LoadWIPs();
                                UpdateSummary();
                                MessageBox.Show("在制品状态更新成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("在制品状态更新失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    dgvWIP.Refresh();
                    MessageBox.Show("在制品状态已更新", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogManager.Info(string.Format("更新在制品状态：{0} -> {1}", selectedWIP.WIPId, selectedWIP.StatusText));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新在制品状态失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("更新在制品状态失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导出在制品数据
        /// </summary>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentWIPs == null || _currentWIPs.Count == 0)
                {
                    MessageBox.Show("没有可导出的数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 模拟导出功能
                var result = MessageBox.Show(string.Format("确定要导出 {0} 条在制品记录吗？", _currentWIPs.Count),
                    "确认导出", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 这里应该实现实际的导出逻辑
                    MessageBox.Show("在制品数据导出成功！\n\n注：实际项目中应实现Excel导出功能",
                        "导出成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogManager.Info(string.Format("导出在制品数据，共 {0} 条记录", _currentWIPs.Count));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("导出在制品数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("导出在制品数据失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            SearchWIPs();
        }

        /// <summary>
        /// 搜索框回车
        /// </summary>
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchWIPs();
            }
        }

        /// <summary>
        /// 选择变化
        /// </summary>
        private void DgvWIP_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        /// <summary>
        /// 车间选择变化
        /// </summary>
        private void CmbWorkshop_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchWIPs();
        }

        /// <summary>
        /// 状态选择变化
        /// </summary>
        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchWIPs();
        }

        /// <summary>
        /// 日期选择变化
        /// </summary>
        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            SearchWIPs();
        }

        #endregion

        /// <summary>
        /// 搜索在制品
        /// </summary>
        private void SearchWIPs()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                int? statusFilter = null;
                int? workshopFilter = null;
                DateTime? startDate = dtpStartDate.Value.Date;
                DateTime? endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);

                // 解析状态过滤条件
                if (cmbStatus.SelectedItem != null)
                {
                    var statusValue = ((dynamic)cmbStatus.SelectedItem).Value;
                    if (!string.IsNullOrEmpty(statusValue))
                    {
                        statusFilter = int.Parse(statusValue);
                    }
                }

                // 解析车间过滤条件
                if (cmbWorkshop.SelectedItem != null)
                {
                    var workshopValue = ((dynamic)cmbWorkshop.SelectedItem).Value;
                    if (workshopValue > 0)
                    {
                        workshopFilter = workshopValue;
                    }
                }

                // 使用BLL进行数据库查询
                var searchResults = _wipBLL.SearchWIPs(workshopFilter, statusFilter, startDate, endDate,
                    string.IsNullOrEmpty(keyword) ? null : keyword);

                _currentWIPs = searchResults;
                dgvWIP.DataSource = _currentWIPs;
                lblTotal.Text = string.Format("共 {0} 条记录", _currentWIPs.Count);

                // 更新汇总信息
                UpdateSummary();
                UpdateButtonStates();

                LogManager.Info(string.Format("搜索在制品完成，条件：关键词={0}, 状态={1}, 车间={2}, 结果数量={3}",
                    keyword, statusFilter, workshopFilter, _currentWIPs.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索在制品失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("搜索在制品失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
