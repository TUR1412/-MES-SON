# 任务清单: 日志轮转/保留策略对齐 + 仓库备份文件清理

目录: `helloagents/plan/202601120344_log_hygiene/`

---

## 1. 日志能力对齐（MES.Common）
- [√] 1.1 在 `src/MES.Common/Logging/LogManager.cs` 支持 `LogMaxFileSize`（示例：`10MB`）并在超过阈值时对当日日志进行安全轮转（保留 `MES_yyyyMMdd.log` 作为当前活跃文件）
- [√] 1.2 在 `src/MES.Common/Logging/LogManager.cs` 支持按 `LogMaxFiles`（示例：`30`）进行日志保留清理（按 LastWriteTimeUtc 排序，保留最近 N 个 `MES_*.log` 文件）
- [√] 1.3 自检：日志轮转/清理逻辑不影响现有 `GetTodayLogFilePath()` 与故障排查中心的日志列表加载规则（`MES_*.log`）

## 2. 仓库清理（Repository Hygiene）
- [√] 2.1 删除仓库中不应提交的备份文件：
  - `src/MES.BLL/Material/ProcessRouteBLL.cs.backup`
  - `src/MES.BLL/Workshop/WIPBLL.cs.backup`
  - `src/MES.UI/MES.UI.csproj.backup`
- [√] 2.2 更新 `.gitignore`：忽略 `*.backup`（避免再次进入版本控制）

## 3. 文档与知识库（SSOT）
- [√] 3.1 更新 `README.md`（中英同步）：补齐日志轮转/保留策略的配置说明（不写入任何敏感信息示例）
- [√] 3.2 更新 `helloagents/wiki/modules/MES.Common.md`（记录日志策略与配置键）
- [√] 3.3 更新 `helloagents/CHANGELOG.md`

## 4. 测试
- [√] 4.1 运行 `./test.ps1 -Configuration Debug` 并确保通过
