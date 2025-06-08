namespace MES.UI.Forms.Production
{
    partial class ProductionExecutionControlForm
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.panelDetailsContent = new System.Windows.Forms.Panel();
            this.groupBoxTimeInfo = new System.Windows.Forms.GroupBox();
            this.labelCreateTime = new System.Windows.Forms.Label();
            this.labelEndTime = new System.Windows.Forms.Label();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.groupBoxProgressInfo = new System.Windows.Forms.GroupBox();
            this.labelProgressPercent = new System.Windows.Forms.Label();
            this.progressBarExecution = new System.Windows.Forms.ProgressBar();
            this.textBoxCompletedQuantity = new System.Windows.Forms.TextBox();
            this.labelCompletedQuantity = new System.Windows.Forms.Label();
            this.textBoxPlannedQuantity = new System.Windows.Forms.TextBox();
            this.labelPlannedQuantity = new System.Windows.Forms.Label();
            this.groupBoxExecutionInfo = new System.Windows.Forms.GroupBox();
            this.textBoxOperator = new System.Windows.Forms.TextBox();
            this.labelOperator = new System.Windows.Forms.Label();
            this.textBoxWorkshop = new System.Windows.Forms.TextBox();
            this.labelWorkshop = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBoxBasicInfo = new System.Windows.Forms.GroupBox();
            this.textBoxProductName = new System.Windows.Forms.TextBox();
            this.labelProductName = new System.Windows.Forms.Label();
            this.textBoxOrderNumber = new System.Windows.Forms.TextBox();
            this.labelOrderNumber = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.dataGridViewExecution = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            this.panelDetailsContent.SuspendLayout();
            this.groupBoxTimeInfo.SuspendLayout();
            this.groupBoxProgressInfo.SuspendLayout();
            this.groupBoxExecutionInfo.SuspendLayout();
            this.groupBoxBasicInfo.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExecution)).BeginInit();
            this.SuspendLayout();
            //
            // panelTop
            //
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(58)))), ((int)(((byte)(64)))));
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1400, 60);
            this.panelTop.TabIndex = 0;
            //
            // labelTitle
            //
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(20, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(239, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "🎯 生产执行控制";
            //
            // panelSearch
            //
            this.panelSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelSearch.Controls.Add(this.textBoxSearch);
            this.panelSearch.Controls.Add(this.labelSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 60);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1400, 50);
            this.panelSearch.TabIndex = 1;
            //
            // textBoxSearch
            //
            this.textBoxSearch.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxSearch.Location = new System.Drawing.Point(100, 12);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(300, 25);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            //
            // labelSearch
            //
            this.labelSearch.AutoSize = true;
            this.labelSearch.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelSearch.Location = new System.Drawing.Point(20, 15);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(79, 20);
            this.labelSearch.TabIndex = 0;
            this.labelSearch.Text = "🔍 搜索：";
            //
            // panelButtons
            //
            this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Controls.Add(this.btnComplete);
            this.panelButtons.Controls.Add(this.btnPause);
            this.panelButtons.Controls.Add(this.btnStart);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 110);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1400, 60);
            this.panelButtons.TabIndex = 2;
            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(350, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "🔄 刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnComplete
            //
            this.btnComplete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnComplete.FlatAppearance.BorderSize = 0;
            this.btnComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComplete.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnComplete.ForeColor = System.Drawing.Color.White;
            this.btnComplete.Location = new System.Drawing.Point(240, 15);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(100, 30);
            this.btnComplete.TabIndex = 2;
            this.btnComplete.Text = "✅ 完成";
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            //
            // btnPause
            //
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnPause.ForeColor = System.Drawing.Color.Black;
            this.btnPause.Location = new System.Drawing.Point(130, 15);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(100, 30);
            this.btnPause.TabIndex = 1;
            this.btnPause.Text = "⏸️ 暂停";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            //
            // btnStart
            //
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(20, 15);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "▶️ 开始";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            //
            // panelMain
            //
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.splitter1);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 170);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1400, 630);
            this.panelMain.TabIndex = 3;
            //
            // panelRight
            //
            this.panelRight.Controls.Add(this.groupBoxDetails);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(803, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(10);
            this.panelRight.Size = new System.Drawing.Size(597, 630);
            this.panelRight.TabIndex = 2;
            //
            // groupBoxDetails
            //
            this.groupBoxDetails.Controls.Add(this.panelDetailsContent);
            this.groupBoxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDetails.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxDetails.Location = new System.Drawing.Point(10, 10);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(577, 610);
            this.groupBoxDetails.TabIndex = 0;
            this.groupBoxDetails.TabStop = false;
            this.groupBoxDetails.Text = "📋 执行详细信息";
            //
            // panelDetailsContent
            //
            this.panelDetailsContent.AutoScroll = true;
            this.panelDetailsContent.Controls.Add(this.groupBoxTimeInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxProgressInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxExecutionInfo);
            this.panelDetailsContent.Controls.Add(this.groupBoxBasicInfo);
            this.panelDetailsContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailsContent.Location = new System.Drawing.Point(3, 21);
            this.panelDetailsContent.Name = "panelDetailsContent";
            this.panelDetailsContent.Padding = new System.Windows.Forms.Padding(10);
            this.panelDetailsContent.Size = new System.Drawing.Size(571, 586);
            this.panelDetailsContent.TabIndex = 0;
            //
            // groupBoxTimeInfo
            //
            this.groupBoxTimeInfo.Controls.Add(this.labelCreateTime);
            this.groupBoxTimeInfo.Controls.Add(this.labelEndTime);
            this.groupBoxTimeInfo.Controls.Add(this.labelStartTime);
            this.groupBoxTimeInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTimeInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxTimeInfo.Location = new System.Drawing.Point(10, 370);
            this.groupBoxTimeInfo.Name = "groupBoxTimeInfo";
            this.groupBoxTimeInfo.Size = new System.Drawing.Size(551, 100);
            this.groupBoxTimeInfo.TabIndex = 3;
            this.groupBoxTimeInfo.TabStop = false;
            this.groupBoxTimeInfo.Text = "⏰ 时间信息";
            //
            // labelCreateTime
            //
            this.labelCreateTime.AutoSize = true;
            this.labelCreateTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelCreateTime.Location = new System.Drawing.Point(20, 70);
            this.labelCreateTime.Name = "labelCreateTime";
            this.labelCreateTime.Size = new System.Drawing.Size(68, 17);
            this.labelCreateTime.TabIndex = 2;
            this.labelCreateTime.Text = "创建时间：";
            //
            // labelEndTime
            //
            this.labelEndTime.AutoSize = true;
            this.labelEndTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelEndTime.Location = new System.Drawing.Point(20, 45);
            this.labelEndTime.Name = "labelEndTime";
            this.labelEndTime.Size = new System.Drawing.Size(68, 17);
            this.labelEndTime.TabIndex = 1;
            this.labelEndTime.Text = "结束时间：";
            //
            // labelStartTime
            //
            this.labelStartTime.AutoSize = true;
            this.labelStartTime.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelStartTime.Location = new System.Drawing.Point(20, 20);
            this.labelStartTime.Name = "labelStartTime";
            this.labelStartTime.Size = new System.Drawing.Size(68, 17);
            this.labelStartTime.TabIndex = 0;
            this.labelStartTime.Text = "开始时间：";
            //
            // groupBoxProgressInfo
            //
            this.groupBoxProgressInfo.Controls.Add(this.labelProgressPercent);
            this.groupBoxProgressInfo.Controls.Add(this.progressBarExecution);
            this.groupBoxProgressInfo.Controls.Add(this.textBoxCompletedQuantity);
            this.groupBoxProgressInfo.Controls.Add(this.labelCompletedQuantity);
            this.groupBoxProgressInfo.Controls.Add(this.textBoxPlannedQuantity);
            this.groupBoxProgressInfo.Controls.Add(this.labelPlannedQuantity);
            this.groupBoxProgressInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProgressInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxProgressInfo.Location = new System.Drawing.Point(10, 250);
            this.groupBoxProgressInfo.Name = "groupBoxProgressInfo";
            this.groupBoxProgressInfo.Size = new System.Drawing.Size(551, 120);
            this.groupBoxProgressInfo.TabIndex = 2;
            this.groupBoxProgressInfo.TabStop = false;
            this.groupBoxProgressInfo.Text = "📊 进度信息";
            //
            // labelProgressPercent
            //
            this.labelProgressPercent.AutoSize = true;
            this.labelProgressPercent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.labelProgressPercent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.labelProgressPercent.Location = new System.Drawing.Point(450, 90);
            this.labelProgressPercent.Name = "labelProgressPercent";
            this.labelProgressPercent.Size = new System.Drawing.Size(25, 17);
            this.labelProgressPercent.TabIndex = 5;
            this.labelProgressPercent.Text = "0%";
            //
            // progressBarExecution
            //
            this.progressBarExecution.Location = new System.Drawing.Point(100, 85);
            this.progressBarExecution.Name = "progressBarExecution";
            this.progressBarExecution.Size = new System.Drawing.Size(340, 25);
            this.progressBarExecution.TabIndex = 4;
            //
            // textBoxCompletedQuantity
            //
            this.textBoxCompletedQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxCompletedQuantity.Location = new System.Drawing.Point(100, 55);
            this.textBoxCompletedQuantity.Name = "textBoxCompletedQuantity";
            this.textBoxCompletedQuantity.ReadOnly = true;
            this.textBoxCompletedQuantity.Size = new System.Drawing.Size(150, 23);
            this.textBoxCompletedQuantity.TabIndex = 3;
            //
            // labelCompletedQuantity
            //
            this.labelCompletedQuantity.AutoSize = true;
            this.labelCompletedQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelCompletedQuantity.Location = new System.Drawing.Point(20, 58);
            this.labelCompletedQuantity.Name = "labelCompletedQuantity";
            this.labelCompletedQuantity.Size = new System.Drawing.Size(68, 17);
            this.labelCompletedQuantity.TabIndex = 2;
            this.labelCompletedQuantity.Text = "完成数量：";
            //
            // textBoxPlannedQuantity
            //
            this.textBoxPlannedQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxPlannedQuantity.Location = new System.Drawing.Point(100, 25);
            this.textBoxPlannedQuantity.Name = "textBoxPlannedQuantity";
            this.textBoxPlannedQuantity.ReadOnly = true;
            this.textBoxPlannedQuantity.Size = new System.Drawing.Size(150, 23);
            this.textBoxPlannedQuantity.TabIndex = 1;
            //
            // labelPlannedQuantity
            //
            this.labelPlannedQuantity.AutoSize = true;
            this.labelPlannedQuantity.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelPlannedQuantity.Location = new System.Drawing.Point(20, 28);
            this.labelPlannedQuantity.Name = "labelPlannedQuantity";
            this.labelPlannedQuantity.Size = new System.Drawing.Size(68, 17);
            this.labelPlannedQuantity.TabIndex = 0;
            this.labelPlannedQuantity.Text = "计划数量：";
            //
            // groupBoxExecutionInfo
            //
            this.groupBoxExecutionInfo.Controls.Add(this.textBoxOperator);
            this.groupBoxExecutionInfo.Controls.Add(this.labelOperator);
            this.groupBoxExecutionInfo.Controls.Add(this.textBoxWorkshop);
            this.groupBoxExecutionInfo.Controls.Add(this.labelWorkshop);
            this.groupBoxExecutionInfo.Controls.Add(this.textBoxStatus);
            this.groupBoxExecutionInfo.Controls.Add(this.labelStatus);
            this.groupBoxExecutionInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxExecutionInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxExecutionInfo.Location = new System.Drawing.Point(10, 130);
            this.groupBoxExecutionInfo.Name = "groupBoxExecutionInfo";
            this.groupBoxExecutionInfo.Size = new System.Drawing.Size(551, 120);
            this.groupBoxExecutionInfo.TabIndex = 1;
            this.groupBoxExecutionInfo.TabStop = false;
            this.groupBoxExecutionInfo.Text = "🎯 执行信息";
            //
            // textBoxOperator
            //
            this.textBoxOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxOperator.Location = new System.Drawing.Point(100, 85);
            this.textBoxOperator.Name = "textBoxOperator";
            this.textBoxOperator.ReadOnly = true;
            this.textBoxOperator.Size = new System.Drawing.Size(200, 23);
            this.textBoxOperator.TabIndex = 5;
            //
            // labelOperator
            //
            this.labelOperator.AutoSize = true;
            this.labelOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelOperator.Location = new System.Drawing.Point(20, 88);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(56, 17);
            this.labelOperator.TabIndex = 4;
            this.labelOperator.Text = "操作员：";
            //
            // textBoxWorkshop
            //
            this.textBoxWorkshop.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxWorkshop.Location = new System.Drawing.Point(100, 55);
            this.textBoxWorkshop.Name = "textBoxWorkshop";
            this.textBoxWorkshop.ReadOnly = true;
            this.textBoxWorkshop.Size = new System.Drawing.Size(200, 23);
            this.textBoxWorkshop.TabIndex = 3;
            //
            // labelWorkshop
            //
            this.labelWorkshop.AutoSize = true;
            this.labelWorkshop.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelWorkshop.Location = new System.Drawing.Point(20, 58);
            this.labelWorkshop.Name = "labelWorkshop";
            this.labelWorkshop.Size = new System.Drawing.Size(68, 17);
            this.labelWorkshop.TabIndex = 2;
            this.labelWorkshop.Text = "执行车间：";
            //
            // textBoxStatus
            //
            this.textBoxStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxStatus.Location = new System.Drawing.Point(100, 25);
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.Size = new System.Drawing.Size(200, 23);
            this.textBoxStatus.TabIndex = 1;
            //
            // labelStatus
            //
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelStatus.Location = new System.Drawing.Point(20, 28);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(68, 17);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "执行状态：";
            //
            // groupBoxBasicInfo
            //
            this.groupBoxBasicInfo.Controls.Add(this.textBoxProductName);
            this.groupBoxBasicInfo.Controls.Add(this.labelProductName);
            this.groupBoxBasicInfo.Controls.Add(this.textBoxOrderNumber);
            this.groupBoxBasicInfo.Controls.Add(this.labelOrderNumber);
            this.groupBoxBasicInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasicInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBoxBasicInfo.Location = new System.Drawing.Point(10, 10);
            this.groupBoxBasicInfo.Name = "groupBoxBasicInfo";
            this.groupBoxBasicInfo.Size = new System.Drawing.Size(551, 120);
            this.groupBoxBasicInfo.TabIndex = 0;
            this.groupBoxBasicInfo.TabStop = false;
            this.groupBoxBasicInfo.Text = "📋 基本信息";
            //
            // textBoxProductName
            //
            this.textBoxProductName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxProductName.Location = new System.Drawing.Point(100, 55);
            this.textBoxProductName.Name = "textBoxProductName";
            this.textBoxProductName.ReadOnly = true;
            this.textBoxProductName.Size = new System.Drawing.Size(200, 23);
            this.textBoxProductName.TabIndex = 3;
            //
            // labelProductName
            //
            this.labelProductName.AutoSize = true;
            this.labelProductName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelProductName.Location = new System.Drawing.Point(20, 58);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(68, 17);
            this.labelProductName.TabIndex = 2;
            this.labelProductName.Text = "产品名称：";
            //
            // textBoxOrderNumber
            //
            this.textBoxOrderNumber.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.textBoxOrderNumber.Location = new System.Drawing.Point(100, 25);
            this.textBoxOrderNumber.Name = "textBoxOrderNumber";
            this.textBoxOrderNumber.ReadOnly = true;
            this.textBoxOrderNumber.Size = new System.Drawing.Size(200, 23);
            this.textBoxOrderNumber.TabIndex = 1;
            //
            // labelOrderNumber
            //
            this.labelOrderNumber.AutoSize = true;
            this.labelOrderNumber.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelOrderNumber.Location = new System.Drawing.Point(20, 28);
            this.labelOrderNumber.Name = "labelOrderNumber";
            this.labelOrderNumber.Size = new System.Drawing.Size(68, 17);
            this.labelOrderNumber.TabIndex = 0;
            this.labelOrderNumber.Text = "订单编号：";
            //
            // splitter1
            //
            this.splitter1.Location = new System.Drawing.Point(800, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 630);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            //
            // panelLeft
            //
            this.panelLeft.Controls.Add(this.dataGridViewExecution);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelLeft.Size = new System.Drawing.Size(800, 630);
            this.panelLeft.TabIndex = 0;
            //
            // dataGridViewExecution
            //
            this.dataGridViewExecution.AllowUserToAddRows = false;
            this.dataGridViewExecution.AllowUserToDeleteRows = false;
            this.dataGridViewExecution.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewExecution.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewExecution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExecution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExecution.Location = new System.Drawing.Point(10, 10);
            this.dataGridViewExecution.MultiSelect = false;
            this.dataGridViewExecution.Name = "dataGridViewExecution";
            this.dataGridViewExecution.ReadOnly = true;
            this.dataGridViewExecution.RowHeadersVisible = false;
            this.dataGridViewExecution.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewExecution.Size = new System.Drawing.Size(780, 610);
            this.dataGridViewExecution.TabIndex = 0;
            this.dataGridViewExecution.SelectionChanged += new System.EventHandler(this.dataGridViewExecution_SelectionChanged);
            //
            // ProductionExecutionControlForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "ProductionExecutionControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生产执行控制";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.groupBoxDetails.ResumeLayout(false);
            this.panelDetailsContent.ResumeLayout(false);
            this.groupBoxTimeInfo.ResumeLayout(false);
            this.groupBoxTimeInfo.PerformLayout();
            this.groupBoxProgressInfo.ResumeLayout(false);
            this.groupBoxProgressInfo.PerformLayout();
            this.groupBoxExecutionInfo.ResumeLayout(false);
            this.groupBoxExecutionInfo.PerformLayout();
            this.groupBoxBasicInfo.ResumeLayout(false);
            this.groupBoxBasicInfo.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExecution)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Panel panelDetailsContent;
        private System.Windows.Forms.GroupBox groupBoxTimeInfo;
        private System.Windows.Forms.Label labelCreateTime;
        private System.Windows.Forms.Label labelEndTime;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.GroupBox groupBoxProgressInfo;
        private System.Windows.Forms.Label labelProgressPercent;
        private System.Windows.Forms.ProgressBar progressBarExecution;
        private System.Windows.Forms.TextBox textBoxCompletedQuantity;
        private System.Windows.Forms.Label labelCompletedQuantity;
        private System.Windows.Forms.TextBox textBoxPlannedQuantity;
        private System.Windows.Forms.Label labelPlannedQuantity;
        private System.Windows.Forms.GroupBox groupBoxExecutionInfo;
        private System.Windows.Forms.TextBox textBoxOperator;
        private System.Windows.Forms.Label labelOperator;
        private System.Windows.Forms.TextBox textBoxWorkshop;
        private System.Windows.Forms.Label labelWorkshop;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.GroupBox groupBoxBasicInfo;
        private System.Windows.Forms.TextBox textBoxProductName;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TextBox textBoxOrderNumber;
        private System.Windows.Forms.Label labelOrderNumber;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.DataGridView dataGridViewExecution;
    }
}