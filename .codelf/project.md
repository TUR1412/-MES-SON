## MES制造执行系统 (Manufacturing Execution System)

> 企业级制造执行系统，用于管理和控制生产过程，实现从生产订单到产品完成的全流程数字化管理

> 项目目标：为制造企业提供完整的生产管理解决方案，包括物料管理、生产管理、车间管理等核心模块

> 项目状态：L成员物料管理已完成，制定三人同步开发计划，天帝开发时间至6月12日

> 项目团队：天帝(组长/系统架构)、L成员(物料管理)、H成员(生产管理)、S成员(车间管理)

> 技术栈：C# .NET Framework 4.8 + WinForms + MySQL 8.0，采用三层架构(UI/BLL/DAL/Models/Common)，Git版本控制，GitHub协作

> 特色：采用设计器+动态代码的混合界面架构，既支持可视化设计又保持系统灵活性，适合企业级应用开发

## Dependencies

* .NET Framework 4.8: 应用程序运行时框架
* System.Windows.Forms: WinForms桌面应用程序界面框架
* System.Data: 数据访问和操作
* MySQL.Data (8.0.33): MySQL数据库连接驱动(计划中)
* System.Configuration: 应用程序配置管理

## Development Environment

> 开发环境要求：
> - Visual Studio 2019/2022 或 VS Code
> - .NET Framework 4.8 SDK
> - Git for Windows
> - MySQL 8.0 (可选，用于数据库开发)
>
> 编译运行：
> - dotnet build MES.sln (编译整个解决方案)
> - src\MES.UI\bin\Debug\MES.UI.exe (运行主程序)
>
> Git工作流程：详见 docs/GIT_WORKFLOW.md


## Structure (三层架构 + 文档体系)

> MES系统采用标准的三层架构设计，清晰分离表示层、业务逻辑层和数据访问层，便于维护和扩展

```
root
├── .gitignore                          # Git忽略文件配置
├── .codelf/                            # Augment项目配置目录
│   ├── project.md                      # 项目主配置文件(当前文件)
│   ├── attention.md                    # 重要注意事项
│   └── _changelog.md                   # 变更日志
├── database/                           # 数据库相关文件
│   └── init_database.sql              # 数据库初始化脚本，包含用户、物料、生产等基础表结构
├── docs/                               # 项目文档目录(重要)
│   ├── Git工作流程.md                  # Git工作流程指南，团队协作必读
│   ├── 项目总览.md                     # 项目进度记录，实时更新开发状态
│   ├── 三人同步开发详细指导.md          # 三人同步开发详细指导，无等待策略
│   ├── 开发指南.md                     # 团队开发指南和最佳实践
│   └── PR_REVIEW_6.md                 # PR #6代码审查报告，L成员物料管理模块审查
├── MES.sln                            # Visual Studio解决方案文件，包含所有项目引用
├── README.md                          # 项目说明文档，项目概览和快速开始
└── src/                               # 源代码目录，三层架构实现
    ├── MES.Common/                    # 公共组件层(重要基础设施)
    │   ├── Configuration/
    │   │   └── ConfigManager.cs       # 配置管理器，统一管理系统配置，支持多环境
    │   ├── Exceptions/
    │   │   └── MESException.cs        # 自定义异常类，统一异常处理体系
    │   ├── Logging/
    │   │   ├── LogLevel.cs            # 日志级别枚举定义
    │   │   └── LogManager.cs          # 日志管理器，提供统一的日志记录功能
    │   ├── Properties/
    │   │   └── AssemblyInfo.cs        # 程序集信息
    │   └── MES.Common.csproj          # 公共组件项目文件
    ├── MES.Models/                    # 数据模型层(重要数据结构)
    │   ├── Base/
    │   │   └── BaseModel.cs           # 基础模型类，提供通用属性(ID、创建时间等)
    │   ├── Material/                  # 物料相关模型(L成员负责)
    │   │   ├── BOMInfo.cs             # BOM物料清单模型，定义产品组成结构
    │   │   └── MaterialInfo.cs        # 物料信息模型，包含物料基本属性和分类
    │   ├── Properties/
    │   │   └── AssemblyInfo.cs        # 程序集信息
    │   └── MES.Models.csproj          # 数据模型项目文件
    ├── MES.DAL/                       # 数据访问层(待开发)
    │   ├── Properties/
    │   │   └── AssemblyInfo.cs        # 程序集信息
    │   └── MES.DAL.csproj             # 数据访问层项目文件，引用Common和Models
    ├── MES.BLL/                       # 业务逻辑层(L成员已完成物料管理)
    │   ├── Material/                  # 物料管理业务逻辑(L成员已完成)
    │   │   ├── MaterialBLL.cs         # 物料业务逻辑类(已完成)
    │   │   ├── BOMBLL.cs              # BOM业务逻辑类(已完成)
    │   │   ├── IMaterialBLL.cs        # 物料业务接口(已完成)
    │   │   └── IBOMBLL.cs             # BOM业务接口(已完成)
    │   ├── Properties/
    │   │   └── AssemblyInfo.cs        # 程序集信息
    │   └── MES.BLL.csproj             # 业务逻辑层项目文件，引用DAL、Models、Common
    └── MES.UI/                        # 用户界面层(重要主程序)
        ├── Forms/                     # 窗体目录
        │   ├── MainForm.cs            # 主窗体代码，实现混合界面架构，包含菜单、工具栏、导航树
        │   ├── MainForm.Designer.cs   # 主窗体设计器代码，定义界面布局和控件
        │   └── MainForm.resx          # 主窗体资源文件
        ├── Properties/                # 应用程序属性
        │   ├── AssemblyInfo.cs        # 程序集信息
        │   ├── Resources.Designer.cs  # 资源文件设计器代码
        │   ├── Resources.resx         # 应用程序资源
        │   ├── Settings.Designer.cs   # 设置文件设计器代码
        │   └── Settings.settings      # 应用程序设置
        ├── App.config                 # 应用程序配置文件
        ├── Program.cs                 # 程序入口点，包含异常处理和日志初始化
        └── MES.UI.csproj              # UI层项目文件，引用BLL、Models、Common
```

### 重要文件详细说明

**核心架构文件**:
- `MES.Common/LogManager.cs`: 全局日志管理，支持多级别日志记录，自动文件轮转
- `MES.Common/ConfigManager.cs`: 配置管理中心，支持系统标题、版本等配置
- `MES.Models/Base/BaseModel.cs`: 所有数据模型的基类，提供ID、创建时间等通用属性
- `MES.UI/Forms/MainForm.cs`: 主界面实现，采用设计器+动态代码混合架构

**团队协作文件**:
- `docs/Git工作流程.md`: 详细的Git分支管理策略和团队协作流程
- `docs/项目总览.md`: 实时项目进度跟踪和里程碑记录
- `docs/三人同步开发详细指导.md`: 三人完全同步开发计划，无等待策略
- `docs/开发指南.md`: 团队开发指南和最佳实践
- `docs/PR_REVIEW_6.md`: L成员物料管理模块代码审查报告，包含详细问题分析和修复建议



**开发规范**:
- 所有业务模型继承自BaseModel
- 统一使用LogManager进行日志记录
- 异常处理统一使用MESException
- UI开发采用设计器+代码混合模式
