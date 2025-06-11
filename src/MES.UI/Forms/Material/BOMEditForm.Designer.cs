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
            this.txtBOMCode = new System.Windows.Forms.TextBox();
            this.txtBomName = new System.Windows.Forms.TextBox();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtMaterialCode = new System.Windows.Forms.TextBox();
            this.txtMaterialName = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.txtLossRate = new System.Windows.Forms.TextBox();
            this.txtSubstituteMaterial = new System.Windows.Forms.TextBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtBOMVersion = new System.Windows.Forms.TextBox();
            this.cmbBOMType = new System.Windows.Forms.ComboBox();
            this.dtpEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.dtpExpireDate = new System.Windows.Forms.DateTimePicker();
            this.chkHasExpireDate = new System.Windows.Forms.CheckBox();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBOMCode
            // 
            this.txtBOMCode.Location = new System.Drawing.Point(120, 20);
            this.txtBOMCode.Name = "txtBOMCode";
            this.txtBOMCode.Size = new System.Drawing.Size(200, 20);
            this.txtBOMCode.TabIndex = 0;
            // 
            // txtBomName
            // 
            this.txtBomName.Location = new System.Drawing.Point(120, 50);
            this.txtBomName.Name = "txtBomName";
            this.txtBomName.Size = new System.Drawing.Size(200, 20);
            this.txtBomName.TabIndex = 1;
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(120, 80);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(200, 20);
            this.txtProductCode.TabIndex = 2;
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(120, 110);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 20);
            this.txtProductName.TabIndex = 3;
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Location = new System.Drawing.Point(120, 140);
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(200, 20);
            this.txtMaterialCode.TabIndex = 4;
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.Location = new System.Drawing.Point(120, 170);
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new System.Drawing.Size(200, 20);
            this.txtMaterialName.TabIndex = 5;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(120, 200);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(100, 20);
            this.txtQuantity.TabIndex = 6;
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(280, 200);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(80, 20);
            this.txtUnit.TabIndex = 7;
            // 
            // txtLossRate
            // 
            this.txtLossRate.Location = new System.Drawing.Point(120, 230);
            this.txtLossRate.Name = "txtLossRate";
            this.txtLossRate.Size = new System.Drawing.Size(100, 20);
            this.txtLossRate.TabIndex = 8;
            // 
            // txtSubstituteMaterial
            // 
            this.txtSubstituteMaterial.Location = new System.Drawing.Point(120, 260);
            this.txtSubstituteMaterial.Name = "txtSubstituteMaterial";
            this.txtSubstituteMaterial.Size = new System.Drawing.Size(200, 20);
            this.txtSubstituteMaterial.TabIndex = 9;
            // 
            // txtRemarks
            //
            this.txtRemarks.Location = new System.Drawing.Point(120, 290);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(200, 50);
            this.txtRemarks.TabIndex = 10;
            // 
            // txtBOMVersion
            //
            this.txtBOMVersion.Location = new System.Drawing.Point(550, 30);
            this.txtBOMVersion.Name = "txtBOMVersion";
            this.txtBOMVersion.Size = new System.Drawing.Size(200, 20);
            this.txtBOMVersion.TabIndex = 11;
            // 
            // cmbBOMType
            // 
            this.cmbBOMType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBOMType.FormattingEnabled = true;
            this.cmbBOMType.Items.AddRange(new object[] {
            "PRODUCTION",
            "ENGINEERING"});
            this.cmbBOMType.Location = new System.Drawing.Point(550, 70);
            this.cmbBOMType.Name = "cmbBOMType";
            this.cmbBOMType.Size = new System.Drawing.Size(200, 21);
            this.cmbBOMType.TabIndex = 12;
            // 
            // dtpEffectiveDate
            //
            this.dtpEffectiveDate.Location = new System.Drawing.Point(550, 110);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(200, 20);
            this.dtpEffectiveDate.TabIndex = 13;
            // 
            // dtpExpireDate
            //
            this.dtpExpireDate.Location = new System.Drawing.Point(550, 190);
            this.dtpExpireDate.Name = "dtpExpireDate";
            this.dtpExpireDate.Size = new System.Drawing.Size(200, 20);
            this.dtpExpireDate.TabIndex = 14;
            // 
            // chkHasExpireDate
            //
            this.chkHasExpireDate.AutoSize = true;
            this.chkHasExpireDate.Location = new System.Drawing.Point(400, 150);
            this.chkHasExpireDate.Name = "chkHasExpireDate";
            this.chkHasExpireDate.Size = new System.Drawing.Size(96, 17);
            this.chkHasExpireDate.TabIndex = 15;
            this.chkHasExpireDate.Text = "设置失效日期";
            this.chkHasExpireDate.UseVisualStyleBackColor = true;
            this.chkHasExpireDate.CheckedChanged += new System.EventHandler(this.chkHasExpireDate_CheckedChanged);
            // 
            // chkStatus
            //
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(550, 230);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(48, 17);
            this.chkStatus.TabIndex = 16;
            this.chkStatus.Text = "启用";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(600, 420);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 35);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(690, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 35);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "BOM编码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "BOM名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "产品编码";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "产品名称";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "物料编码";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "物料名称";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "数量";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(240, 203);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "单位";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "损耗率(%)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 263);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "替代料";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 293);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "备注";
            // 
            // label12
            //
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(450, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "BOM版本";
            // 
            // label13
            //
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(450, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "BOM类型";
            // 
            // label14
            //
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(450, 113);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 32;
            this.label14.Text = "生效日期";
            //
            // label15
            //
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(450, 193);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "失效日期";
            //
            // BOMEditForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.chkHasExpireDate);
            this.Controls.Add(this.dtpExpireDate);
            this.Controls.Add(this.dtpEffectiveDate);
            this.Controls.Add(this.cmbBOMType);
            this.Controls.Add(this.txtBOMVersion);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.txtSubstituteMaterial);
            this.Controls.Add(this.txtLossRate);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.txtMaterialName);
            this.Controls.Add(this.txtMaterialCode);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.txtProductCode);
            this.Controls.Add(this.txtBomName);
            this.Controls.Add(this.txtBOMCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BOMEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BOM编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBOMCode;
        private System.Windows.Forms.TextBox txtBomName;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.TextBox txtMaterialCode;
        private System.Windows.Forms.TextBox txtMaterialName;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.TextBox txtLossRate;
        private System.Windows.Forms.TextBox txtSubstituteMaterial;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.TextBox txtBOMVersion;
        private System.Windows.Forms.ComboBox cmbBOMType;
        private System.Windows.Forms.DateTimePicker dtpEffectiveDate;
        private System.Windows.Forms.DateTimePicker dtpExpireDate;
        private System.Windows.Forms.CheckBox chkHasExpireDate;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
    }
}
