using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// LoL 客户端风进度条（暗底 + 暗金描边 + 渐变填充）
    /// 说明：WinForms 原生 ProgressBar 难以完全自定义外观，这里使用自绘控件以统一主题质感。
    /// </summary>
    public sealed class LolProgressBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;

        public LolProgressBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            ForeColor = LeagueColors.TextHighlight;
            Size = new Size(240, 12);
        }

        [Category("行为")]
        [Description("进度条最小值")]
        [DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                if (_maximum < _minimum) _maximum = _minimum;
                if (_value < _minimum) _value = _minimum;
                Invalidate();
            }
        }

        [Category("行为")]
        [Description("进度条最大值")]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = Math.Max(value, _minimum);
                if (_value > _maximum) _value = _maximum;
                Invalidate();
            }
        }

        [Category("行为")]
        [Description("进度条当前值")]
        [DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set
            {
                _value = Math.Min(Math.Max(value, _minimum), _maximum);
                Invalidate();
            }
        }

        [Browsable(false)]
        public float Progress
        {
            get
            {
                var range = _maximum - _minimum;
                if (range <= 0) return 0f;
                return (float)(_value - _minimum) / (float)range;
            }
            set
            {
                var p = value;
                if (p < 0f) p = 0f;
                if (p > 1f) p = 1f;
                Value = _minimum + (int)Math.Round((_maximum - _minimum) * p, MidpointRounding.AwayFromZero);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.Clear(Color.Transparent);

            var rect = ClientRectangle;
            if (rect.Width <= 2 || rect.Height <= 2) return;

            rect.Width -= 1;
            rect.Height -= 1;

            var chamfer = Math.Min(6, Math.Max(2, rect.Height / 3));

            var oldSmoothing = g.SmoothingMode;
            try
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            catch
            {
                // ignore
            }

            using (var path = CreateChamferedRectPath(rect, chamfer))
            {
                // Track
                using (var trackBrush = new SolidBrush(Color.FromArgb(180, LeagueColors.InputBackground)))
                {
                    g.FillPath(trackBrush, path);
                }

                // Fill
                var progress = Progress;
                if (progress > 0f)
                {
                    var fillWidth = (int)Math.Round(rect.Width * progress, MidpointRounding.AwayFromZero);
                    if (fillWidth < 1) fillWidth = 1;
                    if (fillWidth > rect.Width) fillWidth = rect.Width;

                    var fillRect = new Rectangle(rect.Left, rect.Top, fillWidth, rect.Height);

                    var oldClip = g.Clip;
                    try
                    {
                        g.SetClip(path, CombineMode.Intersect);

                        using (var fillBrush = new LinearGradientBrush(
                            fillRect,
                            Color.FromArgb(255, LeagueColors.RiotGold),
                            Color.FromArgb(255, LeagueColors.RiotGoldHover),
                            LinearGradientMode.Horizontal))
                        {
                            g.FillRectangle(fillBrush, fillRect);
                        }

                        // 顶部微高光线（更像客户端材质）
                        using (var hi = new Pen(Color.FromArgb(60, LeagueColors.TextHighlight), 1))
                        {
                            g.DrawLine(hi, rect.Left + chamfer, rect.Top + 1, rect.Right - chamfer, rect.Top + 1);
                        }
                    }
                    finally
                    {
                        try { g.Clip = oldClip; } catch { }
                    }
                }

                // Border
                using (var borderPen = new Pen(Color.FromArgb(140, LeagueColors.RiotBorderGold), 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            try
            {
                g.SmoothingMode = oldSmoothing;
            }
            catch
            {
                // ignore
            }
        }

        private static GraphicsPath CreateChamferedRectPath(Rectangle rect, int chamfer)
        {
            var c = Math.Max(0, chamfer);
            if (c == 0)
            {
                var p = new GraphicsPath();
                p.AddRectangle(rect);
                return p;
            }

            // 防止在极窄宽度下几何异常
            if (c * 2 > rect.Width) c = Math.Max(0, rect.Width / 2);
            if (c * 2 > rect.Height) c = Math.Max(0, rect.Height / 2);

            var path = new GraphicsPath();
            path.StartFigure();

            path.AddLine(rect.Left + c, rect.Top, rect.Right - c, rect.Top);
            path.AddLine(rect.Right - c, rect.Top, rect.Right, rect.Top + c);
            path.AddLine(rect.Right, rect.Top + c, rect.Right, rect.Bottom - c);
            path.AddLine(rect.Right, rect.Bottom - c, rect.Right - c, rect.Bottom);
            path.AddLine(rect.Right - c, rect.Bottom, rect.Left + c, rect.Bottom);
            path.AddLine(rect.Left + c, rect.Bottom, rect.Left, rect.Bottom - c);
            path.AddLine(rect.Left, rect.Bottom - c, rect.Left, rect.Top + c);
            path.AddLine(rect.Left, rect.Top + c, rect.Left + c, rect.Top);

            path.CloseFigure();
            return path;
        }
    }
}

