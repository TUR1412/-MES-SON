using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Models.Production;
using MES.Common.Logging;
using MES.UI.Framework.Themes;
using MES.UI.Framework.Controls;

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

            this.Load += (sender, e) =>
            {
                ApplyDeepLeagueTheme();
                LoadDataToControls();
            };
        }

        /// <summary>
        /// 应用真实LOL主题 - 基于真实LOL客户端设计
        /// 严格遵循C# 5.0语法规范
        /// </summary>
        private void ApplyDeepLeagueTheme()
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
        /// 应用LOL样式到面板
        /// </summary>
        private void ApplyLeagueStyleToPanel(Panel panel)
        {
            panel.BackColor = LeagueColors.DarkSurface;
        }

        /// <summary>
        /// 应用LOL样式到标签
        /// </summary>
        private void ApplyLeagueStyleToLabel(Label label)
        {
            label.ForeColor = LeagueColors.TextPrimary;
            label.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 应用LOL样式到文本框
        /// </summary>
        private void ApplyLeagueStyleToTextBox(TextBox textBox)
        {
            textBox.BackColor = LeagueColors.InputBackground;
            textBox.ForeColor = LeagueColors.TextPrimary;
            textBox.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            textBox.BorderStyle = BorderStyle.None;
            textBox.Paint += TextBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到下拉框
        /// </summary>
        private void ApplyLeagueStyleToComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = LeagueColors.InputBackground;
            comboBox.ForeColor = LeagueColors.TextPrimary;
            comboBox.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Paint += ComboBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到按钮
        /// </summary>
        private void ApplyLeagueStyleToButton(Button button)
        {
            button.BackColor = Color.Transparent;
            button.Font = new Font("微软雅黑", 11F, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            // 根据按钮名称应用不同样式
            if (button.Name == "btnSave" || button.Text.Contains("保存") || button.Text.Contains("确定"))
            {
                button.ForeColor = LeagueColors.TextWhite;
                button.Paint += BtnSave_LeaguePaint;
            }
            else
            {
                button.ForeColor = LeagueColors.TextPrimary;
                button.Paint += BtnCancel_LeaguePaint;
            }
        }

        /// <summary>
        /// 应用LOL样式到日期选择器
        /// </summary>
        private void ApplyLeagueStyleToDateTimePicker(DateTimePicker dateTimePicker)
        {
            dateTimePicker.BackColor = LeagueColors.InputBackground;
            dateTimePicker.ForeColor = LeagueColors.TextPrimary;
            dateTimePicker.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            dateTimePicker.Paint += DateTimePicker_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到复选框
        /// </summary>
        private void ApplyLeagueStyleToCheckBox(CheckBox checkBox)
        {
            checkBox.ForeColor = LeagueColors.TextPrimary;
            checkBox.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            checkBox.BackColor = Color.Transparent;
            checkBox.Paint += CheckBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到分组框
        /// </summary>
        private void ApplyLeagueStyleToGroupBox(GroupBox groupBox)
        {
            groupBox.ForeColor = LeagueColors.AccentBlue;
            groupBox.Font = new Font("微软雅黑", 11F, FontStyle.Bold);
            groupBox.BackColor = Color.Transparent;
            groupBox.Paint += GroupBox_LeaguePaint;
        }

        /// <summary>
        /// 应用LOL样式到数字输入框
        /// </summary>
        private void ApplyLeagueStyleToNumericUpDown(NumericUpDown numericUpDown)
        {
            numericUpDown.BackColor = LeagueColors.InputBackground;
            numericUpDown.ForeColor = LeagueColors.TextPrimary;
            numericUpDown.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            numericUpDown.BorderStyle = BorderStyle.None;
            numericUpDown.Paint += NumericUpDown_LeaguePaint;
        }

        /// <summary>
        /// 启用基础视觉效果
        /// </summary>
        private void EnableBasicVisualEffects()
        {
            // 设置窗体双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);
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



        /// <summary>
        /// 面板深度绘制事件
        /// </summary>
        private void Panel_DeepPaint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = panel.ClientRectangle;

            // 使用LeagueVisualEffects绘制增强面板
            LeagueVisualEffects.DrawEnhancedLeaguePanel(g, bounds, panel);

            // 绘制生产主题装饰（蓝色六边形）
            if (bounds.Width > 100 && bounds.Height > 100)
            {
                LOLGeometry.DrawGlowingHexagon(g, bounds.Right - 30, bounds.Y + 30, 12, LeagueColors.AccentBlue);
                LOLGeometry.DrawGlowingHexagon(g, bounds.X + 25, bounds.Bottom - 25, 8, LeagueColors.AccentBlueDark);
            }
        }

        /// <summary>
        /// 标签深度绘制事件
        /// </summary>
        private void Label_DeepPaint(object sender, PaintEventArgs e)
        {
            Label label = sender as Label;
            if (label == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = label.ClientRectangle;

            // 绘制标签背景装饰
            using (var brush = new SolidBrush(Color.FromArgb(15, LeagueColors.AccentBlue)))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制左侧装饰线（生产主题蓝色）
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                g.DrawLine(pen, 0, 0, 0, bounds.Height);
            }
        }

        /// <summary>
        /// 文本框深度绘制事件
        /// </summary>
        private void TextBox_DeepPaint(object sender, PaintEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, textBox.Width, textBox.Height);

            // 绘制背景渐变
            using (var brush = LOLGradients.CreateCardBackgroundGradient(bounds))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制多层边框效果（生产主题）
            Color borderColor = textBox.Focused ? LeagueColors.AccentBlue : LeagueColors.PrimaryGold;

            // 外层发光边框
            using (var pen = new Pen(Color.FromArgb(80, borderColor), 3))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 内层精细边框
            using (var pen = new Pen(borderColor, 1))
            {
                g.DrawRectangle(pen, 2, 2, bounds.Width - 5, bounds.Height - 5);
            }

            // 如果有焦点，绘制脉冲效果
            if (textBox.Focused)
            {
                LeagueVisualEffects.DrawPulseEffect(g, bounds, 1.0f);
                LeagueVisualEffects.DrawParticleEffects(g, textBox);
            }

            // 绘制角落装饰
            LOLGeometry.DrawGlowDots(g, bounds, borderColor, 3);
        }

        /// <summary>
        /// 下拉框深度绘制事件
        /// </summary>
        private void ComboBox_DeepPaint(object sender, PaintEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, comboBox.Width, comboBox.Height);

            // 绘制背景渐变
            using (var brush = LOLGradients.CreateCardBackgroundGradient(bounds))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框（生产主题）
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                g.DrawRectangle(pen, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }

            // 绘制右侧下拉箭头装饰
            var arrowRect = new Rectangle(bounds.Right - 25, bounds.Y + 5, 20, bounds.Height - 10);
            using (var fillBrush = LOLGradients.CreateArrowFillGradient(
                new PointF(arrowRect.X, arrowRect.Y),
                new PointF(arrowRect.Right, arrowRect.Bottom)))
            using (var borderPen = new Pen(LeagueColors.AccentBlue, 1))
            {
                LOLGeometry.DrawArrowDecoration(g, arrowRect, fillBrush, borderPen);
            }
        }

        /// <summary>
        /// 日期选择器深度绘制事件
        /// </summary>
        private void DateTimePicker_DeepPaint(object sender, PaintEventArgs e)
        {
            DateTimePicker dateTimePicker = sender as DateTimePicker;
            if (dateTimePicker == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, dateTimePicker.Width, dateTimePicker.Height);

            // 绘制背景渐变
            using (var brush = LOLGradients.CreateCardBackgroundGradient(bounds))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框（生产主题）
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                g.DrawRectangle(pen, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }

            // 绘制日历图标装饰
            var iconRect = new Rectangle(bounds.Right - 25, bounds.Y + 5, 16, 16);
            using (var brush = new SolidBrush(LeagueColors.AccentBlue))
            {
                g.FillRectangle(brush, iconRect);

                // 绘制简单的日历图标
                using (var pen = new Pen(LeagueColors.TextWhite, 1))
                {
                    g.DrawRectangle(pen, iconRect.X + 2, iconRect.Y + 4, iconRect.Width - 4, iconRect.Height - 6);
                    g.DrawLine(pen, iconRect.X + 5, iconRect.Y + 2, iconRect.X + 5, iconRect.Y + 6);
                    g.DrawLine(pen, iconRect.X + 11, iconRect.Y + 2, iconRect.X + 11, iconRect.Y + 6);
                }
            }
        }

        /// <summary>
        /// 保存按钮深度绘制事件 - 使用LeagueVisualEffects完整绘制
        /// </summary>
        private void BtnSave_DeepPaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            // 使用LeagueVisualEffects绘制完整LOL按钮
            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            LeagueVisualEffects.DrawLeagueButton(g, bounds, isHovered, isPressed, button.Text, button.Font);
        }

        /// <summary>
        /// 取消按钮深度绘制事件 - 使用LOL深色主题
        /// </summary>
        private void BtnCancel_DeepPaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));

            // 绘制背景渐变
            Color startColor = isHovered ? LeagueColors.DarkSurfaceLight : LeagueColors.DarkSurface;
            using (var brush = LOLGradients.CreateButtonHoverGradient(bounds, startColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            using (var pen = new Pen(LeagueColors.DarkBorder, 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 绘制文字
            using (var brush = new SolidBrush(LeagueColors.TextPrimary))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(button.Text, button.Font, brush, bounds, sf);
            }
        }

        #region LOL主题绘制事件

        /// <summary>
        /// 文本框LOL风格绘制事件
        /// </summary>
        private void TextBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, textBox.Width, textBox.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 生产主题蓝色
            Color borderColor = textBox.Focused ? LeagueColors.AccentBlue : LeagueColors.BorderGold;
            using (var pen = new Pen(borderColor, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 如果有焦点，绘制内发光效果
            if (textBox.Focused)
            {
                using (var glowPen = new Pen(LeagueColors.CreateGlow(LeagueColors.AccentBlue), 1))
                {
                    Rectangle glowRect = bounds;
                    glowRect.Inflate(-1, -1);
                    g.DrawRectangle(glowPen, glowRect);
                }
            }
        }

        /// <summary>
        /// 下拉框LOL风格绘制事件
        /// </summary>
        private void ComboBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, comboBox.Width, comboBox.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 生产主题蓝色
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 绘制右侧下拉箭头
            var arrowRect = new Rectangle(bounds.Right - 25, bounds.Y + 8, 15, 10);
            using (var brush = new SolidBrush(LeagueColors.AccentBlue))
            {
                // 绘制简单的下拉箭头
                Point[] arrowPoints = new Point[]
                {
                    new Point(arrowRect.X, arrowRect.Y),
                    new Point(arrowRect.Right, arrowRect.Y),
                    new Point(arrowRect.X + arrowRect.Width / 2, arrowRect.Bottom)
                };
                g.FillPolygon(brush, arrowPoints);
            }
        }

        /// <summary>
        /// 日期选择器LOL风格绘制事件
        /// </summary>
        private void DateTimePicker_LeaguePaint(object sender, PaintEventArgs e)
        {
            DateTimePicker dateTimePicker = sender as DateTimePicker;
            if (dateTimePicker == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, dateTimePicker.Width, dateTimePicker.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 生产主题蓝色
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 绘制日历图标装饰
            var iconRect = new Rectangle(bounds.Right - 25, bounds.Y + 5, 16, 16);
            using (var brush = new SolidBrush(LeagueColors.AccentBlue))
            {
                g.FillRectangle(brush, iconRect);

                // 绘制简单的日历图标
                using (var pen = new Pen(LeagueColors.TextWhite, 1))
                {
                    g.DrawRectangle(pen, iconRect.X + 2, iconRect.Y + 4, iconRect.Width - 4, iconRect.Height - 6);
                    g.DrawLine(pen, iconRect.X + 5, iconRect.Y + 2, iconRect.X + 5, iconRect.Y + 6);
                    g.DrawLine(pen, iconRect.X + 11, iconRect.Y + 2, iconRect.X + 11, iconRect.Y + 6);
                }
            }
        }

        /// <summary>
        /// 保存按钮LOL风格绘制事件
        /// </summary>
        private void BtnSave_LeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = button.ClientRectangle;

            // 检测鼠标状态
            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 使用LeagueVisualEffects绘制完整LOL按钮
            LeagueVisualEffects.DrawLeagueButton(g, bounds, isHovered, isPressed, button.Text, button.Font);
        }

        /// <summary>
        /// 取消按钮LOL风格绘制事件
        /// </summary>
        private void BtnCancel_LeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle bounds = button.ClientRectangle;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            // 绘制背景渐变
            Color startColor = isPressed ? LeagueColors.DarkBackground :
                              isHovered ? LeagueColors.DarkSurfaceLight : LeagueColors.DarkSurface;
            Color endColor = isPressed ? LeagueColors.DarkSurface :
                            isHovered ? LeagueColors.DarkSurface : LeagueColors.DarkBackground;

            using (var brush = LeagueColors.CreateVerticalGradient(bounds, startColor, endColor))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框
            Color borderColor = isHovered ? LeagueColors.AccentBlue : LeagueColors.DarkBorder;
            using (var pen = new Pen(borderColor, 2))
            {
                g.DrawRectangle(pen, 1, 1, bounds.Width - 3, bounds.Height - 3);
            }

            // 绘制文字
            using (var brush = new SolidBrush(LeagueColors.TextPrimary))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(button.Text, button.Font, brush, bounds, sf);
            }
        }

        /// <summary>
        /// 复选框LOL风格绘制事件
        /// </summary>
        private void CheckBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = checkBox.ClientRectangle;

            // 绘制复选框背景
            var checkRect = new Rectangle(0, 0, 16, 16);
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, checkRect);
            }

            // 绘制复选框边框 - 生产主题蓝色
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                g.DrawRectangle(pen, checkRect);
            }

            // 如果选中，绘制勾选标记
            if (checkBox.Checked)
            {
                using (var pen = new Pen(LeagueColors.AccentBlue, 3))
                {
                    g.DrawLine(pen, 3, 8, 7, 12);
                    g.DrawLine(pen, 7, 12, 13, 4);
                }
            }

            // 绘制文字
            if (!string.IsNullOrEmpty(checkBox.Text))
            {
                using (var brush = new SolidBrush(LeagueColors.TextPrimary))
                {
                    var textRect = new Rectangle(20, 0, bounds.Width - 20, bounds.Height);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(checkBox.Text, checkBox.Font, brush, textRect, sf);
                }
            }
        }

        /// <summary>
        /// 分组框LOL风格绘制事件
        /// </summary>
        private void GroupBox_LeaguePaint(object sender, PaintEventArgs e)
        {
            GroupBox groupBox = sender as GroupBox;
            if (groupBox == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = groupBox.ClientRectangle;

            // 绘制边框 - 生产主题蓝色
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                g.DrawRectangle(pen, 0, 8, bounds.Width - 1, bounds.Height - 9);
            }

            // 绘制标题背景
            if (!string.IsNullOrEmpty(groupBox.Text))
            {
                var textSize = g.MeasureString(groupBox.Text, groupBox.Font);
                var textRect = new Rectangle(10, 0, (int)textSize.Width + 10, (int)textSize.Height);

                using (var brush = new SolidBrush(LeagueColors.DarkSurface))
                {
                    g.FillRectangle(brush, textRect);
                }

                // 绘制标题文字
                using (var brush = new SolidBrush(LeagueColors.AccentBlue))
                {
                    g.DrawString(groupBox.Text, groupBox.Font, brush, 15, 0);
                }
            }
        }

        /// <summary>
        /// 数字输入框LOL风格绘制事件
        /// </summary>
        private void NumericUpDown_LeaguePaint(object sender, PaintEventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (numericUpDown == null) return;

            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, numericUpDown.Width, numericUpDown.Height);

            // 绘制背景
            using (var brush = new SolidBrush(LeagueColors.InputBackground))
            {
                g.FillRectangle(brush, bounds);
            }

            // 绘制边框 - 生产主题蓝色
            using (var pen = new Pen(LeagueColors.AccentBlue, 2))
            {
                Rectangle borderRect = bounds;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }

            // 绘制右侧上下箭头区域
            var arrowRect = new Rectangle(bounds.Right - 20, bounds.Y + 1, 18, bounds.Height - 2);
            using (var brush = new SolidBrush(LeagueColors.DarkSurfaceLight))
            {
                g.FillRectangle(brush, arrowRect);
            }

            // 绘制上箭头
            var upArrowRect = new Rectangle(arrowRect.X + 4, arrowRect.Y + 3, 10, 6);
            using (var brush = new SolidBrush(LeagueColors.AccentBlue))
            {
                Point[] upArrowPoints = new Point[]
                {
                    new Point(upArrowRect.X + 5, upArrowRect.Y),
                    new Point(upArrowRect.X, upArrowRect.Bottom),
                    new Point(upArrowRect.Right, upArrowRect.Bottom)
                };
                g.FillPolygon(brush, upArrowPoints);
            }

            // 绘制下箭头
            var downArrowRect = new Rectangle(arrowRect.X + 4, arrowRect.Y + arrowRect.Height - 9, 10, 6);
            using (var brush = new SolidBrush(LeagueColors.AccentBlue))
            {
                Point[] downArrowPoints = new Point[]
                {
                    new Point(downArrowRect.X, downArrowRect.Y),
                    new Point(downArrowRect.Right, downArrowRect.Y),
                    new Point(downArrowRect.X + 5, downArrowRect.Bottom)
                };
                g.FillPolygon(brush, downArrowPoints);
            }
        }

        #endregion
    }
}