using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using MES.Common.Logging;
using MES.BLL.WorkOrder;
using MES.Models.WorkOrder;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.WorkOrder
{
    /// <summary>
    /// 工单管理统一窗体
    /// 集成创建、提交、取消等工单操作功能
    /// </summary>
    public partial class WorkOrderManagementForm : ThemedForm
    {
        #region 私有字段

        private readonly WorkOrderBLL _workOrderBLL;
        private bool _lolLayoutABuilt;

        // Layout A：左侧表格 + 右侧详情卡（LoL 客户端风）
        private SplitContainer _splitMain;
        private TableLayoutPanel _detailsLayout;

        // 详情：概览
        private Label _lblSummaryWorkOrderNo;
        private Label _lblSummaryProduct;
        private Panel _panelStatusBadge;
        private Label _lblStatusBadgeText;

        // 详情：进度
        private LolProgressBar _progressBar;
        private Label _lblProgressText;

        // 详情：关键信息
        private Label _valProductName;
        private Label _valPlannedQty;
        private Label _valOutputQty;
        private Label _valCreateTime;
        private Label _valDueTime;
        private Label _valDueIn;

        // 详情：快捷操作
        private LolActionButton _btnViewDetails;
        private LolActionButton _btnCopyWorkOrderNo;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化工单管理窗体
        /// </summary>
        public WorkOrderManagementForm()
        {
            InitializeComponent();
            BuildLolLayoutA();
            _workOrderBLL = new WorkOrderBLL();
            InitializeEvents();
            // 主题应用放到 Shown：避免 DataGridView 绑定/控件句柄创建后“又被刷回系统白底”
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
            this.Shown += (sender, e) => ApplyLolPolish();
            InitializeQueryDefaults();
            LoadWorkOrderData();
            LogManager.Info("工单管理窗体初始化完成");
        }

        #endregion

        #region 初始化方法

        private void InitializeQueryDefaults()
        {
            try
            {
                // 状态默认：全部
                if (cmbStatus != null && cmbStatus.Items != null && cmbStatus.Items.Count > 0)
                {
                    cmbStatus.SelectedIndex = 0;
                }

                // 日期筛选默认不启用（用 CheckBox 控制）
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
            this.btnCreateWorkOrder.Click += BtnCreateWorkOrder_Click;
            this.btnSubmitWorkOrder.Click += BtnSubmitWorkOrder_Click;
            this.btnCancelWorkOrder.Click += BtnCancelWorkOrder_Click;
            this.btnRefresh.Click += BtnRefresh_Click;
            this.btnSearch.Click += BtnSearch_Click;
            
            // 数据网格事件
            this.dgvWorkOrders.SelectionChanged += DgvWorkOrders_SelectionChanged;
            this.dgvWorkOrders.CellDoubleClick += DgvWorkOrders_CellDoubleClick;
            this.dgvWorkOrders.DataBindingComplete += (s, e) =>
            {
                UpdateActionButtons();
                UpdateDetailsPanel();
            };
            this.dgvWorkOrders.CellClick += (s, e) => UpdateActionButtons();
            this.dgvWorkOrders.CellFormatting += DgvWorkOrders_CellFormatting;
            
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
                UpdateActionButtons();
                UpdateDetailsPanel();

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
        /// 根据产品ID获取产品名称
        /// </summary>
        private string GetProductName(int productId)
        {
            try
            {
                if (productId <= 0)
                {
                    return "未指定产品";
                }

                // 这里应该调用产品BLL获取产品名称
                // 由于当前项目中没有产品管理模块，暂时使用默认格式
                // 在实际项目中，应该调用 ProductBLL.GetProductById(productId).ProductName
                return string.Format("产品{0}", productId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取产品名称失败，产品ID: {0}", productId), ex);
                return string.Format("产品{0}", productId);
            }
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

                try
                {
                    dgvWorkOrders.Columns["计划数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvWorkOrders.Columns["已完成数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvWorkOrders.Columns["创建时间"].DefaultCellStyle.Format = "yyyy-MM-dd";
                    dgvWorkOrders.Columns["计划完成时间"].DefaultCellStyle.Format = "yyyy-MM-dd";
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
        }

        /// <summary>
        /// 创建工单按钮点击事件
        /// </summary>
        private void BtnCreateWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new CreateWorkOrder())
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
                        LoadWorkOrderData();
                    }
                }
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
                var selectedRow = TryGetSelectedRow();
                if (selectedRow == null)
                {
                    MessageBox.Show("请先选择要提交的工单", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var workOrderNo = selectedRow.Cells["工单号"].Value != null ? selectedRow.Cells["工单号"].Value.ToString() : "";
                if (string.IsNullOrWhiteSpace(workOrderNo))
                {
                    MessageBox.Show("选中的工单号为空，无法提交。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var form = new SubmitWorkOrder())
                {
                    try
                    {
                        form.PreselectWorkOrder(workOrderNo);
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
                        LoadWorkOrderData();
                    }
                }
                
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
                var selectedRow = TryGetSelectedRow();
                if (selectedRow == null)
                {
                    MessageBox.Show("请先选择要取消的工单", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var workOrderNo = selectedRow.Cells["工单号"].Value != null ? selectedRow.Cells["工单号"].Value.ToString() : "";
                if (string.IsNullOrWhiteSpace(workOrderNo))
                {
                    MessageBox.Show("选中的工单号为空，无法取消。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var form = new CancelWorkOrder())
                {
                    try
                    {
                        form.PreselectWorkOrder(workOrderNo);
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
                        LoadWorkOrderData();
                    }
                }
                
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
            SearchWorkOrders();
        }

        /// <summary>
        /// 数据网格选择变化事件
        /// </summary>
        private void DgvWorkOrders_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtons();
            UpdateDetailsPanel();
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
                
                ShowWorkOrderDetails(workOrderNo);
            }
        }

        private void DgvWorkOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dgvWorkOrders == null) return;
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                if (dgvWorkOrders.Columns == null || e.ColumnIndex >= dgvWorkOrders.Columns.Count) return;

                var column = dgvWorkOrders.Columns[e.ColumnIndex];
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
                else if (string.Equals(status, "已完成", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.SuccessGreen;
                    back = Color.FromArgb(18, 46, 32);
                }
                else if (string.Equals(status, "已关闭", StringComparison.OrdinalIgnoreCase))
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

        #endregion

        #region 私有方法

        /// <summary>
        /// 搜索工单
        /// </summary>
        private void SearchWorkOrders()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();

                var status = cmbStatus != null && cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : null;

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

                // 使用 BLL 的多条件筛选（关键词 + 状态 + 日期）
                var searchResults = _workOrderBLL.SearchWorkOrders(keyword, status, startDate, endDate);
                var dataTable = ConvertWorkOrdersToDataTable(searchResults);
                dgvWorkOrders.DataSource = dataTable;

                // 设置列标题
                SetupDataGridColumns();

                // 更新统计信息
                lblTotal.Text = string.Format("共 {0} 条工单记录", searchResults.Count);
                UpdateActionButtons();
                UpdateDetailsPanel();

                LogManager.Info(string.Format("搜索工单完成，关键词：{0}，结果数量：{1}", keyword, searchResults.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索工单失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("搜索失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示工单详情
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        private void ShowWorkOrderDetails(string workOrderNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(workOrderNo))
                {
                    MessageBox.Show("工单号不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 从BLL获取工单详情
                var allWorkOrders = _workOrderBLL.GetAllWorkOrders();
                var workOrder = allWorkOrders.FirstOrDefault(w => w.WorkOrderNum == workOrderNo);

                if (workOrder == null)
                {
                    MessageBox.Show(string.Format("未找到工单号为 {0} 的工单", workOrderNo), "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 构建详情信息
                var details = string.Format(
                    "工单详情\n\n" +
                    "工单号：{0}\n" +
                    "产品ID：{1}\n" +
                    "产品名称：{2}\n" +
                    "计划数量：{3}\n" +
                    "已完成数量：{4}\n" +
                    "状态：{5}\n" +
                    "创建时间：{6}\n" +
                    "计划完成时间：{7}\n" +
                    "描述：{8}",
                    workOrder.WorkOrderNum,
                    workOrder.ProductId,
                    GetProductName(workOrder.ProductId),
                    workOrder.PlannedQuantity,
                    workOrder.OutputQuantity,
                    GetStatusText(workOrder.WorkOrderStatus),
                    workOrder.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    workOrder.PlannedDueDate != null ? workOrder.PlannedDueDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "未设置",
                    workOrder.Description ?? "无"
                );

                MessageBox.Show(details, "工单详情", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info(string.Format("查看工单详情：{0}", workOrderNo));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("显示工单详情失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("显示工单详情失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 资源清理

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
        }

        #endregion

        #region 按钮状态逻辑

        private DataGridViewRow TryGetSelectedRow()
        {
            try
            {
                if (dgvWorkOrders == null) return null;
                if (dgvWorkOrders.SelectedRows != null && dgvWorkOrders.SelectedRows.Count > 0)
                {
                    return dgvWorkOrders.SelectedRows[0];
                }

                if (dgvWorkOrders.CurrentRow != null && !dgvWorkOrders.CurrentRow.IsNewRow)
                {
                    return dgvWorkOrders.CurrentRow;
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
                if (btnSubmitWorkOrder == null || btnCancelWorkOrder == null || dgvWorkOrders == null)
                {
                    return;
                }

                var row = TryGetSelectedRow();
                if (row == null)
                {
                    btnSubmitWorkOrder.Enabled = false;
                    btnCancelWorkOrder.Enabled = false;
                    return;
                }

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

                // 提交：仅“待开始”可提交；取消：待开始/进行中可取消
                var canSubmit = string.Equals(statusText, "待开始", StringComparison.OrdinalIgnoreCase);
                var canCancel = string.Equals(statusText, "待开始", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(statusText, "进行中", StringComparison.OrdinalIgnoreCase);

                btnSubmitWorkOrder.Enabled = canSubmit;
                btnCancelWorkOrder.Enabled = canCancel;
            }
            catch
            {
                // 最差情况：不让错误影响界面显示
            }
        }

        #endregion

        #region LoL 布局 A（左表格 + 右侧详情卡）

        private void BuildLolLayoutA()
        {
            if (_lolLayoutABuilt) return;
            _lolLayoutABuilt = true;

            if (panelMain == null || dgvWorkOrders == null) return;

            try
            {
                panelMain.SuspendLayout();

                // 防重复：如果设计器/其他逻辑已经放入 SplitContainer，直接跳过
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
                dgvWorkOrders.Dock = DockStyle.Fill;
                _splitMain.Panel1.Padding = new Padding(0, 0, 8, 0);
                _splitMain.Panel1.Controls.Add(dgvWorkOrders);

                // 右：详情栏（固定宽度更像 LoL 客户端侧边栏）
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
                // ignore（布局失败不影响业务功能）
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

                // SplitContainer 有最小宽度约束，避免异常抛错
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
            lblTitle.Text = "工单概览";
            lblTitle.AutoEllipsis = true;
            layout.Controls.Add(lblTitle, 0, 0);
            layout.SetColumnSpan(lblTitle, 2);

            _lblSummaryWorkOrderNo = new Label();
            _lblSummaryWorkOrderNo.Dock = DockStyle.Fill;
            _lblSummaryWorkOrderNo.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummaryWorkOrderNo.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            _lblSummaryWorkOrderNo.ForeColor = LeagueColors.TextHighlight;
            _lblSummaryWorkOrderNo.Text = "未选择工单";
            _lblSummaryWorkOrderNo.AutoEllipsis = true;
            layout.Controls.Add(_lblSummaryWorkOrderNo, 0, 1);

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

            _lblSummaryProduct = new Label();
            _lblSummaryProduct.Dock = DockStyle.Fill;
            _lblSummaryProduct.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummaryProduct.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            _lblSummaryProduct.ForeColor = LeagueColors.TextSecondary;
            _lblSummaryProduct.Text = "请选择左侧工单查看详情";
            _lblSummaryProduct.AutoEllipsis = true;
            layout.Controls.Add(_lblSummaryProduct, 0, 2);
            layout.SetColumnSpan(_lblSummaryProduct, 2);
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
            lblTitle.Text = "完成进度";
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
            info.RowCount = 6;
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86F));
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 6; i++)
            {
                info.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            }
            layout.Controls.Add(info, 0, 1);

            AddInfoRow(info, 0, "产品", out _valProductName);
            AddInfoRow(info, 1, "计划数", out _valPlannedQty);
            AddInfoRow(info, 2, "已完成", out _valOutputQty);
            AddInfoRow(info, 3, "创建", out _valCreateTime);
            AddInfoRow(info, 4, "交期", out _valDueTime);
            AddInfoRow(info, 5, "剩余", out _valDueIn);
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
            _btnViewDetails.Text = "查看详情";
            _btnViewDetails.Width = 110;
            _btnViewDetails.Compact = true;
            _btnViewDetails.Enabled = false;
            _btnViewDetails.Click += (s, e) =>
            {
                try
                {
                    var row = TryGetSelectedRow();
                    if (row == null) return;
                    var workOrderNo = GetCellString(row, "工单号");
                    if (string.IsNullOrWhiteSpace(workOrderNo)) return;
                    ShowWorkOrderDetails(workOrderNo);
                }
                catch
                {
                    // ignore
                }
            };

            _btnCopyWorkOrderNo = new LolActionButton();
            _btnCopyWorkOrderNo.Text = "复制工单号";
            _btnCopyWorkOrderNo.Width = 120;
            _btnCopyWorkOrderNo.Compact = true;
            _btnCopyWorkOrderNo.Enabled = false;
            _btnCopyWorkOrderNo.Click += (s, e) =>
            {
                try
                {
                    var row = TryGetSelectedRow();
                    if (row == null) return;
                    var workOrderNo = GetCellString(row, "工单号");
                    if (string.IsNullOrWhiteSpace(workOrderNo)) return;

                    try
                    {
                        Clipboard.SetText(workOrderNo);
                    }
                    catch
                    {
                        // ignore
                    }
                }
                catch
                {
                    // ignore
                }
            };

            flow.Controls.Add(_btnViewDetails);
            flow.Controls.Add(_btnCopyWorkOrderNo);
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
                    ApplyDetailsEmptyState("请选择左侧工单查看详情");
                    return;
                }

                var workOrderNo = GetCellString(row, "工单号");
                var productName = GetCellString(row, "产品名称");
                var status = GetCellString(row, "状态");

                var planned = GetCellDecimal(row, "计划数量");
                var output = GetCellDecimal(row, "已完成数量");
                var createTime = GetCellDateTime(row, "创建时间");
                var dueTime = GetCellDateTime(row, "计划完成时间");

                if (_lblSummaryWorkOrderNo != null)
                {
                    _lblSummaryWorkOrderNo.Text = string.IsNullOrWhiteSpace(workOrderNo) ? "未选择工单" : workOrderNo;
                }

                if (_lblSummaryProduct != null)
                {
                    _lblSummaryProduct.Text = string.IsNullOrWhiteSpace(productName)
                        ? "产品：—"
                        : string.Format("产品：{0}", productName);
                }

                ApplyStatusBadge(status);

                var plannedVal = planned.GetValueOrDefault(0m);
                var outputVal = output.GetValueOrDefault(0m);
                float progress = 0f;
                if (plannedVal > 0m)
                {
                    progress = (float)Math.Max(0m, Math.Min(1m, outputVal / plannedVal));
                }

                if (_progressBar != null)
                {
                    try { _progressBar.Progress = progress; } catch { }
                }

                if (_lblProgressText != null)
                {
                    var percent = (int)Math.Round(progress * 100, MidpointRounding.AwayFromZero);
                    _lblProgressText.Text = plannedVal > 0m
                        ? string.Format("已完成 {0} / {1}（{2}%）", FormatQuantity(outputVal), FormatQuantity(plannedVal), percent)
                        : string.Format("已完成 {0}", FormatQuantity(outputVal));
                }

                if (_valProductName != null) _valProductName.Text = string.IsNullOrWhiteSpace(productName) ? "—" : productName;
                if (_valPlannedQty != null) _valPlannedQty.Text = planned.HasValue ? FormatQuantity(plannedVal) : "—";
                if (_valOutputQty != null) _valOutputQty.Text = output.HasValue ? FormatQuantity(outputVal) : "—";
                if (_valCreateTime != null) _valCreateTime.Text = createTime.HasValue ? createTime.Value.ToString("yyyy-MM-dd HH:mm") : "—";
                if (_valDueTime != null) _valDueTime.Text = dueTime.HasValue ? dueTime.Value.ToString("yyyy-MM-dd") : "未设置";

                if (_valDueIn != null)
                {
                    _valDueIn.ForeColor = LeagueColors.TextPrimary;
                    if (dueTime.HasValue)
                    {
                        var days = (dueTime.Value.Date - DateTime.Today).Days;
                        if (days < 0)
                        {
                            _valDueIn.Text = string.Format("已逾期 {0} 天", Math.Abs(days));
                            _valDueIn.ForeColor = LeagueColors.ErrorRed;
                        }
                        else if (days <= 2)
                        {
                            _valDueIn.Text = string.Format("剩余 {0} 天", days);
                            _valDueIn.ForeColor = LeagueColors.WarningOrange;
                        }
                        else
                        {
                            _valDueIn.Text = string.Format("剩余 {0} 天", days);
                            _valDueIn.ForeColor = LeagueColors.TextPrimary;
                        }
                    }
                    else
                    {
                        _valDueIn.Text = "—";
                        _valDueIn.ForeColor = LeagueColors.TextDisabled;
                    }
                }

                var hasNo = !string.IsNullOrWhiteSpace(workOrderNo);
                if (_btnViewDetails != null) _btnViewDetails.Enabled = hasNo;
                if (_btnCopyWorkOrderNo != null) _btnCopyWorkOrderNo.Enabled = hasNo;
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
                if (_lblSummaryWorkOrderNo != null) _lblSummaryWorkOrderNo.Text = "未选择工单";
                if (_lblSummaryProduct != null) _lblSummaryProduct.Text = hint ?? "请选择左侧工单查看详情";

                ApplyStatusBadge(string.Empty);

                if (_progressBar != null) _progressBar.Value = 0;
                if (_lblProgressText != null) _lblProgressText.Text = "—";

                if (_valProductName != null) _valProductName.Text = "—";
                if (_valPlannedQty != null) _valPlannedQty.Text = "—";
                if (_valOutputQty != null) _valOutputQty.Text = "—";
                if (_valCreateTime != null) _valCreateTime.Text = "—";
                if (_valDueTime != null) _valDueTime.Text = "—";
                if (_valDueIn != null) { _valDueIn.Text = "—"; _valDueIn.ForeColor = LeagueColors.TextDisabled; }

                if (_btnViewDetails != null) _btnViewDetails.Enabled = false;
                if (_btnCopyWorkOrderNo != null) _btnCopyWorkOrderNo.Enabled = false;
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
                else if (string.Equals(status, "已完成", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.SuccessGreen;
                    back = Color.FromArgb(18, 46, 32);
                }
                else if (string.Equals(status, "已关闭", StringComparison.OrdinalIgnoreCase))
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
                var s = GetCellString(row, columnName);
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
                // 去掉 emoji，更贴近 LoL 客户端克制风
                if (lblTitle != null)
                {
                    lblTitle.Text = "工单管理中心";
                    lblTitle.ForeColor = LeagueColors.RiotGoldHover;
                }

                if (btnSearch != null) btnSearch.Text = "搜索";
                if (btnRefresh != null) btnRefresh.Text = "刷新";
                if (btnCreateWorkOrder != null) btnCreateWorkOrder.Text = "创建工单";
                if (btnSubmitWorkOrder != null) btnSubmitWorkOrder.Text = "提交工单";
                if (btnCancelWorkOrder != null) btnCancelWorkOrder.Text = "取消工单";

                // 将几个区域面板做成“暗色卡片”，由主题系统绘制金描边
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
            }
            catch
            {
                // ignore
            }
        }

        #endregion
    }
}
