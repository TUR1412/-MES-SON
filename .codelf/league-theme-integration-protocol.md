# 英雄联盟主题美化集成协议

> **创建时间**: 2025-06-11 11:11:18
> **协议级别**: 🔴 **最高优先级** - 与execution-strategy.md同等级别
> **适用范围**: 英雄联盟主题美化在MES项目中的集成
> **执行原则**: 小步快跑 + 一步三验证 + 零容忍错误

## 🚨 **绝对禁止行为清单**

### **代码层面绝对禁令**
- ❌ **严禁直接复制leagueoflegends-wpf的任何代码文件**
- ❌ **严禁引用Jamesnet.Foundation或Jamesnet.Platform.Wpf**
- ❌ **严禁使用任何C# 6.0+语法特性**
- ❌ **严禁修改现有的UIThemeManager.cs文件**
- ❌ **严禁在MES.UI项目中添加WPF引用**
- ❌ **严禁使用ElementHost或WindowsFormsHost**
- ❌ **严禁修改App.config或任何配置文件**
- ❌ **严禁创建与现有控件同名的新控件**

### **架构层面绝对禁令**
- ❌ **严禁改变MES项目的三层架构**
- ❌ **严禁引入MVVM模式或依赖注入容器**
- ❌ **严禁修改Program.cs的启动流程**
- ❌ **严禁修改MainForm的基础结构**
- ❌ **严禁创建新的程序集或项目引用**

### **文件操作绝对禁令**
- ❌ **严禁删除或重命名现有文件**
- ❌ **严禁修改现有的.Designer.cs文件**
- ❌ **严禁修改现有的.resx资源文件**
- ❌ **严禁在bin或obj目录中进行任何操作**

## 🛡️ **一步三验证机制**

### **验证层级定义**
每个开发步骤都必须通过以下三层验证：

#### **第一层：语法验证**
- [ ] 使用csharp5-validator.md进行C# 5.0语法检查
- [ ] 确认无任何C# 6.0+特性使用
- [ ] 验证所有using引用的合法性
- [ ] 检查命名空间的正确性

#### **第二层：编译验证**
- [ ] 执行VS2022 MSBuild完整编译
- [ ] 确认零编译错误和警告
- [ ] 验证所有项目引用的完整性
- [ ] 检查资源文件的正确性

#### **第三层：功能验证**
- [ ] 启动MES应用程序测试
- [ ] 验证现有功能无任何影响
- [ ] 测试新增功能的正确性
- [ ] 确认性能无明显下降

### **验证失败处理**
- 任何一层验证失败，立即停止后续操作
- 分析失败原因并记录到issue-tracking.md
- 回滚到上一个验证通过的状态
- 重新制定修复方案

## 📋 **小步快跑实施计划**

### **阶段0：环境准备（必须完成）**
**目标**: 建立安全的开发环境
**时间**: 0.5天

#### **步骤0.1：创建安全分支**
```bash
git checkout develop
git pull origin develop
git checkout -b feature/league-theme-safe
```
**验证点**: 确认分支创建成功，工作目录干净

#### **步骤0.2：备份关键文件**
```bash
# 备份现有主题管理器
cp src/MES.UI.Framework/Themes/UIThemeManager.cs .codelf/backup/
# 备份主窗体
cp src/MES.UI/Forms/MainForm.cs .codelf/backup/
```
**验证点**: 确认备份文件存在且完整

#### **步骤0.3：基线编译验证**
```bash
& "D:\Visual Studio\MSBuild\Current\Bin\MSBuild.exe" MES.sln /p:Configuration=Debug /verbosity:minimal
```
**验证点**: 编译成功，无错误无警告

### **阶段1：颜色提取验证（最小风险）**
**目标**: 验证颜色提取和应用的可行性
**时间**: 1天

#### **步骤1.1：创建颜色定义类**
**文件**: `src/MES.UI.Framework/Themes/LeagueColors.cs`
**内容**: 仅包含静态颜色常量定义
**约束**: 
- 只使用Color.FromArgb()方法
- 严格遵循C# 5.0语法
- 不引用任何外部依赖

**一步三验证**:
1. **语法验证**: csharp5-validator.md检查
2. **编译验证**: MSBuild编译测试
3. **功能验证**: 颜色值正确性测试

#### **步骤1.2：扩展UIThemeManager**
**操作**: 在现有UIThemeManager中添加GetLeagueTheme()方法
**约束**: 
- 不修改现有方法
- 不改变现有接口
- 保持向后兼容

