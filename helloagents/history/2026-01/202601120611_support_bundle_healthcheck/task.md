# 任务清单: Support Bundle 集成系统健康检查 + 健康检查逻辑复用

目录: `helloagents/plan/202601120611_support_bundle_healthcheck/`

---

## 1. 健康检查能力抽取（OCP）
- [√] 1.1 新增 `SystemHealthChecks`（可复用的健康检查采集器）：UI 与 Support Bundle 共用
- [√] 1.2 `SystemHealthCheckForm` 复用采集器（避免逻辑分叉）

## 2. Support Bundle 集成
- [√] 2.1 导出 Support Bundle 时额外生成 `health_check.txt`（默认脱敏、仅摘要）

## 3. 文档与知识库（SSOT）
- [√] 3.1 更新 `README.md`（中英同步）：说明 Support Bundle 包含 health_check.txt
- [√] 3.2 更新 `helloagents/wiki/modules/MES.UI.md`
- [√] 3.3 更新 `helloagents/CHANGELOG.md`

## 4. 测试
- [√] 4.1 运行 `./test.ps1 -Configuration Debug` 并确保通过
