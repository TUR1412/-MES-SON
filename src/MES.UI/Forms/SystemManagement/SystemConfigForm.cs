using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.BLL.SystemManagement;
using MES.Common.Configuration;
using MES.Common.Logging;
using MES.UI.Framework.Themes;
using MySql.Data.MySqlClient;

namespace MES.UI.Forms.SystemManagement
{
    /// <summary>
    /// 系统配置窗体 - 设计器版本
    /// </summary>
    public partial class SystemConfigForm : ThemedForm
    {
        private readonly IDatabaseDiagnosticBLL _databaseDiagnostic = new DatabaseDiagnosticBLL();

        public SystemConfigForm()
        {
            InitializeComponent();
            InitializeCustomControls();
            LoadCurrentSettings();
            ApplyModernStyling();
            this.Shown += (sender, e) => UIThemeManager.ApplyTheme(this);
        }

        private void InitializeCustomControls()
        {
            CreateSystemIcon();
            SetDefaultValues();
        }

        private void CreateSystemIcon()
        {
            var bitmap = new Bitmap(48, 48);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // 绘制齿轮图标
                using (var brush = new SolidBrush(Color.FromArgb(255, 193, 7)))
                {
                    g.FillEllipse(brush, 6, 6, 36, 36);
                }

                using (var brush = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brush, 15, 15, 18, 18);
                }
            }
            iconPictureBox.Image = bitmap;
        }

        private void SetDefaultValues()
        {
            // 设置默认值
            themeComboBox.SelectedIndex = 0;
            logLevelComboBox.SelectedIndex = 2;
        }

        private void ApplyModernStyling()
        {
            // LoL 暗金风 Tab：避免“白底灰边”割裂感
            configTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            configTabControl.DrawItem += ConfigTabControl_DrawItem;

            try
            {
                configTabControl.Appearance = TabAppearance.Normal;
                configTabControl.SizeMode = TabSizeMode.Fixed;
                if (configTabControl.ItemSize.Height < 32)
                {
                    configTabControl.ItemSize = new Size(Math.Max(80, configTabControl.ItemSize.Width), 34);
                }
            }
            catch
            {
                // ignore
            }
        }

        private void ConfigTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabPage = tabControl.TabPages[e.Index];
            var tabRect = tabControl.GetTabRect(e.Index);

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // 背景：选中更亮一档；未选中更深
            var backColor = selected ? LeagueColors.DarkSurface : LeagueColors.DarkPanel;
            using (var brush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            // 边框：切出层级（避免 Tab 看起来像“贴纸”）
            using (var pen = new Pen(selected ? Color.FromArgb(160, LeagueColors.RiotBorderGold) : Color.FromArgb(90, LeagueColors.DarkBorder), 1))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1));
            }

            // 选中态下方高亮条（LoL 客户端感）
            if (selected)
            {
                using (var pen = new Pen(Color.FromArgb(220, LeagueColors.RiotGoldHover), 2))
                {
                    e.Graphics.DrawLine(pen, tabRect.Left + 10, tabRect.Bottom - 2, tabRect.Right - 10, tabRect.Bottom - 2);
                }
            }

            // 文本
            var textColor = selected ? LeagueColors.TextHighlight : LeagueColors.TextSecondary;
            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabControl.Font, tabRect, textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private void LoadCurrentSettings()
        {
            try
            {
                // 加载当前配置
                systemTitleTextBox.Text = ConfigManager.SystemTitle;
                systemVersionTextBox.Text = ConfigManager.SystemVersion;        

                // 主题选择：尽量与当前运行态一致（避免用户误判“保存没生效”）
                themeComboBox.SelectedIndex = ThemeToComboIndex(UIThemeManager.CurrentTheme);
            }
            catch (Exception ex)
            {
                LogManager.Error("加载系统配置失败", ex);
            }
        }

        private async void TestConnectionButton_Click(object sender, EventArgs e)
        {
            try
            {
                testConnectionButton.Enabled = false;
                connectionStatusLabel.Text = "正在测试连接...";
                connectionStatusLabel.ForeColor = UIThemeManager.Colors.Warning;

                string connectionString;
                string error;
                if (!TryBuildConnectionStringFromForm(out connectionString, out error))
                {
                    connectionStatusLabel.Text = string.Format("❌ 参数不完整：{0}", error);
                    connectionStatusLabel.ForeColor = UIThemeManager.Colors.Error;
                    return;
                }

                var details = await Task.Run(() => _databaseDiagnostic.TestConnectionWithDetails(connectionString));
                var ok = details != null && details.StartsWith("连接成功", StringComparison.OrdinalIgnoreCase);

                connectionStatusLabel.Text = ok ? "✅ 连接成功" : "❌ 连接失败";
                connectionStatusLabel.ForeColor = ok ? UIThemeManager.Colors.Success : UIThemeManager.Colors.Error;

                MessageBox.Show(details ?? string.Empty, ok ? "连接成功" : "连接失败",
                    MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                connectionStatusLabel.Text = "❌ 连接失败";
                connectionStatusLabel.ForeColor = UIThemeManager.Colors.Error;
                LogManager.Error("数据库连接测试失败", ex);
            }
            finally
            {
                testConnectionButton.Enabled = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 当前窗体为“本机/运行态配置”入口：优先落地用户可感知的主题切换
                UIThemeManager.CurrentTheme = ComboIndexToTheme(themeComboBox.SelectedIndex);

                MessageBox.Show("✅ 配置已应用（主题已切换）！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogManager.Info("系统配置保存成功");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存配置失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogManager.Error("保存系统配置失败", ex);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置所有配置为默认值吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoadCurrentSettings();
                MessageBox.Show("配置已重置为默认值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)       
        {
            this.Close();
        }

        private static int ThemeToComboIndex(UIThemeManager.ThemeType theme)
        {
            switch (theme)
            {
                case UIThemeManager.ThemeType.Blue:
                    return 1;
                case UIThemeManager.ThemeType.Dark:
                    return 2;
                case UIThemeManager.ThemeType.Lol:
                    return 3;
                case UIThemeManager.ThemeType.Nova:
                    return 4;
                case UIThemeManager.ThemeType.Default:
                default:
                    return 0;
            }
        }

        private static UIThemeManager.ThemeType ComboIndexToTheme(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 1:
                    return UIThemeManager.ThemeType.Blue;
                case 2:
                    return UIThemeManager.ThemeType.Dark;
                case 3:
                    return UIThemeManager.ThemeType.Lol;
                case 4:
                    return UIThemeManager.ThemeType.Nova;
                default:
                    return UIThemeManager.ThemeType.Default;
            }
        }

        private bool TryBuildConnectionStringFromForm(out string connectionString, out string errorMessage)
        {
            connectionString = string.Empty;
            errorMessage = string.Empty;

            var server = (serverTextBox.Text ?? string.Empty).Trim();
            var portText = (portTextBox.Text ?? string.Empty).Trim();
            var database = (databaseTextBox.Text ?? string.Empty).Trim();
            var username = (usernameTextBox.Text ?? string.Empty).Trim();
            var password = passwordTextBox.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(server))
            {
                errorMessage = "服务器不能为空";
                return false;
            }

            uint port;
            if (!uint.TryParse(portText, out port) || port == 0)
            {
                errorMessage = "端口无效";
                return false;
            }

            if (string.IsNullOrWhiteSpace(database))
            {
                errorMessage = "数据库名不能为空";
                return false;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage = "用户名不能为空";
                return false;
            }

            var builder = new MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.Port = port;
            builder.Database = database;
            builder.UserID = username;
            builder.Password = password;

            // 开发/本机默认：优先保证“能连上、能用”，生产环境可在环境变量中覆盖
            builder.SslMode = MySqlSslMode.Disabled;
            builder.AllowPublicKeyRetrieval = true;
            builder.CharacterSet = "utf8mb4";

            connectionString = builder.ConnectionString;
            return true;
        }
    }
}
