using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.BLL.Material;       // 引用BLL接口
using MES.BLL.Material.DTO;   // 引用DTO
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.Material
{
    /// <summary>
    /// 物料信息管理窗体 - 现代化界面设计
    /// </summary>
    public partial class MaterialManagementForm : ThemedForm
    {
        // --- 依赖于BLL接口和DTO ---
        private readonly IMaterialBLL _materialBLL;
        private List<MaterialDto> materialList;
        private List<MaterialDto> filteredMaterialList;
        private MaterialDto currentMaterial;
        private Timer _searchDebounceTimer;

        // Layout A：左侧表格 + 右侧详情卡（LoL 客户端风）
        private bool _lolLayoutABuilt;
        private SplitContainer _splitMain;
        private TableLayoutPanel _detailsLayout;

        // 右侧详情：概览
        private Label _lblSummaryCode;
        private Label _lblSummaryName;
        private Panel _panelTypeBadge;
        private Label _lblTypeBadgeText;

        // 右侧详情：关键信息
        private Label _valMaterialType;
        private Label _valUnit;
        private Label _valSpec;
        private Label _valSupplier;
        private Label _valPrice;

        // 右侧详情：备注（复用原设计器 TextBox，避免重复控件）
        private TextBox _txtRemark;

        // 右侧详情：快捷操作
        private LolActionButton _btnEditQuick;
        private LolActionButton _btnCopyCode;

        public MaterialManagementForm()
        {

            InitializeComponent();
            BuildLolLayoutA();
            // 实例化BLL
            _materialBLL = new MaterialBLL();
            InitializeForm();

            // 主题应用应尽量在控件创建完成后执行；
            // 这里用 Shown，避免被 InitializeForm/数据绑定过程中反复覆盖样式。
            Shown += (sender, e) =>
            {
                try
                {
                    UIThemeManager.ApplyTheme(this);
                }
                catch
                {
                    // ignore
                }
            };

            // LoL 细节打磨（不改业务逻辑）
            Shown += (sender, e) =>
            {
                try
                {
                    ApplyLolPolish();
                }
                catch
                {
                    // ignore
                }
            };
        }

        private void ApplyLolPolish()
        {
            try
            {
                // 标题与文本（去 emoji，更贴近 LoL 客户端克制风）
                if (labelTitle != null)
                {
                    labelTitle.Text = "物料管理";
                    labelTitle.ForeColor = LeagueColors.RiotGoldHover;
                }

                if (labelSearch != null)
                {
                    labelSearch.Text = "搜索：";
                    labelSearch.ForeColor = LeagueColors.TextSecondary;
                }

                if (btnAdd != null) btnAdd.Text = "新增";
                if (btnEdit != null) btnEdit.Text = "编辑";
                if (btnDelete != null) btnDelete.Text = "删除";
                if (btnRefresh != null) btnRefresh.Text = "刷新";

                // 面板做成暗色卡片块，交由主题系统绘制描边
                if (panelMain != null) panelMain.BackColor = LeagueColors.DarkBackground;

                if (panelHeader != null)
                {
                    panelHeader.BorderStyle = BorderStyle.None;
                    panelHeader.BackColor = LeagueColors.DarkPanel;
                }

                if (panelSearch != null)
                {
                    panelSearch.BorderStyle = BorderStyle.None;
                    panelSearch.BackColor = LeagueColors.DarkSurface;
                }

                if (panelButtons != null)
                {
                    panelButtons.BorderStyle = BorderStyle.None;
                    panelButtons.BackColor = LeagueColors.DarkSurface;
                }

                // 如果仍在使用旧的 GroupBox（未重构时），也做一次去 emoji
                try
                {
                    if (groupBoxBasicInfo != null) groupBoxBasicInfo.Text = "基本信息";
                    if (groupBoxAdvancedInfo != null) groupBoxAdvancedInfo.Text = "详细信息";
                }
                catch
                {
                    // ignore
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                materialList = new List<MaterialDto>();
                filteredMaterialList = new List<MaterialDto>();
                currentMaterial = null;

                SetupDataGridView();
                InitializeSearchDebounceTimer();

                // --- 从BLL加载真实数据，而不是示例数据 ---
                LoadMaterialData();

                RefreshDataGridView();
                ClearDetailsPanel();

                LogManager.Info("物料管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("物料管理窗体初始化失败", ex);
                MessageBox.Show("窗体初始化失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 从BLL加载物料数据
        /// </summary>
        private void LoadMaterialData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                materialList = _materialBLL.GetAllMaterialDtos();
                // 默认显示所有
                filteredMaterialList = new List<MaterialDto>(materialList);
            }
            catch (Exception ex)
            {
                LogManager.Error("加载物料数据失败", ex);
                MessageBox.Show("加载物料数据失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                // 记录当前选中项的ID，以便刷新后能重新选中
                int selectedId = (currentMaterial != null) ? currentMaterial.Id : 0;

                dataGridViewMaterials.DataSource = null;
                dataGridViewMaterials.DataSource = filteredMaterialList;

                if (filteredMaterialList.Any())
                {
                    // 尝试重新选中之前的项，如果找不到则选中第一项
                    var itemToSelect = filteredMaterialList.FirstOrDefault(m => m.Id == selectedId) ?? filteredMaterialList.First();
                    SelectMaterialById(itemToSelect.Id);
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
                // 输入过程中做轻量防抖，避免每个字符都触发全量刷新造成卡顿
                if (_searchDebounceTimer == null)
                {
                    ApplySearchFilter();
                    RefreshDataGridView();
                    return;
                }

                _searchDebounceTimer.Stop();
                _searchDebounceTimer.Start();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索失败", ex);
                MessageBox.Show("搜索失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSearchDebounceTimer()
        {
            _searchDebounceTimer = new Timer();
            _searchDebounceTimer.Interval = 300;
            _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;
        }

        private void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();

            try
            {
                ApplySearchFilter();
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                LogManager.Error("搜索失败", ex);
                MessageBox.Show("搜索失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplySearchFilter()
        {
            string searchTerm = textBoxSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredMaterialList = new List<MaterialDto>(materialList);
                return;
            }

            // 使用DTO的属性进行客户端筛选
            filteredMaterialList = materialList.Where(m =>
                (m.MaterialCode ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (m.MaterialName ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (m.MaterialType ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (m.Specification ?? string.Empty).IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_searchDebounceTimer != null)
            {
                _searchDebounceTimer.Stop();
                _searchDebounceTimer.Dispose();
                _searchDebounceTimer = null;
            }

            base.OnFormClosed(e);
        }

        /// <summary>
        /// DataGridView选择变化事件
        /// </summary>
        private void dataGridViewMaterials_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewMaterials.CurrentRow != null &&
                    dataGridViewMaterials.CurrentRow.DataBoundItem != null)
                {
                    // 将选中项转为DTO
                    var material = dataGridViewMaterials.CurrentRow.DataBoundItem as MaterialDto;
                    if (material != null)
                    {
                        ShowMaterialDetails(material);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("显示物料详情失败", ex);
            }
        }

        /// <summary>
        /// 显示物料详情
        /// </summary>
        private void ShowMaterialDetails(MaterialDto material)
        {
            if (material == null)
            {
                ClearDetailsPanel();
                return;
            }
            currentMaterial = material;

            // 旧版详情控件（如果仍存在）同步填充，避免未来代码依赖
            textBoxMaterialCode.Text = material.MaterialCode;
            textBoxMaterialName.Text = material.MaterialName;
            textBoxMaterialType.Text = material.MaterialType;
            textBoxUnit.Text = material.Unit;
            textBoxSpecification.Text = material.Specification;
            textBoxSupplier.Text = material.Supplier;
            textBoxPrice.Text = material.Price.ToString("F2");
            textBoxRemark.Text = material.Remark;

            UpdateDetailsPanel();
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 打开新增窗体
                using (var form = new MaterialEditForm(null))
                {
                    // 如果用户在编辑窗体点击了“保存”
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // 获取新窗体返回的、已填充好的DTO
                        var newMaterialDto = form.MaterialData;

                        // 调用BLL进行添加
                        if (_materialBLL.AddMaterial(newMaterialDto))
                        {
                            MessageBox.Show("物料添加成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogManager.Info(string.Format("添加物料：{0}", newMaterialDto.MaterialName));
                            // 重新加载数据并刷新界面
                            LoadMaterialData();
                            RefreshDataGridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加物料操作失败", ex);
                MessageBox.Show("添加物料失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑按钮点击事件
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (currentMaterial == null)
            {
                MessageBox.Show("请先选择要编辑的物料！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 将当前选中的物料DTO传递给编辑窗体
                using (var form = new MaterialEditForm(currentMaterial))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // 获取编辑后的DTO
                        var editedMaterialDto = form.MaterialData;

                        // 调用BLL进行更新
                        if (_materialBLL.UpdateMaterial(editedMaterialDto))
                        {
                            MessageBox.Show("物料编辑成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogManager.Info(string.Format("编辑物料：{0}", editedMaterialDto.MaterialName));
                            LoadMaterialData();
                            RefreshDataGridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("编辑物料操作失败", ex);
                MessageBox.Show("编辑物料失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentMaterial == null)
            {
                MessageBox.Show("请先选择要删除的物料！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var confirmResult = MessageBox.Show(string.Format("您确定要删除物料 [{0}] 吗？", currentMaterial.MaterialName),
                                                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // 调用BLL进行删除
                    if (_materialBLL.DeleteMaterial(currentMaterial.Id))
                    {
                        MessageBox.Show("物料删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogManager.Info(string.Format("删除物料 ID: {0}, 名称: {1}", currentMaterial.Id, currentMaterial.MaterialName));
                        LoadMaterialData();
                        RefreshDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("删除失败，可能该物料已被使用或不存在。", "删除失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除物料操作失败", ex);
                MessageBox.Show("删除物料失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            textBoxSearch.Clear();
            LoadMaterialData();
            RefreshDataGridView();
            // 体验优化：刷新不弹窗（避免“操作打断感”）
        }

        // --- 以下是辅助方法，基本不变或微调 ---

        /// <summary>
        /// 根据ID选中DataGridView中的行
        /// </summary>
        private void SelectMaterialById(int materialId)
        {
            for (int i = 0; i < dataGridViewMaterials.Rows.Count; i++)
            {
                var material = dataGridViewMaterials.Rows[i].DataBoundItem as MaterialDto;
                if (material != null && material.Id == materialId)
                {
                    dataGridViewMaterials.ClearSelection();
                    dataGridViewMaterials.Rows[i].Selected = true;
                    dataGridViewMaterials.CurrentCell = dataGridViewMaterials.Rows[i].Cells[0];
                    return;
                }
            }
        }

        /// <summary>
        /// 清空详情面板
        /// </summary>
        private void ClearDetailsPanel()
        {
            try
            {
                currentMaterial = null;

                // 旧版详情控件
                if (textBoxMaterialCode != null) textBoxMaterialCode.Text = string.Empty;
                if (textBoxMaterialName != null) textBoxMaterialName.Text = string.Empty;
                if (textBoxMaterialType != null) textBoxMaterialType.Text = string.Empty;
                if (textBoxUnit != null) textBoxUnit.Text = string.Empty;
                if (textBoxSpecification != null) textBoxSpecification.Text = string.Empty;
                if (textBoxSupplier != null) textBoxSupplier.Text = string.Empty;
                if (textBoxPrice != null) textBoxPrice.Text = string.Empty;
                if (textBoxRemark != null) textBoxRemark.Text = string.Empty;

                // 新版详情卡
                if (_lblSummaryCode != null) _lblSummaryCode.Text = "未选择物料";
                if (_lblSummaryName != null) _lblSummaryName.Text = "请选择左侧物料查看详情";
                ApplyTypeBadge(string.Empty);

                if (_valMaterialType != null) _valMaterialType.Text = "—";
                if (_valUnit != null) _valUnit.Text = "—";
                if (_valSpec != null) _valSpec.Text = "—";
                if (_valSupplier != null) _valSupplier.Text = "—";
                if (_valPrice != null) _valPrice.Text = "—";

                if (_txtRemark != null) _txtRemark.Text = string.Empty;

                if (_btnEditQuick != null) _btnEditQuick.Enabled = false;
                if (_btnCopyCode != null) _btnCopyCode.Enabled = false;
            }
            catch
            {
                // ignore
            }
        }

        private void UpdateDetailsPanel()
        {
            try
            {
                if (_detailsLayout == null) return;

                var m = currentMaterial;
                if (m == null)
                {
                    ClearDetailsPanel();
                    return;
                }

                var code = m.MaterialCode ?? string.Empty;
                var name = m.MaterialName ?? string.Empty;
                var type = m.MaterialType ?? string.Empty;
                var unit = m.Unit ?? string.Empty;
                var spec = m.Specification ?? string.Empty;
                var supplier = m.Supplier ?? string.Empty;

                if (_lblSummaryCode != null)
                {
                    _lblSummaryCode.Text = string.IsNullOrWhiteSpace(code) ? "未选择物料" : code;
                }

                if (_lblSummaryName != null)
                {
                    _lblSummaryName.Text = string.IsNullOrWhiteSpace(name) ? "—" : name;
                }

                ApplyTypeBadge(type);

                if (_valMaterialType != null) _valMaterialType.Text = string.IsNullOrWhiteSpace(type) ? "—" : type;
                if (_valUnit != null) _valUnit.Text = string.IsNullOrWhiteSpace(unit) ? "—" : unit;
                if (_valSpec != null) _valSpec.Text = string.IsNullOrWhiteSpace(spec) ? "—" : spec;
                if (_valSupplier != null) _valSupplier.Text = string.IsNullOrWhiteSpace(supplier) ? "—" : supplier;

                if (_valPrice != null)
                {
                    try
                    {
                        _valPrice.Text = m.Price.ToString("0.##");
                    }
                    catch
                    {
                        _valPrice.Text = "—";
                    }
                }

                if (_txtRemark != null)
                {
                    _txtRemark.Text = m.Remark ?? string.Empty;
                }

                var enabled = !string.IsNullOrWhiteSpace(code) || m.Id > 0;
                if (_btnEditQuick != null) _btnEditQuick.Enabled = enabled;
                if (_btnCopyCode != null) _btnCopyCode.Enabled = !string.IsNullOrWhiteSpace(code);
            }
            catch
            {
                // ignore
            }
        }

        private void ApplyTypeBadge(string materialType)
        {
            try
            {
                if (_panelTypeBadge == null || _lblTypeBadgeText == null) return;

                var type = materialType ?? string.Empty;
                var text = string.IsNullOrWhiteSpace(type) ? "—" : type;

                var fore = LeagueColors.TextPrimary;
                var back = Color.FromArgb(35, 35, 35);

                // 简单映射（避免过度“业务猜测”）：按关键词给一个大致的 LoL 色板
                if (type.IndexOf("原", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    fore = LeagueColors.RiotGoldHover;
                    back = Color.FromArgb(45, 40, 28);
                }
                else if (type.IndexOf("电子", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    fore = LeagueColors.SpecialCyan;
                    back = Color.FromArgb(18, 44, 48);
                }
                else if (type.IndexOf("成品", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    fore = LeagueColors.SuccessGreen;
                    back = Color.FromArgb(18, 46, 32);
                }

                _panelTypeBadge.BackColor = back;
                _lblTypeBadgeText.ForeColor = fore;
                _lblTypeBadgeText.Text = text;
            }
            catch
            {
                // ignore
            }
        }

        private void BuildLolLayoutA()
        {
            if (_lolLayoutABuilt) return;
            _lolLayoutABuilt = true;

            if (panelContent == null || dataGridViewMaterials == null) return;

            try
            {
                panelContent.SuspendLayout();

                // 防重复
                foreach (Control c in panelContent.Controls)
                {
                    if (c is SplitContainer)
                    {
                        _splitMain = c as SplitContainer;
                        return;
                    }
                }

                panelContent.Controls.Clear();

                _splitMain = new SplitContainer();
                _splitMain.Name = "splitMain";
                _splitMain.Dock = DockStyle.Fill;
                _splitMain.Orientation = Orientation.Vertical;
                _splitMain.SplitterWidth = 6;
                _splitMain.Panel1MinSize = 520;
                _splitMain.Panel2MinSize = 320;
                _splitMain.FixedPanel = FixedPanel.Panel2;
                _splitMain.BackColor = LeagueColors.SeparatorColor;

                // 左：列表
                dataGridViewMaterials.Dock = DockStyle.Fill;
                _splitMain.Panel1.Padding = new Padding(0, 0, 8, 0);
                _splitMain.Panel1.Controls.Add(dataGridViewMaterials);

                // 右：详情栏
                _splitMain.Panel2.Padding = new Padding(12);

                var detailsHost = new Panel();
                detailsHost.Dock = DockStyle.Fill;
                detailsHost.BackColor = Color.Transparent;
                _splitMain.Panel2.Controls.Add(detailsHost);

                _detailsLayout = new TableLayoutPanel();
                _detailsLayout.Dock = DockStyle.Fill;
                _detailsLayout.ColumnCount = 1;
                _detailsLayout.RowCount = 4;
                _detailsLayout.BackColor = Color.Transparent;
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F)); // 概览
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // 信息
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F)); // 备注
                _detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F)); // 操作
                detailsHost.Controls.Add(_detailsLayout);

                var cardSummary = CreateLolCardPanel();
                var cardInfo = CreateLolCardPanel();
                var cardRemark = CreateLolCardPanel();
                var cardActions = CreateLolCardPanel();

                _detailsLayout.Controls.Add(cardSummary, 0, 0);
                _detailsLayout.Controls.Add(cardInfo, 0, 1);
                _detailsLayout.Controls.Add(cardRemark, 0, 2);
                _detailsLayout.Controls.Add(cardActions, 0, 3);

                BuildSummaryCard(cardSummary);
                BuildInfoCard(cardInfo);
                BuildRemarkCard(cardRemark);
                BuildActionsCard(cardActions);

                panelContent.Controls.Add(_splitMain);

                // 初始空态
                ClearDetailsPanel();

                // 初始分隔：右侧详情栏保持稳定宽度（更像 LoL 客户端侧边栏）
                UpdateSplitDistance();
                panelContent.SizeChanged += (s, e) => UpdateSplitDistance();
            }
            catch
            {
                // ignore
            }
            finally
            {
                try { panelContent.ResumeLayout(true); } catch { }
            }
        }

        private static Panel CreateLolCardPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = LeagueColors.DarkSurface;
            panel.Padding = new Padding(12);
            panel.Margin = new Padding(0);
            return panel;
        }

        private void UpdateSplitDistance()
        {
            try
            {
                if (_splitMain == null) return;

                var detailsWidth = 420;
                var desired = Math.Max(_splitMain.Panel1MinSize, _splitMain.Width - detailsWidth);

                var maxAllowed = _splitMain.Width - _splitMain.Panel2MinSize;
                if (desired > maxAllowed) desired = maxAllowed;
                if (desired < _splitMain.Panel1MinSize) desired = _splitMain.Panel1MinSize;
                if (desired < 0) desired = 0;

                _splitMain.SplitterDistance = desired;
            }
            catch
            {
                // ignore
            }
        }

        private void BuildSummaryCard(Panel host)
        {
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 2;
            layout.RowCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "物料概览";
            lblTitle.AutoEllipsis = true;
            layout.Controls.Add(lblTitle, 0, 0);
            layout.SetColumnSpan(lblTitle, 2);

            _lblSummaryCode = new Label();
            _lblSummaryCode.Dock = DockStyle.Fill;
            _lblSummaryCode.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummaryCode.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
            _lblSummaryCode.ForeColor = LeagueColors.TextHighlight;
            _lblSummaryCode.Text = "未选择物料";
            _lblSummaryCode.AutoEllipsis = true;
            layout.Controls.Add(_lblSummaryCode, 0, 1);

            _panelTypeBadge = new Panel();
            _panelTypeBadge.Dock = DockStyle.Fill;
            _panelTypeBadge.BackColor = Color.FromArgb(35, 35, 35);
            _panelTypeBadge.Padding = new Padding(6, 2, 6, 2);
            layout.Controls.Add(_panelTypeBadge, 1, 1);

            _lblTypeBadgeText = new Label();
            _lblTypeBadgeText.Dock = DockStyle.Fill;
            _lblTypeBadgeText.TextAlign = ContentAlignment.MiddleCenter;
            _lblTypeBadgeText.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            _lblTypeBadgeText.Text = "—";
            _lblTypeBadgeText.ForeColor = LeagueColors.TextDisabled;
            _lblTypeBadgeText.BackColor = Color.Transparent;
            _panelTypeBadge.Controls.Add(_lblTypeBadgeText);

            _lblSummaryName = new Label();
            _lblSummaryName.Dock = DockStyle.Fill;
            _lblSummaryName.TextAlign = ContentAlignment.MiddleLeft;
            _lblSummaryName.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            _lblSummaryName.ForeColor = LeagueColors.TextSecondary;
            _lblSummaryName.Text = "请选择左侧物料查看详情";
            _lblSummaryName.AutoEllipsis = true;
            layout.Controls.Add(_lblSummaryName, 0, 2);
            layout.SetColumnSpan(_lblSummaryName, 2);
        }

        private void BuildInfoCard(Panel host)
        {
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "关键信息";
            layout.Controls.Add(lblTitle, 0, 0);

            var info = new TableLayoutPanel();
            info.Dock = DockStyle.Fill;
            info.BackColor = Color.Transparent;
            info.ColumnCount = 2;
            info.RowCount = 5;
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86F));
            info.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            for (int i = 0; i < 5; i++)
            {
                info.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            }
            layout.Controls.Add(info, 0, 1);

            AddInfoRow(info, 0, "类型", out _valMaterialType);
            AddInfoRow(info, 1, "单位", out _valUnit);
            AddInfoRow(info, 2, "规格", out _valSpec);
            AddInfoRow(info, 3, "供应商", out _valSupplier);
            AddInfoRow(info, 4, "价格", out _valPrice);
        }

        private void BuildRemarkCard(Panel host)
        {
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "备注";
            layout.Controls.Add(lblTitle, 0, 0);

            _txtRemark = textBoxRemark;
            if (_txtRemark == null)
            {
                _txtRemark = new TextBox();
                _txtRemark.Multiline = true;
                _txtRemark.ScrollBars = ScrollBars.Vertical;
                _txtRemark.ReadOnly = true;
            }

            _txtRemark.Dock = DockStyle.Fill;
            _txtRemark.BorderStyle = BorderStyle.None;
            _txtRemark.BackColor = LeagueColors.InputBackground;
            _txtRemark.ForeColor = Color.FromArgb(241, 241, 241);
            layout.Controls.Add(_txtRemark, 0, 1);
        }

        private void BuildActionsCard(Panel host)
        {
            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.BackColor = Color.Transparent;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            host.Controls.Add(layout);

            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
            lblTitle.ForeColor = LeagueColors.RiotGoldHover;
            lblTitle.Text = "快捷操作";
            layout.Controls.Add(lblTitle, 0, 0);

            var flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.FlowDirection = FlowDirection.RightToLeft;
            flow.WrapContents = false;
            flow.Padding = new Padding(0, 4, 0, 0);
            flow.BackColor = Color.Transparent;
            layout.Controls.Add(flow, 0, 1);

            _btnEditQuick = new LolActionButton();
            _btnEditQuick.Text = "编辑";
            _btnEditQuick.Width = 90;
            _btnEditQuick.Compact = true;
            _btnEditQuick.Enabled = false;
            _btnEditQuick.Click += (s, e) =>
            {
                try
                {
                    if (btnEdit != null)
                    {
                        btnEdit.PerformClick();
                    }
                }
                catch
                {
                    // ignore
                }
            };

            _btnCopyCode = new LolActionButton();
            _btnCopyCode.Text = "复制编码";
            _btnCopyCode.Width = 110;
            _btnCopyCode.Compact = true;
            _btnCopyCode.Enabled = false;
            _btnCopyCode.Click += (s, e) =>
            {
                try
                {
                    var code = currentMaterial != null ? currentMaterial.MaterialCode : null;
                    if (string.IsNullOrWhiteSpace(code)) return;
                    try { Clipboard.SetText(code); } catch { }
                }
                catch
                {
                    // ignore
                }
            };

            flow.Controls.Add(_btnEditQuick);
            flow.Controls.Add(_btnCopyCode);
        }

        private static void AddInfoRow(TableLayoutPanel table, int rowIndex, string key, out Label valueLabel)
        {
            var k = new Label();
            k.Dock = DockStyle.Fill;
            k.TextAlign = ContentAlignment.MiddleLeft;
            k.Font = new Font("微软雅黑", 9F, FontStyle.Regular);
            k.ForeColor = LeagueColors.TextSecondary;
            k.Text = key;
            k.AutoEllipsis = true;
            table.Controls.Add(k, 0, rowIndex);

            valueLabel = new Label();
            valueLabel.Dock = DockStyle.Fill;
            valueLabel.TextAlign = ContentAlignment.MiddleLeft;
            valueLabel.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            valueLabel.ForeColor = LeagueColors.TextPrimary;
            valueLabel.Text = "—";
            valueLabel.AutoEllipsis = true;
            table.Controls.Add(valueLabel, 1, rowIndex);
        }
        private void SetupDataGridView()
        {
            dataGridViewMaterials.AutoGenerateColumns = false;
            dataGridViewMaterials.Columns.Clear();

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialCode",
                HeaderText = "物料编码",
                DataPropertyName = "MaterialCode",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialName",
                HeaderText = "物料名称",
                DataPropertyName = "MaterialName",
                Width = 200,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialType",
                HeaderText = "物料类型",
                DataPropertyName = "MaterialType",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Unit",
                HeaderText = "计量单位",
                DataPropertyName = "Unit",
                Width = 80,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Specification",
                HeaderText = "规格型号",
                DataPropertyName = "Specification",
                Width = 150,
                ReadOnly = true
            });

            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Supplier",
                HeaderText = "供应商",
                DataPropertyName = "Supplier",
                Width = 120,
                ReadOnly = true
            });

            // 设置样式：当启用 LoL 主题时，避免窗体代码把 DataGridView 强制刷回“白底蓝选中”
            dataGridViewMaterials.EnableHeadersVisualStyles = false;
            dataGridViewMaterials.ColumnHeadersHeight = 40;
            dataGridViewMaterials.DefaultCellStyle.Font = new Font("微软雅黑", 9F);

            if (UIThemeManager.CurrentTheme == UIThemeManager.ThemeType.Lol)
            {
                dataGridViewMaterials.BackgroundColor = LeagueColors.DarkSurface;
                // 注意：DataGridView.GridColor 不支持透明色（alpha < 255）
                dataGridViewMaterials.GridColor = LeagueColors.DarkBorder;

                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.BackColor = LeagueColors.DarkBackground;
                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.ForeColor = LeagueColors.TextHighlight;
                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.SelectionBackColor = LeagueColors.DarkBackground;
                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;
                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);

                dataGridViewMaterials.DefaultCellStyle.BackColor = LeagueColors.DarkSurface;
                dataGridViewMaterials.DefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
                dataGridViewMaterials.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
                dataGridViewMaterials.DefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;

                dataGridViewMaterials.AlternatingRowsDefaultCellStyle.BackColor = LeagueColors.DarkPanel;
                dataGridViewMaterials.AlternatingRowsDefaultCellStyle.ForeColor = LeagueColors.TextPrimary;
                dataGridViewMaterials.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 49, 33);
                dataGridViewMaterials.AlternatingRowsDefaultCellStyle.SelectionForeColor = LeagueColors.TextHighlight;
            }
            else
            {
                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridViewMaterials.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);

                dataGridViewMaterials.DefaultCellStyle.BackColor = Color.White;
                dataGridViewMaterials.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
                dataGridViewMaterials.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
                dataGridViewMaterials.DefaultCellStyle.SelectionForeColor = Color.White;

                dataGridViewMaterials.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
                dataGridViewMaterials.GridColor = Color.FromArgb(222, 226, 230);
            }
        }

        /// <summary>
        /// 显示物料编辑对话框
        /// </summary>
        private MaterialDto ShowMaterialEditDialog(MaterialDto material)
        {
            try
            {
                // 打开真实的物料编辑窗体
                using (var form = new MaterialEditForm(material))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        return form.MaterialData; // 返回编辑后的DTO
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("打开物料编辑窗体失败", ex);
                MessageBox.Show("打开编辑窗体失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
