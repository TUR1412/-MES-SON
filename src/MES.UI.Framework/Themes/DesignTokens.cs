using System;
using System.Drawing;
using System.Linq;
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
            public const float BaseSize = 10F;
            public const float TitleSize = 12F;
            public const float CaptionSize = 8.5F;

            private static readonly string[] PreferredFontNames = new[]
            {
                "HarmonyOS Sans SC",
                "Source Han Sans SC",
                "Microsoft YaHei UI",
                "PingFang SC",
                "Segoe UI"
            };

            private static FontFamily _cachedFamily;

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
                var family = ResolvePreferredFamily();
                return new Font(family, size, style);
            }

            private static FontFamily ResolvePreferredFamily()
            {
                if (_cachedFamily != null)
                {
                    return _cachedFamily;
                }

                try
                {
                    foreach (var name in PreferredFontNames)
                    {
                        if (FontFamily.Families.Any(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase)))
                        {
                            _cachedFamily = new FontFamily(name);
                            return _cachedFamily;
                        }
                    }
                }
                catch
                {
                    // ignore
                }

                _cachedFamily = SystemFonts.MessageBoxFont.FontFamily;
                return _cachedFamily;
            }
        }

        public static class Radius
        {
            public const int Sm = 6;
            public const int Md = 12;
            public const int Lg = 18;
        }

        public static class Motion
        {
            public const int FastMs = 120;
            public const int NormalMs = 180;
            public const int SlowMs = 240;
        }

        public static class Elevation
        {
            public static readonly Color ShadowColor = Color.FromArgb(110, 0, 0, 0);
            public const int ShadowBlur = 16;
            public const int ShadowOffsetX = 0;
            public const int ShadowOffsetY = 6;
        }
    }
}



