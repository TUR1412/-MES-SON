# MES项目开发规范与标准

> **创建时间**: 2025-06-10 16:15:05
> **适用范围**: MES项目所有开发人员和AI助手
> **强制级别**: 最高优先级，与系统用户指南同等级别

## 🔴 核心约束原则 (绝对遵循)

### 技术栈严格限制
- **C# .NET Framework 4.8** - 严禁使用更高版本语法特性
- **WinForms设计器模式** - 所有窗体控件必须通过Visual Studio设计器创建
- **MySQL 8.0数据库** - 所有数据操作必须通过DAL层
- **企业级三层架构** - 严格遵循UI→BLL→DAL→Database的调用链

### C# 5.0语法强制约束

#### 绝对禁止的语法特性
```csharp
// ❌ 严禁使用的C# 6.0+语法
❌ 字符串插值: $"Hello {name}"
❌ 空传播运算符: obj?.Method()
❌ nameof运算符: nameof(variable)
❌ using static: using static System.Math;
❌ 表达式体成员: public int Value => _value;
❌ 自动属性初始化: public string Name { get; set; } = "Default";
❌ 异常筛选器: catch (Exception ex) when (ex.Message.Contains("error"))
❌ await in catch/finally: await SomeMethodAsync(); (在catch/finally中)
```

#### 强制使用的C# 5.0替代语法
```csharp
// ✅ 正确的C# 5.0语法
✅ 字符串格式化: string.Format("Hello {0}", name)
✅ 空值检查: obj != null ? obj.Method() : defaultValue
✅ 字符串字面量: "variable"
✅ 完整命名空间: System.Math.Sqrt(value)
✅ 传统方法体: public int GetValue() { return _value; }
✅ 构造函数初始化: public MyClass() { Name = "Default"; }
✅ 传统异常处理: catch (Exception ex) { if (ex.Message.Contains("error")) ... }
✅ 传统异步处理: 在try块中使用await
```

### WinForms设计器强制使用规则

#### 绝对禁止的操作
```csharp
// ❌ 严禁在代码中动态创建控件
❌ Button dynamicButton = new Button();
❌ this.Controls.Add(dynamicButton);
❌ this.btnSave.Size = new Size(100, 30);
❌ this.txtName.Location = new Point(10, 10);
❌ TabPage newPage = new TabPage("New Tab");
❌ this.tabControl.TabPages.Add(newPage);
```

#### 强制要求的操作
```csharp
// ✅ 正确：所有控件在Designer.cs中定义
// 在 Form.Designer.cs 文件中：
private void InitializeComponent()
{
    this.btnSave = new System.Windows.Forms.Button();
    this.txtName = new System.Windows.Forms.TextBox();
    // ... 设置属性
    this.btnSave.Name = "btnSave";
    this.btnSave.Size = new System.Drawing.Size(75, 23);
    this.btnSave.Text = "保存";
    this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
}

// ✅ 在窗体代码中只处理业务逻辑
public partial class MaterialForm : Form
{
    public MaterialForm()
    {
        InitializeComponent(); // 只调用设计器初始化
        LoadData(); // 只处理数据加载
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        SaveMaterial(); // 只处理业务逻辑
    }
}
```

## 🏗️ 架构设计规范

### 三层架构职责划分

#### UI层 (MES.UI) - 表示层职责
```csharp
// ✅ UI层应该做的事情
- 数据展示和用户交互
- 基本的输入验证（格式检查）
- 调用BLL层方法处理业务逻辑
- 异常信息的用户友好展示

// ❌ UI层绝对不能做的事情
- 直接访问数据库或调用DAL层
- 包含复杂的业务逻辑
- 直接操作数据库连接
- 包含SQL语句
```

#### BLL层 (MES.BLL) - 业务逻辑层职责
```csharp
// ✅ BLL层应该做的事情
- 所有业务逻辑处理
- 数据验证和业务规则检查
- 权限验证和安全控制
- 调用DAL层进行数据操作
- 事务管理和异常处理

// ❌ BLL层绝对不能做的事情
- 包含UI相关的代码
- 直接操作数据库连接
- 包含具体的SQL语句
- 依赖于特定的UI控件
```

