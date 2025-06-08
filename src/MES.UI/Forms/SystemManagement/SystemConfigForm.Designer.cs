namespace MES.UI.Forms.SystemManagement
{
    partial class SystemConfigForm
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
            this.headerPanel = new System.Windows.Forms.Panel();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.subtitleLabel = new System.Windows.Forms.Label();
            this.configTabControl = new System.Windows.Forms.TabControl();
            this.dbTabPage = new System.Windows.Forms.TabPage();
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.databaseTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.connectionStatusLabel = new System.Windows.Forms.Label();
            this.systemTabPage = new System.Windows.Forms.TabPage();
            this.systemTitleLabel = new System.Windows.Forms.Label();
            this.systemTitleTextBox = new System.Windows.Forms.TextBox();
            this.systemVersionLabel = new System.Windows.Forms.Label();
            this.systemVersionTextBox = new System.Windows.Forms.TextBox();
            this.themeLabel = new System.Windows.Forms.Label();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.autoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.enableLoggingCheckBox = new System.Windows.Forms.CheckBox();
            this.logLevelLabel = new System.Windows.Forms.Label();
            this.logLevelComboBox = new System.Windows.Forms.ComboBox();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.configTabControl.SuspendLayout();
            this.dbTabPage.SuspendLayout();
            this.systemTabPage.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.headerPanel.Controls.Add(this.iconPictureBox);
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Controls.Add(this.subtitleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(580, 80);
            this.headerPanel.TabIndex = 0;
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Location = new System.Drawing.Point(20, 16);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(48, 48);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(80, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(126, 26);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "⚙️ 系统配置";
            // 
            // subtitleLabel
            // 
            this.subtitleLabel.AutoSize = true;
            this.subtitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.subtitleLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.subtitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(212)))), ((int)(((byte)(218)))));
            this.subtitleLabel.Location = new System.Drawing.Point(80, 45);
            this.subtitleLabel.Name = "subtitleLabel";
            this.subtitleLabel.Size = new System.Drawing.Size(152, 17);
            this.subtitleLabel.TabIndex = 2;
            this.subtitleLabel.Text = "配置系统参数和数据库连接";
            // 
            // configTabControl
            // 
            this.configTabControl.Controls.Add(this.dbTabPage);
            this.configTabControl.Controls.Add(this.systemTabPage);
            this.configTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configTabControl.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.configTabControl.ItemSize = new System.Drawing.Size(100, 32);
            this.configTabControl.Location = new System.Drawing.Point(0, 80);
            this.configTabControl.Name = "configTabControl";
            this.configTabControl.Padding = new System.Drawing.Point(20, 15);
            this.configTabControl.SelectedIndex = 0;
            this.configTabControl.Size = new System.Drawing.Size(580, 340);
            this.configTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.configTabControl.TabIndex = 1;
            //
            // dbTabPage
            //
            this.dbTabPage.BackColor = System.Drawing.Color.White;
            this.dbTabPage.Controls.Add(this.connectionStatusLabel);
            this.dbTabPage.Controls.Add(this.testConnectionButton);
            this.dbTabPage.Controls.Add(this.passwordTextBox);
            this.dbTabPage.Controls.Add(this.passwordLabel);
            this.dbTabPage.Controls.Add(this.usernameTextBox);
            this.dbTabPage.Controls.Add(this.usernameLabel);
            this.dbTabPage.Controls.Add(this.databaseTextBox);
            this.dbTabPage.Controls.Add(this.databaseLabel);
            this.dbTabPage.Controls.Add(this.portTextBox);
            this.dbTabPage.Controls.Add(this.portLabel);
            this.dbTabPage.Controls.Add(this.serverTextBox);
            this.dbTabPage.Controls.Add(this.serverLabel);
            this.dbTabPage.Location = new System.Drawing.Point(4, 36);
            this.dbTabPage.Name = "dbTabPage";
            this.dbTabPage.Padding = new System.Windows.Forms.Padding(20);
            this.dbTabPage.Size = new System.Drawing.Size(572, 300);
            this.dbTabPage.TabIndex = 0;
            this.dbTabPage.Text = "数据库配置";
            //
            // serverLabel
            //
            this.serverLabel.AutoSize = true;
            this.serverLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.serverLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.serverLabel.Location = new System.Drawing.Point(15, 25);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(80, 17);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "数据库服务器:";
            //
            // serverTextBox
            //
            this.serverTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverTextBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.serverTextBox.Location = new System.Drawing.Point(120, 25);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(140, 23);
            this.serverTextBox.TabIndex = 1;
            this.serverTextBox.Text = "localhost";
            //
            // portLabel
            //
            this.portLabel.AutoSize = true;
            this.portLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.portLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.portLabel.Location = new System.Drawing.Point(280, 25);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(32, 17);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "端口:";
            //
            // portTextBox
            //
            this.portTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.portTextBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.portTextBox.Location = new System.Drawing.Point(320, 25);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(100, 23);
            this.portTextBox.TabIndex = 3;
            this.portTextBox.Text = "3306";
            //
            // databaseLabel
            //
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.databaseLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.databaseLabel.Location = new System.Drawing.Point(15, 65);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(56, 17);
            this.databaseLabel.TabIndex = 4;
            this.databaseLabel.Text = "数据库名:";
            //
            // databaseTextBox
            //
            this.databaseTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.databaseTextBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.databaseTextBox.Location = new System.Drawing.Point(120, 65);
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.Size = new System.Drawing.Size(180, 23);
            this.databaseTextBox.TabIndex = 5;
            this.databaseTextBox.Text = "mes_db";
            //
            // usernameLabel
            //
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.usernameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.usernameLabel.Location = new System.Drawing.Point(15, 105);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(44, 17);
            this.usernameLabel.TabIndex = 6;
            this.usernameLabel.Text = "用户名:";
            //
            // usernameTextBox
            //
            this.usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.usernameTextBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.usernameTextBox.Location = new System.Drawing.Point(120, 105);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(180, 23);
            this.usernameTextBox.TabIndex = 7;
            this.usernameTextBox.Text = "root";
            //
            // passwordLabel
            //
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.passwordLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.passwordLabel.Location = new System.Drawing.Point(15, 145);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(32, 17);
            this.passwordLabel.TabIndex = 8;
            this.passwordLabel.Text = "密码:";
            //
            // passwordTextBox
            //
            this.passwordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordTextBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.passwordTextBox.Location = new System.Drawing.Point(120, 145);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(180, 23);
            this.passwordTextBox.TabIndex = 9;
            this.passwordTextBox.UseSystemPasswordChar = true;
            //
            // testConnectionButton
            //
            this.testConnectionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.testConnectionButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.testConnectionButton.FlatAppearance.BorderSize = 0;
            this.testConnectionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.testConnectionButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.testConnectionButton.ForeColor = System.Drawing.Color.White;
            this.testConnectionButton.Location = new System.Drawing.Point(120, 185);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(100, 32);
            this.testConnectionButton.TabIndex = 10;
            this.testConnectionButton.Text = "🔗 测试连接";
            this.testConnectionButton.UseVisualStyleBackColor = false;
            this.testConnectionButton.Click += new System.EventHandler(this.TestConnectionButton_Click);
            //
            // connectionStatusLabel
            //
            this.connectionStatusLabel.AutoSize = true;
            this.connectionStatusLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.connectionStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.connectionStatusLabel.Location = new System.Drawing.Point(250, 190);
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(44, 17);
            this.connectionStatusLabel.TabIndex = 11;
            this.connectionStatusLabel.Text = "未测试";
            //
            // systemTabPage
            //
            this.systemTabPage.BackColor = System.Drawing.Color.White;
            this.systemTabPage.Controls.Add(this.logLevelComboBox);
            this.systemTabPage.Controls.Add(this.logLevelLabel);
            this.systemTabPage.Controls.Add(this.enableLoggingCheckBox);
            this.systemTabPage.Controls.Add(this.autoStartCheckBox);
            this.systemTabPage.Controls.Add(this.themeComboBox);
            this.systemTabPage.Controls.Add(this.themeLabel);
            this.systemTabPage.Controls.Add(this.systemVersionTextBox);
            this.systemTabPage.Controls.Add(this.systemVersionLabel);
            this.systemTabPage.Controls.Add(this.systemTitleTextBox);
            this.systemTabPage.Controls.Add(this.systemTitleLabel);
            this.systemTabPage.Location = new System.Drawing.Point(4, 36);
            this.systemTabPage.Name = "systemTabPage";
            this.systemTabPage.Padding = new System.Windows.Forms.Padding(20);
            this.systemTabPage.Size = new System.Drawing.Size(572, 300);
            this.systemTabPage.TabIndex = 1;
            this.systemTabPage.Text = "系统设置";
            //
            // systemTitleLabel
            //
            this.systemTitleLabel.AutoSize = true;
            this.systemTitleLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.systemTitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.systemTitleLabel.Location = new System.Drawing.Point(15, 25);
            this.systemTitleLabel.Name = "systemTitleLabel";
            this.systemTitleLabel.Size = new System.Drawing.Size(56, 17);
            this.systemTitleLabel.TabIndex = 0;
            this.systemTitleLabel.Text = "系统标题:";
            //
            // systemTitleTextBox
            //
            this.systemTitleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.systemTitleTextBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.systemTitleTextBox.Location = new System.Drawing.Point(120, 25);
            this.systemTitleTextBox.Name = "systemTitleTextBox";
            this.systemTitleTextBox.Size = new System.Drawing.Size(180, 23);
            this.systemTitleTextBox.TabIndex = 1;
            this.systemTitleTextBox.Text = "MES制造执行系统";
            //
            // systemVersionLabel
            //
            this.systemVersionLabel.AutoSize = true;
            this.systemVersionLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.systemVersionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.systemVersionLabel.Location = new System.Drawing.Point(15, 65);
            this.systemVersionLabel.Name = "systemVersionLabel";
            this.systemVersionLabel.Size = new System.Drawing.Size(56, 17);
            this.systemVersionLabel.TabIndex = 2;
            this.systemVersionLabel.Text = "系统版本:";
            //
            // systemVersionTextBox
            //
            this.systemVersionTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.systemVersionTextBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.systemVersionTextBox.Location = new System.Drawing.Point(120, 65);
            this.systemVersionTextBox.Name = "systemVersionTextBox";
            this.systemVersionTextBox.Size = new System.Drawing.Size(180, 23);
            this.systemVersionTextBox.TabIndex = 3;
            this.systemVersionTextBox.Text = "1.0.0";
            //
            // themeLabel
            //
            this.themeLabel.AutoSize = true;
            this.themeLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.themeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.themeLabel.Location = new System.Drawing.Point(15, 105);
            this.themeLabel.Name = "themeLabel";
            this.themeLabel.Size = new System.Drawing.Size(56, 17);
            this.themeLabel.TabIndex = 4;
            this.themeLabel.Text = "界面主题:";
            //
            // themeComboBox
            //
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeComboBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Items.AddRange(new object[] {
            "默认主题",
            "蓝色主题",
            "深色主题"});
            this.themeComboBox.Location = new System.Drawing.Point(120, 105);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(180, 25);
            this.themeComboBox.TabIndex = 5;
            //
            // autoStartCheckBox
            //
            this.autoStartCheckBox.AutoSize = true;
            this.autoStartCheckBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.autoStartCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.autoStartCheckBox.Location = new System.Drawing.Point(120, 145);
            this.autoStartCheckBox.Name = "autoStartCheckBox";
            this.autoStartCheckBox.Size = new System.Drawing.Size(99, 21);
            this.autoStartCheckBox.TabIndex = 6;
            this.autoStartCheckBox.Text = "开机自动启动";
            this.autoStartCheckBox.UseVisualStyleBackColor = true;
            //
            // enableLoggingCheckBox
            //
            this.enableLoggingCheckBox.AutoSize = true;
            this.enableLoggingCheckBox.Checked = true;
            this.enableLoggingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableLoggingCheckBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.enableLoggingCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.enableLoggingCheckBox.Location = new System.Drawing.Point(120, 175);
            this.enableLoggingCheckBox.Name = "enableLoggingCheckBox";
            this.enableLoggingCheckBox.Size = new System.Drawing.Size(99, 21);
            this.enableLoggingCheckBox.TabIndex = 7;
            this.enableLoggingCheckBox.Text = "启用系统日志";
            this.enableLoggingCheckBox.UseVisualStyleBackColor = true;
            //
            // logLevelLabel
            //
            this.logLevelLabel.AutoSize = true;
            this.logLevelLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.logLevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.logLevelLabel.Location = new System.Drawing.Point(15, 205);
            this.logLevelLabel.Name = "logLevelLabel";
            this.logLevelLabel.Size = new System.Drawing.Size(56, 17);
            this.logLevelLabel.TabIndex = 8;
            this.logLevelLabel.Text = "日志级别:";
            //
            // logLevelComboBox
            //
            this.logLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.logLevelComboBox.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.logLevelComboBox.FormattingEnabled = true;
            this.logLevelComboBox.Items.AddRange(new object[] {
            "Error",
            "Warning",
            "Info",
            "Debug"});
            this.logLevelComboBox.Location = new System.Drawing.Point(120, 205);
            this.logLevelComboBox.Name = "logLevelComboBox";
            this.logLevelComboBox.Size = new System.Drawing.Size(180, 25);
            this.logLevelComboBox.TabIndex = 9;
            //
            // buttonPanel
            //
            this.buttonPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.buttonPanel.Controls.Add(this.resetButton);
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.Controls.Add(this.saveButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 420);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.buttonPanel.Size = new System.Drawing.Size(580, 60);
            this.buttonPanel.TabIndex = 2;
            //
            // saveButton
            //
            this.saveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.saveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.saveButton.FlatAppearance.BorderSize = 0;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(460, 12);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 36);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "💾 保存";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            //
            // cancelButton
            //
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(380, 12);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(80, 36);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "❌ 取消";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            //
            // resetButton
            //
            this.resetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.resetButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.resetButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.resetButton.FlatAppearance.BorderSize = 0;
            this.resetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.resetButton.ForeColor = System.Drawing.Color.White;
            this.resetButton.Location = new System.Drawing.Point(20, 12);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(80, 36);
            this.resetButton.TabIndex = 0;
            this.resetButton.Text = "🔄 重置";
            this.resetButton.UseVisualStyleBackColor = false;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            //
            // SystemConfigForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(580, 480);
            this.Controls.Add(this.configTabControl);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统配置";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.configTabControl.ResumeLayout(false);
            this.dbTabPage.ResumeLayout(false);
            this.dbTabPage.PerformLayout();
            this.systemTabPage.ResumeLayout(false);
            this.systemTabPage.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label subtitleLabel;
        private System.Windows.Forms.TabControl configTabControl;
        private System.Windows.Forms.TabPage dbTabPage;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.TextBox databaseTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button testConnectionButton;
        private System.Windows.Forms.Label connectionStatusLabel;
        private System.Windows.Forms.TabPage systemTabPage;
        private System.Windows.Forms.Label systemTitleLabel;
        private System.Windows.Forms.TextBox systemTitleTextBox;
        private System.Windows.Forms.Label systemVersionLabel;
        private System.Windows.Forms.TextBox systemVersionTextBox;
        private System.Windows.Forms.Label themeLabel;
        private System.Windows.Forms.ComboBox themeComboBox;
        private System.Windows.Forms.CheckBox autoStartCheckBox;
        private System.Windows.Forms.CheckBox enableLoggingCheckBox;
        private System.Windows.Forms.Label logLevelLabel;
        private System.Windows.Forms.ComboBox logLevelComboBox;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button resetButton;
    }
}
