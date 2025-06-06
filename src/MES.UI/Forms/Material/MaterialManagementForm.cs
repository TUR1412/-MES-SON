using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.BLL.Material;
using MES.Models.Material;
using MES.Common.Logging;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 物料管理主界面
    /// 提供物料信息的增删改查功能
    /// </summary>
    public partial class MaterialManagementForm : Form
    {
        #region 私有字段

        private MaterialBLL materialBLL;
        private BOMBLL bomBLL;
        private List<MaterialInfo> currentMaterials;

        #endregion

        #region 构造函数

        public MaterialManagementForm()
        {
            InitializeComponent();
            InitializeData();
            InitializeEvents();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitializeData()
        {
            try
            {
                materialBLL = new MaterialBLL();
                bomBLL = new BOMBLL();
                currentMaterials = new List<MaterialInfo>();

                LoadMaterialList();
                LoadCategories();
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化物料管理界面失败", ex);
                MessageBox.Show($"初始化失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitializeEvents()
        {
            // TODO: L成员需要在设计器中添加控件后，在这里绑定事件
            // 示例：
            // btnAdd.Click += BtnAdd_Click;
            // btnEdit.Click += BtnEdit_Click;
            // btnDelete.Click += BtnDelete_Click;
            // btnRefresh.Click += BtnRefresh_Click;
            // txtSearch.TextChanged += TxtSearch_TextChanged;
        }

        #endregion

        #region 数据加载方法

        /// <summary>
        /// 加载物料列表
        /// </summary>
        private void LoadMaterialList()
        {
            try
            {
                currentMaterials = materialBLL.GetAll();
                // TODO: L成员需要将数据绑定到DataGridView
                // dgvMaterials.DataSource = currentMaterials;

                LogManager.Info($"加载物料列表成功，共 {currentMaterials.Count} 条记录");
            }
            catch (Exception ex)
            {
                LogManager.Error("加载物料列表失败", ex);
                MessageBox.Show($"加载物料列表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载物料类别
        /// </summary>
        private void LoadCategories()
        {
            try
            {
                var categories = materialBLL.GetAllCategories();
                // TODO: L成员需要将类别绑定到ComboBox
                // cmbCategory.Items.Clear();
                // cmbCategory.Items.Add("全部");
                // foreach (var category in categories)
                // {
                //     cmbCategory.Items.Add(category);
                // }
                // cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogManager.Error("加载物料类别失败", ex);
            }
        }

        #endregion

        #region 事件处理方法（示例，L成员需要根据实际控件实现）

        /// <summary>
        /// 添加物料按钮点击事件
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: L成员需要创建物料编辑对话框
                // var dialog = new MaterialEditDialog();
                // if (dialog.ShowDialog() == DialogResult.OK)
                // {
                //     LoadMaterialList();
                // }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加物料失败", ex);
                MessageBox.Show($"添加物料失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 搜索文本变化事件
        /// </summary>
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // TODO: L成员实现搜索逻辑
                // var searchText = txtSearch.Text.Trim();
                // if (string.IsNullOrEmpty(searchText))
                // {
                //     dgvMaterials.DataSource = currentMaterials;
                // }
                // else
                // {
                //     var filtered = currentMaterials.Where(m =>
                //         m.MaterialName.Contains(searchText) ||
                //         m.MaterialCode.Contains(searchText)).ToList();
                //     dgvMaterials.DataSource = filtered;
                // }
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索物料失败", ex);
            }
        }

        #endregion
    }
}