#### DAL层 (MES.DAL) - 数据访问层职责
```csharp
// ✅ DAL层应该做的事情
- 数据库连接管理
- SQL语句执行
- 数据库事务处理
- 数据模型与数据库记录的转换

// ❌ DAL层绝对不能做的事情
- 包含业务逻辑
- 进行数据验证（业务规则验证）
- 直接与UI层交互
- 包含权限检查逻辑
```

### 接口设计规范

#### 接口命名规范
```csharp
// ✅ 正确的接口命名
public interface IMaterialBLL
public interface IEquipmentDAL
public interface IUserService

// ❌ 错误的接口命名
public interface MaterialBLL
public interface EquipmentData
public interface UserManager
```

#### 接口方法设计
```csharp
// ✅ 正确的接口方法设计
public interface IMaterialBLL
{
    /// <summary>
    /// 获取物料信息
    /// </summary>
    /// <param name="materialId">物料ID</param>
    /// <returns>物料信息，如果不存在返回null</returns>
    MaterialInfo GetMaterialById(int materialId);
    
    /// <summary>
    /// 保存物料信息
    /// </summary>
    /// <param name="material">物料信息</param>
    /// <returns>保存成功返回true，失败返回false</returns>
    bool SaveMaterial(MaterialInfo material);
}
```

## 📝 代码编写规范

### 命名规范

#### 控件命名规范
```csharp
// ✅ 正确的控件命名
btnSave, btnDelete, btnSearch     // 按钮：btn + 功能描述
txtMaterialCode, txtProductName  // 文本框：txt + 字段名
lblMaterialCode, lblProductName  // 标签：lbl + 字段名
dgvMaterials, dgvOrders         // 数据网格：dgv + 数据类型
cmbCategory, cmbStatus          // 下拉框：cmb + 字段名
pnlSearch, pnlButtons           // 面板：pnl + 功能区域

// ❌ 错误的控件命名
button1, textBox1, label1       // 使用默认名称
saveBtn, materialTxt            // 前缀后置
materialCodeTextBox             // 过于冗长
```

#### 类和方法命名规范
```csharp
// ✅ 正确的类命名
public class MaterialBLL
public class EquipmentDAL
public class UserInfo

// ✅ 正确的方法命名
public MaterialInfo GetMaterialById(int id)
public bool SaveMaterial(MaterialInfo material)
public List<MaterialInfo> SearchMaterials(string keyword)

// ❌ 错误的命名
public class materialBLL          // 首字母小写
public class Material_BLL         // 使用下划线
public MaterialInfo getMaterial() // 首字母小写
public bool save_material()       // 使用下划线
```

### 异常处理规范

#### 标准异常处理模式
```csharp
// ✅ 正确的异常处理
public bool SaveMaterial(MaterialInfo material)
{
    try
    {
        // 参数验证
        if (material == null)
        {
            LogManager.Warning("保存物料失败：物料信息为空");
            return false;
        }

        // 业务逻辑处理
        var result = _materialDAL.SaveMaterial(material);
        
        if (result)
        {
            LogManager.Info(string.Format("保存物料成功：{0}", material.MaterialCode));
        }
        
        return result;
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("保存物料失败：{0}", ex.Message), ex);
        throw new MESException("保存物料失败", ex);
    }
}

// ✅ UI层的异常处理
private void btnSave_Click(object sender, EventArgs e)
{
    try
    {
        var material = GetMaterialFromUI();
        bool result = _materialBLL.SaveMaterial(material);
        
        if (result)
        {
            MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadMaterialData();
        }
        else
        {
            MessageBox.Show("保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    catch (MESException ex)
    {
        LogManager.Error(string.Format("保存物料失败：{0}", ex.Message), ex);
        MessageBox.Show(string.Format("保存失败：{0}", ex.Message), "错误", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    catch (Exception ex)
    {
        LogManager.Error(string.Format("保存物料时发生未知错误：{0}", ex.Message), ex);
        MessageBox.Show("保存失败：系统错误", "错误", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

### 日志记录规范

#### 日志级别使用标准
```csharp
// ✅ 正确的日志使用
LogManager.Info("用户登录成功：张三");                    // 正常业务操作
LogManager.Warning("物料编码重复：MAT001");               // 业务警告
LogManager.Error("数据库连接失败", ex);                   // 系统错误
LogManager.Debug("查询SQL：SELECT * FROM materials");    // 调试信息

