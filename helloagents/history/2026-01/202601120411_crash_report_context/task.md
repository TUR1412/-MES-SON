# 任务清单: 崩溃报告附带日志上下文（脱敏）+ 可排障性增强

目录: `helloagents/plan/202601120411_crash_report_context/`

---

## 1. CrashReports（可观测/排障）
- [√] 1.1 在 `src/MES.UI.Framework/Utilities/CrashReporting/CrashReport.cs` 扩展字段：增加“今日日志尾部（脱敏）”等上下文字段
- [√] 1.2 在 `src/MES.UI.Framework/Utilities/CrashReporting/CrashReportWriter.cs` 写入崩溃报告时附带：
  - 今日日志 `MES_yyyyMMdd.log` 的尾部片段（限制行数/字节，避免超大文件）
  - 统一通过 `MES.Common.Configuration.ConnectionStringHelper.MaskSecretsInText` 对日志片段/环境信息做脱敏
- [√] 1.3 自检：不影响现有错误边界弹窗行为；崩溃报告仍写入 `Logs/CrashReports/`，并保持 UTF-8 输出
- [√] 1.4 Support Bundle 导出增强：复制/尾部片段默认脱敏（避免误携带敏感字段）

## 2. 单元测试
- [√] 2.1 为崩溃报告“脱敏 + 日志尾部”行为补齐最小单测（可使用反射调用 internal 方法，避免修改可见性）

## 3. 文档与知识库（SSOT）
- [√] 3.1 更新 `README.md`（中英同步）：补齐 CrashReports 现在包含“脱敏日志尾部上下文”的说明，并避免触发 CI secret guard
- [√] 3.2 更新 `helloagents/wiki/modules/MES.UI.Framework.md`
- [√] 3.3 更新 `helloagents/wiki/modules/MES.Common.md`
- [√] 3.4 更新 `helloagents/wiki/modules/MES.UI.md`
- [√] 3.5 更新 `helloagents/CHANGELOG.md`

## 4. 测试
- [√] 4.1 运行 `./test.ps1 -Configuration Debug` 并确保通过
