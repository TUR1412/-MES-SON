# MES项目Git工作流程与开发规范

> **更新时间：2025-06-05 14:30**

## 🔒 重要通知：分支保护规则已启用！

### ⚠️ 紧急变更（2025-06-05 14:08）
**从即日起，develop和main分支已启用保护规则，不能直接推送！**

#### 🛡️ 分支保护详情
- ✅ **develop分支保护**：强制PR审查 + GitHub Actions检查
- ✅ **main分支保护**：强制PR审查 + GitHub Actions检查
- ❌ **禁止直接推送**：`git push origin develop` 将被拒绝
- ❌ **禁止强制推送**：`git push --force` 将被拒绝
- ✅ **必须通过PR**：所有代码变更必须通过Pull Request

#### 📋 新的工作流程
1. 在功能分支开发 ✅
2. 推送功能分支 ✅
3. **创建Pull Request** ✅
4. **等待GitHub Actions检查通过** ⏳
5. **等待代码审查批准** ⏳
6. **通过GitHub界面合并** ✅

## 📋 分支架构

```
📦 MES项目仓库
├── 🔥 main分支 (生产稳定版)
├── 🚀 develop分支 (开发集成)
└── 🌿 功能分支 (feature/成员-模块)
    ├── feature/L-material-management (L成员-物料管理)
    ├── feature/H-production-management (H成员-生产管理)
    └── feature/S-workshop-management (S成员-车间管理)
```

## 🚀 快速开始

### 📥 初始设置
```bash
# 1. 克隆项目
git clone https://github.com/TUR1412/-MES-SON.git
cd MES-SON

# 2. 切换到develop分支
git checkout develop
git pull origin develop
```

### 🌿 创建功能分支
```bash
# L成员（物料管理）
git checkout -b feature/L-material-management
git push -u origin feature/L-material-management

# H成员（生产管理）
git checkout -b feature/H-production-management
git push -u origin feature/H-production-management

# S成员（车间管理）
git checkout -b feature/S-workshop-management
git push -u origin feature/S-workshop-management
```

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

## 📋 提交规范

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

### 👀 代码审查
**天帝审查清单**:
- [ ] 代码风格符合项目规范
- [ ] 功能实现正确完整
- [ ] 没有明显的性能问题
- [ ] 注释清晰，文档完整
- [ ] 测试通过，无编译错误

### 🔄 PR合并后的重要操作

**⚠️ 关键步骤：所有团队成员都必须执行！**

当任何PR合并到develop后，**所有团队成员**（包括天帝）都需要立即同步本地develop分支：

```bash
# 1. 切换到develop分支
git checkout develop

# 2. 拉取最新的远程更新
git pull origin develop

# 3. 验证同步成功
git log --oneline -3  # 查看最新提交
```

**为什么必须同步？**
- ✅ 确保下次创建功能分支时基于最新代码
- ✅ 避免不必要的合并冲突
- ✅ 保持团队开发进度同步
- ✅ 防止基于过时代码开发新功能

**同步时机：**
- 每次看到有PR被合并时
- 每天开始工作前
- 创建新功能分支前

## 🚨 冲突解决

### 预防冲突
- 频繁同步develop分支
- 避免修改他人负责的文件
- 提前沟通可能的冲突点

### 解决冲突
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

## ⚠️ 注意事项

### 🚫 禁止操作（已强制执行）
- **禁止直接推送到develop分支** ❌ `git push origin develop`
- **禁止直接推送到main分支** ❌ `git push origin main`
- **禁止强制推送** ❌ `git push -f`
- **禁止删除他人的分支**
- **禁止修改他人负责的核心文件**

### 💡 如果遇到推送被拒绝
```bash
# 错误示例（将被拒绝）：
$ git push origin develop
remote: error: GH006: Protected branch update failed for refs/heads/develop.

# 正确做法：
1. 推送到功能分支：git push origin feature/你的分支名
2. 在GitHub上创建Pull Request
3. 等待审查和检查通过
4. 通过GitHub界面合并
```

### ✅ 推荐操作
- **经常拉取更新**
- **及时解决冲突**
- **保持提交历史清晰**
- **主动沟通协作**

## 🆘 常见问题

### Q: 忘记切换分支，在develop上直接开发了怎么办？
```bash
# 1. 暂存当前更改
git stash

# 2. 切换到正确的功能分支
git checkout feature/你的分支名

# 3. 恢复更改
git stash pop
```

### Q: 提交了错误的代码怎么办？
```bash
# 撤销最后一次提交（保留更改）
git reset --soft HEAD~1

# 撤销最后一次提交（丢弃更改）
git reset --hard HEAD~1
```

---
**维护者**: 天帝（组长）  
**最后更新**: 2025年6月5日09:40
