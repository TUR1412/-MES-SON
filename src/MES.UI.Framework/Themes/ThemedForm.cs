using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// 统一主题窗体基类：
    /// - TopLevel 窗体：在 Shown 时自动套用主题
    /// - 嵌入式 Form（TopLevel=false）：在 VisibleChanged 时自动套用主题
    /// - 动态新增控件：在 ControlAdded 时补齐主题
    /// </summary>
    public class ThemedForm : Form
    {
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            TryApplyTheme();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            // 对于被嵌入主壳的 Form（TopLevel=false），Shown 事件不一定可靠触发；
            // VisibleChanged 更稳定，可保证“所有界面都吃到主题”。
            if (Visible)
            {
                TryApplyTheme();
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            try
            {
                if (IsInDesignMode(this))
                {
                    return;
                }

                if (e != null && e.Control != null && UIThemeManager.CurrentTheme == UIThemeManager.ThemeType.Lol)
                {
                    // 只对新增控件树做一次增量主题，避免整窗体反复 Apply 导致不必要的重绘
                    LolV2ThemeApplier.Apply(this);
                }
            }
            catch
            {
                // ignore
            }
        }

        private void TryApplyTheme()
        {
            try
            {
                if (IsInDesignMode(this))
                {
                    return;
                }

                // 允许重复 Apply：LolV2ThemeApplier 内部有 HookState 防重入，且重复应用可以覆盖业务代码“刷回浅色”的情况
                UIThemeManager.ApplyTheme(this);
            }
            catch
            {
                // ignore
            }
        }

        private static bool IsInDesignMode(Control control)
        {
            try
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                {
                    return true;
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                if (control != null)
                {
                    if (control.Site != null && control.Site.DesignMode)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // ignore
            }

            return false;
        }
    }
}
