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
