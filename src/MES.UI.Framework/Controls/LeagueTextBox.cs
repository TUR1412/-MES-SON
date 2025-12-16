using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 英雄联盟风格文本输入框控件
    /// 严格遵循C# 5.0语法规范和WinForms设计器模式
    /// </summary>
    public class LeagueTextBox : TextBox
    {
        #region 私有字段

        private bool _isFocused = false;
        private bool _isHovered = false;
        private Color _borderColor = LeagueColors.BorderGold;
        private Color _focusBorderColor = LeagueColors.BorderGoldHover;
        private Color _backgroundColorCustom = LeagueColors.InputBackground;
        private int _borderWidth = 2;
        private bool _showSearchIcon = false;
        private bool _showClearButton = false;
        private Rectangle _clearButtonRect;

        #endregion

        #region 公共属性

        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set 
            { 
                _borderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 焦点时的边框颜色
        /// </summary>
        public Color FocusBorderColor
        {
            get { return _focusBorderColor; }
            set 
            { 
                _focusBorderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 自定义背景颜色
        /// </summary>
        public Color BackgroundColorCustom
        {
            get { return _backgroundColorCustom; }
            set 
            { 
                _backgroundColorCustom = value;
                this.BackColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 边框宽度
        /// </summary>
        public int BorderWidth
        {
            get { return _borderWidth; }
            set 
            { 
                _borderWidth = Math.Max(1, value);
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示搜索图标
        /// </summary>
        public bool ShowSearchIcon
        {
            get { return _showSearchIcon; }
            set 
            { 
                _showSearchIcon = value;
                UpdatePadding();
                this.Invalidate();
            }
        }

        /// <summary>
        /// 是否显示清除按钮
        /// </summary>
        public bool ShowClearButton
        {
            get { return _showClearButton; }
            set 
            { 
                _showClearButton = value;
                UpdatePadding();
                this.Invalidate();
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化LeagueTextBox
        /// </summary>
        public LeagueTextBox()
        {
            InitializeLeagueTextBox();
        }

        /// <summary>
        /// 初始化英雄联盟文本框
        /// </summary>
        private void InitializeLeagueTextBox()
        {
            // 设置基本属性
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
                         ControlStyles.UserPaint | 
                         ControlStyles.DoubleBuffer | 
                         ControlStyles.ResizeRedraw, true);

            this.BorderStyle = BorderStyle.None;
            this.BackColor = _backgroundColorCustom;
            this.ForeColor = LeagueColors.TextPrimary;
            this.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            
            // 设置内边距
            UpdatePadding();
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 获得焦点事件
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            _isFocused = true;
            this.Invalidate();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            _isFocused = false;
            this.Invalidate();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            // 检查是否点击了清除按钮
            if (_showClearButton && _clearButtonRect.Contains(e.Location))
            {
                this.Clear();
                this.Focus();
                return;
            }
            
            base.OnMouseClick(e);
        }

        /// <summary>
        /// 文字改变事件
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnTextChanged(e);
        }

        #endregion

        #region 绘制方法

        /// <summary>
        /// 自定义绘制方法
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            // 先绘制背景和边框
            DrawBackground(e.Graphics);
            DrawBorder(e.Graphics);
            
            // 绘制图标
            if (_showSearchIcon)
            {
                DrawSearchIcon(e.Graphics);
            }
            
            if (_showClearButton && !string.IsNullOrEmpty(this.Text))
            {
                DrawClearButton(e.Graphics);
            }
            
            // 调用基类绘制文字
            base.OnPaint(e);
        }

        /// <summary>
        /// 绘制背景
        /// </summary>
        private void DrawBackground(Graphics g)
        {
            using (var brush = new SolidBrush(_backgroundColorCustom))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        private void DrawBorder(Graphics g)
        {
            // 悬停时轻微高亮（避免 _isHovered 成为“无用字段”，同时增强交互反馈）
            Color currentBorderColor = _isFocused ? _focusBorderColor : (_isHovered ? _focusBorderColor : _borderColor);
            
            using (var pen = new Pen(currentBorderColor, _borderWidth))
            {
                Rectangle borderRect = this.ClientRectangle;
                borderRect.Width -= 1;
                borderRect.Height -= 1;
                g.DrawRectangle(pen, borderRect);
            }
            
            // 如果有焦点，绘制内发光效果
            if (_isFocused || _isHovered)
            {
                using (var glowPen = new Pen(LeagueColors.CreateGlow(_focusBorderColor), 1))
                {
                    Rectangle glowRect = this.ClientRectangle;
                    glowRect.Inflate(-1, -1);
                    g.DrawRectangle(glowPen, glowRect);
                }
            }
        }

        /// <summary>
        /// 绘制搜索图标
        /// </summary>
        private void DrawSearchIcon(Graphics g)
        {
            int iconSize = 16;
            int margin = 8;
            Rectangle iconRect = new Rectangle(margin, (this.Height - iconSize) / 2, iconSize, iconSize);
            
            using (var pen = new Pen(LeagueColors.TextSecondary, 2))
            {
                // 绘制放大镜圆圈
                g.DrawEllipse(pen, iconRect.X, iconRect.Y, iconSize - 6, iconSize - 6);
                
                // 绘制放大镜手柄
                g.DrawLine(pen, 
                    iconRect.X + iconSize - 6, iconRect.Y + iconSize - 6,
                    iconRect.X + iconSize - 2, iconRect.Y + iconSize - 2);
            }
        }

        /// <summary>
        /// 绘制清除按钮
        /// </summary>
        private void DrawClearButton(Graphics g)
        {
            int buttonSize = 16;
            int margin = 8;
            _clearButtonRect = new Rectangle(
                this.Width - buttonSize - margin, 
                (this.Height - buttonSize) / 2, 
                buttonSize, 
                buttonSize);
            
            // 绘制清除按钮背景
            using (var brush = new SolidBrush(Color.FromArgb(50, LeagueColors.TextSecondary)))
            {
                g.FillEllipse(brush, _clearButtonRect);
            }
            
            // 绘制X图标
            using (var pen = new Pen(LeagueColors.TextSecondary, 2))
            {
                int crossMargin = 4;
                g.DrawLine(pen,
                    _clearButtonRect.X + crossMargin, _clearButtonRect.Y + crossMargin,
                    _clearButtonRect.Right - crossMargin, _clearButtonRect.Bottom - crossMargin);
                g.DrawLine(pen,
                    _clearButtonRect.Right - crossMargin, _clearButtonRect.Y + crossMargin,
                    _clearButtonRect.X + crossMargin, _clearButtonRect.Bottom - crossMargin);
            }
        }

        /// <summary>
        /// 更新内边距
        /// </summary>
        private void UpdatePadding()
        {
            int leftPadding = _showSearchIcon ? 32 : 8;
            int rightPadding = _showClearButton ? 32 : 8;
            
            // 设置文字区域的内边距
            this.Padding = new Padding(leftPadding, 4, rightPadding, 4);
        }

        #endregion
    }
}
