using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.UI.Models;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 物料信息管理窗体 - 现代化界面设计
    /// </summary>
    public partial class MaterialManagementForm : Form
    {
        private List<MaterialInfo> materialList;
        private List<MaterialInfo> filteredMaterialList;
        private MaterialInfo currentMaterial;

        public MaterialManagementForm()
        {
            InitializeComponent();
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

                // 加载示例数据
                LoadSampleData();

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
        /// 加载示例数据
        /// </summary>
        private void LoadSampleData()
        {
            materialList.Clear();

            // 添加示例物料数据
            materialList.Add(new MaterialInfo("M001", "钢板", "原材料", "张")
            {
                Id = 1,
                Specification = "Q235 10mm",
                Supplier = "钢铁公司A",
                Price = 1200.50m,
                Remark = "优质钢板，用于结构件制造"
            });

            materialList.Add(new MaterialInfo("M002", "螺栓", "标准件", "个")
            {
                Id = 2,
                Specification = "M8×20",
                Supplier = "标准件厂B",
                Price = 0.85m,
                Remark = "304不锈钢螺栓"
            });

            materialList.Add(new MaterialInfo("M003", "轴承", "机械件", "个")
            {
                Id = 3,
                Specification = "6205-2RS",
                Supplier = "轴承制造商C",
                Price = 25.60m,
                Remark = "深沟球轴承，密封型"
            });

            materialList.Add(new MaterialInfo("M004", "电机", "电气件", "台")
            {
                Id = 4,
                Specification = "Y90L-4 1.5KW",
                Supplier = "电机厂D",
                Price = 680.00m,
                Remark = "三相异步电机"
            });

            materialList.Add(new MaterialInfo("M005", "铝型材", "原材料", "米")
            {
                Id = 5,
                Specification = "4040工业铝型材",
                Supplier = "铝业公司E",
                Price = 15.80m,
                Remark = "工业用铝合金型材"
            });

            // 复制到过滤列表
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
                string searchTerm = textBoxSearch.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // 显示所有物料
                    filteredMaterialList = new List<MaterialInfo>(materialList);
                }
                else
                {
                    // 根据物料编码、名称、类型进行搜索
                    filteredMaterialList = materialList.Where(m =>
                        m.MaterialCode.ToLower().Contains(searchTerm) ||
                        m.MaterialName.ToLower().Contains(searchTerm) ||
                        m.MaterialType.ToLower().Contains(searchTerm) ||
                        m.Specification.ToLower().Contains(searchTerm)
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
            textBoxPrice.Text = material.Price.ToString("F2");
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
                    // 生成新ID
                    result.Id = materialList.Count > 0 ? materialList.Max(m => m.Id) + 1 : 1;
                    result.CreateTime = DateTime.Now;
                    result.UpdateTime = DateTime.Now;

                    // 添加到列表
                    materialList.Add(result);

                    // 刷新显示
                    RefreshDataGridView();

                    // 选中新添加的物料
                    SelectMaterialById(result.Id);

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
                    // 更新物料信息
                    var originalMaterial = materialList.FirstOrDefault(m => m.Id == currentMaterial.Id);
                    if (originalMaterial != null)
                    {
                        originalMaterial.MaterialCode = result.MaterialCode;
                        originalMaterial.MaterialName = result.MaterialName;
                        originalMaterial.MaterialType = result.MaterialType;
                        originalMaterial.Unit = result.Unit;
                        originalMaterial.Specification = result.Specification;
                        originalMaterial.Supplier = result.Supplier;
                        originalMaterial.Price = result.Price;
                        originalMaterial.Remark = result.Remark;
                        originalMaterial.UpdateTime = DateTime.Now;

                        // 刷新显示
                        RefreshDataGridView();

                        // 重新选中编辑的物料
                        SelectMaterialById(originalMaterial.Id);

                        MessageBox.Show("物料编辑成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("编辑物料：{0} - {1}",
                            originalMaterial.MaterialCode, originalMaterial.MaterialName));
                    }
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
                    // 从列表中移除
                    materialList.RemoveAll(m => m.Id == currentMaterial.Id);

                    // 刷新显示
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
                LoadSampleData();
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
            // 这里应该打开一个物料编辑对话框
            // 为了演示，我们使用简单的输入框
            string title = material == null ? "新增物料" : "编辑物料";

            // 简化的编辑逻辑，实际应该使用专门的编辑窗体
            var editMaterial = material != null ? material.Clone() : new MaterialInfo();

            // 这里可以实现一个简单的编辑对话框
            // 或者调用专门的MaterialEditForm

            // 暂时返回示例数据用于演示
            if (material == null)
            {
                // 新增物料的示例
                return new MaterialInfo("M" + (materialList.Count + 1).ToString("000"),
                    "新物料", "新类型", "个")
                {
                    Specification = "规格待定",
                    Supplier = "供应商待定",
                    Price = 0,
                    Remark = "新增物料"
                };
            }
            else
            {
                // 编辑现有物料
                editMaterial.MaterialName = editMaterial.MaterialName + " (已编辑)";
                return editMaterial;
            }
        }
    }
}
