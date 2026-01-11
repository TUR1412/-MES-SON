# 任务清单: 跟随尾部刷新优化与排障中心快捷键补齐

目录: `helloagents/plan/202601120310_follow_tail_opt/`

---

## 1. 故障排查中心（Troubleshooting Center）
- [√] 1.1 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 优化“跟随尾部”刷新：仅当文件发生变更（LastWriteTime/Length）或尾部行数变化时才触发读取
- [√] 1.2 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 补齐快捷键：`Ctrl+F` 聚焦搜索框，`F3/Shift+F3` Next/Prev，`Ctrl+G` 跳转到最新错误（基于当前页）
- [√] 1.3 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 在元信息栏显示跟随状态与刷新间隔（不影响原文件信息展示）

## 2. 安全检查
- [√] 2.1 自检：不新增敏感信息字样与明文连接串；避免触发 CI secret guard 规则

## 3. 文档更新（SSOT）
- [√] 3.1 更新 `README.md`（中英同步）补齐新增能力说明
- [√] 3.2 更新 `helloagents/wiki/modules/MES.UI.md`
- [√] 3.3 更新 `helloagents/CHANGELOG.md`

## 4. 测试
- [√] 4.1 运行 `./test.ps1 -Configuration Debug` 并确保通过
