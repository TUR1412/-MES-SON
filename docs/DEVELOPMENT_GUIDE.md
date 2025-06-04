# MES系统开发指南

## 1. 开发环境配置

### 必需软件
- **Visual Studio 2022** (包含.NET桌面开发工作负载)
- **MySQL 8.0** 数据库服务器
- **Git** 版本控制工具
- **MySQL Workbench** (可选，数据库管理工具)

### 环境配置步骤
1. 安装Visual Studio 2022，确保包含以下组件：
   - .NET Framework 4.8 开发工具
   - Windows Forms 设计器
   - NuGet 包管理器

2. 安装MySQL 8.0：
   - 下载并安装MySQL Server 8.0
   - 设置root用户密码为：`123456`（开发环境）
   - 确保MySQL服务正常启动

3. 配置数据库：
   ```sql
   -- 执行 database/init_database.sql 脚本
   mysql -u root -p < database/init_database.sql
   ```

## 2. 项目结构说明

```
MES-SON/
├── MES.sln                          # 解决方案文件
├── src/                             # 源代码目录
│   ├── MES.UI/                      # 用户界面层 (WinForms)
│   │   ├── Forms/                   # 窗体文件
│   │   │   ├── Material/            # 物料管理窗体 (L成员)
│   │   │   ├── Production/          # 生产管理窗体 (H成员)
│   │   │   └── Workshop/            # 车间管理窗体 (S成员)
│   │   └── Controls/                # 自定义控件
│   ├── MES.BLL/                     # 业务逻辑层
│   ├── MES.DAL/                     # 数据访问层
│   ├── MES.Models/                  # 数据模型层
│   └── MES.Common/                  # 公共组件层
├── docs/                            # 文档目录
├── database/                        # 数据库脚本
└── config/                          # 配置文件
```

## 3. 开发规范

### 命名规范
- **类名**: 使用PascalCase，如 `MaterialInfo`
- **方法名**: 使用PascalCase，如 `GetMaterialList`
- **属性名**: 使用PascalCase，如 `MaterialCode`
- **字段名**: 使用camelCase，如 `_materialService`
- **常量**: 使用UPPER_CASE，如 `MAX_RETRY_COUNT`

### 代码注释规范
```csharp
/// <summary>
/// 获取物料信息列表
/// </summary>
/// <param name="pageIndex">页码</param>
/// <param name="pageSize">页大小</param>
/// <returns>物料信息列表</returns>
public List<MaterialInfo> GetMaterialList(int pageIndex, int pageSize)
{
    // 实现代码...
}
```

### 异常处理规范
```csharp
try
{
    // 业务逻辑代码
}
catch (MESException ex)
{
    // 记录业务异常
    LogManager.Error($"业务异常: {ex.Message}", ex);
    throw;
}
catch (Exception ex)
{
    // 记录系统异常
    LogManager.Error($"系统异常: {ex.Message}", ex);
    throw new MESException("系统内部错误", ex);
}
```

## 4. 团队协作流程

### Git分支策略
- **main**: 稳定发布分支
- **develop**: 主开发分支
- **feature/L-功能名**: L成员功能分支
- **feature/H-功能名**: H成员功能分支
- **feature/S-功能名**: S成员功能分支

### 开发流程
1. **开始新功能开发**:
   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b feature/L-material-management
   ```

2. **提交代码**:
   ```bash
   git add .
   git commit -m "L: 物料管理 - 完成物料信息新增功能"
   git push origin feature/L-material-management
   ```

3. **创建Pull Request**:
   - 在GitHub上创建PR
   - 目标分支选择 `develop`
   - 添加详细的功能描述
   - @天帝 进行代码审查

### 提交信息格式
```
[成员标识]: [模块名] - [具体功能描述]

示例:
L: 物料管理 - 完成BOM编辑功能
H: 生产管理 - 实现生产订单查询
S: 车间管理 - 添加在制品状态更新
```

## 5. 模块分工详情

### L成员 - 物料与工艺规则配置模块
**负责功能**:
- 物料信息管理 (`Forms/Material/MaterialForm.cs`)
- BOM管理 (`Forms/Material/BOMForm.cs`)
- 工艺路线管理 (`Forms/Material/ProcessRouteForm.cs`)

**相关文件**:
- `Models/Material/MaterialInfo.cs`
- `DAL/Material/MaterialDAL.cs`
- `BLL/Material/MaterialBLL.cs`

### H成员 - 生产订单、执行核心及用户权限模块
**负责功能**:
- 生产订单管理 (`Forms/Production/ProductionOrderForm.cs`)
- 生产执行管理 (`Forms/Production/ProductionExecutionForm.cs`)
- 用户权限管理 (`Forms/System/UserPermissionForm.cs`)

**相关文件**:
- `Models/Production/ProductionOrder.cs`
- `DAL/Production/ProductionOrderDAL.cs`
- `BLL/Production/ProductionOrderBLL.cs`

### S成员 - 车间作业与辅助管理模块
**负责功能**:
- 车间作业管理 (`Forms/Workshop/WorkshopOperationForm.cs`)
- 在制品管理 (`Forms/Workshop/WIPForm.cs`)
- 设备管理 (`Forms/Workshop/EquipmentForm.cs`)

**相关文件**:
- `Models/Workshop/WorkshopInfo.cs`
- `DAL/Workshop/WorkshopDAL.cs`
- `BLL/Workshop/WorkshopBLL.cs`

## 6. 数据库操作规范

### 连接字符串使用
```csharp
// 获取当前环境的连接字符串
string connectionString = ConfigManager.GetCurrentConnectionString();
```

### 数据访问模式
```csharp
// 继承BaseDAL，使用统一的数据访问模式
public class MaterialDAL : BaseDAL<MaterialInfo>
{
    public List<MaterialInfo> GetByCategory(string category)
    {
        // 实现具体查询逻辑
    }
}
```

## 7. 调试与测试

### 本地调试
1. 确保MySQL服务运行
2. 检查App.config中的连接字符串
3. 设置MES.UI为启动项目
4. 按F5开始调试

### 单元测试
- 为每个BLL类编写单元测试
- 使用Mock对象模拟DAL层
- 测试覆盖率目标：80%以上

## 8. 常见问题解决

### 数据库连接失败
1. 检查MySQL服务是否启动
2. 验证连接字符串配置
3. 确认数据库用户权限

### 编译错误
1. 清理解决方案：Build -> Clean Solution
2. 重新生成：Build -> Rebuild Solution
3. 检查NuGet包引用

### Git冲突解决
1. 拉取最新代码：`git pull origin develop`
2. 解决冲突文件
3. 提交合并结果：`git commit -m "解决合并冲突"`

## 9. 联系方式

- **项目组长（天帝）**: 负责架构设计、代码审查、技术决策
- **L成员**: 物料管理模块
- **H成员**: 生产管理模块  
- **S成员**: 车间管理模块

如有技术问题，请及时在团队群中沟通或创建GitHub Issue。
