# 模块：MES.Models

## 职责

- 领域模型/实体/DTO 的集中存放位置
- 为 UI/BLL/DAL 提供共享类型（避免循环依赖）

## 开发约定

- Models 不依赖 UI、DAL 的实现细节
- 避免把数据库访问或 WinForms 控件类型带入 Models
