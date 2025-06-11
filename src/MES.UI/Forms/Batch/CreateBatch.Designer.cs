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
            this.Cancel = new System.Windows.Forms.Button();
            this.Create = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.status_box = new System.Windows.Forms.ComboBox();
            this.workorder_id_box = new System.Windows.Forms.ComboBox();
            this.user_text = new System.Windows.Forms.TextBox();
            this.Batchnum_text = new System.Windows.Forms.TextBox();
            this.Production_id_text = new System.Windows.Forms.TextBox();
            this.Batch_id_text = new System.Windows.Forms.TextBox();
            this.user = new System.Windows.Forms.Label();
            this.plan_time = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.Batchnum = new System.Windows.Forms.Label();
            this.workorder_id = new System.Windows.Forms.Label();
            this.Production_id = new System.Windows.Forms.Label();
            this.Batch_id = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.Font = new System.Drawing.Font("宋体", 12F);
            this.Cancel.Location = new System.Drawing.Point(439, 469);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(88, 50);
            this.Cancel.TabIndex = 32;
            this.Cancel.Text = "取消";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Create
            // 
            this.Create.Font = new System.Drawing.Font("宋体", 12F);
            this.Create.Location = new System.Drawing.Point(344, 469);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(88, 50);
            this.Create.TabIndex = 31;
            this.Create.Text = "创建";
            this.Create.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Font = new System.Drawing.Font("宋体", 12F);
            this.dateTimePicker.Location = new System.Drawing.Point(265, 330);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(262, 35);
            this.dateTimePicker.TabIndex = 30;
            // 
            // status_box
            // 
            this.status_box.Font = new System.Drawing.Font("宋体", 12F);
            this.status_box.FormattingEnabled = true;
            this.status_box.Location = new System.Drawing.Point(265, 267);
            this.status_box.Name = "status_box";
            this.status_box.Size = new System.Drawing.Size(262, 32);
            this.status_box.TabIndex = 29;
            // 
            // workorder_id_box
            // 
            this.workorder_id_box.Font = new System.Drawing.Font("宋体", 12F);
            this.workorder_id_box.FormattingEnabled = true;
            this.workorder_id_box.Location = new System.Drawing.Point(265, 150);
            this.workorder_id_box.Name = "workorder_id_box";
            this.workorder_id_box.Size = new System.Drawing.Size(262, 32);
            this.workorder_id_box.TabIndex = 28;
            // 
            // user_text
            // 
            this.user_text.Font = new System.Drawing.Font("宋体", 12F);
            this.user_text.Location = new System.Drawing.Point(265, 384);
            this.user_text.Name = "user_text";
            this.user_text.Size = new System.Drawing.Size(262, 35);
            this.user_text.TabIndex = 27;
            // 
            // Batchnum_text
            // 
            this.Batchnum_text.Font = new System.Drawing.Font("宋体", 12F);
            this.Batchnum_text.Location = new System.Drawing.Point(265, 207);
            this.Batchnum_text.Name = "Batchnum_text";
            this.Batchnum_text.Size = new System.Drawing.Size(262, 35);
            this.Batchnum_text.TabIndex = 26;
            // 
            // Production_id_text
            // 
            this.Production_id_text.Font = new System.Drawing.Font("宋体", 12F);
            this.Production_id_text.Location = new System.Drawing.Point(265, 87);
            this.Production_id_text.Name = "Production_id_text";
            this.Production_id_text.Size = new System.Drawing.Size(262, 35);
            this.Production_id_text.TabIndex = 25;
            // 
            // Batch_id_text
            // 
            this.Batch_id_text.Font = new System.Drawing.Font("宋体", 12F);
            this.Batch_id_text.Location = new System.Drawing.Point(265, 27);
            this.Batch_id_text.Name = "Batch_id_text";
            this.Batch_id_text.Size = new System.Drawing.Size(262, 35);
            this.Batch_id_text.TabIndex = 24;
            // 
            // user
            // 
            this.user.AutoSize = true;
            this.user.Font = new System.Drawing.Font("宋体", 12F);
            this.user.Location = new System.Drawing.Point(116, 390);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(82, 24);
            this.user.TabIndex = 23;
            this.user.Text = "负责人";
            // 
            // plan_time
            // 
            this.plan_time.AutoSize = true;
            this.plan_time.Font = new System.Drawing.Font("宋体", 12F);
            this.plan_time.Location = new System.Drawing.Point(44, 330);
            this.plan_time.Name = "plan_time";
            this.plan_time.Size = new System.Drawing.Size(154, 24);
            this.plan_time.TabIndex = 22;
            this.plan_time.Text = "计划完成时间";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Font = new System.Drawing.Font("宋体", 12F);
            this.status.Location = new System.Drawing.Point(140, 270);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(58, 24);
            this.status.TabIndex = 21;
            this.status.Text = "状态";
            // 
            // Batchnum
            // 
            this.Batchnum.AutoSize = true;
            this.Batchnum.Font = new System.Drawing.Font("宋体", 12F);
            this.Batchnum.Location = new System.Drawing.Point(92, 210);
            this.Batchnum.Name = "Batchnum";
            this.Batchnum.Size = new System.Drawing.Size(106, 24);
            this.Batchnum.TabIndex = 20;
            this.Batchnum.Text = "批次数量";
            // 
            // workorder_id
            // 
            this.workorder_id.AutoSize = true;
            this.workorder_id.Font = new System.Drawing.Font("宋体", 12F);
            this.workorder_id.Location = new System.Drawing.Point(116, 150);
            this.workorder_id.Name = "workorder_id";
            this.workorder_id.Size = new System.Drawing.Size(82, 24);
            this.workorder_id.TabIndex = 19;
            this.workorder_id.Text = "工单号";
            // 
            // Production_id
            // 
            this.Production_id.AutoSize = true;
            this.Production_id.Font = new System.Drawing.Font("宋体", 12F);
            this.Production_id.Location = new System.Drawing.Point(92, 90);
            this.Production_id.Name = "Production_id";
            this.Production_id.Size = new System.Drawing.Size(106, 24);
            this.Production_id.TabIndex = 18;
            this.Production_id.Text = "产品名称";
            // 
            // Batch_id
            // 
            this.Batch_id.AutoSize = true;
            this.Batch_id.Font = new System.Drawing.Font("宋体", 12F);
            this.Batch_id.Location = new System.Drawing.Point(116, 30);
            this.Batch_id.Name = "Batch_id";
            this.Batch_id.Size = new System.Drawing.Size(82, 24);
            this.Batch_id.TabIndex = 17;
            this.Batch_id.Text = "批次号";
            // 
            // CreateBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(601, 556);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Create);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.status_box);
            this.Controls.Add(this.workorder_id_box);
            this.Controls.Add(this.user_text);
            this.Controls.Add(this.Batchnum_text);
            this.Controls.Add(this.Production_id_text);
            this.Controls.Add(this.Batch_id_text);
            this.Controls.Add(this.user);
            this.Controls.Add(this.plan_time);
            this.Controls.Add(this.status);
            this.Controls.Add(this.Batchnum);
            this.Controls.Add(this.workorder_id);
            this.Controls.Add(this.Production_id);
            this.Controls.Add(this.Batch_id);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateBatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建生产批次";
            this.Load += new System.EventHandler(this.CreateBatch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.ComboBox status_box;
        private System.Windows.Forms.ComboBox workorder_id_box;
        private System.Windows.Forms.TextBox user_text;
        private System.Windows.Forms.TextBox Batchnum_text;
        private System.Windows.Forms.TextBox Production_id_text;
        private System.Windows.Forms.TextBox Batch_id_text;
        private System.Windows.Forms.Label user;
        private System.Windows.Forms.Label plan_time;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label Batchnum;
        private System.Windows.Forms.Label workorder_id;
        private System.Windows.Forms.Label Production_id;
        private System.Windows.Forms.Label Batch_id;
    }
}