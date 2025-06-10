# MES项目数据库配置与操作指南

> **创建时间**: 2025-06-10 16:15:05
> **数据库版本**: MySQL 8.0+
> **字符集**: utf8mb4
> **连接方式**: MySQL.Data.MySqlClient

## 🔧 数据库连接配置

### 标准连接字符串
```xml
<!-- App.config 中的连接字符串配置 -->
<connectionStrings>
  <add name="MESConnection" 
       connectionString="Server=localhost;Database=mes_db;Uid=root;Pwd=Qwe.123;charset=utf8mb4;" 
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

### 连接参数说明
- **Server**: 数据库服务器地址 (localhost 或 IP地址)
- **Database**: 数据库名称 (mes_db)
- **Uid**: 数据库用户名 (root)
- **Pwd**: 数据库密码 (Qwe.123)
- **charset**: 字符集 (utf8mb4，支持完整的UTF-8字符)

### 环境配置要求
- **MySQL版本**: 8.0 或更高版本
- **字符集**: utf8mb4 (支持emoji和特殊字符)
- **时区**: 建议设置为系统本地时区
- **连接池**: 启用连接池以提高性能

## 📊 数据库结构

### 核心业务表

#### 物料管理相关表
```sql
-- 物料信息表
CREATE TABLE materials (
    id INT PRIMARY KEY AUTO_INCREMENT,
    material_code VARCHAR(50) UNIQUE NOT NULL COMMENT '物料编码',
    material_name VARCHAR(200) NOT NULL COMMENT '物料名称',
    category_id INT COMMENT '分类ID',
    unit VARCHAR(20) COMMENT '计量单位',
    status TINYINT DEFAULT 1 COMMENT '状态：0-停用，1-启用',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- BOM物料清单表
CREATE TABLE bom_info (
    id INT PRIMARY KEY AUTO_INCREMENT,
    bom_code VARCHAR(50) UNIQUE NOT NULL COMMENT 'BOM编码',
    product_id INT NOT NULL COMMENT '产品ID',
    version VARCHAR(20) DEFAULT '1.0' COMMENT 'BOM版本',
    status TINYINT DEFAULT 1 COMMENT '状态',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 工艺路线表
CREATE TABLE process_routes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    route_code VARCHAR(50) UNIQUE NOT NULL COMMENT '工艺路线编码',
    route_name VARCHAR(200) NOT NULL COMMENT '工艺路线名称',
    product_id INT COMMENT '适用产品ID',
    version VARCHAR(20) DEFAULT '1.0' COMMENT '版本号',
    status TINYINT DEFAULT 1 COMMENT '状态',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

#### 生产管理相关表
```sql
-- 生产订单表
CREATE TABLE production_orders (
    id INT PRIMARY KEY AUTO_INCREMENT,
    order_no VARCHAR(50) UNIQUE NOT NULL COMMENT '订单号',
    product_id INT NOT NULL COMMENT '产品ID',
    quantity DECIMAL(10,2) NOT NULL COMMENT '生产数量',
    plan_start_date DATE COMMENT '计划开始日期',
    plan_end_date DATE COMMENT '计划完成日期',
    status TINYINT DEFAULT 0 COMMENT '状态：0-待开始，1-生产中，2-已完成',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 工单表
CREATE TABLE work_orders (
    id INT PRIMARY KEY AUTO_INCREMENT,
    work_order_no VARCHAR(50) UNIQUE NOT NULL COMMENT '工单号',
    production_order_id INT NOT NULL COMMENT '生产订单ID',
    workshop_id INT COMMENT '车间ID',
    status TINYINT DEFAULT 0 COMMENT '状态',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

#### 车间管理相关表
```sql
-- 车间信息表
CREATE TABLE workshops (
    id INT PRIMARY KEY AUTO_INCREMENT,
    workshop_code VARCHAR(50) UNIQUE NOT NULL COMMENT '车间编码',
    workshop_name VARCHAR(200) NOT NULL COMMENT '车间名称',
    manager VARCHAR(100) COMMENT '车间主管',
    status TINYINT DEFAULT 1 COMMENT '状态',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 批次信息表
CREATE TABLE batches (
    id INT PRIMARY KEY AUTO_INCREMENT,
    batch_number VARCHAR(50) UNIQUE NOT NULL COMMENT '批次号',
    product_id INT NOT NULL COMMENT '产品ID',
    quantity DECIMAL(10,2) NOT NULL COMMENT '批次数量',
    workshop_id INT COMMENT '所在车间',
    status TINYINT DEFAULT 0 COMMENT '状态',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 在制品信息表
CREATE TABLE wip_info (
    id INT PRIMARY KEY AUTO_INCREMENT,
    wip_id VARCHAR(50) UNIQUE NOT NULL COMMENT '在制品编号',
    batch_id INT NOT NULL COMMENT '批次ID',
    current_workshop_id INT COMMENT '当前车间',
    status TINYINT DEFAULT 0 COMMENT '状态',
    completed_quantity DECIMAL(10,2) DEFAULT 0 COMMENT '已完成数量',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

#### 设备管理相关表
```sql
-- 设备信息表
CREATE TABLE equipment (
    id INT PRIMARY KEY AUTO_INCREMENT,
    equipment_code VARCHAR(50) UNIQUE NOT NULL COMMENT '设备编码',
    equipment_name VARCHAR(200) NOT NULL COMMENT '设备名称',
    workshop_id INT COMMENT '所属车间',
    equipment_type_id INT COMMENT '设备类型',
    status TINYINT DEFAULT 1 COMMENT '状态：0-停机，1-运行，2-维护',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

### 系统管理相关表
```sql
-- 用户表
CREATE TABLE users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(50) UNIQUE NOT NULL COMMENT '用户名',
    password VARCHAR(255) NOT NULL COMMENT '密码(加密)',
    real_name VARCHAR(100) COMMENT '真实姓名',
    email VARCHAR(100) COMMENT '邮箱',
    status TINYINT DEFAULT 1 COMMENT '状态',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 角色表
CREATE TABLE roles (
    id INT PRIMARY KEY AUTO_INCREMENT,
    role_code VARCHAR(50) UNIQUE NOT NULL COMMENT '角色编码',
    role_name VARCHAR(100) NOT NULL COMMENT '角色名称',
    description TEXT COMMENT '角色描述',
    status TINYINT DEFAULT 1 COMMENT '状态',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    update_time DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 用户角色关联表
CREATE TABLE user_roles (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL COMMENT '用户ID',
    role_id INT NOT NULL COMMENT '角色ID',
    create_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY uk_user_role (user_id, role_id)
);
```

## 🔌 数据库操作规范

### DAL层标准操作模式

#### 基础CRUD操作
```csharp
// 标准查询操作
public MaterialInfo GetMaterialById(int id)
{
    try
    {
        string sql = "SELECT * FROM materials WHERE id = @id";
        var parameters = new List<MySqlParameter>
        {
            DatabaseHelper.CreateParameter("@id", id)
        };
        
        var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());
        
        if (dataTable.Rows.Count > 0)
        {
            return ConvertToMaterialInfo(dataTable.Rows[0]);
        }
        
        return null;
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("查询物料失败，ID：{0}，错误：{1}", id, ex.Message), ex);
        throw;
    }
}

// 标准插入操作
public bool InsertMaterial(MaterialInfo material)
{
    try
    {
        string sql = @"INSERT INTO materials 
                      (material_code, material_name, category_id, unit, status) 
                      VALUES (@code, @name, @categoryId, @unit, @status)";
        
        var parameters = new List<MySqlParameter>
        {
            DatabaseHelper.CreateParameter("@code", material.MaterialCode),
            DatabaseHelper.CreateParameter("@name", material.MaterialName),
            DatabaseHelper.CreateParameter("@categoryId", material.CategoryId),
            DatabaseHelper.CreateParameter("@unit", material.Unit),
            DatabaseHelper.CreateParameter("@status", material.Status)
        };
        
        int result = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray());
        return result > 0;
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("插入物料失败：{0}", ex.Message), ex);
        throw;
    }
}

// 标准更新操作
public bool UpdateMaterial(MaterialInfo material)
{
    try
    {
        string sql = @"UPDATE materials SET 
                      material_name = @name, 
                      category_id = @categoryId, 
                      unit = @unit, 
                      status = @status,
                      update_time = NOW()
                      WHERE id = @id";
        
        var parameters = new List<MySqlParameter>
        {
            DatabaseHelper.CreateParameter("@name", material.MaterialName),
            DatabaseHelper.CreateParameter("@categoryId", material.CategoryId),
            DatabaseHelper.CreateParameter("@unit", material.Unit),
            DatabaseHelper.CreateParameter("@status", material.Status),
            DatabaseHelper.CreateParameter("@id", material.Id)
        };
        
        int result = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray());
        return result > 0;
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("更新物料失败：{0}", ex.Message), ex);
        throw;
    }
}

