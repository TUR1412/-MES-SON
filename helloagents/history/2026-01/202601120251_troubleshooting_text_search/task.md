# 任务清单: 故障排查中心可用性增强（文本内搜索/定位最新错误/快捷复制尾部）

目录: `helloagents/plan/202601120251_troubleshooting_text_search/`

---

## 1. 故障排查中心（Troubleshooting Center）
- [√] 1.1 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 增加“文本内搜索”（支持 Next/Prev、区分大小写开关、结果计数；不阻塞 UI）
- [√] 1.2 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 增加“定位最新错误/警告”快捷按钮（在当前文本中向上查找 ERROR/WARN/Exception）
- [√] 1.3 在 `src/MES.UI/Forms/SystemManagement/TroubleshootingCenterForm.cs` 增加“一键复制尾部内容”（默认复制当前加载的尾部文本即可）

## 2. 命令面板（Ctrl+K）
- [√] 2.1 在 `src/MES.UI/Forms/MainFormLolV2.cs` 增加快捷命令：复制今日日志尾部到剪贴板（避免读完整文件，复用 `TextFileTailReader`）

## 3. 安全检查
- [√] 3.1 自检：不新增敏感信息字样与明文连接串；避免触发 CI secret guard 规则

## 4. 文档更新（SSOT）
- [√] 4.1 更新 `README.md`（中英同步）补齐新增能力说明
- [√] 4.2 更新 `helloagents/wiki/modules/MES.UI.md`
- [√] 4.3 更新 `helloagents/CHANGELOG.md`

## 5. 测试
- [√] 5.1 运行 `./test.ps1 -Configuration Debug` 并确保通过
