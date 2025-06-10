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
-- 5. 车间作业信息表 (基于WorkshopOperationInfo模型)
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
-- 6. 批次信息表 (基于车间管理模块)
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
-- 7. 在制品信息表 (基于WIPInfo模型)
-- =============================================
CREATE TABLE `wip_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '在制品ID',
    `wip_id` VARCHAR(50) NOT NULL COMMENT '在制品编号（唯一标识）',
    `batch_number` VARCHAR(50) NULL COMMENT '批次号',
    `work_order_number` VARCHAR(50) NULL COMMENT '工单号',
    `product_id` INT NOT NULL COMMENT '产品ID',
    `product_code` VARCHAR(50) NOT NULL COMMENT '产品编码',
    `product_name` VARCHAR(200) NOT NULL COMMENT '产品名称',
    `workshop_id` INT NOT NULL COMMENT '当前车间ID',
    `workstation_id` INT NULL COMMENT '当前工位ID',
    `workstation_name` VARCHAR(100) NULL COMMENT '当前工位名称',
    `quantity` INT NOT NULL COMMENT '数量',
    `completed_quantity` INT DEFAULT 0 COMMENT '已完成数量',
    `status` INT DEFAULT 0 COMMENT '状态 (0:待开始, 1:生产中, 2:质检中, 3:暂停, 4:已完成)',
    `priority` INT DEFAULT 2 COMMENT '优先级 (1:低, 2:普通, 3:高, 4:紧急)',
    `start_time` DATETIME NOT NULL COMMENT '开始时间',
    `estimated_end_time` DATETIME NOT NULL COMMENT '预计完成时间',
    `actual_end_time` DATETIME NULL COMMENT '实际完成时间',
    `unit_price` DECIMAL(18,4) DEFAULT 0 COMMENT '单价',
    `quality_grade` VARCHAR(10) DEFAULT 'C' COMMENT '质量等级 (A:优秀, B:良好, C:合格, D:不合格)',
    `responsible_person` VARCHAR(100) NULL COMMENT '负责人',
    `remarks` TEXT NULL COMMENT '备注',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `update_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `create_by` VARCHAR(100) NULL COMMENT '创建人',
    `update_by` VARCHAR(100) NULL COMMENT '更新人',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    UNIQUE KEY `uk_wip_id` (`wip_id`),
    INDEX `idx_batch_number` (`batch_number`),
    INDEX `idx_work_order_number` (`work_order_number`),
    INDEX `idx_product_id` (`product_id`),
    INDEX `idx_product_code` (`product_code`),
    INDEX `idx_workshop_id` (`workshop_id`),
    INDEX `idx_status` (`status`),
    INDEX `idx_priority` (`priority`),
    INDEX `idx_start_time` (`start_time`),
    INDEX `idx_estimated_end_time` (`estimated_end_time`),
    INDEX `idx_responsible_person` (`responsible_person`),
    INDEX `idx_is_deleted` (`is_deleted`),
    FOREIGN KEY `fk_wip_workshop` (`workshop_id`) REFERENCES `workshop_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='在制品信息表';

-- =============================================
-- 8. 设备信息表 (基于EquipmentStatusInfo模型)
-- =============================================
CREATE TABLE `equipment_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '设备ID',
    `equipment_code` VARCHAR(50) NOT NULL COMMENT '设备编码（唯一标识）',
    `equipment_name` VARCHAR(200) NOT NULL COMMENT '设备名称',
    `equipment_type` VARCHAR(100) NULL COMMENT '设备类型名称',
    `equipment_type_id` INT NOT NULL COMMENT '设备类型ID (1:加工设备, 2:装配设备, 3:检测设备, 4:包装设备, 5:运输设备)',
    `workshop_id` INT NOT NULL COMMENT '所属车间ID',
    `location` VARCHAR(200) NULL COMMENT '设备位置',
    `status` INT DEFAULT 0 COMMENT '设备状态 (0:停止, 1:运行, 2:故障, 3:维护)',
    `efficiency` DECIMAL(5,2) DEFAULT 0.00 COMMENT '运行效率百分比',
    `temperature` DECIMAL(6,2) DEFAULT 0.00 COMMENT '温度(°C)',
    `pressure` DECIMAL(6,3) DEFAULT 0.000 COMMENT '压力(MPa)',
    `speed` DECIMAL(8,2) DEFAULT 0.00 COMMENT '转速(rpm)',
    `power` DECIMAL(8,2) DEFAULT 0.00 COMMENT '功率(kW)',
    `vibration` DECIMAL(6,3) DEFAULT 0.000 COMMENT '振动值',
    `last_maintenance` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '上次维护时间',
    `next_maintenance` DATETIME NOT NULL COMMENT '下次维护时间',
    `maintenance_cycle` INT DEFAULT 30 COMMENT '维护周期(天)',
    `operator` VARCHAR(100) NULL COMMENT '当前操作员',
    `operator_id` INT NULL COMMENT '操作员ID',
    `manufacturer` VARCHAR(200) NULL COMMENT '设备制造商',
    `model` VARCHAR(100) NULL COMMENT '设备型号',
    `purchase_date` DATE NULL COMMENT '购买日期',
    `warranty_until` DATE NULL COMMENT '保修期至',
    `value` DECIMAL(18,4) NULL COMMENT '设备价值',
    `is_enabled` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
    `remarks` TEXT NULL COMMENT '备注',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `update_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `create_by` VARCHAR(100) NULL COMMENT '创建人',
    `update_by` VARCHAR(100) NULL COMMENT '更新人',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    UNIQUE KEY `uk_equipment_code` (`equipment_code`),
    INDEX `idx_equipment_name` (`equipment_name`),
    INDEX `idx_equipment_type_id` (`equipment_type_id`),
    INDEX `idx_workshop_id` (`workshop_id`),
    INDEX `idx_status` (`status`),
    INDEX `idx_efficiency` (`efficiency`),
    INDEX `idx_next_maintenance` (`next_maintenance`),
    INDEX `idx_operator` (`operator`),
    INDEX `idx_is_enabled` (`is_enabled`),
    INDEX `idx_is_deleted` (`is_deleted`),
    FOREIGN KEY `fk_equipment_workshop` (`workshop_id`) REFERENCES `workshop_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='设备信息表';

