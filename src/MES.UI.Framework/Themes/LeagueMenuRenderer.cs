using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 英雄联盟风格菜单渲染器
    /// </summary>
    public class LeagueMenuRenderer : ToolStripProfessionalRenderer
    {
        public LeagueMenuRenderer() : base(new LeagueColorTable())
        {
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
            {
                // 绘制英雄联盟风格的选中/按下背景
                using (var brush = new LinearGradientBrush(
                    e.Item.Bounds,
                    Color.FromArgb(80, LeagueColors.PrimaryGold),
                    Color.FromArgb(40, LeagueColors.PrimaryGoldLight),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, e.Item.Bounds);
                }

                // 绘制发光边框
                using (var pen = new Pen(Color.FromArgb(120, LeagueColors.TextGold), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(
                        e.Item.Bounds.X, e.Item.Bounds.Y,
                        e.Item.Bounds.Width - 1, e.Item.Bounds.Height - 1));
                }
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            // 绘制英雄联盟风格菜单栏背景
            LeagueVisualEffects.DrawLeagueMenuBar(e.Graphics, e.AffectedBounds);
        }
    }

    /// <summary>
    /// 英雄联盟风格工具栏渲染器
    /// </summary>
    public class LeagueToolStripRenderer : ToolStripProfessionalRenderer
    {
        public LeagueToolStripRenderer() : base(new LeagueColorTable())
        {
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var button = e.Item as ToolStripButton;
            if (button != null && (button.Selected || button.Pressed))
            {
                // 绘制英雄联盟风格按钮背景
                using (var brush = new LinearGradientBrush(
                    e.Item.Bounds,
                    Color.FromArgb(60, LeagueColors.PrimaryGold),
                    Color.FromArgb(30, LeagueColors.PrimaryGoldDark),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, e.Item.Bounds);
                }

                // 绘制边框
                using (var pen = new Pen(Color.FromArgb(100, LeagueColors.TextGold), 1))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(
                        e.Item.Bounds.X, e.Item.Bounds.Y,
                        e.Item.Bounds.Width - 1, e.Item.Bounds.Height - 1));
                }
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            // 绘制工具栏背景
            using (var brush = new LinearGradientBrush(
                e.AffectedBounds,
                Color.FromArgb(20, 25, 35),
                Color.FromArgb(25, 30, 40),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }

            // 绘制底部金色分隔线
            using (var pen = new Pen(Color.FromArgb(80, LeagueColors.PrimaryGold), 1))
            {
                e.Graphics.DrawLine(pen, 
                    e.AffectedBounds.X, e.AffectedBounds.Bottom - 1,
                    e.AffectedBounds.Right, e.AffectedBounds.Bottom - 1);
            }
        }
    }

    /// <summary>
    /// 英雄联盟颜色表
    /// </summary>
    public class LeagueColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(80, LeagueColors.PrimaryGold);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(80, LeagueColors.PrimaryGold);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(40, LeagueColors.PrimaryGoldLight);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(100, LeagueColors.PrimaryGoldDark);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(60, LeagueColors.PrimaryGold);
        public override Color MenuItemBorder => Color.FromArgb(120, LeagueColors.TextGold);
        public override Color MenuBorder => Color.FromArgb(80, LeagueColors.PrimaryGold);
        public override Color ToolStripDropDownBackground => Color.FromArgb(25, 30, 40);
        public override Color ImageMarginGradientBegin => Color.FromArgb(20, 25, 35);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(22, 27, 37);
        public override Color ImageMarginGradientEnd => Color.FromArgb(25, 30, 40);
        public override Color SeparatorDark => Color.FromArgb(60, LeagueColors.PrimaryGold);
        public override Color SeparatorLight => Color.FromArgb(30, LeagueColors.PrimaryGoldLight);
    }
}
