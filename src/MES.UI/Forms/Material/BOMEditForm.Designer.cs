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
            this.lblBOMCode.Location = new System.Drawing.Point(45, 45);
            this.lblBOMCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBOMCode.Name = "lblBOMCode";
            this.lblBOMCode.Size = new System.Drawing.Size(89, 18);
            this.lblBOMCode.TabIndex = 0;
            this.lblBOMCode.Text = "BOM编码：";
            // 
            // txtBOMCode
            // 
            this.txtBOMCode.Location = new System.Drawing.Point(180, 40);
            this.txtBOMCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBOMCode.Name = "txtBOMCode";
            this.txtBOMCode.Size = new System.Drawing.Size(298, 28);
            this.txtBOMCode.TabIndex = 1;
            // 
            // lblBOMVersion
            // 
            this.lblBOMVersion.AutoSize = true;
            this.lblBOMVersion.Location = new System.Drawing.Point(525, 45);
            this.lblBOMVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBOMVersion.Name = "lblBOMVersion";
            this.lblBOMVersion.Size = new System.Drawing.Size(89, 18);
            this.lblBOMVersion.TabIndex = 2;
            this.lblBOMVersion.Text = "BOM版本：";
            // 
            // txtBOMVersion
            // 
            this.txtBOMVersion.Location = new System.Drawing.Point(645, 40);
            this.txtBOMVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBOMVersion.Name = "txtBOMVersion";
            this.txtBOMVersion.Size = new System.Drawing.Size(178, 28);
            this.txtBOMVersion.TabIndex = 3;
            // 
            // lblBOMType
            // 
            this.lblBOMType.AutoSize = true;
            this.lblBOMType.Location = new System.Drawing.Point(45, 105);
            this.lblBOMType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBOMType.Name = "lblBOMType";
            this.lblBOMType.Size = new System.Drawing.Size(89, 18);
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
            this.cmbBOMType.Location = new System.Drawing.Point(180, 100);
            this.cmbBOMType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbBOMType.Name = "cmbBOMType";
            this.cmbBOMType.Size = new System.Drawing.Size(298, 26);
            this.cmbBOMType.TabIndex = 5;
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Location = new System.Drawing.Point(45, 165);
            this.lblProductCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(98, 18);
            this.lblProductCode.TabIndex = 6;
            this.lblProductCode.Text = "产品编码：";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(180, 160);
            this.txtProductCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(298, 28);
            this.txtProductCode.TabIndex = 7;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(525, 165);
            this.lblProductName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(98, 18);
            this.lblProductName.TabIndex = 8;
            this.lblProductName.Text = "产品名称：";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(645, 160);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(178, 28);
            this.txtProductName.TabIndex = 9;
            // 
            // lblMaterialCode
            // 
            this.lblMaterialCode.AutoSize = true;
            this.lblMaterialCode.Location = new System.Drawing.Point(45, 225);
            this.lblMaterialCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterialCode.Name = "lblMaterialCode";
            this.lblMaterialCode.Size = new System.Drawing.Size(98, 18);
            this.lblMaterialCode.TabIndex = 10;
            this.lblMaterialCode.Text = "物料编码：";
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Location = new System.Drawing.Point(180, 220);
            this.txtMaterialCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(298, 28);
            this.txtMaterialCode.TabIndex = 11;
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.AutoSize = true;
            this.lblMaterialName.Location = new System.Drawing.Point(525, 225);
            this.lblMaterialName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(98, 18);
            this.lblMaterialName.TabIndex = 12;
            this.lblMaterialName.Text = "物料名称：";
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.Location = new System.Drawing.Point(645, 220);
            this.txtMaterialName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new System.Drawing.Size(178, 28);
            this.txtMaterialName.TabIndex = 13;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(45, 285);
            this.lblQuantity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(62, 18);
            this.lblQuantity.TabIndex = 14;
            this.lblQuantity.Text = "数量：";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(180, 280);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(148, 28);
            this.txtQuantity.TabIndex = 15;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(375, 285);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(62, 18);
            this.lblUnit.TabIndex = 16;
            this.lblUnit.Text = "单位：";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(450, 280);
            this.txtUnit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(118, 28);
            this.txtUnit.TabIndex = 17;
            // 
            // lblLossRate
            // 
            this.lblLossRate.AutoSize = true;
            this.lblLossRate.Location = new System.Drawing.Point(600, 285);
            this.lblLossRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLossRate.Name = "lblLossRate";
            this.lblLossRate.Size = new System.Drawing.Size(107, 18);
            this.lblLossRate.TabIndex = 18;
            this.lblLossRate.Text = "损耗率(%)：";
            // 
            // txtLossRate
            // 
            this.txtLossRate.Location = new System.Drawing.Point(720, 280);
            this.txtLossRate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLossRate.Name = "txtLossRate";
            this.txtLossRate.Size = new System.Drawing.Size(103, 28);
            this.txtLossRate.TabIndex = 19;
            // 
            // lblSubstituteMaterial
            // 
            this.lblSubstituteMaterial.AutoSize = true;
            this.lblSubstituteMaterial.Location = new System.Drawing.Point(45, 345);
            this.lblSubstituteMaterial.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubstituteMaterial.Name = "lblSubstituteMaterial";
            this.lblSubstituteMaterial.Size = new System.Drawing.Size(98, 18);
            this.lblSubstituteMaterial.TabIndex = 20;
            this.lblSubstituteMaterial.Text = "替代物料：";
            // 
            // txtSubstituteMaterial
            // 
            this.txtSubstituteMaterial.Location = new System.Drawing.Point(180, 340);
            this.txtSubstituteMaterial.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSubstituteMaterial.Name = "txtSubstituteMaterial";
            this.txtSubstituteMaterial.Size = new System.Drawing.Size(643, 28);
            this.txtSubstituteMaterial.TabIndex = 21;
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.AutoSize = true;
            this.lblEffectiveDate.Location = new System.Drawing.Point(45, 405);
            this.lblEffectiveDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(98, 18);
            this.lblEffectiveDate.TabIndex = 22;
            this.lblEffectiveDate.Text = "生效日期：";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Location = new System.Drawing.Point(180, 400);
            this.dtpEffectiveDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(298, 28);
            this.dtpEffectiveDate.TabIndex = 23;
            // 
            // chkHasExpireDate
            // 
            this.chkHasExpireDate.AutoSize = true;
            this.chkHasExpireDate.Location = new System.Drawing.Point(525, 405);
            this.chkHasExpireDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkHasExpireDate.Name = "chkHasExpireDate";
            this.chkHasExpireDate.Size = new System.Drawing.Size(106, 22);
            this.chkHasExpireDate.TabIndex = 24;
            this.chkHasExpireDate.Text = "失效日期";
            this.chkHasExpireDate.UseVisualStyleBackColor = true;
            this.chkHasExpireDate.CheckedChanged += new System.EventHandler(this.chkHasExpireDate_CheckedChanged);
            // 
            // dtpExpireDate
            // 
            this.dtpExpireDate.Enabled = false;
            this.dtpExpireDate.Location = new System.Drawing.Point(645, 400);
            this.dtpExpireDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpExpireDate.Name = "dtpExpireDate";
            this.dtpExpireDate.Size = new System.Drawing.Size(178, 28);
            this.dtpExpireDate.TabIndex = 25;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Checked = true;
            this.chkStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStatus.Location = new System.Drawing.Point(45, 465);
            this.chkStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(70, 22);
            this.chkStatus.TabIndex = 26;
            this.chkStatus.Text = "启用";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(45, 525);
            this.lblRemarks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(62, 18);
            this.lblRemarks.TabIndex = 27;
            this.lblRemarks.Text = "备注：";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(180, 520);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(643, 118);
            this.txtRemarks.TabIndex = 28;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(525, 675);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(135, 52);
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
            this.btnCancel.Location = new System.Drawing.Point(690, 675);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(135, 52);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // BOMEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 763);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
