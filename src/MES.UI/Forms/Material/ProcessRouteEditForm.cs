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
using MES.Models.Production;
using MES.Common.Logging;

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
        private readonly IProcessRouteBLL _processRouteBLL; // 关键：依赖业务逻辑层接口
        private readonly IProductBLL _productBLL; // 引入产品BLL接口
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
            _processRouteBLL = new ProcessRouteBLL(); // 关键：实例化BLL
            _productBLL = new ProductBLL();

            // 在窗体加载时应用真实LOL主题
            this.Load += (sender, e) =>
            {
                ApplyLeagueTheme();
            };

            InitializeForm();
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
        /// 初始化产品下拉框(已修复：从真实数据源加载)
        /// </summary>
        private void InitializeProductComboBox()
        {
            cmbProduct.Items.Clear();
            try
            {
                // 通过BLL获取所有产品信息
                List<ProductionInfo> products = _productBLL.GetAllProducts(); // 使用注入的BLL实例

                // 设置数据源
                cmbProduct.DataSource = products;
                cmbProduct.DisplayMember = "ProductName"; // 显示产品名称
                cmbProduct.ValueMember = "ProductId";   // 产品ID作为值
                cmbProduct.SelectedIndex = -1; // 默认不选择任何项
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载产品列表失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    // 编辑模式：调用BLL的更新方法
                    processRoute.Id = _processRoute.Id;
                    result = _processRouteBLL.UpdateProcessRoute(processRoute);
                }
                else
                {
                    // 新增模式：调用BLL的新增方法
                    result = _processRouteBLL.AddProcessRoute(processRoute);
                }

                if (result)
                {
                    IsSuccess = true;
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // 设置对话框结果，通知父窗体刷新
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // ★★★ 核心修正：如果是在编辑模式下，必须继承原有的工艺步骤列表 ★★★
            if (_isEditMode && _processRoute != null)
            {
                processRoute.Steps = _processRoute.Steps;
            }

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