// ❌ 错误的日志使用
LogManager.Error("用户登录成功");                        // 级别使用错误
LogManager.Info("数据库连接失败");                       // 级别使用错误
LogManager.Warning("查询物料列表");                      // 不必要的日志
```

## 🔍 质量检查清单

### 窗体开发11步验证法
1. **设计器检查** - 所有控件在InitializeComponent()中定义
2. **命名规范验证** - 控件命名符合规范
3. **事件绑定检查** - 事件在设计器中绑定
4. **布局验证** - 控件对齐、间距统一
5. **数据绑定检查** - DataSource正确设置
6. **异常处理验证** - 完整的try-catch机制
7. **业务逻辑分离** - UI层不包含业务逻辑
8. **资源释放检查** - 正确释放资源
9. **权限控制验证** - 权限检查正确实现
10. **日志记录确认** - 关键操作记录日志
11. **主界面入口验证** - 菜单入口正确配置

### C# 5.0语法检查清单
- [ ] 检查是否使用了字符串插值 ($"")
- [ ] 检查是否使用了空传播运算符 (?.)
- [ ] 检查是否使用了nameof运算符
- [ ] 检查是否使用了using static语句
- [ ] 检查是否使用了表达式体成员 (=>)
- [ ] 检查是否使用了自动属性初始化器
- [ ] 检查是否使用了异常筛选器 (when)
- [ ] 检查是否在catch/finally中使用了await

### 架构合规性检查清单
- [ ] UI层是否直接调用DAL层
- [ ] BLL层是否包含UI相关代码
- [ ] DAL层是否包含业务逻辑
- [ ] 是否正确使用了接口抽象
- [ ] 是否遵循了依赖倒置原则
- [ ] 异常处理是否完整
- [ ] 日志记录是否规范
- [ ] 资源释放是否正确

## 🚨 违规处理机制

### 违规等级定义
- **A级违规** - 导致编译失败或系统崩溃的严重问题
- **B级违规** - 违反架构原则或编码规范的重要问题  
- **C级违规** - 代码风格或命名不规范的轻微问题

### 处理流程
1. **发现违规** - 通过代码审查或自动检查发现
2. **记录问题** - 在issue-tracking.md中详细记录
3. **立即修复** - A级违规必须立即修复
4. **预防措施** - 制定预防措施避免再次发生
5. **经验总结** - 更新开发规范和检查清单

### 自动检查机制
- **编译前检查** - 使用csharp5-validator.md进行语法检查
- **提交前检查** - Git提交前自动执行规范检查
- **定期扫描** - 定期对整个项目进行规范性扫描
- **持续集成** - 在CI/CD流程中集成质量检查

---

**重要声明**: 本规范为MES项目的技术基石，所有开发人员和AI助手都必须严格遵循。任何违反都将被视为严重的质量问题，必须立即纠正。

---

# 数据库设计与操作指南

> **整合说明**: 原database-guide.md内容已整合到此文档中
> **更新时间**: 2025-06-10 18:52:41

## 🗄️ 数据库架构设计

### 数据库连接配置
```csharp
// 标准数据库连接字符串
private const string CONNECTION_STRING =
    "Server=localhost;Database=mes_db;Uid=root;CharSet=utf8mb4;SslMode=none;";
