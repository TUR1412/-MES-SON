using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Models.Material;
using MES.Common.Logging;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// BOM编辑窗体 - 现代化UI设计
    /// 功能：新增和编辑BOM物料清单信息
    /// </summary>
    public partial class BOMEditForm : Form
    {
        #region 私有字段

        private BOMInfo _bomData;
        private bool _isEditMode;

        #endregion

        #region 公共属性

        /// <summary>
        /// BOM数据
        /// </summary>
        public BOMInfo BOMData
        {
            get { return _bomData; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 新增模式构造函数
        /// </summary>
        public BOMEditForm()
        {
            InitializeComponent();
            _isEditMode = false;
            _bomData = new BOMInfo();
            InitializeForm();
        }

        /// <summary>
        /// 编辑模式构造函数
        /// </summary>
        /// <param name="bom">要编辑的BOM信息</param>
        public BOMEditForm(BOMInfo bom)
        {
            InitializeComponent();
            _isEditMode = bom != null;
            _bomData = bom != null ? CloneBOM(bom) : new BOMInfo();
            InitializeForm();
            if (_isEditMode)
            {
                LoadBOMData();
            }
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // 设置窗体属性
                this.Text = _isEditMode ? "编辑BOM" : "新增BOM";
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.Size = new Size(600, 700);

                // 设置现代化样式
                this.BackColor = Color.FromArgb(240, 244, 248);
                this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);

                LogManager.Info(string.Format("BOM编辑窗体初始化完成，模式：{0}", _isEditMode ? "编辑" : "新增"));
            }
            catch (Exception ex)
            {
                LogManager.Error("BOM编辑窗体初始化失败", ex);
                MessageBox.Show(string.Format("窗体初始化失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载BOM数据到界面
        /// </summary>
        private void LoadBOMData()
        {
            try
            {
                if (_bomData == null) return;

                txtBOMCode.Text = _bomData.BOMCode;
                txtBOMVersion.Text = _bomData.BOMVersion;
                cmbBOMType.Text = _bomData.BOMType;
                txtProductCode.Text = _bomData.ProductCode;
                txtProductName.Text = _bomData.ProductName;
                txtMaterialCode.Text = _bomData.MaterialCode;
                txtMaterialName.Text = _bomData.MaterialName;
                txtQuantity.Text = _bomData.Quantity.ToString();
                txtUnit.Text = _bomData.Unit;
                txtLossRate.Text = _bomData.LossRate.ToString();
                txtSubstituteMaterial.Text = _bomData.SubstituteMaterial;
                dtpEffectiveDate.Value = _bomData.EffectiveDate;
                
                if (_bomData.ExpireDate.HasValue)
                {
                    chkHasExpireDate.Checked = true;
                    dtpExpireDate.Value = _bomData.ExpireDate.Value;
                    dtpExpireDate.Enabled = true;
                }
                else
                {
                    chkHasExpireDate.Checked = false;
                    dtpExpireDate.Enabled = false;
                }
                
                chkStatus.Checked = _bomData.Status;
                txtRemarks.Text = _bomData.Remarks;

                // 编辑模式下BOM编码不可修改
                if (_isEditMode)
                {
                    txtBOMCode.ReadOnly = true;
                    txtBOMCode.BackColor = Color.FromArgb(248, 249, 250);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("加载BOM数据失败", ex);
                MessageBox.Show(string.Format("加载BOM数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    SaveBOMData();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("保存BOM数据失败", ex);
                MessageBox.Show(string.Format("保存失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// 失效日期复选框变化事件
        /// </summary>
        private void chkHasExpireDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpExpireDate.Enabled = chkHasExpireDate.Checked;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 验证输入数据
        /// </summary>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtBOMCode.Text))
            {
                MessageBox.Show("BOM编码不能为空！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBOMCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProductCode.Text))
            {
                MessageBox.Show("产品编码不能为空！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMaterialCode.Text))
            {
                MessageBox.Show("物料编码不能为空！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterialCode.Focus();
                return false;
            }

            decimal quantity;
            if (!decimal.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("数量必须是大于0的数字！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            decimal lossRate;
            if (!decimal.TryParse(txtLossRate.Text, out lossRate) || lossRate < 0)
            {
                MessageBox.Show("损耗率必须是大于等于0的数字！", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLossRate.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 保存BOM数据
        /// </summary>
        private void SaveBOMData()
        {
            _bomData.BOMCode = txtBOMCode.Text.Trim();
            _bomData.BOMVersion = txtBOMVersion.Text.Trim();
            _bomData.BOMType = cmbBOMType.Text.Trim();
            _bomData.ProductCode = txtProductCode.Text.Trim();
            _bomData.ProductName = txtProductName.Text.Trim();
            _bomData.MaterialCode = txtMaterialCode.Text.Trim();
            _bomData.MaterialName = txtMaterialName.Text.Trim();
            _bomData.Quantity = decimal.Parse(txtQuantity.Text);
            _bomData.Unit = txtUnit.Text.Trim();
            _bomData.LossRate = decimal.Parse(txtLossRate.Text);
            _bomData.SubstituteMaterial = txtSubstituteMaterial.Text.Trim();
            _bomData.EffectiveDate = dtpEffectiveDate.Value;
            _bomData.ExpireDate = chkHasExpireDate.Checked ? (DateTime?)dtpExpireDate.Value : null;
            _bomData.Status = chkStatus.Checked;
            _bomData.Remarks = txtRemarks.Text.Trim();
            
            if (!_isEditMode)
            {
                _bomData.CreateTime = DateTime.Now;
            }
            _bomData.UpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 克隆BOM对象
        /// </summary>
        private BOMInfo CloneBOM(BOMInfo source)
        {
            return new BOMInfo
            {
                Id = source.Id,
                BOMCode = source.BOMCode,
                BOMVersion = source.BOMVersion,
                BOMType = source.BOMType,
                ProductId = source.ProductId,
                ProductCode = source.ProductCode,
                ProductName = source.ProductName,
                MaterialId = source.MaterialId,
                MaterialCode = source.MaterialCode,
                MaterialName = source.MaterialName,
                Quantity = source.Quantity,
                Unit = source.Unit,
                LossRate = source.LossRate,
                SubstituteMaterial = source.SubstituteMaterial,
                EffectiveDate = source.EffectiveDate,
                ExpireDate = source.ExpireDate,
                Status = source.Status,
                Remarks = source.Remarks,
                CreateTime = source.CreateTime,
                UpdateTime = source.UpdateTime
            };
        }

        #endregion
    }
}
