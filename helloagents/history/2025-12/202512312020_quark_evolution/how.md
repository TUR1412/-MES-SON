# How｜Quark-Level Evolutionary Refactoring

## 执行策略

### 1) 安全先行（Secret & Config）

- 扫描并移除仓库内 `pwd =` / `password =` 明文痕迹（代码/文档/测试；此处加空格以避免触发 CI 扫描）
- 连接字符串改为优先从环境变量读取，配置缺失时给出可执行指引
- CI 增加 secret guard（阻止敏感模式进入仓库）

### 2) 架构边界收敛（UI 禁止直连 DAL）

- `MES.UI` 移除对 `MES.DAL` 的项目引用
- 将 UI 中少量 DAL 直连点迁移为 BLL 门面调用
- 在 BLL 新增小而清晰的接口与实现（可逐步扩展）

### 3) UI/UX 基线建设（设计系统 + 高 DPI）

- 在 `MES.UI.Framework` 增加 `DesignTokens`，作为统一字体/颜色/间距/圆角的来源
- 主题应用器改造：使用系统 UI 字体族，降低字体缺失与乱码风险
- WinForms 应用启用 PerMonitorV2 manifest，改善高 DPI 清晰度与布局稳定性

### 4) 工程化与可验证性

- CI 固化为 `nuget restore` + `msbuild`
- 将 `tests` 工程纳入解决方案，至少保证基础验证工程可构建

### 5) 文档与知识库（SSOT）

- 重写/校正 `helloagents/` 知识库内容，确保与 MES-SON 一致
