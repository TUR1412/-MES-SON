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
            // 主面板
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            
            // 左侧基本信息区域
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
            
            // 计划信息区域
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
            
            // 右侧BOM物料清单区域
            this.grpBOMList = new System.Windows.Forms.GroupBox();
            this.dgvBOMList = new System.Windows.Forms.DataGridView();
            this.panelBOMButtons = new System.Windows.Forms.Panel();
            this.btnAddBOM = new System.Windows.Forms.Button();
            this.btnRemoveBOM = new System.Windows.Forms.Button();
            this.btnRefreshBOM = new System.Windows.Forms.Button();
            
            // 底部操作区域
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
            this.panelRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.grpBasicInfo.SuspendLayout();
            this.grpPlanInfo.SuspendLayout();
            this.grpBOMList.SuspendLayout();
            this.grpRemarks.SuspendLayout();
            this.panelBOMButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOMList)).BeginInit();
            this.SuspendLayout();
            
            // 
            // panelTop - 顶部标题区域
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 60);
            this.panelTop.TabIndex = 0;
            
            // 
            // lblTitle - 窗体标题
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(181, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "➕ 创建工单";
            
            // 
            // panelMain - 主内容区域
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 60);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(1200, 480);
            this.panelMain.TabIndex = 1;
            
            // 
            // panelLeft - 左侧信息面板
            // 
            this.panelLeft.Controls.Add(this.grpBasicInfo);
            this.panelLeft.Controls.Add(this.grpPlanInfo);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(20, 20);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(580, 440);
            this.panelLeft.TabIndex = 0;
            
            // 
            // grpBasicInfo - 基本信息组
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
            this.grpBasicInfo.Name = "grpBasicInfo";
            this.grpBasicInfo.Size = new System.Drawing.Size(580, 220);
            this.grpBasicInfo.TabIndex = 0;
            this.grpBasicInfo.TabStop = false;
            this.grpBasicInfo.Text = "📋 基本信息";
            
            // 
            // lblWorkOrderType - 工单类型标签
            // 
            this.lblWorkOrderType.AutoSize = true;
            this.lblWorkOrderType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblWorkOrderType.ForeColor = System.Drawing.Color.Black;
            this.lblWorkOrderType.Location = new System.Drawing.Point(20, 35);
            this.lblWorkOrderType.Name = "lblWorkOrderType";
            this.lblWorkOrderType.Size = new System.Drawing.Size(68, 17);
            this.lblWorkOrderType.TabIndex = 0;
            this.lblWorkOrderType.Text = "工单类型：";
            
            // 
            // cmbWorkOrderType - 工单类型下拉框
            // 
            this.cmbWorkOrderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkOrderType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbWorkOrderType.FormattingEnabled = true;
            this.cmbWorkOrderType.Items.AddRange(new object[] {
            "生产工单",
            "维修工单",
            "测试工单",
            "返工工单"});
            this.cmbWorkOrderType.Location = new System.Drawing.Point(120, 32);
            this.cmbWorkOrderType.Name = "cmbWorkOrderType";
            this.cmbWorkOrderType.Size = new System.Drawing.Size(150, 25);
            this.cmbWorkOrderType.TabIndex = 1;
            
            // 
            // lblWorkOrderDesc - 工单说明标签
            // 
            this.lblWorkOrderDesc.AutoSize = true;
            this.lblWorkOrderDesc.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblWorkOrderDesc.ForeColor = System.Drawing.Color.Black;
            this.lblWorkOrderDesc.Location = new System.Drawing.Point(20, 70);
            this.lblWorkOrderDesc.Name = "lblWorkOrderDesc";
            this.lblWorkOrderDesc.Size = new System.Drawing.Size(68, 17);
            this.lblWorkOrderDesc.TabIndex = 2;
            this.lblWorkOrderDesc.Text = "工单说明：";
            
            // 
            // txtWorkOrderDesc - 工单说明文本框
            // 
            this.txtWorkOrderDesc.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtWorkOrderDesc.Location = new System.Drawing.Point(120, 67);
            this.txtWorkOrderDesc.Name = "txtWorkOrderDesc";
            this.txtWorkOrderDesc.Size = new System.Drawing.Size(430, 23);
            this.txtWorkOrderDesc.TabIndex = 3;
            
            // 
            // lblProductCode - 产品编号标签
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblProductCode.ForeColor = System.Drawing.Color.Black;
            this.lblProductCode.Location = new System.Drawing.Point(20, 105);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(68, 17);
            this.lblProductCode.TabIndex = 4;
            this.lblProductCode.Text = "产品编号：";
            
            // 
            // cmbProductCode - 产品编号下拉框
            // 
            this.cmbProductCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbProductCode.FormattingEnabled = true;
            this.cmbProductCode.Location = new System.Drawing.Point(120, 102);
            this.cmbProductCode.Name = "cmbProductCode";
            this.cmbProductCode.Size = new System.Drawing.Size(200, 25);
            this.cmbProductCode.TabIndex = 5;
            
            // 
            // lblFinishedWorkOrder - 成品工单号标签
            // 
            this.lblFinishedWorkOrder.AutoSize = true;
            this.lblFinishedWorkOrder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblFinishedWorkOrder.ForeColor = System.Drawing.Color.Black;
            this.lblFinishedWorkOrder.Location = new System.Drawing.Point(340, 105);
            this.lblFinishedWorkOrder.Name = "lblFinishedWorkOrder";
            this.lblFinishedWorkOrder.Size = new System.Drawing.Size(80, 17);
            this.lblFinishedWorkOrder.TabIndex = 6;
            this.lblFinishedWorkOrder.Text = "成品工单号：";
            
            // 
            // cmbFinishedWorkOrder - 成品工单号下拉框
            // 
            this.cmbFinishedWorkOrder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbFinishedWorkOrder.FormattingEnabled = true;
            this.cmbFinishedWorkOrder.Location = new System.Drawing.Point(430, 102);
            this.cmbFinishedWorkOrder.Name = "cmbFinishedWorkOrder";
            this.cmbFinishedWorkOrder.Size = new System.Drawing.Size(120, 25);
            this.cmbFinishedWorkOrder.TabIndex = 7;

            //
            // lblBOMCode - BOM编号标签
            //
            this.lblBOMCode.AutoSize = true;
            this.lblBOMCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblBOMCode.ForeColor = System.Drawing.Color.Black;
            this.lblBOMCode.Location = new System.Drawing.Point(20, 140);
            this.lblBOMCode.Name = "lblBOMCode";
            this.lblBOMCode.Size = new System.Drawing.Size(68, 17);
            this.lblBOMCode.TabIndex = 8;
            this.lblBOMCode.Text = "BOM编号：";

            //
            // txtBOMCode - BOM编号文本框
            //
            this.txtBOMCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBOMCode.Location = new System.Drawing.Point(120, 137);
            this.txtBOMCode.Name = "txtBOMCode";
            this.txtBOMCode.ReadOnly = true;
            this.txtBOMCode.Size = new System.Drawing.Size(150, 23);
            this.txtBOMCode.TabIndex = 9;

            //
            // txtBOMVersion - BOM版本文本框
            //
            this.txtBOMVersion.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBOMVersion.Location = new System.Drawing.Point(280, 137);
            this.txtBOMVersion.Name = "txtBOMVersion";
            this.txtBOMVersion.ReadOnly = true;
            this.txtBOMVersion.Size = new System.Drawing.Size(80, 23);
            this.txtBOMVersion.TabIndex = 10;

            //
            // btnSelectBOM - 选择BOM按钮
            //
            this.btnSelectBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSelectBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSelectBOM.ForeColor = System.Drawing.Color.White;
            this.btnSelectBOM.Location = new System.Drawing.Point(370, 135);
            this.btnSelectBOM.Name = "btnSelectBOM";
            this.btnSelectBOM.Size = new System.Drawing.Size(80, 27);
            this.btnSelectBOM.TabIndex = 11;
            this.btnSelectBOM.Text = "🔍 选择BOM";
            this.btnSelectBOM.UseVisualStyleBackColor = false;

            //
            // grpPlanInfo - 计划信息组
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
            this.grpPlanInfo.Location = new System.Drawing.Point(0, 220);
            this.grpPlanInfo.Name = "grpPlanInfo";
            this.grpPlanInfo.Size = new System.Drawing.Size(580, 220);
            this.grpPlanInfo.TabIndex = 1;
            this.grpPlanInfo.TabStop = false;
            this.grpPlanInfo.Text = "📅 计划信息";

            //
            // lblPlanStartDate - 计划开始日期标签
            //
            this.lblPlanStartDate.AutoSize = true;
            this.lblPlanStartDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblPlanStartDate.ForeColor = System.Drawing.Color.Black;
            this.lblPlanStartDate.Location = new System.Drawing.Point(20, 35);
            this.lblPlanStartDate.Name = "lblPlanStartDate";
            this.lblPlanStartDate.Size = new System.Drawing.Size(92, 17);
            this.lblPlanStartDate.TabIndex = 0;
            this.lblPlanStartDate.Text = "计划开始日期：";

            //
            // dtpPlanStartDate - 计划开始日期选择器
            //
            this.dtpPlanStartDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dtpPlanStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPlanStartDate.Location = new System.Drawing.Point(120, 32);
            this.dtpPlanStartDate.Name = "dtpPlanStartDate";
            this.dtpPlanStartDate.Size = new System.Drawing.Size(150, 23);
            this.dtpPlanStartDate.TabIndex = 1;

            //
            // lblPlanEndDate - 计划结束日期标签
            //
            this.lblPlanEndDate.AutoSize = true;
            this.lblPlanEndDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblPlanEndDate.ForeColor = System.Drawing.Color.Black;
            this.lblPlanEndDate.Location = new System.Drawing.Point(290, 35);
            this.lblPlanEndDate.Name = "lblPlanEndDate";
            this.lblPlanEndDate.Size = new System.Drawing.Size(92, 17);
            this.lblPlanEndDate.TabIndex = 2;
            this.lblPlanEndDate.Text = "计划结束日期：";

            //
            // dtpPlanEndDate - 计划结束日期选择器
            //
            this.dtpPlanEndDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dtpPlanEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPlanEndDate.Location = new System.Drawing.Point(390, 32);
            this.dtpPlanEndDate.Name = "dtpPlanEndDate";
            this.dtpPlanEndDate.Size = new System.Drawing.Size(150, 23);
            this.dtpPlanEndDate.TabIndex = 3;

            //
            // lblPlanQuantity - 计划数量标签
            //
            this.lblPlanQuantity.AutoSize = true;
            this.lblPlanQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblPlanQuantity.ForeColor = System.Drawing.Color.Black;
            this.lblPlanQuantity.Location = new System.Drawing.Point(20, 70);
            this.lblPlanQuantity.Name = "lblPlanQuantity";
            this.lblPlanQuantity.Size = new System.Drawing.Size(68, 17);
            this.lblPlanQuantity.TabIndex = 4;
            this.lblPlanQuantity.Text = "计划数量：";

            //
            // txtPlanQuantity - 计划数量文本框
            //
            this.txtPlanQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtPlanQuantity.Location = new System.Drawing.Point(120, 67);
            this.txtPlanQuantity.Name = "txtPlanQuantity";
            this.txtPlanQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtPlanQuantity.TabIndex = 5;

            //
            // lblUnit - 单位标签
            //
            this.lblUnit.AutoSize = true;
            this.lblUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblUnit.ForeColor = System.Drawing.Color.Black;
            this.lblUnit.Location = new System.Drawing.Point(240, 70);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(44, 17);
            this.lblUnit.TabIndex = 6;
            this.lblUnit.Text = "单位：";

            //
            // txtUnit - 单位文本框
            //
            this.txtUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtUnit.Location = new System.Drawing.Point(290, 67);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(80, 23);
            this.txtUnit.TabIndex = 7;

            //
            // lblProductType - 产品类型标签
            //
            this.lblProductType.AutoSize = true;
            this.lblProductType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblProductType.ForeColor = System.Drawing.Color.Black;
            this.lblProductType.Location = new System.Drawing.Point(390, 70);
            this.lblProductType.Name = "lblProductType";
            this.lblProductType.Size = new System.Drawing.Size(68, 17);
            this.lblProductType.TabIndex = 8;
            this.lblProductType.Text = "产品类型：";

            //
            // txtProductType - 产品类型文本框
            //
            this.txtProductType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtProductType.Location = new System.Drawing.Point(460, 67);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(100, 23);
            this.txtProductType.TabIndex = 9;

            //
            // panelRight - 右侧BOM面板
            //
            this.panelRight.Controls.Add(this.grpBOMList);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(600, 20);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.panelRight.Size = new System.Drawing.Size(580, 440);
            this.panelRight.TabIndex = 1;

            //
            // grpBOMList - BOM物料清单组
            //
            this.grpBOMList.Controls.Add(this.dgvBOMList);
            this.grpBOMList.Controls.Add(this.panelBOMButtons);
            this.grpBOMList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBOMList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpBOMList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.grpBOMList.Location = new System.Drawing.Point(20, 0);
            this.grpBOMList.Name = "grpBOMList";
            this.grpBOMList.Size = new System.Drawing.Size(560, 440);
            this.grpBOMList.TabIndex = 0;
            this.grpBOMList.TabStop = false;
            this.grpBOMList.Text = "📦 BOM物料清单";

            //
            // dgvBOMList - BOM物料清单数据网格
            //
            this.dgvBOMList.AllowUserToAddRows = false;
            this.dgvBOMList.AllowUserToDeleteRows = false;
            this.dgvBOMList.BackgroundColor = System.Drawing.Color.White;
            this.dgvBOMList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBOMList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBOMList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBOMList.Location = new System.Drawing.Point(3, 19);
            this.dgvBOMList.MultiSelect = false;
            this.dgvBOMList.Name = "dgvBOMList";
            this.dgvBOMList.ReadOnly = true;
            this.dgvBOMList.RowTemplate.Height = 25;
            this.dgvBOMList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBOMList.Size = new System.Drawing.Size(554, 378);
            this.dgvBOMList.TabIndex = 0;

            //
            // panelBOMButtons - BOM操作按钮面板
            //
            this.panelBOMButtons.Controls.Add(this.btnAddBOM);
            this.panelBOMButtons.Controls.Add(this.btnRemoveBOM);
            this.panelBOMButtons.Controls.Add(this.btnRefreshBOM);
            this.panelBOMButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBOMButtons.Location = new System.Drawing.Point(3, 397);
            this.panelBOMButtons.Name = "panelBOMButtons";
            this.panelBOMButtons.Size = new System.Drawing.Size(554, 40);
            this.panelBOMButtons.TabIndex = 1;

            //
            // btnAddBOM - 添加BOM按钮
            //
            this.btnAddBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnAddBOM.ForeColor = System.Drawing.Color.White;
            this.btnAddBOM.Location = new System.Drawing.Point(10, 5);
            this.btnAddBOM.Name = "btnAddBOM";
            this.btnAddBOM.Size = new System.Drawing.Size(80, 30);
            this.btnAddBOM.TabIndex = 0;
            this.btnAddBOM.Text = "➕ 添加";
            this.btnAddBOM.UseVisualStyleBackColor = false;

            //
            // btnRemoveBOM - 移除BOM按钮
            //
            this.btnRemoveBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRemoveBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRemoveBOM.ForeColor = System.Drawing.Color.White;
            this.btnRemoveBOM.Location = new System.Drawing.Point(100, 5);
            this.btnRemoveBOM.Name = "btnRemoveBOM";
            this.btnRemoveBOM.Size = new System.Drawing.Size(80, 30);
            this.btnRemoveBOM.TabIndex = 1;
            this.btnRemoveBOM.Text = "➖ 移除";
            this.btnRemoveBOM.UseVisualStyleBackColor = false;

            //
            // btnRefreshBOM - 刷新BOM按钮
            //
            this.btnRefreshBOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefreshBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshBOM.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefreshBOM.ForeColor = System.Drawing.Color.White;
            this.btnRefreshBOM.Location = new System.Drawing.Point(190, 5);
            this.btnRefreshBOM.Name = "btnRefreshBOM";
            this.btnRefreshBOM.Size = new System.Drawing.Size(80, 30);
            this.btnRefreshBOM.TabIndex = 2;
            this.btnRefreshBOM.Text = "🔄 刷新";
            this.btnRefreshBOM.UseVisualStyleBackColor = false;

            //
            // panelBottom - 底部操作面板
            //
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.grpRemarks);
            this.panelBottom.Controls.Add(this.btnSave);
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 540);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(20);
            this.panelBottom.Size = new System.Drawing.Size(1200, 100);
            this.panelBottom.TabIndex = 2;

            //
            // grpRemarks - 备注信息组
            //
            this.grpRemarks.Controls.Add(this.lblRemarks);
            this.grpRemarks.Controls.Add(this.txtRemarks);
            this.grpRemarks.Controls.Add(this.lblCreatedBy);
            this.grpRemarks.Controls.Add(this.txtCreatedBy);
            this.grpRemarks.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpRemarks.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpRemarks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.grpRemarks.Location = new System.Drawing.Point(20, 20);
            this.grpRemarks.Name = "grpRemarks";
            this.grpRemarks.Size = new System.Drawing.Size(800, 60);
            this.grpRemarks.TabIndex = 0;
            this.grpRemarks.TabStop = false;
            this.grpRemarks.Text = "📝 备注信息";

            //
            // lblRemarks - 备注标签
            //
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblRemarks.ForeColor = System.Drawing.Color.Black;
            this.lblRemarks.Location = new System.Drawing.Point(20, 25);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(44, 17);
            this.lblRemarks.TabIndex = 0;
            this.lblRemarks.Text = "备注：";

            //
            // txtRemarks - 备注文本框
            //
            this.txtRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtRemarks.Location = new System.Drawing.Point(70, 22);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(400, 23);
            this.txtRemarks.TabIndex = 1;

            //
            // lblCreatedBy - 创建人标签
            //
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblCreatedBy.ForeColor = System.Drawing.Color.Black;
            this.lblCreatedBy.Location = new System.Drawing.Point(490, 25);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(56, 17);
            this.lblCreatedBy.TabIndex = 2;
            this.lblCreatedBy.Text = "创建人：";

            //
            // txtCreatedBy - 创建人文本框
            //
            this.txtCreatedBy.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtCreatedBy.Location = new System.Drawing.Point(550, 22);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(120, 23);
            this.txtCreatedBy.TabIndex = 3;

            //
            // btnSave - 保存按钮
            //
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(950, 35);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "💾 保存";
            this.btnSave.UseVisualStyleBackColor = false;

            //
            // btnCancel - 取消按钮
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(1070, 35);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "❌ 取消";
            this.btnCancel.UseVisualStyleBackColor = false;

            //
            // CreateWorkOrder - 创建工单窗体
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1200, 640);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateWorkOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建工单";

            // 布局恢复
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.grpBasicInfo.ResumeLayout(false);
            this.grpBasicInfo.PerformLayout();
            this.grpPlanInfo.ResumeLayout(false);
            this.grpPlanInfo.PerformLayout();
            this.grpBOMList.ResumeLayout(false);
            this.grpRemarks.ResumeLayout(false);
            this.grpRemarks.PerformLayout();
            this.panelBOMButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOMList)).EndInit();
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
