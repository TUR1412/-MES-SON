namespace MES.UI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            // LOL 终版主页布局仍保留菜单/工具/状态容器（由代码动态填充内容）
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelNavContent = new System.Windows.Forms.Panel();
            this.treeViewModules = new System.Windows.Forms.TreeView();
            this.panelNavFooter = new System.Windows.Forms.Panel();
            this.labelNavInfo = new System.Windows.Forms.Label();
            this.panelNavHeader = new System.Windows.Forms.Panel();
            this.labelNavTitle = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelStatusInfo = new System.Windows.Forms.Panel();
            this.labelTechInfo = new System.Windows.Forms.Label();
            this.labelStatusTitle = new System.Windows.Forms.Label();
            this.panelModuleCards = new System.Windows.Forms.Panel();
            this.btnWorkshopCard = new System.Windows.Forms.Button();
            this.btnProductionCard = new System.Windows.Forms.Button();
            this.btnMaterialCard = new System.Windows.Forms.Button();
            this.panelWelcome = new System.Windows.Forms.Panel();
            this.labelSystemVersion = new System.Windows.Forms.Label();
            this.labelSystemTitle = new System.Windows.Forms.Label();
            this.panelLeft.SuspendLayout();
            this.panelNavContent.SuspendLayout();
            this.panelNavFooter.SuspendLayout();
            this.panelNavHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelStatusInfo.SuspendLayout();
            this.panelModuleCards.SuspendLayout();
            this.panelWelcome.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Bold);
            this.menuStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(170)))), ((int)(((byte)(110)))));
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(15, 6, 0, 6);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(35)))));
            this.statusStrip1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 728);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(35)))));
            this.toolStrip1.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1200, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "快捷工具栏";
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelLeft.Controls.Add(this.panelNavContent);
            this.panelLeft.Controls.Add(this.panelNavFooter);
            this.panelLeft.Controls.Add(this.panelNavHeader);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 49);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(280, 679);
            this.panelLeft.TabIndex = 2;
            // 
            // panelNavContent
            // 
            this.panelNavContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.panelNavContent.Controls.Add(this.treeViewModules);
            this.panelNavContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNavContent.Location = new System.Drawing.Point(0, 50);
            this.panelNavContent.Name = "panelNavContent";
            this.panelNavContent.Padding = new System.Windows.Forms.Padding(8, 5, 8, 5);
            this.panelNavContent.Size = new System.Drawing.Size(278, 587);
            this.panelNavContent.TabIndex = 1;
            // 
            // treeViewModules
            // 
            this.treeViewModules.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.treeViewModules.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewModules.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeViewModules.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(170)))), ((int)(((byte)(110)))));
            this.treeViewModules.FullRowSelect = true;
            this.treeViewModules.HideSelection = false;
            this.treeViewModules.ItemHeight = 32;
            this.treeViewModules.Location = new System.Drawing.Point(8, 5);
            this.treeViewModules.Name = "treeViewModules";
            this.treeViewModules.ShowLines = false;
            this.treeViewModules.ShowRootLines = false;
            this.treeViewModules.Size = new System.Drawing.Size(262, 577);
            this.treeViewModules.TabIndex = 0;
            // 
            // panelNavFooter
            // 
            this.panelNavFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.panelNavFooter.Controls.Add(this.labelNavInfo);
            this.panelNavFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelNavFooter.Location = new System.Drawing.Point(0, 637);
            this.panelNavFooter.Name = "panelNavFooter";
            this.panelNavFooter.Padding = new System.Windows.Forms.Padding(15, 5, 15, 10);
            this.panelNavFooter.Size = new System.Drawing.Size(278, 40);
            this.panelNavFooter.TabIndex = 2;
            // 
            // labelNavInfo
            // 
            this.labelNavInfo.AutoSize = true;
            this.labelNavInfo.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelNavInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(130)))), ((int)(((byte)(90)))));
            this.labelNavInfo.Location = new System.Drawing.Point(15, 10);
            this.labelNavInfo.Name = "labelNavInfo";
            this.labelNavInfo.Size = new System.Drawing.Size(136, 24);
            this.labelNavInfo.TabIndex = 0;
            this.labelNavInfo.Text = "请选择功能模块";
            // 
            // panelNavHeader
            // 
            this.panelNavHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.panelNavHeader.Controls.Add(this.labelNavTitle);
            this.panelNavHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNavHeader.Location = new System.Drawing.Point(0, 0);
            this.panelNavHeader.Name = "panelNavHeader";
            this.panelNavHeader.Padding = new System.Windows.Forms.Padding(15, 10, 15, 5);
            this.panelNavHeader.Size = new System.Drawing.Size(278, 50);
            this.panelNavHeader.TabIndex = 0;
            // 
            // labelNavTitle
            // 
            this.labelNavTitle.AutoSize = true;
            this.labelNavTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.labelNavTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(170)))), ((int)(((byte)(110)))));
            this.labelNavTitle.Location = new System.Drawing.Point(15, 15);
            this.labelNavTitle.Name = "labelNavTitle";
            this.labelNavTitle.Size = new System.Drawing.Size(110, 31);
            this.labelNavTitle.TabIndex = 0;
            this.labelNavTitle.Text = "功能导航";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(170)))), ((int)(((byte)(110)))));
            this.splitter1.Location = new System.Drawing.Point(280, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 679);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.panelMain.Controls.Add(this.panelStatusInfo);
            this.panelMain.Controls.Add(this.panelModuleCards);
            this.panelMain.Controls.Add(this.panelWelcome);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(284, 49);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(916, 679);
            this.panelMain.TabIndex = 5;
            // 
            // panelStatusInfo
            // 
            this.panelStatusInfo.BackColor = System.Drawing.Color.Transparent;
            this.panelStatusInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelStatusInfo.Controls.Add(this.labelTechInfo);
            this.panelStatusInfo.Controls.Add(this.labelStatusTitle);
            this.panelStatusInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatusInfo.Location = new System.Drawing.Point(20, 340);
            this.panelStatusInfo.Name = "panelStatusInfo";
            this.panelStatusInfo.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.panelStatusInfo.Size = new System.Drawing.Size(876, 140);
            this.panelStatusInfo.TabIndex = 2;
            // 
            // labelTechInfo
            // 
            this.labelTechInfo.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.labelTechInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(130)))), ((int)(((byte)(90)))));
            this.labelTechInfo.Location = new System.Drawing.Point(30, 60);
            this.labelTechInfo.Name = "labelTechInfo";
            this.labelTechInfo.Size = new System.Drawing.Size(800, 60);
            this.labelTechInfo.TabIndex = 1;
            this.labelTechInfo.Text = "技术架构：C# .NET Framework 4.8 + WinForms + MySQL 8.0\r\n开发模式：三层架构 (UI/BLL/DAL/Models/C" +
    "ommon)\r\n版本控制：Git + GitHub (main/develop分支)";
            // 
            // labelStatusTitle
            // 
            this.labelStatusTitle.AutoSize = true;
            this.labelStatusTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.labelStatusTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(170)))), ((int)(((byte)(110)))));
            this.labelStatusTitle.Location = new System.Drawing.Point(30, 20);
            this.labelStatusTitle.Name = "labelStatusTitle";
            this.labelStatusTitle.Size = new System.Drawing.Size(521, 37);
            this.labelStatusTitle.TabIndex = 0;
            this.labelStatusTitle.Text = "当前状态：基础框架已完成，各模块就绪";
            // 
            // panelModuleCards
            // 
            this.panelModuleCards.Controls.Add(this.btnWorkshopCard);
            this.panelModuleCards.Controls.Add(this.btnProductionCard);
            this.panelModuleCards.Controls.Add(this.btnMaterialCard);
            this.panelModuleCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModuleCards.Location = new System.Drawing.Point(20, 140);
            this.panelModuleCards.Name = "panelModuleCards";
            this.panelModuleCards.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.panelModuleCards.Size = new System.Drawing.Size(876, 200);
            this.panelModuleCards.TabIndex = 1;
            // 
            // btnWorkshopCard
            // 
            this.btnWorkshopCard.BackColor = System.Drawing.Color.Transparent;
            this.btnWorkshopCard.FlatAppearance.BorderSize = 0;
            this.btnWorkshopCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWorkshopCard.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnWorkshopCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnWorkshopCard.Location = new System.Drawing.Point(600, 20);
            this.btnWorkshopCard.Name = "btnWorkshopCard";
            this.btnWorkshopCard.Size = new System.Drawing.Size(280, 160);
            this.btnWorkshopCard.TabIndex = 2;
            this.btnWorkshopCard.Text = "▶ 车间管理 (S成员)\r\n\r\n• 车间作业管理\r\n• 在制品管理\r\n• 设备状态管理";
            this.btnWorkshopCard.UseVisualStyleBackColor = false;
            // 
            // btnProductionCard
            // 
            this.btnProductionCard.BackColor = System.Drawing.Color.Transparent;
            this.btnProductionCard.FlatAppearance.BorderSize = 0;
            this.btnProductionCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductionCard.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnProductionCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnProductionCard.Location = new System.Drawing.Point(300, 20);
            this.btnProductionCard.Name = "btnProductionCard";
            this.btnProductionCard.Size = new System.Drawing.Size(280, 160);
            this.btnProductionCard.TabIndex = 1;
            this.btnProductionCard.Text = "▶ 生产管理 (H成员)\r\n\r\n• 生产订单管理\r\n• 生产执行控制\r\n• 用户权限管理";
            this.btnProductionCard.UseVisualStyleBackColor = false;
            // 
            // btnMaterialCard
            // 
            this.btnMaterialCard.BackColor = System.Drawing.Color.Transparent;
            this.btnMaterialCard.FlatAppearance.BorderSize = 0;
            this.btnMaterialCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaterialCard.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.btnMaterialCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(135)))), ((int)(((byte)(84)))));
            this.btnMaterialCard.Location = new System.Drawing.Point(0, 20);
            this.btnMaterialCard.Name = "btnMaterialCard";
            this.btnMaterialCard.Size = new System.Drawing.Size(280, 160);
            this.btnMaterialCard.TabIndex = 0;
            this.btnMaterialCard.Text = "▶ 物料管理 (L成员)\r\n\r\n• 物料信息管理\r\n• BOM物料清单\r\n• 工艺路线配置";
            this.btnMaterialCard.UseVisualStyleBackColor = false;
            // 
            // panelWelcome
            // 
            this.panelWelcome.BackColor = System.Drawing.Color.Transparent;
            this.panelWelcome.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelWelcome.Controls.Add(this.labelSystemVersion);
            this.panelWelcome.Controls.Add(this.labelSystemTitle);
            this.panelWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWelcome.Location = new System.Drawing.Point(20, 20);
            this.panelWelcome.Name = "panelWelcome";
            this.panelWelcome.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.panelWelcome.Size = new System.Drawing.Size(876, 120);
            this.panelWelcome.TabIndex = 0;
            // 
            // labelSystemVersion
            // 
            this.labelSystemVersion.AutoSize = true;
            this.labelSystemVersion.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSystemVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(130)))), ((int)(((byte)(90)))));
            this.labelSystemVersion.Location = new System.Drawing.Point(35, 75);
            this.labelSystemVersion.Name = "labelSystemVersion";
            this.labelSystemVersion.Size = new System.Drawing.Size(315, 31);
            this.labelSystemVersion.TabIndex = 1;
            this.labelSystemVersion.Text = "版本 1.0.0 - 企业级制造管理";
            // 
            // labelSystemTitle
            // 
            this.labelSystemTitle.AutoSize = true;
            this.labelSystemTitle.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold);
            this.labelSystemTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(170)))), ((int)(((byte)(110)))));
            this.labelSystemTitle.Location = new System.Drawing.Point(30, 20);
            this.labelSystemTitle.Name = "labelSystemTitle";
            this.labelSystemTitle.Size = new System.Drawing.Size(420, 64);
            this.labelSystemTitle.TabIndex = 0;
            this.labelSystemTitle.Text = "MES制造执行系统";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MES制造执行系统";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelLeft.ResumeLayout(false);
            this.panelNavContent.ResumeLayout(false);
            this.panelNavFooter.ResumeLayout(false);
            this.panelNavFooter.PerformLayout();
            this.panelNavHeader.ResumeLayout(false);
            this.panelNavHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelStatusInfo.ResumeLayout(false);
            this.panelStatusInfo.PerformLayout();
            this.panelModuleCards.ResumeLayout(false);
            this.panelWelcome.ResumeLayout(false);
            this.panelWelcome.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelNavHeader;
        private System.Windows.Forms.Label labelNavTitle;
        private System.Windows.Forms.Panel panelNavContent;
        private System.Windows.Forms.TreeView treeViewModules;
        private System.Windows.Forms.Panel panelNavFooter;
        private System.Windows.Forms.Label labelNavInfo;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelWelcome;
        private System.Windows.Forms.Label labelSystemTitle;
        private System.Windows.Forms.Label labelSystemVersion;
        private System.Windows.Forms.Panel panelModuleCards;
        private System.Windows.Forms.Button btnMaterialCard;
        private System.Windows.Forms.Button btnProductionCard;
        private System.Windows.Forms.Button btnWorkshopCard;
        private System.Windows.Forms.Panel panelStatusInfo;
        private System.Windows.Forms.Label labelStatusTitle;
        private System.Windows.Forms.Label labelTechInfo;
    }
}
