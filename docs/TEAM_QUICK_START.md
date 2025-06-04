# MES团队成员快速上手指南

## 🚀 5分钟快速开始

### 📥 第一步：获取代码
```bash
# 克隆项目
git clone https://github.com/TUR1412/-MES-SON.git
cd MES-SON

# 切换到开发分支
git checkout develop
```

### 🌿 第二步：创建你的功能分支
```bash
# L成员 (物料管理)
git checkout -b feature/L-material-management
git push -u origin feature/L-material-management

# H成员 (生产管理)  
git checkout -b feature/H-production-management
git push -u origin feature/H-production-management

# S成员 (车间管理)
git checkout -b feature/S-workshop-management
git push -u origin feature/S-workshop-management
```

### 🛠️ 第三步：验证环境
```bash
# 编译项目
dotnet build MES.sln

# 运行程序
src\MES.UI\bin\Debug\MES.UI.exe
```

### ✅ 看到MES系统界面就成功了！

## 👨‍💻 开发环境配置

### 🔧 必需工具
- **Visual Studio 2019/2022** 或 **VS Code**
- **.NET Framework 4.8 SDK**
- **Git for Windows**
- **MySQL 8.0** (可选，用于数据库开发)

### 📁 项目结构理解
```
MES-SON/
├── src/
│   ├── MES.Common/     # 公共组件 (日志、配置等)
│   ├── MES.Models/     # 数据模型
│   ├── MES.DAL/        # 数据访问层
│   ├── MES.BLL/        # 业务逻辑层
│   └── MES.UI/         # 用户界面层
├── database/           # 数据库脚本
├── docs/              # 项目文档
└── README.md          # 项目说明
```

## 🎯 各成员开发重点

### 🟢 L成员 - 物料管理模块

**主要文件**:
- `src/MES.Models/Material/` - 物料相关数据模型
- `src/MES.DAL/Material/` - 物料数据访问
- `src/MES.BLL/Material/` - 物料业务逻辑
- `src/MES.UI/Forms/Material/` - 物料管理界面

**开发任务**:
1. **物料信息管理**: 物料的增删改查
2. **BOM管理**: 物料清单的维护
3. **工艺路线配置**: 生产工艺流程设置

**界面设计**:
- 可以在MainForm的panelMain中设计
- 或创建独立的MaterialForm窗体
- 使用设计器拖拽控件

### 🔵 H成员 - 生产管理模块

**主要文件**:
- `src/MES.Models/Production/` - 生产相关数据模型
- `src/MES.DAL/Production/` - 生产数据访问
- `src/MES.BLL/Production/` - 生产业务逻辑
- `src/MES.UI/Forms/Production/` - 生产管理界面

**开发任务**:
1. **生产订单管理**: 订单的创建、跟踪、完成
2. **生产执行控制**: 生产过程的监控
3. **用户权限管理**: 系统用户和角色管理

### 🔴 S成员 - 车间管理模块

**主要文件**:
- `src/MES.Models/Workshop/` - 车间相关数据模型
- `src/MES.DAL/Workshop/` - 车间数据访问
- `src/MES.BLL/Workshop/` - 车间业务逻辑
- `src/MES.UI/Forms/Workshop/` - 车间管理界面

**开发任务**:
1. **车间作业管理**: 作业计划和执行
2. **在制品管理**: WIP状态跟踪
3. **设备状态管理**: 设备监控和维护

## 📝 日常开发流程

### 🌅 每日开始工作
```bash
# 1. 切换到你的功能分支
git checkout feature/你的分支名

# 2. 拉取最新更改
git pull origin feature/你的分支名

# 3. 同步develop分支的更新
git fetch origin
git merge origin/develop

# 4. 开始编码...
```

### 🌆 每日结束工作
```bash
# 1. 添加所有更改
git add .

# 2. 提交更改
git commit -m "你的成员标识: 模块 - 今天完成的功能"

# 3. 推送到远程
git push origin feature/你的分支名
```

### 📋 提交消息示例
```
L: 物料管理 - 完成物料信息增删改查功能
H: 生产管理 - 实现生产订单创建界面
S: 车间管理 - 添加设备状态监控组件
```

## 🖥️ 界面开发指南

### 🎨 设计器使用
1. **打开MainForm.cs[设计]** - 可以看到完整布局
2. **在panelMain中设计** - 主工作区域
3. **或创建新窗体** - 独立的功能窗体
4. **使用treeViewModules** - 左侧导航树

### 🎯 界面规范
- **字体**: 微软雅黑
- **主色调**: 蓝色系 (#337ab7)
- **成功色**: 绿色 (#5cb85c)
- **警告色**: 橙色 (#f0ad4e)
- **错误色**: 红色 (#d9534f)

### 📱 响应式设计
- 支持窗体大小调整
- 使用Dock和Anchor属性
- 考虑不同分辨率的显示效果

## 🔧 常用代码模板

### 📊 数据模型示例
```csharp
// src/MES.Models/你的模块/你的Model.cs
using MES.Models.Base;

namespace MES.Models.你的模块
{
    public class 你的Model : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // 其他属性...
    }
}
```

### 💾 数据访问示例
```csharp
// src/MES.DAL/你的模块/你的DAL.cs
using MES.Models.你的模块;
using MES.Common.Logging;

namespace MES.DAL.你的模块
{
    public class 你的DAL
    {
        public List<你的Model> GetAll()
        {
            try
            {
                // 数据库查询逻辑
                LogManager.Info("查询成功");
                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error("查询失败", ex);
                throw;
            }
        }
    }
}
```

### 🏢 业务逻辑示例
```csharp
// src/MES.BLL/你的模块/你的BLL.cs
using MES.DAL.你的模块;
using MES.Models.你的模块;

namespace MES.BLL.你的模块
{
    public class 你的BLL
    {
        private 你的DAL dal = new 你的DAL();
        
        public bool Add(你的Model model)
        {
            // 业务验证
            if (string.IsNullOrEmpty(model.Name))
                return false;
                
            // 调用数据层
            return dal.Add(model);
        }
    }
}
```

## 🆘 遇到问题怎么办

### 🔍 自助解决
1. **查看错误日志** - `Logs/` 目录下的日志文件
2. **检查编译错误** - Visual Studio错误列表
3. **查阅项目文档** - `docs/` 目录下的文档

### 💬 寻求帮助
1. **GitHub Issues** - 创建问题报告
2. **团队群聊** - 及时沟通
3. **找天帝** - 技术难题和架构问题

### 📋 问题报告模板
```markdown
## 问题描述
简要描述遇到的问题

## 重现步骤
1. 第一步
2. 第二步
3. 第三步

## 期望结果
应该发生什么

## 实际结果
实际发生了什么

## 环境信息
- 操作系统: Windows 10
- Visual Studio版本: 2022
- .NET Framework版本: 4.8
```

## 🎉 完成功能后

### ✅ 自测清单
- [ ] 代码可以正常编译
- [ ] 功能按预期工作
- [ ] 没有明显的bug
- [ ] 界面美观易用
- [ ] 添加了必要的注释

### 📤 提交Pull Request
1. 推送最终代码到你的功能分支
2. 在GitHub上创建Pull Request
3. 目标分支选择 `develop`
4. 填写详细的PR描述
5. 等待天帝审查和合并

### 🎊 庆祝完成
恭喜你完成了MES系统的一个重要模块！
你的贡献让整个团队更接近成功！🎉

---
**快速上手指南** - 让每个团队成员都能快速开始贡献代码  
**维护者**: 天帝 (组长)  
**最后更新**: 2025年6月4日10:54:32
