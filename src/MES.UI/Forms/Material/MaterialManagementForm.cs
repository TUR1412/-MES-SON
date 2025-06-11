using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.BLL.Material;
using MES.Models.Material;
using MES.BLL.Material.DTO;
using MES.UI.Framework.Themes;
using MES.Common.Logging;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 物料管理主窗口 - 使用真实LOL主题
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public partial class MaterialManagementForm : Form
    {
        private MaterialBLL materialBLL;
        private List<MaterialInfo> materialList;

        public MaterialManagementForm()
        {
            InitializeComponent();
            materialBLL = new MaterialBLL();
            materialList = new List<MaterialInfo>();

            // 在窗体加载时应用真实LOL主题
            this.Load += (sender, e) =>
            {
                ApplyLeagueTheme();
                LoadMaterialData();
            };
        }

        /// <summary>
        /// 应用真实LOL主题 - 基于真实LOL客户端设计
        /// 严格遵循C# 5.0语法规范
        /// </summary>
        private void ApplyLeagueTheme()
        {
            try
            {
                // 使用新的真实LOL主题应用器
                MES.UI.Framework.Themes.RealLeagueThemeApplier.ApplyRealLeagueTheme(this);
            }
            catch (Exception ex)
            {
                LogManager.Error("应用真实LOL主题失败", ex);
                MessageBox.Show(string.Format("应用真实LOL主题失败: {0}", ex.Message), "主题错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 加载物料数据
        /// </summary>
        private void LoadMaterialData()
        {
            try
            {
                materialList = materialBLL.GetAllMaterials();

                // 绑定到DataGridView
                dataGridViewMaterials.DataSource = materialList;

                // 设置列标题
                if (dataGridViewMaterials.Columns.Count > 0)
                {
                    dataGridViewMaterials.Columns["MaterialCode"].HeaderText = "物料编码";
                    dataGridViewMaterials.Columns["MaterialName"].HeaderText = "物料名称";
                    dataGridViewMaterials.Columns["MaterialType"].HeaderText = "物料类型";
                    dataGridViewMaterials.Columns["Unit"].HeaderText = "计量单位";
                    dataGridViewMaterials.Columns["Price"].HeaderText = "参考价格";
                    dataGridViewMaterials.Columns["Specification"].HeaderText = "规格型号";
                    dataGridViewMaterials.Columns["Supplier"].HeaderText = "供应商";

                    // 隐藏不需要显示的列
                    if (dataGridViewMaterials.Columns["Id"] != null)
                        dataGridViewMaterials.Columns["Id"].Visible = false;
                }

                // 清空详细信息显示
                ClearDetailDisplay();
            }
            catch (Exception ex)
            {
                LogManager.Error("加载物料数据失败", ex);
                MessageBox.Show(string.Format("加载物料数据失败: {0}", ex.Message), "数据错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清空详细信息显示
        /// </summary>
        private void ClearDetailDisplay()
        {
            textBoxMaterialCode.Text = "";
            textBoxMaterialName.Text = "";
            textBoxMaterialType.Text = "";
            textBoxUnit.Text = "";
            textBoxSpecification.Text = "";
            textBoxSupplier.Text = "";
            textBoxPrice.Text = "";
            textBoxRemark.Text = "";
        }

        /// <summary>
        /// 显示选中物料的详细信息
        /// </summary>
        /// <param name="material">选中的物料信息</param>
        private void DisplayMaterialDetail(MaterialInfo material)
        {
            if (material != null)
            {
                textBoxMaterialCode.Text = material.MaterialCode ?? "";
                textBoxMaterialName.Text = material.MaterialName ?? "";
                textBoxMaterialType.Text = material.MaterialType ?? "";
                textBoxUnit.Text = material.Unit ?? "";
                textBoxSpecification.Text = material.Specification ?? "";
                textBoxSupplier.Text = material.Supplier ?? "";
                textBoxPrice.Text = material.Price.ToString("F2");
                textBoxRemark.Text = material.Remark ?? "";
            }
            else
            {
                ClearDetailDisplay();
            }
        }

        /// <summary>
        /// 获取当前选中的物料
        /// </summary>
        /// <returns>选中的物料信息，如果没有选中则返回null</returns>
        private MaterialInfo GetSelectedMaterial()
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0)
            {
                return dataGridViewMaterials.SelectedRows[0].DataBoundItem as MaterialInfo;
            }
            return null;
        }

        #region 事件处理

        /// <summary>
        /// 新增物料按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialEditForm editForm = new MaterialEditForm(null); // null表示新增模式
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadMaterialData(); // 刷新数据
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("打开新增物料窗口失败", ex);
                MessageBox.Show(string.Format("打开新增物料窗口失败: {0}", ex.Message), "操作错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑物料按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialInfo selectedMaterial = GetSelectedMaterial();
                if (selectedMaterial != null)
                {
                    // 需要将MaterialInfo转换为MaterialDto
                    var materialDto = ConvertToMaterialDto(selectedMaterial);
                    MaterialEditForm editForm = new MaterialEditForm(materialDto);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // 保存编辑后的数据
                        SaveMaterialData(editForm.MaterialData);
                        LoadMaterialData(); // 刷新数据
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要编辑的物料！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("打开编辑物料窗口失败", ex);
                MessageBox.Show(string.Format("打开编辑物料窗口失败: {0}", ex.Message), "操作错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除物料按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialInfo selectedMaterial = GetSelectedMaterial();
                if (selectedMaterial != null)
                {
                    DialogResult result = MessageBox.Show(
                        string.Format("确定要删除物料 [{0}] {1} 吗？",
                            selectedMaterial.MaterialCode, selectedMaterial.MaterialName),
                        "确认删除",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        materialBLL.DeleteMaterial(selectedMaterial.Id);
                        LoadMaterialData(); // 刷新数据
                        MessageBox.Show("删除成功！", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要删除的物料！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除物料失败", ex);
                MessageBox.Show(string.Format("删除物料失败: {0}", ex.Message), "操作错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新数据按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMaterialData();
        }

        /// <summary>
        /// 搜索文本框文本改变事件
        /// </summary>
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = textBoxSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchText))
                {
                    // 显示所有数据
                    dataGridViewMaterials.DataSource = materialList;
                }
                else
                {
                    // 过滤数据
                    var filteredList = materialList.Where(m =>
                        (m.MaterialCode != null && m.MaterialCode.Contains(searchText)) ||
                        (m.MaterialName != null && m.MaterialName.Contains(searchText)) ||
                        (m.MaterialType != null && m.MaterialType.Contains(searchText)) ||
                        (m.Supplier != null && m.Supplier.Contains(searchText))
                    ).ToList();

                    dataGridViewMaterials.DataSource = filteredList;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索物料失败", ex);
            }
        }

        /// <summary>
        /// DataGridView选择改变事件
        /// </summary>
        private void dataGridViewMaterials_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                MaterialInfo selectedMaterial = GetSelectedMaterial();
                DisplayMaterialDetail(selectedMaterial);
            }
            catch (Exception ex)
            {
                LogManager.Error("显示物料详细信息失败", ex);
            }
        }

        #endregion

        #region 数据转换和保存

        /// <summary>
        /// 将MaterialInfo转换为MaterialDto
        /// </summary>
        /// <param name="materialInfo">物料信息</param>
        /// <returns>物料DTO</returns>
        private MaterialDto ConvertToMaterialDto(MaterialInfo materialInfo)
        {
            return new MaterialDto
            {
                Id = materialInfo.Id,
                MaterialCode = materialInfo.MaterialCode,
                MaterialName = materialInfo.MaterialName,
                MaterialType = materialInfo.MaterialType,
                Unit = materialInfo.Unit,
                Specification = materialInfo.Specification,
                Supplier = materialInfo.Supplier,
                Price = materialInfo.Price,
                Remark = materialInfo.Remark
            };
        }

        /// <summary>
        /// 保存物料数据
        /// </summary>
        /// <param name="materialDto">物料DTO</param>
        private void SaveMaterialData(MaterialDto materialDto)
        {
            try
            {
                if (materialDto.Id > 0)
                {
                    // 更新现有物料
                    var materialInfo = new MaterialInfo
                    {
                        Id = materialDto.Id,
                        MaterialCode = materialDto.MaterialCode,
                        MaterialName = materialDto.MaterialName,
                        MaterialType = materialDto.MaterialType,
                        Unit = materialDto.Unit,
                        Specification = materialDto.Specification,
                        Supplier = materialDto.Supplier,
                        Price = materialDto.Price,
                        Remark = materialDto.Remark
                    };
                    materialBLL.UpdateMaterial(materialInfo);
                }
                else
                {
                    // 新增物料
                    var materialInfo = new MaterialInfo
                    {
                        MaterialCode = materialDto.MaterialCode,
                        MaterialName = materialDto.MaterialName,
                        MaterialType = materialDto.MaterialType,
                        Unit = materialDto.Unit,
                        Specification = materialDto.Specification,
                        Supplier = materialDto.Supplier,
                        Price = materialDto.Price,
                        Remark = materialDto.Remark
                    };
                    materialBLL.AddMaterial(materialInfo);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("保存物料数据失败", ex);
                throw;
            }
        }

        #endregion
    }
}