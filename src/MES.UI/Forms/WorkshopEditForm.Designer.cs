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
            // lblManager
            //
            this.lblManager.AutoSize = true;
            this.lblManager.Location = new System.Drawing.Point(30, 170);
            this.lblManager.Name = "lblManager";
            this.lblManager.Size = new System.Drawing.Size(65, 12);
            this.lblManager.TabIndex = 8;
            this.lblManager.Text = "负责人：";
            //
            // txtManager
            //
            this.txtManager.Location = new System.Drawing.Point(120, 167);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(200, 21);
            this.txtManager.TabIndex = 9;
            //
            // lblPhone
            //
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(30, 205);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(65, 12);
            this.lblPhone.TabIndex = 10;
            this.lblPhone.Text = "联系电话：";
            //
            // txtPhone
            //
            this.txtPhone.Location = new System.Drawing.Point(120, 202);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(200, 21);
            this.txtPhone.TabIndex = 11;
            //
            // lblLocation
            //
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(30, 240);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(41, 12);
            this.lblLocation.TabIndex = 12;
            this.lblLocation.Text = "位置：";
            //
            // txtLocation
            //
            this.txtLocation.Location = new System.Drawing.Point(120, 237);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(200, 21);
            this.txtLocation.TabIndex = 13;
            //
            // lblArea
            //
            this.lblArea.AutoSize = true;
            this.lblArea.Location = new System.Drawing.Point(30, 275);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(65, 12);
            this.lblArea.TabIndex = 14;
            this.lblArea.Text = "面积(㎡)：";
            //
            // txtArea
            //
            this.txtArea.Location = new System.Drawing.Point(120, 272);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(200, 21);
            this.txtArea.TabIndex = 15;
            //
            // lblProductionCapacity
            //
            this.lblProductionCapacity.AutoSize = true;
            this.lblProductionCapacity.Location = new System.Drawing.Point(30, 310);
            this.lblProductionCapacity.Name = "lblProductionCapacity";
            this.lblProductionCapacity.Size = new System.Drawing.Size(89, 12);
            this.lblProductionCapacity.TabIndex = 16;
            this.lblProductionCapacity.Text = "产能(件/天)：";
            //
            // txtProductionCapacity
            //
            this.txtProductionCapacity.Location = new System.Drawing.Point(120, 307);
            this.txtProductionCapacity.Name = "txtProductionCapacity";
            this.txtProductionCapacity.Size = new System.Drawing.Size(200, 21);
            this.txtProductionCapacity.TabIndex = 17;
            //
            // lblEmployeeCount
            //
            this.lblEmployeeCount.AutoSize = true;
            this.lblEmployeeCount.Location = new System.Drawing.Point(30, 345);
            this.lblEmployeeCount.Name = "lblEmployeeCount";
            this.lblEmployeeCount.Size = new System.Drawing.Size(65, 12);
            this.lblEmployeeCount.TabIndex = 18;
            this.lblEmployeeCount.Text = "员工数量：";
            //
            // txtEmployeeCount
            //
            this.txtEmployeeCount.Location = new System.Drawing.Point(120, 342);
            this.txtEmployeeCount.Name = "txtEmployeeCount";
            this.txtEmployeeCount.Size = new System.Drawing.Size(200, 21);
            this.txtEmployeeCount.TabIndex = 19;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(30, 380);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "状态：";
            //
            // cmbStatus
            //
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(120, 377);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(200, 20);
            this.cmbStatus.TabIndex = 21;
            // 
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(30, 415);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(41, 12);
            this.lblDescription.TabIndex = 22;
            this.lblDescription.Text = "描述：";
            //
            // txtDescription
            //
            this.txtDescription.Location = new System.Drawing.Point(120, 412);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 60);
            this.txtDescription.TabIndex = 23;
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(180, 490);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(270, 490);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // WorkshopEditForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 540);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtEmployeeCount);
            this.Controls.Add(this.lblEmployeeCount);
            this.Controls.Add(this.txtProductionCapacity);
            this.Controls.Add(this.lblProductionCapacity);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.lblArea);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtManager);
            this.Controls.Add(this.lblManager);
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
        private System.Windows.Forms.Label lblManager;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.TextBox txtArea;
        private System.Windows.Forms.Label lblProductionCapacity;
        private System.Windows.Forms.TextBox txtProductionCapacity;
        private System.Windows.Forms.Label lblEmployeeCount;
        private System.Windows.Forms.TextBox txtEmployeeCount;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
