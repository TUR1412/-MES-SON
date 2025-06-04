# MES项目Git工作流程指南

## 📋 分支架构概览

```
📦 MES项目仓库 (https://github.com/TUR1412/-MES-SON.git)
├── 🔥 main分支 (生产稳定版)
│   ├── 完全可运行的MES系统
│   ├── 经过测试的稳定代码
│   └── 每次合并都是里程碑
│
├── 🚀 develop分支 (开发集成)
│   ├── 团队协作的主分支
│   ├── 接收所有功能分支
│   └── 定期合并到main
│
└── 🌿 功能分支 (feature/成员-模块)
    ├── feature/L-material-management (L成员-物料管理)
    ├── feature/H-production-management (H成员-生产管理)
    └── feature/S-workshop-management (S成员-车间管理)
```

## 👥 团队成员角色与权限

### 🎯 天帝 (组长)
**权限**: 所有分支的读写权限
**职责**:
- 维护main和develop分支
- 审查和合并Pull Request
- 制定技术决策和架构调整
- 解决代码冲突

### 👨‍💻 L成员 (物料管理)
**权限**: develop分支读权限，feature/L-*分支写权限
**负责模块**: 物料信息管理、BOM管理、工艺路线配置

### 👨‍💻 H成员 (生产管理)
**权限**: develop分支读权限，feature/H-*分支写权限
**负责模块**: 生产订单管理、生产执行控制、用户权限管理

### 👨‍💻 S成员 (车间管理)
**权限**: develop分支读权限，feature/S-*分支写权限
**负责模块**: 车间作业管理、在制品管理、设备状态管理

## 🚀 开发工作流程

### 📥 初始环境搭建

```bash
# 1. 克隆项目
git clone https://github.com/TUR1412/-MES-SON.git
cd MES-SON

# 2. 查看所有分支
git branch -a

# 3. 切换到develop分支
git checkout develop

# 4. 确保本地develop是最新的
git pull origin develop
```

### 🌿 创建功能分支

**L成员示例**:
```bash
# 确保在develop分支
git checkout develop
git pull origin develop

# 创建物料管理功能分支
git checkout -b feature/L-material-management

# 推送到远程仓库
git push -u origin feature/L-material-management
```

**H成员示例**:
```bash
# 创建生产管理功能分支
git checkout develop
git pull origin develop
git checkout -b feature/H-production-management
git push -u origin feature/H-production-management
```

**S成员示例**:
```bash
# 创建车间管理功能分支
git checkout develop
git pull origin develop
git checkout -b feature/S-workshop-management
git push -u origin feature/S-workshop-management
```

### 💻 日常开发流程

```bash
# 1. 开始工作前，确保分支是最新的
git checkout feature/你的分支名
git pull origin feature/你的分支名

# 2. 进行开发工作
# 编辑代码、添加功能...

# 3. 提交更改
git add .
git commit -m "功能描述: 具体修改内容"

# 4. 推送到远程分支
git push origin feature/你的分支名

# 5. 重复步骤2-4直到功能完成
```

### 🔄 同步develop分支更新

```bash
# 当develop分支有新更新时，同步到你的功能分支
git checkout feature/你的分支名
git fetch origin
git merge origin/develop

# 如果有冲突，解决冲突后
git add .
git commit -m "合并develop分支的更新"
git push origin feature/你的分支名
```

## 📝 提交规范

### 🏷️ 提交消息格式
```
成员标识: 功能模块 - 具体描述

示例:
L: 物料管理 - 添加物料信息CRUD功能
H: 生产管理 - 实现生产订单状态更新
S: 车间管理 - 完成设备状态监控界面
天帝: 系统架构 - 优化数据库连接池配置
```

### ✅ 提交最佳实践
- **频繁提交**: 每完成一个小功能就提交
- **清晰描述**: 提交消息要说明做了什么
- **原子提交**: 每次提交只包含一个逻辑变更
- **测试通过**: 提交前确保代码可以编译运行

## 🔀 Pull Request流程

