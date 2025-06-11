namespace MES.UI.Forms.WorkOrder
{
    partial class CancelWorkOrder
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnClose.Location = new System.Drawing.Point(812, 562);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 39);
            this.btnClose.TabIndex = 139;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnCancel.Location = new System.Drawing.Point(700, 562);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 39);
            this.btnCancel.TabIndex = 138;
            this.btnCancel.Text = "取消工单";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtCancelReason
            // 
            this.txtCancelReason.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtCancelReason.Location = new System.Drawing.Point(112, 506);
            this.txtCancelReason.Multiline = true;
            this.txtCancelReason.Name = "txtCancelReason";
            this.txtCancelReason.Size = new System.Drawing.Size(805, 44);
            this.txtCancelReason.TabIndex = 137;
            // 
            // lblWorkOrderCount
            // 
            this.lblWorkOrderCount.AutoSize = true;
            this.lblWorkOrderCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblWorkOrderCount.Location = new System.Drawing.Point(22, 472);
            this.lblWorkOrderCount.Name = "lblWorkOrderCount";
            this.lblWorkOrderCount.Size = new System.Drawing.Size(103, 24);
            this.lblWorkOrderCount.TabIndex = 135;
            this.lblWorkOrderCount.Text = "共 0 条工单";
            // 
            // dgvWorkOrders
            // 
            this.dgvWorkOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkOrders.Location = new System.Drawing.Point(22, 56);
            this.dgvWorkOrders.Name = "dgvWorkOrders";
            this.dgvWorkOrders.RowHeadersWidth = 62;
            this.dgvWorkOrders.RowTemplate.Height = 30;
            this.dgvWorkOrders.Size = new System.Drawing.Size(562, 338);
            this.dgvWorkOrders.TabIndex = 133;
            this.dgvWorkOrders.SelectionChanged += new System.EventHandler(this.dgvWorkOrders_SelectionChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.btnRefresh.Location = new System.Drawing.Point(587, 562);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(106, 39);
            this.btnRefresh.TabIndex = 131;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblWorkOrderNo
            // 
            this.lblWorkOrderNo.AutoSize = true;
            this.lblWorkOrderNo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblWorkOrderNo.Location = new System.Drawing.Point(698, 79);
            this.lblWorkOrderNo.Name = "lblWorkOrderNo";
            this.lblWorkOrderNo.Size = new System.Drawing.Size(0, 24);
            this.lblWorkOrderNo.TabIndex = 130;
            // 
            // lblWorkOrderType
            // 
            this.lblWorkOrderType.AutoSize = true;
            this.lblWorkOrderType.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblWorkOrderType.Location = new System.Drawing.Point(698, 112);
            this.lblWorkOrderType.Name = "lblWorkOrderType";
            this.lblWorkOrderType.Size = new System.Drawing.Size(0, 24);
            this.lblWorkOrderType.TabIndex = 129;
            // 
            // lblProductCode
            // 
            this.lblProductCode.AutoSize = true;
            this.lblProductCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblProductCode.Location = new System.Drawing.Point(698, 146);
            this.lblProductCode.Name = "lblProductCode";
            this.lblProductCode.Size = new System.Drawing.Size(0, 24);
            this.lblProductCode.TabIndex = 128;
            // 
            // lblPlanQuantity
            // 
            this.lblPlanQuantity.AutoSize = true;
            this.lblPlanQuantity.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblPlanQuantity.Location = new System.Drawing.Point(698, 180);
            this.lblPlanQuantity.Name = "lblPlanQuantity";
            this.lblPlanQuantity.Size = new System.Drawing.Size(0, 24);
            this.lblPlanQuantity.TabIndex = 127;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(698, 214);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 24);
            this.lblStatus.TabIndex = 126;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblCreatedBy.Location = new System.Drawing.Point(698, 248);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(0, 24);
            this.lblCreatedBy.TabIndex = 125;
            // 
            // lblCreatedDate
            // 
            this.lblCreatedDate.AutoSize = true;
            this.lblCreatedDate.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.lblCreatedDate.Location = new System.Drawing.Point(698, 281);
            this.lblCreatedDate.Name = "lblCreatedDate";
            this.lblCreatedDate.Size = new System.Drawing.Size(0, 24);
            this.lblCreatedDate.TabIndex = 124;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label1.Location = new System.Drawing.Point(608, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 180;
            this.label1.Text = "工单号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label2.Location = new System.Drawing.Point(608, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 24);
            this.label2.TabIndex = 181;
            this.label2.Text = "工单类型：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label3.Location = new System.Drawing.Point(608, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 24);
            this.label3.TabIndex = 182;
            this.label3.Text = "产品编号：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label4.Location = new System.Drawing.Point(608, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 24);
            this.label4.TabIndex = 183;
            this.label4.Text = "计划数量：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label5.Location = new System.Drawing.Point(608, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 24);
            this.label5.TabIndex = 184;
            this.label5.Text = "状态：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label6.Location = new System.Drawing.Point(608, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 24);
            this.label6.TabIndex = 185;
            this.label6.Text = "创建人：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label7.Location = new System.Drawing.Point(608, 281);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 24);
            this.label7.TabIndex = 186;
            this.label7.Text = "创建时间：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.label8.Location = new System.Drawing.Point(22, 518);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 24);
            this.label8.TabIndex = 187;
            this.label8.Text = "取消原因：";
            // 
            // CancelWorkOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 619);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCreatedDate);
            this.Controls.Add(this.lblCreatedBy);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblPlanQuantity);
            this.Controls.Add(this.lblProductCode);
            this.Controls.Add(this.lblWorkOrderType);
            this.Controls.Add(this.lblWorkOrderNo);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblWorkOrderCount);
            this.Controls.Add(this.dgvWorkOrders);
            this.Controls.Add(this.txtCancelReason);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClose);
            this.Name = "CancelWorkOrder";
            this.Text = "取消工单";
            this.Load += new System.EventHandler(this.CancelWorkOrder_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtCancelReason;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}