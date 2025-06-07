# MES制造执行系统 (Manufacturing Execution System)

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.8-blue.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-orange.svg)](https://www.mysql.com/)
[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-purple.svg)](https://visualstudio.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Last Update](https://img.shields.io/badge/Last%20Update-2025--06--07-green.svg)](#)

## 📋 项目概述

本项目是一套功能完善的制造执行系统(MES)，旨在实现生产过程的数字化管理，提高生产效率和产品质量。系统采用经典的三层架构设计，具有良好的可维护性和可扩展性。

### 🏗️ 技术架构

- **UI层**: Windows Forms (WinForms) - 用户界面层
- **业务逻辑层(BLL)**: C# 类库 - 核心业务逻辑处理
- **数据访问层(DAL)**: C# 类库 - 数据库操作封装
- **数据模型层(Models)**: C# 类库 - 数据实体定义
- **公共组件层(Common)**: C# 类库 - 通用工具和配置
- **数据库**: MySQL 8.0 - 数据存储

## 🚀 快速开始

### 环境要求

- **开发工具**: Visual Studio 2022 (包含.NET桌面开发工作负载)
- **运行环境**: .NET Framework 4.8
- **数据库**: MySQL 8.0
- **版本控制**: Git

### 安装步骤

1. **克隆项目**
   ```bash
   git clone https://github.com/TUR1412/-MES-SON.git
   cd MES-SON
   ```

2. **配置数据库**
   ```sql
   # 创建数据库并导入初始数据
   mysql -u root -p < database/init_database.sql
   ```

3. **配置连接字符串**
   - 编辑 `src/MES.UI/App.config`
   - 修改数据库连接参数

4. **编译运行**
   - 使用Visual Studio 2022打开 `MES.sln`
   - 设置 `MES.UI` 为启动项目
   - 按 F5 运行项目

## 📚 文档导航

- **[开发指南](docs/开发指南.md)** - 完整的开发环境配置和工作流程
- **[系统架构](docs/系统架构.md)** - 详细的技术架构和模块设计
- **[部署运维](docs/部署运维.md)** - 系统部署、配置和运维指南

## 📁 项目结构

```
MES-SON/
├── 📁 src/                          # 源代码目录
│   ├── 📁 MES.UI/                   # 用户界面层
│   │   ├── 📁 Forms/                # 窗体文件
│   │   │   ├── 📁 Material/         # 物料管理窗体
│   │   │   ├── 📁 Production/       # 生产管理窗体
│   │   │   └── 📁 Workshop/         # 车间管理窗体
│   │   └── 📁 Controls/             # 自定义控件
│   ├── 📁 MES.BLL/                  # 业务逻辑层
│   ├── 📁 MES.DAL/                  # 数据访问层
│   ├── 📁 MES.Models/               # 数据模型层
│   └── 📁 MES.Common/               # 公共组件层
├── 📁 docs/                         # 文档目录
├── 📁 database/                     # 数据库脚本
├── 📄 MES.sln                       # 解决方案文件
└── 📄 README.md                     # 项目说明
```

## 👥 团队分工

| 成员 | 职责 | 负责模块 |
|------|------|----------|
| **天帝 (组长)** | 系统架构、代码审查、技术决策 | 框架设计、项目管理 |
| **L 成员** | 物料与工艺规则配置 | 物料管理、BOM管理、工艺路线 |
| **H 成员** | 生产订单与执行核心 | 生产订单、生产执行、用户权限 |
| **S 成员** | 车间作业与辅助管理 | 车间作业、在制品管理、设备管理 |

## 🔧 核心功能

### 物料管理模块 (L成员负责)
- ✅ 物料信息管理
- ✅ BOM物料清单管理
- ✅ 工艺路线配置

### 生产管理模块 (H成员负责)
- ✅ 生产订单管理
- ✅ 生产执行控制
- ✅ 用户权限管理

### 车间管理模块 (S成员负责)
- ✅ 车间作业管理
- ✅ 在制品(WIP)管理
- ✅ 设备状态管理

### 系统管理模块
- ✅ 用户管理
- ✅ 角色权限
- ✅ 系统配置
- ✅ 日志管理

## 🛠️ 开发指南

### Git工作流程

1. **功能开发**
   ```bash
   git checkout develop
   git pull origin develop
   git checkout -b feature/[成员标识]-[功能名称]
   ```

2. **提交代码**
   ```bash
   git add .
   git commit -m "[成员标识]: [模块名] - [功能描述]"
   git push origin feature/[成员标识]-[功能名称]
   ```

3. **创建Pull Request**
   - 目标分支: `develop`
   - 请求审查: @天帝

### 代码规范

- **命名规范**: 遵循C#标准命名约定
- **注释要求**: 所有公共API必须有XML文档注释
- **异常处理**: 使用统一的MES异常处理机制
- **日志记录**: 关键操作必须记录日志

## 📊 数据库设计

### 核心表结构

- `sys_user` - 用户信息表
- `sys_role` - 角色信息表
- `material_info` - 物料信息表
- `bom_info` - BOM物料清单表
- `production_order` - 生产订单表
- `workshop_info` - 车间信息表

详细的数据库设计请参考: [系统架构文档](docs/系统架构.md)

##  版本历史

- **v1.0.0** (2025-06-07) - 项目初始化，基础框架建立，完全可运行版本
- **v1.1.0** (2025-06-07) - 简化Git工作流程，完善文档体系

## 🤝 贡献指南

### 团队成员开发流程
```bash
# 1. 拉取最新代码
git checkout develop
git pull origin develop

# 2. 开发功能
# 进行代码开发...

# 3. 提交代码
git add .
git commit -m "成员标识: 模块 - 功能描述"
git push origin develop
```

### 外部贡献者
1. Fork 本仓库
2. 在develop分支基础上开发
3. 提交Pull Request到develop分支

## 📞 联系方式

- **项目组长**: 天帝 - 负责架构设计和技术决策
- **技术支持**: 请在GitHub Issues中提出问题
- **开发文档**: 详见 [docs/开发指南.md](docs/开发指南.md)

## 📄 许可证

本项目采用 MIT 许可证 - 详情请参阅 [LICENSE](LICENSE) 文件

---

**MES制造执行系统** - 让制造更智能 🏭✨
