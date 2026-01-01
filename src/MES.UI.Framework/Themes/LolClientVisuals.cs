using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// LoL 客户端风格视觉绘制（V2）
    /// 目标：暗色底 + 金色描边 + 微噪点纹理 + 低侵入动效（悬停/按下）
    /// 严格遵循 C# 5.0 语法规范
    /// </summary>
    public static class LolClientVisuals
    {
        private static readonly object NoiseLock = new object();
        private static Bitmap _noiseBitmap;

        /// <summary>
        /// 绘制全局背景：渐变 + 径向高光 + 噪点 + 六边形纹理
        /// </summary>
        public static void DrawClientBackground(Graphics g, Rectangle bounds)
        {
            if (g == null) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1) 基础渐变底色
            using (var brush = CreateBackgroundBrush(bounds))
            {
                g.FillRectangle(brush, bounds);
            }

            // 2) 径向高光（避免“死黑”）
            DrawRadialGlow(g, bounds);

            // 3) 六边形纹理（很淡）
            DrawHexGrid(g, bounds);

            // 4) 噪点纹理（微弱，提升质感）
            DrawNoise(g, bounds);
        }

        /// <summary>
        /// 绘制全局背景（支持背景图）：背景图 Cover + 暗色遮罩 + LoL 纹理层
        /// 说明：不内置任何版权素材；背景图由调用方自行提供（本地文件/自有素材）。
        /// </summary>
        public static void DrawClientBackground(Graphics g, Rectangle bounds, Image currentBackdrop, float currentOpacity, Image nextBackdrop, float nextOpacity)
        {
            if (g == null) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 0) 背景图（Cover，避免尺寸不一导致拉伸变形）
            if (currentBackdrop != null && currentOpacity > 0.001f)
            {
                DrawCoverImage(g, bounds, currentBackdrop, Clamp01(currentOpacity));
            }
            if (nextBackdrop != null && nextOpacity > 0.001f)
            {
                DrawCoverImage(g, bounds, nextBackdrop, Clamp01(nextOpacity));
            }

            // 1) 暗色遮罩（保证前景 UI 可读）
            using (var scrim = CreateBackgroundScrimBrush(bounds, 190))
            {
                g.FillRectangle(scrim, bounds);
            }

            // 2) 径向高光（避免“死黑”）
            DrawRadialGlow(g, bounds);

            // 3) 六边形纹理（很淡）
            DrawHexGrid(g, bounds);

            // 4) 噪点纹理（微弱，提升质感）
            DrawNoise(g, bounds);
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

        private static Color WithAlpha(Color color, int alpha)
        {
            if (alpha < 0) alpha = 0;
            if (alpha > 255) alpha = 255;
            return Color.FromArgb(alpha, color);
        }

        private static Color LerpColor(Color a, Color b, float t)
        {
            t = Clamp01(t);
            int aa = a.A + (int)Math.Round((b.A - a.A) * t);
            int rr = a.R + (int)Math.Round((b.R - a.R) * t);
            int gg = a.G + (int)Math.Round((b.G - a.G) * t);
            int bb = a.B + (int)Math.Round((b.B - a.B) * t);

            if (aa < 0) aa = 0; if (aa > 255) aa = 255;
            if (rr < 0) rr = 0; if (rr > 255) rr = 255;
            if (gg < 0) gg = 0; if (gg > 255) gg = 255;
            if (bb < 0) bb = 0; if (bb > 255) bb = 255;

            return Color.FromArgb(aa, rr, gg, bb);
        }

        /// <summary>
        /// 绘制侧边栏背景：更深的渐变 + 右侧描边
        /// </summary>
        public static void DrawSidebarBackground(Graphics g, Rectangle bounds)
        {
            if (g == null) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new LinearGradientBrush(
                bounds,
                LeagueColors.DarkestBackground,
                LeagueColors.DarkBackground,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, bounds);
            }

            // 右侧分隔线
            using (var pen = new Pen(Color.FromArgb(90, LeagueColors.RiotBorderGold), 1))
            {
                g.DrawLine(pen, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Bottom);
            }
        }

        /// <summary>
        /// 绘制导航按钮
        /// </summary>
        public static void DrawNavButton(
            Graphics g,
            Rectangle bounds,
            bool isHovered,
            bool isPressed,
            bool isSelected,
            string title,
            string subtitle,
            string iconGlyph,
            Font titleFont,
            Font subtitleFont)
        {
            DrawNavButton(
                g,
                bounds,
                isHovered ? 1f : 0f,
                isPressed ? 1f : 0f,
                isSelected,
                title,
                subtitle,
                iconGlyph,
                titleFont,
                subtitleFont);
        }

        /// <summary>
        /// 绘制导航按钮（支持 hover/press 的平滑过渡）
        /// </summary>
        public static void DrawNavButton(
            Graphics g,
            Rectangle bounds,
            float hoverProgress,
            float pressProgress,
            bool isSelected,
            string title,
            string subtitle,
            string iconGlyph,
            Font titleFont,
            Font subtitleFont)
        {
            if (g == null) return;

            float h = EaseOutCubic(Clamp01(hoverProgress));
            float p = EaseOutCubic(Clamp01(pressProgress));

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var drawRect = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            if (p > 0.001f)
            {
                var off = (int)Math.Round(1f * p);
                if (off != 0) drawRect.Offset(off, off);
            }

            // 背景（每次都填充：按钮禁用了 OnPaintBackground，否则会出现残影/文字叠影）
            var normalTop = LeagueColors.DarkestBackground;
            var normalBottom = LeagueColors.DarkBackground;
            var hoverTop = LeagueColors.DarkSurface;
            var hoverBottom = LeagueColors.DarkPanel;

            var selectedTop = LeagueColors.DarkSurfaceLight;
            var selectedBottom = LeagueColors.DarkPanel;

            var baseTop = isSelected ? selectedTop : normalTop;
            var baseBottom = isSelected ? selectedBottom : normalBottom;

            var targetTop = isSelected ? selectedTop : hoverTop;
            var targetBottom = isSelected ? selectedBottom : hoverBottom;

            var bgTop = LerpColor(baseTop, targetTop, h);
            var bgBottom = LerpColor(baseBottom, targetBottom, h);

            // Glassmorphism：轻微透明，让全局背景纹理透出来（桌面端也能做出“玻璃感”）
            var baseAlpha = isSelected ? 230 : 210;
            var alpha = (int)Math.Round(baseAlpha + 20 * h);
            bgTop = WithAlpha(bgTop, alpha);
            bgBottom = WithAlpha(bgBottom, alpha);

            using (var bgBrush = new LinearGradientBrush(drawRect, bgTop, bgBottom, LinearGradientMode.Vertical))
            {
                g.FillRectangle(bgBrush, drawRect);
            }

            // 左侧选中条 / Hover 提示条
            if (isSelected)
            {
                using (var brush = new SolidBrush(LeagueColors.RiotGold))
                {
                    g.FillRectangle(brush, drawRect.X, drawRect.Y, 4, drawRect.Height);
                }
            }
            else if (h > 0.01f)
            {
                using (var brush = new SolidBrush(WithAlpha(LeagueColors.RiotGoldHover, (int)Math.Round(70 * h))))
                {
                    g.FillRectangle(brush, drawRect.X, drawRect.Y, 2, drawRect.Height);
                }
            }

            // Border（悬停更亮/更“可点击”）
            var borderBase = WithAlpha(LeagueColors.RiotBorderGold, 120);
            var borderHover = WithAlpha(LeagueColors.RiotGoldHover, 220);
            var borderColor = LerpColor(borderBase, borderHover, h);
            using (var pen = new Pen(borderColor, 1))
            {
                g.DrawRectangle(pen, drawRect);
            }

            // 轻微外发光（微交互）
            if (h > 0.01f)
            {
                var glowAlpha = (int)Math.Round(50 + 80 * h);
                using (var glowPen = new Pen(WithAlpha(LeagueColors.RiotGoldHover, glowAlpha), 1))
                {
                    var glowRect = drawRect;
                    glowRect.Inflate(1, 1);
                    g.DrawRectangle(glowPen, glowRect);
                }
            }

            // 底部分隔线（更淡）
            using (var pen = new Pen(Color.FromArgb(40, LeagueColors.SeparatorColor), 1))
            {
                g.DrawLine(pen, drawRect.Left + 6, drawRect.Bottom, drawRect.Right - 6, drawRect.Bottom);
            }

            // 布局：icon | title/subtitle
            var leftPadding = 12;
            var iconArea = new Rectangle(drawRect.Left + leftPadding, drawRect.Top, 28, drawRect.Height);
            var textArea = new Rectangle(iconArea.Right + 8, drawRect.Top + 8, drawRect.Width - (iconArea.Right + 18 - drawRect.Left), drawRect.Height - 16);

            // Icon（使用 Segoe MDL2 Assets 字体绘制，避免外部资源依赖）
            if (!string.IsNullOrWhiteSpace(iconGlyph))
            {
                var iconBase = isSelected ? LeagueColors.RiotGoldHover : LeagueColors.TextPrimary;
                var iconColor = LerpColor(iconBase, LeagueColors.TextHighlight, h);

                Font iconFont = null;
                try
                {
                    iconFont = new Font("Segoe MDL2 Assets", 15F, FontStyle.Regular);
                }
                catch
                {
                    // 某些精简系统可能缺少该字体；此时跳过图标绘制即可
                }

                if (iconFont != null)
                {
                    try
                    {
                        using (iconFont)
                        using (var iconBrush = new SolidBrush(iconColor))
                        {
                            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                            g.DrawString(iconGlyph, iconFont, iconBrush, iconArea, sf);
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }

            // Title
            var titleBase = isSelected ? LeagueColors.TextHighlight : LeagueColors.TextPrimary;
            var titleColor = LerpColor(titleBase, LeagueColors.TextHighlight, h);
            var subtitleColor = LerpColor(LeagueColors.TextSecondary, WithAlpha(LeagueColors.TextHighlight, 200), h * 0.6f);

            using (var titleBrush = new SolidBrush(titleColor))
            using (var subtitleBrush = new SolidBrush(subtitleColor))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
                g.DrawString(title ?? string.Empty, titleFont, titleBrush, textArea, sf);

                if (!string.IsNullOrWhiteSpace(subtitle))
                {
                    var subtitleRect = new Rectangle(textArea.Left, textArea.Top + (int)titleFont.GetHeight(g) + 3, textArea.Width, textArea.Height);
                    g.DrawString(subtitle, subtitleFont, subtitleBrush, subtitleRect, sf);
                }
            }
        }

        /// <summary>
        /// 绘制卡片按钮（大厅入口）
        /// </summary>
        public static void DrawCardButton(
            Graphics g,
            Rectangle bounds,
            bool isHovered,
            bool isPressed,
            string title,
            string description,
            string iconGlyph,
            Color accentColor,
            Font titleFont,
            Font bodyFont)
        {
            DrawCardButton(
                g,
                bounds,
                isHovered ? 1f : 0f,
                isPressed ? 1f : 0f,
                title,
                description,
                iconGlyph,
                accentColor,
                titleFont,
                bodyFont);
        }

        /// <summary>
        /// 绘制卡片按钮（大厅入口，支持 hover/press 平滑过渡与“玻璃质感”）
        /// </summary>
        public static void DrawCardButton(
            Graphics g,
            Rectangle bounds,
            float hoverProgress,
            float pressProgress,
            string title,
            string description,
            string iconGlyph,
            Color accentColor,
            Font titleFont,
            Font bodyFont)
        {
            if (g == null) return;

            float h = EaseOutCubic(Clamp01(hoverProgress));
            float p = EaseOutCubic(Clamp01(pressProgress));

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var drawRect = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);

            // Hover：轻微抬升；Press：轻微下沉
            var lift = (int)Math.Round(-2f * h);
            var pressOffset = (int)Math.Round(1f * p);
            if (lift != 0 || pressOffset != 0)
            {
                drawRect.Offset(0, lift + pressOffset);
            }

            var radius = Math.Min(14, Math.Max(10, drawRect.Height / 8));

            // Shadow（更像“悬浮卡片”）
            DrawCardShadow(g, drawRect, radius, h);

            using (var cardPath = CreateRoundedRectPath(drawRect, radius))
            {
                // 背景渐变（Glassmorphism：半透明，让底层 LoL 背景纹理透出来）
                var bgA = (int)Math.Round(170 + 25 * h);
                var bgTop = WithAlpha(LeagueColors.DarkSurface, bgA);
                var bgBottom = WithAlpha(LeagueColors.DarkPanel, bgA);

                using (var bgBrush = new LinearGradientBrush(drawRect, bgTop, bgBottom, LinearGradientMode.Vertical))
                {
                    g.FillPath(bgBrush, cardPath);
                }

                // 角落暗角与轻微高光（更像客户端）
                var overlayA = (int)Math.Round(26 + 22 * h);
                using (var overlay = new LinearGradientBrush(drawRect,
                    WithAlpha(accentColor, overlayA),
                    WithAlpha(accentColor, 0),
                    LinearGradientMode.ForwardDiagonal))
                {
                    g.FillPath(overlay, cardPath);
                }

                // 内高光（Neomorphism：左上轻高光）
                using (var hiPen = new Pen(WithAlpha(Color.White, (int)Math.Round(28 + 24 * h)), 1))
                {
                    var hiRect = drawRect;
                    hiRect.Inflate(-2, -2);
                    using (var hiPath = CreateRoundedRectPath(hiRect, Math.Max(0, radius - 2)))
                    {
                        g.DrawPath(hiPen, hiPath);
                    }
                }

                // Border（渐变 + hover 增强）
                using (var borderBrush = CreateBorderBrush(drawRect, accentColor))
                using (var borderPen = new Pen(borderBrush, 1))
                {
                    g.DrawPath(borderPen, cardPath);
                }

                // 悬停外发光（柔和而不刺眼）
                if (h > 0.01f)
                {
                    var glowAlpha = (int)Math.Round(40 + 110 * h);
                    using (var glowPen = new Pen(WithAlpha(accentColor, glowAlpha), 2))
                    {
                        var glowRect = drawRect;
                        glowRect.Inflate(1, 1);
                        using (var glowPath = CreateRoundedRectPath(glowRect, radius + 1))
                        {
                            g.DrawPath(glowPen, glowPath);
                        }
                    }
                }

                // 顶部强调线
                using (var brush = new SolidBrush(WithAlpha(accentColor, (int)Math.Round(120 + 80 * h))))
                {
                    var lineRect = new Rectangle(drawRect.Left + 10, drawRect.Top + 10, Math.Max(1, drawRect.Width - 20), 2);
                    g.FillRectangle(brush, lineRect);
                }

                // 内容布局
                var padding = 16;
                var contentRect = new Rectangle(drawRect.Left + padding, drawRect.Top + padding, drawRect.Width - padding * 2, drawRect.Height - padding * 2);
                var titleRect = new Rectangle(contentRect.Left, contentRect.Top, contentRect.Width, 32);
                var descRect = new Rectangle(contentRect.Left, contentRect.Top + 36, contentRect.Width, contentRect.Height - 72);

                // 底部 CTA 区
                var ctaRect = new Rectangle(contentRect.Left, drawRect.Bottom - 40, contentRect.Width, 22);

                // 右上角 icon
                if (!string.IsNullOrWhiteSpace(iconGlyph))
                {
                    var iconRect = new Rectangle(drawRect.Right - 36, drawRect.Top + 12, 24, 24);
                    Font iconFont = null;
                    try
                    {
                        iconFont = new Font("Segoe MDL2 Assets", 18F, FontStyle.Regular);
                    }
                    catch
                    {
                        // ignore
                    }

                    if (iconFont != null)
                    {
                        try
                        {
                            using (iconFont)
                            using (var iconBrush = new SolidBrush(WithAlpha(accentColor, (int)Math.Round(200 + 35 * h))))
                            {
                                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                                g.DrawString(iconGlyph, iconFont, iconBrush, iconRect, sf);
                            }
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                }

                // 右下角水印 icon（更像客户端的装饰层）
                if (!string.IsNullOrWhiteSpace(iconGlyph))
                {
                    Font watermarkFont = null;
                    try
                    {
                        watermarkFont = new Font("Segoe MDL2 Assets", 64F, FontStyle.Regular);
                    }
                    catch
                    {
                        // ignore
                    }

                    if (watermarkFont != null)
                    {
                        try
                        {
                            using (watermarkFont)
                            using (var watermarkBrush = new SolidBrush(WithAlpha(accentColor, (int)Math.Round(16 + 10 * h))))
                            {
                                var wmRect = new Rectangle(drawRect.Right - 110, drawRect.Bottom - 110, 100, 100);
                                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                                g.DrawString(iconGlyph, watermarkFont, watermarkBrush, wmRect, sf);
                            }
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                }

                // Title + Description
                var titleColor = LerpColor(LeagueColors.TextHighlight, WithAlpha(Color.White, 255), h * 0.15f);
                var bodyColor = LerpColor(LeagueColors.TextSecondary, WithAlpha(LeagueColors.TextHighlight, 215), h * 0.35f);

                using (var titleBrush = new SolidBrush(titleColor))
                using (var bodyBrush = new SolidBrush(bodyColor))
                {
                    var sfTitle = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
                    if (titleFont != null)
                    {
                        g.DrawString(title ?? string.Empty, titleFont, titleBrush, titleRect, sfTitle);
                    }

                    var sfBody = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        if (bodyFont != null)
                        {
                            g.DrawString(description, bodyFont, bodyBrush, descRect, sfBody);
                        }
                    }
                }

                // CTA（右下角“进入”）
                var ctaText = h > 0.55f ? "进入  →" : "进入";
                using (var ctaBrush = new SolidBrush(WithAlpha(accentColor, (int)Math.Round(200 + 35 * h))))
                using (var ctaFont = new Font("微软雅黑", 9.5F, FontStyle.Bold))
                {
                    var sfCta = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
                    g.DrawString(ctaText, ctaFont, ctaBrush, ctaRect, sfCta);
                }
            }
        }

        private static void DrawCardShadow(Graphics g, Rectangle rect, int radius, float hover)
        {
            if (g == null) return;
            if (rect.Width <= 2 || rect.Height <= 2) return;

            // hover 越强，阴影越“抬起来”
            var depth = 6 + (int)Math.Round(4 * hover);
            var spread = 6 + (int)Math.Round(8 * hover);
            var baseAlpha = 22 + (int)Math.Round(28 * hover);

            for (int i = 0; i < 4; i++)
            {
                var a = baseAlpha - i * 5;
                if (a <= 0) continue;

                var shadowRect = rect;
                shadowRect.Offset(0, depth);
                shadowRect.Inflate(spread + i, spread + i);

                using (var path = CreateRoundedRectPath(shadowRect, radius + spread + i))
                using (var brush = new SolidBrush(Color.FromArgb(a, 0, 0, 0)))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
        {
            var r = Math.Max(0, radius);
            if (r == 0)
            {
                var p = new GraphicsPath();
                p.AddRectangle(rect);
                return p;
            }

            int d = r * 2;
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;

            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static LinearGradientBrush CreateBackgroundBrush(Rectangle bounds)
        {
            var brush = new LinearGradientBrush(bounds, LeagueColors.DarkestBackground, LeagueColors.DarkBackground, LinearGradientMode.Vertical);

            // 三段渐变，让层次更“LoL 客户端”
            var blend = new ColorBlend();
            blend.Colors = new[]
            {
                LeagueColors.DarkestBackground,
                LeagueColors.DarkBackground,
                LeagueColors.DarkPanel
            };
            blend.Positions = new[] { 0.0f, 0.55f, 1.0f };
            brush.InterpolationColors = blend;

            return brush;
        }

        private static LinearGradientBrush CreateBackgroundScrimBrush(Rectangle bounds, int alpha)
        {
            int a = alpha;
            if (a < 0) a = 0;
            if (a > 255) a = 255;

            var c0 = Color.FromArgb(a, LeagueColors.DarkestBackground);
            var c1 = Color.FromArgb(a, LeagueColors.DarkBackground);
            var c2 = Color.FromArgb(a, LeagueColors.DarkPanel);

            var brush = new LinearGradientBrush(bounds, c0, c2, LinearGradientMode.Vertical);
            var blend = new ColorBlend();
            blend.Colors = new[] { c0, c1, c2 };
            blend.Positions = new[] { 0.0f, 0.55f, 1.0f };
            brush.InterpolationColors = blend;
            return brush;
        }

        private static void DrawCoverImage(Graphics g, Rectangle bounds, Image image, float opacity)
        {
            if (g == null || image == null) return;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;
            if (image.Width <= 0 || image.Height <= 0) return;

            float scaleX = bounds.Width / (float)image.Width;
            float scaleY = bounds.Height / (float)image.Height;
            float scale = Math.Max(scaleX, scaleY);

            int drawW = (int)Math.Ceiling(image.Width * scale);
            int drawH = (int)Math.Ceiling(image.Height * scale);

            int drawX = bounds.Left + (bounds.Width - drawW) / 2;
            int drawY = bounds.Top + (bounds.Height - drawH) / 2;

            var dest = new Rectangle(drawX, drawY, drawW, drawH);

            var oldMode = g.InterpolationMode;
            try
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
            catch
            {
                // ignore
            }

            try
            {
                using (var ia = new ImageAttributes())
                {
                    var cm = new ColorMatrix();
                    cm.Matrix33 = Clamp01(opacity);
                    ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    g.DrawImage(image, dest, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                g.InterpolationMode = oldMode;
            }
            catch
            {
                // ignore
            }
        }

        private static LinearGradientBrush CreateBorderBrush(Rectangle bounds, Color accentColor)
        {
            var top = Color.FromArgb(220, accentColor);
            var mid = Color.FromArgb(140, accentColor);
            var bottom = Color.FromArgb(220, LeagueColors.RiotGoldDark);

            var brush = new LinearGradientBrush(bounds, top, bottom, LinearGradientMode.Vertical);
            var blend = new ColorBlend();
            blend.Colors = new[] { top, mid, bottom };
            blend.Positions = new[] { 0.0f, 0.5f, 1.0f };
            brush.InterpolationColors = blend;
            return brush;
        }

        private static void DrawRadialGlow(Graphics g, Rectangle bounds)
        {
            // 在左上制造一点“金色氛围光”
            var glowCenter = new PointF(bounds.Left + bounds.Width * 0.35f, bounds.Top + bounds.Height * 0.22f);
            var radius = Math.Max(bounds.Width, bounds.Height) * 0.55f;

            using (var path = new GraphicsPath())
            {
                path.AddEllipse(glowCenter.X - radius, glowCenter.Y - radius, radius * 2, radius * 2);
                using (var pgb = new PathGradientBrush(path))
                {
                    pgb.CenterColor = Color.FromArgb(55, LeagueColors.RiotGold);
                    pgb.SurroundColors = new[] { Color.FromArgb(0, LeagueColors.RiotGold) };
                    g.FillPath(pgb, path);
                }
            }
        }

        private static void DrawNoise(Graphics g, Rectangle bounds)
        {
            var noise = GetNoiseBitmap();
            if (noise == null) return;

            using (var brush = new TextureBrush(noise, WrapMode.Tile))
            {
                g.FillRectangle(brush, bounds);
            }
        }

        private static void DrawHexGrid(Graphics g, Rectangle bounds)
        {
            // 非常淡的六边形网格，避免抢眼
            using (var pen = new Pen(Color.FromArgb(18, LeagueColors.RiotBorderGold), 1))
            {
                var stepX = 90;
                var stepY = 78;
                var r = 18;

                for (int y = bounds.Top - stepY; y < bounds.Bottom + stepY; y += stepY)
                {
                    for (int x = bounds.Left - stepX; x < bounds.Right + stepX; x += stepX)
                    {
                        var offsetX = ((y / stepY) % 2 == 0) ? 0 : (stepX / 2);
                        var center = new Point(x + offsetX, y);
                        var pts = GetHexagonPoints(center, r);
                        g.DrawPolygon(pen, pts);
                    }
                }
            }
        }

        private static Point[] GetHexagonPoints(Point center, int radius)
        {
            var pts = new Point[6];
            for (int i = 0; i < 6; i++)
            {
                // 60 度一个点
                double angle = Math.PI / 180.0 * (60 * i - 30);
                int x = center.X + (int)(radius * Math.Cos(angle));
                int y = center.Y + (int)(radius * Math.Sin(angle));
                pts[i] = new Point(x, y);
            }
            return pts;
        }

        private static Bitmap GetNoiseBitmap()
        {
            lock (NoiseLock)
            {
                if (_noiseBitmap != null)
                {
                    return _noiseBitmap;
                }

                // 生成一张小噪点贴图（自带低透明度），用于 TextureBrush 平铺
                var bmp = new Bitmap(128, 128);
                var rand = new Random(20251215); // 固定种子：避免每次启动风格变化太大

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        // 轻微颗粒感：黑白随机 + 低透明度
                        var v = rand.Next(0, 255);
                        var alpha = rand.Next(8, 22);
                        bmp.SetPixel(x, y, Color.FromArgb(alpha, v, v, v));
                    }
                }

                _noiseBitmap = bmp;
                return _noiseBitmap;
            }
        }
    }
}
