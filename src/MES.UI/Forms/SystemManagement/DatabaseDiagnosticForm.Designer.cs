namespace MES.UI.Forms.SystemManagement
{
    partial class DatabaseDiagnosticForm
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupBoxConnection = new System.Windows.Forms.GroupBox();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.lblConnectionStatusValue = new System.Windows.Forms.Label();
            this.lblServerInfo = new System.Windows.Forms.Label();
            this.lblServerInfoValue = new System.Windows.Forms.Label();
            this.lblDatabaseName = new System.Windows.Forms.Label();
            this.lblDatabaseNameValue = new System.Windows.Forms.Label();
            this.groupBoxPerformance = new System.Windows.Forms.GroupBox();
            this.lblConnectionCount = new System.Windows.Forms.Label();
            this.lblConnectionCountValue = new System.Windows.Forms.Label();
            this.lblDatabaseSize = new System.Windows.Forms.Label();
            this.lblDatabaseSizeValue = new System.Windows.Forms.Label();
            this.lblTableCount = new System.Windows.Forms.Label();
            this.lblTableCountValue = new System.Windows.Forms.Label();
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.dgvDiagnosticInfo = new System.Windows.Forms.DataGridView();
            this.colProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.panelMain.SuspendLayout();
            this.groupBoxConnection.SuspendLayout();
            this.groupBoxPerformance.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiagnosticInfo)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.groupBoxDetails);
            this.panelMain.Controls.Add(this.groupBoxPerformance);
            this.panelMain.Controls.Add(this.groupBoxConnection);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(10);
            this.panelMain.Size = new System.Drawing.Size(800, 550);
            this.panelMain.TabIndex = 0;
            // 
            // groupBoxConnection
            // 
            this.groupBoxConnection.Controls.Add(this.lblDatabaseNameValue);
            this.groupBoxConnection.Controls.Add(this.lblDatabaseName);
            this.groupBoxConnection.Controls.Add(this.lblServerInfoValue);
            this.groupBoxConnection.Controls.Add(this.lblServerInfo);
            this.groupBoxConnection.Controls.Add(this.lblConnectionStatusValue);
            this.groupBoxConnection.Controls.Add(this.lblConnectionStatus);
            this.groupBoxConnection.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxConnection.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxConnection.Location = new System.Drawing.Point(10, 10);
            this.groupBoxConnection.Name = "groupBoxConnection";
            this.groupBoxConnection.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxConnection.Size = new System.Drawing.Size(780, 120);
            this.groupBoxConnection.TabIndex = 0;
            this.groupBoxConnection.TabStop = false;
            this.groupBoxConnection.Text = "数据库连接信息";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConnectionStatus.Location = new System.Drawing.Point(20, 30);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(68, 17);
            this.lblConnectionStatus.TabIndex = 0;
            this.lblConnectionStatus.Text = "连接状态：";
            // 
            // lblConnectionStatusValue
            // 
            this.lblConnectionStatusValue.AutoSize = true;
            this.lblConnectionStatusValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblConnectionStatusValue.ForeColor = System.Drawing.Color.Green;
            this.lblConnectionStatusValue.Location = new System.Drawing.Point(100, 30);
            this.lblConnectionStatusValue.Name = "lblConnectionStatusValue";
            this.lblConnectionStatusValue.Size = new System.Drawing.Size(44, 17);
            this.lblConnectionStatusValue.TabIndex = 1;
            this.lblConnectionStatusValue.Text = "正常";
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.AutoSize = true;
            this.lblServerInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblServerInfo.Location = new System.Drawing.Point(20, 60);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(68, 17);
            this.lblServerInfo.TabIndex = 2;
            this.lblServerInfo.Text = "服务器：";
            // 
            // lblServerInfoValue
            // 
            this.lblServerInfoValue.AutoSize = true;
            this.lblServerInfoValue.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblServerInfoValue.Location = new System.Drawing.Point(100, 60);
            this.lblServerInfoValue.Name = "lblServerInfoValue";
            this.lblServerInfoValue.Size = new System.Drawing.Size(68, 17);
            this.lblServerInfoValue.TabIndex = 3;
            this.lblServerInfoValue.Text = "localhost";
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.AutoSize = true;
            this.lblDatabaseName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDatabaseName.Location = new System.Drawing.Point(20, 90);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(68, 17);
            this.lblDatabaseName.TabIndex = 4;
            this.lblDatabaseName.Text = "数据库：";
            // 
            // lblDatabaseNameValue
            // 
            this.lblDatabaseNameValue.AutoSize = true;
            this.lblDatabaseNameValue.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDatabaseNameValue.Location = new System.Drawing.Point(100, 90);
            this.lblDatabaseNameValue.Name = "lblDatabaseNameValue";
            this.lblDatabaseNameValue.Size = new System.Drawing.Size(44, 17);
            this.lblDatabaseNameValue.TabIndex = 5;
            this.lblDatabaseNameValue.Text = "mes_db";
            // 
            // groupBoxPerformance
            // 
            this.groupBoxPerformance.Controls.Add(this.lblTableCountValue);
            this.groupBoxPerformance.Controls.Add(this.lblTableCount);
            this.groupBoxPerformance.Controls.Add(this.lblDatabaseSizeValue);
            this.groupBoxPerformance.Controls.Add(this.lblDatabaseSize);
            this.groupBoxPerformance.Controls.Add(this.lblConnectionCountValue);
            this.groupBoxPerformance.Controls.Add(this.lblConnectionCount);
            this.groupBoxPerformance.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxPerformance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxPerformance.Location = new System.Drawing.Point(10, 130);
            this.groupBoxPerformance.Name = "groupBoxPerformance";
            this.groupBoxPerformance.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxPerformance.Size = new System.Drawing.Size(780, 120);
            this.groupBoxPerformance.TabIndex = 1;
            this.groupBoxPerformance.TabStop = false;
            this.groupBoxPerformance.Text = "性能统计信息";
            // 
            // lblConnectionCount
            // 
            this.lblConnectionCount.AutoSize = true;
            this.lblConnectionCount.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConnectionCount.Location = new System.Drawing.Point(20, 30);
            this.lblConnectionCount.Name = "lblConnectionCount";
            this.lblConnectionCount.Size = new System.Drawing.Size(68, 17);
            this.lblConnectionCount.TabIndex = 0;
            this.lblConnectionCount.Text = "连接数：";
            // 
            // lblConnectionCountValue
            // 
            this.lblConnectionCountValue.AutoSize = true;
            this.lblConnectionCountValue.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConnectionCountValue.Location = new System.Drawing.Point(100, 30);
            this.lblConnectionCountValue.Name = "lblConnectionCountValue";
            this.lblConnectionCountValue.Size = new System.Drawing.Size(17, 17);
            this.lblConnectionCountValue.TabIndex = 1;
            this.lblConnectionCountValue.Text = "1";
            // 
            // lblDatabaseSize
            // 
            this.lblDatabaseSize.AutoSize = true;
            this.lblDatabaseSize.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDatabaseSize.Location = new System.Drawing.Point(20, 60);
            this.lblDatabaseSize.Name = "lblDatabaseSize";
            this.lblDatabaseSize.Size = new System.Drawing.Size(80, 17);
            this.lblDatabaseSize.TabIndex = 2;
            this.lblDatabaseSize.Text = "数据库大小：";
            // 
            // lblDatabaseSizeValue
            // 
            this.lblDatabaseSizeValue.AutoSize = true;
            this.lblDatabaseSizeValue.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDatabaseSizeValue.Location = new System.Drawing.Point(110, 60);
            this.lblDatabaseSizeValue.Name = "lblDatabaseSizeValue";
            this.lblDatabaseSizeValue.Size = new System.Drawing.Size(44, 17);
            this.lblDatabaseSizeValue.TabIndex = 3;
            this.lblDatabaseSizeValue.Text = "0 MB";
            // 
            // lblTableCount
            // 
            this.lblTableCount.AutoSize = true;
            this.lblTableCount.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblTableCount.Location = new System.Drawing.Point(20, 90);
            this.lblTableCount.Name = "lblTableCount";
            this.lblTableCount.Size = new System.Drawing.Size(56, 17);
            this.lblTableCount.TabIndex = 4;
            this.lblTableCount.Text = "表数量：";
            // 
            // lblTableCountValue
            // 
            this.lblTableCountValue.AutoSize = true;
            this.lblTableCountValue.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblTableCountValue.Location = new System.Drawing.Point(100, 90);
            this.lblTableCountValue.Name = "lblTableCountValue";
            this.lblTableCountValue.Size = new System.Drawing.Size(17, 17);
            this.lblTableCountValue.TabIndex = 5;
            this.lblTableCountValue.Text = "0";
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.Controls.Add(this.dgvDiagnosticInfo);
            this.groupBoxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDetails.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.groupBoxDetails.Location = new System.Drawing.Point(10, 250);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Padding = new System.Windows.Forms.Padding(10);
            this.groupBoxDetails.Size = new System.Drawing.Size(780, 300);
            this.groupBoxDetails.TabIndex = 2;
            this.groupBoxDetails.TabStop = false;
            this.groupBoxDetails.Text = "详细诊断信息";
            // 
            // dgvDiagnosticInfo
            // 
            this.dgvDiagnosticInfo.AllowUserToAddRows = false;
            this.dgvDiagnosticInfo.AllowUserToDeleteRows = false;
            this.dgvDiagnosticInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDiagnosticInfo.BackgroundColor = System.Drawing.Color.White;
            this.dgvDiagnosticInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiagnosticInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProperty,
            this.colValue,
            this.colStatus});
            this.dgvDiagnosticInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDiagnosticInfo.Location = new System.Drawing.Point(10, 26);
            this.dgvDiagnosticInfo.Name = "dgvDiagnosticInfo";
            this.dgvDiagnosticInfo.ReadOnly = true;
            this.dgvDiagnosticInfo.RowHeadersVisible = false;
            this.dgvDiagnosticInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDiagnosticInfo.Size = new System.Drawing.Size(760, 264);
            this.dgvDiagnosticInfo.TabIndex = 0;
            // 
            // colProperty
            // 
            this.colProperty.HeaderText = "检查项目";
            this.colProperty.Name = "colProperty";
            this.colProperty.ReadOnly = true;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "检查结果";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnClose);
            this.panelButtons.Controls.Add(this.btnExportReport);
            this.panelButtons.Controls.Add(this.btnTestConnection);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 550);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
            this.panelButtons.Size = new System.Drawing.Size(800, 60);
            this.panelButtons.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefresh.Location = new System.Drawing.Point(20, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "刷新诊断";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnTestConnection.Location = new System.Drawing.Point(140, 15);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(100, 30);
            this.btnTestConnection.TabIndex = 1;
            this.btnTestConnection.Text = "测试连接";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // btnExportReport
            // 
            this.btnExportReport.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnExportReport.Location = new System.Drawing.Point(260, 15);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(100, 30);
            this.btnExportReport.TabIndex = 2;
            this.btnExportReport.Text = "导出报告";
            this.btnExportReport.UseVisualStyleBackColor = true;
            this.btnExportReport.Click += new System.EventHandler(this.btnExportReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnClose.Location = new System.Drawing.Point(680, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 610);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel.Text = "就绪";
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 30000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // DatabaseDiagnosticForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 632);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseDiagnosticForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据库诊断工具";
            this.Load += new System.EventHandler(this.DatabaseDiagnosticForm_Load);
            this.panelMain.ResumeLayout(false);
            this.groupBoxConnection.ResumeLayout(false);
            this.groupBoxConnection.PerformLayout();
            this.groupBoxPerformance.ResumeLayout(false);
            this.groupBoxPerformance.PerformLayout();
            this.groupBoxDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiagnosticInfo)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupBoxConnection;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblConnectionStatusValue;
        private System.Windows.Forms.Label lblServerInfo;
        private System.Windows.Forms.Label lblServerInfoValue;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.Label lblDatabaseNameValue;
        private System.Windows.Forms.GroupBox groupBoxPerformance;
        private System.Windows.Forms.Label lblConnectionCount;
        private System.Windows.Forms.Label lblConnectionCountValue;
        private System.Windows.Forms.Label lblDatabaseSize;
        private System.Windows.Forms.Label lblDatabaseSizeValue;
        private System.Windows.Forms.Label lblTableCount;
        private System.Windows.Forms.Label lblTableCountValue;
        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.DataGridView dgvDiagnosticInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProperty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnExportReport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.Timer timerRefresh;
    }
}