```

### 核心数据表设计

#### 1. 用户管理表 (已移除)
**注意**: 根据项目需求，用户管理功能已完全移除，不需要相关表结构。

#### 2. 生产管理核心表

**工单表 (work_orders)**
```sql
CREATE TABLE work_orders (
    id INT PRIMARY KEY AUTO_INCREMENT,
    work_order_no VARCHAR(50) UNIQUE NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    status ENUM('待开始', '进行中', '已完成', '已取消') DEFAULT '待开始',
    priority ENUM('低', '中', '高', '紧急') DEFAULT '中',
    planned_start_date DATETIME,
    planned_end_date DATETIME,
    actual_start_date DATETIME,
    actual_end_date DATETIME,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

**产品表 (products)**
```sql
CREATE TABLE products (
    id INT PRIMARY KEY AUTO_INCREMENT,
    product_code VARCHAR(50) UNIQUE NOT NULL,
    product_name VARCHAR(100) NOT NULL,
    specification TEXT,
    unit VARCHAR(20) DEFAULT '个',
    status ENUM('启用', '禁用') DEFAULT '启用',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

**BOM表 (bom_items)**
```sql
CREATE TABLE bom_items (
    id INT PRIMARY KEY AUTO_INCREMENT,
    parent_product_id INT NOT NULL,
    child_product_id INT NOT NULL,
    quantity DECIMAL(10,4) NOT NULL,
    unit VARCHAR(20) DEFAULT '个',
    sequence_no INT DEFAULT 1,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (parent_product_id) REFERENCES products(id),
    FOREIGN KEY (child_product_id) REFERENCES products(id)
);
```

#### 3. 车间管理表

**车间表 (workshops)**
```sql
CREATE TABLE workshops (
    id INT PRIMARY KEY AUTO_INCREMENT,
    workshop_code VARCHAR(50) UNIQUE NOT NULL,
    workshop_name VARCHAR(100) NOT NULL,
    description TEXT,
    status ENUM('启用', '禁用') DEFAULT '启用',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

**设备表 (equipment)**
```sql
CREATE TABLE equipment (
    id INT PRIMARY KEY AUTO_INCREMENT,
    equipment_code VARCHAR(50) UNIQUE NOT NULL,
    equipment_name VARCHAR(100) NOT NULL,
    workshop_id INT NOT NULL,
    equipment_type VARCHAR(50),
    status ENUM('运行', '停机', '维护', '故障') DEFAULT '停机',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (workshop_id) REFERENCES workshops(id)
);
```

#### 4. 工艺管理表

**工艺路线表 (process_routes)**
```sql
CREATE TABLE process_routes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    route_code VARCHAR(50) UNIQUE NOT NULL,
    route_name VARCHAR(100) NOT NULL,
    product_id INT NOT NULL,
    version VARCHAR(20) DEFAULT '1.0',
    status ENUM('启用', '禁用') DEFAULT '启用',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES products(id)
);
```

**工艺步骤表 (process_steps)**
```sql
CREATE TABLE process_steps (
    id INT PRIMARY KEY AUTO_INCREMENT,
    route_id INT NOT NULL,
    step_no INT NOT NULL,
    step_name VARCHAR(100) NOT NULL,
    workshop_id INT NOT NULL,
    equipment_id INT,
    standard_time INT DEFAULT 0, -- 标准工时(分钟)
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (route_id) REFERENCES process_routes(id),
    FOREIGN KEY (workshop_id) REFERENCES workshops(id),
    FOREIGN KEY (equipment_id) REFERENCES equipment(id)
);
```

### 数据库操作规范

#### 1. 连接管理
```csharp
// 使用using语句确保连接正确释放
using (var connection = new MySqlConnection(CONNECTION_STRING))
{
    connection.Open();
    // 执行数据库操作
}
```

#### 2. 参数化查询 (必须)
```csharp
// ✅ 正确的参数化查询
var sql = "SELECT * FROM products WHERE product_code = @code";
var command = new MySqlCommand(sql, connection);
command.Parameters.AddWithValue("@code", productCode);

// ❌ 禁止字符串拼接
var sql = string.Format("SELECT * FROM products WHERE product_code = '{0}'", productCode); // 危险!
```

#### 3. 事务处理
```csharp
using (var transaction = connection.BeginTransaction())
{
    try
    {
        // 执行多个相关操作
        // ...
        transaction.Commit();
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        throw;
    }
}
```

#### 4. 错误处理
```csharp
try
{
    // 数据库操作
}
catch (MySqlException ex)
{
    LogManager.Error(string.Format("数据库操作失败: {0}", ex.Message), ex);
    throw new ApplicationException("数据库操作失败，请稍后重试");
}
```

### 数据库维护规范

#### 1. 备份策略
- **每日备份**: 自动执行数据库完整备份
- **增量备份**: 每小时执行增量备份
- **备份验证**: 定期验证备份文件的完整性

#### 2. 性能优化
- **索引管理**: 为常用查询字段建立适当索引
- **查询优化**: 定期分析慢查询并优化
- **数据清理**: 定期清理过期的临时数据

#### 3. 安全管理
- **访问控制**: 严格控制数据库访问权限
- **密码策略**: 定期更换数据库密码
- **审计日志**: 记录重要的数据库操作日志
