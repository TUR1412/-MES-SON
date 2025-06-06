namespace MES.UI.Forms
{
    partial class WorkshopEditForm
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
            this.lblWorkshopCode = new System.Windows.Forms.Label();
            this.txtWorkshopCode = new System.Windows.Forms.TextBox();
            this.lblWorkshopName = new System.Windows.Forms.Label();
            this.txtWorkshopName = new System.Windows.Forms.TextBox();
            this.lblWorkshopType = new System.Windows.Forms.Label();
            this.cmbWorkshopType = new System.Windows.Forms.ComboBox();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.lblManager = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.lblArea = new System.Windows.Forms.Label();
            this.txtArea = new System.Windows.Forms.TextBox();
            this.lblProductionCapacity = new System.Windows.Forms.Label();
            this.txtProductionCapacity = new System.Windows.Forms.TextBox();
            this.lblEmployeeCount = new System.Windows.Forms.Label();
            this.txtEmployeeCount = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWorkshopCode
            // 
            this.lblWorkshopCode.AutoSize = true;
            this.lblWorkshopCode.Location = new System.Drawing.Point(30, 30);
            this.lblWorkshopCode.Name = "lblWorkshopCode";
            this.lblWorkshopCode.Size = new System.Drawing.Size(65, 12);
            this.lblWorkshopCode.TabIndex = 0;
            this.lblWorkshopCode.Text = "车间编码：";
            // 
            // txtWorkshopCode
            // 
            this.txtWorkshopCode.Location = new System.Drawing.Point(120, 27);
            this.txtWorkshopCode.Name = "txtWorkshopCode";
            this.txtWorkshopCode.Size = new System.Drawing.Size(200, 21);
            this.txtWorkshopCode.TabIndex = 1;
            // 
            // lblWorkshopName
            // 
            this.lblWorkshopName.AutoSize = true;
            this.lblWorkshopName.Location = new System.Drawing.Point(30, 65);
            this.lblWorkshopName.Name = "lblWorkshopName";
            this.lblWorkshopName.Size = new System.Drawing.Size(65, 12);
            this.lblWorkshopName.TabIndex = 2;
            this.lblWorkshopName.Text = "车间名称：";
            // 
            // txtWorkshopName
            // 
            this.txtWorkshopName.Location = new System.Drawing.Point(120, 62);
            this.txtWorkshopName.Name = "txtWorkshopName";
            this.txtWorkshopName.Size = new System.Drawing.Size(200, 21);
            this.txtWorkshopName.TabIndex = 3;
            // 
            // lblWorkshopType
            // 
            this.lblWorkshopType.AutoSize = true;
            this.lblWorkshopType.Location = new System.Drawing.Point(30, 100);
            this.lblWorkshopType.Name = "lblWorkshopType";
            this.lblWorkshopType.Size = new System.Drawing.Size(65, 12);
            this.lblWorkshopType.TabIndex = 4;
            this.lblWorkshopType.Text = "车间类型：";
            // 
            // cmbWorkshopType
            // 
            this.cmbWorkshopType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkshopType.FormattingEnabled = true;
            this.cmbWorkshopType.Location = new System.Drawing.Point(120, 97);
            this.cmbWorkshopType.Name = "cmbWorkshopType";
            this.cmbWorkshopType.Size = new System.Drawing.Size(200, 20);
            this.cmbWorkshopType.TabIndex = 5;
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new System.Drawing.Point(30, 135);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(65, 12);
            this.lblDepartment.TabIndex = 6;
            this.lblDepartment.Text = "所属部门：";
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(120, 132);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(200, 20);
            this.cmbDepartment.TabIndex = 7;
            // 
            // lblCapacity
            // 
            this.lblCapacity.AutoSize = true;
            this.lblCapacity.Location = new System.Drawing.Point(30, 170);
            this.lblCapacity.Name = "lblCapacity";
            this.lblCapacity.Size = new System.Drawing.Size(89, 12);
            this.lblCapacity.TabIndex = 8;
            this.lblCapacity.Text = "产能(件/天)：";
            // 
            // txtCapacity
            // 
            this.txtCapacity.Location = new System.Drawing.Point(120, 167);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.Size = new System.Drawing.Size(200, 21);
            this.txtCapacity.TabIndex = 9;
            // 
            // lblLocationId
            // 
            this.lblLocationId.AutoSize = true;
            this.lblLocationId.Location = new System.Drawing.Point(30, 205);
            this.lblLocationId.Name = "lblLocationId";
            this.lblLocationId.Size = new System.Drawing.Size(41, 12);
            this.lblLocationId.TabIndex = 10;
            this.lblLocationId.Text = "位置：";
            // 
            // txtLocationId
            // 
            this.txtLocationId.Location = new System.Drawing.Point(120, 202);
            this.txtLocationId.Name = "txtLocationId";
            this.txtLocationId.Size = new System.Drawing.Size(200, 21);
            this.txtLocationId.TabIndex = 11;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(30, 240);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(41, 12);
            this.lblDescription.TabIndex = 12;
            this.lblDescription.Text = "描述：";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(120, 237);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 60);
            this.txtDescription.TabIndex = 13;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(120, 315);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(48, 16);
            this.chkStatus.TabIndex = 14;
            this.chkStatus.Text = "启用";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(180, 350);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // WorkshopEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 391);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtLocationId);
            this.Controls.Add(this.lblLocationId);
            this.Controls.Add(this.txtCapacity);
            this.Controls.Add(this.lblCapacity);
            this.Controls.Add(this.cmbDepartment);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.cmbWorkshopType);
            this.Controls.Add(this.lblWorkshopType);
            this.Controls.Add(this.txtWorkshopName);
            this.Controls.Add(this.lblWorkshopName);
            this.Controls.Add(this.txtWorkshopCode);
            this.Controls.Add(this.lblWorkshopCode);
            this.Name = "WorkshopEditForm";
            this.Text = "车间编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWorkshopCode;
        private System.Windows.Forms.TextBox txtWorkshopCode;
        private System.Windows.Forms.Label lblWorkshopName;
        private System.Windows.Forms.TextBox txtWorkshopName;
        private System.Windows.Forms.Label lblWorkshopType;
        private System.Windows.Forms.ComboBox cmbWorkshopType;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label lblCapacity;
        private System.Windows.Forms.TextBox txtCapacity;
        private System.Windows.Forms.Label lblLocationId;
        private System.Windows.Forms.TextBox txtLocationId;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
