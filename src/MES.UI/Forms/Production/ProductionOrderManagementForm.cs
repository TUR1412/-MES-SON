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

        public ProductionOrderManagementForm()
        {
            InitializeComponent();
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

                // 加载示例数据
                LoadSampleData();

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
        /// 加载示例数据
        /// </summary>
        private void LoadSampleData()
        {
            orderList.Clear();

            // 添加示例生产订单数据
            orderList.Add(new ProductionOrderInfo
            {
                Id = 1,
                OrderNo = "PO202506080001",
                ProductCode = "P001",
                ProductName = "钢制支架",
                Quantity = 100,
                ActualQuantity = 85,
                Unit = "个",
                Status = "进行中",
                Priority = "重要",
                WorkshopId = 1,
                WorkshopName = "机械加工车间",
                ResponsiblePerson = "张工",
                CustomerName = "ABC制造公司",
                SalesOrderNumber = "SO202506080001",
                PlanStartTime = DateTime.Now.AddDays(-5),
                PlanEndTime = DateTime.Now.AddDays(2),
                ActualStartTime = DateTime.Now.AddDays(-5),
                Remarks = "优质钢材制作，要求精度高"
            });

            orderList.Add(new ProductionOrderInfo
            {
                Id = 2,
                OrderNo = "PO202506080002",
                ProductCode = "P002",
                ProductName = "铝合金外壳",
                Quantity = 200,
                ActualQuantity = 0,
                Unit = "个",
                Status = "待开始",
                Priority = "普通",
                WorkshopId = 2,
                WorkshopName = "冲压车间",
                ResponsiblePerson = "李师傅",
                CustomerName = "XYZ电子公司",
                SalesOrderNumber = "SO202506080002",
                PlanStartTime = DateTime.Now.AddDays(1),
                PlanEndTime = DateTime.Now.AddDays(7),
                Remarks = "表面需要阳极氧化处理"
            });

            orderList.Add(new ProductionOrderInfo
            {
                Id = 3,
                OrderNo = "PO202506080003",
                ProductCode = "P003",
                ProductName = "精密齿轮",
                Quantity = 50,
                ActualQuantity = 50,
                Unit = "个",
                Status = "已完成",
                Priority = "紧急",
                WorkshopId = 1,
                WorkshopName = "机械加工车间",
                ResponsiblePerson = "王工",
                CustomerName = "DEF机械公司",
                SalesOrderNumber = "SO202506080003",
                PlanStartTime = DateTime.Now.AddDays(-10),
                PlanEndTime = DateTime.Now.AddDays(-3),
                ActualStartTime = DateTime.Now.AddDays(-10),
                ActualEndTime = DateTime.Now.AddDays(-3),
                Remarks = "高精度要求，已按时完成"
            });

            orderList.Add(new ProductionOrderInfo
            {
                Id = 4,
                OrderNo = "PO202506080004",
                ProductCode = "P004",
                ProductName = "电机外壳",
                Quantity = 150,
                ActualQuantity = 0,
                Unit = "个",
                Status = "已暂停",
                Priority = "普通",
                WorkshopId = 3,
                WorkshopName = "装配车间",
                ResponsiblePerson = "赵主管",
                CustomerName = "GHI电机公司",
                SalesOrderNumber = "SO202506080004",
                PlanStartTime = DateTime.Now.AddDays(-2),
                PlanEndTime = DateTime.Now.AddDays(5),
                Remarks = "等待原材料到货，暂停生产"
            });

            orderList.Add(new ProductionOrderInfo
            {
                Id = 5,
                OrderNo = "PO202506080005",
                ProductCode = "P005",
                ProductName = "不锈钢管件",
                Quantity = 300,
                ActualQuantity = 120,
                Unit = "个",
                Status = "进行中",
                Priority = "重要",
                WorkshopId = 2,
                WorkshopName = "冲压车间",
                ResponsiblePerson = "孙师傅",
                CustomerName = "JKL管道公司",
                SalesOrderNumber = "SO202506080005",
                PlanStartTime = DateTime.Now.AddDays(-3),
                PlanEndTime = DateTime.Now.AddDays(4),
                ActualStartTime = DateTime.Now.AddDays(-3),
                Remarks = "304不锈钢材质，防腐要求高"
            });

            // 复制到过滤列表
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
                    // 生成新ID
                    result.Id = orderList.Count > 0 ? orderList.Max(o => o.Id) + 1 : 1;
                    result.CreateTime = DateTime.Now;
                    result.UpdateTime = DateTime.Now;

                    // 添加到列表
                    orderList.Add(result);

                    // 刷新显示
                    RefreshDataGridView();

                    // 选中新添加的订单
                    SelectOrderById(result.Id);

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
                    // 更新订单信息
                    var originalOrder = orderList.FirstOrDefault(o => o.Id == currentOrder.Id);
                    if (originalOrder != null)
                    {
                        originalOrder.OrderNo = result.OrderNo;
                        originalOrder.ProductCode = result.ProductCode;
                        originalOrder.ProductName = result.ProductName;
                        originalOrder.Quantity = result.Quantity;
                        originalOrder.ActualQuantity = result.ActualQuantity;
                        originalOrder.Unit = result.Unit;
                        originalOrder.Status = result.Status;
                        originalOrder.Priority = result.Priority;
                        originalOrder.WorkshopName = result.WorkshopName;
                        originalOrder.ResponsiblePerson = result.ResponsiblePerson;
                        originalOrder.PlanStartTime = result.PlanStartTime;
                        originalOrder.PlanEndTime = result.PlanEndTime;
                        originalOrder.ActualStartTime = result.ActualStartTime;
                        originalOrder.ActualEndTime = result.ActualEndTime;
                        originalOrder.CustomerName = result.CustomerName;
                        originalOrder.SalesOrderNumber = result.SalesOrderNumber;
                        originalOrder.Remarks = result.Remarks;
                        originalOrder.UpdateTime = DateTime.Now;

                        // 刷新显示
                        RefreshDataGridView();

                        // 重新选中编辑的订单
                        SelectOrderById(originalOrder.Id);

                        MessageBox.Show("生产订单编辑成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("编辑生产订单：{0} - {1}",
                            originalOrder.OrderNo, originalOrder.ProductName));
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
                    // 从列表中移除
                    orderList.RemoveAll(o => o.Id == currentOrder.Id);

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
                LoadSampleData();
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
            // 这里应该打开一个订单编辑对话框
            // 为了演示，我们使用简单的输入框
            string title = order == null ? "新增生产订单" : "编辑生产订单";

            // 简化的编辑逻辑，实际应该使用专门的编辑窗体
            var editOrder = order != null ? order.Clone() : new ProductionOrderInfo();

            // 这里可以实现一个简单的编辑对话框
            // 或者调用专门的ProductionOrderEditForm

            // 暂时返回示例数据用于演示
            if (order == null)
            {
                // 新增订单的示例
                return new ProductionOrderInfo
                {
                    OrderNo = "PO" + DateTime.Now.ToString("yyyyMMddHHmm"),
                    ProductCode = "P" + (orderList.Count + 1).ToString("000"),
                    ProductName = "新产品",
                    Quantity = 100,
                    ActualQuantity = 0,
                    Unit = "个",
                    Status = "待开始",
                    Priority = "普通",
                    WorkshopName = "待分配",
                    ResponsiblePerson = "待分配",
                    CustomerName = "新客户",
                    SalesOrderNumber = "SO" + DateTime.Now.ToString("yyyyMMddHHmm"),
                    PlanStartTime = DateTime.Now.AddDays(1),
                    PlanEndTime = DateTime.Now.AddDays(7),
                    Remarks = "新增订单"
                };
            }
            else
            {
                // 编辑现有订单
                editOrder.ProductName = editOrder.ProductName + " (已编辑)";
                return editOrder;
            }
        }
    }
}