# MES-SON 概览（SSOT）

MES-SON 是一个基于 **.NET Framework 4.8 + WinForms** 的桌面端 MES/产线管理类系统。项目采用经典分层（UI/BLL/DAL）与 UI Framework（设计系统）并行演进：既保持传统 WinForms 的稳定交付能力，也为“现代化一致体验（高 DPI + 统一 Token）”打下结构性基础。
本次升级引入运营洞察与 Nova 主题，聚合多模块指标并提供风险预警入口。

## 快速开始（开发者）

1) 配置连接字符串（强烈推荐：环境变量）
- `MES_CONNECTION_STRING`

2) 还原 NuGet 依赖（`packages.config`）
- `nuget restore MES.sln`

3) 构建
- Visual Studio 打开 `MES.sln` 直接编译，或：
- `msbuild MES.sln /t:Build /p:Configuration=Release /p:GenerateResourceMSBuildArchitecture=x64`

4) 运行
- 启动 `src/MES.UI` 生成的 WinForms 可执行文件

## 关键约束（必须遵守）

- **安全第一**：仓库内禁止出现明文数据库密码（含文档/脚本/配置），仅允许脱敏示例
- **分层守门**：`MES.UI` 禁止直接依赖 `MES.DAL`，所有数据访问通过 `MES.BLL` 编排
- **视觉一致性**：新增/改造界面优先使用 `MES.UI.Framework` 的 `DesignTokens`
- **高 DPI**：以 `MES.UI/app.manifest`（PerMonitorV2）为基线，避免“糊、虚、错位”

## UI 功能模块（以源码目录为准）

- 批次：`src/MES.UI/Forms/Batch/`
- 物料：`src/MES.UI/Forms/Material/`
- 生产：`src/MES.UI/Forms/Production/`
- 工单：`src/MES.UI/Forms/WorkOrder/`
- 车间：`src/MES.UI/Forms/Workshop/`
- 运营洞察：`src/MES.UI/Forms/Insight/`
- 系统管理：`src/MES.UI/Forms/SystemManagement/`（含数据库诊断）

## 知识库导航

- 架构：`helloagents/wiki/arch.md`
- 数据与连接：`helloagents/wiki/data.md`
- 内部 API / 约定：`helloagents/wiki/api.md`
- 模块说明：`helloagents/wiki/modules/`
