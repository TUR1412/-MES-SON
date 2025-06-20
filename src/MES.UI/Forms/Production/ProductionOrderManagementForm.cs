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

namespace MES.UI.Forms.Production
{
    /// <summary>
    /// 生产订单管理窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class ProductionOrderManagementForm : Form
    {
        private List<ProductionOrderInfo> orderList;
        private List<ProductionOrderInfo> filteredOrderList;
        private ProductionOrderInfo currentOrder;
        private readonly IProductionOrderBLL _productionOrderBLL;

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
                string searchTerm = textBoxSearch.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // 显示所有订单
                    filteredOrderList = new List<ProductionOrderInfo>(orderList);
                }
                else
                {
                    // 根据订单编号、产品名称、状态进行搜索
                    filteredOrderList = orderList.Where(o =>
                        o.OrderNo.ToLower().Contains(searchTerm) ||
                        o.ProductName.ToLower().Contains(searchTerm) ||
                        o.Status.ToLower().Contains(searchTerm) ||
                        o.WorkshopName.ToLower().Contains(searchTerm)
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
                    result.UpdateTime = DateTime.Now;

                    // 调用BLL层更新到数据库
                    bool updateResult = _productionOrderBLL.UpdateProductionOrder(result);

                    if (updateResult)
                    {
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
                    else
                    {
                        MessageBox.Show("更新生产订单失败，请检查数据库连接", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                    bool deleteResult = _productionOrderBLL.DeleteProductionOrder(currentOrder.Id);

                    if (deleteResult)
                    {
                        // 重新加载数据
                        LoadProductionOrderData();

                        // 刷新显示
                        RefreshDataGridView();

                        MessageBox.Show("生产订单删除成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("删除生产订单：{0} - {1}",
                            currentOrder.OrderNo, currentOrder.ProductName));
                    }
                    else
                    {
                        MessageBox.Show("删除生产订单失败，请检查数据库连接", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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