using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.BLL.Material;
using MES.BLL.WorkOrder;
using MES.UI.Forms.Common;
using MES.Common.Logging;

namespace MES.UI.Forms.WorkOrder
{
    /// <summary>
    /// 创建工单窗体 - 现代化UI设计
    /// 功能：创建新的生产工单，包含完整的BOM物料清单管理
    /// </summary>
    public partial class CreateWorkOrder : Form
    {
        #region 私有字段

        private WorkOrderBLL workOrderBLL;
        private BOMBLL bomBLL;
        private ProductBLL productBLL;
        private DataTable bomDataTable;
        private string currentBOMCode;
        private string currentBOMVersion;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化创建工单窗体
        /// </summary>
        public CreateWorkOrder()
        {
            InitializeComponent();
            InitializeBusinessLogic();
            InitializeUI();
            LoadInitialData();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化业务逻辑层
        /// </summary>
        private void InitializeBusinessLogic()
        {
            try
            {
                workOrderBLL = new WorkOrderBLL();
                bomBLL = new BOMBLL();
                productBLL = new ProductBLL();
                bomDataTable = new DataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("初始化业务逻辑失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化UI界面
        /// </summary>
        private void InitializeUI()
        {
            try
            {
                // 设置窗体图标和标题
                this.Icon = SystemIcons.Application;

                // 设置默认值
                dtpPlanStartDate.Value = DateTime.Now;
                dtpPlanEndDate.Value = DateTime.Now.AddDays(7);
                txtCreatedBy.Text = Environment.UserName;

                // 绑定事件
                BindEvents();

                // 初始化BOM数据网格
                InitializeBOMDataGrid();

                // 设置控件状态
                SetControlStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("初始化界面失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 绑定控件事件
        /// </summary>
        private void BindEvents()
        {
            // 按钮事件
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSelectBOM.Click += BtnSelectBOM_Click;
            btnAddBOM.Click += BtnAddBOM_Click;
            btnRemoveBOM.Click += BtnRemoveBOM_Click;
            btnRefreshBOM.Click += BtnRefreshBOM_Click;

            // 下拉框事件
            cmbProductCode.SelectedIndexChanged += CmbProductCode_SelectedIndexChanged;
            cmbWorkOrderType.SelectedIndexChanged += CmbWorkOrderType_SelectedIndexChanged;

            // 文本框事件
            txtPlanQuantity.KeyPress += TxtPlanQuantity_KeyPress;

            // 日期选择器事件
            dtpPlanStartDate.ValueChanged += DtpPlanStartDate_ValueChanged;
        }

        /// <summary>
        /// 初始化BOM数据网格
        /// </summary>
        private void InitializeBOMDataGrid()
        {
            try
            {
                // 创建数据表结构
                bomDataTable.Columns.Add("物料编号", typeof(string));
                bomDataTable.Columns.Add("物料名称", typeof(string));
                bomDataTable.Columns.Add("规格型号", typeof(string));
                bomDataTable.Columns.Add("需求数量", typeof(decimal));
                bomDataTable.Columns.Add("单位", typeof(string));
                bomDataTable.Columns.Add("库存数量", typeof(decimal));
                bomDataTable.Columns.Add("状态", typeof(string));

                // 绑定数据源
                dgvBOMList.DataSource = bomDataTable;

                // 设置列宽
                dgvBOMList.Columns["物料编号"].Width = 100;
                dgvBOMList.Columns["物料名称"].Width = 150;
                dgvBOMList.Columns["规格型号"].Width = 120;
                dgvBOMList.Columns["需求数量"].Width = 80;
                dgvBOMList.Columns["单位"].Width = 60;
                dgvBOMList.Columns["库存数量"].Width = 80;
                dgvBOMList.Columns["状态"].Width = 80;

                // 设置列样式
                dgvBOMList.Columns["需求数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvBOMList.Columns["库存数量"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvBOMList.Columns["状态"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("初始化BOM数据网格失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        private void SetControlStates()
        {
            // 设置只读控件
            txtBOMCode.ReadOnly = true;
            txtBOMVersion.ReadOnly = true;
            txtCreatedBy.ReadOnly = true;

            // 设置按钮状态
            btnRemoveBOM.Enabled = false;
            btnRefreshBOM.Enabled = false;
        }

        /// <summary>
        /// 加载初始数据
        /// </summary>
        private void LoadInitialData()
        {
            try
            {
                LoadProductCodes();
                LoadFinishedWorkOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载初始数据失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载产品编号列表
        /// </summary>
        private void LoadProductCodes()
        {
            try
            {
                if (productBLL != null)
                {
                    DataTable productTable = productBLL.GetAllProducts();
                    cmbProductCode.DataSource = productTable;
                    cmbProductCode.DisplayMember = "ProductCode";
                    cmbProductCode.ValueMember = "ProductCode";
                    cmbProductCode.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载产品编号失败：{0}", ex.Message),
                    "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 加载成品工单号列表
        /// </summary>
        private void LoadFinishedWorkOrders()
        {
            try
            {
                if (workOrderBLL != null)
                {
                    DataTable workOrderTable = workOrderBLL.GetFinishedWorkOrders();
                    cmbFinishedWorkOrder.DataSource = workOrderTable;
                    cmbFinishedWorkOrder.DisplayMember = "WorkOrderNo";
                    cmbFinishedWorkOrder.ValueMember = "WorkOrderNo";
                    cmbFinishedWorkOrder.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载成品工单号失败：{0}", ex.Message),
                    "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region 事件处理方法

        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (SaveWorkOrder())
                    {
                        MessageBox.Show("工单创建成功！", "操作成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存工单失败：{0}", ex.Message),
                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要取消创建工单吗？", "确认取消",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        /// <summary>
        /// 选择BOM按钮点击事件
        /// </summary>
        private void BtnSelectBOM_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开BOM选择窗体
                SelectBOMDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("选择BOM失败：{0}", ex.Message),
                    "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加BOM按钮点击事件
        /// </summary>
        private void BtnAddBOM_Click(object sender, EventArgs e)
        {
            try
            {
                // 这里应该打开物料选择窗体
                AddBOMItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("添加BOM项失败：{0}", ex.Message),
                    "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 移除BOM按钮点击事件
        /// </summary>
        private void BtnRemoveBOM_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBOMList.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("确定要移除选中的BOM项吗？", "确认移除",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dgvBOMList.SelectedRows)
                        {
                            if (!row.IsNewRow)
                            {
                                dgvBOMList.Rows.Remove(row);
                            }
                        }
                        UpdateBOMButtonStates();
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要移除的BOM项。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("移除BOM项失败：{0}", ex.Message),
                    "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新BOM按钮点击事件
        /// </summary>
        private void BtnRefreshBOM_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshBOMList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("刷新BOM列表失败：{0}", ex.Message),
                    "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 产品编号选择变更事件
        /// </summary>
        private void CmbProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProductCode.SelectedValue != null)
                {
                    string productCode = cmbProductCode.SelectedValue.ToString();
                    LoadProductInfo(productCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载产品信息失败：{0}", ex.Message),
                    "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 工单类型选择变更事件
        /// </summary>
        private void CmbWorkOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 根据工单类型调整界面显示
                UpdateUIByWorkOrderType();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("更新界面失败：{0}", ex.Message),
                    "界面更新错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 计划数量文本框按键事件（只允许输入数字）
        /// </summary>
        private void TxtPlanQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 只允许输入数字、小数点和退格键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // 只允许一个小数点
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 计划开始日期变更事件
        /// </summary>
        private void DtpPlanStartDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // 确保结束日期不早于开始日期
                if (dtpPlanEndDate.Value < dtpPlanStartDate.Value)
                {
                    dtpPlanEndDate.Value = dtpPlanStartDate.Value.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("日期验证失败：{0}", ex.Message),
                    "日期错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region 业务方法

        /// <summary>
        /// 验证输入数据
        /// </summary>
        private bool ValidateInput()
        {
            try
            {
                // 验证工单类型
                if (cmbWorkOrderType.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择工单类型。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbWorkOrderType.Focus();
                    return false;
                }

                // 验证工单说明
                if (string.IsNullOrWhiteSpace(txtWorkOrderDesc.Text))
                {
                    MessageBox.Show("请输入工单说明。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtWorkOrderDesc.Focus();
                    return false;
                }

                // 验证产品编号
                if (cmbProductCode.SelectedIndex == -1)
                {
                    MessageBox.Show("请选择产品编号。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbProductCode.Focus();
                    return false;
                }

                // 验证计划数量
                decimal planQuantity;
                if (!decimal.TryParse(txtPlanQuantity.Text, out planQuantity) || planQuantity <= 0)
                {
                    MessageBox.Show("请输入有效的计划数量。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPlanQuantity.Focus();
                    return false;
                }

                // 验证单位
                if (string.IsNullOrWhiteSpace(txtUnit.Text))
                {
                    MessageBox.Show("请输入单位。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUnit.Focus();
                    return false;
                }

                // 验证日期
                if (dtpPlanEndDate.Value < dtpPlanStartDate.Value)
                {
                    MessageBox.Show("计划结束日期不能早于开始日期。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpPlanEndDate.Focus();
                    return false;
                }

                // 验证BOM列表
                if (bomDataTable.Rows.Count == 0)
                {
                    MessageBox.Show("请至少添加一个BOM物料项。", "验证失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSelectBOM.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("验证输入数据时发生错误：{0}", ex.Message),
                    "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 保存工单
        /// </summary>
        private bool SaveWorkOrder()
        {
            try
            {
                // 创建工单对象
                WorkOrderModel workOrder = new WorkOrderModel
                {
                    WorkOrderNo = GenerateWorkOrderNo(),
                    WorkOrderType = cmbWorkOrderType.Text,
                    WorkOrderDesc = txtWorkOrderDesc.Text.Trim(),
                    ProductCode = cmbProductCode.SelectedValue.ToString(),
                    FinishedWorkOrderNo = cmbFinishedWorkOrder.SelectedValue != null ? cmbFinishedWorkOrder.SelectedValue.ToString() : null,
                    BOMCode = currentBOMCode,
                    BOMVersion = currentBOMVersion,
                    PlanStartDate = dtpPlanStartDate.Value,
                    PlanEndDate = dtpPlanEndDate.Value,
                    PlanQuantity = decimal.Parse(txtPlanQuantity.Text),
                    Unit = txtUnit.Text.Trim(),
                    ProductType = txtProductType.Text.Trim(),
                    Remarks = txtRemarks.Text.Trim(),
                    CreatedBy = txtCreatedBy.Text.Trim(),
                    CreatedDate = DateTime.Now,
                    Status = "待生产"
                };

                // 保存工单
                bool result = workOrderBLL.CreateWorkOrder(workOrder);

                if (result)
                {
                    // 保存BOM明细
                    SaveBOMDetails(workOrder.WorkOrderNo);
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存工单时发生错误：{0}", ex.Message),
                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 生成工单号
        /// </summary>
        private string GenerateWorkOrderNo()
        {
            try
            {
                // 生成格式：WO + 年月日 + 4位序号
                string dateStr = DateTime.Now.ToString("yyyyMMdd");
                string prefix = string.Format("WO{0}", dateStr);

                // 获取当日最大序号
                int maxSeq = workOrderBLL.GetMaxSequenceByDate(DateTime.Now);
                string seqStr = (maxSeq + 1).ToString("D4");

                return string.Format("{0}{1}", prefix, seqStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("生成工单号失败：{0}", ex.Message),
                    "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Format("WO{0}0001", DateTime.Now.ToString("yyyyMMdd"));
            }
        }

        /// <summary>
        /// 保存BOM明细
        /// </summary>
        private void SaveBOMDetails(string workOrderNo)
        {
            try
            {
                foreach (DataRow row in bomDataTable.Rows)
                {
                    WorkOrderBOMModel bomDetail = new WorkOrderBOMModel
                    {
                        WorkOrderNo = workOrderNo,
                        MaterialCode = row["物料编号"].ToString(),
                        MaterialName = row["物料名称"].ToString(),
                        Specification = row["规格型号"].ToString(),
                        RequiredQuantity = Convert.ToDecimal(row["需求数量"]),
                        Unit = row["单位"].ToString(),
                        StockQuantity = Convert.ToDecimal(row["库存数量"]),
                        Status = row["状态"].ToString()
                    };

                    workOrderBLL.CreateWorkOrderBOM(bomDetail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存BOM明细失败：{0}", ex.Message),
                    "保存错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 加载产品信息
        /// </summary>
        private void LoadProductInfo(string productCode)
        {
            try
            {
                if (productBLL != null)
                {
                    ProductModel product = productBLL.GetProductByCode(productCode);
                    if (product != null)
                    {
                        txtProductType.Text = product.ProductType;
                        txtUnit.Text = product.Unit;

                        // 自动加载默认BOM
                        LoadDefaultBOM(productCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载产品信息失败：{0}", ex.Message),
                    "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 加载默认BOM
        /// </summary>
        private void LoadDefaultBOM(string productCode)
        {
            try
            {
                if (bomBLL != null)
                {
                    BOMModel defaultBOM = bomBLL.GetDefaultBOMByProduct(productCode);
                    if (defaultBOM != null)
                    {
                        currentBOMCode = defaultBOM.BOMCode;
                        currentBOMVersion = defaultBOM.Version;
                        txtBOMCode.Text = currentBOMCode;
                        txtBOMVersion.Text = currentBOMVersion;

                        // 加载BOM明细
                        LoadBOMDetails(currentBOMCode, currentBOMVersion);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载默认BOM失败：{0}", ex.Message),
                    "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 加载BOM明细
        /// </summary>
        private void LoadBOMDetails(string bomCode, string bomVersion)
        {
            try
            {
                if (bomBLL != null)
                {
                    DataTable bomDetails = bomBLL.GetBOMDetails(bomCode, bomVersion);

                    // 清空现有数据
                    bomDataTable.Clear();

                    // 添加BOM明细数据
                    foreach (DataRow sourceRow in bomDetails.Rows)
                    {
                        DataRow newRow = bomDataTable.NewRow();
                        newRow["物料编号"] = sourceRow["MaterialCode"];
                        newRow["物料名称"] = sourceRow["MaterialName"];
                        newRow["规格型号"] = sourceRow["Specification"];
                        newRow["需求数量"] = sourceRow["RequiredQuantity"];
                        newRow["单位"] = sourceRow["Unit"];
                        newRow["库存数量"] = GetStockQuantity(sourceRow["MaterialCode"].ToString());
                        newRow["状态"] = GetMaterialStatus(sourceRow["MaterialCode"].ToString());
                        bomDataTable.Rows.Add(newRow);
                    }

                    UpdateBOMButtonStates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("加载BOM明细失败：{0}", ex.Message),
                    "数据加载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 获取物料库存数量
        /// </summary>
        private decimal GetStockQuantity(string materialCode)
        {
            try
            {
                // 调用库存管理的BLL获取实际库存
                var materialBLL = new MaterialBLL();
                var material = materialBLL.GetMaterialByCode(materialCode);
                if (material != null)
                {
                    return material.StockQuantity.HasValue ? material.StockQuantity.Value : 0;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取物料库存失败，物料编码：{0}", materialCode), ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取物料状态
        /// </summary>
        private string GetMaterialStatus(string materialCode)
        {
            try
            {
                decimal stockQty = GetStockQuantity(materialCode);
                if (stockQty > 100)
                    return "充足";
                else if (stockQty > 0)
                    return "不足";
                else
                    return "缺料";
            }
            catch
            {
                return "未知";
            }
        }

        /// <summary>
        /// 更新BOM按钮状态
        /// </summary>
        private void UpdateBOMButtonStates()
        {
            btnRemoveBOM.Enabled = dgvBOMList.Rows.Count > 0;
            btnRefreshBOM.Enabled = !string.IsNullOrEmpty(currentBOMCode);
        }

        /// <summary>
        /// 根据工单类型更新界面
        /// </summary>
        private void UpdateUIByWorkOrderType()
        {
            try
            {
                string workOrderType = cmbWorkOrderType.Text;

                // 根据不同工单类型调整界面显示
                switch (workOrderType)
                {
                    case "生产工单":
                        lblFinishedWorkOrder.Visible = false;
                        cmbFinishedWorkOrder.Visible = false;
                        break;
                    case "返工工单":
                        lblFinishedWorkOrder.Visible = true;
                        cmbFinishedWorkOrder.Visible = true;
                        break;
                    default:
                        lblFinishedWorkOrder.Visible = false;
                        cmbFinishedWorkOrder.Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("更新界面失败：{0}", ex.Message),
                    "界面更新错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 选择BOM对话框
        /// </summary>
        private void SelectBOMDialog()
        {
            try
            {
                // 打开BOM选择窗体
                using (var form = new BOMSelectForm())
                {
                    if (form.ShowDialog() == DialogResult.OK && form.SelectedBOM != null)
                    {
                        currentBOMCode = form.SelectedBOM.BOMCode;
                        currentBOMVersion = form.SelectedBOM.Version.ToString();
                        txtBOMCode.Text = currentBOMCode;
                        txtBOMVersion.Text = currentBOMVersion;

                        // 加载真实的BOM明细数据
                        LoadBOMDetails(form.SelectedBOM.BOMCode, form.SelectedBOM.Version.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("选择BOM失败：{0}", ex.Message),
                    "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加BOM项
        /// </summary>
        private void AddBOMItem()
        {
            try
            {
                // 打开物料选择窗体
                using (var form = new MaterialSelectForm())
                {
                    if (form.ShowDialog() == DialogResult.OK && form.SelectedMaterial != null)
                    {
                        var material = form.SelectedMaterial;

                        // 检查是否已存在该物料
                        bool exists = false;
                        foreach (DataRow row in bomDataTable.Rows)
                        {
                            if (row["物料编号"].ToString() == material.MaterialCode)
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (exists)
                        {
                            MessageBox.Show("该物料已存在于BOM中！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // 添加新的BOM项
                        DataRow newRow = bomDataTable.NewRow();
                        newRow["物料编号"] = material.MaterialCode;
                        newRow["物料名称"] = material.MaterialName;
                        newRow["规格型号"] = material.Specification;
                        newRow["需求数量"] = 1; // 默认数量，用户可以修改
                        newRow["单位"] = material.Unit;
                        newRow["库存数量"] = GetStockQuantity(material.MaterialCode).ToString();
                        newRow["状态"] = GetMaterialStatus(material.MaterialCode);
                        bomDataTable.Rows.Add(newRow);

                        UpdateBOMButtonStates();
                        MessageBox.Show("BOM项添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("添加BOM项失败：{0}", ex.Message),
                    "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新BOM列表
        /// </summary>
        private void RefreshBOMList()
        {
            try
            {
                if (!string.IsNullOrEmpty(currentBOMCode))
                {
                    LoadBOMDetails(currentBOMCode, currentBOMVersion);
                    MessageBox.Show("BOM列表已刷新。", "操作成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("刷新BOM列表失败：{0}", ex.Message),
                    "操作错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #endregion
    }
}
