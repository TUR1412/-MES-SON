namespace MES.UI.Forms.Batch
{
    partial class CreateBatch
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
            // 主面板
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtWorkOrderNo = new System.Windows.Forms.TextBox();
            this.lblWorkOrderNo = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.dgvWorkOrders = new System.Windows.Forms.DataGridView();
            this.panelBatch = new System.Windows.Forms.Panel();
            this.grpBatchInfo = new System.Windows.Forms.GroupBox();
            this.lblBatchQuantity = new System.Windows.Forms.Label();
            this.txtBatchQuantity = new System.Windows.Forms.TextBox();
            this.lblBatchNo = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.lblResponsible = new System.Windows.Forms.Label();
            this.cmbResponsible = new System.Windows.Forms.ComboBox();
            this.lblPlanDate = new System.Windows.Forms.Label();
            this.dtpPlanDate = new System.Windows.Forms.DateTimePicker();
            this.lblRemarks = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).BeginInit();
            this.panelBatch.SuspendLayout();
            this.grpBatchInfo.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            //
            // panelTop - 顶部标题区域
            //
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1000, 60);
            this.panelTop.TabIndex = 0;

            //
            // lblTitle - 窗体标题
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(181, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "➕ 创建生产批次";

            //
            // panelSearch - 搜索区域
            //
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.txtWorkOrderNo);
            this.panelSearch.Controls.Add(this.lblWorkOrderNo);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 60);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1000, 60);
            this.panelSearch.TabIndex = 1;

            //
            // lblWorkOrderNo - 工单号标签
            //
            this.lblWorkOrderNo.AutoSize = true;
            this.lblWorkOrderNo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblWorkOrderNo.Location = new System.Drawing.Point(20, 20);
            this.lblWorkOrderNo.Name = "lblWorkOrderNo";
            this.lblWorkOrderNo.Size = new System.Drawing.Size(56, 17);
            this.lblWorkOrderNo.TabIndex = 0;
            this.lblWorkOrderNo.Text = "工单号：";

            //
            // txtWorkOrderNo - 工单号文本框
            //
            this.txtWorkOrderNo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtWorkOrderNo.Location = new System.Drawing.Point(80, 17);
            this.txtWorkOrderNo.Name = "txtWorkOrderNo";
            this.txtWorkOrderNo.Size = new System.Drawing.Size(200, 23);
            this.txtWorkOrderNo.TabIndex = 1;

            //
            // btnSearch - 搜索按钮
            //
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(300, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 27);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "🔍 搜索";
            this.btnSearch.UseVisualStyleBackColor = false;

            //
            // panelMain - 主内容区域
            //
            this.panelMain.Controls.Add(this.dgvWorkOrders);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 120);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(10);
            this.panelMain.Size = new System.Drawing.Size(1000, 300);
            this.panelMain.TabIndex = 2;

            //
            // dgvWorkOrders - 工单数据网格
            //
            this.dgvWorkOrders.AllowUserToAddRows = false;
            this.dgvWorkOrders.AllowUserToDeleteRows = false;
            this.dgvWorkOrders.BackgroundColor = System.Drawing.Color.White;
            this.dgvWorkOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvWorkOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWorkOrders.Location = new System.Drawing.Point(10, 10);
            this.dgvWorkOrders.MultiSelect = false;
            this.dgvWorkOrders.Name = "dgvWorkOrders";
            this.dgvWorkOrders.ReadOnly = true;
            this.dgvWorkOrders.RowTemplate.Height = 25;
            this.dgvWorkOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkOrders.Size = new System.Drawing.Size(980, 280);
            this.dgvWorkOrders.TabIndex = 0;

            //
            // panelBatch - 批次信息面板
            //
            this.panelBatch.BackColor = System.Drawing.Color.White;
            this.panelBatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBatch.Controls.Add(this.grpBatchInfo);
            this.panelBatch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBatch.Location = new System.Drawing.Point(0, 420);
            this.panelBatch.Name = "panelBatch";
            this.panelBatch.Size = new System.Drawing.Size(1000, 120);
            this.panelBatch.TabIndex = 3;

            //
            // grpBatchInfo - 批次信息组
            //
            this.grpBatchInfo.Controls.Add(this.lblBatchNo);
            this.grpBatchInfo.Controls.Add(this.txtBatchNo);
            this.grpBatchInfo.Controls.Add(this.lblBatchQuantity);
            this.grpBatchInfo.Controls.Add(this.txtBatchQuantity);
            this.grpBatchInfo.Controls.Add(this.lblResponsible);
            this.grpBatchInfo.Controls.Add(this.cmbResponsible);
            this.grpBatchInfo.Controls.Add(this.lblPlanDate);
            this.grpBatchInfo.Controls.Add(this.dtpPlanDate);
            this.grpBatchInfo.Controls.Add(this.lblRemarks);
            this.grpBatchInfo.Controls.Add(this.txtRemarks);
            this.grpBatchInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBatchInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpBatchInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.grpBatchInfo.Location = new System.Drawing.Point(0, 0);
            this.grpBatchInfo.Name = "grpBatchInfo";
            this.grpBatchInfo.Size = new System.Drawing.Size(998, 118);
            this.grpBatchInfo.TabIndex = 0;
            this.grpBatchInfo.TabStop = false;
            this.grpBatchInfo.Text = "📋 批次信息";

            //
            // lblBatchNo - 批次号标签
            //
            this.lblBatchNo.AutoSize = true;
            this.lblBatchNo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblBatchNo.ForeColor = System.Drawing.Color.Black;
            this.lblBatchNo.Location = new System.Drawing.Point(20, 30);
            this.lblBatchNo.Name = "lblBatchNo";
            this.lblBatchNo.Size = new System.Drawing.Size(56, 17);
            this.lblBatchNo.TabIndex = 0;
            this.lblBatchNo.Text = "批次号：";

            //
            // txtBatchNo - 批次号文本框
            //
            this.txtBatchNo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBatchNo.Location = new System.Drawing.Point(80, 27);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ReadOnly = true;
            this.txtBatchNo.Size = new System.Drawing.Size(150, 23);
            this.txtBatchNo.TabIndex = 1;

            //
            // lblBatchQuantity - 批次数量标签
            //
            this.lblBatchQuantity.AutoSize = true;
            this.lblBatchQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblBatchQuantity.ForeColor = System.Drawing.Color.Black;
            this.lblBatchQuantity.Location = new System.Drawing.Point(250, 30);
            this.lblBatchQuantity.Name = "lblBatchQuantity";
            this.lblBatchQuantity.Size = new System.Drawing.Size(68, 17);
            this.lblBatchQuantity.TabIndex = 2;
            this.lblBatchQuantity.Text = "批次数量：";

            //
            // txtBatchQuantity - 批次数量文本框
            //
            this.txtBatchQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtBatchQuantity.Location = new System.Drawing.Point(320, 27);
            this.txtBatchQuantity.Name = "txtBatchQuantity";
            this.txtBatchQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtBatchQuantity.TabIndex = 3;

            //
            // lblResponsible - 负责人标签
            //
            this.lblResponsible.AutoSize = true;
            this.lblResponsible.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblResponsible.ForeColor = System.Drawing.Color.Black;
            this.lblResponsible.Location = new System.Drawing.Point(440, 30);
            this.lblResponsible.Name = "lblResponsible";
            this.lblResponsible.Size = new System.Drawing.Size(56, 17);
            this.lblResponsible.TabIndex = 4;
            this.lblResponsible.Text = "负责人：";

            //
            // cmbResponsible - 负责人下拉框
            //
            this.cmbResponsible.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResponsible.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbResponsible.FormattingEnabled = true;
            this.cmbResponsible.Items.AddRange(new object[] {
            "张三",
            "李四",
            "王五",
            "赵六"});
            this.cmbResponsible.Location = new System.Drawing.Point(500, 27);
            this.cmbResponsible.Name = "cmbResponsible";
            this.cmbResponsible.Size = new System.Drawing.Size(120, 25);
            this.cmbResponsible.TabIndex = 5;

            //
            // lblPlanDate - 计划完成日期标签
            //
            this.lblPlanDate.AutoSize = true;
            this.lblPlanDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblPlanDate.ForeColor = System.Drawing.Color.Black;
            this.lblPlanDate.Location = new System.Drawing.Point(640, 30);
            this.lblPlanDate.Name = "lblPlanDate";
            this.lblPlanDate.Size = new System.Drawing.Size(92, 17);
            this.lblPlanDate.TabIndex = 6;
            this.lblPlanDate.Text = "计划完成日期：";

            //
            // dtpPlanDate - 计划完成日期选择器
            //
            this.dtpPlanDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dtpPlanDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPlanDate.Location = new System.Drawing.Point(740, 27);
            this.dtpPlanDate.Name = "dtpPlanDate";
            this.dtpPlanDate.Size = new System.Drawing.Size(120, 23);
            this.dtpPlanDate.TabIndex = 7;

            //
            // lblRemarks - 备注标签
            //
            this.lblRemarks.AutoSize = true;
            this.lblRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblRemarks.ForeColor = System.Drawing.Color.Black;
            this.lblRemarks.Location = new System.Drawing.Point(20, 65);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Size = new System.Drawing.Size(44, 17);
            this.lblRemarks.TabIndex = 8;
            this.lblRemarks.Text = "备注：";

            //
            // txtRemarks - 备注文本框
            //
            this.txtRemarks.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtRemarks.Location = new System.Drawing.Point(80, 62);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(780, 40);
            this.txtRemarks.TabIndex = 9;

            //
            // panelButtons - 按钮面板
            //
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelButtons.Controls.Add(this.btnCreate);
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 540);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1000, 60);
            this.panelButtons.TabIndex = 4;

            //
            // btnCreate - 创建按钮
            //
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.Location = new System.Drawing.Point(750, 15);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(100, 30);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "✅ 创建批次";
            this.btnCreate.UseVisualStyleBackColor = false;

            //
            // btnCancel - 取消按钮
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(870, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "❌ 取消";
            this.btnCancel.UseVisualStyleBackColor = false;

            //
            // CreateBatch - 创建批次窗体
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelBatch);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateBatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建生产批次";

            // 布局恢复
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).EndInit();
            this.panelBatch.ResumeLayout(false);
            this.grpBatchInfo.ResumeLayout(false);
            this.grpBatchInfo.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // 主面板控件
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblWorkOrderNo;
        private System.Windows.Forms.TextBox txtWorkOrderNo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.DataGridView dgvWorkOrders;
        private System.Windows.Forms.Panel panelBatch;
        private System.Windows.Forms.GroupBox grpBatchInfo;
        private System.Windows.Forms.Label lblBatchNo;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Label lblBatchQuantity;
        private System.Windows.Forms.TextBox txtBatchQuantity;
        private System.Windows.Forms.Label lblResponsible;
        private System.Windows.Forms.ComboBox cmbResponsible;
        private System.Windows.Forms.Label lblPlanDate;
        private System.Windows.Forms.DateTimePicker dtpPlanDate;
        private System.Windows.Forms.Label lblRemarks;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
    }
}