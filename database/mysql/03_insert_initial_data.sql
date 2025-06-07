-- =============================================
-- MES制造执行系统 - 初始数据插入脚本
-- 版本: 1.0
-- 创建时间: 2025-06-06
-- 说明: 插入系统初始数据，包括管理员用户、基础字典等
-- =============================================

USE `mes_system`;

-- =============================================
-- 系统配置初始数据
-- =============================================

-- 插入系统配置
INSERT INTO `sys_config` (`config_key`, `config_value`, `config_type`, `description`, `is_system`, `status`) VALUES
('SYSTEM_NAME', 'MES制造执行系统', 'STRING', '系统名称', 1, 1),
('SYSTEM_VERSION', '1.0.0', 'STRING', '系统版本', 1, 1),
('SYSTEM_COPYRIGHT', '© 2025 MES Team', 'STRING', '版权信息', 1, 1),
('DEFAULT_PASSWORD', '123456', 'STRING', '默认密码', 1, 1),
('PASSWORD_MIN_LENGTH', '6', 'NUMBER', '密码最小长度', 1, 1),
('SESSION_TIMEOUT', '30', 'NUMBER', '会话超时时间(分钟)', 1, 1),
('MAX_LOGIN_ATTEMPTS', '5', 'NUMBER', '最大登录尝试次数', 1, 1),
('BACKUP_RETENTION_DAYS', '30', 'NUMBER', '备份保留天数', 1, 1),
('LOG_RETENTION_DAYS', '90', 'NUMBER', '日志保留天数', 1, 1),
('ENABLE_AUDIT_LOG', 'true', 'BOOLEAN', '启用审计日志', 1, 1);

-- =============================================
-- 数据字典初始数据
-- =============================================

-- 用户状态字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('USER_STATUS', '用户状态', 'ENABLED', '启用', '1', 1, 1),
('USER_STATUS', '用户状态', 'DISABLED', '禁用', '0', 2, 1);

-- 物料类型字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('MATERIAL_TYPE', '物料类型', 'RAW_MATERIAL', '原材料', 'RAW_MATERIAL', 1, 1),
('MATERIAL_TYPE', '物料类型', 'SEMI_FINISHED', '半成品', 'SEMI_FINISHED', 2, 1),
('MATERIAL_TYPE', '物料类型', 'FINISHED_PRODUCT', '成品', 'FINISHED_PRODUCT', 3, 1),
('MATERIAL_TYPE', '物料类型', 'AUXILIARY_MATERIAL', '辅料', 'AUXILIARY_MATERIAL', 4, 1),
('MATERIAL_TYPE', '物料类型', 'PACKAGING_MATERIAL', '包装材料', 'PACKAGING_MATERIAL', 5, 1);

-- 生产订单状态字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('PRODUCTION_ORDER_STATUS', '生产订单状态', 'PENDING', '待开始', 'PENDING', 1, 1),
('PRODUCTION_ORDER_STATUS', '生产订单状态', 'IN_PROGRESS', '进行中', 'IN_PROGRESS', 2, 1),
('PRODUCTION_ORDER_STATUS', '生产订单状态', 'PAUSED', '已暂停', 'PAUSED', 3, 1),
('PRODUCTION_ORDER_STATUS', '生产订单状态', 'COMPLETED', '已完成', 'COMPLETED', 4, 1),
('PRODUCTION_ORDER_STATUS', '生产订单状态', 'CANCELLED', '已取消', 'CANCELLED', 5, 1);

-- 车间状态字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('WORKSHOP_STATUS', '车间状态', 'ENABLED', '启用', 'ENABLED', 1, 1),
('WORKSHOP_STATUS', '车间状态', 'DISABLED', '禁用', 'DISABLED', 2, 1),
('WORKSHOP_STATUS', '车间状态', 'MAINTENANCE', '维护中', 'MAINTENANCE', 3, 1);

-- 设备状态字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('EQUIPMENT_STATUS', '设备状态', 'NORMAL', '正常', '1', 1, 1),
('EQUIPMENT_STATUS', '设备状态', 'MAINTENANCE', '维护中', '2', 2, 1),
('EQUIPMENT_STATUS', '设备状态', 'FAULT', '故障', '3', 3, 1),
('EQUIPMENT_STATUS', '设备状态', 'DISABLED', '停用', '4', 4, 1);

