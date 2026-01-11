// Nova 主题视觉渲染器，统一卡片、按钮与导航视觉效果。
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// Nova 主题视觉绘制工具
    /// </summary>
    public static class NovaVisuals
    {
        private static readonly Font IconFont = new Font("Segoe MDL2 Assets", 18f, FontStyle.Regular);

        public static void DrawCardButton(Graphics g, Rectangle bounds, bool hovered, bool pressed, string title, string description, string glyph, Color accent, Font titleFont, Font bodyFont)
        {
            if (g == null) return;
            var colors = UIThemeManager.Colors;
            var rect = bounds;
            rect.Width -= 1;
            rect.Height -= 1;

            var radius = DesignTokens.Radius.Md;
            using (var path = CreateRoundedRectPath(rect, radius))
            using (var brush = new LinearGradientBrush(rect, colors.Surface, colors.Background, 90f))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(brush, path);

                if (hovered || pressed)
                {
                    var glow = Color.FromArgb(pressed ? 90 : 60, accent);
                    using (var glowPen = new Pen(glow, pressed ? 2f : 1.4f))
                    {
                        g.DrawPath(glowPen, path);
                    }
                }

                using (var borderPen = new Pen(Color.FromArgb(90, colors.Border), 1))
                {
                    g.DrawPath(borderPen, path);
                }
            }

            var titleRect = new Rectangle(rect.Left + 18, rect.Top + 14, rect.Width - 36, 26);
            TextRenderer.DrawText(g, title ?? string.Empty, titleFont, titleRect, colors.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            var descRect = new Rectangle(rect.Left + 18, rect.Top + 44, rect.Width - 36, rect.Height - 60);
            TextRenderer.DrawText(g, description ?? string.Empty, bodyFont, descRect, colors.TextSecondary,
                TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak);

            if (!string.IsNullOrWhiteSpace(glyph))
            {
                var glyphRect = new Rectangle(rect.Right - 48, rect.Top + 12, 32, 32);
                TextRenderer.DrawText(g, glyph, IconFont, glyphRect, Color.FromArgb(160, accent),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        public static void DrawNavButton(Graphics g, Rectangle bounds, bool hovered, bool pressed, bool selected, string title, string subtitle, string glyph, Font titleFont, Font subtitleFont)
        {
            if (g == null) return;
            var colors = UIThemeManager.Colors;
            var rect = bounds;
            rect.Width -= 1;
            rect.Height -= 1;

            using (var brush = new SolidBrush(colors.Surface))
            {
                g.FillRectangle(brush, rect);
            }

            if (selected || hovered)
            {
                var accent = Color.FromArgb(selected ? 90 : 60, colors.Primary);
                using (var accentBrush = new SolidBrush(accent))
                {
                    g.FillRectangle(accentBrush, rect);
                }
            }

            using (var pen = new Pen(Color.FromArgb(80, colors.Border), 1))
            {
                g.DrawRectangle(pen, rect);
            }

            var iconRect = new Rectangle(rect.Left + 12, rect.Top + 12, 28, 28);
            if (!string.IsNullOrWhiteSpace(glyph))
            {
                TextRenderer.DrawText(g, glyph, IconFont, iconRect, colors.Primary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            var titleRect = new Rectangle(rect.Left + 46, rect.Top + 8, rect.Width - 56, 22);
            TextRenderer.DrawText(g, title ?? string.Empty, titleFont, titleRect, colors.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            var subtitleRect = new Rectangle(rect.Left + 46, rect.Top + 28, rect.Width - 56, 20);
            TextRenderer.DrawText(g, subtitle ?? string.Empty, subtitleFont, subtitleRect, colors.TextSecondary,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        public static void DrawActionButton(Graphics g, Rectangle bounds, bool hovered, bool pressed, string text, Font font)
        {
            if (g == null) return;
            var colors = UIThemeManager.Colors;
            var rect = bounds;
            rect.Width -= 1;
            rect.Height -= 1;

            var radius = DesignTokens.Radius.Sm;
            using (var path = CreateRoundedRectPath(rect, radius))
            {
                var start = pressed ? colors.Hover : colors.Surface;
                var end = pressed ? colors.Surface : colors.Background;
                using (var brush = new LinearGradientBrush(rect, start, end, 90f))
                {
                    g.FillPath(brush, path);
                }

                var borderColor = hovered ? Color.FromArgb(160, colors.Primary) : Color.FromArgb(90, colors.Border);
                using (var pen = new Pen(borderColor, 1))
                {
                    g.DrawPath(pen, path);
                }
            }

            TextRenderer.DrawText(g, text ?? string.Empty, font, rect, colors.Text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        public static void DrawProgressBar(Graphics g, Rectangle bounds, float progress)
        {
            if (g == null) return;
            var colors = UIThemeManager.Colors;
            var rect = bounds;
            rect.Width -= 1;
            rect.Height -= 1;

            var radius = Math.Min(DesignTokens.Radius.Sm, rect.Height / 2);
            using (var path = CreateRoundedRectPath(rect, radius))
            using (var trackBrush = new SolidBrush(Color.FromArgb(180, colors.Surface)))
            {
                g.FillPath(trackBrush, path);

                var fillWidth = Math.Max(1, (int)Math.Round(rect.Width * Math.Max(0f, Math.Min(1f, progress))));
                var fillRect = new Rectangle(rect.Left, rect.Top, fillWidth, rect.Height);
                using (var fillBrush = new LinearGradientBrush(fillRect, colors.Primary, colors.Secondary, LinearGradientMode.Horizontal))
                {
                    var oldClip = g.Clip;
                    g.SetClip(path, CombineMode.Intersect);
                    g.FillRectangle(fillBrush, fillRect);
                    g.Clip = oldClip;
                }

                using (var pen = new Pen(Color.FromArgb(100, colors.Border), 1))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            int diameter = radius * 2;
            var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

            path.AddArc(arcRect, 180, 90);
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}

