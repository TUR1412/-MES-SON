using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Models.Material;
using MES.BLL.Material;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// BOM物料清单管理窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class BOMManagementForm : Form
    {
        private List<BOMInfo> bomList;
        private List<BOMInfo> filteredBomList;
        private BOMInfo currentBom;
        private BOMBLL bomBLL;

        public BOMManagementForm()
        {
            InitializeComponent();
            bomBLL = new BOMBLL();
            InitializeForm();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // 初始化数据
                bomList = new List<BOMInfo>();
                filteredBomList = new List<BOMInfo>();
                currentBom = null;

                // 设置DataGridView
                SetupDataGridView();

                // 加载BOM数据
                LoadBOMData();

                // 刷新显示
                RefreshDataGridView();

                // 清空详情面板
                ClearDetailsPanel();

                LogManager.Info("BOM物料清单管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("BOM物料清单管理窗体初始化失败", ex);
                MessageBox.Show("窗体初始化失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置DataGridView
        /// </summary>
        private void SetupDataGridView()
        {
            // 设置列
            dataGridViewBOM.AutoGenerateColumns = false;
            dataGridViewBOM.Columns.Clear();

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BOMCode",
                HeaderText = "BOM编码",
                DataPropertyName = "BOMCode",
                Width = 140,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName",
                HeaderText = "产品名称",
                DataPropertyName = "ProductName",
                Width = 150,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialName",
                HeaderText = "子物料名称",
                DataPropertyName = "MaterialName",
                Width = 150,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Quantity",
                HeaderText = "需求数量",
                DataPropertyName = "Quantity",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "单位",
                DataPropertyName = "Unit",
                Width = 80,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BOMVersion",
                HeaderText = "版本",
                DataPropertyName = "BOMVersion",
                Width = 80,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BOMType",
                HeaderText = "类型",
                DataPropertyName = "BOMType",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewBOM.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "Status",
                Width = 80,
                ReadOnly = true
            });

            // 设置样式
            dataGridViewBOM.EnableHeadersVisualStyles = false;
            dataGridViewBOM.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewBOM.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewBOM.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            dataGridViewBOM.ColumnHeadersHeight = 40;

            dataGridViewBOM.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
            dataGridViewBOM.DefaultCellStyle.BackColor = Color.White;
            dataGridViewBOM.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewBOM.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewBOM.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridViewBOM.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridViewBOM.GridColor = Color.FromArgb(222, 226, 230);
        }

        /// <summary>
        /// 加载BOM数据
        /// </summary>
        private void LoadBOMData()
        {
            try
            {
                bomList.Clear();

                // 从数据库加载真实的BOM数据
                bomList = bomBLL.GetAllBOMs();

                // 如果数据库中没有数据，可以添加一些示例数据用于演示
                if (bomList == null || bomList.Count == 0)
                {
                    bomList = new List<BOMInfo>();
                    // 这里可以添加示例数据，但现在我们使用真实的数据库数据
                }



            // 复制到过滤列表
            filteredBomList = new List<BOMInfo>(bomList);
            }
            catch (Exception ex)
            {
                LogManager.Error("加载BOM数据失败", ex);
                MessageBox.Show(string.Format("加载BOM数据失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                dataGridViewBOM.DataSource = null;
                dataGridViewBOM.DataSource = filteredBomList;

                // 如果有数据，选中第一行
                if (filteredBomList.Count > 0)
                {
                    dataGridViewBOM.Rows[0].Selected = true;
                    ShowBOMDetails(filteredBomList[0]);
                }
                else
                {
                    ClearDetailsPanel();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据显示失败", ex);
            }
        }

        /// <summary>
        /// 搜索框文本变化事件
        /// </summary>
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = textBoxSearch.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // 显示所有BOM记录
                    filteredBomList = new List<BOMInfo>(bomList);
                }
                else
                {
                    // 根据BOM编码、产品名称、物料名称进行搜索
                    filteredBomList = bomList.Where(b =>
                        b.BOMCode.ToLower().Contains(searchTerm) ||
                        b.ProductName.ToLower().Contains(searchTerm) ||
                        b.MaterialName.ToLower().Contains(searchTerm) ||
                        b.ProductCode.ToLower().Contains(searchTerm) ||
                        b.MaterialCode.ToLower().Contains(searchTerm) ||
                        b.BOMType.ToLower().Contains(searchTerm)
                    ).ToList();
                }

                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索失败", ex);
                MessageBox.Show("搜索失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// DataGridView选择变化事件
        /// </summary>
        private void dataGridViewBOM_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewBOM.CurrentRow != null &&
                    dataGridViewBOM.CurrentRow.DataBoundItem != null)
                {
                    var bom = dataGridViewBOM.CurrentRow.DataBoundItem as BOMInfo;
                    if (bom != null)
                    {
                        ShowBOMDetails(bom);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("显示BOM详情失败", ex);
            }
        }

        /// <summary>
        /// 显示BOM详情
        /// </summary>
        private void ShowBOMDetails(BOMInfo bom)
        {
            if (bom == null)
            {
                ClearDetailsPanel();
                return;
            }

            currentBom = bom;

            // 填充基本信息
            textBoxBOMCode.Text = bom.BOMCode;
            textBoxBOMVersion.Text = bom.BOMVersion;
            comboBoxBOMType.Text = bom.BOMType;
            checkBoxStatus.Checked = bom.Status;

            // 填充产品信息
            textBoxProductCode.Text = bom.ProductCode;
            textBoxProductName.Text = bom.ProductName;

            // 填充物料信息
            textBoxMaterialCode.Text = bom.MaterialCode;
            textBoxMaterialName.Text = bom.MaterialName;
            textBoxQuantity.Text = bom.Quantity.ToString();
            textBoxUnit.Text = bom.Unit;
            textBoxLossRate.Text = bom.LossRate.ToString();
            textBoxSubstituteMaterial.Text = bom.SubstituteMaterial;

            // 填充时间信息
            dateTimePickerEffectiveDate.Value = bom.EffectiveDate;
            if (bom.ExpireDate.HasValue)
            {
                checkBoxHasExpireDate.Checked = true;
                dateTimePickerExpireDate.Value = bom.ExpireDate.Value;
                dateTimePickerExpireDate.Enabled = true;
            }
            else
            {
                checkBoxHasExpireDate.Checked = false;
                dateTimePickerExpireDate.Enabled = false;
            }

            // 填充备注
            textBoxRemarks.Text = bom.Remarks;

            // 填充创建时间
            if (bom.CreateTime != DateTime.MinValue)
            {
                labelCreateTime.Text = "创建时间：" + bom.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                labelCreateTime.Text = "创建时间：未知";
            }
        }

        /// <summary>
        /// 清空详情面板
        /// </summary>
        private void ClearDetailsPanel()
        {
            currentBom = null;

            textBoxBOMCode.Text = string.Empty;
            textBoxBOMVersion.Text = string.Empty;
            comboBoxBOMType.SelectedIndex = -1;
            checkBoxStatus.Checked = false;

            textBoxProductCode.Text = string.Empty;
            textBoxProductName.Text = string.Empty;

            textBoxMaterialCode.Text = string.Empty;
            textBoxMaterialName.Text = string.Empty;
            textBoxQuantity.Text = string.Empty;
            textBoxUnit.Text = string.Empty;
            textBoxLossRate.Text = string.Empty;
            textBoxSubstituteMaterial.Text = string.Empty;

            dateTimePickerEffectiveDate.Value = DateTime.Now;
            checkBoxHasExpireDate.Checked = false;
            dateTimePickerExpireDate.Enabled = false;

            textBoxRemarks.Text = string.Empty;
            labelCreateTime.Text = "创建时间：";
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建新BOM对话框
                var result = ShowBOMEditDialog(null);
                if (result != null)
                {
                    // 设置创建时间
                    result.CreateTime = DateTime.Now;
                    result.UpdateTime = DateTime.Now;

                    // 调用BLL层真正保存到数据库
                    if (bomBLL.AddBOM(result))
                    {
                        // 重新加载数据以获取数据库生成的ID
                        LoadBOMData();
                        RefreshDataGridView();

                        // 选中新添加的BOM
                        var newBom = bomList.FirstOrDefault(b => b.BOMCode == result.BOMCode);
                        if (newBom != null)
                        {
                            SelectBOMById(newBom.Id);
                        }

                        MessageBox.Show("BOM记录添加成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("添加BOM记录：{0} - {1}",
                            result.BOMCode, result.ProductName));
                    }
                    else
                    {
                        MessageBox.Show("BOM记录添加失败！", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加BOM记录失败", ex);
                MessageBox.Show("添加BOM记录失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentBom == null)
                {
                    MessageBox.Show("请先选择要编辑的BOM记录！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 创建编辑对话框
                var result = ShowBOMEditDialog(currentBom);
                if (result != null)
                {
                    // 设置更新时间
                    result.UpdateTime = DateTime.Now;

                    // 调用BLL层真正更新到数据库
                    if (bomBLL.UpdateBOM(result))
                    {
                        // 重新加载数据以获取最新状态
                        LoadBOMData();
                        RefreshDataGridView();

                        // 重新选中编辑的BOM
                        SelectBOMById(result.Id);

                        MessageBox.Show("BOM记录编辑成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("编辑BOM记录：{0} - {1}",
                            result.BOMCode, result.ProductName));
                    }
                    else
                    {
                        MessageBox.Show("BOM记录编辑失败！", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("编辑BOM记录失败", ex);
                MessageBox.Show("编辑BOM记录失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentBom == null)
                {
                    MessageBox.Show("请先选择要删除的BOM记录！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    string.Format("确认要删除BOM记录 [{0} - {1}] 吗？\n\n此操作不可撤销！",
                        currentBom.BOMCode, currentBom.ProductName),
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // 调用BLL层真正从数据库删除
                    if (bomBLL.DeleteBOM(currentBom.Id))
                    {
                        // 重新加载数据以反映删除结果
                        LoadBOMData();
                        RefreshDataGridView();

                        MessageBox.Show("BOM记录删除成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("删除BOM记录：{0} - {1}",
                            currentBom.BOMCode, currentBom.ProductName));
                    }
                    else
                    {
                        MessageBox.Show("BOM记录删除失败！", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除BOM记录失败", ex);
                MessageBox.Show("删除BOM记录失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空搜索框
                textBoxSearch.Text = string.Empty;

                // 重新加载数据
                LoadBOMData();
                RefreshDataGridView();

                MessageBox.Show("数据刷新成功！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info("BOM物料清单数据已刷新");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show("刷新数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据ID选中BOM
        /// </summary>
        private void SelectBOMById(int bomId)
        {
            try
            {
                for (int i = 0; i < dataGridViewBOM.Rows.Count; i++)
                {
                    var bom = dataGridViewBOM.Rows[i].DataBoundItem as BOMInfo;
                    if (bom != null && bom.Id == bomId)
                    {
                        dataGridViewBOM.ClearSelection();
                        dataGridViewBOM.Rows[i].Selected = true;
                        dataGridViewBOM.CurrentCell = dataGridViewBOM.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("选中BOM失败", ex);
            }
        }

        /// <summary>
        /// 显示BOM编辑对话框
        /// </summary>
        private BOMInfo ShowBOMEditDialog(BOMInfo bom)
        {
            try
            {
                // 使用真实的BOM编辑窗体
                using (var editForm = new BOMEditForm(bom))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        return editForm.BOMData;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error("显示BOM编辑对话框失败", ex);
                MessageBox.Show(string.Format("打开BOM编辑窗体失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// 失效日期复选框变化事件
        /// </summary>
        private void checkBoxHasExpireDate_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerExpireDate.Enabled = checkBoxHasExpireDate.Checked;
        }
    }
}