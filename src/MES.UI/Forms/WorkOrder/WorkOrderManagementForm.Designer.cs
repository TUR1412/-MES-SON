namespace MES.UI.Forms.WorkOrder
{
    partial class WorkOrderManagementForm
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
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCancelWorkOrder = new System.Windows.Forms.Button();
            this.btnSubmitWorkOrder = new System.Windows.Forms.Button();
            this.btnCreateWorkOrder = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.dgvWorkOrders = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1200, 60);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(181, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📋 工单管理中心";
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.cmbStatus);
            this.panelSearch.Controls.Add(this.lblStatus);
            this.panelSearch.Controls.Add(this.dtpEndDate);
            this.panelSearch.Controls.Add(this.lblEndDate);
            this.panelSearch.Controls.Add(this.dtpStartDate);
            this.panelSearch.Controls.Add(this.lblStartDate);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 60);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1200, 80);
            this.panelSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(1050, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "🔍 搜索";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtSearch.Location = new System.Drawing.Point(880, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 23);
            this.txtSearch.TabIndex = 7;
            // this.txtSearch.PlaceholderText = "输入工单号或产品名称"; // .NET Framework 4.8不支持PlaceholderText
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblSearch.Location = new System.Drawing.Point(820, 30);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 17);
            this.lblSearch.TabIndex = 6;
            this.lblSearch.Text = "关键字：";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "全部状态",
            "待开始",
            "进行中",
            "已暂停",
            "已完成",
            "已取消"});
            this.cmbStatus.Location = new System.Drawing.Point(650, 27);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(120, 25);
            this.cmbStatus.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblStatus.Location = new System.Drawing.Point(590, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 17);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "状态：";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(380, 27);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(150, 23);
            this.dtpEndDate.TabIndex = 3;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblEndDate.Location = new System.Drawing.Point(300, 30);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(80, 17);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "结束日期：";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(100, 27);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(150, 23);
            this.dtpStartDate.TabIndex = 1;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblStartDate.Location = new System.Drawing.Point(20, 30);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(80, 17);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "开始日期：";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnCancelWorkOrder);
            this.panelButtons.Controls.Add(this.btnSubmitWorkOrder);
            this.panelButtons.Controls.Add(this.btnCreateWorkOrder);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 140);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1200, 60);
            this.panelButtons.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(350, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnCancelWorkOrder
            // 
            this.btnCancelWorkOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancelWorkOrder.Enabled = false;
            this.btnCancelWorkOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelWorkOrder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCancelWorkOrder.ForeColor = System.Drawing.Color.White;
            this.btnCancelWorkOrder.Location = new System.Drawing.Point(240, 15);
            this.btnCancelWorkOrder.Name = "btnCancelWorkOrder";
            this.btnCancelWorkOrder.Size = new System.Drawing.Size(100, 30);
            this.btnCancelWorkOrder.TabIndex = 2;
            this.btnCancelWorkOrder.Text = "❌ 取消工单";
            this.btnCancelWorkOrder.UseVisualStyleBackColor = false;
            // 
            // btnSubmitWorkOrder
            // 
            this.btnSubmitWorkOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnSubmitWorkOrder.Enabled = false;
            this.btnSubmitWorkOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmitWorkOrder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSubmitWorkOrder.ForeColor = System.Drawing.Color.White;
            this.btnSubmitWorkOrder.Location = new System.Drawing.Point(130, 15);
            this.btnSubmitWorkOrder.Name = "btnSubmitWorkOrder";
            this.btnSubmitWorkOrder.Size = new System.Drawing.Size(100, 30);
            this.btnSubmitWorkOrder.TabIndex = 1;
            this.btnSubmitWorkOrder.Text = "📤 提交工单";
            this.btnSubmitWorkOrder.UseVisualStyleBackColor = false;
            // 
            // btnCreateWorkOrder
            // 
            this.btnCreateWorkOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCreateWorkOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateWorkOrder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCreateWorkOrder.ForeColor = System.Drawing.Color.White;
            this.btnCreateWorkOrder.Location = new System.Drawing.Point(20, 15);
            this.btnCreateWorkOrder.Name = "btnCreateWorkOrder";
            this.btnCreateWorkOrder.Size = new System.Drawing.Size(100, 30);
            this.btnCreateWorkOrder.TabIndex = 0;
            this.btnCreateWorkOrder.Text = "➕ 创建工单";
            this.btnCreateWorkOrder.UseVisualStyleBackColor = false;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.dgvWorkOrders);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 200);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1200, 400);
            this.panelMain.TabIndex = 3;
            // 
            // dgvWorkOrders
            // 
            this.dgvWorkOrders.AllowUserToAddRows = false;
            this.dgvWorkOrders.AllowUserToDeleteRows = false;
            this.dgvWorkOrders.BackgroundColor = System.Drawing.Color.White;
            this.dgvWorkOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvWorkOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWorkOrders.Location = new System.Drawing.Point(0, 0);
            this.dgvWorkOrders.MultiSelect = false;
            this.dgvWorkOrders.Name = "dgvWorkOrders";
            this.dgvWorkOrders.ReadOnly = true;
            this.dgvWorkOrders.RowTemplate.Height = 25;
            this.dgvWorkOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkOrders.Size = new System.Drawing.Size(1200, 400);
            this.dgvWorkOrders.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelBottom.Controls.Add(this.lblTotal);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 600);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1200, 40);
            this.panelBottom.TabIndex = 4;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblTotal.Location = new System.Drawing.Point(20, 12);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(68, 17);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "共 0 条记录";
            // 
            // WorkOrderManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 640);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);
            this.Name = "WorkOrderManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工单管理中心";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCancelWorkOrder;
        private System.Windows.Forms.Button btnSubmitWorkOrder;
        private System.Windows.Forms.Button btnCreateWorkOrder;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.DataGridView dgvWorkOrders;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblTotal;
    }
}
