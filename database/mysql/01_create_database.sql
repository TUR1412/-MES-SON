-- =============================================
-- MES制造执行系统 - MySQL数据库创建脚本
-- 版本: 1.0
-- 创建时间: 2025-06-06
-- 数据库: MySQL 8.0
-- 字符集: utf8mb4
-- 排序规则: utf8mb4_unicode_ci
-- =============================================

-- 创建数据库
DROP DATABASE IF EXISTS `mes_system`;
CREATE DATABASE `mes_system` 
    CHARACTER SET utf8mb4 
    COLLATE utf8mb4_unicode_ci
    COMMENT 'MES制造执行系统数据库';

-- 使用数据库
USE `mes_system`;

-- 设置SQL模式
SET SQL_MODE = 'STRICT_TRANS_TABLES,NO_ZERO_DATE,NO_ZERO_IN_DATE,ERROR_FOR_DIVISION_BY_ZERO';

-- 设置时区
SET time_zone = '+08:00';

-- =============================================
-- 创建用户和权限设置
-- =============================================

-- 创建MES系统专用用户
DROP USER IF EXISTS 'mes_user'@'localhost';
CREATE USER 'mes_user'@'localhost' IDENTIFIED BY 'MES@2025!';

-- 授予权限
GRANT SELECT, INSERT, UPDATE, DELETE ON mes_system.* TO 'mes_user'@'localhost';
GRANT CREATE, ALTER, INDEX ON mes_system.* TO 'mes_user'@'localhost';
GRANT EXECUTE ON mes_system.* TO 'mes_user'@'localhost';

-- 刷新权限
FLUSH PRIVILEGES;

-- =============================================
-- 系统配置表
-- =============================================

-- 系统配置表
CREATE TABLE `sys_config` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '配置ID',
    `config_key` VARCHAR(100) NOT NULL COMMENT '配置键',
    `config_value` TEXT COMMENT '配置值',
    `config_type` VARCHAR(50) DEFAULT 'STRING' COMMENT '配置类型',
    `description` VARCHAR(500) COMMENT '配置描述',
    `is_system` TINYINT DEFAULT 0 COMMENT '是否系统配置：1-是，0-否',
    `status` TINYINT DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_config_key` (`config_key`),
    INDEX `idx_config_type` (`config_type`),
    INDEX `idx_status` (`status`),
    INDEX `idx_create_time` (`create_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='系统配置表';

-- 数据字典表
CREATE TABLE `sys_dictionary` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '字典ID',
    `dict_type` VARCHAR(100) NOT NULL COMMENT '字典类型',
    `dict_type_name` VARCHAR(200) DEFAULT '' COMMENT '字典类型名称',
    `dict_code` VARCHAR(100) NOT NULL COMMENT '字典编码',
    `dict_name` VARCHAR(200) NOT NULL COMMENT '字典名称',
    `dict_value` VARCHAR(500) DEFAULT '' COMMENT '字典值',
    `parent_id` INT DEFAULT 0 COMMENT '父级ID',
    `sort_order` INT DEFAULT 0 COMMENT '排序号',
    `status` TINYINT DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `is_system` TINYINT DEFAULT 0 COMMENT '是否系统内置：1-是，0-否',
    `description` VARCHAR(500) DEFAULT '' COMMENT '描述',
    `extend_field1` VARCHAR(200) DEFAULT '' COMMENT '扩展字段1',
    `extend_field2` VARCHAR(200) DEFAULT '' COMMENT '扩展字段2',
    `extend_field3` VARCHAR(200) DEFAULT '' COMMENT '扩展字段3',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_dict_type_code` (`dict_type`, `dict_code`),
    INDEX `idx_dict_type` (`dict_type`),
    INDEX `idx_parent_id` (`parent_id`),
    INDEX `idx_sort_order` (`sort_order`),
    INDEX `idx_status` (`status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='数据字典表';

