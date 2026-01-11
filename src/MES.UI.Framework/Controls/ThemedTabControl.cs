using System;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 原子控件：主题自适应 TabControl（OwnerDraw），用于避免深色主题下 Tab 页签不一致。
    /// </summary>
    public class ThemedTabControl : TabControl
    {
        private bool _themeHooked = false;

        public ThemedTabControl()
        {
            try
            {
                DrawMode = TabDrawMode.OwnerDrawFixed;
                SizeMode = TabSizeMode.Fixed;
                ItemSize = new Size(200, 34);
                Padding = new Point(12, 6);
            }
            catch
            {
                // ignore
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyTheme();
            HookThemeChanged();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnhookThemeChanged();
            }
            base.Dispose(disposing);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                if (e == null) return;
                if (e.Index < 0 || e.Index >= TabPages.Count) return;

                var colors = UIThemeManager.Colors;
                bool selected = (e.Index == SelectedIndex);

                var rect = GetTabRect(e.Index);
                rect.Inflate(-1, -1);

                var back = selected ? colors.Surface : colors.Background;
                var text = selected ? colors.Text : colors.TextSecondary;
                var border = selected ? colors.Primary : colors.Border;

                using (var brush = new SolidBrush(back))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }

                using (var pen = new Pen(border, selected ? 2f : 1f))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }

                // 底部强调条，让选中态更明显
                if (selected)
                {
                    var barRect = new Rectangle(rect.X + 2, rect.Bottom - 4, rect.Width - 4, 3);
                    using (var brush = new SolidBrush(colors.Primary))
                    {
                        e.Graphics.FillRectangle(brush, barRect);
                    }
                }

                var textRect = new Rectangle(rect.X + 10, rect.Y + 6, rect.Width - 20, rect.Height - 12);
                TextRenderer.DrawText(
                    e.Graphics,
                    TabPages[e.Index].Text ?? string.Empty,
                    Font,
                    textRect,
                    text,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            }
            catch
            {
                // ignore
            }
        }

        private void HookThemeChanged()
        {
            if (_themeHooked) return;
            _themeHooked = true;
            UIThemeManager.OnThemeChanged += HandleThemeChanged;
        }

        private void UnhookThemeChanged()
        {
            if (!_themeHooked) return;
            _themeHooked = false;
            try { UIThemeManager.OnThemeChanged -= HandleThemeChanged; } catch { }
        }

        private void HandleThemeChanged()
        {
            try
            {
                if (IsDisposed) return;
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(ApplyTheme));
                    return;
                }

                ApplyTheme();
            }
            catch
            {
                // ignore
            }
        }

        private void ApplyTheme()
        {
            try
            {
                var colors = UIThemeManager.Colors;
                BackColor = colors.Background;
                ForeColor = colors.Text;
                Font = UIThemeManager.GetFont(9f);
                Invalidate();
            }
            catch
            {
                // ignore
            }
        }
    }
}

