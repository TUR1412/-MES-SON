using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private bool _isDiagnosing = false;
        private readonly CancellationTokenSource _lifetimeCts = new CancellationTokenSource();

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化数据库诊断窗体
        /// </summary>
        public DatabaseDiagnosticForm()
        {
            InitializeComponent();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);       
            this.FormClosing += DatabaseDiagnosticForm_FormClosing;
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
            connectionString = ConnectionStringHelper.EnsureAllowPublicKeyRetrieval(connectionString);

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
        private async void DatabaseDiagnosticForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    return;
                }

                // 执行初始诊断
                await PerformDiagnosisAsync(DiagnosisTrigger.InitialLoad);

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
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel.Text = "正在刷新诊断信息...";        
                btnRefresh.Enabled = false;

                await PerformDiagnosisAsync(DiagnosisTrigger.ManualRefresh);

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
        private async void btnTestConnection_Click(object sender, EventArgs e)  
        {
            try
            {
                toolStripStatusLabel.Text = "正在测试数据库连接...";      
                btnTestConnection.Enabled = false;

                bool testResult = await Task.Run(() => TestDatabaseConnection());

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
                TryCancelLifetime();

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
        private async void timerRefresh_Tick(object sender, EventArgs e)        
        {
            try
            {
                // 每30秒自动刷新一次
                await PerformDiagnosisAsync(DiagnosisTrigger.AutoRefresh);
            }
            catch (Exception ex)
            {
                LogManager.Error("自动刷新诊断信息失败", ex);
            }
        }

        #endregion

        #region 核心诊断方法

        private enum DiagnosisTrigger
        {
            InitialLoad,
            ManualRefresh,
            AutoRefresh
        }

        private sealed class DiagnosisRow
        {
            public string Property { get; set; }
            public string Value { get; set; }
            public string Status { get; set; }

            public DiagnosisRow(string property, string value, string status)
            {
                Property = property;
                Value = value;
                Status = status;
            }
        }

        private sealed class DiagnosisSnapshot
        {
            public bool IsConnected { get; set; }
            public string ServerVersion { get; set; }
            public string DatabaseName { get; set; }
            public int ConnectionCount { get; set; }
            public double DatabaseSizeMb { get; set; }
            public int TableCount { get; set; }
            public List<DiagnosisRow> Rows { get; set; }
            public string ErrorMessage { get; set; }
        }

        private async Task PerformDiagnosisAsync(DiagnosisTrigger trigger)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return;
            }

            if (_isDiagnosing)
            {
                if (trigger == DiagnosisTrigger.ManualRefresh)
                {
                    toolStripStatusLabel.Text = "正在诊断中，请稍候...";
                }
                return;
            }

            bool wasTimerEnabled = false;
            try { wasTimerEnabled = timerRefresh != null && timerRefresh.Enabled; }
            catch { wasTimerEnabled = false; }

            _isDiagnosing = true;

            try
            {
                if (wasTimerEnabled)
                {
                    try { timerRefresh.Stop(); } catch { }
                }

                SetUiBusy(true);
                toolStripStatusLabel.Text = GetStartStatusText(trigger);

                var startAt = DateTime.Now;
                var token = _lifetimeCts.Token;

                var snapshot = await Task.Run(() => CollectDiagnosisSnapshot(token), token);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                if (IsDisposed || !IsHandleCreated)
                {
                    return;
                }

                ApplyDiagnosisSnapshot(snapshot);

                _lastRefreshTime = DateTime.Now;
                var elapsedMs = (int)(DateTime.Now - startAt).TotalMilliseconds;
                toolStripStatusLabel.Text = string.Format("诊断完成 · {0}（{1}ms）", _lastRefreshTime.ToString("HH:mm:ss"), elapsedMs);
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                LogManager.Error("执行数据库诊断失败", ex);
                try
                {
                    toolStripStatusLabel.Text = "诊断失败";
                    _isConnected = false;
                    UpdateConnectionStatus();
                    dgvDiagnosticInfo.Rows.Clear();
                    AddDiagnosticRow("诊断失败", ex.Message, "✗ 异常");
                }
                catch
                {
                    // ignore
                }
            }
            finally
            {
                _isDiagnosing = false;
                SetUiBusy(false);

                if (wasTimerEnabled && !_lifetimeCts.IsCancellationRequested)
                {
                    try { timerRefresh.Start(); } catch { }
                }
            }
        }

        private DiagnosisSnapshot CollectDiagnosisSnapshot(CancellationToken token)
        {
            var snapshot = new DiagnosisSnapshot
            {
                Rows = new List<DiagnosisRow>()
            };

            try
            {
                token.ThrowIfCancellationRequested();

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    snapshot.IsConnected = connection.State == ConnectionState.Open;
                    if (!snapshot.IsConnected)
                    {
                        snapshot.ErrorMessage = "数据库连接未打开";
                        snapshot.Rows.Add(new DiagnosisRow("数据库连接", "断开", "✗ 异常"));
                        return snapshot;
                    }

                    snapshot.ServerVersion = connection.ServerVersion;
                    snapshot.DatabaseName = connection.Database;

                    token.ThrowIfCancellationRequested();
                    snapshot.ConnectionCount = GetConnectionCount(connection);

                    token.ThrowIfCancellationRequested();
                    snapshot.DatabaseSizeMb = GetDatabaseSize(connection);

                    token.ThrowIfCancellationRequested();
                    snapshot.TableCount = GetTableCount(connection);

                    var maxConn = GetMaxConnections(connection);
                    var cacheStatus = GetQueryCacheStatus(connection);

                    snapshot.Rows.Add(new DiagnosisRow("数据库连接", "正常", "✓ 正常"));
                    snapshot.Rows.Add(new DiagnosisRow("MySQL版本", string.Format("MySQL {0}", snapshot.ServerVersion), "✓ 正常"));

                    snapshot.Rows.Add(ToRow("字符集", GetCharacterSet(connection), "✓ 正常", "⚠ 获取失败"));
                    snapshot.Rows.Add(ToRow("时区", GetTimeZone(connection), "✓ 正常", "⚠ 获取失败"));
                    snapshot.Rows.Add(new DiagnosisRow("最大连接数", maxConn > 0 ? maxConn.ToString() : "未知", maxConn > 0 ? "✓ 正常" : "⚠ 未知"));

                    snapshot.Rows.Add(BuildConnectionUtilizationRow(snapshot.ConnectionCount, maxConn));
                    snapshot.Rows.Add(BuildQueryCacheRow(cacheStatus));
                    snapshot.Rows.Add(ToRow("InnoDB状态", GetInnoDBStatus(connection), "✓ 正常", "⚠ 获取失败"));

                    snapshot.Rows.Add(new DiagnosisRow("上次检查时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ℹ 信息"));
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                snapshot.IsConnected = false;
                snapshot.ErrorMessage = ex.Message;
                snapshot.Rows.Add(new DiagnosisRow("数据库连接", "连接失败", "✗ 异常"));
                snapshot.Rows.Add(new DiagnosisRow("错误信息", ex.Message, "⚠ 注意"));
                LogManager.Error("采集数据库诊断信息失败", ex);
            }

            return snapshot;
        }

        private void ApplyDiagnosisSnapshot(DiagnosisSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return;
            }

            _isConnected = snapshot.IsConnected;
            UpdateConnectionStatus();

            if (snapshot.IsConnected)
            {
                lblServerInfoValue.Text = string.Format("MySQL {0}", snapshot.ServerVersion ?? "未知");
                lblDatabaseNameValue.Text = snapshot.DatabaseName ?? "未知";
                lblConnectionCountValue.Text = snapshot.ConnectionCount.ToString();
                lblDatabaseSizeValue.Text = string.Format("{0:F2} MB", snapshot.DatabaseSizeMb);
                lblTableCountValue.Text = snapshot.TableCount.ToString();
            }
            else
            {
                lblServerInfoValue.Text = "无法连接";
                lblDatabaseNameValue.Text = "无法连接";
                lblConnectionCountValue.Text = "0";
                lblDatabaseSizeValue.Text = "0 MB";
                lblTableCountValue.Text = "0";

                if (!string.IsNullOrWhiteSpace(snapshot.ErrorMessage))
                {
                    toolStripStatusLabel.Text = string.Format("连接失败：{0}", snapshot.ErrorMessage);
                }
            }

            dgvDiagnosticInfo.Rows.Clear();
            if (snapshot.Rows != null)
            {
                foreach (var row in snapshot.Rows)
                {
                    if (row == null) continue;
                    AddDiagnosticRow(row.Property, row.Value, row.Status);
                }
            }
        }

        private static DiagnosisRow ToRow(string property, string value, string okStatus, string failStatus)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "获取失败")
            {
                return new DiagnosisRow(property, string.IsNullOrWhiteSpace(value) ? "未知" : value, failStatus);
            }

            return new DiagnosisRow(property, value, okStatus);
        }

        private static DiagnosisRow BuildConnectionUtilizationRow(int connectionCount, int maxConn)
        {
            if (maxConn <= 0)
            {
                return new DiagnosisRow("连接占用", "未知", "ℹ 信息");
            }

            double ratio = Math.Min(1.0, Math.Max(0.0, (double)connectionCount / (double)maxConn));
            string value = string.Format("{0:P0}（{1}/{2}）", ratio, connectionCount, maxConn);

            if (ratio >= 0.95)
            {
                return new DiagnosisRow("连接占用", value, "✗ 过高");
            }

            if (ratio >= 0.80)
            {
                return new DiagnosisRow("连接占用", value, "⚠ 偏高");
            }

            return new DiagnosisRow("连接占用", value, "✓ 正常");
        }

        private static DiagnosisRow BuildQueryCacheRow(string cacheStatus)
        {
            if (string.IsNullOrWhiteSpace(cacheStatus))
            {
                return new DiagnosisRow("查询缓存", "未知", "ℹ 信息");
            }

            if (cacheStatus.IndexOf("不支持", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return new DiagnosisRow("查询缓存", cacheStatus, "ℹ 不适用");
            }

            if (cacheStatus == "0" || cacheStatus.Equals("OFF", StringComparison.OrdinalIgnoreCase))
            {
                return new DiagnosisRow("查询缓存", "关闭", "ℹ 关闭");
            }

            return new DiagnosisRow("查询缓存", cacheStatus, "ℹ 信息");
        }

        private static string GetStartStatusText(DiagnosisTrigger trigger)
        {
            switch (trigger)
            {
                case DiagnosisTrigger.InitialLoad:
                    return "正在初始化诊断信息...";
                case DiagnosisTrigger.ManualRefresh:
                    return "正在刷新诊断信息...";
                case DiagnosisTrigger.AutoRefresh:
                    return "正在自动刷新诊断信息...";
                default:
                    return "正在诊断...";
            }
        }

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
                var colors = UIThemeManager.Colors;
                if (_isConnected)
                {
                    lblConnectionStatusValue.Text = "正常";
                    lblConnectionStatusValue.ForeColor = colors.Success;
                }
                else
                {
                    lblConnectionStatusValue.Text = "断开";
                    lblConnectionStatusValue.ForeColor = colors.Error;
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
                using (var command = new MySqlCommand("SHOW STATUS LIKE 'Threads_connected'", connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // SHOW STATUS 返回两列：Variable_name, Value
                        object result = reader.FieldCount > 1 ? reader.GetValue(1) : null;
                        int count;
                        if (result != null && int.TryParse(result.ToString(), out count))
                        {
                            return count;
                        }
                    }
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
                object result;
                using (var command = new MySqlCommand(sql, connection))
                {
                    result = command.ExecuteScalar();
                }
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
                object result;
                using (var command = new MySqlCommand(sql, connection))
                {
                    result = command.ExecuteScalar();
                }
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
                var colors = UIThemeManager.Colors;
                if (status.Contains("✗"))
                {
                    dgvDiagnosticInfo.Rows[row].DefaultCellStyle.ForeColor = colors.Error;
                }
                else if (status.Contains("⚠"))
                {
                    dgvDiagnosticInfo.Rows[row].DefaultCellStyle.ForeColor = colors.Warning;
                }
                else if (status.Contains("ℹ"))
                {
                    dgvDiagnosticInfo.Rows[row].DefaultCellStyle.ForeColor = colors.TextSecondary;
                }
                else
                {
                    dgvDiagnosticInfo.Rows[row].DefaultCellStyle.ForeColor = colors.Success;
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
                object result;
                using (var command = new MySqlCommand("SELECT @@character_set_database", connection))
                {
                    result = command.ExecuteScalar();
                }
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
                object result;
                using (var command = new MySqlCommand("SELECT @@time_zone", connection))
                {
                    result = command.ExecuteScalar();
                }
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
                object result;
                using (var command = new MySqlCommand("SELECT @@max_connections", connection))
                {
                    result = command.ExecuteScalar();
                }
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
                object result;
                using (var command = new MySqlCommand("SELECT @@query_cache_type", connection))
                {
                    result = command.ExecuteScalar();
                }
                return result != null ? result.ToString() : "未知";
            }
            catch (MySqlException ex)
            {
                // MySQL 8+ 已移除 query cache，Unknown system variable 属于“正常的不适用”
                if (ex != null && ex.Message != null && ex.Message.IndexOf("Unknown system variable", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return "不支持（MySQL 8+ 已移除）";
                }

                LogManager.Error("获取查询缓存状态失败", ex);
                return "获取失败";
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
                object result;
                using (var command = new MySqlCommand("SELECT @@innodb_version", connection))
                {
                    result = command.ExecuteScalar();
                }
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

        private void DatabaseDiagnosticForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                try { timerRefresh.Stop(); } catch { }
                TryCancelLifetime();
            }
            catch
            {
                // ignore
            }
        }

        private void TryCancelLifetime()
        {
            try
            {
                if (_lifetimeCts != null && !_lifetimeCts.IsCancellationRequested)
                {
                    _lifetimeCts.Cancel();
                }
            }
            catch
            {
                // ignore
            }
        }

        private void SetUiBusy(bool busy)
        {
            try { btnRefresh.Enabled = !busy; } catch { }
            try { btnTestConnection.Enabled = !busy; } catch { }
            try { btnExportReport.Enabled = !busy; } catch { }
        }

        #endregion
    }
}

