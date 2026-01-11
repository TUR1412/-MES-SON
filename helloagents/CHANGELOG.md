# HelloAGENTS Changelog（MES-SON）

本文件记录 **HelloAGENTS 知识库维度** 的重要变更摘要（并非代码提交历史）。

## 2026-01-12
### UI/UX
- 故障排查中心增强：新增文件列表筛选、跟随尾部自动刷新、关键字高亮（ERROR/WARN/Exception/[perf]）
- 故障排查中心增强：新增文本内搜索（Next/Prev/Aa 区分大小写）与一键定位最新错误
- 命令面板增强：新增“打开崩溃报告目录/打开最新崩溃报告/打开最新诊断包（zip）”快捷入口
- 命令面板增强：新增“复制今日日志尾部到剪贴板”快捷入口

## 2026-01-11
### 运营洞察
- 新增运营洞察聚合服务，涵盖生产风险、在制品老化、设备健康、库存告警、质量缺陷、批次良率

### UI/UX
- 引入 Nova 未来感主题与视觉 Token 升级
- 主界面新增运营洞察入口与指挥中心式布局
- 新增全局命令面板（Ctrl+K）：快速跳转模块/工具，支持运行态主题切换
- 命令面板增强：支持模糊匹配/多词搜索排序，主题切换时面板配色实时跟随
- 系统配置页补齐主题选项，并提供真实数据库联通性测试（异步不阻塞 UI）
- 引入原子控件：ThemedTextBox（占位提示）与 ThemedListBox（双缓冲），提升命令面板输入体验与渲染稳定性
- 引入崩溃对话框（Error Boundary Dialog）：与主题系统一致，支持复制详情/打开日志目录/打开崩溃报告
- 新增故障排查中心：内置日志尾部查看 + CrashReports 查看/复制，并挂载到命令面板（Ctrl+K）入口
- 故障排查中心增强：支持一键导出诊断包（SupportBundles），并补齐 TabControl/TabPage 主题一致性
- 故障排查中心增强：导出诊断包同时生成 Zip 文件；新增 ThemedTabControl（OwnerDraw）提升 Tab 页签在深色主题下的一致性

### 性能与清理
- 运营洞察快照引入轻量缓存与聚合查询，降低刷新成本
- 数据库诊断改为后台采集 + 单连接聚合查询，修复连接数统计口径，减少 UI 卡顿/重入风险
- 增加轻量性能埋点（PerfScope）：关键路径可渐进接入，输出统一 `[perf]` 日志

### 安全与配置
- 数据库连接诊断输出默认脱敏连接字符串，避免泄露真实密码
- 全局异常边界：捕获 UI/AppDomain/未观察到任务异常，并生成 CrashReports 便于排障
- 增加 `scripts/restore.ps1`（自动下载 nuget.exe）与 `build.ps1` 自动还原依赖，提升一键构建体验

### 工程化
- CI（GitHub Actions）在构建后运行单元测试，并上传 TRX 测试结果（artifact: `test-results`）
- `test.ps1` 支持 `-SkipBuild/-SkipRestore/-ResultsDirectory`，并通过 vswhere 兼容更多 VS 安装形态（Community/BuildTools/Enterprise 等）
- `.gitignore` 忽略 `TestResults/`，避免本地/CI 产物误入版本控制

### 文档
- 重写双语 README，并同步更新知识库架构/模块说明
- README 结构重制：中英完整覆盖（构建/运行/连接配置/快捷键/架构边界）
- README 补齐错误边界/崩溃报告与单元测试说明（`test.ps1`）

## 2025-12-31

### 安全与配置

- 移除仓库内明文数据库密码/连接串泄露点（代码与文档）
- 连接字符串支持优先从环境变量读取（推荐 `MES_CONNECTION_STRING`）
- CI 增加 secret guard：阻止 `pwd =` / `password =` 模式进入仓库（此处加空格以避免触发 CI 扫描）

### 架构与边界

- UI 层移除对 DAL 的直接依赖，建立 BLL 门面承接数据库诊断调用

### UI/UX 基线

- 引入 `DesignTokens`（设计 Token）作为统一视觉基线
- 增加 PerMonitorV2 manifest，提升高 DPI 显示质量

### 工程化

- 解决方案纳入 `tests` 工程，CI/MSBuild 构建链路更完整
