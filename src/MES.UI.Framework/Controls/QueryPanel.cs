using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MES.UI.Framework.Themes;
using MES.Common.Logging;

namespace MES.UI.Framework.Controls
{
    /// <summary>
    /// 查询面板控件 - 提供统一的查询条件输入界面
    /// </summary>
    public partial class QueryPanel : UserControl
    {
        #region 私有字段

        private TableLayoutPanel _mainLayout;
        private Panel _buttonPanel;
        private ModernButton _searchButton;
        private ModernButton _resetButton;
        private ModernButton _expandButton;
        private bool _isExpanded = true;
        private int _collapsedHeight = 60;
        private int _expandedHeight = 120;
        private List<QueryField> _queryFields;

        #endregion

        #region 查询字段定义

        /// <summary>
        /// 查询字段信息
        /// </summary>
        public class QueryField
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public QueryFieldType FieldType { get; set; }
            public Control InputControl { get; set; }
            public object DefaultValue { get; set; }
            public bool IsRequired { get; set; }
            public string[] ComboBoxItems { get; set; }
        }

        /// <summary>
        /// 查询字段类型
        /// </summary>
        public enum QueryFieldType
        {
            Text,
            Number,
            Date,
            DateRange,
            ComboBox,
            CheckBox
        }

        #endregion

        #region 事件定义

        /// <summary>
        /// 查询事件
        /// </summary>
        public event EventHandler<QueryEventArgs> SearchClicked;

        /// <summary>
        /// 重置事件
        /// </summary>
        public event EventHandler ResetClicked;

        /// <summary>
        /// 查询事件参数
        /// </summary>
        public class QueryEventArgs : EventArgs
        {
            public Dictionary<string, object> QueryParameters { get; set; }
        }

        #endregion

        #region 公共属性

