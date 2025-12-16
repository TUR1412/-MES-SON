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
using MES.UI.Forms.Production.Edit;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Production
{
    /// <summary>
    /// 生产订单管理窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class ProductionOrderManagementForm : ThemedForm
    {
        private List<ProductionOrderInfo> orderList;
        private List<ProductionOrderInfo> filteredOrderList;
        private ProductionOrderInfo currentOrder;
        private readonly IProductionOrderBLL _productionOrderBLL;
        private Timer _searchDebounceTimer;

        // Layout A：左侧表格 + 右侧详情卡（LoL 客户端风）
        private bool _lolLayoutABuilt;
        private SplitContainer _splitMain;
        private TableLayoutPanel _detailsLayout;

        // 详情：概览
        private Label _lblSummaryOrderNo;
        private Label _lblSummaryProduct;
        private Panel _panelStatusBadge;
        private Label _lblStatusBadgeText;
        private Panel _panelPriorityBadge;
        private Label _lblPriorityBadgeText;

        // 详情：进度
        private LolProgressBar _progressBar;
        private Label _lblProgressText;

        // 详情：关键信息
        private Label _valProductCode;
        private Label _valPlannedQty;
        private Label _valActualQty;
        private Label _valUnit;
        private Label _valWorkshop;
        private Label _valResponsible;
        private Label _valPlanWindow;
        private Label _valActualWindow;

        // 详情：客户/备注
        private Label _valCustomer;
        private Label _valSalesOrder;
        private TextBox _txtRemarks;

        // 详情：快捷操作
        private LolActionButton _btnEditQuick;
        private LolActionButton _btnCopyOrderNo;

        public ProductionOrderManagementForm()
        {
            InitializeComponent();
            BuildLolLayoutA();
            _productionOrderBLL = new ProductionOrderBLL();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
            this.Shown += (sender, e) =>
            {
                try
                {
                    ApplyLolPolish();
                }
                catch
                {
                    // ignore
                }
            };
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
                orderList = new List<ProductionOrderInfo>();
                filteredOrderList = new List<ProductionOrderInfo>();
                currentOrder = null;

                // 设置DataGridView
                SetupDataGridView();
                InitializeSearchDebounceTimer();
                try
                {
                    dataGridViewOrders.CellFormatting += DataGridViewOrders_CellFormatting;
                }
                catch
                {
                    // ignore
                }

                // 加载真实数据
                LoadProductionOrderData();

                // 刷新显示
                RefreshDataGridView();

                // 清空详情面板
                ClearDetailsPanel();

                LogManager.Info("生产订单管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("生产订单管理窗体初始化失败", ex);
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
            dataGridViewOrders.AutoGenerateColumns = false;
            dataGridViewOrders.Columns.Clear();
            dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewOrders.RowHeadersVisible = false;

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                HeaderText = "订单编号",
                DataPropertyName = "OrderNo",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 18F,
                MinimumWidth = 120,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 28F,
                MinimumWidth = 160,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "计划数量",
                DataPropertyName = "Quantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12F,
                MinimumWidth = 90,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActualQuantity",
                HeaderText = "完成数量",
                DataPropertyName = "ActualQuantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12F,
                MinimumWidth = 90,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "订单状态",
                DataPropertyName = "Status",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12F,
                MinimumWidth = 90,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Priority",
                HeaderText = "优先级",
                DataPropertyName = "Priority",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 9F,
                MinimumWidth = 70,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopName",
                HeaderText = "负责车间",
                DataPropertyName = "WorkshopName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 17F,
                MinimumWidth = 110,
                ReadOnly = true
            });

            // 视觉：表格配色/密度由主题系统统一处理，避免这里写死“白底蓝选中”造成割裂感
            dataGridViewOrders.EnableHeadersVisualStyles = false;

            // 信息层级：数字右对齐，状态/优先级居中
            try
            {
                if (dataGridViewOrders.Columns.Contains("Quantity"))
                {
                    dataGridViewOrders.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewOrders.Columns["Quantity"].DefaultCellStyle.Format = "N0";
                }

                if (dataGridViewOrders.Columns.Contains("ActualQuantity"))
                {
                    dataGridViewOrders.Columns["ActualQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridViewOrders.Columns["ActualQuantity"].DefaultCellStyle.Format = "N0";
                }

                if (dataGridViewOrders.Columns.Contains("Status"))
                {
                    dataGridViewOrders.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dataGridViewOrders.Columns.Contains("Priority"))
                {
                    dataGridViewOrders.Columns["Priority"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// 加载生产订单数据
        /// </summary>
        private void LoadProductionOrderData()
        {
            try
            {
                orderList.Clear();

                // 从BLL层获取真实数据
                var orders = _productionOrderBLL.GetAllProductionOrders();
                if (orders != null && orders.Count > 0)
                {
                    orderList.AddRange(orders);
                }

                // 复制到过滤列表
                filteredOrderList = new List<ProductionOrderInfo>(orderList);

                LogManager.Info(string.Format("成功加载生产订单数据，共 {0} 条记录", orderList.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error("加载生产订单数据失败", ex);
                MessageBox.Show("加载生产订单数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 初始化空列表
                orderList = new List<ProductionOrderInfo>();
                filteredOrderList = new List<ProductionOrderInfo>();
            }
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                int selectedId = currentOrder != null ? currentOrder.Id : 0;

                dataGridViewOrders.DataSource = null;
                dataGridViewOrders.DataSource = filteredOrderList;

                // 如果有数据，尽量保持原选择（找不到则选中第一条）
                if (filteredOrderList.Count > 0)
                {
                    var itemToSelect = filteredOrderList.FirstOrDefault(o => o.Id == selectedId) ?? filteredOrderList[0];
                    SelectOrderById(itemToSelect.Id);
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
                if (_searchDebounceTimer == null)
                {
                    ApplySearchFilter();
                    RefreshDataGridView();
                    return;
                }

                _searchDebounceTimer.Stop();
                _searchDebounceTimer.Start();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索失败", ex);
                MessageBox.Show("搜索失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSearchDebounceTimer()
        {
            _searchDebounceTimer = new Timer();
            _searchDebounceTimer.Interval = 300;
            _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
        }

        private void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();

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

        private void ApplySearchFilter()
        {
            string searchTerm = textBoxSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredOrderList = new List<ProductionOrderInfo>(orderList);
                return;
            }

            // 根据订单编号、产品名称、状态进行搜索
            filteredOrderList = orderList.Where(o =>
                (o.OrderNo ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (o.ProductName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (o.Status ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (o.WorkshopName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_searchDebounceTimer != null)
            {
                _searchDebounceTimer.Stop();
                _searchDebounceTimer.Dispose();
                _searchDebounceTimer = null;
            }

            base.OnFormClosed(e);
        }

        /// <summary>
        /// DataGridView选择变化事件
        /// </summary>
        private void dataGridViewOrders_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewOrders.CurrentRow != null &&
                    dataGridViewOrders.CurrentRow.DataBoundItem != null)
                {
                    var order = dataGridViewOrders.CurrentRow.DataBoundItem as ProductionOrderInfo;
                    if (order != null)
                    {
                        ShowOrderDetails(order);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("显示订单详情失败", ex);
            }
        }

        /// <summary>
        /// 显示订单详情
        /// </summary>
        private void ShowOrderDetails(ProductionOrderInfo order)
        {
            if (order == null)
            {
                ClearDetailsPanel();
                return;
            }

            currentOrder = order;

            // 填充基本信息
            textBoxOrderNo.Text = order.OrderNo;
            textBoxProductName.Text = order.ProductName;
            textBoxProductCode.Text = order.ProductCode;
            textBoxQuantity.Text = order.Quantity.ToString("F2");
            textBoxActualQuantity.Text = order.ActualQuantity.ToString("F2");
            textBoxUnit.Text = order.Unit;

            // 填充状态信息
            comboBoxStatus.Text = order.Status;
            comboBoxPriority.Text = order.Priority;
            textBoxWorkshopName.Text = order.WorkshopName;
            textBoxResponsiblePerson.Text = order.ResponsiblePerson;

            // 填充时间信息
            dateTimePickerPlanStart.Value = order.PlanStartTime;
            dateTimePickerPlanEnd.Value = order.PlanEndTime;

            if (order.ActualStartTime.HasValue)
            {
                dateTimePickerActualStart.Value = order.ActualStartTime.Value;
                dateTimePickerActualStart.Enabled = true;
            }
            else
            {
                dateTimePickerActualStart.Value = DateTime.Now;
                dateTimePickerActualStart.Enabled = false;
            }

            if (order.ActualEndTime.HasValue)
            {
                dateTimePickerActualEnd.Value = order.ActualEndTime.Value;
                dateTimePickerActualEnd.Enabled = true;
            }
            else
            {
                dateTimePickerActualEnd.Value = DateTime.Now;
                dateTimePickerActualEnd.Enabled = false;
            }

            // 填充其他信息
            textBoxCustomerName.Text = order.CustomerName;
            textBoxSalesOrderNumber.Text = order.SalesOrderNumber;
            textBoxRemarks.Text = order.Remarks;

            UpdateDetailsPanel();
        }

        /// <summary>
        /// 清空详情面板
        /// </summary>
        private void ClearDetailsPanel()
        {
            currentOrder = null;

            textBoxOrderNo.Text = string.Empty;
            textBoxProductName.Text = string.Empty;
            textBoxProductCode.Text = string.Empty;
            textBoxQuantity.Text = string.Empty;
            textBoxActualQuantity.Text = string.Empty;
            textBoxUnit.Text = string.Empty;

            comboBoxStatus.Text = string.Empty;
            comboBoxPriority.Text = string.Empty;
            textBoxWorkshopName.Text = string.Empty;
            textBoxResponsiblePerson.Text = string.Empty;

            dateTimePickerPlanStart.Value = DateTime.Now;
            dateTimePickerPlanEnd.Value = DateTime.Now;
            dateTimePickerActualStart.Value = DateTime.Now;
            dateTimePickerActualEnd.Value = DateTime.Now;
            dateTimePickerActualStart.Enabled = false;
            dateTimePickerActualEnd.Enabled = false;

            textBoxCustomerName.Text = string.Empty;
            textBoxSalesOrderNumber.Text = string.Empty;
            textBoxRemarks.Text = string.Empty;

            try
            {
                if (_lblSummaryOrderNo != null) _lblSummaryOrderNo.Text = "未选择订单";
                if (_lblSummaryProduct != null) _lblSummaryProduct.Text = "请选择左侧订单查看详情";

                ApplyStatusBadge(string.Empty);
                ApplyPriorityBadge(string.Empty);

                if (_progressBar != null) _progressBar.Value = 0;
                if (_lblProgressText != null) _lblProgressText.Text = "—";

                if (_valProductCode != null) _valProductCode.Text = "—";
                if (_valPlannedQty != null) _valPlannedQty.Text = "—";
                if (_valActualQty != null) _valActualQty.Text = "—";
                if (_valUnit != null) _valUnit.Text = "—";
                if (_valWorkshop != null) _valWorkshop.Text = "—";
                if (_valResponsible != null) _valResponsible.Text = "—";
                if (_valPlanWindow != null) _valPlanWindow.Text = "—";
                if (_valActualWindow != null) { _valActualWindow.Text = "—"; _valActualWindow.ForeColor = LeagueColors.TextDisabled; }

                if (_valCustomer != null) _valCustomer.Text = "—";
                if (_valSalesOrder != null) _valSalesOrder.Text = "—";
                if (_txtRemarks != null) _txtRemarks.Text = string.Empty;

                if (_btnEditQuick != null) _btnEditQuick.Enabled = false;
                if (_btnCopyOrderNo != null) _btnCopyOrderNo.Enabled = false;
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建新订单对话框
                var result = ShowOrderEditDialog(null);
                if (result != null)
                {
                    // 设置创建时间和更新时间
                    result.CreateTime = DateTime.Now;
                    result.UpdateTime = DateTime.Now;

                    // 调用BLL层保存到数据库
                    bool saveResult = _productionOrderBLL.AddProductionOrder(result);

                    if (saveResult)
                    {
                        // 重新加载数据以获取最新的ID
                        LoadProductionOrderData();

                        // 刷新显示
                        RefreshDataGridView();

                        // 尝试选中新添加的订单
                        if (!string.IsNullOrEmpty(result.OrderNo))
                        {
                            SelectOrderByNo(result.OrderNo);
                        }

                        MessageBox.Show("生产订单添加成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("添加生产订单：{0} - {1}", result.OrderNo, result.ProductName));
                    }
                    else
                    {
                        MessageBox.Show("保存生产订单失败，请检查数据库连接", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加生产订单失败", ex);
                MessageBox.Show("添加生产订单失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentOrder == null)
                {
                    MessageBox.Show("请先选择要编辑的生产订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 创建编辑对话框
                var result = ShowOrderEditDialog(currentOrder);
                if (result != null)
                {
                    // 设置更新时间
                    result.Id = currentOrder.Id;
                    result.UpdateTime = DateTime.Now;

                    // 调用BLL层更新到数据库
                    var success = _productionOrderBLL.UpdateProductionOrder(result);
                    if (!success)
                    {
                        MessageBox.Show("生产订单编辑失败：记录可能已不存在。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载数据以获取最新状态
                    LoadProductionOrderData();

                    // 刷新显示
                    RefreshDataGridView();

                    // 重新选中编辑的订单
                    SelectOrderById(result.Id);

                    MessageBox.Show("生产订单编辑成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("编辑生产订单：{0} - {1}",
                        result.OrderNo, result.ProductName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("编辑生产订单失败", ex);
                MessageBox.Show("编辑生产订单失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentOrder == null)
                {
                    MessageBox.Show("请先选择要删除的生产订单！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    string.Format("确认要删除生产订单 [{0} - {1}] 吗？\n\n此操作不可撤销！",
                        currentOrder.OrderNo, currentOrder.ProductName),
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // 调用BLL层删除数据库记录
                    var success = _productionOrderBLL.DeleteProductionOrder(currentOrder.Id);
                    if (!success)
                    {
                        MessageBox.Show("生产订单删除失败：记录可能已不存在或状态不允许删除。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载数据
                    LoadProductionOrderData();

                    // 刷新显示
                    RefreshDataGridView();

                    MessageBox.Show("生产订单删除成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("删除生产订单：{0} - {1}",
                        currentOrder.OrderNo, currentOrder.ProductName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除生产订单失败", ex);
                MessageBox.Show("删除生产订单失败：" + ex.Message, "错误",
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
                LoadProductionOrderData();
                RefreshDataGridView();

                LogManager.Info("生产订单数据已刷新");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show("刷新数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据ID选中订单
        /// </summary>
        private void SelectOrderById(int orderId)
        {
            try
            {
                for (int i = 0; i < dataGridViewOrders.Rows.Count; i++)
                {
                    var order = dataGridViewOrders.Rows[i].DataBoundItem as ProductionOrderInfo;
                    if (order != null && order.Id == orderId)
                    {
                        dataGridViewOrders.ClearSelection();
                        dataGridViewOrders.Rows[i].Selected = true;
                        dataGridViewOrders.CurrentCell = dataGridViewOrders.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("选中订单失败", ex);
            }
        }

        /// <summary>
        /// 根据订单号选中订单
        /// </summary>
        private void SelectOrderByNo(string orderNo)
        {
            try
            {
                for (int i = 0; i < dataGridViewOrders.Rows.Count; i++)
                {
                    var order = dataGridViewOrders.Rows[i].DataBoundItem as ProductionOrderInfo;
                    if (order != null && order.OrderNo == orderNo)
                    {
                        dataGridViewOrders.ClearSelection();
                        dataGridViewOrders.Rows[i].Selected = true;
                        dataGridViewOrders.CurrentCell = dataGridViewOrders.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("根据订单号选中订单失败", ex);
            }
        }

        private void DataGridViewOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataGridViewOrders == null) return;
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                if (dataGridViewOrders.Columns == null || e.ColumnIndex >= dataGridViewOrders.Columns.Count) return;

                var column = dataGridViewOrders.Columns[e.ColumnIndex];
                if (column == null) return;

                if (string.Equals(column.Name, "Status", StringComparison.OrdinalIgnoreCase))
                {
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
                    else if (string.Equals(status, "已暂停", StringComparison.OrdinalIgnoreCase))
                    {
                        fore = LeagueColors.WarningOrange;
                        back = Color.FromArgb(52, 36, 12);
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
                    return;
                }

                if (string.Equals(column.Name, "Priority", StringComparison.OrdinalIgnoreCase))
                {
                    var p = e.Value != null ? e.Value.ToString() : string.Empty;
                    if (string.Equals(p, "紧急", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.ForeColor = LeagueColors.ErrorRed;
                    }
                    else if (string.Equals(p, "重要", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.ForeColor = LeagueColors.RiotGoldHover;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = LeagueColors.TextSecondary;
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        #region LoL 详情卡（布局 A）

        private void UpdateDetailsPanel()
        {
            try
            {
                if (_detailsLayout == null) return;

                var o = currentOrder;
                if (o == null)
                {
                    return;
                }

                var orderNo = o.OrderNo ?? string.Empty;
                var productName = o.ProductName ?? string.Empty;
                var status = o.Status ?? string.Empty;
                var priority = o.Priority ?? string.Empty;

                if (_lblSummaryOrderNo != null) _lblSummaryOrderNo.Text = string.IsNullOrWhiteSpace(orderNo) ? "未选择订单" : orderNo;
                if (_lblSummaryProduct != null) _lblSummaryProduct.Text = string.IsNullOrWhiteSpace(productName) ? "—" : productName;

                ApplyStatusBadge(status);
                ApplyPriorityBadge(priority);

                decimal planned = o.Quantity;
                decimal actual = o.ActualQuantity;

                float progress = 0f;
                if (planned > 0m)
                {
                    progress = (float)Math.Max(0m, Math.Min(1m, actual / planned));
                }

                if (_progressBar != null)
                {
                    try { _progressBar.Progress = progress; } catch { }
                }

                if (_lblProgressText != null)
                {
                    var percent = (int)Math.Round(progress * 100, MidpointRounding.AwayFromZero);
                    _lblProgressText.Text = planned > 0m
                        ? string.Format("已完成 {0} / {1}（{2}%）", FormatQty(actual), FormatQty(planned), percent)
                        : string.Format("已完成 {0}", FormatQty(actual));
                }

                if (_valProductCode != null) _valProductCode.Text = string.IsNullOrWhiteSpace(o.ProductCode) ? "—" : o.ProductCode;
                if (_valPlannedQty != null) _valPlannedQty.Text = FormatQty(planned);
                if (_valActualQty != null) _valActualQty.Text = FormatQty(actual);
                if (_valUnit != null) _valUnit.Text = string.IsNullOrWhiteSpace(o.Unit) ? "—" : o.Unit;
                if (_valWorkshop != null) _valWorkshop.Text = string.IsNullOrWhiteSpace(o.WorkshopName) ? "—" : o.WorkshopName;
                if (_valResponsible != null) _valResponsible.Text = string.IsNullOrWhiteSpace(o.ResponsiblePerson) ? "—" : o.ResponsiblePerson;

                if (_valPlanWindow != null)
                {
                    _valPlanWindow.Text = string.Format("{0} ~ {1}", FormatDate(o.PlanStartTime), FormatDate(o.PlanEndTime));
                }

                if (_valActualWindow != null)
                {
                    var s = o.ActualStartTime.HasValue ? FormatDate(o.ActualStartTime.Value) : "—";
                    var e = o.ActualEndTime.HasValue ? FormatDate(o.ActualEndTime.Value) : "—";
                    _valActualWindow.Text = string.Format("{0} ~ {1}", s, e);
                    _valActualWindow.ForeColor = (o.ActualStartTime.HasValue || o.ActualEndTime.HasValue) ? LeagueColors.TextPrimary : LeagueColors.TextDisabled;
                }

                if (_valCustomer != null) _valCustomer.Text = string.IsNullOrWhiteSpace(o.CustomerName) ? "—" : o.CustomerName;
                if (_valSalesOrder != null) _valSalesOrder.Text = string.IsNullOrWhiteSpace(o.SalesOrderNumber) ? "—" : o.SalesOrderNumber;
                if (_txtRemarks != null) _txtRemarks.Text = o.Remarks ?? string.Empty;

                var hasNo = !string.IsNullOrWhiteSpace(orderNo) || o.Id > 0;
                if (_btnEditQuick != null) _btnEditQuick.Enabled = hasNo;
                if (_btnCopyOrderNo != null) _btnCopyOrderNo.Enabled = !string.IsNullOrWhiteSpace(orderNo);
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

                if (string.Equals(status, "待开始", StringComparison.OrdinalIgnoreCase) || string.Equals(status, "待下发", StringComparison.OrdinalIgnoreCase))
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
                else if (string.Equals(status, "已暂停", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.WarningOrange;
                    back = Color.FromArgb(52, 36, 12);
                }
                else if (string.Equals(status, "已取消", StringComparison.OrdinalIgnoreCase) || string.Equals(status, "已关闭", StringComparison.OrdinalIgnoreCase))
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

        private void ApplyPriorityBadge(string priority)
        {
            try
            {
                if (_panelPriorityBadge == null || _lblPriorityBadgeText == null) return;

                var p = priority ?? string.Empty;
                var fore = LeagueColors.TextSecondary;
                var back = Color.FromArgb(35, 35, 35);
                var text = string.IsNullOrWhiteSpace(p) ? "—" : p;

                if (string.Equals(p, "紧急", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.ErrorRed;
                    back = Color.FromArgb(52, 20, 20);
                }
                else if (string.Equals(p, "重要", StringComparison.OrdinalIgnoreCase))
                {
                    fore = LeagueColors.RiotGoldHover;
                    back = Color.FromArgb(45, 40, 28);
                }

                _panelPriorityBadge.BackColor = back;
                _lblPriorityBadgeText.ForeColor = fore;
                _lblPriorityBadgeText.Text = text;
            }
            catch
            {
                // ignore
            }
        }

        private static string FormatQty(decimal value)
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

        private static string FormatDate(DateTime dt)
        {
            try
            {
                return dt.ToString("yyyy-MM-dd");
            }
            catch
            {
                return dt.ToString();
            }
        }

        private void BuildLolLayoutA()
        {
            if (_lolLayoutABuilt) return;
            _lolLayoutABuilt = true;

            if (panelMain == null || dataGridViewOrders == null) return;

            try
            {
                panelMain.SuspendLayout();

                // 防重复：如果已是 SplitContainer 则跳过
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
                _splitMain.Panel1MinSize = 560;
                _splitMain.Panel2MinSize = 360;
                _splitMain.FixedPanel = FixedPanel.Panel2;
                _splitMain.BackColor = LeagueColors.SeparatorColor;

                // 左：列表卡片
                _splitMain.Panel1.Padding = new Padding(12, 12, 8, 12);
                var leftCard = CreateLolCardPanel();
                leftCard.Padding = new Padding(12);
                _splitMain.Panel1.Controls.Add(leftCard);

                var leftLayout = new TableLayoutPanel();
                leftLayout.Dock = DockStyle.Fill;
                leftLayout.BackColor = Color.Transparent;
                leftLayout.ColumnCount = 1;
                leftLayout.RowCount = 2;
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                leftCard.Controls.Add(leftLayout);

                var leftTitle = new Label();
                leftTitle.Dock = DockStyle.Fill;
                leftTitle.TextAlign = ContentAlignment.MiddleLeft;
                leftTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
                leftTitle.ForeColor = LeagueColors.RiotGoldHover;
                leftTitle.Text = "生产订单列表";
                leftLayout.Controls.Add(leftTitle, 0, 0);

                dataGridViewOrders.Dock = DockStyle.Fill;
                leftLayout.Controls.Add(dataGridViewOrders, 0, 1);

                // 右：详情栏
                _splitMain.Panel2.Padding = new Padding(12, 12, 12, 12);
                var detailsHost = new Panel();
                detailsHost.Dock = DockStyle.Fill;
                detailsHost.BackColor = Color.Transparent;
                _splitMain.Panel2.Controls.Add(detailsHost);

                _detailsLayout = new TableLayoutPanel();
                _detailsLayout.Dock = DockStyle.Fill;
                _detailsLayout.ColumnCount = 1;
                _detailsLayout.RowCount = 5;
                _detailsLayout.BackColor = Color.Transparent;
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 138F)); // 概览
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));  // 进度
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // 关键信息
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F)); // 客户/备注
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));  // 操作
                detailsHost.Controls.Add(_detailsLayout);

                var cardSummary = CreateLolCardPanel();
                var cardProgress = CreateLolCardPanel();
                var cardInfo = CreateLolCardPanel();
                var cardOther = CreateLolCardPanel();
                var cardActions = CreateLolCardPanel();

                _detailsLayout.Controls.Add(cardSummary, 0, 0);
                _detailsLayout.Controls.Add(cardProgress, 0, 1);
                _detailsLayout.Controls.Add(cardInfo, 0, 2);
                _detailsLayout.Controls.Add(cardOther, 0, 3);
                _detailsLayout.Controls.Add(cardActions, 0, 4);

                BuildSummaryCard(cardSummary);
                BuildProgressCard(cardProgress);
                BuildInfoCard(cardInfo);
                BuildOtherCard(cardOther);
                BuildActionsCard(cardActions);

                panelMain.Controls.Add(_splitMain);

                // 初始分隔：给右侧固定宽度
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
                var detailsWidth = 520;
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
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 2;
            layout.RowCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 128F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "订单概览";
            lblTitle.AutoEllipsis = true;
            layout.Controls.Add(lblTitle, 0, 0);
            layout.SetColumnSpan(lblTitle, 2);

            _lblSummaryOrderNo = new Label();
            _lblSummaryOrderNo.Dock = DockStyle.Fill;
            _lblSummaryOrderNo.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummaryOrderNo.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            _lblSummaryOrderNo.ForeColor = LeagueColors.TextHighlight;
            _lblSummaryOrderNo.Text = "未选择订单";
            _lblSummaryOrderNo.AutoEllipsis = true;
            layout.Controls.Add(_lblSummaryOrderNo, 0, 1);

            var badgeStack = new TableLayoutPanel();
            badgeStack.Dock = DockStyle.Fill;
            badgeStack.BackColor = Color.Transparent;
            badgeStack.ColumnCount = 1;
            badgeStack.RowCount = 2;
            badgeStack.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            badgeStack.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            layout.Controls.Add(badgeStack, 1, 1);
            layout.SetRowSpan(badgeStack, 2);

            _panelStatusBadge = new Panel();
            _panelStatusBadge.Dock = DockStyle.Fill;
            _panelStatusBadge.BackColor = Color.FromArgb(35, 35, 35);
            _panelStatusBadge.Padding = new Padding(6, 2, 6, 2);
            badgeStack.Controls.Add(_panelStatusBadge, 0, 0);

            _lblStatusBadgeText = new Label();
            _lblStatusBadgeText.Dock = DockStyle.Fill;
            _lblStatusBadgeText.TextAlign = ContentAlignment.MiddleCenter;
            _lblStatusBadgeText.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            _lblStatusBadgeText.Text = "—";
            _lblStatusBadgeText.ForeColor = LeagueColors.TextDisabled;
            _lblStatusBadgeText.BackColor = Color.Transparent;
            _panelStatusBadge.Controls.Add(_lblStatusBadgeText);

            _panelPriorityBadge = new Panel();
            _panelPriorityBadge.Dock = DockStyle.Fill;
            _panelPriorityBadge.BackColor = Color.FromArgb(35, 35, 35);
            _panelPriorityBadge.Padding = new Padding(6, 2, 6, 2);
            badgeStack.Controls.Add(_panelPriorityBadge, 0, 1);

            _lblPriorityBadgeText = new Label();
            _lblPriorityBadgeText.Dock = DockStyle.Fill;
            _lblPriorityBadgeText.TextAlign = ContentAlignment.MiddleCenter;
            _lblPriorityBadgeText.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            _lblPriorityBadgeText.Text = "—";
            _lblPriorityBadgeText.ForeColor = LeagueColors.TextDisabled;
            _lblPriorityBadgeText.BackColor = Color.Transparent;
            _panelPriorityBadge.Controls.Add(_lblPriorityBadgeText);

            _lblSummaryProduct = new Label();
            _lblSummaryProduct.Dock = DockStyle.Fill;
            _lblSummaryProduct.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummaryProduct.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            _lblSummaryProduct.ForeColor = LeagueColors.TextSecondary;
            _lblSummaryProduct.Text = "请选择左侧订单查看详情";
            _lblSummaryProduct.AutoEllipsis = true;
            layout.Controls.Add(_lblSummaryProduct, 0, 2);
        }

        private void BuildProgressCard(Panel host)
        {
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
            info.RowCount = 8;
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86F));
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 8; i++)
            {
                info.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            }
            layout.Controls.Add(info, 0, 1);

            AddInfoRow(info, 0, "产品编码", out _valProductCode);
            AddInfoRow(info, 1, "计划数", out _valPlannedQty);
            AddInfoRow(info, 2, "已完成", out _valActualQty);
            AddInfoRow(info, 3, "单位", out _valUnit);
            AddInfoRow(info, 4, "车间", out _valWorkshop);
            AddInfoRow(info, 5, "负责人", out _valResponsible);
            AddInfoRow(info, 6, "计划", out _valPlanWindow);
            AddInfoRow(info, 7, "实际", out _valActualWindow);
        }

        private void BuildOtherCard(Panel host)
        {
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "客户与备注";
            layout.Controls.Add(lblTitle, 0, 0);

            var kv = new TableLayoutPanel();
            kv.Dock = DockStyle.Fill;
            kv.BackColor = Color.Transparent;
            kv.ColumnCount = 2;
            kv.RowCount = 2;
            kv.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86F));
            kv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            kv.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            kv.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            layout.Controls.Add(kv, 0, 1);

            AddInfoRow(kv, 0, "客户", out _valCustomer);
            AddInfoRow(kv, 1, "销售单", out _valSalesOrder);

            _txtRemarks = textBoxRemarks;
            if (_txtRemarks == null)
            {
                _txtRemarks = new TextBox();
                _txtRemarks.Multiline = true;
                _txtRemarks.ScrollBars = ScrollBars.Vertical;
                _txtRemarks.ReadOnly = true;
            }

            _txtRemarks.Dock = DockStyle.Fill;
            _txtRemarks.BorderStyle = BorderStyle.None;
            _txtRemarks.BackColor = LeagueColors.InputBackground;
            _txtRemarks.ForeColor = Color.FromArgb(241, 241, 241);
            layout.Controls.Add(_txtRemarks, 0, 2);
        }

        private void BuildActionsCard(Panel host)
        {
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

            _btnEditQuick = new LolActionButton();
            _btnEditQuick.Text = "编辑";
            _btnEditQuick.Width = 90;
            _btnEditQuick.Compact = true;
            _btnEditQuick.Enabled = false;
            _btnEditQuick.Click += (s, e) =>
            {
                try
                {
                    if (btnEdit != null)
                    {
                        btnEdit.PerformClick();
                    }
                }
                catch
                {
                    // ignore
                }
            };

            _btnCopyOrderNo = new LolActionButton();
            _btnCopyOrderNo.Text = "复制订单号";
            _btnCopyOrderNo.Width = 120;
            _btnCopyOrderNo.Compact = true;
            _btnCopyOrderNo.Enabled = false;
            _btnCopyOrderNo.Click += (s, e) =>
            {
                try
                {
                    var o = currentOrder;
                    var orderNo = o != null ? o.OrderNo : null;
                    if (string.IsNullOrWhiteSpace(orderNo)) return;
                    try { Clipboard.SetText(orderNo); } catch { }
                }
                catch
                {
                    // ignore
                }
            };

            flow.Controls.Add(_btnEditQuick);
            flow.Controls.Add(_btnCopyOrderNo);
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

        #endregion

        private void ApplyLolPolish()
        {
            try
            {
                // 文案（去 emoji，更贴近 LoL 客户端克制风）
                if (labelTitle != null)
                {
                    labelTitle.Text = "生产订单管理";
                    labelTitle.ForeColor = LeagueColors.RiotGoldHover;
                }

                if (labelSearch != null)
                {
                    labelSearch.Text = "搜索：";
                    labelSearch.ForeColor = LeagueColors.TextSecondary;
                }

                if (btnAdd != null) btnAdd.Text = "新增";
                if (btnEdit != null) btnEdit.Text = "编辑";
                if (btnDelete != null) btnDelete.Text = "删除";
                if (btnRefresh != null) btnRefresh.Text = "刷新";

                if (panelTop != null)
                {
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
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// 显示订单编辑对话框
        /// </summary>
        private ProductionOrderInfo ShowOrderEditDialog(ProductionOrderInfo order)
        {
            try
            {
                // 使用真实的编辑对话框
                using (var editForm = new ProductionOrderEditForm(order))
                {
                    if (editForm.ShowDialog(this) == DialogResult.OK)
                    {
                        // 返回编辑后的订单数据
                        return editForm.OrderData;
                    }
                }

                // 用户取消编辑
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error("显示订单编辑对话框失败", ex);
                MessageBox.Show(string.Format("打开编辑对话框失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
