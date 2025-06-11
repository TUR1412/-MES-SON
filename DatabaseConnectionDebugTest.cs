using System;
using System.Windows.Forms;
using MES.DAL.Core;
using MES.Common.Logging;

namespace MES.Tests
{
    /// <summary>
    /// 数据库连接调试测试
    /// 用于诊断FileNotFoundException问题
    /// </summary>
    public partial class DatabaseConnectionDebugTest : Form
    {
        public DatabaseConnectionDebugTest()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // 窗体设置
            this.Text = "数据库连接调试测试";
            this.Size = new System.Drawing.Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 测试按钮
            var btnTest = new Button
            {
                Text = "测试数据库连接",
                Location = new System.Drawing.Point(50, 50),
                Size = new System.Drawing.Size(150, 30)
            };
            btnTest.Click += BtnTest_Click;
            this.Controls.Add(btnTest);
            
            // 结果显示
            var txtResult = new TextBox
            {
                Location = new System.Drawing.Point(50, 100),
                Size = new System.Drawing.Size(500, 250),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };
            this.Controls.Add(txtResult);
            this.txtResult = txtResult;
            
            this.ResumeLayout(false);
        }
        
        private TextBox txtResult;

        private void BtnTest_Click(object sender, EventArgs e)
        {
            txtResult.Clear();
            txtResult.AppendText("开始数据库连接测试...\r\n");
            txtResult.AppendText("=========================\r\n");
            
            try
            {
                // 测试1：基本连接测试
                txtResult.AppendText("1. 基本连接测试:\r\n");
                bool connectionResult = DatabaseHelper.TestConnection();
                txtResult.AppendText(string.Format("   结果: {0}\r\n", connectionResult ? "成功" : "失败"));
                
                // 测试2：详细连接信息
                txtResult.AppendText("\r\n2. 详细连接信息:\r\n");
                string detailResult = DatabaseHelper.TestConnectionWithDetails();
                txtResult.AppendText(string.Format("   {0}\r\n", detailResult));
                
                // 测试3：创建连接对象
                txtResult.AppendText("\r\n3. 创建连接对象测试:\r\n");
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    txtResult.AppendText(string.Format("   连接字符串: {0}\r\n", connection.ConnectionString));
                    txtResult.AppendText("   连接对象创建成功\r\n");
                }
                
                // 测试4：执行简单查询
                txtResult.AppendText("\r\n4. 执行简单查询测试:\r\n");
                try
                {
                    var result = DatabaseHelper.ExecuteScalar("SELECT 1", null);
                    txtResult.AppendText(string.Format("   查询结果: {0}\r\n", result));
                    txtResult.AppendText("   简单查询执行成功\r\n");
                }
                catch (Exception queryEx)
                {
                    txtResult.AppendText(string.Format("   查询失败: {0}\r\n", queryEx.Message));
                }
                
                // 测试5：检查表是否存在
                txtResult.AppendText("\r\n5. 检查关键表是否存在:\r\n");
                try
                {
                    var tables = new[] { "process_route", "process_step", "workstation_info" };
                    foreach (var table in tables)
                    {
                        var tableResult = DatabaseHelper.ExecuteScalar(
                            "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'mes_db' AND table_name = @tableName",
                            new[] { DatabaseHelper.CreateParameter("@tableName", table) });
                        bool exists = Convert.ToInt32(tableResult) > 0;
                        txtResult.AppendText(string.Format("   表 {0}: {1}\r\n", table, exists ? "存在" : "不存在"));
                    }
                }
                catch (Exception tableEx)
                {
                    txtResult.AppendText(string.Format("   检查表失败: {0}\r\n", tableEx.Message));
                }
                
            }
            catch (Exception ex)
            {
                txtResult.AppendText(string.Format("\r\n测试过程中发生异常:\r\n{0}\r\n", ex.ToString()));
                LogManager.Error("数据库连接测试失败", ex);
            }
            
            txtResult.AppendText("\r\n=========================\r\n");
            txtResult.AppendText("测试完成\r\n");
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DatabaseConnectionDebugTest());
        }
    }
}