// 标准删除操作
public bool DeleteMaterial(int id)
{
    try
    {
        string sql = "DELETE FROM materials WHERE id = @id";
        var parameters = new List<MySqlParameter>
        {
            DatabaseHelper.CreateParameter("@id", id)
        };
        
        int result = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray());
        return result > 0;
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("删除物料失败，ID：{0}，错误：{1}", id, ex.Message), ex);
        throw;
    }
}
```

#### 复杂查询操作
```csharp
// 分页查询
public List<MaterialInfo> GetMaterialsByPage(int pageIndex, int pageSize, string keyword)
{
    try
    {
        var sql = new StringBuilder();
        sql.Append("SELECT * FROM materials WHERE 1=1");
        
        var parameters = new List<MySqlParameter>();
        
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            sql.Append(" AND (material_code LIKE @keyword OR material_name LIKE @keyword)");
            parameters.Add(DatabaseHelper.CreateParameter("@keyword", string.Format("%{0}%", keyword)));
        }
        
        sql.Append(" ORDER BY create_time DESC");
        sql.Append(" LIMIT @offset, @pageSize");
        
        parameters.Add(DatabaseHelper.CreateParameter("@offset", (pageIndex - 1) * pageSize));
        parameters.Add(DatabaseHelper.CreateParameter("@pageSize", pageSize));
        
        var dataTable = DatabaseHelper.ExecuteQuery(sql.ToString(), parameters.ToArray());
        return ConvertToMaterialList(dataTable);
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("分页查询物料失败：{0}", ex.Message), ex);
        throw;
    }
}

