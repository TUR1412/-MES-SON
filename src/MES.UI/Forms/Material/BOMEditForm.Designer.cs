namespace MES.UI.Forms.Material
{
    partial class BOMEditForm
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
            this.lblBOMCode = new System.Windows.Forms.Label();
            this.txtBOMCode = new System.Windows.Forms.TextBox();
            this.lblBOMVersion = new System.Windows.Forms.Label();
            this.txtBOMVersion = new System.Windows.Forms.TextBox();
            this.lblBOMType = new System.Windows.Forms.Label();
            this.cmbBOMType = new System.Windows.Forms.ComboBox();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblMaterialCode = new System.Windows.Forms.Label();
            this.txtMaterialCode = new System.Windows.Forms.TextBox();
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.txtMaterialName = new System.Windows.Forms.TextBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblLossRate = new System.Windows.Forms.Label();
            this.txtLossRate = new System.Windows.Forms.TextBox();
            this.lblSubstituteMaterial = new System.Windows.Forms.Label();
            this.txtSubstituteMaterial = new System.Windows.Forms.TextBox();
            this.lblEffectiveDate = new System.Windows.Forms.Label();
            this.dtpEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.chkHasExpireDate = new System.Windows.Forms.CheckBox();
            this.dtpExpireDate = new System.Windows.Forms.DateTimePicker();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBOMCode
            // 
            this.lblBOMCode.AutoSize = true;
            this.lblBOMCode.Location = new System.Drawing.Point(30, 30);
            this.lblBOMCode.Name = "lblBOMCode";
            this.lblBOMCode.Size = new System.Drawing.Size(65, 12);
            this.lblBOMCode.TabIndex = 0;
            this.lblBOMCode.Text = "BOM编码：";
            // 
            // txtBOMCode
            // 
            this.txtBOMCode.Location = new System.Drawing.Point(120, 27);
            this.txtBOMCode.Name = "txtBOMCode";
            this.txtBOMCode.Size = new System.Drawing.Size(200, 21);
            this.txtBOMCode.TabIndex = 1;
            // 
            // lblBOMVersion
            // 
            this.lblBOMVersion.AutoSize = true;
            this.lblBOMVersion.Location = new System.Drawing.Point(350, 30);
            this.lblBOMVersion.Name = "lblBOMVersion";
            this.lblBOMVersion.Size = new System.Drawing.Size(65, 12);
            this.lblBOMVersion.TabIndex = 2;
            this.lblBOMVersion.Text = "BOM版本：";
            // 
            // txtBOMVersion
            // 
            this.txtBOMVersion.Location = new System.Drawing.Point(430, 27);
            this.txtBOMVersion.Name = "txtBOMVersion";
            this.txtBOMVersion.Size = new System.Drawing.Size(120, 21);
            this.txtBOMVersion.TabIndex = 3;
            // 
            // lblBOMType
            // 
            this.lblBOMType.AutoSize = true;
            this.lblBOMType.Location = new System.Drawing.Point(30, 70);
            this.lblBOMType.Name = "lblBOMType";
            this.lblBOMType.Size = new System.Drawing.Size(65, 12);
            this.lblBOMType.TabIndex = 4;
            this.lblBOMType.Text = "BOM类型：";
            // 
            // cmbBOMType
            // 
            this.cmbBOMType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBOMType.FormattingEnabled = true;
            this.cmbBOMType.Items.AddRange(new object[] {
            "PRODUCTION",
            "ENGINEERING",
            "MAINTENANCE"});
            this.cmbBOMType.Location = new System.Drawing.Point(120, 67);
            this.cmbBOMType.Name = "cmbBOMType";
            this.cmbBOMType.Size = new System.Drawing.Size(200, 20);
            this.cmbBOMType.TabIndex = 5;
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Location = new System.Drawing.Point(30, 110);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(65, 12);
            this.lblProductCode.TabIndex = 6;
            this.lblProductCode.Text = "产品编码：";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(120, 107);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(200, 21);
            this.txtProductCode.TabIndex = 7;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(350, 110);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(65, 12);
            this.lblProductName.TabIndex = 8;
            this.lblProductName.Text = "产品名称：";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(430, 107);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(120, 21);
            this.txtProductName.TabIndex = 9;
            // 
            // lblMaterialCode
            // 
            this.lblMaterialCode.AutoSize = true;
            this.lblMaterialCode.Location = new System.Drawing.Point(30, 150);
            this.lblMaterialCode.Name = "lblMaterialCode";
            this.lblMaterialCode.Size = new System.Drawing.Size(65, 12);
            this.lblMaterialCode.TabIndex = 10;
            this.lblMaterialCode.Text = "物料编码：";
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Location = new System.Drawing.Point(120, 147);
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(200, 21);
            this.txtMaterialCode.TabIndex = 11;
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.AutoSize = true;
            this.lblMaterialName.Location = new System.Drawing.Point(350, 150);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(65, 12);
            this.lblMaterialName.TabIndex = 12;
            this.lblMaterialName.Text = "物料名称：";
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.Location = new System.Drawing.Point(430, 147);
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new System.Drawing.Size(120, 21);
            this.txtMaterialName.TabIndex = 13;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(30, 190);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(41, 12);
            this.lblQuantity.TabIndex = 14;
            this.lblQuantity.Text = "数量：";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(120, 187);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(100, 21);
            this.txtQuantity.TabIndex = 15;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(250, 190);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(41, 12);
            this.lblUnit.TabIndex = 16;
            this.lblUnit.Text = "单位：";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(300, 187);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(80, 21);
            this.txtUnit.TabIndex = 17;
            // 
            // lblLossRate
            // 
            this.lblLossRate.AutoSize = true;
            this.lblLossRate.Location = new System.Drawing.Point(400, 190);
            this.lblLossRate.Name = "lblLossRate";
            this.lblLossRate.Size = new System.Drawing.Size(65, 12);
            this.lblLossRate.TabIndex = 18;
            this.lblLossRate.Text = "损耗率(%)：";
            // 
            // txtLossRate
            // 
            this.txtLossRate.Location = new System.Drawing.Point(480, 187);
            this.txtLossRate.Name = "txtLossRate";
            this.txtLossRate.Size = new System.Drawing.Size(70, 21);
            this.txtLossRate.TabIndex = 19;
            // 
            // lblSubstituteMaterial
            // 
            this.lblSubstituteMaterial.AutoSize = true;
            this.lblSubstituteMaterial.Location = new System.Drawing.Point(30, 230);
            this.lblSubstituteMaterial.Name = "lblSubstituteMaterial";
            this.lblSubstituteMaterial.Size = new System.Drawing.Size(65, 12);
            this.lblSubstituteMaterial.TabIndex = 20;
            this.lblSubstituteMaterial.Text = "替代物料：";
            // 
            // txtSubstituteMaterial
            // 
            this.txtSubstituteMaterial.Location = new System.Drawing.Point(120, 227);
            this.txtSubstituteMaterial.Name = "txtSubstituteMaterial";
            this.txtSubstituteMaterial.Size = new System.Drawing.Size(430, 21);
            this.txtSubstituteMaterial.TabIndex = 21;
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.AutoSize = true;
            this.lblEffectiveDate.Location = new System.Drawing.Point(30, 270);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(65, 12);
            this.lblEffectiveDate.TabIndex = 22;
            this.lblEffectiveDate.Text = "生效日期：";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Location = new System.Drawing.Point(120, 267);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(200, 21);
            this.dtpEffectiveDate.TabIndex = 23;
            // 
            // chkHasExpireDate
            // 
            this.chkHasExpireDate.AutoSize = true;
            this.chkHasExpireDate.Location = new System.Drawing.Point(350, 270);
            this.chkHasExpireDate.Name = "chkHasExpireDate";
            this.chkHasExpireDate.Size = new System.Drawing.Size(72, 16);
            this.chkHasExpireDate.TabIndex = 24;
            this.chkHasExpireDate.Text = "失效日期";
            this.chkHasExpireDate.UseVisualStyleBackColor = true;
            this.chkHasExpireDate.CheckedChanged += new System.EventHandler(this.chkHasExpireDate_CheckedChanged);
            // 
            // dtpExpireDate
            // 
            this.dtpExpireDate.Enabled = false;
            this.dtpExpireDate.Location = new System.Drawing.Point(430, 267);
            this.dtpExpireDate.Name = "dtpExpireDate";
            this.dtpExpireDate.Size = new System.Drawing.Size(120, 21);
            this.dtpExpireDate.TabIndex = 25;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Checked = true;
            this.chkStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStatus.Location = new System.Drawing.Point(30, 310);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(48, 16);
            this.chkStatus.TabIndex = 26;
            this.chkStatus.Text = "启用";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(30, 350);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(41, 12);
            this.lblRemarks.TabIndex = 27;
            this.lblRemarks.Text = "备注：";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(120, 347);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(430, 80);
            this.txtRemarks.TabIndex = 28;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(350, 450);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(460, 450);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // BOMEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 511);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.dtpExpireDate);
            this.Controls.Add(this.chkHasExpireDate);
            this.Controls.Add(this.dtpEffectiveDate);
            this.Controls.Add(this.lblEffectiveDate);
            this.Controls.Add(this.txtSubstituteMaterial);
            this.Controls.Add(this.lblSubstituteMaterial);
            this.Controls.Add(this.txtLossRate);
            this.Controls.Add(this.lblLossRate);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.txtMaterialName);
            this.Controls.Add(this.lblMaterialName);
            this.Controls.Add(this.txtMaterialCode);
            this.Controls.Add(this.lblMaterialCode);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.txtProductCode);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.cmbBOMType);
            this.Controls.Add(this.lblBOMType);
            this.Controls.Add(this.txtBOMVersion);
            this.Controls.Add(this.lblBOMVersion);
            this.Controls.Add(this.txtBOMCode);
            this.Controls.Add(this.lblBOMCode);
            this.Name = "BOMEditForm";
            this.Text = "BOM编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBOMCode;
        private System.Windows.Forms.TextBox txtBOMCode;
        private System.Windows.Forms.Label lblBOMVersion;
        private System.Windows.Forms.TextBox txtBOMVersion;
        private System.Windows.Forms.Label lblBOMType;
        private System.Windows.Forms.ComboBox cmbBOMType;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblMaterialCode;
        private System.Windows.Forms.TextBox txtMaterialCode;
        private System.Windows.Forms.Label lblMaterialName;
        private System.Windows.Forms.TextBox txtMaterialName;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblLossRate;
        private System.Windows.Forms.TextBox txtLossRate;
        private System.Windows.Forms.Label lblSubstituteMaterial;
        private System.Windows.Forms.TextBox txtSubstituteMaterial;
        private System.Windows.Forms.Label lblEffectiveDate;
        private System.Windows.Forms.DateTimePicker dtpEffectiveDate;
        private System.Windows.Forms.CheckBox chkHasExpireDate;
        private System.Windows.Forms.DateTimePicker dtpExpireDate;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
