using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.BLL.Material;       // 引用BLL接口
using MES.BLL.Material.DTO;   // 引用DTO

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 物料信息管理窗体 - 现代化界面设计
    /// </summary>
    public partial class MaterialManagementForm : Form
    {
        // --- 依赖于BLL接口和DTO ---
        private readonly IMaterialBLL _materialBLL;
        private List<MaterialDto> materialList;
        private List<MaterialDto> filteredMaterialList;
        private MaterialDto currentMaterial;

        public MaterialManagementForm()
        {

            InitializeComponent();
            // 实例化BLL
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
                materialList = new List<MaterialDto>();
                filteredMaterialList = new List<MaterialDto>();
                currentMaterial = null;

                SetupDataGridView();

                // --- 从BLL加载真实数据，而不是示例数据 ---
                LoadMaterialData();

                RefreshDataGridView();
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
        /// 从BLL加载物料数据
        /// </summary>
        private void LoadMaterialData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                materialList = _materialBLL.GetAllMaterialDtos();
                // 默认显示所有
                filteredMaterialList = new List<MaterialDto>(materialList);
            }
            catch (Exception ex)
            {
                LogManager.Error("加载物料数据失败", ex);
                MessageBox.Show("加载物料数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                // 记录当前选中项的ID，以便刷新后能重新选中
                int selectedId = (currentMaterial != null) ? currentMaterial.Id : 0;

                dataGridViewMaterials.DataSource = null;
                dataGridViewMaterials.DataSource = filteredMaterialList;

                if (filteredMaterialList.Any())
                {
                    // 尝试重新选中之前的项，如果找不到则选中第一项
                    var itemToSelect = filteredMaterialList.FirstOrDefault(m => m.Id == selectedId) ?? filteredMaterialList.First();
                    SelectMaterialById(itemToSelect.Id);
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
                    filteredMaterialList = new List<MaterialDto>(materialList);
                }
                else
                {
                    // 使用DTO的属性进行客户端筛选
                    filteredMaterialList = materialList.Where(m =>
                        (m.MaterialCode != null && m.MaterialCode.ToLower().Contains(searchTerm)) ||
                        (m.MaterialName != null && m.MaterialName.ToLower().Contains(searchTerm)) ||
                        (m.MaterialType != null && m.MaterialType.ToLower().Contains(searchTerm)) ||
                        (m.Specification != null && m.Specification.ToLower().Contains(searchTerm))
                    ).ToList();
                }

                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索失败", ex);
                MessageBox.Show("搜索失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    // 将选中项转为DTO
                    var material = dataGridViewMaterials.CurrentRow.DataBoundItem as MaterialDto;
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
        private void ShowMaterialDetails(MaterialDto material)
        {
            if (material == null)
            {
                ClearDetailsPanel();
                return;
            }
            currentMaterial = material;
            textBoxMaterialCode.Text = material.MaterialCode;
            textBoxMaterialName.Text = material.MaterialName;
            textBoxMaterialType.Text = material.MaterialType;
            textBoxUnit.Text = material.Unit;
            textBoxSpecification.Text = material.Specification;
            textBoxSupplier.Text = material.Supplier;
            textBoxPrice.Text = material.Price.ToString("F2");
            textBoxRemark.Text = material.Remark;
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开新增窗体
                using (var form = new MaterialEditForm(null))
                {
                    // 如果用户在编辑窗体点击了“保存”
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // 获取新窗体返回的、已填充好的DTO
                        var newMaterialDto = form.MaterialData;

                        // 调用BLL进行添加
                        if (_materialBLL.AddMaterial(newMaterialDto))
                        {
                            MessageBox.Show("物料添加成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogManager.Info(string.Format("添加物料：{0}", newMaterialDto.MaterialName));
                            // 重新加载数据并刷新界面
                            LoadMaterialData();
                            RefreshDataGridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加物料操作失败", ex);
                MessageBox.Show("添加物料失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (currentMaterial == null)
            {
                MessageBox.Show("请先选择要编辑的物料！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 将当前选中的物料DTO传递给编辑窗体
                using (var form = new MaterialEditForm(currentMaterial))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // 获取编辑后的DTO
                        var editedMaterialDto = form.MaterialData;

                        // 调用BLL进行更新
                        if (_materialBLL.UpdateMaterial(editedMaterialDto))
                        {
                            MessageBox.Show("物料编辑成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogManager.Info(string.Format("编辑物料：{0}", editedMaterialDto.MaterialName));
                            LoadMaterialData();
                            RefreshDataGridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("编辑物料操作失败", ex);
                MessageBox.Show("编辑物料失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentMaterial == null)
            {
                MessageBox.Show("请先选择要删除的物料！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var confirmResult = MessageBox.Show(string.Format("您确定要删除物料 [{0}] 吗？", currentMaterial.MaterialName),
                                                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // 调用BLL进行删除
                    if (_materialBLL.DeleteMaterial(currentMaterial.Id))
                    {
                        MessageBox.Show("物料删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("删除物料 ID: {0}, 名称: {1}", currentMaterial.Id, currentMaterial.MaterialName));
                        LoadMaterialData();
                        RefreshDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("删除失败，可能该物料已被使用或不存在。", "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除物料操作失败", ex);
                MessageBox.Show("删除物料失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
            LoadMaterialData();
            RefreshDataGridView();
            MessageBox.Show("数据已刷新", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- 以下是辅助方法，基本不变或微调 ---

        /// <summary>
        /// 根据ID选中DataGridView中的行
        /// </summary>
        private void SelectMaterialById(int materialId)
        {
            for (int i = 0; i < dataGridViewMaterials.Rows.Count; i++)
            {
                var material = dataGridViewMaterials.Rows[i].DataBoundItem as MaterialDto;
                if (material != null && material.Id == materialId)
                {
                    dataGridViewMaterials.ClearSelection();
                    dataGridViewMaterials.Rows[i].Selected = true;
                    dataGridViewMaterials.CurrentCell = dataGridViewMaterials.Rows[i].Cells[0];
                    return;
                }
            }
        }

        /// <summary>
        /// 清空详情面板
        /// </summary>
        private void ClearDetailsPanel() { /* ... 此方法无需修改 ... */ }
        private void SetupDataGridView() { /* ... 此方法无需修改 ... */ }

        /// <summary>
        /// 显示物料编辑对话框 (模拟)
        /// </summary>
        private MaterialDto ShowMaterialEditDialog(MaterialDto material)
        {
            // 在真实应用中，这里会打开一个新窗体，例如：
            // using (var form = new MaterialEditForm(material))
            // {
            //     if (form.ShowDialog() == DialogResult.OK)
            //     {
            //         return form.MaterialData; // 返回编辑后的DTO
            //     }
            //     return null;
            // }

            // --- 以下为模拟代码 ---
            if (material == null) // 新增模式
            {
                return new MaterialDto
                {
                    MaterialCode = "M-NEW-" + new Random().Next(100, 999),
                    MaterialName = "新物料-" + DateTime.Now.Second,
                    MaterialType = "原材料",
                    Unit = "个",
                    Price = 10.0m,
                    Remark = "这是一个新增的物料"
                };
            }
            else // 编辑模式
            {
                var editedDto = material.Clone(); // 创建副本进行编辑
                editedDto.MaterialName += " (已编辑)";
                editedDto.Price += 1;
                return editedDto;
            }
        }
    }
}