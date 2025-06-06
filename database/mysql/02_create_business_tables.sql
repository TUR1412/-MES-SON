-- =============================================
-- MES制造执行系统 - 业务表创建脚本
-- 版本: 1.0
-- 创建时间: 2025-06-06
-- 说明: 创建物料、生产、车间、设备、质量等业务表
-- =============================================

USE `mes_system`;

-- =============================================
-- 物料管理相关表
-- =============================================

-- 物料信息表
CREATE TABLE `material` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '物料ID',
    `material_code` VARCHAR(50) NOT NULL COMMENT '物料编码',
    `material_name` VARCHAR(200) NOT NULL COMMENT '物料名称',
    `material_type` VARCHAR(100) DEFAULT '' COMMENT '物料类型',
    `specification` VARCHAR(500) DEFAULT '' COMMENT '规格型号',
    `unit` VARCHAR(20) DEFAULT '' COMMENT '计量单位',
    `safety_stock` DECIMAL(18,4) DEFAULT 0 COMMENT '安全库存',
    `max_stock` DECIMAL(18,4) DEFAULT 0 COMMENT '最大库存',
    `min_stock` DECIMAL(18,4) DEFAULT 0 COMMENT '最小库存',
    `current_stock` DECIMAL(18,4) DEFAULT 0 COMMENT '当前库存',
    `unit_price` DECIMAL(18,4) DEFAULT 0 COMMENT '单价',
    `supplier` VARCHAR(200) DEFAULT '' COMMENT '供应商',
    `manufacturer` VARCHAR(200) DEFAULT '' COMMENT '制造商',
    `shelf_life` INT DEFAULT 0 COMMENT '保质期(天)',
    `storage_location` VARCHAR(200) DEFAULT '' COMMENT '存储位置',
    `status` TINYINT DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `description` TEXT COMMENT '物料描述',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_material_code` (`material_code`),
    INDEX `idx_material_name` (`material_name`),
    INDEX `idx_material_type` (`material_type`),
    INDEX `idx_supplier` (`supplier`),
    INDEX `idx_status` (`status`),
    INDEX `idx_create_time` (`create_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='物料信息表';

