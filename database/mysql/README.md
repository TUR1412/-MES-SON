# MES制造执行系统 - MySQL数据库部署指南

## 📋 概述

本目录包含MES制造执行系统的完整MySQL数据库脚本，支持MySQL 8.0及以上版本。

## 重要说明（避免“脚本跑了但业务不通”的幽灵问题）

- 当前代码（DAL）主要使用表名：`material_info`、`bom_info`、`production_order` 等。
- `database/init_database.sql` 已按上述表名与 `MES.UI/App.config` 的默认库名 `mes_system` 对齐，且包含生产订单表。
- 本目录中的部分脚本历史上使用了 `material`、`bom` 等表名，与当前 DAL 不完全一致；请勿混合执行两套脚本。

建议：优先执行 `database/init_database.sql` 作为单一权威脚本入口。

## 📁 文件结构

```
database/mysql/
├── README.md                           # 本文件 - 部署指南
├── 01_create_database.sql              # 数据库和基础表创建
├── 02_create_business_tables.sql       # 业务表创建
├── 03_insert_initial_data.sql          # 初始数据插入
├── 04_create_indexes_and_procedures.sql # 索引、存储过程、视图创建
└── backup/                             # 备份脚本目录
    ├── backup_database.sql             # 数据库备份脚本
    └── restore_database.sql            # 数据库恢复脚本
```

## 🚀 快速部署

### 1. 环境要求

- **MySQL版本**: 8.0 或更高版本
- **字符集**: utf8mb4
- **排序规则**: utf8mb4_unicode_ci
- **时区**: +08:00 (北京时间)

### 2. 部署步骤

#### 步骤1：创建数据库和基础表
```sql
-- 在MySQL命令行或工具中执行
mysql -u root -p < 01_create_database.sql
```

#### 步骤2：创建业务表
```sql
mysql -u root -p < 02_create_business_tables.sql
```

#### 步骤3：插入初始数据
```sql
mysql -u root -p < 03_insert_initial_data.sql
```

#### 步骤4：创建索引和存储过程
```sql
mysql -u root -p < 04_create_indexes_and_procedures.sql
```

### 3. 一键部署脚本

创建 `deploy.sh` 文件：
```bash
#!/bin/bash
echo "开始部署MES数据库..."

# 检查MySQL是否运行
if ! systemctl is-active --quiet mysql; then
    echo "MySQL服务未运行，请先启动MySQL服务"
    exit 1
fi

# 执行SQL脚本
echo "1. 创建数据库和基础表..."
mysql -u root -p < 01_create_database.sql

echo "2. 创建业务表..."
mysql -u root -p < 02_create_business_tables.sql

echo "3. 插入初始数据..."
mysql -u root -p < 03_insert_initial_data.sql

echo "4. 创建索引和存储过程..."
mysql -u root -p < 04_create_indexes_and_procedures.sql

echo "MES数据库部署完成！"
echo "默认管理员账号: admin / 123456"
echo "团队成员账号: tianDi/123456, userL/123456, userH/123456, userS/123456"
```

## 🗄️ 数据库结构

### 系统管理表

| 表名 | 说明 | 主要字段 |
|------|------|----------|
| `sys_config` | 系统配置表 | config_key, config_value, config_type |
| `sys_dictionary` | 数据字典表 | dict_type, dict_code, dict_name, dict_value |
| `sys_log` | 系统日志表 | log_level, module_name, log_message |
| `sys_user` | 用户表 | user_code, user_name, password, email |
| `sys_role` | 角色表 | role_code, role_name, permissions |
| `sys_user_role` | 用户角色关联表 | user_id, role_id |
| `sys_permission` | 权限表 | permission_code, permission_name, permission_type |

### 业务数据表

| 表名 | 说明 | 主要字段 |
|------|------|----------|
| `material` | 物料信息表 | material_code, material_name, current_stock |
| `bom` | BOM清单表 | bom_code, parent_material_id, child_material_id |
| `workshop` | 车间信息表 | workshop_code, workshop_name, capacity |
| `production_order` | 生产订单表 | order_number, product_code, planned_quantity |
| `equipment` | 设备信息表 | equipment_code, equipment_name, status |
| `equipment_maintenance` | 设备维护记录表 | equipment_id, maintenance_date, maintenance_content |
| `quality_inspection` | 质量检验表 | inspection_number, inspection_result, inspector_id |

