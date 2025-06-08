using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.Models.System;

namespace MES.UI.Forms.Production
{
    /// <summary>
    /// 用户权限管理窗体 - 现代化界面设计
    /// 严格遵循C# 5.0语法和设计器模式约束
    /// </summary>
    public partial class UserPermissionManagementForm : Form
    {
        private List<UserInfo> userList;
        private List<UserInfo> filteredUserList;
        private List<RoleInfo> roleList;
        private UserInfo currentUser;

        public UserPermissionManagementForm()
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
                userList = new List<UserInfo>();
                filteredUserList = new List<UserInfo>();
                roleList = new List<RoleInfo>();
                currentUser = null;

                // 设置DataGridView
                SetupDataGridView();

                // 加载角色数据
                LoadRoleData();

                // 设置角色下拉框
                SetupRoleComboBox();

                // 加载示例数据
                LoadSampleData();

                // 刷新显示
                RefreshDataGridView();

                // 清空详情面板
                ClearDetailsPanel();

                LogManager.Info("用户权限管理窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("用户权限管理窗体初始化失败", ex);
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
            dataGridViewUsers.AutoGenerateColumns = false;
            dataGridViewUsers.Columns.Clear();

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserCode",
                HeaderText = "用户编码",
                DataPropertyName = "UserCode",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserName",
                HeaderText = "用户姓名",
                DataPropertyName = "UserName",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LoginName",
                HeaderText = "登录名",
                DataPropertyName = "LoginName",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Department",
                HeaderText = "部门",
                DataPropertyName = "Department",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Position",
                HeaderText = "职位",
                DataPropertyName = "Position",
                Width = 100,
                ReadOnly = true
            });

            dataGridViewUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RoleName",
                HeaderText = "角色",
                DataPropertyName = "RoleName",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewUsers.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Status",
                HeaderText = "状态",
                DataPropertyName = "Status",
                Width = 80,
                ReadOnly = true
            });

            // 设置样式
            dataGridViewUsers.EnableHeadersVisualStyles = false;
            dataGridViewUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewUsers.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            dataGridViewUsers.ColumnHeadersHeight = 40;

            dataGridViewUsers.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
            dataGridViewUsers.DefaultCellStyle.BackColor = Color.White;
            dataGridViewUsers.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255);
            dataGridViewUsers.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridViewUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridViewUsers.GridColor = Color.FromArgb(222, 226, 230);
        }

        /// <summary>
        /// 加载角色数据
        /// </summary>
        private void LoadRoleData()
        {
            roleList.Clear();

            // 添加系统角色
            roleList.Add(new RoleInfo("ADMIN", "系统管理员", "拥有系统所有权限"));
            roleList.Add(new RoleInfo("MANAGER", "生产经理", "生产管理相关权限"));
            roleList.Add(new RoleInfo("SUPERVISOR", "车间主管", "车间管理相关权限"));
            roleList.Add(new RoleInfo("OPERATOR", "操作员", "基本操作权限"));
            roleList.Add(new RoleInfo("QUALITY", "质检员", "质量检验相关权限"));
            roleList.Add(new RoleInfo("MATERIAL", "物料员", "物料管理相关权限"));

            // 设置角色ID
            for (int i = 0; i < roleList.Count; i++)
            {
                roleList[i].Id = i + 1;
            }
        }

        /// <summary>
        /// 设置角色下拉框
        /// </summary>
        private void SetupRoleComboBox()
        {
            comboBoxRole.DisplayMember = "RoleName";
            comboBoxRole.ValueMember = "Id";
            comboBoxRole.DataSource = roleList;
            comboBoxRole.SelectedIndex = -1;
        }

        /// <summary>
        /// 加载示例数据
        /// </summary>
        private void LoadSampleData()
        {
            userList.Clear();

            // 添加示例用户数据
            userList.Add(new UserInfo
            {
                Id = 1,
                UserCode = "U001",
                UserName = "张三",
                LoginName = "zhangsan",
                Password = "******",
                Department = "生产部",
                Position = "生产经理",
                Email = "zhangsan@company.com",
                Phone = "13800138001",
                Status = true,
                LastLoginTime = DateTime.Now.AddHours(-2),
                RoleId = 2,
                RoleName = "生产经理"
            });

            userList.Add(new UserInfo
            {
                Id = 2,
                UserCode = "U002",
                UserName = "李四",
                LoginName = "lisi",
                Password = "******",
                Department = "车间一",
                Position = "车间主管",
                Email = "lisi@company.com",
                Phone = "13800138002",
                Status = true,
                LastLoginTime = DateTime.Now.AddMinutes(-30),
                RoleId = 3,
                RoleName = "车间主管"
            });

            userList.Add(new UserInfo
            {
                Id = 3,
                UserCode = "U003",
                UserName = "王五",
                LoginName = "wangwu",
                Password = "******",
                Department = "质检部",
                Position = "质检员",
                Email = "wangwu@company.com",
                Phone = "13800138003",
                Status = true,
                LastLoginTime = DateTime.Now.AddDays(-1),
                RoleId = 5,
                RoleName = "质检员"
            });

            userList.Add(new UserInfo
            {
                Id = 4,
                UserCode = "U004",
                UserName = "赵六",
                LoginName = "zhaoliu",
                Password = "******",
                Department = "物料部",
                Position = "物料员",
                Email = "zhaoliu@company.com",
                Phone = "13800138004",
                Status = false,
                LastLoginTime = DateTime.Now.AddDays(-7),
                RoleId = 6,
                RoleName = "物料员"
            });

            userList.Add(new UserInfo
            {
                Id = 5,
                UserCode = "U005",
                UserName = "孙七",
                LoginName = "sunqi",
                Password = "******",
                Department = "车间二",
                Position = "操作员",
                Email = "sunqi@company.com",
                Phone = "13800138005",
                Status = true,
                LastLoginTime = DateTime.Now.AddHours(-1),
                RoleId = 4,
                RoleName = "操作员"
            });

            userList.Add(new UserInfo
            {
                Id = 6,
                UserCode = "ADMIN",
                UserName = "系统管理员",
                LoginName = "admin",
                Password = "******",
                Department = "信息部",
                Position = "系统管理员",
                Email = "admin@company.com",
                Phone = "13800138000",
                Status = true,
                LastLoginTime = DateTime.Now.AddMinutes(-10),
                RoleId = 1,
                RoleName = "系统管理员"
            });

            // 复制到过滤列表
            filteredUserList = new List<UserInfo>(userList);
        }

        /// <summary>
        /// 刷新DataGridView显示
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                dataGridViewUsers.DataSource = null;
                dataGridViewUsers.DataSource = filteredUserList;

                // 如果有数据，选中第一行
                if (filteredUserList.Count > 0)
                {
                    dataGridViewUsers.Rows[0].Selected = true;
                    ShowUserDetails(filteredUserList[0]);
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
                    // 显示所有用户
                    filteredUserList = new List<UserInfo>(userList);
                }
                else
                {
                    // 根据用户编码、姓名、登录名、部门进行搜索
                    filteredUserList = userList.Where(u =>
                        u.UserCode.ToLower().Contains(searchTerm) ||
                        u.UserName.ToLower().Contains(searchTerm) ||
                        u.LoginName.ToLower().Contains(searchTerm) ||
                        u.Department.ToLower().Contains(searchTerm) ||
                        u.RoleName.ToLower().Contains(searchTerm)
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
        private void dataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewUsers.CurrentRow != null &&
                    dataGridViewUsers.CurrentRow.DataBoundItem != null)
                {
                    var user = dataGridViewUsers.CurrentRow.DataBoundItem as UserInfo;
                    if (user != null)
                    {
                        ShowUserDetails(user);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("显示用户详情失败", ex);
            }
        }

        /// <summary>
        /// 显示用户详情
        /// </summary>
        private void ShowUserDetails(UserInfo user)
        {
            if (user == null)
            {
                ClearDetailsPanel();
                return;
            }

            currentUser = user;

            // 填充基本信息
            textBoxUserCode.Text = user.UserCode;
            textBoxUserName.Text = user.UserName;
            textBoxLoginName.Text = user.LoginName;
            textBoxDepartment.Text = user.Department;
            textBoxPosition.Text = user.Position;

            // 填充联系信息
            textBoxEmail.Text = user.Email;
            textBoxPhone.Text = user.Phone;

            // 填充状态信息
            checkBoxStatus.Checked = user.Status;

            // 填充角色信息
            comboBoxRole.SelectedValue = user.RoleId;

            // 填充时间信息
            if (user.LastLoginTime.HasValue)
            {
                labelLastLoginTime.Text = "最后登录：" + user.LastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                labelLastLoginTime.Text = "最后登录：从未登录";
            }

            if (user.CreateTime != DateTime.MinValue)
            {
                labelCreateTime.Text = "创建时间：" + user.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
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
            currentUser = null;

            textBoxUserCode.Text = string.Empty;
            textBoxUserName.Text = string.Empty;
            textBoxLoginName.Text = string.Empty;
            textBoxDepartment.Text = string.Empty;
            textBoxPosition.Text = string.Empty;
            textBoxEmail.Text = string.Empty;
            textBoxPhone.Text = string.Empty;

            checkBoxStatus.Checked = false;
            comboBoxRole.SelectedIndex = -1;

            labelLastLoginTime.Text = "最后登录：";
            labelCreateTime.Text = "创建时间：";
        }

        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建新用户对话框
                var result = ShowUserEditDialog(null);
                if (result != null)
                {
                    // 生成新ID
                    result.Id = userList.Count > 0 ? userList.Max(u => u.Id) + 1 : 1;
                    result.CreateTime = DateTime.Now;
                    result.UpdateTime = DateTime.Now;

                    // 添加到列表
                    userList.Add(result);

                    // 刷新显示
                    RefreshDataGridView();

                    // 选中新添加的用户
                    SelectUserById(result.Id);

                    MessageBox.Show("用户添加成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("添加用户：{0} - {1}", result.UserCode, result.UserName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加用户失败", ex);
                MessageBox.Show("添加用户失败：" + ex.Message, "错误",
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
                if (currentUser == null)
                {
                    MessageBox.Show("请先选择要编辑的用户！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 创建编辑对话框
                var result = ShowUserEditDialog(currentUser);
                if (result != null)
                {
                    // 更新用户信息
                    var originalUser = userList.FirstOrDefault(u => u.Id == currentUser.Id);
                    if (originalUser != null)
                    {
                        originalUser.UserCode = result.UserCode;
                        originalUser.UserName = result.UserName;
                        originalUser.LoginName = result.LoginName;
                        originalUser.Department = result.Department;
                        originalUser.Position = result.Position;
                        originalUser.Email = result.Email;
                        originalUser.Phone = result.Phone;
                        originalUser.Status = result.Status;
                        originalUser.RoleId = result.RoleId;
                        originalUser.RoleName = result.RoleName;
                        originalUser.UpdateTime = DateTime.Now;

                        // 刷新显示
                        RefreshDataGridView();

                        // 重新选中编辑的用户
                        SelectUserById(originalUser.Id);

                        MessageBox.Show("用户编辑成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogManager.Info(string.Format("编辑用户：{0} - {1}",
                            originalUser.UserCode, originalUser.UserName));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("编辑用户失败", ex);
                MessageBox.Show("编辑用户失败：" + ex.Message, "错误",
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
                if (currentUser == null)
                {
                    MessageBox.Show("请先选择要删除的用户！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 系统管理员不能删除
                if (currentUser.UserCode == "ADMIN")
                {
                    MessageBox.Show("系统管理员账户不能删除！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    string.Format("确认要删除用户 [{0} - {1}] 吗？\n\n此操作不可撤销！",
                        currentUser.UserCode, currentUser.UserName),
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // 从列表中移除
                    userList.RemoveAll(u => u.Id == currentUser.Id);

                    // 刷新显示
                    RefreshDataGridView();

                    MessageBox.Show("用户删除成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LogManager.Info(string.Format("删除用户：{0} - {1}",
                        currentUser.UserCode, currentUser.UserName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("删除用户失败", ex);
                MessageBox.Show("删除用户失败：" + ex.Message, "错误",
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
                LoadRoleData();
                LoadSampleData();
                RefreshDataGridView();

                MessageBox.Show("数据刷新成功！", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info("用户权限数据已刷新");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新数据失败", ex);
                MessageBox.Show("刷新数据失败：" + ex.Message, "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据ID选中用户
        /// </summary>
        private void SelectUserById(int userId)
        {
            try
            {
                for (int i = 0; i < dataGridViewUsers.Rows.Count; i++)
                {
                    var user = dataGridViewUsers.Rows[i].DataBoundItem as UserInfo;
                    if (user != null && user.Id == userId)
                    {
                        dataGridViewUsers.ClearSelection();
                        dataGridViewUsers.Rows[i].Selected = true;
                        dataGridViewUsers.CurrentCell = dataGridViewUsers.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("选中用户失败", ex);
            }
        }

        /// <summary>
        /// 显示用户编辑对话框
        /// </summary>
        private UserInfo ShowUserEditDialog(UserInfo user)
        {
            // 这里应该打开一个用户编辑对话框
            // 为了演示，我们使用简单的输入框
            string title = user == null ? "新增用户" : "编辑用户";

            // 简化的编辑逻辑，实际应该使用专门的编辑窗体
            var editUser = user != null ? user.Clone() : new UserInfo();

            // 这里可以实现一个简单的编辑对话框
            // 或者调用专门的UserEditForm

            // 暂时返回示例数据用于演示
            if (user == null)
            {
                // 新增用户的示例
                return new UserInfo
                {
                    UserCode = "U" + (userList.Count + 1).ToString("000"),
                    UserName = "新用户",
                    LoginName = "newuser" + (userList.Count + 1),
                    Department = "待分配",
                    Position = "待分配",
                    Email = "newuser@company.com",
                    Phone = "138000000" + (userList.Count + 1).ToString("00"),
                    Status = true,
                    RoleId = 4,
                    RoleName = "操作员"
                };
            }
            else
            {
                // 编辑现有用户
                editUser.UserName = editUser.UserName + " (已编辑)";
                return editUser;
            }
        }
    }
}