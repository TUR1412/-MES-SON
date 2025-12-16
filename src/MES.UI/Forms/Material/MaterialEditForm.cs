// MaterialEditForm.cs (最终完善版)

using MES.BLL.Material.DTO;
using System;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 物料信息编辑与新增窗体。
    /// </summary>
    public partial class MaterialEditForm : ThemedForm
    {
        /// <summary>
        /// 公开属性，用于在窗体间传递物料数据。
        /// 主窗体通过此属性获取用户编辑或新增的结果。
        /// </summary>
        public MaterialDto MaterialData { get; private set; }

        /// <summary>
        /// 内部标志，用于区分当前是新增模式还是编辑模式。
        /// </summary>
        private readonly bool isNew;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="materialDto">要编辑的物料DTO。如果传入null，则进入新增模式。</param>
        public MaterialEditForm(MaterialDto materialDto)
        {
            InitializeComponent();

            if (materialDto == null)
            {
                // --- 新增模式 ---
                this.isNew = true;
                this.Text = "新增物料";
                // 创建一个新的DTO对象，并设置默认值
                this.MaterialData = new MaterialDto { Status = true };
            }
            else
            {
                // --- 编辑模式 ---
                this.isNew = false;
                this.Text = "编辑物料";
                // 使用传入DTO的副本进行编辑，这样即使用户点击“取消”，主窗体的数据也不会被更改
                this.MaterialData = materialDto.Clone();
            }

            // 在窗体加载时，将数据显示到界面控件上
            this.Load += (sender, e) => LoadDataToControls();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 将 MaterialData DTO 中的数据显示到窗体控件上。
        /// </summary>
        private void LoadDataToControls()
        {
            // --- 填充文本框 ---
            txtMaterialCode.Text = MaterialData.MaterialCode;
            txtMaterialName.Text = MaterialData.MaterialName;
            txtMaterialType.Text = MaterialData.MaterialType; // ComboBox可以直接设置Text
            txtSpecification.Text = MaterialData.Specification;
            txtUnit.Text = MaterialData.Unit;
            txtCategory.Text = MaterialData.Category;
            txtSupplier.Text = MaterialData.Supplier;

            // --- 填充可空的数值类型，如果值为null，则显示空字符串 ---
            txtStandardCost.Text = MaterialData.StandardCost.ToString();
            txtSafetyStock.Text = MaterialData.SafetyStock.ToString();
            txtMinStock.Text = MaterialData.MinStock.ToString();
            txtMaxStock.Text = MaterialData.MaxStock.ToString();
            txtStockQuantity.Text = MaterialData.StockQuantity.ToString();
            txtLeadTime.Text = MaterialData.LeadTime.ToString();

            // --- 填充布尔和普通数值类型 ---
            txtStatus.Text = MaterialData.Status.ToString(); // 显示 "True" 或 "False"
            txtPrice.Text = MaterialData.Price.ToString("F2"); // 格式化为两位小数

            // --- UI交互逻辑 ---
            // 新增时，物料编码允许编辑；编辑时，为防止误改，设为只读
            txtMaterialCode.ReadOnly = !isNew;
            // 库存数量通常由入库、出库等业务流程自动更新，不应在此手动编辑
            txtStockQuantity.ReadOnly = true;
        }

        /// <summary>
        /// 从窗体控件收集数据到 MaterialData DTO 中，并进行全面的数据验证。
        /// </summary>
        /// <returns>如果所有数据都有效并成功收集，返回 true；否则返回 false。</returns>
        private bool CollectAndValidateData()
        {
            // --- 1. 必填项验证 ---
            if (string.IsNullOrWhiteSpace(txtMaterialCode.Text))
            {
                MessageBox.Show("物料编码不能为空！", "输入验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterialCode.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtMaterialName.Text))
            {
                MessageBox.Show("物料名称不能为空！", "输入验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterialName.Focus();
                return false;
            }

            // --- 2. 收集字符串类型数据 ---
            MaterialData.MaterialCode = txtMaterialCode.Text.Trim();
            MaterialData.MaterialName = txtMaterialName.Text.Trim();
            MaterialData.MaterialType = txtMaterialType.Text.Trim();
            MaterialData.Specification = txtSpecification.Text.Trim();
            MaterialData.Unit = txtUnit.Text.Trim();
            MaterialData.Category = txtCategory.Text.Trim();
            MaterialData.Supplier = txtSupplier.Text.Trim();

            // --- 3. 安全地收集和验证数值类型 ---
            decimal tempDecimal;
            int tempInt;

            // 参考价格（假设为必填项）
            if (!decimal.TryParse(txtPrice.Text, out tempDecimal))
            {
                MessageBox.Show("参考价格必须是有效的数字！", "输入验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }
            MaterialData.Price = tempDecimal;

            // 标准成本（可空）
            MaterialData.StandardCost = decimal.TryParse(txtStandardCost.Text, out tempDecimal) ? tempDecimal : (decimal?)null;
            // 安全库存（可空）
            MaterialData.SafetyStock = decimal.TryParse(txtSafetyStock.Text, out tempDecimal) ? tempDecimal : (decimal?)null;
            // 最小库存（可空）
            MaterialData.MinStock = decimal.TryParse(txtMinStock.Text, out tempDecimal) ? tempDecimal : (decimal?)null;
            // 最大库存（可空）
            MaterialData.MaxStock = decimal.TryParse(txtMaxStock.Text, out tempDecimal) ? tempDecimal : (decimal?)null;
            // 采购提前期（可空）
            MaterialData.LeadTime = int.TryParse(txtLeadTime.Text, out tempInt) ? tempInt : (int?)null;

            // --- 4. 收集布尔类型 ---
            bool status;
            MaterialData.Status = bool.TryParse(txtStatus.Text, out status) && status;

            // --- 5. 业务逻辑验证 ---
            if (MaterialData.MinStock.HasValue && MaterialData.MaxStock.HasValue && MaterialData.MinStock > MaterialData.MaxStock)
            {
                MessageBox.Show("逻辑错误：最小库存不能大于最大库存！", "业务规则验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMinStock.Focus();
                return false;
            }

            // --- 6. 收集备注 ---
            // 假设您已将最后一个TextBox命名为txtRemark
            // MaterialData.Remark = txtRemark.Text.Trim();

            // 所有验证和数据收集均已通过
            return true;
        }

        /// <summary>
        /// “保存”按钮的点击事件处理程序。
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 调用统一的数据收集与验证方法
            if (CollectAndValidateData())
            {
                // 如果数据有效，则设置对话框结果为 OK。
                // 主窗体的 form.ShowDialog() 方法将收到此结果，并继续执行保存逻辑。
                this.DialogResult = DialogResult.OK;
                // 关闭当前编辑窗体
                this.Close();
            }
            // 如果 CollectAndValidateData() 返回 false，说明验证失败，
            // 相应的提示信息已在该方法内部弹出，此时不关闭窗体，等待用户修正输入。
        }

        /// <summary>
        /// “取消”按钮的点击事件处理程序。
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 设置对话框结果为 Cancel。
            // 主窗体的 form.ShowDialog() 方法将收到此结果，并放弃保存操作。
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