-- 质量检验类型字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('INSPECTION_TYPE', '检验类型', 'INCOMING', '进料检验', '1', 1, 1),
('INSPECTION_TYPE', '检验类型', 'PROCESS', '过程检验', '2', 2, 1),
('INSPECTION_TYPE', '检验类型', 'FINAL', '成品检验', '3', 3, 1),
('INSPECTION_TYPE', '检验类型', 'OUTGOING', '出货检验', '4', 4, 1);

-- 质量检验结果字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('INSPECTION_RESULT', '检验结果', 'QUALIFIED', '合格', '1', 1, 1),
('INSPECTION_RESULT', '检验结果', 'UNQUALIFIED', '不合格', '2', 2, 1),
('INSPECTION_RESULT', '检验结果', 'CONDITIONAL', '让步接收', '3', 3, 1);

-- 计量单位字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('UNIT', '计量单位', 'PCS', '个', 'PCS', 1, 1),
('UNIT', '计量单位', 'KG', '千克', 'KG', 2, 1),
('UNIT', '计量单位', 'G', '克', 'G', 3, 1),
('UNIT', '计量单位', 'M', '米', 'M', 4, 1),
('UNIT', '计量单位', 'CM', '厘米', 'CM', 5, 1),
('UNIT', '计量单位', 'L', '升', 'L', 6, 1),
('UNIT', '计量单位', 'ML', '毫升', 'ML', 7, 1),
('UNIT', '计量单位', 'BOX', '箱', 'BOX', 8, 1),
('UNIT', '计量单位', 'SET', '套', 'SET', 9, 1);

-- 优先级字典
INSERT INTO `sys_dictionary` (`dict_type`, `dict_type_name`, `dict_code`, `dict_name`, `dict_value`, `sort_order`, `is_system`) VALUES
('PRIORITY', '优先级', 'LOW', '低', 'LOW', 1, 1),
('PRIORITY', '优先级', 'NORMAL', '普通', 'NORMAL', 2, 1),
('PRIORITY', '优先级', 'HIGH', '高', 'HIGH', 3, 1),
('PRIORITY', '优先级', 'URGENT', '紧急', 'URGENT', 4, 1);

-- =============================================
-- 权限初始数据
-- =============================================

-- 插入系统权限
INSERT INTO `sys_permission` (`permission_code`, `permission_name`, `permission_type`, `parent_id`, `module_name`, `resource_url`, `sort_order`, `is_system`) VALUES
-- 系统管理
('SYSTEM_MANAGE', '系统管理', 'MENU', 0, 'SYSTEM', '/system', 1, 1),
('USER_MANAGE', '用户管理', 'MENU', 1, 'SYSTEM', '/system/user', 1, 1),
('USER_VIEW', '查看用户', 'BUTTON', 2, 'SYSTEM', '', 1, 1),
('USER_ADD', '新增用户', 'BUTTON', 2, 'SYSTEM', '', 2, 1),
('USER_EDIT', '编辑用户', 'BUTTON', 2, 'SYSTEM', '', 3, 1),
('USER_DELETE', '删除用户', 'BUTTON', 2, 'SYSTEM', '', 4, 1),
('ROLE_MANAGE', '角色管理', 'MENU', 1, 'SYSTEM', '/system/role', 2, 1),
('ROLE_VIEW', '查看角色', 'BUTTON', 7, 'SYSTEM', '', 1, 1),
('ROLE_ADD', '新增角色', 'BUTTON', 7, 'SYSTEM', '', 2, 1),
('ROLE_EDIT', '编辑角色', 'BUTTON', 7, 'SYSTEM', '', 3, 1),
('ROLE_DELETE', '删除角色', 'BUTTON', 7, 'SYSTEM', '', 4, 1),
('PERMISSION_MANAGE', '权限管理', 'MENU', 1, 'SYSTEM', '/system/permission', 3, 1),

