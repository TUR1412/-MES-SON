# 任务清单: 日志目录可写性回退 + 系统健康检查（命令面板入口）

目录: `helloagents/plan/202601120547_health_check_logpath/`

---

## 1. 日志基础设施（稳定性）
- [√] 1.1 增强 `LogManager.Initialize()`：当默认/配置日志目录不可创建或不可写时，自动回退到 LocalAppData（避免安装目录写入失败）
- [√] 1.2 DAL 侧连接串兼容逻辑去重：`DatabaseHelper` 复用 `ConnectionStringHelper.EnsureAllowPublicKeyRetrieval`

## 2. 系统健康检查（UI）
- [√] 2.1 新增“系统健康检查”窗体：展示日志目录/CrashReports/磁盘空间/数据库连通性等关键项
- [√] 2.2 命令面板新增入口（Ctrl+K）：支持一键打开健康检查

## 3. 文档与知识库（SSOT）
- [√] 3.1 更新 `README.md`（中英同步）：说明日志目录回退策略与健康检查入口
- [√] 3.2 更新 `helloagents/wiki/modules/MES.Common.md` / `helloagents/wiki/modules/MES.UI.md` / `helloagents/wiki/modules/MES.DAL.md`
- [√] 3.3 更新 `helloagents/CHANGELOG.md`

## 4. 测试
- [√] 4.1 运行 `./test.ps1 -Configuration Debug` 并确保通过
