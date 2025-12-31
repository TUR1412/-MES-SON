# MES系统数据库连接配置

## 📋 数据库信息

- **数据库服务器**: localhost
- **端口**: 3306 (默认)
- **数据库名称**: `mes_db`
- **用户名**: `root`
- **密码**: `Qwe.123`
- **字符集**: `utf8mb4`

## 🔧 应用程序连接字符串

### App.config 配置
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <connectionStrings>
    <add name="MESDatabase" 
         connectionString="Server=localhost;Database=mes_db;Uid=root;Charset=utf8mb4;SslMode=none;" 
         providerName="MySql.Data.MySqlClient" />
  </connectionStrings>
</configuration>
```

### C# 代码中使用
```csharp
// 获取连接字符串
string connectionString = ConfigurationManager.ConnectionStrings["MESDatabase"].ConnectionString;

// 创建连接
using (var connection = new MySqlConnection(connectionString))
{
    connection.Open();
    // 执行数据库操作
}
```

## 📊 已创建的数据表

| 表名 | 中文名称 | 记录数 | 说明 |
|------|----------|--------|------|
| `material_info` | 物料信息表 | 5 | 包含原材料和成品信息 |
| `workshop_info` | 车间信息表 | 4 | 生产、装配、包装、质检车间 |
| `production_order_info` | 生产订单表 | 3 | 生产订单管理 |
| `work_order_info` | 工单信息表 | 3 | 工单管理 |
| `user_info` | 用户信息表 | 1 | 系统用户（含管理员） |
| `batch_info` | 批次信息表 | 3 | 生产批次管理 |
| `workshop_operation_info` | 车间作业表 | 3 | 车间作业管理 |

## 👤 默认用户账户

- **用户名**: admin
- **密码**: 123456 (MD5加密后存储)
- **角色**: 系统管理员
- **部门**: 信息技术部

## 🏭 示例数据概览

### 车间信息
- 生产车间A (WS001) - 生产部
- 装配车间B (WS002) - 生产部  
- 包装车间C (WS003) - 生产部
- 质检车间D (WS004) - 质量部

### 物料信息
- 钢板A型 (MAT001) - 原材料
- 螺栓M8 (MAT002) - 标准件
- 电机220V (MAT003) - 电气元件
- 产品A (PRD001) - 成品
- 产品B (PRD002) - 成品

### 生产订单
- PO202506090001 - 产品A，100台，待开始
- PO202506090002 - 产品B，50台，进行中
- PO202506090003 - 产品A，80台，已完成

## ⚠️ 重要提醒

1. **密码安全**: 生产环境请务必修改默认密码
2. **权限控制**: 建议创建专用数据库用户，限制权限
3. **备份策略**: 定期备份数据库，确保数据安全
4. **连接池**: 生产环境建议配置连接池参数

## 🔍 验证连接

可以使用以下SQL语句验证连接和数据：

```sql
-- 验证连接
USE mes_db;
SELECT 'Database connected successfully!' AS status;

-- 检查表结构
SHOW TABLES;

-- 验证数据
SELECT COUNT(*) AS total_materials FROM material_info;
SELECT COUNT(*) AS total_workshops FROM workshop_info;
SELECT COUNT(*) AS total_orders FROM production_order_info;
```

## 📞 技术支持

如有数据库连接问题，请检查：
1. MySQL服务是否启动
2. 防火墙设置
3. 连接字符串格式
4. 用户权限配置

---
**创建时间**: 2025-06-09  
**最后更新**: 2025-06-09

