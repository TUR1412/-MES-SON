using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.BLL.WorkOrder;
using MES.Models.WorkOrder;

namespace MES.UI.Forms.WorkOrder
{
    /// <summary>
    /// 工单管理统一窗体
    /// 集成创建、提交、取消等工单操作功能
    /// </summary>
    public partial class WorkOrderManagementForm : Form
    {
        #region 私有字段

        private CreateWorkOrder createForm;
        private SubmitWorkOrder submitForm;
        private CancelWorkOrder cancelForm;
        private readonly WorkOrderBLL _workOrderBLL;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化工单管理窗体
        /// </summary>
        public WorkOrderManagementForm()
        {
            InitializeComponent();
            _workOrderBLL = new WorkOrderBLL();
            InitializeEvents();
            LoadWorkOrderData();
            LogManager.Info("工单管理窗体初始化完成");
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化事件绑定
        /// </summary>
        private void InitializeEvents()
        {
            // 按钮事件绑定
            this.btnCreateWorkOrder.Click += BtnCreateWorkOrder_Click;
            this.btnSubmitWorkOrder.Click += BtnSubmitWorkOrder_Click;
            this.btnCancelWorkOrder.Click += BtnCancelWorkOrder_Click;
            this.btnRefresh.Click += BtnRefresh_Click;
            this.btnSearch.Click += BtnSearch_Click;
            
            // 数据网格事件
            this.dgvWorkOrders.SelectionChanged += DgvWorkOrders_SelectionChanged;
            this.dgvWorkOrders.CellDoubleClick += DgvWorkOrders_CellDoubleClick;
            
            // 窗体事件
            this.Load += WorkOrderManagementForm_Load;
        }

        /// <summary>
        /// 加载工单数据
        /// </summary>
        private void LoadWorkOrderData()
        {
            try
            {
                // 从BLL层获取真实工单数据
                var workOrders = _workOrderBLL.GetAllWorkOrders();
                var dataTable = ConvertWorkOrdersToDataTable(workOrders);
                dgvWorkOrders.DataSource = dataTable;

                // 设置列标题
                SetupDataGridColumns();

                // 更新统计信息
                UpdateStatistics();

                LogManager.Info(string.Format("工单数据加载完成，共 {0} 条记录", workOrders.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error("加载工单数据失败", ex);
                MessageBox.Show(string.Format("加载工单数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 将工单列表转换为DataTable
        /// </summary>
        private System.Data.DataTable ConvertWorkOrdersToDataTable(System.Collections.Generic.List<WorkOrderInfo> workOrders)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("工单号", typeof(string));
            table.Columns.Add("产品名称", typeof(string));
            table.Columns.Add("计划数量", typeof(decimal));
            table.Columns.Add("已完成数量", typeof(decimal));
            table.Columns.Add("状态", typeof(string));
            table.Columns.Add("创建时间", typeof(DateTime));
            table.Columns.Add("计划完成时间", typeof(DateTime));

            foreach (var workOrder in workOrders)
            {
                string statusText = GetStatusText(workOrder.WorkOrderStatus);
                table.Rows.Add(
                    workOrder.WorkOrderNum,
                    GetProductName(workOrder.ProductId), // 需要根据ProductId获取产品名称
                    workOrder.PlannedQuantity,
                    workOrder.OutputQuantity,
                    statusText,
                    workOrder.CreateTime,
                    workOrder.PlannedDueDate ?? DateTime.Now.AddDays(7)
                );
            }

            return table;
        }

        /// <summary>
        /// 获取状态文本
        /// </summary>
        private string GetStatusText(int status)
        {
            switch (status)
            {
                case 0: return "待开始";
                case 1: return "进行中";
                case 2: return "已完成";
                case 3: return "已关闭";
                default: return "未知";
            }
        }

        /// <summary>
        /// 根据产品ID获取产品名称（简化实现）
        /// </summary>
        private string GetProductName(int productId)
        {
            // 这里应该调用产品BLL获取产品名称，暂时返回默认值
            return string.Format("产品{0}", productId);
        }

        /// <summary>
        /// 设置数据网格列
        /// </summary>
        private void SetupDataGridColumns()
        {
            if (dgvWorkOrders.Columns.Count > 0)
            {
                dgvWorkOrders.Columns["工单号"].Width = 120;
                dgvWorkOrders.Columns["产品名称"].Width = 150;
                dgvWorkOrders.Columns["计划数量"].Width = 80;
                dgvWorkOrders.Columns["已完成数量"].Width = 100;
                dgvWorkOrders.Columns["状态"].Width = 80;
                dgvWorkOrders.Columns["创建时间"].Width = 120;
                dgvWorkOrders.Columns["计划完成时间"].Width = 120;
            }
        }

        /// <summary>
        /// 更新统计信息
        /// </summary>
        private void UpdateStatistics()
        {
            var totalCount = dgvWorkOrders.Rows.Count;
            lblTotal.Text = string.Format("共 {0} 条工单记录", totalCount);
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void WorkOrderManagementForm_Load(object sender, EventArgs e)
        {
            // 设置窗体状态
            this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 创建工单按钮点击事件
        /// </summary>
        private void BtnCreateWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (createForm == null || createForm.IsDisposed)
                {
                    createForm = new CreateWorkOrder();
                }
                
                createForm.ShowDialog(this);
                
                // 刷新数据
                LoadWorkOrderData();
                
                LogManager.Info("打开创建工单窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开创建工单窗体失败", ex);
                MessageBox.Show(string.Format("打开创建工单窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 提交工单按钮点击事件
        /// </summary>
        private void BtnSubmitWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvWorkOrders.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要提交的工单", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                if (submitForm == null || submitForm.IsDisposed)
                {
                    submitForm = new SubmitWorkOrder();
                }
                
                // 传递选中的工单信息
                var selectedRow = dgvWorkOrders.SelectedRows[0];
                var workOrderNo = selectedRow.Cells["工单号"].Value != null ? selectedRow.Cells["工单号"].Value.ToString() : "";
                
                submitForm.ShowDialog(this);
                
                // 刷新数据
                LoadWorkOrderData();
                
                LogManager.Info(string.Format("打开提交工单窗体，工单号：{0}", workOrderNo));
            }
            catch (Exception ex)
            {
                LogManager.Error("打开提交工单窗体失败", ex);
                MessageBox.Show(string.Format("打开提交工单窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消工单按钮点击事件
        /// </summary>
        private void BtnCancelWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvWorkOrders.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要取消的工单", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                if (cancelForm == null || cancelForm.IsDisposed)
                {
                    cancelForm = new CancelWorkOrder();
                }
                
                // 传递选中的工单信息
                var selectedRow = dgvWorkOrders.SelectedRows[0];
                var workOrderNo = selectedRow.Cells["工单号"].Value != null ? selectedRow.Cells["工单号"].Value.ToString() : "";
                
                cancelForm.ShowDialog(this);
                
                // 刷新数据
                LoadWorkOrderData();
                
                LogManager.Info(string.Format("打开取消工单窗体，工单号：{0}", workOrderNo));
            }
            catch (Exception ex)
            {
                LogManager.Error("打开取消工单窗体失败", ex);
                MessageBox.Show(string.Format("打开取消工单窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadWorkOrderData();
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            // TODO: 实现搜索功能
            MessageBox.Show("搜索功能开发中...", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 数据网格选择变化事件
        /// </summary>
        private void DgvWorkOrders_SelectionChanged(object sender, EventArgs e)
        {
            // 根据选中状态更新按钮状态
            var hasSelection = dgvWorkOrders.SelectedRows.Count > 0;
            btnSubmitWorkOrder.Enabled = hasSelection;
            btnCancelWorkOrder.Enabled = hasSelection;
        }

        /// <summary>
        /// 数据网格双击事件
        /// </summary>
        private void DgvWorkOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 双击查看工单详情
                var selectedRow = dgvWorkOrders.Rows[e.RowIndex];
                var workOrderNo = selectedRow.Cells["工单号"].Value != null ? selectedRow.Cells["工单号"].Value.ToString() : "";
                
                MessageBox.Show(string.Format("工单详情：{0}\n\n功能开发中...", workOrderNo), "工单详情",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region 资源清理

        /// <summary>
        /// 清理自定义资源
        /// </summary>
        private void CleanupCustomResources()
        {
            if (createForm != null) createForm.Dispose();
            if (submitForm != null) submitForm.Dispose();
            if (cancelForm != null) cancelForm.Dispose();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CleanupCustomResources();
            base.OnFormClosed(e);
        }

        #endregion
    }
}
