using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Models.Workshop;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Workshop
{
    /// <summary>
    /// 在制品状态更新对话框
    /// </summary>
    public partial class WIPStatusUpdateDialog : ThemedForm
    {
        private WIPInfo _wipInfo;
        private ComboBox cmbNewStatus;
        private TextBox txtCompletedQuantity;
        private TextBox txtRemarks;
        private Button btnOK;
        private Button btnCancel;
        private Label lblWIPInfo;
        private Label lblNewStatus;
        private Label lblCompletedQuantity;
        private Label lblRemarks;

        /// <summary>
        /// 新状态
        /// </summary>
        public int NewStatus { get; private set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public int CompletedQuantity { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        public WIPStatusUpdateDialog(WIPInfo wipInfo)
        {
            _wipInfo = wipInfo;
            InitializeComponent();
            LoadData();
            UIThemeManager.ApplyTheme(this);
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "更新在制品状态";
            this.Size = new Size(450, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 在制品信息标签
            lblWIPInfo = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(400, 40),
                Font = new Font("微软雅黑", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(51, 51, 51)
            };
            this.Controls.Add(lblWIPInfo);

            // 新状态标签
            lblNewStatus = new Label
            {
                Text = "新状态：",
                Location = new Point(20, 80),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblNewStatus);

            // 新状态下拉框
            cmbNewStatus = new ComboBox
            {
                Location = new Point(110, 80),
                Size = new Size(150, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbNewStatus);

            // 已完成数量标签
            lblCompletedQuantity = new Label
            {
                Text = "已完成数量：",
                Location = new Point(20, 120),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblCompletedQuantity);

            // 已完成数量文本框
            txtCompletedQuantity = new TextBox
            {
                Location = new Point(110, 120),
                Size = new Size(150, 23)
            };
            this.Controls.Add(txtCompletedQuantity);

            // 备注标签
            lblRemarks = new Label
            {
                Text = "备注：",
                Location = new Point(20, 160),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblRemarks);

            // 备注文本框
            txtRemarks = new TextBox
            {
                Location = new Point(110, 160),
                Size = new Size(300, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtRemarks);

            // 确定按钮
            btnOK = new Button
            {
                Text = "确定",
                Location = new Point(250, 235),
                Size = new Size(75, 25),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);

            // 取消按钮
            btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(335, 235),
                Size = new Size(75, 25),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            if (_wipInfo == null) return;

            // 显示在制品信息
            lblWIPInfo.Text = string.Format("在制品：{0} | 产品：{1} | 当前状态：{2}",
                _wipInfo.WIPId, _wipInfo.ProductName, _wipInfo.StatusText);

            // 初始化状态下拉框
            cmbNewStatus.Items.Clear();
            cmbNewStatus.Items.Add(new { Text = "待开始", Value = 0 });
            cmbNewStatus.Items.Add(new { Text = "生产中", Value = 1 });
            cmbNewStatus.Items.Add(new { Text = "质检中", Value = 2 });
            cmbNewStatus.Items.Add(new { Text = "暂停", Value = 3 });
            cmbNewStatus.Items.Add(new { Text = "已完成", Value = 4 });
            cmbNewStatus.DisplayMember = "Text";
            cmbNewStatus.ValueMember = "Value";

            // 设置当前状态为默认选择
            for (int i = 0; i < cmbNewStatus.Items.Count; i++)
            {
                var item = (dynamic)cmbNewStatus.Items[i];
                if (item.Value == _wipInfo.Status)
                {
                    cmbNewStatus.SelectedIndex = i;
                    break;
                }
            }

            // 设置已完成数量
            txtCompletedQuantity.Text = _wipInfo.CompletedQuantity.ToString();

            // 状态变化事件
            cmbNewStatus.SelectedIndexChanged += CmbNewStatus_SelectedIndexChanged;
        }

        /// <summary>
        /// 状态选择变化事件
        /// </summary>
        private void CmbNewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNewStatus.SelectedItem != null)
            {
                var selectedStatus = ((dynamic)cmbNewStatus.SelectedItem).Value;
                
                // 如果选择已完成，自动设置完成数量为总数量
                if (selectedStatus == 4)
                {
                    txtCompletedQuantity.Text = _wipInfo.Quantity.ToString();
                }
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (cmbNewStatus.SelectedItem == null)
                {
                    MessageBox.Show("请选择新状态", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbNewStatus.Focus();
                    return;
                }

                int completedQty;
                if (!int.TryParse(txtCompletedQuantity.Text, out completedQty) || completedQty < 0)
                {
                    MessageBox.Show("请输入有效的已完成数量", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCompletedQuantity.Focus();
                    return;
                }

                if (completedQty > _wipInfo.Quantity)
                {
                    MessageBox.Show(string.Format("已完成数量不能超过总数量 {0}", _wipInfo.Quantity), 
                        "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCompletedQuantity.Focus();
                    return;
                }

                // 获取输入值
                NewStatus = ((dynamic)cmbNewStatus.SelectedItem).Value;
                CompletedQuantity = completedQty;
                Remarks = txtRemarks.Text.Trim();

                // 验证状态逻辑
                if (NewStatus == 4 && CompletedQuantity < _wipInfo.Quantity)
                {
                    var result = MessageBox.Show(
                        string.Format("状态设为已完成但完成数量({0})小于总数量({1})，是否继续？", 
                            CompletedQuantity, _wipInfo.Quantity),
                        "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("操作失败：{0}", ex.Message), "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
