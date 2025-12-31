# 模块：MES.DAL

## 职责

- 数据访问层（Data Access Layer）
- 负责 MySQL 连接、命令执行、数据读写

## 关键入口

- 连接与通用数据库操作：
  - `src/MES.DAL/Core/DatabaseHelper.cs`

## 安全约定（强制）

- 禁止硬编码默认连接字符串（尤其是包含密码的默认值）
- 禁止在异常/日志中输出明文密码
- SQL 必须参数化，避免注入风险
