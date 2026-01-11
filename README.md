# MES-SON · Nova Command Center

[![Build (.NET Framework 4.8)](https://github.com/TUR1412/-MES-SON/actions/workflows/build.yml/badge.svg)](https://github.com/TUR1412/-MES-SON/actions/workflows/build.yml)

中文：一个基于 **.NET Framework 4.8 + WinForms** 的桌面端 MES 示例工程，强调 **分层清晰**、**可观测诊断** 与 **键盘优先** 的现代化 UI（Nova / LoL 主题）。
English: A **.NET Framework 4.8 + WinForms** MES sample focused on **clean layering**, **diagnostics/observability**, and a **keyboard-first** modern UI (Nova / LoL themes).

- [中文](#中文)
- [English](#english)

---

## 中文

### ✨ 关键特性

- **运营洞察**：生产风险预警、在制品老化、设备健康、库存告警、质量缺陷、批次良率一屏聚合
- **分层架构（守门）**：`UI → BLL → DAL → MySQL`，避免 UI 直连数据库（UI 禁止依赖 DAL）
- **未来感主题（Nova / LoL）**：统一 Design Tokens，卡片化入口与高对比可读性
- **快捷命令面板（Ctrl+K）**：支持模糊匹配/多词搜索，快速跳转模块/工具，支持运行态主题切换
- **数据库诊断（不阻塞 UI）**：后台采集、单连接聚合查询、展示连接占用率；诊断输出默认脱敏连接串
- **错误边界（Error Boundary）**：全局异常边界 + 自动生成崩溃报告（CrashReports），提升线上可排障性
- **日志可观测**：命令面板支持打开日志目录/今日日志/复制今日日志尾部/崩溃报告目录/最新崩溃报告/最新诊断包（zip），便于排障与回溯
- **故障排查中心**：内置日志（尾部读取）与崩溃报告查看/复制，支持文件列表筛选、跟随尾部自动刷新（仅在文件变更时读取）、关键字高亮、文本内搜索（Next/Prev/Aa 区分大小写）与一键定位最新错误，并支持一键导出诊断包（Support Bundle，含 Zip），减少对外部工具依赖
- **工程化脚本**：`scripts/restore.ps1` 自动下载 `nuget.exe` 并还原 `packages.config` 依赖，`build.ps1` 一键构建
- **单元测试**：新增 `tests/MES.UnitTests`，可用 `./test.ps1` 一键构建并运行单测
- **CI（GitHub Actions）**：自动构建 + 运行单元测试（TRX 归档）+ secret guard（阻止误提交敏感字样）

---

### 🧱 目录结构

```text
src/
  MES.UI              # WinForms 客户端（业务窗体）
  MES.UI.Framework    # 设计系统/主题/控件
  MES.BLL             # 业务逻辑层（UI 只调用这里）
  MES.DAL             # 数据访问层（SQL/参数化/连接管理）
  MES.Models          # 领域模型/DTO
  MES.Common          # 配置/日志/通用能力

tests/               # 测试与验证工具
docs/                # 项目文档
database/            # 数据库脚本
helloagents/         # SSOT（架构/模块/变更与方案包）
```

---

### 🚀 快速开始

#### 环境要求

- Windows 10/11
- Visual Studio 2022（建议）或 Build Tools
- .NET Framework 4.8 Developer Pack
- MySQL 8.x（示例默认）

#### 依赖还原与构建（推荐）

无需预装 NuGet CLI：脚本会自动下载 `nuget.exe` 并还原 `packages/`。

```powershell
./scripts/restore.ps1
./build.ps1 -Configuration Release -BuildSolution
```

或使用本机已安装的 NuGet/MSBuild（更贴近 CI）：

```powershell
nuget restore MES.sln
msbuild MES.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU" /p:GenerateResourceMSBuildArchitecture=x64
```

#### 运行

- 运行 `src/MES.UI` 生成的可执行文件
- 默认主题：`Nova`
- 按 `Ctrl+K` 打开命令面板（模糊搜索/快速跳转/主题切换）
- 在命令面板中搜索 `故障排查中心`：查看日志/崩溃报告并一键复制/导出诊断包

#### 运行单元测试

```powershell
./test.ps1 -Configuration Debug
```

如需生成 TRX 结果（便于 CI 归档/排障）：

```powershell
./test.ps1 -Configuration Release -ResultsDirectory TestResults
```

---

### 🔑 数据库连接配置（安全优先）

推荐使用环境变量（避免仓库写入真实密码）：

- `MES_CONNECTION_STRING`（推荐）
- `MES_TEST_CONNECTION_STRING`（可选）
- `MES_PROD_CONNECTION_STRING`（可选）

示例（仅示意，禁止提交真实密码）：

```text
Server=127.0.0.1;Port=3306;Database=mes;User Id=root;Password=******;SslMode=None;
```

说明：

- 当环境变量未设置时，才回退读取 `src/MES.UI/App.config`（仅本机/开发机使用，不提交含真实密码的配置）。
- MySQL 8+ 默认认证可能触发 “Public Key Retrieval is not allowed”，项目已在连接串层做兼容增强（自动补齐 `AllowPublicKeyRetrieval=True`）。

---

### 🧯 错误边界与崩溃报告

- 全局异常边界已启用：捕获 UI 线程异常、应用域异常、未观察到的任务异常
- 发生未处理异常时会写入崩溃报告到日志目录下的 `CrashReports/`
- 可通过命令面板打开日志目录/报告，便于快速定位与复盘

---

### 🧾 日志与保留策略

- 日志文件：`Logs/MES_yyyyMMdd.log`（默认）
- 轮转（可选）：当 `LogMaxFileSize` 设置且文件超过阈值时，会将当日日志归档为 `MES_yyyyMMdd_001.log`、`MES_yyyyMMdd_002.log`... 并继续写入 `MES_yyyyMMdd.log`
- 保留（可选）：当 `LogMaxFiles` 设置为 `N` 时，应用启动会自动清理旧日志，仅保留最近 `N` 个 `MES_*.log` 文件

---

### ⌨️ 快捷键

- 命令面板（Command Palette）
  - `Ctrl+K`：打开命令面板
  - `Enter`：执行选中命令
  - `Esc`：关闭命令面板
  - `↑/↓`：选择命令
- 故障排查中心（Troubleshooting Center）
  - `Ctrl+F`：聚焦当前页搜索框（Logs/CrashReports）
  - `F3` / `Shift+F3`：下一个 / 上一个匹配
  - `Ctrl+G`：跳转到最新错误
  - 搜索框内：`Enter`（Next）、`Shift+Enter`（Prev）、`Esc`（清空搜索文本）

---

### 🧭 架构边界与开发约定

- **UI 层（MES.UI）只允许依赖**：`MES.BLL` / `MES.Common` / `MES.Models` / `MES.UI.Framework`
- **UI 层禁止依赖**：`MES.DAL`（所有数据访问必须经由 BLL 门面）
- 连接字符串与脱敏展示统一走：`MES.Common.Configuration.ConnectionStringHelper`
- 新窗体/新控件样式优先从 `DesignTokens` 获取，避免新增硬编码视觉债务
- 工程多数使用 `LangVersion=5`，请避免引入更高版本 C# 语法（例如 `nameof`、表达式体成员等）

---

### 📚 文档与 SSOT

- `docs/`：项目说明文档
- `helloagents/`：架构/模块/变更与方案包（SSOT；当文档与代码不一致时以代码为准并同步）

---

## English

### ✨ Highlights

- **Operational Insight**: risk alerts, WIP aging, equipment health, inventory alarms, quality defects, batch yield overview
- **Clean Layering (guard rails)**: `UI → BLL → DAL → MySQL` (UI must NOT reference DAL)
- **Modern Themes (Nova / LoL)**: design tokens + card-based layout with high readability
- **Command Palette (`Ctrl+K`)**: fuzzy/multi-token search, fast navigation, runtime theme toggle
- **Database Diagnostics (non-blocking)**: background collection, single-connection aggregation, connection utilization insight; redacted diagnostics by default
- **Error Boundary**: global exception boundary + automatic crash reports (CrashReports) for faster troubleshooting
- **Log Observability**: open log folder / today's log / copy today's log tail / CrashReports folder / latest CrashReport / latest Support Bundle (zip) directly from the command palette
- **Troubleshooting Center**: built-in log tail viewer + crash report viewer/copy, with file list filtering, follow-tail auto refresh (reads only on file changes), keyword highlighting, in-text search (Next/Prev/case toggle), jump-to-latest error, and one-click support bundle export (zip included) to reduce reliance on external tools
- **Engineering Scripts**: `scripts/restore.ps1` downloads `nuget.exe` and restores `packages.config`, `build.ps1` builds the solution
- **Unit Tests**: `tests/MES.UnitTests` with a one-command runner: `./test.ps1`
- **CI (GitHub Actions)**: build + unit tests (TRX artifact) + secret guard (blocks sensitive patterns)

---

### 🧱 Structure

```text
src/
  MES.UI              # WinForms client (forms)
  MES.UI.Framework    # design system / themes / controls
  MES.BLL             # business logic layer (UI calls this only)
  MES.DAL             # data access layer (SQL/connection management)
  MES.Models          # domain models / DTOs
  MES.Common          # config / logging / shared utilities

tests/               # test & verification tools
docs/                # documentation
database/            # database scripts
helloagents/         # SSOT (architecture/modules/changelog/plan packages)
```

---

### 🚀 Getting Started

#### Requirements

- Windows 10/11
- Visual Studio 2022 (recommended) or Build Tools
- .NET Framework 4.8 Developer Pack
- MySQL 8.x (default target)

#### Restore & Build (recommended)

No need to pre-install NuGet CLI. The script downloads `nuget.exe` and restores `packages/`.

```powershell
./scripts/restore.ps1
./build.ps1 -Configuration Release -BuildSolution
```

Or use your installed NuGet/MSBuild (closer to CI):

```powershell
nuget restore MES.sln
msbuild MES.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU" /p:GenerateResourceMSBuildArchitecture=x64
```

#### Run

- Run the executable produced by `src/MES.UI`
- Default theme: `Nova`
- Press `Ctrl+K` to open the Command Palette (fuzzy search / navigation / theme toggle)
- Search `Troubleshooting Center` from the palette to view logs/crash reports and copy/export details

#### Run Unit Tests

```powershell
./test.ps1 -Configuration Debug
```

To generate TRX results (useful for CI archiving/troubleshooting):

```powershell
./test.ps1 -Configuration Release -ResultsDirectory TestResults
```

---

### 🔑 Database Configuration (security first)

Use environment variables to avoid committing secrets:

- `MES_CONNECTION_STRING` (recommended)
- `MES_TEST_CONNECTION_STRING` (optional)
- `MES_PROD_CONNECTION_STRING` (optional)

Example (placeholder only; never commit real passwords):

```text
Server=127.0.0.1;Port=3306;Database=mes;User Id=root;Password=******;SslMode=None;
```

Notes:

- If env vars are not set, the app falls back to `src/MES.UI/App.config` (local/dev only; never commit real secrets).
- MySQL 8+ auth may trigger “Public Key Retrieval is not allowed”; the project includes a compatibility guard that auto-adds `AllowPublicKeyRetrieval=True`.

---

### 🧯 Error Boundary & Crash Reports

- A global exception boundary is enabled (UI thread, AppDomain, and unobserved task exceptions)
- Crash reports are written under `CrashReports/` inside the log directory
- Use the command palette to open the log folder / today's log for troubleshooting

---

### 🧾 Logging & Retention

- Log files: `Logs/MES_yyyyMMdd.log` (default)
- Rotation (optional): when `LogMaxFileSize` is set and the active file exceeds it, the current-day log is archived as `MES_yyyyMMdd_001.log`, `MES_yyyyMMdd_002.log`, ... and writing continues to `MES_yyyyMMdd.log`
- Retention (optional): when `LogMaxFiles` is set to `N`, the app cleans up old logs on startup and keeps the newest `N` `MES_*.log` files

---

### ⌨️ Keyboard Shortcuts

- Command Palette
  - `Ctrl+K`: open Command Palette
  - `Enter`: run selected command
  - `Esc`: close palette
  - `↑/↓`: navigate commands
- Troubleshooting Center
  - `Ctrl+F`: focus search box in current tab (Logs/CrashReports)
  - `F3` / `Shift+F3`: next / previous match
  - `Ctrl+G`: jump to latest issue
  - In the search box: `Enter` (Next), `Shift+Enter` (Prev), `Esc` (Clear)

---

### 🧭 Architecture Rules

- UI (`MES.UI`) may depend on: `MES.BLL` / `MES.Common` / `MES.Models` / `MES.UI.Framework`
- UI (`MES.UI`) must NOT depend on: `MES.DAL` (all DB access goes through BLL facades)
- Connection string handling / redaction is centralized in `MES.Common.Configuration.ConnectionStringHelper`
- For UI styling, prefer `DesignTokens` and avoid new hard-coded visual debt
- Many projects compile with `LangVersion=5`; avoid newer C# language features (e.g. `nameof`, expression-bodied members)

---

### 📚 Docs / SSOT

- `docs/`: documentation
- `helloagents/`: architecture/modules/changelog/plan packages (SSOT; code is the source of truth)

