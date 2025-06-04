-- MES制造执行系统数据库初始化脚本
-- 创建时间: 2025-06-04 10:54:32
-- 数据库版本: MySQL 8.0
-- 字符集: utf8mb4

-- 创建数据库
CREATE DATABASE IF NOT EXISTS `MES_DB` 
DEFAULT CHARACTER SET utf8mb4 
DEFAULT COLLATE utf8mb4_unicode_ci;
 
USE `MES_DB`;

-- ========================================
-- 系统管理模块表结构
-- ========================================

-- 用户信息表
CREATE TABLE `sys_user` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `user_code` varchar(50) NOT NULL COMMENT '用户编码',
  `user_name` varchar(100) NOT NULL COMMENT '用户姓名',
  `login_name` varchar(50) NOT NULL COMMENT '登录名',
  `password` varchar(255) NOT NULL COMMENT '密码（加密）',
  `email` varchar(100) DEFAULT NULL COMMENT '邮箱',
  `phone` varchar(20) DEFAULT NULL COMMENT '电话',
  `department` varchar(100) DEFAULT NULL COMMENT '部门',
  `position` varchar(100) DEFAULT NULL COMMENT '职位',
  `status` tinyint NOT NULL DEFAULT '1' COMMENT '状态：1-启用，0-禁用',
  `last_login_time` datetime DEFAULT NULL COMMENT '最后登录时间',
  `login_count` int DEFAULT '0' COMMENT '登录次数',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `update_user_id` int DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) DEFAULT NULL COMMENT '更新人姓名',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '是否删除：1-已删除，0-未删除',
  `delete_time` datetime DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) DEFAULT NULL COMMENT '删除人姓名',
  `remark` text COMMENT '备注',
  `version` int NOT NULL DEFAULT '1' COMMENT '版本号',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_user_code` (`user_code`),
  UNIQUE KEY `uk_login_name` (`login_name`),
  KEY `idx_status` (`status`),
  KEY `idx_is_deleted` (`is_deleted`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='用户信息表';

-- 角色信息表
CREATE TABLE `sys_role` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '角色ID',
  `role_code` varchar(50) NOT NULL COMMENT '角色编码',
  `role_name` varchar(100) NOT NULL COMMENT '角色名称',
  `description` text COMMENT '角色描述',
  `status` tinyint NOT NULL DEFAULT '1' COMMENT '状态：1-启用，0-禁用',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `update_user_id` int DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) DEFAULT NULL COMMENT '更新人姓名',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '是否删除：1-已删除，0-未删除',
  `delete_time` datetime DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) DEFAULT NULL COMMENT '删除人姓名',
  `remark` text COMMENT '备注',
  `version` int NOT NULL DEFAULT '1' COMMENT '版本号',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_role_code` (`role_code`),
  KEY `idx_status` (`status`),
  KEY `idx_is_deleted` (`is_deleted`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='角色信息表';

-- ========================================
-- 物料管理模块表结构 (L成员负责)
-- ========================================

-- 物料信息表
CREATE TABLE `material_info` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '物料ID',
  `material_code` varchar(50) NOT NULL COMMENT '物料编码',
  `material_name` varchar(200) NOT NULL COMMENT '物料名称',
  `material_type` varchar(50) NOT NULL COMMENT '物料类型：原材料、半成品、成品等',
  `specification` varchar(200) DEFAULT NULL COMMENT '规格型号',
  `unit` varchar(20) NOT NULL COMMENT '计量单位',
  `category` varchar(100) DEFAULT NULL COMMENT '物料分类',
  `supplier` varchar(200) DEFAULT NULL COMMENT '供应商',
  `standard_cost` decimal(10,4) DEFAULT NULL COMMENT '标准成本',
  `safety_stock` decimal(10,2) DEFAULT NULL COMMENT '安全库存',
  `min_stock` decimal(10,2) DEFAULT NULL COMMENT '最小库存',
  `max_stock` decimal(10,2) DEFAULT NULL COMMENT '最大库存',
  `lead_time` int DEFAULT NULL COMMENT '采购提前期（天）',
  `status` tinyint NOT NULL DEFAULT '1' COMMENT '状态：1-启用，0-禁用',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `update_user_id` int DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) DEFAULT NULL COMMENT '更新人姓名',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '是否删除：1-已删除，0-未删除',
  `delete_time` datetime DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) DEFAULT NULL COMMENT '删除人姓名',
  `remark` text COMMENT '备注',
  `version` int NOT NULL DEFAULT '1' COMMENT '版本号',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_material_code` (`material_code`),
  KEY `idx_material_type` (`material_type`),
  KEY `idx_category` (`category`),
  KEY `idx_status` (`status`),
  KEY `idx_is_deleted` (`is_deleted`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='物料信息表';

-- BOM物料清单表
CREATE TABLE `bom_info` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'BOM ID',
  `bom_code` varchar(50) NOT NULL COMMENT 'BOM编码',
  `product_id` int NOT NULL COMMENT '产品物料ID',
  `bom_version` varchar(20) NOT NULL DEFAULT '1.0' COMMENT 'BOM版本',
  `bom_type` varchar(20) NOT NULL DEFAULT 'PRODUCTION' COMMENT 'BOM类型：PRODUCTION-生产，ENGINEERING-工程',
  `effective_date` date NOT NULL COMMENT '生效日期',
  `expire_date` date DEFAULT NULL COMMENT '失效日期',
  `status` tinyint NOT NULL DEFAULT '1' COMMENT '状态：1-启用，0-禁用',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `update_user_id` int DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) DEFAULT NULL COMMENT '更新人姓名',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '是否删除：1-已删除，0-未删除',
  `delete_time` datetime DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) DEFAULT NULL COMMENT '删除人姓名',
  `remark` text COMMENT '备注',
  `version` int NOT NULL DEFAULT '1' COMMENT '版本号',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_bom_code` (`bom_code`),
  KEY `idx_product_id` (`product_id`),
  KEY `idx_status` (`status`),
  KEY `idx_is_deleted` (`is_deleted`),
  CONSTRAINT `fk_bom_product` FOREIGN KEY (`product_id`) REFERENCES `material_info` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='BOM物料清单表';


