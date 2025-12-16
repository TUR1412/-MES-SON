using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.BLL.Workshop;
using MES.BLL.WorkOrder;
using MES.Common.Logging;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Batch
{
    /// <summary>
    /// 创建生产批次窗体 - 现代化UI设计
    /// 功能：基于工单创建生产批次，支持批次信息配置
    /// </summary>
    public partial class CreateBatch : ThemedForm
    {
        #region 私有字段

        private WorkOrderBLL workOrderBLL;
        private BatchBLL batchBLL;
        private DataTable workOrderTable;
        private string selectedWorkOrderNo;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化创建批次窗体
        /// </summary>
        public CreateBatch()
        {
            InitializeComponent();
            InitializeBusinessLogic();
            InitializeUI();
            LoadWorkOrderData();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
            LogManager.Info("创建批次窗体初始化完成");
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化业务逻辑层
        /// </summary>
        private void InitializeBusinessLogic()
        {
            try
            {
                workOrderBLL = new WorkOrderBLL();
                batchBLL = new BatchBLL();
                workOrderTable = new DataTable();
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化业务逻辑失败", ex);
                MessageBox.Show(string.Format("初始化业务逻辑失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化UI界面
        /// </summary>
        private void InitializeUI()
        {
            try
            {
                // 设置窗体图标
                this.Icon = SystemIcons.Application;

                // 设置默认值
                dtpPlanDate.Value = DateTime.Now.AddDays(7);
                txtBatchNo.Text = GenerateRealBatchNo();

                // 绑定事件
                BindEvents();

                // 初始化工单数据网格
                InitializeWorkOrderDataGrid();

                // 设置控件状态
                SetControlStates();
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化界面失败", ex);
                MessageBox.Show(string.Format("初始化界面失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 绑定控件事件
        /// </summary>
        private void BindEvents()
        {
            // 按钮事件
            btnSearch.Click += BtnSearch_Click;
            btnCreate.Click += BtnCreate_Click;
            btnCancel.Click += BtnCancel_Click;

            // 数据网格事件
            dgvWorkOrders.SelectionChanged += DgvWorkOrders_SelectionChanged;
            dgvWorkOrders.CellDoubleClick += DgvWorkOrders_CellDoubleClick;

            // 文本框事件
            txtBatchQuantity.KeyPress += TxtBatchQuantity_KeyPress;
            txtWorkOrderNo.KeyPress += TxtWorkOrderNo_KeyPress;
        }

        /// <summary>
        /// 初始化工单数据网格
        /// </summary>
        private void InitializeWorkOrderDataGrid()
        {
            try
            {
                // 创建数据表结构
                workOrderTable.Columns.Add("工单号", typeof(string));
                workOrderTable.Columns.Add("产品名称", typeof(string));
                workOrderTable.Columns.Add("产品编号", typeof(string));
                workOrderTable.Columns.Add("计划数量", typeof(decimal));
                workOrderTable.Columns.Add("已完成数量", typeof(decimal));
                workOrderTable.Columns.Add("剩余数量", typeof(decimal));
                workOrderTable.Columns.Add("状态", typeof(string));
                workOrderTable.Columns.Add("创建日期", typeof(DateTime));

                // 绑定数据源
                dgvWorkOrders.DataSource = workOrderTable;

                // 设置列宽
                dgvWorkOrders.Columns["工单号"].Width = 120;
                dgvWorkOrders.Columns["产品名称"].Width = 150;
                dgvWorkOrders.Columns["产品编号"].Width = 120;
                dgvWorkOrders.Columns["计划数量"].Width = 80;
                dgvWorkOrders.Columns["已完成数量"].Width = 100;
                dgvWorkOrders.Columns["剩余数量"].Width = 80;
                dgvWorkOrders.Columns["状态"].Width = 80;
                dgvWorkOrders.Columns["创建日期"].Width = 120;

                // 设置列样式
                dgvWorkOrders.Columns["计划数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvWorkOrders.Columns["已完成数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvWorkOrders.Columns["剩余数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvWorkOrders.Columns["状态"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化工单数据网格失败", ex);
                MessageBox.Show(string.Format("初始化工单数据网格失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        private void SetControlStates()
        {
            // 设置只读控件
            txtBatchNo.ReadOnly = true;

            // 设置按钮状态
            btnCreate.Enabled = false;

            // 设置默认选择
            if (cmbResponsible.Items.Count > 0)
            {
                cmbResponsible.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 加载工单数据
        /// </summary>
        private void LoadWorkOrderData()
        {
            try
            {
                LoadRealWorkOrderData();
                LogManager.Info("工单数据加载完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("加载工单数据失败", ex);
                MessageBox.Show(string.Format("加载工单数据失败：{0}", ex.Message),
                    "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载真实工单数据
        /// </summary>
        private void LoadRealWorkOrderData()
        {
            try
            {
                workOrderTable.Clear();

                // 从BLL层获取真实的工单数据
                var workOrderBLL = new WorkOrderBLL();
                var workOrders = workOrderBLL.GetAllWorkOrders();

                if (workOrders != null && workOrders.Count > 0)
                {
                    foreach (var workOrder in workOrders)
                    {
                        workOrderTable.Rows.Add(
                            workOrder.WorkOrderNum,
                            workOrder.ProductName,
                            workOrder.ProductCode,
                            workOrder.PlannedQuantity,
                            workOrder.InputQuantity,
                            workOrder.OutputQuantity,
                            GetWorkOrderStatusText((int)workOrder.WorkOrderStatus),
                            workOrder.PlannedStartTime
                        );
                    }
                }

                LogManager.Info(string.Format("成功加载工单数据，共 {0} 条记录", workOrders != null ? workOrders.Count : 0));
            }
            catch (Exception ex)
            {
                LogManager.Error("加载工单数据失败", ex);
                throw;
            }
        }

        /// <summary>
        /// 获取工单状态文本
        /// </summary>
        private string GetWorkOrderStatusText(int status)
        {
            switch (status)
            {
                case 0: return "待生产";
                case 1: return "生产中";
                case 2: return "已完成";
                case 3: return "已暂停";
                case 4: return "已取消";
                default: return "未知";
            }
        }

        /// <summary>
        /// 生成批次号
        /// </summary>
        private string GenerateBatchNo()
        {
            try
            {
                // 生成格式：BATCH + 年月日 + 4位序号
                string dateStr = DateTime.Now.ToString("yyyyMMdd");
                string prefix = string.Format("BATCH{0}", dateStr);

                // 获取当日最大序号（从数据库查询）
                var batchBLL = new BatchBLL();
                int maxSeq = batchBLL.GetMaxSequenceForDate(DateTime.Now.Date);
                int nextSeq = maxSeq + 1;
                string seqStr = nextSeq.ToString("D4");

                return string.Format("{0}{1}", prefix, seqStr);
            }
            catch (Exception ex)
            {
                LogManager.Error("生成批次号失败", ex);
                // 发生错误时使用时间戳作为序号
                string timeStamp = DateTime.Now.ToString("HHmmss");
                return string.Format("BATCH{0}{1}", DateTime.Now.ToString("yyyyMMdd"), timeStamp);
            }
        }

        #endregion

        #region 事件处理方法

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string workOrderNo = txtWorkOrderNo.Text.Trim();
                if (string.IsNullOrEmpty(workOrderNo))
                {
                    LoadWorkOrderData();
                    return;
                }

                // 过滤工单数据
                FilterWorkOrderData(workOrderNo);
                LogManager.Info(string.Format("搜索工单：{0}", workOrderNo));
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索工单失败", ex);
                MessageBox.Show(string.Format("搜索工单失败：{0}", ex.Message),
                    "搜索错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 创建按钮点击事件
        /// </summary>
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (CreateBatchRecord())
                    {
                        MessageBox.Show("批次创建成功！", "操作成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("创建批次失败", ex);
                MessageBox.Show(string.Format("创建批次失败：{0}", ex.Message),
                    "创建错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要取消创建批次吗？", "确认取消",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        /// <summary>
        /// 工单选择变更事件
        /// </summary>
        private void DgvWorkOrders_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvWorkOrders.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvWorkOrders.SelectedRows[0];
                    selectedWorkOrderNo = selectedRow.Cells["工单号"].Value != null ? selectedRow.Cells["工单号"].Value.ToString() : null;

                    // 自动填充批次数量为剩余数量
                    var remainingQty = selectedRow.Cells["剩余数量"].Value;
                    if (remainingQty != null)
                    {
                        txtBatchQuantity.Text = remainingQty.ToString();
                    }

                    btnCreate.Enabled = true;
                }
                else
                {
                    selectedWorkOrderNo = null;
                    txtBatchQuantity.Text = "";
                    btnCreate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("处理工单选择变更失败", ex);
            }
        }

        /// <summary>
        /// 工单双击事件
        /// </summary>
        private void DgvWorkOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnCreate.Enabled)
            {
                // 双击直接创建批次
                BtnCreate_Click(sender, e);
            }
        }

        /// <summary>
        /// 批次数量文本框按键事件（只允许输入数字）
        /// </summary>
        private void TxtBatchQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 只允许输入数字、小数点和退格键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // 只允许一个小数点
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 工单号文本框按键事件
        /// </summary>
        private void TxtWorkOrderNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 按回车键执行搜索
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSearch_Click(sender, e);
                e.Handled = true;
            }
        }

        #endregion

        #region 业务方法

        /// <summary>
        /// 验证输入数据
        /// </summary>
        private bool ValidateInput()
        {
            try
            {
                // 验证是否选择了工单
                if (string.IsNullOrEmpty(selectedWorkOrderNo))
                {
                    MessageBox.Show("请选择要创建批次的工单。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvWorkOrders.Focus();
                    return false;
                }

                // 验证批次数量
                decimal batchQuantity;
                if (!decimal.TryParse(txtBatchQuantity.Text, out batchQuantity) || batchQuantity <= 0)
                {
                    MessageBox.Show("请输入有效的批次数量。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBatchQuantity.Focus();
                    return false;
                }

                // 验证批次数量不能超过剩余数量
                if (dgvWorkOrders.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvWorkOrders.SelectedRows[0];
                    var remainingQty = Convert.ToDecimal(selectedRow.Cells["剩余数量"].Value);

                    if (batchQuantity > remainingQty)
                    {
                        MessageBox.Show(string.Format("批次数量不能超过剩余数量（{0}）。", remainingQty),
                            "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtBatchQuantity.Focus();
                        return false;
                    }
                }

                // 验证负责人
                if (cmbResponsible.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择负责人。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbResponsible.Focus();
                    return false;
                }

                // 验证计划完成日期
                if (dtpPlanDate.Value < DateTime.Now.Date)
                {
                    MessageBox.Show("计划完成日期不能早于今天。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpPlanDate.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Error("验证输入数据时发生错误", ex);
                MessageBox.Show(string.Format("验证输入数据时发生错误：{0}", ex.Message),
                    "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 创建批次记录
        /// </summary>
        private bool CreateBatchRecord()
        {
            try
            {
                // 获取选中的工单信息
                var selectedRow = dgvWorkOrders.SelectedRows[0];

                // 创建批次对象
                BatchModel batch = new BatchModel
                {
                    BatchNo = txtBatchNo.Text.Trim(),
                    WorkOrderNo = selectedWorkOrderNo,
                    ProductCode = selectedRow.Cells["产品编号"].Value != null ? selectedRow.Cells["产品编号"].Value.ToString() : null,
                    ProductName = selectedRow.Cells["产品名称"].Value != null ? selectedRow.Cells["产品名称"].Value.ToString() : null,
                    BatchQuantity = decimal.Parse(txtBatchQuantity.Text),
                    CompletedQuantity = 0,
                    Status = "待开始",
                    ResponsiblePerson = cmbResponsible.Text,
                    PlanCompletionDate = dtpPlanDate.Value,
                    Remarks = txtRemarks.Text.Trim(),
                    CreatedBy = Environment.UserName,
                    CreatedDate = DateTime.Now
                };

                // 保存批次记录
                bool result = batchBLL.CreateBatch(batch);

                if (result)
                {
                    LogManager.Info(string.Format("批次创建成功，批次号：{0}", batch.BatchNo));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error("创建批次记录时发生错误", ex);
                MessageBox.Show(string.Format("创建批次记录时发生错误：{0}", ex.Message),
                    "创建错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 过滤工单数据
        /// </summary>
        private void FilterWorkOrderData(string workOrderNo)
        {
            try
            {
                DataView dataView = workOrderTable.DefaultView;
                dataView.RowFilter = string.Format("工单号 LIKE '%{0}%'", workOrderNo);

                if (dataView.Count == 0)
                {
                    MessageBox.Show("未找到匹配的工单。", "搜索结果",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("过滤工单数据失败", ex);
                MessageBox.Show(string.Format("过滤工单数据失败：{0}", ex.Message),
                    "搜索错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成真实的批次号（基于数据库序号）
        /// </summary>
        private string GenerateRealBatchNo()
        {
            try
            {
                // 调用BLL层获取基于数据库的批次号
                return batchBLL.GenerateNewBatchNumber();
            }
            catch (Exception ex)
            {
                LogManager.Error("生成批次号失败", ex);
                // 如果生成失败，使用时间戳作为备用方案
                return string.Format("BATCH{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
        }

        #endregion
    }
}
