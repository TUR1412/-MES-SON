using System.Drawing;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 设计系统 Token：
    /// - 统一字号、圆角、阴影与动效时长等基础视觉参数
    /// - 作为“默认值”供主题应用器与控件基座复用
    /// </summary>
    public static class DesignTokens
    {
        public static class Typography
        {
            public const float BaseSize = 9F;
            public const float TitleSize = 10F;
            public const float CaptionSize = 8F;

            public static Font CreateBaseFont()
            {
                return CreateFont(BaseSize, FontStyle.Regular);
            }

            public static Font CreateTitleFont()
            {
                return CreateFont(TitleSize, FontStyle.Bold);
            }

            public static Font CreateCaptionFont()
            {
                return CreateFont(CaptionSize, FontStyle.Regular);
            }

            public static Font CreateFont(float size, FontStyle style)
            {
                // 以系统默认 UI 字体为基准，避免硬编码字体在不同语言/系统下缺失。
                var family = SystemFonts.MessageBoxFont.FontFamily;
                return new Font(family, size, style);
            }
        }

        public static class Radius
        {
            public const int Sm = 4;
            public const int Md = 8;
            public const int Lg = 12;
        }

        public static class Motion
        {
            public const int FastMs = 120;
            public const int NormalMs = 180;
            public const int SlowMs = 240;
        }

        public static class Elevation
        {
            public static readonly Color ShadowColor = Color.FromArgb(90, 0, 0, 0);
            public const int ShadowBlur = 12;
            public const int ShadowOffsetX = 0;
            public const int ShadowOffsetY = 4;
        }
    }
}