-- =============================================
-- 9. 设备故障日志表
-- =============================================
CREATE TABLE `equipment_fault_log` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '故障日志ID',
    `equipment_code` VARCHAR(50) NOT NULL COMMENT '设备编码',
    `fault_description` TEXT NOT NULL COMMENT '故障描述',
    `fault_time` DATETIME NOT NULL COMMENT '故障时间',
    `operator` VARCHAR(100) NULL COMMENT '操作员',
    `repair_description` TEXT NULL COMMENT '维修描述',
    `repair_time` DATETIME NULL COMMENT '维修时间',
    `repair_person` VARCHAR(100) NULL COMMENT '维修人员',
    `status` INT DEFAULT 0 COMMENT '故障状态 (0:未处理, 1:处理中, 2:已修复)',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    INDEX `idx_equipment_code` (`equipment_code`),
    INDEX `idx_fault_time` (`fault_time`),
    INDEX `idx_status` (`status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='设备故障日志表';

-- =============================================
-- 10. 工艺路线表 (基于ProcessRoute模型)
-- =============================================
CREATE TABLE `process_route` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '工艺路线ID',
    `route_code` VARCHAR(50) NOT NULL COMMENT '工艺路线编码（唯一标识）',
    `route_name` VARCHAR(200) NOT NULL COMMENT '工艺路线名称',
    `product_id` INT NOT NULL COMMENT '产品ID',
    `version` VARCHAR(20) NOT NULL DEFAULT 'V1.0' COMMENT '版本号',
    `status` INT NOT NULL DEFAULT 0 COMMENT '状态 (0:草稿, 1:启用, 2:停用)',
    `description` TEXT NULL COMMENT '描述',
    `create_user_id` INT NOT NULL COMMENT '创建人ID',
    `create_user_name` VARCHAR(100) NULL COMMENT '创建人姓名',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `update_user_id` INT NULL COMMENT '更新人ID',
    `update_user_name` VARCHAR(100) NULL COMMENT '更新人姓名',
    `update_time` DATETIME NULL COMMENT '更新时间',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    UNIQUE KEY `uk_route_code` (`route_code`),
    INDEX `idx_route_name` (`route_name`),
    INDEX `idx_product_id` (`product_id`),
    INDEX `idx_status` (`status`),
    INDEX `idx_version` (`version`),
    INDEX `idx_create_time` (`create_time`),
    INDEX `idx_is_deleted` (`is_deleted`),
    FOREIGN KEY `fk_process_route_product` (`product_id`) REFERENCES `material_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='工艺路线表';

-- =============================================
-- 11. 工作站信息表 (支持工艺步骤)
-- =============================================
CREATE TABLE `workstation_info` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '工作站ID',
    `workstation_code` VARCHAR(50) NOT NULL COMMENT '工作站编码',
    `workstation_name` VARCHAR(200) NOT NULL COMMENT '工作站名称',
    `workshop_id` INT NOT NULL COMMENT '所属车间ID',
    `workstation_type` INT NOT NULL DEFAULT 1 COMMENT '工作站类型 (1:加工, 2:检验, 3:装配, 4:包装, 5:测试)',
    `capacity` INT NOT NULL DEFAULT 1 COMMENT '产能（件/小时）',
    `status` INT NOT NULL DEFAULT 1 COMMENT '状态 (0:停用, 1:启用, 2:维护)',
    `location` VARCHAR(200) NULL COMMENT '位置',
    `description` TEXT NULL COMMENT '描述',
    `is_enabled` TINYINT(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `update_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    UNIQUE KEY `uk_workstation_code` (`workstation_code`),
    INDEX `idx_workstation_name` (`workstation_name`),
    INDEX `idx_workshop_id` (`workshop_id`),
    INDEX `idx_workstation_type` (`workstation_type`),
    INDEX `idx_status` (`status`),
    INDEX `idx_is_enabled` (`is_enabled`),
    INDEX `idx_is_deleted` (`is_deleted`),
    FOREIGN KEY `fk_workstation_workshop` (`workshop_id`) REFERENCES `workshop_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='工作站信息表';

-- =============================================
-- 12. 工艺步骤表 (基于ProcessStep模型)
-- =============================================
CREATE TABLE `process_step` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '工艺步骤ID',
    `process_route_id` INT NOT NULL COMMENT '工艺路线ID',
    `step_number` INT NOT NULL COMMENT '步骤序号',
    `step_name` VARCHAR(200) NOT NULL COMMENT '步骤名称',
    `step_type` INT NOT NULL DEFAULT 1 COMMENT '步骤类型 (1:加工, 2:检验, 3:装配, 4:包装, 5:测试)',
    `workstation_id` INT NOT NULL COMMENT '工作站ID',
    `standard_time` DECIMAL(8,2) NOT NULL DEFAULT 0.00 COMMENT '标准工时（分钟）',
    `setup_time` DECIMAL(8,2) NOT NULL DEFAULT 0.00 COMMENT '准备时间（分钟）',
    `wait_time` DECIMAL(8,2) NOT NULL DEFAULT 0.00 COMMENT '等待时间（分钟）',
    `description` TEXT NULL COMMENT '步骤描述',
    `operation_instructions` TEXT NULL COMMENT '操作说明',
    `quality_requirements` TEXT NULL COMMENT '质量要求',
    `safety_notes` TEXT NULL COMMENT '安全注意事项',
    `required_skill_level` INT NOT NULL DEFAULT 1 COMMENT '所需技能等级 (1-10)',
    `is_critical` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否关键步骤',
    `requires_inspection` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否需要检验',
    `status` INT NOT NULL DEFAULT 1 COMMENT '状态 (0:停用, 1:启用)',
    `create_time` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `update_time` DATETIME NULL COMMENT '更新时间',
    `is_deleted` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
    INDEX `idx_process_route_id` (`process_route_id`),
    INDEX `idx_step_number` (`step_number`),
    INDEX `idx_step_name` (`step_name`),
    INDEX `idx_step_type` (`step_type`),
    INDEX `idx_workstation_id` (`workstation_id`),
    INDEX `idx_standard_time` (`standard_time`),
    INDEX `idx_is_critical` (`is_critical`),
    INDEX `idx_status` (`status`),
    INDEX `idx_is_deleted` (`is_deleted`),
    FOREIGN KEY `fk_process_step_route` (`process_route_id`) REFERENCES `process_route` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY `fk_process_step_workstation` (`workstation_id`) REFERENCES `workstation_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='工艺步骤表';



-- =============================================
-- 13. 创建复合索引优化查询性能
-- =============================================
CREATE INDEX `idx_material_type_status` ON `material_info` (`material_type`, `status`, `is_deleted`);
CREATE INDEX `idx_production_order_status_time` ON `production_order_info` (`status`, `plan_start_time`);
CREATE INDEX `idx_workshop_status_type` ON `workshop_info` (`status`, `workshop_type`);
CREATE INDEX `idx_work_order_status_time` ON `work_order_info` (`work_order_status`, `planned_start_time`);
CREATE INDEX `idx_wip_workshop_status` ON `wip_info` (`workshop_id`, `status`, `is_deleted`);
CREATE INDEX `idx_wip_status_priority` ON `wip_info` (`status`, `priority`, `start_time`);
CREATE INDEX `idx_wip_batch_status` ON `wip_info` (`batch_number`, `status`);
CREATE INDEX `idx_wip_workorder_status` ON `wip_info` (`work_order_number`, `status`);
CREATE INDEX `idx_equipment_workshop_status` ON `equipment_info` (`workshop_id`, `status`, `is_deleted`);
CREATE INDEX `idx_equipment_type_status` ON `equipment_info` (`equipment_type_id`, `status`, `is_enabled`);
CREATE INDEX `idx_equipment_maintenance` ON `equipment_info` (`next_maintenance`, `status`);
CREATE INDEX `idx_equipment_efficiency` ON `equipment_info` (`efficiency`, `status`);
CREATE INDEX `idx_process_route_product_status` ON `process_route` (`product_id`, `status`, `is_deleted`);
CREATE INDEX `idx_process_route_status_time` ON `process_route` (`status`, `create_time`);
CREATE INDEX `idx_process_step_route_number` ON `process_step` (`process_route_id`, `step_number`, `is_deleted`);
CREATE INDEX `idx_process_step_workstation_type` ON `process_step` (`workstation_id`, `step_type`, `status`);
CREATE INDEX `idx_workstation_workshop_type` ON `workstation_info` (`workshop_id`, `workstation_type`, `is_enabled`);

-- =============================================
-- 14. 插入初始数据
-- =============================================

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

-- 插入示例在制品数据
INSERT INTO `wip_info` (
    `wip_id`, `batch_number`, `work_order_number`, `product_id`, `product_code`, `product_name`,
    `workshop_id`, `workstation_name`, `quantity`, `completed_quantity`, `status`, `priority`,
    `start_time`, `estimated_end_time`, `actual_end_time`, `unit_price`, `quality_grade`,
    `responsible_person`, `remarks`, `create_by`, `update_by`
) VALUES
('WIP000001', 'B202506090001', 'WO202506090001', 4, 'PRD001', '产品A', 1, '工位A1', 100, 0, 0, 2, '2025-06-10 08:00:00', '2025-06-17 18:00:00', NULL, 1500.00, 'C', '张三', '待开始生产', '系统', '系统'),
('WIP000002', 'B202506090002', 'WO202506090002', 5, 'PRD002', '产品B', 1, '工位A2', 50, 10, 1, 3, '2025-06-09 08:00:00', '2025-06-20 18:00:00', NULL, 2200.00, 'B', '张三', '生产中', '系统', '系统'),
('WIP000003', 'B202506090003', 'WO202506090003', 4, 'PRD001', '产品A', 2, '工位B1', 80, 80, 4, 2, '2025-06-01 08:00:00', '2025-06-08 18:00:00', '2025-06-08 17:30:00', 1500.00, 'A', '李四', '已完成', '系统', '系统'),
('WIP000004', 'B202506090004', 'WO202506090004', 4, 'PRD001', '产品A', 3, '工位C1', 60, 30, 2, 2, '2025-06-11 08:00:00', '2025-06-18 18:00:00', NULL, 1500.00, 'B', '王五', '质检中', '系统', '系统'),
('WIP000005', 'B202506090005', 'WO202506090005', 5, 'PRD002', '产品B', 1, '工位A3', 40, 0, 3, 1, '2025-06-12 08:00:00', '2025-06-22 18:00:00', NULL, 2200.00, 'C', '张三', '暂停维护', '系统', '系统');

-- 插入示例设备数据
INSERT INTO `equipment_info` (
    `equipment_code`, `equipment_name`, `equipment_type`, `equipment_type_id`, `workshop_id`,
    `location`, `status`, `efficiency`, `temperature`, `pressure`, `speed`, `power`, `vibration`,
    `last_maintenance`, `next_maintenance`, `maintenance_cycle`, `operator`, `manufacturer`,
    `model`, `purchase_date`, `warranty_until`, `value`, `is_enabled`, `remarks`, `create_by`, `update_by`
) VALUES
('EQ0001', 'CNC加工中心01', '加工设备', 1, 1, 'A区-01号位', 1, 85.50, 45.2, 2.35, 1800.00, 15.5, 0.025, '2025-05-15 08:00:00', '2025-06-14 08:00:00', 30, '张三', '德马吉', 'DMU 50', '2023-01-15', '2026-01-15', 850000.00, 1, '运行正常', '系统', '系统'),
('EQ0002', 'CNC加工中心02', '加工设备', 1, 1, 'A区-02号位', 0, 0.00, 25.0, 0.00, 0.00, 0.0, 0.000, '2025-05-20 08:00:00', '2025-06-19 08:00:00', 30, NULL, '德马吉', 'DMU 50', '2023-01-20', '2026-01-20', 850000.00, 1, '停机待料', '系统', '系统'),
('EQ0003', '装配线01', '装配设备', 2, 2, 'B区-装配线', 1, 92.30, 28.5, 0.50, 120.00, 8.2, 0.015, '2025-05-10 08:00:00', '2025-06-09 08:00:00', 30, '李四', '博世', 'AL-2000', '2023-03-10', '2026-03-10', 450000.00, 1, '高效运行', '系统', '系统'),
('EQ0004', '装配线02', '装配设备', 2, 2, 'B区-装配线', 3, 0.00, 25.0, 0.00, 0.00, 0.0, 0.000, '2025-06-01 08:00:00', '2025-07-01 08:00:00', 30, '王五', '博世', 'AL-2000', '2023-03-15', '2026-03-15', 450000.00, 1, '计划维护中', '系统', '系统'),
('EQ0005', '质检设备01', '检测设备', 3, 3, 'C区-质检室', 1, 88.70, 22.8, 0.10, 0.00, 2.1, 0.005, '2025-05-25 08:00:00', '2025-06-24 08:00:00', 30, '赵六', '蔡司', 'CMM-500', '2023-02-01', '2026-02-01', 320000.00, 1, '精度良好', '系统', '系统'),
('EQ0006', '包装机01', '包装设备', 4, 4, 'D区-包装线', 2, 65.20, 35.5, 1.20, 450.00, 5.8, 0.035, '2025-05-18 08:00:00', '2025-06-17 08:00:00', 30, '钱七', '克朗斯', 'PK-300', '2023-04-01', '2026-04-01', 280000.00, 1, '故障待修', '系统', '系统'),
('EQ0007', '输送带01', '运输设备', 5, 1, 'A区-输送线', 1, 95.80, 30.2, 0.80, 60.00, 3.2, 0.010, '2025-05-12 08:00:00', '2025-06-11 08:00:00', 30, '孙八', '西门子', 'CV-1000', '2023-05-01', '2026-05-01', 120000.00, 1, '运行稳定', '系统', '系统'),
('EQ0008', '输送带02', '运输设备', 5, 2, 'B区-输送线', 1, 90.40, 29.8, 0.75, 65.00, 3.5, 0.012, '2025-05-14 08:00:00', '2025-06-13 08:00:00', 30, '周九', '西门子', 'CV-1000', '2023-05-05', '2026-05-05', 120000.00, 1, '运行正常', '系统', '系统');

-- 插入示例工作站数据
INSERT INTO `workstation_info` (
    `workstation_code`, `workstation_name`, `workshop_id`, `workstation_type`, `capacity`,
    `status`, `location`, `description`, `is_enabled`
) VALUES
('WS001', 'SMT生产线', 1, 1, 50, 1, 'A区-01', 'SMT表面贴装生产线', 1),
('WS002', '回流焊炉', 1, 1, 60, 1, 'A区-02', '高温回流焊接设备', 1),
('WS003', '测试工位', 1, 5, 30, 1, 'A区-03', '电路功能测试工位', 1),
('WS004', '装配线01', 2, 3, 40, 1, 'B区-01', '产品装配生产线', 1),
('WS005', '装配线02', 2, 3, 40, 2, 'B区-02', '产品装配生产线（维护中）', 1),
('WS006', '质检工位', 3, 2, 25, 1, 'C区-01', '产品质量检验工位', 1),
('WS007', '包装线01', 4, 4, 80, 1, 'D区-01', '产品包装生产线', 1),
('WS008', '包装线02', 4, 4, 80, 1, 'D区-02', '产品包装生产线', 1);

-- 插入示例工艺路线数据
INSERT INTO `process_route` (
    `route_code`, `route_name`, `product_id`, `version`, `status`, `description`,
    `create_user_id`, `create_user_name`
) VALUES
('PR001', '智能手机主板生产工艺', 4, 'V1.0', 1, 'SMT贴片+回流焊接+功能测试的完整工艺流程', 1, '系统'),
('PR002', '锂电池组装工艺', 5, 'V2.1', 0, '电芯检测+组装焊接的电池生产工艺', 1, '系统'),
('PR003', '产品A标准工艺', 4, 'V1.2', 1, '产品A的标准生产工艺路线', 1, '系统'),
('PR004', '产品B精密工艺', 5, 'V1.0', 2, '产品B的精密加工工艺路线（已停用）', 1, '系统');

-- 插入示例工艺步骤数据
INSERT INTO `process_step` (
    `process_route_id`, `step_number`, `step_name`, `step_type`, `workstation_id`,
    `standard_time`, `setup_time`, `wait_time`, `description`, `operation_instructions`,
    `quality_requirements`, `safety_notes`, `required_skill_level`, `is_critical`, `requires_inspection`
) VALUES
-- 工艺路线1的步骤
(1, 1, 'SMT贴片', 1, 1, 30.00, 5.00, 2.00, '表面贴装技术，将电子元件贴装到PCB上', '按照贴片程序进行自动贴片，注意元件方向', '贴片精度±0.05mm，无漏贴、错贴', '注意静电防护，佩戴防静电手套', 3, 1, 1),
(1, 2, '回流焊接', 1, 2, 15.00, 3.00, 1.00, '高温焊接工艺，使焊膏熔化形成焊点', '设置正确的温度曲线，监控焊接质量', '焊点饱满，无虚焊、连焊现象', '高温作业，注意防烫伤', 4, 1, 1),
(1, 3, '功能测试', 5, 3, 20.00, 2.00, 1.00, '电路功能检测，确保产品性能', '按照测试程序进行全功能测试', '所有功能正常，测试通过率≥99%', '注意用电安全', 2, 1, 1),
-- 工艺路线2的步骤
(2, 1, '电芯检测', 2, 6, 10.00, 1.00, 0.50, '电芯质量检测，筛选合格电芯', '使用专用设备检测电芯容量和内阻', '容量偏差≤2%，内阻≤规定值', '注意电池安全，防止短路', 2, 1, 1),
(2, 2, '组装焊接', 3, 4, 25.00, 5.00, 2.00, '电芯组装焊接，形成电池组', '按照工艺要求进行串并联焊接', '焊接牢固，绝缘良好', '焊接作业，注意通风', 4, 1, 1),
-- 工艺路线3的步骤
(3, 1, '预处理', 1, 1, 8.00, 2.00, 1.00, '产品预处理工序', '清洁和预处理产品表面', '表面清洁，无污染', '使用化学品时注意防护', 2, 0, 0),
(3, 2, '精密加工', 1, 2, 45.00, 10.00, 3.00, '精密加工工序', '按照图纸要求进行精密加工', '尺寸精度±0.01mm', '机械加工，注意安全操作', 5, 1, 1),
(3, 3, '质量检验', 2, 6, 15.00, 1.00, 0.50, '产品质量检验', '全面检验产品质量', '符合质量标准要求', '检验设备操作安全', 3, 1, 1),
(3, 4, '包装入库', 4, 7, 5.00, 1.00, 0.50, '产品包装入库', '按照包装标准进行包装', '包装完整，标识清楚', '搬运时注意轻拿轻放', 1, 0, 0);

-- 输出创建结果
SELECT 'MES系统数据库创建完成！' AS result;
SELECT '数据库名称: mes_db' AS database_name;
SELECT '已插入初始数据，包括车间、物料、生产订单等' AS initial_data;
