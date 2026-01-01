using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// LoL 客户端风格导航按钮（V2）
    /// 目标：左侧栏导航条，暗色底 + 金色描边 + 选中高亮
    /// </summary>
    public class LolNavButton : Button
    {
        private bool _isHovered;
        private bool _isPressed;
        private bool _isSelected;
        private string _subtitle;
        private string _iconGlyph;
        private ControlAnimationState _animState;

        private Font _titleFont;
        private Font _subtitleFont;

        public LolNavButton()
        {
            InitializeLolNavButton();
        }

        [Category("LOL")]
        [Description("副标题（用于说明功能范围）")]
        public string Subtitle
        {
            get { return _subtitle; }
            set
            {
                _subtitle = value;
                Invalidate();
            }
        }

        [Category("LOL")]
        [Description("Segoe MDL2 Assets 图标字符，例如 \\uE8A7")]
        public string IconGlyph
        {
            get { return _iconGlyph; }
            set
            {
                _iconGlyph = value;
                Invalidate();
            }
        }

        [Category("LOL")]
        [Description("是否处于选中状态")]
        [DefaultValue(false)]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    Invalidate();
                }
            }
        }

        private void InitializeLolNavButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.Transparent;
            ForeColor = LeagueColors.TextPrimary;
            Cursor = Cursors.Hand;

            // 默认尺寸更适合左侧栏
            Height = 56;
            Width = 260;

            // 字体：主标题略粗，副标题更小
            _titleFont = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            _subtitleFont = new Font("微软雅黑", 8.0F, FontStyle.Regular);

            // 让 Tab 导航可用（键盘 Enter/Space 触发）
            TabStop = true;

            try
            {
                // 注册到动画系统：实现“丝滑”的 hover/click 过渡
                _animState = LeagueAnimationManager.Instance.GetControlState(this);
            }
            catch
            {
                _animState = null;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var state = _animState;
            if (state == null)
            {
                try { state = LeagueAnimationManager.Instance.GetControlState(this); } catch { state = null; }
            }

            float hover = (_isHovered && Enabled) ? 1f : 0f;
            float press = (_isPressed && Enabled) ? 1f : 0f;

            if (state != null)
            {
                hover = state.HoverProgress;
                press = state.PressProgress;
            }

            // 键盘焦点也给到轻微“可交互”反馈，避免 Tab 导航时看不出落点
            if (Focused && hover < 0.25f)
            {
                hover = 0.25f;
            }

            LolClientVisuals.DrawNavButton(
                pevent.Graphics,
                ClientRectangle,
                hover,
                press,
                _isSelected,
                Text,
                _subtitle,
                _iconGlyph,
                _titleFont,
                _subtitleFont);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // 背景由 OnPaint 统一绘制，避免闪烁
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            try
            {
                if (_animState != null) _animState.SetHover(true);
            }
            catch
            {
                // ignore
            }
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            _isPressed = false;
            try
            {
                if (_animState != null)
                {
                    _animState.SetHover(false);
                    _animState.SetPressed(false);
                }
            }
            catch
            {
                // ignore
            }
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _isPressed = true;
                try
                {
                    if (_animState != null) _animState.SetPressed(true);
                }
                catch
                {
                    // ignore
                }
                Invalidate();
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (_isPressed)
            {
                _isPressed = false;
                try
                {
                    if (_animState != null) _animState.SetPressed(false);
                }
                catch
                {
                    // ignore
                }
                Invalidate();
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            try
            {
                if (!Enabled && _animState != null)
                {
                    _animState.SetHover(false);
                    _animState.SetPressed(false);
                }
            }
            catch
            {
                // ignore
            }

            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_titleFont != null)
                {
                    _titleFont.Dispose();
                    _titleFont = null;
                }
                if (_subtitleFont != null)
                {
                    _subtitleFont.Dispose();
                    _subtitleFont = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}

