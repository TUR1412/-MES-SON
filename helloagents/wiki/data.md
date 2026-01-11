# 数据与数据库（SSOT）

## 1) 数据库类型

- **MySQL**（通过 `MySql.Data` 访问）

> 说明：仓库中可能存在历史脚本/文档，请以本文件与源码为准，并确保所有示例均为脱敏内容。

## 2) 连接字符串（安全规范）

### 2.1 推荐（环境变量）

- `MES_CONNECTION_STRING`
- `MES_TEST_CONNECTION_STRING`（可选）
- `MES_PROD_CONNECTION_STRING`（可选）

示例（脱敏占位符，仅示意格式）：

`Server=127.0.0.1;Port=3306;Database=mes;User Id=root;Password = ******;SslMode=None;`

### 2.2 回退（本机 App.config）

仅用于开发机本地运行；不得提交真实密码到仓库。

相关文件：

- `src/MES.UI/App.config`
- `tests/App.config`

## 3) 数据访问约定（DAL）

- 必须参数化 SQL，避免注入风险
- 不在日志/异常中输出明文密码
- 数据库连接统一从 `MES.Common/Configuration/ConfigManager.cs` 获取

关键入口：

- `src/MES.DAL/Core/DatabaseHelper.cs`

## 4) 洞察派生指标（无新增表）

- 生产订单风险：`production_order_info`
- 在制品老化：`wip_info`
- 设备健康与维护：`equipment`
- 物料库存告警：`material_info`
- 质量缺陷统计：`quality_inspection`
- 批次良率：`batch_info`

## 5) 资料位置

- 数据库连接说明：`database/MES_Connection_Config.md`
- 部署/运维：`docs/部署运维.md`
