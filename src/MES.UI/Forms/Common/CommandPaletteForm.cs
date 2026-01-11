using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Common
{
    /// <summary>
    /// 全局命令面板（Command Palette）
    /// - 目标：键盘优先、快速跳转模块/常用工具
    /// - 入口：由主窗体捕获 Ctrl+K 打开
    /// </summary>
    public class CommandPaletteForm : ThemedForm
    {
        public class CommandPaletteItem
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Keywords { get; set; }
            public Action Action { get; set; }

            internal string SearchText
            {
                get
                {
                    var parts = new[] { Title, Subtitle, Keywords }
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select(s => s.Trim());
                    return string.Join(" ", parts);
                }
            }

            public override string ToString()
            {
                return Title ?? string.Empty;
            }
        }

        private readonly TextBox _query = new TextBox();
        private readonly ListBox _list = new ListBox();
        private readonly Label _hint = new Label();

        private List<CommandPaletteItem> _allItems = new List<CommandPaletteItem>();

        public CommandPaletteForm()
        {
            InitializeWindow();
            BuildLayout();
            WireEvents();
        }

        public void SetItems(IEnumerable<CommandPaletteItem> items)
        {
            _allItems = items == null
                ? new List<CommandPaletteItem>()
                : items.Where(i => i != null).ToList();
            ApplyFilter();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            try
            {
                _query.Focus();
                _query.SelectAll();
            }
            catch
            {
                // ignore
            }
        }

        private void InitializeWindow()
        {
            Text = "Command Palette";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            TopMost = true;
            KeyPreview = true;

            Width = 760;
            Height = 460;

            BackColor = UIThemeManager.Colors.Background;
            ForeColor = UIThemeManager.Colors.Text;
            Font = UIThemeManager.GetFont(9f);
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(16);
            layout.ColumnCount = 1;
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            Controls.Add(layout);

            _query.Dock = DockStyle.Fill;
            _query.BorderStyle = BorderStyle.FixedSingle;
            _query.Font = UIThemeManager.GetTitleFont(11f);
            _query.BackColor = UIThemeManager.Colors.Surface;
            _query.ForeColor = UIThemeManager.Colors.Text;
            layout.Controls.Add(_query, 0, 0);

            _list.Dock = DockStyle.Fill;
            _list.BorderStyle = BorderStyle.FixedSingle;
            _list.IntegralHeight = false;
            _list.DrawMode = DrawMode.OwnerDrawFixed;
            _list.ItemHeight = 56;
            _list.BackColor = UIThemeManager.Colors.Surface;
            _list.ForeColor = UIThemeManager.Colors.Text;
            layout.Controls.Add(_list, 0, 1);

            _hint.Dock = DockStyle.Fill;
            _hint.TextAlign = ContentAlignment.MiddleLeft;
            _hint.Text = "Enter 执行 · Esc 关闭 · ↑↓ 选择";
            _hint.ForeColor = UIThemeManager.Colors.TextSecondary;
            layout.Controls.Add(_hint, 0, 2);
        }

        private void WireEvents()
        {
            _query.TextChanged += (s, e) => ApplyFilter();
            _query.KeyDown += Query_KeyDown;

            _list.DrawItem += List_DrawItem;
            _list.DoubleClick += (s, e) => ExecuteSelectedOrClose();
            _list.KeyDown += List_KeyDown;

            KeyDown += Palette_KeyDown;
        }

        private void Palette_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;

            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close();
            }
        }

        private void Query_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;

            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (_list.Items.Count > 0)
                {
                    _list.Focus();
                    if (_list.SelectedIndex < 0) _list.SelectedIndex = 0;
                }
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                ExecuteSelectedOrClose();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close();
            }
        }

        private void List_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null) return;

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                ExecuteSelectedOrClose();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close();
                return;
            }

            if (e.KeyCode == Keys.Back)
            {
                // 退格：回到搜索框继续过滤
                try
                {
                    _query.Focus();
                    _query.SelectionStart = _query.TextLength;
                }
                catch
                {
                    // ignore
                }
            }
        }

        private void ExecuteSelectedOrClose()
        {
            var item = _list.SelectedItem as CommandPaletteItem;
            if (item == null || item.Action == null)
            {
                Close();
                return;
            }

            try
            {
                item.Action();
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show(string.Format("执行失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    // ignore
                }
            }
            finally
            {
                Close();
            }
        }

        private void ApplyFilter()
        {
            var query = (_query.Text ?? string.Empty).Trim();

            IEnumerable<CommandPaletteItem> items = _allItems;
            if (!string.IsNullOrWhiteSpace(query))
            {
                items = _allItems
                    .Where(i => i.SearchText.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                    .OrderByDescending(i => Score(i, query))
                    .ThenBy(i => i.Title);
            }

            var list = items.ToList();

            _list.BeginUpdate();
            try
            {
                _list.Items.Clear();
                foreach (var item in list)
                {
                    _list.Items.Add(item);
                }

                if (_list.Items.Count > 0)
                {
                    _list.SelectedIndex = 0;
                }
            }
            finally
            {
                _list.EndUpdate();
            }
        }

        private static int Score(CommandPaletteItem item, string query)
        {
            if (item == null) return 0;
            if (string.IsNullOrWhiteSpace(query)) return 0;

            var title = item.Title ?? string.Empty;
            var subtitle = item.Subtitle ?? string.Empty;
            var q = query.Trim();

            // 轻量打分：更偏向标题命中，兼顾关键词
            int score = 0;
            if (title.StartsWith(q, StringComparison.OrdinalIgnoreCase)) score += 100;
            if (title.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) score += 60;
            if (subtitle.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) score += 20;
            if (!string.IsNullOrWhiteSpace(item.Keywords) &&
                item.Keywords.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) score += 15;
            return score;
        }

        private void List_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0 || e.Index >= _list.Items.Count)
            {
                return;
            }

            var item = _list.Items[e.Index] as CommandPaletteItem;
            if (item == null)
            {
                return;
            }

            var colors = UIThemeManager.Colors;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            var back = selected ? colors.Selected : colors.Surface;
            using (var brush = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            using (var pen = new Pen(Color.FromArgb(80, colors.Border), 1))
            {
                e.Graphics.DrawRectangle(pen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }

            var padding = 12;
            var titleRect = new Rectangle(e.Bounds.X + padding, e.Bounds.Y + 8, e.Bounds.Width - padding * 2, 22);
            var subRect = new Rectangle(e.Bounds.X + padding, e.Bounds.Y + 30, e.Bounds.Width - padding * 2, 18);

            TextRenderer.DrawText(
                e.Graphics,
                item.Title ?? string.Empty,
                UIThemeManager.GetTitleFont(10f),
                titleRect,
                colors.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            var subColor = selected ? Color.FromArgb(230, colors.TextSecondary) : colors.TextSecondary;
            TextRenderer.DrawText(
                e.Graphics,
                item.Subtitle ?? string.Empty,
                UIThemeManager.GetFont(8.5f),
                subRect,
                subColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            e.DrawFocusRectangle();
        }
    }
}

