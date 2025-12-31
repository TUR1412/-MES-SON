using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MES.Common.Configuration;
using MES.Common.Logging;
using MySql.Data.MySqlClient;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms.SystemManagement
{
    /// <summary>
    /// 数据库诊断工具窗体
    /// </summary>
    public partial class DatabaseDiagnosticForm : ThemedForm
    {
        #region 私有字段

        private bool _isConnected = false;
        private string _connectionString = string.Empty;
        private DateTime _lastRefreshTime = DateTime.MinValue;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化数据库诊断窗体
        /// </summary>
        public DatabaseDiagnosticForm()
        {
            InitializeComponent();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
            InitializeForm();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // 获取数据库连接字符串（以 App.config 为准，避免硬编码导致“能跑但不通”的幽灵问题）
                _connectionString = GetEffectiveConnectionString();
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    toolStripStatusLabel.Text = "未配置数据库连接字符串：请设置 MES_CONNECTION_STRING 或配置 App.config";
                    return;
                }

                // 设置窗体图标
                this.Icon = SystemIcons.Information;
                
                // 初始化数据网格
                InitializeDataGrid();
                
                // 设置状态栏
                toolStripStatusLabel.Text = "数据库诊断工具已启动";
                
                LogManager.Info("数据库诊断工具窗体初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化数据库诊断窗体失败", ex);
                MessageBox.Show(string.Format("初始化失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetEffectiveConnectionString()
        {
            // 优先使用统一配置入口（Environment=Development/Test/Production）
            var connectionString = ConfigManager.GetCurrentConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return string.Empty;
            }

            // 兼容 MySQL 8/9 的默认认证（caching_sha2_password）在非 SSL 连接下的 RSA Key 获取限制
            if (connectionString.IndexOf("AllowPublicKeyRetrieval", StringComparison.OrdinalIgnoreCase) < 0 &&
                connectionString.IndexOf("SslMode=none", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (!connectionString.TrimEnd().EndsWith(";"))
                {
                    connectionString += ";";
                }
                connectionString += "AllowPublicKeyRetrieval=true;";
            }

            return connectionString;
        }

        /// <summary>
        /// 初始化数据网格
        /// </summary>
        private void InitializeDataGrid()
        {
            try
            {
                // 设置数据网格样式
                dgvDiagnosticInfo.EnableHeadersVisualStyles = false;

                // 视觉：DataGridView 的配色/密度由主题系统统一管理（避免“浅底黑字”割裂）
                dgvDiagnosticInfo.DefaultCellStyle.Font = DesignTokens.Typography.CreateBaseFont();
                
                // 设置列宽比例
                colProperty.FillWeight = 30;
                colValue.FillWeight = 50;
                colStatus.FillWeight = 20;
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化数据网格失败", ex);
            }
        }

        #endregion

        #region 窗体事件

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void DatabaseDiagnosticForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    return;
                }

                // 执行初始诊断
                PerformDiagnosis();
                
                // 启动自动刷新定时器
                timerRefresh.Start();
                
                LogManager.Info("数据库诊断工具窗体加载完成");
            }
            catch (Exception ex)
            {
                LogManager.Error("窗体加载失败", ex);
                MessageBox.Show(string.Format("加载失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 按钮事件

        /// <summary>
        /// 刷新诊断按钮点击事件
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel.Text = "正在刷新诊断信息...";
                btnRefresh.Enabled = false;
                
                PerformDiagnosis();
                
                toolStripStatusLabel.Text = string.Format("诊断信息已刷新 - {0}", DateTime.Now.ToString("HH:mm:ss"));
                LogManager.Info("手动刷新数据库诊断信息");
            }
            catch (Exception ex)
            {
                LogManager.Error("刷新诊断信息失败", ex);
                MessageBox.Show(string.Format("刷新失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "刷新失败";
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        /// <summary>
        /// 测试连接按钮点击事件
        /// </summary>
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel.Text = "正在测试数据库连接...";
                btnTestConnection.Enabled = false;
                
                bool testResult = TestDatabaseConnection();
                
                if (testResult)
                {
                    MessageBox.Show("数据库连接测试成功！", "连接测试",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    toolStripStatusLabel.Text = "连接测试成功";
                    LogManager.Info("数据库连接测试成功");
                }
                else
                {
                    MessageBox.Show("数据库连接测试失败！请检查连接配置。", "连接测试",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    toolStripStatusLabel.Text = "连接测试失败";
                    LogManager.Warning("数据库连接测试失败");
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("测试数据库连接失败", ex);
                MessageBox.Show(string.Format("连接测试失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "连接测试异常";
            }
            finally
            {
                btnTestConnection.Enabled = true;
            }
        }

        /// <summary>
        /// 导出报告按钮点击事件
        /// </summary>
        private void btnExportReport_Click(object sender, EventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*",
                    DefaultExt = "txt",
                    FileName = string.Format("数据库诊断报告_{0}.txt", DateTime.Now.ToString("yyyyMMdd_HHmmss"))
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportDiagnosticReport(saveDialog.FileName);
                    MessageBox.Show("诊断报告导出成功！", "导出完成",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogManager.Info(string.Format("导出数据库诊断报告：{0}", saveDialog.FileName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("导出诊断报告失败", ex);
                MessageBox.Show(string.Format("导出失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                // 停止定时器
                timerRefresh.Stop();
                
                // 关闭窗体
                this.Close();
                
                LogManager.Info("数据库诊断工具窗体已关闭");
            }
            catch (Exception ex)
            {
                LogManager.Error("关闭窗体失败", ex);
            }
        }

        #endregion

        #region 定时器事件

        /// <summary>
        /// 自动刷新定时器事件
        /// </summary>
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                // 每30秒自动刷新一次
                PerformDiagnosis();
                _lastRefreshTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                LogManager.Error("自动刷新诊断信息失败", ex);
            }
        }

        #endregion

        #region 核心诊断方法

        /// <summary>
        /// 执行数据库诊断
        /// </summary>
        private void PerformDiagnosis()
        {
            try
            {
                // 测试数据库连接
                _isConnected = TestDatabaseConnection();
                
                // 更新连接状态显示
                UpdateConnectionStatus();
                
                if (_isConnected)
                {
                    // 获取数据库信息
                    UpdateDatabaseInfo();
                    
                    // 获取性能统计
                    UpdatePerformanceStats();
                    
                    // 更新详细诊断信息
                    UpdateDetailedDiagnostics();
                }
                else
                {
                    // 清空信息显示
                    ClearDiagnosticInfo();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("执行数据库诊断失败", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        private bool TestDatabaseConnection()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("测试数据库连接失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 更新连接状态显示
        /// </summary>
        private void UpdateConnectionStatus()
        {
            try
            {
                if (_isConnected)
                {
                    lblConnectionStatusValue.Text = "正常";
                    lblConnectionStatusValue.ForeColor = Color.Green;
                }
                else
                {
                    lblConnectionStatusValue.Text = "断开";
                    lblConnectionStatusValue.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("更新连接状态显示失败", ex);
            }
        }

        /// <summary>
        /// 更新数据库信息
        /// </summary>
        private void UpdateDatabaseInfo()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 获取服务器信息
                    var serverVersion = connection.ServerVersion;
                    lblServerInfoValue.Text = string.Format("MySQL {0}", serverVersion);

                    // 获取数据库名称
                    var databaseName = connection.Database;
                    lblDatabaseNameValue.Text = databaseName;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("更新数据库信息失败", ex);
            }
        }

        /// <summary>
        /// 更新性能统计信息
        /// </summary>
        private void UpdatePerformanceStats()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 获取连接数
                    var connectionCount = GetConnectionCount(connection);
                    lblConnectionCountValue.Text = connectionCount.ToString();

                    // 获取数据库大小
                    var databaseSize = GetDatabaseSize(connection);
                    lblDatabaseSizeValue.Text = string.Format("{0:F2} MB", databaseSize);

                    // 获取表数量
                    var tableCount = GetTableCount(connection);
                    lblTableCountValue.Text = tableCount.ToString();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("更新性能统计信息失败", ex);
            }
        }

        /// <summary>
        /// 获取连接数
        /// </summary>
        private int GetConnectionCount(MySqlConnection connection)
        {
            try
            {
                var command = new MySqlCommand("SHOW STATUS LIKE 'Threads_connected'", connection);
                var result = command.ExecuteScalar();
                int count;
                if (result != null && int.TryParse(result.ToString(), out count))
                {
                    return count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取连接数失败", ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取数据库大小（MB）
        /// </summary>
        private double GetDatabaseSize(MySqlConnection connection)
        {
            try
            {
                var sql = "SELECT ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS 'DB Size in MB' " +
                         "FROM information_schema.tables WHERE table_schema = DATABASE()";
                var command = new MySqlCommand(sql, connection);
                var result = command.ExecuteScalar();
                double size;
                if (result != null && result != DBNull.Value && double.TryParse(result.ToString(), out size))
                {
                    return size;
                }
                return 0.0;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取数据库大小失败", ex);
                return 0.0;
            }
        }

        /// <summary>
        /// 获取表数量
        /// </summary>
        private int GetTableCount(MySqlConnection connection)
        {
            try
            {
                var sql = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE()";
                var command = new MySqlCommand(sql, connection);
                var result = command.ExecuteScalar();
                int count;
                if (result != null && int.TryParse(result.ToString(), out count))
                {
                    return count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取表数量失败", ex);
                return 0;
            }
        }

        /// <summary>
        /// 更新详细诊断信息
        /// </summary>
        private void UpdateDetailedDiagnostics()
        {
            try
            {
                dgvDiagnosticInfo.Rows.Clear();

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 添加各种诊断信息
                    AddDiagnosticRow("数据库连接", "正常", "✓ 正常");
                    AddDiagnosticRow("MySQL版本", connection.ServerVersion, "✓ 正常");
                    AddDiagnosticRow("字符集", GetCharacterSet(connection), "✓ 正常");
                    AddDiagnosticRow("时区", GetTimeZone(connection), "✓ 正常");
                    AddDiagnosticRow("最大连接数", GetMaxConnections(connection).ToString(), "✓ 正常");
                    AddDiagnosticRow("查询缓存", GetQueryCacheStatus(connection), "✓ 正常");
                    AddDiagnosticRow("InnoDB状态", GetInnoDBStatus(connection), "✓ 正常");
                    AddDiagnosticRow("上次检查时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "✓ 正常");
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("更新详细诊断信息失败", ex);
                AddDiagnosticRow("诊断失败", ex.Message, "✗ 异常");
            }
        }

        /// <summary>
        /// 添加诊断行
        /// </summary>
        private void AddDiagnosticRow(string property, string value, string status)
        {
            try
            {
                var row = dgvDiagnosticInfo.Rows.Add();
                dgvDiagnosticInfo.Rows[row].Cells[0].Value = property;
                dgvDiagnosticInfo.Rows[row].Cells[1].Value = value;
                dgvDiagnosticInfo.Rows[row].Cells[2].Value = status;

                // 根据状态设置行颜色
                if (status.Contains("✗"))
                {
                    dgvDiagnosticInfo.Rows[row].DefaultCellStyle.ForeColor = Color.Red;
                }
                else if (status.Contains("⚠"))
                {
                    dgvDiagnosticInfo.Rows[row].DefaultCellStyle.ForeColor = Color.Orange;
                }
                else
                {
                    dgvDiagnosticInfo.Rows[row].DefaultCellStyle.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("添加诊断行失败", ex);
            }
        }

        /// <summary>
        /// 获取字符集
        /// </summary>
        private string GetCharacterSet(MySqlConnection connection)
        {
            try
            {
                var command = new MySqlCommand("SELECT @@character_set_database", connection);
                var result = command.ExecuteScalar();
                return result != null ? result.ToString() : "未知";
            }
            catch (Exception ex)
            {
                LogManager.Error("获取字符集失败", ex);
                return "获取失败";
            }
        }

        /// <summary>
        /// 获取时区
        /// </summary>
        private string GetTimeZone(MySqlConnection connection)
        {
            try
            {
                var command = new MySqlCommand("SELECT @@time_zone", connection);
                var result = command.ExecuteScalar();
                return result != null ? result.ToString() : "未知";
            }
            catch (Exception ex)
            {
                LogManager.Error("获取时区失败", ex);
                return "获取失败";
            }
        }

        /// <summary>
        /// 获取最大连接数
        /// </summary>
        private int GetMaxConnections(MySqlConnection connection)
        {
            try
            {
                var command = new MySqlCommand("SELECT @@max_connections", connection);
                var result = command.ExecuteScalar();
                int maxConn;
                if (result != null && int.TryParse(result.ToString(), out maxConn))
                {
                    return maxConn;
                }
                return 0;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取最大连接数失败", ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取查询缓存状态
        /// </summary>
        private string GetQueryCacheStatus(MySqlConnection connection)
        {
            try
            {
                var command = new MySqlCommand("SELECT @@query_cache_type", connection);
                var result = command.ExecuteScalar();
                return result != null ? result.ToString() : "未知";
            }
            catch (Exception ex)
            {
                LogManager.Error("获取查询缓存状态失败", ex);
                return "获取失败";
            }
        }

        /// <summary>
        /// 获取InnoDB状态
        /// </summary>
        private string GetInnoDBStatus(MySqlConnection connection)
        {
            try
            {
                var command = new MySqlCommand("SELECT @@innodb_version", connection);
                var result = command.ExecuteScalar();
                return result != null ? string.Format("版本 {0}", result.ToString()) : "未知";
            }
            catch (Exception ex)
            {
                LogManager.Error("获取InnoDB状态失败", ex);
                return "获取失败";
            }
        }

        /// <summary>
        /// 清空诊断信息
        /// </summary>
        private void ClearDiagnosticInfo()
        {
            try
            {
                lblServerInfoValue.Text = "无法连接";
                lblDatabaseNameValue.Text = "无法连接";
                lblConnectionCountValue.Text = "0";
                lblDatabaseSizeValue.Text = "0 MB";
                lblTableCountValue.Text = "0";

                dgvDiagnosticInfo.Rows.Clear();
                AddDiagnosticRow("数据库连接", "连接失败", "✗ 异常");
            }
            catch (Exception ex)
            {
                LogManager.Error("清空诊断信息失败", ex);
            }
        }

        /// <summary>
        /// 导出诊断报告
        /// </summary>
        private void ExportDiagnosticReport(string fileName)
        {
            try
            {
                var report = new StringBuilder();
                report.AppendLine("=== MES数据库诊断报告 ===");
                report.AppendLine(string.Format("生成时间：{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                report.AppendLine();

                // 连接信息
                report.AppendLine("【连接信息】");
                report.AppendLine(string.Format("连接状态：{0}", lblConnectionStatusValue.Text));
                report.AppendLine(string.Format("服务器：{0}", lblServerInfoValue.Text));
                report.AppendLine(string.Format("数据库：{0}", lblDatabaseNameValue.Text));
                report.AppendLine();

                // 性能统计
                report.AppendLine("【性能统计】");
                report.AppendLine(string.Format("连接数：{0}", lblConnectionCountValue.Text));
                report.AppendLine(string.Format("数据库大小：{0}", lblDatabaseSizeValue.Text));
                report.AppendLine(string.Format("表数量：{0}", lblTableCountValue.Text));
                report.AppendLine();

                // 详细诊断
                report.AppendLine("【详细诊断】");
                foreach (DataGridViewRow row in dgvDiagnosticInfo.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        report.AppendLine(string.Format("{0}：{1} - {2}",
                            row.Cells[0].Value.ToString(),
                            row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "",
                            row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : ""));
                    }
                }

                report.AppendLine();
                report.AppendLine("=== 报告结束 ===");

                File.WriteAllText(fileName, report.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogManager.Error("导出诊断报告失败", ex);
                throw;
            }
        }

        #endregion
    }
}

