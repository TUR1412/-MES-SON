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
    /// 生产订单管理窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class ProductionOrderManagementForm : Form
    {
        private readonly IProductionOrderBLL _productionOrderBLL;

        private List<ProductionOrderInfo> orderList;
        private List<ProductionOrderInfo> filteredOrderList;
        private ProductionOrderInfo currentOrder;

        public ProductionOrderManagementForm()
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
                orderList = new List<ProductionOrderInfo>();
                filteredOrderList = new List<ProductionOrderInfo>();
                currentOrder = null;

                // 设置DataGridView
                SetupDataGridView();

                // 加载数据（离线/数据库由 BLL 决定）
                LoadData();

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

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "OrderNo",
                HeaderText = "订单编号",
                DataPropertyName = "OrderNo",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 200,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "计划数量",
                DataPropertyName = "Quantity",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ActualQuantity",
                HeaderText = "完成数量",
                DataPropertyName = "ActualQuantity",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "订单状态",
                DataPropertyName = "Status",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Priority",
                HeaderText = "优先级",
                DataPropertyName = "Priority",
                Width = 80,
                ReadOnly = true
            });

            dataGridViewOrders.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopName",
                HeaderText = "负责车间",
                DataPropertyName = "WorkshopName",
                Width = 120,
                ReadOnly = true
            });

            // 设置样式
            dataGridViewOrders.EnableHeadersVisualStyles = false;
            dataGridViewOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewOrders.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            dataGridViewOrders.ColumnHeadersHeight = 40;

            dataGridViewOrders.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
            dataGridViewOrders.DefaultCellStyle.BackColor = Color.White;
            dataGridViewOrders.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewOrders.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridViewOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridViewOrders.GridColor = Color.FromArgb(222, 226, 230);
        }

        /// <summary>
        /// 加载生产订单数据（离线/数据库由 BLL 决定）
        /// </summary>
        private void LoadData()
        {
            orderList.Clear();

            var data = _productionOrderBLL.GetAllProductionOrders();
            if (data != null && data.Count > 0)
            {
                orderList.AddRange(data);
            }

            filteredOrderList = new List<ProductionOrderInfo>(orderList);
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                dataGridViewOrders.DataSource = null;
                dataGridViewOrders.DataSource = filteredOrderList;

                // 如果有数据，选中第一行
                if (filteredOrderList.Count > 0)
                {
                    dataGridViewOrders.Rows[0].Selected = true;
                    ShowOrderDetails(filteredOrderList[0]);
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
                string searchTerm = textBoxSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    // 显示所有订单
                    filteredOrderList = new List<ProductionOrderInfo>(orderList);
                }
                else
                {
                    // 根据订单编号、产品名称、状态进行搜索
                    filteredOrderList = orderList.Where(o =>
                        (o.OrderNo ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (o.ProductName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (o.Status ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (o.WorkshopName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
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
                    var success = _productionOrderBLL.AddProductionOrder(result);
                    if (!success)
                    {
                        MessageBox.Show("生产订单添加失败！", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    // 选中新添加的订单（按订单号查找）
                    var added = orderList.FirstOrDefault(o =>
                        !string.IsNullOrEmpty(o.OrderNo) &&
                        o.OrderNo.Equals(result.OrderNo, StringComparison.OrdinalIgnoreCase));
                    if (added != null)
                    {
                        SelectOrderById(added.Id);
                    }

                    MessageBox.Show("生产订单添加成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("添加生产订单：{0} - {1}", result.OrderNo, result.ProductName));
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
                    result.Id = currentOrder.Id;

                    var success = _productionOrderBLL.UpdateProductionOrder(result);
                    if (!success)
                    {
                        MessageBox.Show("生产订单编辑失败：记录可能已不存在。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    // 重新选中编辑的订单
                    SelectOrderById(result.Id);

                    MessageBox.Show("生产订单编辑成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("编辑生产订单：{0} - {1}", result.OrderNo, result.ProductName));
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
                    var success = _productionOrderBLL.DeleteProductionOrder(currentOrder.Id);
                    if (!success)
                    {
                        MessageBox.Show("生产订单删除失败：记录可能已不存在或状态不允许删除。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
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
                LoadData();
                RefreshDataGridView();

                MessageBox.Show("数据刷新成功！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        /// 显示订单编辑对话框
        /// </summary>
        private ProductionOrderInfo ShowOrderEditDialog(ProductionOrderInfo order)
        {
            using (var dialog = new ProductionOrderEditDialog(order))
            {
                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.Result : null;
            }
        }

        private sealed class ProductionOrderEditDialog : Form
        {
            private readonly TextBox _textOrderNo;
            private readonly TextBox _textProductCode;
            private readonly TextBox _textProductName;
            private readonly NumericUpDown _numQuantity;
            private readonly TextBox _textUnit;
            private readonly ComboBox _comboPriority;
            private readonly TextBox _textWorkshopName;
            private readonly TextBox _textResponsiblePerson;
            private readonly TextBox _textCustomerName;
            private readonly TextBox _textSalesOrderNumber;
            private readonly DateTimePicker _dtPlanStart;
            private readonly DateTimePicker _dtPlanEnd;
            private readonly TextBox _textRemarks;
            private readonly TextBox _textStatus;

            public ProductionOrderInfo Result { get; private set; }

            public ProductionOrderEditDialog(ProductionOrderInfo order)
            {
                Text = order == null ? "新增生产订单" : "编辑生产订单";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                Width = 640;
                Height = 680;

                var layout = new TableLayoutPanel();
                layout.Dock = DockStyle.Fill;
                layout.Padding = new Padding(12);
                layout.ColumnCount = 2;
                layout.RowCount = 14;
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                for (int i = 0; i < layout.RowCount; i++)
                {
                    layout.RowStyles.Add(new RowStyle(SizeType.Absolute, i == 13 ? 110 : 34));
                }

                _textOrderNo = new TextBox { Dock = DockStyle.Fill };
                _textProductCode = new TextBox { Dock = DockStyle.Fill };
                _textProductName = new TextBox { Dock = DockStyle.Fill };
                _numQuantity = new NumericUpDown { Dock = DockStyle.Left, Width = 220, DecimalPlaces = 4, Maximum = 99999999, Minimum = 0 };
                _textUnit = new TextBox { Dock = DockStyle.Fill };

                _comboPriority = new ComboBox { Dock = DockStyle.Left, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
                _comboPriority.Items.AddRange(new object[] { "普通", "重要", "紧急" });

                _textWorkshopName = new TextBox { Dock = DockStyle.Fill };
                _textResponsiblePerson = new TextBox { Dock = DockStyle.Fill };
                _textCustomerName = new TextBox { Dock = DockStyle.Fill };
                _textSalesOrderNumber = new TextBox { Dock = DockStyle.Fill };

                _dtPlanStart = new DateTimePicker { Dock = DockStyle.Left, Width = 220, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };
                _dtPlanEnd = new DateTimePicker { Dock = DockStyle.Left, Width = 220, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm" };

                _textStatus = new TextBox { Dock = DockStyle.Fill, ReadOnly = true };
                _textRemarks = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };

                AddRow(layout, 0, "订单编号", _textOrderNo);
                AddRow(layout, 1, "产品编码", _textProductCode);
                AddRow(layout, 2, "产品名称", _textProductName);
                AddRow(layout, 3, "计划数量", _numQuantity);
                AddRow(layout, 4, "单位", _textUnit);
                AddRow(layout, 5, "优先级", _comboPriority);
                AddRow(layout, 6, "负责车间", _textWorkshopName);
                AddRow(layout, 7, "负责人", _textResponsiblePerson);
                AddRow(layout, 8, "客户名称", _textCustomerName);
                AddRow(layout, 9, "销售订单号", _textSalesOrderNumber);
                AddRow(layout, 10, "计划开始时间", _dtPlanStart);
                AddRow(layout, 11, "计划结束时间", _dtPlanEnd);
                AddRow(layout, 12, "状态", _textStatus);
                AddRow(layout, 13, "备注", _textRemarks);

                var panelButtons = new FlowLayoutPanel();
                panelButtons.Dock = DockStyle.Bottom;
                panelButtons.FlowDirection = FlowDirection.RightToLeft;
                panelButtons.Padding = new Padding(12);
                panelButtons.Height = 54;

                var btnOk = new Button { Text = "确定", Width = 90, Height = 30, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "取消", Width = 90, Height = 30, DialogResult = DialogResult.Cancel };
                btnOk.Click += (s, e) =>
                {
                    if (!TryBuildResult(order))
                    {
                        DialogResult = DialogResult.None;
                    }
                };

                panelButtons.Controls.Add(btnOk);
                panelButtons.Controls.Add(btnCancel);

                Controls.Add(layout);
                Controls.Add(panelButtons);

                AcceptButton = btnOk;
                CancelButton = btnCancel;

                if (order != null)
                {
                    _textOrderNo.Text = order.OrderNo ?? string.Empty;
                    _textProductCode.Text = order.ProductCode ?? string.Empty;
                    _textProductName.Text = order.ProductName ?? string.Empty;
                    _numQuantity.Value = order.Quantity;
                    _textUnit.Text = order.Unit ?? string.Empty;
                    _comboPriority.SelectedItem = string.IsNullOrEmpty(order.Priority) ? "普通" : order.Priority;
                    _textWorkshopName.Text = order.WorkshopName ?? string.Empty;
                    _textResponsiblePerson.Text = order.ResponsiblePerson ?? string.Empty;
                    _textCustomerName.Text = order.CustomerName ?? string.Empty;
                    _textSalesOrderNumber.Text = order.SalesOrderNumber ?? string.Empty;
                    _dtPlanStart.Value = order.PlanStartTime == DateTime.MinValue ? DateTime.Now : order.PlanStartTime;
                    _dtPlanEnd.Value = order.PlanEndTime == DateTime.MinValue ? DateTime.Now.AddDays(7) : order.PlanEndTime;
                    _textRemarks.Text = order.Remarks ?? string.Empty;
                    _textStatus.Text = order.Status ?? string.Empty;
                }
                else
                {
                    var now = DateTime.Now;
                    _textOrderNo.Text = "PO" + now.ToString("yyyyMMddHHmmss");
                    _textSalesOrderNumber.Text = "SO" + now.ToString("yyyyMMddHHmmss");
                    _numQuantity.Value = 1;
                    _textUnit.Text = "个";
                    _comboPriority.SelectedItem = "普通";
                    _dtPlanStart.Value = now.AddDays(1);
                    _dtPlanEnd.Value = now.AddDays(7);
                    _textStatus.Text = "待开始";
                }
            }

            private static void AddRow(TableLayoutPanel layout, int row, string labelText, Control control)
            {
                var label = new Label
                {
                    Text = labelText + "：",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight
                };

                layout.Controls.Add(label, 0, row);
                layout.Controls.Add(control, 1, row);
            }

            private bool TryBuildResult(ProductionOrderInfo original)
            {
                var orderNo = (_textOrderNo.Text ?? string.Empty).Trim();
                var productCode = (_textProductCode.Text ?? string.Empty).Trim();
                var productName = (_textProductName.Text ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(orderNo))
                {
                    MessageBox.Show("订单编号不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(productCode))
                {
                    MessageBox.Show("产品编码不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(productName))
                {
                    MessageBox.Show("产品名称不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (_numQuantity.Value <= 0)
                {
                    MessageBox.Show("计划数量必须大于0！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (_dtPlanStart.Value >= _dtPlanEnd.Value)
                {
                    MessageBox.Show("计划开始时间必须早于计划结束时间！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var entity = original != null ? original.Clone() : new ProductionOrderInfo();
                entity.OrderNo = orderNo;
                entity.ProductCode = productCode;
                entity.ProductName = productName;
                entity.Quantity = _numQuantity.Value;
                entity.Unit = (_textUnit.Text ?? string.Empty).Trim();
                entity.Priority = _comboPriority.SelectedItem != null ? _comboPriority.SelectedItem.ToString() : "普通";
                entity.WorkshopName = (_textWorkshopName.Text ?? string.Empty).Trim();
                entity.ResponsiblePerson = (_textResponsiblePerson.Text ?? string.Empty).Trim();
                entity.CustomerName = (_textCustomerName.Text ?? string.Empty).Trim();
                entity.SalesOrderNumber = (_textSalesOrderNumber.Text ?? string.Empty).Trim();
                entity.PlanStartTime = _dtPlanStart.Value;
                entity.PlanEndTime = _dtPlanEnd.Value;
                entity.Remarks = (_textRemarks.Text ?? string.Empty).Trim();

                if (original == null && string.IsNullOrEmpty(entity.Status))
                {
                    entity.Status = "待开始";
                }

                Result = entity;
                return true;
            }
        }
    }
}
