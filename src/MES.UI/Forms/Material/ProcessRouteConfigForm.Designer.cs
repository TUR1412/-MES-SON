namespace MES.UI.Forms.Material
{
    partial class ProcessRouteConfigForm
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
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.dgvRoutes = new System.Windows.Forms.DataGridView();
            this.lblRouteList = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.dgvSteps = new System.Windows.Forms.DataGridView();
            this.panelStepButtons = new System.Windows.Forms.Panel();
            this.btnStepDown = new System.Windows.Forms.Button();
            this.btnStepUp = new System.Windows.Forms.Button();
            this.btnDeleteStep = new System.Windows.Forms.Button();
            this.btnEditStep = new System.Windows.Forms.Button();
            this.btnAddStep = new System.Windows.Forms.Button();
            this.lblStepList = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).BeginInit();
            this.panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).BeginInit();
            this.panelStepButtons.SuspendLayout();
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
            this.lblTitle.Text = "⚙️ 工艺路线配置";
            //
            // panelSearch
            //
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Controls.Add(this.lblSearch);
            this.panelSearch.Controls.Add(this.cmbProduct);
            this.panelSearch.Controls.Add(this.lblProduct);
            this.panelSearch.Controls.Add(this.cmbStatus);
            this.panelSearch.Controls.Add(this.lblStatus);
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
            this.btnSearch.Location = new System.Drawing.Point(850, 25);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = false;
            //
            // txtSearch
            //
            this.txtSearch.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtSearch.Location = new System.Drawing.Point(680, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(150, 23);
            this.txtSearch.TabIndex = 5;
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblSearch.Location = new System.Drawing.Point(620, 30);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 17);
            this.lblSearch.TabIndex = 4;
            this.lblSearch.Text = "关键字：";
            //
            // cmbProduct
            //
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(80, 27);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(150, 25);
            this.cmbProduct.TabIndex = 1;
            //
            // lblProduct
            //
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblProduct.Location = new System.Drawing.Point(20, 30);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(56, 17);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "产品：";
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
            // panelButtons
            //
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnCopy);
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
            // btnDelete
            //
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(250, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            //
            // btnEdit
            //
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(150, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = false;
            //
            // btnAdd
            //
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(50, 15);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = false;
            //
            // btnCopy
            //
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnCopy.ForeColor = System.Drawing.Color.White;
            this.btnCopy.Location = new System.Drawing.Point(350, 15);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(80, 30);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = false;
            //
            // splitContainer
            //
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 200);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Size = new System.Drawing.Size(1200, 400);
            this.splitContainer.SplitterDistance = 600;
            this.splitContainer.TabIndex = 3;
            //
            // splitContainer.Panel1
            //
            this.splitContainer.Panel1.Controls.Add(this.panelLeft);
            //
            // splitContainer.Panel2
            //
            this.splitContainer.Panel2.Controls.Add(this.panelRight);
            //
            // panelLeft
            //
            this.panelLeft.Controls.Add(this.dgvRoutes);
            this.panelLeft.Controls.Add(this.lblRouteList);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(600, 400);
            this.panelLeft.TabIndex = 0;
            //
            // dgvRoutes
            //
            this.dgvRoutes.AllowUserToAddRows = false;
            this.dgvRoutes.AllowUserToDeleteRows = false;
            this.dgvRoutes.BackgroundColor = System.Drawing.Color.White;
            this.dgvRoutes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRoutes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoutes.Location = new System.Drawing.Point(0, 30);
            this.dgvRoutes.Name = "dgvRoutes";
            this.dgvRoutes.ReadOnly = true;
            this.dgvRoutes.RowTemplate.Height = 23;
            this.dgvRoutes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoutes.Size = new System.Drawing.Size(600, 370);
            this.dgvRoutes.TabIndex = 1;
            //
            // lblRouteList
            //
            this.lblRouteList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.lblRouteList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRouteList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.lblRouteList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblRouteList.Location = new System.Drawing.Point(0, 0);
            this.lblRouteList.Name = "lblRouteList";
            this.lblRouteList.Size = new System.Drawing.Size(600, 30);
            this.lblRouteList.TabIndex = 0;
            this.lblRouteList.Text = "📋 工艺路线列表";
            this.lblRouteList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelRight
            //
            this.panelRight.Controls.Add(this.dgvSteps);
            this.panelRight.Controls.Add(this.panelStepButtons);
            this.panelRight.Controls.Add(this.lblStepList);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(596, 400);
            this.panelRight.TabIndex = 0;
            //
            // dgvSteps
            //
            this.dgvSteps.AllowUserToAddRows = false;
            this.dgvSteps.AllowUserToDeleteRows = false;
            this.dgvSteps.BackgroundColor = System.Drawing.Color.White;
            this.dgvSteps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSteps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSteps.Location = new System.Drawing.Point(0, 30);
            this.dgvSteps.Name = "dgvSteps";
            this.dgvSteps.ReadOnly = true;
            this.dgvSteps.RowTemplate.Height = 23;
            this.dgvSteps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSteps.Size = new System.Drawing.Size(596, 320);
            this.dgvSteps.TabIndex = 2;
            //
            // panelStepButtons
            //
            this.panelStepButtons.BackColor = System.Drawing.Color.White;
            this.panelStepButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStepButtons.Controls.Add(this.btnStepDown);
            this.panelStepButtons.Controls.Add(this.btnStepUp);
            this.panelStepButtons.Controls.Add(this.btnDeleteStep);
            this.panelStepButtons.Controls.Add(this.btnEditStep);
            this.panelStepButtons.Controls.Add(this.btnAddStep);
            this.panelStepButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStepButtons.Location = new System.Drawing.Point(0, 350);
            this.panelStepButtons.Name = "panelStepButtons";
            this.panelStepButtons.Size = new System.Drawing.Size(596, 50);
            this.panelStepButtons.TabIndex = 1;
            //
            // btnStepDown
            //
            this.btnStepDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnStepDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStepDown.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnStepDown.ForeColor = System.Drawing.Color.White;
            this.btnStepDown.Location = new System.Drawing.Point(420, 10);
            this.btnStepDown.Name = "btnStepDown";
            this.btnStepDown.Size = new System.Drawing.Size(70, 30);
            this.btnStepDown.TabIndex = 4;
            this.btnStepDown.Text = "下移";
            this.btnStepDown.UseVisualStyleBackColor = false;
            //
            // btnStepUp
            //
            this.btnStepUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnStepUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStepUp.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnStepUp.ForeColor = System.Drawing.Color.White;
            this.btnStepUp.Location = new System.Drawing.Point(340, 10);
            this.btnStepUp.Name = "btnStepUp";
            this.btnStepUp.Size = new System.Drawing.Size(70, 30);
            this.btnStepUp.TabIndex = 3;
            this.btnStepUp.Text = "上移";
            this.btnStepUp.UseVisualStyleBackColor = false;
            //
            // btnDeleteStep
            //
            this.btnDeleteStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeleteStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteStep.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnDeleteStep.ForeColor = System.Drawing.Color.White;
            this.btnDeleteStep.Location = new System.Drawing.Point(180, 10);
            this.btnDeleteStep.Name = "btnDeleteStep";
            this.btnDeleteStep.Size = new System.Drawing.Size(70, 30);
            this.btnDeleteStep.TabIndex = 2;
            this.btnDeleteStep.Text = "删除";
            this.btnDeleteStep.UseVisualStyleBackColor = false;
            //
            // btnEditStep
            //
            this.btnEditStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnEditStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditStep.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnEditStep.ForeColor = System.Drawing.Color.White;
            this.btnEditStep.Location = new System.Drawing.Point(100, 10);
            this.btnEditStep.Name = "btnEditStep";
            this.btnEditStep.Size = new System.Drawing.Size(70, 30);
            this.btnEditStep.TabIndex = 1;
            this.btnEditStep.Text = "编辑";
            this.btnEditStep.UseVisualStyleBackColor = false;
            //
            // btnAddStep
            //
            this.btnAddStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStep.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnAddStep.ForeColor = System.Drawing.Color.White;
            this.btnAddStep.Location = new System.Drawing.Point(20, 10);
            this.btnAddStep.Name = "btnAddStep";
            this.btnAddStep.Size = new System.Drawing.Size(70, 30);
            this.btnAddStep.TabIndex = 0;
            this.btnAddStep.Text = "新增";
            this.btnAddStep.UseVisualStyleBackColor = false;
            //
            // lblStepList
            //
            this.lblStepList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.lblStepList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStepList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.lblStepList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(80)))), ((int)(((byte)(87)))));
            this.lblStepList.Location = new System.Drawing.Point(0, 0);
            this.lblStepList.Name = "lblStepList";
            this.lblStepList.Size = new System.Drawing.Size(596, 30);
            this.lblStepList.TabIndex = 0;
            this.lblStepList.Text = "🔧 工艺步骤详情";
            this.lblStepList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // ProcessRouteConfigForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 640);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);
            this.Name = "ProcessRouteConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工艺路线配置";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutes)).EndInit();
            this.panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).EndInit();
            this.panelStepButtons.ResumeLayout(false);
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
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.DataGridView dgvRoutes;
        private System.Windows.Forms.Label lblRouteList;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.DataGridView dgvSteps;
        private System.Windows.Forms.Panel panelStepButtons;
        private System.Windows.Forms.Button btnStepDown;
        private System.Windows.Forms.Button btnStepUp;
        private System.Windows.Forms.Button btnDeleteStep;
        private System.Windows.Forms.Button btnEditStep;
        private System.Windows.Forms.Button btnAddStep;
        private System.Windows.Forms.Label lblStepList;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblTotal;
    }
}