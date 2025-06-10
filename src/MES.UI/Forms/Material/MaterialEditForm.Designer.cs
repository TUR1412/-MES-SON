namespace MES.UI.Forms.Material
{
    partial class MaterialEditForm
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
            this.lbMaterialCode = new System.Windows.Forms.Label();
            this.txtMaterialCode = new System.Windows.Forms.TextBox();
            this.lbMaterialName = new System.Windows.Forms.Label();
            this.txtMaterialName = new System.Windows.Forms.TextBox();
            this.lbMaterialType = new System.Windows.Forms.Label();
            this.txtMaterialType = new System.Windows.Forms.ComboBox();
            this.lbSpecification = new System.Windows.Forms.Label();
            this.txtSpecification = new System.Windows.Forms.TextBox();
            this.lbUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lbCategory = new System.Windows.Forms.Label();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSupplier = new System.Windows.Forms.TextBox();
            this.lbSupplier = new System.Windows.Forms.Label();
            this.txtStandardCost = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSafetyStock = new System.Windows.Forms.TextBox();
            this.lbSafetyStock = new System.Windows.Forms.Label();
            this.txtMinStock = new System.Windows.Forms.TextBox();
            this.lbMaxStock = new System.Windows.Forms.Label();
            this.txtMaxStock = new System.Windows.Forms.TextBox();
            this.lbStockQuantity = new System.Windows.Forms.Label();
            this.txtStockQuantity = new System.Windows.Forms.TextBox();
            this.lbLeadTime = new System.Windows.Forms.Label();
            this.txtLeadTime = new System.Windows.Forms.TextBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.lbPrice = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbMaterialCode
            // 
            this.lbMaterialCode.AutoSize = true;
            this.lbMaterialCode.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaterialCode.Location = new System.Drawing.Point(48, 25);
            this.lbMaterialCode.Name = "lbMaterialCode";
            this.lbMaterialCode.Size = new System.Drawing.Size(154, 24);
            this.lbMaterialCode.TabIndex = 0;
            this.lbMaterialCode.Text = "    物料编码";
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMaterialCode.Location = new System.Drawing.Point(221, 21);
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(300, 33);
            this.txtMaterialCode.TabIndex = 1;
            // 
            // lbMaterialName
            // 
            this.lbMaterialName.AutoSize = true;
            this.lbMaterialName.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaterialName.Location = new System.Drawing.Point(48, 78);
            this.lbMaterialName.Name = "lbMaterialName";
            this.lbMaterialName.Size = new System.Drawing.Size(154, 24);
            this.lbMaterialName.TabIndex = 0;
            this.lbMaterialName.Text = "    物料名称";
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMaterialName.Location = new System.Drawing.Point(221, 74);
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new System.Drawing.Size(300, 33);
            this.txtMaterialName.TabIndex = 1;
            // 
            // lbMaterialType
            // 
            this.lbMaterialType.AutoSize = true;
            this.lbMaterialType.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaterialType.Location = new System.Drawing.Point(48, 135);
            this.lbMaterialType.Name = "lbMaterialType";
            this.lbMaterialType.Size = new System.Drawing.Size(154, 24);
            this.lbMaterialType.TabIndex = 0;
            this.lbMaterialType.Text = "    物料类型";
            // 
            // txtMaterialType
            // 
            this.txtMaterialType.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMaterialType.FormattingEnabled = true;
            this.txtMaterialType.Items.AddRange(new object[] {
            "RAW_MATERIAL",
            "SEMI_FINISHED",
            "FINISHED_PRODUCT",
            "Cap",
            "Ind",
            "Res",
            "Wire",
            "DAF",
            "IC",
            "LF",
            "Ext"});
            this.txtMaterialType.Location = new System.Drawing.Point(221, 132);
            this.txtMaterialType.Name = "txtMaterialType";
            this.txtMaterialType.Size = new System.Drawing.Size(300, 30);
            this.txtMaterialType.TabIndex = 2;
            // 
            // lbSpecification
            // 
            this.lbSpecification.AutoSize = true;
            this.lbSpecification.Font = new System.Drawing.Font("宋体", 12F);
            this.lbSpecification.Location = new System.Drawing.Point(48, 188);
            this.lbSpecification.Name = "lbSpecification";
            this.lbSpecification.Size = new System.Drawing.Size(154, 24);
            this.lbSpecification.TabIndex = 0;
            this.lbSpecification.Text = "    规格型号";
            // 
            // txtSpecification
            // 
            this.txtSpecification.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSpecification.Location = new System.Drawing.Point(221, 184);
            this.txtSpecification.Name = "txtSpecification";
            this.txtSpecification.Size = new System.Drawing.Size(300, 33);
            this.txtSpecification.TabIndex = 1;
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("宋体", 12F);
            this.lbUnit.Location = new System.Drawing.Point(48, 242);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(154, 24);
            this.lbUnit.TabIndex = 0;
            this.lbUnit.Text = "    计量单位";
            // 
            // txtUnit
            // 
            this.txtUnit.Font = new System.Drawing.Font("宋体", 11F);
            this.txtUnit.Location = new System.Drawing.Point(221, 238);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(300, 33);
            this.txtUnit.TabIndex = 1;
            // 
            // lbCategory
            // 
            this.lbCategory.AutoSize = true;
            this.lbCategory.Font = new System.Drawing.Font("宋体", 12F);
            this.lbCategory.Location = new System.Drawing.Point(48, 295);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(154, 24);
            this.lbCategory.TabIndex = 0;
            this.lbCategory.Text = "    物料分类";
            // 
            // txtCategory
            // 
            this.txtCategory.Font = new System.Drawing.Font("宋体", 11F);
            this.txtCategory.Location = new System.Drawing.Point(221, 291);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(300, 33);
            this.txtCategory.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.Location = new System.Drawing.Point(48, 349);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "      供应商";
            // 
            // txtSupplier
            // 
            this.txtSupplier.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSupplier.Location = new System.Drawing.Point(221, 345);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(300, 33);
            this.txtSupplier.TabIndex = 1;
            // 
            // lbSupplier
            // 
            this.lbSupplier.AutoSize = true;
            this.lbSupplier.Font = new System.Drawing.Font("宋体", 12F);
            this.lbSupplier.Location = new System.Drawing.Point(48, 402);
            this.lbSupplier.Name = "lbSupplier";
            this.lbSupplier.Size = new System.Drawing.Size(154, 24);
            this.lbSupplier.TabIndex = 0;
            this.lbSupplier.Text = "    标准成本";
            // 
            // txtStandardCost
            // 
            this.txtStandardCost.Font = new System.Drawing.Font("宋体", 11F);
            this.txtStandardCost.Location = new System.Drawing.Point(221, 398);
            this.txtStandardCost.Name = "txtStandardCost";
            this.txtStandardCost.Size = new System.Drawing.Size(300, 33);
            this.txtStandardCost.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F);
            this.label9.Location = new System.Drawing.Point(48, 453);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(154, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "    安全库存";
            // 
            // txtSafetyStock
            // 
            this.txtSafetyStock.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSafetyStock.Location = new System.Drawing.Point(221, 449);
            this.txtSafetyStock.Name = "txtSafetyStock";
            this.txtSafetyStock.Size = new System.Drawing.Size(300, 33);
            this.txtSafetyStock.TabIndex = 1;
            // 
            // lbSafetyStock
            // 
            this.lbSafetyStock.AutoSize = true;
            this.lbSafetyStock.Font = new System.Drawing.Font("宋体", 12F);
            this.lbSafetyStock.Location = new System.Drawing.Point(48, 505);
            this.lbSafetyStock.Name = "lbSafetyStock";
            this.lbSafetyStock.Size = new System.Drawing.Size(154, 24);
            this.lbSafetyStock.TabIndex = 0;
            this.lbSafetyStock.Text = "    最小库存";
            // 
            // txtMinStock
            // 
            this.txtMinStock.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMinStock.Location = new System.Drawing.Point(221, 501);
            this.txtMinStock.Name = "txtMinStock";
            this.txtMinStock.Size = new System.Drawing.Size(300, 33);
            this.txtMinStock.TabIndex = 1;
            // 
            // lbMaxStock
            // 
            this.lbMaxStock.AutoSize = true;
            this.lbMaxStock.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaxStock.Location = new System.Drawing.Point(48, 556);
            this.lbMaxStock.Name = "lbMaxStock";
            this.lbMaxStock.Size = new System.Drawing.Size(154, 24);
            this.lbMaxStock.TabIndex = 0;
            this.lbMaxStock.Text = "    最大库存";
            // 
            // txtMaxStock
            // 
            this.txtMaxStock.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMaxStock.Location = new System.Drawing.Point(221, 552);
            this.txtMaxStock.Name = "txtMaxStock";
            this.txtMaxStock.Size = new System.Drawing.Size(300, 33);
            this.txtMaxStock.TabIndex = 1;
            // 
            // lbStockQuantity
            // 
            this.lbStockQuantity.AutoSize = true;
            this.lbStockQuantity.Font = new System.Drawing.Font("宋体", 12F);
            this.lbStockQuantity.Location = new System.Drawing.Point(48, 607);
            this.lbStockQuantity.Name = "lbStockQuantity";
            this.lbStockQuantity.Size = new System.Drawing.Size(154, 24);
            this.lbStockQuantity.TabIndex = 0;
            this.lbStockQuantity.Text = "当前库存数量";
            // 
            // txtStockQuantity
            // 
            this.txtStockQuantity.Font = new System.Drawing.Font("宋体", 11F);
            this.txtStockQuantity.Location = new System.Drawing.Point(221, 603);
            this.txtStockQuantity.Name = "txtStockQuantity";
            this.txtStockQuantity.Size = new System.Drawing.Size(300, 33);
            this.txtStockQuantity.TabIndex = 1;
            // 
            // lbLeadTime
            // 
            this.lbLeadTime.AutoSize = true;
            this.lbLeadTime.Font = new System.Drawing.Font("宋体", 12F);
            this.lbLeadTime.Location = new System.Drawing.Point(18, 657);
            this.lbLeadTime.Name = "lbLeadTime";
            this.lbLeadTime.Size = new System.Drawing.Size(202, 24);
            this.lbLeadTime.TabIndex = 0;
            this.lbLeadTime.Text = "采购提前期（天）";
            // 
            // txtLeadTime
            // 
            this.txtLeadTime.Font = new System.Drawing.Font("宋体", 11F);
            this.txtLeadTime.Location = new System.Drawing.Point(221, 653);
            this.txtLeadTime.Name = "txtLeadTime";
            this.txtLeadTime.Size = new System.Drawing.Size(300, 33);
            this.txtLeadTime.TabIndex = 1;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("宋体", 12F);
            this.lbStatus.Location = new System.Drawing.Point(18, 707);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(190, 24);
            this.lbStatus.TabIndex = 0;
            this.lbStatus.Text = "           状态";
            // 
            // txtStatus
            // 
            this.txtStatus.Font = new System.Drawing.Font("宋体", 11F);
            this.txtStatus.Location = new System.Drawing.Point(221, 703);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(300, 33);
            this.txtStatus.TabIndex = 1;
            // 
            // lbPrice
            // 
            this.lbPrice.AutoSize = true;
            this.lbPrice.Font = new System.Drawing.Font("宋体", 12F);
            this.lbPrice.Location = new System.Drawing.Point(18, 758);
            this.lbPrice.Name = "lbPrice";
            this.lbPrice.Size = new System.Drawing.Size(190, 24);
            this.lbPrice.TabIndex = 0;
            this.lbPrice.Text = "       参考价格";
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("宋体", 11F);
            this.txtPrice.Location = new System.Drawing.Point(221, 754);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(300, 33);
            this.txtPrice.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSave.Font = new System.Drawing.Font("宋体", 11F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(99, 812);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(171, 50);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 11F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(384, 812);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(171, 50);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MaterialEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 874);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtMaterialType);
            this.Controls.Add(this.lbMaterialType);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lbPrice);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.txtLeadTime);
            this.Controls.Add(this.lbLeadTime);
            this.Controls.Add(this.txtStockQuantity);
            this.Controls.Add(this.lbStockQuantity);
            this.Controls.Add(this.txtMaxStock);
            this.Controls.Add(this.lbMaxStock);
            this.Controls.Add(this.txtMinStock);
            this.Controls.Add(this.lbSafetyStock);
            this.Controls.Add(this.txtSafetyStock);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtStandardCost);
            this.Controls.Add(this.lbSupplier);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.lbCategory);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.lbUnit);
            this.Controls.Add(this.txtSpecification);
            this.Controls.Add(this.lbSpecification);
            this.Controls.Add(this.txtMaterialName);
            this.Controls.Add(this.lbMaterialName);
            this.Controls.Add(this.txtMaterialCode);
            this.Controls.Add(this.lbMaterialCode);
            this.Name = "MaterialEditForm";
            this.Text = "MaterialEditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMaterialCode;
        private System.Windows.Forms.TextBox txtMaterialCode;
        private System.Windows.Forms.Label lbMaterialName;
        private System.Windows.Forms.TextBox txtMaterialName;
        private System.Windows.Forms.Label lbMaterialType;
        private System.Windows.Forms.ComboBox txtMaterialType;
        private System.Windows.Forms.Label lbSpecification;
        private System.Windows.Forms.TextBox txtSpecification;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lbCategory;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSupplier;
        private System.Windows.Forms.Label lbSupplier;
        private System.Windows.Forms.TextBox txtStandardCost;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSafetyStock;
        private System.Windows.Forms.Label lbSafetyStock;
        private System.Windows.Forms.TextBox txtMinStock;
        private System.Windows.Forms.Label lbMaxStock;
        private System.Windows.Forms.TextBox txtMaxStock;
        private System.Windows.Forms.Label lbStockQuantity;
        private System.Windows.Forms.TextBox txtStockQuantity;
        private System.Windows.Forms.Label lbLeadTime;
        private System.Windows.Forms.TextBox txtLeadTime;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label lbPrice;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}