-- 物料管理
('MATERIAL_MANAGE', '物料管理', 'MENU', 0, 'MATERIAL', '/material', 2, 1),
('MATERIAL_VIEW', '查看物料', 'BUTTON', 13, 'MATERIAL', '', 1, 1),
('MATERIAL_ADD', '新增物料', 'BUTTON', 13, 'MATERIAL', '', 2, 1),
('MATERIAL_EDIT', '编辑物料', 'BUTTON', 13, 'MATERIAL', '', 3, 1),
('MATERIAL_DELETE', '删除物料', 'BUTTON', 13, 'MATERIAL', '', 4, 1),
('BOM_MANAGE', 'BOM管理', 'MENU', 13, 'MATERIAL', '/material/bom', 2, 1),
('BOM_VIEW', '查看BOM', 'BUTTON', 18, 'MATERIAL', '', 1, 1),
('BOM_ADD', '新增BOM', 'BUTTON', 18, 'MATERIAL', '', 2, 1),
('BOM_EDIT', '编辑BOM', 'BUTTON', 18, 'MATERIAL', '', 3, 1),
('BOM_DELETE', '删除BOM', 'BUTTON', 18, 'MATERIAL', '', 4, 1),

-- 生产管理
('PRODUCTION_MANAGE', '生产管理', 'MENU', 0, 'PRODUCTION', '/production', 3, 1),
('PRODUCTION_ORDER_VIEW', '查看生产订单', 'BUTTON', 22, 'PRODUCTION', '', 1, 1),
('PRODUCTION_ORDER_ADD', '新增生产订单', 'BUTTON', 22, 'PRODUCTION', '', 2, 1),
('PRODUCTION_ORDER_EDIT', '编辑生产订单', 'BUTTON', 22, 'PRODUCTION', '', 3, 1),
('PRODUCTION_ORDER_DELETE', '删除生产订单', 'BUTTON', 22, 'PRODUCTION', '', 4, 1),
('PRODUCTION_ORDER_START', '启动生产订单', 'BUTTON', 22, 'PRODUCTION', '', 5, 1),
('PRODUCTION_ORDER_COMPLETE', '完成生产订单', 'BUTTON', 22, 'PRODUCTION', '', 6, 1),

-- 车间管理
('WORKSHOP_MANAGE', '车间管理', 'MENU', 0, 'WORKSHOP', '/workshop', 4, 1),
('WORKSHOP_VIEW', '查看车间', 'BUTTON', 28, 'WORKSHOP', '', 1, 1),
('WORKSHOP_ADD', '新增车间', 'BUTTON', 28, 'WORKSHOP', '', 2, 1),
('WORKSHOP_EDIT', '编辑车间', 'BUTTON', 28, 'WORKSHOP', '', 3, 1),
('WORKSHOP_DELETE', '删除车间', 'BUTTON', 28, 'WORKSHOP', '', 4, 1),

-- 设备管理
('EQUIPMENT_MANAGE', '设备管理', 'MENU', 0, 'EQUIPMENT', '/equipment', 5, 1),
('EQUIPMENT_VIEW', '查看设备', 'BUTTON', 33, 'EQUIPMENT', '', 1, 1),
('EQUIPMENT_ADD', '新增设备', 'BUTTON', 33, 'EQUIPMENT', '', 2, 1),
('EQUIPMENT_EDIT', '编辑设备', 'BUTTON', 33, 'EQUIPMENT', '', 3, 1),
('EQUIPMENT_DELETE', '删除设备', 'BUTTON', 33, 'EQUIPMENT', '', 4, 1),
('EQUIPMENT_MAINTENANCE', '设备维护', 'BUTTON', 33, 'EQUIPMENT', '', 5, 1),

-- 质量管理
('QUALITY_MANAGE', '质量管理', 'MENU', 0, 'QUALITY', '/quality', 6, 1),
('QUALITY_INSPECTION_VIEW', '查看质量检验', 'BUTTON', 39, 'QUALITY', '', 1, 1),
('QUALITY_INSPECTION_ADD', '新增质量检验', 'BUTTON', 39, 'QUALITY', '', 2, 1),
('QUALITY_INSPECTION_EDIT', '编辑质量检验', 'BUTTON', 39, 'QUALITY', '', 3, 1),
('QUALITY_INSPECTION_DELETE', '删除质量检验', 'BUTTON', 39, 'QUALITY', '', 4, 1),
('QUALITY_INSPECTION_REVIEW', '审核质量检验', 'BUTTON', 39, 'QUALITY', '', 5, 1);

