using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// LoL 客户端风格“动作按钮”（小按钮）
    /// 目标：克制的暗金边框 + 悬停/按下反馈；用于状态区/工具区的快捷入口。
    /// </summary>
    public class LolActionButton : Button
    {
        private bool _isHovered;
        private bool _isPressed;
        private ControlAnimationState _animState;

        public LolActionButton()
        {
            InitializeLolActionButton();
        }

        [Category("LOL")]
        [Description("是否显示为紧凑模式（更窄、更像工具区按钮）")]
        [DefaultValue(false)]
        public bool Compact { get; set; }

        private void InitializeLolActionButton()
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
            Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            Cursor = Cursors.Hand;

            Height = 34;
            Width = 120;

            TabStop = true;

            try
            {
                _animState = LeagueAnimationManager.Instance.GetControlState(this);
            }
            catch
            {
                _animState = null;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            try
            {
                var state = _animState;
                if (state == null)
                {
                    try { state = LeagueAnimationManager.Instance.GetControlState(this); } catch { state = null; }
                }

                float hover = (Enabled && _isHovered) ? 1f : 0f;
                float press = (Enabled && _isPressed) ? 1f : 0f;
                if (state != null)
                {
                    hover = state.HoverProgress;
                    press = state.PressProgress;
                }

                // 更像 LoL：使用真实 RiotButton 渲染（平滑 hover/click）
                RealLeagueRenderer.DrawRealLeagueButton(pevent.Graphics, ClientRectangle, hover, press, Text, Font);
            }
            catch
            {
                // ignore
            }
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

        protected override void OnTextChanged(EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            // 支持 Compact 模式：宽度更紧凑
            if (Compact && Width > 110)
            {
                Width = 110;
            }
            Invalidate();
            base.OnSizeChanged(e);
        }
    }
}

