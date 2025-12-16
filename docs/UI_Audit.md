# UI 全站巡检清单（LoL 暗金风一致性）

更新时间：2025-12-16

目标：解决“部分页面仍是系统默认控件/白底白边/蓝色选中”的割裂感，确保 **所有窗体/对话框** 在 LoL 主题下都有统一的控件质感、层级与可读性。

## 1) 全局策略（兜底：不遗漏）

- **所有窗体统一继承 `ThemedForm`**
  - TopLevel 窗体：`Shown` 自动应用主题
  - 嵌入式窗体（`TopLevel=false`）：`VisibleChanged` 自动应用主题
  - 动态新增控件：`ControlAdded` 会触发主题增量套用
- **LoL 主题统一由 `LolV2ThemeApplier` 接管**
  - 输入控件：`TextBox/ComboBox/DateTimePicker/NumericUpDown` 暗底 + 暗金描边 + Hover/Focus 发光
  - “白边/白底”治理：对部分原生控件禁用 native theme（uxtheme，使用 `" "` 参数更稳定）
  - TabPage 背景兜底：强制 `UseVisualStyleBackColor=false`（否则 BackColor 会被系统视觉样式接管，导致看起来像“主题没生效”）
  - 表格：`DataGridView` 行高/内边距/交替行/绑定后回写（避免业务代码刷回浅色）
  - 编辑态兜底：`DataGridView.EditingControlShowing` 对编辑控件再次套主题，避免编辑时突然变白
  - 下拉空白兜底：`ComboBox.DropDownHeight` 限制高度，减少“空白区域仍是系统白底”的割裂感
  - 日期下拉兜底：补齐 `CalendarTitleBackColor/ForeColor/TrailingForeColor`，让日历弹层也更接近暗金风
  - 菜单兜底：`ContextMenuStrip` 统一暗底与选中高光（避免右键菜单仍是白底）
- **大厅背景轮播（纯视觉，不影响业务）**
  - 从 `assets\\lobby_backgrounds\\` 读取本地图片并自动轮播（尺寸不一也按 Cover 铺满）
  - 无按钮，带淡入淡出；叠加暗色遮罩，保证前景文字可读
  - 说明：不内置任何版权素材，图片由你自行放置（见 `docs/Lobby_Backgrounds.md`）

## 2) 窗体覆盖清单（按目录）

### 主界面
- `src/MES.UI/Forms/MainFormLolV2.cs`：LoL 大厅主壳/导航
- `src/MES.UI/Forms/MainForm.cs`：历史主窗体（已纳入主题兜底，通常不作为入口）

### 物料
- `src/MES.UI/Forms/Material/MaterialManagementForm.cs`：物料管理（布局 A：左列表 + 右详情卡）
- `src/MES.UI/Forms/Material/MaterialEditForm.cs`：物料编辑/新增
- `src/MES.UI/Forms/Material/BOMManagementForm.cs`：BOM 管理（表格样式由主题统一接管）
- `src/MES.UI/Forms/Material/BOMEditForm.cs`：BOM 编辑（只读输入框不再写死浅底）
- `src/MES.UI/Forms/Material/ProcessRouteConfigForm.cs`：工艺路线配置（SplitContainer 分割条暗色化）
- `src/MES.UI/Forms/Material/ProcessRouteEditForm.cs`：工艺路线编辑（移除白底/只读浅底，交给主题统一）

### 生产
- `src/MES.UI/Forms/Production/ProductionOrderManagementForm.cs`：生产订单管理（布局 A：左列表 + 右详情卡）
- `src/MES.UI/Forms/Production/Edit/ProductionOrderEditForm.cs`：生产订单编辑
- `src/MES.UI/Forms/Production/ProductionExecutionControlForm.cs`：生产执行控制（表格样式由主题统一接管）

### 工单 / 批次
- `src/MES.UI/Forms/WorkOrder/WorkOrderManagementForm.cs`：工单管理（布局 A）
- `src/MES.UI/Forms/WorkOrder/CreateWorkOrder.cs`：创建工单
- `src/MES.UI/Forms/WorkOrder/SubmitWorkOrder.cs`：提交工单
- `src/MES.UI/Forms/WorkOrder/CancelWorkOrder.cs`：取消工单
- `src/MES.UI/Forms/Batch/BatchManagementForm.cs`：批次管理（布局 A）
- `src/MES.UI/Forms/Batch/CreateBatch.cs`：创建批次
- `src/MES.UI/Forms/Batch/CancelBatch.cs`：取消批次

### 车间 / 在制 / 设备
- `src/MES.UI/Forms/WorkshopManagementForm.cs`：车间管理
- `src/MES.UI/Forms/WorkshopEditForm.cs`：车间编辑
- `src/MES.UI/Forms/Workshop/WorkshopOperationForm.cs`：车间作业
- `src/MES.UI/Forms/Workshop/WIPManagementForm.cs`：在制品管理
- `src/MES.UI/Forms/Workshop/WIPStatusUpdateDialog.cs`：在制品状态更新对话框
- `src/MES.UI/Forms/Workshop/EquipmentStatusForm.cs`：设备状态（状态色改为暗色底 + 明亮文字，且保证选中态可见）

### 系统 / 通用选择
- `src/MES.UI/Forms/SystemManagement/SystemConfigForm.cs`：系统配置（Tab 改为 LoL 暗金风，去掉白底灰边）
- `src/MES.UI/Forms/SystemManagement/DatabaseDiagnosticForm.cs`：数据库诊断（表格样式由主题统一接管）
- `src/MES.UI/Forms/SystemManagement/AboutForm.cs`：关于（移除蓝色 hover/浅色 RTF 配色，改为 LoL 高对比）
- `src/MES.UI/Forms/Common/MaterialSelectForm.cs`：物料选择
- `src/MES.UI/Forms/Common/BOMSelectForm.cs`：BOM 选择

## 3) 已知限制（WinForms 原生控件客观约束）

- `DateTimePicker` / `NumericUpDown` 的部分“箭头按钮”在 WinForms 下无法做到 100% 自绘一致；当前策略是 **禁用 native theme + 暗底 + 描边**，尽量接近 LoL 观感。
- 若需要“完全 1:1 LoL 控件”，需要自定义控件替换（例如自绘 DatePicker）。
