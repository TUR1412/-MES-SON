using System;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 原子控件：带主题自适应与双缓冲的 ListBox（降低闪烁）
    /// </summary>
    public class ThemedListBox : ListBox
    {
        private bool _themeHooked = false;

        public ThemedListBox()
        {
            try
            {
                // ListBox 在 OwnerDraw 模式下容易闪烁，开启双缓冲能显著改善
                DoubleBuffered = true;
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
                BackColor = colors.Surface;
                ForeColor = colors.Text;
            }
            catch
            {
                // ignore
            }
        }
    }
}