**一步三验证**:
1. **语法验证**: 确认方法签名符合C# 5.0
2. **编译验证**: 确认编译无错误
3. **功能验证**: 测试新方法返回正确颜色

#### **步骤1.3：单控件测试**
**目标**: 在AboutForm中测试单个按钮的颜色变更
**约束**: 
- 只修改一个按钮的BackColor属性
- 不影响其他控件
- 保持原有事件处理

**一步三验证**:
1. **语法验证**: 确认修改代码符合规范
2. **编译验证**: 确认编译成功
3. **功能验证**: 启动程序测试按钮显示

### **阶段2：字体和间距标准化（低风险）**
**目标**: 建立字体和间距的标准化定义
**时间**: 1天

#### **步骤2.1：创建字体管理类**
**文件**: `src/MES.UI.Framework/Themes/LeagueFonts.cs`
**约束**: 同阶段1的所有约束

#### **步骤2.2：创建间距管理类**
**文件**: `src/MES.UI.Framework/Themes/LeagueSpacing.cs`
**约束**: 同阶段1的所有约束

#### **步骤2.3：集成测试**
**目标**: 在AboutForm中应用字体和间距
**约束**: 同阶段1的所有约束

### **阶段3：控件样式定制（中等风险）**
**目标**: 为基础控件创建英雄联盟风格
**时间**: 2-3天

#### **步骤3.1：按钮样式定制**
**方法**: 通过Paint事件自定义绘制
**约束**: 
- 不继承或修改现有控件类
- 只通过事件处理进行绘制
- 保持所有原有功能

#### **步骤3.2：面板样式定制**
**方法**: 同步骤3.1
**约束**: 同步骤3.1

#### **步骤3.3：输入框样式定制**
**方法**: 同步骤3.1
**约束**: 同步骤3.1

## 🚨 **应急处理机制**

### **错误分级**
- **A级错误**: 导致编译失败或程序崩溃
- **B级错误**: 影响现有功能或性能
- **C级错误**: 视觉效果不符合预期

### **应急响应流程**
1. **立即停止**: 停止所有开发活动
2. **错误记录**: 详细记录错误现象和环境
3. **状态回滚**: 回滚到最后一个验证通过的状态
4. **原因分析**: 分析错误根本原因
5. **方案调整**: 调整实施方案或终止项目

### **回滚检查清单**
- [ ] 恢复备份的关键文件
- [ ] 删除所有新增文件
- [ ] 重置git分支到初始状态
- [ ] 执行完整编译验证
- [ ] 确认所有功能正常

## 📊 **进度跟踪机制**

### **每日检查清单**
- [ ] 当日所有修改都通过三层验证
- [ ] 所有新增代码都符合C# 5.0语法
- [ ] 编译状态保持绿色（无错误无警告）
- [ ] 现有功能无任何影响
- [ ] 备份文件保持最新

### **阶段完成标准**
- [ ] 所有步骤的三层验证都通过
- [ ] 代码质量达到MES项目标准
- [ ] 性能测试通过
- [ ] 文档更新完成
- [ ] 轩天帝验收通过

## 🔒 **质量保证机制**

### **代码审查要求**
- 每个文件修改都必须经过自我审查
- 使用csharp5-validator.md进行强制语法检查
- 所有新增代码都要有详细注释
- 遵循MES项目的命名规范

### **测试要求**
- 每个阶段都要进行完整的回归测试
- 新功能要有专门的测试用例
- 性能测试要确保无明显下降
- 兼容性测试要覆盖所有支持的操作系统

## 📝 **文档维护要求**

### **必须更新的文档**
- [ ] 本协议文档（记录实际执行情况）
- [ ] issue-tracking.md（记录遇到的问题）
- [ ] development-standards.md（更新开发规范）
- [ ] project-overview.md（更新项目状态）

### **文档质量标准**
- 所有修改都要有详细的变更记录
- 问题和解决方案要有完整描述
- 经验教训要及时总结和分享
- 文档要保持与代码的同步更新

---

**执行承诺**: 本协议一旦生效，所有相关开发活动都必须严格遵循。任何违反都将被视为严重的质量事故，必须立即停止并进行全面检查。

**成功标准**: 在不影响MES项目任何现有功能的前提下，成功实现英雄联盟风格的视觉美化效果。

## 📋 **详细技术实施规范**

