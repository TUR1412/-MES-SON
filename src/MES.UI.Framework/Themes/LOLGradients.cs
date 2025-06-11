using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// LOL风格渐变系统 - 基于leagueoflegends-wpf分析
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public static class LOLGradients
    {
        #region 金色渐变系统 (Gold Gradient System)

        /// <summary>
        /// 创建标准金色边框渐变 - 垂直方向
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>金色边框渐变画刷</returns>
        public static LinearGradientBrush CreateGoldBorderGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                LeagueColors.PrimaryGold,     // #C8AA6E
                LeagueColors.PrimaryGoldDark, // #795C28
                LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建高亮金色渐变 - 用于悬停状态
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>高亮金色渐变画刷</returns>
        public static LinearGradientBrush CreateGoldHighlightGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                LeagueColors.PrimaryGoldLight, // #F0E6D2
                LeagueColors.PrimaryGold,      // #C8AA6E
                LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建金色文字渐变 - 水平方向
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <returns>金色文字渐变画刷</returns>
        public static LinearGradientBrush CreateGoldTextGradient(PointF startPoint, PointF endPoint)
        {
            return new LinearGradientBrush(startPoint, endPoint,
                LeagueColors.TextHighlight,   // #F0E6D2
                LeagueColors.PrimaryGold);    // #C8AA6E
        }

        #endregion

        #region 深色背景渐变系统 (Dark Background Gradient System)

        /// <summary>
        /// 创建主窗体背景渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>主窗体背景渐变画刷</returns>
        public static LinearGradientBrush CreateMainBackgroundGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                LeagueColors.DarkestBackground, // #01070D
                LeagueColors.DarkBackground,    // #0F1A20
                LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建卡片背景渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>卡片背景渐变画刷</returns>
        public static LinearGradientBrush CreateCardBackgroundGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                Color.FromArgb(30, 35, 40),  // 更深的起始色
                Color.FromArgb(15, 20, 25),  // 更深的结束色
                LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建侧边栏背景渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>侧边栏背景渐变画刷</returns>
        public static LinearGradientBrush CreateSidebarBackgroundGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                Color.FromArgb(5, 10, 15),   // 深色起始
                Color.FromArgb(20, 25, 30),  // 深色结束
                LinearGradientMode.Vertical);
        }

        #endregion

        #region 发光效果渐变系统 (Glow Effect Gradient System)

        /// <summary>
        /// 创建内部发光渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="themeColor">主题颜色</param>
        /// <returns>内部发光渐变画刷</returns>
        public static LinearGradientBrush CreateInnerGlowGradient(Rectangle bounds, Color themeColor)
        {
            return new LinearGradientBrush(bounds,
                Color.FromArgb(25, themeColor.R, themeColor.G, themeColor.B),
                Color.FromArgb(5, themeColor.R, themeColor.G, themeColor.B),
                LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建外部发光渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="themeColor">主题颜色</param>
        /// <returns>外部发光渐变画刷</returns>
        public static LinearGradientBrush CreateOuterGlowGradient(Rectangle bounds, Color themeColor)
        {
            return new LinearGradientBrush(bounds,
                Color.FromArgb(80, themeColor.R, themeColor.G, themeColor.B),
                Color.FromArgb(10, themeColor.R, themeColor.G, themeColor.B),
                LinearGradientMode.Vertical);
        }

        #endregion

        #region 进度条渐变系统 (Progress Bar Gradient System)

        /// <summary>
        /// 创建进度条背景渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>进度条背景渐变画刷</returns>
        public static LinearGradientBrush CreateProgressBackgroundGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                LeagueColors.BorderGold,     // #785A28
                Color.FromArgb(70, 55, 20),  // #463714
                LinearGradientMode.Horizontal);
        }

        /// <summary>
        /// 创建进度条前景渐变 - 青色系
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>进度条前景渐变画刷</returns>
        public static LinearGradientBrush CreateProgressForegroundGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                Color.FromArgb(0, 95, 107),   // #005F6B
                Color.FromArgb(0, 217, 235),  // #00D9EB
                LinearGradientMode.Horizontal);
        }

        #endregion

        #region 按钮渐变系统 (Button Gradient System)

        /// <summary>
        /// 创建按钮正常状态渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <returns>按钮正常状态渐变画刷</returns>
        public static LinearGradientBrush CreateButtonNormalGradient(Rectangle bounds)
        {
            return new LinearGradientBrush(bounds,
                LeagueColors.DarkSurface,      // #1E2328
                LeagueColors.DarkSurfaceLight, // #141D24
                LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建按钮悬停状态渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="themeColor">主题颜色</param>
        /// <returns>按钮悬停状态渐变画刷</returns>
        public static LinearGradientBrush CreateButtonHoverGradient(Rectangle bounds, Color themeColor)
        {
            return new LinearGradientBrush(bounds,
                Color.FromArgb(60, themeColor.R, themeColor.G, themeColor.B),
                Color.FromArgb(30, themeColor.R, themeColor.G, themeColor.B),
                LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建按钮按下状态渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="themeColor">主题颜色</param>
        /// <returns>按钮按下状态渐变画刷</returns>
        public static LinearGradientBrush CreateButtonPressedGradient(Rectangle bounds, Color themeColor)
        {
            return new LinearGradientBrush(bounds,
                Color.FromArgb(40, themeColor.R, themeColor.G, themeColor.B),
                Color.FromArgb(80, themeColor.R, themeColor.G, themeColor.B),
                LinearGradientMode.Vertical);
        }

        #endregion

        #region 箭头装饰渐变系统 (Arrow Decoration Gradient System)

        /// <summary>
        /// 创建箭头填充渐变 - LOL蓝色系
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <returns>箭头填充渐变画刷</returns>
        public static LinearGradientBrush CreateArrowFillGradient(PointF startPoint, PointF endPoint)
        {
            return new LinearGradientBrush(startPoint, endPoint,
                LeagueColors.ArrowBlue,     // #1D3B4A
                LeagueColors.ArrowBlueDark); // #082734
        }

        /// <summary>
        /// 创建箭头边框渐变 - LOL青色系
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <returns>箭头边框渐变画刷</returns>
        public static LinearGradientBrush CreateArrowBorderGradient(PointF startPoint, PointF endPoint)
        {
            return new LinearGradientBrush(startPoint, endPoint,
                Color.FromArgb(175, 245, 255), // #AFF5FF
                Color.FromArgb(70, 230, 255));  // #46E6FF
        }

        #endregion

        #region 辅助方法 (Helper Methods)

        /// <summary>
        /// 创建多层渐变画刷 - 5个渐变点
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="colors">颜色数组 (需要5个颜色)</param>
        /// <param name="mode">渐变模式</param>
        /// <returns>多层渐变画刷</returns>
        public static LinearGradientBrush CreateMultiLayerGradient(Rectangle bounds, Color[] colors, LinearGradientMode mode)
        {
            if (colors.Length != 5)
                throw new ArgumentException("多层渐变需要5个颜色");

            var brush = new LinearGradientBrush(bounds, colors[0], colors[4], mode);
            
            var blend = new ColorBlend();
            blend.Colors = colors;
            blend.Positions = new float[] { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };
            brush.InterpolationColors = blend;
            
            return brush;
        }

        /// <summary>
        /// 创建径向渐变画刷 - 发光效果
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="centerColor">中心颜色</param>
        /// <param name="edgeColor">边缘颜色</param>
        /// <returns>径向渐变画刷</returns>
        public static PathGradientBrush CreateRadialGlowGradient(Rectangle bounds, Color centerColor, Color edgeColor)
        {
            var path = new GraphicsPath();
            path.AddEllipse(bounds);
            
            var brush = new PathGradientBrush(path);
            brush.CenterColor = centerColor;
            brush.SurroundColors = new Color[] { edgeColor };
            
            return brush;
        }

        #endregion
    }
}
