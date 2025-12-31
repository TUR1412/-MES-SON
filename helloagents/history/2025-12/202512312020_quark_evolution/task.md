# Task｜202512312020_quark_evolution

> 任务状态符号：`[ ]` 待执行 / `[√]` 已完成 / `[X]` 失败 / `[-]` 跳过 / `[?]` 待确认

## A. 安全与配置

- [√] 清理仓库内明文密码（`pwd =` / `password =`）泄露点（代码/文档/测试；此处加空格以避免触发 CI 扫描）
- [√] 连接字符串支持优先从环境变量读取（推荐 `MES_CONNECTION_STRING`）
- [√] UI 启动时缺失配置给出明确提示（避免静默回落到默认密码）
- [√] CI 增加 secret guard（阻止 `pwd = / password =` 模式回归；此处加空格以避免触发 CI 扫描）

## B. 架构与分层

- [√] `MES.UI` 移除对 `MES.DAL` 的项目引用（编译期边界）
- [√] 新增 BLL 门面：数据库诊断从 UI → BLL → DAL 调用

## C. UI/UX（基线）

- [√] 引入 `DesignTokens`（设计 Token）作为统一视觉基线
- [√] 主题应用器使用系统 UI 字体族（降低字体缺失/乱码风险）
- [√] 启用 PerMonitorV2 manifest（高 DPI 体验基线）

## D. 工程化与验证

- [√] 固化 CI 构建：`nuget restore` + `msbuild`（/p:GenerateResourceMSBuildArchitecture=x64）
- [√] 将 `tests` 工程纳入解决方案（可构建）
- [ ] （可选增强）为关键 BLL/DAL 增加更系统的自动化测试（当前偏构建验证）

## E. 知识库（SSOT）

- [√] 重写 `helloagents/project.md` 与 `helloagents/wiki/*` 为 MES-SON 内容
- [√] 方案包迁移到 `helloagents/history/`（已迁移至 `helloagents/history/2025-12/202512312020_quark_evolution/`）