-- 系统日志表
CREATE TABLE `sys_log` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY COMMENT '日志ID',
    `log_level` VARCHAR(20) NOT NULL COMMENT '日志级别',
    `log_type` VARCHAR(50) DEFAULT 'SYSTEM' COMMENT '日志类型',
    `module_name` VARCHAR(100) DEFAULT '' COMMENT '模块名称',
    `operation_name` VARCHAR(200) DEFAULT '' COMMENT '操作名称',
    `log_message` TEXT COMMENT '日志消息',
    `exception_info` TEXT COMMENT '异常信息',
    `request_url` VARCHAR(500) DEFAULT '' COMMENT '请求URL',
    `request_method` VARCHAR(20) DEFAULT '' COMMENT '请求方法',
    `request_params` TEXT COMMENT '请求参数',
    `response_data` TEXT COMMENT '响应数据',
    `execution_time` INT DEFAULT 0 COMMENT '执行时间(毫秒)',
    `user_id` INT DEFAULT 0 COMMENT '用户ID',
    `user_name` VARCHAR(100) DEFAULT '' COMMENT '用户名',
    `ip_address` VARCHAR(50) DEFAULT '' COMMENT 'IP地址',
    `user_agent` VARCHAR(500) DEFAULT '' COMMENT '用户代理',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    INDEX `idx_log_level` (`log_level`),
    INDEX `idx_log_type` (`log_type`),
    INDEX `idx_module_name` (`module_name`),
    INDEX `idx_user_id` (`user_id`),
    INDEX `idx_create_time` (`create_time`),
    INDEX `idx_ip_address` (`ip_address`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='系统日志表';

-- =============================================
-- 用户权限相关表
-- =============================================

-- 用户表
CREATE TABLE `sys_user` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '用户ID',
    `user_code` VARCHAR(50) NOT NULL COMMENT '用户编码',
    `user_name` VARCHAR(100) NOT NULL COMMENT '用户名',
    `real_name` VARCHAR(100) DEFAULT '' COMMENT '真实姓名',
    `password` VARCHAR(255) NOT NULL COMMENT '密码(加密)',
    `salt` VARCHAR(50) DEFAULT '' COMMENT '密码盐值',
    `email` VARCHAR(200) DEFAULT '' COMMENT '邮箱',
    `phone` VARCHAR(20) DEFAULT '' COMMENT '手机号',
    `avatar` VARCHAR(500) DEFAULT '' COMMENT '头像路径',
    `department` VARCHAR(100) DEFAULT '' COMMENT '部门',
    `position` VARCHAR(100) DEFAULT '' COMMENT '职位',
    `status` TINYINT DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `last_login_time` DATETIME DEFAULT NULL COMMENT '最后登录时间',
    `last_login_ip` VARCHAR(50) DEFAULT '' COMMENT '最后登录IP',
    `login_count` INT DEFAULT 0 COMMENT '登录次数',
    `password_update_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '密码更新时间',
    `account_expire_time` DATETIME DEFAULT NULL COMMENT '账户过期时间',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_user_code` (`user_code`),
    UNIQUE KEY `uk_email` (`email`),
    INDEX `idx_user_name` (`user_name`),
    INDEX `idx_real_name` (`real_name`),
    INDEX `idx_department` (`department`),
    INDEX `idx_status` (`status`),
    INDEX `idx_create_time` (`create_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='用户表';

-- 角色表
CREATE TABLE `sys_role` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '角色ID',
    `role_code` VARCHAR(50) NOT NULL COMMENT '角色编码',
    `role_name` VARCHAR(100) NOT NULL COMMENT '角色名称',
    `description` VARCHAR(500) DEFAULT '' COMMENT '角色描述',
    `status` TINYINT DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `permissions` TEXT COMMENT '权限列表(JSON格式)',
    `sort_order` INT DEFAULT 0 COMMENT '排序号',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_role_code` (`role_code`),
    INDEX `idx_role_name` (`role_name`),
    INDEX `idx_status` (`status`),
    INDEX `idx_sort_order` (`sort_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='角色表';

-- 用户角色关联表
CREATE TABLE `sys_user_role` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '关联ID',
    `user_id` INT NOT NULL COMMENT '用户ID',
    `role_id` INT NOT NULL COMMENT '角色ID',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    UNIQUE KEY `uk_user_role` (`user_id`, `role_id`),
    INDEX `idx_user_id` (`user_id`),
    INDEX `idx_role_id` (`role_id`),
    CONSTRAINT `fk_user_role_user` FOREIGN KEY (`user_id`) REFERENCES `sys_user` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_user_role_role` FOREIGN KEY (`role_id`) REFERENCES `sys_role` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='用户角色关联表';

-- 权限表
CREATE TABLE `sys_permission` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '权限ID',
    `permission_code` VARCHAR(100) NOT NULL COMMENT '权限编码',
    `permission_name` VARCHAR(200) NOT NULL COMMENT '权限名称',
    `permission_type` VARCHAR(50) DEFAULT 'MENU' COMMENT '权限类型：MENU-菜单，BUTTON-按钮，API-接口',
    `parent_id` INT DEFAULT 0 COMMENT '父级权限ID',
    `module_name` VARCHAR(100) DEFAULT '' COMMENT '模块名称',
    `resource_url` VARCHAR(500) DEFAULT '' COMMENT '资源URL',
    `icon` VARCHAR(100) DEFAULT '' COMMENT '图标',
    `sort_order` INT DEFAULT 0 COMMENT '排序号',
    `status` TINYINT DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
    `description` VARCHAR(500) DEFAULT '' COMMENT '权限描述',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `create_user_id` INT DEFAULT 0 COMMENT '创建用户ID',
    `create_user_name` VARCHAR(100) DEFAULT '' COMMENT '创建用户名',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    `update_user_id` INT DEFAULT 0 COMMENT '更新用户ID',
    `update_user_name` VARCHAR(100) DEFAULT '' COMMENT '更新用户名',
    `is_deleted` TINYINT DEFAULT 0 COMMENT '是否删除：1-是，0-否',
    UNIQUE KEY `uk_permission_code` (`permission_code`),
    INDEX `idx_permission_type` (`permission_type`),
    INDEX `idx_parent_id` (`parent_id`),
    INDEX `idx_module_name` (`module_name`),
    INDEX `idx_sort_order` (`sort_order`),
    INDEX `idx_status` (`status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='权限表';

-- 输出创建结果
SELECT 'MES系统数据库和基础表创建完成' AS result;
