# 模块：MES.UI

## 职责

- WinForms 应用入口与界面实现
- 负责交互与展示，不承载数据访问细节

## 关键入口

- 应用启动：
  - `src/MES.UI/Program.cs`
- 主窗体：
  - `src/MES.UI/Forms/MainForm.cs`
  - `src/MES.UI/Forms/MainFormLolV2.cs`
- 快捷命令面板（Ctrl+K）：
  - `src/MES.UI/Forms/Common/CommandPaletteForm.cs`
- 运营洞察：
  - `src/MES.UI/Forms/Insight/OperationsInsightForm.cs`
- 数据库诊断：
  - `src/MES.UI/Forms/SystemManagement/DatabaseDiagnosticForm.cs`
- 系统配置：
  - `src/MES.UI/Forms/SystemManagement/SystemConfigForm.cs`
- 系统健康检查：
  - `src/MES.UI/Forms/SystemManagement/SystemHealthCheckForm.cs`
- 系统健康检查采集器（可复用）：
  - `src/MES.UI/Forms/SystemManagement/SystemHealthChecks.cs`
- 故障排查中心：
  - `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs`      

## 依赖关系（守门）

- ✅ 允许：`MES.BLL` / `MES.Common` / `MES.Models` / `MES.UI.Framework`
- ❌ 禁止：`MES.DAL`（所有数据访问通过 BLL）

## UI 约定

- 高 DPI：`src/MES.UI/app.manifest`（PerMonitorV2）
- 字体/间距/配色优先从 `DesignTokens` 获取
- 默认主题：`Nova`（可通过配置切换）
- 交互入口：`Ctrl+K` 打开命令面板（模糊匹配/多词搜索/跳转模块与工具/主题切换）  
- 观测与排障：命令面板支持“故障排查中心”（日志/CrashReports 尾部查看、筛选、跟随尾部自动刷新（仅文件变更才读盘）、关键字高亮、文本内搜索（Next/Prev/Aa）、一键定位最新错误、一键复制/导出诊断包），并提供打开日志目录/今日日志/复制今日日志尾部/崩溃报告目录/最新崩溃报告/最新诊断包（zip）入口；全局异常边界生成 CrashReports（日志目录下）       
- 命令面板增强：新增“复制健康检查摘要”（快速/不测数据库）快捷入口，便于快速拷贝排障信息
- 诊断包导出：Support Bundle 导出时默认对日志/崩溃报告复制与尾部片段做脱敏，降低信息泄露风险；同时生成 `health_check.txt`（系统健康检查摘要）
- 系统健康检查：由 `SystemHealthChecks` 采集（OCP：`CollectWithProbes` + `IHealthCheckProbe` 可扩展），命令面板一键运行，覆盖日志目录/CrashReports/磁盘空间/数据库连通性（默认脱敏）
- 故障排查中心快捷键：`Ctrl+F` 聚焦搜索框；`F3/Shift+F3` 下一个/上一个匹配；`Ctrl+G` 跳转到最新错误
- 数据库诊断：后台采集、单连接聚合，避免在 UI 线程进行多次 Open/查询造成卡顿；提供连接占用率提示
