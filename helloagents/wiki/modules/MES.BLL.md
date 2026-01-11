# 模块：MES.BLL

## 职责

- 业务逻辑层（Business Logic Layer）
- 承接 UI 事件与业务流程编排
- 对 DAL 做 “可替换的门面封装”，避免 UI 直接依赖 DAL

## 依赖关系

- 允许引用：`MES.DAL`、`MES.Common`、`MES.Models`
- 禁止引用：`MES.UI`、`System.Windows.Forms`

## 关键入口

- 数据库诊断门面（供 UI 调用）：
  - `src/MES.BLL/SystemManagement/IDatabaseDiagnosticBLL.cs`
  - `src/MES.BLL/SystemManagement/DatabaseDiagnosticBLL.cs`
- 运营洞察聚合服务：
  - `src/MES.BLL/Analytics/IInsightBLL.cs`
  - `src/MES.BLL/Analytics/InsightBLL.cs`

## 开发约定

- UI 新增功能优先先落到 BLL 接口，再由 Form 调用接口
- 错误信息不要携带完整连接字符串；如必须输出，务必脱敏
- 洞察类需求统一聚合到 Analytics 子目录，避免散落在 UI
