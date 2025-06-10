using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Models.Material;
using MES.BLL.Material;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 工艺路线编辑窗体
    /// 用于新增和编辑工艺路线信息
    /// </summary>
    public partial class ProcessRouteEditForm : Form
    {
        #region 私有字段

        private ProcessRoute _processRoute;
        private readonly IProcessRouteBLL _processRouteBLL;
        private bool _isEditMode;

        #endregion

        #region 公共属性

        /// <summary>
        /// 编辑的工艺路线
        /// </summary>
        public ProcessRoute ProcessRoute
        {
            get { return _processRoute; }
            set 
            { 
                _processRoute = value;
                _isEditMode = value != null && value.Id > 0;
                LoadData();
            }
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化工艺路线编辑窗体
        /// </summary>
        public ProcessRouteEditForm()
        {
            InitializeComponent();
            _processRouteBLL = new ProcessRouteBLL();
            InitializeForm();
        }

        /// <summary>
        /// 初始化工艺路线编辑窗体（编辑模式）
        /// </summary>
        /// <param name="processRoute">要编辑的工艺路线</param>
        public ProcessRouteEditForm(ProcessRoute processRoute) : this()
        {
            ProcessRoute = processRoute;
        }

        #endregion

        #region 窗体初始化

        /// <summary>
        /// 初始化窗体设置
        /// </summary>
        private void InitializeForm()
        {
            // 设置窗体属性
            this.BackColor = Color.White;
            
            // 初始化控件
            InitializeControls();
            
            // 绑定事件
            BindEvents();
            
            // 设置默认值
            SetDefaultValues();
        }

        /// <summary>
        /// 初始化控件设置
        /// </summary>
        private void InitializeControls()
        {
            // 初始化产品下拉框
            InitializeProductComboBox();
            
            // 初始化状态下拉框
            InitializeStatusComboBox();
        }

        /// <summary>
        /// 初始化产品下拉框
        /// </summary>
        private void InitializeProductComboBox()
        {
            cmbProduct.Items.Clear();
            
            // 模拟产品数据（实际项目中应该从数据库加载）
            var products = new List<ProductInfo>
            {
                new ProductInfo { Id = 1, Name = "智能手机主板" },
                new ProductInfo { Id = 2, Name = "锂电池组" },
                new ProductInfo { Id = 3, Name = "显示屏模组" },
                new ProductInfo { Id = 4, Name = "摄像头模组" },
                new ProductInfo { Id = 5, Name = "充电器" }
            };
            
            cmbProduct.DisplayMember = "Name";
            cmbProduct.ValueMember = "Id";
            cmbProduct.DataSource = products;
            cmbProduct.SelectedIndex = -1;
        }

        /// <summary>
        /// 初始化状态下拉框
        /// </summary>
        private void InitializeStatusComboBox()
        {
            cmbStatus.Items.Clear();
            
            var statusItems = new List<StatusItem>
            {
                new StatusItem { Value = ProcessRouteStatus.Draft, Text = "草稿" },
                new StatusItem { Value = ProcessRouteStatus.Active, Text = "启用" },
                new StatusItem { Value = ProcessRouteStatus.Inactive, Text = "停用" }
            };
            
            cmbStatus.DisplayMember = "Text";
            cmbStatus.ValueMember = "Value";
            cmbStatus.DataSource = statusItems;
            cmbStatus.SelectedIndex = 0; // 默认选择草稿
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        private void SetDefaultValues()
        {
            if (!_isEditMode)
            {
                txtVersion.Text = "V1.0";
                cmbStatus.SelectedValue = ProcessRouteStatus.Draft;
            }
        }

        #endregion

        #region 事件绑定

        /// <summary>
        /// 绑定控件事件
        /// </summary>
        private void BindEvents()
        {
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            
            // 表单验证事件
            txtRouteCode.Leave += TxtRouteCode_Leave;
            txtRouteName.Leave += TxtRouteName_Leave;
        }

        #endregion

        #region 数据加载

        /// <summary>
        /// 加载数据到控件
        /// </summary>
        private void LoadData()
        {
            if (_processRoute == null) return;

            try
            {
                // 更新标题
                lblTitle.Text = _isEditMode ? "⚙️ 编辑工艺路线" : "⚙️ 新增工艺路线";
                this.Text = _isEditMode ? "编辑工艺路线" : "新增工艺路线";

                if (_isEditMode)
                {
                    // 加载工艺路线数据
                    txtRouteCode.Text = _processRoute.RouteCode;
                    txtRouteName.Text = _processRoute.RouteName;
                    txtVersion.Text = _processRoute.Version;
                    txtDescription.Text = _processRoute.Description;
                    
                    // 设置产品选择
                    cmbProduct.SelectedValue = _processRoute.ProductId;
                    
                    // 设置状态选择
                    cmbStatus.SelectedValue = _processRoute.Status;
                    
                    // 编辑模式下编码不可修改
                    txtRouteCode.ReadOnly = true;
                    txtRouteCode.BackColor = Color.FromArgb(248, 249, 250);
                }
                else
                {
                    // 新增模式，清空所有控件
                    ClearControls();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        private void ClearControls()
        {
            txtRouteCode.Text = "";
            txtRouteName.Text = "";
            txtDescription.Text = "";
            cmbProduct.SelectedIndex = -1;
            
            txtRouteCode.ReadOnly = false;
            txtRouteCode.BackColor = Color.White;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                {
                    return;
                }

                var processRoute = GetProcessRouteFromControls();
                bool result;

                if (_isEditMode)
                {
                    processRoute.Id = _processRoute.Id;
                    result = _processRouteBLL.UpdateProcessRoute(processRoute);
                }
                else
                {
                    result = _processRouteBLL.AddProcessRoute(processRoute);
                }

                if (result)
                {
                    _processRoute = processRoute;
                    IsSuccess = true;
                    MessageBox.Show("保存成功", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 路线编码失去焦点事件
        /// </summary>
        private void TxtRouteCode_Leave(object sender, EventArgs e)
        {
            if (!_isEditMode && !string.IsNullOrWhiteSpace(txtRouteCode.Text))
            {
                // 检查编码是否重复
                CheckRouteCodeUniqueness();
            }
        }

        /// <summary>
        /// 检查工艺路线编码唯一性
        /// </summary>
        private void CheckRouteCodeUniqueness()
        {
            try
            {
                string routeCode = txtRouteCode.Text.Trim();
                if (string.IsNullOrEmpty(routeCode))
                {
                    return;
                }

                // 使用BLL检查编码唯一性
                bool isUnique = _processRouteBLL.IsRouteCodeUnique(routeCode, _isEditMode ? _processRoute.Id : 0);

                if (!isUnique)
                {
                    MessageBox.Show(string.Format("工艺路线编码 '{0}' 已存在，请使用其他编码", routeCode),
                        "编码重复", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // 清空编码并聚焦
                    txtRouteCode.Text = "";
                    txtRouteCode.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("检查编码唯一性失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 路线名称失去焦点事件
        /// </summary>
        private void TxtRouteName_Leave(object sender, EventArgs e)
        {
            // 可以在这里添加名称验证逻辑
        }

        #endregion

        #region 数据验证

        /// <summary>
        /// 验证输入数据
        /// </summary>
        /// <returns>验证结果</returns>
        private bool ValidateInput()
        {
            // 验证路线编码
            if (string.IsNullOrWhiteSpace(txtRouteCode.Text))
            {
                MessageBox.Show("请输入工艺路线编码", "验证失败", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRouteCode.Focus();
                return false;
            }

            // 验证路线名称
            if (string.IsNullOrWhiteSpace(txtRouteName.Text))
            {
                MessageBox.Show("请输入工艺路线名称", "验证失败", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRouteName.Focus();
                return false;
            }

            // 验证产品选择
            if (cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("请选择产品", "验证失败", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProduct.Focus();
                return false;
            }

            // 验证版本号
            if (string.IsNullOrWhiteSpace(txtVersion.Text))
            {
                MessageBox.Show("请输入版本号", "验证失败", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtVersion.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 从控件获取工艺路线对象
        /// </summary>
        /// <returns>工艺路线对象</returns>
        private ProcessRoute GetProcessRouteFromControls()
        {
            var processRoute = new ProcessRoute
            {
                RouteCode = txtRouteCode.Text.Trim(),
                RouteName = txtRouteName.Text.Trim(),
                ProductId = (int)cmbProduct.SelectedValue,
                ProductName = cmbProduct.Text,
                Version = txtVersion.Text.Trim(),
                Status = (ProcessRouteStatus)cmbStatus.SelectedValue,
                Description = txtDescription.Text.Trim()
            };

            return processRoute;
        }

        #endregion
    }

    #region 辅助类

    /// <summary>
    /// 产品信息类
    /// </summary>
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 状态项类
    /// </summary>
    public class StatusItem
    {
        public ProcessRouteStatus Value { get; set; }
        public string Text { get; set; }
    }

    #endregion
}
