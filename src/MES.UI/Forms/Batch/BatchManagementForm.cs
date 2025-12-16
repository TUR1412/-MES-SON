using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.BLL.Workshop;
using MES.Models.Workshop;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Batch
{
    /// <summary>
    /// 批次管理统一窗体
    /// 集成创建、取消等批次操作功能
    /// </summary>
    public partial class BatchManagementForm : ThemedForm
    {
        #region 私有字段

        private readonly BatchBLL _batchBLL;
        private bool _lolLayoutABuilt;

        // Layout A：左侧表格 + 右侧详情卡（LoL 客户端风）
        private SplitContainer _splitMain;
        private TableLayoutPanel _detailsLayout;

        // 详情：概览
        private Label _lblSummaryBatchNo;
        private Label _lblSummarySub;
        private Panel _panelStatusBadge;
        private Label _lblStatusBadgeText;

        // 详情：进度
        private LolProgressBar _progressBar;
        private Label _lblProgressText;

        // 详情：关键信息
        private Label _valProductName;
        private Label _valWorkOrderNo;
        private Label _valPlannedQty;
        private Label _valActualQty;
        private Label _valOperator;
        private Label _valCreateTime;
        private Label _valDueTime;

        // 详情：快捷操作
        private LolActionButton _btnViewDetails;
        private LolActionButton _btnCopyBatchNo;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化批次管理窗体
        /// </summary>
        public BatchManagementForm()
        {
            InitializeComponent();
            BuildLolLayoutA();
            _batchBLL = new BatchBLL();
            InitializeEvents();
            // 主题应用放到 Shown：避免 DataGridView 绑定/控件句柄创建后“又被刷回系统白底”
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
            this.Shown += (sender, e) => ApplyLolPolish();
            InitializeQueryDefaults();
            LogManager.Info("批次管理窗体初始化完成");
        }

        #endregion

        #region 初始化方法

        private void InitializeQueryDefaults()
        {
            try
            {
                if (cmbStatus != null && cmbStatus.Items != null && cmbStatus.Items.Count > 0 && cmbStatus.SelectedIndex < 0)
                {
                    cmbStatus.SelectedIndex = 0;
                }

                if (dtpStartDate != null)
                {
                    dtpStartDate.ShowCheckBox = true;
                    dtpStartDate.Checked = false;
                }

                if (dtpEndDate != null)
                {
                    dtpEndDate.ShowCheckBox = true;
                    dtpEndDate.Checked = false;
                }
            }
            catch
            {
                // ignore
            }
        }

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
            this.dgvBatches.DataBindingComplete += (s, e) =>
            {
                UpdateActionButtons();
                UpdateDetailsPanel();
            };
            this.dgvBatches.CellClick += (s, e) => UpdateActionButtons();
            this.dgvBatches.CellFormatting += DgvBatches_CellFormatting;

            // 窗体事件
            this.Load += BatchManagementForm_Load;

            // 体验优化：回车直接搜索
            if (txtSearch != null)
            {
                txtSearch.KeyDown += (s, e) =>
                {
                    try
                    {
                        if (e != null && e.KeyCode == Keys.Enter)
                        {
                            e.SuppressKeyPress = true;
                            SearchBatches();
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
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
                UpdateActionButtons();
                UpdateDetailsPanel();

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

                try
                {
                    dgvBatches.Columns["批次数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvBatches.Columns["已完成数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvBatches.Columns["创建时间"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    dgvBatches.Columns["计划完成时间"].DefaultCellStyle.Format = "yyyy-MM-dd";
                }
                catch
                {
                    // ignore
                }
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
            // 仅在“独立打开”时最大化；嵌入 LoL 主壳（TopLevel=false）时禁止最大化，避免布局/滚动异常
            try
            {
                if (this.TopLevel)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.WindowState = FormWindowState.Normal;
                    }
                }
            }
            catch
            {
                // ignore
            }

            // 初始化状态下拉框
            try
            {
                if (cmbStatus != null && cmbStatus.Items != null && cmbStatus.Items.Count > 0 && cmbStatus.SelectedIndex < 0)
                {
                    cmbStatus.SelectedIndex = 0; // 选择"全部状态"
                }
            }
            catch
            {
                // ignore
            }

            // 在窗体加载时加载数据，而不是在构造函数中
            LoadBatchData();
            UpdateActionButtons();
        }

        /// <summary>
        /// 创建批次按钮点击事件
        /// </summary>
        private void BtnCreateBatch_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new CreateBatch())
                {
                    try
                    {
                        UIThemeManager.ApplyTheme(form);
                    }
                    catch
                    {
                        // ignore
                    }

                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        LoadBatchData();
                    }
                }
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
                var selectedRow = TryGetSelectedRow();
                if (selectedRow == null)
                {
                    MessageBox.Show("请先选择要取消的批次", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                var batchNo = selectedRow.Cells["批次号"].Value != null ? selectedRow.Cells["批次号"].Value.ToString() : "";
                var status = selectedRow.Cells["状态"].Value != null ? selectedRow.Cells["状态"].Value.ToString() : "";
                
                // 检查批次状态
                if (status == "已完成" || status == "已取消")
                {
                    MessageBox.Show("已完成或已取消的批次无法再次取消", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(batchNo))
                {
                    MessageBox.Show("选中的批次号为空，无法取消。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var form = new CancelBatch())
                {
                    try
                    {
                        form.PreselectBatch(batchNo);
                    }
                    catch
                    {
                        // ignore
                    }

                    try
                    {
                        UIThemeManager.ApplyTheme(form);
                    }
                    catch
                    {
                        // ignore
                    }

                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        LoadBatchData();
                    }
                }
                
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
            SearchBatches();
        }

        /// <summary>
        /// 搜索批次
        /// </summary>
        private void SearchBatches()
        {
            try
            {
                // 获取搜索条件
                string keyword = txtSearch != null ? txtSearch.Text.Trim() : null;
                string status = cmbStatus != null && cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : null;

                DateTime? startDate = null;
                DateTime? endDate = null;

                if (dtpStartDate != null && dtpStartDate.ShowCheckBox && dtpStartDate.Checked)
                {
                    startDate = dtpStartDate.Value.Date;
                }

                if (dtpEndDate != null && dtpEndDate.ShowCheckBox && dtpEndDate.Checked)
                {
                    endDate = dtpEndDate.Value.Date;
                }

                // 使用BLL进行搜索
                var searchResults = _batchBLL.SearchBatches(keyword, status, startDate, endDate);

                // 更新显示
                var dataTable = ConvertBatchesToDataTable(searchResults);
                dgvBatches.DataSource = dataTable;

                // 设置列标题
                SetupDataGridColumns();

                // 更新统计信息
                UpdateStatistics();
                UpdateActionButtons();
                UpdateDetailsPanel();

                LogManager.Info(string.Format("搜索批次完成：关键词={0} 状态={1} 结果数量={2}", keyword, status, searchResults.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索批次失败", ex);
                MessageBox.Show(string.Format("搜索批次失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvBatches_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dgvBatches == null) return;
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                if (dgvBatches.Columns == null || e.ColumnIndex >= dgvBatches.Columns.Count) return;

                var column = dgvBatches.Columns[e.ColumnIndex];
                if (column == null) return;
                if (!string.Equals(column.Name, "状态", StringComparison.OrdinalIgnoreCase)) return;

                var status = e.Value != null ? e.Value.ToString() : string.Empty;

                var fore = LeagueColors.TextPrimary;
                var back = LeagueColors.DarkSurface;

                if (string.Equals(status, "待开始", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.RiotGoldHover;
                    back = Color.FromArgb(45, 40, 28);
                }
                else if (string.Equals(status, "进行中", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.SpecialCyan;
                    back = Color.FromArgb(18, 44, 48);
                }
                else if (string.Equals(status, "已暂停", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.WarningOrange;
                    back = Color.FromArgb(52, 36, 12);
                }
                else if (string.Equals(status, "已完成", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.SuccessGreen;
                    back = Color.FromArgb(18, 46, 32);
                }
                else if (string.Equals(status, "已取消", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.TextDisabled;
                    back = Color.FromArgb(35, 35, 35);
                }

                e.CellStyle.ForeColor = fore;
                e.CellStyle.BackColor = back;
                e.CellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
                e.CellStyle.SelectionForeColor = LeagueColors.TextHighlight;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// 数据网格选择变化事件
        /// </summary>
        private void DgvBatches_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtons();
            UpdateDetailsPanel();
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

        #region 按钮状态逻辑

        private DataGridViewRow TryGetSelectedRow()
        {
            try
            {
                if (dgvBatches == null) return null;
                if (dgvBatches.SelectedRows != null && dgvBatches.SelectedRows.Count > 0)
                {
                    return dgvBatches.SelectedRows[0];
                }

                if (dgvBatches.CurrentRow != null && !dgvBatches.CurrentRow.IsNewRow)
                {
                    return dgvBatches.CurrentRow;
                }
            }
            catch
            {
                // ignore
            }

            return null;
        }

        private void UpdateActionButtons()
        {
            try
            {
                if (btnCancelBatch == null || btnBatchDetails == null || dgvBatches == null)
                {
                    return;
                }

                var row = TryGetSelectedRow();
                if (row == null)
                {
                    btnCancelBatch.Enabled = false;
                    btnBatchDetails.Enabled = false;
                    return;
                }

                btnBatchDetails.Enabled = true;

                var statusText = string.Empty;
                try
                {
                    if (row.Cells != null)
                    {
                        DataGridViewCell cell = null;
                        try { cell = row.Cells["状态"]; } catch { }
                        if (cell != null && cell.Value != null)
                        {
                            statusText = cell.Value.ToString();
                        }
                    }
                }
                catch
                {
                    // ignore
                }

                // 取消：待开始/进行中/已暂停 可取消；已完成/已取消 不可取消
                var canCancel = string.Equals(statusText, "待开始", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(statusText, "进行中", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(statusText, "已暂停", StringComparison.OrdinalIgnoreCase);

                btnCancelBatch.Enabled = canCancel;
            }
            catch
            {
                // ignore
            }
        }

        #endregion

        #region LoL 布局 A（左表格 + 右侧详情卡）

        private void BuildLolLayoutA()
        {
            if (_lolLayoutABuilt) return;
            _lolLayoutABuilt = true;

            if (panelMain == null || dgvBatches == null) return;

            try
            {
                panelMain.SuspendLayout();

                // 防重复
                foreach (Control c in panelMain.Controls)
                {
                    if (c is SplitContainer)
                    {
                        _splitMain = c as SplitContainer;
                        return;
                    }
                }

                panelMain.Controls.Clear();

                _splitMain = new SplitContainer();
                _splitMain.Name = "splitMain";
                _splitMain.Dock = DockStyle.Fill;
                _splitMain.Orientation = Orientation.Vertical;
                _splitMain.SplitterWidth = 6;
                _splitMain.Panel1MinSize = 520;
                _splitMain.Panel2MinSize = 320;
                _splitMain.FixedPanel = FixedPanel.Panel2;
                _splitMain.IsSplitterFixed = false;
                _splitMain.BackColor = LeagueColors.SeparatorColor;

                // 左：列表
                dgvBatches.Dock = DockStyle.Fill;
                _splitMain.Panel1.Padding = new Padding(0, 0, 8, 0);
                _splitMain.Panel1.Controls.Add(dgvBatches);

                // 右：详情栏
                _splitMain.Panel2.Padding = new Padding(12);

                var detailsHost = new Panel();
                detailsHost.Dock = DockStyle.Fill;
                detailsHost.BackColor = Color.Transparent;
                _splitMain.Panel2.Controls.Add(detailsHost);

                _detailsLayout = new TableLayoutPanel();
                _detailsLayout.Dock = DockStyle.Fill;
                _detailsLayout.ColumnCount = 1;
                _detailsLayout.RowCount = 4;
                _detailsLayout.Padding = new Padding(0);
                _detailsLayout.BackColor = Color.Transparent;
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 138F)); // 概览
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));  // 进度
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // 信息
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));  // 操作
                detailsHost.Controls.Add(_detailsLayout);

                var cardSummary = CreateLolCardPanel();
                var cardProgress = CreateLolCardPanel();
                var cardInfo = CreateLolCardPanel();
                var cardActions = CreateLolCardPanel();

                _detailsLayout.Controls.Add(cardSummary, 0, 0);
                _detailsLayout.Controls.Add(cardProgress, 0, 1);
                _detailsLayout.Controls.Add(cardInfo, 0, 2);
                _detailsLayout.Controls.Add(cardActions, 0, 3);

                BuildSummaryCard(cardSummary);
                BuildProgressCard(cardProgress);
                BuildInfoCard(cardInfo);
                BuildActionsCard(cardActions);

                panelMain.Controls.Add(_splitMain);

                UpdateSplitDistance();
                panelMain.SizeChanged += (s, e) => UpdateSplitDistance();
            }
            catch
            {
                // ignore
            }
            finally
            {
                try { panelMain.ResumeLayout(true); } catch { }
            }
        }

        private void UpdateSplitDistance()
        {
            try
            {
                if (_splitMain == null) return;

                var detailsWidth = 380;
                var desired = Math.Max(_splitMain.Panel1MinSize, _splitMain.Width - detailsWidth);

                var maxAllowed = _splitMain.Width - _splitMain.Panel2MinSize;
                if (desired > maxAllowed) desired = maxAllowed;
                if (desired < _splitMain.Panel1MinSize) desired = _splitMain.Panel1MinSize;
                if (desired < 0) desired = 0;

                _splitMain.SplitterDistance = desired;
            }
            catch
            {
                // ignore
            }
        }

        private static Panel CreateLolCardPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = LeagueColors.DarkSurface;
            panel.Padding = new Padding(12);
            panel.Margin = new Padding(0);
            return panel;
        }

        private void BuildSummaryCard(Panel host)
        {
            if (host == null) return;

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 2;
            layout.RowCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "批次概览";
            lblTitle.AutoEllipsis = true;
            layout.Controls.Add(lblTitle, 0, 0);
            layout.SetColumnSpan(lblTitle, 2);

            _lblSummaryBatchNo = new Label();
            _lblSummaryBatchNo.Dock = DockStyle.Fill;
            _lblSummaryBatchNo.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummaryBatchNo.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            _lblSummaryBatchNo.ForeColor = LeagueColors.TextHighlight;
            _lblSummaryBatchNo.Text = "未选择批次";
            _lblSummaryBatchNo.AutoEllipsis = true;
            layout.Controls.Add(_lblSummaryBatchNo, 0, 1);

            _panelStatusBadge = new Panel();
            _panelStatusBadge.Dock = DockStyle.Fill;
            _panelStatusBadge.BackColor = Color.FromArgb(35, 35, 35);
            _panelStatusBadge.Padding = new Padding(6, 2, 6, 2);
            layout.Controls.Add(_panelStatusBadge, 1, 1);

            _lblStatusBadgeText = new Label();
            _lblStatusBadgeText.Dock = DockStyle.Fill;
            _lblStatusBadgeText.TextAlign = ContentAlignment.MiddleCenter;
            _lblStatusBadgeText.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            _lblStatusBadgeText.Text = "—";
            _lblStatusBadgeText.BackColor = Color.Transparent;
            _lblStatusBadgeText.ForeColor = LeagueColors.TextDisabled;
            _panelStatusBadge.Controls.Add(_lblStatusBadgeText);

            _lblSummarySub = new Label();
            _lblSummarySub.Dock = DockStyle.Fill;
            _lblSummarySub.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummarySub.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            _lblSummarySub.ForeColor = LeagueColors.TextSecondary;
            _lblSummarySub.Text = "请选择左侧批次查看详情";
            _lblSummarySub.AutoEllipsis = true;
            layout.Controls.Add(_lblSummarySub, 0, 2);
            layout.SetColumnSpan(_lblSummarySub, 2);
        }

        private void BuildProgressCard(Panel host)
        {
            if (host == null) return;

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "产出进度";
            layout.Controls.Add(lblTitle, 0, 0);

            _progressBar = new LolProgressBar();
            _progressBar.Dock = DockStyle.Fill;
            _progressBar.Margin = new Padding(0, 4, 0, 4);
            _progressBar.Minimum = 0;
            _progressBar.Maximum = 1000;
            _progressBar.Value = 0;
            layout.Controls.Add(_progressBar, 0, 1);

            _lblProgressText = new Label();
            _lblProgressText.Dock = DockStyle.Fill;
            _lblProgressText.TextAlign = ContentAlignment.MiddleLeft;
            _lblProgressText.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            _lblProgressText.ForeColor = LeagueColors.TextSecondary;
            _lblProgressText.Text = "—";
            _lblProgressText.AutoEllipsis = true;
            layout.Controls.Add(_lblProgressText, 0, 2);
        }

        private void BuildInfoCard(Panel host)
        {
            if (host == null) return;

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "关键信息";
            layout.Controls.Add(lblTitle, 0, 0);

            var info = new TableLayoutPanel();
            info.Dock = DockStyle.Fill;
            info.BackColor = Color.Transparent;
            info.ColumnCount = 2;
            info.RowCount = 7;
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86F));
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 7; i++)
            {
                info.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            }
            layout.Controls.Add(info, 0, 1);

            AddInfoRow(info, 0, "产品", out _valProductName);
            AddInfoRow(info, 1, "工单", out _valWorkOrderNo);
            AddInfoRow(info, 2, "计划数", out _valPlannedQty);
            AddInfoRow(info, 3, "已完成", out _valActualQty);
            AddInfoRow(info, 4, "负责人", out _valOperator);
            AddInfoRow(info, 5, "创建", out _valCreateTime);
            AddInfoRow(info, 6, "交期", out _valDueTime);
        }

        private void BuildActionsCard(Panel host)
        {
            if (host == null) return;

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "快捷操作";
            layout.Controls.Add(lblTitle, 0, 0);

            var flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.FlowDirection = FlowDirection.RightToLeft;
            flow.WrapContents = false;
            flow.Padding = new Padding(0, 4, 0, 0);
            flow.BackColor = Color.Transparent;
            layout.Controls.Add(flow, 0, 1);

            _btnViewDetails = new LolActionButton();
            _btnViewDetails.Text = "批次详情";
            _btnViewDetails.Width = 110;
            _btnViewDetails.Compact = true;
            _btnViewDetails.Enabled = false;
            _btnViewDetails.Click += (s, e) =>
            {
                try
                {
                    if (btnBatchDetails != null && btnBatchDetails.Enabled)
                    {
                        BtnBatchDetails_Click(s, e);
                    }
                }
                catch
                {
                    // ignore
                }
            };

            _btnCopyBatchNo = new LolActionButton();
            _btnCopyBatchNo.Text = "复制批次号";
            _btnCopyBatchNo.Width = 120;
            _btnCopyBatchNo.Compact = true;
            _btnCopyBatchNo.Enabled = false;
            _btnCopyBatchNo.Click += (s, e) =>
            {
                try
                {
                    var row = TryGetSelectedRow();
                    if (row == null) return;
                    var batchNo = GetCellString(row, "批次号");
                    if (string.IsNullOrWhiteSpace(batchNo)) return;

                    try { Clipboard.SetText(batchNo); } catch { }
                }
                catch
                {
                    // ignore
                }
            };

            flow.Controls.Add(_btnViewDetails);
            flow.Controls.Add(_btnCopyBatchNo);
        }

        private static void AddInfoRow(TableLayoutPanel table, int rowIndex, string key, out Label valueLabel)
        {
            var k = new Label();
            k.Dock = DockStyle.Fill;
            k.TextAlign = ContentAlignment.MiddleLeft;
            k.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            k.ForeColor = LeagueColors.TextSecondary;
            k.Text = key;
            k.AutoEllipsis = true;
            table.Controls.Add(k, 0, rowIndex);

            valueLabel = new Label();
            valueLabel.Dock = DockStyle.Fill;
            valueLabel.TextAlign = ContentAlignment.MiddleLeft;
            valueLabel.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            valueLabel.ForeColor = LeagueColors.TextPrimary;
            valueLabel.Text = "—";
            valueLabel.AutoEllipsis = true;
            table.Controls.Add(valueLabel, 1, rowIndex);
        }

        private void UpdateDetailsPanel()
        {
            try
            {
                if (_detailsLayout == null) return;

                var row = TryGetSelectedRow();
                if (row == null)
                {
                    ApplyDetailsEmptyState("请选择左侧批次查看详情");
                    return;
                }

                var batchNo = GetCellString(row, "批次号");
                var productName = GetCellString(row, "产品名称");
                var workOrderNo = GetCellString(row, "工单号");
                var status = GetCellString(row, "状态");
                var operatorName = GetCellString(row, "负责人");

                var planned = GetCellDecimal(row, "批次数量");
                var actual = GetCellDecimal(row, "已完成数量");
                var createTime = GetCellDateTime(row, "创建时间");
                var dueTime = GetCellDateTime(row, "计划完成时间");

                if (_lblSummaryBatchNo != null)
                {
                    _lblSummaryBatchNo.Text = string.IsNullOrWhiteSpace(batchNo) ? "未选择批次" : batchNo;
                }

                if (_lblSummarySub != null)
                {
                    var p = string.IsNullOrWhiteSpace(productName) ? "—" : productName;
                    var w = string.IsNullOrWhiteSpace(workOrderNo) ? "—" : workOrderNo;
                    _lblSummarySub.Text = string.Format("产品：{0}  |  工单：{1}", p, w);
                }

                ApplyStatusBadge(status);

                var plannedVal = planned.GetValueOrDefault(0m);
                var actualVal = actual.GetValueOrDefault(0m);
                float progress = 0f;
                if (plannedVal > 0m)
                {
                    progress = (float)Math.Max(0m, Math.Min(1m, actualVal / plannedVal));
                }

                if (_progressBar != null)
                {
                    try { _progressBar.Progress = progress; } catch { }
                }

                if (_lblProgressText != null)
                {
                    var percent = (int)Math.Round(progress * 100, MidpointRounding.AwayFromZero);
                    _lblProgressText.Text = plannedVal > 0m
                        ? string.Format("已完成 {0} / {1}（{2}%）", FormatQuantity(actualVal), FormatQuantity(plannedVal), percent)
                        : string.Format("已完成 {0}", FormatQuantity(actualVal));
                }

                if (_valProductName != null) _valProductName.Text = string.IsNullOrWhiteSpace(productName) ? "—" : productName;
                if (_valWorkOrderNo != null) _valWorkOrderNo.Text = string.IsNullOrWhiteSpace(workOrderNo) ? "—" : workOrderNo;
                if (_valPlannedQty != null) _valPlannedQty.Text = planned.HasValue ? FormatQuantity(plannedVal) : "—";
                if (_valActualQty != null) _valActualQty.Text = actual.HasValue ? FormatQuantity(actualVal) : "—";
                if (_valOperator != null) _valOperator.Text = string.IsNullOrWhiteSpace(operatorName) ? "—" : operatorName;
                if (_valCreateTime != null) _valCreateTime.Text = createTime.HasValue ? createTime.Value.ToString("yyyy-MM-dd HH:mm") : "—";
                if (_valDueTime != null) _valDueTime.Text = dueTime.HasValue ? dueTime.Value.ToString("yyyy-MM-dd") : "未设置";

                var hasNo = !string.IsNullOrWhiteSpace(batchNo);
                if (_btnViewDetails != null) _btnViewDetails.Enabled = hasNo;
                if (_btnCopyBatchNo != null) _btnCopyBatchNo.Enabled = hasNo;
            }
            catch
            {
                // ignore
            }
        }

        private void ApplyDetailsEmptyState(string hint)
        {
            try
            {
                if (_lblSummaryBatchNo != null) _lblSummaryBatchNo.Text = "未选择批次";
                if (_lblSummarySub != null) _lblSummarySub.Text = hint ?? "请选择左侧批次查看详情";

                ApplyStatusBadge(string.Empty);

                if (_progressBar != null) _progressBar.Value = 0;
                if (_lblProgressText != null) _lblProgressText.Text = "—";

                if (_valProductName != null) _valProductName.Text = "—";
                if (_valWorkOrderNo != null) _valWorkOrderNo.Text = "—";
                if (_valPlannedQty != null) _valPlannedQty.Text = "—";
                if (_valActualQty != null) _valActualQty.Text = "—";
                if (_valOperator != null) _valOperator.Text = "—";
                if (_valCreateTime != null) _valCreateTime.Text = "—";
                if (_valDueTime != null) _valDueTime.Text = "—";

                if (_btnViewDetails != null) _btnViewDetails.Enabled = false;
                if (_btnCopyBatchNo != null) _btnCopyBatchNo.Enabled = false;
            }
            catch
            {
                // ignore
            }
        }

        private void ApplyStatusBadge(string status)
        {
            try
            {
                if (_panelStatusBadge == null || _lblStatusBadgeText == null) return;

                var fore = LeagueColors.TextPrimary;
                var back = Color.FromArgb(35, 35, 35);
                var text = string.IsNullOrWhiteSpace(status) ? "—" : status;

                if (string.Equals(status, "待开始", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.RiotGoldHover;
                    back = Color.FromArgb(45, 40, 28);
                }
                else if (string.Equals(status, "进行中", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.SpecialCyan;
                    back = Color.FromArgb(18, 44, 48);
                }
                else if (string.Equals(status, "已暂停", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.WarningOrange;
                    back = Color.FromArgb(52, 36, 12);
                }
                else if (string.Equals(status, "已完成", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.SuccessGreen;
                    back = Color.FromArgb(18, 46, 32);
                }
                else if (string.Equals(status, "已取消", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.TextDisabled;
                    back = Color.FromArgb(35, 35, 35);
                }

                _panelStatusBadge.BackColor = back;
                _lblStatusBadgeText.ForeColor = fore;
                _lblStatusBadgeText.Text = text;
            }
            catch
            {
                // ignore
            }
        }

        private static string FormatQuantity(decimal value)
        {
            try
            {
                return value.ToString("0.##");
            }
            catch
            {
                return value.ToString();
            }
        }

        private static string GetCellString(DataGridViewRow row, string columnName)
        {
            try
            {
                if (row == null || row.Cells == null) return string.Empty;
                DataGridViewCell cell = null;
                try { cell = row.Cells[columnName]; } catch { }
                if (cell == null || cell.Value == null) return string.Empty;
                return cell.Value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static decimal? GetCellDecimal(DataGridViewRow row, string columnName)
        {
            try
            {
                if (row == null || row.Cells == null) return null;
                DataGridViewCell cell = null;
                try { cell = row.Cells[columnName]; } catch { }
                if (cell == null || cell.Value == null) return null;

                if (cell.Value is decimal) return (decimal)cell.Value;
                if (cell.Value is int) return (int)cell.Value;
                if (cell.Value is long) return (long)cell.Value;
                if (cell.Value is double) return (decimal)(double)cell.Value;
                if (cell.Value is float) return (decimal)(float)cell.Value;

                var s = cell.Value.ToString();
                if (string.IsNullOrWhiteSpace(s)) return null;
                decimal d;
                if (decimal.TryParse(s, out d)) return d;
            }
            catch
            {
                // ignore
            }

            return null;
        }

        private static DateTime? GetCellDateTime(DataGridViewRow row, string columnName)
        {
            try
            {
                if (row == null || row.Cells == null) return null;
                DataGridViewCell cell = null;
                try { cell = row.Cells[columnName]; } catch { }
                if (cell == null || cell.Value == null) return null;

                if (cell.Value is DateTime)
                {
                    return (DateTime)cell.Value;
                }

                var s = cell.Value.ToString();
                if (string.IsNullOrWhiteSpace(s)) return null;
                DateTime dt;
                if (DateTime.TryParse(s, out dt)) return dt;
            }
            catch
            {
                // ignore
            }

            return null;
        }

        #endregion

        #region LoL 细节打磨

        private void ApplyLolPolish()
        {
            try
            {
                if (lblTitle != null)
                {
                    lblTitle.Text = "批次管理中心";
                    lblTitle.ForeColor = LeagueColors.RiotGoldHover;
                }

                if (btnSearch != null) btnSearch.Text = "搜索";
                if (btnRefresh != null) btnRefresh.Text = "刷新";
                if (btnCreateBatch != null) btnCreateBatch.Text = "创建批次";
                if (btnCancelBatch != null) btnCancelBatch.Text = "取消批次";
                if (btnBatchDetails != null) btnBatchDetails.Text = "批次详情";

                if (panelTop != null)
                {
                    panelTop.BorderStyle = BorderStyle.None;
                    panelTop.BackColor = LeagueColors.DarkPanel;
                }

                if (panelSearch != null)
                {
                    panelSearch.BorderStyle = BorderStyle.None;
                    panelSearch.BackColor = LeagueColors.DarkSurface;
                }

                if (panelButtons != null)
                {
                    panelButtons.BorderStyle = BorderStyle.None;
                    panelButtons.BackColor = LeagueColors.DarkSurface;
                }

                if (panelBottom != null)
                {
                    panelBottom.BackColor = LeagueColors.DarkPanel;
                }

                if (lblTotal != null)
                {
                    lblTotal.ForeColor = LeagueColors.TextSecondary;
                }

                if (lblStatistics != null)
                {
                    lblStatistics.ForeColor = LeagueColors.TextSecondary;
                }
            }
            catch
            {
                // ignore
            }
        }

        #endregion
    }
}