// 统计查询
public int GetMaterialCount(string keyword)
{
    try
    {
        var sql = new StringBuilder();
        sql.Append("SELECT COUNT(*) FROM materials WHERE 1=1");
        
        var parameters = new List<MySqlParameter>();
        
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            sql.Append(" AND (material_code LIKE @keyword OR material_name LIKE @keyword)");
            parameters.Add(DatabaseHelper.CreateParameter("@keyword", string.Format("%{0}%", keyword)));
        }
        
        var result = DatabaseHelper.ExecuteScalar(sql.ToString(), parameters.ToArray());
        return Convert.ToInt32(result);
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("统计物料数量失败：{0}", ex.Message), ex);
        throw;
    }
}
```

### 事务处理规范

#### 标准事务操作
```csharp
public bool SaveBOMWithDetails(BOMInfo bom, List<BOMDetailInfo> details)
{
    MySqlConnection connection = null;
    MySqlTransaction transaction = null;
    
    try
    {
        connection = DatabaseHelper.GetConnection();
        connection.Open();
        transaction = connection.BeginTransaction();
        
        // 保存BOM主表
        string bomSql = @"INSERT INTO bom_info 
                         (bom_code, product_id, version, status) 
                         VALUES (@code, @productId, @version, @status)";
        
        var bomParams = new List<MySqlParameter>
        {
            DatabaseHelper.CreateParameter("@code", bom.BOMCode),
            DatabaseHelper.CreateParameter("@productId", bom.ProductId),
            DatabaseHelper.CreateParameter("@version", bom.Version),
            DatabaseHelper.CreateParameter("@status", bom.Status)
        };
        
        int bomResult = DatabaseHelper.ExecuteNonQuery(bomSql, bomParams.ToArray(), transaction);
        
        if (bomResult <= 0)
        {
            transaction.Rollback();
            return false;
        }
        
        // 获取新插入的BOM ID
        long bomId = DatabaseHelper.GetLastInsertId(transaction);
        
        // 保存BOM明细
        foreach (var detail in details)
        {
            string detailSql = @"INSERT INTO bom_details 
                               (bom_id, material_id, quantity, unit) 
                               VALUES (@bomId, @materialId, @quantity, @unit)";
            
            var detailParams = new List<MySqlParameter>
            {
                DatabaseHelper.CreateParameter("@bomId", bomId),
                DatabaseHelper.CreateParameter("@materialId", detail.MaterialId),
                DatabaseHelper.CreateParameter("@quantity", detail.Quantity),
                DatabaseHelper.CreateParameter("@unit", detail.Unit)
            };
            
            int detailResult = DatabaseHelper.ExecuteNonQuery(detailSql, detailParams.ToArray(), transaction);
            
            if (detailResult <= 0)
            {
                transaction.Rollback();
                return false;
            }
        }
        
        transaction.Commit();
        return true;
    }
    catch (Exception ex)
    {
        if (transaction != null)
        {
            transaction.Rollback();
        }
        
        LogManager.Error(string.Format("保存BOM及明细失败：{0}", ex.Message), ex);
        throw;
    }
    finally
    {
        if (transaction != null)
        {
            transaction.Dispose();
        }
        
        if (connection != null)
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
```

## 🛠️ 数据库维护

### 性能优化建议

#### 索引优化
```sql
-- 为常用查询字段创建索引
CREATE INDEX idx_materials_code ON materials(material_code);
CREATE INDEX idx_materials_name ON materials(material_name);
CREATE INDEX idx_materials_status ON materials(status);
CREATE INDEX idx_materials_create_time ON materials(create_time);

-- 为外键字段创建索引
CREATE INDEX idx_bom_details_bom_id ON bom_details(bom_id);
CREATE INDEX idx_bom_details_material_id ON bom_details(material_id);

-- 复合索引
CREATE INDEX idx_materials_status_code ON materials(status, material_code);
```

#### 查询优化
```sql
-- 使用EXPLAIN分析查询性能
EXPLAIN SELECT * FROM materials WHERE material_code LIKE 'MAT%';

-- 避免全表扫描
-- ❌ 错误：没有使用索引
SELECT * FROM materials WHERE UPPER(material_code) = 'MAT001';

-- ✅ 正确：使用索引
SELECT * FROM materials WHERE material_code = 'MAT001';
```

### 数据备份策略

#### 每日备份脚本
```bash
#!/bin/bash
# 数据库备份脚本
DATE=$(date +%Y%m%d_%H%M%S)
BACKUP_DIR="/backup/mysql"
DB_NAME="mes_db"

# 创建备份目录
mkdir -p $BACKUP_DIR

# 执行备份
mysqldump -u root -pQwe.123 --single-transaction --routines --triggers $DB_NAME > $BACKUP_DIR/mes_db_$DATE.sql

# 压缩备份文件
gzip $BACKUP_DIR/mes_db_$DATE.sql

# 删除7天前的备份
find $BACKUP_DIR -name "mes_db_*.sql.gz" -mtime +7 -delete
```

#### 数据恢复流程
```bash
# 1. 停止应用服务
systemctl stop mes-service

# 2. 创建恢复用数据库
mysql -u root -pQwe.123 -e "CREATE DATABASE mes_db_restore;"

# 3. 恢复数据
gunzip -c /backup/mysql/mes_db_20250610_120000.sql.gz | mysql -u root -pQwe.123 mes_db_restore

# 4. 验证数据完整性
mysql -u root -pQwe.123 mes_db_restore -e "SELECT COUNT(*) FROM materials;"

# 5. 切换数据库
mysql -u root -pQwe.123 -e "DROP DATABASE mes_db; RENAME DATABASE mes_db_restore TO mes_db;"

# 6. 重启应用服务
systemctl start mes-service
```

## 🔍 故障排查

### 常见问题及解决方案

#### 连接问题
```
问题：Unable to connect to any of the specified MySQL hosts
解决：
1. 检查MySQL服务是否启动
2. 检查防火墙设置
3. 验证连接字符串中的服务器地址和端口
4. 确认用户名和密码正确
```

#### 字符集问题
```
问题：中文字符显示为乱码
解决：
1. 确保数据库字符集为utf8mb4
2. 检查连接字符串中的charset参数
3. 验证表和字段的字符集设置
```

#### 性能问题
```
问题：查询响应缓慢
解决：
1. 使用EXPLAIN分析查询计划
2. 检查是否缺少必要的索引
3. 优化SQL语句，避免全表扫描
4. 考虑分页查询大数据集
```

### 监控和日志

#### 性能监控SQL
```sql
-- 查看当前连接数
SHOW STATUS LIKE 'Threads_connected';

-- 查看慢查询
SHOW STATUS LIKE 'Slow_queries';

-- 查看表锁等待
SHOW STATUS LIKE 'Table_locks_waited';

-- 查看当前正在执行的查询
SHOW PROCESSLIST;
```

#### 日志配置
```ini
# my.cnf 配置
[mysqld]
# 启用慢查询日志
slow_query_log = 1
slow_query_log_file = /var/log/mysql/slow.log
long_query_time = 2

# 启用错误日志
log_error = /var/log/mysql/error.log

# 启用二进制日志
log_bin = /var/log/mysql/mysql-bin.log
expire_logs_days = 7
```

---

**维护提醒**: 定期检查数据库性能，及时优化慢查询，确保系统稳定运行。数据备份是系统安全的重要保障，必须严格执行备份策略。