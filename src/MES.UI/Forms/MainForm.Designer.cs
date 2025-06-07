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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.treeViewModules = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelWelcome = new System.Windows.Forms.Panel();
            this.labelSystemTitle = new System.Windows.Forms.Label();
            this.labelSystemVersion = new System.Windows.Forms.Label();
            this.panelModuleCards = new System.Windows.Forms.Panel();
            this.panelMaterialCard = new System.Windows.Forms.Panel();
            this.labelMaterialTitle = new System.Windows.Forms.Label();
            this.labelMaterialDesc = new System.Windows.Forms.Label();
            this.pictureBoxMaterial = new System.Windows.Forms.PictureBox();
            this.panelProductionCard = new System.Windows.Forms.Panel();
            this.labelProductionTitle = new System.Windows.Forms.Label();
            this.labelProductionDesc = new System.Windows.Forms.Label();
            this.pictureBoxProduction = new System.Windows.Forms.PictureBox();
            this.panelWorkshopCard = new System.Windows.Forms.Panel();
            this.labelWorkshopTitle = new System.Windows.Forms.Label();
            this.labelWorkshopDesc = new System.Windows.Forms.Label();
            this.pictureBoxWorkshop = new System.Windows.Forms.PictureBox();
            this.panelStatusInfo = new System.Windows.Forms.Panel();
            this.labelStatusTitle = new System.Windows.Forms.Label();
            this.labelTechInfo = new System.Windows.Forms.Label();
            this.panelLeft.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelWelcome.SuspendLayout();
            this.panelModuleCards.SuspendLayout();
            this.panelMaterialCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaterial)).BeginInit();
            this.panelProductionCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProduction)).BeginInit();
            this.panelWorkshopCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorkshop)).BeginInit();
            this.panelStatusInfo.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip1
            //
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 9.5F, System.Drawing.FontStyle.Bold);
            this.menuStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(15, 6, 0, 6);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1200, 44);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            //
            // toolStrip1
            //
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Location = new System.Drawing.Point(0, 44);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(15, 3, 1, 3);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(1200, 30);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "快捷工具栏";
            //
            // panelLeft
            //
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLeft.Controls.Add(this.treeViewModules);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 74);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(8);
            this.panelLeft.Size = new System.Drawing.Size(280, 652);
            this.panelLeft.TabIndex = 2;
            //
            // treeViewModules
            //
            this.treeViewModules.BackColor = System.Drawing.Color.White;
            this.treeViewModules.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewModules.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.treeViewModules.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.treeViewModules.FullRowSelect = true;
            this.treeViewModules.HideSelection = false;
            this.treeViewModules.ItemHeight = 32;
            this.treeViewModules.Location = new System.Drawing.Point(8, 8);
            this.treeViewModules.Name = "treeViewModules";
            this.treeViewModules.ShowLines = false;
            this.treeViewModules.ShowPlusMinus = false;
            this.treeViewModules.ShowRootLines = false;
            this.treeViewModules.Size = new System.Drawing.Size(262, 645);
            this.treeViewModules.TabIndex = 0;
            //
            // splitter1
            //
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(226)))), ((int)(((byte)(230)))));
            this.splitter1.Location = new System.Drawing.Point(280, 74);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 652);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            //
            // statusStrip1
            //
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.statusStrip1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 726);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1200, 24);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            //
            // panelMain
            //
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.panelMain.Controls.Add(this.panelStatusInfo);
            this.panelMain.Controls.Add(this.panelModuleCards);
            this.panelMain.Controls.Add(this.panelWelcome);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(284, 74);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(20);
            this.panelMain.Size = new System.Drawing.Size(916, 652);
            this.panelMain.TabIndex = 5;
            //
            // panelWelcome
            //
            this.panelWelcome.BackColor = System.Drawing.Color.White;
            this.panelWelcome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelWelcome.Controls.Add(this.labelSystemVersion);
            this.panelWelcome.Controls.Add(this.labelSystemTitle);
            this.panelWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWelcome.Location = new System.Drawing.Point(20, 20);
            this.panelWelcome.Name = "panelWelcome";
            this.panelWelcome.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.panelWelcome.Size = new System.Drawing.Size(876, 120);
            this.panelWelcome.TabIndex = 0;
            //
            // labelSystemTitle
            //
            this.labelSystemTitle.AutoSize = true;
            this.labelSystemTitle.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Bold);
            this.labelSystemTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.labelSystemTitle.Location = new System.Drawing.Point(30, 20);
            this.labelSystemTitle.Name = "labelSystemTitle";
            this.labelSystemTitle.Size = new System.Drawing.Size(305, 52);
            this.labelSystemTitle.TabIndex = 0;
            this.labelSystemTitle.Text = "MES制造执行系统";
            //
            // labelSystemVersion
            //
            this.labelSystemVersion.AutoSize = true;
            this.labelSystemVersion.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.labelSystemVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.labelSystemVersion.Location = new System.Drawing.Point(35, 75);
            this.labelSystemVersion.Name = "labelSystemVersion";
            this.labelSystemVersion.Size = new System.Drawing.Size(232, 27);
            this.labelSystemVersion.TabIndex = 1;
            this.labelSystemVersion.Text = "版本 1.0.0 - 企业级制造管理";
            //
            // panelModuleCards
            //
            this.panelModuleCards.Controls.Add(this.panelWorkshopCard);
            this.panelModuleCards.Controls.Add(this.panelProductionCard);
            this.panelModuleCards.Controls.Add(this.panelMaterialCard);
            this.panelModuleCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelModuleCards.Location = new System.Drawing.Point(20, 140);
            this.panelModuleCards.Name = "panelModuleCards";
            this.panelModuleCards.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.panelModuleCards.Size = new System.Drawing.Size(876, 200);
            this.panelModuleCards.TabIndex = 1;
            //
            // panelMaterialCard
            //
            this.panelMaterialCard.BackColor = System.Drawing.Color.White;
            this.panelMaterialCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMaterialCard.Controls.Add(this.pictureBoxMaterial);
            this.panelMaterialCard.Controls.Add(this.labelMaterialDesc);
            this.panelMaterialCard.Controls.Add(this.labelMaterialTitle);
            this.panelMaterialCard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelMaterialCard.Location = new System.Drawing.Point(0, 20);
            this.panelMaterialCard.Name = "panelMaterialCard";
            this.panelMaterialCard.Padding = new System.Windows.Forms.Padding(20);
            this.panelMaterialCard.Size = new System.Drawing.Size(280, 160);
            this.panelMaterialCard.TabIndex = 0;
            //
            // pictureBoxMaterial
            //
            this.pictureBoxMaterial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.pictureBoxMaterial.Location = new System.Drawing.Point(20, 20);
            this.pictureBoxMaterial.Name = "pictureBoxMaterial";
            this.pictureBoxMaterial.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxMaterial.TabIndex = 0;
            this.pictureBoxMaterial.TabStop = false;
            //
            // labelMaterialTitle
            //
            this.labelMaterialTitle.AutoSize = true;
            this.labelMaterialTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.labelMaterialTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.labelMaterialTitle.Location = new System.Drawing.Point(80, 25);
            this.labelMaterialTitle.Name = "labelMaterialTitle";
            this.labelMaterialTitle.Size = new System.Drawing.Size(162, 31);
            this.labelMaterialTitle.TabIndex = 1;
            this.labelMaterialTitle.Text = "物料管理 (L成员)";
            //
            // labelMaterialDesc
            //
            this.labelMaterialDesc.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelMaterialDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.labelMaterialDesc.Location = new System.Drawing.Point(20, 80);
            this.labelMaterialDesc.Name = "labelMaterialDesc";
            this.labelMaterialDesc.Size = new System.Drawing.Size(240, 60);
            this.labelMaterialDesc.TabIndex = 2;
            this.labelMaterialDesc.Text = "• 物料信息管理\r\n• BOM物料清单\r\n• 工艺路线配置";
            //
            // panelProductionCard
            //
            this.panelProductionCard.BackColor = System.Drawing.Color.White;
            this.panelProductionCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProductionCard.Controls.Add(this.pictureBoxProduction);
            this.panelProductionCard.Controls.Add(this.labelProductionDesc);
            this.panelProductionCard.Controls.Add(this.labelProductionTitle);
            this.panelProductionCard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelProductionCard.Location = new System.Drawing.Point(300, 20);
            this.panelProductionCard.Name = "panelProductionCard";
            this.panelProductionCard.Padding = new System.Windows.Forms.Padding(20);
            this.panelProductionCard.Size = new System.Drawing.Size(280, 160);
            this.panelProductionCard.TabIndex = 1;
            //
            // pictureBoxProduction
            //
            this.pictureBoxProduction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.pictureBoxProduction.Location = new System.Drawing.Point(20, 20);
            this.pictureBoxProduction.Name = "pictureBoxProduction";
            this.pictureBoxProduction.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxProduction.TabIndex = 0;
            this.pictureBoxProduction.TabStop = false;
            //
            // labelProductionTitle
            //
            this.labelProductionTitle.AutoSize = true;
            this.labelProductionTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.labelProductionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.labelProductionTitle.Location = new System.Drawing.Point(80, 25);
            this.labelProductionTitle.Name = "labelProductionTitle";
            this.labelProductionTitle.Size = new System.Drawing.Size(162, 31);
            this.labelProductionTitle.TabIndex = 1;
            this.labelProductionTitle.Text = "生产管理 (H成员)";
            //
            // labelProductionDesc
            //
            this.labelProductionDesc.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelProductionDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.labelProductionDesc.Location = new System.Drawing.Point(20, 80);
            this.labelProductionDesc.Name = "labelProductionDesc";
            this.labelProductionDesc.Size = new System.Drawing.Size(240, 60);
            this.labelProductionDesc.TabIndex = 2;
            this.labelProductionDesc.Text = "• 生产订单管理\r\n• 生产执行控制\r\n• 用户权限管理";
            //
            // panelWorkshopCard
            //
            this.panelWorkshopCard.BackColor = System.Drawing.Color.White;
            this.panelWorkshopCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelWorkshopCard.Controls.Add(this.pictureBoxWorkshop);
            this.panelWorkshopCard.Controls.Add(this.labelWorkshopDesc);
            this.panelWorkshopCard.Controls.Add(this.labelWorkshopTitle);
            this.panelWorkshopCard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelWorkshopCard.Location = new System.Drawing.Point(600, 20);
            this.panelWorkshopCard.Name = "panelWorkshopCard";
            this.panelWorkshopCard.Padding = new System.Windows.Forms.Padding(20);
            this.panelWorkshopCard.Size = new System.Drawing.Size(280, 160);
            this.panelWorkshopCard.TabIndex = 2;
            //
            // pictureBoxWorkshop
            //
            this.pictureBoxWorkshop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.pictureBoxWorkshop.Location = new System.Drawing.Point(20, 20);
            this.pictureBoxWorkshop.Name = "pictureBoxWorkshop";
            this.pictureBoxWorkshop.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxWorkshop.TabIndex = 0;
            this.pictureBoxWorkshop.TabStop = false;
            //
            // labelWorkshopTitle
            //
            this.labelWorkshopTitle.AutoSize = true;
            this.labelWorkshopTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.labelWorkshopTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.labelWorkshopTitle.Location = new System.Drawing.Point(80, 25);
            this.labelWorkshopTitle.Name = "labelWorkshopTitle";
            this.labelWorkshopTitle.Size = new System.Drawing.Size(162, 31);
            this.labelWorkshopTitle.TabIndex = 1;
            this.labelWorkshopTitle.Text = "车间管理 (S成员)";
            //
            // labelWorkshopDesc
            //
            this.labelWorkshopDesc.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelWorkshopDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.labelWorkshopDesc.Location = new System.Drawing.Point(20, 80);
            this.labelWorkshopDesc.Name = "labelWorkshopDesc";
            this.labelWorkshopDesc.Size = new System.Drawing.Size(240, 60);
            this.labelWorkshopDesc.TabIndex = 2;
            this.labelWorkshopDesc.Text = "• 车间作业管理\r\n• 在制品管理\r\n• 设备状态管理";
            //
            // panelStatusInfo
            //
            this.panelStatusInfo.BackColor = System.Drawing.Color.White;
            this.panelStatusInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatusInfo.Controls.Add(this.labelTechInfo);
            this.panelStatusInfo.Controls.Add(this.labelStatusTitle);
            this.panelStatusInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatusInfo.Location = new System.Drawing.Point(20, 340);
            this.panelStatusInfo.Name = "panelStatusInfo";
            this.panelStatusInfo.Padding = new System.Windows.Forms.Padding(30, 20, 30, 20);
            this.panelStatusInfo.Size = new System.Drawing.Size(876, 140);
            this.panelStatusInfo.TabIndex = 2;
            //
            // labelStatusTitle
            //
            this.labelStatusTitle.AutoSize = true;
            this.labelStatusTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.labelStatusTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(135)))), ((int)(((byte)(84)))));
            this.labelStatusTitle.Location = new System.Drawing.Point(30, 20);
            this.labelStatusTitle.Name = "labelStatusTitle";
            this.labelStatusTitle.Size = new System.Drawing.Size(372, 31);
            this.labelStatusTitle.TabIndex = 0;
            this.labelStatusTitle.Text = "当前状态：基础框架已完成，各模块就绪";
            //
            // labelTechInfo
            //
            this.labelTechInfo.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.labelTechInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.labelTechInfo.Location = new System.Drawing.Point(30, 60);
            this.labelTechInfo.Name = "labelTechInfo";
            this.labelTechInfo.Size = new System.Drawing.Size(800, 60);
            this.labelTechInfo.TabIndex = 1;
            this.labelTechInfo.Text = "技术架构：C# .NET Framework 4.8 + WinForms + MySQL 8.0\r\n开发模式：三层架构 (UI/BLL/DAL/Models/Common)\r\n版本控制：Git + GitHub (main/develop分支)";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.IsMdiContainer = false;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MES制造执行系统";
            this.panelLeft.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelWelcome.ResumeLayout(false);
            this.panelWelcome.PerformLayout();
            this.panelModuleCards.ResumeLayout(false);
            this.panelMaterialCard.ResumeLayout(false);
            this.panelMaterialCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaterial)).EndInit();
            this.panelProductionCard.ResumeLayout(false);
            this.panelProductionCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProduction)).EndInit();
            this.panelWorkshopCard.ResumeLayout(false);
            this.panelWorkshopCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorkshop)).EndInit();
            this.panelStatusInfo.ResumeLayout(false);
            this.panelStatusInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.TreeView treeViewModules;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelWelcome;
        private System.Windows.Forms.Label labelSystemTitle;
        private System.Windows.Forms.Label labelSystemVersion;
        private System.Windows.Forms.Panel panelModuleCards;
        private System.Windows.Forms.Panel panelMaterialCard;
        private System.Windows.Forms.Label labelMaterialTitle;
        private System.Windows.Forms.Label labelMaterialDesc;
        private System.Windows.Forms.PictureBox pictureBoxMaterial;
        private System.Windows.Forms.Panel panelProductionCard;
        private System.Windows.Forms.Label labelProductionTitle;
        private System.Windows.Forms.Label labelProductionDesc;
        private System.Windows.Forms.PictureBox pictureBoxProduction;
        private System.Windows.Forms.Panel panelWorkshopCard;
        private System.Windows.Forms.Label labelWorkshopTitle;
        private System.Windows.Forms.Label labelWorkshopDesc;
        private System.Windows.Forms.PictureBox pictureBoxWorkshop;
        private System.Windows.Forms.Panel panelStatusInfo;
        private System.Windows.Forms.Label labelStatusTitle;
        private System.Windows.Forms.Label labelTechInfo;
    }
}