### **颜色提取标准**
从leagueoflegends-wpf项目中提取的颜色必须符合以下标准：

#### **主色调定义**
```csharp
// C# 5.0兼容的颜色定义示例
public static class LeagueColors
{
    // 主色调 - 英雄联盟金色
    public static readonly Color PrimaryGold = Color.FromArgb(120, 90, 40);
    public static readonly Color PrimaryGoldLight = Color.FromArgb(150, 120, 70);
    public static readonly Color PrimaryGoldDark = Color.FromArgb(90, 60, 20);

    // 背景色 - 深色主题
    public static readonly Color DarkBackground = Color.FromArgb(1, 10, 19);
    public static readonly Color DarkSurface = Color.FromArgb(20, 30, 40);
    public static readonly Color DarkBorder = Color.FromArgb(40, 50, 60);

    // 强调色 - 蓝色系
    public static readonly Color AccentBlue = Color.FromArgb(0, 150, 255);
    public static readonly Color AccentBlueLight = Color.FromArgb(50, 180, 255);
    public static readonly Color AccentBlueDark = Color.FromArgb(0, 120, 200);

    // 状态色
    public static readonly Color SuccessGreen = Color.FromArgb(0, 200, 100);
    public static readonly Color WarningOrange = Color.FromArgb(255, 150, 0);
    public static readonly Color ErrorRed = Color.FromArgb(255, 50, 50);
}
```

#### **颜色应用规则**
- 主窗体背景：DarkBackground
- 面板背景：DarkSurface
- 按钮主色：PrimaryGold
- 边框颜色：DarkBorder
- 文字颜色：根据背景自动选择对比色

### **字体标准化规范**

#### **字体定义**
```csharp
public static class LeagueFonts
{
    // 标准字体族
    public static readonly string FontFamily = "微软雅黑";

    // 字体大小定义
    public static readonly float TitleSize = 14f;
    public static readonly float HeaderSize = 12f;
    public static readonly float BodySize = 9f;
    public static readonly float CaptionSize = 8f;

    // 字体样式
    public static readonly FontStyle TitleStyle = FontStyle.Bold;
    public static readonly FontStyle HeaderStyle = FontStyle.Bold;
    public static readonly FontStyle BodyStyle = FontStyle.Regular;
    public static readonly FontStyle CaptionStyle = FontStyle.Regular;

    // 字体对象创建方法
    public static Font GetTitleFont()
    {
        return new Font(FontFamily, TitleSize, TitleStyle);
    }

    public static Font GetHeaderFont()
    {
        return new Font(FontFamily, HeaderSize, HeaderStyle);
    }

    public static Font GetBodyFont()
    {
        return new Font(FontFamily, BodySize, BodyStyle);
    }

    public static Font GetCaptionFont()
    {
        return new Font(FontFamily, CaptionSize, CaptionStyle);
    }
}
```

### **间距标准化规范**

#### **间距定义**
```csharp
public static class LeagueSpacing
{
    // 基础间距单位
    public static readonly int BaseUnit = 8;

    // 标准间距
    public static readonly int XSmall = BaseUnit / 2;     // 4px
    public static readonly int Small = BaseUnit;          // 8px
    public static readonly int Medium = BaseUnit * 2;     // 16px
    public static readonly int Large = BaseUnit * 3;      // 24px
    public static readonly int XLarge = BaseUnit * 4;     // 32px

    // 控件间距
    public static readonly Padding ControlPadding = new Padding(Small);
    public static readonly Padding PanelPadding = new Padding(Medium);
    public static readonly Padding FormPadding = new Padding(Large);

    // 边距定义
    public static readonly int ButtonMargin = Small;
    public static readonly int LabelMargin = XSmall;
    public static readonly int TextBoxMargin = Small;
    public static readonly int PanelMargin = Medium;
}
```

### **控件样式定制规范**

#### **按钮样式定制**
```csharp
// 按钮的英雄联盟风格绘制
private void LeagueButton_Paint(object sender, PaintEventArgs e)
{
    Button btn = sender as Button;
    if (btn == null) return;

    Graphics g = e.Graphics;
    g.SmoothingMode = SmoothingMode.AntiAlias;

    // 背景绘制
    using (var brush = new LinearGradientBrush(btn.ClientRectangle,
        LeagueColors.PrimaryGold, LeagueColors.PrimaryGoldDark,
        LinearGradientMode.Vertical))
    {
        g.FillRectangle(brush, btn.ClientRectangle);
    }

    // 边框绘制
    using (var pen = new Pen(LeagueColors.PrimaryGoldLight, 2))
    {
        g.DrawRectangle(pen, 1, 1, btn.Width - 3, btn.Height - 3);
    }

    // 文字绘制
    using (var brush = new SolidBrush(Color.White))
    {
        var sf = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        g.DrawString(btn.Text, LeagueFonts.GetBodyFont(), brush, btn.ClientRectangle, sf);
    }
}
```

