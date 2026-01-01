using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 真实LOL渲染器 - 基于leagueoflegends-wpf项目的精确实现
    /// 完全复制RiotButton.xaml和RiotTextBox.xaml的视觉效果
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public static class RealLeagueRenderer
    {
        #region 精确的LOL颜色定义（基于真实LOL客户端）

        // RiotButton颜色系统
        private static readonly Color ButtonNormalBackground = Color.FromArgb(30, 35, 40);        // #1E2328
        private static readonly Color ButtonNormalBorderStart = Color.FromArgb(200, 170, 110);    // #C8AA6E
        private static readonly Color ButtonNormalBorderEnd = Color.FromArgb(121, 92, 40);        // #795c28
        private static readonly Color ButtonNormalText = Color.FromArgb(205, 190, 145);           // #CDBE91

        private static readonly Color ButtonHoverBackgroundStart = Color.FromArgb(30, 35, 42);    // #1e232a
        private static readonly Color ButtonHoverBackgroundEnd = Color.FromArgb(68, 62, 46);      // #443e2e
        private static readonly Color ButtonHoverBorderStart = Color.FromArgb(240, 230, 215);     // #f0e6d7
        private static readonly Color ButtonHoverBorderEnd = Color.FromArgb(201, 157, 61);        // #c99d3d
        private static readonly Color ButtonHoverText = Color.FromArgb(240, 230, 210);            // #f0e6d2

        private static readonly Color ButtonPressedBackgroundStart = Color.FromArgb(55, 49, 33);  // #373121
        private static readonly Color ButtonPressedBackgroundEnd = Color.FromArgb(30, 35, 40);    // #1e2328
        private static readonly Color ButtonPressedBorderStart = Color.FromArgb(120, 90, 40);     // #785a28
        private static readonly Color ButtonPressedBorderEnd = Color.FromArgb(200, 170, 110);     // #c8aa6e
        private static readonly Color ButtonPressedText = Color.FromArgb(228, 202, 165);          // #e4caa5

        private static readonly Color ButtonInnerBorder = Color.FromArgb(204, 9, 17, 25);         // #CC091119

        // RiotTextBox颜色系统
        private static readonly Color TextBoxNormalBackground = Color.FromArgb(0, 4, 7);          // #000407
        private static readonly Color TextBoxNormalBorder = Color.FromArgb(120, 90, 40);          // #785A28
        private static readonly Color TextBoxNormalText = Color.FromArgb(241, 241, 241);          // #F1F1F1

        private static readonly Color TextBoxHoverBackgroundStart = Color.FromArgb(6, 16, 26);    // #06101A
        private static readonly Color TextBoxHoverBackgroundEnd = Color.FromArgb(20, 29, 36);     // #141D24
        private static readonly Color TextBoxHoverBorderStart = Color.FromArgb(121, 91, 41);      // #795B29
        private static readonly Color TextBoxHoverBorderMiddle = Color.FromArgb(158, 128, 73);    // #9E8049
        private static readonly Color TextBoxHoverBorderEnd = Color.FromArgb(199, 169, 110);      // #C7A96E

        #endregion
        #region 真实LOL按钮渲染（基于RiotButton.xaml精确实现）

        /// <summary>
        /// 绘制真实LOL风格按钮 - 完全基于RiotButton.xaml的精确实现
        /// 严格遵循C# 5.0语法规范，避免使用C# 6.0+特性
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="isHovered">是否悬停</param>
        /// <param name="isPressed">是否按下</param>
        /// <param name="text">按钮文字</param>
        /// <param name="font">字体</param>
        public static void DrawRealLeagueButton(Graphics g, Rectangle bounds, bool isHovered, bool isPressed, string text, Font font)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 根据状态选择颜色（完全基于RiotButton.xaml）
            Color backgroundColor1, backgroundColor2, borderColor1, borderColor2, textColor;

            if (isPressed)
            {
                // 按下状态：RiotButtonBackgroundPressed + RiotButtonBorderBrushPressed
                backgroundColor1 = ButtonPressedBackgroundStart;  // #373121
                backgroundColor2 = ButtonPressedBackgroundEnd;    // #1e2328
                borderColor1 = ButtonPressedBorderStart;          // #785a28
                borderColor2 = ButtonPressedBorderEnd;            // #c8aa6e
                textColor = ButtonPressedText;                    // #e4caa5
            }
            else if (isHovered)
            {
                // 悬停状态：RiotButtonBackgroundHover + RiotButtonBorderBrushHover
                backgroundColor1 = ButtonHoverBackgroundStart;    // #1e232a
                backgroundColor2 = ButtonHoverBackgroundEnd;      // #443e2e
                borderColor1 = ButtonHoverBorderStart;            // #f0e6d7
                borderColor2 = ButtonHoverBorderEnd;              // #c99d3d
                textColor = ButtonHoverText;                      // #f0e6d2
            }
            else
            {
                // 正常状态：单色背景 + RiotButtonBorderBrush
                backgroundColor1 = ButtonNormalBackground;        // #1E2328
                backgroundColor2 = ButtonNormalBackground;        // #1E2328 (单色)
                borderColor1 = ButtonNormalBorderStart;           // #C8AA6E
                borderColor2 = ButtonNormalBorderEnd;             // #795c28
                textColor = ButtonNormalText;                     // #CDBE91
            }

            // 绘制背景（正常状态为单色，悬停/按下状态为渐变）
            if (isHovered || isPressed)
            {
                using (var backgroundBrush = new LinearGradientBrush(bounds, backgroundColor1, backgroundColor2, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(backgroundBrush, bounds);
                }
            }
            else
            {
                using (var backgroundBrush = new SolidBrush(backgroundColor1))
                {
                    g.FillRectangle(backgroundBrush, bounds);
                }
            }

            // 绘制边框渐变（所有状态都使用渐变）
            using (var borderBrush = new LinearGradientBrush(bounds, borderColor1, borderColor2, LinearGradientMode.Vertical))
            using (var borderPen = new Pen(borderBrush, 1))
            {
                Rectangle borderRect = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                g.DrawRectangle(borderPen, borderRect);
            }

            // 绘制内边框阴影 - LOL特有的InnerBorderStyle效果
            using (var innerPen = new Pen(ButtonInnerBorder, 1))
            {
                Rectangle innerRect = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 3, bounds.Height - 3);
                g.DrawRectangle(innerPen, innerRect);
            }

            // 绘制文字（居中对齐）
            if (!string.IsNullOrEmpty(text))
            {
                using (var textBrush = new SolidBrush(textColor))
                {
                    var stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(text, font, textBrush, bounds, stringFormat);
                }
            }
        }

        /// <summary>
        /// 绘制真实LOL风格按钮（支持 hover/press 平滑过渡）
        /// </summary>
        public static void DrawRealLeagueButton(Graphics g, Rectangle bounds, float hoverProgress, float pressProgress, string text, Font font)
        {
            if (g == null) return;

            float h = EaseOutCubic(Clamp01(hoverProgress));
            float p = EaseOutCubic(Clamp01(pressProgress));

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Normal -> Hover -> Pressed
            var backgroundColor1 = LerpColor(ButtonNormalBackground, ButtonHoverBackgroundStart, h);
            backgroundColor1 = LerpColor(backgroundColor1, ButtonPressedBackgroundStart, p);

            var backgroundColor2 = LerpColor(ButtonNormalBackground, ButtonHoverBackgroundEnd, h);
            backgroundColor2 = LerpColor(backgroundColor2, ButtonPressedBackgroundEnd, p);

            var borderColor1 = LerpColor(ButtonNormalBorderStart, ButtonHoverBorderStart, h);
            borderColor1 = LerpColor(borderColor1, ButtonPressedBorderStart, p);

            var borderColor2 = LerpColor(ButtonNormalBorderEnd, ButtonHoverBorderEnd, h);
            borderColor2 = LerpColor(borderColor2, ButtonPressedBorderEnd, p);

            var textColor = LerpColor(ButtonNormalText, ButtonHoverText, h);
            textColor = LerpColor(textColor, ButtonPressedText, p);

            // 背景（正常态为单色，hover/press 为渐变）
            if (h > 0.001f || p > 0.001f)
            {
                using (var backgroundBrush = new LinearGradientBrush(bounds, backgroundColor1, backgroundColor2, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(backgroundBrush, bounds);
                }
            }
            else
            {
                using (var backgroundBrush = new SolidBrush(backgroundColor1))
                {
                    g.FillRectangle(backgroundBrush, bounds);
                }
            }

            // 边框渐变
            using (var borderBrush = new LinearGradientBrush(bounds, borderColor1, borderColor2, LinearGradientMode.Vertical))
            using (var borderPen = new Pen(borderBrush, 1))
            {
                Rectangle borderRect = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                g.DrawRectangle(borderPen, borderRect);
            }

            // 内边框阴影 - LOL特有的 InnerBorderStyle 效果
            using (var innerPen = new Pen(ButtonInnerBorder, 1))
            {
                Rectangle innerRect = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 3, bounds.Height - 3);
                g.DrawRectangle(innerPen, innerRect);
            }

            // 文字（居中对齐）
            if (!string.IsNullOrEmpty(text))
            {
                using (var textBrush = new SolidBrush(textColor))
                {
                    var stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(text, font, textBrush, bounds, stringFormat);
                }
            }
        }

        private static float Clamp01(float v)
        {
            if (v < 0f) return 0f;
            if (v > 1f) return 1f;
            return v;
        }

        private static float EaseOutCubic(float t)
        {
            t = Clamp01(t);
            float p = 1f - t;
            return 1f - p * p * p;
        }

        private static Color LerpColor(Color a, Color b, float t)
        {
            t = Clamp01(t);
            int rr = a.R + (int)Math.Round((b.R - a.R) * t);
            int gg = a.G + (int)Math.Round((b.G - a.G) * t);
            int bb = a.B + (int)Math.Round((b.B - a.B) * t);

            if (rr < 0) rr = 0; if (rr > 255) rr = 255;
            if (gg < 0) gg = 0; if (gg > 255) gg = 255;
            if (bb < 0) bb = 0; if (bb > 255) bb = 255;

            return Color.FromArgb(255, rr, gg, bb);
        }

        #endregion

        #region 真实LOL文本框渲染（基于RiotTextBox.xaml精确实现）

        /// <summary>
        /// 绘制真实LOL风格文本框 - 完全基于RiotTextBox.xaml的精确实现
        /// 严格遵循C# 5.0语法规范，避免使用C# 6.0+特性
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="isFocused">是否获得焦点</param>
        /// <param name="isHovered">是否悬停</param>
        public static void DrawRealLeagueTextBox(Graphics g, Rectangle bounds, bool isFocused, bool isHovered)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 根据状态选择颜色（完全基于RiotTextBox.xaml）
            Color backgroundColor1, backgroundColor2;
            Color borderColor1, borderColor2, borderColor3;

            if (isFocused || isHovered)
            {
                // 焦点/悬停状态：TextboxBackgroundHoverBrush + TextboxBorderHoverBrush
                backgroundColor1 = TextBoxHoverBackgroundStart;   // #06101A
                backgroundColor2 = TextBoxHoverBackgroundEnd;     // #141D24
                borderColor1 = TextBoxHoverBorderStart;           // #795B29
                borderColor2 = TextBoxHoverBorderMiddle;          // #9E8049
                borderColor3 = TextBoxHoverBorderEnd;             // #C7A96E
            }
            else
            {
                // 正常状态：单色背景和边框
                backgroundColor1 = TextBoxNormalBackground;       // #000407
                backgroundColor2 = TextBoxNormalBackground;       // #000407 (单色)
                borderColor1 = TextBoxNormalBorder;               // #785A28
                borderColor2 = TextBoxNormalBorder;               // #785A28 (单色)
                borderColor3 = TextBoxNormalBorder;               // #785A28 (单色)
            }

            // 绘制背景（正常状态为单色，焦点/悬停状态为渐变）
            if (isFocused || isHovered)
            {
                using (var backgroundBrush = new LinearGradientBrush(bounds, backgroundColor1, backgroundColor2, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(backgroundBrush, bounds);
                }
            }
            else
            {
                using (var backgroundBrush = new SolidBrush(backgroundColor1))
                {
                    g.FillRectangle(backgroundBrush, bounds);
                }
            }

            // 绘制边框（焦点/悬停状态使用三色渐变，正常状态使用单色）
            if (isFocused || isHovered)
            {
                // 创建三色渐变边框（模拟RiotTextBox的TextboxBorderHoverBrush）
                using (var borderBrush = CreateThreeColorGradientBrush(bounds, borderColor1, borderColor2, borderColor3))
                using (var borderPen = new Pen(borderBrush, 1))
                {
                    Rectangle borderRect = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                    g.DrawRectangle(borderPen, borderRect);
                }
            }
            else
            {
                using (var borderPen = new Pen(borderColor1, 1))
                {
                    Rectangle borderRect = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                    g.DrawRectangle(borderPen, borderRect);
                }
            }
        }

        /// <summary>
        /// 创建三色渐变画刷 - 模拟RiotTextBox的TextboxBorderHoverBrush
        /// 严格遵循C# 5.0语法规范
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="color1">起始颜色</param>
        /// <param name="color2">中间颜色</param>
        /// <param name="color3">结束颜色</param>
        /// <returns>三色渐变画刷</returns>
        private static LinearGradientBrush CreateThreeColorGradientBrush(Rectangle bounds, Color color1, Color color2, Color color3)
        {
            var brush = new LinearGradientBrush(bounds, color1, color3, LinearGradientMode.Vertical);

            // 设置三色渐变停止点（模拟XAML中的GradientStop）
            ColorBlend colorBlend = new ColorBlend();
            colorBlend.Colors = new Color[] { color1, color2, color3 };
            colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f };
            brush.InterpolationColors = colorBlend;

            return brush;
        }

        #endregion

        #region 真实LOL复选框渲染

        /// <summary>
        /// 绘制真实LOL风格复选框 - 基于RiotCheckBox.xaml的精确实现
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="checkRect">复选框区域</param>
        /// <param name="isChecked">是否选中</param>
        /// <param name="isHovered">是否悬停</param>
        public static void DrawRealLeagueCheckBox(Graphics g, Rectangle checkRect, bool isChecked, bool isHovered)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 背景色
            Color backgroundColor = Color.FromArgb(0, 4, 7); // #000407
            using (var backgroundBrush = new SolidBrush(backgroundColor))
            {
                g.FillRectangle(backgroundBrush, checkRect);
            }

            // 边框色
            Color borderColor = isHovered ? Color.FromArgb(199, 169, 110) : Color.FromArgb(120, 90, 40); // #C7A96E : #785A28
            using (var borderPen = new Pen(borderColor, 1))
            {
                Rectangle borderRect = new Rectangle(checkRect.X, checkRect.Y, checkRect.Width - 1, checkRect.Height - 1);
                g.DrawRectangle(borderPen, borderRect);
            }

            // 如果选中，绘制勾选标记
            if (isChecked)
            {
                Color checkColor = Color.FromArgb(240, 230, 210); // #F0E6D2
                using (var checkPen = new Pen(checkColor, 2))
                {
                    // 绘制勾选标记
                    Point[] checkPoints = new Point[]
                    {
                        new Point(checkRect.X + 3, checkRect.Y + checkRect.Height / 2),
                        new Point(checkRect.X + checkRect.Width / 2, checkRect.Y + checkRect.Height - 4),
                        new Point(checkRect.X + checkRect.Width - 3, checkRect.Y + 3)
                    };
                    g.DrawLines(checkPen, checkPoints);
                }
            }
        }

        #endregion

        #region 真实LOL下拉框渲染

        /// <summary>
        /// 绘制真实LOL风格下拉框 - 基于RiotComboBox.xaml的精确实现
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="isHovered">是否悬停</param>
        public static void DrawRealLeagueComboBox(Graphics g, Rectangle bounds, bool isHovered)
        {
            // 使用与文本框相同的绘制逻辑
            DrawRealLeagueTextBox(g, bounds, false, isHovered);

            // 绘制下拉箭头
            Rectangle arrowRect = new Rectangle(bounds.Right - 20, bounds.Y + (bounds.Height - 8) / 2, 12, 8);
            Color arrowColor = Color.FromArgb(240, 230, 210); // #F0E6D2
            
            using (var arrowBrush = new SolidBrush(arrowColor))
            {
                Point[] arrowPoints = new Point[]
                {
                    new Point(arrowRect.X, arrowRect.Y),
                    new Point(arrowRect.Right, arrowRect.Y),
                    new Point(arrowRect.X + arrowRect.Width / 2, arrowRect.Bottom)
                };
                g.FillPolygon(arrowBrush, arrowPoints);
            }
        }

        #endregion
    }
}
