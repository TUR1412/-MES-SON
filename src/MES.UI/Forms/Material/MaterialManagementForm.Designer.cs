namespace MES.UI.Forms.Material
{
    partial class MaterialManagementForm
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
            // 主要控件声明
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.dataGridViewMaterials = new System.Windows.Forms.DataGridView();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.groupBoxBasicInfo = new System.Windows.Forms.GroupBox();
            this.textBoxMaterialCode = new System.Windows.Forms.TextBox();
            this.labelMaterialCode = new System.Windows.Forms.Label();
            this.textBoxMaterialName = new System.Windows.Forms.TextBox();
            this.labelMaterialName = new System.Windows.Forms.Label();
            this.textBoxMaterialType = new System.Windows.Forms.TextBox();
            this.labelMaterialType = new System.Windows.Forms.Label();
            this.textBoxUnit = new System.Windows.Forms.TextBox();
            this.labelUnit = new System.Windows.Forms.Label();
            this.groupBoxAdvancedInfo = new System.Windows.Forms.GroupBox();
            this.textBoxSpecification = new System.Windows.Forms.TextBox();
            this.labelSpecification = new System.Windows.Forms.Label();
            this.textBoxSupplier = new System.Windows.Forms.TextBox();
            this.labelSupplier = new System.Windows.Forms.Label();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.labelPrice = new System.Windows.Forms.Label();
            this.textBoxRemark = new System.Windows.Forms.TextBox();
            this.labelRemark = new System.Windows.Forms.Label();

            // 开始布局
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterials)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelDetails.SuspendLayout();
            this.groupBoxBasicInfo.SuspendLayout();
            this.groupBoxAdvancedInfo.SuspendLayout();
            this.SuspendLayout();

            //
            // panelMain
            //
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.panelMain.Controls.Add(this.panelContent);
            this.panelMain.Controls.Add(this.panelButtons);
            this.panelMain.Controls.Add(this.panelSearch);
            this.panelMain.Controls.Add(this.panelHeader);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(1400, 800);
            this.panelMain.TabIndex = 0;

            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(20, 20);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelHeader.Size = new System.Drawing.Size(1360, 60);
            this.panelHeader.TabIndex = 0;

            //
            // labelTitle
            //
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.labelTitle.Location = new System.Drawing.Point(20, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(132, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "📦 物料管理";

            //
            // panelSearch
            //
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Controls.Add(this.textBoxSearch);
            this.panelSearch.Controls.Add(this.labelSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(20, 80);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelSearch.Size = new System.Drawing.Size(1360, 60);
            this.panelSearch.TabIndex = 1;

            //
            // labelSearch
            //
            this.labelSearch.AutoSize = true;
            this.labelSearch.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelSearch.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelSearch.Location = new System.Drawing.Point(20, 20);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(79, 20);
            this.labelSearch.TabIndex = 0;
            this.labelSearch.Text = "🔍 搜索：";

            //
            // textBoxSearch
            //
            this.textBoxSearch.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxSearch.Location = new System.Drawing.Point(105, 17);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(300, 25);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);

            //
            // panelButtons
            //
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(20, 140);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelButtons.Size = new System.Drawing.Size(1360, 70);
            this.panelButtons.TabIndex = 2;

            //
            // btnAdd
            //
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(20, 15);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 40);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "➕ 新增";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            //
            // btnEdit
            //
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(130, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 40);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "✏️ 编辑";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            //
            // btnDelete
            //
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(240, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "🗑️ 删除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(350, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 40);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            //
            // panelContent
            //
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.panelDetails);
            this.panelContent.Controls.Add(this.dataGridViewMaterials);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(20, 210);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(10);
            this.panelContent.Size = new System.Drawing.Size(1360, 570);
            this.panelContent.TabIndex = 3;

            //
            // dataGridViewMaterials
            //
            this.dataGridViewMaterials.AllowUserToAddRows = false;
            this.dataGridViewMaterials.AllowUserToDeleteRows = false;
            this.dataGridViewMaterials.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewMaterials.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewMaterials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMaterials.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewMaterials.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewMaterials.MultiSelect = false;
            this.dataGridViewMaterials.Name = "dataGridViewMaterials";
            this.dataGridViewMaterials.ReadOnly = true;
            this.dataGridViewMaterials.RowHeadersWidth = 62;
            this.dataGridViewMaterials.RowTemplate.Height = 35;
            this.dataGridViewMaterials.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMaterials.Size = new System.Drawing.Size(800, 550);
            this.dataGridViewMaterials.TabIndex = 0;
            this.dataGridViewMaterials.SelectionChanged += new System.EventHandler(this.dataGridViewMaterials_SelectionChanged);

            //
            // panelDetails
            //
            this.panelDetails.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);
            this.panelDetails.Controls.Add(this.groupBoxAdvancedInfo);
            this.panelDetails.Controls.Add(this.groupBoxBasicInfo);
            this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetails.Location = new System.Drawing.Point(810, 10);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Padding = new System.Windows.Forms.Padding(10);
            this.panelDetails.Size = new System.Drawing.Size(540, 550);
            this.panelDetails.TabIndex = 1;

            //
            // groupBoxBasicInfo
            //
            this.groupBoxBasicInfo.BackColor = System.Drawing.Color.White;
            this.groupBoxBasicInfo.Controls.Add(this.textBoxUnit);
            this.groupBoxBasicInfo.Controls.Add(this.labelUnit);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxMaterialType);
            this.groupBoxBasicInfo.Controls.Add(this.labelMaterialType);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxMaterialName);
            this.groupBoxBasicInfo.Controls.Add(this.labelMaterialName);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxMaterialCode);
            this.groupBoxBasicInfo.Controls.Add(this.labelMaterialCode);
            this.groupBoxBasicInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasicInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxBasicInfo.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.groupBoxBasicInfo.Location = new System.Drawing.Point(10, 10);
            this.groupBoxBasicInfo.Name = "groupBoxBasicInfo";
            this.groupBoxBasicInfo.Padding = new System.Windows.Forms.Padding(15);
            this.groupBoxBasicInfo.Size = new System.Drawing.Size(520, 250);
            this.groupBoxBasicInfo.TabIndex = 0;
            this.groupBoxBasicInfo.TabStop = false;
            this.groupBoxBasicInfo.Text = "📋 基本信息";

            //
            // labelMaterialCode
            //
            this.labelMaterialCode.AutoSize = true;
            this.labelMaterialCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelMaterialCode.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelMaterialCode.Location = new System.Drawing.Point(20, 40);
            this.labelMaterialCode.Name = "labelMaterialCode";
            this.labelMaterialCode.Size = new System.Drawing.Size(68, 17);
            this.labelMaterialCode.TabIndex = 0;
            this.labelMaterialCode.Text = "物料编码：";

            //
            // textBoxMaterialCode
            //
            this.textBoxMaterialCode.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxMaterialCode.Location = new System.Drawing.Point(120, 37);
            this.textBoxMaterialCode.Name = "textBoxMaterialCode";
            this.textBoxMaterialCode.ReadOnly = true;
            this.textBoxMaterialCode.Size = new System.Drawing.Size(350, 23);
            this.textBoxMaterialCode.TabIndex = 1;

            //
            // labelMaterialName
            //
            this.labelMaterialName.AutoSize = true;
            this.labelMaterialName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelMaterialName.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelMaterialName.Location = new System.Drawing.Point(20, 80);
            this.labelMaterialName.Name = "labelMaterialName";
            this.labelMaterialName.Size = new System.Drawing.Size(68, 17);
            this.labelMaterialName.TabIndex = 2;
            this.labelMaterialName.Text = "物料名称：";

            //
            // textBoxMaterialName
            //
            this.textBoxMaterialName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxMaterialName.Location = new System.Drawing.Point(120, 77);
            this.textBoxMaterialName.Name = "textBoxMaterialName";
            this.textBoxMaterialName.ReadOnly = true;
            this.textBoxMaterialName.Size = new System.Drawing.Size(350, 23);
            this.textBoxMaterialName.TabIndex = 3;

            //
            // labelMaterialType
            //
            this.labelMaterialType.AutoSize = true;
            this.labelMaterialType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelMaterialType.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelMaterialType.Location = new System.Drawing.Point(20, 120);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(68, 17);
            this.labelMaterialType.TabIndex = 4;
            this.labelMaterialType.Text = "物料类型：";

            //
            // textBoxMaterialType
            //
            this.textBoxMaterialType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxMaterialType.Location = new System.Drawing.Point(120, 117);
            this.textBoxMaterialType.Name = "textBoxMaterialType";
            this.textBoxMaterialType.ReadOnly = true;
            this.textBoxMaterialType.Size = new System.Drawing.Size(350, 23);
            this.textBoxMaterialType.TabIndex = 5;

            //
            // labelUnit
            //
            this.labelUnit.AutoSize = true;
            this.labelUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelUnit.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelUnit.Location = new System.Drawing.Point(20, 160);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(68, 17);
            this.labelUnit.TabIndex = 6;
            this.labelUnit.Text = "计量单位：";

            //
            // textBoxUnit
            //
            this.textBoxUnit.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxUnit.Location = new System.Drawing.Point(120, 157);
            this.textBoxUnit.Name = "textBoxUnit";
            this.textBoxUnit.ReadOnly = true;
            this.textBoxUnit.Size = new System.Drawing.Size(350, 23);
            this.textBoxUnit.TabIndex = 7;

            //
            // groupBoxAdvancedInfo
            //
            this.groupBoxAdvancedInfo.BackColor = System.Drawing.Color.White;
            this.groupBoxAdvancedInfo.Controls.Add(this.textBoxRemark);
            this.groupBoxAdvancedInfo.Controls.Add(this.labelRemark);
            this.groupBoxAdvancedInfo.Controls.Add(this.textBoxPrice);
            this.groupBoxAdvancedInfo.Controls.Add(this.labelPrice);
            this.groupBoxAdvancedInfo.Controls.Add(this.textBoxSupplier);
            this.groupBoxAdvancedInfo.Controls.Add(this.labelSupplier);
            this.groupBoxAdvancedInfo.Controls.Add(this.textBoxSpecification);
            this.groupBoxAdvancedInfo.Controls.Add(this.labelSpecification);
            this.groupBoxAdvancedInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAdvancedInfo.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxAdvancedInfo.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            this.groupBoxAdvancedInfo.Location = new System.Drawing.Point(10, 260);
            this.groupBoxAdvancedInfo.Name = "groupBoxAdvancedInfo";
            this.groupBoxAdvancedInfo.Padding = new System.Windows.Forms.Padding(15);
            this.groupBoxAdvancedInfo.Size = new System.Drawing.Size(520, 280);
            this.groupBoxAdvancedInfo.TabIndex = 1;
            this.groupBoxAdvancedInfo.TabStop = false;
            this.groupBoxAdvancedInfo.Text = "📊 详细信息";

            //
            // labelSpecification
            //
            this.labelSpecification.AutoSize = true;
            this.labelSpecification.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelSpecification.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelSpecification.Location = new System.Drawing.Point(20, 40);
            this.labelSpecification.Name = "labelSpecification";
            this.labelSpecification.Size = new System.Drawing.Size(68, 17);
            this.labelSpecification.TabIndex = 0;
            this.labelSpecification.Text = "规格型号：";

            //
            // textBoxSpecification
            //
            this.textBoxSpecification.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxSpecification.Location = new System.Drawing.Point(120, 37);
            this.textBoxSpecification.Name = "textBoxSpecification";
            this.textBoxSpecification.ReadOnly = true;
            this.textBoxSpecification.Size = new System.Drawing.Size(350, 23);
            this.textBoxSpecification.TabIndex = 1;

            //
            // labelSupplier
            //
            this.labelSupplier.AutoSize = true;
            this.labelSupplier.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelSupplier.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelSupplier.Location = new System.Drawing.Point(20, 80);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(56, 17);
            this.labelSupplier.TabIndex = 2;
            this.labelSupplier.Text = "供应商：";

            //
            // textBoxSupplier
            //
            this.textBoxSupplier.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxSupplier.Location = new System.Drawing.Point(120, 77);
            this.textBoxSupplier.Name = "textBoxSupplier";
            this.textBoxSupplier.ReadOnly = true;
            this.textBoxSupplier.Size = new System.Drawing.Size(350, 23);
            this.textBoxSupplier.TabIndex = 3;

            //
            // labelPrice
            //
            this.labelPrice.AutoSize = true;
            this.labelPrice.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelPrice.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelPrice.Location = new System.Drawing.Point(20, 120);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(68, 17);
            this.labelPrice.TabIndex = 4;
            this.labelPrice.Text = "参考价格：";

            //
            // textBoxPrice
            //
            this.textBoxPrice.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxPrice.Location = new System.Drawing.Point(120, 117);
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.ReadOnly = true;
            this.textBoxPrice.Size = new System.Drawing.Size(350, 23);
            this.textBoxPrice.TabIndex = 5;

            //
            // labelRemark
            //
            this.labelRemark.AutoSize = true;
            this.labelRemark.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelRemark.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.labelRemark.Location = new System.Drawing.Point(20, 160);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(44, 17);
            this.labelRemark.TabIndex = 6;
            this.labelRemark.Text = "备注：";

            //
            // textBoxRemark
            //
            this.textBoxRemark.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxRemark.Location = new System.Drawing.Point(120, 157);
            this.textBoxRemark.Multiline = true;
            this.textBoxRemark.Name = "textBoxRemark";
            this.textBoxRemark.ReadOnly = true;
            this.textBoxRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxRemark.Size = new System.Drawing.Size(350, 80);
            this.textBoxRemark.TabIndex = 7;

            //
            // MaterialManagementForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panelMain);
            this.Name = "MaterialManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "物料信息管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMaterials)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelDetails.ResumeLayout(false);
            this.groupBoxBasicInfo.ResumeLayout(false);
            this.groupBoxBasicInfo.PerformLayout();
            this.groupBoxAdvancedInfo.ResumeLayout(false);
            this.groupBoxAdvancedInfo.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.DataGridView dataGridViewMaterials;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.GroupBox groupBoxBasicInfo;
        private System.Windows.Forms.TextBox textBoxMaterialCode;
        private System.Windows.Forms.Label labelMaterialCode;
        private System.Windows.Forms.TextBox textBoxMaterialName;
        private System.Windows.Forms.Label labelMaterialName;
        private System.Windows.Forms.TextBox textBoxMaterialType;
        private System.Windows.Forms.Label labelMaterialType;
        private System.Windows.Forms.TextBox textBoxUnit;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.GroupBox groupBoxAdvancedInfo;
        private System.Windows.Forms.TextBox textBoxSpecification;
        private System.Windows.Forms.Label labelSpecification;
        private System.Windows.Forms.TextBox textBoxSupplier;
        private System.Windows.Forms.Label labelSupplier;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.TextBox textBoxRemark;
        private System.Windows.Forms.Label labelRemark;
    }
}