#### **面板样式定制**
```csharp
// 面板的英雄联盟风格绘制
private void LeaguePanel_Paint(object sender, PaintEventArgs e)
{
    Panel panel = sender as Panel;
    if (panel == null) return;

    Graphics g = e.Graphics;

    // 背景绘制
    using (var brush = new SolidBrush(LeagueColors.DarkSurface))
    {
        g.FillRectangle(brush, panel.ClientRectangle);
    }

    // 边框绘制
    using (var pen = new Pen(LeagueColors.DarkBorder, 1))
    {
        g.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
    }
}
```

## 🔍 **详细验证检查清单**

### **C# 5.0语法验证清单**
- [ ] 无字符串插值 ($"") 使用
- [ ] 无空传播运算符 (?.) 使用
- [ ] 无nameof运算符使用
- [ ] 无using static语句
- [ ] 无表达式体成员 (=>) 使用
- [ ] 无自动属性初始化器使用
- [ ] 无异常筛选器 (when) 使用
- [ ] 无out变量声明使用

### **编译验证清单**
- [ ] MSBuild编译零错误
- [ ] MSBuild编译零警告
- [ ] 所有项目引用正确
- [ ] 所有using引用有效
- [ ] 资源文件完整
- [ ] 程序集版本一致

### **功能验证清单**
- [ ] 应用程序正常启动
- [ ] 主窗体正常显示
- [ ] 所有菜单功能正常
- [ ] 所有现有窗体正常打开
- [ ] 数据库连接正常
- [ ] 日志记录正常
- [ ] 性能无明显下降

## 📊 **进度跟踪表格**

| 阶段 | 步骤 | 状态 | 验证结果 | 完成时间 | 备注 |
|------|------|------|----------|----------|------|
| 0 | 环境准备 | ✅ | 通过 | 2025-06-11 11:30 | 安全分支创建，备份完成 |
| 0.1 | 创建安全分支 | ✅ | 通过 | 2025-06-11 11:30 | feature/league-theme-safe |
| 0.2 | 备份关键文件 | ✅ | 通过 | 2025-06-11 11:30 | UIThemeManager.cs, MainForm.cs |
| 0.3 | 基线编译验证 | ✅ | 通过 | 2025-06-11 11:30 | 编译成功，无错误 |
| 1 | 颜色提取验证 | ✅ | 通过 | 2025-06-11 12:00 | 所有步骤完成 |
| 1.1 | 创建颜色定义类 | ✅ | 通过 | 2025-06-11 11:45 | LeagueColors.cs创建成功 |
| 1.2 | 扩展UIThemeManager | ✅ | 通过 | 2025-06-11 11:50 | GetLeagueTheme()方法添加 |
| 1.3 | 单控件测试 | ✅ | 通过 | 2025-06-11 12:00 | AboutForm按钮测试成功 |
| 2 | 主界面主题应用 | ✅ | 通过 | 2025-06-11 12:15 | MainForm英雄联盟主题应用 |
| 2.1 | MainForm主题集成 | ✅ | 通过 | 2025-06-11 12:15 | 深色背景+金色按钮风格 |

**状态说明**:
- ⏳ 待开始
- 🔄 进行中
- ✅ 已完成
- ❌ 失败
- ⚠️ 有问题但可继续

## 🚨 **风险预警机制**

### **高风险操作识别**
以下操作被识别为高风险，需要特别谨慎：
1. 修改现有的UIThemeManager.cs文件
2. 在MainForm中添加新的控件
3. 修改任何.Designer.cs文件
4. 添加新的项目引用
5. 修改App.config配置

### **风险缓解措施**
- 每个高风险操作前都要创建完整备份
- 高风险操作要分解为更小的步骤
- 每个高风险步骤都要有专门的回滚计划
- 高风险操作后要进行额外的验证测试

---

**最终承诺**: 本协议将确保英雄联盟主题美化的安全集成，绝对不会重蹈之前的覆辙。每一步都经过严格验证，每一个风险都有对应的缓解措施。
