# 任务清单: SystemHealthChecks OCP 化（Probe 模型）+ 单测补齐

目录: `helloagents/plan/202601120645_health_checks_ocp/`

## 1. OCP 重构（不破坏既有调用）
- [√] 1.1 `SystemHealthChecks` 增加 Probe 扩展点（保持 `Collect(options)` 兼容）
- [√] 1.2 默认健康检查改为按 Probe 顺序执行（行为与输出保持一致）

## 2. 单元测试
- [√] 2.1 `tests/MES.UnitTests` 引用 `MES.UI` 并新增 `SystemHealthChecksTests`
- [√] 2.2 覆盖：RenderText 基本输出、Collect 基本可用性、additional probes 可扩展性

## 3. 文档与知识库（SSOT）
- [√] 3.1 更新 `helloagents/wiki/modules/MES.UI.md`：补充健康检查 OCP 扩展点说明
- [√] 3.2 更新 `helloagents/CHANGELOG.md`

## 4. 测试
- [√] 4.1 运行 `./test.ps1 -Configuration Debug` 并确保通过