## 👥 默认用户账号

| 用户名 | 密码 | 角色 | 说明 |
|--------|------|------|------|
| `admin` | `123456` | 系统管理员 | 拥有所有权限 |
| `tianDi` | `123456` | 系统管理员 | 项目组长 |
| `userL` | `123456` | 物料经理 | L成员 - 物料管理 |
| `userH` | `123456` | 生产经理 | H成员 - 生产管理 |
| `userS` | `123456` | 车间主管 | S成员 - 车间管理 |

## 🔧 配置说明

### 数据库连接配置

```csharp
// C# 连接字符串示例
string connectionString = "Server=localhost;Database=mes_system;Uid=mes_user;Pwd=MES@2025!;CharSet=utf8mb4;";
```

### 重要配置项

- **数据库名称**: `mes_system`
- **专用用户**: `mes_user` / `MES@2025!`
- **字符集**: `utf8mb4`
- **时区**: `+08:00`

## 📊 性能优化

### 已创建的索引

1. **复合索引**: 针对常用查询组合创建
2. **外键索引**: 保证关联查询性能
3. **时间索引**: 优化按时间范围查询
4. **状态索引**: 优化按状态筛选查询

### 存储过程

| 存储过程名 | 功能 | 参数 |
|------------|------|------|
| `sp_get_material_stock_alert` | 获取库存预警物料 | 无 |
| `sp_get_production_order_statistics` | 生产订单统计 | 开始日期, 结束日期 |
| `sp_get_equipment_maintenance_schedule` | 设备维护计划 | 提前天数 |
| `sp_get_quality_statistics` | 质量统计 | 开始日期, 结束日期, 检验类型 |
| `sp_cleanup_old_logs` | 清理旧日志 | 保留天数 |

### 视图

| 视图名 | 功能 | 说明 |
|--------|------|------|
| `v_material_stock_alert` | 物料库存预警 | 实时库存状态 |
| `v_production_order_progress` | 生产订单进度 | 完成度和延期情况 |
| `v_equipment_maintenance_schedule` | 设备维护计划 | 维护时间安排 |
| `v_quality_inspection_summary` | 质量检验汇总 | 按产品和日期汇总 |

## 🔒 安全配置

### 用户权限

- `mes_user`: 应用程序专用用户，仅有必要的数据操作权限
- `root`: 仅用于初始部署，生产环境建议禁用

### 数据安全

- 密码使用MD5加密存储
- 支持软删除，重要数据不物理删除
- 完整的审计日志记录

## 📈 监控和维护

### 日志管理

```sql
-- 查看系统日志
SELECT * FROM sys_log WHERE create_time >= DATE_SUB(NOW(), INTERVAL 1 DAY);

-- 清理30天前的日志
CALL sp_cleanup_old_logs(30);
```

### 性能监控

```sql
-- 查看库存预警
SELECT * FROM v_material_stock_alert WHERE alert_level <= 2;

-- 查看设备维护计划
SELECT * FROM v_equipment_maintenance_schedule WHERE days_until_maintenance <= 7;
```

### 备份建议

- **每日备份**: 使用mysqldump进行全量备份
- **增量备份**: 启用binlog进行增量备份
- **备份验证**: 定期验证备份文件完整性

## 🚨 故障排除

### 常见问题

1. **字符集问题**
   ```sql
   -- 检查字符集
   SHOW VARIABLES LIKE 'character_set%';
   ```

2. **权限问题**
   ```sql
   -- 检查用户权限
   SHOW GRANTS FOR 'mes_user'@'localhost';
   ```

3. **连接问题**
   ```sql
   -- 检查连接数
   SHOW STATUS LIKE 'Threads_connected';
   ```

### 性能调优

1. **查询优化**
   ```sql
   -- 分析慢查询
   SHOW PROCESSLIST;
   ```

2. **索引优化**
   ```sql
   -- 检查索引使用情况
   EXPLAIN SELECT * FROM material WHERE material_type = 'RAW_MATERIAL';
   ```

## 📞 技术支持

如遇到部署问题，请检查：

1. MySQL版本是否为8.0+
2. 字符集是否正确设置
3. 用户权限是否充足
4. 防火墙设置是否正确

---

**维护者**: MES开发团队  
**最后更新**: 2025-06-06  
**版本**: 1.0
