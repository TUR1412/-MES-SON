using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MES.UI.Framework.Controls;

namespace MES.UI.Framework.Themes
{
    /// <summary>
    /// LoL 客户端暗金风主题应用器（V2）
    /// 目标：低侵入统一视觉（暗底/金描边/按钮悬停反馈），尽量不破坏既有布局与事件绑定。
    /// </summary>
    public static class LolV2ThemeApplier
    {
        private sealed class HookState
        {
            public bool BackgroundPaintHooked;
            public bool DataGridViewHooked;
            public bool PanelHooked;
            public bool GroupBoxHooked;
            public bool ToolStripHooked;
            public bool ComboBoxHooked;
            public bool ComboBoxPopupHooked;
            public IntPtr ComboListHandle;
            public ComboListBoxNativeWindow ComboListWindow;
            public bool InputFrameHooked;
            public bool InputInteractionHooked;
            public bool NativeThemeDisabled;
            public bool DateTimePickerPopupHooked;
        }

        private static readonly object HooksLock = new object();
        private static readonly ConditionalWeakTable<Control, HookState> Hooks = new ConditionalWeakTable<Control, HookState>();

        // WinForms 原生控件在 Windows 10/11 下会强制使用系统视觉主题，导致“白边/白底输入框”割裂。
        // 通过禁用控件的 native theme（uxtheme）让 BackColor/OwnerDraw 更稳定。
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref COMBOBOXINFO info);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COMBOBOXINFO
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndItem;
            public IntPtr hwndList;
        }

        private const int WM_ERASEBKGND = 0x0014;
        private const int DTM_FIRST = 0x1000;
        private const int DTM_GETMONTHCAL = DTM_FIRST + 8;
        private const int MCM_FIRST = 0x1000;
        private const int MCM_SETCOLOR = MCM_FIRST + 10;

        private const int MCSC_BACKGROUND = 0;
        private const int MCSC_TEXT = 1;
        private const int MCSC_TITLEBK = 2;
        private const int MCSC_TITLETEXT = 3;
        private const int MCSC_MONTHBK = 4;
        private const int MCSC_TRAILINGTEXT = 5;

        private sealed class ComboListBoxNativeWindow : NativeWindow
        {
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_ERASEBKGND)
                {
                    try
                    {
                        var hdc = m.WParam;
                        if (hdc != IntPtr.Zero)
                        {
                            using (var g = Graphics.FromHdc(hdc))
                            {
                                g.Clear(LeagueColors.InputBackground);
                            }
                            m.Result = (IntPtr)1;
                            return;
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }

                base.WndProc(ref m);
            }
        }

        public static void Apply(Form form)
        {
            if (form == null) return;

            try
            {
                ApplyFormBaseStyle(form);
                ApplyControlsRecursive(form.Controls);
            }
            catch
            {
                // 主题应用失败不应阻塞业务
            }
        }

        private static void ApplyFormBaseStyle(Form form)
        {
            form.BackColor = LeagueColors.DarkBackground;
            form.ForeColor = LeagueColors.TextPrimary;
            form.Font = new Font("微软雅黑", 9F, FontStyle.Regular);

            EnableDoubleBuffering(form);

            var state = GetHookState(form);
            if (state != null && !state.BackgroundPaintHooked)
            {
                state.BackgroundPaintHooked = true;
                form.Paint += (s, e) =>
                {
                    try
                    {
                        LolClientVisuals.DrawClientBackground(e.Graphics, form.ClientRectangle);
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
        }

        private static void ApplyControlsRecursive(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                try
                {
                    ApplySingleControl(control);
                }
                catch
                {
                    // 单个控件主题失败不应影响其它控件
                }

                if (control.HasChildren)
                {
                    ApplyControlsRecursive(control.Controls);
                }
            }
        }

        private static void ApplySingleControl(Control control)
        {
            if (control == null) return;

            // 右键菜单（ContextMenuStrip）不是 Controls 树的一部分，必须额外处理
            try
            {
                if (control.ContextMenuStrip != null)
                {
                    ApplyContextMenuStrip(control.ContextMenuStrip);
                }
            }
            catch
            {
                // ignore
            }

            // 先处理一些“自绘控件/特殊控件”，避免被覆盖
            if (control is LolNavButton || control is LolCardButton || control is LolActionButton || control is ModernButton || control is LeagueButton)
            {
                // 自己绘制，不做二次主题覆盖
                return;
            }

            // TabPage 继承自 Panel，必须优先处理（否则 UseVisualStyleBackColor 会导致 BackColor 失效，看起来像“主题没应用”）
            var tabPage = control as TabPage;
            if (tabPage != null)
            {
                try
                {
                    tabPage.UseVisualStyleBackColor = false;
                }
                catch
                {
                    // ignore
                }

                tabPage.BackColor = LeagueColors.DarkSurface;
                tabPage.ForeColor = LeagueColors.TextPrimary;
                EnableDoubleBuffering(tabPage);
                return;
            }

            // 容器类
            var panel = control as Panel;
            if (panel != null)
            {
                ApplyPanel(panel);
                return;
            }

            var groupBox = control as GroupBox;
            if (groupBox != null)
            {
                ApplyGroupBox(groupBox);
                return;
            }

            var tableLayout = control as TableLayoutPanel;
            if (tableLayout != null)
            {
                tableLayout.BackColor = Color.Transparent;
                tableLayout.ForeColor = LeagueColors.TextPrimary;
                EnableDoubleBuffering(tableLayout);
                return;
            }

            var splitContainer = control as SplitContainer;
            if (splitContainer != null)
            {
                // SplitterBar 的颜色主要取决于 SplitContainer 自身 BackColor
                splitContainer.BackColor = LeagueColors.DarkBorder;
                splitContainer.ForeColor = LeagueColors.TextPrimary;
                EnableDoubleBuffering(splitContainer);
                return;
            }

            var tabControl = control as TabControl;
            if (tabControl != null)
            {
                tabControl.BackColor = LeagueColors.DarkSurface;
                tabControl.ForeColor = LeagueColors.TextPrimary;
                return;
            }

            // 基础文本
            var label = control as Label;
            if (label != null)
            {
                label.ForeColor = LeagueColors.TextPrimary;
                if (LooksLight(label.BackColor) || label.BackColor == SystemColors.Control)
                {
                    label.BackColor = Color.Transparent;
                }
                return;
            }

            // 输入控件
            var textBox = control as TextBox;
            if (textBox != null)
            {
                ApplyTextBox(textBox);
                return;
            }

            var richTextBox = control as RichTextBox;
            if (richTextBox != null)
            {
                richTextBox.BackColor = LeagueColors.InputBackground;
                richTextBox.ForeColor = Color.FromArgb(241, 241, 241);
                richTextBox.BorderStyle = BorderStyle.FixedSingle;
                return;
            }

            var comboBox = control as ComboBox;
            if (comboBox != null)
            {
                ApplyComboBox(comboBox);
                return;
            }

            var dateTimePicker = control as DateTimePicker;
            if (dateTimePicker != null)
            {
                DisableNativeTheme(dateTimePicker);
                dateTimePicker.CalendarForeColor = Color.FromArgb(241, 241, 241);
                dateTimePicker.CalendarMonthBackground = LeagueColors.InputBackground;
                dateTimePicker.CalendarTitleBackColor = LeagueColors.DarkBackground;
                dateTimePicker.CalendarTitleForeColor = LeagueColors.TextHighlight;
                dateTimePicker.CalendarTrailingForeColor = LeagueColors.TextDisabled;
                dateTimePicker.BackColor = LeagueColors.InputBackground;
                dateTimePicker.ForeColor = Color.FromArgb(241, 241, 241);
                if (!dateTimePicker.Enabled)
                {
                    dateTimePicker.BackColor = LeagueColors.DarkSurfaceLight;
                    dateTimePicker.ForeColor = LeagueColors.TextDisabled;
                }
                HookDateTimePickerPopupTheme(dateTimePicker);
                EnsureInputFrameAndInteraction(dateTimePicker);
                return;
            }

            var numericUpDown = control as NumericUpDown;
            if (numericUpDown != null)
            {
                DisableNativeTheme(numericUpDown);
                numericUpDown.BackColor = LeagueColors.InputBackground;
                numericUpDown.ForeColor = Color.FromArgb(241, 241, 241);
                if (!numericUpDown.Enabled)
                {
                    numericUpDown.BackColor = LeagueColors.DarkSurfaceLight;
                    numericUpDown.ForeColor = LeagueColors.TextDisabled;
                }
                EnsureInputFrameAndInteraction(numericUpDown);
                return;
            }

            // 列表/树/表格
            var dataGridView = control as DataGridView;
            if (dataGridView != null)
            {
                ApplyDataGridView(dataGridView);
                return;
            }

            var treeView = control as TreeView;
            if (treeView != null)
            {
                treeView.BackColor = LeagueColors.DarkBackground;
                treeView.ForeColor = LeagueColors.TextPrimary;
                treeView.LineColor = Color.FromArgb(80, LeagueColors.SeparatorColor);
                return;
            }

            var listView = control as ListView;
            if (listView != null)
            {
                listView.BackColor = LeagueColors.DarkSurface;
                listView.ForeColor = LeagueColors.TextPrimary;
                return;
            }

            // 工具条/菜单
            var menuStrip = control as MenuStrip;
            if (menuStrip != null)
            {
                ApplyMenuStrip(menuStrip);
                return;
            }

            var toolStrip = control as ToolStrip;
            if (toolStrip != null)
            {
                ApplyToolStripBase(toolStrip);
                return;
            }

            var statusStrip = control as StatusStrip;
            if (statusStrip != null)
            {
                ApplyToolStripBase(statusStrip);
                return;
            }

            // 按钮
            var button = control as Button;
            if (button != null)
            {
                ApplyButton(button);
                return;
            }

            // 勾选/单选
            var checkBox = control as CheckBox;
            if (checkBox != null)
            {
                checkBox.BackColor = Color.Transparent;
                checkBox.ForeColor = LeagueColors.TextPrimary;
                checkBox.FlatStyle = FlatStyle.Flat;
                checkBox.FlatAppearance.BorderColor = LeagueColors.RiotBorderGold;
                return;
            }

            var radioButton = control as RadioButton;
            if (radioButton != null)
            {
                radioButton.BackColor = Color.Transparent;
                radioButton.ForeColor = LeagueColors.TextPrimary;
                return;
            }

            // 默认兜底：确保文字颜色不会“黑底黑字/白底白字”
            if (control.ForeColor == Color.Black || control.ForeColor == SystemColors.ControlText)
            {
                control.ForeColor = LeagueColors.TextPrimary;
            }
            if (LooksLight(control.BackColor) || control.BackColor == SystemColors.Control)
            {
                control.BackColor = LeagueColors.DarkSurface;
            }
        }

        private static bool LooksLight(Color c)
        {
            try
            {
                if (c.A == 0) return false;
                if (c == Color.Transparent) return false;

                // 使用相对亮度（比硬编码颜色更稳）
                var lum = (0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B) / 255.0;
                return lum > 0.78;
            }
            catch
            {
                return false;
            }
        }

        private static bool LooksDark(Color c)
        {
            try
            {
                if (c.A == 0) return false;
                if (c == Color.Transparent) return false;

                var lum = (0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B) / 255.0;
                return lum < 0.35;
            }
            catch
            {
                return false;
            }
        }

        private static void ApplyPanel(Panel panel)
        {
            panel.ForeColor = LeagueColors.TextPrimary;

            // 常见设计器默认是白色/浅灰（如 #F8F9FA）；统一压到暗底或透明（让父容器暗底透出来）
            if (LooksLight(panel.BackColor) || panel.BackColor == SystemColors.Control || panel.BackColor == Color.FromArgb(245, 246, 250))
            {
                if (panel.Parent != null && LooksDark(panel.Parent.BackColor))
                {
                    // 经验法则：带明确语义的“功能区面板”（Header/Search/Buttons/...）应该是暗色卡片；
                    // 纯布局面板才用透明，避免“满屏卡片底”。
                    panel.BackColor = LooksLikeSectionPanel(panel) ? LeagueColors.DarkSurface : Color.Transparent;
                }
                else
                {
                    panel.BackColor = LeagueColors.DarkSurface;
                }
            }

            EnableDoubleBuffering(panel);

            // 对“看起来像卡片/分组容器”的 Panel 增加轻描边（不改布局）
            var state = GetHookState(panel);
            if (state != null && !state.PanelHooked)
            {
                state.PanelHooked = true;
                panel.Paint += (s, e) =>
                {
                    try
                    {
                        var rect = panel.ClientRectangle;
                        rect.Width -= 1;
                        rect.Height -= 1;

                        // 背景（透明面板由父容器负责绘制）
                        if (panel.BackColor != Color.Transparent && panel.BackColor.A > 0)
                        {
                            var oldS = e.Graphics.SmoothingMode;
                            try
                            {
                                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            }
                            catch
                            {
                                // ignore
                            }

                            using (var bg = new SolidBrush(panel.BackColor))
                            using (var fillPath = CreateChamferedRectPath(rect, 6))
                            {
                                e.Graphics.FillPath(bg, fillPath);
                            }

                            try
                            {
                                e.Graphics.SmoothingMode = oldS;
                            }
                            catch
                            {
                                // ignore
                            }
                        }

                        // 透明面板大多用于布局，不画描边，避免“满屏框线”
                        if (panel.BackColor == Color.Transparent)
                        {
                            return;
                        }

                        // 描边：LoL 客户端常见的“切角”轮廓（比纯矩形更像）
                        var old = e.Graphics.SmoothingMode;
                        try
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        }
                        catch
                        {
                            // ignore
                        }

                        using (var path = CreateChamferedRectPath(rect, 6))
                        using (var pen = new Pen(Color.FromArgb(90, LeagueColors.RiotBorderGold), 1))
                        {
                            e.Graphics.DrawPath(pen, path);
                        }

                        try
                        {
                            e.Graphics.SmoothingMode = old;
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
        }

        private static bool LooksLikeSectionPanel(Panel panel)
        {
            if (panel == null) return false;

            string name = string.Empty;
            try { name = panel.Name ?? string.Empty; } catch { name = string.Empty; }
            if (string.IsNullOrWhiteSpace(name)) return false;

            name = name.ToLowerInvariant();

            // 常见命名：panelHeader / panelSearch / panelButtons / panelContent / panelLeft / panelRight / panelDetails ...
            return name.Contains("header")
                   || name.Contains("search")
                   || name.Contains("button")
                   || name.Contains("toolbar")
                   || name.Contains("filter")
                   || name.Contains("content")
                   || name.Contains("detail")
                   || name.Contains("left")
                   || name.Contains("right")
                   || name.Contains("footer");
        }

        private static void ApplyGroupBox(GroupBox groupBox)
        {
            if (groupBox == null) return;

            groupBox.BackColor = LeagueColors.DarkSurface;
            groupBox.ForeColor = LeagueColors.RiotGoldHover;
            groupBox.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);

            EnableDoubleBuffering(groupBox);

            var state = GetHookState(groupBox);
            if (state != null && !state.GroupBoxHooked)
            {
                state.GroupBoxHooked = true;
                groupBox.Paint += (s, e) =>
                {
                    try
                    {
                        var g = e.Graphics;
                        var rect = groupBox.ClientRectangle;
                        if (rect.Width <= 2 || rect.Height <= 2) return;

                        rect.Width -= 1;
                        rect.Height -= 1;

                        var oldS = g.SmoothingMode;
                        try
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                        }
                        catch
                        {
                            // ignore
                        }

                        // 背景填充（避免系统 GroupBox 灰边/浅底残留）
                        using (var bg = new SolidBrush(groupBox.BackColor))
                        using (var fillPath = CreateChamferedRectPath(rect, 6))
                        {
                            g.FillPath(bg, fillPath);
                        }

                        // 边框（暗金切角轮廓）
                        using (var path = CreateChamferedRectPath(rect, 6))
                        using (var pen = new Pen(Color.FromArgb(120, LeagueColors.RiotBorderGold), 1))
                        {
                            g.DrawPath(pen, path);
                        }

                        // 标题：覆盖边框线并重新绘制文本（更像 LoL 客户端分区标题）
                        var title = groupBox.Text;
                        if (!string.IsNullOrWhiteSpace(title))
                        {
                            var titleX = rect.Left + 12;
                            var titleY = rect.Top + 2;

                            var titleSize = TextRenderer.MeasureText(g, title, groupBox.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                            var cover = new Rectangle(titleX - 4, titleY - 2, Math.Min(rect.Width - 16, titleSize.Width + 8), titleSize.Height);

                            using (var coverBrush = new SolidBrush(groupBox.BackColor))
                            {
                                g.FillRectangle(coverBrush, cover);
                            }

                            TextRenderer.DrawText(g, title, groupBox.Font, new Point(titleX, titleY), groupBox.ForeColor, TextFormatFlags.Left | TextFormatFlags.NoPrefix);

                            // 标题下方细线（微层级）
                            var underlineWidth = Math.Min(70, cover.Width);
                            using (var underline = new Pen(Color.FromArgb(110, LeagueColors.RiotBorderGold), 1))
                            {
                                g.DrawLine(underline, titleX, titleY + titleSize.Height - 1, titleX + underlineWidth, titleY + titleSize.Height - 1);
                            }
                        }

                        try
                        {
                            g.SmoothingMode = oldS;
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
        }

        private static void ApplyTextBox(TextBox textBox)
        {
            bool isReadOnly = false;
            try { isReadOnly = textBox.ReadOnly; } catch { isReadOnly = false; }

            // 基础色板
            textBox.BackColor = LeagueColors.InputBackground;
            textBox.ForeColor = Color.FromArgb(241, 241, 241);

            // 只读/禁用：降低对比、弱化交互感（避免看起来像“可输入但点不动”）
            if (!textBox.Enabled || isReadOnly)
            {
                textBox.BackColor = LeagueColors.DarkSurfaceLight;
                textBox.ForeColor = LeagueColors.TextSecondary;
            }

            // WinForms 原生 TextBox 不支持真正自绘；这里走“低侵入可用方案”
            if (textBox.BorderStyle == BorderStyle.None)
            {
                // 保持 None：通常已经被外层容器做了边框
                EnsureInputFrameAndInteraction(textBox);
                return;
            }

            // 尽量去掉系统边框，交给父容器绘制暗金描边（更像 LoL 客户端）
            // 注意：不强制改高度，避免破坏既有布局
            try
            {
                textBox.BorderStyle = BorderStyle.None;
            }
            catch
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }

            EnsureInputFrameAndInteraction(textBox);
        }

        private static void DisableNativeTheme(Control control)
        {
            if (control == null) return;

            var state = GetHookState(control);
            if (state != null && state.NativeThemeDisabled) return;
            if (state != null) state.NativeThemeDisabled = true;

            EventHandler apply = (s, e) =>
            {
                try
                {
                    if (control.IsHandleCreated && control.Handle != IntPtr.Zero)
                    {
                        // 关键：传入单个空格可“移除”视觉主题（比空字符串更稳定）
                        // 目的：让 BackColor/OwnerDraw 在 Win10/11 下不被系统主题刷回白底/白边
                        SetWindowTheme(control.Handle, " ", " ");
                    }
                }
                catch
                {
                    // ignore
                }
            };

            try
            {
                control.HandleCreated += apply;
            }
            catch
            {
                // ignore
            }

            // Handle 已存在则立即尝试一次
            try
            {
                apply(control, EventArgs.Empty);
            }
            catch
            {
                // ignore
            }
        }

        private static IntPtr ColorToCOLORREF(Color color)
        {
            // COLORREF: 0x00BBGGRR
            try
            {
                int value = (color.R) | (color.G << 8) | (color.B << 16);
                return (IntPtr)value;
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        private static void ApplyMonthCalendarTheme(IntPtr monthCalHandle)
        {
            if (monthCalHandle == IntPtr.Zero) return;

            try
            {
                // 去系统主题，避免弹层仍显示白底/浅色标题
                SetWindowTheme(monthCalHandle, " ", " ");
            }
            catch
            {
                // ignore
            }

            try
            {
                SendMessage(monthCalHandle, MCM_SETCOLOR, (IntPtr)MCSC_BACKGROUND, ColorToCOLORREF(LeagueColors.DarkSurface));
                SendMessage(monthCalHandle, MCM_SETCOLOR, (IntPtr)MCSC_MONTHBK, ColorToCOLORREF(LeagueColors.InputBackground));
                SendMessage(monthCalHandle, MCM_SETCOLOR, (IntPtr)MCSC_TEXT, ColorToCOLORREF(LeagueColors.TextPrimary));
                SendMessage(monthCalHandle, MCM_SETCOLOR, (IntPtr)MCSC_TITLEBK, ColorToCOLORREF(LeagueColors.DarkBackground));
                SendMessage(monthCalHandle, MCM_SETCOLOR, (IntPtr)MCSC_TITLETEXT, ColorToCOLORREF(LeagueColors.TextHighlight));
                SendMessage(monthCalHandle, MCM_SETCOLOR, (IntPtr)MCSC_TRAILINGTEXT, ColorToCOLORREF(LeagueColors.TextDisabled));
            }
            catch
            {
                // ignore
            }
        }

        private static void HookDateTimePickerPopupTheme(DateTimePicker dateTimePicker)
        {
            if (dateTimePicker == null) return;

            var state = GetHookState(dateTimePicker);
            if (state == null || state.DateTimePickerPopupHooked) return;
            state.DateTimePickerPopupHooked = true;

            EventHandler apply = (s, e) =>
            {
                try
                {
                    if (!dateTimePicker.IsHandleCreated || dateTimePicker.Handle == IntPtr.Zero) return;

                    var monthCal = SendMessage(dateTimePicker.Handle, DTM_GETMONTHCAL, IntPtr.Zero, IntPtr.Zero);
                    if (monthCal != IntPtr.Zero)
                    {
                        ApplyMonthCalendarTheme(monthCal);
                    }
                }
                catch
                {
                    // ignore
                }
            };

            try { dateTimePicker.DropDown += apply; } catch { }
            try { dateTimePicker.HandleCreated += apply; } catch { }
        }

        private static void HookComboBoxPopupTheme(ComboBox comboBox)
        {
            if (comboBox == null) return;

            var state = GetHookState(comboBox);
            if (state == null || state.ComboBoxPopupHooked) return;
            state.ComboBoxPopupHooked = true;

            EventHandler apply = (s, e) =>
            {
                try
                {
                    if (!comboBox.IsHandleCreated || comboBox.Handle == IntPtr.Zero) return;

                    var info = new COMBOBOXINFO();
                    info.cbSize = Marshal.SizeOf(typeof(COMBOBOXINFO));

                    if (!GetComboBoxInfo(comboBox.Handle, ref info))
                    {
                        return;
                    }

                    // 关键：下拉列表是独立 HWND（ListBox），必须单独移除系统主题
                    if (info.hwndList != IntPtr.Zero)
                    {
                        try { SetWindowTheme(info.hwndList, " ", " "); } catch { }

                        // 给下拉列表补一个“暗底擦除”，避免空白区域仍是系统白底
                        try
                        {
                            var listState = GetHookState(comboBox);
                            if (listState != null)
                            {
                                // 句柄变化（少数情况下会重建），需要重新挂载
                                if (listState.ComboListWindow == null || listState.ComboListHandle != info.hwndList)
                                {
                                    try
                                    {
                                        if (listState.ComboListWindow != null)
                                        {
                                            listState.ComboListWindow.ReleaseHandle();
                                        }
                                    }
                                    catch
                                    {
                                        // ignore
                                    }

                                    listState.ComboListHandle = info.hwndList;
                                    listState.ComboListWindow = new ComboListBoxNativeWindow();
                                    listState.ComboListWindow.AssignHandle(info.hwndList);

                                    // 控件销毁时释放，避免挂到无效 HWND
                                    comboBox.HandleDestroyed += (ss, ee) =>
                                    {
                                        try
                                        {
                                            if (listState.ComboListWindow != null)
                                            {
                                                listState.ComboListWindow.ReleaseHandle();
                                            }
                                            listState.ComboListWindow = null;
                                            listState.ComboListHandle = IntPtr.Zero;
                                        }
                                        catch
                                        {
                                            // ignore
                                        }
                                    };
                                }
                            }
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                }
                catch
                {
                    // ignore
                }
            };

            try { comboBox.DropDown += apply; } catch { }
            try { comboBox.HandleCreated += apply; } catch { }
        }

        private static void ApplyDataGridView(DataGridView grid)
        {
            if (grid == null) return;

            grid.EnableHeadersVisualStyles = false;
            grid.BackgroundColor = LeagueColors.DarkSurface;
            // 注意：DataGridView.GridColor 不接受透明色（alpha < 255），否则会抛异常
            grid.GridColor = LeagueColors.DarkBorder;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowHeadersVisible = false;

            // Header
            grid.ColumnHeadersDefaultCellStyle.BackColor = LeagueColors.DarkBackground;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = LeagueColors.TextHighlight;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = LeagueColors.DarkBackground;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);

            // Rows
            grid.DefaultCellStyle.BackColor = LeagueColors.DarkSurface;
            grid.DefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33); // 深金棕（不透明，避免异常）
            grid.DefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;
            grid.DefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            // RowsDefault / Template：很多窗体会在代码里写 DefaultCellStyle 或列样式，导致视觉“回到白底蓝选中”
            // 这里显式覆盖到行与模板层级，提升稳定性（低侵入，不改列定义与数据绑定逻辑）
            grid.RowsDefaultCellStyle.BackColor = LeagueColors.DarkSurface;
            grid.RowsDefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
            grid.RowsDefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;

            // 交替行：更有层级，提升可读性
            grid.AlternatingRowsDefaultCellStyle.BackColor = LeagueColors.DarkSurfaceLight;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;

            // 行/表头高度：更像客户端列表密度
            try
            {
                grid.RowTemplate.Height = Math.Max(grid.RowTemplate.Height, 30);
            }
            catch
            {
                // ignore
            }

            try
            {
                grid.ColumnHeadersHeight = Math.Max(grid.ColumnHeadersHeight, 34);
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            }
            catch
            {
                // ignore
            }

            try
            {
                grid.RowTemplate.DefaultCellStyle.BackColor = LeagueColors.DarkSurface;
                grid.RowTemplate.DefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
                grid.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
                grid.RowTemplate.DefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;
            }
            catch
            {
                // ignore
            }

            grid.AlternatingRowsDefaultCellStyle.BackColor = LeagueColors.DarkPanel;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;

            // 列级样式：若业务代码为每列设置了默认样式（或控件默认携带样式），这里统一回到暗金风
            try
            {
                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if (col == null) continue;
                    col.DefaultCellStyle.BackColor = LeagueColors.DarkSurface;
                    col.DefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
                    col.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
                    col.DefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;
                    col.DefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
                }
            }
            catch
            {
                // ignore
            }

            // 绑定数据后常见会被业务代码再次写回样式；这里做一次“绑定后回写”，保证主题稳定
            var state = GetHookState(grid);
            if (state != null && !state.DataGridViewHooked)
            {
                state.DataGridViewHooked = true;
                grid.DataBindingComplete += (s, e) =>
                {
                    try
                    {
                        ApplyDataGridView(grid);
                    }
                    catch
                    {
                        // ignore
                    }
                };

                // 单元格编辑控件（TextBox/ComboBox）在暗色主题下常被系统主题刷回白底；
                // 这里在编辑控件出现时进行一次针对性主题应用。
                grid.EditingControlShowing += (s, e) =>
                {
                    try
                    {
                        if (e == null || e.Control == null) return;

                        var tb = e.Control as TextBox;
                        if (tb != null)
                        {
                            ApplyTextBox(tb);
                            return;
                        }

                        var cb = e.Control as ComboBox;
                        if (cb != null)
                        {
                            ApplyComboBox(cb);
                            return;
                        }

                        var dtp = e.Control as DateTimePicker;
                        if (dtp != null)
                        {
                            DisableNativeTheme(dtp);
                            dtp.BackColor = LeagueColors.InputBackground;
                            dtp.ForeColor = Color.FromArgb(241, 241, 241);
                            return;
                        }

                        var nud = e.Control as NumericUpDown;
                        if (nud != null)
                        {
                            DisableNativeTheme(nud);
                            nud.BackColor = LeagueColors.InputBackground;
                            nud.ForeColor = Color.FromArgb(241, 241, 241);
                            return;
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
        }

        private static void ApplyToolStripBase(ToolStrip strip)
        {
            if (strip == null) return;

            strip.BackColor = LeagueColors.DarkSurface;
            strip.ForeColor = LeagueColors.TextPrimary;
            strip.Renderer = new LeagueToolStripRenderer();

            var state = GetHookState(strip);
            if (state != null && !state.ToolStripHooked)
            {
                state.ToolStripHooked = true;
                strip.Paint += (s, e) =>
                {
                    // 给工具条底部加一条淡金分隔线
                    try
                    {
                        using (var pen = new Pen(Color.FromArgb(60, LeagueColors.RiotBorderGold), 1))
                        {
                            e.Graphics.DrawLine(pen, 0, strip.Height - 1, strip.Width, strip.Height - 1);
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
        }

        private static void ApplyMenuStrip(MenuStrip strip)
        {
            if (strip == null) return;

            strip.BackColor = LeagueColors.DarkSurface;
            strip.ForeColor = LeagueColors.TextPrimary;

            try
            {
                strip.Renderer = new LeagueMenuRenderer();
            }
            catch
            {
                strip.Renderer = new LeagueToolStripRenderer();
            }

            var state = GetHookState(strip);
            if (state != null && !state.ToolStripHooked)
            {
                state.ToolStripHooked = true;
                strip.Paint += (s, e) =>
                {
                    // MenuStrip 底部分隔线（更像客户端顶部导航条）
                    try
                    {
                        using (var pen = new Pen(Color.FromArgb(70, LeagueColors.RiotBorderGold), 1))
                        {
                            e.Graphics.DrawLine(pen, 0, strip.Height - 1, strip.Width, strip.Height - 1);
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
        }

        private static void ApplyContextMenuStrip(ContextMenuStrip menu)
        {
            if (menu == null) return;

            var state = GetHookState(menu);
            if (state != null && state.ToolStripHooked) return;
            if (state != null) state.ToolStripHooked = true;

            try
            {
                menu.ShowImageMargin = false;
                menu.ShowCheckMargin = false;
            }
            catch
            {
                // ignore
            }

            try
            {
                // ContextMenu 更像“下拉菜单”，用 MenuRenderer 的选中高光更贴近 LoL 观感
                menu.Renderer = new LeagueMenuRenderer();
            }
            catch
            {
                // ignore
            }

            try
            {
                menu.BackColor = LeagueColors.DarkSurface;
                menu.ForeColor = LeagueColors.TextPrimary;
            }
            catch
            {
                // ignore
            }

            try
            {
                menu.Opening += (s, e) =>
                {
                    try
                    {
                        // 强制刷新一次，避免系统主题把 DropDown 刷回浅色
                        menu.BackColor = LeagueColors.DarkSurface;
                        menu.ForeColor = LeagueColors.TextPrimary;
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }
            catch
            {
                // ignore
            }
        }

        private static void ApplyButton(Button button)
        {
            if (button == null) return;

            // WinForms 原生 Button 无法可靠 UserPaint，这里走属性驱动的“暗金风”按钮风格
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.BackColor = Color.FromArgb(30, 35, 40); // #1E2328
            button.ForeColor = Color.FromArgb(205, 190, 145); // #CDBE91
            button.Cursor = Cursors.Hand;

            // 金描边 + Hover/Pressed 反馈
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(200, 170, 110); // #C8AA6E
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(68, 62, 46); // #443E2E
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(55, 49, 33); // #373121

            try
            {
                if (button.Font == null || button.Font.Size < 9F)
                {
                    button.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
                }
            }
            catch
            {
                // ignore
            }
        }

        private static HookState GetHookState(Control control)
        {
            if (control == null) return null;

            lock (HooksLock)
            {
                HookState state;
                if (!Hooks.TryGetValue(control, out state))
                {
                    state = new HookState();
                    Hooks.Add(control, state);
                }
                return state;
            }
        }

        private static void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;

            try
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }
            catch
            {
                // ignore
            }
        }

        private static GraphicsPath CreateChamferedRectPath(Rectangle rect, int cut)
        {
            var path = new GraphicsPath();

            try
            {
                if (rect.Width <= 0 || rect.Height <= 0)
                {
                    return path;
                }

                int c = Math.Max(0, cut);
                int maxC = Math.Min(rect.Width / 2, rect.Height / 2);
                if (c > maxC) c = maxC;

                if (c <= 0)
                {
                    path.AddRectangle(rect);
                    return path;
                }

                int x = rect.X;
                int y = rect.Y;
                int r = rect.Right;
                int b = rect.Bottom;

                path.StartFigure();
                path.AddLine(x + c, y, r - c, y);
                path.AddLine(r - c, y, r, y + c);
                path.AddLine(r, y + c, r, b - c);
                path.AddLine(r, b - c, r - c, b);
                path.AddLine(r - c, b, x + c, b);
                path.AddLine(x + c, b, x, b - c);
                path.AddLine(x, b - c, x, y + c);
                path.AddLine(x, y + c, x + c, y);
                path.CloseFigure();
            }
            catch
            {
                // ignore
            }

            return path;
        }

        private static void EnsureInputFrameAndInteraction(Control input)
        {
            if (input == null) return;

            try
            {
                // DataGridView 的编辑控件会作为子控件挂在网格内部；
                // 给 DataGridView 本体挂 Paint 会导致额外重绘与潜在闪烁，这里跳过。
                var parent = input.Parent;
                if (parent != null && !(parent is DataGridView))
                {
                    HookInputFrameOnContainer(parent);
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                HookInputInteraction(input);
            }
            catch
            {
                // ignore
            }
        }

        private static void HookInputInteraction(Control input)
        {
            if (input == null) return;
            var state = GetHookState(input);
            if (state == null || state.InputInteractionHooked) return;
            state.InputInteractionHooked = true;

            EventHandler invalidate = (s, e) =>
            {
                try
                {
                    var c = s as Control;
                    if (c == null) return;
                    var p = c.Parent;
                    if (p == null) return;

                    // 只重绘输入框附近区域，减少闪烁
                    var rect = c.Bounds;
                    rect.Inflate(3, 3);
                    p.Invalidate(rect, false);
                }
                catch
                {
                    // ignore
                }
            };

            try { input.GotFocus += invalidate; } catch { }
            try { input.LostFocus += invalidate; } catch { }
            try { input.Enter += invalidate; } catch { }
            try { input.Leave += invalidate; } catch { }
            try { input.MouseEnter += invalidate; } catch { }
            try { input.MouseLeave += invalidate; } catch { }
            try { input.EnabledChanged += invalidate; } catch { }
            try { input.VisibleChanged += invalidate; } catch { }
        }

        private static void HookInputFrameOnContainer(Control container)
        {
            if (container == null) return;

            var state = GetHookState(container);
            if (state == null || state.InputFrameHooked) return;
            state.InputFrameHooked = true;

            EnableDoubleBuffering(container);

            container.Paint += (s, e) =>
            {
                try
                {
                    DrawInputFrames(container, e.Graphics);
                }
                catch
                {
                    // ignore
                }
            };
        }

        private static void DrawInputFrames(Control container, Graphics g)
        {
            if (container == null || g == null) return;

            Point mouse;
            try
            {
                mouse = container.PointToClient(Control.MousePosition);
            }
            catch
            {
                mouse = Point.Empty;
            }

            foreach (Control child in container.Controls)
            {
                if (child == null) continue;
                if (!child.Visible) continue;

                // 仅对常见输入控件绘制描边（不触碰布局）
                bool isInput = child is TextBox || child is ComboBox || child is DateTimePicker || child is NumericUpDown;
                if (!isInput) continue;

                DrawSingleInputFrame(container, child, g, mouse);
            }
        }

        private static void DrawSingleInputFrame(Control container, Control input, Graphics g, Point mouse)
        {
            if (container == null || input == null || g == null) return;

            Rectangle rect = input.Bounds;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            // 给描边留一点余量，避免被子控件盖住
            rect.Inflate(1, 1);

            bool hovered = rect.Contains(mouse);
            bool focused = false;
            try { focused = input.Focused; } catch { focused = false; }

            bool enabled = input.Enabled;
            bool readOnly = false;

            try
            {
                var tb = input as TextBox;
                if (tb != null)
                {
                    readOnly = tb.ReadOnly;
                }
            }
            catch
            {
                readOnly = false;
            }

            Color border = Color.FromArgb(80, LeagueColors.RiotBorderGold);
            Color glow = Color.FromArgb(0, 0, 0, 0);

            if (!enabled)
            {
                border = Color.FromArgb(40, LeagueColors.SeparatorColor);
            }
            else if (readOnly)
            {
                border = Color.FromArgb(70, LeagueColors.SeparatorColor);
                glow = Color.FromArgb(0, 0, 0, 0);
            }
            else if (focused)
            {
                border = Color.FromArgb(200, LeagueColors.RiotGoldHover);
                glow = Color.FromArgb(70, LeagueColors.RiotGoldHover);
            }
            else if (hovered)
            {
                border = Color.FromArgb(140, LeagueColors.RiotGold);
                glow = Color.FromArgb(45, LeagueColors.RiotGold);
            }

            // 外描边
            var old = g.SmoothingMode;
            try
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            catch
            {
                // ignore
            }

            using (var path = CreateChamferedRectPath(rect, 4))
            using (var pen = new Pen(border, 1))
            {
                g.DrawPath(pen, path);
            }

            // 内发光（焦点/悬停）
            if (glow.A > 0)
            {
                Rectangle inner = rect;
                inner.Inflate(-1, -1);
                using (var path2 = CreateChamferedRectPath(inner, 3))
                using (var pen2 = new Pen(glow, 1))
                {
                    g.DrawPath(pen2, path2);
                }
            }

            try
            {
                g.SmoothingMode = old;
            }
            catch
            {
                // ignore
            }
        }

        private static void ApplyComboBox(ComboBox comboBox)
        {
            if (comboBox == null) return;

            DisableNativeTheme(comboBox);

            comboBox.BackColor = LeagueColors.InputBackground;
            comboBox.ForeColor = Color.FromArgb(241, 241, 241);
            comboBox.FlatStyle = FlatStyle.Flat;
            if (!comboBox.Enabled)
            {
                comboBox.BackColor = LeagueColors.DarkSurfaceLight;
                comboBox.ForeColor = LeagueColors.TextDisabled;
            }

            try
            {
                if (comboBox.Font == null || comboBox.Font.Size < 9F)
                {
                    comboBox.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
                }
            }
            catch
            {
                // ignore
            }

            // WinForms 默认 ComboBox（尤其 DropDownList）在暗色主题下经常出现“白底黑字/白底白框”。
            // 这里对 DropDownList 启用 OwnerDrawFixed，让下拉项与选中项可稳定使用暗金风色板。
            try
            {
                // 兼容：DropDown 也经常被系统主题“刷白”，这里统一用 OwnerDrawFixed 提升一致性。
                if (comboBox.DrawMode == DrawMode.Normal)
                {
                    comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                }

                if (comboBox.ItemHeight < 22)
                {
                    comboBox.ItemHeight = 22;
                }

                comboBox.IntegralHeight = false;
            }
            catch
            {
                // ignore
            }

            var state = GetHookState(comboBox);
            if (state != null && !state.ComboBoxHooked)
            {
                state.ComboBoxHooked = true;

                comboBox.DropDown += (s, e) =>
                {
                    try
                    {
                        // 限制下拉高度，减少“空白区域仍是系统白底”的割裂感
                        int itemHeight = Math.Max(1, comboBox.ItemHeight);
                        int visibleItems = Math.Min(Math.Max(1, comboBox.Items.Count), 10);
                        comboBox.DropDownHeight = Math.Min(260, visibleItems * itemHeight + 2);
                    }
                    catch
                    {
                        // ignore
                    }
                };

                comboBox.DrawItem += (s, e) =>
                {
                    try
                    {
                        e.DrawBackground();

                        var bounds = e.Bounds;
                        if (bounds.Width <= 0 || bounds.Height <= 0)
                        {
                            return;
                        }

                        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                        bool disabled = (e.State & DrawItemState.Disabled) == DrawItemState.Disabled || !comboBox.Enabled;

                        var bg = selected ? Color.FromArgb(55, 49, 33) : LeagueColors.InputBackground;
                        var fg = disabled ? Color.FromArgb(140, 140, 140) : (selected ? LeagueColors.TextHighlight : LeagueColors.TextPrimary);

                        using (var brush = new SolidBrush(bg))
                        {
                            e.Graphics.FillRectangle(brush, bounds);
                        }

                        string text = string.Empty;
                        try
                        {
                            if (e.Index >= 0 && e.Index < comboBox.Items.Count)
                            {
                                text = comboBox.GetItemText(comboBox.Items[e.Index]);
                            }
                            else
                            {
                                text = comboBox.Text ?? string.Empty;
                            }
                        }
                        catch
                        {
                            text = comboBox.Text ?? string.Empty;
                        }

                        var textBounds = new Rectangle(bounds.X + 8, bounds.Y + 2, Math.Max(1, bounds.Width - 12), Math.Max(1, bounds.Height - 4));
                        TextRenderer.DrawText(e.Graphics, text, comboBox.Font, textBounds, fg,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                        // 下拉项分隔线（很淡，避免“列表像一坨”）
                        if ((e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit && e.Index >= 0)
                        {
                            using (var pen = new Pen(Color.FromArgb(45, LeagueColors.RiotBorderGold), 1))
                            {
                                e.Graphics.DrawLine(pen, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                            }
                        }

                        e.DrawFocusRectangle();
                    }
                    catch
                    {
                        // ignore
                    }
                };
            }

            HookComboBoxPopupTheme(comboBox);
            EnsureInputFrameAndInteraction(comboBox);
        }
    }
}
