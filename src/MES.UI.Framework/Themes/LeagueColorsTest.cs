using System;
using System.Drawing;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 英雄联盟颜色测试类 - 验证颜色定义的正确性
    /// </summary>
    public static class LeagueColorsTest
    {
        /// <summary>
        /// 测试所有颜色定义是否正确
        /// </summary>
        /// <returns>测试结果</returns>
        public static bool TestAllColors()
        {
            try
            {
                // 测试主色调
                var primaryGold = LeagueColors.PrimaryGold;
                var primaryGoldLight = LeagueColors.PrimaryGoldLight;
                var primaryGoldDark = LeagueColors.PrimaryGoldDark;

                // 测试背景色
                var darkBackground = LeagueColors.DarkBackground;
                var darkSurface = LeagueColors.DarkSurface;
                var darkBorder = LeagueColors.DarkBorder;

                // 测试强调色
                var accentBlue = LeagueColors.AccentBlue;
                var accentBlueLight = LeagueColors.AccentBlueLight;
                var accentBlueDark = LeagueColors.AccentBlueDark;

                // 测试状态色
                var successGreen = LeagueColors.SuccessGreen;
                var warningOrange = LeagueColors.WarningOrange;
                var errorRed = LeagueColors.ErrorRed;

                // 测试文字颜色
                var textPrimary = LeagueColors.TextPrimary;
                var textSecondary = LeagueColors.TextSecondary;
                var textDisabled = LeagueColors.TextDisabled;
                var textGold = LeagueColors.TextGold;

                // 测试辅助方法
                var contrastColor = LeagueColors.GetContrastTextColor(darkBackground);
                var alphaColor = LeagueColors.WithAlpha(primaryGold, 128);

                // 验证关键颜色值
                if (primaryGold.R != 120 || primaryGold.G != 90 || primaryGold.B != 40)
                {
                    return false;
                }

                if (darkBackground.R != 1 || darkBackground.G != 10 || darkBackground.B != 19)
                {
                    return false;
                }

                // 验证辅助方法
                if (contrastColor != LeagueColors.TextPrimary)
                {
                    return false;
                }

                if (alphaColor.A != 128)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 显示颜色测试窗体
        /// </summary>
        public static void ShowColorTestForm()
        {
            var form = new Form
            {
                Text = "英雄联盟颜色测试",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = LeagueColors.DarkBackground
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = LeagueColors.DarkSurface
            };

            var label = new Label
            {
                Text = "英雄联盟主题颜色测试",
                Font = new Font("微软雅黑", 14F, FontStyle.Bold),
                ForeColor = LeagueColors.TextGold,
                Location = new Point(20, 20),
                AutoSize = true
            };

            var button = new Button
            {
                Text = "测试按钮",
                BackColor = LeagueColors.PrimaryGold,
                ForeColor = LeagueColors.TextPrimary,
                Location = new Point(20, 60),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat
            };

            panel.Controls.Add(label);
            panel.Controls.Add(button);
            form.Controls.Add(panel);

            form.ShowDialog();
        }
    }
}
