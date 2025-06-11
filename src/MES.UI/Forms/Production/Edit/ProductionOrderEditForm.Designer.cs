namespace MES.UI.Forms.Production.Edit
{
    partial class ProductionOrderEditForm
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
            this.lblOrderNo = new System.Windows.Forms.Label();
            this.txtOrderNo = new System.Windows.Forms.TextBox();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.lblActualQuantity = new System.Windows.Forms.Label();
            this.txtActualQuantity = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblPriority = new System.Windows.Forms.Label();
            this.cmbPriority = new System.Windows.Forms.ComboBox();
            this.lblWorkshopName = new System.Windows.Forms.Label();
            this.txtWorkshopName = new System.Windows.Forms.TextBox();
            this.lblResponsiblePerson = new System.Windows.Forms.Label();
            this.txtResponsiblePerson = new System.Windows.Forms.TextBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.lblSalesOrderNumber = new System.Windows.Forms.Label();
            this.txtSalesOrderNumber = new System.Windows.Forms.TextBox();
            this.lblPlanStartTime = new System.Windows.Forms.Label();
            this.dtpPlanStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblPlanEndTime = new System.Windows.Forms.Label();
            this.dtpPlanEndTime = new System.Windows.Forms.DateTimePicker();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblOrderNo
            // 
            this.lblOrderNo.AutoSize = true;
            this.lblOrderNo.Location = new System.Drawing.Point(12, 15);
            this.lblOrderNo.Name = "lblOrderNo";
            this.lblOrderNo.Size = new System.Drawing.Size(65, 12);
            this.lblOrderNo.TabIndex = 0;
            this.lblOrderNo.Text = "订单编号：";
            // 
            // txtOrderNo
            // 
            this.txtOrderNo.Location = new System.Drawing.Point(83, 12);
            this.txtOrderNo.Name = "txtOrderNo";
            this.txtOrderNo.ReadOnly = true;
            this.txtOrderNo.Size = new System.Drawing.Size(150, 21);
            this.txtOrderNo.TabIndex = 1;
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Location = new System.Drawing.Point(250, 15);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(65, 12);
            this.lblProductCode.TabIndex = 2;
            this.lblProductCode.Text = "产品编码：";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(321, 12);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(150, 21);
            this.txtProductCode.TabIndex = 3;            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(12, 45);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(65, 12);
            this.lblProductName.TabIndex = 4;
            this.lblProductName.Text = "产品名称：";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(83, 42);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(150, 21);
            this.txtProductName.TabIndex = 5;
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(250, 45);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(65, 12);
            this.lblQuantity.TabIndex = 6;
            this.lblQuantity.Text = "计划数量：";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(321, 42);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(150, 21);
            this.txtQuantity.TabIndex = 7;
            // 
            // lblActualQuantity
            // 
            this.lblActualQuantity.AutoSize = true;
            this.lblActualQuantity.Location = new System.Drawing.Point(12, 75);
            this.lblActualQuantity.Name = "lblActualQuantity";
            this.lblActualQuantity.Size = new System.Drawing.Size(65, 12);
            this.lblActualQuantity.TabIndex = 8;
            this.lblActualQuantity.Text = "实际数量：";
            // 
            // txtActualQuantity
            // 
            this.txtActualQuantity.Location = new System.Drawing.Point(83, 72);
            this.txtActualQuantity.Name = "txtActualQuantity";
            this.txtActualQuantity.ReadOnly = true;
            this.txtActualQuantity.Size = new System.Drawing.Size(150, 21);
            this.txtActualQuantity.TabIndex = 9;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.Location = new System.Drawing.Point(250, 75);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(41, 12);
            this.lblUnit.TabIndex = 10;
            this.lblUnit.Text = "单位：";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(321, 72);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(150, 21);
            this.txtUnit.TabIndex = 11;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 105);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "状态：";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "待开始",
            "进行中",
            "已暂停",
            "已完成",
            "已取消"});
            this.cmbStatus.Location = new System.Drawing.Point(83, 102);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(150, 20);
            this.cmbStatus.TabIndex = 13;            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.Location = new System.Drawing.Point(250, 105);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(53, 12);
            this.lblPriority.TabIndex = 14;
            this.lblPriority.Text = "优先级：";
            // 
            // cmbPriority
            // 
            this.cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriority.FormattingEnabled = true;
            this.cmbPriority.Items.AddRange(new object[] {
            "低",
            "普通",
            "重要",
            "紧急"});
            this.cmbPriority.Location = new System.Drawing.Point(321, 102);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(150, 20);
            this.cmbPriority.TabIndex = 15;
            // 
            // lblWorkshopName
            // 
            this.lblWorkshopName.AutoSize = true;
            this.lblWorkshopName.Location = new System.Drawing.Point(12, 135);
            this.lblWorkshopName.Name = "lblWorkshopName";
            this.lblWorkshopName.Size = new System.Drawing.Size(65, 12);
            this.lblWorkshopName.TabIndex = 16;
            this.lblWorkshopName.Text = "生产车间：";
            // 
            // txtWorkshopName
            // 
            this.txtWorkshopName.Location = new System.Drawing.Point(83, 132);
            this.txtWorkshopName.Name = "txtWorkshopName";
            this.txtWorkshopName.Size = new System.Drawing.Size(150, 21);
            this.txtWorkshopName.TabIndex = 17;
            // 
            // lblResponsiblePerson
            // 
            this.lblResponsiblePerson.AutoSize = true;
            this.lblResponsiblePerson.Location = new System.Drawing.Point(250, 135);
            this.lblResponsiblePerson.Name = "lblResponsiblePerson";
            this.lblResponsiblePerson.Size = new System.Drawing.Size(53, 12);
            this.lblResponsiblePerson.TabIndex = 18;
            this.lblResponsiblePerson.Text = "负责人：";
            // 
            // txtResponsiblePerson
            // 
            this.txtResponsiblePerson.Location = new System.Drawing.Point(321, 132);
            this.txtResponsiblePerson.Name = "txtResponsiblePerson";
            this.txtResponsiblePerson.Size = new System.Drawing.Size(150, 21);
            this.txtResponsiblePerson.TabIndex = 19;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(12, 165);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(65, 12);
            this.lblCustomerName.TabIndex = 20;
            this.lblCustomerName.Text = "客户名称：";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(83, 162);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(150, 21);
            this.txtCustomerName.TabIndex = 21;            // 
            // lblSalesOrderNumber
            //
            this.lblSalesOrderNumber.AutoSize = true;
            this.lblSalesOrderNumber.Location = new System.Drawing.Point(250, 165);
            this.lblSalesOrderNumber.Name = "lblSalesOrderNumber";
            this.lblSalesOrderNumber.Size = new System.Drawing.Size(65, 12);
            this.lblSalesOrderNumber.TabIndex = 22;
            this.lblSalesOrderNumber.Text = "销售单号：";
            //
            // txtSalesOrderNumber
            //
            this.txtSalesOrderNumber.Location = new System.Drawing.Point(321, 162);
            this.txtSalesOrderNumber.Name = "txtSalesOrderNumber";
            this.txtSalesOrderNumber.Size = new System.Drawing.Size(150, 21);
            this.txtSalesOrderNumber.TabIndex = 23;
            // 
            // lblPlanStartTime
            // 
            this.lblPlanStartTime.AutoSize = true;
            this.lblPlanStartTime.Location = new System.Drawing.Point(12, 195);
            this.lblPlanStartTime.Name = "lblPlanStartTime";
            this.lblPlanStartTime.Size = new System.Drawing.Size(77, 12);
            this.lblPlanStartTime.TabIndex = 24;
            this.lblPlanStartTime.Text = "计划开始时间：";
            // 
            // dtpPlanStartTime
            // 
            this.dtpPlanStartTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpPlanStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPlanStartTime.Location = new System.Drawing.Point(95, 192);
            this.dtpPlanStartTime.Name = "dtpPlanStartTime";
            this.dtpPlanStartTime.Size = new System.Drawing.Size(138, 21);
            this.dtpPlanStartTime.TabIndex = 25;
            // 
            // lblPlanEndTime
            // 
            this.lblPlanEndTime.AutoSize = true;
            this.lblPlanEndTime.Location = new System.Drawing.Point(250, 195);
            this.lblPlanEndTime.Name = "lblPlanEndTime";
            this.lblPlanEndTime.Size = new System.Drawing.Size(77, 12);
            this.lblPlanEndTime.TabIndex = 26;
            this.lblPlanEndTime.Text = "计划结束时间：";
            // 
            // dtpPlanEndTime
            //
            this.dtpPlanEndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpPlanEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPlanEndTime.Location = new System.Drawing.Point(333, 192);
            this.dtpPlanEndTime.Name = "dtpPlanEndTime";
            this.dtpPlanEndTime.Size = new System.Drawing.Size(150, 21);
            this.dtpPlanEndTime.TabIndex = 27;
            // 
            // lblRemarks
            // 
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Location = new System.Drawing.Point(12, 225);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(41, 12);
            this.lblRemarks.TabIndex = 28;
            this.lblRemarks.Text = "备注：";
            // 
            // txtRemarks
            //
            this.txtRemarks.Location = new System.Drawing.Point(83, 222);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(400, 60);
            this.txtRemarks.TabIndex = 29;            //
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(315, 300);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(396, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ProductionOrderEditForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.lblRemarks);
            this.Controls.Add(this.dtpPlanEndTime);
            this.Controls.Add(this.lblPlanEndTime);
            this.Controls.Add(this.dtpPlanStartTime);
            this.Controls.Add(this.lblPlanStartTime);
            this.Controls.Add(this.txtSalesOrderNumber);
            this.Controls.Add(this.lblSalesOrderNumber);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.txtResponsiblePerson);
            this.Controls.Add(this.lblResponsiblePerson);
            this.Controls.Add(this.txtWorkshopName);
            this.Controls.Add(this.lblWorkshopName);
            this.Controls.Add(this.cmbPriority);
            this.Controls.Add(this.lblPriority);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.txtActualQuantity);
            this.Controls.Add(this.lblActualQuantity);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.txtProductCode);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.txtOrderNo);
            this.Controls.Add(this.lblOrderNo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductionOrderEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "生产订单编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrderNo;
        private System.Windows.Forms.TextBox txtOrderNo;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label lblActualQuantity;
        private System.Windows.Forms.TextBox txtActualQuantity;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.ComboBox cmbPriority;
        private System.Windows.Forms.Label lblWorkshopName;
        private System.Windows.Forms.TextBox txtWorkshopName;
        private System.Windows.Forms.Label lblResponsiblePerson;
        private System.Windows.Forms.TextBox txtResponsiblePerson;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.Label lblSalesOrderNumber;
        private System.Windows.Forms.TextBox txtSalesOrderNumber;
        private System.Windows.Forms.Label lblPlanStartTime;
        private System.Windows.Forms.DateTimePicker dtpPlanStartTime;
        private System.Windows.Forms.Label lblPlanEndTime;
        private System.Windows.Forms.DateTimePicker dtpPlanEndTime;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}