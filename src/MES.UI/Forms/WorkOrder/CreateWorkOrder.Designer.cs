namespace MES.UI.Forms.WorkOrder
{
    partial class CreateWorkOrder
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.grpBasicInfo = new System.Windows.Forms.GroupBox();
            this.lblWorkOrderType = new System.Windows.Forms.Label();
            this.cmbWorkOrderType = new System.Windows.Forms.ComboBox();
            this.lblWorkOrderDesc = new System.Windows.Forms.Label();
            this.txtWorkOrderDesc = new System.Windows.Forms.TextBox();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.cmbProductCode = new System.Windows.Forms.ComboBox();
            this.lblFinishedWorkOrder = new System.Windows.Forms.Label();
            this.cmbFinishedWorkOrder = new System.Windows.Forms.ComboBox();
            this.lblBOMCode = new System.Windows.Forms.Label();
            this.txtBOMCode = new System.Windows.Forms.TextBox();
            this.txtBOMVersion = new System.Windows.Forms.TextBox();
            this.btnSelectBOM = new System.Windows.Forms.Button();
            this.grpPlanInfo = new System.Windows.Forms.GroupBox();
            this.lblPlanStartDate = new System.Windows.Forms.Label();
            this.dtpPlanStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblPlanEndDate = new System.Windows.Forms.Label();
            this.dtpPlanEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblPlanQuantity = new System.Windows.Forms.Label();
            this.txtPlanQuantity = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblProductType = new System.Windows.Forms.Label();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.panelRight = new System.Windows.Forms.Panel();
            this.grpBOMList = new System.Windows.Forms.GroupBox();
            this.dgvBOMList = new System.Windows.Forms.DataGridView();
            this.panelBOMButtons = new System.Windows.Forms.Panel();
            this.btnAddBOM = new System.Windows.Forms.Button();
            this.btnRemoveBOM = new System.Windows.Forms.Button();
            this.btnRefreshBOM = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.grpRemarks = new System.Windows.Forms.GroupBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.grpBasicInfo.SuspendLayout();
            this.grpPlanInfo.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.grpBOMList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOMList)).BeginInit();
            this.panelBOMButtons.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.grpRemarks.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1800, 90);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 22);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(195, 42);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "➕ 创建工单";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 90);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.panelMain.Size = new System.Drawing.Size(1800, 720);
            this.panelMain.TabIndex = 1;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.grpPlanInfo);
            this.panelLeft.Controls.Add(this.grpBasicInfo);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(30, 30);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(870, 660);
            this.panelLeft.TabIndex = 0;
            // 
            // grpBasicInfo
            // 
            this.grpBasicInfo.Controls.Add(this.lblWorkOrderType);
            this.grpBasicInfo.Controls.Add(this.cmbWorkOrderType);
            this.grpBasicInfo.Controls.Add(this.lblWorkOrderDesc);
            this.grpBasicInfo.Controls.Add(this.txtWorkOrderDesc);
            this.grpBasicInfo.Controls.Add(this.lblProductCode);
            this.grpBasicInfo.Controls.Add(this.cmbProductCode);
            this.grpBasicInfo.Controls.Add(this.lblFinishedWorkOrder);
            this.grpBasicInfo.Controls.Add(this.cmbFinishedWorkOrder);
            this.grpBasicInfo.Controls.Add(this.lblBOMCode);
            this.grpBasicInfo.Controls.Add(this.txtBOMCode);
            this.grpBasicInfo.Controls.Add(this.txtBOMVersion);
            this.grpBasicInfo.Controls.Add(this.btnSelectBOM);
            this.grpBasicInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBasicInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpBasicInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.grpBasicInfo.Location = new System.Drawing.Point(0, 0);
            this.grpBasicInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpBasicInfo.Name = "grpBasicInfo";
            this.grpBasicInfo.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpBasicInfo.Size = new System.Drawing.Size(870, 330);
            this.grpBasicInfo.TabIndex = 0;
            this.grpBasicInfo.TabStop = false;
            this.grpBasicInfo.Text = "📋 基本信息";
            // 
            // lblWorkOrderType
            // 
            this.lblWorkOrderType.AutoSize = true;
            this.lblWorkOrderType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblWorkOrderType.ForeColor = System.Drawing.Color.Black;
            this.lblWorkOrderType.Location = new System.Drawing.Point(30, 52);
            this.lblWorkOrderType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkOrderType.Name = "lblWorkOrderType";
            this.lblWorkOrderType.Size = new System.Drawing.Size(100, 24);
            this.lblWorkOrderType.TabIndex = 0;
            this.lblWorkOrderType.Text = "工单类型：";
            // 
            // cmbWorkOrderType
            // 
            this.cmbWorkOrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkOrderType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbWorkOrderType.FormattingEnabled = true;
            this.cmbWorkOrderType.Items.AddRange(new object[] {
            "生产工单",
            "维修工单",
            "测试工单",
            "返工工单"});
            this.cmbWorkOrderType.Location = new System.Drawing.Point(180, 48);
            this.cmbWorkOrderType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbWorkOrderType.Name = "cmbWorkOrderType";
            this.cmbWorkOrderType.Size = new System.Drawing.Size(223, 32);
            this.cmbWorkOrderType.TabIndex = 1;
            // 
            // lblWorkOrderDesc
            // 
            this.lblWorkOrderDesc.AutoSize = true;
            this.lblWorkOrderDesc.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblWorkOrderDesc.ForeColor = System.Drawing.Color.Black;
            this.lblWorkOrderDesc.Location = new System.Drawing.Point(30, 105);
            this.lblWorkOrderDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWorkOrderDesc.Name = "lblWorkOrderDesc";
            this.lblWorkOrderDesc.Size = new System.Drawing.Size(100, 24);
            this.lblWorkOrderDesc.TabIndex = 2;
            this.lblWorkOrderDesc.Text = "工单说明：";
            // 
            // txtWorkOrderDesc
            // 
            this.txtWorkOrderDesc.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtWorkOrderDesc.Location = new System.Drawing.Point(180, 100);
            this.txtWorkOrderDesc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtWorkOrderDesc.Name = "txtWorkOrderDesc";
            this.txtWorkOrderDesc.Size = new System.Drawing.Size(643, 31);
            this.txtWorkOrderDesc.TabIndex = 3;
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblProductCode.ForeColor = System.Drawing.Color.Black;
            this.lblProductCode.Location = new System.Drawing.Point(30, 158);
            this.lblProductCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(100, 24);
            this.lblProductCode.TabIndex = 4;
            this.lblProductCode.Text = "产品编号：";
            // 
            // cmbProductCode
            // 
            this.cmbProductCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbProductCode.FormattingEnabled = true;
            this.cmbProductCode.Location = new System.Drawing.Point(180, 153);
            this.cmbProductCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbProductCode.Name = "cmbProductCode";
            this.cmbProductCode.Size = new System.Drawing.Size(298, 32);
            this.cmbProductCode.TabIndex = 5;
            // 
            // lblFinishedWorkOrder
            // 
            this.lblFinishedWorkOrder.AutoSize = true;
            this.lblFinishedWorkOrder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblFinishedWorkOrder.ForeColor = System.Drawing.Color.Black;
            this.lblFinishedWorkOrder.Location = new System.Drawing.Point(510, 158);
            this.lblFinishedWorkOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFinishedWorkOrder.Name = "lblFinishedWorkOrder";
            this.lblFinishedWorkOrder.Size = new System.Drawing.Size(118, 24);
            this.lblFinishedWorkOrder.TabIndex = 6;
            this.lblFinishedWorkOrder.Text = "成品工单号：";
            // 
            // cmbFinishedWorkOrder
            // 
            this.cmbFinishedWorkOrder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbFinishedWorkOrder.FormattingEnabled = true;
            this.cmbFinishedWorkOrder.Location = new System.Drawing.Point(645, 153);
            this.cmbFinishedWorkOrder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbFinishedWorkOrder.Name = "cmbFinishedWorkOrder";
            this.cmbFinishedWorkOrder.Size = new System.Drawing.Size(178, 32);
            this.cmbFinishedWorkOrder.TabIndex = 7;
            // 
            // lblBOMCode
            // 
            this.lblBOMCode.AutoSize = true;
            this.lblBOMCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblBOMCode.ForeColor = System.Drawing.Color.Black;
            this.lblBOMCode.Location = new System.Drawing.Point(30, 210);
            this.lblBOMCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBOMCode.Name = "lblBOMCode";
            this.lblBOMCode.Size = new System.Drawing.Size(108, 24);
            this.lblBOMCode.TabIndex = 8;
            this.lblBOMCode.Text = "BOM编号：";
            // 
            // txtBOMCode
            // 
            this.txtBOMCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBOMCode.Location = new System.Drawing.Point(180, 206);
            this.txtBOMCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBOMCode.Name = "txtBOMCode";
            this.txtBOMCode.ReadOnly = true;
            this.txtBOMCode.Size = new System.Drawing.Size(223, 31);
            this.txtBOMCode.TabIndex = 9;
            // 
            // txtBOMVersion
            // 
            this.txtBOMVersion.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBOMVersion.Location = new System.Drawing.Point(420, 206);
            this.txtBOMVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBOMVersion.Name = "txtBOMVersion";
            this.txtBOMVersion.ReadOnly = true;
            this.txtBOMVersion.Size = new System.Drawing.Size(118, 31);
            this.txtBOMVersion.TabIndex = 10;
            // 
            // btnSelectBOM
            // 
            this.btnSelectBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSelectBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSelectBOM.ForeColor = System.Drawing.Color.White;
            this.btnSelectBOM.Location = new System.Drawing.Point(555, 202);
            this.btnSelectBOM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectBOM.Name = "btnSelectBOM";
            this.btnSelectBOM.Size = new System.Drawing.Size(120, 40);
            this.btnSelectBOM.TabIndex = 11;
            this.btnSelectBOM.Text = "🔍 选择BOM";
            this.btnSelectBOM.UseVisualStyleBackColor = false;
            // 
            // grpPlanInfo
            // 
            this.grpPlanInfo.Controls.Add(this.lblPlanStartDate);
            this.grpPlanInfo.Controls.Add(this.dtpPlanStartDate);
            this.grpPlanInfo.Controls.Add(this.lblPlanEndDate);
            this.grpPlanInfo.Controls.Add(this.dtpPlanEndDate);
            this.grpPlanInfo.Controls.Add(this.lblPlanQuantity);
            this.grpPlanInfo.Controls.Add(this.txtPlanQuantity);
            this.grpPlanInfo.Controls.Add(this.lblUnit);
            this.grpPlanInfo.Controls.Add(this.txtUnit);
            this.grpPlanInfo.Controls.Add(this.lblProductType);
            this.grpPlanInfo.Controls.Add(this.txtProductType);
            this.grpPlanInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPlanInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpPlanInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.grpPlanInfo.Location = new System.Drawing.Point(0, 330);
            this.grpPlanInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpPlanInfo.Name = "grpPlanInfo";
            this.grpPlanInfo.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpPlanInfo.Size = new System.Drawing.Size(870, 330);
            this.grpPlanInfo.TabIndex = 1;
            this.grpPlanInfo.TabStop = false;
            this.grpPlanInfo.Text = "📅 计划信息";
            // 
            // lblPlanStartDate
            // 
            this.lblPlanStartDate.AutoSize = true;
            this.lblPlanStartDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblPlanStartDate.ForeColor = System.Drawing.Color.Black;
            this.lblPlanStartDate.Location = new System.Drawing.Point(30, 52);
            this.lblPlanStartDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlanStartDate.Name = "lblPlanStartDate";
            this.lblPlanStartDate.Size = new System.Drawing.Size(136, 24);
            this.lblPlanStartDate.TabIndex = 0;
            this.lblPlanStartDate.Text = "计划开始日期：";
            // 
            // dtpPlanStartDate
            // 
            this.dtpPlanStartDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dtpPlanStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPlanStartDate.Location = new System.Drawing.Point(180, 48);
            this.dtpPlanStartDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpPlanStartDate.Name = "dtpPlanStartDate";
            this.dtpPlanStartDate.Size = new System.Drawing.Size(223, 31);
            this.dtpPlanStartDate.TabIndex = 1;
            // 
            // lblPlanEndDate
            // 
            this.lblPlanEndDate.AutoSize = true;
            this.lblPlanEndDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblPlanEndDate.ForeColor = System.Drawing.Color.Black;
            this.lblPlanEndDate.Location = new System.Drawing.Point(435, 52);
            this.lblPlanEndDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlanEndDate.Name = "lblPlanEndDate";
            this.lblPlanEndDate.Size = new System.Drawing.Size(136, 24);
            this.lblPlanEndDate.TabIndex = 2;
            this.lblPlanEndDate.Text = "计划结束日期：";
            // 
            // dtpPlanEndDate
            // 
            this.dtpPlanEndDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dtpPlanEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPlanEndDate.Location = new System.Drawing.Point(585, 48);
            this.dtpPlanEndDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpPlanEndDate.Name = "dtpPlanEndDate";
            this.dtpPlanEndDate.Size = new System.Drawing.Size(223, 31);
            this.dtpPlanEndDate.TabIndex = 3;
            // 
            // lblPlanQuantity
            // 
            this.lblPlanQuantity.AutoSize = true;
            this.lblPlanQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblPlanQuantity.ForeColor = System.Drawing.Color.Black;
            this.lblPlanQuantity.Location = new System.Drawing.Point(30, 105);
            this.lblPlanQuantity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlanQuantity.Name = "lblPlanQuantity";
            this.lblPlanQuantity.Size = new System.Drawing.Size(100, 24);
            this.lblPlanQuantity.TabIndex = 4;
            this.lblPlanQuantity.Text = "计划数量：";
            // 
            // txtPlanQuantity
            // 
            this.txtPlanQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtPlanQuantity.Location = new System.Drawing.Point(180, 100);
            this.txtPlanQuantity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPlanQuantity.Name = "txtPlanQuantity";
            this.txtPlanQuantity.Size = new System.Drawing.Size(148, 31);
            this.txtPlanQuantity.TabIndex = 5;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblUnit.ForeColor = System.Drawing.Color.Black;
            this.lblUnit.Location = new System.Drawing.Point(360, 105);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(64, 24);
            this.lblUnit.TabIndex = 6;
            this.lblUnit.Text = "单位：";
            // 
            // txtUnit
            // 
            this.txtUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtUnit.Location = new System.Drawing.Point(435, 100);
            this.txtUnit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(118, 31);
            this.txtUnit.TabIndex = 7;
            // 
            // lblProductType
            // 
            this.lblProductType.AutoSize = true;
            this.lblProductType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblProductType.ForeColor = System.Drawing.Color.Black;
            this.lblProductType.Location = new System.Drawing.Point(585, 105);
            this.lblProductType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(100, 24);
            this.lblProductType.TabIndex = 8;
            this.lblProductType.Text = "产品类型：";
            // 
            // txtProductType
            // 
            this.txtProductType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtProductType.Location = new System.Drawing.Point(690, 100);
            this.txtProductType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(148, 31);
            this.txtProductType.TabIndex = 9;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.grpBOMList);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(30, 30);
            this.panelRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.panelRight.Size = new System.Drawing.Size(1740, 660);
            this.panelRight.TabIndex = 1;
            // 
            // grpBOMList
            // 
            this.grpBOMList.Controls.Add(this.dgvBOMList);
            this.grpBOMList.Controls.Add(this.panelBOMButtons);
            this.grpBOMList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBOMList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpBOMList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.grpBOMList.Location = new System.Drawing.Point(30, 0);
            this.grpBOMList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpBOMList.Name = "grpBOMList";
            this.grpBOMList.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpBOMList.Size = new System.Drawing.Size(1710, 660);
            this.grpBOMList.TabIndex = 0;
            this.grpBOMList.TabStop = false;
            this.grpBOMList.Text = "📦 BOM物料清单";
            // 
            // dgvBOMList
            // 
            this.dgvBOMList.AllowUserToAddRows = false;
            this.dgvBOMList.AllowUserToDeleteRows = false;
            this.dgvBOMList.BackgroundColor = System.Drawing.Color.White;
            this.dgvBOMList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBOMList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBOMList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBOMList.Location = new System.Drawing.Point(4, 31);
            this.dgvBOMList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvBOMList.MultiSelect = false;
            this.dgvBOMList.Name = "dgvBOMList";
            this.dgvBOMList.ReadOnly = true;
            this.dgvBOMList.RowHeadersWidth = 62;
            this.dgvBOMList.RowTemplate.Height = 25;
            this.dgvBOMList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBOMList.Size = new System.Drawing.Size(1702, 565);
            this.dgvBOMList.TabIndex = 0;
            // 
            // panelBOMButtons
            // 
            this.panelBOMButtons.Controls.Add(this.btnAddBOM);
            this.panelBOMButtons.Controls.Add(this.btnRemoveBOM);
            this.panelBOMButtons.Controls.Add(this.btnRefreshBOM);
            this.panelBOMButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBOMButtons.Location = new System.Drawing.Point(4, 596);
            this.panelBOMButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelBOMButtons.Name = "panelBOMButtons";
            this.panelBOMButtons.Size = new System.Drawing.Size(1702, 60);
            this.panelBOMButtons.TabIndex = 1;
            // 
            // btnAddBOM
            // 
            this.btnAddBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnAddBOM.ForeColor = System.Drawing.Color.White;
            this.btnAddBOM.Location = new System.Drawing.Point(15, 8);
            this.btnAddBOM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddBOM.Name = "btnAddBOM";
            this.btnAddBOM.Size = new System.Drawing.Size(120, 45);
            this.btnAddBOM.TabIndex = 0;
            this.btnAddBOM.Text = "➕ 添加";
            this.btnAddBOM.UseVisualStyleBackColor = false;
            // 
            // btnRemoveBOM
            // 
            this.btnRemoveBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRemoveBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRemoveBOM.ForeColor = System.Drawing.Color.White;
            this.btnRemoveBOM.Location = new System.Drawing.Point(150, 8);
            this.btnRemoveBOM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveBOM.Name = "btnRemoveBOM";
            this.btnRemoveBOM.Size = new System.Drawing.Size(120, 45);
            this.btnRemoveBOM.TabIndex = 1;
            this.btnRemoveBOM.Text = "➖ 移除";
            this.btnRemoveBOM.UseVisualStyleBackColor = false;
            // 
            // btnRefreshBOM
            // 
            this.btnRefreshBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefreshBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefreshBOM.ForeColor = System.Drawing.Color.White;
            this.btnRefreshBOM.Location = new System.Drawing.Point(285, 8);
            this.btnRefreshBOM.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefreshBOM.Name = "btnRefreshBOM";
            this.btnRefreshBOM.Size = new System.Drawing.Size(120, 45);
            this.btnRefreshBOM.TabIndex = 2;
            this.btnRefreshBOM.Text = "🔄 刷新";
            this.btnRefreshBOM.UseVisualStyleBackColor = false;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.grpRemarks);
            this.panelBottom.Controls.Add(this.btnSave);
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 810);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.panelBottom.Size = new System.Drawing.Size(1800, 150);
            this.panelBottom.TabIndex = 2;
            // 
            // grpRemarks
            // 
            this.grpRemarks.Controls.Add(this.lblRemarks);
            this.grpRemarks.Controls.Add(this.txtRemarks);
            this.grpRemarks.Controls.Add(this.lblCreatedBy);
            this.grpRemarks.Controls.Add(this.txtCreatedBy);
            this.grpRemarks.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpRemarks.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpRemarks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.grpRemarks.Location = new System.Drawing.Point(30, 30);
            this.grpRemarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRemarks.Name = "grpRemarks";
            this.grpRemarks.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpRemarks.Size = new System.Drawing.Size(1200, 90);
            this.grpRemarks.TabIndex = 0;
            this.grpRemarks.TabStop = false;
            this.grpRemarks.Text = "📝 备注信息";
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblRemarks.ForeColor = System.Drawing.Color.Black;
            this.lblRemarks.Location = new System.Drawing.Point(30, 38);
            this.lblRemarks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(64, 24);
            this.lblRemarks.TabIndex = 0;
            this.lblRemarks.Text = "备注：";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtRemarks.Location = new System.Drawing.Point(105, 33);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(598, 31);
            this.txtRemarks.TabIndex = 1;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblCreatedBy.ForeColor = System.Drawing.Color.Black;
            this.lblCreatedBy.Location = new System.Drawing.Point(735, 38);
            this.lblCreatedBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(82, 24);
            this.lblCreatedBy.TabIndex = 2;
            this.lblCreatedBy.Text = "创建人：";
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtCreatedBy.Location = new System.Drawing.Point(825, 33);
            this.txtCreatedBy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(178, 31);
            this.txtCreatedBy.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(1425, 52);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 52);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "💾 保存";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(1605, 52);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 52);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "❌ 取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // CreateWorkOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1800, 960);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateWorkOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建工单";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.grpBasicInfo.ResumeLayout(false);
            this.grpBasicInfo.PerformLayout();
            this.grpPlanInfo.ResumeLayout(false);
            this.grpPlanInfo.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.grpBOMList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOMList)).EndInit();
            this.panelBOMButtons.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.grpRemarks.ResumeLayout(false);
            this.grpRemarks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // 主面板控件
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelBottom;

        // 基本信息组控件
        private System.Windows.Forms.GroupBox grpBasicInfo;
        private System.Windows.Forms.Label lblWorkOrderType;
        private System.Windows.Forms.ComboBox cmbWorkOrderType;
        private System.Windows.Forms.Label lblWorkOrderDesc;
        private System.Windows.Forms.TextBox txtWorkOrderDesc;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.ComboBox cmbProductCode;
        private System.Windows.Forms.Label lblFinishedWorkOrder;
        private System.Windows.Forms.ComboBox cmbFinishedWorkOrder;
        private System.Windows.Forms.Label lblBOMCode;
        private System.Windows.Forms.TextBox txtBOMCode;
        private System.Windows.Forms.TextBox txtBOMVersion;
        private System.Windows.Forms.Button btnSelectBOM;

        // 计划信息组控件
        private System.Windows.Forms.GroupBox grpPlanInfo;
        private System.Windows.Forms.Label lblPlanStartDate;
        private System.Windows.Forms.DateTimePicker dtpPlanStartDate;
        private System.Windows.Forms.Label lblPlanEndDate;
        private System.Windows.Forms.DateTimePicker dtpPlanEndDate;
        private System.Windows.Forms.Label lblPlanQuantity;
        private System.Windows.Forms.TextBox txtPlanQuantity;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblProductType;
        private System.Windows.Forms.TextBox txtProductType;

        // BOM物料清单组控件
        private System.Windows.Forms.GroupBox grpBOMList;
        private System.Windows.Forms.DataGridView dgvBOMList;
        private System.Windows.Forms.Panel panelBOMButtons;
        private System.Windows.Forms.Button btnAddBOM;
        private System.Windows.Forms.Button btnRemoveBOM;
        private System.Windows.Forms.Button btnRefreshBOM;

        // 备注信息组控件
        private System.Windows.Forms.GroupBox grpRemarks;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.TextBox txtCreatedBy;

        // 操作按钮
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
