# MES制造执行系统 - 部署运维指南

> **版本**: 1.0  
> **更新时间**: 2025-06-06  
> **维护者**: MES开发团队

## 📋 目录

- [系统概述](#系统概述)
- [环境要求](#环境要求)
- [部署流程](#部署流程)
- [配置管理](#配置管理)
- [运维监控](#运维监控)
- [备份恢复](#备份恢复)
- [故障排除](#故障排除)
- [性能优化](#性能优化)
- [安全管理](#安全管理)

## 🎯 系统概述

MES制造执行系统是基于.NET Framework 4.8 + WinForms + MySQL 8.0的企业级制造执行系统，采用三层架构设计，支持物料管理、生产管理、车间管理、设备管理、质量管理等核心业务功能。

### 系统架构

```
┌─────────────────────────────────────────────────────────────┐
│                    客户端层 (WinForms)                        │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐ │
│  │   主界面        │ │   业务窗体      │ │   报表界面      │ │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                                │
┌─────────────────────────────────────────────────────────────┐
│                   业务逻辑层 (BLL)                            │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐ │
│  │   物料管理      │ │   生产管理      │ │   质量管理      │ │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                                │
┌─────────────────────────────────────────────────────────────┐
│                   数据访问层 (DAL)                            │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐ │
│  │   MySQL连接     │ │   数据操作      │ │   事务管理      │ │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                                │
┌─────────────────────────────────────────────────────────────┐
│                     MySQL 8.0 数据库                         │
└─────────────────────────────────────────────────────────────┘
```

## 🔧 环境要求

### 硬件要求

| 组件 | 最低配置 | 推荐配置 |
|------|----------|----------|
| **CPU** | Intel i3 或同等性能 | Intel i5 或更高 |
| **内存** | 4GB RAM | 8GB RAM 或更高 |
| **硬盘** | 100GB 可用空间 | 500GB SSD |
| **网络** | 100Mbps | 1Gbps |

### 软件要求

| 软件 | 版本要求 | 说明 |
|------|----------|------|
| **操作系统** | Windows 10/11, Windows Server 2016+ | 支持.NET Framework 4.8 |
| **.NET Framework** | 4.8 | 必须安装 |
| **MySQL** | 8.0+ | 数据库服务器 |
| **Visual Studio** | 2019/2022 | 开发环境（可选） |
| **Git** | 2.30+ | 版本控制 |

### 网络要求

- **数据库端口**: 3306 (MySQL)
- **应用端口**: 根据需要配置
- **防火墙**: 允许MySQL端口访问

## 🚀 部署流程

### 1. 环境准备

#### 1.1 安装.NET Framework 4.8
```powershell
# 下载并安装.NET Framework 4.8
# https://dotnet.microsoft.com/download/dotnet-framework/net48
```

#### 1.2 安装MySQL 8.0
```bash
# Windows环境下载MySQL安装包
# https://dev.mysql.com/downloads/mysql/

# 配置MySQL服务
# 设置root密码
# 启用MySQL服务
```

#### 1.3 配置MySQL
```sql
-- 创建MES专用用户
CREATE USER 'mes_user'@'localhost' IDENTIFIED BY 'MES@2025!';
GRANT ALL PRIVILEGES ON mes_system.* TO 'mes_user'@'localhost';
FLUSH PRIVILEGES;

-- 设置字符集
SET GLOBAL character_set_server = 'utf8mb4';
SET GLOBAL collation_server = 'utf8mb4_unicode_ci';
```

### 2. 数据库部署

#### 2.1 执行数据库脚本
```bash
# 进入数据库脚本目录
cd database/mysql/

# 执行部署脚本
mysql -u root -p < 01_create_database.sql
mysql -u root -p < 02_create_business_tables.sql
mysql -u root -p < 03_insert_initial_data.sql
mysql -u root -p < 04_create_indexes_and_procedures.sql
```

#### 2.2 验证数据库部署
```sql
-- 连接数据库
mysql -u mes_user -p mes_system

-- 检查表结构
SHOW TABLES;

-- 检查初始数据
SELECT COUNT(*) FROM sys_user;
SELECT COUNT(*) FROM sys_role;
SELECT COUNT(*) FROM material;
```

### 3. 应用程序部署

#### 3.1 获取源代码
```bash
# 克隆代码仓库
git clone https://github.com/TUR1412/-MES-SON.git
cd -MES-SON

# 切换到主分支
git checkout main
```

#### 3.2 编译应用程序
```bash
# 使用Visual Studio编译
# 或使用MSBuild命令行编译
msbuild MES.sln /p:Configuration=Release /p:Platform="Any CPU"
```

#### 3.3 配置连接字符串
```xml
<!-- 修改App.config文件 -->
<connectionStrings>
    <add name="MESConnection" 
         connectionString="Server=localhost;Database=mes_system;Uid=mes_user;Pwd=MES@2025!;CharSet=utf8mb4;" 
         providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

#### 3.4 部署应用程序
```bash
# 复制编译后的文件到部署目录
xcopy /E /I "bin\Release\*" "C:\MES\Application\"

# 创建桌面快捷方式
# 配置启动参数
```

### 4. 验证部署

#### 4.1 启动应用程序
```bash
# 运行MES.UI.exe
cd C:\MES\Application\
MES.UI.exe
```

#### 4.2 登录测试
- **管理员账号**: admin / 123456
- **测试功能**: 物料管理、生产订单、车间管理

#### 4.3 数据库连接测试
```csharp
// 在应用程序中测试数据库连接
var connectionString = ConfigurationManager.ConnectionStrings["MESConnection"].ConnectionString;
using (var connection = new MySqlConnection(connectionString))
{
    connection.Open();
    // 连接成功
}
```

## ⚙️ 配置管理

### 1. 应用程序配置

#### 1.1 数据库配置
```xml
<appSettings>
    <!-- 数据库配置 -->
    <add key="DatabaseServer" value="localhost" />
    <add key="DatabaseName" value="mes_system" />
    <add key="DatabaseUser" value="mes_user" />
    <add key="DatabasePassword" value="MES@2025!" />
    
    <!-- 连接池配置 -->
    <add key="ConnectionPoolSize" value="100" />
    <add key="ConnectionTimeout" value="30" />
    <add key="CommandTimeout" value="60" />
</appSettings>
```

#### 1.2 日志配置
```xml
<appSettings>
    <!-- 日志配置 -->
    <add key="LogLevel" value="INFO" />
    <add key="LogPath" value="C:\MES\Logs\" />
    <add key="LogRetentionDays" value="30" />
    <add key="EnableFileLog" value="true" />
    <add key="EnableDatabaseLog" value="true" />
</appSettings>
```

#### 1.3 系统配置
```xml
<appSettings>
    <!-- 系统配置 -->
    <add key="SystemName" value="MES制造执行系统" />
    <add key="SystemVersion" value="1.0.0" />
    <add key="SessionTimeout" value="30" />
    <add key="MaxLoginAttempts" value="5" />
    <add key="PasswordMinLength" value="6" />
</appSettings>
```

### 2. 数据库配置

#### 2.1 性能配置
```sql
-- MySQL性能配置
SET GLOBAL innodb_buffer_pool_size = 1073741824; -- 1GB
SET GLOBAL max_connections = 200;
SET GLOBAL query_cache_size = 67108864; -- 64MB
SET GLOBAL slow_query_log = 1;
SET GLOBAL long_query_time = 2;
```

#### 2.2 备份配置
```sql
-- 备份配置
SET GLOBAL backup_retention_days = 30;
SET GLOBAL backup_path = '/var/backups/mes/';
SET GLOBAL auto_backup_enabled = 1;
SET GLOBAL backup_schedule = '0 2 * * *'; -- 每天凌晨2点
```

## 📊 运维监控

### 1. 系统监控

#### 1.1 性能监控脚本
```sql
-- 创建性能监控视图
CREATE VIEW v_system_performance AS
SELECT 
    'Database' AS component,
    'MySQL' AS service,
    CASE WHEN @@read_only = 0 THEN 'Running' ELSE 'ReadOnly' END AS status,
    @@version AS version,
    NOW() AS check_time;

-- 监控存储过程
DELIMITER $$
CREATE PROCEDURE sp_system_health_check()
BEGIN
    -- 检查数据库连接
    SELECT 'Database Connection' AS check_item, 'OK' AS status;
    
    -- 检查表空间
    SELECT 
        table_schema,
        ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS size_mb
    FROM information_schema.tables 
    WHERE table_schema = 'mes_system'
    GROUP BY table_schema;
    
    -- 检查慢查询
    SELECT COUNT(*) AS slow_queries
    FROM mysql.slow_log 
    WHERE start_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR);
END$$
DELIMITER ;
```

#### 1.2 应用程序监控
```csharp
// 应用程序健康检查
public class HealthCheckService
{
    public HealthCheckResult CheckDatabaseConnection()
    {
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return new HealthCheckResult { Status = "OK", Message = "数据库连接正常" };
            }
        }
        catch (Exception ex)
        {
            return new HealthCheckResult { Status = "ERROR", Message = ex.Message };
        }
    }
    
    public HealthCheckResult CheckMemoryUsage()
    {
        var process = Process.GetCurrentProcess();
        var memoryMB = process.WorkingSet64 / 1024 / 1024;
        
        return new HealthCheckResult 
        { 
            Status = memoryMB < 500 ? "OK" : "WARNING",
            Message = $"内存使用: {memoryMB}MB"
        };
    }
}
```

### 2. 日志监控

#### 2.1 日志分析脚本
```sql
-- 错误日志统计
SELECT 
    log_level,
    module_name,
    COUNT(*) AS error_count,
    DATE(create_time) AS error_date
FROM sys_log 
WHERE log_level IN ('ERROR', 'FATAL')
    AND create_time >= DATE_SUB(NOW(), INTERVAL 7 DAY)
GROUP BY log_level, module_name, DATE(create_time)
ORDER BY error_date DESC, error_count DESC;

-- 用户操作统计
SELECT 
    user_name,
    operation_name,
    COUNT(*) AS operation_count,
    DATE(create_time) AS operation_date
FROM sys_log 
WHERE log_type = 'BUSINESS'
    AND create_time >= DATE_SUB(NOW(), INTERVAL 1 DAY)
GROUP BY user_name, operation_name, DATE(create_time)
ORDER BY operation_count DESC;
```

### 3. 告警配置

#### 3.1 数据库告警
```sql
-- 创建告警检查存储过程
DELIMITER $$
CREATE PROCEDURE sp_check_alerts()
BEGIN
    DECLARE alert_count INT DEFAULT 0;
    
    -- 检查磁盘空间
    -- 检查连接数
    -- 检查慢查询
    -- 检查错误日志
    
    -- 发送告警（需要在应用层实现）
    IF alert_count > 0 THEN
        INSERT INTO sys_log (log_level, log_type, module_name, operation_name, log_message)
        VALUES ('WARNING', 'ALERT', 'SYSTEM', 'HEALTH_CHECK', 
                CONCAT('发现 ', alert_count, ' 个系统告警'));
    END IF;
END$$
DELIMITER ;
```

## 💾 备份恢复

### 1. 备份策略

#### 1.1 自动备份脚本
```bash
#!/bin/bash
# MES数据库自动备份脚本

BACKUP_DIR="/var/backups/mes"
DATE=$(date +%Y%m%d_%H%M%S)
BACKUP_FILE="mes_system_full_${DATE}.sql"

# 创建备份目录
mkdir -p $BACKUP_DIR

# 执行备份
mysqldump -u mes_user -p --single-transaction --routines --triggers \
    --events --hex-blob --default-character-set=utf8mb4 \
    mes_system > $BACKUP_DIR/$BACKUP_FILE

# 压缩备份文件
gzip $BACKUP_DIR/$BACKUP_FILE

# 删除7天前的备份
find $BACKUP_DIR -name "*.sql.gz" -mtime +7 -delete

# 记录备份日志
echo "$(date): 备份完成 - $BACKUP_FILE.gz" >> $BACKUP_DIR/backup.log
```

#### 1.2 Windows备份脚本
```batch
@echo off
REM MES数据库Windows备份脚本

set BACKUP_DIR=C:\MES\Backups
set DATE=%date:~0,4%%date:~5,2%%date:~8,2%_%time:~0,2%%time:~3,2%%time:~6,2%
set BACKUP_FILE=mes_system_full_%DATE%.sql

REM 创建备份目录
if not exist %BACKUP_DIR% mkdir %BACKUP_DIR%

REM 执行备份
mysqldump -u mes_user -p --single-transaction --routines --triggers ^
    --events --hex-blob --default-character-set=utf8mb4 ^
    mes_system > %BACKUP_DIR%\%BACKUP_FILE%

REM 记录日志
echo %date% %time%: 备份完成 - %BACKUP_FILE% >> %BACKUP_DIR%\backup.log
```

### 2. 恢复流程

#### 2.1 完整恢复
```bash
# 1. 停止应用程序
# 2. 创建恢复点
mysqldump -u mes_user -p mes_system > /var/backups/mes/restore_point_$(date +%Y%m%d_%H%M%S).sql

# 3. 执行恢复
mysql -u mes_user -p mes_system < /var/backups/mes/mes_system_full_20250606_020000.sql

# 4. 验证恢复结果
mysql -u mes_user -p -e "SELECT COUNT(*) FROM mes_system.sys_user;"

# 5. 重启应用程序
```

#### 2.2 数据恢复
```sql
-- 使用存储过程恢复
CALL sp_restore_database_full(
    '/var/backups/mes/mes_system_full_20250606_020000.sql',
    'admin',
    TRUE,
    '系统故障恢复'
);
```

## 🔍 故障排除

### 1. 常见问题

#### 1.1 数据库连接问题
```sql
-- 检查MySQL服务状态
SHOW PROCESSLIST;

-- 检查用户权限
SHOW GRANTS FOR 'mes_user'@'localhost';

-- 检查连接数
SHOW STATUS LIKE 'Threads_connected';
SHOW VARIABLES LIKE 'max_connections';
```

#### 1.2 应用程序问题
```csharp
// 连接字符串检查
var connectionString = ConfigurationManager.ConnectionStrings["MESConnection"].ConnectionString;
Console.WriteLine($"连接字符串: {connectionString}");

// 数据库连接测试
try
{
    using (var connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("数据库连接成功");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"数据库连接失败: {ex.Message}");
}
```

### 2. 性能问题

#### 2.1 慢查询分析
```sql
-- 启用慢查询日志
SET GLOBAL slow_query_log = 1;
SET GLOBAL long_query_time = 2;

-- 查看慢查询
SELECT * FROM mysql.slow_log ORDER BY start_time DESC LIMIT 10;

-- 分析查询执行计划
EXPLAIN SELECT * FROM material WHERE material_type = 'RAW_MATERIAL';
```

#### 2.2 索引优化
```sql
-- 检查索引使用情况
SHOW INDEX FROM material;

-- 分析表统计信息
ANALYZE TABLE material;

-- 优化表
OPTIMIZE TABLE material;
```

## ⚡ 性能优化

### 1. 数据库优化

#### 1.1 配置优化
```sql
-- InnoDB配置
SET GLOBAL innodb_buffer_pool_size = 2147483648; -- 2GB
SET GLOBAL innodb_log_file_size = 268435456; -- 256MB
SET GLOBAL innodb_flush_log_at_trx_commit = 2;

-- 查询缓存
SET GLOBAL query_cache_type = 1;
SET GLOBAL query_cache_size = 134217728; -- 128MB

-- 连接配置
SET GLOBAL max_connections = 300;
SET GLOBAL wait_timeout = 28800;
```

#### 1.2 索引优化
```sql
-- 创建复合索引
CREATE INDEX idx_material_type_status ON material (material_type, status);
CREATE INDEX idx_production_time_status ON production_order (planned_start_time, status);

-- 删除未使用的索引
-- 定期分析索引使用情况
```

### 2. 应用程序优化

#### 2.1 连接池配置
```xml
<connectionStrings>
    <add name="MESConnection" 
         connectionString="Server=localhost;Database=mes_system;Uid=mes_user;Pwd=MES@2025!;CharSet=utf8mb4;Pooling=true;Min Pool Size=5;Max Pool Size=100;Connection Timeout=30;" 
         providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

#### 2.2 缓存策略
```csharp
// 实现数据缓存
public class CacheService
{
    private static readonly MemoryCache _cache = new MemoryCache();
    
    public T Get<T>(string key) where T : class
    {
        return _cache.Get(key) as T;
    }
    
    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        _cache.Set(key, value, expiration);
    }
}
```

## 🔒 安全管理

### 1. 数据库安全

#### 1.1 用户权限管理
```sql
-- 创建只读用户
CREATE USER 'mes_readonly'@'localhost' IDENTIFIED BY 'ReadOnly@2025!';
GRANT SELECT ON mes_system.* TO 'mes_readonly'@'localhost';

-- 创建备份用户
CREATE USER 'mes_backup'@'localhost' IDENTIFIED BY 'Backup@2025!';
GRANT SELECT, LOCK TABLES, SHOW VIEW ON mes_system.* TO 'mes_backup'@'localhost';

-- 定期更新密码
ALTER USER 'mes_user'@'localhost' IDENTIFIED BY 'NewPassword@2025!';
```

#### 1.2 数据加密
```sql
-- 启用SSL连接
-- 配置数据加密
-- 设置审计日志
```

### 2. 应用程序安全

#### 2.1 密码安全
```csharp
// 密码加密
public class PasswordHelper
{
    public static string HashPassword(string password, string salt)
    {
        using (var md5 = MD5.Create())
        {
            var input = Encoding.UTF8.GetBytes(password + salt);
            var hash = md5.ComputeHash(input);
            return Convert.ToBase64String(hash);
        }
    }
    
    public static bool VerifyPassword(string password, string salt, string hash)
    {
        return HashPassword(password, salt) == hash;
    }
}
```

#### 2.2 访问控制
```csharp
// 权限验证
public class PermissionService
{
    public bool HasPermission(string userCode, string permission)
    {
        // 检查用户权限
        // 验证角色权限
        // 返回验证结果
        return true;
    }
}
```

---

**技术支持**: MES开发团队  
**联系方式**: mes-support@company.com  
**文档版本**: 1.0  
**最后更新**: 2025-06-06
