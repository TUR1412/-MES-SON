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

        public BOMManagementForm()
        {
            InitializeComponent();
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

                // 加载示例数据
                LoadSampleData();

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
        /// 加载示例数据
        /// </summary>
        private void LoadSampleData()
        {
            bomList.Clear();

            // 添加示例BOM数据
            bomList.Add(new BOMInfo
            {
                Id = 1,
                BOMCode = "BOM-001-001",
                ProductId = 1,
                ProductCode = "P001",
                ProductName = "钢制支架",
                MaterialId = 101,
                MaterialCode = "M101",
                MaterialName = "钢板",
                Quantity = 2.5m,
                Unit = "kg",
                LossRate = 5.0m,
                BOMVersion = "1.0",
                BOMType = "PRODUCTION",
                EffectiveDate = DateTime.Now.AddDays(-30),
                Status = true,
                Remarks = "主要结构材料"
            });

            bomList.Add(new BOMInfo
            {
                Id = 2,
                BOMCode = "BOM-001-002",
                ProductId = 1,
                ProductCode = "P001",
                ProductName = "钢制支架",
                MaterialId = 102,
                MaterialCode = "M102",
                MaterialName = "螺栓",
                Quantity = 8,
                Unit = "个",
                LossRate = 2.0m,
                BOMVersion = "1.0",
                BOMType = "PRODUCTION",
                EffectiveDate = DateTime.Now.AddDays(-30),
                Status = true,
                Remarks = "连接件"
            });

            bomList.Add(new BOMInfo
            {
                Id = 3,
                BOMCode = "BOM-002-001",
                ProductId = 2,
                ProductCode = "P002",
                ProductName = "铝合金外壳",
                MaterialId = 201,
                MaterialCode = "M201",
                MaterialName = "铝合金板",
                Quantity = 1.2m,
                Unit = "kg",
                LossRate = 3.0m,
                BOMVersion = "2.1",
                BOMType = "PRODUCTION",
                EffectiveDate = DateTime.Now.AddDays(-15),
                Status = true,
                Remarks = "外壳主体材料"
            });

            bomList.Add(new BOMInfo
            {
                Id = 4,
                BOMCode = "BOM-002-002",
                ProductId = 2,
                ProductCode = "P002",
                ProductName = "铝合金外壳",
                MaterialId = 202,
                MaterialCode = "M202",
                MaterialName = "密封胶条",
                Quantity = 0.5m,
                Unit = "m",
                LossRate = 10.0m,
                BOMVersion = "2.1",
                BOMType = "PRODUCTION",
                EffectiveDate = DateTime.Now.AddDays(-15),
                Status = true,
                Remarks = "防水密封"
            });

            bomList.Add(new BOMInfo
            {
                Id = 5,
                BOMCode = "BOM-003-001",
                ProductId = 3,
                ProductCode = "P003",
                ProductName = "精密齿轮",
                MaterialId = 301,
                MaterialCode = "M301",
                MaterialName = "合金钢",
                Quantity = 0.8m,
                Unit = "kg",
                LossRate = 8.0m,
                BOMVersion = "1.5",
                BOMType = "ENGINEERING",
                EffectiveDate = DateTime.Now.AddDays(-7),
                Status = true,
                Remarks = "高精度加工"
            });

            bomList.Add(new BOMInfo
            {
                Id = 6,
                BOMCode = "BOM-004-001",
                ProductId = 4,
                ProductCode = "P004",
                ProductName = "电机外壳",
                MaterialId = 401,
                MaterialCode = "M401",
                MaterialName = "铸铁",
                Quantity = 3.0m,
                Unit = "kg",
                LossRate = 4.0m,
                BOMVersion = "1.0",
                BOMType = "PRODUCTION",
                EffectiveDate = DateTime.Now.AddDays(-20),
                Status = false,
                Remarks = "已停用版本"
            });

            bomList.Add(new BOMInfo
            {
                Id = 7,
                BOMCode = "BOM-005-001",
                ProductId = 5,
                ProductCode = "P005",
                ProductName = "不锈钢管件",
                MaterialId = 501,
                MaterialCode = "M501",
                MaterialName = "不锈钢管",
                Quantity = 1.0m,
                Unit = "根",
                LossRate = 1.0m,
                BOMVersion = "1.2",
                BOMType = "PRODUCTION",
                EffectiveDate = DateTime.Now.AddDays(-5),
                Status = true,
                Remarks = "标准规格"
            });

            // 复制到过滤列表
            filteredBomList = new List<BOMInfo>(bomList);
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
                    // 生成新ID
                    result.Id = bomList.Count > 0 ? bomList.Max(b => b.Id) + 1 : 1;
                    result.CreateTime = DateTime.Now;
                    result.UpdateTime = DateTime.Now;

                    // 添加到列表
                    bomList.Add(result);

                    // 刷新显示
                    RefreshDataGridView();

                    // 选中新添加的BOM
                    SelectBOMById(result.Id);

                    MessageBox.Show("BOM记录添加成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("添加BOM记录：{0} - {1}",
                        result.BOMCode, result.ProductName));
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
                    // 更新BOM信息
                    var originalBom = bomList.FirstOrDefault(b => b.Id == currentBom.Id);
                    if (originalBom != null)
                    {
                        originalBom.BOMCode = result.BOMCode;
                        originalBom.ProductCode = result.ProductCode;
                        originalBom.ProductName = result.ProductName;
                        originalBom.MaterialCode = result.MaterialCode;
                        originalBom.MaterialName = result.MaterialName;
                        originalBom.Quantity = result.Quantity;
                        originalBom.Unit = result.Unit;
                        originalBom.LossRate = result.LossRate;
                        originalBom.SubstituteMaterial = result.SubstituteMaterial;
                        originalBom.BOMVersion = result.BOMVersion;
                        originalBom.BOMType = result.BOMType;
                        originalBom.EffectiveDate = result.EffectiveDate;
                        originalBom.ExpireDate = result.ExpireDate;
                        originalBom.Status = result.Status;
                        originalBom.Remarks = result.Remarks;
                        originalBom.UpdateTime = DateTime.Now;

                        // 刷新显示
                        RefreshDataGridView();

                        // 重新选中编辑的BOM
                        SelectBOMById(originalBom.Id);

                        MessageBox.Show("BOM记录编辑成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("编辑BOM记录：{0} - {1}",
                            originalBom.BOMCode, originalBom.ProductName));
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
                    // 从列表中移除
                    bomList.RemoveAll(b => b.Id == currentBom.Id);

                    // 刷新显示
                    RefreshDataGridView();

                    MessageBox.Show("BOM记录删除成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("删除BOM记录：{0} - {1}",
                        currentBom.BOMCode, currentBom.ProductName));
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
                LoadSampleData();
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
            // 这里应该打开一个BOM编辑对话框
            // 为了演示，我们使用简单的逻辑
            string title = bom == null ? "新增BOM" : "编辑BOM";

            // 简化的编辑逻辑，实际应该使用专门的编辑窗体
            var editBom = bom != null ? bom.Clone() : new BOMInfo();

            // 暂时返回示例数据用于演示
            if (bom == null)
            {
                // 新增BOM的示例
                return new BOMInfo
                {
                    BOMCode = "BOM-NEW-" + (bomList.Count + 1).ToString("000"),
                    ProductCode = "P" + (bomList.Count + 1).ToString("000"),
                    ProductName = "新产品",
                    MaterialCode = "M" + (bomList.Count + 1).ToString("000"),
                    MaterialName = "新物料",
                    Quantity = 1.0m,
                    Unit = "个",
                    LossRate = 0m,
                    BOMVersion = "1.0",
                    BOMType = "PRODUCTION",
                    EffectiveDate = DateTime.Now,
                    Status = true,
                    Remarks = "新增BOM记录"
                };
            }
            else
            {
                // 编辑现有BOM
                editBom.Remarks = editBom.Remarks + " (已编辑)";
                return editBom;
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