using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.BLL.Workshop;
using MES.Models.Workshop;
using System.Collections.Generic;
using System.Data;

namespace MES.UI.Forms.Batch
{
    /// <summary>
    /// 批次管理统一窗体
    /// 集成创建、取消等批次操作功能
    /// </summary>
    public partial class BatchManagementForm : Form
    {
        #region 私有字段

        private CreateBatch createForm;
        private CancelBatch cancelForm;
        private readonly BatchBLL _batchBLL;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化批次管理窗体
        /// </summary>
        public BatchManagementForm()
        {
            InitializeComponent();
            _batchBLL = new BatchBLL();
            InitializeEvents();
            LogManager.Info("批次管理窗体初始化完成");
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化事件绑定
        /// </summary>
        private void InitializeEvents()
        {
            // 按钮事件绑定
            this.btnCreateBatch.Click -= btnCreateBatch_Click_1;
            this.btnCreateBatch.Click += btnCreateBatch_Click_1;
            this.btnCancelBatch.Click += BtnCancelBatch_Click;
            this.btnRefresh.Click += BtnRefresh_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnBatchDetails.Click += BtnBatchDetails_Click;

            // 数据网格事件
            this.dgvBatches.SelectionChanged += DgvBatches_SelectionChanged;
            this.dgvBatches.CellDoubleClick += DgvBatches_CellDoubleClick;

            // 窗体事件
            this.Load += BatchManagementForm_Load;
        }

        /// <summary>
        /// 加载批次数据
        /// </summary>
        private void LoadBatchData()
        {
            try
            {
                // 从BLL层获取真实批次数据
                var batches = _batchBLL.GetAllBatches();
                var dataTable = ConvertBatchesToDataTable(batches);
                dgvBatches.DataSource = dataTable;

                // 设置列标题
                SetupDataGridColumns();

                // 更新统计信息
                UpdateStatistics();

                LogManager.Info(string.Format("批次数据加载完成，共 {0} 条记录", batches.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error("加载批次数据失败", ex);
                MessageBox.Show(string.Format("加载批次数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 将批次列表转换为DataTable
        /// </summary>
        private System.Data.DataTable ConvertBatchesToDataTable(System.Collections.Generic.List<BatchInfo> batches)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("批次号", typeof(string));
            table.Columns.Add("产品名称", typeof(string));
            table.Columns.Add("工单号", typeof(string));
            table.Columns.Add("批次数量", typeof(decimal));
            table.Columns.Add("已完成数量", typeof(decimal));
            table.Columns.Add("状态", typeof(string));
            table.Columns.Add("创建时间", typeof(DateTime));
            table.Columns.Add("计划完成时间", typeof(DateTime));
            table.Columns.Add("负责人", typeof(string));

            foreach (var batch in batches)
            {
                table.Rows.Add(
                    batch.BatchNumber,
                    batch.ProductName ?? "未知产品",
                    batch.ProductionOrderNumber ?? "",
                    batch.PlannedQuantity,
                    batch.ActualQuantity,
                    batch.BatchStatus,
                    batch.CreateTime,
                    batch.EndTime ?? DateTime.Now.AddDays(7),
                    batch.OperatorName ?? "未分配"
                );
            }

            return table;
        }

        /// <summary>
        /// 设置数据网格列
        /// </summary>
        private void SetupDataGridColumns()
        {
            if (dgvBatches.Columns.Count > 0)
            {
                dgvBatches.Columns["批次号"].Width = 140;
                dgvBatches.Columns["产品名称"].Width = 120;
                dgvBatches.Columns["工单号"].Width = 120;
                dgvBatches.Columns["批次数量"].Width = 80;
                dgvBatches.Columns["已完成数量"].Width = 100;
                dgvBatches.Columns["状态"].Width = 80;
                dgvBatches.Columns["创建时间"].Width = 120;
                dgvBatches.Columns["计划完成时间"].Width = 120;
                dgvBatches.Columns["负责人"].Width = 80;
            }
        }

        /// <summary>
        /// 更新统计信息
        /// </summary>
        private void UpdateStatistics()
        {
            var totalCount = dgvBatches.Rows.Count;
            lblTotal.Text = string.Format("共 {0} 条批次记录", totalCount);

            // 统计各状态数量
            int activeCount = 0, completedCount = 0, pausedCount = 0;

            foreach (DataGridViewRow row in dgvBatches.Rows)
            {
                var status = row.Cells["状态"].Value != null ? row.Cells["状态"].Value.ToString() : "";
                switch (status)
                {
                    case "进行中":
                        activeCount++;
                        break;
                    case "已完成":
                        completedCount++;
                        break;
                    case "已暂停":
                        pausedCount++;
                        break;
                }
            }

            lblStatistics.Text = string.Format("进行中: {0} | 已完成: {1} | 已暂停: {2}", activeCount, completedCount, pausedCount);
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void BatchManagementForm_Load(object sender, EventArgs e)
        {
            // 设置窗体状态
            this.WindowState = FormWindowState.Maximized;


            // 在窗体加载时加载数据，而不是在构造函数中
            LoadBatchData();
        }

        /// <summary>
        /// 创建批次按钮点击事件
        /// </summary>


        /// <summary>
        /// 取消批次按钮点击事件
        /// </summary>
        private void BtnCancelBatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBatches.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要取消的批次", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedRow = dgvBatches.SelectedRows[0];
                var batchNo = selectedRow.Cells["批次号"].Value != null ? selectedRow.Cells["批次号"].Value.ToString() : "";
                var status = selectedRow.Cells["状态"].Value != null ? selectedRow.Cells["状态"].Value.ToString() : "";

                // 检查批次状态
                if (status == "已完成" || status == "已取消")
                {
                    MessageBox.Show("已完成或已取消的批次无法再次取消", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cancelForm == null || cancelForm.IsDisposed)
                {
                    cancelForm = new CancelBatch();
                }

                cancelForm.ShowDialog(this);

                // 刷新数据
                LoadBatchData();

                LogManager.Info(string.Format("打开取消批次窗体，批次号：{0}", batchNo));
            }
            catch (Exception ex)
            {
                LogManager.Error("打开取消批次窗体失败", ex);
                MessageBox.Show(string.Format("打开取消批次窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 批次详情按钮点击事件
        /// </summary>
        private void BtnBatchDetails_Click(object sender, EventArgs e)
        {
            if (dgvBatches.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要查看的批次", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvBatches.SelectedRows[0];
            var batchNo = selectedRow.Cells["批次号"].Value != null ? selectedRow.Cells["批次号"].Value.ToString() : "";
            var productName = selectedRow.Cells["产品名称"].Value != null ? selectedRow.Cells["产品名称"].Value.ToString() : "";
            var status = selectedRow.Cells["状态"].Value != null ? selectedRow.Cells["状态"].Value.ToString() : "";

            var details = string.Format("批次详情\n\n批次号：{0}\n产品名称：{1}\n当前状态：{2}",
                batchNo, productName, status);

            MessageBox.Show(details, "批次详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadBatchData();
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchesBatches();
        }

        /// <summary>
        /// 搜索批次
        /// </summary>
        private void SearchesBatches()
        {

        }

        /// <summary>
        /// 数据网格选择变化事件
        /// </summary>
        private void DgvBatches_SelectionChanged(object sender, EventArgs e)
        {
            // 根据选中状态更新按钮状态
            var hasSelection = dgvBatches.SelectedRows.Count > 0;
            btnCancelBatch.Enabled = hasSelection;
            btnBatchDetails.Enabled = hasSelection;
        }

        /// <summary>
        /// 数据网格双击事件
        /// </summary>
        private void DgvBatches_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 双击查看批次详情
                BtnBatchDetails_Click(sender, e);
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

        private void btnCreateBatch_Click_1(object sender, EventArgs e)
        {
            // 检查是否已有窗体实例
            if (createForm != null && !createForm.IsDisposed)
            {
                createForm.Focus();
                return;
            }

            try
            {
                createForm = new CreateBatch();
                createForm.StartPosition = FormStartPosition.CenterParent;
                createForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                createForm.MaximizeBox = false;
                createForm.MinimizeBox = false;

                var result = createForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    // 刷新批次数据
                    LoadBatchData();
                    MessageBox.Show("批次创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("创建批次过程中发生错误", ex);
                MessageBox.Show(string.Format("创建批次失败：{0}", ex.Message),
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (createForm != null)
                {
                    createForm.Dispose();
                    createForm = null;
                }
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 从DataTable更新统计信息
        /// </summary>
        private void UpdateStatisticsFromDataTable(DataTable dt)
        {
            int total = dt.Rows.Count;
            int inProgress = 0;
            int completed = 0;
            int paused = 0;

            foreach (DataRow row in dt.Rows)
            {
                string status = row["状态"] != null ? row["状态"].ToString() : "";
                switch (status)
                {
                    case "进行中":
                        inProgress++;
                        break;
                    case "已完成":
                        completed++;
                        break;
                    case "已暂停":
                        paused++;
                        break;
                }
            }

            lblTotal.Text = "共 " + total + " 条记录";
            lblStatistics.Text = string.Format("进行中: {0} | 已完成: {1} | 已暂停: {2}",
                inProgress, completed, paused);
        }
        private void BatchManagementForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
