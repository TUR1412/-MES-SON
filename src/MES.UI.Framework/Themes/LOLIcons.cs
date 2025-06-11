using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// LOL风格图标系统 - 基于leagueoflegends-wpf分析
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public static class LOLIcons
    {
        #region MES模块图标系统 (MES Module Icon System)

        /// <summary>
        /// 绘制物料管理图标 - 盾牌样式
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="iconColor">图标颜色</param>
        public static void DrawMaterialIcon(Graphics g, Rectangle bounds, Color iconColor)
        {
            using (var pen = new Pen(iconColor, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, iconColor)))
            {
                // 创建盾牌形状
                var points = new PointF[]
                {
                    new PointF(bounds.X + bounds.Width * 0.5f, bounds.Y),                    // 顶部
                    new PointF(bounds.Right, bounds.Y + bounds.Height * 0.3f),               // 右上
                    new PointF(bounds.Right, bounds.Y + bounds.Height * 0.7f),               // 右下
                    new PointF(bounds.X + bounds.Width * 0.5f, bounds.Bottom),               // 底部
                    new PointF(bounds.X, bounds.Y + bounds.Height * 0.7f),                   // 左下
                    new PointF(bounds.X, bounds.Y + bounds.Height * 0.3f)                    // 左上
                };
                
                // 填充盾牌
                g.FillPolygon(brush, points);
                
                // 绘制盾牌边框
                g.DrawPolygon(pen, points);
                
                // 绘制内部装饰线
                var centerX = bounds.X + bounds.Width * 0.5f;
                var centerY = bounds.Y + bounds.Height * 0.5f;
                g.DrawLine(pen, centerX, bounds.Y + bounds.Height * 0.2f, centerX, bounds.Y + bounds.Height * 0.8f);
                g.DrawLine(pen, bounds.X + bounds.Width * 0.2f, centerY, bounds.X + bounds.Width * 0.8f, centerY);
            }
        }

        /// <summary>
        /// 绘制生产管理图标 - 剑样式
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="iconColor">图标颜色</param>
        public static void DrawProductionIcon(Graphics g, Rectangle bounds, Color iconColor)
        {
            using (var pen = new Pen(iconColor, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, iconColor)))
            {
                var centerX = bounds.X + bounds.Width * 0.5f;
                
                // 剑身
                var swordRect = new RectangleF(centerX - 2, bounds.Y, 4, bounds.Height * 0.8f);
                g.FillRectangle(brush, swordRect);
                g.DrawLine(pen, centerX, bounds.Y, centerX, bounds.Y + bounds.Height * 0.8f);
                
                // 护手
                var guardY = bounds.Y + bounds.Height * 0.2f;
                g.DrawLine(pen, bounds.X + bounds.Width * 0.2f, guardY, bounds.X + bounds.Width * 0.8f, guardY);
                
                // 剑柄
                var handleRect = new RectangleF(centerX - 3, bounds.Y + bounds.Height * 0.8f, 6, bounds.Height * 0.2f);
                g.FillRectangle(brush, handleRect);
                g.DrawRectangle(pen, handleRect.X, handleRect.Y, handleRect.Width, handleRect.Height);
            }
        }

        /// <summary>
        /// 绘制车间管理图标 - 工厂样式
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="iconColor">图标颜色</param>
        public static void DrawWorkshopIcon(Graphics g, Rectangle bounds, Color iconColor)
        {
            using (var pen = new Pen(iconColor, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, iconColor)))
            {
                // 主建筑
                var mainBuilding = new Rectangle(bounds.X, bounds.Y + bounds.Height / 2, bounds.Width, bounds.Height / 2);
                g.FillRectangle(brush, mainBuilding);
                g.DrawRectangle(pen, mainBuilding);
                
                // 烟囱1
                var chimney1 = new Rectangle(bounds.X + bounds.Width / 4, bounds.Y, bounds.Width / 8, bounds.Height / 2);
                g.FillRectangle(brush, chimney1);
                g.DrawRectangle(pen, chimney1);
                
                // 烟囱2
                var chimney2 = new Rectangle(bounds.X + bounds.Width * 3 / 4, bounds.Y + bounds.Height / 4, bounds.Width / 8, bounds.Height / 4);
                g.FillRectangle(brush, chimney2);
                g.DrawRectangle(pen, chimney2);
                
                // 烟雾效果
                using (var smokePen = new Pen(Color.FromArgb(80, iconColor), 1))
                {
                    var smokeY = bounds.Y - 5;
                    g.DrawLine(smokePen, chimney1.X + chimney1.Width / 2, bounds.Y, chimney1.X + chimney1.Width / 2 - 3, smokeY);
                    g.DrawLine(smokePen, chimney2.X + chimney2.Width / 2, chimney2.Y, chimney2.X + chimney2.Width / 2 + 3, smokeY);
                }
            }
        }

        /// <summary>
        /// 绘制系统管理图标 - 齿轮样式
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="iconColor">图标颜色</param>
        public static void DrawSystemIcon(Graphics g, Rectangle bounds, Color iconColor)
        {
            using (var pen = new Pen(iconColor, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, iconColor)))
            {
                var centerX = bounds.X + bounds.Width / 2;
                var centerY = bounds.Y + bounds.Height / 2;
                var outerRadius = Math.Min(bounds.Width, bounds.Height) / 2 - 2;
                var innerRadius = outerRadius * 0.6f;
                
                // 绘制齿轮外圈
                var outerBounds = new RectangleF(centerX - outerRadius, centerY - outerRadius, outerRadius * 2, outerRadius * 2);
                g.FillEllipse(brush, outerBounds);
                g.DrawEllipse(pen, outerBounds);
                
                // 绘制齿轮内圈
                var innerBounds = new RectangleF(centerX - innerRadius, centerY - innerRadius, innerRadius * 2, innerRadius * 2);
                g.DrawEllipse(pen, innerBounds);
                
                // 绘制齿轮齿
                for (int i = 0; i < 8; i++)
                {
                    var angle = i * Math.PI / 4;
                    var x1 = centerX + (float)(innerRadius * Math.Cos(angle));
                    var y1 = centerY + (float)(innerRadius * Math.Sin(angle));
                    var x2 = centerX + (float)(outerRadius * 1.3f * Math.Cos(angle));
                    var y2 = centerY + (float)(outerRadius * 1.3f * Math.Sin(angle));
                    
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
                
                // 中心点
                g.FillEllipse(brush, centerX - 3, centerY - 3, 6, 6);
            }
        }

        #endregion

        #region 状态图标系统 (Status Icon System)

        /// <summary>
        /// 绘制在线状态图标
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="isOnline">是否在线</param>
        public static void DrawOnlineStatusIcon(Graphics g, Rectangle bounds, bool isOnline)
        {
            var statusColor = isOnline ? LeagueColors.SuccessGreen : LeagueColors.ErrorRed;
            
            using (var brush = new SolidBrush(statusColor))
            using (var pen = new Pen(Color.FromArgb(200, statusColor), 1))
            {
                // 绘制状态圆点
                g.FillEllipse(brush, bounds);
                g.DrawEllipse(pen, bounds);
                
                // 如果在线，添加发光效果
                if (isOnline)
                {
                    using (var glowBrush = new SolidBrush(Color.FromArgb(60, statusColor)))
                    {
                        var glowBounds = new Rectangle(bounds.X - 2, bounds.Y - 2, bounds.Width + 4, bounds.Height + 4);
                        g.FillEllipse(glowBrush, glowBounds);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制数据库状态图标
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="isConnected">是否连接</param>
        public static void DrawDatabaseStatusIcon(Graphics g, Rectangle bounds, bool isConnected)
        {
            var statusColor = isConnected ? LeagueColors.AccentBlue : LeagueColors.ErrorRed;
            
            using (var pen = new Pen(statusColor, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, statusColor)))
            {
                // 绘制数据库圆柱体
                var topEllipse = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height / 3);
                var middleRect = new Rectangle(bounds.X, bounds.Y + bounds.Height / 6, bounds.Width, bounds.Height * 2 / 3);
                var bottomEllipse = new Rectangle(bounds.X, bounds.Y + bounds.Height * 2 / 3, bounds.Width, bounds.Height / 3);
                
                // 填充
                g.FillRectangle(brush, middleRect);
                g.FillEllipse(brush, topEllipse);
                g.FillEllipse(brush, bottomEllipse);
                
                // 边框
                g.DrawEllipse(pen, topEllipse);
                g.DrawEllipse(pen, bottomEllipse);
                g.DrawLine(pen, bounds.X, bounds.Y + bounds.Height / 3, bounds.X, bounds.Y + bounds.Height * 2 / 3);
                g.DrawLine(pen, bounds.Right, bounds.Y + bounds.Height / 3, bounds.Right, bounds.Y + bounds.Height * 2 / 3);
            }
        }

        #endregion

        #region 导航图标系统 (Navigation Icon System)

        /// <summary>
        /// 绘制首页图标
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="iconColor">图标颜色</param>
        public static void DrawHomeIcon(Graphics g, Rectangle bounds, Color iconColor)
        {
            using (var pen = new Pen(iconColor, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, iconColor)))
            {
                // 房屋主体
                var houseRect = new Rectangle(bounds.X + bounds.Width / 4, bounds.Y + bounds.Height / 2, 
                    bounds.Width / 2, bounds.Height / 2);
                g.FillRectangle(brush, houseRect);
                g.DrawRectangle(pen, houseRect);
                
                // 屋顶
                var roofPoints = new PointF[]
                {
                    new PointF(bounds.X + bounds.Width * 0.5f, bounds.Y),
                    new PointF(bounds.X, bounds.Y + bounds.Height * 0.5f),
                    new PointF(bounds.Right, bounds.Y + bounds.Height * 0.5f)
                };
                g.FillPolygon(brush, roofPoints);
                g.DrawPolygon(pen, roofPoints);
                
                // 门
                var doorRect = new Rectangle(bounds.X + bounds.Width * 3 / 8, bounds.Y + bounds.Height * 3 / 4,
                    bounds.Width / 4, bounds.Height / 4);
                g.DrawRectangle(pen, doorRect);
            }
        }

        /// <summary>
        /// 绘制设置图标
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="iconColor">图标颜色</param>
        public static void DrawSettingsIcon(Graphics g, Rectangle bounds, Color iconColor)
        {
            // 复用系统管理图标
            DrawSystemIcon(g, bounds, iconColor);
        }

        /// <summary>
        /// 绘制用户图标
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="iconColor">图标颜色</param>
        public static void DrawUserIcon(Graphics g, Rectangle bounds, Color iconColor)
        {
            using (var pen = new Pen(iconColor, 2))
            using (var brush = new SolidBrush(Color.FromArgb(40, iconColor)))
            {
                var centerX = bounds.X + bounds.Width / 2;
                var centerY = bounds.Y + bounds.Height / 2;
                
                // 头部
                var headRadius = bounds.Width / 6;
                var headBounds = new RectangleF(centerX - headRadius, bounds.Y + bounds.Height / 4 - headRadius,
                    headRadius * 2, headRadius * 2);
                g.FillEllipse(brush, headBounds);
                g.DrawEllipse(pen, headBounds);
                
                // 身体
                var bodyWidth = bounds.Width / 2;
                var bodyHeight = bounds.Height / 2;
                var bodyBounds = new RectangleF(centerX - bodyWidth / 2, bounds.Y + bounds.Height / 2,
                    bodyWidth, bodyHeight);
                g.FillEllipse(brush, bodyBounds);
                g.DrawEllipse(pen, bodyBounds);
            }
        }

        #endregion

        #region 辅助方法 (Helper Methods)

        /// <summary>
        /// 绘制通用图标背景
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="backgroundColor">背景颜色</param>
        /// <param name="borderColor">边框颜色</param>
        public static void DrawIconBackground(Graphics g, Rectangle bounds, Color backgroundColor, Color borderColor)
        {
            using (var brush = new SolidBrush(backgroundColor))
            using (var pen = new Pen(borderColor, 1))
            {
                g.FillRectangle(brush, bounds);
                g.DrawRectangle(pen, bounds);
            }
        }

        /// <summary>
        /// 绘制发光图标背景
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bounds">绘制区域</param>
        /// <param name="themeColor">主题颜色</param>
        public static void DrawGlowingIconBackground(Graphics g, Rectangle bounds, Color themeColor)
        {
            // 外层发光
            using (var brush = new SolidBrush(Color.FromArgb(30, themeColor)))
            {
                var glowBounds = new Rectangle(bounds.X - 2, bounds.Y - 2, bounds.Width + 4, bounds.Height + 4);
                g.FillRectangle(brush, glowBounds);
            }
            
            // 内层背景
            using (var brush = new SolidBrush(Color.FromArgb(60, themeColor)))
            using (var pen = new Pen(Color.FromArgb(150, themeColor), 1))
            {
                g.FillRectangle(brush, bounds);
                g.DrawRectangle(pen, bounds);
            }
        }

        #endregion
    }
}
