-- =============================================
-- MES制造执行系统数据库创建脚本
-- 基于已实现窗体的精确字段映射
-- 创建时间: 2025-06-09
-- 数据库: MySQL 8.0
-- 目标数据库: mes_db
-- =============================================

-- 创建数据库
DROP DATABASE IF EXISTS `mes_db`;
CREATE DATABASE `mes_db`
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

-- 使用数据库
USE `mes_db`;

-- 设置SQL模式
SET SQL_MODE = 'STRICT_TRANS_TABLES,NO_ZERO_DATE,NO_ZERO_IN_DATE,ERROR_FOR_DIVISION_BY_ZERO';

-- 设置时区
SET time_zone = '+08:00';

-- =============================================
-- 1. 物料信息表 (基于MaterialInfo模型和MaterialDAL)
-- =============================================
CREATE TABLE `material_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '物料ID',
    `material_code` VARCHAR(50) NOT NULL COMMENT '物料编码',
    `material_name` VARCHAR(200) NOT NULL COMMENT '物料名称',
    `material_type` VARCHAR(50) NOT NULL COMMENT '物料类型',
    `category` VARCHAR(100) NULL COMMENT '物料分类',
    `specification` VARCHAR(200) NULL COMMENT '规格型号',
    `unit` VARCHAR(20) NOT NULL COMMENT '计量单位',
    `standard_cost` DECIMAL(18,4) NULL COMMENT '标准成本',
    `safety_stock` DECIMAL(18,2) NULL COMMENT '安全库存',
    `min_stock` DECIMAL(18,2) NULL COMMENT '最小库存',
    `max_stock` DECIMAL(18,2) NULL COMMENT '最大库存',
    `stock_quantity` DECIMAL(18,2) NULL DEFAULT 0 COMMENT '当前库存数量',
    `supplier` VARCHAR(200) NULL COMMENT '供应商',
    `lead_time` INT NULL COMMENT '采购提前期（天）',
    `price` DECIMAL(18,4) NULL COMMENT '价格',
    `status` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `update_time` DATETIME NULL COMMENT '最后修改时间',
    `update_user_id` INT NULL COMMENT '最后修改人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '最后修改人姓名',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    `delete_time` DATETIME NULL COMMENT '删除时间',
    `delete_user_id` INT NULL COMMENT '删除人ID',
    `delete_user_name` VARCHAR(100) NULL COMMENT '删除人姓名',
    `remark` TEXT NULL COMMENT '备注',
    `version` INT NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
    UNIQUE KEY `uk_material_code` (`material_code`),
    INDEX `idx_material_type` (`material_type`),
    INDEX `idx_category` (`category`),
    INDEX `idx_status` (`status`),
    INDEX `idx_is_deleted` (`is_deleted`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='物料信息表';

-- =============================================
-- 2. 车间信息表 (基于WorkshopInfo模型)
-- =============================================
CREATE TABLE `workshop_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '车间ID',
    `workshop_code` VARCHAR(50) NOT NULL COMMENT '车间编码（唯一标识）',
    `workshop_name` VARCHAR(200) NOT NULL COMMENT '车间名称',
    `department` VARCHAR(100) NULL COMMENT '所属部门',
    `manager` VARCHAR(100) NULL COMMENT '车间负责人',
    `manager_id` INT NULL COMMENT '负责人ID (关联操作员/用户)',
    `phone` VARCHAR(20) NULL COMMENT '联系电话',
    `location` VARCHAR(200) NULL COMMENT '车间位置/地址',
    `area` DECIMAL(10,2) DEFAULT 0 COMMENT '车间面积（平方米）',
    `equipment_count` INT DEFAULT 0 COMMENT '设备数量',
    `employee_count` INT DEFAULT 0 COMMENT '员工数量',
    `workshop_type` INT DEFAULT 1 COMMENT '车间类型：1-生产车间，2-装配车间，3-包装车间，4-质检车间，5-仓储车间',
    `production_capacity` INT DEFAULT 0 COMMENT '生产能力（件/天）',
    `status` VARCHAR(50) DEFAULT '1' COMMENT '车间状态：0-停用，1-正常运行，2-维护中，3-故障停机',
    `work_shift` INT DEFAULT 1 COMMENT '工作班次：1-单班，2-两班，3-三班',
    `safety_level` INT DEFAULT 1 COMMENT '安全等级：1-一般，2-重要，3-关键',
    `environment_requirement` VARCHAR(500) NULL COMMENT '环境要求',
    `quality_standard` VARCHAR(500) NULL COMMENT '质量标准',
    `description` TEXT NULL COMMENT '描述信息',
    `equipment_list` TEXT NULL COMMENT '设备列表 (逗号分隔的设备编码)',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `update_time` DATETIME NULL COMMENT '最后修改时间',
    `update_user_id` INT NULL COMMENT '最后修改人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '最后修改人姓名',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    `delete_time` DATETIME NULL COMMENT '删除时间',
    `delete_user_id` INT NULL COMMENT '删除人ID',
    `delete_user_name` VARCHAR(100) NULL COMMENT '删除人姓名',
    `remark` TEXT NULL COMMENT '备注',
    `version` INT NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
    UNIQUE KEY `uk_workshop_code` (`workshop_code`),
    INDEX `idx_workshop_name` (`workshop_name`),
    INDEX `idx_workshop_type` (`workshop_type`),
    INDEX `idx_manager_id` (`manager_id`),
    INDEX `idx_status` (`status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='车间信息表';

-- =============================================
-- 3. 生产订单表 (基于ProductionOrderInfo模型)
-- =============================================
CREATE TABLE `production_order_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '生产订单ID',
    `order_no` VARCHAR(50) NOT NULL COMMENT '订单编号（唯一标识）',
    `material_id` INT DEFAULT 0 COMMENT '物料ID（关联物料表）',
    `product_code` VARCHAR(50) NOT NULL COMMENT '产品编号',
    `product_name` VARCHAR(200) NOT NULL COMMENT '产品名称',
    `planned_quantity` DECIMAL(18,4) NOT NULL COMMENT '计划生产数量',
    `actual_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '实际完成数量',
    `unit` VARCHAR(20) DEFAULT '个' COMMENT '单位',
    `plan_start_time` DATETIME NOT NULL COMMENT '计划开始时间',
    `plan_end_time` DATETIME NOT NULL COMMENT '计划完成时间',
    `actual_start_time` DATETIME NULL COMMENT '实际开始时间',
    `actual_end_time` DATETIME NULL COMMENT '实际完成时间',
    `status` VARCHAR(50) DEFAULT '待开始' COMMENT '订单状态：待开始，进行中，已完成，已暂停，已取消',
    `priority` VARCHAR(50) DEFAULT '普通' COMMENT '优先级：普通、重要、紧急等',
    `workshop_id` INT DEFAULT 0 COMMENT '负责车间ID',
    `workshop_name` VARCHAR(200) NULL COMMENT '车间名称',
    `responsible_person` VARCHAR(100) NULL COMMENT '负责人',
    `customer_name` VARCHAR(200) NULL COMMENT '客户名称',
    `sales_order_number` VARCHAR(50) NULL COMMENT '销售订单号',
    `remarks` TEXT NULL COMMENT '备注信息',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `update_time` DATETIME NULL COMMENT '最后修改时间',
    `update_user_id` INT NULL COMMENT '最后修改人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '最后修改人姓名',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    `delete_time` DATETIME NULL COMMENT '删除时间',
    `delete_user_id` INT NULL COMMENT '删除人ID',
    `delete_user_name` VARCHAR(100) NULL COMMENT '删除人姓名',
    `remark` TEXT NULL COMMENT '备注',
    `version` INT NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
    UNIQUE KEY `uk_order_no` (`order_no`),
    INDEX `idx_product_code` (`product_code`),
    INDEX `idx_status` (`status`),
    INDEX `idx_priority` (`priority`),
    INDEX `idx_workshop_id` (`workshop_id`),
    INDEX `idx_plan_start_time` (`plan_start_time`),
    INDEX `idx_customer_name` (`customer_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='生产订单表';

-- =============================================
-- 4. 工单信息表 (基于WorkOrderInfo模型)
-- =============================================
CREATE TABLE `work_order_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '工单ID',
    `work_order_id` INT DEFAULT 0 COMMENT '工单ID',
    `work_order_num` VARCHAR(50) NOT NULL COMMENT '工单号',
    `work_order_type` VARCHAR(50) NULL COMMENT '工单类型',
    `product_id` INT DEFAULT 0 COMMENT '产品ID',
    `flow_id` INT DEFAULT 0 COMMENT '工艺流程ID',
    `bom_id` INT DEFAULT 0 COMMENT 'BOM ID',
    `planned_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '计划数量',
    `input_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '投入数量',
    `output_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '产出数量',
    `scrap_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '报废数量',
    `work_order_status` INT DEFAULT 0 COMMENT '工单状态(0:未开始,1:进行中,2:已完成,3:已关闭)',
    `process_status` VARCHAR(50) NULL COMMENT '工艺状态',
    `lock_status` INT DEFAULT 0 COMMENT '锁定状态(0:未锁定,1:已锁定)',
    `factory_id` INT DEFAULT 0 COMMENT '工厂ID',
    `hot_type` VARCHAR(50) NULL COMMENT 'Hot类型',
    `planned_start_time` DATETIME NULL COMMENT '计划开始时间',
    `planned_due_date` DATETIME NULL COMMENT '计划到期日',
    `production_start_time` DATETIME NULL COMMENT '投产时间',
    `completion_time` DATETIME NULL COMMENT '完成时间',
    `close_time` DATETIME NULL COMMENT '关闭时间',
    `work_order_version` VARCHAR(50) NULL COMMENT '工单版本',
    `parent_work_order_version` VARCHAR(50) NULL COMMENT '父工单版本',
    `product_order_no` VARCHAR(50) NULL COMMENT '产品订单号',
    `product_order_version` VARCHAR(50) NULL COMMENT '产品订单版本',
    `sales_order_no` VARCHAR(50) NULL COMMENT '销售单号',
    `main_batch_no` VARCHAR(50) NULL COMMENT '主批次号',
    `description` TEXT NULL COMMENT '说明',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `update_time` DATETIME NULL COMMENT '最后修改时间',
    `update_user_id` INT NULL COMMENT '最后修改人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '最后修改人姓名',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    `delete_time` DATETIME NULL COMMENT '删除时间',
    `delete_user_id` INT NULL COMMENT '删除人ID',
    `delete_user_name` VARCHAR(100) NULL COMMENT '删除人姓名',
    `remark` TEXT NULL COMMENT '备注',
    `version` INT NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
    UNIQUE KEY `uk_work_order_num` (`work_order_num`),
    INDEX `idx_product_id` (`product_id`),
    INDEX `idx_work_order_status` (`work_order_status`),
    INDEX `idx_factory_id` (`factory_id`),
    INDEX `idx_planned_start_time` (`planned_start_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='工单信息表';

-- =============================================
-- 5. 用户信息表 (基于UserInfo模型)
-- =============================================
CREATE TABLE `user_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '用户ID',
    `user_code` VARCHAR(50) NOT NULL COMMENT '用户编码',
    `user_name` VARCHAR(100) NOT NULL COMMENT '用户姓名',
    `login_name` VARCHAR(100) NOT NULL COMMENT '登录名',
    `password` VARCHAR(255) NOT NULL COMMENT '密码（加密后）',
    `department` VARCHAR(100) NULL COMMENT '部门',
    `position` VARCHAR(100) NULL COMMENT '职位',
    `email` VARCHAR(200) NULL COMMENT '邮箱',
    `phone` VARCHAR(20) NULL COMMENT '电话',
    `status` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `last_login_time` DATETIME NULL COMMENT '最后登录时间',
    `role_id` INT NULL COMMENT '角色ID',
    `role_name` VARCHAR(100) NULL COMMENT '角色名称',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `update_time` DATETIME NULL COMMENT '最后修改时间',
    `update_user_id` INT NULL COMMENT '最后修改人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '最后修改人姓名',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    `delete_time` DATETIME NULL COMMENT '删除时间',
    `delete_user_id` INT NULL COMMENT '删除人ID',
    `delete_user_name` VARCHAR(100) NULL COMMENT '删除人姓名',
    `remark` TEXT NULL COMMENT '备注',
    `version` INT NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
    UNIQUE KEY `uk_user_code` (`user_code`),
    UNIQUE KEY `uk_login_name` (`login_name`),
    INDEX `idx_user_name` (`user_name`),
    INDEX `idx_department` (`department`),
    INDEX `idx_status` (`status`),
    INDEX `idx_role_id` (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='用户信息表';

-- =============================================
-- 6. 车间作业信息表 (基于WorkshopOperationInfo模型)
-- =============================================
CREATE TABLE `workshop_operation_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '作业ID',
    `operation_id` VARCHAR(50) NOT NULL COMMENT '作业编号',
    `workshop_name` VARCHAR(200) NOT NULL COMMENT '车间名称',
    `batch_number` VARCHAR(50) NOT NULL COMMENT '批次号',
    `product_code` VARCHAR(50) NOT NULL COMMENT '产品编码',
    `quantity` DECIMAL(18,4) NOT NULL COMMENT '数量',
    `status` INT DEFAULT 0 COMMENT '状态：0-待开始，1-进行中，2-已暂停，3-已完成，4-已停止',
    `status_text` VARCHAR(50) DEFAULT '待开始' COMMENT '状态文本',
    `start_time` DATETIME NULL COMMENT '开始时间',
    `progress` DECIMAL(5,2) DEFAULT 0 COMMENT '进度(%)',
    `operator` VARCHAR(100) NULL COMMENT '操作员',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `update_time` DATETIME NULL COMMENT '最后修改时间',
    `update_user_id` INT NULL COMMENT '最后修改人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '最后修改人姓名',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    `delete_time` DATETIME NULL COMMENT '删除时间',
    `delete_user_id` INT NULL COMMENT '删除人ID',
    `delete_user_name` VARCHAR(100) NULL COMMENT '删除人姓名',
    `remark` TEXT NULL COMMENT '备注',
    `version` INT NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
    UNIQUE KEY `uk_operation_id` (`operation_id`),
    INDEX `idx_workshop_name` (`workshop_name`),
    INDEX `idx_batch_number` (`batch_number`),
    INDEX `idx_product_code` (`product_code`),
    INDEX `idx_status` (`status`),
    INDEX `idx_operator` (`operator`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='车间作业信息表';

-- =============================================
-- 7. 批次信息表 (基于车间管理模块)
-- =============================================
CREATE TABLE `batch_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '批次ID',
    `batch_number` VARCHAR(50) NOT NULL COMMENT '批次号',
    `workshop_id` INT NOT NULL COMMENT '车间ID',
    `workshop_name` VARCHAR(200) NULL COMMENT '车间名称',
    `production_order_id` INT DEFAULT 0 COMMENT '生产订单ID',
    `production_order_number` VARCHAR(50) NULL COMMENT '生产订单号',
    `product_code` VARCHAR(50) NULL COMMENT '产品编码',
    `product_name` VARCHAR(200) NULL COMMENT '产品名称',
    `planned_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '计划数量',
    `actual_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '实际数量',
    `unit` VARCHAR(20) DEFAULT '个' COMMENT '单位',
    `batch_status` VARCHAR(50) DEFAULT '待开始' COMMENT '批次状态',
    `start_time` DATETIME NULL COMMENT '开始时间',
    `end_time` DATETIME NULL COMMENT '结束时间',
    `operator_id` INT DEFAULT 0 COMMENT '操作员ID',
    `operator_name` VARCHAR(100) NULL COMMENT '操作员姓名',
    `remarks` TEXT NULL COMMENT '备注',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `update_time` DATETIME NULL COMMENT '最后修改时间',
    `update_user_id` INT NULL COMMENT '最后修改人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '最后修改人姓名',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    `delete_time` DATETIME NULL COMMENT '删除时间',
    `delete_user_id` INT NULL COMMENT '删除人ID',
    `delete_user_name` VARCHAR(100) NULL COMMENT '删除人姓名',
    `remark` TEXT NULL COMMENT '备注',
    `version` INT NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
    UNIQUE KEY `uk_batch_number` (`batch_number`),
    INDEX `idx_workshop_id` (`workshop_id`),
    INDEX `idx_production_order_id` (`production_order_id`),
    INDEX `idx_batch_status` (`batch_status`),
    INDEX `idx_operator_id` (`operator_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='批次信息表';

-- =============================================
-- 8. 创建复合索引优化查询性能
-- =============================================
CREATE INDEX `idx_material_type_status` ON `material_info` (`material_type`, `status`, `is_deleted`);
CREATE INDEX `idx_production_order_status_time` ON `production_order_info` (`status`, `plan_start_time`);
CREATE INDEX `idx_workshop_status_type` ON `workshop_info` (`status`, `workshop_type`);
CREATE INDEX `idx_work_order_status_time` ON `work_order_info` (`work_order_status`, `planned_start_time`);

-- =============================================
-- 9. 插入初始数据
-- =============================================

-- 插入默认管理员用户
INSERT INTO `user_info` (
    `user_code`, `user_name`, `login_name`, `password`, `department`, `position`,
    `email`, `phone`, `status`, `role_name`, `create_user_name`
) VALUES (
    'ADMIN001', '系统管理员', 'admin', 'e10adc3949ba59abbe56e057f20f883e',
    '信息技术部', '系统管理员', 'admin@mes.com', '13800138000', 1, '系统管理员', '系统'
);

-- 插入示例车间数据
INSERT INTO `workshop_info` (
    `workshop_code`, `workshop_name`, `department`, `manager`, `location`,
    `area`, `workshop_type`, `production_capacity`, `status`, `create_user_name`
) VALUES
('WS001', '生产车间A', '生产部', '张三', '1号厂房', 500.00, 1, 1000, '1', '系统'),
('WS002', '装配车间B', '生产部', '李四', '2号厂房', 300.00, 2, 800, '1', '系统'),
('WS003', '包装车间C', '生产部', '王五', '3号厂房', 200.00, 3, 600, '1', '系统'),
('WS004', '质检车间D', '质量部', '轩天帝', '4号厂房', 150.00, 4, 400, '1', '系统');

-- 插入示例物料数据
INSERT INTO `material_info` (
    `material_code`, `material_name`, `material_type`, `category`, `specification`,
    `unit`, `standard_cost`, `safety_stock`, `min_stock`, `max_stock`, `stock_quantity`,
    `supplier`, `lead_time`, `price`, `status`, `create_user_name`
) VALUES
('MAT001', '钢板A型', '原材料', '金属材料', '1000*2000*5mm', '张', 150.00, 50.00, 20.00, 200.00, 100.00, '钢铁供应商A', 7, 150.00, 1, '系统'),
('MAT002', '螺栓M8', '标准件', '紧固件', 'M8*25', '个', 0.50, 1000.00, 500.00, 5000.00, 2000.00, '紧固件供应商B', 3, 0.50, 1, '系统'),
('MAT003', '电机220V', '电气元件', '动力设备', '220V/1.5KW', '台', 280.00, 10.00, 5.00, 50.00, 20.00, '电机供应商C', 14, 280.00, 1, '系统'),
('PRD001', '产品A', '成品', '机械产品', 'A型机械设备', '台', 1500.00, 5.00, 2.00, 20.00, 8.00, '', 0, 1500.00, 1, '系统'),
('PRD002', '产品B', '成品', '机械产品', 'B型机械设备', '台', 2200.00, 3.00, 1.00, 15.00, 5.00, '', 0, 2200.00, 1, '系统');

-- 插入示例生产订单数据
INSERT INTO `production_order_info` (
    `order_no`, `material_id`, `product_code`, `product_name`, `planned_quantity`,
    `actual_quantity`, `unit`, `plan_start_time`, `plan_end_time`, `status`,
    `priority`, `workshop_id`, `workshop_name`, `responsible_person`,
    `customer_name`, `sales_order_number`, `create_user_name`
) VALUES
('PO202506090001', 4, 'PRD001', '产品A', 100.00, 0.00, '台', '2025-06-10 08:00:00', '2025-06-17 18:00:00', '待开始', '普通', 1, '生产车间A', '张三', '客户甲', 'SO202506090001', '系统'),
('PO202506090002', 5, 'PRD002', '产品B', 50.00, 10.00, '台', '2025-06-09 08:00:00', '2025-06-20 18:00:00', '进行中', '重要', 1, '生产车间A', '张三', '客户乙', 'SO202506090002', '系统'),
('PO202506090003', 4, 'PRD001', '产品A', 80.00, 80.00, '台', '2025-06-01 08:00:00', '2025-06-08 18:00:00', '已完成', '普通', 2, '装配车间B', '李四', '客户丙', 'SO202506090003', '系统');

-- 插入示例工单数据
INSERT INTO `work_order_info` (
    `work_order_num`, `work_order_type`, `product_id`, `planned_quantity`,
    `input_quantity`, `output_quantity`, `work_order_status`, `factory_id`,
    `planned_start_time`, `planned_due_date`, `product_order_no`,
    `sales_order_no`, `main_batch_no`, `description`, `create_user_name`
) VALUES
('WO202506090001', '生产工单', 1, 100.00, 0.00, 0.00, 0, 1, '2025-06-10 08:00:00', '2025-06-17 18:00:00', 'PO202506090001', 'SO202506090001', 'B202506090001', '产品A生产工单', '系统'),
('WO202506090002', '生产工单', 2, 50.00, 50.00, 10.00, 1, 1, '2025-06-09 08:00:00', '2025-06-20 18:00:00', 'PO202506090002', 'SO202506090002', 'B202506090002', '产品B生产工单', '系统'),
('WO202506090003', '生产工单', 1, 80.00, 80.00, 80.00, 2, 1, '2025-06-01 08:00:00', '2025-06-08 18:00:00', 'PO202506090003', 'SO202506090003', 'B202506090003', '产品A生产工单', '系统');

-- 插入示例批次数据
INSERT INTO `batch_info` (
    `batch_number`, `workshop_id`, `workshop_name`, `production_order_id`,
    `production_order_number`, `product_code`, `product_name`, `planned_quantity`,
    `actual_quantity`, `unit`, `batch_status`, `operator_name`, `create_user_name`
) VALUES
('B202506090001', 1, '生产车间A', 1, 'PO202506090001', 'PRD001', '产品A', 100.00, 0.00, '台', '待开始', '张三', '系统'),
('B202506090002', 1, '生产车间A', 2, 'PO202506090002', 'PRD002', '产品B', 50.00, 10.00, '台', '进行中', '张三', '系统'),
('B202506090003', 2, '装配车间B', 3, 'PO202506090003', 'PRD001', '产品A', 80.00, 80.00, '台', '已完成', '李四', '系统');

-- 插入示例车间作业数据
INSERT INTO `workshop_operation_info` (
    `operation_id`, `workshop_name`, `batch_number`, `product_code`, `quantity`,
    `status`, `status_text`, `start_time`, `progress`, `operator`, `create_user_name`
) VALUES
('OP000001', '生产车间A', 'B202506090001', 'PRD001', 100.00, 0, '待开始', NULL, 0.00, '张三', '系统'),
('OP000002', '生产车间A', 'B202506090002', 'PRD002', 50.00, 1, '进行中', '2025-06-09 08:00:00', 20.00, '张三', '系统'),
('OP000003', '装配车间B', 'B202506090003', 'PRD001', 80.00, 3, '已完成', '2025-06-01 08:00:00', 100.00, '李四', '系统');

-- 输出创建结果
SELECT 'MES系统数据库创建完成！' AS result;
SELECT '数据库名称: mes_db' AS database_name;
SELECT '已插入初始数据，包括用户、车间、物料、生产订单等' AS initial_data;
