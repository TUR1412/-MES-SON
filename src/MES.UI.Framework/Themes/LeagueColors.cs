using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 英雄联盟主题颜色定义 - 基于真实LOL客户端
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public static class LeagueColors
    {
        #region 真实LOL主色调 - 金色系

        /// <summary>
        /// LOL标准金色边框 #C8AA6E - 来自真实LOL客户端
        /// </summary>
        public static readonly Color RiotGold = Color.FromArgb(200, 170, 110);

        /// <summary>
        /// LOL悬停金色 #F0E6D2 - 悬停状态
        /// </summary>
        public static readonly Color RiotGoldHover = Color.FromArgb(240, 230, 210);

        /// <summary>
        /// LOL深金色 #795C28 - 边框深色部分
        /// </summary>
        public static readonly Color RiotGoldDark = Color.FromArgb(121, 92, 40);

        /// <summary>
        /// LOL边框金色 #785A28 - 标准边框
        /// </summary>
        public static readonly Color RiotBorderGold = Color.FromArgb(120, 90, 40);

        /// <summary>
        /// LOL按下状态金色 #E4CAA5
        /// </summary>
        public static readonly Color RiotGoldPressed = Color.FromArgb(228, 202, 165);

        // 兼容性别名 - 保持向后兼容
        public static readonly Color PrimaryGold = RiotGold;
        public static readonly Color PrimaryGoldLight = RiotGoldHover;
        public static readonly Color PrimaryGoldDark = RiotGoldDark;
        public static readonly Color BorderGold = RiotBorderGold;
        public static readonly Color BorderGoldHover = RiotGoldHover;

        #endregion

        #region 背景色 - 深色主题

        /// <summary>
        /// 最深背景 - 主窗体背景 #01070D
        /// </summary>
        public static readonly Color DarkestBackground = Color.FromArgb(1, 7, 13);

        /// <summary>
        /// 深色背景 - 面板背景 #0F1A20
        /// </summary>
        public static readonly Color DarkBackground = Color.FromArgb(15, 26, 32);

        /// <summary>
        /// 标准深色 - 控件背景 #1E2328
        /// </summary>
        public static readonly Color DarkSurface = Color.FromArgb(30, 35, 40);

        /// <summary>
        /// 浅深色 - 悬停背景 #141D24
        /// </summary>
        public static readonly Color DarkSurfaceLight = Color.FromArgb(20, 29, 36);

        /// <summary>
        /// 面板深色 - 特殊面板 #0B131B
        /// </summary>
        public static readonly Color DarkPanel = Color.FromArgb(11, 19, 27);

        /// <summary>
        /// 输入框背景 - 文本输入 #000407
        /// </summary>
        public static readonly Color InputBackground = Color.FromArgb(0, 4, 7);

        /// <summary>
        /// 边框深色 - 标准边框 #28282B
        /// </summary>
        public static readonly Color DarkBorder = Color.FromArgb(40, 40, 43);

        /// <summary>
        /// 内阴影 - 深度效果 #CC091119
        /// </summary>
        public static readonly Color InnerShadow = Color.FromArgb(204, 9, 17, 25);

        /// <summary>
        /// 分隔符颜色 - 分隔线 #6B6F73
        /// </summary>
        public static readonly Color SeparatorColor = Color.FromArgb(107, 111, 115);

        #endregion

        #region 强调色 - 蓝色系

        /// <summary>
        /// 强调蓝色 - 用于链接和重要信息
        /// </summary>
        public static readonly Color AccentBlue = Color.FromArgb(0, 150, 255);

        /// <summary>
        /// 浅蓝色 - 用于悬停效果
        /// </summary>
        public static readonly Color AccentBlueLight = Color.FromArgb(50, 180, 255);

        /// <summary>
        /// 深蓝色 - 用于按下效果
        /// </summary>
        public static readonly Color AccentBlueDark = Color.FromArgb(0, 120, 200);

        /// <summary>
        /// 特殊青色 - 进度条和特效 #0AC8B9
        /// </summary>
        public static readonly Color SpecialCyan = Color.FromArgb(10, 200, 185);

        /// <summary>
        /// 箭头蓝色 - 箭头装饰 #1D3B4A
        /// </summary>
        public static readonly Color ArrowBlue = Color.FromArgb(29, 59, 74);

        /// <summary>
        /// 箭头深蓝 - 箭头阴影 #082734
        /// </summary>
        public static readonly Color ArrowBlueDark = Color.FromArgb(8, 39, 52);

        #endregion

        #region 状态色

        /// <summary>
        /// 成功绿色
        /// </summary>
        public static readonly Color SuccessGreen = Color.FromArgb(0, 200, 100);

        /// <summary>
        /// 警告橙色
        /// </summary>
        public static readonly Color WarningOrange = Color.FromArgb(255, 150, 0);

        /// <summary>
        /// 错误红色
        /// </summary>
        public static readonly Color ErrorRed = Color.FromArgb(255, 50, 50);

        #endregion

        #region 文字颜色

        /// <summary>
        /// 主要文字 - 纯白色 #FFFFFF
        /// </summary>
        public static readonly Color TextWhite = Color.FromArgb(255, 255, 255);

        /// <summary>
        /// 高亮文字 - 亮金色 #F0E6D2
        /// </summary>
        public static readonly Color TextHighlight = Color.FromArgb(240, 230, 210);

        /// <summary>
        /// 主要文字 - 标准金色 #CDBE91
        /// </summary>
        public static readonly Color TextPrimary = Color.FromArgb(205, 190, 145);

        /// <summary>
        /// 次要文字 - 灰金色 #A09B8C
        /// </summary>
        public static readonly Color TextSecondary = Color.FromArgb(160, 155, 140);

        /// <summary>
        /// 禁用文字 - 深灰色 #757575
        /// </summary>
        public static readonly Color TextDisabled = Color.FromArgb(117, 117, 117);

        /// <summary>
        /// 特殊金色文字 - 重要信息 #FABE0A
        /// </summary>
        public static readonly Color TextGold = Color.FromArgb(250, 190, 10);

        /// <summary>
        /// 占位符文字 - 深色占位符 #463714
        /// </summary>
        public static readonly Color TextPlaceholder = Color.FromArgb(70, 55, 20);

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取与指定背景色对比度最佳的文字颜色
        /// </summary>
        /// <param name="backgroundColor">背景颜色</param>
        /// <returns>推荐的文字颜色</returns>
        public static Color GetContrastTextColor(Color backgroundColor)
        {
            // 计算背景色的亮度
            double brightness = (backgroundColor.R * 0.299 + backgroundColor.G * 0.587 + backgroundColor.B * 0.114) / 255;
            
            // 根据亮度选择文字颜色
            return brightness > 0.5 ? Color.Black : TextPrimary;
        }

        /// <summary>
        /// 创建指定透明度的颜色
        /// </summary>
        /// <param name="baseColor">基础颜色</param>
        /// <param name="alpha">透明度 (0-255)</param>
        /// <returns>带透明度的颜色</returns>
        public static Color WithAlpha(Color baseColor, int alpha)
        {
            return Color.FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);
        }

        /// <summary>
        /// 创建发光效果颜色 (40%透明度)
        /// </summary>
        /// <param name="color">基础颜色</param>
        /// <returns>发光效果颜色</returns>
        public static Color CreateGlow(Color color)
        {
            return Color.FromArgb(40, color.R, color.G, color.B);
        }

        /// <summary>
        /// 创建悬停效果颜色 (80%透明度)
        /// </summary>
        /// <param name="color">基础颜色</param>
        /// <returns>悬停效果颜色</returns>
        public static Color CreateHover(Color color)
        {
            return Color.FromArgb(80, color.R, color.G, color.B);
        }

        /// <summary>
        /// 创建渐变画刷 - 垂直渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="startColor">起始颜色</param>
        /// <param name="endColor">结束颜色</param>
        /// <returns>垂直渐变画刷</returns>
        public static LinearGradientBrush CreateVerticalGradient(Rectangle bounds, Color startColor, Color endColor)
        {
            return new LinearGradientBrush(bounds, startColor, endColor, LinearGradientMode.Vertical);
        }

        /// <summary>
        /// 创建渐变画刷 - 水平渐变
        /// </summary>
        /// <param name="bounds">绘制区域</param>
        /// <param name="startColor">起始颜色</param>
        /// <param name="endColor">结束颜色</param>
        /// <returns>水平渐变画刷</returns>
        public static LinearGradientBrush CreateHorizontalGradient(Rectangle bounds, Color startColor, Color endColor)
        {
            return new LinearGradientBrush(bounds, startColor, endColor, LinearGradientMode.Horizontal);
        }

        #endregion
    }
}
