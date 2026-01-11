# 任务清单: 故障排查中心交互增强（筛选/跟随/高亮）与命令面板快捷入口

目录: `helloagents/plan/202601120224_troubleshooting_center_polish/`

---

## 1. 故障排查中心（Troubleshooting Center）
- [√] 1.1 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 增加日志/崩溃报告文件列表筛选框（实时过滤，不破坏原列表加载逻辑）
- [√] 1.2 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 增加“跟随尾部/自动刷新”开关（避免 UI 线程阻塞；复用既有异步尾读机制）
- [√] 1.3 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 增加关键字高亮（ERROR/WARN/Exception/[perf]），并保证关闭高亮时恢复默认颜色

## 2. 命令面板（Ctrl+K）
- [√] 2.1 在 `src/MES.UI/Forms/MainFormLolV2.cs` 增加快捷命令：打开崩溃报告目录 / 打开最新崩溃报告 / 打开最新诊断包（zip）

## 3. 安全检查
- [√] 3.1 自检：避免引入敏感信息字样（例如 `password=`/`pwd=`），不新增明文连接串

## 4. 文档更新（SSOT）
- [√] 4.1 更新 `README.md`（中英同步）补齐新增能力说明
- [√] 4.2 更新 `helloagents/wiki/modules/MES.UI.md` 与 `helloagents/wiki/modules/MES.UI.Framework.md`
- [√] 4.3 更新 `helloagents/CHANGELOG.md`（知识库维度变更摘要）

## 5. 测试
- [√] 5.1 运行 `./test.ps1 -Configuration Debug` 并确保通过

