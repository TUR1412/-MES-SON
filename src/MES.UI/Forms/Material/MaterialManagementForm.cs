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
    /// 物料信息管理窗体 - 现代化界面设计
    /// </summary>
    public partial class MaterialManagementForm : Form
    {
        private readonly IMaterialBLL _materialBLL;

        private List<MaterialInfo> materialList;
        private List<MaterialInfo> filteredMaterialList;
        private MaterialInfo currentMaterial;

        public MaterialManagementForm()
        {
            InitializeComponent();
            _materialBLL = new MaterialBLL();
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
                materialList = new List<MaterialInfo>();
                filteredMaterialList = new List<MaterialInfo>();
                currentMaterial = null;

                // 设置DataGridView
                SetupDataGridView();

                // 加载数据（离线/数据库由 BLL 决定）
                LoadData();

                // 刷新显示
                RefreshDataGridView();

                // 清空详情面板
                ClearDetailsPanel();

                LogManager.Info("物料管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("物料管理窗体初始化失败", ex);
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
            dataGridViewMaterials.AutoGenerateColumns = false;
            dataGridViewMaterials.Columns.Clear();

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialCode",
                HeaderText = "物料编码",
                DataPropertyName = "MaterialCode",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialName",
                HeaderText = "物料名称",
                DataPropertyName = "MaterialName",
                Width = 200,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialType",
                HeaderText = "物料类型",
                DataPropertyName = "MaterialType",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "计量单位",
                DataPropertyName = "Unit",
                Width = 80,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Specification",
                HeaderText = "规格型号",
                DataPropertyName = "Specification",
                Width = 150,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Supplier",
                HeaderText = "供应商",
                DataPropertyName = "Supplier",
                Width = 120,
                ReadOnly = true
            });

            // 设置样式
            dataGridViewMaterials.EnableHeadersVisualStyles = false;
            dataGridViewMaterials.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewMaterials.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewMaterials.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            dataGridViewMaterials.ColumnHeadersHeight = 40;

            dataGridViewMaterials.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
            dataGridViewMaterials.DefaultCellStyle.BackColor = Color.White;
            dataGridViewMaterials.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewMaterials.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewMaterials.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridViewMaterials.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridViewMaterials.GridColor = Color.FromArgb(222, 226, 230);
        }

        /// <summary>
        /// 加载物料数据（离线/数据库由 BLL 决定）
        /// </summary>
        private void LoadData()
        {
            materialList.Clear();

            var data = _materialBLL.GetAllMaterials();
            if (data != null && data.Count > 0)
            {
                materialList.AddRange(data);
            }

            filteredMaterialList = new List<MaterialInfo>(materialList);
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                dataGridViewMaterials.DataSource = null;
                dataGridViewMaterials.DataSource = filteredMaterialList;

                // 如果有数据，选中第一行
                if (filteredMaterialList.Count > 0)
                {
                    dataGridViewMaterials.Rows[0].Selected = true;
                    ShowMaterialDetails(filteredMaterialList[0]);
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
                    // 显示所有物料
                    filteredMaterialList = new List<MaterialInfo>(materialList);
                }
                else
                {
                    // 根据物料编码、名称、类型进行搜索
                    filteredMaterialList = materialList.Where(m =>
                        (m.MaterialCode ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (m.MaterialName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (m.MaterialType ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (m.Specification ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
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
        private void dataGridViewMaterials_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewMaterials.CurrentRow != null &&
                    dataGridViewMaterials.CurrentRow.DataBoundItem != null)
                {
                    var material = dataGridViewMaterials.CurrentRow.DataBoundItem as MaterialInfo;
                    if (material != null)
                    {
                        ShowMaterialDetails(material);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("显示物料详情失败", ex);
            }
        }

        /// <summary>
        /// 显示物料详情
        /// </summary>
        private void ShowMaterialDetails(MaterialInfo material)
        {
            if (material == null)
            {
                ClearDetailsPanel();
                return;
            }

            currentMaterial = material;

            // 填充基本信息
            textBoxMaterialCode.Text = material.MaterialCode;
            textBoxMaterialName.Text = material.MaterialName;
            textBoxMaterialType.Text = material.MaterialType;
            textBoxUnit.Text = material.Unit;

            // 填充详细信息
            textBoxSpecification.Text = material.Specification;
            textBoxSupplier.Text = material.Supplier;
            textBoxPrice.Text = material.StandardCost.HasValue ? material.StandardCost.Value.ToString("F2") : string.Empty;
            textBoxRemark.Text = material.Remark;
        }

        /// <summary>
        /// 清空详情面板
        /// </summary>
        private void ClearDetailsPanel()
        {
            currentMaterial = null;

            textBoxMaterialCode.Text = string.Empty;
            textBoxMaterialName.Text = string.Empty;
            textBoxMaterialType.Text = string.Empty;
            textBoxUnit.Text = string.Empty;
            textBoxSpecification.Text = string.Empty;
            textBoxSupplier.Text = string.Empty;
            textBoxPrice.Text = string.Empty;
            textBoxRemark.Text = string.Empty;
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建新物料对话框
                var result = ShowMaterialEditDialog(null);
                if (result != null)
                {
                    var success = _materialBLL.AddMaterial(result);
                    if (!success)
                    {
                        MessageBox.Show("物料添加失败！", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    // 选中新添加的物料（按编码查找）
                    var added = materialList.FirstOrDefault(m =>
                        !string.IsNullOrEmpty(m.MaterialCode) &&
                        m.MaterialCode.Equals(result.MaterialCode, StringComparison.OrdinalIgnoreCase));
                    if (added != null)
                    {
                        SelectMaterialById(added.Id);
                    }

                    MessageBox.Show("物料添加成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("添加物料：{0} - {1}", result.MaterialCode, result.MaterialName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加物料失败", ex);
                MessageBox.Show("添加物料失败：" + ex.Message, "错误",
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
                if (currentMaterial == null)
                {
                    MessageBox.Show("请先选择要编辑的物料！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 创建编辑对话框
                var result = ShowMaterialEditDialog(currentMaterial);
                if (result != null)
                {
                    result.Id = currentMaterial.Id;

                    var success = _materialBLL.UpdateMaterial(result);
                    if (!success)
                    {
                        MessageBox.Show("物料编辑失败：记录可能已不存在。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    // 重新选中编辑的物料
                    SelectMaterialById(result.Id);

                    MessageBox.Show("物料编辑成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("编辑物料：{0} - {1}", result.MaterialCode, result.MaterialName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("编辑物料失败", ex);
                MessageBox.Show("编辑物料失败：" + ex.Message, "错误",
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
                if (currentMaterial == null)
                {
                    MessageBox.Show("请先选择要删除的物料！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    string.Format("确认要删除物料 [{0} - {1}] 吗？\n\n此操作不可撤销！",
                        currentMaterial.MaterialCode, currentMaterial.MaterialName),
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var success = _materialBLL.DeleteMaterial(currentMaterial.Id);
                    if (!success)
                    {
                        MessageBox.Show("物料删除失败：记录可能已不存在。", "失败",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 重新加载 + 刷新显示
                    LoadData();
                    RefreshDataGridView();

                    MessageBox.Show("物料删除成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("删除物料：{0} - {1}",
                        currentMaterial.MaterialCode, currentMaterial.MaterialName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除物料失败", ex);
                MessageBox.Show("删除物料失败：" + ex.Message, "错误",
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

                LogManager.Info("物料数据已刷新");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show("刷新数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据ID选中物料
        /// </summary>
        private void SelectMaterialById(int materialId)
        {
            try
            {
                for (int i = 0; i < dataGridViewMaterials.Rows.Count; i++)
                {
                    var material = dataGridViewMaterials.Rows[i].DataBoundItem as MaterialInfo;
                    if (material != null && material.Id == materialId)
                    {
                        dataGridViewMaterials.ClearSelection();
                        dataGridViewMaterials.Rows[i].Selected = true;
                        dataGridViewMaterials.CurrentCell = dataGridViewMaterials.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("选中物料失败", ex);
            }
        }

        /// <summary>
        /// 显示物料编辑对话框
        /// </summary>
        private MaterialInfo ShowMaterialEditDialog(MaterialInfo material)
        {
            using (var dialog = new MaterialEditDialog(material))
            {
                return dialog.ShowDialog(this) == DialogResult.OK ? dialog.Result : null;
            }
        }

        private sealed class MaterialEditDialog : Form
        {
            private readonly TextBox _textMaterialCode;
            private readonly TextBox _textMaterialName;
            private readonly TextBox _textMaterialType;
            private readonly TextBox _textUnit;
            private readonly TextBox _textSpecification;
            private readonly TextBox _textSupplier;
            private readonly TextBox _textStandardCost;
            private readonly TextBox _textRemark;
            private readonly CheckBox _checkEnabled;

            public MaterialInfo Result { get; private set; }

            public MaterialEditDialog(MaterialInfo material)
            {
                Text = material == null ? "新增物料" : "编辑物料";
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                Width = 560;
                Height = 520;

                var layout = new TableLayoutPanel();
                layout.Dock = DockStyle.Fill;
                layout.Padding = new Padding(12);
                layout.ColumnCount = 2;
                layout.RowCount = 9;
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                for (int i = 0; i < layout.RowCount; i++)
                {
                    layout.RowStyles.Add(new RowStyle(SizeType.Absolute, i == 7 ? 90 : 34));
                }

                _textMaterialCode = new TextBox { Dock = DockStyle.Fill };
                _textMaterialName = new TextBox { Dock = DockStyle.Fill };
                _textMaterialType = new TextBox { Dock = DockStyle.Fill };
                _textUnit = new TextBox { Dock = DockStyle.Fill };
                _textSpecification = new TextBox { Dock = DockStyle.Fill };
                _textSupplier = new TextBox { Dock = DockStyle.Fill };
                _textStandardCost = new TextBox { Dock = DockStyle.Fill };
                _textRemark = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
                _checkEnabled = new CheckBox { Dock = DockStyle.Left, Text = "启用", Checked = true };

                AddRow(layout, 0, "物料编码", _textMaterialCode);
                AddRow(layout, 1, "物料名称", _textMaterialName);
                AddRow(layout, 2, "物料类型", _textMaterialType);
                AddRow(layout, 3, "计量单位", _textUnit);
                AddRow(layout, 4, "规格型号", _textSpecification);
                AddRow(layout, 5, "供应商", _textSupplier);
                AddRow(layout, 6, "标准成本", _textStandardCost);
                AddRow(layout, 7, "备注", _textRemark);
                AddRow(layout, 8, "状态", _checkEnabled);

                var panelButtons = new FlowLayoutPanel();
                panelButtons.Dock = DockStyle.Bottom;
                panelButtons.FlowDirection = FlowDirection.RightToLeft;
                panelButtons.Padding = new Padding(12);
                panelButtons.Height = 54;

                var btnOk = new Button { Text = "确定", Width = 90, Height = 30, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "取消", Width = 90, Height = 30, DialogResult = DialogResult.Cancel };

                btnOk.Click += (s, e) =>
                {
                    if (!TryBuildResult(material))
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

                if (material != null)
                {
                    _textMaterialCode.Text = material.MaterialCode ?? string.Empty;
                    _textMaterialName.Text = material.MaterialName ?? string.Empty;
                    _textMaterialType.Text = material.MaterialType ?? string.Empty;
                    _textUnit.Text = material.Unit ?? string.Empty;
                    _textSpecification.Text = material.Specification ?? string.Empty;
                    _textSupplier.Text = material.Supplier ?? string.Empty;
                    _textStandardCost.Text = material.StandardCost.HasValue ? material.StandardCost.Value.ToString("F2") : string.Empty;
                    _textRemark.Text = material.Remark ?? string.Empty;
                    _checkEnabled.Checked = material.Status;
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

            private bool TryBuildResult(MaterialInfo original)
            {
                var materialCode = (_textMaterialCode.Text ?? string.Empty).Trim();
                var materialName = (_textMaterialName.Text ?? string.Empty).Trim();
                var materialType = (_textMaterialType.Text ?? string.Empty).Trim();
                var unit = (_textUnit.Text ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(materialCode))
                {
                    MessageBox.Show("物料编码不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(materialName))
                {
                    MessageBox.Show("物料名称不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(materialType))
                {
                    MessageBox.Show("物料类型不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(unit))
                {
                    MessageBox.Show("计量单位不能为空！", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                decimal? standardCost = null;
                var costText = (_textStandardCost.Text ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(costText))
                {
                    decimal parsedCost;
                    if (!decimal.TryParse(costText, out parsedCost))
                    {
                        MessageBox.Show("标准成本格式不正确，请输入数字。", "校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    standardCost = parsedCost;
                }

                var entity = new MaterialInfo();
                if (original != null)
                {
                    entity.Id = original.Id;
                }

                entity.MaterialCode = materialCode;
                entity.MaterialName = materialName;
                entity.MaterialType = materialType;
                entity.Unit = unit;
                entity.Specification = (_textSpecification.Text ?? string.Empty).Trim();
                entity.Supplier = (_textSupplier.Text ?? string.Empty).Trim();
                entity.StandardCost = standardCost;
                entity.Remark = (_textRemark.Text ?? string.Empty).Trim();
                entity.Status = _checkEnabled.Checked;

                Result = entity;
                return true;
            }
        }
    }
}
