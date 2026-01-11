# 内部 API 与交互约定（SSOT）

MES-SON 当前为桌面端 WinForms 应用，**没有对外 HTTP API** 作为主要形态；这里的 “API” 指 **跨层调用边界** 与 **对 UI 暴露的 BLL 接口/门面**。

## 1) BLL 对 UI 的建议暴露方式

优先使用 “接口 + 实现” 的方式，形成稳定边界：

- 接口：`src/MES.BLL/SystemManagement/IDatabaseDiagnosticBLL.cs`
- 实现：`src/MES.BLL/SystemManagement/DatabaseDiagnosticBLL.cs`
- 接口：`src/MES.BLL/Analytics/IInsightBLL.cs`
- 实现：`src/MES.BLL/Analytics/InsightBLL.cs`

### 为什么这样做

- UI 不需要了解 DAL 的连接与异常细节
- 便于未来替换实现（例如引入缓存、重试、日志、熔断）而不动 UI

## 2) 配置 API（连接字符串）

统一入口：`src/MES.Common/Configuration/ConfigManager.cs`

推荐调用方向：

- UI：只做 “检测 + 提示” 与 “把业务动作交给 BLL”
- BLL/DAL：通过 `ConfigManager` 获取连接字符串，不要再写默认值/硬编码密码

## 3) 错误与提示（用户体验守则）

- UI 层提示应当：
  - 明确告诉用户缺什么（例如缺连接字符串）
  - 明确告诉用户怎么补（环境变量名 / 配置位置）
  - 避免输出敏感信息（尤其是密码）
- DAL/BLL 抛出异常时：
  - 错误信息避免拼接完整连接字符串
  - 如需打印，仅允许脱敏（`Password = ******`）

## 4) 新增功能的推荐落点（避免泥球）

新增一个 UI 功能点时：

1. UI：只负责界面与交互事件（Form/Control）
2. BLL：新增 `IxxxBLL` + `xxxBLL`，封装业务流程
3. DAL：按需新增数据访问方法（参数化 SQL / 最小权限）
4. Common：横切能力（配置、异常、日志）统一放置
