using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Utilities
{
    /// <summary>
    /// UI通用工具类 - 提供常用的UI操作和辅助方法
    /// </summary>
    public static class UIHelper
    {
        #region 窗体操作

        /// <summary>
        /// 居中显示窗体
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <param name="parent">父窗体，为null时相对于屏幕居中</param>
        public static void CenterForm(Form form, Form parent = null)
        {
            if (form == null) return;

            try
            {
                if (parent != null)
                {
                    // 相对于父窗体居中
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(
                        parent.Location.X + (parent.Width - form.Width) / 2,
                        parent.Location.Y + (parent.Height - form.Height) / 2
                    );
                }
                else
                {
                    // 相对于屏幕居中
                    form.StartPosition = FormStartPosition.CenterScreen;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("居中显示窗体失败: {0}", form.Name), ex);
            }
        }

        /// <summary>
        /// 设置窗体最小尺寸
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <param name="minWidth">最小宽度</param>
        /// <param name="minHeight">最小高度</param>
        public static void SetMinimumSize(Form form, int minWidth = 800, int minHeight = 600)
        {
            if (form == null) return;

            form.MinimumSize = new Size(minWidth, minHeight);
        }

        /// <summary>
        /// 应用标准窗体样式
        /// </summary>
        /// <param name="form">目标窗体</param>
        public static void ApplyStandardFormStyle(Form form)
        {
            if (form == null) return;

            try
            {
                // 应用主题
                UIThemeManager.ApplyTheme(form);
                
                // 设置字体
                form.Font = UIThemeManager.GetFont();
                
                // 设置图标
                form.Icon = SystemIcons.Application;
                
                // 设置最小尺寸
                SetMinimumSize(form);
                
                LogManager.Debug(string.Format("已应用标准窗体样式: {0}", form.Name));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("应用标准窗体样式失败: {0}", form.Name), ex);
            }
        }

        #endregion

        #region 控件操作

        /// <summary>
        /// 设置控件启用状态
        /// </summary>
        /// <param name="enabled">是否启用</param>
        /// <param name="controls">控件数组</param>
        public static void SetControlsEnabled(bool enabled, params Control[] controls)
        {
            if (controls == null) return;

            foreach (var control in controls)
            {
                if (control != null)
                {
                    control.Enabled = enabled;
                }
            }
        }

        /// <summary>
        /// 设置控件可见性
        /// </summary>
        /// <param name="visible">是否可见</param>
        /// <param name="controls">控件数组</param>
        public static void SetControlsVisible(bool visible, params Control[] controls)
        {
            if (controls == null) return;

            foreach (var control in controls)
            {
                if (control != null)
                {
                    control.Visible = visible;
                }
            }
        }

        /// <summary>
        /// 清空文本框内容
        /// </summary>
        /// <param name="textBoxes">文本框数组</param>
        public static void ClearTextBoxes(params TextBox[] textBoxes)
        {
            if (textBoxes == null) return;

            foreach (var textBox in textBoxes)
            {
                if (textBox != null)
                {
                    textBox.Clear();
                }
            }
        }

        /// <summary>
        /// 设置控件焦点
        /// </summary>
        /// <param name="control">目标控件</param>
        public static void SetFocus(Control control)
        {
            if (control != null && control.CanFocus)
            {
                control.Focus();
            }
        }

        #endregion

        #region 消息提示

        /// <summary>
        /// 显示信息消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="owner">父窗体</param>
        public static void ShowInfo(string message, string title = "提示", IWin32Window owner = null)
        {
            MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示警告消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="owner">父窗体</param>
        public static void ShowWarning(string message, string title = "警告", IWin32Window owner = null)
        {
            MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="owner">父窗体</param>
        public static void ShowError(string message, string title = "错误", IWin32Window owner = null)
        {
            MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="title">标题</param>
        /// <param name="owner">父窗体</param>
        /// <returns>用户选择结果</returns>
        public static bool ShowConfirm(string message, string title = "确认", IWin32Window owner = null)
        {
            var result = MessageBox.Show(owner, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        #endregion

        #region 数据网格操作

        /// <summary>
        /// 配置标准数据网格样式
        /// </summary>
        /// <param name="dataGridView">数据网格控件</param>
        public static void ConfigureStandardDataGrid(DataGridView dataGridView)
        {
            if (dataGridView == null) return;

            try
            {
                // 基本设置
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;
                dataGridView.ReadOnly = true;
                dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView.MultiSelect = false;
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                
                // 样式设置
                dataGridView.BorderStyle = BorderStyle.FixedSingle;
                dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dataGridView.RowHeadersVisible = false;
                dataGridView.EnableHeadersVisualStyles = false;
                
                // 应用主题颜色（如果父控件是窗体）
                if (dataGridView.Parent is Form parentForm)
                {
                    UIThemeManager.ApplyTheme(parentForm);
                }
                
                // 列标题样式
                dataGridView.ColumnHeadersDefaultCellStyle.BackColor = UIThemeManager.Colors.Primary;
                dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = UIThemeManager.GetTitleFont(10f);
                dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                LogManager.Debug("已配置标准数据网格样式");
            }
            catch (Exception ex)
            {
                LogManager.Error("配置数据网格样式失败", ex);
            }
        }

        #endregion

        #region 颜色和绘制

        /// <summary>
        /// 混合两种颜色
        /// </summary>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="ratio">混合比例 (0.0-1.0)</param>
        /// <returns>混合后的颜色</returns>
        public static Color BlendColors(Color color1, Color color2, double ratio)
        {
            ratio = Math.Max(0, Math.Min(1, ratio));
            
            int r = (int)(color1.R * (1 - ratio) + color2.R * ratio);
            int g = (int)(color1.G * (1 - ratio) + color2.G * ratio);
            int b = (int)(color1.B * (1 - ratio) + color2.B * ratio);
            
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 调整颜色亮度
        /// </summary>
        /// <param name="color">原始颜色</param>
        /// <param name="factor">亮度因子 (>1变亮, <1变暗)</param>
        /// <returns>调整后的颜色</returns>
        public static Color AdjustBrightness(Color color, float factor)
        {
            int r = Math.Min(255, Math.Max(0, (int)(color.R * factor)));
            int g = Math.Min(255, Math.Max(0, (int)(color.G * factor)));
            int b = Math.Min(255, Math.Max(0, (int)(color.B * factor)));
            
            return Color.FromArgb(color.A, r, g, b);
        }

        #endregion

        #region 尺寸和布局

        /// <summary>
        /// 根据DPI缩放尺寸
        /// </summary>
        /// <param name="size">原始尺寸</param>
        /// <param name="graphics">图形对象</param>
        /// <returns>缩放后的尺寸</returns>
        public static Size ScaleSize(Size size, Graphics graphics)
        {
            if (graphics == null) return size;

            float dpiX = graphics.DpiX / 96f;
            float dpiY = graphics.DpiY / 96f;
            
            return new Size(
                (int)(size.Width * dpiX),
                (int)(size.Height * dpiY)
            );
        }

        /// <summary>
        /// 计算文本尺寸
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="font">字体</param>
        /// <param name="graphics">图形对象</param>
        /// <returns>文本尺寸</returns>
        public static Size MeasureText(string text, Font font, Graphics graphics = null)
        {
            if (string.IsNullOrEmpty(text) || font == null)
                return Size.Empty;

            if (graphics != null)
            {
                return graphics.MeasureString(text, font).ToSize();
            }
            else
            {
                return TextRenderer.MeasureText(text, font);
            }
        }

        #endregion
    }
}
