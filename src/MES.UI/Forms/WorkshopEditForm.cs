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
    /// 车间编辑窗体
    /// 提供车间信息的新增和编辑功能
    /// </summary>
    public partial class WorkshopEditForm : Form
    {
        private readonly IWorkshopBLL _workshopBLL;
        private WorkshopInfo _currentWorkshop;
        private bool _isEditMode;

        /// <summary>
        /// 构造函数 - 新增模式
        /// </summary>
        public WorkshopEditForm()
        {
            InitializeComponent();
            _workshopBLL = new WorkshopBLL();
            _isEditMode = false;
            _currentWorkshop = new WorkshopInfo();
            
            InitializeForm();
        }

        /// <summary>
        /// 构造函数 - 编辑模式
        /// </summary>
        /// <param name="workshop">要编辑的车间信息</param>
        public WorkshopEditForm(WorkshopInfo workshop)
        {
            InitializeComponent();
            _workshopBLL = new WorkshopBLL();
            _isEditMode = true;
            _currentWorkshop = workshop ?? throw new ArgumentNullException(nameof(workshop));
            
            InitializeForm();
            LoadWorkshopData();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            this.Text = _isEditMode ? "编辑车间" : "新增车间";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 初始化下拉框
            InitializeComboBoxes();
            
            // 绑定事件
            BindEvents();
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitializeComboBoxes()
        {
            // 车间类型
            cmbWorkshopType.Items.AddRange(new string[] 
            {
                "装配车间",
                "测试车间", 
                "包装车间",
                "机加工车间",
                "注塑车间",
                "喷涂车间",
                "其他"
            });

            // 部门
            cmbDepartment.Items.AddRange(new string[]
            {
                "生产部",
                "制造部",
                "工程部",
                "质量部",
                "其他"
            });

            // 状态
            chkStatus.Checked = true; // 默认启用
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        /// <summary>
        /// 加载车间数据
        /// </summary>
        private void LoadWorkshopData()
        {
            if (_currentWorkshop == null) return;

            txtWorkshopCode.Text = _currentWorkshop.WorkshopCode;
            txtWorkshopName.Text = _currentWorkshop.WorkshopName;
            cmbWorkshopType.Text = _currentWorkshop.WorkshopType;
            cmbDepartment.Text = _currentWorkshop.Department;
            txtCapacity.Text = _currentWorkshop.Capacity?.ToString();
            txtLocationId.Text = _currentWorkshop.LocationId;
            txtDescription.Text = _currentWorkshop.Description;
            chkStatus.Checked = _currentWorkshop.Status;

            // 编辑模式下车间编码不可修改
            if (_isEditMode)
            {
                txtWorkshopCode.ReadOnly = true;
                txtWorkshopCode.BackColor = SystemColors.Control;
            }
        }

        /// <summary>
        /// 验证输入数据
        /// </summary>
        /// <returns>验证结果</returns>
        private string ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtWorkshopCode.Text))
            {
                return "车间编码不能为空";
            }

            if (string.IsNullOrWhiteSpace(txtWorkshopName.Text))
            {
                return "车间名称不能为空";
            }

            if (!string.IsNullOrWhiteSpace(txtCapacity.Text))
            {
                if (!int.TryParse(txtCapacity.Text, out int capacity) || capacity <= 0)
                {
                    return "产能必须是大于0的整数";
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取窗体数据
        /// </summary>
        /// <returns>车间信息</returns>
        private WorkshopInfo GetFormData()
        {
            var workshop = _isEditMode ? _currentWorkshop : new WorkshopInfo();

            workshop.WorkshopCode = txtWorkshopCode.Text.Trim();
            workshop.WorkshopName = txtWorkshopName.Text.Trim();
            workshop.WorkshopType = cmbWorkshopType.Text.Trim();
            workshop.Department = cmbDepartment.Text.Trim();
            workshop.LocationId = txtLocationId.Text.Trim();
            workshop.Description = txtDescription.Text.Trim();
            workshop.Status = chkStatus.Checked;

            // 产能
            if (!string.IsNullOrWhiteSpace(txtCapacity.Text))
            {
                if (int.TryParse(txtCapacity.Text, out int capacity))
                {
                    workshop.Capacity = capacity;
                }
            }

            return workshop;
        }

        #region 事件处理

        /// <summary>
        /// 保存按钮点击
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                string validationResult = ValidateInput();
                if (!string.IsNullOrEmpty(validationResult))
                {
                    MessageBox.Show(validationResult, "输入验证", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 获取数据
                var workshop = GetFormData();

                // 保存数据
                bool success;
                if (_isEditMode)
                {
                    success = _workshopBLL.UpdateWorkshop(workshop);
                }
                else
                {
                    success = _workshopBLL.AddWorkshop(workshop);
                }

                if (success)
                {
                    MessageBox.Show(_isEditMode ? "更新成功" : "添加成功", "提示", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(_isEditMode ? "更新失败" : "添加失败", "错误", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error($"保存车间信息失败：{ex.Message}", ex);
                MessageBox.Show($"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消按钮点击
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
