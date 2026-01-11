# MES-SON · Nova Command Center

> 一个基于 **.NET Framework 4.8 + WinForms** 的桌面端 MES 示例工程，主打“分层清晰 + 运营洞察 + 未来感 UI”。

A WinForms MES sample focused on **clean layering + operational insight + futuristic UI**.

---

## ✨ 关键特性 | Highlights

- **运营洞察 / Operational Insight**：生产风险预警、在制品老化、设备健康、库存告警、质量缺陷、批次良率一屏聚合
- **分层架构 / Clean Layers**：UI → BLL → DAL → MySQL，避免 UI 直连数据库
- **未来感主题 / Nova Theme**：统一设计 Token、卡片化入口、轻量动效
- **安全默认值 / Secure Defaults**：连接串优先从环境变量读取，避免明文密码
- **可观测诊断 / Diagnostics**：内置数据库诊断与环境检查

---

## 🧱 目录结构 | Structure

```text
src/
  MES.UI              # WinForms 客户端（业务窗体）
  MES.UI.Framework    # 设计系统/主题/控件
  MES.BLL             # 业务逻辑层（UI 只调用这里）
  MES.DAL             # 数据访问层（SQL/参数化/连接管理）
  MES.Models          # 领域模型/DTO
  MES.Common          # 配置/日志/通用能力

tests/                # 测试与验证工具
 docs/                # 项目文档
 database/            # 数据库脚本
 helloagents/         # SSOT（架构/模块/方案包）
```

---

## 🚀 快速开始 | Quick Start

### 环境要求 | Requirements
- Windows 10/11
- Visual Studio 2022（建议）或 Build Tools
- .NET Framework 4.8 开发包
- MySQL 8.x

### 构建 | Build
```powershell
nuget restore MES.sln
msbuild MES.sln /t:Build /p:Configuration=Release /p:Platform="Any CPU" /p:GenerateResourceMSBuildArchitecture=x64
```

或使用脚本：
```powershell
./build.ps1
```

### 运行 | Run
- 运行 `src/MES.UI` 生成的可执行文件
- 默认主题：`Nova`

---

## 🔑 数据库连接 | Database

推荐使用环境变量（避免仓库写入真实密码）：
- `MES_CONNECTION_STRING`
- `MES_TEST_CONNECTION_STRING`（可选）
- `MES_PROD_CONNECTION_STRING`（可选）

如果需要本机配置，可修改 `src/MES.UI/App.config`（仅本机使用，不提交）。

---

## 🛰️ 运营洞察 | Operational Insight

洞察模块提供以下指标：
- 生产订单风险分级与延期提醒
- 在制品老化与瓶颈提示
- 设备维护到期与健康评分
- 物料安全库存与最低库存告警
- 质量缺陷热点与良率趋势
- 批次良率偏差识别

---

## 🎨 UI 设计语言 | UI Design

- 统一 Design Tokens（字号/圆角/动效）
- 卡片化信息密度与分组
- 轻量渐变与高对比文本提升可读性

---

## 📚 文档与 SSOT | Documentation

- `docs/`：项目说明文档
- `helloagents/`：架构/模块/变更与方案包（SSOT）

---

## ✅ 建议体验路径 | Suggested Flow

1. 启动主界面 → 进入 **运营洞察**
2. 查看风险摘要 → 快速跳转模块
3. 使用系统管理进行数据库诊断

---

如需定制化扩展或批量脚本能力，可基于 BLL/DAL 层快速演进。

