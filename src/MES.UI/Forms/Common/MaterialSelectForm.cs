using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MES.BLL.Material.DTO;
using MES.BLL.Material;
using MES.Common.Logging;

namespace MES.UI.Forms.Common
{
    /// <summary>
    /// 物料选择窗体
    /// </summary>
    public partial class MaterialSelectForm : Form
    {
        /// <summary>
        /// 选中的物料
        /// </summary>
        public MaterialDto SelectedMaterial { get; private set; }

        private List<MaterialDto> materialList;
        private List<MaterialDto> filteredList;
        private readonly IMaterialBLL _materialBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialSelectForm()
        {
            InitializeComponent();
            _materialBLL = new MaterialBLL();
            LoadMaterialData();
        }

        /// <summary>
        /// 加载物料数据
        /// </summary>
        private void LoadMaterialData()
        {
            try
            {
                materialList = _materialBLL.GetAllMaterialDtos();
                filteredList = new List<MaterialDto>(materialList);
                dgvMaterials.DataSource = filteredList;
                
                if (filteredList.Count > 0)
                {
                    dgvMaterials.Rows[0].Selected = true;
                }
                
                lblCount.Text = string.Format("共 {0} 条记录", filteredList.Count);
            }
            catch (Exception ex)
            {
                LogManager.Error("加载物料数据失败", ex);
                MessageBox.Show("加载物料数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 搜索物料
        /// </summary>
        private void SearchMaterials()
        {
            try
            {
                string keyword = txtSearch.Text.Trim().ToLower();
                
                if (string.IsNullOrEmpty(keyword))
                {
                    filteredList = new List<MaterialDto>(materialList);
                }
                else
                {
                    filteredList = materialList.Where(m =>
                        m.MaterialCode.ToLower().Contains(keyword) ||
                        m.MaterialName.ToLower().Contains(keyword) ||
                        m.MaterialType.ToLower().Contains(keyword)
                    ).ToList();
                }
                
                dgvMaterials.DataSource = null;
                dgvMaterials.DataSource = filteredList;
                
                if (filteredList.Count > 0)
                {
                    dgvMaterials.Rows[0].Selected = true;
                }
                
                lblCount.Text = string.Format("共 {0} 条记录", filteredList.Count);
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索物料失败", ex);
                MessageBox.Show("搜索物料失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchMaterials();
        }

        /// <summary>
        /// 搜索框回车事件
        /// </summary>
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchMaterials();
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvMaterials.SelectedRows.Count > 0)
            {
                SelectedMaterial = dgvMaterials.SelectedRows[0].DataBoundItem as MaterialDto;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择一个物料！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 双击选择
        /// </summary>
        private void dgvMaterials_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnOK_Click(sender, e);
            }
        }
    }
}