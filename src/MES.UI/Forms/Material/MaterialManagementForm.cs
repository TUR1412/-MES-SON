using MES.BLL.Material;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MES.UI.Forms.Material
{
    public partial class MaterialManagementForm : Form
    {
        private MaterialBLL _materialBLL = new MaterialBLL();
        public MaterialManagementForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 数据加载事件
        /// </summary>
        private void LoadMaterialData()
        {
            try
            {
                var materials = _materialBLL.GetAllMaterialDtos(); // 通过 BLL 获取 DTO 列表
                dataGridView1.DataSource = materials;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载物料数据失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void MaterialManagementForm_Load(object sender, EventArgs e)
        {
            LoadMaterialData();
        }

        /// <summary>
        /// 搜索框的搜索功能
        /// </summary>
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox10.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadMaterialData(); // 如果搜索框为空，显示所有物料
            }
            else
            {
                try
                {
                    var materials = _materialBLL.SearchMaterialDtosByName(searchTerm);
                    dataGridView1.DataSource = materials;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("搜索失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 增加按钮点击事件
        /// </summary>
        private void buttonIns_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                using (var editForm = new MaterialEditForm())
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        var material = editForm.Material;
                        bool success = _materialBLL.AddMaterial(material);
                        if (success)
                        {
                            LoadMaterialData(); // 刷新数据
                            MessageBox.Show("物料添加成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("物料添加失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("添加物料失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
        }


        /// <summary>
        /// 修改按钮点击事件
        /// </summary>
        private void buttonAlt_Click(object sender, EventArgs e)
        {
            /*
             try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    int materialId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
                    var materialDto = _materialBLL.GetAllMaterialDtos().FirstOrDefault(m => m.Id == materialId);

                    if (materialDto != null)
                    {
                        using (var editForm = new MaterialEditForm(materialDto))
                        {
                            if (editForm.ShowDialog() == DialogResult.OK)
                            {
                                bool success = _materialBLL.UpdateMaterial(editForm.Material);
                                if (success)
                                {
                                    LoadMaterialData(); // 刷新数据
                                    MessageBox.Show("物料修改成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("物料修改失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要修改的物料", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("修改物料失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
        }


        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void buttonDel_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    int materialId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

                    DialogResult result = MessageBox.Show(
                        "确认要删除选中的物料吗？",
                        "确认删除",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        bool success = _materialBLL.DeleteMaterial(materialId);
                        if (success)
                        {
                            LoadMaterialData(); // 刷新数据
                            MessageBox.Show("物料删除成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("物料删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要删除的物料", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("删除物料失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
        }
    }
}