### 📤 创建Pull Request

1. **功能开发完成后**，在GitHub上创建Pull Request
2. **源分支**: feature/你的分支名
3. **目标分支**: develop
4. **标题格式**: `[成员] 模块名称 - 功能描述`
5. **描述内容**:
   ```markdown
   ## 功能描述
   - 实现了什么功能
   - 解决了什么问题
   
   ## 测试情况
   - [ ] 编译通过
   - [ ] 功能测试通过
   - [ ] 无明显bug
   
   ## 相关文件
   - 新增文件列表
   - 修改文件列表
   ```

### 👀 代码审查流程

**天帝审查清单**:
- [ ] 代码风格符合项目规范
- [ ] 功能实现正确完整
- [ ] 没有明显的性能问题
- [ ] 注释清晰，文档完整
- [ ] 测试通过，无编译错误

### ✅ 合并流程

1. **天帝审查通过**后，合并到develop分支
2. **删除功能分支**（可选）
3. **通知团队**develop分支有更新

## 🔄 分支同步策略

### 📅 定期同步计划

**每日同步** (工作日):
- 团队成员同步develop分支到自己的功能分支
- 解决可能的合并冲突

**每周合并** (周五):
- 天帝将稳定的develop分支合并到main分支
- 创建版本标签

**里程碑发布**:
- 重大功能完成时，从main分支创建release标签
- 更新项目文档和版本说明

### 🚨 冲突解决

**预防冲突**:
- 频繁同步develop分支
- 避免修改他人负责的文件
- 提前沟通可能的冲突点

**解决冲突**:
```bash
# 1. 拉取最新的develop分支
git fetch origin
git checkout develop
git pull origin develop

# 2. 合并到你的功能分支
git checkout feature/你的分支名
git merge develop

# 3. 解决冲突文件
# 编辑冲突文件，保留正确的代码

# 4. 标记冲突已解决
git add 冲突文件名

# 5. 完成合并
git commit -m "解决与develop分支的合并冲突"

# 6. 推送更新
git push origin feature/你的分支名
```

## 📊 分支状态监控

### 🔍 常用检查命令

```bash
# 查看所有分支状态
git branch -a

# 查看当前分支状态
git status

# 查看提交历史
git log --oneline -10

# 查看分支差异
git diff develop..feature/你的分支名

# 查看远程分支状态
git remote show origin
```

### 📈 进度跟踪

**个人进度**:
- 每日记录开发进度
- 及时推送代码到远程分支
- 主动报告遇到的问题

**团队进度**:
- 每周团队同步会议
- GitHub Projects看板管理
- 里程碑进度跟踪

## ⚠️ 注意事项

### 🚫 禁止操作
- **禁止直接推送到main分支**
- **禁止强制推送** (`git push -f`)
- **禁止删除他人的分支**
- **禁止修改他人负责的核心文件**

### ✅ 推荐操作
- **经常拉取更新**
- **及时解决冲突**
- **保持提交历史清晰**
- **主动沟通协作**

## 🆘 常见问题解决

### Q: 忘记切换分支，在develop上直接开发了怎么办？
```bash
# 1. 暂存当前更改
git stash

# 2. 切换到正确的功能分支
git checkout feature/你的分支名

# 3. 恢复更改
git stash pop
```

### Q: 功能分支落后develop很多版本怎么办？
```bash
# 1. 备份当前工作
git checkout feature/你的分支名
git push origin feature/你的分支名

# 2. 重新基于最新develop创建分支
git checkout develop
git pull origin develop
git checkout -b feature/你的分支名-new

# 3. 合并旧分支的更改
git merge feature/你的分支名
```

### Q: 提交了错误的代码怎么办？
```bash
# 撤销最后一次提交（保留更改）
git reset --soft HEAD~1

# 撤销最后一次提交（丢弃更改）
git reset --hard HEAD~1
```

---
**文档维护**: 天帝 (组长)  
**最后更新**: 2025年6月4日10:54:32  
**版本**: v1.0.0
