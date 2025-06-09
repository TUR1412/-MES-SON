namespace MES.UI.Forms.Workshop
{
    partial class EquipmentStatusForm
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
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cmbWorkshop = new System.Windows.Forms.ComboBox();
            this.lblWorkshop = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnAlarm = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.dgvEquipment = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).BeginInit();
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
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(181, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🔧 设备状态管理";
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.cmbWorkshop);
            this.panelSearch.Controls.Add(this.lblWorkshop);
            this.panelSearch.Controls.Add(this.cmbStatus);
            this.panelSearch.Controls.Add(this.lblStatus);
            this.panelSearch.Controls.Add(this.cmbType);
            this.panelSearch.Controls.Add(this.lblType);
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
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtSearch.Location = new System.Drawing.Point(880, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 23);
            this.txtSearch.TabIndex = 7;
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
            // cmbWorkshop
            // 
            this.cmbWorkshop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkshop.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbWorkshop.FormattingEnabled = true;
            this.cmbWorkshop.Location = new System.Drawing.Point(80, 27);
            this.cmbWorkshop.Name = "cmbWorkshop";
            this.cmbWorkshop.Size = new System.Drawing.Size(150, 25);
            this.cmbWorkshop.TabIndex = 1;
            // 
            // lblWorkshop
            // 
            this.lblWorkshop.AutoSize = true;
            this.lblWorkshop.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblWorkshop.Location = new System.Drawing.Point(20, 30);
            this.lblWorkshop.Name = "lblWorkshop";
            this.lblWorkshop.Size = new System.Drawing.Size(56, 17);
            this.lblWorkshop.TabIndex = 0;
            this.lblWorkshop.Text = "车间：";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(320, 27);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(120, 25);
            this.cmbStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblStatus.Location = new System.Drawing.Point(260, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 17);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "状态：";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(540, 27);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(120, 25);
            this.cmbType.TabIndex = 5;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblType.Location = new System.Drawing.Point(480, 30);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(56, 17);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "类型：";
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnMaintenance);
            this.panelButtons.Controls.Add(this.btnStart);
            this.panelButtons.Controls.Add(this.btnStop);
            this.panelButtons.Controls.Add(this.btnAlarm);
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
            this.btnRefresh.Location = new System.Drawing.Point(450, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnMaintenance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaintenance.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnMaintenance.ForeColor = System.Drawing.Color.White;
            this.btnMaintenance.Location = new System.Drawing.Point(250, 15);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(80, 30);
            this.btnMaintenance.TabIndex = 2;
            this.btnMaintenance.Text = "维护";
            this.btnMaintenance.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(50, 15);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(80, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(150, 15);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 30);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = false;
            // 
            // btnAlarm
            // 
            this.btnAlarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnAlarm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlarm.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnAlarm.ForeColor = System.Drawing.Color.White;
            this.btnAlarm.Location = new System.Drawing.Point(350, 15);
            this.btnAlarm.Name = "btnAlarm";
            this.btnAlarm.Size = new System.Drawing.Size(80, 30);
            this.btnAlarm.TabIndex = 3;
            this.btnAlarm.Text = "报警";
            this.btnAlarm.UseVisualStyleBackColor = false;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.dgvEquipment);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 200);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1200, 400);
            this.panelMain.TabIndex = 3;
            // 
            // dgvEquipment
            // 
            this.dgvEquipment.AllowUserToAddRows = false;
            this.dgvEquipment.AllowUserToDeleteRows = false;
            this.dgvEquipment.BackgroundColor = System.Drawing.Color.White;
            this.dgvEquipment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEquipment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEquipment.Location = new System.Drawing.Point(0, 0);
            this.dgvEquipment.Name = "dgvEquipment";
            this.dgvEquipment.ReadOnly = true;
            this.dgvEquipment.RowTemplate.Height = 23;
            this.dgvEquipment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEquipment.Size = new System.Drawing.Size(1200, 400);
            this.dgvEquipment.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelBottom.Controls.Add(this.lblTotal);
            this.panelBottom.Controls.Add(this.lblSummary);
            this.panelBottom.Controls.Add(this.lblLastUpdate);
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
            this.lblTotal.Text = "共 0 台设备";
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblSummary.Location = new System.Drawing.Point(200, 12);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(200, 17);
            this.lblSummary.TabIndex = 1;
            this.lblSummary.Text = "运行：0 | 停止：0 | 故障：0 | 维护：0";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblLastUpdate.Location = new System.Drawing.Point(1000, 12);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(180, 17);
            this.lblLastUpdate.TabIndex = 2;
            this.lblLastUpdate.Text = "最后更新：2025-06-09 09:11:51";
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 30000;
            // 
            // EquipmentStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 640);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);
            this.Name = "EquipmentStatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备状态管理";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipment)).EndInit();
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
        private System.Windows.Forms.ComboBox cmbWorkshop;
        private System.Windows.Forms.Label lblWorkshop;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnMaintenance;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnAlarm;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.DataGridView dgvEquipment;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.Timer timerRefresh;
    }
}
