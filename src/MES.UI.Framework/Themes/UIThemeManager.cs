using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MES.UI.Framework.Controls;
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
            Blue,
            /// <summary>LOL主题（暗色+金色）</summary>
            Lol,
            /// <summary>Nova主题（未来感）</summary>
            Nova
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
                    ApplyThemeToOpenForms();
                    if (OnThemeChanged != null) OnThemeChanged.Invoke();
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
                case ThemeType.Lol:
                    _currentColors = GetLeagueTheme();
                    break;
                case ThemeType.Nova:
                    _currentColors = GetNovaTheme();
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

        /// <summary>
        /// 获取“英雄联盟（LOL）”风格主题配色（暗色+金色）
        /// </summary>
        public static ThemeColors GetLeagueTheme()
        {
            return new ThemeColors
            {
                Primary = LeagueColors.PrimaryGold,
                Secondary = LeagueColors.AccentBlue,

                Background = LeagueColors.DarkBackground,
                Surface = LeagueColors.DarkSurface,

                Text = LeagueColors.TextPrimary,
                TextSecondary = LeagueColors.TextSecondary,

                Border = LeagueColors.DarkBorder,
                Hover = LeagueColors.PrimaryGoldLight,
                Selected = LeagueColors.AccentBlueLight,

                Success = LeagueColors.SuccessGreen,
                Warning = LeagueColors.WarningOrange,
                Error = LeagueColors.ErrorRed
            };
        }

        /// <summary>
        /// 获取 Nova 主题配色（未来感）
        /// </summary>
        private static ThemeColors GetNovaTheme()
        {
            return new ThemeColors
            {
                Primary = Color.FromArgb(110, 243, 255),
                Secondary = Color.FromArgb(159, 123, 255),

                Background = Color.FromArgb(11, 15, 26),
                Surface = Color.FromArgb(18, 24, 38),

                Text = Color.FromArgb(230, 240, 255),
                TextSecondary = Color.FromArgb(167, 180, 208),

                Border = Color.FromArgb(42, 53, 80),
                Hover = Color.FromArgb(27, 38, 64),
                Selected = Color.FromArgb(32, 58, 90),

                Success = Color.FromArgb(44, 235, 143),
                Warning = Color.FromArgb(255, 181, 71),
                Error = Color.FromArgb(255, 107, 107)
            };
        }

        /// <summary>
        /// 获取 LOL 风格主题配色（暗色+金色）
        /// </summary>
        private static ThemeColors GetLolTheme()
        {
            // 兼容：历史配置可能仍使用 DefaultTheme=Lol
            return GetLeagueTheme();
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
                // LOL 主题（V2）：统一使用“LoL 客户端暗金风”应用器
                if (CurrentTheme == ThemeType.Lol)
                {
                    LolV2ThemeApplier.Apply(form);
                    return;
                }

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
        /// 应用主题到当前所有已打开的窗体
        /// </summary>
        public static void ApplyThemeToOpenForms()
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    ApplyTheme(form);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("应用主题到打开窗体失败", ex);
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

            // 优先处理少数“需要跳过”的自绘控件
            if (control is ModernButton)
            {
                // ModernButton 自己订阅了主题变更事件并管理颜色
                return;
            }

            // 容器类
            var panel = control as Panel;
            if (panel != null)
            {
                panel.BackColor = Colors.Surface;
                return;
            }

            var tableLayoutPanel = control as TableLayoutPanel;
            if (tableLayoutPanel != null)
            {
                tableLayoutPanel.BackColor = Colors.Surface;
                return;
            }

            var groupBox = control as GroupBox;
            if (groupBox != null)
            {
                groupBox.BackColor = Colors.Surface;
                if (IsTooDarkForCurrentTheme(groupBox.ForeColor))
                {
                    groupBox.ForeColor = Colors.Text;
                }
                return;
            }

            var splitContainer = control as SplitContainer;
            if (splitContainer != null)
            {
                splitContainer.BackColor = Colors.Border;
                splitContainer.Panel1.BackColor = Colors.Surface;
                splitContainer.Panel2.BackColor = Colors.Surface;
                return;
            }

            var tabControl = control as TabControl;
            if (tabControl != null)
            {
                tabControl.BackColor = Colors.Background;
                tabControl.ForeColor = Colors.Text;
                return;
            }

            var tabPage = control as TabPage;
            if (tabPage != null)
            {
                tabPage.BackColor = Colors.Background;
                tabPage.ForeColor = Colors.Text;
                try { tabPage.UseVisualStyleBackColor = false; } catch { }    
                return;
            }

            // 输入控件
            var textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = Colors.Surface;
                textBox.ForeColor = Colors.Text;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                return;
            }

            var richTextBox = control as RichTextBox;
            if (richTextBox != null)
            {
                richTextBox.BackColor = Colors.Surface;
                richTextBox.ForeColor = Colors.Text;
                richTextBox.BorderStyle = BorderStyle.FixedSingle;
                return;
            }

            var comboBox = control as ComboBox;
            if (comboBox != null)
            {
                comboBox.BackColor = Colors.Surface;
                comboBox.ForeColor = Colors.Text;
                return;
            }

            var numericUpDown = control as NumericUpDown;
            if (numericUpDown != null)
            {
                numericUpDown.BackColor = Colors.Surface;
                numericUpDown.ForeColor = Colors.Text;
                return;
            }

            var dateTimePicker = control as DateTimePicker;
            if (dateTimePicker != null)
            {
                dateTimePicker.CalendarMonthBackground = Colors.Surface;
                dateTimePicker.CalendarForeColor = Colors.Text;
                dateTimePicker.CalendarTitleBackColor = Colors.Primary;
                dateTimePicker.CalendarTitleForeColor = Color.White;
                return;
            }

            // 文本类
            var label = control as Label;
            if (label != null)
            {
                if (IsTooDarkForCurrentTheme(label.ForeColor))
                {
                    label.ForeColor = Colors.Text;
                }
                return;
            }

            // 按钮（普通 Button：避免粗暴覆盖为 Primary，防止破坏“卡片按钮”等定制样式）
            var button = control as Button;
            if (button != null)
            {
                ApplyThemeToButton(button);
                return;
            }

            // 列表/树/表格
            var treeView = control as TreeView;
            if (treeView != null)
            {
                treeView.BackColor = Colors.Surface;
                if (IsTooDarkForCurrentTheme(treeView.ForeColor))
                {
                    treeView.ForeColor = Colors.Text;
                }
                treeView.BorderStyle = BorderStyle.FixedSingle;
                EnableDoubleBuffering(treeView);
                return;
            }

            var dataGridView = control as DataGridView;
            if (dataGridView != null)
            {
                ApplyThemeToDataGridView(dataGridView);
                return;
            }

            // 菜单/工具栏
            var toolStrip = control as ToolStrip;
            if (toolStrip != null)
            {
                toolStrip.BackColor = Colors.Surface;
                toolStrip.ForeColor = Colors.Text;
                ApplyThemeToToolStripItems(toolStrip.Items);
                return;
            }
        }

        private static void ApplyThemeToButton(Button button)
        {
            if (button == null) return;

            // 自绘按钮不处理
            if (button is ModernButton) return;

            // “卡片式按钮/自定义按钮” 的常见特征：Flat + UseVisualStyleBackColor=false
            if (button.FlatStyle == FlatStyle.Flat && !button.UseVisualStyleBackColor)
            {
                button.BackColor = Colors.Surface;
                button.FlatAppearance.BorderColor = Colors.Border;

                if (IsTooDarkForCurrentTheme(button.ForeColor))
                {
                    button.ForeColor = Colors.Text;
                }
                return;
            }

            // 其他按钮：尽量不改背景，只保证文字在深色主题下可读
            if (IsTooDarkForCurrentTheme(button.ForeColor))
            {
                button.ForeColor = Colors.Text;
            }
        }

        private static void ApplyThemeToDataGridView(DataGridView dataGridView)
        {
            if (dataGridView == null) return;

            dataGridView.EnableHeadersVisualStyles = false;

            dataGridView.BackgroundColor = Colors.Surface;
            dataGridView.GridColor = Colors.Border;

            dataGridView.DefaultCellStyle.BackColor = Colors.Surface;
            dataGridView.DefaultCellStyle.ForeColor = Colors.Text;
            dataGridView.DefaultCellStyle.SelectionBackColor = Colors.Selected;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Colors.Primary;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = GetTitleFont(10f);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 交替行颜色：暗色主题略变亮，亮色主题略变暗
            float altFactor = Colors.Surface.GetBrightness() < 0.5f ? 1.06f : 0.97f;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = AdjustColorBrightness(Colors.Surface, altFactor);

            if (dataGridView.RowHeadersVisible)
            {
                dataGridView.RowHeadersDefaultCellStyle.BackColor = Colors.Background;
                dataGridView.RowHeadersDefaultCellStyle.ForeColor = Colors.Text;
            }

            EnableDoubleBuffering(dataGridView);
        }

        private static void ApplyThemeToToolStripItems(ToolStripItemCollection items)
        {
            if (items == null) return;

            foreach (ToolStripItem item in items)
            {
                if (item == null) continue;

                // 分隔符不处理
                if (item is ToolStripSeparator) continue;

                item.BackColor = Colors.Surface;
                if (IsTooDarkForCurrentTheme(item.ForeColor))
                {
                    item.ForeColor = Colors.Text;
                }

                var dropDownItem = item as ToolStripDropDownItem;
                if (dropDownItem != null)
                {
                    ApplyThemeToToolStripItems(dropDownItem.DropDownItems);
                }
            }
        }

        private static bool IsAccentColor(Color color)
        {
            return color == Colors.Primary ||
                   color == Colors.Secondary ||
                   color == Colors.Selected ||
                   color == Colors.Success ||
                   color == Colors.Warning ||
                   color == Colors.Error;
        }

        private static bool IsTooDarkForCurrentTheme(Color color)
        {
            // 仅在深色系主题下做“纠偏”，避免破坏浅色主题的视觉设计
            if (_currentTheme != ThemeType.Dark && _currentTheme != ThemeType.Lol && _currentTheme != ThemeType.Nova) return false;
            // 仅修正“接近灰阶的暗色文本”（例如黑色/深灰），避免误伤业务模块的彩色强调
            if (!IsNearGray(color)) return false;
            return color.GetBrightness() < 0.65f;
        }

        private static bool IsNearGray(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));
            return (max - min) < 12;
        }

        private static Color AdjustColorBrightness(Color color, float factor)
        {
            int r = Math.Min(255, Math.Max(0, (int)(color.R * factor)));
            int g = Math.Min(255, Math.Max(0, (int)(color.G * factor)));
            int b = Math.Min(255, Math.Max(0, (int)(color.B * factor)));

            return Color.FromArgb(color.A, r, g, b);
        }

        private static void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;

            try
            {
                var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (prop != null)
                {
                    prop.SetValue(control, true, null);
                }
            }
            catch
            {
                // ignore
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
            return DesignTokens.Typography.CreateFont(size, style);
        }

        /// <summary>
        /// 获取标题字体
        /// </summary>
        /// <param name="size">字体大小</param>
        /// <returns>字体对象</returns>
        public static Font GetTitleFont(float size = 12f)
        {
            return DesignTokens.Typography.CreateFont(size, FontStyle.Bold);
        }

        #endregion
    }
}
