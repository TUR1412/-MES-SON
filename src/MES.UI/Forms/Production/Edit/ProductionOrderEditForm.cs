using System;
using System.Windows.Forms;
using MES.Models.Production;
using MES.Common.Logging;

namespace MES.UI.Forms.Production.Edit
{
    /// <summary>
    /// 生产订单编辑窗体
    /// </summary>
    public partial class ProductionOrderEditForm : Form
    {
        /// <summary>
        /// 订单数据属性，用于在窗体间传递数据
        /// </summary>
        public ProductionOrderInfo OrderData { get; private set; }

        /// <summary>
        /// 是否为新增模式
        /// </summary>
        private readonly bool isNew;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="order">要编辑的订单，null表示新增</param>
        public ProductionOrderEditForm(ProductionOrderInfo order)
        {
            InitializeComponent();

            if (order == null)
            {
                // 新增模式
                this.isNew = true;
                this.Text = "新增生产订单";
                this.OrderData = new ProductionOrderInfo
                {
                    OrderNo = GenerateOrderNumber(),
                    Status = "待开始",
                    Priority = "普通",
                    PlanStartTime = DateTime.Now.AddDays(1),
                    PlanEndTime = DateTime.Now.AddDays(7),
                    ActualQuantity = 0
                };
            }
            else
            {
                // 编辑模式
                this.isNew = false;
                this.Text = "编辑生产订单";
                this.OrderData = order.Clone();
            }

            this.Load += (sender, e) => LoadDataToControls();
        }

        /// <summary>
        /// 生成订单编号
        /// </summary>
        private string GenerateOrderNumber()
        {
            return "PO" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 将数据加载到控件
        /// </summary>
        private void LoadDataToControls()
        {
            try
            {
                txtOrderNo.Text = OrderData.OrderNo;
                txtProductCode.Text = OrderData.ProductCode;
                txtProductName.Text = OrderData.ProductName;
                txtQuantity.Text = OrderData.Quantity.ToString();
                txtActualQuantity.Text = OrderData.ActualQuantity.ToString();
                txtUnit.Text = OrderData.Unit;
                cmbStatus.Text = OrderData.Status;
                cmbPriority.Text = OrderData.Priority;
                txtWorkshopName.Text = OrderData.WorkshopName;
                txtResponsiblePerson.Text = OrderData.ResponsiblePerson;
                txtCustomerName.Text = OrderData.CustomerName;
                txtSalesOrderNumber.Text = OrderData.SalesOrderNumber;
                dtpPlanStartTime.Value = OrderData.PlanStartTime;
                dtpPlanEndTime.Value = OrderData.PlanEndTime;
                txtRemarks.Text = OrderData.Remarks;

                // 新增时订单号不可编辑，编辑时为只读
                txtOrderNo.ReadOnly = true;
                // 实际数量通常由生产过程更新，此处设为只读
                txtActualQuantity.ReadOnly = true;

                // 根据新增/编辑模式调整界面
                if (isNew)
                {
                    // 新增模式下，某些字段可能需要特殊处理
                    txtActualQuantity.Text = "0";
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("加载订单数据到控件失败", ex);
                MessageBox.Show("加载数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 收集并验证数据
        /// </summary>
        private bool CollectAndValidateData()
        {
            try
            {
                // 必填项验证
                if (string.IsNullOrWhiteSpace(txtProductCode.Text))
                {
                    MessageBox.Show("产品编码不能为空！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProductCode.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtProductName.Text))
                {
                    MessageBox.Show("产品名称不能为空！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProductName.Focus();
                    return false;
                }

                decimal quantity;
                if (!decimal.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
                {
                    MessageBox.Show("计划数量必须是大于0的数字！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return false;
                }

                // 收集数据
                OrderData.ProductCode = txtProductCode.Text.Trim();
                OrderData.ProductName = txtProductName.Text.Trim();
                OrderData.Quantity = quantity;
                OrderData.Unit = txtUnit.Text.Trim();
                OrderData.Status = cmbStatus.Text;
                OrderData.Priority = cmbPriority.Text;
                OrderData.WorkshopName = txtWorkshopName.Text.Trim();
                OrderData.ResponsiblePerson = txtResponsiblePerson.Text.Trim();
                OrderData.CustomerName = txtCustomerName.Text.Trim();
                OrderData.SalesOrderNumber = txtSalesOrderNumber.Text.Trim();
                OrderData.PlanStartTime = dtpPlanStartTime.Value;
                OrderData.PlanEndTime = dtpPlanEndTime.Value;
                OrderData.Remarks = txtRemarks.Text.Trim();

                // 业务逻辑验证
                if (OrderData.PlanStartTime >= OrderData.PlanEndTime)
                {
                    MessageBox.Show("计划开始时间必须早于计划结束时间！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpPlanStartTime.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Error("收集订单数据失败", ex);
                MessageBox.Show("数据验证失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CollectAndValidateData())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}