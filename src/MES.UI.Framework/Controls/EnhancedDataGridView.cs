using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Themes;
using MES.Common.Logging;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 增强数据网格控件 - 提供美观的数据展示和交互功能
    /// </summary>
    public class EnhancedDataGridView : DataGridView
    {
        #region 私有字段

        private bool _showRowNumbers = true;
        private bool _alternateRowColors = true;
        private Color _alternateRowColor = Color.Empty;
        private string _emptyDataText = "暂无数据";
        private Font _emptyDataFont;

        #endregion

        #region 公共属性

        /// <summary>
        /// 是否显示行号
        /// </summary>
        [Category("外观")]
        [Description("是否在行标题中显示行号")]
        [DefaultValue(true)]
        public bool ShowRowNumbers
        {
            get => _showRowNumbers;
            set
            {
                _showRowNumbers = value;
                if (value)
                {
                    RowHeadersVisible = true;
                    RowHeadersWidth = 50;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// 是否使用交替行颜色
        /// </summary>
        [Category("外观")]
        [Description("是否使用交替行颜色")]
        [DefaultValue(true)]
        public bool AlternateRowColors
        {
            get => _alternateRowColors;
            set
            {
                _alternateRowColors = value;
                UpdateRowColors();
            }
        }

        /// <summary>
        /// 交替行颜色
        /// </summary>
        [Category("外观")]
        [Description("交替行的背景颜色")]
        public Color AlternateRowColor
        {
            get => _alternateRowColor;
            set
            {
                _alternateRowColor = value;
                UpdateRowColors();
            }
        }

        /// <summary>
        /// 空数据时显示的文本
        /// </summary>
        [Category("外观")]
        [Description("当没有数据时显示的文本")]
        [DefaultValue("暂无数据")]
        public string EmptyDataText
        {
            get => _emptyDataText;
            set
            {
                _emptyDataText = value;
                Invalidate();
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化增强数据网格控件
        /// </summary>
        public EnhancedDataGridView()
        {
            InitializeComponent();
            ApplyDefaultStyle();
            
            // 订阅主题变更事件
            UIThemeManager.OnThemeChanged += OnThemeChanged;
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            // 基本设置
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            ReadOnly = true;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MultiSelect = false;
            
            // 外观设置
            BorderStyle = BorderStyle.FixedSingle;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            EnableHeadersVisualStyles = false;
            
            // 行设置
            RowHeadersVisible = _showRowNumbers;
            RowHeadersWidth = 50;
            RowTemplate.Height = 28;
            
            // 列设置
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ColumnHeadersHeight = 35;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            
            // 字体设置
            Font = UIThemeManager.GetFont(9f);
            _emptyDataFont = UIThemeManager.GetFont(12f);
        }

        /// <summary>
        /// 应用默认样式
        /// </summary>
        private void ApplyDefaultStyle()
        {
            try
            {
                var colors = UIThemeManager.Colors;
                
                // 背景颜色
                BackgroundColor = colors.Surface;
                GridColor = colors.Border;
                
                // 默认单元格样式
                DefaultCellStyle.BackColor = colors.Surface;
                DefaultCellStyle.ForeColor = colors.Text;
                DefaultCellStyle.SelectionBackColor = colors.Selected;
                DefaultCellStyle.SelectionForeColor = Color.White;
                DefaultCellStyle.Font = UIThemeManager.GetFont(9f);
                DefaultCellStyle.Padding = new Padding(5, 3, 5, 3);
                
                // 列标题样式
                ColumnHeadersDefaultCellStyle.BackColor = colors.Primary;
                ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                ColumnHeadersDefaultCellStyle.Font = UIThemeManager.GetTitleFont(10f);
                ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 5, 5, 5);
                
                // 行标题样式
                RowHeadersDefaultCellStyle.BackColor = colors.Background;
                RowHeadersDefaultCellStyle.ForeColor = colors.Text;
                RowHeadersDefaultCellStyle.Font = UIThemeManager.GetFont(8f);
                RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                // 更新交替行颜色
                UpdateRowColors();
                
                LogManager.Debug("已应用增强数据网格默认样式");
            }
            catch (Exception ex)
            {
                LogManager.Error("应用数据网格样式失败", ex);
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 主题变更事件处理
        /// </summary>
        private void OnThemeChanged()
        {
            ApplyDefaultStyle();
            Invalidate();
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            // 绘制行号
            if (_showRowNumbers && RowHeadersVisible)
            {
                string rowNumber = (e.RowIndex + 1).ToString();
                
                using (SolidBrush brush = new SolidBrush(RowHeadersDefaultCellStyle.ForeColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    
                    Rectangle rect = new Rectangle(
                        e.RowBounds.Left,
                        e.RowBounds.Top,
                        RowHeadersWidth,
                        e.RowBounds.Height
                    );
                    
                    e.Graphics.DrawString(rowNumber, RowHeadersDefaultCellStyle.Font, brush, rect, sf);
                }
            }
            
            base.OnRowPostPaint(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // 绘制空数据提示
            if (Rows.Count == 0 && !string.IsNullOrEmpty(_emptyDataText))
            {
                DrawEmptyDataText(e.Graphics);
            }
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
            
            // 数据源变更时重新应用样式
            if (DataSource != null)
            {
                ApplyColumnStyles();
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 更新交替行颜色
        /// </summary>
        private void UpdateRowColors()
        {
            if (_alternateRowColors)
            {
                if (_alternateRowColor == Color.Empty)
                {
                    // 使用默认的交替行颜色
                    AlternatingRowsDefaultCellStyle.BackColor = 
                        Color.FromArgb(248, 249, 250); // 浅灰色
                }
                else
                {
                    AlternatingRowsDefaultCellStyle.BackColor = _alternateRowColor;
                }
                
                AlternatingRowsDefaultCellStyle.ForeColor = DefaultCellStyle.ForeColor;
                AlternatingRowsDefaultCellStyle.SelectionBackColor = DefaultCellStyle.SelectionBackColor;
                AlternatingRowsDefaultCellStyle.SelectionForeColor = DefaultCellStyle.SelectionForeColor;
            }
            else
            {
                AlternatingRowsDefaultCellStyle.BackColor = DefaultCellStyle.BackColor;
            }
        }

        /// <summary>
        /// 绘制空数据文本
        /// </summary>
        /// <param name="graphics">图形对象</param>
        private void DrawEmptyDataText(Graphics graphics)
        {
            if (graphics == null) return;
            
            using (SolidBrush brush = new SolidBrush(UIThemeManager.Colors.TextSecondary))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                
                Rectangle rect = new Rectangle(
                    0,
                    ColumnHeadersHeight,
                    Width,
                    Height - ColumnHeadersHeight
                );
                
                graphics.DrawString(_emptyDataText, _emptyDataFont, brush, rect, sf);
            }
        }

        /// <summary>
        /// 应用列样式
        /// </summary>
        private void ApplyColumnStyles()
        {
            try
            {
                foreach (DataGridViewColumn column in Columns)
                {
                    // 根据列的数据类型设置对齐方式
                    if (column.ValueType == typeof(int) || 
                        column.ValueType == typeof(decimal) || 
                        column.ValueType == typeof(double) || 
                        column.ValueType == typeof(float))
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    else if (column.ValueType == typeof(DateTime))
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        column.DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                    }
                    else
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    
                    // 设置列的最小宽度
                    column.MinimumWidth = 80;
                }
                
                LogManager.Debug("已应用数据网格列样式");
            }
            catch (Exception ex)
            {
                LogManager.Error("应用数据网格列样式失败", ex);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 导出数据到CSV文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void ExportToCsv(string filePath)
        {
            try
            {
                using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    // 写入列标题
                    string[] headers = new string[Columns.Count];
                    for (int i = 0; i < Columns.Count; i++)
                    {
                        headers[i] = Columns[i].HeaderText;
                    }
                    writer.WriteLine(string.Join(",", headers));
                    
                    // 写入数据行
                    foreach (DataGridViewRow row in Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string[] values = new string[Columns.Count];
                            for (int i = 0; i < Columns.Count; i++)
                            {
                                values[i] = row.Cells[i].Value != null ? row.Cells[i].Value.ToString() : "";
                            }
                            writer.WriteLine(string.Join(",", values));
                        }
                    }
                }
                
                LogManager.Info(string.Format("数据已导出到CSV文件: {0}", filePath));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("导出CSV文件失败: {0}", filePath), ex);
                throw;
            }
        }

        /// <summary>
        /// 刷新数据并保持选中状态
        /// </summary>
        public void RefreshData()
        {
            try
            {
                int selectedIndex = CurrentRow != null ? CurrentRow.Index : -1;
                
                // 刷新数据源
                if (DataSource is BindingSource bindingSource)
                {
                    bindingSource.ResetBindings(false);
                }
                else
                {
                    Refresh();
                }
                
                // 恢复选中状态
                if (selectedIndex >= 0 && selectedIndex < Rows.Count)
                {
                    CurrentCell = Rows[selectedIndex].Cells[0];
                }
                
                LogManager.Debug("数据网格数据已刷新");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据网格数据失败", ex);
            }
        }

        #endregion

        #region 资源清理

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UIThemeManager.OnThemeChanged -= OnThemeChanged;
                if (_emptyDataFont != null) _emptyDataFont.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