-- ========================================
-- 车间管理模块表结构 (S成员负责)
-- ========================================

-- 车间信息表
CREATE TABLE `workshop` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '车间ID',
  `workshop_code` varchar(50) NOT NULL COMMENT '车间编码',
  `workshop_name` varchar(100) NOT NULL COMMENT '车间名称',
  `manager_id` int DEFAULT NULL COMMENT '负责人ID',
  `capacity` int DEFAULT NULL COMMENT '生产能力(件/天)',
  `status` tinyint NOT NULL DEFAULT '1' COMMENT '状态：1-启用，0-禁用',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `update_user_id` int DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) DEFAULT NULL COMMENT '更新人姓名',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '是否删除：1-已删除，0-未删除',
  `delete_time` datetime DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) DEFAULT NULL COMMENT '删除人姓名',
  `remark` text COMMENT '备注',
  `version` int NOT NULL DEFAULT '1' COMMENT '版本号',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_workshop_code` (`workshop_code`),
  KEY `idx_status` (`status`),
  KEY `idx_is_deleted` (`is_deleted`),
  CONSTRAINT `fk_workshop_manager` FOREIGN KEY (`manager_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_workshop_create_user` FOREIGN KEY (`create_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_workshop_update_user` FOREIGN KEY (`update_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_workshop_delete_user` FOREIGN KEY (`delete_user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='车间信息表';

-- 在制品(WIP)表
CREATE TABLE `wip` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '在制品ID',
  `order_id` int NOT NULL COMMENT '关联订单ID',
