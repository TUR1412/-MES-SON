# MES.DAL 数据访问层使用指南

> **🎉 项目状态：DAL层架构建设完成！**
> **完成时间：2025-06-05 09:34**
> **编译状态：✅ 成功**
> **可用性：L成员可立即开始开发，H/S成员需先创建模型类**

## 📋 概述

MES.DAL是MES制造执行系统的数据访问层，提供统一的数据库操作接口。采用泛型基类设计，支持MySQL数据库，具备完善的异常处理和日志记录机制。

## 🏗️ 架构设计

### 核心组件

- **DatabaseHelper**: 数据库操作助手，提供连接管理、参数化查询、事务处理
- **BaseDAL<T>**: 泛型基类，提供通用CRUD操作
- **具体DAL类**: 继承BaseDAL，实现业务特定的数据访问逻辑

### 目录结构

```
MES.DAL/
├── Core/
│   └── DatabaseHelper.cs          # 数据库操作核心类
├── Base/
│   └── BaseDAL.cs                 # 泛型基类
├── System/
│   └── UserDAL.cs                 # 用户数据访问
├── Material/
│   ├── MaterialDAL.cs             # 物料数据访问
│   └── BOMDAL.cs                  # BOM数据访问
├── Production/
│   └── ProductionOrderDAL.cs      # 生产订单数据访问（H成员负责）
└── Workshop/
    └── WorkshopDAL.cs             # 车间数据访问（S成员负责）
```

## 🚀 快速开始

### 1. 配置数据库连接

在App.config中配置连接字符串：

```xml
<connectionStrings>
  <add name="MESDatabase" 
       connectionString="Server=localhost;Database=mes_db;Uid=root;Pwd=123456;Charset=utf8mb4;" />
</connectionStrings>
```

### 2. 基本使用示例

```csharp
using MES.DAL.Material;
using MES.Models.Material;

// 创建DAL实例
var materialDAL = new MaterialDAL();

// 查询所有物料
var materials = materialDAL.GetAll();

// 根据ID查询
var material = materialDAL.GetById(1);

// 根据条件查询
var categoryMaterials = materialDAL.GetByCategory("原材料");

// 添加物料
var newMaterial = new MaterialInfo
{
    MaterialCode = "M001",
    MaterialName = "钢板",
    Category = "原材料",
    Unit = "kg",
    CreateUserName = "admin"
};
bool success = materialDAL.Add(newMaterial);

// 更新物料
material.MaterialName = "优质钢板";
material.UpdateUserName = "admin";
materialDAL.Update(material);

// 删除物料（逻辑删除）
materialDAL.Delete(1);
```

## 📚 详细使用说明

### DatabaseHelper 使用

```csharp
using MES.DAL.Core;

// 测试数据库连接
bool isConnected = DatabaseHelper.TestConnection();

// 执行查询
var dataTable = DatabaseHelper.ExecuteQuery(
    "SELECT * FROM material_info WHERE category = @category",
    DatabaseHelper.CreateParameter("@category", "原材料")
);

// 执行非查询操作
int rowsAffected = DatabaseHelper.ExecuteNonQuery(
    "UPDATE material_info SET stock_quantity = @stock WHERE id = @id",
    DatabaseHelper.CreateParameter("@stock", 100),
    DatabaseHelper.CreateParameter("@id", 1)
);

// 执行事务
var operations = new List<(string sql, MySqlParameter[] parameters)>
{
    ("INSERT INTO ...", parameters1),
    ("UPDATE ...", parameters2)
};
bool success = DatabaseHelper.ExecuteTransaction(operations);
```

### 自定义DAL类开发

继承BaseDAL<T>创建自定义DAL类：

```csharp
public class CustomDAL : BaseDAL<CustomModel>
{
    protected override string TableName => "custom_table";
    
    // 实现业务特定方法
    public List<CustomModel> GetByCustomCondition(string condition)
    {
        return GetByCondition("custom_field = @condition", 
            DatabaseHelper.CreateParameter("@condition", condition));
    }
    
    // 实现SQL构建方法
    protected override (string sql, MySqlParameter[] parameters) BuildInsertSql(CustomModel entity)
    {
        string sql = "INSERT INTO custom_table (field1, field2) VALUES (@field1, @field2)";
        var parameters = new[]
        {
            DatabaseHelper.CreateParameter("@field1", entity.Field1),
            DatabaseHelper.CreateParameter("@field2", entity.Field2)
        };
        return (sql, parameters);
    }
    
    protected override (string sql, MySqlParameter[] parameters) BuildUpdateSql(CustomModel entity)
    {
        string sql = "UPDATE custom_table SET field1 = @field1, field2 = @field2 WHERE id = @id";
        var parameters = new[]
        {
            DatabaseHelper.CreateParameter("@field1", entity.Field1),
            DatabaseHelper.CreateParameter("@field2", entity.Field2),
            DatabaseHelper.CreateParameter("@id", entity.Id)
        };
        return (sql, parameters);
    }
}
```

## 🔧 最佳实践

### 1. 异常处理

所有DAL操作都会自动包装异常为MESException，包含详细的错误信息和日志记录。

### 2. 参数化查询

始终使用参数化查询防止SQL注入：

```csharp
// ✅ 正确做法
var materials = GetByCondition("material_name LIKE @name", 
    DatabaseHelper.CreateParameter("@name", $"%{searchText}%"));

// ❌ 错误做法 - 容易SQL注入
var sql = $"SELECT * FROM material_info WHERE material_name LIKE '%{searchText}%'";
```

### 3. 事务处理

对于需要保证数据一致性的操作，使用事务：

```csharp
var operations = new List<(string sql, MySqlParameter[] parameters)>();
// 添加多个操作...
bool success = DatabaseHelper.ExecuteTransaction(operations);
```

### 4. 日志记录

DAL层会自动记录操作日志，包括成功和失败的操作。

## 👥 团队协作指南

### H成员（生产管理）

负责完善 `Production/ProductionOrderDAL.cs`：

1. 根据实际业务需求完善ProductionOrderInfo模型
2. 实现BuildInsertSql和BuildUpdateSql方法
3. 添加生产管理特有的查询方法
4. 创建相关的数据模型类

### S成员（车间管理）

负责完善 `Workshop/WorkshopDAL.cs`：

1. 根据实际业务需求完善WorkshopInfo模型
2. 实现BuildInsertSql和BuildUpdateSql方法
3. 添加车间管理特有的查询方法
4. 创建相关的数据模型类

### L成员（物料管理）

可以直接使用已完成的：

- `Material/MaterialDAL.cs`
- `Material/BOMDAL.cs`

根据需要可以扩展更多物料相关的DAL类。

## ⚠️ 注意事项

1. **数据模型**: 所有实体类必须继承BaseModel
2. **命名约定**: 表名使用下划线命名，属性名使用驼峰命名
3. **逻辑删除**: 系统使用逻辑删除，不进行物理删除
4. **时间字段**: CreateTime和UpdateTime会自动设置
5. **连接管理**: 使用using语句确保连接正确释放

## 🔍 故障排除

### 常见问题

1. **连接失败**: 检查连接字符串配置和MySQL服务状态
2. **编译错误**: 确保引用了正确的NuGet包和项目引用
3. **运行时异常**: 查看日志文件获取详细错误信息

### 调试技巧

1. 使用DatabaseHelper.TestConnection()测试连接
2. 查看LogManager记录的详细日志
3. 使用try-catch捕获MESException获取具体错误信息

## 📞 技术支持

如有问题，请联系：
- 架构负责人：天帝（组长）
- 或查看项目文档：docs/DEVELOPMENT_GUIDE.md
