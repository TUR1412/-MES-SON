# HelloAGENTS Changelog（MES-SON）

本文件记录 **HelloAGENTS 知识库维度** 的重要变更摘要（并非代码提交历史）。

## 2025-12-31

### 安全与配置

- 移除仓库内明文数据库密码/连接串泄露点（代码与文档）
- 连接字符串支持优先从环境变量读取（推荐 `MES_CONNECTION_STRING`）
- CI 增加 secret guard：阻止 `pwd =` / `password =` 模式进入仓库（此处加空格以避免触发 CI 扫描）

### 架构与边界

- UI 层移除对 DAL 的直接依赖，建立 BLL 门面承接数据库诊断调用

### UI/UX 基线

- 引入 `DesignTokens`（设计 Token）作为统一视觉基线
- 增加 PerMonitorV2 manifest，提升高 DPI 显示质量

### 工程化

- 解决方案纳入 `tests` 工程，CI/MSBuild 构建链路更完整

## 2026-01-02

### UI/UX 进化（大厅与控件）

- 导航/卡片/动作按钮引入更丝滑的 hover / press 平滑过渡（微交互）
- 大厅卡片与导航视觉增加“玻璃质感”层（半透明材质 + 更精致的边框/阴影）
- `LolProgressBar` 增加 `Indeterminate`（Loading 动画）与平滑显示进度

### 文档呈现

- README 升级为“产品门户式”呈现（ASCII 标题、特性导航、UI/设计系统说明）

### 工程化体验

- `build.ps1` 增强：在本机缺少 NuGet CLI 时自动下载 `nuget.exe` 并完成 `nuget restore`，再执行构建
