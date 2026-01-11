# 模块：MES.Common

## 职责

- 公共基础设施层：配置、异常、通用工具方法等
- 跨层复用能力的唯一归属地（避免散落到 UI/DAL）

## 关键入口

- 连接字符串统一读取：
  - `src/MES.Common/Configuration/ConfigManager.cs`
- 连接字符串兼容与脱敏：
  - `src/MES.Common/Configuration/ConnectionStringHelper.cs`（MaskSecrets / MaskSecretsInText）
- 日志基础设施：
  - `src/MES.Common/Logging/LogManager.cs`（LogDirectory / GetTodayLogFilePath）
- 性能埋点（可渐进接入）：
  - `src/MES.Common/Diagnostics/PerfScope.cs`
- 文本尾部读取（日志尾读）：
  - `src/MES.Common/IO/TextFileTailReader.cs`

## 日志策略

- 文件命名：
  - 活跃文件：`MES_yyyyMMdd.log`
  - 轮转归档：`MES_yyyyMMdd_001.log`、`MES_yyyyMMdd_002.log`...（超过阈值后自动生成）
- 关键配置（`src/MES.UI/App.config` → `appSettings`）：
  - `LogPath`：日志目录（默认 `Logs`）
  - 目录回退：当日志目录不可创建/不可写时，会自动回退到 `%LocalAppData%\\MES-SON\\Logs`
  - `LogLevel`：日志级别（Debug/Info/Warning/Error/Fatal）
  - `LogMaxFileSize`：单文件轮转阈值（例如 `10MB`；为空/0 表示不轮转）
  - `LogMaxFiles`：保留的日志文件数（例如 `30`；0 表示不自动清理）
- 展示与排障：
  - 故障排查中心按 `MES_*.log` 枚举日志，因此轮转归档文件会自动出现在列表中

## 开发约定

- 禁止在 Common 中依赖 UI（WinForms）
- 对外暴露的配置 API 必须避免泄露敏感信息
