using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MES.BLL.Workshop;
using MES.BLL.WorkOrder;
using MES.Models.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Batch
{
    /// <summary>
    /// 取消批次窗体 - 现代化UI设计
    /// 功能：取消已创建的生产批次，包含取消原因记录
    /// </summary>
    public partial class CancelBatch : ThemedForm
    {
        #region 私有字段

        private BatchBLL batchBLL;
        private WorkOrderBLL workOrderBLL;
        private DataTable batchDataTable;
        private string selectedBatchNo;
        private string _preselectBatchNo;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化取消批次窗体
        /// </summary>
        public CancelBatch()
        {
            InitializeComponent();
            InitializeBusinessLogic();
            InitializeUI();
            LoadBatchData();
            // 主题应用放到 Shown：避免初始化过程/数据绑定过程把样式刷回系统默认
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
        }

        #endregion

        /// <summary>
        /// 预选批次（用于从“批次管理”直接对某条批次发起取消操作）
        /// </summary>
        public void PreselectBatch(string batchNo)
        {
            _preselectBatchNo = batchNo;
            TrySelectBatchRow(batchNo);
        }

        #region 初始化方法

        /// <summary>
        /// 初始化业务逻辑层
        /// </summary>
        private void InitializeBusinessLogic()
        {
            try
            {
                batchBLL = new BatchBLL();
                workOrderBLL = new WorkOrderBLL();
                LogManager.Info("取消批次窗体业务逻辑初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("取消批次窗体业务逻辑初始化失败", ex);
                throw new MESException("业务逻辑初始化失败", ex);
            }
        }

        /// <summary>
        /// 初始化UI界面
        /// </summary>
        private void InitializeUI()
        {
            try
            {
                // 设置窗体属性
                this.Text = "取消批次 - MES系统";
                this.StartPosition = FormStartPosition.CenterParent;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;

                // 初始化控件状态
                btnCancel.Enabled = false;
                txtCancelReason.MaxLength = 500;

                // 设置数据网格样式
                SetupDataGridView();

                LogManager.Info("取消批次窗体UI初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("取消批次窗体UI初始化失败", ex);
                throw new MESException("UI初始化失败", ex);
            }
        }

        /// <summary>
        /// 设置数据网格样式
        /// </summary>
        private void SetupDataGridView()
        {
            try
            {
                // 基本样式设置
                dgvBatches.BackgroundColor = LeagueColors.DarkSurface;
                dgvBatches.BorderStyle = BorderStyle.None;
                dgvBatches.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvBatches.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgvBatches.RowHeadersVisible = false;
                dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvBatches.MultiSelect = false;
                dgvBatches.AllowUserToAddRows = false;
                dgvBatches.AllowUserToDeleteRows = false;
                dgvBatches.AllowUserToResizeRows = false;
                dgvBatches.ReadOnly = true;
                dgvBatches.EnableHeadersVisualStyles = false;

                // 基础暗金风（细节交给主题系统统一处理）
                dgvBatches.ColumnHeadersDefaultCellStyle.BackColor = LeagueColors.DarkPanel;
                dgvBatches.ColumnHeadersDefaultCellStyle.ForeColor = LeagueColors.RiotGold;
                dgvBatches.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvBatches.ColumnHeadersHeight = 35;

                dgvBatches.DefaultCellStyle.BackColor = LeagueColors.DarkSurface;
                dgvBatches.DefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
                dgvBatches.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
                dgvBatches.DefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;
                dgvBatches.RowTemplate.Height = 30;

                LogManager.Info("数据网格样式设置完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("数据网格样式设置失败", ex);
            }
        }

        #endregion

        #region 数据操作方法

        /// <summary>
        /// 加载批次数据
        /// </summary>
        private void LoadBatchData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // 只加载可取消状态的批次（已创建但未开始生产）
                batchDataTable = batchBLL.GetCancellableBatches();

                if (batchDataTable != null && batchDataTable.Rows.Count > 0)
                {
                    dgvBatches.DataSource = batchDataTable;
                    SetupDataGridColumns();
                    lblBatchCount.Text = string.Format("共 {0} 条可取消批次", batchDataTable.Rows.Count);
                    TrySelectBatchRow(_preselectBatchNo);
                }
                else
                {
                    dgvBatches.DataSource = null;
                    lblBatchCount.Text = "暂无可取消的批次";
                }

                LogManager.Info(string.Format("加载批次数据完成，共 {0} 条记录",
                    batchDataTable != null ? batchDataTable.Rows.Count : 0));
            }
            catch (Exception ex)
            {
                LogManager.Error("加载批次数据失败", ex);
                MessageBox.Show(string.Format("加载批次数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void TrySelectBatchRow(string batchNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(batchNo)) return;
                if (dgvBatches == null) return;
                if (dgvBatches.Rows == null || dgvBatches.Rows.Count == 0) return;

                dgvBatches.ClearSelection();

                foreach (DataGridViewRow row in dgvBatches.Rows)
                {
                    if (row == null || row.IsNewRow) continue;
                    var cellValue = row.Cells["BatchNo"].Value != null ? row.Cells["BatchNo"].Value.ToString() : null;
                    if (string.Equals(cellValue, batchNo, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        dgvBatches.CurrentCell = row.Cells["BatchNo"];
                        dgvBatches.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// 设置数据网格列
        /// </summary>
        private void SetupDataGridColumns()
        {
            try
            {
                if (dgvBatches.Columns.Count > 0)
                {
                    // 设置列标题和宽度
                    dgvBatches.Columns["BatchNo"].HeaderText = "批次号";
                    dgvBatches.Columns["BatchNo"].Width = 120;

                    dgvBatches.Columns["WorkOrderNo"].HeaderText = "工单号";
                    dgvBatches.Columns["WorkOrderNo"].Width = 120;

                    dgvBatches.Columns["ProductCode"].HeaderText = "产品编号";
                    dgvBatches.Columns["ProductCode"].Width = 120;

                    dgvBatches.Columns["BatchQuantity"].HeaderText = "批次数量";
                    dgvBatches.Columns["BatchQuantity"].Width = 80;

                    dgvBatches.Columns["Status"].HeaderText = "状态";
                    dgvBatches.Columns["Status"].Width = 80;

                    dgvBatches.Columns["CreatedDate"].HeaderText = "创建时间";
                    dgvBatches.Columns["CreatedDate"].Width = 120;

                    dgvBatches.Columns["CreatedBy"].HeaderText = "创建人";
                    dgvBatches.Columns["CreatedBy"].Width = 80;

                    // 隐藏不需要显示的列
                    if (dgvBatches.Columns.Contains("Id"))
                        dgvBatches.Columns["Id"].Visible = false;
                }

                LogManager.Info("数据网格列设置完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("数据网格列设置失败", ex);
            }
        }

        #endregion

        #region 事件处理方法

        /// <summary>
        /// 批次选择变化事件
        /// </summary>
        private void dgvBatches_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvBatches.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvBatches.SelectedRows[0];
                    selectedBatchNo = selectedRow.Cells["BatchNo"].Value != null ? selectedRow.Cells["BatchNo"].Value.ToString() : null;

                    // 显示批次详细信息
                    ShowBatchDetails(selectedRow);

                    // 启用取消按钮
                    btnCancel.Enabled = !string.IsNullOrEmpty(selectedBatchNo);
                }
                else
                {
                    selectedBatchNo = null;
                    ClearBatchDetails();
                    btnCancel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("批次选择变化处理失败", ex);
            }
        }

        /// <summary>
        /// 显示批次详细信息
        /// </summary>
        private void ShowBatchDetails(DataGridViewRow row)
        {
            try
            {
                lblBatchNo.Text = row.Cells["BatchNo"].Value != null ? row.Cells["BatchNo"].Value.ToString() : "";
                lblWorkOrderNo.Text = row.Cells["WorkOrderNo"].Value != null ? row.Cells["WorkOrderNo"].Value.ToString() : "";
                lblProductCode.Text = row.Cells["ProductCode"].Value != null ? row.Cells["ProductCode"].Value.ToString() : "";
                lblBatchQuantity.Text = row.Cells["BatchQuantity"].Value != null ? row.Cells["BatchQuantity"].Value.ToString() : "";
                lblStatus.Text = row.Cells["Status"].Value != null ? row.Cells["Status"].Value.ToString() : "";
                lblCreatedBy.Text = row.Cells["CreatedBy"].Value != null ? row.Cells["CreatedBy"].Value.ToString() : "";
                lblCreatedDate.Text = row.Cells["CreatedDate"].Value != null ?
                    Convert.ToDateTime(row.Cells["CreatedDate"].Value).ToString("yyyy-MM-dd HH:mm:ss") : "";
            }
            catch (Exception ex)
            {
                LogManager.Error("显示批次详细信息失败", ex);
            }
        }

        /// <summary>
        /// 清空批次详细信息
        /// </summary>
        private void ClearBatchDetails()
        {
            lblBatchNo.Text = "";
            lblWorkOrderNo.Text = "";
            lblProductCode.Text = "";
            lblBatchQuantity.Text = "";
            lblStatus.Text = "";
            lblCreatedBy.Text = "";
            lblCreatedDate.Text = "";
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedBatchNo))
                {
                    MessageBox.Show("请先选择要取消的批次！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCancelReason.Text))
                {
                    MessageBox.Show("请输入取消原因！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCancelReason.Focus();
                    return;
                }

                var result = MessageBox.Show(
                    string.Format("确认要取消批次 [{0}] 吗？\n\n取消原因：{1}", selectedBatchNo, txtCancelReason.Text),
                    "确认取消", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    CancelBatchOperation();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("取消批次按钮点击处理失败", ex);
                MessageBox.Show(string.Format("操作失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 执行取消批次操作
        /// </summary>
        private void CancelBatchOperation()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // 调用业务逻辑层取消批次
                bool success = batchBLL.CancelBatch(selectedBatchNo, txtCancelReason.Text.Trim());

                if (success)
                {
                    MessageBox.Show("批次取消成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("批次 [{0}] 取消成功，取消原因：{1}",
                        selectedBatchNo, txtCancelReason.Text.Trim()));

                    // 清空取消原因
                    txtCancelReason.Clear();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("批次取消失败，请检查批次状态或联系系统管理员！", "失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("执行取消批次操作失败", ex);
                MessageBox.Show(string.Format("取消批次失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                txtCancelReason.Clear();
                LoadBatchData();
                MessageBox.Show("数据已刷新！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show(string.Format("刷新数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 窗体事件

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void CancelBatch_Load(object sender, EventArgs e)
        {
            try
            {
                LogManager.Info("取消批次窗体加载完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("取消批次窗体加载失败", ex);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        private void CancelBatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                LogManager.Info("取消批次窗体已关闭");
            }
            catch (Exception ex)
            {
                LogManager.Error("取消批次窗体关闭处理失败", ex);
            }
        }

        #endregion
    }
}
