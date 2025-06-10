using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Models.Production;
using MES.BLL.Production;

namespace MES.UI.Forms.Production
{
    /// <summary>
    /// 生产执行控制窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class ProductionExecutionControlForm : Form
    {
        private List<ProductionOrderInfo> executionList;
        private List<ProductionOrderInfo> filteredExecutionList;
        private ProductionOrderInfo currentExecution;
        private Timer refreshTimer;
        private readonly IProductionOrderBLL _productionOrderBLL;

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

                // 加载真实数据
                LoadProductionExecutionData();

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
                // 模拟实时数据更新
                UpdateExecutionProgress();
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                LogManager.Error("定时刷新失败", ex);
            }
        }

        /// <summary>
        /// 加载生产执行数据
        /// </summary>
        private void LoadProductionExecutionData()
        {
            try
            {
                executionList.Clear();

                // 从BLL层获取真实数据
                var orders = _productionOrderBLL.GetAllProductionOrders();
                if (orders != null && orders.Count > 0)
                {
                    executionList.AddRange(orders);
                }

                // 复制到过滤列表
                filteredExecutionList = new List<ProductionOrderInfo>(executionList);

                LogManager.Info(string.Format("成功加载生产执行数据，共 {0} 条记录", executionList.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error("加载生产执行数据失败", ex);
                MessageBox.Show("加载生产执行数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 初始化空列表
                executionList = new List<ProductionOrderInfo>();
                filteredExecutionList = new List<ProductionOrderInfo>();
            }
        }

        /// <summary>
        /// 更新执行进度（模拟实时更新）
        /// </summary>
        private void UpdateExecutionProgress()
        {
            var random = new Random();

            foreach (var order in executionList)
            {
                if (order.Status == "进行中" && order.CompletedQuantity < order.PlannedQuantity)
                {
                    // 随机增加1-3个完成数量
                    var increment = random.Next(1, 4);
                    order.CompletedQuantity = Math.Min(order.CompletedQuantity + increment, order.PlannedQuantity);

                    // 如果完成了，更新状态
                    if (order.CompletedQuantity >= order.PlannedQuantity)
                    {
                        order.Status = "已完成";
                        order.ActualEndTime = DateTime.Now;
                    }
                }
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
                string searchTerm = textBoxSearch.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // 显示所有执行记录
                    filteredExecutionList = new List<ProductionOrderInfo>(executionList);
                }
                else
                {
                    // 根据订单编号、产品名称、状态、车间进行搜索
                    filteredExecutionList = executionList.Where(o =>
                        o.OrderNumber.ToLower().Contains(searchTerm) ||
                        o.ProductName.ToLower().Contains(searchTerm) ||
                        o.Status.ToLower().Contains(searchTerm) ||
                        o.Workshop.ToLower().Contains(searchTerm) ||
                        o.Operator.ToLower().Contains(searchTerm)
                    ).ToList();
                }

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

                // 更新状态
                currentExecution.Status = "进行中";
                currentExecution.ActualStartTime = DateTime.Now;

                // 刷新显示
                RefreshDataGridView();

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

                // 更新状态
                currentExecution.Status = "已暂停";

                // 刷新显示
                RefreshDataGridView();

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

                // 更新状态
                currentExecution.Status = "已完成";
                currentExecution.CompletedQuantity = currentExecution.PlannedQuantity;
                currentExecution.ActualEndTime = DateTime.Now;

                // 刷新显示
                RefreshDataGridView();

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
                LoadProductionExecutionData();
                RefreshDataGridView();

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
    }
}