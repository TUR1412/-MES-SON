using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.BLL.System;
using MES.Models.System;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.UI.Forms.SystemManagement
{
    /// <summary>
    /// 角色管理窗体
    /// 提供角色的增删改查、权限设置等功能
    /// </summary>
    public partial class RoleManagementForm : Form
    {
        private readonly IRoleBLL _roleBLL;
        private List<RoleInfo> _currentRoles;
        private RoleInfo _selectedRole;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleManagementForm()
        {
            InitializeComponent();
            _roleBLL = new RoleBLL();
            _currentRoles = new List<RoleInfo>();
            InitializeForm();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // 设置窗体属性
                this.Text = "角色管理";
                this.Size = new Size(1000, 700);
                this.StartPosition = FormStartPosition.CenterParent;
                this.MinimumSize = new Size(800, 600);

                // 初始化控件
                InitializeControls();
                
                // 加载数据
                LoadRoles();
                
                // 绑定事件
                BindEvents();
                
                LogManager.Info("角色管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("角色管理窗体初始化失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("窗体初始化失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitializeControls()
        {
            // 创建主面板
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            // 左侧面板 - 角色列表
            var leftPanel = CreateRoleListPanel();
            mainPanel.Controls.Add(leftPanel, 0, 0);

            // 右侧面板 - 角色详情
            var rightPanel = CreateRoleDetailPanel();
            mainPanel.Controls.Add(rightPanel, 1, 0);

            this.Controls.Add(mainPanel);
        }

        /// <summary>
        /// 创建角色列表面板
        /// </summary>
        private Panel CreateRoleListPanel()
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            // 工具栏
            var toolbar = new ToolStrip { Dock = DockStyle.Top };
            
            var btnAdd = new ToolStripButton("新增", null, BtnAdd_Click) { Name = "btnAdd" };
            var btnEdit = new ToolStripButton("编辑", null, BtnEdit_Click) { Name = "btnEdit" };
            var btnDelete = new ToolStripButton("删除", null, BtnDelete_Click) { Name = "btnDelete" };
            var btnRefresh = new ToolStripButton("刷新", null, BtnRefresh_Click) { Name = "btnRefresh" };
            
            toolbar.Items.AddRange(new ToolStripItem[] { btnAdd, btnEdit, btnDelete, new ToolStripSeparator(), btnRefresh });

            // 搜索面板
            var searchPanel = new Panel { Height = 35, Dock = DockStyle.Top };
            var txtSearch = new TextBox
            {
                Name = "txtSearch",
                Text = "输入角色编码或名称搜索...",
                ForeColor = Color.Gray,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };
            var btnSearch = new Button 
            { 
                Text = "搜索", 
                Width = 60, 
                Dock = DockStyle.Right,
                Name = "btnSearch"
            };
            btnSearch.Click += BtnSearch_Click;
            
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            // 角色列表
            var dgvRoles = new DataGridView
            {
                Name = "dgvRoles",
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false
            };

            // 配置列
            dgvRoles.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "RoleCode", HeaderText = "角色编码", DataPropertyName = "RoleCode", Width = 120 },
                new DataGridViewTextBoxColumn { Name = "RoleName", HeaderText = "角色名称", DataPropertyName = "RoleName", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "描述", DataPropertyName = "Description", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill },
                new DataGridViewTextBoxColumn { Name = "StatusText", HeaderText = "状态", DataPropertyName = "StatusText", Width = 80 },
                new DataGridViewTextBoxColumn { Name = "CreateTime", HeaderText = "创建时间", DataPropertyName = "CreateTime", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } }
            });

            dgvRoles.SelectionChanged += DgvRoles_SelectionChanged;

            panel.Controls.Add(dgvRoles);
            panel.Controls.Add(searchPanel);
            panel.Controls.Add(toolbar);

            return panel;
        }

        /// <summary>
        /// 创建角色详情面板
        /// </summary>
        private Panel CreateRoleDetailPanel()
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            var groupBox = new GroupBox 
            { 
                Text = "角色详情", 
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            var tableLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                AutoSize = true
            };

            // 配置列样式
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // 添加控件
            var controls = new (string label, Control control)[]
            {
                ("角色编码:", new TextBox { Name = "txtRoleCode", ReadOnly = true }),
                ("角色名称:", new TextBox { Name = "txtRoleName", ReadOnly = true }),
                ("描述:", new TextBox { Name = "txtDescription", ReadOnly = true, Multiline = true, Height = 60 }),
                ("状态:", new TextBox { Name = "txtStatus", ReadOnly = true }),
                ("排序号:", new TextBox { Name = "txtSortOrder", ReadOnly = true }),
                ("创建时间:", new TextBox { Name = "txtCreateTime", ReadOnly = true }),
                ("更新时间:", new TextBox { Name = "txtUpdateTime", ReadOnly = true }),
                ("权限设置:", CreatePermissionPanel())
            };

            for (int i = 0; i < controls.Length; i++)
            {
                var label = new Label 
                { 
                    Text = controls[i].label, 
                    TextAlign = ContentAlignment.MiddleRight,
                    Dock = DockStyle.Fill
                };
                
                tableLayout.Controls.Add(label, 0, i);
                tableLayout.Controls.Add(controls[i].control, 1, i);
                
                if (i < 7) // 前7行固定高度
                {
                    tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
                }
                else // 最后一行自适应
                {
                    tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                }
            }

            groupBox.Controls.Add(tableLayout);
            panel.Controls.Add(groupBox);

            return panel;
        }

        /// <summary>
        /// 创建权限设置面板
        /// </summary>
        private Panel CreatePermissionPanel()
        {
            var panel = new Panel { Dock = DockStyle.Fill };

            var btnSetPermissions = new Button
            {
                Text = "设置权限",
                Size = new Size(100, 30),
                Location = new Point(0, 0),
                Name = "btnSetPermissions"
            };
            btnSetPermissions.Click += BtnSetPermissions_Click;

            var txtPermissions = new TextBox
            {
                Name = "txtPermissions",
                ReadOnly = true,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(0, 35),
                Size = new Size(panel.Width, panel.Height - 35),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            panel.Controls.Add(btnSetPermissions);
            panel.Controls.Add(txtPermissions);

            return panel;
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            this.Load += RoleManagementForm_Load;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void RoleManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadRoles();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载角色数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("加载数据失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载角色数据
        /// </summary>
        private void LoadRoles()
        {
            try
            {
                _currentRoles = _roleBLL.GetAllRoles();
                
                var dgvRoles = this.Controls.Find("dgvRoles", true).FirstOrDefault() as DataGridView;
                if (dgvRoles != null)
                {
                    // 添加状态文本属性
                    var displayRoles = _currentRoles.Select(r => new
                    {
                        r.Id,
                        r.RoleCode,
                        r.RoleName,
                        r.Description,
                        StatusText = r.GetStatusText(),
                        r.CreateTime,
                        r.SortOrder,
                        r.Permissions
                    }).ToList();

                    dgvRoles.DataSource = displayRoles;
                }

                LogManager.Info(string.Format("成功加载 {0} 个角色", _currentRoles.Count));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("加载角色数据失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 角色选择变更事件
        /// </summary>
        private void DgvRoles_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var dgv = sender as DataGridView;
                if (dgv != null && dgv.CurrentRow != null && dgv.CurrentRow.DataBoundItem != null)
                {
                    var selectedData = dgv.CurrentRow.DataBoundItem;
                    var roleId = (int)selectedData.GetType().GetProperty("Id").GetValue(selectedData);
                    
                    _selectedRole = _currentRoles.FirstOrDefault(r => r.Id == roleId);
                    if (_selectedRole != null)
                    {
                        DisplayRoleDetails(_selectedRole);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("显示角色详情失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 显示角色详情
        /// </summary>
        private void DisplayRoleDetails(RoleInfo role)
        {
            try
            {
                var txtRoleCode = this.Controls.Find("txtRoleCode", true).FirstOrDefault() as TextBox;
                var txtRoleName = this.Controls.Find("txtRoleName", true).FirstOrDefault() as TextBox;
                var txtDescription = this.Controls.Find("txtDescription", true).FirstOrDefault() as TextBox;
                var txtStatus = this.Controls.Find("txtStatus", true).FirstOrDefault() as TextBox;
                var txtSortOrder = this.Controls.Find("txtSortOrder", true).FirstOrDefault() as TextBox;
                var txtCreateTime = this.Controls.Find("txtCreateTime", true).FirstOrDefault() as TextBox;
                var txtUpdateTime = this.Controls.Find("txtUpdateTime", true).FirstOrDefault() as TextBox;
                var txtPermissions = this.Controls.Find("txtPermissions", true).FirstOrDefault() as TextBox;

                if (txtRoleCode != null) txtRoleCode.Text = role.RoleCode;
                if (txtRoleName != null) txtRoleName.Text = role.RoleName;
                if (txtDescription != null) txtDescription.Text = role.Description;
                if (txtStatus != null) txtStatus.Text = role.GetStatusText();
                if (txtSortOrder != null) txtSortOrder.Text = role.SortOrder.ToString();
                if (txtCreateTime != null) txtCreateTime.Text = role.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (txtUpdateTime != null) txtUpdateTime.Text = role.UpdateTime.HasValue ? role.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
                if (txtPermissions != null) txtPermissions.Text = role.Permissions ?? "未设置权限";
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("显示角色详情失败：{0}", ex.Message), ex);
            }
        }

        #region 事件处理

        /// <summary>
        /// 新增角色
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: 打开角色编辑对话框
                MessageBox.Show("新增角色功能开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("新增角色失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("新增角色失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedRole == null)
                {
                    MessageBox.Show("请先选择要编辑的角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // TODO: 打开角色编辑对话框
                MessageBox.Show(string.Format("编辑角色 {0} 功能开发中...", _selectedRole.RoleName), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("编辑角色失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("编辑角色失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedRole == null)
                {
                    MessageBox.Show("请先选择要删除的角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(string.Format("确定要删除角色 '{0}' 吗？", _selectedRole.RoleName), "确认删除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (_roleBLL.DeleteRole(_selectedRole.Id))
                    {
                        MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadRoles();
                    }
                    else
                    {
                        MessageBox.Show("删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除角色失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("删除角色失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadRoles();
                MessageBox.Show("刷新完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("刷新数据失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("刷新失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 搜索角色
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var txtSearch = this.Controls.Find("txtSearch", true).FirstOrDefault() as TextBox;
                if (txtSearch != null && !string.IsNullOrEmpty(txtSearch.Text))
                {
                    var searchResults = _roleBLL.SearchRoles(txtSearch.Text);
                    
                    var dgvRoles = this.Controls.Find("dgvRoles", true).FirstOrDefault() as DataGridView;
                    if (dgvRoles != null)
                    {
                        var displayRoles = searchResults.Select(r => new
                        {
                            r.Id,
                            r.RoleCode,
                            r.RoleName,
                            r.Description,
                            StatusText = r.GetStatusText(),
                            r.CreateTime,
                            r.SortOrder,
                            r.Permissions
                        }).ToList();

                        dgvRoles.DataSource = displayRoles;
                    }
                }
                else
                {
                    LoadRoles();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索角色失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("搜索失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        private void BtnSetPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedRole == null)
                {
                    MessageBox.Show("请先选择要设置权限的角色", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // TODO: 打开权限设置对话框
                MessageBox.Show(string.Format("设置角色 {0} 权限功能开发中...", _selectedRole.RoleName), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设置权限失败：{0}", ex.Message), ex);
                MessageBox.Show(string.Format("设置权限失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
