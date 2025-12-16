using System;
using System.Drawing;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 真实LOL主题应用器 - 简洁高效的主题应用
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public static class RealLeagueThemeApplier
    {
        #region 主题应用入口

        /// <summary>
        /// 应用真实LOL主题到窗体
        /// </summary>
        /// <param name="form">目标窗体</param>
        public static void ApplyRealLeagueTheme(Form form)
        {
            if (form == null) return;

            try
            {
                // 设置窗体基础样式
                form.BackColor = LeagueColors.DarkSurface; // #1E2328
                
                // 递归应用主题到所有控件
                ApplyThemeToControlsRecursive(form);
                
                // 启用双缓冲
                EnableDoubleBuffering(form);
            }
            catch (Exception ex)
            {
                // 静默处理异常，避免影响程序运行
                System.Diagnostics.Debug.WriteLine("应用LOL主题失败: " + ex.Message);
            }
        }

        #endregion

        #region 递归主题应用

        /// <summary>
        /// 递归应用主题到控件及其子控件
        /// </summary>
        /// <param name="parent">父控件</param>
        private static void ApplyThemeToControlsRecursive(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                ApplyThemeToSingleControl(control);
                
                // 递归处理子控件
                if (control.HasChildren)
                {
                    ApplyThemeToControlsRecursive(control);
                }
            }
        }

        /// <summary>
        /// 应用主题到单个控件
        /// </summary>
        /// <param name="control">目标控件</param>
        private static void ApplyThemeToSingleControl(Control control)
        {
            if (control is Button)
            {
                ApplyButtonTheme(control as Button);
            }
            else if (control is TextBox)
            {
                ApplyTextBoxTheme(control as TextBox);
            }
            else if (control is ComboBox)
            {
                ApplyComboBoxTheme(control as ComboBox);
            }
            else if (control is CheckBox)
            {
                ApplyCheckBoxTheme(control as CheckBox);
            }
            else if (control is Label)
            {
                ApplyLabelTheme(control as Label);
            }
            else if (control is Panel)
            {
                ApplyPanelTheme(control as Panel);
            }
            else if (control is DataGridView)
            {
                ApplyDataGridViewTheme(control as DataGridView);
            }
            else if (control is GroupBox)
            {
                ApplyGroupBoxTheme(control as GroupBox);
            }
            else if (control is DateTimePicker)
            {
                ApplyDateTimePickerTheme(control as DateTimePicker);
            }
            else if (control is NumericUpDown)
            {
                ApplyNumericUpDownTheme(control as NumericUpDown);
            }
        }

        #endregion

        #region 具体控件主题应用

        /// <summary>
        /// 应用按钮主题
        /// </summary>
        private static void ApplyButtonTheme(Button button)
        {
            button.BackColor = Color.Transparent;
            button.ForeColor = LeagueColors.TextPrimary; // #CDBE91
            button.Font = new Font("微软雅黑", 12F, FontStyle.Regular);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            
            // 移除旧的Paint事件处理器
            RemovePaintHandlers(button);
            
            // 添加新的Paint事件处理器
            button.Paint += Button_RealLeaguePaint;
        }

        /// <summary>
        /// 应用文本框主题
        /// </summary>
        private static void ApplyTextBoxTheme(TextBox textBox)
        {
            textBox.BackColor = LeagueColors.InputBackground; // #000407
            textBox.ForeColor = Color.FromArgb(241, 241, 241); // #F1F1F1
            textBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular);
            textBox.BorderStyle = BorderStyle.None;
            
            // 移除旧的Paint事件处理器
            RemovePaintHandlers(textBox);
            
            // 添加新的Paint事件处理器
            textBox.Paint += TextBox_RealLeaguePaint;
        }

        /// <summary>
        /// 应用下拉框主题
        /// </summary>
        private static void ApplyComboBoxTheme(ComboBox comboBox)
        {
            comboBox.BackColor = LeagueColors.InputBackground; // #000407
            comboBox.ForeColor = Color.FromArgb(241, 241, 241); // #F1F1F1
            comboBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular);
            comboBox.FlatStyle = FlatStyle.Flat;
            
            // 移除旧的Paint事件处理器
            RemovePaintHandlers(comboBox);
            
            // 添加新的Paint事件处理器
            comboBox.Paint += ComboBox_RealLeaguePaint;
        }

        /// <summary>
        /// 应用复选框主题
        /// </summary>
        private static void ApplyCheckBoxTheme(CheckBox checkBox)
        {
            checkBox.BackColor = Color.Transparent;
            checkBox.ForeColor = LeagueColors.TextPrimary; // #CDBE91
            checkBox.Font = new Font("微软雅黑", 12F, FontStyle.Regular);
            
            // 移除旧的Paint事件处理器
            RemovePaintHandlers(checkBox);
            
            // 添加新的Paint事件处理器
            checkBox.Paint += CheckBox_RealLeaguePaint;
        }

        /// <summary>
        /// 应用标签主题
        /// </summary>
        private static void ApplyLabelTheme(Label label)
        {
            // 使用实体背景色而不是透明，避免重影
            label.BackColor = LeagueColors.DarkBackground; // #0F1A20
            label.ForeColor = LeagueColors.TextPrimary; // #CDBE91
            label.Font = new Font("微软雅黑", 10F, FontStyle.Regular);

            // 确保没有重复绘制
            label.UseCompatibleTextRendering = false;
            label.AutoSize = true;
            label.FlatStyle = FlatStyle.Flat;
        }

        /// <summary>
        /// 应用面板主题
        /// </summary>
        private static void ApplyPanelTheme(Panel panel)
        {
            panel.BackColor = LeagueColors.DarkSurface; // #1E2328
        }

        /// <summary>
        /// 应用DataGridView主题
        /// </summary>
        private static void ApplyDataGridViewTheme(DataGridView dataGridView)
        {
            dataGridView.BackgroundColor = LeagueColors.DarkSurface;
            dataGridView.GridColor = LeagueColors.DarkBorder;
            dataGridView.DefaultCellStyle.BackColor = LeagueColors.DarkSurface;
            dataGridView.DefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
            dataGridView.DefaultCellStyle.Font = new Font("微软雅黑", 10F, FontStyle.Regular);
            dataGridView.DefaultCellStyle.SelectionBackColor = LeagueColors.RiotGold;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = LeagueColors.RiotGold;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 11F, FontStyle.Bold);
            
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.BorderStyle = BorderStyle.None;
        }

        /// <summary>
        /// 应用分组框主题
        /// </summary>
        private static void ApplyGroupBoxTheme(GroupBox groupBox)
        {
            groupBox.BackColor = Color.Transparent;
            groupBox.ForeColor = LeagueColors.RiotGold; // #C8AA6E
            groupBox.Font = new Font("微软雅黑", 12F, FontStyle.Bold);
        }

        /// <summary>
        /// 应用日期选择器主题
        /// </summary>
        private static void ApplyDateTimePickerTheme(DateTimePicker dateTimePicker)
        {
            dateTimePicker.BackColor = LeagueColors.InputBackground;
            dateTimePicker.ForeColor = Color.FromArgb(241, 241, 241);
            dateTimePicker.Font = new Font("微软雅黑", 12F, FontStyle.Regular);
        }

        /// <summary>
        /// 应用数字输入框主题
        /// </summary>
        private static void ApplyNumericUpDownTheme(NumericUpDown numericUpDown)
        {
            numericUpDown.BackColor = LeagueColors.InputBackground;
            numericUpDown.ForeColor = Color.FromArgb(241, 241, 241);
            numericUpDown.Font = new Font("微软雅黑", 12F, FontStyle.Regular);
            numericUpDown.BorderStyle = BorderStyle.None;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 移除控件的Paint事件处理器
        /// </summary>
        private static void RemovePaintHandlers(Control control)
        {
            // 通过反射移除所有Paint事件处理器
            try
            {
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
                // 静默处理异常
            }
        }

        /// <summary>
        /// 启用双缓冲
        /// </summary>
        private static void EnableDoubleBuffering(Control control)
        {
            try
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }
            catch
            {
                // 静默处理异常
            }
        }

        #endregion

        #region Paint事件处理器

        /// <summary>
        /// 按钮真实LOL风格绘制事件
        /// </summary>
        private static void Button_RealLeaguePaint(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            bool isHovered = button.ClientRectangle.Contains(button.PointToClient(Cursor.Position));
            bool isPressed = (Control.MouseButtons & MouseButtons.Left) != 0 && isHovered;

            RealLeagueRenderer.DrawRealLeagueButton(e.Graphics, button.ClientRectangle,
                isHovered, isPressed, button.Text, button.Font);
        }

        /// <summary>
        /// 文本框真实LOL风格绘制事件
        /// </summary>
        private static void TextBox_RealLeaguePaint(object sender, PaintEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            bool isHovered = textBox.ClientRectangle.Contains(textBox.PointToClient(Cursor.Position));

            RealLeagueRenderer.DrawRealLeagueTextBox(e.Graphics,
                new Rectangle(0, 0, textBox.Width, textBox.Height),
                textBox.Focused, isHovered);
        }

        /// <summary>
        /// 下拉框真实LOL风格绘制事件
        /// </summary>
        private static void ComboBox_RealLeaguePaint(object sender, PaintEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            bool isHovered = comboBox.ClientRectangle.Contains(comboBox.PointToClient(Cursor.Position));

            RealLeagueRenderer.DrawRealLeagueComboBox(e.Graphics,
                new Rectangle(0, 0, comboBox.Width, comboBox.Height), isHovered);
        }

        /// <summary>
        /// 复选框真实LOL风格绘制事件
        /// </summary>
        private static void CheckBox_RealLeaguePaint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            bool isHovered = checkBox.ClientRectangle.Contains(checkBox.PointToClient(Cursor.Position));

            // 绘制复选框
            Rectangle checkRect = new Rectangle(0, 0, 16, 16);
            RealLeagueRenderer.DrawRealLeagueCheckBox(e.Graphics, checkRect, checkBox.Checked, isHovered);

            // 绘制文字
            if (!string.IsNullOrEmpty(checkBox.Text))
            {
                using (var textBrush = new SolidBrush(checkBox.ForeColor))
                {
                    var textRect = new Rectangle(20, 0, checkBox.Width - 20, checkBox.Height);
                    var stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(checkBox.Text, checkBox.Font, textBrush, textRect, stringFormat);
                }
            }
        }

        #endregion
    }
}