-- =============================================
-- 角色初始数据
-- =============================================

-- 插入系统角色
INSERT INTO `sys_role` (`role_code`, `role_name`, `description`, `status`, `permissions`, `sort_order`) VALUES
('ADMIN', '系统管理员', '系统管理员，拥有所有权限', 1, '["*"]', 1),
('PRODUCTION_MANAGER', '生产经理', '生产经理，负责生产管理', 1, '["PRODUCTION_MANAGE","PRODUCTION_ORDER_VIEW","PRODUCTION_ORDER_ADD","PRODUCTION_ORDER_EDIT","PRODUCTION_ORDER_START","PRODUCTION_ORDER_COMPLETE","WORKSHOP_VIEW","EQUIPMENT_VIEW"]', 2),
('QUALITY_MANAGER', '质量经理', '质量经理，负责质量管理', 1, '["QUALITY_MANAGE","QUALITY_INSPECTION_VIEW","QUALITY_INSPECTION_ADD","QUALITY_INSPECTION_EDIT","QUALITY_INSPECTION_REVIEW","MATERIAL_VIEW"]', 3),
('WORKSHOP_SUPERVISOR', '车间主管', '车间主管，负责车间管理', 1, '["WORKSHOP_MANAGE","WORKSHOP_VIEW","WORKSHOP_EDIT","EQUIPMENT_VIEW","EQUIPMENT_MAINTENANCE","PRODUCTION_ORDER_VIEW"]', 4),
('MATERIAL_MANAGER', '物料经理', '物料经理，负责物料管理', 1, '["MATERIAL_MANAGE","MATERIAL_VIEW","MATERIAL_ADD","MATERIAL_EDIT","BOM_MANAGE","BOM_VIEW","BOM_ADD","BOM_EDIT"]', 5),
('OPERATOR', '操作员', '普通操作员，基本查看权限', 1, '["MATERIAL_VIEW","PRODUCTION_ORDER_VIEW","WORKSHOP_VIEW","EQUIPMENT_VIEW","QUALITY_INSPECTION_VIEW"]', 6);

-- =============================================
-- 用户初始数据
-- =============================================

-- 插入系统管理员用户
INSERT INTO `sys_user` (`user_code`, `user_name`, `real_name`, `password`, `salt`, `email`, `phone`, `department`, `position`, `status`) VALUES
('admin', 'admin', '系统管理员', 'E10ADC3949BA59ABBE56E057F20F883E', 'salt123', 'admin@mes.com', '13800138000', '信息技术部', '系统管理员', 1),
('tianDi', 'tianDi', '天帝', 'E10ADC3949BA59ABBE56E057F20F883E', 'salt123', 'tiandi@mes.com', '13800138001', '管理层', '项目组长', 1),
('userL', 'userL', 'L成员', 'E10ADC3949BA59ABBE56E057F20F883E', 'salt123', 'userl@mes.com', '13800138002', '物料部', '物料管理员', 1),
('userH', 'userH', 'H成员', 'E10ADC3949BA59ABBE56E057F20F883E', 'salt123', 'userh@mes.com', '13800138003', '生产部', '生产管理员', 1),
('userS', 'userS', 'S成员', 'E10ADC3949BA59ABBE56E057F20F883E', 'salt123', 'users@mes.com', '13800138004', '车间部', '车间管理员', 1);

-- =============================================
-- 用户角色关联初始数据
-- =============================================

-- 分配用户角色
INSERT INTO `sys_user_role` (`user_id`, `role_id`) VALUES
(1, 1), -- admin -> 系统管理员
(2, 1), -- tianDi -> 系统管理员
(3, 5), -- userL -> 物料经理
(4, 2), -- userH -> 生产经理
(5, 4); -- userS -> 车间主管

-- =============================================
-- 示例业务数据
-- =============================================

