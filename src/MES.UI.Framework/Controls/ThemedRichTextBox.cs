using System;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 原子控件：带主题自适应的 RichTextBox（适用于日志/报告展示）。
    /// </summary>
    public class ThemedRichTextBox : RichTextBox
    {
        private bool _themeHooked = false;

        public ThemedRichTextBox()
        {
            try { DetectUrls = false; } catch { }
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
                BorderStyle = BorderStyle.FixedSingle;
            }
            catch
            {
                // ignore
            }
        }
    }
}

