using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 英雄联盟视觉特效绘制类
    /// 实现真正的英雄联盟风格：金属质感、渐变光效、边框装饰
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public static class LeagueVisualEffects
    {
        #region 英雄联盟风格按钮绘制

        /// <summary>
        /// 绘制英雄联盟风格按钮
        /// 包含：金属渐变背景、发光边框、内阴影效果、六边形装饰
        /// </summary>
        public static void DrawLeagueButton(Graphics g, Rectangle bounds, bool isHovered, bool isPressed, string text, Font font)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 计算绘制区域
            var drawRect = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);

            // 1. 绘制外发光效果
            DrawOuterGlow(g, bounds, isHovered);

            // 2. 绘制主体渐变背景
            DrawButtonGradientBackground(g, drawRect, isHovered, isPressed);

            // 3. 绘制金属边框
            DrawMetallicBorder(g, drawRect, isHovered);

            // 4. 绘制内部高光
            DrawInnerHighlight(g, drawRect);

            // 5. 绘制六边形装饰（仅悬停时）
            if (isHovered)
            {
                DrawButtonHexagonDecorations(g, drawRect);
            }

            // 6. 绘制脉冲效果（仅悬停时）
            if (isHovered)
            {
                DrawPulseEffect(g, drawRect, 0.8f);
            }

            // 7. 绘制文字
            DrawButtonText(g, drawRect, text, font, isPressed);
        }

        /// <summary>
        /// 绘制外发光效果 - 增强版
        /// </summary>
        private static void DrawOuterGlow(Graphics g, Rectangle bounds, bool isHovered)
        {
            // 始终绘制基础发光，悬停时增强
            var baseGlowColor = Color.FromArgb(40, LeagueColors.TextGold);
            var hoverGlowColor = Color.FromArgb(120, LeagueColors.TextGold);
            var glowSize = isHovered ? 6 : 3;

            for (int i = 0; i < glowSize; i++)
            {
                var alpha = isHovered ? (int)(120 * (1.0 - (double)i / glowSize)) : (int)(40 * (1.0 - (double)i / glowSize));
                var glowColor = isHovered ? hoverGlowColor : baseGlowColor;

                var expandedRect = new Rectangle(
                    bounds.X - i - 1, bounds.Y - i - 1,
                    bounds.Width + (i + 1) * 2, bounds.Height + (i + 1) * 2);

                using (var pen = new Pen(Color.FromArgb(alpha, glowColor), 2))
                {
                    g.DrawRectangle(pen, expandedRect);
                }
            }
        }

        /// <summary>
        /// 绘制按钮渐变背景 - 增强版
        /// </summary>
        private static void DrawButtonGradientBackground(Graphics g, Rectangle bounds, bool isHovered, bool isPressed)
        {
            Color startColor, middleColor, endColor;

            if (isPressed)
            {
                startColor = Color.FromArgb(80, 60, 20);    // 更深的金色
                middleColor = LeagueColors.PrimaryGoldDark;
                endColor = LeagueColors.PrimaryGold;
            }
            else if (isHovered)
            {
                startColor = Color.FromArgb(255, 215, 0);   // 亮金色
                middleColor = LeagueColors.PrimaryGoldLight;
                endColor = LeagueColors.PrimaryGold;
            }
            else
            {
                startColor = LeagueColors.PrimaryGoldLight;
                middleColor = LeagueColors.PrimaryGold;
                endColor = LeagueColors.PrimaryGoldDark;
            }

            // 主渐变
            using (var brush = new LinearGradientBrush(bounds, startColor, endColor, LinearGradientMode.Vertical))
            {
                var blend = new ColorBlend();
                blend.Colors = new Color[] { startColor, middleColor, endColor };
                blend.Positions = new float[] { 0.0f, 0.4f, 1.0f };
                brush.InterpolationColors = blend;

                g.FillRectangle(brush, bounds);
            }

            // 添加金属光泽效果
            var glossRect = new Rectangle(bounds.X + 5, bounds.Y + 5, bounds.Width - 10, bounds.Height / 2 - 5);
            using (var glossBrush = new LinearGradientBrush(
                glossRect,
                Color.FromArgb(100, Color.White),
                Color.FromArgb(20, Color.White),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(glossBrush, glossRect);
            }

            // 添加底部阴影
            var shadowRect = new Rectangle(bounds.X + 5, bounds.Y + bounds.Height / 2, bounds.Width - 10, bounds.Height / 2 - 5);
            using (var shadowBrush = new LinearGradientBrush(
                shadowRect,
                Color.FromArgb(0, Color.Black),
                Color.FromArgb(40, Color.Black),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(shadowBrush, shadowRect);
            }
        }

        /// <summary>
        /// 绘制金属边框 - 增强版
        /// </summary>
        private static void DrawMetallicBorder(Graphics g, Rectangle bounds, bool isHovered)
        {
            var borderColor = isHovered ? LeagueColors.TextGold : LeagueColors.PrimaryGoldLight;

            // 外边框 - 更粗更明显
            using (var pen = new Pen(borderColor, 3))
            {
                g.DrawRectangle(pen, bounds);
            }

            // 中间边框 - 创造层次感
            var middleRect = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
            using (var pen = new Pen(Color.FromArgb(150, LeagueColors.PrimaryGold), 2))
            {
                g.DrawRectangle(pen, middleRect);
            }

            // 内边框 - 深度感
            var innerRect = new Rectangle(bounds.X + 4, bounds.Y + 4, bounds.Width - 8, bounds.Height - 8);
            using (var pen = new Pen(Color.FromArgb(80, LeagueColors.PrimaryGoldDark), 1))
            {
                g.DrawRectangle(pen, innerRect);
            }

            // 角落强化装饰
            DrawCornerAccents(g, bounds, borderColor);
        }

        /// <summary>
        /// 绘制角落强化装饰
        /// </summary>
        private static void DrawCornerAccents(Graphics g, Rectangle bounds, Color accentColor)
        {
            var accentSize = 8;
            using (var brush = new SolidBrush(Color.FromArgb(200, accentColor)))
            {
                // 四个角的小方块装饰
                g.FillRectangle(brush, bounds.X - 2, bounds.Y - 2, accentSize, accentSize);
                g.FillRectangle(brush, bounds.Right - accentSize + 2, bounds.Y - 2, accentSize, accentSize);
                g.FillRectangle(brush, bounds.X - 2, bounds.Bottom - accentSize + 2, accentSize, accentSize);
                g.FillRectangle(brush, bounds.Right - accentSize + 2, bounds.Bottom - accentSize + 2, accentSize, accentSize);
            }
        }

        /// <summary>
        /// 绘制按钮六边形装饰
        /// </summary>
        private static void DrawButtonHexagonDecorations(Graphics g, Rectangle bounds)
        {
            // 在按钮四角绘制小六边形
            DrawHexagon(g, new Point(bounds.X + 10, bounds.Y + 10), 4, Color.FromArgb(120, LeagueColors.TextGold));
            DrawHexagon(g, new Point(bounds.Right - 10, bounds.Y + 10), 4, Color.FromArgb(120, LeagueColors.TextGold));
            DrawHexagon(g, new Point(bounds.X + 10, bounds.Bottom - 10), 4, Color.FromArgb(120, LeagueColors.TextGold));
            DrawHexagon(g, new Point(bounds.Right - 10, bounds.Bottom - 10), 4, Color.FromArgb(120, LeagueColors.TextGold));
        }

        /// <summary>
        /// 绘制内部高光
        /// </summary>
        private static void DrawInnerHighlight(Graphics g, Rectangle bounds)
        {
            var highlightRect = new Rectangle(bounds.X + 3, bounds.Y + 3, bounds.Width - 6, bounds.Height / 3);
            
            using (var brush = new LinearGradientBrush(
                highlightRect,
                Color.FromArgb(60, Color.White),
                Color.FromArgb(10, Color.White),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, highlightRect);
            }
        }

        /// <summary>
        /// 绘制按钮文字
        /// </summary>
        private static void DrawButtonText(Graphics g, Rectangle bounds, string text, Font font, bool isPressed)
        {
            if (string.IsNullOrEmpty(text)) return;

            var textRect = bounds;
            if (isPressed)
            {
                textRect.Offset(1, 1); // 按下效果
            }

            // 绘制文字阴影
            var shadowRect = textRect;
            shadowRect.Offset(1, 1);
            using (var shadowBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(text, font, shadowBrush, shadowRect, sf);
            }

            // 绘制主文字
            using (var textBrush = new SolidBrush(LeagueColors.TextPrimary))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(text, font, textBrush, textRect, sf);
            }
        }

        #endregion

        #region 英雄联盟风格面板绘制

        /// <summary>
        /// 绘制英雄联盟风格面板
        /// 包含：深色渐变背景、金色边框、角落装饰、六边形装饰
        /// </summary>
        public static void DrawLeaguePanel(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. 绘制渐变背景
            DrawPanelGradientBackground(g, bounds);

            // 2. 绘制边框
            DrawPanelBorder(g, bounds);

            // 3. 绘制角落装饰
            DrawCornerDecorations(g, bounds);

            // 4. 绘制六边形装饰
            DrawHexagonDecorations(g, bounds);

            // 5. 绘制大型背景六边形（仅对较大面板）
            if (bounds.Width > 300 && bounds.Height > 200)
            {
                DrawLargeHexagonBackground(g, bounds);
            }
        }

        /// <summary>
        /// 绘制面板渐变背景
        /// </summary>
        private static void DrawPanelGradientBackground(Graphics g, Rectangle bounds)
        {
            using (var brush = new LinearGradientBrush(
                bounds,
                LeagueColors.DarkBackground,
                LeagueColors.DarkSurface,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, bounds);
            }

            // 添加微妙的纹理效果
            using (var textureBrush = new LinearGradientBrush(
                bounds,
                Color.FromArgb(10, LeagueColors.PrimaryGold),
                Color.FromArgb(5, LeagueColors.PrimaryGold),
                45f))
            {
                g.FillRectangle(textureBrush, bounds);
            }
        }

        /// <summary>
        /// 绘制面板边框
        /// </summary>
        private static void DrawPanelBorder(Graphics g, Rectangle bounds)
        {
            // 主边框
            using (var pen = new Pen(LeagueColors.DarkBorder, 1))
            {
                g.DrawRectangle(pen, bounds);
            }

            // 内边框高光
            var innerRect = new Rectangle(bounds.X + 1, bounds.Y + 1, bounds.Width - 2, bounds.Height - 2);
            using (var pen = new Pen(Color.FromArgb(30, LeagueColors.PrimaryGold), 1))
            {
                g.DrawRectangle(pen, innerRect);
            }
        }

        /// <summary>
        /// 绘制角落装饰
        /// </summary>
        private static void DrawCornerDecorations(Graphics g, Rectangle bounds)
        {
            var cornerSize = 8;
            var decorationColor = Color.FromArgb(80, LeagueColors.PrimaryGold);

            using (var brush = new SolidBrush(decorationColor))
            {
                // 左上角
                var topLeft = new Point[] {
                    new Point(bounds.X, bounds.Y),
                    new Point(bounds.X + cornerSize, bounds.Y),
                    new Point(bounds.X, bounds.Y + cornerSize)
                };
                g.FillPolygon(brush, topLeft);

                // 右上角
                var topRight = new Point[] {
                    new Point(bounds.Right, bounds.Y),
                    new Point(bounds.Right - cornerSize, bounds.Y),
                    new Point(bounds.Right, bounds.Y + cornerSize)
                };
                g.FillPolygon(brush, topRight);

                // 左下角
                var bottomLeft = new Point[] {
                    new Point(bounds.X, bounds.Bottom),
                    new Point(bounds.X + cornerSize, bounds.Bottom),
                    new Point(bounds.X, bounds.Bottom - cornerSize)
                };
                g.FillPolygon(brush, bottomLeft);

                // 右下角
                var bottomRight = new Point[] {
                    new Point(bounds.Right, bounds.Bottom),
                    new Point(bounds.Right - cornerSize, bounds.Bottom),
                    new Point(bounds.Right, bounds.Bottom - cornerSize)
                };
                g.FillPolygon(brush, bottomRight);
            }
        }

        #endregion

        #region 英雄联盟风格菜单绘制

        /// <summary>
        /// 绘制英雄联盟风格菜单栏 - 增强版
        /// </summary>
        public static void DrawLeagueMenuBar(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制顶部双层金色边框（英雄联盟标志性元素）
            using (var brush = new LinearGradientBrush(
                new Rectangle(bounds.X, bounds.Y, bounds.Width, 4),
                LeagueColors.TextGold,
                LeagueColors.PrimaryGold,
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(brush, bounds.X, bounds.Y, bounds.Width, 4);
            }

            // 第二层金色边框
            using (var brush = new LinearGradientBrush(
                new Rectangle(bounds.X, bounds.Y + 4, bounds.Width, 2),
                Color.FromArgb(150, LeagueColors.PrimaryGoldLight),
                Color.FromArgb(80, LeagueColors.PrimaryGold),
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(brush, bounds.X, bounds.Y + 4, bounds.Width, 2);
            }

            // 绘制主体背景 - 多层渐变
            var mainRect = new Rectangle(bounds.X, bounds.Y + 6, bounds.Width, bounds.Height - 6);
            using (var brush = new LinearGradientBrush(
                mainRect,
                Color.FromArgb(25, 30, 40),
                Color.FromArgb(15, 20, 30),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, mainRect);
            }

            // 添加顶部高光效果
            var highlightRect = new Rectangle(bounds.X, bounds.Y + 6, bounds.Width, mainRect.Height / 3);
            using (var brush = new LinearGradientBrush(highlightRect,
                Color.FromArgb(30, Color.White),
                Color.FromArgb(5, Color.White),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, highlightRect);
            }

            // 绘制底部双分隔线
            using (var pen = new Pen(LeagueColors.PrimaryGold, 2))
            {
                g.DrawLine(pen, bounds.X, bounds.Bottom - 2, bounds.Right, bounds.Bottom - 2);
            }
            using (var pen = new Pen(Color.FromArgb(100, LeagueColors.PrimaryGoldLight), 1))
            {
                g.DrawLine(pen, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
            }

            // 添加角落装饰
            DrawMenuBarCornerAccents(g, bounds);
        }

        /// <summary>
        /// 绘制菜单栏角落装饰
        /// </summary>
        private static void DrawMenuBarCornerAccents(Graphics g, Rectangle bounds)
        {
            var accentSize = 8;
            using (var brush = new SolidBrush(Color.FromArgb(180, LeagueColors.PrimaryGold)))
            {
                // 左上角三角装饰
                var leftTop = new Point[] {
                    new Point(bounds.X, bounds.Y),
                    new Point(bounds.X + accentSize, bounds.Y),
                    new Point(bounds.X, bounds.Y + accentSize)
                };
                g.FillPolygon(brush, leftTop);

                // 右上角三角装饰
                var rightTop = new Point[] {
                    new Point(bounds.Right - accentSize, bounds.Y),
                    new Point(bounds.Right, bounds.Y),
                    new Point(bounds.Right, bounds.Y + accentSize)
                };
                g.FillPolygon(brush, rightTop);
            }
        }

        #endregion

        #region 英雄联盟六边形装饰

        /// <summary>
        /// 绘制英雄联盟标志性六边形装饰
        /// </summary>
        public static void DrawHexagonDecorations(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制多个六边形装饰
            DrawHexagon(g, new Point(bounds.X + 20, bounds.Y + 20), 8, Color.FromArgb(80, LeagueColors.PrimaryGold));
            DrawHexagon(g, new Point(bounds.Right - 30, bounds.Y + 15), 6, Color.FromArgb(60, LeagueColors.PrimaryGoldLight));
            DrawHexagon(g, new Point(bounds.X + 15, bounds.Bottom - 25), 7, Color.FromArgb(70, LeagueColors.TextGold));
            DrawHexagon(g, new Point(bounds.Right - 25, bounds.Bottom - 20), 5, Color.FromArgb(50, LeagueColors.PrimaryGold));
        }

        /// <summary>
        /// 绘制单个六边形
        /// </summary>
        public static void DrawHexagon(Graphics g, Point center, int radius, Color color)
        {
            var points = new Point[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = Math.PI / 3 * i;
                points[i] = new Point(
                    (int)(center.X + radius * Math.Cos(angle)),
                    (int)(center.Y + radius * Math.Sin(angle))
                );
            }

            // 填充六边形
            using (var brush = new SolidBrush(Color.FromArgb(30, color)))
            {
                g.FillPolygon(brush, points);
            }

            // 绘制六边形边框
            using (var pen = new Pen(color, 1))
            {
                g.DrawPolygon(pen, points);
            }
        }

        /// <summary>
        /// 绘制大型装饰六边形（用于背景）
        /// </summary>
        public static void DrawLargeHexagonBackground(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 计算六边形大小和位置
            int hexSize = Math.Min(bounds.Width, bounds.Height) / 3;
            Point center = new Point(bounds.Right - hexSize, bounds.Y + hexSize);

            var points = new Point[6];
            for (int i = 0; i < 6; i++)
            {
                double angle = Math.PI / 3 * i;
                points[i] = new Point(
                    (int)(center.X + hexSize * Math.Cos(angle)),
                    (int)(center.Y + hexSize * Math.Sin(angle))
                );
            }

            // 绘制半透明六边形背景
            using (var brush = new LinearGradientBrush(
                new Rectangle(center.X - hexSize, center.Y - hexSize, hexSize * 2, hexSize * 2),
                Color.FromArgb(15, LeagueColors.PrimaryGold),
                Color.FromArgb(5, LeagueColors.PrimaryGoldLight),
                LinearGradientMode.ForwardDiagonal))
            {
                g.FillPolygon(brush, points);
            }

            // 绘制六边形边框
            using (var pen = new Pen(Color.FromArgb(40, LeagueColors.PrimaryGold), 2))
            {
                g.DrawPolygon(pen, points);
            }
        }

        #endregion

        #region 英雄联盟动态光效

        /// <summary>
        /// 绘制脉冲光效
        /// </summary>
        public static void DrawPulseEffect(Graphics g, Rectangle bounds, float intensity = 1.0f)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 计算脉冲强度（可以通过Timer动态改变）
            int alpha = (int)(60 * intensity);

            // 绘制多层脉冲光效
            for (int i = 0; i < 3; i++)
            {
                var glowRect = new Rectangle(
                    bounds.X - i * 2,
                    bounds.Y - i * 2,
                    bounds.Width + i * 4,
                    bounds.Height + i * 4);

                using (var pen = new Pen(Color.FromArgb(alpha / (i + 1), LeagueColors.TextGold), 1))
                {
                    g.DrawRectangle(pen, glowRect);
                }
            }
        }

        /// <summary>
        /// 绘制能量流光效
        /// </summary>
        public static void DrawEnergyFlow(Graphics g, Rectangle bounds, float progress = 0.5f)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 计算流光位置
            int flowWidth = 20;
            int flowX = (int)(bounds.X + (bounds.Width - flowWidth) * progress);

            // 绘制流光效果
            using (var brush = new LinearGradientBrush(
                new Rectangle(flowX, bounds.Y, flowWidth, bounds.Height),
                Color.FromArgb(0, LeagueColors.TextGold),
                Color.FromArgb(120, LeagueColors.TextGold),
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(brush, flowX, bounds.Y, flowWidth, bounds.Height);
            }
        }

        #endregion

        #region 粒子效果集成

        private static Dictionary<Control, LeagueParticleSystem> particleSystems = new Dictionary<Control, LeagueParticleSystem>();

        /// <summary>
        /// 为控件启用粒子效果
        /// </summary>
        public static void EnableParticleEffects(Control control)
        {
            if (!particleSystems.ContainsKey(control))
            {
                particleSystems[control] = new LeagueParticleSystem(control.ClientRectangle);
            }
        }

        /// <summary>
        /// 绘制控件的粒子效果
        /// </summary>
        public static void DrawParticleEffects(Graphics g, Control control)
        {
            if (particleSystems.ContainsKey(control))
            {
                var system = particleSystems[control];
                system.Update();
                system.Draw(g);
            }
        }

        /// <summary>
        /// 在指定位置触发粒子爆发
        /// </summary>
        public static void TriggerParticleBurst(Control control, Point location, int count = 15)
        {
            if (particleSystems.ContainsKey(control))
            {
                particleSystems[control].Burst(location, count);
            }
        }

        /// <summary>
        /// 绘制增强的英雄联盟面板（包含粒子效果）
        /// </summary>
        public static void DrawEnhancedLeaguePanel(Graphics g, Rectangle bounds, Control control = null)
        {
            // 绘制基础面板
            DrawLeaguePanel(g, bounds);

            // 绘制粒子效果
            if (control != null)
            {
                DrawParticleEffects(g, control);
            }

            // 绘制额外的装饰效果
            DrawAdvancedDecorations(g, bounds);
        }

        /// <summary>
        /// 绘制高级装饰效果
        /// </summary>
        private static void DrawAdvancedDecorations(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制能量线条
            DrawEnergyLines(g, bounds);

            // 绘制角落光效
            DrawCornerGlow(g, bounds);
        }

        /// <summary>
        /// 绘制能量线条
        /// </summary>
        private static void DrawEnergyLines(Graphics g, Rectangle bounds)
        {
            using (var pen = new Pen(Color.FromArgb(80, LeagueColors.PrimaryGold), 1))
            {
                // 绘制对角线能量线
                g.DrawLine(pen, bounds.X + 10, bounds.Y + 10, bounds.X + 30, bounds.Y + 30);
                g.DrawLine(pen, bounds.Right - 30, bounds.Y + 10, bounds.Right - 10, bounds.Y + 30);
                g.DrawLine(pen, bounds.X + 10, bounds.Bottom - 30, bounds.X + 30, bounds.Bottom - 10);
                g.DrawLine(pen, bounds.Right - 30, bounds.Bottom - 30, bounds.Right - 10, bounds.Bottom - 10);
            }
        }

        /// <summary>
        /// 绘制角落光效
        /// </summary>
        private static void DrawCornerGlow(Graphics g, Rectangle bounds)
        {
            var glowSize = 15;
            var glowColor = Color.FromArgb(40, LeagueColors.PrimaryGoldLight);

            // 创建径向渐变效果的路径
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, glowSize, glowSize);

            using (var brush = new PathGradientBrush(path))
            {
                brush.CenterColor = glowColor;
                brush.SurroundColors = new Color[] { Color.Transparent };

                // 四个角的光效
                var matrix = new Matrix();

                // 左上角
                matrix.Reset();
                matrix.Translate(bounds.X - glowSize/2, bounds.Y - glowSize/2);
                brush.Transform = matrix;
                g.FillEllipse(brush, bounds.X - glowSize/2, bounds.Y - glowSize/2, glowSize, glowSize);

                // 右上角
                matrix.Reset();
                matrix.Translate(bounds.Right - glowSize/2, bounds.Y - glowSize/2);
                brush.Transform = matrix;
                g.FillEllipse(brush, bounds.Right - glowSize/2, bounds.Y - glowSize/2, glowSize, glowSize);

                // 左下角
                matrix.Reset();
                matrix.Translate(bounds.X - glowSize/2, bounds.Bottom - glowSize/2);
                brush.Transform = matrix;
                g.FillEllipse(brush, bounds.X - glowSize/2, bounds.Bottom - glowSize/2, glowSize, glowSize);

                // 右下角
                matrix.Reset();
                matrix.Translate(bounds.Right - glowSize/2, bounds.Bottom - glowSize/2);
                brush.Transform = matrix;
                g.FillEllipse(brush, bounds.Right - glowSize/2, bounds.Bottom - glowSize/2, glowSize, glowSize);
            }

            path.Dispose();
        }

        #endregion
    }


}
