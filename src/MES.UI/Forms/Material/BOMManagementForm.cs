using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.BLL.Material;
using MES.Models.Material;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// BOM物料清单管理窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class BOMManagementForm : Form
    {
        private readonly IBOMBLL _bomBLL;

        private List<BOMInfo> bomList;
        private List<BOMInfo> filteredBomList;
        private BOMInfo currentBom;

        public BOMManagementForm()
        {
            InitializeComponent();
            _bomBLL = new BOMBLL();
            InitializeForm();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // 初始化数据
                bomList = new List<BOMInfo>();
                filteredBomList = new List<BOMInfo>();
                currentBom = null;

                // 设置DataGridView
                SetupDataGridView();

                // 加载数据（离线/数据库由 BLL 决定）
                LoadData();

                // 刷新显示
                RefreshDataGridView();

                // 清空详情面板
                ClearDetailsPanel();

                LogManager.Info("BOM物料清单管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("BOM物料清单管理窗体初始化失败", ex);
                MessageBox.Show("窗体初始化失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置DataGridView
        /// </summary>
        private void SetupDataGridView()
        {
            // 设置列
            dataGridViewBOM.AutoGenerateColumns = false;
            dataGridViewBOM.Columns.Clear();

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BOMCode",
                HeaderText = "BOM编码",
                DataPropertyName = "BOMCode",
                Width = 140,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 150,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialName",
                HeaderText = "子物料名称",
                DataPropertyName = "MaterialName",
                Width = 150,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "需求数量",
                DataPropertyName = "Quantity",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "单位",
                DataPropertyName = "Unit",
                Width = 80,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BOMVersion",
                HeaderText = "版本",
                DataPropertyName = "BOMVersion",
                Width = 80,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BOMType",
                HeaderText = "类型",
                DataPropertyName = "BOMType",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "Status",
                Width = 80,
                ReadOnly = true
            });

            // 设置样式
            dataGridViewBOM.EnableHeadersVisualStyles = false;
            dataGridViewBOM.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewBOM.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewBOM.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            dataGridViewBOM.ColumnHeadersHeight = 40;

            dataGridViewBOM.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
            dataGridViewBOM.DefaultCellStyle.BackColor = Color.White;
            dataGridViewBOM.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewBOM.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewBOM.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridViewBOM.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridViewBOM.GridColor = Color.FromArgb(222, 226, 230);
        }

        /// <summary>
        /// 加载BOM数据（离线/数据库由 BLL 决定）
        /// </summary>
        private void LoadData()
        {
            bomList.Clear();

            var data = _bomBLL.GetAllBOMs();
            if (data != null && data.Count > 0)
            {
                bomList.AddRange(data);
            }

            filteredBomList = new List<BOMInfo>(bomList);
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                dataGridViewBOM.DataSource = null;
                dataGridViewBOM.DataSource = filteredBomList;

                // 如果有数据，选中第一行
                if (filteredBomList.Count > 0)
                {
                    dataGridViewBOM.Rows[0].Selected = true;
                    ShowBOMDetails(filteredBomList[0]);
                }
                else
                {
                    ClearDetailsPanel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据显示失败", ex);
            }
        }

        /// <summary>
        /// 搜索框文本变化事件
        /// </summary>
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = textBoxSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    // 显示所有BOM记录
                    filteredBomList = new List<BOMInfo>(bomList);
                }
                else
                {
                    // 根据BOM编码、产品名称、物料名称进行搜索
                    filteredBomList = bomList.Where(b =>
                        (b.BOMCode ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (b.ProductName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (b.MaterialName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (b.ProductCode ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (b.MaterialCode ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (b.BOMType ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                    ).ToList();
                }

                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索失败", ex);
                MessageBox.Show("搜索失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DataGridView选择变化事件
        /// </summary>
        private void dataGridViewBOM_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewBOM.CurrentRow != null &&
                    dataGridViewBOM.CurrentRow.DataBoundItem != null)
                {
                    var bom = dataGridViewBOM.CurrentRow.DataBoundItem as BOMInfo;
                    if (bom != null)
                    {
                        ShowBOMDetails(bom);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("显示BOM详情失败", ex);
            }
        }

        /// <summary>
        /// 显示BOM详情
        /// </summary>
        private void ShowBOMDetails(BOMInfo bom)
        {
            if (bom == null)
            {
                ClearDetailsPanel();
                return;
            }

            currentBom = bom;

            // 填充基本信息
            textBoxBOMCode.Text = bom.BOMCode;
            textBoxBOMVersion.Text = bom.BOMVersion;
            comboBoxBOMType.Text = bom.BOMType;
            checkBoxStatus.Checked = bom.Status;

            // 填充产品信息
            textBoxProductCode.Text = bom.ProductCode;
            textBoxProductName.Text = bom.ProductName;

            // 填充物料信息
            textBoxMaterialCode.Text = bom.MaterialCode;
            textBoxMaterialName.Text = bom.MaterialName;
            textBoxQuantity.Text = bom.Quantity.ToString();
            textBoxUnit.Text = bom.Unit;
            textBoxLossRate.Text = bom.LossRate.ToString();
            textBoxSubstituteMaterial.Text = bom.SubstituteMaterial;

            // 填充时间信息
            dateTimePickerEffectiveDate.Value = bom.EffectiveDate;
            if (bom.ExpireDate.HasValue)
            {
                checkBoxHasExpireDate.Checked = true;
                dateTimePickerExpireDate.Value = bom.ExpireDate.Value;
                dateTimePickerExpireDate.Enabled = true;
            }
            else
            {
                checkBoxHasExpireDate.Checked = false;
                dateTimePickerExpireDate.Enabled = false;
            }

            // 填充备注
            textBoxRemarks.Text = bom.Remarks;

            // 填充创建时间
            if (bom.CreateTime != DateTime.MinValue)
            {
                labelCreateTime.Text = "创建时间：" + bom.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                labelCreateTime.Text = "创建时间：未知";
            }
        }

        /// <summary>
        /// 清空详情面板
        /// </summary>
        private void ClearDetailsPanel()
        {
            currentBom = null;

            textBoxBOMCode.Text = string.Empty;
            textBoxBOMVersion.Text = string.Empty;
            comboBoxBOMType.SelectedIndex = -1;
            checkBoxStatus.Checked = false;

            textBoxProductCode.Text = string.Empty;
            textBoxProductName.Text = string.Empty;

            textBoxMaterialCode.Text = string.Empty;
            textBoxMaterialName.Text = string.Empty;
            textBoxQuantity.Text = string.Empty;
            textBoxUnit.Text = string.Empty;
            textBoxLossRate.Text = string.Empty;
            textBoxSubstituteMaterial.Text = string.Empty;

            dateTimePickerEffectiveDate.Value = DateTime.Now;
            checkBoxHasExpireDate.Checked = false;
            dateTimePickerExpireDate.Enabled = false;

            textBoxRemarks.Text = string.Empty;
            labelCreateTime.Text = "创建时间：";
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建新BOM对话框
                var result = ShowBOMEditDialog(null);
                if (result != null)
                {
                    var success = _bomBLL.AddBOM(result);
                    if (!success)
                    {
                        MessageBox.Show("BOM记录添加失败！", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    // 选中新添加的BOM（按编码查找）
                    var added = bomList.FirstOrDefault(b =>
                        !string.IsNullOrEmpty(b.BOMCode) &&
                        b.BOMCode.Equals(result.BOMCode, StringComparison.OrdinalIgnoreCase));
                    if (added != null)
                    {
                        SelectBOMById(added.Id);
                    }

                    MessageBox.Show("BOM记录添加成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("添加BOM记录：{0} - {1}",
                        result.BOMCode, result.ProductName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加BOM记录失败", ex);
                MessageBox.Show("添加BOM记录失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentBom == null)
                {
                    MessageBox.Show("请先选择要编辑的BOM记录！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 创建编辑对话框
                var result = ShowBOMEditDialog(currentBom);
                if (result != null)
                {
                    result.Id = currentBom.Id;

                    var success = _bomBLL.UpdateBOM(result);
                    if (!success)
                    {
                        MessageBox.Show("BOM记录编辑失败：记录可能已不存在。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    // 重新选中编辑的BOM
                    SelectBOMById(result.Id);

                    MessageBox.Show("BOM记录编辑成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("编辑BOM记录：{0} - {1}", result.BOMCode, result.ProductName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("编辑BOM记录失败", ex);
                MessageBox.Show("编辑BOM记录失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentBom == null)
                {
                    MessageBox.Show("请先选择要删除的BOM记录！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    string.Format("确认要删除BOM记录 [{0} - {1}] 吗？\n\n此操作不可撤销！",
                        currentBom.BOMCode, currentBom.ProductName),
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var success = _bomBLL.DeleteBOM(currentBom.Id);
                    if (!success)
                    {
                        MessageBox.Show("BOM记录删除失败：记录可能已不存在。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    MessageBox.Show("BOM记录删除成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("删除BOM记录：{0} - {1}",
                        currentBom.BOMCode, currentBom.ProductName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除BOM记录失败", ex);
                MessageBox.Show("删除BOM记录失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空搜索框
                textBoxSearch.Text = string.Empty;

                // 重新加载数据
                LoadData();
                RefreshDataGridView();

                MessageBox.Show("数据刷新成功！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info("BOM物料清单数据已刷新");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show("刷新数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据ID选中BOM
        /// </summary>
        private void SelectBOMById(int bomId)
        {
            try
            {
                for (int i = 0; i < dataGridViewBOM.Rows.Count; i++)
                {
                    var bom = dataGridViewBOM.Rows[i].DataBoundItem as BOMInfo;
                    if (bom != null && bom.Id == bomId)
                    {
                        dataGridViewBOM.ClearSelection();
                        dataGridViewBOM.Rows[i].Selected = true;
                        dataGridViewBOM.CurrentCell = dataGridViewBOM.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("选中BOM失败", ex);
            }
        }

        /// <summary>
        /// 显示BOM编辑对话框
        /// </summary>
        private BOMInfo ShowBOMEditDialog(BOMInfo bom)
        {
            using (var dialog = new BOMEditDialog(bom))
            {
                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.Result : null;
            }
        }

        private sealed class BOMEditDialog : Form
        {
            private readonly TextBox _textBomCode;
            private readonly TextBox _textProductCode;
            private readonly TextBox _textProductName;
            private readonly TextBox _textMaterialCode;
            private readonly TextBox _textMaterialName;
            private readonly NumericUpDown _numQuantity;
            private readonly TextBox _textUnit;
            private readonly NumericUpDown _numLossRate;
            private readonly TextBox _textSubstituteMaterial;
            private readonly DateTimePicker _dtEffectiveDate;
            private readonly CheckBox _checkHasExpireDate;
            private readonly DateTimePicker _dtExpireDate;
            private readonly CheckBox _checkEnabled;
            private readonly TextBox _textRemarks;

            public BOMInfo Result { get; private set; }

            public BOMEditDialog(BOMInfo bom)
            {
                Text = bom == null ? "新增BOM" : "编辑BOM";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                Width = 620;
                Height = 650;

                var layout = new TableLayoutPanel();
                layout.Dock = DockStyle.Fill;
                layout.Padding = new Padding(12);
                layout.ColumnCount = 2;
                layout.RowCount = 13;
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                for (int i = 0; i < layout.RowCount; i++)
                {
                    layout.RowStyles.Add(new RowStyle(SizeType.Absolute, i == 12 ? 110 : 34));
                }

                _textBomCode = new TextBox { Dock = DockStyle.Fill };
                _textProductCode = new TextBox { Dock = DockStyle.Fill };
                _textProductName = new TextBox { Dock = DockStyle.Fill };
                _textMaterialCode = new TextBox { Dock = DockStyle.Fill };
                _textMaterialName = new TextBox { Dock = DockStyle.Fill };

                _numQuantity = new NumericUpDown { Dock = DockStyle.Left, Width = 180, DecimalPlaces = 4, Maximum = 99999999, Minimum = 0 };
                _textUnit = new TextBox { Dock = DockStyle.Fill };
                _numLossRate = new NumericUpDown { Dock = DockStyle.Left, Width = 180, DecimalPlaces = 4, Maximum = 100, Minimum = 0 };
                _textSubstituteMaterial = new TextBox { Dock = DockStyle.Fill };

                _dtEffectiveDate = new DateTimePicker { Dock = DockStyle.Left, Width = 220, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd" };
                _checkHasExpireDate = new CheckBox { Dock = DockStyle.Left, Text = "设置失效日期" };
                _dtExpireDate = new DateTimePicker { Dock = DockStyle.Left, Width = 220, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", Enabled = false };
                _checkEnabled = new CheckBox { Dock = DockStyle.Left, Text = "启用", Checked = true };
                _textRemarks = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };

                _checkHasExpireDate.CheckedChanged += (s, e) => { _dtExpireDate.Enabled = _checkHasExpireDate.Checked; };

                AddRow(layout, 0, "BOM编码", _textBomCode);
                AddRow(layout, 1, "产品编码", _textProductCode);
                AddRow(layout, 2, "产品名称", _textProductName);
                AddRow(layout, 3, "子物料编码", _textMaterialCode);
                AddRow(layout, 4, "子物料名称", _textMaterialName);
                AddRow(layout, 5, "用量", _numQuantity);
                AddRow(layout, 6, "单位", _textUnit);
                AddRow(layout, 7, "损耗率(%)", _numLossRate);
                AddRow(layout, 8, "替代料编码", _textSubstituteMaterial);
                AddRow(layout, 9, "生效日期", _dtEffectiveDate);

                var expirePanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
                expirePanel.Controls.Add(_checkHasExpireDate);
                expirePanel.Controls.Add(_dtExpireDate);
                AddRow(layout, 10, "失效日期", expirePanel);

                AddRow(layout, 11, "状态", _checkEnabled);
                AddRow(layout, 12, "备注", _textRemarks);

                var panelButtons = new FlowLayoutPanel();
                panelButtons.Dock = DockStyle.Bottom;
                panelButtons.FlowDirection = FlowDirection.RightToLeft;
                panelButtons.Padding = new Padding(12);
                panelButtons.Height = 54;

                var btnOk = new Button { Text = "确定", Width = 90, Height = 30, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "取消", Width = 90, Height = 30, DialogResult = DialogResult.Cancel };
                btnOk.Click += (s, e) =>
                {
                    if (!TryBuildResult(bom))
                    {
                        DialogResult = DialogResult.None;
                    }
                };

                panelButtons.Controls.Add(btnOk);
                panelButtons.Controls.Add(btnCancel);

                Controls.Add(layout);
                Controls.Add(panelButtons);

                AcceptButton = btnOk;
                CancelButton = btnCancel;

                if (bom != null)
                {
                    _textBomCode.Text = bom.BOMCode ?? string.Empty;
                    _textProductCode.Text = bom.ProductCode ?? string.Empty;
                    _textProductName.Text = bom.ProductName ?? string.Empty;
                    _textMaterialCode.Text = bom.MaterialCode ?? string.Empty;
                    _textMaterialName.Text = bom.MaterialName ?? string.Empty;
                    _numQuantity.Value = bom.Quantity;
                    _textUnit.Text = bom.Unit ?? string.Empty;
                    _numLossRate.Value = bom.LossRate;
                    _textSubstituteMaterial.Text = bom.SubstituteMaterial ?? string.Empty;
                    _dtEffectiveDate.Value = bom.EffectiveDate == DateTime.MinValue ? DateTime.Now : bom.EffectiveDate;

                    if (bom.ExpireDate.HasValue)
                    {
                        _checkHasExpireDate.Checked = true;
                        _dtExpireDate.Value = bom.ExpireDate.Value;
                    }
                    else
                    {
                        _checkHasExpireDate.Checked = false;
                        _dtExpireDate.Value = DateTime.Now;
                    }

                    _checkEnabled.Checked = bom.Status;
                    _textRemarks.Text = bom.Remarks ?? string.Empty;
                }
                else
                {
                    _dtEffectiveDate.Value = DateTime.Now;
                    _dtExpireDate.Value = DateTime.Now;
                }
            }

            private static void AddRow(TableLayoutPanel layout, int row, string labelText, Control control)
            {
                var label = new Label
                {
                    Text = labelText + "：",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight
                };

                layout.Controls.Add(label, 0, row);
                layout.Controls.Add(control, 1, row);
            }

            private bool TryBuildResult(BOMInfo original)
            {
                var bomCode = (_textBomCode.Text ?? string.Empty).Trim();
                var productCode = (_textProductCode.Text ?? string.Empty).Trim();
                var productName = (_textProductName.Text ?? string.Empty).Trim();
                var materialCode = (_textMaterialCode.Text ?? string.Empty).Trim();
                var materialName = (_textMaterialName.Text ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(bomCode))
                {
                    MessageBox.Show("BOM编码不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(productCode))
                {
                    MessageBox.Show("产品编码不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(productName))
                {
                    MessageBox.Show("产品名称不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(materialCode))
                {
                    MessageBox.Show("子物料编码不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(materialName))
                {
                    MessageBox.Show("子物料名称不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (_numQuantity.Value <= 0)
                {
                    MessageBox.Show("用量必须大于0！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var entity = new BOMInfo();
                if (original != null)
                {
                    entity.Id = original.Id;
                    entity.ProductId = original.ProductId;
                    entity.MaterialId = original.MaterialId;
                    entity.BOMVersion = string.IsNullOrWhiteSpace(original.BOMVersion) ? "1.0" : original.BOMVersion;
                    entity.BOMType = string.IsNullOrWhiteSpace(original.BOMType) ? "PRODUCTION" : original.BOMType;
                }
                else
                {
                    entity.BOMVersion = "1.0";
                    entity.BOMType = "PRODUCTION";
                }

                entity.BOMCode = bomCode;
                entity.ProductCode = productCode;
                entity.ProductName = productName;
                entity.MaterialCode = materialCode;
                entity.MaterialName = materialName;
                entity.Quantity = _numQuantity.Value;
                entity.Unit = (_textUnit.Text ?? string.Empty).Trim();
                entity.LossRate = _numLossRate.Value;
                entity.SubstituteMaterial = (_textSubstituteMaterial.Text ?? string.Empty).Trim();
                entity.EffectiveDate = _dtEffectiveDate.Value.Date;
                entity.ExpireDate = _checkHasExpireDate.Checked ? (DateTime?)_dtExpireDate.Value.Date : null;
                entity.Status = _checkEnabled.Checked;
                entity.Remarks = (_textRemarks.Text ?? string.Empty).Trim();

                Result = entity;
                return true;
            }
        }

        /// <summary>
        /// 失效日期复选框变化事件
        /// </summary>
        private void checkBoxHasExpireDate_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerExpireDate.Enabled = checkBoxHasExpireDate.Checked;
        }
    }
}