-- 插入示例物料数据	(L: 物料管理 - 2025/6/7 修改示例数据以匹配新的material表）
INSERT INTO `material` (`material_code`, `material_name`, `material_type`, `specification`, `unit`, `category`, `supplier`, `standard_cost`, `safety_stock`, `min_stock`, `max_stock`, 
`stock_quantity`, `lead_time`, `status`, `create_user_id`, `create_user_name`, `remark`, `version`) VALUES
('MAT001', '钢板A型', 'RAW_MATERIAL', '厚度: 10mm', 'PCS', '金属材料', '钢铁供应商A', 15.50, 100.00, 50.00, 500.00, 200.00, 7, 1, 1, 'admin', '优质钢板，用于机械加工', 1),
('MAT002', '螺丝M6', 'RAW_MATERIAL', 'M6x30mm', 'PCS', '紧固件', '五金供应商B', 0.50, 500.00, 200.00, 1000.00, 600.00, 3, 1, 1, 'admin', 'M6规格螺丝，镀锌处理', 1),
('MAT003', '电机组件', 'SEMI_FINISHED', '功率: 1KW', 'PCS', '电气组件', '电机厂商C', 120.00, 50.00, 20.00, 200.00, 80.00, 14, 1, 1, 'admin', '标准电机组件', 1),
('PROD001', '机械产品A', 'FINISHED_PRODUCT', '尺寸: 500x300x200mm', 'PCS', '机械设备', '', 1500.00, 20.00, 10.00, 100.00, 40.00, 21, 1, 1, 'admin', '机械产品A，包含多个组件', 1);

-- 插入示例车间数据
INSERT INTO `workshop` (`workshop_code`, `workshop_name`, `workshop_type`, `capacity`, `area`, `location`, `manager_name`, `status`, `description`) VALUES
('WS001', '机加工车间', '生产车间', 1000, 500.00, '厂区A栋1楼', '张三', '启用', '主要进行机械加工作业'),
('WS002', '装配车间', '装配车间', 800, 400.00, '厂区A栋2楼', '李四', '启用', '产品装配和测试'),
('WS003', '包装车间', '包装车间', 500, 200.00, '厂区B栋1楼', '王五', '启用', '产品包装和出货准备');

-- 插入示例生产订单数据
INSERT INTO `production_order` (`order_number`, `product_code`, `product_name`, `planned_quantity`, `unit`, `priority`, `status`, `planned_start_time`, `planned_end_time`, `workshop_name`, `customer`) VALUES
('PO20250601001', 'PROD001', '机械产品A', 100, 'PCS', '普通', '待开始', '2025-06-07 08:00:00', '2025-06-14 18:00:00', '机加工车间', '客户A公司'),
('PO20250601002', 'PROD001', '机械产品A', 50, 'PCS', '高', '待开始', '2025-06-08 08:00:00', '2025-06-12 18:00:00', '装配车间', '客户B公司');

-- 插入示例设备数据
INSERT INTO `equipment` (`equipment_code`, `equipment_name`, `equipment_type`, `workshop_id`, `workshop_name`, `status`, `manufacturer`, `model`, `purchase_date`, `install_date`, `enable_date`, `maintenance_cycle`, `location`, `responsible_person_name`) VALUES
('EQ001', '数控机床01', '加工设备', 1, '机加工车间', 1, '机床制造商A', 'CNC-2000', '2024-01-15', '2024-02-01', '2024-02-05', 30, '机加工车间A区', '张三'),
('EQ002', '装配线01', '装配设备', 2, '装配车间', 1, '自动化设备商B', 'ASM-500', '2024-03-10', '2024-03-20', '2024-03-25', 15, '装配车间B区', '李四'),
('EQ003', '包装机01', '包装设备', 3, '包装车间', 1, '包装设备商C', 'PKG-300', '2024-05-01', '2024-05-10', '2024-05-15', 20, '包装车间C区', '王五');

-- 输出插入结果
SELECT 'MES系统初始数据插入完成' AS result;
SELECT '默认管理员账号: admin/123456' AS admin_info;
SELECT '团队成员账号: tianDi/123456, userL/123456, userH/123456, userS/123456' AS team_info;
