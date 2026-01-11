using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 原子控件：带主题自适应与占位提示的 TextBox（Win32 Cue Banner）
    /// </summary>
    public class ThemedTextBox : TextBox
    {
        private const int EM_SETCUEBANNER = 0x1501;

        private string _placeholderText = string.Empty;
        private bool _themeHooked = false;

        /// <summary>
        /// 占位提示文本（无需额外控件）
        /// </summary>
        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = value ?? string.Empty;
                ApplyCueBanner();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyTheme();
            ApplyCueBanner();
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

        private void ApplyCueBanner()
        {
            try
            {
                if (!IsHandleCreated) return;
                SendMessage(Handle, EM_SETCUEBANNER, new IntPtr(1), _placeholderText);
            }
            catch
            {
                // ignore
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
    }
}

