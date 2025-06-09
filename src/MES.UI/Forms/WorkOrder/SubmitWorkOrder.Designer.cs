namespace MES.UI.Forms.WorkOrder
{
    partial class SubmitWorkOrder
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.txtSubmitRemark = new System.Windows.Forms.TextBox();
            this.lblWorkOrderCount = new System.Windows.Forms.Label();
            this.dgvWorkOrders = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblWorkOrderNo = new System.Windows.Forms.Label();
            this.lblWorkOrderType = new System.Windows.Forms.Label();
            this.lblProductCode = new System.Windows.Forms.Label();
            this.lblPlanQuantity = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.lblCreatedDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).BeginInit();
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
            // btnSubmit
            //
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnSubmit.Location = new System.Drawing.Point(630, 500);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 35);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "提交工单";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            //
            // txtSubmitRemark
            //
            this.txtSubmitRemark.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtSubmitRemark.Location = new System.Drawing.Point(30, 450);
            this.txtSubmitRemark.Multiline = true;
            this.txtSubmitRemark.Name = "txtSubmitRemark";
            this.txtSubmitRemark.Size = new System.Drawing.Size(820, 35);
            this.txtSubmitRemark.TabIndex = 3;
            //
            // lblWorkOrderCount
            //
            this.lblWorkOrderCount.AutoSize = true;
            this.lblWorkOrderCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblWorkOrderCount.Location = new System.Drawing.Point(30, 80);
            this.lblWorkOrderCount.Name = "lblWorkOrderCount";
            this.lblWorkOrderCount.Size = new System.Drawing.Size(120, 20);
            this.lblWorkOrderCount.TabIndex = 4;
            this.lblWorkOrderCount.Text = "工单数量统计";
            //
            // dgvWorkOrders
            //
            this.dgvWorkOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkOrders.Location = new System.Drawing.Point(30, 110);
            this.dgvWorkOrders.Name = "dgvWorkOrders";
            this.dgvWorkOrders.RowHeadersWidth = 62;
            this.dgvWorkOrders.RowTemplate.Height = 30;
            this.dgvWorkOrders.Size = new System.Drawing.Size(820, 250);
            this.dgvWorkOrders.TabIndex = 5;
            this.dgvWorkOrders.SelectionChanged += new System.EventHandler(this.dgvWorkOrders_SelectionChanged);
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
            // lblWorkOrderNo
            //
            this.lblWorkOrderNo.AutoSize = true;
            this.lblWorkOrderNo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblWorkOrderNo.Location = new System.Drawing.Point(30, 380);
            this.lblWorkOrderNo.Name = "lblWorkOrderNo";
            this.lblWorkOrderNo.Size = new System.Drawing.Size(80, 20);
            this.lblWorkOrderNo.TabIndex = 7;
            this.lblWorkOrderNo.Text = "工单号：";
            //
            // lblWorkOrderType
            //
            this.lblWorkOrderType.AutoSize = true;
            this.lblWorkOrderType.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblWorkOrderType.Location = new System.Drawing.Point(150, 380);
            this.lblWorkOrderType.Name = "lblWorkOrderType";
            this.lblWorkOrderType.Size = new System.Drawing.Size(100, 20);
            this.lblWorkOrderType.TabIndex = 8;
            this.lblWorkOrderType.Text = "工单类型：";
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
            // lblPlanQuantity
            //
            this.lblPlanQuantity.AutoSize = true;
            this.lblPlanQuantity.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblPlanQuantity.Location = new System.Drawing.Point(410, 380);
            this.lblPlanQuantity.Name = "lblPlanQuantity";
            this.lblPlanQuantity.Size = new System.Drawing.Size(100, 20);
            this.lblPlanQuantity.TabIndex = 10;
            this.lblPlanQuantity.Text = "计划数量：";
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
            // SubmitWorkOrder
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 560);
            this.Controls.Add(this.lblCreatedDate);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblPlanQuantity);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.lblWorkOrderType);
            this.Controls.Add(this.lblWorkOrderNo);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvWorkOrders);
            this.Controls.Add(this.lblWorkOrderCount);
            this.Controls.Add(this.txtSubmitRemark);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnClose);
            this.Name = "SubmitWorkOrder";
            this.Text = "提交工单";
            this.Load += new System.EventHandler(this.SubmitWorkOrder_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SubmitWorkOrder_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox txtSubmitRemark;
        private System.Windows.Forms.Label lblWorkOrderCount;
        private System.Windows.Forms.DataGridView dgvWorkOrders;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblWorkOrderNo;
        private System.Windows.Forms.Label lblWorkOrderType;
        private System.Windows.Forms.Label lblProductCode;
        private System.Windows.Forms.Label lblPlanQuantity;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.Label lblCreatedDate;
    }
}