        /// <summary>
        /// 是否展开状态
        /// </summary>
        [Category("外观")]
        [Description("查询面板是否处于展开状态")]
        [DefaultValue(true)]
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                UpdateExpandState();
            }
        }

        /// <summary>
        /// 折叠时的高度
        /// </summary>
        [Category("布局")]
        [Description("查询面板折叠时的高度")]
        [DefaultValue(60)]
        public int CollapsedHeight
        {
            get => _collapsedHeight;
            set
            {
                _collapsedHeight = Math.Max(40, value);
                if (!_isExpanded)
                {
                    Height = _collapsedHeight;
                }
            }
        }

        /// <summary>
        /// 展开时的高度
        /// </summary>
        [Category("布局")]
        [Description("查询面板展开时的高度")]
        [DefaultValue(120)]
        public int ExpandedHeight
        {
            get => _expandedHeight;
            set
            {
                _expandedHeight = Math.Max(_collapsedHeight + 20, value);
                if (_isExpanded)
                {
                    Height = _expandedHeight;
                }
            }
        }

        /// <summary>
        /// 查询字段列表
        /// </summary>
        [Browsable(false)]
        public List<QueryField> QueryFields => _queryFields;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化查询面板
        /// </summary>
        public QueryPanel()
        {
            _queryFields = new List<QueryField>();
            InitializeComponent();
            ApplyTheme();
            
            // 订阅主题变更事件
            UIThemeManager.OnThemeChanged += ApplyTheme;
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            
            // 设置基本属性
            Size = new Size(800, _expandedHeight);
            BackColor = UIThemeManager.Colors.Surface;
            BorderStyle = BorderStyle.FixedSingle;
            
            // 创建主布局
            _mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                Padding = new Padding(10)
            };
            
            // 设置列宽比例
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            
            // 设置行高
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            
            Controls.Add(_mainLayout);
            
            // 创建按钮面板
            CreateButtonPanel();
            
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// 创建按钮面板
        /// </summary>
        private void CreateButtonPanel()
        {
            _buttonPanel = new Panel
            {
                Height = 35,
                Dock = DockStyle.None,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            
            // 查询按钮
            _searchButton = new ModernButton
            {
                Text = "查询",
                Size = new Size(80, 30),
                Style = ModernButton.ButtonStyle.Primary,
                Location = new Point(0, 2)
            };
            _searchButton.Click += SearchButton_Click;
            
            // 重置按钮
            _resetButton = new ModernButton
            {
                Text = "重置",
                Size = new Size(80, 30),
                Style = ModernButton.ButtonStyle.Secondary,
                Location = new Point(90, 2)
            };
            _resetButton.Click += ResetButton_Click;
            
            // 展开/折叠按钮
            _expandButton = new ModernButton
            {
                Text = "折叠",
                Size = new Size(60, 30),
                Style = ModernButton.ButtonStyle.Outline,
                Location = new Point(180, 2)
            };
            _expandButton.Click += ExpandButton_Click;
            
            _buttonPanel.Controls.AddRange(new Control[] { _searchButton, _resetButton, _expandButton });
            _buttonPanel.Size = new Size(250, 35);
            
            // 将按钮面板添加到主布局的最后一行
            _mainLayout.Controls.Add(_buttonPanel, 0, 2);
            _mainLayout.SetColumnSpan(_buttonPanel, 4);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 添加查询字段
        /// </summary>
        /// <param name="field">查询字段信息</param>
        public void AddQueryField(QueryField field)
        {
            if (field == null || string.IsNullOrEmpty(field.Name))
                return;
            
            try
            {
                // 创建标签
                Label label = new Label
                {
                    Text = field.DisplayName + (field.IsRequired ? " *" : ""),
                    ForeColor = field.IsRequired ? UIThemeManager.Colors.Error : UIThemeManager.Colors.Text,
                    Font = UIThemeManager.GetFont(9f),
                    AutoSize = true,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top,
                    Margin = new Padding(0, 8, 0, 0)
                };
                
                // 创建输入控件
                Control inputControl = CreateInputControl(field);
                field.InputControl = inputControl;
                
                // 添加到布局
                int row = _queryFields.Count / 2;
                int col = (_queryFields.Count % 2) * 2;
                
                if (row < 2) // 只显示前两行
                {
                    _mainLayout.Controls.Add(label, col, row);
                    _mainLayout.Controls.Add(inputControl, col + 1, row);
                }
                
                _queryFields.Add(field);
                
                LogManager.Debug($"已添加查询字段: {field.Name}");
            }
            catch (Exception ex)
            {
                LogManager.Error($"添加查询字段失败: {field.Name}", ex);
            }
        }

        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <returns>查询参数字典</returns>
        public Dictionary<string, object> GetQueryParameters()
        {
            var parameters = new Dictionary<string, object>();
            
            foreach (var field in _queryFields)
            {
                try
                {
                    object value = GetFieldValue(field);
                    if (value != null)
                    {
                        parameters[field.Name] = value;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error($"获取查询字段值失败: {field.Name}", ex);
                }
            }
            
            return parameters;
        }

        /// <summary>
        /// 重置所有查询字段
        /// </summary>
        public void ResetFields()
        {
            foreach (var field in _queryFields)
            {
                try
                {
                    SetFieldValue(field, field.DefaultValue);
                }
                catch (Exception ex)
                {
                    LogManager.Error($"重置查询字段失败: {field.Name}", ex);
                }
            }
        }

        /// <summary>
        /// 设置查询字段值
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>
        public void SetFieldValue(string fieldName, object value)
        {
            var field = _queryFields.Find(f => f.Name == fieldName);
            if (field != null)
            {
                SetFieldValue(field, value);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建输入控件
        /// </summary>
        /// <param name="field">查询字段</param>
        /// <returns>输入控件</returns>
        private Control CreateInputControl(QueryField field)
        {
            Control control = null;
            
            switch (field.FieldType)
            {
                case QueryFieldType.Text:
                    control = new TextBox
                    {
                        Font = UIThemeManager.GetFont(9f),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                    };
                    break;
                    
                case QueryFieldType.Number:
                    control = new NumericUpDown
                    {
                        Font = UIThemeManager.GetFont(9f),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                        DecimalPlaces = 0,
                        Maximum = decimal.MaxValue,
                        Minimum = decimal.MinValue
                    };
                    break;
                    
                case QueryFieldType.Date:
                    control = new DateTimePicker
                    {
                        Font = UIThemeManager.GetFont(9f),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                        Format = DateTimePickerFormat.Short
                    };
                    break;
                    
                case QueryFieldType.ComboBox:
                    var comboBox = new ComboBox
                    {
                        Font = UIThemeManager.GetFont(9f),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    if (field.ComboBoxItems != null)
                    {
                        comboBox.Items.AddRange(field.ComboBoxItems);
                    }
                    control = comboBox;
                    break;
                    
                case QueryFieldType.CheckBox:
                    control = new CheckBox
                    {
                        Font = UIThemeManager.GetFont(9f),
                        Anchor = AnchorStyles.Left | AnchorStyles.Top,
                        AutoSize = true
                    };
                    break;
                    
                default:
                    control = new TextBox
                    {
                        Font = UIThemeManager.GetFont(9f),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
                    };
                    break;
            }
            
            // 设置默认值
            if (field.DefaultValue != null)
            {
                SetFieldValue(field, field.DefaultValue);
            }
            
            return control;
        }

        /// <summary>
        /// 获取字段值
        /// </summary>
        /// <param name="field">查询字段</param>
        /// <returns>字段值</returns>
        private object GetFieldValue(QueryField field)
        {
            if (field.InputControl == null) return null;
            
            switch (field.FieldType)
            {
                case QueryFieldType.Text:
                    return ((TextBox)field.InputControl).Text;
                    
                case QueryFieldType.Number:
                    return ((NumericUpDown)field.InputControl).Value;
                    
                case QueryFieldType.Date:
                    return ((DateTimePicker)field.InputControl).Value;
                    
                case QueryFieldType.ComboBox:
                    return ((ComboBox)field.InputControl).SelectedItem;
                    
                case QueryFieldType.CheckBox:
                    return ((CheckBox)field.InputControl).Checked;
                    
                default:
                    return field.InputControl.Text;
            }
        }

        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="field">查询字段</param>
        /// <param name="value">字段值</param>
        private void SetFieldValue(QueryField field, object value)
        {
            if (field.InputControl == null || value == null) return;
            
            try
            {
                switch (field.FieldType)
                {
                    case QueryFieldType.Text:
                        ((TextBox)field.InputControl).Text = value.ToString();
                        break;
                        
                    case QueryFieldType.Number:
                        if (decimal.TryParse(value.ToString(), out decimal numValue))
                        {
                            ((NumericUpDown)field.InputControl).Value = numValue;
                        }
                        break;
                        
                    case QueryFieldType.Date:
                        if (DateTime.TryParse(value.ToString(), out DateTime dateValue))
                        {
                            ((DateTimePicker)field.InputControl).Value = dateValue;
                        }
                        break;
                        
                    case QueryFieldType.ComboBox:
                        ((ComboBox)field.InputControl).SelectedItem = value;
                        break;
                        
                    case QueryFieldType.CheckBox:
                        if (bool.TryParse(value.ToString(), out bool boolValue))
                        {
                            ((CheckBox)field.InputControl).Checked = boolValue;
                        }
                        break;
                        
                    default:
                        field.InputControl.Text = value.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"设置字段值失败: {field.Name}", ex);
            }
        }

        /// <summary>
        /// 应用主题
        /// </summary>
        private void ApplyTheme()
        {
            BackColor = UIThemeManager.Colors.Surface;
            // 应用主题到父窗体（如果存在）
            var parentForm = FindForm();
            if (parentForm != null)
            {
                UIThemeManager.ApplyTheme(parentForm);
            }
        }

        /// <summary>
        /// 更新展开状态
        /// </summary>
        private void UpdateExpandState()
        {
            if (_isExpanded)
            {
                Height = _expandedHeight;
                _expandButton.Text = "折叠";
            }
            else
            {
                Height = _collapsedHeight;
                _expandButton.Text = "展开";
            }
            
            // 隐藏或显示第二行的控件
            foreach (Control control in _mainLayout.Controls)
            {
                if (_mainLayout.GetRow(control) == 1)
                {
                    control.Visible = _isExpanded;
                }
            }
        }

        #endregion

        #region 事件处理

        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                var parameters = GetQueryParameters();
                SearchClicked?.Invoke(this, new QueryEventArgs { QueryParameters = parameters });
                LogManager.Debug("查询按钮被点击");
            }
            catch (Exception ex)
            {
                LogManager.Error("执行查询失败", ex);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            try
            {
                ResetFields();
                ResetClicked?.Invoke(this, EventArgs.Empty);
                LogManager.Debug("重置按钮被点击");
            }
            catch (Exception ex)
            {
                LogManager.Error("重置查询字段失败", ex);
            }
        }

        private void ExpandButton_Click(object sender, EventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        #endregion

        #region 资源清理

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UIThemeManager.OnThemeChanged -= ApplyTheme;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
