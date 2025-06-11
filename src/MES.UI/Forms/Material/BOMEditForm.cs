using System;
using System.Drawing;
using System.Windows.Forms;
using MES.BLL.Material.DTO;
using MES.Common.Logging;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// BOM编辑窗体 - 使用真实LOL主题
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public partial class BOMEditForm : Form
    {
        /// <summary>
        /// BOM数据传输对象
        /// </summary>
        public BOMDto BOMData { get; private set; }

        /// <summary>
        /// 是否为新增模式
        /// </summary>
        private readonly bool isNew;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bomDto">要编辑的BOM DTO。如果传入null，则进入新增模式。</param>
        public BOMEditForm(BOMDto bomDto)
        {
            InitializeComponent();

            if (bomDto == null)
            {
                // --- 新增模式 ---
                this.isNew = true;
                this.Text = "新增BOM";
                // 创建一个新的DTO对象，并设置默认值
                this.BOMData = new BOMDto { Status = true };
            }
            else
            {
                // --- 编辑模式 ---
                this.isNew = false;
                this.Text = "编辑BOM";
                // 使用传入DTO的副本进行编辑
                this.BOMData = bomDto.Clone();
            }

            // 在窗体加载时，将数据显示到界面控件上
            this.Load += (sender, e) =>
            {
                // 暂时禁用LOL主题以解决重影问题
                // ApplyRealLeagueTheme();
                FixTextRenderingIssues();
                LoadDataToControls();
            };
        }

        /// <summary>
        /// 修复文字渲染问题，彻底解决重影
        /// </summary>
        private void FixTextRenderingIssues()
        {
            try
            {
                // 强制设置窗体的文字渲染属性
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.ResizeRedraw, true);
                this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);

                // 递归修复所有Label控件的渲染问题
                FixControlTextRendering(this);
            }
            catch (Exception ex)
            {
                LogManager.Error("修复文字渲染问题失败", ex);
            }
        }

        /// <summary>
        /// 递归修复控件的文字渲染问题
        /// </summary>
        private void FixControlTextRendering(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Label)
                {
                    var label = control as Label;
                    // 强制设置Label的渲染属性
                    label.UseCompatibleTextRendering = false;
                    label.AutoSize = true;
                    label.BackColor = Color.White;
                    label.ForeColor = Color.Black;
                    label.Font = new Font("微软雅黑", 9F, FontStyle.Regular);

                    // 移除所有可能的Paint事件处理器
                    RemovePaintEvents(label);
                }
                else if (control is CheckBox)
                {
                    var checkBox = control as CheckBox;
                    checkBox.UseCompatibleTextRendering = false;
                    checkBox.BackColor = Color.White;
                    checkBox.ForeColor = Color.Black;
                    checkBox.Font = new Font("微软雅黑", 9F, FontStyle.Regular);

                    // 移除所有可能的Paint事件处理器
                    RemovePaintEvents(checkBox);
                }

                // 递归处理子控件
                if (control.HasChildren)
                {
                    FixControlTextRendering(control);
                }
            }
        }

        /// <summary>
        /// 移除控件的Paint事件处理器
        /// </summary>
        private void RemovePaintEvents(Control control)
        {
            try
            {
                // 通过反射移除Paint事件
                var eventField = typeof(Control).GetField("EVENT_PAINT",
                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                if (eventField != null)
                {
                    var eventKey = eventField.GetValue(null);
                    var eventsField = typeof(Control).GetField("events",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    if (eventsField != null)
                    {
                        var events = eventsField.GetValue(control);
                        if (events != null)
                        {
                            var removeMethod = events.GetType().GetMethod("RemoveHandler");
                            if (removeMethod != null)
                            {
                                removeMethod.Invoke(events, new object[] { eventKey, null });
                            }
                        }
                    }
                }
            }
            catch
            {
                // 忽略反射错误
            }
        }

        /// <summary>
        /// 应用真实LOL主题 - 基于真实LOL客户端设计
        /// 严格遵循C# 5.0语法规范
        /// </summary>
        private void ApplyRealLeagueTheme()
        {
            try
            {
                // 使用新的真实LOL主题应用器
                MES.UI.Framework.Themes.RealLeagueThemeApplier.ApplyRealLeagueTheme(this);
            }
            catch (Exception ex)
            {
                LogManager.Error("应用真实LOL主题失败", ex);
                MessageBox.Show(string.Format("应用真实LOL主题失败: {0}", ex.Message), "主题错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 将 BOMData DTO 中的数据显示到窗体控件上
        /// </summary>
        private void LoadDataToControls()
        {
            // --- 填充文本框 ---
            txtBOMCode.Text = BOMData.BOMCode ?? "";
            txtBomName.Text = BOMData.BomName ?? "";
            txtProductCode.Text = BOMData.ProductCode ?? "";
            txtProductName.Text = BOMData.ProductName ?? "";
            txtMaterialCode.Text = BOMData.MaterialCode ?? "";
            txtMaterialName.Text = BOMData.MaterialName ?? "";
            txtQuantity.Text = BOMData.Quantity.ToString("F2");
            txtUnit.Text = BOMData.Unit ?? "";
            txtLossRate.Text = BOMData.LossRate.ToString("F2");
            txtSubstituteMaterial.Text = BOMData.SubstituteMaterial ?? "";
            txtRemarks.Text = BOMData.Remarks ?? "";
            txtBOMVersion.Text = BOMData.BOMVersion ?? "";

            // --- 填充下拉框 ---
            cmbBOMType.Text = BOMData.BOMType ?? "PRODUCTION";

            // --- 填充日期控件 ---
            dtpEffectiveDate.Value = BOMData.EffectiveDate;
            if (BOMData.ExpireDate.HasValue)
            {
                dtpExpireDate.Value = BOMData.ExpireDate.Value;
                chkHasExpireDate.Checked = true;
            }
            else
            {
                chkHasExpireDate.Checked = false;
                dtpExpireDate.Enabled = false;
            }

            // --- 填充状态 ---
            chkStatus.Checked = BOMData.Status;

            // --- UI交互逻辑 ---
            // 新增时，BOM编码允许编辑；编辑时，为防止误改，设为只读
            txtBOMCode.ReadOnly = !isNew;
        }

        /// <summary>
        /// 从窗体控件收集数据到 BOMData DTO 中，并进行全面的数据验证
        /// </summary>
        /// <returns>如果所有数据都有效并成功收集，返回 true；否则返回 false</returns>
        private bool CollectAndValidateData()
        {
            // --- 1. 必填项验证 ---
            if (string.IsNullOrWhiteSpace(txtBOMCode.Text))
            {
                MessageBox.Show("BOM编码不能为空！", "输入验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBOMCode.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtBomName.Text))
            {
                MessageBox.Show("BOM名称不能为空！", "输入验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBomName.Focus();
                return false;
            }

            // --- 2. 收集字符串类型数据 ---
            BOMData.BOMCode = txtBOMCode.Text.Trim();
            BOMData.BomName = txtBomName.Text.Trim();
            BOMData.ProductCode = txtProductCode.Text.Trim();
            BOMData.ProductName = txtProductName.Text.Trim();
            BOMData.MaterialCode = txtMaterialCode.Text.Trim();
            BOMData.MaterialName = txtMaterialName.Text.Trim();
            BOMData.Unit = txtUnit.Text.Trim();
            BOMData.SubstituteMaterial = txtSubstituteMaterial.Text.Trim();
            BOMData.Remarks = txtRemarks.Text.Trim();
            BOMData.BOMVersion = txtBOMVersion.Text.Trim();
            BOMData.BOMType = cmbBOMType.Text.Trim();

            // --- 3. 安全地收集和验证数值类型 ---
            decimal tempDecimal;

            // 数量验证
            if (!decimal.TryParse(txtQuantity.Text, out tempDecimal) || tempDecimal <= 0)
            {
                MessageBox.Show("数量必须是大于0的数字！", "输入验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }
            BOMData.Quantity = tempDecimal;

            // 损耗率验证
            if (!decimal.TryParse(txtLossRate.Text, out tempDecimal) || tempDecimal < 0 || tempDecimal > 100)
            {
                MessageBox.Show("损耗率必须是0-100之间的数字！", "输入验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLossRate.Focus();
                return false;
            }
            BOMData.LossRate = tempDecimal;

            // --- 4. 收集日期和状态 ---
            BOMData.EffectiveDate = dtpEffectiveDate.Value;
            BOMData.ExpireDate = chkHasExpireDate.Checked ? (DateTime?)dtpExpireDate.Value : null;
            BOMData.Status = chkStatus.Checked;

            // --- 5. 业务逻辑验证 ---
            if (BOMData.ExpireDate.HasValue && BOMData.ExpireDate <= BOMData.EffectiveDate)
            {
                MessageBox.Show("失效日期必须晚于生效日期！", "业务规则验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpExpireDate.Focus();
                return false;
            }

            // 所有验证和数据收集均已通过
            return true;
        }

        /// <summary>
        /// "保存"按钮的点击事件处理程序
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // 调用统一的数据收集与验证方法
            if (CollectAndValidateData())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// "取消"按钮的点击事件处理程序
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 失效日期复选框状态改变事件
        /// </summary>
        private void chkHasExpireDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpExpireDate.Enabled = chkHasExpireDate.Checked;
        }
    }
}
