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
- 数据库诊断：
  - `src/MES.UI/Forms/SystemManagement/DatabaseDiagnosticForm.cs`

## 依赖关系（守门）

- ✅ 允许：`MES.BLL` / `MES.Common` / `MES.Models` / `MES.UI.Framework`
- ❌ 禁止：`MES.DAL`（所有数据访问通过 BLL）

## UI 约定

- 高 DPI：`src/MES.UI/app.manifest`（PerMonitorV2）
- 字体/间距/配色优先从 `DesignTokens` 获取
