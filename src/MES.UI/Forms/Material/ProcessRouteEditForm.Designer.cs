namespace MES.UI.Forms.Material
{
    partial class ProcessRouteEditForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupBoxDescription = new System.Windows.Forms.GroupBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.groupBoxBasic = new System.Windows.Forms.GroupBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtRouteName = new System.Windows.Forms.TextBox();
            this.lblRouteName = new System.Windows.Forms.Label();
            this.txtRouteCode = new System.Windows.Forms.TextBox();
            this.lblRouteCode = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.groupBoxDescription.SuspendLayout();
            this.groupBoxBasic.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(979, 90);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 27);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(232, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "⚙️ 工艺路线编辑";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.groupBoxDescription);
            this.panelMain.Controls.Add(this.groupBoxBasic);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 90);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.panelMain.Size = new System.Drawing.Size(979, 585);
            this.panelMain.TabIndex = 1;
            // 
            // groupBoxDescription
            // 
            this.groupBoxDescription.Controls.Add(this.txtDescription);
            this.groupBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDescription.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxDescription.Location = new System.Drawing.Point(30, 330);
            this.groupBoxDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxDescription.Name = "groupBoxDescription";
            this.groupBoxDescription.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxDescription.Size = new System.Drawing.Size(919, 225);
            this.groupBoxDescription.TabIndex = 1;
            this.groupBoxDescription.TabStop = false;
            this.groupBoxDescription.Text = "描述信息";
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtDescription.Location = new System.Drawing.Point(4, 31);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(911, 190);
            this.txtDescription.TabIndex = 0;
            // 
            // groupBoxBasic
            // 
            this.groupBoxBasic.Controls.Add(this.cmbStatus);
            this.groupBoxBasic.Controls.Add(this.lblStatus);
            this.groupBoxBasic.Controls.Add(this.txtVersion);
            this.groupBoxBasic.Controls.Add(this.lblVersion);
            this.groupBoxBasic.Controls.Add(this.cmbProduct);
            this.groupBoxBasic.Controls.Add(this.lblProduct);
            this.groupBoxBasic.Controls.Add(this.txtRouteName);
            this.groupBoxBasic.Controls.Add(this.lblRouteName);
            this.groupBoxBasic.Controls.Add(this.txtRouteCode);
            this.groupBoxBasic.Controls.Add(this.lblRouteCode);
            this.groupBoxBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasic.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxBasic.Location = new System.Drawing.Point(30, 30);
            this.groupBoxBasic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxBasic.Name = "groupBoxBasic";
            this.groupBoxBasic.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxBasic.Size = new System.Drawing.Size(919, 300);
            this.groupBoxBasic.TabIndex = 0;
            this.groupBoxBasic.TabStop = false;
            this.groupBoxBasic.Text = "基本信息";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(570, 225);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(223, 32);
            this.cmbStatus.TabIndex = 9;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblStatus.Location = new System.Drawing.Point(480, 230);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(64, 24);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "状态：";
            // 
            // txtVersion
            // 
            this.txtVersion.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtVersion.Location = new System.Drawing.Point(150, 225);
            this.txtVersion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(223, 31);
            this.txtVersion.TabIndex = 7;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblVersion.Location = new System.Drawing.Point(45, 230);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(82, 24);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "版本号：";
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(150, 165);
            this.cmbProduct.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(643, 32);
            this.cmbProduct.TabIndex = 5;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblProduct.Location = new System.Drawing.Point(45, 170);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(64, 24);
            this.lblProduct.TabIndex = 4;
            this.lblProduct.Text = "产品：";
            // 
            // txtRouteName
            // 
            this.txtRouteName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtRouteName.Location = new System.Drawing.Point(150, 105);
            this.txtRouteName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRouteName.Name = "txtRouteName";
            this.txtRouteName.Size = new System.Drawing.Size(643, 31);
            this.txtRouteName.TabIndex = 3;
            // 
            // lblRouteName
            // 
            this.lblRouteName.AutoSize = true;
            this.lblRouteName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblRouteName.Location = new System.Drawing.Point(45, 110);
            this.lblRouteName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRouteName.Name = "lblRouteName";
            this.lblRouteName.Size = new System.Drawing.Size(100, 24);
            this.lblRouteName.TabIndex = 2;
            this.lblRouteName.Text = "路线名称：";
            // 
            // txtRouteCode
            // 
            this.txtRouteCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtRouteCode.Location = new System.Drawing.Point(150, 45);
            this.txtRouteCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRouteCode.Name = "txtRouteCode";
            this.txtRouteCode.Size = new System.Drawing.Size(298, 31);
            this.txtRouteCode.TabIndex = 1;
            // 
            // lblRouteCode
            // 
            this.lblRouteCode.AutoSize = true;
            this.lblRouteCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblRouteCode.Location = new System.Drawing.Point(45, 50);
            this.lblRouteCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRouteCode.Name = "lblRouteCode";
            this.lblRouteCode.Size = new System.Drawing.Size(100, 24);
            this.lblRouteCode.TabIndex = 0;
            this.lblRouteCode.Text = "路线编码：";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 675);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(979, 90);
            this.panelButtons.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(600, 22);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 52);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(780, 22);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 52);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // ProcessRouteEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 765);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessRouteEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "工艺路线编辑";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.groupBoxDescription.ResumeLayout(false);
            this.groupBoxDescription.PerformLayout();
            this.groupBoxBasic.ResumeLayout(false);
            this.groupBoxBasic.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupBoxBasic;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtRouteName;
        private System.Windows.Forms.Label lblRouteName;
        private System.Windows.Forms.TextBox txtRouteCode;
        private System.Windows.Forms.Label lblRouteCode;
        private System.Windows.Forms.GroupBox groupBoxDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}
