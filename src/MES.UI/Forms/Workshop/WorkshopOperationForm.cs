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
using MES.UI.Framework.Controls;

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
        private readonly IWorkshopOperationBLL _workshopOperationBLL;
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
            _workshopOperationBLL = new WorkshopOperationBLL();
            _currentOperations = new List<WorkshopOperationInfo>();
            _workshops = new List<WorkshopInfo>();

            InitializeForm();
            ApplyDeepLeagueTheme();
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
        /// 应用优化的英雄联盟主题样式 - 车间作业管理专用
        /// 严格遵循C# 5.0语法规范
        /// </summary>
        private void ApplyDeepLeagueTheme()
        {
            try
            {
                // 设置窗体基础样式（保持原有边框）
                this.BackColor = LeagueColors.DarkestBackground;

                // 应用简化的LOL样式到所有控件
                ApplySimpleLeagueStyleToAllControls();

                // 特殊处理DataGridView
                if (dgvOperations != null)
                {
                    ApplyLeagueStyleToDataGridView(dgvOperations);
                }

                // 启用基础视觉效果
                EnableBasicVisualEffects();
            }
            catch (Exception ex)
            {
                LogManager.Error("应用LOL主题失败", ex);
                MessageBox.Show(string.Format("应用LOL主题失败: {0}", ex.Message), "主题错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 应用真正的LOL样式到所有控件 - 递归遍历整个窗体
        /// </summary>
        private void ApplySimpleLeagueStyleToAllControls()
        {
            ApplyLeagueStyleToControlsRecursive(this);
        }

        /// <summary>
        /// 递归应用真正的LOL样式到控件及其子控件
        /// </summary>
        private void ApplyLeagueStyleToControlsRecursive(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Panel)
                {
                    ApplyLeagueStyleToPanel(control as Panel);
                }
                else if (control is Label)
                {
                    ApplyLeagueStyleToLabel(control as Label);
                }
                else if (control is TextBox)
                {
                    ApplyLeagueStyleToTextBox(control as TextBox);
                }
                else if (control is ComboBox)
                {
                    ApplyLeagueStyleToComboBox(control as ComboBox);
                }
                else if (control is Button)
                {
                    ApplyLeagueStyleToButton(control as Button);
                }
                else if (control is DataGridView)
                {
                    ApplyLeagueStyleToDataGridView(control as DataGridView);
                }
                else if (control is CheckBox)
                {
                    ApplyLeagueStyleToCheckBox(control as CheckBox);
                }
                else if (control is DateTimePicker)
                {
                    ApplyLeagueStyleToDateTimePicker(control as DateTimePicker);
                }
                else if (control is GroupBox)
                {
                    ApplyLeagueStyleToGroupBox(control as GroupBox);
                }
                else if (control is NumericUpDown)
                {
                    ApplyLeagueStyleToNumericUpDown(control as NumericUpDown);
                }

                if (control.HasChildren)
                {
                    ApplyLeagueStyleToControlsRecursive(control);
                }
            }
        }

        /// <summary>
        /// 应用LOL样式到面板
        /// </summary>
        private void ApplyLeagueStyleToPanel(Panel panel)
        {
            panel.BackColor = LeagueColors.DarkSurface;
        }

        /// <summary>
        /// 应用LOL样式到标签
        /// </summary>
        private void ApplyLeagueStyleToLabel(Label label)
        {
            label.ForeColor = LeagueColors.TextPrimary;
            label.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 应用LOL样式到文本框
        /// </summary>
        private void ApplyLeagueStyleToTextBox(TextBox textBox)
        {
            textBox.BackColor = LeagueColors.InputBackground;
            textBox.ForeColor = LeagueColors.TextPrimary;
            textBox.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            textBox.BorderStyle = BorderStyle.None;
            textBox.Paint += TextBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到下拉框
        /// </summary>
        private void ApplyLeagueStyleToComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = LeagueColors.InputBackground;
            comboBox.ForeColor = LeagueColors.TextPrimary;
            comboBox.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Paint += ComboBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到按钮
        /// </summary>
        private void ApplyLeagueStyleToButton(Button button)
        {
            button.BackColor = Color.Transparent;
            button.Font = new Font("微软雅黑", 11F, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            // 根据按钮功能应用不同样式（车间管理红色主题）
            if (button.Name.Contains("Start") || button.Text.Contains("开始"))
            {
                button.ForeColor = LeagueColors.TextWhite;
                button.Paint += BtnStart_LeaguePaint;
            }
            else if (button.Name.Contains("Pause") || button.Text.Contains("暂停"))
            {
                button.ForeColor = LeagueColors.TextWhite;
                button.Paint += BtnPause_LeaguePaint;
            }
            else if (button.Name.Contains("Stop") || button.Text.Contains("停止"))
            {
                button.ForeColor = LeagueColors.TextWhite;
                button.Paint += BtnStop_LeaguePaint;
            }
            else if (button.Name.Contains("Complete") || button.Text.Contains("完成"))
            {
                button.ForeColor = LeagueColors.TextWhite;
                button.Paint += BtnComplete_LeaguePaint;
            }
            else
            {
                button.ForeColor = LeagueColors.TextPrimary;
                button.Paint += BtnDefault_LeaguePaint;
            }
        }

        /// <summary>
        /// 应用LOL样式到DataGridView
        /// </summary>
        private void ApplyLeagueStyleToDataGridView(DataGridView dataGridView)
        {
            if (dataGridView != null)
            {
                dataGridView.BackgroundColor = LeagueColors.DarkSurface;
                dataGridView.GridColor = LeagueColors.DarkBorder;
                dataGridView.DefaultCellStyle.BackColor = LeagueColors.DarkSurface;
                dataGridView.DefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
                dataGridView.DefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
                dataGridView.DefaultCellStyle.SelectionBackColor = LeagueColors.ErrorRed;
                dataGridView.DefaultCellStyle.SelectionForeColor = LeagueColors.TextWhite;

                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = LeagueColors.ErrorRed;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = LeagueColors.TextWhite;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
                dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = LeagueColors.ErrorRed;

                dataGridView.EnableHeadersVisualStyles = false;
                dataGridView.BorderStyle = BorderStyle.None;
                dataGridView.Paint += DataGridView_LeaguePaint;
            }
        }

        /// <summary>
        /// 应用LOL样式到复选框
        /// </summary>
        private void ApplyLeagueStyleToCheckBox(CheckBox checkBox)
        {
            checkBox.ForeColor = LeagueColors.TextPrimary;
            checkBox.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            checkBox.BackColor = Color.Transparent;
            checkBox.Paint += CheckBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到日期选择器
        /// </summary>
        private void ApplyLeagueStyleToDateTimePicker(DateTimePicker dateTimePicker)
        {
            dateTimePicker.BackColor = LeagueColors.InputBackground;
            dateTimePicker.ForeColor = LeagueColors.TextPrimary;
            dateTimePicker.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            dateTimePicker.Paint += DateTimePicker_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到分组框
        /// </summary>
        private void ApplyLeagueStyleToGroupBox(GroupBox groupBox)
        {
            groupBox.ForeColor = LeagueColors.ErrorRed;
            groupBox.Font = new Font("微软雅黑", 11F, FontStyle.Bold);
            groupBox.BackColor = Color.Transparent;
            groupBox.Paint += GroupBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到数字输入框
        /// </summary>
        private void ApplyLeagueStyleToNumericUpDown(NumericUpDown numericUpDown)
        {
            numericUpDown.BackColor = LeagueColors.InputBackground;
            numericUpDown.ForeColor = LeagueColors.TextPrimary;
            numericUpDown.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            numericUpDown.BorderStyle = BorderStyle.None;
            numericUpDown.Paint += NumericUpDown_LeaguePaint;
        }

        /// <summary>
        /// 启用基础视觉效果
        /// </summary>
        private void EnableBasicVisualEffects()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);
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
                // 从BLL层获取真实作业数据
                _currentOperations = _workshopOperationBLL.GetAllOperations();

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
        /// 从数据库获取真实作业数据
        /// </summary>
        private List<WorkshopOperationInfo> LoadRealOperations()
        {
            try
            {
                // 从BLL层获取真实的车间作业数据
                var operations = _workshopOperationBLL.GetAllOperations();
                if (operations != null && operations.Count > 0)
                {
                    LogManager.Info(string.Format("成功加载车间作业数据，共 {0} 条记录", operations.Count));
                    return operations;
                }
                else
                {
                    LogManager.Info("未找到车间作业数据");
                    return new List<WorkshopOperationInfo>();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("加载车间作业数据失败", ex);
                MessageBox.Show("加载作业数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<WorkshopOperationInfo>();
            }
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
                    // 更新内存中的状态
                    selectedOperation.Status = 1;
                    selectedOperation.StatusText = "进行中";
                    selectedOperation.StartTime = DateTime.Now;

                    // 保存到数据库
                    bool saveResult = _workshopOperationBLL.UpdateOperationStatus(selectedOperation.OperationId, 1, DateTime.Now);

                    if (saveResult)
                    {
                        dgvOperations.Refresh();
                        UpdateButtonStates();

                        MessageBox.Show("作业已开始", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("开始作业：{0}", selectedOperation.OperationId));
                    }
                    else
                    {
                        // 如果保存失败，恢复原状态
                        selectedOperation.Status = 0;
                        selectedOperation.StatusText = "待开始";
                        selectedOperation.StartTime = null;
                        MessageBox.Show("开始作业失败，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                    // 更新内存中的状态
                    int originalStatus = selectedOperation.Status;
                    string originalStatusText = selectedOperation.StatusText;

                    selectedOperation.Status = 2;
                    selectedOperation.StatusText = "已暂停";

                    // 保存到数据库
                    bool saveResult = _workshopOperationBLL.UpdateOperationStatus(selectedOperation.OperationId, 2, null);

                    if (saveResult)
                    {
                        dgvOperations.Refresh();
                        UpdateButtonStates();

                        MessageBox.Show("作业已暂停", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("暂停作业：{0}", selectedOperation.OperationId));
                    }
                    else
                    {
                        // 如果保存失败，恢复原状态
                        selectedOperation.Status = originalStatus;
                        selectedOperation.StatusText = originalStatusText;
                        MessageBox.Show("暂停作业失败，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                    // 更新内存中的状态
                    int originalStatus = selectedOperation.Status;
                    string originalStatusText = selectedOperation.StatusText;

                    selectedOperation.Status = 4;
                    selectedOperation.StatusText = "已停止";

                    // 保存到数据库
                    bool saveResult = _workshopOperationBLL.UpdateOperationStatus(selectedOperation.OperationId, 4, null);

                    if (saveResult)
                    {
                        dgvOperations.Refresh();
                        UpdateButtonStates();

                        MessageBox.Show("作业已停止", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("停止作业：{0}", selectedOperation.OperationId));
                    }
                    else
                    {
                        // 如果保存失败，恢复原状态
                        selectedOperation.Status = originalStatus;
                        selectedOperation.StatusText = originalStatusText;
                        MessageBox.Show("停止作业失败，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                    // 更新内存中的状态
                    int originalStatus = selectedOperation.Status;
                    string originalStatusText = selectedOperation.StatusText;
                    decimal originalProgress = selectedOperation.Progress;

                    selectedOperation.Status = 3;
                    selectedOperation.StatusText = "已完成";
                    selectedOperation.Progress = 100;

                    // 保存到数据库
                    bool saveResult = _workshopOperationBLL.UpdateOperationStatus(selectedOperation.OperationId, 3, null);

                    if (saveResult)
                    {
                        dgvOperations.Refresh();
                        UpdateButtonStates();

                        MessageBox.Show("作业已完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("完成作业：{0}", selectedOperation.OperationId));
                    }
                    else
                    {
                        // 如果保存失败，恢复原状态
                        selectedOperation.Status = originalStatus;
                        selectedOperation.StatusText = originalStatusText;
                        selectedOperation.Progress = originalProgress;
                        MessageBox.Show("完成作业失败，请重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// 面板深度绘制事件
        /// </summary>
        private void Panel_DeepPaint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = panel.ClientRectangle;

            LeagueVisualEffects.DrawEnhancedLeaguePanel(g, bounds, panel);

            // 绘制车间主题装饰（红色六边形）
            if (bounds.Width > 100 && bounds.Height > 100)
            {
                LOLGeometry.DrawGlowingHexagon(g, bounds.Right - 30, bounds.Y + 30, 12, LeagueColors.ErrorRed);
            }
        }

        /// <summary>
        /// 标签深度绘制事件
        /// </summary>
        private void Label_DeepPaint(object sender, PaintEventArgs e)
        {
            Label label = sender as Label;
            if (label == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = label.ClientRectangle;

            using (var brush = new SolidBrush(Color.FromArgb(15, LeagueColors.ErrorRed)))
            {
                g.FillRectangle(brush, bounds);
            }

            using (var pen = new Pen(LeagueColors.ErrorRed, 2))
            {
                g.DrawLine(pen, 0, 0, 0, bounds.Height);
            }
        }

        /// <summary>
        /// 文本框深度绘制事件
        /// </summary>
        private void TextBox_DeepPaint(object sender, PaintEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, textBox.Width, textBox.Height);

            using (var brush = LOLGradients.CreateCardBackgroundGradient(bounds))
            {
                g.FillRectangle(brush, bounds);
            }

            Color borderColor = textBox.Focused ? LeagueColors.ErrorRed : LeagueColors.PrimaryGold;
            using (var pen = new Pen(borderColor, 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }
        }

        /// <summary>
        /// 下拉框深度绘制事件
        /// </summary>
        private void ComboBox_DeepPaint(object sender, PaintEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, comboBox.Width, comboBox.Height);

            using (var brush = LOLGradients.CreateCardBackgroundGradient(bounds))
            {
                g.FillRectangle(brush, bounds);
            }

            using (var pen = new Pen(LeagueColors.ErrorRed, 2))
            {
                g.DrawRectangle(pen, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }
        }

        /// <summary>
        /// DataGridView深度绘制事件
        /// </summary>
        private void DataGridView_DeepPaint(object sender, PaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, dgv.Width, dgv.Height);

            using (var pen = new Pen(LeagueColors.ErrorRed, 3))
            {
                g.DrawRectangle(pen, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }
        }

        /// <summary>
        /// 开始按钮绘制事件
        /// </summary>
        private void BtnStart_DeepPaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            LeagueVisualEffects.DrawLeagueButton(g, bounds, isHovered, isPressed, button.Text, button.Font);
        }

        /// <summary>
        /// 暂停按钮绘制事件
        /// </summary>
        private void BtnPause_DeepPaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            using (var brush = new SolidBrush(Color.FromArgb(200, 150, 0)))
            {
                g.FillRectangle(brush, bounds);
            }

            using (var pen = new Pen(LeagueColors.TextGold, 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            using (var textBrush = new SolidBrush(LeagueColors.TextWhite))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(button.Text, button.Font, textBrush, bounds, sf);
            }
        }

        /// <summary>
        /// 停止按钮绘制事件
        /// </summary>
        private void BtnStop_DeepPaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            using (var brush = new SolidBrush(LeagueColors.ErrorRed))
            {
                g.FillRectangle(brush, bounds);
            }

            using (var pen = new Pen(Color.FromArgb(200, 0, 0), 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            using (var textBrush = new SolidBrush(LeagueColors.TextWhite))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(button.Text, button.Font, textBrush, bounds, sf);
            }
        }

        /// <summary>
        /// 完成按钮绘制事件
        /// </summary>
        private void BtnComplete_DeepPaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            using (var brush = new SolidBrush(LeagueColors.SuccessGreen))
            {
                g.FillRectangle(brush, bounds);
            }

            using (var pen = new Pen(Color.FromArgb(0, 150, 0), 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            using (var textBrush = new SolidBrush(LeagueColors.TextWhite))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(button.Text, button.Font, textBrush, bounds, sf);
            }
        }

        /// <summary>
        /// 默认按钮绘制事件
        /// </summary>
        private void BtnDefault_DeepPaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            using (var brush = new SolidBrush(LeagueColors.DarkSurface))
            {
                g.FillRectangle(brush, bounds);
            }

            using (var pen = new Pen(LeagueColors.DarkBorder, 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            using (var textBrush = new SolidBrush(LeagueColors.TextPrimary))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(button.Text, button.Font, textBrush, bounds, sf);
            }
        }

        #region LOL主题绘制事件

        /// <summary>
        /// 文本框LOL风格绘制事件
        /// </summary>
        private void TextBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, textBox.Width, textBox.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 车间主题红色
            Color borderColor = textBox.Focused ? LeagueColors.ErrorRed : LeagueColors.BorderGold;
            using (var pen = new Pen(borderColor, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 如果有焦点，绘制内发光效果
            if (textBox.Focused)
            {
                using (var glowPen = new Pen(LeagueColors.CreateGlow(LeagueColors.ErrorRed), 1))
                {
                    Rectangle glowRect = bounds;
                    glowRect.Inflate(-1, -1);
                    g.DrawRectangle(glowPen, glowRect);
                }
            }
        }

        /// <summary>
        /// 下拉框LOL风格绘制事件
        /// </summary>
        private void ComboBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, comboBox.Width, comboBox.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 车间主题红色
            using (var pen = new Pen(LeagueColors.ErrorRed, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 绘制右侧下拉箭头
            var arrowRect = new Rectangle(bounds.Right - 25, bounds.Y + 8, 15, 10);
            using (var brush = new SolidBrush(LeagueColors.ErrorRed))
            {
                // 绘制简单的下拉箭头
                Point[] arrowPoints = new Point[]
                {
                    new Point(arrowRect.X, arrowRect.Y),
                    new Point(arrowRect.Right, arrowRect.Y),
                    new Point(arrowRect.X + arrowRect.Width / 2, arrowRect.Bottom)
                };
                g.FillPolygon(brush, arrowPoints);
            }
        }

        /// <summary>
        /// DataGridView LOL风格绘制事件
        /// </summary>
        private void DataGridView_LeaguePaint(object sender, PaintEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, dgv.Width, dgv.Height);

            // 绘制边框 - 车间主题红色
            using (var pen = new Pen(LeagueColors.ErrorRed, 3))
            {
                g.DrawRectangle(pen, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }
        }

        /// <summary>
        /// 开始按钮LOL风格绘制事件
        /// </summary>
        private void BtnStart_LeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 绘制背景渐变 - 绿色主题
            Color startColor = isPressed ? Color.FromArgb(0, 100, 0) :
                              isHovered ? LeagueColors.SuccessGreen : Color.FromArgb(0, 150, 0);
            Color endColor = isPressed ? Color.FromArgb(0, 150, 0) :
                            isHovered ? Color.FromArgb(0, 100, 0) : LeagueColors.SuccessGreen;

            using (var brush = LeagueColors.CreateVerticalGradient(bounds, startColor, endColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            using (var pen = new Pen(Color.FromArgb(0, 200, 0), 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 绘制文字
            using (var brush = new SolidBrush(LeagueColors.TextWhite))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(button.Text, button.Font, brush, bounds, sf);
            }
        }

        /// <summary>
        /// 暂停按钮LOL风格绘制事件
        /// </summary>
        private void BtnPause_LeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 绘制背景渐变 - 黄色主题
            Color startColor = isPressed ? Color.FromArgb(150, 100, 0) :
                              isHovered ? Color.FromArgb(200, 150, 0) : Color.FromArgb(180, 130, 0);
            Color endColor = isPressed ? Color.FromArgb(200, 150, 0) :
                            isHovered ? Color.FromArgb(150, 100, 0) : Color.FromArgb(200, 150, 0);

            using (var brush = LeagueColors.CreateVerticalGradient(bounds, startColor, endColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            using (var pen = new Pen(LeagueColors.TextGold, 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 绘制文字
            using (var brush = new SolidBrush(LeagueColors.TextWhite))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(button.Text, button.Font, brush, bounds, sf);
            }
        }

        /// <summary>
        /// 停止按钮LOL风格绘制事件
        /// </summary>
        private void BtnStop_LeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 绘制背景渐变 - 红色主题
            Color startColor = isPressed ? Color.FromArgb(150, 0, 0) :
                              isHovered ? LeagueColors.ErrorRed : Color.FromArgb(180, 30, 30);
            Color endColor = isPressed ? LeagueColors.ErrorRed :
                            isHovered ? Color.FromArgb(150, 0, 0) : LeagueColors.ErrorRed;

            using (var brush = LeagueColors.CreateVerticalGradient(bounds, startColor, endColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            using (var pen = new Pen(Color.FromArgb(200, 0, 0), 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 绘制文字
            using (var brush = new SolidBrush(LeagueColors.TextWhite))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(button.Text, button.Font, brush, bounds, sf);
            }
        }

        /// <summary>
        /// 完成按钮LOL风格绘制事件
        /// </summary>
        private void BtnComplete_LeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 绘制背景渐变 - 蓝色主题
            Color startColor = isPressed ? LeagueColors.AccentBlueDark :
                              isHovered ? LeagueColors.AccentBlue : Color.FromArgb(70, 130, 180);
            Color endColor = isPressed ? LeagueColors.AccentBlue :
                            isHovered ? LeagueColors.AccentBlueDark : LeagueColors.AccentBlue;

            using (var brush = LeagueColors.CreateVerticalGradient(bounds, startColor, endColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            using (var pen = new Pen(Color.FromArgb(100, 150, 200), 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 绘制文字
            using (var brush = new SolidBrush(LeagueColors.TextWhite))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(button.Text, button.Font, brush, bounds, sf);
            }
        }

        /// <summary>
        /// 默认按钮LOL风格绘制事件
        /// </summary>
        private void BtnDefault_LeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 绘制背景渐变
            Color startColor = isPressed ? LeagueColors.DarkBackground :
                              isHovered ? LeagueColors.DarkSurfaceLight : LeagueColors.DarkSurface;
            Color endColor = isPressed ? LeagueColors.DarkSurface :
                            isHovered ? LeagueColors.DarkSurface : LeagueColors.DarkBackground;

            using (var brush = LeagueColors.CreateVerticalGradient(bounds, startColor, endColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            Color borderColor = isHovered ? LeagueColors.ErrorRed : LeagueColors.DarkBorder;
            using (var pen = new Pen(borderColor, 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 绘制文字
            using (var brush = new SolidBrush(LeagueColors.TextPrimary))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(button.Text, button.Font, brush, bounds, sf);
            }
        }

        /// <summary>
        /// 复选框LOL风格绘制事件
        /// </summary>
        private void CheckBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = checkBox.ClientRectangle;

            // 绘制复选框背景
            var checkRect = new Rectangle(0, 0, 16, 16);
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, checkRect);
            }

            // 绘制复选框边框 - 车间主题红色
            using (var pen = new Pen(LeagueColors.ErrorRed, 2))
            {
                g.DrawRectangle(pen, checkRect);
            }

            // 如果选中，绘制勾选标记
            if (checkBox.Checked)
            {
                using (var pen = new Pen(LeagueColors.ErrorRed, 3))
                {
                    g.DrawLine(pen, 3, 8, 7, 12);
                    g.DrawLine(pen, 7, 12, 13, 4);
                }
            }

            // 绘制文字
            if (!string.IsNullOrEmpty(checkBox.Text))
            {
                using (var brush = new SolidBrush(LeagueColors.TextPrimary))
                {
                    var textRect = new Rectangle(20, 0, bounds.Width - 20, bounds.Height);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(checkBox.Text, checkBox.Font, brush, textRect, sf);
                }
            }
        }

        /// <summary>
        /// 日期选择器LOL风格绘制事件
        /// </summary>
        private void DateTimePicker_LeaguePaint(object sender, PaintEventArgs e)
        {
            DateTimePicker dateTimePicker = sender as DateTimePicker;
            if (dateTimePicker == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, dateTimePicker.Width, dateTimePicker.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 车间主题红色
            using (var pen = new Pen(LeagueColors.ErrorRed, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 绘制日历图标装饰
            var iconRect = new Rectangle(bounds.Right - 25, bounds.Y + 5, 16, 16);
            using (var brush = new SolidBrush(LeagueColors.ErrorRed))
            {
                g.FillRectangle(brush, iconRect);

                // 绘制简单的日历图标
                using (var pen = new Pen(LeagueColors.TextWhite, 1))
                {
                    g.DrawRectangle(pen, iconRect.X + 2, iconRect.Y + 4, iconRect.Width - 4, iconRect.Height - 6);
                    g.DrawLine(pen, iconRect.X + 5, iconRect.Y + 2, iconRect.X + 5, iconRect.Y + 6);
                    g.DrawLine(pen, iconRect.X + 11, iconRect.Y + 2, iconRect.X + 11, iconRect.Y + 6);
                }
            }
        }

        /// <summary>
        /// 分组框LOL风格绘制事件
        /// </summary>
        private void GroupBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            GroupBox groupBox = sender as GroupBox;
            if (groupBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = groupBox.ClientRectangle;

            // 绘制边框 - 车间主题红色
            using (var pen = new Pen(LeagueColors.ErrorRed, 2))
            {
                g.DrawRectangle(pen, 0, 8, bounds.Width - 1, bounds.Height - 9);
            }

            // 绘制标题背景
            if (!string.IsNullOrEmpty(groupBox.Text))
            {
                var textSize = g.MeasureString(groupBox.Text, groupBox.Font);
                var textRect = new Rectangle(10, 0, (int)textSize.Width + 10, (int)textSize.Height);

                using (var brush = new SolidBrush(LeagueColors.DarkSurface))
                {
                    g.FillRectangle(brush, textRect);
                }

                // 绘制标题文字
                using (var brush = new SolidBrush(LeagueColors.ErrorRed))
                {
                    g.DrawString(groupBox.Text, groupBox.Font, brush, 15, 0);
                }
            }
        }

        /// <summary>
        /// 数字输入框LOL风格绘制事件
        /// </summary>
        private void NumericUpDown_LeaguePaint(object sender, PaintEventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (numericUpDown == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, numericUpDown.Width, numericUpDown.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 车间主题红色
            using (var pen = new Pen(LeagueColors.ErrorRed, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 绘制右侧上下箭头区域
            var arrowRect = new Rectangle(bounds.Right - 20, bounds.Y + 1, 18, bounds.Height - 2);
            using (var brush = new SolidBrush(LeagueColors.DarkSurfaceLight))
            {
                g.FillRectangle(brush, arrowRect);
            }

            // 绘制上箭头
            var upArrowRect = new Rectangle(arrowRect.X + 4, arrowRect.Y + 3, 10, 6);
            using (var brush = new SolidBrush(LeagueColors.ErrorRed))
            {
                Point[] upArrowPoints = new Point[]
                {
                    new Point(upArrowRect.X + 5, upArrowRect.Y),
                    new Point(upArrowRect.X, upArrowRect.Bottom),
                    new Point(upArrowRect.Right, upArrowRect.Bottom)
                };
                g.FillPolygon(brush, upArrowPoints);
            }

            // 绘制下箭头
            var downArrowRect = new Rectangle(arrowRect.X + 4, arrowRect.Y + arrowRect.Height - 9, 10, 6);
            using (var brush = new SolidBrush(LeagueColors.ErrorRed))
            {
                Point[] downArrowPoints = new Point[]
                {
                    new Point(downArrowRect.X, downArrowRect.Y),
                    new Point(downArrowRect.Right, downArrowRect.Y),
                    new Point(downArrowRect.X + 5, downArrowRect.Bottom)
                };
                g.FillPolygon(brush, downArrowPoints);
            }
        }

        #endregion
    }
}