-- BOM清单表
CREATE TABLE `bom` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT 'BOM ID',
    `bom_code` VARCHAR(50) NOT NULL COMMENT 'BOM编码',
    `bom_name` VARCHAR(200) NOT NULL COMMENT 'BOM名称',
    `parent_material_id` INT NOT NULL COMMENT '父物料ID',
    `parent_material_code` VARCHAR(50) NOT NULL COMMENT '父物料编码',
    `child_material_id` INT NOT NULL COMMENT '子物料ID',
    `child_material_code` VARCHAR(50) NOT NULL COMMENT '子物料编码',
    `quantity` DECIMAL(18,4) NOT NULL DEFAULT 1 COMMENT '用量',
    `unit` VARCHAR(20) DEFAULT '' COMMENT '单位',
    `loss_rate` DECIMAL(5,4) DEFAULT 0 COMMENT '损耗率',
    `substitute_material_id` INT DEFAULT NULL COMMENT '替代物料ID',
    `substitute_material_code` VARCHAR(50) DEFAULT '' COMMENT '替代物料编码',
    `level` INT DEFAULT 1 COMMENT 'BOM层级',
    `sequence` INT DEFAULT 0 COMMENT '序号',
    `effective_date` DATE DEFAULT NULL COMMENT '生效日期',
    `expire_date` DATE DEFAULT NULL COMMENT '失效日期',
    `status` TINYINT DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `description` VARCHAR(500) DEFAULT '' COMMENT '描述',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_bom_code` (`bom_code`),
    INDEX `idx_parent_material` (`parent_material_id`, `parent_material_code`),
    INDEX `idx_child_material` (`child_material_id`, `child_material_code`),
    INDEX `idx_level` (`level`),
    INDEX `idx_status` (`status`),
    CONSTRAINT `fk_bom_parent_material` FOREIGN KEY (`parent_material_id`) REFERENCES `material` (`id`),
    CONSTRAINT `fk_bom_child_material` FOREIGN KEY (`child_material_id`) REFERENCES `material` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='BOM清单表';

-- =============================================
-- 车间管理相关表
-- =============================================

-- 车间信息表
CREATE TABLE `workshop` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '车间ID',
    `workshop_code` VARCHAR(50) NOT NULL COMMENT '车间编码',
    `workshop_name` VARCHAR(200) NOT NULL COMMENT '车间名称',
    `workshop_type` VARCHAR(100) DEFAULT '' COMMENT '车间类型',
    `capacity` INT DEFAULT 0 COMMENT '产能',
    `area` DECIMAL(10,2) DEFAULT 0 COMMENT '面积(平方米)',
    `location` VARCHAR(200) DEFAULT '' COMMENT '位置',
    `manager_id` INT DEFAULT 0 COMMENT '负责人ID',
    `manager_name` VARCHAR(100) DEFAULT '' COMMENT '负责人姓名',
    `equipment_list` TEXT COMMENT '设备列表(逗号分隔)',
    `status` VARCHAR(50) DEFAULT '启用' COMMENT '状态',
    `description` TEXT COMMENT '车间描述',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_workshop_code` (`workshop_code`),
    INDEX `idx_workshop_name` (`workshop_name`),
    INDEX `idx_workshop_type` (`workshop_type`),
    INDEX `idx_manager_id` (`manager_id`),
    INDEX `idx_status` (`status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='车间信息表';

-- =============================================
-- 生产管理相关表
-- =============================================

-- 生产订单表
CREATE TABLE `production_order` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '生产订单ID',
    `order_number` VARCHAR(50) NOT NULL COMMENT '订单号',
    `product_code` VARCHAR(50) NOT NULL COMMENT '产品编码',
    `product_name` VARCHAR(200) NOT NULL COMMENT '产品名称',
    `planned_quantity` DECIMAL(18,4) NOT NULL COMMENT '计划数量',
    `actual_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '实际数量',
    `unit` VARCHAR(20) DEFAULT '' COMMENT '单位',
    `priority` VARCHAR(50) DEFAULT '普通' COMMENT '优先级',
    `status` VARCHAR(50) DEFAULT '待开始' COMMENT '状态',
    `planned_start_time` DATETIME NOT NULL COMMENT '计划开始时间',
    `planned_end_time` DATETIME NOT NULL COMMENT '计划结束时间',
    `actual_start_time` DATETIME DEFAULT NULL COMMENT '实际开始时间',
    `actual_end_time` DATETIME DEFAULT NULL COMMENT '实际结束时间',
    `workshop_id` INT DEFAULT 0 COMMENT '车间ID',
    `workshop_name` VARCHAR(200) DEFAULT '' COMMENT '车间名称',
    `customer` VARCHAR(200) DEFAULT '' COMMENT '客户',
    `sales_order_number` VARCHAR(50) DEFAULT '' COMMENT '销售订单号',
    `remarks` TEXT COMMENT '备注',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_order_number` (`order_number`),
    INDEX `idx_product_code` (`product_code`),
    INDEX `idx_status` (`status`),
    INDEX `idx_priority` (`priority`),
    INDEX `idx_workshop_id` (`workshop_id`),
    INDEX `idx_planned_start_time` (`planned_start_time`),
    INDEX `idx_customer` (`customer`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='生产订单表';

-- =============================================
-- 设备管理相关表
-- =============================================

-- 设备信息表
CREATE TABLE `equipment` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '设备ID',
    `equipment_code` VARCHAR(50) NOT NULL COMMENT '设备编码',
    `equipment_name` VARCHAR(200) NOT NULL COMMENT '设备名称',
    `equipment_type` VARCHAR(100) DEFAULT '' COMMENT '设备类型',
    `workshop_id` INT DEFAULT 0 COMMENT '所属车间ID',
    `workshop_name` VARCHAR(200) DEFAULT '' COMMENT '所属车间名称',
    `status` INT DEFAULT 1 COMMENT '设备状态：1-正常，2-维护中，3-故障，4-停用',
    `specification` VARCHAR(500) DEFAULT '' COMMENT '设备规格',
    `manufacturer` VARCHAR(200) DEFAULT '' COMMENT '制造商',
    `model` VARCHAR(100) DEFAULT '' COMMENT '设备型号',
    `purchase_date` DATE DEFAULT NULL COMMENT '购买日期',
    `install_date` DATE DEFAULT NULL COMMENT '安装日期',
    `enable_date` DATE DEFAULT NULL COMMENT '启用日期',
    `last_maintenance_date` DATE DEFAULT NULL COMMENT '最后维护日期',
    `next_maintenance_date` DATE DEFAULT NULL COMMENT '下次维护日期',
    `maintenance_cycle` INT DEFAULT 30 COMMENT '维护周期(天)',
    `location` VARCHAR(200) DEFAULT '' COMMENT '设备位置',
    `responsible_person_id` INT DEFAULT 0 COMMENT '负责人ID',
    `responsible_person_name` VARCHAR(100) DEFAULT '' COMMENT '负责人姓名',
    `description` TEXT COMMENT '设备描述',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_equipment_code` (`equipment_code`),
    INDEX `idx_equipment_name` (`equipment_name`),
    INDEX `idx_equipment_type` (`equipment_type`),
    INDEX `idx_workshop_id` (`workshop_id`),
    INDEX `idx_status` (`status`),
    INDEX `idx_next_maintenance_date` (`next_maintenance_date`),
    INDEX `idx_responsible_person_id` (`responsible_person_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='设备信息表';

-- 设备维护记录表
CREATE TABLE `equipment_maintenance` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '维护记录ID',
    `equipment_id` INT NOT NULL COMMENT '设备ID',
    `equipment_code` VARCHAR(50) NOT NULL COMMENT '设备编码',
    `maintenance_type` VARCHAR(50) DEFAULT '定期维护' COMMENT '维护类型',
    `maintenance_date` DATE NOT NULL COMMENT '维护日期',
    `maintenance_person` VARCHAR(100) DEFAULT '' COMMENT '维护人员',
    `maintenance_content` TEXT COMMENT '维护内容',
    `maintenance_result` TEXT COMMENT '维护结果',
    `cost` DECIMAL(18,2) DEFAULT 0 COMMENT '维护费用',
    `next_maintenance_date` DATE DEFAULT NULL COMMENT '下次维护日期',
    `status` VARCHAR(50) DEFAULT '已完成' COMMENT '维护状态',
    `remarks` TEXT COMMENT '备注',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    INDEX `idx_equipment_id` (`equipment_id`),
    INDEX `idx_equipment_code` (`equipment_code`),
    INDEX `idx_maintenance_date` (`maintenance_date`),
    INDEX `idx_maintenance_type` (`maintenance_type`),
    INDEX `idx_status` (`status`),
    CONSTRAINT `fk_maintenance_equipment` FOREIGN KEY (`equipment_id`) REFERENCES `equipment` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='设备维护记录表';

-- =============================================
-- 质量管理相关表
-- =============================================

-- 质量检验表
CREATE TABLE `quality_inspection` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '检验记录ID',
    `inspection_number` VARCHAR(50) NOT NULL COMMENT '检验单号',
    `production_order_id` INT DEFAULT 0 COMMENT '生产订单ID',
    `production_order_number` VARCHAR(50) DEFAULT '' COMMENT '生产订单号',
    `product_code` VARCHAR(50) NOT NULL COMMENT '产品编码',
    `product_name` VARCHAR(200) DEFAULT '' COMMENT '产品名称',
    `inspection_type` INT NOT NULL COMMENT '检验类型：1-进料检验，2-过程检验，3-成品检验，4-出货检验',
    `inspection_stage` VARCHAR(100) DEFAULT '' COMMENT '检验阶段',
    `inspection_quantity` DECIMAL(18,4) NOT NULL COMMENT '检验数量',
    `sample_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '抽样数量',
    `qualified_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '合格数量',
    `unqualified_quantity` DECIMAL(18,4) DEFAULT 0 COMMENT '不合格数量',
    `inspection_result` INT DEFAULT 1 COMMENT '检验结果：1-合格，2-不合格，3-让步接收',
    `inspection_standard` VARCHAR(500) DEFAULT '' COMMENT '检验标准',
    `inspection_items` TEXT COMMENT '检验项目(JSON格式)',
    `inspection_data` TEXT COMMENT '检验数据(JSON格式)',
    `unqualified_reason` TEXT COMMENT '不合格原因',
    `treatment_measure` TEXT COMMENT '处理措施',
    `inspector_id` INT NOT NULL COMMENT '检验员ID',
    `inspector_name` VARCHAR(100) NOT NULL COMMENT '检验员姓名',
    `inspection_time` DATETIME NOT NULL COMMENT '检验时间',
    `reviewer_id` INT DEFAULT 0 COMMENT '审核员ID',
    `reviewer_name` VARCHAR(100) DEFAULT '' COMMENT '审核员姓名',
    `review_time` DATETIME DEFAULT NULL COMMENT '审核时间',
    `review_status` INT DEFAULT 1 COMMENT '审核状态：1-待审核，2-已审核，3-审核不通过',
    `review_comments` TEXT COMMENT '审核意见',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_inspection_number` (`inspection_number`),
    INDEX `idx_production_order_id` (`production_order_id`),
    INDEX `idx_product_code` (`product_code`),
    INDEX `idx_inspection_type` (`inspection_type`),
    INDEX `idx_inspection_result` (`inspection_result`),
    INDEX `idx_inspector_id` (`inspector_id`),
    INDEX `idx_inspection_time` (`inspection_time`),
    INDEX `idx_review_status` (`review_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='质量检验表';

-- 输出创建结果
SELECT 'MES系统业务表创建完成' AS result;