-- wip 表中的 order_id 字段保留，但未添加外键约束，等待H成员完成生产订单表（production_order）后再补充。
  `material_id` int NOT NULL COMMENT '物料ID',
  `current_stage` varchar(50) DEFAULT NULL COMMENT '当前生产阶段',
  `quantity` int NOT NULL COMMENT '数量',
  `workshop_id` int NOT NULL COMMENT '所在车间ID',
  `status` tinyint NOT NULL DEFAULT '1' COMMENT '状态：1-在制，2-等待，3-暂停，4-已完成',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `update_user_id` int DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) DEFAULT NULL COMMENT '更新人姓名',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '是否删除：1-已删除，0-未删除',
  `delete_time` datetime DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) DEFAULT NULL COMMENT '删除人姓名',
  `remark` text COMMENT '备注',
  `version` int NOT NULL DEFAULT '1' COMMENT '版本号',
  PRIMARY KEY (`id`),
  KEY `idx_order_id` (`order_id`),
  KEY `idx_material_id` (`material_id`),
  KEY `idx_workshop_id` (`workshop_id`),
  KEY `idx_status` (`status`),
  KEY `idx_is_deleted` (`is_deleted`),
  CONSTRAINT `fk_wip_create_user` FOREIGN KEY (`create_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_wip_update_user` FOREIGN KEY (`update_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_wip_delete_user` FOREIGN KEY (`delete_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_wip_material` FOREIGN KEY (`material_id`) REFERENCES `material_info` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_wip_workshop` FOREIGN KEY (`workshop_id`) REFERENCES `workshop` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='在制品管理表';

-- 设备信息表
CREATE TABLE `equipment` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '设备ID',
  `equipment_code` varchar(50) NOT NULL COMMENT '设备编码',
  `equipment_name` varchar(100) NOT NULL COMMENT '设备名称',
  `equipment_type` varchar(50) DEFAULT NULL COMMENT '设备类型',
  `workshop_id` int DEFAULT NULL COMMENT '所属车间ID',
  `status` tinyint NOT NULL DEFAULT '1' COMMENT '状态：1-正常，2-维护中，3-故障，4-停用',
  `last_maintenance_date` date DEFAULT NULL COMMENT '最后维护日期',
  `next_maintenance_date` date DEFAULT NULL COMMENT '下次维护日期',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `update_user_id` int DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) DEFAULT NULL COMMENT '更新人姓名',
  `is_deleted` tinyint NOT NULL DEFAULT '0' COMMENT '是否删除：1-已删除，0-未删除',
  `delete_time` datetime DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) DEFAULT NULL COMMENT '删除人姓名',
  `remark` text COMMENT '备注',
  `version` int NOT NULL DEFAULT '1' COMMENT '版本号',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_equipment_code` (`equipment_code`),
  KEY `idx_workshop_id` (`workshop_id`),
  KEY `idx_status` (`status`),
  KEY `idx_is_deleted` (`is_deleted`),
  CONSTRAINT `fk_equipment_workshop` FOREIGN KEY (`workshop_id`) REFERENCES `workshop` (`id`),
  CONSTRAINT `fk_equipment_create_user` FOREIGN KEY (`create_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_equipment_update_user` FOREIGN KEY (`update_user_id`) REFERENCES `sys_user` (`id`),
  CONSTRAINT `fk_equipment_delete_user` FOREIGN KEY (`delete_user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='设备信息表';

-- 设备状态历史表
CREATE TABLE `equipment_status_history` (
  `id` int NOT NULL AUTO_INCREMENT,
  `equipment_id` int NOT NULL,
  `old_status` tinyint NOT NULL,
  `new_status` tinyint NOT NULL,
  `change_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `change_user_id` int NOT NULL,
  `remark` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_equipment_id` (`equipment_id`),
  CONSTRAINT `fk_equipment_status_history_equipment` FOREIGN KEY (`equipment_id`) REFERENCES `equipment` (`id`),
  CONSTRAINT `fk_equipment_status_history_user` FOREIGN KEY (`change_user_id`) REFERENCES `sys_user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='设备状态变更历史';



-- 插入初始数据
INSERT INTO `sys_user` (`user_code`, `user_name`, `login_name`, `password`, `department`, `position`, `create_user_name`) 
VALUES 
('ADMIN', '系统管理员', 'admin', 'e10adc3949ba59abbe56e057f20f883e', '信息技术部', '系统管理员', '系统初始化'),
('USER001', '测试用户', 'test', 'e10adc3949ba59abbe56e057f20f883e', '生产部', '操作员', '系统初始化');

INSERT INTO `sys_role` (`role_code`, `role_name`, `description`, `create_user_name`) 
VALUES 
('ADMIN', '系统管理员', '拥有系统所有权限', '系统初始化'),
('OPERATOR', '操作员', '基本操作权限', '系统初始化'),
('MANAGER', '管理员', '管理权限', '系统初始化');

-- 提交事务
COMMIT;
