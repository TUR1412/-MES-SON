namespace MES.UI.Forms.Batch
{
    partial class CancelBatch
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCancelReason = new System.Windows.Forms.TextBox();
            this.lblBatchCount = new System.Windows.Forms.Label();
            this.dgvBatches = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblBatchNo = new System.Windows.Forms.Label();
            this.lblWorkOrderNo = new System.Windows.Forms.Label();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.lblBatchQuantity = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblCreatedDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatches)).BeginInit();
            this.SuspendLayout();
            //
            // btnClose
            //
            this.btnClose.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnClose.Location = new System.Drawing.Point(750, 500);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // btnCancel
            //
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnCancel.Location = new System.Drawing.Point(630, 500);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消批次";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // txtCancelReason
            //
            this.txtCancelReason.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtCancelReason.Location = new System.Drawing.Point(30, 450);
            this.txtCancelReason.Multiline = true;
            this.txtCancelReason.Name = "txtCancelReason";
            this.txtCancelReason.Size = new System.Drawing.Size(820, 35);
            this.txtCancelReason.TabIndex = 3;
            //
            // lblBatchCount
            //
            this.lblBatchCount.AutoSize = true;
            this.lblBatchCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblBatchCount.Location = new System.Drawing.Point(30, 80);
            this.lblBatchCount.Name = "lblBatchCount";
            this.lblBatchCount.Size = new System.Drawing.Size(120, 20);
            this.lblBatchCount.TabIndex = 4;
            this.lblBatchCount.Text = "批次数量统计";
            //
            // dgvBatches
            //
            this.dgvBatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBatches.Location = new System.Drawing.Point(30, 110);
            this.dgvBatches.Name = "dgvBatches";
            this.dgvBatches.RowHeadersWidth = 62;
            this.dgvBatches.RowTemplate.Height = 30;
            this.dgvBatches.Size = new System.Drawing.Size(820, 250);
            this.dgvBatches.TabIndex = 5;
            this.dgvBatches.SelectionChanged += new System.EventHandler(this.dgvBatches_SelectionChanged);
            //
            // btnRefresh
            //
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnRefresh.Location = new System.Drawing.Point(750, 75);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // lblBatchNo
            //
            this.lblBatchNo.AutoSize = true;
            this.lblBatchNo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblBatchNo.Location = new System.Drawing.Point(30, 380);
            this.lblBatchNo.Name = "lblBatchNo";
            this.lblBatchNo.Size = new System.Drawing.Size(80, 20);
            this.lblBatchNo.TabIndex = 7;
            this.lblBatchNo.Text = "批次号：";
            //
            // lblWorkOrderNo
            //
            this.lblWorkOrderNo.AutoSize = true;
            this.lblWorkOrderNo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblWorkOrderNo.Location = new System.Drawing.Point(150, 380);
            this.lblWorkOrderNo.Name = "lblWorkOrderNo";
            this.lblWorkOrderNo.Size = new System.Drawing.Size(80, 20);
            this.lblWorkOrderNo.TabIndex = 8;
            this.lblWorkOrderNo.Text = "工单号：";
            //
            // lblProductCode
            //
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblProductCode.Location = new System.Drawing.Point(280, 380);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(100, 20);
            this.lblProductCode.TabIndex = 9;
            this.lblProductCode.Text = "产品编号：";
            //
            // lblBatchQuantity
            //
            this.lblBatchQuantity.AutoSize = true;
            this.lblBatchQuantity.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblBatchQuantity.Location = new System.Drawing.Point(410, 380);
            this.lblBatchQuantity.Name = "lblBatchQuantity";
            this.lblBatchQuantity.Size = new System.Drawing.Size(100, 20);
            this.lblBatchQuantity.TabIndex = 10;
            this.lblBatchQuantity.Text = "批次数量：";
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(540, 380);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 20);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "状态：";
            //
            // lblCreatedBy
            //
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblCreatedBy.Location = new System.Drawing.Point(30, 410);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(80, 20);
            this.lblCreatedBy.TabIndex = 12;
            this.lblCreatedBy.Text = "创建人：";
            //
            // lblCreatedDate
            //
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblCreatedDate.Location = new System.Drawing.Point(150, 410);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new System.Drawing.Size(100, 20);
            this.lblCreatedDate.TabIndex = 13;
            this.lblCreatedDate.Text = "创建时间：";
            //
            // CancelBatch
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 560);
            this.Controls.Add(this.lblCreatedDate);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblBatchQuantity);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.lblWorkOrderNo);
            this.Controls.Add(this.lblBatchNo);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvBatches);
            this.Controls.Add(this.lblBatchCount);
            this.Controls.Add(this.txtCancelReason);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClose);
            this.Name = "CancelBatch";
            this.Text = "取消批次";
            this.Load += new System.EventHandler(this.CancelBatch_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CancelBatch_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatches)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtCancelReason;
        private System.Windows.Forms.Label lblBatchCount;
        private System.Windows.Forms.DataGridView dgvBatches;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblBatchNo;
        private System.Windows.Forms.Label lblWorkOrderNo;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Label lblBatchQuantity;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblCreatedDate;
    }
}