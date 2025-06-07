using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.BLL.Workshop;
using MES.Models.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.UI.Forms
{
    /// <summary>
    /// 车间管理窗体
    /// 提供车间信息的增删改查功能
    /// </summary>
    public partial class WorkshopManagementForm : Form
    {
        private readonly IWorkshopBLL _workshopBLL;
        private List<WorkshopInfo> _currentWorkshops;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkshopManagementForm()
        {
            InitializeComponent();
            _workshopBLL = new WorkshopBLL();
            _currentWorkshops = new List<WorkshopInfo>();
            
            InitializeForm();
            LoadWorkshops();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            this.Text = "车间管理";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 设置DataGridView列
            SetupDataGridView();
            
            // 绑定事件
            BindEvents();
        }

        /// <summary>
        /// 设置DataGridView列
        /// </summary>
        private void SetupDataGridView()
        {
            dgvWorkshops.AutoGenerateColumns = false;
            dgvWorkshops.AllowUserToAddRows = false;
            dgvWorkshops.AllowUserToDeleteRows = false;
            dgvWorkshops.ReadOnly = true;
            dgvWorkshops.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWorkshops.MultiSelect = false;

            // 添加列
            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopCode",
                HeaderText = "车间编码",
                DataPropertyName = "WorkshopCode",
                Width = 120
            });

            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopName",
                HeaderText = "车间名称",
                DataPropertyName = "WorkshopName",
                Width = 150
            });

            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "WorkshopType",
                HeaderText = "车间类型",
                DataPropertyName = "WorkshopType",
                Width = 100
            });

            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Department",
                HeaderText = "所属部门",
                DataPropertyName = "Department",
                Width = 120
            });

            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductionCapacity",
                HeaderText = "产能(件/天)",
                DataPropertyName = "ProductionCapacity",
                Width = 100
            });

            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "Status",
                Width = 80
            });

            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Location",
                HeaderText = "位置",
                DataPropertyName = "Location",
                Width = 100
            });

            dgvWorkshops.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CreateTime",
                HeaderText = "创建时间",
                DataPropertyName = "CreateTime",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd HH:mm:ss" }
            });
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnSearch.Click += BtnSearch_Click;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            dgvWorkshops.SelectionChanged += DgvWorkshops_SelectionChanged;
        }

        /// <summary>
        /// 加载车间数据
        /// </summary>
        private void LoadWorkshops()
        {
            try
            {
                _currentWorkshops = _workshopBLL.GetAllWorkshops();
                dgvWorkshops.DataSource = _currentWorkshops;
                
                lblTotal.Text = string.Format("共 {0} 条记录", _currentWorkshops.Count);
                
                // 更新按钮状态
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载车间数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载车间数据失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 搜索车间
        /// </summary>
        private void SearchWorkshops()
        {
            try
            {
                string keyword = txtSearch.Text.Trim();
                
                if (string.IsNullOrEmpty(keyword))
                {
                    LoadWorkshops();
                    return;
                }

                _currentWorkshops = _workshopBLL.SearchWorkshops(keyword);
                dgvWorkshops.DataSource = _currentWorkshops;
                
                lblTotal.Text = string.Format("共 {0} 条记录", _currentWorkshops.Count);
                
                // 更新按钮状态
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索车间失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("搜索车间失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新按钮状态
        /// </summary>
        private void UpdateButtonStates()
        {
            bool hasSelection = dgvWorkshops.SelectedRows.Count > 0;
            btnEdit.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        /// <summary>
        /// 获取选中的车间
        /// </summary>
        /// <returns>选中的车间信息</returns>
        private WorkshopInfo GetSelectedWorkshop()
        {
            if (dgvWorkshops.SelectedRows.Count == 0)
                return null;

            int selectedIndex = dgvWorkshops.SelectedRows[0].Index;
            if (selectedIndex >= 0 && selectedIndex < _currentWorkshops.Count)
            {
                return _currentWorkshops[selectedIndex];
            }

            return null;
        }

        #region 事件处理

        /// <summary>
        /// 添加车间
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new WorkshopEditForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadWorkshops();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加车间失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("添加车间失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑车间
        /// </summary>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedWorkshop = GetSelectedWorkshop();
                if (selectedWorkshop == null)
                {
                    MessageBox.Show("请选择要编辑的车间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var form = new WorkshopEditForm(selectedWorkshop))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadWorkshops();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("编辑车间失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("编辑车间失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除车间
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedWorkshop = GetSelectedWorkshop();
                if (selectedWorkshop == null)
                {
                    MessageBox.Show("请选择要删除的车间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要删除车间 '{0}' 吗？", selectedWorkshop.WorkshopName),
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    bool success = _workshopBLL.DeleteWorkshop(selectedWorkshop.Id);
                    if (success)
                    {
                        MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadWorkshops();
                    }
                    else
                    {
                        MessageBox.Show("删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除车间失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("删除车间失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadWorkshops();
        }

        /// <summary>
        /// 搜索按钮点击
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            SearchWorkshops();
        }

        /// <summary>
        /// 搜索框回车
        /// </summary>
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchWorkshops();
            }
        }

        /// <summary>
        /// 选择变化
        /// </summary>
        private void DgvWorkshops_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        #endregion
    }
}
