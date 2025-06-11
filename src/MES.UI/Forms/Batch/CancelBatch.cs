using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MES.BLL.Workshop;
using MES.Models.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;
using MES.DAL.Core;

namespace MES.UI.Forms.Batch
{
    /// <summary>
    /// 取消批次窗体 - 现代化UI设计
    /// 功能：取消已创建的生产批次，包含取消原因记录
    /// </summary>
    public partial class CancelBatch : Form
    {
        #region 私有字段

        private BatchBLL batchBLL;
        private DataTable batchDataTable;
        private string selectedBatchNo;
        private int selectedBatchId;

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
                batchBLL = new BatchBLL();
            }
            catch (Exception ex)
            {
                LogManager.Error("业务逻辑初始化失败", ex);
                throw new MESException("业务逻辑初始化失败", ex);
            }
        }
        /// <summary>
        /// 初始化UI界面
        /// </summary>
        private void InitializeUI()
        {
            // 设置窗体属性
            this.Text = "取消批次";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // 初始化控件状态
            btnCancel.Enabled = false;
            txtCancelReason.MaxLength = 500;

            // 设置数据网格样式（根据图片中的蓝色表头）
            dgvBatches.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
            dgvBatches.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBatches.EnableHeadersVisualStyles = false;
        }
        /// <summary>
        /// 设置数据网格样式
        /// </summary>
        private void SetupDataGridView()
        {
            dgvBatches.BackgroundColor = Color.White;
            dgvBatches.BorderStyle = BorderStyle.None;
            dgvBatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBatches.RowHeadersVisible = false;
            dgvBatches.AllowUserToAddRows = false;
            dgvBatches.AllowUserToDeleteRows = false;
            dgvBatches.ReadOnly = true;
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

                // 从数据库获取可取消批次数据
                batchDataTable = batchBLL.GetCancellableBatches();

                if (batchDataTable == null || batchDataTable.Rows.Count == 0)
                {
                    dgvBatches.DataSource = null;
                    lblBatchCount.Text = "暂无可取消的批次";
                    return;
                }

                // 绑定数据源
                dgvBatches.DataSource = batchDataTable;

                // 设置列显示
                dgvBatches.Columns["Id"].Visible = false;
                dgvBatches.Columns["BatchNumber"].HeaderText = "批次号";
                dgvBatches.Columns["ProductionOrderNumber"].HeaderText = "生产订单号";
                dgvBatches.Columns["ProductCode"].HeaderText = "产品编码";
                dgvBatches.Columns["PlannedQuantity"].HeaderText = "计划数量";
                dgvBatches.Columns["BatchStatus"].HeaderText = "状态";
                dgvBatches.Columns["CreateUserName"].HeaderText = "创建人";
                dgvBatches.Columns["CreateTime"].HeaderText = "创建时间";

                // 设置列宽
                dgvBatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lblBatchCount.Text = string.Format("共 {0} 条可取消批次", batchDataTable.Rows.Count);
            }
            catch (Exception ex)
            {
                LogManager.Error("加载批次数据失败: " + ex.Message);
                MessageBox.Show("加载批次数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 设置数据网格列
        /// </summary>
        private void SetupDataGridColumns()
        {
            try
            {
                // 确保 DataGridView 有数据源且列存在
                if (dgvBatches.DataSource == null || dgvBatches.Columns.Count == 0)
                {
                    return;
                }

                // 安全地设置列标题和宽度
                if (dgvBatches.Columns.Contains("BatchNo"))
                {
                    dgvBatches.Columns["BatchNo"].HeaderText = "批次号";
                    dgvBatches.Columns["BatchNo"].Width = 120;
                }

                if (dgvBatches.Columns.Contains("WorkOrderNo"))
                {
                    dgvBatches.Columns["WorkOrderNo"].HeaderText = "工单号";
                    dgvBatches.Columns["WorkOrderNo"].Width = 120;
                }

                if (dgvBatches.Columns.Contains("ProductCode"))
                {
                    dgvBatches.Columns["ProductCode"].HeaderText = "产品编号";
                    dgvBatches.Columns["ProductCode"].Width = 120;
                }

                if (dgvBatches.Columns.Contains("BatchQuantity"))
                {
                    dgvBatches.Columns["BatchQuantity"].HeaderText = "批次数量";
                    dgvBatches.Columns["BatchQuantity"].Width = 80;
                }

                if (dgvBatches.Columns.Contains("Status"))
                {
                    dgvBatches.Columns["Status"].HeaderText = "状态";
                    dgvBatches.Columns["Status"].Width = 80;
                }

                if (dgvBatches.Columns.Contains("CreatedDate"))
                {
                    dgvBatches.Columns["CreatedDate"].HeaderText = "创建时间";
                    dgvBatches.Columns["CreatedDate"].Width = 120;
                }

                if (dgvBatches.Columns.Contains("CreatedBy"))
                {
                    dgvBatches.Columns["CreatedBy"].HeaderText = "创建人";
                    dgvBatches.Columns["CreatedBy"].Width = 80;
                }

                // 隐藏不需要显示的列
                if (dgvBatches.Columns.Contains("Id"))
                {
                    dgvBatches.Columns["Id"].Visible = false;
                }

                LogManager.Info("数据网格列设置完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("数据网格列设置失败", ex);
                throw;
            }
        }

        #endregion

        #region 事件处理方法

        /// <summary>
        /// 批次选择变化事件
        /// </summary>
        private void dgvBatches_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBatches.SelectedRows.Count == 0)
            {
                btnCancel.Enabled = false;
                return;
            }

            try
            {
                DataGridViewRow selectedRow = dgvBatches.SelectedRows[0];

                // 获取选中行的数据
                selectedBatchId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                selectedBatchNo = selectedRow.Cells["BatchNumber"].Value.ToString();

                // 更新下方显示区域
                lblBatchNo.Text = selectedRow.Cells["BatchNumber"].Value.ToString();
                lblWorkOrderNo.Text = selectedRow.Cells["ProductionOrderNumber"].Value.ToString();
                lblProductCode.Text = selectedRow.Cells["ProductCode"].Value.ToString();
                lblBatchQuantity.Text = selectedRow.Cells["PlannedQuantity"].Value.ToString();
                lblStatus.Text = selectedRow.Cells["BatchStatus"].Value.ToString();
                lblCreatedBy.Text = selectedRow.Cells["CreateUserName"].Value.ToString();
                lblCreatedDate.Text = Convert.ToDateTime(selectedRow.Cells["CreateTime"].Value).ToString("yyyy-MM-dd HH:mm:ss");

                // 启用取消按钮
                btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                LogManager.Error("选择批次时出错: " + ex.Message);
                btnCancel.Enabled = false;
            }
        }

        /// <summary>
        /// 显示批次详细信息
        /// </summary>
        private void ShowBatchDetails(DataGridViewRow row)
        {
            try
            {
                lblBatchNo.Text = row.Cells["BatchNo"].Value != null ?
                    row.Cells["BatchNo"].Value.ToString() : "";
                lblWorkOrderNo.Text = row.Cells["WorkOrderNo"].Value != null ?
                    row.Cells["WorkOrderNo"].Value.ToString() : "";
                lblProductCode.Text = row.Cells["ProductCode"].Value != null ?
                    row.Cells["ProductCode"].Value.ToString() : "";
                lblBatchQuantity.Text = row.Cells["BatchQuantity"].Value != null ?
                    row.Cells["BatchQuantity"].Value.ToString() : "";
                lblStatus.Text = row.Cells["Status"].Value != null ?
                    row.Cells["Status"].Value.ToString() : "";
                lblCreatedBy.Text = row.Cells["CreatedBy"].Value != null ?
                    row.Cells["CreatedBy"].Value.ToString() : "";
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
            if (string.IsNullOrEmpty(selectedBatchNo) || selectedBatchId <= 0)
            {
                MessageBox.Show("请先选择要取消的批次！", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DialogResult result = MessageBox.Show(
                    "确认要取消批次 [" + selectedBatchNo + "] 吗？",
                    "确认取消",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;

                    // 执行物理删除
                    string sql = "DELETE FROM batch_info WHERE id = @id";
                    var parameters = new[]
                    {
                        DatabaseHelper.CreateParameter("@id", selectedBatchId)
                    };

                    int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("批次取消成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 记录日志
                        LogManager.Info(string.Format("批次 [{0}] 已取消", selectedBatchNo));

                        // 刷新数据
                        LoadBatchData();

                        // 清空显示区域
                        ClearBatchDetails();
                    }
                    else
                    {
                        MessageBox.Show("批次取消失败！", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("取消批次失败: " + ex.Message);
                MessageBox.Show("取消批次失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
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

                // 调用业务逻辑层取消批次（物理删除）
                bool success = batchBLL.DeleteBatch(selectedBatchId);

                if (success)
                {
                    MessageBox.Show("批次取消成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info("批次 [" + selectedBatchNo + "] 取消成功，取消原因：" + txtCancelReason.Text.Trim());

                    // 清空取消原因
                    txtCancelReason.Clear();

                    // 重新加载数据
                    LoadBatchData();

                    // 如果没有更多可取消的批次，关闭窗体
                    if (batchDataTable == null || batchDataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("已无可取消的批次，窗体将关闭。", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
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
                MessageBox.Show("取消批次失败：" + ex.Message, "错误",
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
                MessageBox.Show("刷新数据失败：" + ex.Message, "错误",
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