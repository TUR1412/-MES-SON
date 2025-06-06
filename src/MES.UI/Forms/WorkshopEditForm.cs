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
            // 车间类型 - 使用数字编码
            cmbWorkshopType.Items.AddRange(new object[]
            {
                new { Text = "生产车间", Value = 1 },
                new { Text = "装配车间", Value = 2 },
                new { Text = "包装车间", Value = 3 },
                new { Text = "质检车间", Value = 4 },
                new { Text = "仓储车间", Value = 5 }
            });
            cmbWorkshopType.DisplayMember = "Text";
            cmbWorkshopType.ValueMember = "Value";

            // 部门
            cmbDepartment.Items.AddRange(new string[]
            {
                "生产部",
                "制造部",
                "工程部",
                "质量部",
                "其他"
            });

            // 状态 - 使用下拉框而不是复选框
            cmbStatus.Items.AddRange(new object[]
            {
                new { Text = "正常运行", Value = "1" },
                new { Text = "停用", Value = "0" },
                new { Text = "维护中", Value = "2" },
                new { Text = "故障停机", Value = "3" }
            });
            cmbStatus.DisplayMember = "Text";
            cmbStatus.ValueMember = "Value";
            cmbStatus.SelectedIndex = 0; // 默认正常运行
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

            // 设置车间类型
            for (int i = 0; i < cmbWorkshopType.Items.Count; i++)
            {
                var item = (dynamic)cmbWorkshopType.Items[i];
                if (item.Value == _currentWorkshop.WorkshopType)
                {
                    cmbWorkshopType.SelectedIndex = i;
                    break;
                }
            }

            cmbDepartment.Text = _currentWorkshop.Department;
            txtManager.Text = _currentWorkshop.Manager;
            txtPhone.Text = _currentWorkshop.Phone;
            txtLocation.Text = _currentWorkshop.Location;
            txtArea.Text = _currentWorkshop.Area.ToString();
            txtProductionCapacity.Text = _currentWorkshop.ProductionCapacity.ToString();
            txtEmployeeCount.Text = _currentWorkshop.EmployeeCount.ToString();
            txtDescription.Text = _currentWorkshop.Description;

            // 设置状态
            for (int i = 0; i < cmbStatus.Items.Count; i++)
            {
                var item = (dynamic)cmbStatus.Items[i];
                if (item.Value == _currentWorkshop.Status)
                {
                    cmbStatus.SelectedIndex = i;
                    break;
                }
            }

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

            if (!string.IsNullOrWhiteSpace(txtProductionCapacity.Text))
            {
                if (!int.TryParse(txtProductionCapacity.Text, out int capacity) || capacity <= 0)
                {
                    return "产能必须是大于0的整数";
                }
            }

            if (!string.IsNullOrWhiteSpace(txtArea.Text))
            {
                if (!decimal.TryParse(txtArea.Text, out decimal area) || area <= 0)
                {
                    return "车间面积必须是大于0的数字";
                }
            }

            if (!string.IsNullOrWhiteSpace(txtEmployeeCount.Text))
            {
                if (!int.TryParse(txtEmployeeCount.Text, out int count) || count < 0)
                {
                    return "员工数量必须是非负整数";
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

            // 车间类型
            if (cmbWorkshopType.SelectedItem != null)
            {
                workshop.WorkshopType = ((dynamic)cmbWorkshopType.SelectedItem).Value;
            }

            workshop.Department = cmbDepartment.Text.Trim();
            workshop.Manager = txtManager.Text.Trim();
            workshop.Phone = txtPhone.Text.Trim();
            workshop.Location = txtLocation.Text.Trim();
            workshop.Description = txtDescription.Text.Trim();

            // 状态
            if (cmbStatus.SelectedItem != null)
            {
                workshop.Status = ((dynamic)cmbStatus.SelectedItem).Value;
            }

            // 面积
            if (!string.IsNullOrWhiteSpace(txtArea.Text))
            {
                if (decimal.TryParse(txtArea.Text, out decimal area))
                {
                    workshop.Area = area;
                }
            }

            // 产能
            if (!string.IsNullOrWhiteSpace(txtProductionCapacity.Text))
            {
                if (int.TryParse(txtProductionCapacity.Text, out int capacity))
                {
                    workshop.ProductionCapacity = capacity;
                }
            }

            // 员工数量
            if (!string.IsNullOrWhiteSpace(txtEmployeeCount.Text))
            {
                if (int.TryParse(txtEmployeeCount.Text, out int count))
                {
                    workshop.EmployeeCount = count;
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
