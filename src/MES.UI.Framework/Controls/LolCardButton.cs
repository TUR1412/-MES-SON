using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// LoL 客户端风格卡片按钮（大厅入口）
    /// 目标：Bento 卡片 + 暗色底 + 金色描边 + 悬停微发光
    /// </summary>
    public class LolCardButton : Button
    {
        private bool _isHovered;
        private bool _isPressed;
        private string _description;
        private string _iconGlyph;
        private Color _accentColor = LeagueColors.RiotGold;

        private Font _titleFont;
        private Font _bodyFont;

        public LolCardButton()
        {
            InitializeLolCardButton();
        }

        [Category("LOL")]
        [Description("卡片描述（可多行）")]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                Invalidate();
            }
        }

        [Category("LOL")]
        [Description("右上角图标（Segoe MDL2 Assets 字符）")]
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
        [Description("强调色（默认金色）")]
        public Color AccentColor
        {
            get { return _accentColor; }
            set
            {
                _accentColor = value;
                Invalidate();
            }
        }

        private void InitializeLolCardButton()
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

            // 更适合作为“入口卡片”
            Height = 170;
            Width = 280;

            _titleFont = new Font("微软雅黑", 12F, FontStyle.Bold);
            _bodyFont = new Font("微软雅黑", 9F, FontStyle.Regular);

            TabStop = true;
            TextAlign = ContentAlignment.TopLeft; // 实际绘制由 OnPaint 控制，但保持语义一致
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            try
            {
                LolClientVisuals.DrawCardButton(
                    pevent.Graphics,
                    ClientRectangle,
                    _isHovered,
                    _isPressed,
                    Text,
                    _description,
                    _iconGlyph,
                    _accentColor,
                    _titleFont,
                    _bodyFont);
            }
            catch
            {
                // 兜底：至少画出一个可点击的暗底卡片，避免“整块空白”
                using (var bg = new SolidBrush(LeagueColors.DarkSurface))
                {
                    pevent.Graphics.FillRectangle(bg, ClientRectangle);
                }
                using (var pen = new Pen(Color.FromArgb(90, LeagueColors.RiotBorderGold), 1))
                {
                    var r = ClientRectangle;
                    r.Width -= 1;
                    r.Height -= 1;
                    pevent.Graphics.DrawRectangle(pen, r);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // 背景由 OnPaint 统一绘制，避免闪烁
        }

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
            if (mevent.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (_isPressed)
            {
                _isPressed = false;
                Invalidate();
            }
            base.OnMouseUp(mevent);
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
                if (_bodyFont != null)
                {
                    _bodyFont.Dispose();
                    _bodyFont = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
