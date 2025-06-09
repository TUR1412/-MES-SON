using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;

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

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化批次管理窗体
        /// </summary>
        public BatchManagementForm()
        {
            InitializeComponent();
            InitializeEvents();
            LoadBatchData();
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
            this.btnCreateBatch.Click += BtnCreateBatch_Click;
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
                // TODO: 从数据库加载批次数据
                // 这里暂时使用示例数据
                var dataTable = CreateSampleData();
                dgvBatches.DataSource = dataTable;
                
                // 设置列标题
                SetupDataGridColumns();
                
                // 更新统计信息
                UpdateStatistics();
                
                LogManager.Info("批次数据加载完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("加载批次数据失败", ex);
                MessageBox.Show(string.Format("加载批次数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 创建示例数据
        /// </summary>
        private System.Data.DataTable CreateSampleData()
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("批次号", typeof(string));
            table.Columns.Add("产品名称", typeof(string));
            table.Columns.Add("工单号", typeof(string));
            table.Columns.Add("批次数量", typeof(int));
            table.Columns.Add("已完成数量", typeof(int));
            table.Columns.Add("状态", typeof(string));
            table.Columns.Add("创建时间", typeof(DateTime));
            table.Columns.Add("计划完成时间", typeof(DateTime));
            table.Columns.Add("负责人", typeof(string));
            
            // 添加示例数据
            table.Rows.Add("BATCH202506090001", "产品A", "WO202506090001", 50, 0, "待开始", DateTime.Now, DateTime.Now.AddDays(3), "张三");
            table.Rows.Add("BATCH202506090002", "产品B", "WO202506090002", 100, 30, "进行中", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2), "李四");
            table.Rows.Add("BATCH202506090003", "产品C", "WO202506090003", 75, 75, "已完成", DateTime.Now.AddDays(-3), DateTime.Now.AddDays(-1), "王五");
            table.Rows.Add("BATCH202506090004", "产品A", "WO202506090004", 80, 0, "已暂停", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(1), "赵六");
            
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
            
            // 初始化状态下拉框
            cmbStatus.SelectedIndex = 0; // 选择"全部状态"
        }

        /// <summary>
        /// 创建批次按钮点击事件
        /// </summary>
        private void BtnCreateBatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (createForm == null || createForm.IsDisposed)
                {
                    createForm = new CreateBatch();
                }
                
                createForm.ShowDialog(this);
                
                // 刷新数据
                LoadBatchData();
                
                LogManager.Info("打开创建批次窗体");
            }
            catch (Exception ex)
            {
                LogManager.Error("打开创建批次窗体失败", ex);
                MessageBox.Show(string.Format("打开创建批次窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            
            var details = string.Format("批次详情\n\n批次号：{0}\n产品名称：{1}\n当前状态：{2}\n\n详细信息功能开发中...",
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
            // TODO: 实现搜索功能
            MessageBox.Show("搜索功能开发中...", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
