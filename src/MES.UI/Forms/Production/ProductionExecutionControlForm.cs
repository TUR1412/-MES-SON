using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.BLL.Production;
using MES.Models.Production;

namespace MES.UI.Forms.Production
{
    /// <summary>
    /// 生产执行控制窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class ProductionExecutionControlForm : Form
    {
        private readonly IProductionOrderBLL _productionOrderBLL;

        private List<ProductionOrderInfo> executionList;
        private List<ProductionOrderInfo> filteredExecutionList;
        private ProductionOrderInfo currentExecution;
        private Timer refreshTimer;

        public ProductionExecutionControlForm()
        {
            InitializeComponent();
            _productionOrderBLL = new ProductionOrderBLL();
            InitializeForm();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // 初始化数据
                executionList = new List<ProductionOrderInfo>();
                filteredExecutionList = new List<ProductionOrderInfo>();
                currentExecution = null;

                // 设置DataGridView
                SetupDataGridView();

                // 加载数据（离线/数据库由 BLL 决定）
                LoadData();

                // 初始化定时器
                InitializeTimer();

                // 刷新显示
                RefreshDataGridView();

                // 清空详情面板
                ClearDetailsPanel();

                LogManager.Info("生产执行控制窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("生产执行控制窗体初始化失败", ex);
                MessageBox.Show("窗体初始化失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置DataGridView
        /// </summary>
        private void SetupDataGridView()
        {
            // 设置列
            dataGridViewExecution.AutoGenerateColumns = false;
            dataGridViewExecution.Columns.Clear();

            dataGridViewExecution.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNumber",
                HeaderText = "订单编号",
                DataPropertyName = "OrderNumber",
                Width = 140,
                ReadOnly = true
            });

            dataGridViewExecution.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewExecution.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "执行状态",
                DataPropertyName = "Status",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewExecution.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Progress",
                HeaderText = "进度",
                DataPropertyName = "ProgressText",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewExecution.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Workshop",
                HeaderText = "执行车间",
                DataPropertyName = "Workshop",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewExecution.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Operator",
                HeaderText = "操作员",
                DataPropertyName = "Operator",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewExecution.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StartTime",
                HeaderText = "开始时间",
                DataPropertyName = "ActualStartTime",
                Width = 140,
                ReadOnly = true
            });

            // 设置样式
            dataGridViewExecution.EnableHeadersVisualStyles = false;
            dataGridViewExecution.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewExecution.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewExecution.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            dataGridViewExecution.ColumnHeadersHeight = 40;

            dataGridViewExecution.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
            dataGridViewExecution.DefaultCellStyle.BackColor = Color.White;
            dataGridViewExecution.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewExecution.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewExecution.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridViewExecution.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridViewExecution.GridColor = Color.FromArgb(222, 226, 230);
        }

        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 5000; // 5秒刷新一次
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        /// <summary>
        /// 定时器刷新事件
        /// </summary>
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ReloadAndRefresh(true);
            }
            catch (Exception ex)
            {
                LogManager.Error("定时刷新失败", ex);
            }
        }

        /// <summary>
        /// 加载执行数据（离线/数据库由 BLL 决定）
        /// </summary>
        private void LoadData()
        {
            executionList.Clear();

            var data = _productionOrderBLL.GetAllProductionOrders();
            if (data != null && data.Count > 0)
            {
                executionList.AddRange(data);
            }

            filteredExecutionList = new List<ProductionOrderInfo>(executionList);
        }

        private void ApplySearchFilter()
        {
            string searchTerm = textBoxSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredExecutionList = new List<ProductionOrderInfo>(executionList);
                return;
            }

            filteredExecutionList = executionList.Where(o =>
                (o.OrderNumber ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (o.ProductName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (o.Status ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (o.Workshop ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (o.Operator ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        private void ReloadAndRefresh(bool preserveSelection)
        {
            var selectedId = preserveSelection && currentExecution != null ? currentExecution.Id : 0;

            LoadData();
            ApplySearchFilter();
            RefreshDataGridView();

            if (selectedId > 0)
            {
                SelectExecutionById(selectedId);
            }
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                dataGridViewExecution.DataSource = null;
                dataGridViewExecution.DataSource = filteredExecutionList;

                // 如果有数据，选中第一行
                if (filteredExecutionList.Count > 0)
                {
                    dataGridViewExecution.Rows[0].Selected = true;
                    ShowExecutionDetails(filteredExecutionList[0]);
                }
                else
                {
                    ClearDetailsPanel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据显示失败", ex);
            }
        }

        /// <summary>
        /// 搜索框文本变化事件
        /// </summary>
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ApplySearchFilter();
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索失败", ex);
                MessageBox.Show("搜索失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DataGridView选择变化事件
        /// </summary>
        private void dataGridViewExecution_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewExecution.CurrentRow != null &&
                    dataGridViewExecution.CurrentRow.DataBoundItem != null)
                {
                    var execution = dataGridViewExecution.CurrentRow.DataBoundItem as ProductionOrderInfo;
                    if (execution != null)
                    {
                        ShowExecutionDetails(execution);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("显示执行详情失败", ex);
            }
        }

        /// <summary>
        /// 显示执行详情
        /// </summary>
        private void ShowExecutionDetails(ProductionOrderInfo execution)
        {
            if (execution == null)
            {
                ClearDetailsPanel();
                return;
            }

            currentExecution = execution;

            // 填充基本信息
            textBoxOrderNumber.Text = execution.OrderNumber;
            textBoxProductName.Text = execution.ProductName;
            textBoxStatus.Text = execution.Status;
            textBoxWorkshop.Text = execution.Workshop;
            textBoxOperator.Text = execution.Operator;

            // 填充进度信息
            textBoxPlannedQuantity.Text = execution.PlannedQuantity.ToString();
            textBoxCompletedQuantity.Text = execution.CompletedQuantity.ToString();

            // 计算进度百分比
            var progressPercent = execution.PlannedQuantity > 0
                ? (double)execution.CompletedQuantity / (double)execution.PlannedQuantity * 100
                : 0;
            progressBarExecution.Value = (int)Math.Min(progressPercent, 100);
            labelProgressPercent.Text = string.Format("{0:F1}%", progressPercent);

            // 填充时间信息
            if (execution.ActualStartTime.HasValue)
            {
                labelStartTime.Text = "开始时间：" + execution.ActualStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                labelStartTime.Text = "开始时间：未开始";
            }

            if (execution.ActualEndTime.HasValue)
            {
                labelEndTime.Text = "结束时间：" + execution.ActualEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                labelEndTime.Text = "结束时间：未结束";
            }

            if (execution.CreateTime != DateTime.MinValue)
            {
                labelCreateTime.Text = "创建时间：" + execution.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                labelCreateTime.Text = "创建时间：未知";
            }
        }

        /// <summary>
        /// 清空详情面板
        /// </summary>
        private void ClearDetailsPanel()
        {
            currentExecution = null;

            textBoxOrderNumber.Text = string.Empty;
            textBoxProductName.Text = string.Empty;
            textBoxStatus.Text = string.Empty;
            textBoxWorkshop.Text = string.Empty;
            textBoxOperator.Text = string.Empty;
            textBoxPlannedQuantity.Text = string.Empty;
            textBoxCompletedQuantity.Text = string.Empty;

            progressBarExecution.Value = 0;
            labelProgressPercent.Text = "0%";

            labelStartTime.Text = "开始时间：";
            labelEndTime.Text = "结束时间：";
            labelCreateTime.Text = "创建时间：";
        }

        /// <summary>
        /// 开始执行按钮点击事件
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentExecution == null)
                {
                    MessageBox.Show("请先选择要开始执行的订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (currentExecution.Status != "待开始")
                {
                    MessageBox.Show("只能开始执行状态为'待开始'的订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var success = _productionOrderBLL.StartProductionOrder(currentExecution.Id);
                if (!success)
                {
                    MessageBox.Show("启动执行失败：请检查订单状态是否为'待开始'。", "失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ReloadAndRefresh(true);

                MessageBox.Show("订单执行已开始！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info(string.Format("开始执行订单：{0} - {1}",
                    currentExecution.OrderNumber, currentExecution.ProductName));
            }
            catch (Exception ex)
            {
                LogManager.Error("开始执行失败", ex);
                MessageBox.Show("开始执行失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 暂停执行按钮点击事件
        /// </summary>
        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentExecution == null)
                {
                    MessageBox.Show("请先选择要暂停执行的订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (currentExecution.Status != "进行中")
                {
                    MessageBox.Show("只能暂停状态为'进行中'的订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var reason = PromptText("暂停原因（可选）", "暂停原因", "手动暂停");
                var success = _productionOrderBLL.PauseProductionOrder(currentExecution.Id, reason);
                if (!success)
                {
                    MessageBox.Show("暂停执行失败：请检查订单状态是否为'进行中'。", "失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ReloadAndRefresh(true);

                MessageBox.Show("订单执行已暂停！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info(string.Format("暂停执行订单：{0} - {1}",
                    currentExecution.OrderNumber, currentExecution.ProductName));
            }
            catch (Exception ex)
            {
                LogManager.Error("暂停执行失败", ex);
                MessageBox.Show("暂停执行失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 完成执行按钮点击事件
        /// </summary>
        private void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentExecution == null)
                {
                    MessageBox.Show("请先选择要完成执行的订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (currentExecution.Status != "进行中")
                {
                    MessageBox.Show("只能完成状态为'进行中'的订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var actualQuantity = PromptDecimal("请输入实际完成数量", "完成数量", currentExecution.PlannedQuantity);
                if (actualQuantity <= 0)
                {
                    MessageBox.Show("实际完成数量必须大于0！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var success = _productionOrderBLL.CompleteProductionOrder(currentExecution.Id, actualQuantity);
                if (!success)
                {
                    MessageBox.Show("完成执行失败：请检查订单状态是否为'进行中'。", "失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ReloadAndRefresh(true);

                MessageBox.Show("订单执行已完成！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info(string.Format("完成执行订单：{0} - {1}",
                    currentExecution.OrderNumber, currentExecution.ProductName));
            }
            catch (Exception ex)
            {
                LogManager.Error("完成执行失败", ex);
                MessageBox.Show("完成执行失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空搜索框
                textBoxSearch.Text = string.Empty;

                // 重新加载数据
                ReloadAndRefresh(false);

                MessageBox.Show("数据刷新成功！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info("生产执行控制数据已刷新");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show("刷新数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                // 停止定时器
                if (refreshTimer != null)
                {
                    refreshTimer.Stop();
                    refreshTimer.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("关闭窗体时清理资源失败", ex);
            }

            base.OnFormClosed(e);
        }

        private void SelectExecutionById(int executionId)
        {
            try
            {
                for (int i = 0; i < dataGridViewExecution.Rows.Count; i++)
                {
                    var item = dataGridViewExecution.Rows[i].DataBoundItem as ProductionOrderInfo;
                    if (item != null && item.Id == executionId)
                    {
                        dataGridViewExecution.ClearSelection();
                        dataGridViewExecution.Rows[i].Selected = true;
                        dataGridViewExecution.CurrentCell = dataGridViewExecution.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("选中执行记录失败", ex);
            }
        }

        private static string PromptText(string title, string label, string defaultValue)
        {
            using (var dialog = new Form())
            {
                dialog.Text = title;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.Width = 520;
                dialog.Height = 190;

                var panel = new TableLayoutPanel();
                panel.Dock = DockStyle.Fill;
                panel.Padding = new Padding(12);
                panel.ColumnCount = 2;
                panel.RowCount = 2;
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));

                var labelControl = new Label
                {
                    Text = label + "：",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight
                };
                var input = new TextBox { Dock = DockStyle.Fill, Text = defaultValue ?? string.Empty };

                panel.Controls.Add(labelControl, 0, 0);
                panel.Controls.Add(input, 1, 0);

                var buttons = new FlowLayoutPanel();
                buttons.Dock = DockStyle.Fill;
                buttons.FlowDirection = FlowDirection.RightToLeft;

                var ok = new Button { Text = "确定", Width = 90, DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "取消", Width = 90, DialogResult = DialogResult.Cancel };
                buttons.Controls.Add(ok);
                buttons.Controls.Add(cancel);

                panel.Controls.Add(buttons, 0, 1);
                panel.SetColumnSpan(buttons, 2);

                dialog.Controls.Add(panel);
                dialog.AcceptButton = ok;
                dialog.CancelButton = cancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return input.Text;
                }

                return defaultValue ?? string.Empty;
            }
        }

        private static decimal PromptDecimal(string title, string label, decimal defaultValue)
        {
            using (var dialog = new Form())
            {
                dialog.Text = title;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.Width = 520;
                dialog.Height = 200;

                var panel = new TableLayoutPanel();
                panel.Dock = DockStyle.Fill;
                panel.Padding = new Padding(12);
                panel.ColumnCount = 2;
                panel.RowCount = 2;
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
                panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));

                var labelControl = new Label
                {
                    Text = label + "：",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight
                };
                var input = new NumericUpDown
                {
                    Dock = DockStyle.Left,
                    Width = 200,
                    DecimalPlaces = 4,
                    Maximum = 99999999,
                    Minimum = 0,
                    Value = defaultValue < 0 ? 0 : defaultValue
                };

                panel.Controls.Add(labelControl, 0, 0);
                panel.Controls.Add(input, 1, 0);

                var buttons = new FlowLayoutPanel();
                buttons.Dock = DockStyle.Fill;
                buttons.FlowDirection = FlowDirection.RightToLeft;

                var ok = new Button { Text = "确定", Width = 90, DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "取消", Width = 90, DialogResult = DialogResult.Cancel };
                buttons.Controls.Add(ok);
                buttons.Controls.Add(cancel);

                panel.Controls.Add(buttons, 0, 1);
                panel.SetColumnSpan(buttons, 2);

                dialog.Controls.Add(panel);
                dialog.AcceptButton = ok;
                dialog.CancelButton = cancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return input.Value;
                }

                return defaultValue;
            }
        }
    }
}
