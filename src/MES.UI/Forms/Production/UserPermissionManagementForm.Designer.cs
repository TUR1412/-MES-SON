namespace MES.UI.Forms.Production
{
    partial class UserPermissionManagementForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.panelDetailsContent = new System.Windows.Forms.Panel();
            this.groupBoxTimeInfo = new System.Windows.Forms.GroupBox();
            this.labelCreateTime = new System.Windows.Forms.Label();
            this.labelLastLoginTime = new System.Windows.Forms.Label();
            this.groupBoxRoleInfo = new System.Windows.Forms.GroupBox();
            this.comboBoxRole = new System.Windows.Forms.ComboBox();
            this.labelRole = new System.Windows.Forms.Label();
            this.checkBoxStatus = new System.Windows.Forms.CheckBox();
            this.groupBoxContactInfo = new System.Windows.Forms.GroupBox();
            this.textBoxPhone = new System.Windows.Forms.TextBox();
            this.labelPhone = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.groupBoxBasicInfo = new System.Windows.Forms.GroupBox();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.labelPosition = new System.Windows.Forms.Label();
            this.textBoxDepartment = new System.Windows.Forms.TextBox();
            this.labelDepartment = new System.Windows.Forms.Label();
            this.textBoxLoginName = new System.Windows.Forms.TextBox();
            this.labelLoginName = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.textBoxUserCode = new System.Windows.Forms.TextBox();
            this.labelUserCode = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.dataGridViewUsers = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            this.panelDetailsContent.SuspendLayout();
            this.groupBoxTimeInfo.SuspendLayout();
            this.groupBoxRoleInfo.SuspendLayout();
            this.groupBoxContactInfo.SuspendLayout();
            this.groupBoxBasicInfo.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).BeginInit();
            this.SuspendLayout();
            //
            // panelTop
            //
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1400, 60);
            this.panelTop.TabIndex = 0;
            //
            // labelTitle
            //
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(20, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(239, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "👥 用户权限管理";
            //
            // panelSearch
            //
            this.panelSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelSearch.Controls.Add(this.textBoxSearch);
            this.panelSearch.Controls.Add(this.labelSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 60);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1400, 50);
            this.panelSearch.TabIndex = 1;
            //
            // textBoxSearch
            //
            this.textBoxSearch.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxSearch.Location = new System.Drawing.Point(100, 12);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(300, 25);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            //
            // labelSearch
            //
            this.labelSearch.AutoSize = true;
            this.labelSearch.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelSearch.Location = new System.Drawing.Point(20, 15);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(79, 20);
            this.labelSearch.TabIndex = 0;
            this.labelSearch.Text = "🔍 搜索：";
            //
            // panelButtons
            //
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 110);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1400, 60);
            this.panelButtons.TabIndex = 2;
            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(350, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnDelete
            //
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(240, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "🗑️ 删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnEdit
            //
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(130, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "✏️ 编辑";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            //
            // btnAdd
            //
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(20, 15);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "➕ 新增";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // panelMain
            //
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.splitter1);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 170);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1400, 630);
            this.panelMain.TabIndex = 3;
            //
            // panelRight
            //
            this.panelRight.Controls.Add(this.groupBoxDetails);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(803, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(10);
            this.panelRight.Size = new System.Drawing.Size(597, 630);
            this.panelRight.TabIndex = 2;
            //
            // groupBoxDetails
            //
            this.groupBoxDetails.Controls.Add(this.panelDetailsContent);
            this.groupBoxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDetails.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxDetails.Location = new System.Drawing.Point(10, 10);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(577, 610);
            this.groupBoxDetails.TabIndex = 0;
            this.groupBoxDetails.TabStop = false;
            this.groupBoxDetails.Text = "📋 用户详细信息";
            //
            // panelDetailsContent
            //
            this.panelDetailsContent.AutoScroll = true;
            this.panelDetailsContent.Controls.Add(this.groupBoxTimeInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxRoleInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxContactInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxBasicInfo);
            this.panelDetailsContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailsContent.Location = new System.Drawing.Point(3, 21);
            this.panelDetailsContent.Name = "panelDetailsContent";
            this.panelDetailsContent.Padding = new System.Windows.Forms.Padding(10);
            this.panelDetailsContent.Size = new System.Drawing.Size(571, 586);
            this.panelDetailsContent.TabIndex = 0;
            //
            // groupBoxTimeInfo
            //
            this.groupBoxTimeInfo.Controls.Add(this.labelCreateTime);
            this.groupBoxTimeInfo.Controls.Add(this.labelLastLoginTime);
            this.groupBoxTimeInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTimeInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxTimeInfo.Location = new System.Drawing.Point(10, 370);
            this.groupBoxTimeInfo.Name = "groupBoxTimeInfo";
            this.groupBoxTimeInfo.Size = new System.Drawing.Size(551, 80);
            this.groupBoxTimeInfo.TabIndex = 3;
            this.groupBoxTimeInfo.TabStop = false;
            this.groupBoxTimeInfo.Text = "⏰ 时间信息";
            //
            // labelCreateTime
            //
            this.labelCreateTime.AutoSize = true;
            this.labelCreateTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelCreateTime.Location = new System.Drawing.Point(20, 50);
            this.labelCreateTime.Name = "labelCreateTime";
            this.labelCreateTime.Size = new System.Drawing.Size(68, 17);
            this.labelCreateTime.TabIndex = 1;
            this.labelCreateTime.Text = "创建时间：";
            //
            // labelLastLoginTime
            //
            this.labelLastLoginTime.AutoSize = true;
            this.labelLastLoginTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelLastLoginTime.Location = new System.Drawing.Point(20, 25);
            this.labelLastLoginTime.Name = "labelLastLoginTime";
            this.labelLastLoginTime.Size = new System.Drawing.Size(68, 17);
            this.labelLastLoginTime.TabIndex = 0;
            this.labelLastLoginTime.Text = "最后登录：";
            //
            // groupBoxRoleInfo
            //
            this.groupBoxRoleInfo.Controls.Add(this.comboBoxRole);
            this.groupBoxRoleInfo.Controls.Add(this.labelRole);
            this.groupBoxRoleInfo.Controls.Add(this.checkBoxStatus);
            this.groupBoxRoleInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRoleInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxRoleInfo.Location = new System.Drawing.Point(10, 280);
            this.groupBoxRoleInfo.Name = "groupBoxRoleInfo";
            this.groupBoxRoleInfo.Size = new System.Drawing.Size(551, 90);
            this.groupBoxRoleInfo.TabIndex = 2;
            this.groupBoxRoleInfo.TabStop = false;
            this.groupBoxRoleInfo.Text = "🔐 权限信息";
            //
            // comboBoxRole
            //
            this.comboBoxRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRole.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxRole.FormattingEnabled = true;
            this.comboBoxRole.Location = new System.Drawing.Point(100, 25);
            this.comboBoxRole.Name = "comboBoxRole";
            this.comboBoxRole.Size = new System.Drawing.Size(200, 25);
            this.comboBoxRole.TabIndex = 1;
            //
            // labelRole
            //
            this.labelRole.AutoSize = true;
            this.labelRole.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelRole.Location = new System.Drawing.Point(20, 28);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(44, 17);
            this.labelRole.TabIndex = 0;
            this.labelRole.Text = "角色：";
            //
            // checkBoxStatus
            //
            this.checkBoxStatus.AutoSize = true;
            this.checkBoxStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.checkBoxStatus.Location = new System.Drawing.Point(100, 60);
            this.checkBoxStatus.Name = "checkBoxStatus";
            this.checkBoxStatus.Size = new System.Drawing.Size(51, 21);
            this.checkBoxStatus.TabIndex = 2;
            this.checkBoxStatus.Text = "启用";
            this.checkBoxStatus.UseVisualStyleBackColor = true;
            //
            // groupBoxContactInfo
            //
            this.groupBoxContactInfo.Controls.Add(this.textBoxPhone);
            this.groupBoxContactInfo.Controls.Add(this.labelPhone);
            this.groupBoxContactInfo.Controls.Add(this.textBoxEmail);
            this.groupBoxContactInfo.Controls.Add(this.labelEmail);
            this.groupBoxContactInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxContactInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxContactInfo.Location = new System.Drawing.Point(10, 190);
            this.groupBoxContactInfo.Name = "groupBoxContactInfo";
            this.groupBoxContactInfo.Size = new System.Drawing.Size(551, 90);
            this.groupBoxContactInfo.TabIndex = 1;
            this.groupBoxContactInfo.TabStop = false;
            this.groupBoxContactInfo.Text = "📞 联系信息";
            //
            // textBoxPhone
            //
            this.textBoxPhone.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxPhone.Location = new System.Drawing.Point(100, 55);
            this.textBoxPhone.Name = "textBoxPhone";
            this.textBoxPhone.ReadOnly = true;
            this.textBoxPhone.Size = new System.Drawing.Size(200, 23);
            this.textBoxPhone.TabIndex = 3;
            //
            // labelPhone
            //
            this.labelPhone.AutoSize = true;
            this.labelPhone.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelPhone.Location = new System.Drawing.Point(20, 58);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(44, 17);
            this.labelPhone.TabIndex = 2;
            this.labelPhone.Text = "电话：";
            //
            // textBoxEmail
            //
            this.textBoxEmail.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxEmail.Location = new System.Drawing.Point(100, 25);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.ReadOnly = true;
            this.textBoxEmail.Size = new System.Drawing.Size(300, 23);
            this.textBoxEmail.TabIndex = 1;
            //
            // labelEmail
            //
            this.labelEmail.AutoSize = true;
            this.labelEmail.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelEmail.Location = new System.Drawing.Point(20, 28);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(44, 17);
            this.labelEmail.TabIndex = 0;
            this.labelEmail.Text = "邮箱：";
            //
            // groupBoxBasicInfo
            //
            this.groupBoxBasicInfo.Controls.Add(this.textBoxPosition);
            this.groupBoxBasicInfo.Controls.Add(this.labelPosition);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxDepartment);
            this.groupBoxBasicInfo.Controls.Add(this.labelDepartment);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxLoginName);
            this.groupBoxBasicInfo.Controls.Add(this.labelLoginName);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxUserName);
            this.groupBoxBasicInfo.Controls.Add(this.labelUserName);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxUserCode);
            this.groupBoxBasicInfo.Controls.Add(this.labelUserCode);
            this.groupBoxBasicInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasicInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxBasicInfo.Location = new System.Drawing.Point(10, 10);
            this.groupBoxBasicInfo.Name = "groupBoxBasicInfo";
            this.groupBoxBasicInfo.Size = new System.Drawing.Size(551, 180);
            this.groupBoxBasicInfo.TabIndex = 0;
            this.groupBoxBasicInfo.TabStop = false;
            this.groupBoxBasicInfo.Text = "📋 基本信息";
            //
            // textBoxPosition
            //
            this.textBoxPosition.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxPosition.Location = new System.Drawing.Point(100, 145);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.ReadOnly = true;
            this.textBoxPosition.Size = new System.Drawing.Size(200, 23);
            this.textBoxPosition.TabIndex = 9;
            //
            // labelPosition
            //
            this.labelPosition.AutoSize = true;
            this.labelPosition.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelPosition.Location = new System.Drawing.Point(20, 148);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(44, 17);
            this.labelPosition.TabIndex = 8;
            this.labelPosition.Text = "职位：";
            //
            // textBoxDepartment
            //
            this.textBoxDepartment.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxDepartment.Location = new System.Drawing.Point(100, 115);
            this.textBoxDepartment.Name = "textBoxDepartment";
            this.textBoxDepartment.ReadOnly = true;
            this.textBoxDepartment.Size = new System.Drawing.Size(200, 23);
            this.textBoxDepartment.TabIndex = 7;
            //
            // labelDepartment
            //
            this.labelDepartment.AutoSize = true;
            this.labelDepartment.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelDepartment.Location = new System.Drawing.Point(20, 118);
            this.labelDepartment.Name = "labelDepartment";
            this.labelDepartment.Size = new System.Drawing.Size(44, 17);
            this.labelDepartment.TabIndex = 6;
            this.labelDepartment.Text = "部门：";
            //
            // textBoxLoginName
            //
            this.textBoxLoginName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxLoginName.Location = new System.Drawing.Point(100, 85);
            this.textBoxLoginName.Name = "textBoxLoginName";
            this.textBoxLoginName.ReadOnly = true;
            this.textBoxLoginName.Size = new System.Drawing.Size(200, 23);
            this.textBoxLoginName.TabIndex = 5;
            //
            // labelLoginName
            //
            this.labelLoginName.AutoSize = true;
            this.labelLoginName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelLoginName.Location = new System.Drawing.Point(20, 88);
            this.labelLoginName.Name = "labelLoginName";
            this.labelLoginName.Size = new System.Drawing.Size(56, 17);
            this.labelLoginName.TabIndex = 4;
            this.labelLoginName.Text = "登录名：";
            //
            // textBoxUserName
            //
            this.textBoxUserName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxUserName.Location = new System.Drawing.Point(100, 55);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.ReadOnly = true;
            this.textBoxUserName.Size = new System.Drawing.Size(200, 23);
            this.textBoxUserName.TabIndex = 3;
            //
            // labelUserName
            //
            this.labelUserName.AutoSize = true;
            this.labelUserName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelUserName.Location = new System.Drawing.Point(20, 58);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(68, 17);
            this.labelUserName.TabIndex = 2;
            this.labelUserName.Text = "用户姓名：";
            //
            // textBoxUserCode
            //
            this.textBoxUserCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxUserCode.Location = new System.Drawing.Point(100, 25);
            this.textBoxUserCode.Name = "textBoxUserCode";
            this.textBoxUserCode.ReadOnly = true;
            this.textBoxUserCode.Size = new System.Drawing.Size(200, 23);
            this.textBoxUserCode.TabIndex = 1;
            //
            // labelUserCode
            //
            this.labelUserCode.AutoSize = true;
            this.labelUserCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelUserCode.Location = new System.Drawing.Point(20, 28);
            this.labelUserCode.Name = "labelUserCode";
            this.labelUserCode.Size = new System.Drawing.Size(68, 17);
            this.labelUserCode.TabIndex = 0;
            this.labelUserCode.Text = "用户编码：";
            //
            // splitter1
            //
            this.splitter1.Location = new System.Drawing.Point(800, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 630);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            //
            // panelLeft
            //
            this.panelLeft.Controls.Add(this.dataGridViewUsers);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelLeft.Size = new System.Drawing.Size(800, 630);
            this.panelLeft.TabIndex = 0;
            //
            // dataGridViewUsers
            //
            this.dataGridViewUsers.AllowUserToAddRows = false;
            this.dataGridViewUsers.AllowUserToDeleteRows = false;
            this.dataGridViewUsers.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUsers.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewUsers.MultiSelect = false;
            this.dataGridViewUsers.Name = "dataGridViewUsers";
            this.dataGridViewUsers.ReadOnly = true;
            this.dataGridViewUsers.RowHeadersVisible = false;
            this.dataGridViewUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUsers.Size = new System.Drawing.Size(780, 610);
            this.dataGridViewUsers.TabIndex = 0;
            this.dataGridViewUsers.SelectionChanged += new System.EventHandler(this.dataGridViewUsers_SelectionChanged);
            //
            // UserPermissionManagementForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "UserPermissionManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户权限管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.groupBoxDetails.ResumeLayout(false);
            this.panelDetailsContent.ResumeLayout(false);
            this.groupBoxTimeInfo.ResumeLayout(false);
            this.groupBoxTimeInfo.PerformLayout();
            this.groupBoxRoleInfo.ResumeLayout(false);
            this.groupBoxRoleInfo.PerformLayout();
            this.groupBoxContactInfo.ResumeLayout(false);
            this.groupBoxContactInfo.PerformLayout();
            this.groupBoxBasicInfo.ResumeLayout(false);
            this.groupBoxBasicInfo.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Panel panelDetailsContent;
        private System.Windows.Forms.GroupBox groupBoxTimeInfo;
        private System.Windows.Forms.Label labelCreateTime;
        private System.Windows.Forms.Label labelLastLoginTime;
        private System.Windows.Forms.GroupBox groupBoxRoleInfo;
        private System.Windows.Forms.ComboBox comboBoxRole;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.CheckBox checkBoxStatus;
        private System.Windows.Forms.GroupBox groupBoxContactInfo;
        private System.Windows.Forms.TextBox textBoxPhone;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.GroupBox groupBoxBasicInfo;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Label labelPosition;
        private System.Windows.Forms.TextBox textBoxDepartment;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.TextBox textBoxLoginName;
        private System.Windows.Forms.Label labelLoginName;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.TextBox textBoxUserCode;
        private System.Windows.Forms.Label labelUserCode;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.DataGridView dataGridViewUsers;
    }
}