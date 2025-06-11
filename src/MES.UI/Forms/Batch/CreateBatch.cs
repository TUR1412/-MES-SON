using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MES.BLL.WorkOrder;
using MES.BLL.Workshop;
using MES.Common.Logging;
using MES.DAL.Core;
using MES.Models.Workshop;

namespace MES.UI.Forms.Batch
{
    public partial class CreateBatch : Form
    {
        private WorkOrderBLL workOrderBLL;
        private BatchBLL batchBLL;
        private DataTable workOrderTable;
        private string selectedWorkOrderNo;

        public CreateBatch()
        {
            InitializeComponent();
            InitializeBusinessLogic();
            LogManager.Info("创建批次窗体初始化完成");
        }

        private void InitializeBusinessLogic()
        {
            try
            {
                workOrderBLL = new WorkOrderBLL();
                batchBLL = new BatchBLL();
                workOrderTable = new DataTable();

                // 初始化批次号
                Batch_id_text.Text = batchBLL.GenerateNewBatchNumber();
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化业务逻辑失败", ex);
                MessageBox.Show(string.Format("初始化业务逻辑失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateBatch_Load(object sender, EventArgs e)
        {
            try
            {
                // 绑定工单号下拉框
                BindWorkOrderComboBox();

                // 绑定状态下拉框
                BindStatusComboBox();

                // 设置默认计划完成时间为明天
                dateTimePicker.Value = DateTime.Now.AddDays(1);
            }
            catch (Exception ex)
            {
                LogManager.Error("窗体加载时发生错误", ex);
                MessageBox.Show("初始化数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindWorkOrderComboBox()
        {
            try
            {
                // 获取所有生产订单数据
                string sql = "SELECT id, order_no FROM production_order_info WHERE is_deleted = 0";
                workOrderTable = DatabaseHelper.ExecuteQuery(sql);

                workorder_id_box.DataSource = workOrderTable;
                workorder_id_box.DisplayMember = "order_no";
                workorder_id_box.ValueMember = "id";
            }
            catch (Exception ex)
            {
                LogManager.Error("绑定工单号下拉框失败", ex);
                throw;
            }
        }

        private void BindStatusComboBox()
        {
            try
            {
                // 添加批次状态选项
                status_box.Items.Add("待开始");
                status_box.Items.Add("进行中");
                status_box.Items.Add("已暂停");
                status_box.Items.Add("已完成");
                status_box.Items.Add("已取消");

                // 默认选择"待开始"
                status_box.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogManager.Error("绑定状态下拉框失败", ex);
                throw;
            }
        }

        private string GenerateBatchNo()
        {
            return batchBLL.GenerateNewBatchNumber();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(Batch_id_text.Text))
            {
                MessageBox.Show("批次号不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(Production_id_text.Text))
            {
                MessageBox.Show("产品名称不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (workorder_id_box.SelectedIndex < 0)
            {
                MessageBox.Show("请选择工单号", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            decimal batchQuantity;
            if (!decimal.TryParse(Batchnum_text.Text, out batchQuantity) || batchQuantity <= 0)
            {
                MessageBox.Show("批次数量必须为大于0的数字", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (status_box.SelectedIndex < 0)
            {
                MessageBox.Show("请选择批次状态", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(user_text.Text))
            {
                MessageBox.Show("负责人不能为空", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool CreateBatchRecord()
        {
            try
            {
                // 创建批次信息对象
                var batchInfo = new BatchInfo
                {
                    BatchNumber = Batch_id_text.Text,
                    ProductName = Production_id_text.Text,
                    ProductionOrderId = Convert.ToInt32(workorder_id_box.SelectedValue),
                    ProductionOrderNumber = workorder_id_box.Text,
                    PlannedQuantity = Convert.ToDecimal(Batchnum_text.Text),
                    BatchStatus = status_box.SelectedItem.ToString(),
                    ProductionEndTime = dateTimePicker.Value,
                    OperatorName = user_text.Text,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    IsDeleted = false,
                    Version = 1
                };

                // 调用BLL层添加批次
                bool result = batchBLL.AddBatch(batchInfo);

                if (result)
                {
                    LogManager.Info(string.Format("成功创建批次: {0}", batchInfo.BatchNumber));
                    return true;
                }
                else
                {
                    LogManager.Error(string.Format("创建批次失败: {0}", batchInfo.BatchNumber));
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("创建批次记录时发生错误", ex);
                MessageBox.Show("创建批次记录时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void TestDatabaseInsert()
        {
            try
            {
                string testSql = "INSERT INTO batch_info (batch_number, product_name, production_order_id, planned_quantity, batch_status, create_time, is_deleted, version) " +
                                "VALUES (@batchNumber, @productName, @orderId, @quantity, @status, @createTime, @isDeleted, @version)";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@batchNumber", "TEST123"),
                    DatabaseHelper.CreateParameter("@productName", "测试产品"),
                    DatabaseHelper.CreateParameter("@orderId", 1),
                    DatabaseHelper.CreateParameter("@quantity", 100),
                    DatabaseHelper.CreateParameter("@status", "待开始"),
                    DatabaseHelper.CreateParameter("@createTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@isDeleted", false),
                    DatabaseHelper.CreateParameter("@version", 1)
                };

                int result = DatabaseHelper.ExecuteNonQuery(testSql, parameters);
                MessageBox.Show("测试插入结果: " + (result > 0 ? "成功" : "失败"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("测试插入失败: " + ex.Message);
            }
        }

        private void Create_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (!ValidateInput())
                {
                    return;
                }

                // 创建批次记录
                var batchInfo = new BatchInfo
                {
                    BatchNumber = Batch_id_text.Text,
                    ProductName = Production_id_text.Text,
                    ProductionOrderId = Convert.ToInt32(workorder_id_box.SelectedValue),
                    ProductionOrderNumber = workorder_id_box.Text,
                    PlannedQuantity = Convert.ToDecimal(Batchnum_text.Text),
                    BatchStatus = status_box.SelectedItem.ToString(),
                    ProductionEndTime = dateTimePicker.Value,
                    OperatorName = user_text.Text,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    IsDeleted = false,
                    Version = 1,
                    Unit = "个", // 添加默认单位
                    WorkshopId = 1, // 添加默认车间ID
                    WorkshopName = "默认车间", // 添加默认车间名称
                    ActualQuantity = 0 // 添加实际数量默认值
                };

                // 调用BLL层添加批次
                bool result = batchBLL.AddBatch(batchInfo);

                if (result)
                {
                    MessageBox.Show("批次创建成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("批次创建失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("创建批次时发生错误", ex);
                MessageBox.Show("创建批次时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}