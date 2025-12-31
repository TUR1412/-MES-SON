# 模块：MES.Common

## 职责

- 公共基础设施层：配置、异常、通用工具方法等
- 跨层复用能力的唯一归属地（避免散落到 UI/DAL）

## 关键入口

- 连接字符串统一读取：
  - `src/MES.Common/Configuration/ConfigManager.cs`

## 开发约定

- 禁止在 Common 中依赖 UI（WinForms）
- 对外暴露的配置 API 必须避免泄露敏感信息
