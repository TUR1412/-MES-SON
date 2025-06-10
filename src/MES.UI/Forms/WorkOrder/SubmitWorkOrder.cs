using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MES.BLL.WorkOrder;
using MES.Models.WorkOrder;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.UI.Forms.WorkOrder
{
    /// <summary>
    /// 提交工单窗体 - 现代化UI设计
    /// 功能：提交已创建的生产工单，进入生产准备状态
    /// </summary>
    public partial class SubmitWorkOrder : Form
    {
        #region 私有字段

        private WorkOrderBLL workOrderBLL;
        private DataTable workOrderDataTable;
        private string selectedWorkOrderNo;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化提交工单窗体
        /// </summary>
        public SubmitWorkOrder()
        {
            InitializeComponent();
            InitializeBusinessLogic();
            InitializeUI();
            LoadWorkOrderData();
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
                LogManager.Info("提交工单窗体业务逻辑初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("提交工单窗体业务逻辑初始化失败", ex);
                throw new MESException("业务逻辑初始化失败", ex);
            }
        }

        /// <summary>
        /// 初始化UI界面
        /// </summary>
        private void InitializeUI()
        {
            try
            {
                // 设置窗体属性
                this.Text = "提交工单 - MES系统";
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;

                // 设置现代化样式
                this.BackColor = Color.FromArgb(240, 244, 248);
                this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);

                // 初始化控件状态
                btnSubmit.Enabled = false;
                txtSubmitRemark.MaxLength = 500;

                // 设置数据网格样式
                SetupDataGridView();

                LogManager.Info("提交工单窗体UI初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("提交工单窗体UI初始化失败", ex);
                throw new MESException("UI初始化失败", ex);
            }
        }

        /// <summary>
        /// 设置数据网格样式
        /// </summary>
        private void SetupDataGridView()
        {
            try
            {
                // 基本样式设置
                dgvWorkOrders.BackgroundColor = Color.White;
                dgvWorkOrders.BorderStyle = BorderStyle.None;
                dgvWorkOrders.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvWorkOrders.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgvWorkOrders.RowHeadersVisible = false;
                dgvWorkOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvWorkOrders.MultiSelect = false;
                dgvWorkOrders.AllowUserToAddRows = false;
                dgvWorkOrders.AllowUserToDeleteRows = false;
                dgvWorkOrders.AllowUserToResizeRows = false;
                dgvWorkOrders.ReadOnly = true;

                // 现代化颜色方案
                dgvWorkOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 204, 113);
                dgvWorkOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvWorkOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
                dgvWorkOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvWorkOrders.ColumnHeadersHeight = 35;

                dgvWorkOrders.DefaultCellStyle.BackColor = Color.White;
                dgvWorkOrders.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
                dgvWorkOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 255, 230);
                dgvWorkOrders.DefaultCellStyle.SelectionForeColor = Color.FromArgb(64, 64, 64);
                dgvWorkOrders.DefaultCellStyle.Font = new Font("Microsoft YaHei UI", 9F);
                dgvWorkOrders.RowTemplate.Height = 30;

                dgvWorkOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

                LogManager.Info("数据网格样式设置完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("数据网格样式设置失败", ex);
            }
        }

        #endregion

        #region 数据操作方法

        /// <summary>
        /// 加载工单数据
        /// </summary>
        private void LoadWorkOrderData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // 只加载可提交状态的工单（已创建但未提交）
                workOrderDataTable = workOrderBLL.GetSubmittableWorkOrders();

                if (workOrderDataTable != null && workOrderDataTable.Rows.Count > 0)
                {
                    dgvWorkOrders.DataSource = workOrderDataTable;
                    SetupDataGridColumns();
                    lblWorkOrderCount.Text = string.Format("共 {0} 条可提交工单", workOrderDataTable.Rows.Count);
                }
                else
                {
                    dgvWorkOrders.DataSource = null;
                    lblWorkOrderCount.Text = "暂无可提交的工单";
                }

                LogManager.Info(string.Format("加载工单数据完成，共 {0} 条记录",
                    workOrderDataTable != null ? workOrderDataTable.Rows.Count : 0));
            }
            catch (Exception ex)
            {
                LogManager.Error("加载工单数据失败", ex);
                MessageBox.Show(string.Format("加载工单数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 设置数据网格列
        /// </summary>
        private void SetupDataGridColumns()
        {
            try
            {
                if (dgvWorkOrders.Columns.Count > 0)
                {
                    // 设置列标题和宽度
                    dgvWorkOrders.Columns["WorkOrderNo"].HeaderText = "工单号";
                    dgvWorkOrders.Columns["WorkOrderNo"].Width = 120;

                    dgvWorkOrders.Columns["WorkOrderType"].HeaderText = "工单类型";
                    dgvWorkOrders.Columns["WorkOrderType"].Width = 100;

                    dgvWorkOrders.Columns["ProductCode"].HeaderText = "产品编号";
                    dgvWorkOrders.Columns["ProductCode"].Width = 120;

                    dgvWorkOrders.Columns["PlanQuantity"].HeaderText = "计划数量";
                    dgvWorkOrders.Columns["PlanQuantity"].Width = 80;

                    dgvWorkOrders.Columns["Status"].HeaderText = "状态";
                    dgvWorkOrders.Columns["Status"].Width = 80;

                    dgvWorkOrders.Columns["CreatedDate"].HeaderText = "创建时间";
                    dgvWorkOrders.Columns["CreatedDate"].Width = 120;

                    dgvWorkOrders.Columns["CreatedBy"].HeaderText = "创建人";
                    dgvWorkOrders.Columns["CreatedBy"].Width = 80;

                    // 隐藏不需要显示的列
                    if (dgvWorkOrders.Columns.Contains("Id"))
                        dgvWorkOrders.Columns["Id"].Visible = false;
                }

                LogManager.Info("数据网格列设置完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("数据网格列设置失败", ex);
            }
        }

        #endregion

        #region 事件处理方法

        /// <summary>
        /// 工单选择变化事件
        /// </summary>
        private void dgvWorkOrders_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvWorkOrders.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvWorkOrders.SelectedRows[0];
                    selectedWorkOrderNo = selectedRow.Cells["WorkOrderNo"].Value != null ? selectedRow.Cells["WorkOrderNo"].Value.ToString() : null;

                    // 显示工单详细信息
                    ShowWorkOrderDetails(selectedRow);

                    // 启用提交按钮
                    btnSubmit.Enabled = !string.IsNullOrEmpty(selectedWorkOrderNo);
                }
                else
                {
                    selectedWorkOrderNo = null;
                    ClearWorkOrderDetails();
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("工单选择变化处理失败", ex);
            }
        }

        /// <summary>
        /// 显示工单详细信息
        /// </summary>
        private void ShowWorkOrderDetails(DataGridViewRow row)
        {
            try
            {
                lblWorkOrderNo.Text = row.Cells["WorkOrderNo"].Value != null ? row.Cells["WorkOrderNo"].Value.ToString() : "";
                lblWorkOrderType.Text = row.Cells["WorkOrderType"].Value != null ? row.Cells["WorkOrderType"].Value.ToString() : "";
                lblProductCode.Text = row.Cells["ProductCode"].Value != null ? row.Cells["ProductCode"].Value.ToString() : "";
                lblPlanQuantity.Text = row.Cells["PlanQuantity"].Value != null ? row.Cells["PlanQuantity"].Value.ToString() : "";
                lblStatus.Text = row.Cells["Status"].Value != null ? row.Cells["Status"].Value.ToString() : "";
                lblCreatedBy.Text = row.Cells["CreatedBy"].Value != null ? row.Cells["CreatedBy"].Value.ToString() : "";
                lblCreatedDate.Text = row.Cells["CreatedDate"].Value != null ?
                    Convert.ToDateTime(row.Cells["CreatedDate"].Value).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
            catch (Exception ex)
            {
                LogManager.Error("显示工单详细信息失败", ex);
            }
        }

        /// <summary>
        /// 清空工单详细信息
        /// </summary>
        private void ClearWorkOrderDetails()
        {
            lblWorkOrderNo.Text = "";
            lblWorkOrderType.Text = "";
            lblProductCode.Text = "";
            lblPlanQuantity.Text = "";
            lblStatus.Text = "";
            lblCreatedBy.Text = "";
            lblCreatedDate.Text = "";
        }

        /// <summary>
        /// 提交按钮点击事件
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedWorkOrderNo))
                {
                    MessageBox.Show("请先选择要提交的工单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    string.Format("确认要提交工单 [{0}] 吗？\n\n提交后工单将进入生产准备状态。", selectedWorkOrderNo),
                    "确认提交", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SubmitWorkOrderOperation();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("提交工单按钮点击处理失败", ex);
                MessageBox.Show(string.Format("操作失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行提交工单操作
        /// </summary>
        private void SubmitWorkOrderOperation()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // 调用业务逻辑层提交工单
                string submitRemark = string.IsNullOrWhiteSpace(txtSubmitRemark.Text) ? "正常提交" : txtSubmitRemark.Text.Trim();
                bool success = workOrderBLL.SubmitWorkOrder(selectedWorkOrderNo, submitRemark);

                if (success)
                {
                    MessageBox.Show("工单提交成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("工单 [{0}] 提交成功，备注：{1}",
                        selectedWorkOrderNo, submitRemark));

                    // 清空提交备注
                    txtSubmitRemark.Clear();

                    // 重新加载数据
                    LoadWorkOrderData();

                    // 如果没有更多可提交的工单，关闭窗体
                    if (workOrderDataTable == null || workOrderDataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("已无可提交的工单，窗体将关闭。", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("工单提交失败，请检查工单状态或联系系统管理员！", "失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("执行提交工单操作失败", ex);
                MessageBox.Show(string.Format("提交工单失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtSubmitRemark.Clear();
                LoadWorkOrderData();
                MessageBox.Show("数据已刷新！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show(string.Format("刷新数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 窗体事件

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void SubmitWorkOrder_Load(object sender, EventArgs e)
        {
            try
            {
                LogManager.Info("提交工单窗体加载完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("提交工单窗体加载失败", ex);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        private void SubmitWorkOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                LogManager.Info("提交工单窗体已关闭");
            }
            catch (Exception ex)
            {
                LogManager.Error("提交工单窗体关闭处理失败", ex);
            }
        }

        #endregion
    }
}
