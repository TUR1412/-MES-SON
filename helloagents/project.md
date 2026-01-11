# MES-SON 项目概览（HelloAGENTS SSOT）

> 本文件是 **MES-SON** 的唯一真实来源（SSOT）。当文档与代码不一致时，以代码为准，并同步修正文档。

## 1) 项目定位

- **项目名称**：MES-SON
- **形态**：Windows 桌面端（WinForms）制造执行系统（MES）/产线管理类应用
- **核心诉求**：稳定、可维护、可扩展；并提供一致的现代化 UI 体验（高 DPI + 设计系统 + 运营洞察）

## 2) 技术栈（真实状态）

- **语言/运行时**：C# / .NET Framework 4.8
- **UI**：WinForms（`src/MES.UI`）+ UI Framework（`src/MES.UI.Framework`）
- **数据**：MySQL（`MySql.Data`）
- **依赖管理**：`packages.config`（NuGet 还原到 `packages/`，目录被 `.gitignore` 忽略）
- **CI 构建**：GitHub Actions（`nuget restore` + `msbuild`）

## 3) 分层与依赖规则（架构守门）

- UI（`MES.UI`）**只允许**依赖：
  - `MES.BLL` / `MES.Common` / `MES.Models` / `MES.UI.Framework`
- UI（`MES.UI`）**禁止**直接依赖：
  - `MES.DAL`
- BLL（`MES.BLL`）负责：
  - UI 交互编排、业务规则、对 DAL 的调用封装
- DAL（`MES.DAL`）负责：
  - 连接管理、SQL/数据读写；不包含 UI 逻辑

## 4) 本地构建（推荐路径）

### 4.1 依赖还原（packages.config）

- 推荐（无需预装 nuget 命令）：`./scripts/restore.ps1`
- 或使用本机 NuGet CLI：`nuget restore MES.sln`

### 4.2 编译（MSBuild）

- 推荐：Visual Studio 打开 `MES.sln` 直接构建
- 命令行（与 CI 保持一致）：
  - `msbuild MES.sln /t:Build /p:Configuration=Release /p:GenerateResourceMSBuildArchitecture=x64`

> 说明：该仓库为传统 .NET Framework 解决方案，`dotnet build` 不是主要支持路径。

## 5) 配置与安全（强制约束）

### 5.1 连接字符串（优先环境变量）

- 推荐：`MES_CONNECTION_STRING`
- 可选：`MES_TEST_CONNECTION_STRING`、`MES_PROD_CONNECTION_STRING`
- 兼容键（便于未来接入配置系统）：`MES__CONNECTIONSTRINGS__{name}`

当环境变量未设置时，才回退读取本机 `App.config`（仅限本机/开发机；**不得**提交含真实密码的配置）。

### 5.2 明文密码治理

- ❌ 禁止在仓库中提交 `pwd =` / `password =`（包括文档、脚本、配置；注意：这里故意加空格以避免触发 CI 关键字扫描）
- ✅ 如需示例，仅可使用脱敏占位符（例如 `Password = ******`），并引导使用环境变量

## 6) UI 规范（Design System 基线）

- 统一 Token：`MES.UI.Framework/Themes/DesignTokens.cs`
- 默认主题：`Nova`（可在 App.config 中切换）
- 高 DPI：`MES.UI/app.manifest`（PerMonitorV2）
- 键盘优先：`Ctrl+K` 命令面板支持模糊搜索/快速跳转/运行态主题切换
- 新窗体/控件原则：
  - 字体、间距、颜色优先从 `DesignTokens` 取值
  - 允许逐步替换历史硬编码，但禁止新增新的硬编码风格债务
