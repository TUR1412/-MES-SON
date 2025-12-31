using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.Common.Configuration;
using MES.Common.Logging;
using MES.UI.Forms.Batch;
using MES.UI.Forms.Material;
using MES.UI.Forms.Production;
using MES.UI.Forms.SystemManagement;
using MES.UI.Forms.WorkOrder;
using MES.UI.Forms.Workshop;
using MES.UI.Framework.Controls;
using MES.UI.Framework.Themes;

namespace MES.UI.Forms
{
    /// <summary>
    /// LoL 客户端风格主界面（V2 / 全新大厅式）
    /// 目标：一眼 LoL 氛围、交互可点、布局简洁、入口明确
    /// </summary>
    public class MainFormLolV2 : ThemedForm
    {
        private readonly string _startupModuleKey;
        private Panel _root;
        private Panel _sidebar;
        private Panel _content;
        private Panel _topBar;
        private Panel _pageContainer;
        private Panel _homeView;
        private Panel _moduleHost;
        private Form _activeModuleForm;
        private Label _pageTitle;
        private LolActionButton _backHomeButton;

        private Panel _cardsHost;
        private FlowLayoutPanel _cardsPanel;
        private Panel _hero;

        private FlowLayoutPanel _navList;
        private LolNavButton _navMaterial;
        private LolNavButton _navProduction;
        private LolNavButton _navWorkOrder;
        private LolNavButton _navBatch;
        private LolNavButton _navWorkshop;
        private LolNavButton _navSystem;

        private Label _dbStatusLabel;
        private Label _timeLabel;
        private Timer _clockTimer;

        private readonly List<string> _lobbyBackgroundFiles = new List<string>();
        private int _lobbyBackgroundIndex = -1;
        private Image _lobbyBackgroundCurrent;
        private Image _lobbyBackgroundNext;
        private float _lobbyBackgroundFadeProgress = 0f;
        private DateTime _lobbyBackgroundFadeStartUtc = DateTime.MinValue;
        private Timer _lobbyBackgroundSwapTimer;
        private Timer _lobbyBackgroundFadeTimer;

        private const int LobbyBackgroundSwapIntervalMs = 12000;
        private const int LobbyBackgroundFadeDurationMs = 900;
        private const float LobbyBackgroundImageOpacity = 0.95f;

        private const int MaxCardColumns = 3;
        private readonly Size _defaultCardSize = new Size(400, 210);
        private readonly Padding _defaultCardMargin = new Padding(14);

        public MainFormLolV2()
            : this(null)
        {
        }

