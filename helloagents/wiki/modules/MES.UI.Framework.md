# 模块：MES.UI.Framework

## 职责

- UI Framework / Design System 基座
- 提供可复用的主题应用、设计 Token、控件扩展，统一视觉语言

## 关键入口

- 设计 Token：
  - `src/MES.UI.Framework/Themes/DesignTokens.cs`
- 主题应用器：
  - `src/MES.UI.Framework/Themes/UIThemeManager.cs`
  - `src/MES.UI.Framework/Themes/LolV2ThemeApplier.cs`
  - `src/MES.UI.Framework/Themes/NovaVisuals.cs`
- 原子控件（Atoms）：
  - `src/MES.UI.Framework/Controls/ThemedTextBox.cs`（占位提示 + 主题自适应）
  - `src/MES.UI.Framework/Controls/ThemedListBox.cs`（双缓冲 + 主题自适应）
  - `src/MES.UI.Framework/Controls/ThemedRichTextBox.cs`（主题自适应，适用于日志/报告）
  - `src/MES.UI.Framework/Controls/ThemedTabControl.cs`（OwnerDraw + 主题自适应，适用于深色 Tab）
- 错误边界（Error Boundary）：
  - `src/MES.UI.Framework/Utilities/CrashReporting/GlobalExceptionBoundary.cs`
  - `src/MES.UI.Framework/Utilities/CrashReporting/CrashReportDialog.cs`
  - `src/MES.UI.Framework/Utilities/CrashReporting/CrashReportWriter.cs`
- 搜索打分（纯逻辑，可测）：
  - `src/MES.UI.Framework/Utilities/Search/CommandPaletteSearch.cs`

## 开发约定

- 新增 UI 视觉规则应优先沉淀为 Token（字体/颜色/圆角/间距）
- 日志/报告类展示建议使用 `DesignTokens.Typography.CreateCodeFont()` 以获得更好的可读性
- 主题应用覆盖容器控件：包含 TabControl/TabPage 等常见容器（避免部分界面“漏主题”）
- 允许渐进式替换历史硬编码，但禁止新增新的硬编码风格债务
- RichTextBox 类控件如需关键字高亮，可使用 SelectionColor 渐进增强，但应限制匹配次数以避免性能回退
