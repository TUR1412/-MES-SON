namespace MES.UI.Forms.Material
{
    partial class BOMManagementForm
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
            this.checkBoxHasExpireDate = new System.Windows.Forms.CheckBox();
            this.dateTimePickerExpireDate = new System.Windows.Forms.DateTimePicker();
            this.labelExpireDate = new System.Windows.Forms.Label();
            this.dateTimePickerEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.labelEffectiveDate = new System.Windows.Forms.Label();
            this.groupBoxMaterialInfo = new System.Windows.Forms.GroupBox();
            this.textBoxRemarks = new System.Windows.Forms.TextBox();
            this.labelRemarks = new System.Windows.Forms.Label();
            this.textBoxSubstituteMaterial = new System.Windows.Forms.TextBox();
            this.labelSubstituteMaterial = new System.Windows.Forms.Label();
            this.textBoxLossRate = new System.Windows.Forms.TextBox();
            this.labelLossRate = new System.Windows.Forms.Label();
            this.textBoxUnit = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.textBoxQuantity = new System.Windows.Forms.TextBox();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.textBoxMaterialName = new System.Windows.Forms.TextBox();
            this.labelMaterialName = new System.Windows.Forms.Label();
            this.textBoxMaterialCode = new System.Windows.Forms.TextBox();
            this.labelMaterialCode = new System.Windows.Forms.Label();
            this.groupBoxProductInfo = new System.Windows.Forms.GroupBox();
            this.textBoxProductName = new System.Windows.Forms.TextBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.textBoxProductCode = new System.Windows.Forms.TextBox();
            this.labelProductCode = new System.Windows.Forms.Label();
            this.groupBoxBasicInfo = new System.Windows.Forms.GroupBox();
            this.checkBoxStatus = new System.Windows.Forms.CheckBox();
            this.comboBoxBOMType = new System.Windows.Forms.ComboBox();
            this.labelBOMType = new System.Windows.Forms.Label();
            this.textBoxBOMVersion = new System.Windows.Forms.TextBox();
            this.labelBOMVersion = new System.Windows.Forms.Label();
            this.textBoxBOMCode = new System.Windows.Forms.TextBox();
            this.labelBOMCode = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.dataGridViewBOM = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            this.panelDetailsContent.SuspendLayout();
            this.groupBoxTimeInfo.SuspendLayout();
            this.groupBoxMaterialInfo.SuspendLayout();
            this.groupBoxProductInfo.SuspendLayout();
            this.groupBoxBasicInfo.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBOM)).BeginInit();
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
            this.labelTitle.Text = "📋 BOM物料清单管理";
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
            this.groupBoxDetails.Text = "📋 BOM详细信息";
            //
            // panelDetailsContent
            //
            this.panelDetailsContent.AutoScroll = true;
            this.panelDetailsContent.Controls.Add(this.groupBoxTimeInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxMaterialInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxProductInfo);
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
            this.groupBoxTimeInfo.Controls.Add(this.checkBoxHasExpireDate);
            this.groupBoxTimeInfo.Controls.Add(this.dateTimePickerExpireDate);
            this.groupBoxTimeInfo.Controls.Add(this.labelExpireDate);
            this.groupBoxTimeInfo.Controls.Add(this.dateTimePickerEffectiveDate);
            this.groupBoxTimeInfo.Controls.Add(this.labelEffectiveDate);
            this.groupBoxTimeInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTimeInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxTimeInfo.Location = new System.Drawing.Point(10, 490);
            this.groupBoxTimeInfo.Name = "groupBoxTimeInfo";
            this.groupBoxTimeInfo.Size = new System.Drawing.Size(551, 120);
            this.groupBoxTimeInfo.TabIndex = 3;
            this.groupBoxTimeInfo.TabStop = false;
            this.groupBoxTimeInfo.Text = "⏰ 时间信息";
            //
            // labelCreateTime
            //
            this.labelCreateTime.AutoSize = true;
            this.labelCreateTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelCreateTime.Location = new System.Drawing.Point(20, 90);
            this.labelCreateTime.Name = "labelCreateTime";
            this.labelCreateTime.Size = new System.Drawing.Size(68, 17);
            this.labelCreateTime.TabIndex = 5;
            this.labelCreateTime.Text = "创建时间：";
            //
            // checkBoxHasExpireDate
            //
            this.checkBoxHasExpireDate.AutoSize = true;
            this.checkBoxHasExpireDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.checkBoxHasExpireDate.Location = new System.Drawing.Point(20, 60);
            this.checkBoxHasExpireDate.Name = "checkBoxHasExpireDate";
            this.checkBoxHasExpireDate.Size = new System.Drawing.Size(75, 21);
            this.checkBoxHasExpireDate.TabIndex = 4;
            this.checkBoxHasExpireDate.Text = "失效日期";
            this.checkBoxHasExpireDate.UseVisualStyleBackColor = true;
            this.checkBoxHasExpireDate.CheckedChanged += new System.EventHandler(this.checkBoxHasExpireDate_CheckedChanged);
            //
            // dateTimePickerExpireDate
            //
            this.dateTimePickerExpireDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateTimePickerExpireDate.Location = new System.Drawing.Point(100, 58);
            this.dateTimePickerExpireDate.Name = "dateTimePickerExpireDate";
            this.dateTimePickerExpireDate.Size = new System.Drawing.Size(200, 23);
            this.dateTimePickerExpireDate.TabIndex = 3;
            //
            // labelExpireDate
            //
            this.labelExpireDate.AutoSize = true;
            this.labelExpireDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelExpireDate.Location = new System.Drawing.Point(320, 62);
            this.labelExpireDate.Name = "labelExpireDate";
            this.labelExpireDate.Size = new System.Drawing.Size(0, 17);
            this.labelExpireDate.TabIndex = 2;
            //
            // dateTimePickerEffectiveDate
            //
            this.dateTimePickerEffectiveDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateTimePickerEffectiveDate.Location = new System.Drawing.Point(100, 25);
            this.dateTimePickerEffectiveDate.Name = "dateTimePickerEffectiveDate";
            this.dateTimePickerEffectiveDate.Size = new System.Drawing.Size(200, 23);
            this.dateTimePickerEffectiveDate.TabIndex = 1;
            //
            // labelEffectiveDate
            //
            this.labelEffectiveDate.AutoSize = true;
            this.labelEffectiveDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelEffectiveDate.Location = new System.Drawing.Point(20, 28);
            this.labelEffectiveDate.Name = "labelEffectiveDate";
            this.labelEffectiveDate.Size = new System.Drawing.Size(68, 17);
            this.labelEffectiveDate.TabIndex = 0;
            this.labelEffectiveDate.Text = "生效日期：";
            //
            // groupBoxMaterialInfo - 简化版本，包含主要控件
            //
            this.groupBoxMaterialInfo.Controls.Add(this.textBoxRemarks);
            this.groupBoxMaterialInfo.Controls.Add(this.labelRemarks);
            this.groupBoxMaterialInfo.Controls.Add(this.textBoxSubstituteMaterial);
            this.groupBoxMaterialInfo.Controls.Add(this.labelSubstituteMaterial);
            this.groupBoxMaterialInfo.Controls.Add(this.textBoxLossRate);
            this.groupBoxMaterialInfo.Controls.Add(this.labelLossRate);
            this.groupBoxMaterialInfo.Controls.Add(this.textBoxUnit);
            this.groupBoxMaterialInfo.Controls.Add(this.labelUnit);
            this.groupBoxMaterialInfo.Controls.Add(this.textBoxQuantity);
            this.groupBoxMaterialInfo.Controls.Add(this.labelQuantity);
            this.groupBoxMaterialInfo.Controls.Add(this.textBoxMaterialName);
            this.groupBoxMaterialInfo.Controls.Add(this.labelMaterialName);
            this.groupBoxMaterialInfo.Controls.Add(this.textBoxMaterialCode);
            this.groupBoxMaterialInfo.Controls.Add(this.labelMaterialCode);
            this.groupBoxMaterialInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxMaterialInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxMaterialInfo.Location = new System.Drawing.Point(10, 250);
            this.groupBoxMaterialInfo.Name = "groupBoxMaterialInfo";
            this.groupBoxMaterialInfo.Size = new System.Drawing.Size(551, 240);
            this.groupBoxMaterialInfo.TabIndex = 2;
            this.groupBoxMaterialInfo.TabStop = false;
            this.groupBoxMaterialInfo.Text = "🔧 物料信息";
            // 简化控件定义
            this.textBoxMaterialCode.Location = new System.Drawing.Point(100, 25);
            this.textBoxMaterialCode.Name = "textBoxMaterialCode";
            this.textBoxMaterialCode.ReadOnly = true;
            this.textBoxMaterialCode.Size = new System.Drawing.Size(200, 23);
            this.labelMaterialCode.AutoSize = true;
            this.labelMaterialCode.Location = new System.Drawing.Point(20, 28);
            this.labelMaterialCode.Text = "物料编码：";
            this.textBoxMaterialName.Location = new System.Drawing.Point(100, 55);
            this.textBoxMaterialName.Name = "textBoxMaterialName";
            this.textBoxMaterialName.ReadOnly = true;
            this.textBoxMaterialName.Size = new System.Drawing.Size(200, 23);
            this.labelMaterialName.AutoSize = true;
            this.labelMaterialName.Location = new System.Drawing.Point(20, 58);
            this.labelMaterialName.Text = "物料名称：";
            this.textBoxQuantity.Location = new System.Drawing.Point(100, 85);
            this.textBoxQuantity.Name = "textBoxQuantity";
            this.textBoxQuantity.ReadOnly = true;
            this.textBoxQuantity.Size = new System.Drawing.Size(100, 23);
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Location = new System.Drawing.Point(20, 88);
            this.labelQuantity.Text = "需求数量：";
            this.textBoxUnit.Location = new System.Drawing.Point(220, 85);
            this.textBoxUnit.Name = "textBoxUnit";
            this.textBoxUnit.ReadOnly = true;
            this.textBoxUnit.Size = new System.Drawing.Size(80, 23);
            this.labelUnit.AutoSize = true;
            this.labelUnit.Location = new System.Drawing.Point(210, 88);
            this.labelUnit.Text = "单位：";
            this.textBoxLossRate.Location = new System.Drawing.Point(100, 115);
            this.textBoxLossRate.Name = "textBoxLossRate";
            this.textBoxLossRate.ReadOnly = true;
            this.textBoxLossRate.Size = new System.Drawing.Size(100, 23);
            this.labelLossRate.AutoSize = true;
            this.labelLossRate.Location = new System.Drawing.Point(20, 118);
            this.labelLossRate.Text = "损耗率(%)：";
            this.textBoxSubstituteMaterial.Location = new System.Drawing.Point(100, 145);
            this.textBoxSubstituteMaterial.Name = "textBoxSubstituteMaterial";
            this.textBoxSubstituteMaterial.ReadOnly = true;
            this.textBoxSubstituteMaterial.Size = new System.Drawing.Size(200, 23);
            this.labelSubstituteMaterial.AutoSize = true;
            this.labelSubstituteMaterial.Location = new System.Drawing.Point(20, 148);
            this.labelSubstituteMaterial.Text = "替代料：";
            this.textBoxRemarks.Location = new System.Drawing.Point(100, 175);
            this.textBoxRemarks.Multiline = true;
            this.textBoxRemarks.Name = "textBoxRemarks";
            this.textBoxRemarks.ReadOnly = true;
            this.textBoxRemarks.Size = new System.Drawing.Size(400, 50);
            this.labelRemarks.AutoSize = true;
            this.labelRemarks.Location = new System.Drawing.Point(20, 178);
            this.labelRemarks.Text = "备注：";
            //
            // groupBoxProductInfo - 简化版本
            //
            this.groupBoxProductInfo.Controls.Add(this.textBoxProductName);
            this.groupBoxProductInfo.Controls.Add(this.labelProductName);
            this.groupBoxProductInfo.Controls.Add(this.textBoxProductCode);
            this.groupBoxProductInfo.Controls.Add(this.labelProductCode);
            this.groupBoxProductInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProductInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxProductInfo.Location = new System.Drawing.Point(10, 130);
            this.groupBoxProductInfo.Name = "groupBoxProductInfo";
            this.groupBoxProductInfo.Size = new System.Drawing.Size(551, 120);
            this.groupBoxProductInfo.TabIndex = 1;
            this.groupBoxProductInfo.TabStop = false;
            this.groupBoxProductInfo.Text = "📦 产品信息";
            this.textBoxProductCode.Location = new System.Drawing.Point(100, 25);
            this.textBoxProductCode.Name = "textBoxProductCode";
            this.textBoxProductCode.ReadOnly = true;
            this.textBoxProductCode.Size = new System.Drawing.Size(200, 23);
            this.labelProductCode.AutoSize = true;
            this.labelProductCode.Location = new System.Drawing.Point(20, 28);
            this.labelProductCode.Text = "产品编码：";
            this.textBoxProductName.Location = new System.Drawing.Point(100, 55);
            this.textBoxProductName.Name = "textBoxProductName";
            this.textBoxProductName.ReadOnly = true;
            this.textBoxProductName.Size = new System.Drawing.Size(200, 23);
            this.labelProductName.AutoSize = true;
            this.labelProductName.Location = new System.Drawing.Point(20, 58);
            this.labelProductName.Text = "产品名称：";
            //
            // groupBoxBasicInfo - 简化版本
            //
            this.groupBoxBasicInfo.Controls.Add(this.checkBoxStatus);
            this.groupBoxBasicInfo.Controls.Add(this.comboBoxBOMType);
            this.groupBoxBasicInfo.Controls.Add(this.labelBOMType);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxBOMVersion);
            this.groupBoxBasicInfo.Controls.Add(this.labelBOMVersion);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxBOMCode);
            this.groupBoxBasicInfo.Controls.Add(this.labelBOMCode);
            this.groupBoxBasicInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasicInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxBasicInfo.Location = new System.Drawing.Point(10, 10);
            this.groupBoxBasicInfo.Name = "groupBoxBasicInfo";
            this.groupBoxBasicInfo.Size = new System.Drawing.Size(551, 120);
            this.groupBoxBasicInfo.TabIndex = 0;
            this.groupBoxBasicInfo.TabStop = false;
            this.groupBoxBasicInfo.Text = "📋 基本信息";
            this.textBoxBOMCode.Location = new System.Drawing.Point(100, 25);
            this.textBoxBOMCode.Name = "textBoxBOMCode";
            this.textBoxBOMCode.ReadOnly = true;
            this.textBoxBOMCode.Size = new System.Drawing.Size(200, 23);
            this.labelBOMCode.AutoSize = true;
            this.labelBOMCode.Location = new System.Drawing.Point(20, 28);
            this.labelBOMCode.Text = "BOM编码：";
            this.textBoxBOMVersion.Location = new System.Drawing.Point(100, 55);
            this.textBoxBOMVersion.Name = "textBoxBOMVersion";
            this.textBoxBOMVersion.ReadOnly = true;
            this.textBoxBOMVersion.Size = new System.Drawing.Size(100, 23);
            this.labelBOMVersion.AutoSize = true;
            this.labelBOMVersion.Location = new System.Drawing.Point(20, 58);
            this.labelBOMVersion.Text = "版本：";
            this.comboBoxBOMType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBOMType.Items.AddRange(new object[] { "PRODUCTION", "ENGINEERING" });
            this.comboBoxBOMType.Location = new System.Drawing.Point(300, 55);
            this.comboBoxBOMType.Name = "comboBoxBOMType";
            this.comboBoxBOMType.Size = new System.Drawing.Size(120, 25);
            this.labelBOMType.AutoSize = true;
            this.labelBOMType.Location = new System.Drawing.Point(250, 58);
            this.labelBOMType.Text = "类型：";
            this.checkBoxStatus.AutoSize = true;
            this.checkBoxStatus.Location = new System.Drawing.Point(100, 85);
            this.checkBoxStatus.Name = "checkBoxStatus";
            this.checkBoxStatus.Size = new System.Drawing.Size(51, 21);
            this.checkBoxStatus.Text = "启用";
            this.checkBoxStatus.UseVisualStyleBackColor = true;
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
            this.panelLeft.Controls.Add(this.dataGridViewBOM);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelLeft.Size = new System.Drawing.Size(800, 630);
            this.panelLeft.TabIndex = 0;
            //
            // dataGridViewBOM
            //
            this.dataGridViewBOM.AllowUserToAddRows = false;
            this.dataGridViewBOM.AllowUserToDeleteRows = false;
            this.dataGridViewBOM.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewBOM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBOM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBOM.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewBOM.MultiSelect = false;
            this.dataGridViewBOM.Name = "dataGridViewBOM";
            this.dataGridViewBOM.ReadOnly = true;
            this.dataGridViewBOM.RowHeadersVisible = false;
            this.dataGridViewBOM.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBOM.Size = new System.Drawing.Size(780, 610);
            this.dataGridViewBOM.TabIndex = 0;
            this.dataGridViewBOM.SelectionChanged += new System.EventHandler(this.dataGridViewBOM_SelectionChanged);
            //
            // BOMManagementForm
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
            this.Name = "BOMManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOM物料清单管理";
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
            this.groupBoxMaterialInfo.ResumeLayout(false);
            this.groupBoxMaterialInfo.PerformLayout();
            this.groupBoxProductInfo.ResumeLayout(false);
            this.groupBoxProductInfo.PerformLayout();
            this.groupBoxBasicInfo.ResumeLayout(false);
            this.groupBoxBasicInfo.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBOM)).EndInit();
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
        private System.Windows.Forms.CheckBox checkBoxHasExpireDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerExpireDate;
        private System.Windows.Forms.Label labelExpireDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerEffectiveDate;
        private System.Windows.Forms.Label labelEffectiveDate;
        private System.Windows.Forms.GroupBox groupBoxMaterialInfo;
        private System.Windows.Forms.TextBox textBoxRemarks;
        private System.Windows.Forms.Label labelRemarks;
        private System.Windows.Forms.TextBox textBoxSubstituteMaterial;
        private System.Windows.Forms.Label labelSubstituteMaterial;
        private System.Windows.Forms.TextBox textBoxLossRate;
        private System.Windows.Forms.Label labelLossRate;
        private System.Windows.Forms.TextBox textBoxUnit;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.TextBox textBoxQuantity;
        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.TextBox textBoxMaterialName;
        private System.Windows.Forms.Label labelMaterialName;
        private System.Windows.Forms.TextBox textBoxMaterialCode;
        private System.Windows.Forms.Label labelMaterialCode;
        private System.Windows.Forms.GroupBox groupBoxProductInfo;
        private System.Windows.Forms.TextBox textBoxProductName;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TextBox textBoxProductCode;
        private System.Windows.Forms.Label labelProductCode;
        private System.Windows.Forms.GroupBox groupBoxBasicInfo;
        private System.Windows.Forms.CheckBox checkBoxStatus;
        private System.Windows.Forms.ComboBox comboBoxBOMType;
        private System.Windows.Forms.Label labelBOMType;
        private System.Windows.Forms.TextBox textBoxBOMVersion;
        private System.Windows.Forms.Label labelBOMVersion;
        private System.Windows.Forms.TextBox textBoxBOMCode;
        private System.Windows.Forms.Label labelBOMCode;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.DataGridView dataGridViewBOM;
    }
}