using System;
using System.Drawing;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// UI主题管理器 - 统一管理系统界面主题和样式
    /// </summary>
    public static class UIThemeManager
    {
        #region 主题定义

        /// <summary>
        /// 当前主题类型
        /// </summary>
        public enum ThemeType
        {
            /// <summary>默认主题</summary>
            Default,
            /// <summary>深色主题</summary>
            Dark,
            /// <summary>蓝色主题</summary>
            Blue
        }

        /// <summary>
        /// 主题配色方案
        /// </summary>
        public class ThemeColors
        {
            public Color Primary { get; set; }
            public Color Secondary { get; set; }
            public Color Background { get; set; }
            public Color Surface { get; set; }
            public Color Text { get; set; }
            public Color TextSecondary { get; set; }
            public Color Border { get; set; }
            public Color Hover { get; set; }
            public Color Selected { get; set; }
            public Color Success { get; set; }
            public Color Warning { get; set; }
            public Color Error { get; set; }
        }

        #endregion

        #region 私有字段

        private static ThemeType _currentTheme = ThemeType.Default;
        private static ThemeColors _currentColors;

        #endregion

        #region 公共属性

        /// <summary>
        /// 当前主题
        /// </summary>
        public static ThemeType CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    LoadThemeColors();
                    OnThemeChanged?.Invoke();
                    LogManager.Info(string.Format("主题已切换为: {0}", value));
                }
            }
        }

        /// <summary>
        /// 当前主题配色
        /// </summary>
        public static ThemeColors Colors => _currentColors;

        /// <summary>
        /// 主题变更事件
        /// </summary>
        public static event Action OnThemeChanged;

        #endregion

        #region 静态构造函数

        static UIThemeManager()
        {
            LoadThemeColors();
        }

        #endregion

        #region 主题加载

        /// <summary>
        /// 加载主题配色
        /// </summary>
        private static void LoadThemeColors()
        {
            switch (_currentTheme)
            {
                case ThemeType.Default:
                    _currentColors = GetDefaultTheme();
                    break;
                case ThemeType.Dark:
                    _currentColors = GetDarkTheme();
                    break;
                case ThemeType.Blue:
                    _currentColors = GetBlueTheme();
                    break;
                default:
                    _currentColors = GetDefaultTheme();
                    break;
            }
        }

        /// <summary>
        /// 获取默认主题配色
        /// </summary>
        private static ThemeColors GetDefaultTheme()
        {
            return new ThemeColors
            {
                Primary = Color.FromArgb(51, 122, 183),
                Secondary = Color.FromArgb(108, 117, 125),
                Background = Color.FromArgb(248, 249, 250),
                Surface = Color.White,
                Text = Color.FromArgb(33, 37, 41),
                TextSecondary = Color.FromArgb(108, 117, 125),
                Border = Color.FromArgb(222, 226, 230),
                Hover = Color.FromArgb(233, 236, 239),
                Selected = Color.FromArgb(0, 123, 255),
                Success = Color.FromArgb(25, 135, 84),
                Warning = Color.FromArgb(255, 193, 7),
                Error = Color.FromArgb(220, 53, 69)
            };
        }

        /// <summary>
        /// 获取深色主题配色
        /// </summary>
        private static ThemeColors GetDarkTheme()
        {
            return new ThemeColors
            {
                Primary = Color.FromArgb(13, 110, 253),
                Secondary = Color.FromArgb(108, 117, 125),
                Background = Color.FromArgb(33, 37, 41),
                Surface = Color.FromArgb(52, 58, 64),
                Text = Color.FromArgb(248, 249, 250),
                TextSecondary = Color.FromArgb(173, 181, 189),
                Border = Color.FromArgb(73, 80, 87),
                Hover = Color.FromArgb(73, 80, 87),
                Selected = Color.FromArgb(13, 110, 253),
                Success = Color.FromArgb(25, 135, 84),
                Warning = Color.FromArgb(255, 193, 7),
                Error = Color.FromArgb(220, 53, 69)
            };
        }

        /// <summary>
        /// 获取蓝色主题配色
        /// </summary>
        private static ThemeColors GetBlueTheme()
        {
            return new ThemeColors
            {
                Primary = Color.FromArgb(0, 86, 179),
                Secondary = Color.FromArgb(64, 128, 191),
                Background = Color.FromArgb(240, 248, 255),
                Surface = Color.White,
                Text = Color.FromArgb(33, 37, 41),
                TextSecondary = Color.FromArgb(108, 117, 125),
                Border = Color.FromArgb(176, 216, 255),
                Hover = Color.FromArgb(224, 242, 255),
                Selected = Color.FromArgb(0, 86, 179),
                Success = Color.FromArgb(25, 135, 84),
                Warning = Color.FromArgb(255, 193, 7),
                Error = Color.FromArgb(220, 53, 69)
            };
        }

        #endregion

        #region 控件样式应用

        /// <summary>
        /// 应用主题到窗体
        /// </summary>
        /// <param name="form">目标窗体</param>
        public static void ApplyTheme(Form form)
        {
            if (form == null) return;

            try
            {
                form.BackColor = Colors.Background;
                form.ForeColor = Colors.Text;
                
                // 递归应用到所有子控件
                ApplyThemeToControls(form.Controls);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("应用主题到窗体失败: {0}", form.Name), ex);
            }
        }

        /// <summary>
        /// 递归应用主题到控件集合
        /// </summary>
        /// <param name="controls">控件集合</param>
        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                ApplyThemeToControl(control);
                
                // 递归处理子控件
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }

        /// <summary>
        /// 应用主题到单个控件
        /// </summary>
        /// <param name="control">目标控件</param>
        private static void ApplyThemeToControl(Control control)
        {
            if (control == null) return;

            // 根据控件类型应用不同样式
            switch (control)
            {
                case Panel panel:
                    panel.BackColor = Colors.Surface;
                    break;
                case Button button:
                    button.BackColor = Colors.Primary;
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Colors.Primary;
                    break;
                case TextBox textBox:
                    textBox.BackColor = Colors.Surface;
                    textBox.ForeColor = Colors.Text;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case Label label:
                    label.ForeColor = Colors.Text;
                    break;
                case TreeView treeView:
                    treeView.BackColor = Colors.Surface;
                    treeView.ForeColor = Colors.Text;
                    treeView.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case DataGridView dataGridView:
                    dataGridView.BackgroundColor = Colors.Surface;
                    dataGridView.GridColor = Colors.Border;
                    dataGridView.DefaultCellStyle.BackColor = Colors.Surface;
                    dataGridView.DefaultCellStyle.ForeColor = Colors.Text;
                    dataGridView.DefaultCellStyle.SelectionBackColor = Colors.Selected;
                    dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
                    break;
            }
        }

        #endregion

        #region 字体管理

        /// <summary>
        /// 获取标准字体
        /// </summary>
        /// <param name="size">字体大小</param>
        /// <param name="style">字体样式</param>
        /// <returns>字体对象</returns>
        public static Font GetFont(float size = 9f, FontStyle style = FontStyle.Regular)
        {
            return new Font("微软雅黑", size, style);
        }

        /// <summary>
        /// 获取标题字体
        /// </summary>
        /// <param name="size">字体大小</param>
        /// <returns>字体对象</returns>
        public static Font GetTitleFont(float size = 12f)
        {
            return new Font("微软雅黑", size, FontStyle.Bold);
        }

        #endregion
    }
}
