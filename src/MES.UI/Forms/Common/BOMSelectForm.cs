using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MES.Models.Material;
using MES.BLL.Material;
using MES.Common.Logging;

namespace MES.UI.Forms.Common
{
    /// <summary>
    /// BOM选择窗体
    /// </summary>
    public partial class BOMSelectForm : Form
    {
        /// <summary>
        /// 选中的BOM
        /// </summary>
        public BOMInfo SelectedBOM { get; private set; }

        private List<BOMInfo> bomList;
        private readonly IBOMBLL _bomBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BOMSelectForm()
        {
            InitializeComponent();
            _bomBLL = new BOMBLL();
            this.Load += BOMSelectForm_Load;
            this.Load += (sender, e) =>
            {
                ApplyLeagueTheme();
            };
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
        /// 窗体加载事件
        /// </summary>
        private void BOMSelectForm_Load(object sender, EventArgs e)
        {
            LoadBOMData();
        }

        /// <summary>
        /// 加载BOM数据
        /// </summary>
        private void LoadBOMData()
        {
            try
            {
                bomList = _bomBLL.GetAllBOMs();
                dgvBOMs.DataSource = bomList;
                
                if (bomList.Count > 0)
                {
                    dgvBOMs.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("加载BOM数据失败", ex);
                MessageBox.Show("加载BOM数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvBOMs.SelectedRows.Count > 0)
            {
                SelectedBOM = dgvBOMs.SelectedRows[0].DataBoundItem as BOMInfo;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("请选择一个BOM！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 双击选择
        /// </summary>
        private void dgvBOMs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnOK_Click(sender, e);
            }
        }
    }
}