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
    /// 设备状态管理窗体
    /// 提供生产设备状态监控和管理功能 - S成员负责
    /// </summary>
    public partial class EquipmentStatusForm : Form
    {
        private readonly IWorkshopBLL _workshopBLL;
        private readonly IEquipmentBLL _equipmentBLL;
        private List<EquipmentStatusInfo> _currentEquipments;
        private List<WorkshopInfo> _workshops;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentStatusForm()
        {
            InitializeComponent();
            _workshopBLL = new WorkshopBLL();
            _equipmentBLL = new EquipmentBLL();
            _currentEquipments = new List<EquipmentStatusInfo>();
            _workshops = new List<WorkshopInfo>();

            InitializeForm();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                this.Text = "设备状态管理";
                this.Size = new Size(1200, 640);
                this.StartPosition = FormStartPosition.CenterScreen;

                // 设置DataGridView列
                SetupDataGridView();
                
                // 初始化下拉框
                InitializeComboBoxes();
                
                // 绑定事件
                BindEvents();

                // 启动定时器
                timerRefresh.Interval = 30000; // 30秒刷新一次
                timerRefresh.Tick += TimerRefresh_Tick;
                timerRefresh.Start();

                LogManager.Info("设备状态管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设备状态管理窗体初始化失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("窗体初始化失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置DataGridView列
        /// </summary>
        private void SetupDataGridView()
        {
            dgvEquipment.AutoGenerateColumns = false;
            dgvEquipment.AllowUserToAddRows = false;
            dgvEquipment.AllowUserToDeleteRows = false;
            dgvEquipment.ReadOnly = true;
            dgvEquipment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEquipment.MultiSelect = false;

            // 添加列
            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EquipmentCode",
                HeaderText = "设备编码",
                DataPropertyName = "EquipmentCode",
                Width = 120
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EquipmentName",
                HeaderText = "设备名称",
                DataPropertyName = "EquipmentName",
                Width = 150
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EquipmentType",
                HeaderText = "设备类型",
                DataPropertyName = "EquipmentType",
                Width = 100
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopName",
                HeaderText = "所属车间",
                DataPropertyName = "WorkshopName",
                Width = 120
            });

            // 状态列使用颜色显示
            var statusColumn = new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "运行状态",
                DataPropertyName = "StatusText",
                Width = 100
            };
            dgvEquipment.Columns.Add(statusColumn);

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Efficiency",
                HeaderText = "运行效率(%)",
                DataPropertyName = "Efficiency",
                Width = 100
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Temperature",
                HeaderText = "温度(°C)",
                DataPropertyName = "Temperature",
                Width = 80
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Pressure",
                HeaderText = "压力(MPa)",
                DataPropertyName = "Pressure",
                Width = 80
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Speed",
                HeaderText = "转速(rpm)",
                DataPropertyName = "Speed",
                Width = 80
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LastMaintenance",
                HeaderText = "上次维护",
                DataPropertyName = "LastMaintenance",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" }
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NextMaintenance",
                HeaderText = "下次维护",
                DataPropertyName = "NextMaintenance",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" }
            });

            dgvEquipment.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Operator",
                HeaderText = "操作员",
                DataPropertyName = "Operator",
                Width = 100
            });

            // 设置行颜色
            dgvEquipment.CellFormatting += DgvEquipment_CellFormatting;
        }

        /// <summary>
        /// 设置单元格格式
        /// </summary>
        private void DgvEquipment_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _currentEquipments.Count)
            {
                var equipment = _currentEquipments[e.RowIndex];
                
                // 根据设备状态设置行颜色
                switch (equipment.Status)
                {
                    case 0: // 停止
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(248, 215, 218);
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(114, 28, 36);
                        break;
                    case 1: // 运行
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(212, 237, 218);
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(21, 87, 36);
                        break;
                    case 2: // 故障
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(253, 236, 200);
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(133, 77, 14);
                        break;
                    case 3: // 维护
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(207, 226, 255);
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(8, 60, 130);
                        break;
                    default:
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        dgvEquipment.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                        break;
                }
            }
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
                cmbStatus.Items.Add(new { Text = "停止", Value = "0" });
                cmbStatus.Items.Add(new { Text = "运行", Value = "1" });
                cmbStatus.Items.Add(new { Text = "故障", Value = "2" });
                cmbStatus.Items.Add(new { Text = "维护", Value = "3" });
                cmbStatus.DisplayMember = "Text";
                cmbStatus.ValueMember = "Value";
                cmbStatus.SelectedIndex = 0;

                // 设备类型下拉框
                cmbType.Items.Clear();
                cmbType.Items.Add(new { Text = "全部", Value = "" });
                cmbType.Items.Add(new { Text = "加工设备", Value = "1" });
                cmbType.Items.Add(new { Text = "装配设备", Value = "2" });
                cmbType.Items.Add(new { Text = "检测设备", Value = "3" });
                cmbType.Items.Add(new { Text = "包装设备", Value = "4" });
                cmbType.Items.Add(new { Text = "运输设备", Value = "5" });
                cmbType.DisplayMember = "Text";
                cmbType.ValueMember = "Value";
                cmbType.SelectedIndex = 0;

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
            btnStop.Click += BtnStop_Click;
            btnMaintenance.Click += BtnMaintenance_Click;
            btnAlarm.Click += BtnAlarm_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnSearch.Click += BtnSearch_Click;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            dgvEquipment.SelectionChanged += DgvEquipment_SelectionChanged;
            cmbWorkshop.SelectedIndexChanged += CmbWorkshop_SelectedIndexChanged;
            cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;
            cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;

            // 绑定窗体Load事件
            this.Load += EquipmentStatusForm_Load;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void EquipmentStatusForm_Load(object sender, EventArgs e)
        {
            // 在窗体加载时加载数据，而不是在构造函数中
            LoadData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                LoadEquipments();
                UpdateButtonStates();
                UpdateSummary();
                UpdateLastUpdateTime();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载数据失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载设备数据
        /// </summary>
        private void LoadEquipments()
        {
            try
            {
                // 从数据库加载真实设备数据
                _currentEquipments = _equipmentBLL.GetAllEquipments();

                dgvEquipment.DataSource = _currentEquipments;

                lblTotal.Text = string.Format("共 {0} 台设备", _currentEquipments.Count);

                LogManager.Info(string.Format("加载设备状态数据完成，共 {0} 台设备", _currentEquipments.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载设备数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载设备数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 如果数据库加载失败，使用空列表避免程序崩溃
                _currentEquipments = new List<EquipmentStatusInfo>();
                dgvEquipment.DataSource = _currentEquipments;
                lblTotal.Text = "共 0 台设备";
            }
        }



        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            bool hasSelection = dgvEquipment.SelectedRows.Count > 0;
            var selectedEquipment = GetSelectedEquipment();

            if (hasSelection && selectedEquipment != null)
            {
                btnStart.Enabled = selectedEquipment.Status == 0 || selectedEquipment.Status == 3; // 停止或维护
                btnStop.Enabled = selectedEquipment.Status == 1; // 运行
                btnMaintenance.Enabled = selectedEquipment.Status != 3; // 非维护状态
                btnAlarm.Enabled = selectedEquipment.Status == 2; // 故障状态
            }
            else
            {
                btnStart.Enabled = false;
                btnStop.Enabled = false;
                btnMaintenance.Enabled = false;
                btnAlarm.Enabled = false;
            }
        }

        /// <summary>
        /// 更新汇总信息
        /// </summary>
        private void UpdateSummary()
        {
            try
            {
                if (_currentEquipments == null || _currentEquipments.Count == 0)
                {
                    lblSummary.Text = "运行：0 | 停止：0 | 故障：0 | 维护：0";
                    return;
                }

                int runningCount = _currentEquipments.Count(e => e.Status == 1);
                int stoppedCount = _currentEquipments.Count(e => e.Status == 0);
                int faultCount = _currentEquipments.Count(e => e.Status == 2);
                int maintenanceCount = _currentEquipments.Count(e => e.Status == 3);

                lblSummary.Text = string.Format("运行：{0} | 停止：{1} | 故障：{2} | 维护：{3}",
                    runningCount, stoppedCount, faultCount, maintenanceCount);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新汇总信息失败：{0}", ex.Message), ex);
                lblSummary.Text = "汇总信息计算失败";
            }
        }

        /// <summary>
        /// 更新最后更新时间
        /// </summary>
        private void UpdateLastUpdateTime()
        {
            lblLastUpdate.Text = string.Format("最后更新：{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
        }

        /// <summary>
        /// 获取选中的设备
        /// </summary>
        private EquipmentStatusInfo GetSelectedEquipment()
        {
            if (dgvEquipment.SelectedRows.Count == 0)
                return null;

            int selectedIndex = dgvEquipment.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < _currentEquipments.Count)
            {
                return _currentEquipments[selectedIndex];
            }

            return null;
        }

        #region 事件处理方法

        /// <summary>
        /// 启动设备
        /// </summary>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedEquipment = GetSelectedEquipment();
                if (selectedEquipment == null)
                {
                    MessageBox.Show("请选择要启动的设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedEquipment.Status != 0 && selectedEquipment.Status != 3)
                {
                    MessageBox.Show("只能启动停止或维护状态的设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要启动设备 '{0}' 吗？", selectedEquipment.EquipmentName),
                    "确认启动", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 使用真实的设备启动操作
                    bool startResult = _equipmentBLL.StartEquipment(selectedEquipment.EquipmentCode, "当前用户");

                    if (startResult)
                    {
                        // 刷新数据显示
                        LoadEquipments();
                        UpdateButtonStates();
                        UpdateSummary();

                        MessageBox.Show("设备已启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("启动设备：{0}", selectedEquipment.EquipmentCode));
                    }
                    else
                    {
                        MessageBox.Show("设备启动失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("启动设备失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("启动设备失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 停止设备
        /// </summary>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedEquipment = GetSelectedEquipment();
                if (selectedEquipment == null)
                {
                    MessageBox.Show("请选择要停止的设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedEquipment.Status != 1)
                {
                    MessageBox.Show("只能停止运行中的设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要停止设备 '{0}' 吗？", selectedEquipment.EquipmentName),
                    "确认停止", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 使用真实的设备停止操作
                    bool stopResult = _equipmentBLL.StopEquipment(selectedEquipment.EquipmentCode, "当前用户");

                    if (stopResult)
                    {
                        // 刷新数据显示
                        LoadEquipments();
                        UpdateButtonStates();
                        UpdateSummary();

                        MessageBox.Show("设备已停止", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("停止设备：{0}", selectedEquipment.EquipmentCode));
                    }
                    else
                    {
                        MessageBox.Show("设备停止失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("停止设备失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("停止设备失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设备维护
        /// </summary>
        private void BtnMaintenance_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedEquipment = GetSelectedEquipment();
                if (selectedEquipment == null)
                {
                    MessageBox.Show("请选择要维护的设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedEquipment.Status == 3)
                {
                    MessageBox.Show("设备已在维护状态", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要对设备 '{0}' 进行维护吗？", selectedEquipment.EquipmentName),
                    "确认维护", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 使用真实的设备维护操作
                    bool maintenanceResult = _equipmentBLL.MaintenanceEquipment(selectedEquipment.EquipmentCode, "当前用户");

                    if (maintenanceResult)
                    {
                        // 刷新数据显示
                        LoadEquipments();
                        UpdateButtonStates();
                        UpdateSummary();

                        MessageBox.Show("设备已进入维护状态", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("设备进入维护：{0}", selectedEquipment.EquipmentCode));
                    }
                    else
                    {
                        MessageBox.Show("设备维护操作失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设备维护失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("设备维护失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设备报警处理
        /// </summary>
        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedEquipment = GetSelectedEquipment();
                if (selectedEquipment == null)
                {
                    MessageBox.Show("请选择要处理报警的设备", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (selectedEquipment.Status != 2)
                {
                    MessageBox.Show("只能处理故障状态设备的报警", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string alarmInfo = string.Format(
                    "设备报警信息\n\n" +
                    "设备编码：{0}\n" +
                    "设备名称：{1}\n" +
                    "设备类型：{2}\n" +
                    "所属车间：{3}\n" +
                    "当前状态：{4}\n" +
                    "运行效率：{5:F1}%\n" +
                    "温度：{6:F1}°C\n" +
                    "压力：{7:F2}MPa\n" +
                    "转速：{8:F0}rpm\n" +
                    "操作员：{9}\n\n" +
                    "建议：请立即检查设备并进行维护",
                    selectedEquipment.EquipmentCode,
                    selectedEquipment.EquipmentName,
                    selectedEquipment.EquipmentType,
                    selectedEquipment.WorkshopName,
                    selectedEquipment.StatusText,
                    selectedEquipment.Efficiency,
                    selectedEquipment.Temperature,
                    selectedEquipment.Pressure,
                    selectedEquipment.Speed,
                    selectedEquipment.Operator);

                MessageBox.Show(alarmInfo, "设备报警详情", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogManager.Info(string.Format("查看设备报警信息：{0}", selectedEquipment.EquipmentCode));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("处理设备报警失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("处理设备报警失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            SearchEquipments();
        }

        /// <summary>
        /// 搜索框回车
        /// </summary>
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchEquipments();
            }
        }

        /// <summary>
        /// 选择变化
        /// </summary>
        private void DgvEquipment_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        /// <summary>
        /// 车间选择变化
        /// </summary>
        private void CmbWorkshop_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchEquipments();
        }

        /// <summary>
        /// 状态选择变化
        /// </summary>
        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchEquipments();
        }

        /// <summary>
        /// 类型选择变化
        /// </summary>
        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchEquipments();
        }

        /// <summary>
        /// 定时器刷新
        /// </summary>
        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                // 从数据库重新加载设备状态（真实的实时更新）
                LoadEquipments();
                UpdateSummary();
                UpdateLastUpdateTime();

                LogManager.Info("设备状态自动刷新完成");
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设备状态自动刷新失败：{0}", ex.Message), ex);
            }
        }

        #endregion

        /// <summary>
        /// 搜索设备
        /// </summary>
        private void SearchEquipments()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                int? statusFilter = null;
                int? typeFilter = null;
                int? workshopFilter = null;

                // 解析状态过滤条件
                if (cmbStatus.SelectedItem != null)
                {
                    var statusValue = ((dynamic)cmbStatus.SelectedItem).Value;
                    if (!string.IsNullOrEmpty(statusValue))
                    {
                        statusFilter = int.Parse(statusValue);
                    }
                }

                // 解析设备类型过滤条件
                if (cmbType.SelectedItem != null)
                {
                    var typeValue = ((dynamic)cmbType.SelectedItem).Value;
                    if (!string.IsNullOrEmpty(typeValue))
                    {
                        typeFilter = int.Parse(typeValue);
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
                var searchResults = _equipmentBLL.SearchEquipments(workshopFilter, statusFilter, typeFilter,
                    string.IsNullOrEmpty(keyword) ? null : keyword);

                _currentEquipments = searchResults;
                dgvEquipment.DataSource = _currentEquipments;
                lblTotal.Text = string.Format("共 {0} 台设备", _currentEquipments.Count);

                // 更新汇总信息
                UpdateSummary();
                UpdateButtonStates();

                LogManager.Info(string.Format("搜索设备完成，条件：关键词={0}, 状态={1}, 类型={2}, 车间={3}, 结果数量={4}",
                    keyword, statusFilter, typeFilter, workshopFilter, _currentEquipments.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索设备失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("搜索设备失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体关闭时停止定时器
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (timerRefresh != null)
            {
                timerRefresh.Stop();
                timerRefresh.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}
