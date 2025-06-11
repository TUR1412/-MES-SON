using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 英雄联盟风格按钮控件
    /// 具有金属质感、渐变光效、边框装饰的真正英雄联盟视觉效果
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public class LeagueButton : Button
    {
        private bool isHovered = false;
        private bool isPressed = false;

        public LeagueButton()
        {
            InitializeLeagueButton();
        }

        /// <summary>
        /// 初始化英雄联盟按钮
        /// </summary>
        private void InitializeLeagueButton()
        {
            // 设置基本属性
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Transparent;
            this.ForeColor = LeagueColors.TextPrimary;
            this.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            this.Size = new Size(120, 35);
            this.Cursor = Cursors.Hand;

            // 禁用默认绘制
            this.UseVisualStyleBackColor = false;
        }

        /// <summary>
        /// 重写绘制方法
        /// </summary>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            // 使用英雄联盟视觉特效绘制
            LeagueVisualEffects.DrawLeagueButton(
                pevent.Graphics,
                this.ClientRectangle,
                isHovered,
                isPressed,
                this.Text,
                this.Font);
        }

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            isPressed = true;
            this.Invalidate();
            base.OnMouseDown(mevent);
        }

        /// <summary>
        /// 鼠标释放事件
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            isPressed = false;
            this.Invalidate();
            base.OnMouseUp(mevent);
        }

        /// <summary>
        /// 获得焦点事件
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            this.Invalidate();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// 失去焦点事件
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            this.Invalidate();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// 文字改变事件
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnTextChanged(e);
        }

        /// <summary>
        /// 字体改变事件
        /// </summary>
        protected override void OnFontChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnFontChanged(e);
        }

        /// <summary>
        /// 大小改变事件
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// 禁用默认背景绘制
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // 不绘制背景，由OnPaint处理
        }
    }

    /// <summary>
    /// 英雄联盟风格面板控件
    /// </summary>
    public class LeaguePanel : Panel
    {
        public LeaguePanel()
        {
            InitializeLeaguePanel();
        }

        /// <summary>
        /// 初始化英雄联盟面板
        /// </summary>
        private void InitializeLeaguePanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 重写绘制方法
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            LeagueVisualEffects.DrawLeaguePanel(e.Graphics, this.ClientRectangle);
            base.OnPaint(e);
        }

        /// <summary>
        /// 禁用默认背景绘制
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 不绘制背景，由OnPaint处理
        }
    }

    /// <summary>
    /// 英雄联盟风格菜单栏控件
    /// </summary>
    public class LeagueMenuStrip : MenuStrip
    {
        public LeagueMenuStrip()
        {
            InitializeLeagueMenuStrip();
        }

        /// <summary>
        /// 初始化英雄联盟菜单栏
        /// </summary>
        private void InitializeLeagueMenuStrip()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Transparent;
            this.ForeColor = LeagueColors.TextPrimary;
            this.Font = new Font("微软雅黑", 9F);
            this.Renderer = new LeagueMenuRenderer();
        }

        /// <summary>
        /// 重写绘制方法
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            LeagueVisualEffects.DrawLeagueMenuBar(e.Graphics, this.ClientRectangle);
            base.OnPaint(e);
        }

        /// <summary>
        /// 禁用默认背景绘制
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 不绘制背景，由OnPaint处理
        }
    }

    /// <summary>
    /// 英雄联盟风格菜单渲染器
    /// </summary>
    public class LeagueMenuRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected || e.Item.Pressed)
            {
                var bounds = new Rectangle(Point.Empty, e.Item.Size);
                using (var brush = new SolidBrush(Color.FromArgb(50, LeagueColors.PrimaryGold)))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                }

                using (var pen = new Pen(LeagueColors.PrimaryGold, 1))
                {
                    e.Graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                }
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            // 不绘制背景，由控件自己处理
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // 不绘制边框，由控件自己处理
        }
    }
}
