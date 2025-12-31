# Why｜Quark-Level Evolutionary Refactoring

## 背景问题（痛点）

1. **安全风险**：仓库内出现过包含数据库密码的连接字符串（代码/文档/测试），存在泄露与误用风险。
2. **架构耦合**：UI 直接依赖 DAL，导致界面层与数据访问层高度绑定，难以测试、难以替换、难以演进。
3. **UI 体验债务**：字体/配色/间距大量硬编码，高 DPI 场景容易出现模糊、错位、不一致的问题。
4. **工程链路脆弱**：传统 .NET Framework + packages.config 组合对构建方式敏感，需要固化可重复的构建策略。
5. **文档不可用**：关键配置与安全约束缺少“可执行”的说明，导致新同学上手成本高、踩坑概率大。

## 本次目标（Definition of Done）

- 彻底消除明文密码在仓库中的泄露点，并建立自动化门禁阻止回归
- 明确分层边界：UI → BLL → DAL → DB，UI 不再依赖 DAL
- 引入设计系统基座（Design Tokens）与高 DPI 基线（PerMonitorV2）
- 固化 CI 构建路径，使其在 GitHub Actions 上可重复构建
- 将知识库（`helloagents/`）校正为 MES-SON 的 SSOT
