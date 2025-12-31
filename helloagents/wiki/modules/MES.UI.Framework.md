# 模块：MES.UI.Framework

## 职责

- UI Framework / Design System 基座
- 提供可复用的主题应用、设计 Token、控件扩展，统一视觉语言

## 关键入口

- 设计 Token：
  - `src/MES.UI.Framework/Themes/DesignTokens.cs`
- 主题应用器：
  - `src/MES.UI.Framework/Themes/LolV2ThemeApplier.cs`

## 开发约定

- 新增 UI 视觉规则应优先沉淀为 Token（字体/颜色/圆角/间距）
- 允许渐进式替换历史硬编码，但禁止新增新的硬编码风格债务
