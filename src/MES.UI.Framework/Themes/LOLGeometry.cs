using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// LOL风格几何装饰系统 - 基于leagueoflegends-wpf分析
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public static class LOLGeometry
    {
        #region 六边形系统 (Hexagon System)

        /// <summary>
        /// 创建六边形路径
        /// </summary>
        /// <param name="centerX">中心X坐标</param>
        /// <param name="centerY">中心Y坐标</param>
        /// <param name="radius">半径</param>
        /// <returns>六边形点数组</returns>
        public static PointF[] CreateHexagon(float centerX, float centerY, float radius)
        {
            var points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                var angle = Math.PI / 3 * i;
                points[i] = new PointF(
                    centerX + (float)(radius * Math.Cos(angle)),
                    centerY + (float)(radius * Math.Sin(angle))
                );
            }
            return points;
        }

        /// <summary>
        /// 绘制LOL风格六边形装饰
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="centerX">中心X坐标</param>
        /// <param name="centerY">中心Y坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="fillColor">填充颜色</param>
        /// <param name="borderColor">边框颜色</param>
        public static void DrawHexagonDecoration(Graphics g, float centerX, float centerY, float radius, 
            Color fillColor, Color borderColor)
        {
            var points = CreateHexagon(centerX, centerY, radius);
            
            // 绘制填充
            using (var brush = new SolidBrush(fillColor))
            {
                g.FillPolygon(brush, points);
            }
            
            // 绘制边框
            using (var pen = new Pen(borderColor, 1))
            {
                g.DrawPolygon(pen, points);
            }
        }

        /// <summary>
        /// 绘制发光六边形
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="centerX">中心X坐标</param>
        /// <param name="centerY">中心Y坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="themeColor">主题颜色</param>
        public static void DrawGlowingHexagon(Graphics g, float centerX, float centerY, float radius, Color themeColor)
        {
            var points = CreateHexagon(centerX, centerY, radius);
            
            // 外层发光
            using (var brush = new SolidBrush(Color.FromArgb(40, themeColor)))
            {
                var outerPoints = CreateHexagon(centerX, centerY, radius + 2);
                g.FillPolygon(brush, outerPoints);
            }
            
            // 内层填充
            using (var brush = new SolidBrush(Color.FromArgb(120, themeColor)))
            {
                g.FillPolygon(brush, points);
            }
            
            // 边框
            using (var pen = new Pen(Color.FromArgb(180, themeColor), 1))
            {
                g.DrawPolygon(pen, points);
            }
        }

        #endregion

        #region 菱形系统 (Diamond System)

        /// <summary>
        /// 创建菱形路径 - 仿RadioButton样式
        /// </summary>
        /// <param name="centerX">中心X坐标</param>
        /// <param name="centerY">中心Y坐标</param>
        /// <param name="size">尺寸</param>
        /// <returns>菱形点数组</returns>
        public static PointF[] CreateDiamond(float centerX, float centerY, float size)
        {
            var halfSize = size / 2;
            return new PointF[]
            {
                new PointF(centerX, centerY - halfSize),      // 上
                new PointF(centerX + halfSize, centerY),      // 右
                new PointF(centerX, centerY + halfSize),      // 下
                new PointF(centerX - halfSize, centerY)       // 左
            };
        }

        /// <summary>
        /// 绘制LOL风格菱形装饰
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="centerX">中心X坐标</param>
        /// <param name="centerY">中心Y坐标</param>
        /// <param name="size">尺寸</param>
        /// <param name="fillColor">填充颜色</param>
        /// <param name="borderColor">边框颜色</param>
        public static void DrawDiamondDecoration(Graphics g, float centerX, float centerY, float size,
            Color fillColor, Color borderColor)
        {
            var points = CreateDiamond(centerX, centerY, size);
            
            // 绘制填充
            using (var brush = new SolidBrush(fillColor))
            {
                g.FillPolygon(brush, points);
            }
            
            // 绘制边框
            using (var pen = new Pen(borderColor, 1))
            {
                g.DrawPolygon(pen, points);
            }
        }

        #endregion

        #region 箭头系统 (Arrow System)

        /// <summary>
        /// 创建箭头路径 - 仿RiotPlayButton样式
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>箭头点数组</returns>
        public static PointF[] CreateArrowPath(Rectangle bounds)
        {
            var width = bounds.Width;
            var height = bounds.Height;
            
            return new PointF[]
            {
                new PointF(bounds.X, bounds.Y),
                new PointF(bounds.X + width * 0.8f, bounds.Y),
                new PointF(bounds.Right, bounds.Y + height * 0.5f),
                new PointF(bounds.X + width * 0.8f, bounds.Bottom),
                new PointF(bounds.X, bounds.Bottom)
            };
        }

        /// <summary>
        /// 绘制LOL风格箭头装饰
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="fillBrush">填充画刷</param>
        /// <param name="borderPen">边框画笔</param>
        public static void DrawArrowDecoration(Graphics g, Rectangle bounds, Brush fillBrush, Pen borderPen)
        {
            var points = CreateArrowPath(bounds);
            
            // 绘制填充
            g.FillPolygon(fillBrush, points);
            
            // 绘制边框
            g.DrawPolygon(borderPen, points);
        }

        #endregion

        #region 角落装饰系统 (Corner Decoration System)

        /// <summary>
        /// 绘制角落装饰线条 - LOL风格
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="decorColor">装饰颜色</param>
        /// <param name="lineLength">线条长度</param>
        public static void DrawCornerDecorations(Graphics g, Rectangle bounds, Color decorColor, int lineLength = 17)
        {
            using (var pen = new Pen(decorColor, 2))
            {
                // 左上角装饰
                g.DrawLine(pen, bounds.X + 8, bounds.Y + 8, bounds.X + 8 + lineLength, bounds.Y + 8);
                g.DrawLine(pen, bounds.X + 8, bounds.Y + 8, bounds.X + 8, bounds.Y + 8 + lineLength);
                
                // 右下角装饰
                g.DrawLine(pen, bounds.Right - 8 - lineLength, bounds.Bottom - 8, bounds.Right - 8, bounds.Bottom - 8);
                g.DrawLine(pen, bounds.Right - 8, bounds.Bottom - 8 - lineLength, bounds.Right - 8, bounds.Bottom - 8);
            }
        }

        /// <summary>
        /// 绘制发光点装饰
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="glowColor">发光颜色</param>
        /// <param name="dotSize">点的大小</param>
        public static void DrawGlowDots(Graphics g, Rectangle bounds, Color glowColor, int dotSize = 4)
        {
            using (var brush = new SolidBrush(glowColor))
            {
                // 左上角发光点
                g.FillEllipse(brush, bounds.X + 6, bounds.Y + 6, dotSize, dotSize);
                
                // 右下角发光点
                g.FillEllipse(brush, bounds.Right - 10, bounds.Bottom - 10, dotSize, dotSize);
            }
        }

        #endregion

        #region 进度条几何系统 (Progress Bar Geometry System)

        /// <summary>
        /// 创建复杂进度条路径 - 仿RiotHorizontalProgress
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="progress">进度值 (0.0 - 1.0)</param>
        /// <returns>进度条路径</returns>
        public static GraphicsPath CreateProgressPath(Rectangle bounds, float progress)
        {
            var path = new GraphicsPath();
            
            // 计算进度宽度
            var progressWidth = (int)(bounds.Width * progress);
            var progressRect = new Rectangle(bounds.X, bounds.Y, progressWidth, bounds.Height);
            
            // 添加矩形路径
            path.AddRectangle(progressRect);
            
            return path;
        }

        /// <summary>
        /// 绘制LOL风格进度条
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="progress">进度值 (0.0 - 1.0)</param>
        /// <param name="backgroundBrush">背景画刷</param>
        /// <param name="foregroundBrush">前景画刷</param>
        /// <param name="borderPen">边框画笔</param>
        public static void DrawProgressBar(Graphics g, Rectangle bounds, float progress,
            Brush backgroundBrush, Brush foregroundBrush, Pen borderPen)
        {
            // 绘制背景
            g.FillRectangle(backgroundBrush, bounds);
            
            // 绘制进度
            if (progress > 0)
            {
                var progressWidth = (int)(bounds.Width * progress);
                var progressRect = new Rectangle(bounds.X, bounds.Y, progressWidth, bounds.Height);
                g.FillRectangle(foregroundBrush, progressRect);
            }
            
            // 绘制边框
            g.DrawRectangle(borderPen, bounds);
        }

        #endregion

        #region 圆形装饰系统 (Circle Decoration System)

        /// <summary>
        /// 绘制LOL风格圆形按钮 - 仿RiotCircleButton
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="centerX">中心X坐标</param>
        /// <param name="centerY">中心Y坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="fillBrush">填充画刷</param>
        /// <param name="borderPen">边框画笔</param>
        public static void DrawCircleButton(Graphics g, float centerX, float centerY, float radius,
            Brush fillBrush, Pen borderPen)
        {
            var bounds = new RectangleF(centerX - radius, centerY - radius, radius * 2, radius * 2);
            
            // 绘制填充
            g.FillEllipse(fillBrush, bounds);
            
            // 绘制边框
            g.DrawEllipse(borderPen, bounds);
        }

        /// <summary>
        /// 绘制多层圆形装饰 - 3层边框效果
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="centerX">中心X坐标</param>
        /// <param name="centerY">中心Y坐标</param>
        /// <param name="radius">半径</param>
        /// <param name="themeColor">主题颜色</param>
        public static void DrawMultiLayerCircle(Graphics g, float centerX, float centerY, float radius, Color themeColor)
        {
            // 外层发光
            using (var pen = new Pen(Color.FromArgb(60, themeColor), 3))
            {
                var outerBounds = new RectangleF(centerX - radius - 2, centerY - radius - 2, (radius + 2) * 2, (radius + 2) * 2);
                g.DrawEllipse(pen, outerBounds);
            }
            
            // 中层边框
            using (var pen = new Pen(Color.FromArgb(120, themeColor), 2))
            {
                var bounds = new RectangleF(centerX - radius, centerY - radius, radius * 2, radius * 2);
                g.DrawEllipse(pen, bounds);
            }
            
            // 内层填充
            using (var brush = new SolidBrush(Color.FromArgb(40, themeColor)))
            {
                var innerBounds = new RectangleF(centerX - radius + 2, centerY - radius + 2, (radius - 2) * 2, (radius - 2) * 2);
                g.FillEllipse(brush, innerBounds);
            }
        }

        #endregion
    }
}
