namespace MES.UI.Forms.Production
{
    partial class ProductionOrderManagementForm
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
            this.groupBoxOtherInfo = new System.Windows.Forms.GroupBox();
            this.textBoxRemarks = new System.Windows.Forms.TextBox();
            this.labelRemarks = new System.Windows.Forms.Label();
            this.textBoxSalesOrderNumber = new System.Windows.Forms.TextBox();
            this.labelSalesOrderNumber = new System.Windows.Forms.Label();
            this.textBoxCustomerName = new System.Windows.Forms.TextBox();
            this.labelCustomerName = new System.Windows.Forms.Label();
            this.groupBoxTimeInfo = new System.Windows.Forms.GroupBox();
            this.dateTimePickerActualEnd = new System.Windows.Forms.DateTimePicker();
            this.labelActualEndTime = new System.Windows.Forms.Label();
            this.dateTimePickerActualStart = new System.Windows.Forms.DateTimePicker();
            this.labelActualStartTime = new System.Windows.Forms.Label();
            this.dateTimePickerPlanEnd = new System.Windows.Forms.DateTimePicker();
            this.labelPlanEndTime = new System.Windows.Forms.Label();
            this.dateTimePickerPlanStart = new System.Windows.Forms.DateTimePicker();
            this.labelPlanStartTime = new System.Windows.Forms.Label();
            this.groupBoxStatusInfo = new System.Windows.Forms.GroupBox();
            this.textBoxResponsiblePerson = new System.Windows.Forms.TextBox();
            this.labelResponsiblePerson = new System.Windows.Forms.Label();
            this.textBoxWorkshopName = new System.Windows.Forms.TextBox();
            this.labelWorkshopName = new System.Windows.Forms.Label();
            this.comboBoxPriority = new System.Windows.Forms.ComboBox();
            this.labelPriority = new System.Windows.Forms.Label();
            this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBoxBasicInfo = new System.Windows.Forms.GroupBox();
            this.textBoxUnit = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxActualQuantity = new System.Windows.Forms.TextBox();
            this.labelActualQuantity = new System.Windows.Forms.Label();
            this.textBoxQuantity = new System.Windows.Forms.TextBox();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.textBoxProductCode = new System.Windows.Forms.TextBox();
            this.labelProductCode = new System.Windows.Forms.Label();
            this.textBoxProductName = new System.Windows.Forms.TextBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.textBoxOrderNo = new System.Windows.Forms.TextBox();
            this.labelOrderNo = new System.Windows.Forms.Label();
            this.labelOrderDetails = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.dataGridViewOrders = new System.Windows.Forms.DataGridView();
            this.labelOrderList = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.groupBoxOtherInfo.SuspendLayout();
            this.groupBoxTimeInfo.SuspendLayout();
            this.groupBoxStatusInfo.SuspendLayout();
            this.groupBoxBasicInfo.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).BeginInit();
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
            this.labelTitle.Size = new System.Drawing.Size(289, 42);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "📋 生产订单管理   ";
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
            this.textBoxSearch.Location = new System.Drawing.Point(120, 12);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(300, 34);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelSearch.Location = new System.Drawing.Point(20, 15);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(145, 27);
            this.labelSearch.TabIndex = 0;
            this.labelSearch.Text = "🔍 快速搜索：";
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
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(350, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 35);
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
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(240, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 35);
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
            this.btnEdit.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(130, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 35);
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
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(20, 15);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 35);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "➕ 新增";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 170);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1400, 530);
            this.panelMain.TabIndex = 3;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.groupBoxOtherInfo);
            this.panelRight.Controls.Add(this.groupBoxTimeInfo);
            this.panelRight.Controls.Add(this.groupBoxStatusInfo);
            this.panelRight.Controls.Add(this.groupBoxBasicInfo);
            this.panelRight.Controls.Add(this.labelOrderDetails);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(800, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(600, 530);
            this.panelRight.TabIndex = 1;
            // 
            // groupBoxOtherInfo
            // 
            this.groupBoxOtherInfo.Controls.Add(this.textBoxRemarks);
            this.groupBoxOtherInfo.Controls.Add(this.labelRemarks);
            this.groupBoxOtherInfo.Controls.Add(this.textBoxSalesOrderNumber);
            this.groupBoxOtherInfo.Controls.Add(this.labelSalesOrderNumber);
            this.groupBoxOtherInfo.Controls.Add(this.textBoxCustomerName);
            this.groupBoxOtherInfo.Controls.Add(this.labelCustomerName);
            this.groupBoxOtherInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOtherInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxOtherInfo.Location = new System.Drawing.Point(0, 340);
            this.groupBoxOtherInfo.Name = "groupBoxOtherInfo";
            this.groupBoxOtherInfo.Size = new System.Drawing.Size(600, 190);
            this.groupBoxOtherInfo.TabIndex = 4;
            this.groupBoxOtherInfo.TabStop = false;
            this.groupBoxOtherInfo.Text = "📝 其他信息";
            // 
            // textBoxRemarks
            // 
            this.textBoxRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxRemarks.Location = new System.Drawing.Point(70, 52);
            this.textBoxRemarks.Multiline = true;
            this.textBoxRemarks.Name = "textBoxRemarks";
            this.textBoxRemarks.ReadOnly = true;
            this.textBoxRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxRemarks.Size = new System.Drawing.Size(475, 120);
            this.textBoxRemarks.TabIndex = 5;
            // 
            // labelRemarks
            // 
            this.labelRemarks.AutoSize = true;
            this.labelRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelRemarks.Location = new System.Drawing.Point(20, 55);
            this.labelRemarks.Name = "labelRemarks";
            this.labelRemarks.Size = new System.Drawing.Size(64, 24);
            this.labelRemarks.TabIndex = 4;
            this.labelRemarks.Text = "备注：";
            // 
            // textBoxSalesOrderNumber
            // 
            this.textBoxSalesOrderNumber.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxSalesOrderNumber.Location = new System.Drawing.Point(395, 22);
            this.textBoxSalesOrderNumber.Name = "textBoxSalesOrderNumber";
            this.textBoxSalesOrderNumber.ReadOnly = true;
            this.textBoxSalesOrderNumber.Size = new System.Drawing.Size(150, 31);
            this.textBoxSalesOrderNumber.TabIndex = 3;
            // 
            // labelSalesOrderNumber
            // 
            this.labelSalesOrderNumber.AutoSize = true;
            this.labelSalesOrderNumber.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelSalesOrderNumber.Location = new System.Drawing.Point(310, 25);
            this.labelSalesOrderNumber.Name = "labelSalesOrderNumber";
            this.labelSalesOrderNumber.Size = new System.Drawing.Size(118, 24);
            this.labelSalesOrderNumber.TabIndex = 2;
            this.labelSalesOrderNumber.Text = "销售订单号：";
            // 
            // textBoxCustomerName
            // 
            this.textBoxCustomerName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxCustomerName.Location = new System.Drawing.Point(90, 22);
            this.textBoxCustomerName.Name = "textBoxCustomerName";
            this.textBoxCustomerName.ReadOnly = true;
            this.textBoxCustomerName.Size = new System.Drawing.Size(200, 31);
            this.textBoxCustomerName.TabIndex = 1;
            // 
            // labelCustomerName
            // 
            this.labelCustomerName.AutoSize = true;
            this.labelCustomerName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelCustomerName.Location = new System.Drawing.Point(20, 25);
            this.labelCustomerName.Name = "labelCustomerName";
            this.labelCustomerName.Size = new System.Drawing.Size(100, 24);
            this.labelCustomerName.TabIndex = 0;
            this.labelCustomerName.Text = "客户名称：";
            // 
            // groupBoxTimeInfo
            // 
            this.groupBoxTimeInfo.Controls.Add(this.dateTimePickerActualEnd);
            this.groupBoxTimeInfo.Controls.Add(this.labelActualEndTime);
            this.groupBoxTimeInfo.Controls.Add(this.dateTimePickerActualStart);
            this.groupBoxTimeInfo.Controls.Add(this.labelActualStartTime);
            this.groupBoxTimeInfo.Controls.Add(this.dateTimePickerPlanEnd);
            this.groupBoxTimeInfo.Controls.Add(this.labelPlanEndTime);
            this.groupBoxTimeInfo.Controls.Add(this.dateTimePickerPlanStart);
            this.groupBoxTimeInfo.Controls.Add(this.labelPlanStartTime);
            this.groupBoxTimeInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTimeInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxTimeInfo.Location = new System.Drawing.Point(0, 250);
            this.groupBoxTimeInfo.Name = "groupBoxTimeInfo";
            this.groupBoxTimeInfo.Size = new System.Drawing.Size(600, 90);
            this.groupBoxTimeInfo.TabIndex = 3;
            this.groupBoxTimeInfo.TabStop = false;
            this.groupBoxTimeInfo.Text = "⏰ 时间信息";
            // 
            // dateTimePickerActualEnd
            // 
            this.dateTimePickerActualEnd.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateTimePickerActualEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerActualEnd.Location = new System.Drawing.Point(335, 52);
            this.dateTimePickerActualEnd.Name = "dateTimePickerActualEnd";
            this.dateTimePickerActualEnd.Size = new System.Drawing.Size(120, 31);
            this.dateTimePickerActualEnd.TabIndex = 7;
            // 
            // labelActualEndTime
            // 
            this.labelActualEndTime.AutoSize = true;
            this.labelActualEndTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelActualEndTime.Location = new System.Drawing.Point(250, 55);
            this.labelActualEndTime.Name = "labelActualEndTime";
            this.labelActualEndTime.Size = new System.Drawing.Size(136, 24);
            this.labelActualEndTime.TabIndex = 6;
            this.labelActualEndTime.Text = "实际完成时间：";
            // 
            // dateTimePickerActualStart
            // 
            this.dateTimePickerActualStart.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateTimePickerActualStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerActualStart.Location = new System.Drawing.Point(105, 52);
            this.dateTimePickerActualStart.Name = "dateTimePickerActualStart";
            this.dateTimePickerActualStart.Size = new System.Drawing.Size(120, 31);
            this.dateTimePickerActualStart.TabIndex = 5;
            // 
            // labelActualStartTime
            // 
            this.labelActualStartTime.AutoSize = true;
            this.labelActualStartTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelActualStartTime.Location = new System.Drawing.Point(20, 55);
            this.labelActualStartTime.Name = "labelActualStartTime";
            this.labelActualStartTime.Size = new System.Drawing.Size(136, 24);
            this.labelActualStartTime.TabIndex = 4;
            this.labelActualStartTime.Text = "实际开始时间：";
            // 
            // dateTimePickerPlanEnd
            // 
            this.dateTimePickerPlanEnd.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateTimePickerPlanEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerPlanEnd.Location = new System.Drawing.Point(335, 22);
            this.dateTimePickerPlanEnd.Name = "dateTimePickerPlanEnd";
            this.dateTimePickerPlanEnd.Size = new System.Drawing.Size(120, 31);
            this.dateTimePickerPlanEnd.TabIndex = 3;
            // 
            // labelPlanEndTime
            // 
            this.labelPlanEndTime.AutoSize = true;
            this.labelPlanEndTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelPlanEndTime.Location = new System.Drawing.Point(250, 25);
            this.labelPlanEndTime.Name = "labelPlanEndTime";
            this.labelPlanEndTime.Size = new System.Drawing.Size(136, 24);
            this.labelPlanEndTime.TabIndex = 2;
            this.labelPlanEndTime.Text = "计划完成时间：";
            // 
            // dateTimePickerPlanStart
            // 
            this.dateTimePickerPlanStart.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateTimePickerPlanStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerPlanStart.Location = new System.Drawing.Point(105, 22);
            this.dateTimePickerPlanStart.Name = "dateTimePickerPlanStart";
            this.dateTimePickerPlanStart.Size = new System.Drawing.Size(120, 31);
            this.dateTimePickerPlanStart.TabIndex = 1;
            // 
            // labelPlanStartTime
            // 
            this.labelPlanStartTime.AutoSize = true;
            this.labelPlanStartTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelPlanStartTime.Location = new System.Drawing.Point(20, 25);
            this.labelPlanStartTime.Name = "labelPlanStartTime";
            this.labelPlanStartTime.Size = new System.Drawing.Size(136, 24);
            this.labelPlanStartTime.TabIndex = 0;
            this.labelPlanStartTime.Text = "计划开始时间：";
            // 
            // groupBoxStatusInfo
            // 
            this.groupBoxStatusInfo.Controls.Add(this.textBoxResponsiblePerson);
            this.groupBoxStatusInfo.Controls.Add(this.labelResponsiblePerson);
            this.groupBoxStatusInfo.Controls.Add(this.textBoxWorkshopName);
            this.groupBoxStatusInfo.Controls.Add(this.labelWorkshopName);
            this.groupBoxStatusInfo.Controls.Add(this.comboBoxPriority);
            this.groupBoxStatusInfo.Controls.Add(this.labelPriority);
            this.groupBoxStatusInfo.Controls.Add(this.comboBoxStatus);
            this.groupBoxStatusInfo.Controls.Add(this.labelStatus);
            this.groupBoxStatusInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxStatusInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxStatusInfo.Location = new System.Drawing.Point(0, 160);
            this.groupBoxStatusInfo.Name = "groupBoxStatusInfo";
            this.groupBoxStatusInfo.Size = new System.Drawing.Size(600, 90);
            this.groupBoxStatusInfo.TabIndex = 2;
            this.groupBoxStatusInfo.TabStop = false;
            this.groupBoxStatusInfo.Text = "🎛️ 状态信息";
            // 
            // textBoxResponsiblePerson
            // 
            this.textBoxResponsiblePerson.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxResponsiblePerson.Location = new System.Drawing.Point(320, 52);
            this.textBoxResponsiblePerson.Name = "textBoxResponsiblePerson";
            this.textBoxResponsiblePerson.ReadOnly = true;
            this.textBoxResponsiblePerson.Size = new System.Drawing.Size(120, 31);
            this.textBoxResponsiblePerson.TabIndex = 7;
            // 
            // labelResponsiblePerson
            // 
            this.labelResponsiblePerson.AutoSize = true;
            this.labelResponsiblePerson.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelResponsiblePerson.Location = new System.Drawing.Point(260, 55);
            this.labelResponsiblePerson.Name = "labelResponsiblePerson";
            this.labelResponsiblePerson.Size = new System.Drawing.Size(82, 24);
            this.labelResponsiblePerson.TabIndex = 6;
            this.labelResponsiblePerson.Text = "负责人：";
            // 
            // textBoxWorkshopName
            // 
            this.textBoxWorkshopName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxWorkshopName.Location = new System.Drawing.Point(90, 52);
            this.textBoxWorkshopName.Name = "textBoxWorkshopName";
            this.textBoxWorkshopName.ReadOnly = true;
            this.textBoxWorkshopName.Size = new System.Drawing.Size(150, 31);
            this.textBoxWorkshopName.TabIndex = 5;
            // 
            // labelWorkshopName
            // 
            this.labelWorkshopName.AutoSize = true;
            this.labelWorkshopName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelWorkshopName.Location = new System.Drawing.Point(20, 55);
            this.labelWorkshopName.Name = "labelWorkshopName";
            this.labelWorkshopName.Size = new System.Drawing.Size(100, 24);
            this.labelWorkshopName.TabIndex = 4;
            this.labelWorkshopName.Text = "负责车间：";
            // 
            // comboBoxPriority
            // 
            this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPriority.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxPriority.FormattingEnabled = true;
            this.comboBoxPriority.Items.AddRange(new object[] {
            "普通",
            "重要",
            "紧急"});
            this.comboBoxPriority.Location = new System.Drawing.Point(320, 22);
            this.comboBoxPriority.Name = "comboBoxPriority";
            this.comboBoxPriority.Size = new System.Drawing.Size(100, 32);
            this.comboBoxPriority.TabIndex = 3;
            // 
            // labelPriority
            // 
            this.labelPriority.AutoSize = true;
            this.labelPriority.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelPriority.Location = new System.Drawing.Point(260, 25);
            this.labelPriority.Name = "labelPriority";
            this.labelPriority.Size = new System.Drawing.Size(82, 24);
            this.labelPriority.TabIndex = 2;
            this.labelPriority.Text = "优先级：";
            // 
            // comboBoxStatus
            // 
            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.Items.AddRange(new object[] {
            "待开始",
            "进行中",
            "已完成",
            "已暂停",
            "已取消"});
            this.comboBoxStatus.Location = new System.Drawing.Point(90, 22);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.Size = new System.Drawing.Size(120, 32);
            this.comboBoxStatus.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelStatus.Location = new System.Drawing.Point(20, 25);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(100, 24);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "订单状态：";
            // 
            // groupBoxBasicInfo
            // 
            this.groupBoxBasicInfo.Controls.Add(this.textBoxUnit);
            this.groupBoxBasicInfo.Controls.Add(this.labelUnit);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxActualQuantity);
            this.groupBoxBasicInfo.Controls.Add(this.labelActualQuantity);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxQuantity);
            this.groupBoxBasicInfo.Controls.Add(this.labelQuantity);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxProductCode);
            this.groupBoxBasicInfo.Controls.Add(this.labelProductCode);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxProductName);
            this.groupBoxBasicInfo.Controls.Add(this.labelProductName);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxOrderNo);
            this.groupBoxBasicInfo.Controls.Add(this.labelOrderNo);
            this.groupBoxBasicInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasicInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxBasicInfo.Location = new System.Drawing.Point(0, 40);
            this.groupBoxBasicInfo.Name = "groupBoxBasicInfo";
            this.groupBoxBasicInfo.Size = new System.Drawing.Size(600, 120);
            this.groupBoxBasicInfo.TabIndex = 1;
            this.groupBoxBasicInfo.TabStop = false;
            this.groupBoxBasicInfo.Text = "📋 基本信息";
            // 
            // textBoxUnit
            // 
            this.textBoxUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxUnit.Location = new System.Drawing.Point(310, 82);
            this.textBoxUnit.Name = "textBoxUnit";
            this.textBoxUnit.ReadOnly = true;
            this.textBoxUnit.Size = new System.Drawing.Size(80, 31);
            this.textBoxUnit.TabIndex = 11;
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelUnit.Location = new System.Drawing.Point(260, 85);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(64, 24);
            this.labelUnit.TabIndex = 10;
            this.labelUnit.Text = "单位：";
            // 
            // textBoxActualQuantity
            // 
            this.textBoxActualQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxActualQuantity.Location = new System.Drawing.Point(90, 82);
            this.textBoxActualQuantity.Name = "textBoxActualQuantity";
            this.textBoxActualQuantity.ReadOnly = true;
            this.textBoxActualQuantity.Size = new System.Drawing.Size(100, 31);
            this.textBoxActualQuantity.TabIndex = 9;
            // 
            // labelActualQuantity
            // 
            this.labelActualQuantity.AutoSize = true;
            this.labelActualQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelActualQuantity.Location = new System.Drawing.Point(20, 85);
            this.labelActualQuantity.Name = "labelActualQuantity";
            this.labelActualQuantity.Size = new System.Drawing.Size(100, 24);
            this.labelActualQuantity.TabIndex = 8;
            this.labelActualQuantity.Text = "完成数量：";
            // 
            // textBoxQuantity
            // 
            this.textBoxQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxQuantity.Location = new System.Drawing.Point(330, 52);
            this.textBoxQuantity.Name = "textBoxQuantity";
            this.textBoxQuantity.ReadOnly = true;
            this.textBoxQuantity.Size = new System.Drawing.Size(100, 31);
            this.textBoxQuantity.TabIndex = 7;
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelQuantity.Location = new System.Drawing.Point(260, 55);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(100, 24);
            this.labelQuantity.TabIndex = 6;
            this.labelQuantity.Text = "计划数量：";
            // 
            // textBoxProductCode
            // 
            this.textBoxProductCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxProductCode.Location = new System.Drawing.Point(90, 52);
            this.textBoxProductCode.Name = "textBoxProductCode";
            this.textBoxProductCode.ReadOnly = true;
            this.textBoxProductCode.Size = new System.Drawing.Size(150, 31);
            this.textBoxProductCode.TabIndex = 5;
            // 
            // labelProductCode
            // 
            this.labelProductCode.AutoSize = true;
            this.labelProductCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelProductCode.Location = new System.Drawing.Point(20, 55);
            this.labelProductCode.Name = "labelProductCode";
            this.labelProductCode.Size = new System.Drawing.Size(100, 24);
            this.labelProductCode.TabIndex = 4;
            this.labelProductCode.Text = "产品编号：";
            // 
            // textBoxProductName
            // 
            this.textBoxProductName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxProductName.Location = new System.Drawing.Point(330, 22);
            this.textBoxProductName.Name = "textBoxProductName";
            this.textBoxProductName.ReadOnly = true;
            this.textBoxProductName.Size = new System.Drawing.Size(150, 31);
            this.textBoxProductName.TabIndex = 3;
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelProductName.Location = new System.Drawing.Point(260, 25);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(100, 24);
            this.labelProductName.TabIndex = 2;
            this.labelProductName.Text = "产品名称：";
            // 
            // textBoxOrderNo
            // 
            this.textBoxOrderNo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxOrderNo.Location = new System.Drawing.Point(90, 22);
            this.textBoxOrderNo.Name = "textBoxOrderNo";
            this.textBoxOrderNo.ReadOnly = true;
            this.textBoxOrderNo.Size = new System.Drawing.Size(150, 31);
            this.textBoxOrderNo.TabIndex = 1;
            // 
            // labelOrderNo
            // 
            this.labelOrderNo.AutoSize = true;
            this.labelOrderNo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelOrderNo.Location = new System.Drawing.Point(20, 25);
            this.labelOrderNo.Name = "labelOrderNo";
            this.labelOrderNo.Size = new System.Drawing.Size(100, 24);
            this.labelOrderNo.TabIndex = 0;
            this.labelOrderNo.Text = "订单编号：";
            // 
            // labelOrderDetails
            // 
            this.labelOrderDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.labelOrderDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOrderDetails.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.labelOrderDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.labelOrderDetails.Location = new System.Drawing.Point(0, 0);
            this.labelOrderDetails.Name = "labelOrderDetails";
            this.labelOrderDetails.Size = new System.Drawing.Size(600, 40);
            this.labelOrderDetails.TabIndex = 0;
            this.labelOrderDetails.Text = "📊 订单详细信息";
            this.labelOrderDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.dataGridViewOrders);
            this.panelLeft.Controls.Add(this.labelOrderList);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(800, 530);
            this.panelLeft.TabIndex = 0;
            // 
            // dataGridViewOrders
            // 
            this.dataGridViewOrders.AllowUserToAddRows = false;
            this.dataGridViewOrders.AllowUserToDeleteRows = false;
            this.dataGridViewOrders.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOrders.Location = new System.Drawing.Point(0, 40);
            this.dataGridViewOrders.MultiSelect = false;
            this.dataGridViewOrders.Name = "dataGridViewOrders";
            this.dataGridViewOrders.ReadOnly = true;
            this.dataGridViewOrders.RowHeadersVisible = false;
            this.dataGridViewOrders.RowHeadersWidth = 62;
            this.dataGridViewOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOrders.Size = new System.Drawing.Size(800, 490);
            this.dataGridViewOrders.TabIndex = 1;
            this.dataGridViewOrders.SelectionChanged += new System.EventHandler(this.dataGridViewOrders_SelectionChanged);
            // 
            // labelOrderList
            // 
            this.labelOrderList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.labelOrderList.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOrderList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.labelOrderList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.labelOrderList.Location = new System.Drawing.Point(0, 0);
            this.labelOrderList.Name = "labelOrderList";
            this.labelOrderList.Size = new System.Drawing.Size(800, 40);
            this.labelOrderList.TabIndex = 0;
            this.labelOrderList.Text = "📋 生产订单列表";
            this.labelOrderList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProductionOrderManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 700);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "ProductionOrderManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生产订单管理 - MES制造执行系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.groupBoxOtherInfo.ResumeLayout(false);
            this.groupBoxOtherInfo.PerformLayout();
            this.groupBoxTimeInfo.ResumeLayout(false);
            this.groupBoxTimeInfo.PerformLayout();
            this.groupBoxStatusInfo.ResumeLayout(false);
            this.groupBoxStatusInfo.PerformLayout();
            this.groupBoxBasicInfo.ResumeLayout(false);
            this.groupBoxBasicInfo.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBoxOtherInfo;
        private System.Windows.Forms.TextBox textBoxRemarks;
        private System.Windows.Forms.Label labelRemarks;
        private System.Windows.Forms.TextBox textBoxSalesOrderNumber;
        private System.Windows.Forms.Label labelSalesOrderNumber;
        private System.Windows.Forms.TextBox textBoxCustomerName;
        private System.Windows.Forms.Label labelCustomerName;
        private System.Windows.Forms.GroupBox groupBoxTimeInfo;
        private System.Windows.Forms.DateTimePicker dateTimePickerActualEnd;
        private System.Windows.Forms.Label labelActualEndTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerActualStart;
        private System.Windows.Forms.Label labelActualStartTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerPlanEnd;
        private System.Windows.Forms.Label labelPlanEndTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerPlanStart;
        private System.Windows.Forms.Label labelPlanStartTime;
        private System.Windows.Forms.GroupBox groupBoxStatusInfo;
        private System.Windows.Forms.TextBox textBoxResponsiblePerson;
        private System.Windows.Forms.Label labelResponsiblePerson;
        private System.Windows.Forms.TextBox textBoxWorkshopName;
        private System.Windows.Forms.Label labelWorkshopName;
        private System.Windows.Forms.ComboBox comboBoxPriority;
        private System.Windows.Forms.Label labelPriority;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.GroupBox groupBoxBasicInfo;
        private System.Windows.Forms.TextBox textBoxUnit;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox textBoxActualQuantity;
        private System.Windows.Forms.Label labelActualQuantity;
        private System.Windows.Forms.TextBox textBoxQuantity;
        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.TextBox textBoxProductCode;
        private System.Windows.Forms.Label labelProductCode;
        private System.Windows.Forms.TextBox textBoxProductName;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TextBox textBoxOrderNo;
        private System.Windows.Forms.Label labelOrderNo;
        private System.Windows.Forms.Label labelOrderDetails;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.DataGridView dataGridViewOrders;
        private System.Windows.Forms.Label labelOrderList;
    }
}