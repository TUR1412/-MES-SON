using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 现代化按钮控件 - 提供美观的按钮样式和交互效果
    /// </summary>
    public class ModernButton : Button
    {
        #region 私有字段

        private bool _isHovered = false;
        private bool _isPressed = false;
        private int _borderRadius = 4;
        private Color _hoverColor = Color.Empty;
        private Color _pressedColor = Color.Empty;
        private ButtonStyle _buttonStyle = ButtonStyle.Primary;

        #endregion

        #region 枚举定义

        /// <summary>
        /// 按钮样式类型
        /// </summary>
        public enum ButtonStyle
        {
            /// <summary>主要按钮</summary>
            Primary,
            /// <summary>次要按钮</summary>
            Secondary,
            /// <summary>成功按钮</summary>
            Success,
            /// <summary>警告按钮</summary>
            Warning,
            /// <summary>危险按钮</summary>
            Danger,
            /// <summary>信息按钮</summary>
            Info,
            /// <summary>轮廓按钮</summary>
            Outline
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 边框圆角半径
        /// </summary>
        [Category("外观")]
        [Description("按钮边框的圆角半径")]
        [DefaultValue(4)]
        public int BorderRadius
        {
            get { return _borderRadius; }
            set
            {
                _borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        /// <summary>
        /// 按钮样式
        /// </summary>
        [Category("外观")]
        [Description("按钮的样式类型")]
        [DefaultValue(ButtonStyle.Primary)]
        public ButtonStyle Style
        {
            get { return _buttonStyle; }
            set
            {
                _buttonStyle = value;
                UpdateColors();
                Invalidate();
            }
        }

        /// <summary>
        /// 悬停时的颜色
        /// </summary>
        [Category("外观")]
        [Description("鼠标悬停时的背景颜色")]
        public Color HoverColor
        {
            get { return _hoverColor; }
            set
            {
                _hoverColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 按下时的颜色
        /// </summary>
        [Category("外观")]
        [Description("鼠标按下时的背景颜色")]
        public Color PressedColor
        {
            get { return _pressedColor; }
            set
            {
                _pressedColor = value;
                Invalidate();
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化现代化按钮
        /// </summary>
        public ModernButton()
        {
            // 设置控件样式
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            // 基本设置
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = UIThemeManager.GetFont(9f);
            Size = new Size(100, 35);
            Cursor = Cursors.Hand;

            // 更新颜色
            UpdateColors();

            // 订阅主题变更事件
            UIThemeManager.OnThemeChanged += UpdateColors;
        }

        #endregion

        #region 事件处理

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            _isPressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _isPressed = true;
            Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _isPressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        #endregion

        #region 绘制方法

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 获取绘制区域
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

            // 确定背景颜色
            Color backgroundColor = GetCurrentBackgroundColor();
            Color textColor = GetCurrentTextColor();

            // 绘制背景
            using (GraphicsPath path = CreateRoundedRectangle(rect, _borderRadius))
            {
                // 填充背景
                using (SolidBrush brush = new SolidBrush(backgroundColor))
                {
                    g.FillPath(brush, path);
                }

                // 绘制边框（仅轮廓样式）
                if (_buttonStyle == ButtonStyle.Outline)
                {
                    using (Pen pen = new Pen(backgroundColor, 1))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            // 绘制文本
            if (!string.IsNullOrEmpty(Text))
            {
                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    g.DrawString(Text, Font, textBrush, rect, sf);
                }
            }

            // 绘制图像
            if (Image != null)
            {
                Rectangle imageRect = GetImageRectangle();
                if (Enabled)
                {
                    g.DrawImage(Image, imageRect);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(g, Image, imageRect.X, imageRect.Y, backgroundColor);
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 更新颜色配置
        /// </summary>
        private void UpdateColors()
        {
            var colors = UIThemeManager.Colors;

            switch (_buttonStyle)
            {
                case ButtonStyle.Primary:
                    BackColor = colors.Primary;
                    ForeColor = GetContrastingTextColor(colors.Primary);
                    _hoverColor = UIThemeManager.Colors.Primary;
                    _pressedColor = UIThemeManager.Colors.Primary;
                    break;

                case ButtonStyle.Secondary:
                    BackColor = colors.Secondary;
                    ForeColor = GetContrastingTextColor(colors.Secondary);
                    _hoverColor = colors.Secondary;
                    _pressedColor = colors.Secondary;
                    break;

                case ButtonStyle.Success:
                    BackColor = colors.Success;
                    ForeColor = GetContrastingTextColor(colors.Success);
                    _hoverColor = colors.Success;
                    _pressedColor = colors.Success;
                    break;

                case ButtonStyle.Warning:
                    BackColor = colors.Warning;
                    ForeColor = GetContrastingTextColor(colors.Warning);
                    _hoverColor = colors.Warning;
                    _pressedColor = colors.Warning;
                    break;

                case ButtonStyle.Danger:
                    BackColor = colors.Error;
                    ForeColor = GetContrastingTextColor(colors.Error);
                    _hoverColor = colors.Error;
                    _pressedColor = colors.Error;
                    break;

                case ButtonStyle.Info:
                    BackColor = colors.Primary;
                    ForeColor = GetContrastingTextColor(colors.Primary);
                    _hoverColor = colors.Primary;
                    _pressedColor = colors.Primary;
                    break;

                case ButtonStyle.Outline:
                    BackColor = Color.Transparent;
                    ForeColor = colors.Primary;
                    _hoverColor = colors.Primary;
                    _pressedColor = colors.Primary;
                    break;
            }
        }

        /// <summary>
        /// 获取当前背景颜色
        /// </summary>
        private Color GetCurrentBackgroundColor()
        {
            if (!Enabled)
            {
                return UIThemeManager.Colors.Secondary;
            }

            if (_isPressed && _pressedColor != Color.Empty)
            {
                return AdjustColorBrightness(_pressedColor, 0.8f);
            }

            if (_isHovered && _hoverColor != Color.Empty)
            {
                return AdjustColorBrightness(_hoverColor, 1.1f);
            }

            return _buttonStyle == ButtonStyle.Outline ? Color.Transparent : BackColor;
        }

        /// <summary>
        /// 获取当前文本颜色
        /// </summary>
        private Color GetCurrentTextColor()
        {
            if (!Enabled)
            {
                return UIThemeManager.Colors.TextSecondary;
            }

            if (_buttonStyle == ButtonStyle.Outline)
            {
                if (_isHovered || _isPressed)
                {
                    var bg = GetCurrentBackgroundColor();
                    if (bg == Color.Transparent)
                    {
                        bg = UIThemeManager.Colors.Primary;
                    }
                    return GetContrastingTextColor(bg);
                }
                return ForeColor;
            }

            return ForeColor;
        }

        /// <summary>
        /// 创建圆角矩形路径
        /// </summary>
        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(rect.Location, size);

            // 左上角
            path.AddArc(arc, 180, 90);

            // 右上角
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // 右下角
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // 左下角
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// 获取图像绘制区域
        /// </summary>
        private Rectangle GetImageRectangle()
        {
            if (Image == null) return Rectangle.Empty;

            int x = (Width - Image.Width) / 2;
            int y = (Height - Image.Height) / 2;

            return new Rectangle(x, y, Image.Width, Image.Height);
        }

        /// <summary>
        /// 调整颜色亮度
        /// </summary>
        private Color AdjustColorBrightness(Color color, float factor)
        {
            int r = Math.Min(255, Math.Max(0, (int)(color.R * factor)));
            int g = Math.Min(255, Math.Max(0, (int)(color.G * factor)));
            int b = Math.Min(255, Math.Max(0, (int)(color.B * factor)));

            return Color.FromArgb(color.A, r, g, b);
        }

        private Color GetContrastingTextColor(Color backgroundColor)
        {
            // 采用近似亮度判断，确保在浅色/深色主题下都保持可读性
            // 参考：0.299R + 0.587G + 0.114B
            double luminance = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255.0;
            return luminance >= 0.62 ? Color.Black : Color.White;
        }

        #endregion

        #region 资源清理

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UIThemeManager.OnThemeChanged -= UpdateColors;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
