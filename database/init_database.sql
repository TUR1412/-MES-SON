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
