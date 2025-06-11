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
            this.mySqlDataAdapter1 = new MySql.Data.MySqlClient.MySqlDataAdapter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbMaterialCode
            //
            this.lbMaterialCode.AutoSize = true;
            this.lbMaterialCode.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaterialCode.Location = new System.Drawing.Point(20, 32);
            this.lbMaterialCode.Name = "lbMaterialCode";
            this.lbMaterialCode.Size = new System.Drawing.Size(96, 24);
            this.lbMaterialCode.TabIndex = 0;
            this.lbMaterialCode.Text = "物料编码";
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMaterialCode.Location = new System.Drawing.Point(147, 28);
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Size = new System.Drawing.Size(300, 33);
            this.txtMaterialCode.TabIndex = 1;
            // 
            // lbMaterialName
            //
            this.lbMaterialName.AutoSize = true;
            this.lbMaterialName.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaterialName.Location = new System.Drawing.Point(20, 85);
            this.lbMaterialName.Name = "lbMaterialName";
            this.lbMaterialName.Size = new System.Drawing.Size(96, 24);
            this.lbMaterialName.TabIndex = 0;
            this.lbMaterialName.Text = "物料名称";
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMaterialName.Location = new System.Drawing.Point(147, 81);
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Size = new System.Drawing.Size(300, 33);
            this.txtMaterialName.TabIndex = 1;
            // 
            // lbMaterialType
            //
            this.lbMaterialType.AutoSize = true;
            this.lbMaterialType.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaterialType.Location = new System.Drawing.Point(20, 142);
            this.lbMaterialType.Name = "lbMaterialType";
            this.lbMaterialType.Size = new System.Drawing.Size(96, 24);
            this.lbMaterialType.TabIndex = 0;
            this.lbMaterialType.Text = "物料类型";
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
            this.txtMaterialType.Location = new System.Drawing.Point(147, 139);
            this.txtMaterialType.Name = "txtMaterialType";
            this.txtMaterialType.Size = new System.Drawing.Size(300, 30);
            this.txtMaterialType.TabIndex = 2;
            // 
            // lbSpecification
            //
            this.lbSpecification.AutoSize = true;
            this.lbSpecification.Font = new System.Drawing.Font("宋体", 12F);
            this.lbSpecification.Location = new System.Drawing.Point(20, 195);
            this.lbSpecification.Name = "lbSpecification";
            this.lbSpecification.Size = new System.Drawing.Size(96, 24);
            this.lbSpecification.TabIndex = 0;
            this.lbSpecification.Text = "规格型号";
            //
            // txtSpecification
            //
            this.txtSpecification.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSpecification.Location = new System.Drawing.Point(147, 191);
            this.txtSpecification.Name = "txtSpecification";
            this.txtSpecification.Size = new System.Drawing.Size(300, 33);
            this.txtSpecification.TabIndex = 1;
            //
            // lbUnit
            //
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("宋体", 12F);
            this.lbUnit.Location = new System.Drawing.Point(20, 249);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(96, 24);
            this.lbUnit.TabIndex = 0;
            this.lbUnit.Text = "计量单位";
            //
            // txtUnit
            //
            this.txtUnit.Font = new System.Drawing.Font("宋体", 11F);
            this.txtUnit.Location = new System.Drawing.Point(147, 245);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(300, 33);
            this.txtUnit.TabIndex = 1;
            //
            // lbCategory
            //
            this.lbCategory.AutoSize = true;
            this.lbCategory.Font = new System.Drawing.Font("宋体", 12F);
            this.lbCategory.Location = new System.Drawing.Point(20, 302);
            this.lbCategory.Name = "lbCategory";
            this.lbCategory.Size = new System.Drawing.Size(96, 24);
            this.lbCategory.TabIndex = 0;
            this.lbCategory.Text = "物料分类";
            //
            // txtCategory
            //
            this.txtCategory.Font = new System.Drawing.Font("宋体", 11F);
            this.txtCategory.Location = new System.Drawing.Point(147, 298);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(300, 33);
            this.txtCategory.TabIndex = 1;
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F);
            this.label7.Location = new System.Drawing.Point(20, 356);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "供应商";
            // 
            // txtSupplier
            // 
            this.txtSupplier.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSupplier.Location = new System.Drawing.Point(147, 352);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(300, 33);
            this.txtSupplier.TabIndex = 1;
            // 
            // lbSupplier
            //
            this.lbSupplier.AutoSize = true;
            this.lbSupplier.Font = new System.Drawing.Font("宋体", 12F);
            this.lbSupplier.Location = new System.Drawing.Point(520, 32);
            this.lbSupplier.Name = "lbSupplier";
            this.lbSupplier.Size = new System.Drawing.Size(96, 24);
            this.lbSupplier.TabIndex = 0;
            this.lbSupplier.Text = "标准成本";
            //
            // txtStandardCost
            //
            this.txtStandardCost.Font = new System.Drawing.Font("宋体", 11F);
            this.txtStandardCost.Location = new System.Drawing.Point(647, 28);
            this.txtStandardCost.Name = "txtStandardCost";
            this.txtStandardCost.Size = new System.Drawing.Size(300, 33);
            this.txtStandardCost.TabIndex = 1;
            //
            // label9
            //
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F);
            this.label9.Location = new System.Drawing.Point(520, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "安全库存";
            //
            // txtSafetyStock
            //
            this.txtSafetyStock.Font = new System.Drawing.Font("宋体", 11F);
            this.txtSafetyStock.Location = new System.Drawing.Point(647, 81);
            this.txtSafetyStock.Name = "txtSafetyStock";
            this.txtSafetyStock.Size = new System.Drawing.Size(300, 33);
            this.txtSafetyStock.TabIndex = 1;
            //
            // lbSafetyStock
            //
            this.lbSafetyStock.AutoSize = true;
            this.lbSafetyStock.Font = new System.Drawing.Font("宋体", 12F);
            this.lbSafetyStock.Location = new System.Drawing.Point(520, 140);
            this.lbSafetyStock.Name = "lbSafetyStock";
            this.lbSafetyStock.Size = new System.Drawing.Size(96, 24);
            this.lbSafetyStock.TabIndex = 0;
            this.lbSafetyStock.Text = "最小库存";
            // 
            // txtMinStock
            //
            this.txtMinStock.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMinStock.Location = new System.Drawing.Point(647, 136);
            this.txtMinStock.Name = "txtMinStock";
            this.txtMinStock.Size = new System.Drawing.Size(300, 33);
            this.txtMinStock.TabIndex = 1;
            //
            // lbMaxStock
            //
            this.lbMaxStock.AutoSize = true;
            this.lbMaxStock.Font = new System.Drawing.Font("宋体", 12F);
            this.lbMaxStock.Location = new System.Drawing.Point(520, 195);
            this.lbMaxStock.Name = "lbMaxStock";
            this.lbMaxStock.Size = new System.Drawing.Size(96, 24);
            this.lbMaxStock.TabIndex = 0;
            this.lbMaxStock.Text = "最大库存";
            //
            // txtMaxStock
            //
            this.txtMaxStock.Font = new System.Drawing.Font("宋体", 11F);
            this.txtMaxStock.Location = new System.Drawing.Point(647, 191);
            this.txtMaxStock.Name = "txtMaxStock";
            this.txtMaxStock.Size = new System.Drawing.Size(300, 33);
            this.txtMaxStock.TabIndex = 1;
            //
            // lbStockQuantity
            //
            this.lbStockQuantity.AutoSize = true;
            this.lbStockQuantity.Font = new System.Drawing.Font("宋体", 12F);
            this.lbStockQuantity.Location = new System.Drawing.Point(480, 249);
            this.lbStockQuantity.Name = "lbStockQuantity";
            this.lbStockQuantity.Size = new System.Drawing.Size(144, 24);
            this.lbStockQuantity.TabIndex = 0;
            this.lbStockQuantity.Text = "当前库存数量";
            //
            // txtStockQuantity
            //
            this.txtStockQuantity.Font = new System.Drawing.Font("宋体", 11F);
            this.txtStockQuantity.Location = new System.Drawing.Point(647, 245);
            this.txtStockQuantity.Name = "txtStockQuantity";
            this.txtStockQuantity.Size = new System.Drawing.Size(300, 33);
            this.txtStockQuantity.TabIndex = 1;
            // 
            // lbLeadTime
            //
            this.lbLeadTime.AutoSize = true;
            this.lbLeadTime.Font = new System.Drawing.Font("宋体", 12F);
            this.lbLeadTime.Location = new System.Drawing.Point(440, 302);
            this.lbLeadTime.Name = "lbLeadTime";
            this.lbLeadTime.Size = new System.Drawing.Size(192, 24);
            this.lbLeadTime.TabIndex = 0;
            this.lbLeadTime.Text = "采购提前期（天）";
            //
            // txtLeadTime
            //
            this.txtLeadTime.Font = new System.Drawing.Font("宋体", 11F);
            this.txtLeadTime.Location = new System.Drawing.Point(647, 298);
            this.txtLeadTime.Name = "txtLeadTime";
            this.txtLeadTime.Size = new System.Drawing.Size(300, 33);
            this.txtLeadTime.TabIndex = 1;
            //
            // lbStatus
            //
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("宋体", 12F);
            this.lbStatus.Location = new System.Drawing.Point(520, 356);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(48, 24);
            this.lbStatus.TabIndex = 0;
            this.lbStatus.Text = "状态";
            //
            // txtStatus
            //
            this.txtStatus.Font = new System.Drawing.Font("宋体", 11F);
            this.txtStatus.Location = new System.Drawing.Point(647, 352);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(300, 33);
            this.txtStatus.TabIndex = 1;
            //
            // lbPrice
            //
            this.lbPrice.AutoSize = true;
            this.lbPrice.Font = new System.Drawing.Font("宋体", 12F);
            this.lbPrice.Location = new System.Drawing.Point(520, 407);
            this.lbPrice.Name = "lbPrice";
            this.lbPrice.Size = new System.Drawing.Size(96, 24);
            this.lbPrice.TabIndex = 0;
            this.lbPrice.Text = "参考价格";
            //
            // txtPrice
            //
            this.txtPrice.Font = new System.Drawing.Font("宋体", 11F);
            this.txtPrice.Location = new System.Drawing.Point(647, 403);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(300, 33);
            this.txtPrice.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSave.Font = new System.Drawing.Font("宋体", 11F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(512, 613);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(171, 59);
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
            this.btnCancel.Location = new System.Drawing.Point(797, 613);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(171, 59);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // mySqlDataAdapter1
            // 
            this.mySqlDataAdapter1.DeleteCommand = null;
            this.mySqlDataAdapter1.InsertCommand = null;
            this.mySqlDataAdapter1.SelectCommand = null;
            this.mySqlDataAdapter1.UpdateCommand = null;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbMaterialCode);
            this.panel1.Controls.Add(this.txtMaterialCode);
            this.panel1.Controls.Add(this.lbMaterialName);
            this.panel1.Controls.Add(this.txtPrice);
            this.panel1.Controls.Add(this.txtMaterialType);
            this.panel1.Controls.Add(this.lbPrice);
            this.panel1.Controls.Add(this.txtMaterialName);
            this.panel1.Controls.Add(this.txtStatus);
            this.panel1.Controls.Add(this.lbMaterialType);
            this.panel1.Controls.Add(this.lbStatus);
            this.panel1.Controls.Add(this.lbSpecification);
            this.panel1.Controls.Add(this.txtLeadTime);
            this.panel1.Controls.Add(this.txtSpecification);
            this.panel1.Controls.Add(this.lbLeadTime);
            this.panel1.Controls.Add(this.lbUnit);
            this.panel1.Controls.Add(this.txtStockQuantity);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.lbStockQuantity);
            this.panel1.Controls.Add(this.lbCategory);
            this.panel1.Controls.Add(this.txtMaxStock);
            this.panel1.Controls.Add(this.txtCategory);
            this.panel1.Controls.Add(this.lbMaxStock);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtMinStock);
            this.panel1.Controls.Add(this.txtSupplier);
            this.panel1.Controls.Add(this.lbSafetyStock);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSafetyStock);
            this.panel1.Controls.Add(this.lbSupplier);
            this.panel1.Controls.Add(this.txtStandardCost);
            this.panel1.Location = new System.Drawing.Point(12, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1007, 485);
            this.panel1.TabIndex = 4;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1064, 90);
            this.panelTop.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 27);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(288, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "物料信息添加&编辑";
            // 
            // MaterialEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 695);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "MaterialEditForm";
            this.Text = "MaterialEditForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

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
        private MySql.Data.MySqlClient.MySqlDataAdapter mySqlDataAdapter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
    }
}