        public MainFormLolV2(string startupModuleKey)
        {
            _startupModuleKey = startupModuleKey;
            InitializeWindow();
            BuildLayout();
            InitializeLobbyBackgroundSlideshow();
            BuildTopBar();
            BuildNavigation();
            BuildSidebarHeader();
            BuildContentCards(); // Home（大厅）
            BuildStatusArea();
            StartClock();

            // 首帧稳态：等窗体真正显示后再做一次布局与状态复位，避免“看不到内容/只看到最后一个字”的体验
            Shown += (s, e) =>
            {
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        ShowHome();
                        OpenStartupModuleIfRequested();
                        SafeInvalidateRoot();
                        try
                        {
                            if (_navMaterial != null) _navMaterial.Focus();
                        }
                        catch
                        {
                            // ignore
                        }
                    }));
                }
                catch
                {
                    // ignore
                }
            };
        }

        private void OpenStartupModuleIfRequested()
        {
            if (string.IsNullOrWhiteSpace(_startupModuleKey))
            {
                return;
            }

            var key = _startupModuleKey.Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            key = key.ToLowerInvariant();
            LogManager.Info(string.Format("启动参数请求打开模块: {0}", key));

            try
            {
                if (key == "material" || key == "materials" || key == "mat")
                {
                    ShowModule<MaterialManagementForm>("物料管理", _navMaterial);
                    return;
                }

                if (key == "production" || key == "prod")
                {
                    ShowModule<ProductionOrderManagementForm>("生产管理", _navProduction);
                    return;
                }

                if (key == "workorder" || key == "wo")
                {
                    ShowModule<WorkOrderManagementForm>("工单管理", _navWorkOrder);
                    return;
                }

                if (key == "batch")
                {
                    ShowModule<BatchManagementForm>("批次管理", _navBatch);
                    return;
                }

                if (key == "workshop")
                {
                    ShowModule<WorkshopManagementForm>("车间管理", _navWorkshop);
                    return;
                }

                if (key == "system")
                {
                    ShowModule<SystemConfigForm>("系统管理", _navSystem);
                    return;
                }

                if (key == "db" || key == "dbdiag" || key == "database")
                {
                    ShowModule<DatabaseDiagnosticForm>("数据库诊断", _navSystem);
                    return;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("启动模块失败", ex);
            }

            LogManager.Warning(string.Format("未知启动模块参数: {0}", _startupModuleKey));
        }

        private void InitializeWindow()
        {
            Text = string.Format("{0} v{1} - LoL 主题（V2）", ConfigManager.SystemTitle, ConfigManager.SystemVersion);
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1200, 760);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = LeagueColors.DarkBackground;
            ForeColor = LeagueColors.TextPrimary;
            Font = new Font("微软雅黑", 9F, FontStyle.Regular);

            // 双缓冲减少闪烁
            DoubleBuffered = true;

            FormClosing += MainFormLolV2_FormClosing;
        }

        private void BuildLayout()
        {
            _root = new Panel { Dock = DockStyle.Fill };
            EnableDoubleBuffering(_root);
            _root.Paint += Root_Paint;
            Controls.Add(_root);

            _content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                AutoScroll = false
            };
            EnableDoubleBuffering(_content);
            _root.Controls.Add(_content);

            _sidebar = new Panel { Dock = DockStyle.Left, Width = 300, BackColor = Color.Transparent };
            EnableDoubleBuffering(_sidebar);
            _sidebar.Paint += (s, e) => LolClientVisuals.DrawSidebarBackground(e.Graphics, _sidebar.ClientRectangle);
            _root.Controls.Add(_sidebar);

            _content.SizeChanged += (s, e) => UpdateCardsPanelLayout();
        }

        private void Root_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                var rect = _root.ClientRectangle;
                if (rect.Width <= 0 || rect.Height <= 0) return;

                // 大厅背景：支持本地图片轮播（不内置任何版权素材）
                if (_lobbyBackgroundCurrent != null || _lobbyBackgroundNext != null)
                {
                    var fade = _lobbyBackgroundFadeProgress;
                    if (fade < 0f) fade = 0f;
                    if (fade > 1f) fade = 1f;

                    var currentOpacity = (1f - fade) * LobbyBackgroundImageOpacity;
                    var nextOpacity = fade * LobbyBackgroundImageOpacity;

                    LolClientVisuals.DrawClientBackground(e.Graphics, rect, _lobbyBackgroundCurrent, currentOpacity, _lobbyBackgroundNext, nextOpacity);
                    return;
                }

                // 没有背景图时：回落到默认 LoL 渐变/噪点背景
                LolClientVisuals.DrawClientBackground(e.Graphics, rect);
            }
            catch
            {
                // ignore
            }
        }

        private void InitializeLobbyBackgroundSlideshow()
        {
            try
            {
                EnsureLobbyBackgroundFolderExists();
                ReloadLobbyBackgroundFiles();

                if (_lobbyBackgroundFiles.Count == 0)
                {
                    return;
                }

                // 先加载第一张
                _lobbyBackgroundIndex = 0;
                _lobbyBackgroundCurrent = TryLoadImageNoLock(_lobbyBackgroundFiles[_lobbyBackgroundIndex]);

                _lobbyBackgroundSwapTimer = new Timer();
                _lobbyBackgroundSwapTimer.Interval = LobbyBackgroundSwapIntervalMs;
                _lobbyBackgroundSwapTimer.Tick += (s, e) => BeginLobbyBackgroundFadeToNext();
                _lobbyBackgroundSwapTimer.Start();

                _lobbyBackgroundFadeTimer = new Timer();
                _lobbyBackgroundFadeTimer.Interval = 30;
                _lobbyBackgroundFadeTimer.Tick += (s, e) => TickLobbyBackgroundFade();
            }
            catch (Exception ex)
            {
                LogManager.Error("初始化大厅背景轮播失败", ex);
            }
        }

        private void EnsureLobbyBackgroundFolderExists()
        {
            try
            {
                var folder = GetLobbyBackgroundFolder();
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch
            {
                // ignore
            }
        }

        private string GetLobbyBackgroundFolder()
        {
            try
            {
                // 使用 exe 同级目录：方便你直接丢图进去，无需改代码/重新编译
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "lobby_backgrounds");
            }
            catch
            {
                return "assets\\lobby_backgrounds";
            }
        }

        private void ReloadLobbyBackgroundFiles()
        {
            _lobbyBackgroundFiles.Clear();

            try
            {
                var folder = GetLobbyBackgroundFolder();
                if (!Directory.Exists(folder))
                {
                    return;
                }

                // 仅支持 WinForms 原生能加载的常见格式
                string[] patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp" };
                foreach (var pattern in patterns)
                {
                    string[] files;
                    try
                    {
                        files = Directory.GetFiles(folder, pattern, SearchOption.TopDirectoryOnly);
                    }
                    catch
                    {
                        continue;
                    }

                    if (files == null || files.Length == 0) continue;
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(files[i]))
                        {
                            _lobbyBackgroundFiles.Add(files[i]);
                        }
                    }
                }

                // 简单排序，保证轮播顺序稳定
                _lobbyBackgroundFiles.Sort(StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                // ignore
            }
        }

        private void BeginLobbyBackgroundFadeToNext()
        {
            try
            {
                if (_lobbyBackgroundFiles.Count == 0) return;
                if (_lobbyBackgroundCurrent == null) return;

                int nextIndex = _lobbyBackgroundIndex + 1;
                if (nextIndex >= _lobbyBackgroundFiles.Count) nextIndex = 0;

                var nextImage = TryLoadImageNoLock(_lobbyBackgroundFiles[nextIndex]);
                if (nextImage == null) return;

                if (_lobbyBackgroundNext != null)
                {
                    try { _lobbyBackgroundNext.Dispose(); } catch { }
                    _lobbyBackgroundNext = null;
                }

                _lobbyBackgroundNext = nextImage;
                _lobbyBackgroundFadeProgress = 0f;
                _lobbyBackgroundFadeStartUtc = DateTime.UtcNow;

                if (_lobbyBackgroundFadeTimer != null)
                {
                    _lobbyBackgroundFadeTimer.Start();
                }

                SafeInvalidateRoot();
            }
            catch
            {
                // ignore
            }
        }

        private void TickLobbyBackgroundFade()
        {
            try
            {
                if (_lobbyBackgroundNext == null || _lobbyBackgroundFadeStartUtc == DateTime.MinValue)
                {
                    _lobbyBackgroundFadeProgress = 0f;
                    if (_lobbyBackgroundFadeTimer != null) _lobbyBackgroundFadeTimer.Stop();
                    return;
                }

                var elapsedMs = (DateTime.UtcNow - _lobbyBackgroundFadeStartUtc).TotalMilliseconds;
                var t = (float)(elapsedMs / LobbyBackgroundFadeDurationMs);

                if (t >= 1f)
                {
                    t = 1f;
                }

                _lobbyBackgroundFadeProgress = t;
                SafeInvalidateRoot();

                if (t >= 1f)
                {
                    // 完成：切换 next -> current
                    if (_lobbyBackgroundCurrent != null)
                    {
                        try { _lobbyBackgroundCurrent.Dispose(); } catch { }
                    }

                    _lobbyBackgroundCurrent = _lobbyBackgroundNext;
                    _lobbyBackgroundNext = null;
                    _lobbyBackgroundFadeProgress = 0f;

                    _lobbyBackgroundIndex++;
                    if (_lobbyBackgroundIndex >= _lobbyBackgroundFiles.Count)
                    {
                        _lobbyBackgroundIndex = 0;
                    }

                    _lobbyBackgroundFadeStartUtc = DateTime.MinValue;
                    if (_lobbyBackgroundFadeTimer != null) _lobbyBackgroundFadeTimer.Stop();
                    SafeInvalidateRoot();
                }
            }
            catch
            {
                // ignore
            }
        }

        private static Image TryLoadImageNoLock(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;

            try
            {
                if (!File.Exists(path)) return null;

                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var img = Image.FromStream(fs))
                {
                    // 克隆一份，避免锁文件 & 避免 stream 生命周期问题
                    return new Bitmap(img);
                }
            }
            catch
            {
                return null;
            }
        }

        private void BuildSidebarHeader()
        {
            var header = new Panel { Dock = DockStyle.Top, Height = 120, BackColor = Color.Transparent, Padding = new Padding(18, 18, 18, 12) };
            EnableDoubleBuffering(header);
            _sidebar.Controls.Add(header);

            var title = new Label
            {
                AutoSize = true,
                Text = "MES · League UI",
                ForeColor = LeagueColors.TextHighlight,
                Font = new Font("微软雅黑", 16F, FontStyle.Bold),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            header.Controls.Add(title);

            var subtitle = new Label
            {
                AutoSize = true,
                Text = "大厅入口 · 暗金风 · 交互优先",
                ForeColor = LeagueColors.TextSecondary,
                Font = new Font("微软雅黑", 9F, FontStyle.Regular),
                Location = new Point(2, 40),
                BackColor = Color.Transparent
            };
            header.Controls.Add(subtitle);

            var hint = new Label
            {
                AutoSize = true,
                Text = "提示：左侧导航 / 右侧卡片均可进入模块",
                ForeColor = Color.FromArgb(180, LeagueColors.TextSecondary),
                Font = new Font("微软雅黑", 8.5F, FontStyle.Regular),
                Location = new Point(2, 68),
                BackColor = Color.Transparent
            };
            header.Controls.Add(hint);
        }

        private void BuildTopBar()
        {
            // 内容区：顶部栏 + 页面容器（Home / ModuleHost）
            _topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.Transparent,
                Padding = new Padding(18, 12, 18, 12)
            };
            EnableDoubleBuffering(_topBar);
            _topBar.Paint += (s, e) =>
            {
                try
                {
                    using (var pen = new Pen(Color.FromArgb(80, LeagueColors.RiotBorderGold), 1))
                    {
                        e.Graphics.DrawLine(pen, 0, _topBar.Height - 1, _topBar.Width, _topBar.Height - 1);
                    }
                }
                catch
                {
                    // ignore
                }
            };

            _pageContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            EnableDoubleBuffering(_pageContainer);
            _content.Controls.Add(_pageContainer);
            _content.Controls.Add(_topBar);

            _homeView = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(22, 18, 22, 18)
            };
            EnableDoubleBuffering(_homeView);
            _pageContainer.Controls.Add(_homeView);

            _moduleHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(22, 18, 22, 18),
                Visible = false
            };
            EnableDoubleBuffering(_moduleHost);
            _pageContainer.Controls.Add(_moduleHost);

            _backHomeButton = new LolActionButton
            {
                Text = "返回大厅",
                Width = 110,
                Height = 32,
                Location = new Point(_topBar.Padding.Left, _topBar.Padding.Top),
                Compact = true,
                Visible = false
            };
            _backHomeButton.Click += (s, e) => ShowHome();
            _topBar.Controls.Add(_backHomeButton);

            _pageTitle = new Label
            {
                AutoSize = false,
                Text = "大厅",
                ForeColor = LeagueColors.TextHighlight,
                Font = new Font("微软雅黑", 12F, FontStyle.Bold),
                Location = new Point(0, 0),
                Height = 32,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            _topBar.Controls.Add(_pageTitle);

            _topBar.SizeChanged += (s, e) =>
            {
                UpdateTopBarLayout();
            };

            UpdateTopBarLayout();
        }

        private void BuildNavigation()
        {
            _navList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(14, 12, 14, 12),
                BackColor = Color.Transparent
            };
            EnableDoubleBuffering(_navList);
            _sidebar.Controls.Add(_navList);

            _navMaterial = CreateNav("物料管理", "物料 / BOM / 工艺路线", "\uE8A7", (s, e) => ShowModule<MaterialManagementForm>("物料管理", _navMaterial), true);
            _navProduction = CreateNav("生产管理", "订单 / 执行控制", "\uE7C1", (s, e) => ShowModule<ProductionOrderManagementForm>("生产管理", _navProduction), false);
            _navWorkOrder = CreateNav("工单管理", "工单增删改查", "\uE8EF", (s, e) => ShowModule<WorkOrderManagementForm>("工单管理", _navWorkOrder), false);
            _navBatch = CreateNav("批次管理", "创建 / 取消 / 查询", "\uE7B8", (s, e) => ShowModule<BatchManagementForm>("批次管理", _navBatch), false);
            _navWorkshop = CreateNav("车间管理", "车间 / WIP / 设备", "\uE80F", (s, e) => ShowModule<WorkshopManagementForm>("车间管理", _navWorkshop), false);
            _navSystem = CreateNav("系统管理", "系统配置 / 数据库诊断", "\uE713", (s, e) => ShowModule<SystemConfigForm>("系统管理", _navSystem), false);

            _navList.Controls.Add(_navMaterial);
            _navList.Controls.Add(_navProduction);
            _navList.Controls.Add(_navWorkOrder);
            _navList.Controls.Add(_navBatch);
            _navList.Controls.Add(_navWorkshop);
            _navList.Controls.Add(_navSystem);
        }

        private LolNavButton CreateNav(string title, string subtitle, string glyph, EventHandler onClick, bool selected)
        {
            var btn = new LolNavButton
            {
                Text = title,
                Subtitle = subtitle,
                IconGlyph = glyph,
                IsSelected = selected,
                Margin = new Padding(0, 0, 0, 10)
            };

            btn.Click += (s, e) =>
            {
                SelectNav(btn);
                if (onClick != null) onClick(s, e);
            };

            return btn;
        }

        private void SelectNav(LolNavButton selected)
        {
            // 轻量“选中态”管理：只处理本页的 6 个按钮
            var buttons = new[] { _navMaterial, _navProduction, _navWorkOrder, _navBatch, _navWorkshop, _navSystem };
            foreach (var b in buttons)
            {
                if (b != null) b.IsSelected = (selected != null) && (b == selected);
            }
        }

        private void BuildContentCards()
        {
            if (_homeView == null)
            {
                // 极端情况兜底：若 TopBar/容器未初始化，至少不让程序崩
                _homeView = _content;
            }

            // Home（大厅）：上方欢迎区 + 下方 Bento 卡片
            _homeView.Controls.Clear();

            // Hero 与卡片区均为 Dock=Top：WinForms Dock 计算依赖控件加入顺序（后加入的控件会先参与 Dock）。
            // 因此先添加卡片区，再添加 Hero，才能保证 Hero 始终在最上方。
            _hero = new Panel { Dock = DockStyle.Top, Height = 116, BackColor = Color.Transparent, Padding = new Padding(6, 6, 6, 10) };
            EnableDoubleBuffering(_hero);

            // 注意：_content 开启 AutoScroll 时，AutoSize=true 的长文本 Label 可能触发横向滚动，导致“只看到最后一个字”。
            // 因此这里使用固定高度 + Anchor 拉伸宽度，避免横向滚动条与奇怪的滚动位置。
            var title = new Label
            {
                AutoSize = false,
                Text = "欢迎回来 · LoL 风格大厅",
                ForeColor = LeagueColors.TextHighlight,
                Font = new Font("微软雅黑", 20F, FontStyle.Bold),
                Location = new Point(0, 0),
                Height = 42,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            _hero.Controls.Add(title);

            var subtitle = new Label
            {
                AutoSize = false,
                Text = "选择一个模块进入 · 暗金风 · 悬停/按下交互反馈",
                ForeColor = LeagueColors.TextSecondary,
                Font = new Font("微软雅黑", 10F, FontStyle.Regular),
                Location = new Point(2, 46),
                Height = 20,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            _hero.Controls.Add(subtitle);

            var tips = new Label
            {
                AutoSize = false,
                Text = "首次运行若提示缺库：按弹窗一键初始化，或执行 database/init_mes_db.ps1。",
                ForeColor = Color.FromArgb(180, LeagueColors.TextSecondary),
                Font = new Font("微软雅黑", 9F, FontStyle.Regular),
                Location = new Point(2, 76),
                Height = 20,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            _hero.Controls.Add(tips);

            // 首次布局时就先把宽度设上（避免窗口首次显示时出现“被裁切/不显示”）
            UpdateHeroLayout();
            _hero.SizeChanged += (s, e) => UpdateHeroLayout();

            // 卡片区（Dock=Top）
            _cardsHost = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 18)
            };
            EnableDoubleBuffering(_cardsHost);
            _cardsHost.SizeChanged += (s, e) => UpdateCardsPanelLayout();
            _homeView.Controls.Add(_cardsHost);
            _homeView.Controls.Add(_hero);

            // 固定栅格宽度（最大 3 列），避免 2K/4K 屏出现 “5+1” 这种不平衡布局
            _cardsPanel = new FlowLayoutPanel
            {
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent,
                Padding = new Padding(0),
                Margin = new Padding(0),
                AutoScroll = false,
                AutoSize = false
            };
            EnableDoubleBuffering(_cardsPanel);
            _cardsHost.Controls.Add(_cardsPanel);

            // 入口卡片（更像 LoL：有明确“进入”指向、图标水印、暗金描边）
            var cardMaterial = CreateCard("物料管理", "物料维护\nBOM 管理\n工艺路线", "\uE8A7", LeagueColors.RiotGold, (s, e) => ShowModule<MaterialManagementForm>("物料管理", _navMaterial));
            var cardProduction = CreateCard("生产管理", "生产订单\n执行控制\n进度追踪", "\uE7C1", LeagueColors.AccentBlueLight, (s, e) => ShowModule<ProductionOrderManagementForm>("生产管理", _navProduction));
            var cardWorkOrder = CreateCard("工单管理", "工单创建\n提交 / 取消\n状态跟踪", "\uE8EF", Color.FromArgb(255, 140, 80), (s, e) => ShowModule<WorkOrderManagementForm>("工单管理", _navWorkOrder));
            var cardBatch = CreateCard("批次管理", "创建批次\n取消批次\n批次查询", "\uE7B8", LeagueColors.SpecialCyan, (s, e) => ShowModule<BatchManagementForm>("批次管理", _navBatch));
            var cardWorkshop = CreateCard("车间管理", "车间维护\nWIP 管理\n设备状态", "\uE80F", LeagueColors.ErrorRed, (s, e) => ShowModule<WorkshopManagementForm>("车间管理", _navWorkshop));
            var cardSystem = CreateCard("系统管理", "系统配置\n数据库诊断\n环境检查", "\uE713", LeagueColors.RiotGoldHover, (s, e) => ShowModule<SystemConfigForm>("系统管理", _navSystem));

            ConfigureCard(cardMaterial, _defaultCardSize);
            ConfigureCard(cardProduction, _defaultCardSize);
            ConfigureCard(cardWorkOrder, _defaultCardSize);
            ConfigureCard(cardBatch, _defaultCardSize);
            ConfigureCard(cardWorkshop, _defaultCardSize);
            ConfigureCard(cardSystem, _defaultCardSize);

            _cardsPanel.Controls.Add(cardMaterial);
            _cardsPanel.Controls.Add(cardProduction);
            _cardsPanel.Controls.Add(cardWorkOrder);
            _cardsPanel.Controls.Add(cardBatch);
            _cardsPanel.Controls.Add(cardWorkshop);
            _cardsPanel.Controls.Add(cardSystem);

            // 先布局一次，避免首屏出现“未居中/宽度不对”的闪现
            UpdateCardsPanelLayout();
        }

        private void UpdateHeroLayout()
        {
            if (_hero == null) return;

            try
            {
                var width = _hero.ClientSize.Width;
                if (width <= 0) return;

                foreach (Control c in _hero.Controls)
                {
                    var label = c as Label;
                    if (label == null) continue;
                    label.Width = Math.Max(1, width - label.Left);
                }
            }
            catch
            {
                // ignore
            }
        }

        private LolCardButton CreateCard(string title, string description, string glyph, Color accent, EventHandler onClick)
        {
            var card = new LolCardButton
            {
                Text = title,
                Description = description,
                IconGlyph = glyph,
                AccentColor = accent,
                Margin = _defaultCardMargin
            };

            if (onClick != null)
            {
                card.Click += onClick;
            }

            return card;
        }

        private static void ConfigureCard(LolCardButton card, Size size)
        {
            if (card == null) return;
            card.Size = size;
            card.Margin = new Padding(14);
            card.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }

        private void UpdateCardsPanelLayout()
        {
            if (_cardsHost == null || _cardsPanel == null) return;

            try
            {
                // 根据当前可用宽度计算“每行能放几张卡”，并限制最大列数，避免宽屏出现 “5+1” 的不平衡
                var availableWidth = _cardsHost.ClientSize.Width - _cardsHost.Padding.Left - _cardsHost.Padding.Right;
                if (availableWidth <= 0) return;

                var cardOuterWidth = _defaultCardSize.Width + _defaultCardMargin.Left + _defaultCardMargin.Right;
                if (cardOuterWidth <= 0) return;

                var colsByWidth = Math.Max(1, availableWidth / cardOuterWidth);
                var cols = Math.Min(MaxCardColumns, colsByWidth);
                cols = Math.Max(1, cols);

                var gridWidth = cols * cardOuterWidth;
                if (gridWidth > availableWidth) gridWidth = availableWidth;

                var left = Math.Max(0, (availableWidth - gridWidth) / 2);

                _cardsPanel.SuspendLayout();
                try
                {
                    // 宽度限制：FlowLayoutPanel 的换行依据“控件外边距 + 容器宽度”，因此必须明确约束 width
                    if (_cardsPanel.Width != gridWidth)
                    {
                        _cardsPanel.Width = gridWidth;
                    }

                    var desiredX = _cardsHost.Padding.Left + left;
                    var desiredY = _cardsHost.Padding.Top;
                    if (_cardsPanel.Location.X != desiredX || _cardsPanel.Location.Y != desiredY)
                    {
                        _cardsPanel.Location = new Point(desiredX, desiredY);
                    }

                // 高度：用确定性栅格算法计算，避免依赖 FlowLayoutPanel 的 PreferredSize（首帧/高 DPI 下可能不稳定）
                var cardCount = _cardsPanel.Controls.Count;
                if (cardCount <= 0) cardCount = 1;

                var cardOuterHeight = _defaultCardSize.Height + _defaultCardMargin.Top + _defaultCardMargin.Bottom;
                var rows = (cardCount + cols - 1) / cols;
                var desiredHeight = Math.Max(1, rows * cardOuterHeight);
                if (_cardsPanel.Height != desiredHeight)
                {
                    _cardsPanel.Height = desiredHeight;
                }

                    var hostHeight = _cardsHost.Padding.Top + _cardsPanel.Height + _cardsHost.Padding.Bottom;
                    if (_cardsHost.Height != hostHeight)
                    {
                        _cardsHost.Height = hostHeight;
                    }
                }
                finally
                {
                    _cardsPanel.ResumeLayout();
                }
            }
            catch
            {
                // ignore
            }
        }

        private void BuildStatusArea()
        {
            var status = new Panel { Dock = DockStyle.Bottom, Height = 120, BackColor = Color.Transparent, Padding = new Padding(18, 10, 18, 16) };
            EnableDoubleBuffering(status);
            _sidebar.Controls.Add(status);

            _dbStatusLabel = new Label
            {
                AutoSize = true,
                Text = "数据库：未检测",
                ForeColor = LeagueColors.TextSecondary,
                Font = new Font("微软雅黑", 9F, FontStyle.Regular),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            status.Controls.Add(_dbStatusLabel);

            _timeLabel = new Label
            {
                AutoSize = true,
                Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ForeColor = Color.FromArgb(190, LeagueColors.TextSecondary),
                Font = new Font("微软雅黑", 9F, FontStyle.Regular),
                Location = new Point(0, 28),
                BackColor = Color.Transparent
            };
            status.Controls.Add(_timeLabel);

            var quickDbDiag = new LolActionButton
            {
                Text = "数据库诊断",
                Width = 120,
                Height = 34,
                Location = new Point(0, 62)
            };
            quickDbDiag.Click += (s, e) => ShowModule<DatabaseDiagnosticForm>("数据库诊断", _navSystem);
            status.Controls.Add(quickDbDiag);

            var quickSystem = new LolActionButton
            {
                Text = "系统配置",
                Width = 120,
                Height = 34,
                Location = new Point(130, 62)
            };
            quickSystem.Click += (s, e) => ShowModule<SystemConfigForm>("系统管理", _navSystem);
            status.Controls.Add(quickSystem);

            // 启动后做一次“轻量 DB 状态刷新”（不弹窗；后台线程检测，避免 UI 卡顿）
            TriggerDatabaseStatusRefresh();
        }

        private void StartClock()
        {
            _clockTimer = new Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += (s, e) =>
            {
                if (_timeLabel != null)
                {
                    _timeLabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            };
            _clockTimer.Start();
        }

        private void TriggerDatabaseStatusRefresh()
        {
            Task.Run(new Action(() =>
            {
                bool ok = false;
                try
                {
                    ok = new MES.BLL.SystemManagement.DatabaseDiagnosticBLL().TestConnection();
                }
                catch
                {
                    ok = false;
                }

                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (_dbStatusLabel == null) return;
                        _dbStatusLabel.Text = ok ? "数据库：已连接" : "数据库：连接失败";
                        _dbStatusLabel.ForeColor = ok ? LeagueColors.SuccessGreen : LeagueColors.WarningOrange;
                    }));
                }
                catch
                {
                    // ignore
                }
            }));
        }

        private void ShowHome()
        {
            try
            {
                ClearActiveModule();

                if (_pageTitle != null) _pageTitle.Text = "大厅";
                if (_backHomeButton != null) _backHomeButton.Visible = false;
                SelectNav(null);

                if (_homeView != null)
                {
                    _homeView.Visible = true;
                    _homeView.BringToFront();
                }

                if (_moduleHost != null) _moduleHost.Visible = false;

                UpdateTopBarLayout();
                UpdateCardsPanelLayout();
            }
            catch
            {
                // ignore
            }
        }

        private void ShowModule<T>(string title, LolNavButton navButton) where T : Form, new()
        {
            try
            {
                ShowModule(title, new T(), navButton);
            }
            catch (Exception ex)
            {
                LogManager.Error("打开模块失败", ex);
                MessageBox.Show(string.Format("打开模块失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowModule(string title, Form form, LolNavButton navButton)
        {
            if (form == null) return;

            try
            {
                if (navButton != null)
                {
                    SelectNav(navButton);
                }

                if (_pageTitle != null) _pageTitle.Text = string.IsNullOrWhiteSpace(title) ? "模块" : title;
                if (_backHomeButton != null) _backHomeButton.Visible = true;

                if (_homeView != null) _homeView.Visible = false;
                if (_moduleHost != null)
                {
                    _moduleHost.Visible = true;
                    _moduleHost.BringToFront();
                }

                ClearActiveModule();

                // 将旧窗体“嵌入”到主界面内容区：更像 LoL 客户端（无多窗口乱飞）
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;

                try
                {
                    UIThemeManager.ApplyTheme(form);
                }
                catch
                {
                    // ignore
                }

                if (_moduleHost != null)
                {
                    var frame = CreateModuleFrame();
                    _moduleHost.Controls.Add(frame);
                    frame.Controls.Add(form);
                }

                _activeModuleForm = form;

                 form.Show();

                 // 再应用一次主题：部分 WinForms 控件（尤其 DataGridView）在 Handle 创建后可能重置样式
                 try
                 {
                     UIThemeManager.ApplyTheme(form);
                 }
                 catch
                 {
                     // ignore
                 }

                 UpdateTopBarLayout();
             }
             catch (Exception ex)
             {
                LogManager.Error("打开模块失败", ex);
                MessageBox.Show(string.Format("打开模块失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // 失败时回到大厅，避免卡死在空白页
                ShowHome();
            }
        }

        private Panel CreateModuleFrame()
        {
            var frame = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = LeagueColors.DarkSurface,
                ForeColor = LeagueColors.TextPrimary,
                Padding = new Padding(14),
                Margin = new Padding(0)
            };
            EnableDoubleBuffering(frame);

            frame.Paint += (s, e) =>
            {
                try
                {
                    var rect = frame.ClientRectangle;
                    rect.Width -= 1;
                    rect.Height -= 1;

                    using (var pen = new Pen(Color.FromArgb(110, LeagueColors.RiotBorderGold), 1))
                    {
                        e.Graphics.DrawRectangle(pen, rect);
                    }

                    // 内描边（更克制）
                    rect.Inflate(-2, -2);
                    using (var pen2 = new Pen(Color.FromArgb(40, LeagueColors.RiotGoldHover), 1))
                    {
                        e.Graphics.DrawRectangle(pen2, rect);
                    }
                }
                catch
                {
                    // ignore
                }
            };

            return frame;
        }

        private void ClearActiveModule()
        {
            try
            {
                if (_activeModuleForm != null)
                {
                    try { _activeModuleForm.Close(); } catch { }
                    try { _activeModuleForm.Dispose(); } catch { }
                    _activeModuleForm = null;
                }

                if (_moduleHost != null)
                {
                    try { _moduleHost.Controls.Clear(); } catch { }
                }
            }
            catch
            {
                // ignore
            }
        }

        private void UpdateTopBarLayout()
        {
            try
            {
                if (_topBar == null || _pageTitle == null || _backHomeButton == null) return;

                var left = _topBar.Padding.Left;
                var top = _topBar.Padding.Top;
                var innerHeight = Math.Max(1, _topBar.Height - _topBar.Padding.Top - _topBar.Padding.Bottom);

                _backHomeButton.Location = new Point(left, top);
                _backHomeButton.Height = innerHeight;

                _pageTitle.Top = top;
                _pageTitle.Height = innerHeight;
                _pageTitle.Left = _backHomeButton.Visible ? (_backHomeButton.Right + 12) : left;
                _pageTitle.Width = Math.Max(1, _topBar.ClientSize.Width - _topBar.Padding.Right - _pageTitle.Left);
            }
            catch
            {
                // ignore
            }
        }

        private void OpenSingletonForm<T>() where T : Form, new()
        {
            try
            {
                foreach (Form open in Application.OpenForms)
                {
                    if (open is T)
                    {
                        try
                        {
                            UIThemeManager.ApplyTheme(open);
                        }
                        catch
                        {
                            // ignore
                        }
                        if (open.WindowState == FormWindowState.Minimized)
                        {
                            open.WindowState = FormWindowState.Normal;
                        }
                        open.BringToFront();
                        open.Activate();
                        return;
                    }
                }

                var form = new T();
                try
                {
                    // 确保新开的窗体立即套用全局主题（避免用户误判“主题没生效”）
                    UIThemeManager.ApplyTheme(form);
                }
                catch
                {
                    // ignore
                }
                form.Show(this);
            }
            catch (Exception ex)
            {
                LogManager.Error("打开窗体失败", ex);
                MessageBox.Show(string.Format("打开窗体失败：{0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;
            try
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }
            catch
            {
                // ignore
            }
        }

        private void MainFormLolV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_clockTimer != null)
                {
                    _clockTimer.Stop();
                    _clockTimer.Dispose();
                    _clockTimer = null;
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                if (_lobbyBackgroundSwapTimer != null)
                {
                    _lobbyBackgroundSwapTimer.Stop();
                    _lobbyBackgroundSwapTimer.Dispose();
                    _lobbyBackgroundSwapTimer = null;
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                if (_lobbyBackgroundFadeTimer != null)
                {
                    _lobbyBackgroundFadeTimer.Stop();
                    _lobbyBackgroundFadeTimer.Dispose();
                    _lobbyBackgroundFadeTimer = null;
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                if (_lobbyBackgroundCurrent != null)
                {
                    _lobbyBackgroundCurrent.Dispose();
                    _lobbyBackgroundCurrent = null;
                }
            }
            catch
            {
                // ignore
            }

            try
            {
                if (_lobbyBackgroundNext != null)
                {
                    _lobbyBackgroundNext.Dispose();
                    _lobbyBackgroundNext = null;
                }
            }
            catch
            {
                // ignore
            }
        }

        private void ResetContentScroll()
        {
            if (_content == null) return;

            try
            {
                // AutoScrollPosition 取值为负数；直接设 (0,0) 可强制复位到左上角
                _content.AutoScrollPosition = new Point(0, 0);
            }
            catch
            {
                // ignore
            }
        }

        private void SafeInvalidateRoot()
        {
            try
            {
                if (_root != null) _root.Invalidate(true);
            }
            catch
            {
                // ignore
            }
        }
    }
}
