/*
 Navicat Premium Dump SQL

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 80405 (8.4.5)
 Source Host           : localhost:3306
 Source Schema         : mes_db

 Target Server Type    : MySQL
 Target Server Version : 80405 (8.4.5)
 File Encoding         : 65001

 Date: 12/06/2025 00:21:17
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for batch_info
-- ----------------------------
DROP TABLE IF EXISTS `batch_info`;
CREATE TABLE `batch_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '批次ID',
  `batch_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '批次号',
  `workshop_id` int NOT NULL COMMENT '车间ID',
  `workshop_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '车间名称',
  `production_order_id` int NULL DEFAULT 0 COMMENT '生产订单ID',
  `production_order_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '生产订单号',
  `product_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '产品编码',
  `product_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '产品名称',
  `planned_quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '计划数量',
  `actual_quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '实际数量',
  `unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '个' COMMENT '单位',
  `batch_status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '待开始' COMMENT '批次状态',
  `start_time` datetime NULL DEFAULT NULL COMMENT '开始时间',
  `end_time` datetime NULL DEFAULT NULL COMMENT '结束时间',
  `operator_id` int NULL DEFAULT 0 COMMENT '操作员ID',
  `operator_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '操作员姓名',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int NULL DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  `update_user_id` int NULL DEFAULT NULL COMMENT '最后修改人ID',
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '最后修改人姓名',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  `delete_time` datetime NULL DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int NULL DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '删除人姓名',
  `remark` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `version` int NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_batch_number`(`batch_number` ASC) USING BTREE,
  INDEX `idx_workshop_id`(`workshop_id` ASC) USING BTREE,
  INDEX `idx_production_order_id`(`production_order_id` ASC) USING BTREE,
  INDEX `idx_batch_status`(`batch_status` ASC) USING BTREE,
  INDEX `idx_operator_id`(`operator_id` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '批次信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of batch_info
-- ----------------------------
INSERT INTO `batch_info` VALUES (1, 'B202506090001', 1, '生产车间A', 1, 'PO202506090001', 'PRD001', '产品A', 100.0000, 0.0000, '台', '待开始', NULL, NULL, 0, '操作员A', '2025-06-10 16:38:56', NULL, '生产调度员', '2025-06-10 20:25:54', NULL, '生产调度员', 0, NULL, NULL, NULL, '首批试产批次，质量要求严格', 1);
INSERT INTO `batch_info` VALUES (2, 'B202506090002', 1, '生产车间A', 2, 'PO202506090002', 'PRD002', '产品B', 50.0000, 10.0000, '台', '进行中', NULL, NULL, 0, '操作员B', '2025-06-10 16:38:56', NULL, '生产调度员', '2025-06-10 20:25:54', NULL, '生产调度员', 0, NULL, NULL, NULL, '正常生产批次，按计划进行', 1);
INSERT INTO `batch_info` VALUES (3, 'B202506090003', 2, '装配车间B', 3, 'PO202506090003', 'PRD001', '产品A', 80.0000, 80.0000, '台', '已完成', NULL, NULL, 0, '操作员C', '2025-06-10 16:38:56', NULL, '生产调度员', '2025-06-10 20:25:54', NULL, '生产调度员', 0, NULL, NULL, NULL, '已完成批次，质量合格', 1);
INSERT INTO `batch_info` VALUES (4, 'B202506100004', 2, '装配车间', 4, 'PO202506100004', 'P004', '智能手表', 100.0000, 0.0000, '块', '待开始', NULL, NULL, 104, NULL, '2025-06-10 20:25:40', NULL, '生产调度员', '2025-06-10 20:25:54', NULL, '生产调度员', 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `batch_info` VALUES (5, 'B202506100005', 1, '加工车间', 5, 'PO202506100005', 'P005', '平板电脑', 75.0000, 25.0000, '台', '进行中', '2025-06-10 09:00:00', NULL, 105, NULL, '2025-06-10 20:25:40', NULL, '生产调度员', '2025-06-10 20:25:54', NULL, '生产调度员', 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `batch_info` VALUES (6, 'B202506100006', 3, '包装车间', 6, 'PO202506100006', 'P006', '智能音箱', 150.0000, 150.0000, '台', '已完成', '2025-06-01 08:30:00', '2025-06-04 17:30:00', 106, NULL, '2025-06-10 20:25:40', NULL, '生产调度员', '2025-06-10 20:25:54', NULL, '生产调度员', 0, NULL, NULL, NULL, NULL, 1);

-- ----------------------------
-- Table structure for bom_info
-- ----------------------------
DROP TABLE IF EXISTS `bom_info`;
CREATE TABLE `bom_info`  (
  `id` int NOT NULL AUTO_INCREMENT,
  `bom_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `bom_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `product_id` int NULL DEFAULT NULL,
  `product_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `product_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '1.0',
  `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '有效',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `create_user_id` int NULL DEFAULT NULL,
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `update_time` datetime NULL DEFAULT NULL,
  `update_user_id` int NULL DEFAULT NULL,
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NULL DEFAULT 0,
  `delete_time` datetime NULL DEFAULT NULL,
  `delete_user_id` int NULL DEFAULT NULL,
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `bom_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '标准BOM',
  `effective_date` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `expire_date` datetime NULL DEFAULT NULL,
  `material_id` int NULL DEFAULT NULL,
  `material_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `material_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `quantity` decimal(18, 4) NULL DEFAULT 1.0000,
  `unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '个',
  `loss_rate` decimal(5, 2) NULL DEFAULT 0.00,
  `substitute_material` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `bom_code`(`bom_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 18 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of bom_info
-- ----------------------------
INSERT INTO `bom_info` VALUES (7, 'BOM001-001', '智能手机BOM-主板', 1, 'P001', '智能手机', '1.0', '有效', '智能手机主板组件', '2025-06-10 20:40:13', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:13', '2026-06-10 20:40:13', 1, 'M001', '主板', 1.0000, '片', 2.00, NULL);
INSERT INTO `bom_info` VALUES (8, 'BOM001-002', '智能手机BOM-屏幕', 1, 'P001', '智能手机', '1.0', '有效', '智能手机屏幕组件', '2025-06-10 20:40:13', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:13', '2026-06-10 20:40:13', 2, 'M002', '6.1寸OLED屏幕', 1.0000, '片', 1.00, NULL);
INSERT INTO `bom_info` VALUES (9, 'BOM001-003', '智能手机BOM-电池', 1, 'P001', '智能手机', '1.0', '有效', '智能手机电池组件', '2025-06-10 20:40:13', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:13', '2026-06-10 20:40:13', 3, 'M003', '锂电池4000mAh', 1.0000, '个', 0.50, NULL);
INSERT INTO `bom_info` VALUES (10, 'BOM001-004', '智能手机BOM-外壳', 1, 'P001', '智能手机', '1.0', '有效', '智能手机外壳组件', '2025-06-10 20:40:13', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:13', '2026-06-10 20:40:13', 4, 'M004', '铝合金外壳', 1.0000, '套', 3.00, NULL);
INSERT INTO `bom_info` VALUES (11, 'BOM002-001', '笔记本电脑BOM-主板', 2, 'P002', '笔记本电脑', '1.0', '有效', '笔记本电脑主板组件', '2025-06-10 20:40:28', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:28', '2026-06-10 20:40:28', 1, 'M001', '主板', 1.0000, '片', 2.00, NULL);
INSERT INTO `bom_info` VALUES (12, 'BOM002-002', '笔记本电脑BOM-屏幕', 2, 'P002', '笔记本电脑', '1.0', '有效', '笔记本电脑屏幕组件', '2025-06-10 20:40:28', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:28', '2026-06-10 20:40:28', 7, 'M007', '10.1寸LCD屏幕', 1.0000, '片', 1.00, NULL);
INSERT INTO `bom_info` VALUES (13, 'BOM002-003', '笔记本电脑BOM-内存', 2, 'P002', '笔记本电脑', '1.0', '有效', '笔记本电脑内存组件', '2025-06-10 20:40:28', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:28', '2026-06-10 20:40:28', 5, 'M005', '8GB内存条', 1.0000, '条', 0.50, NULL);
INSERT INTO `bom_info` VALUES (14, 'BOM002-004', '笔记本电脑BOM-键盘', 2, 'P002', '笔记本电脑', '1.0', '有效', '笔记本电脑键盘组件', '2025-06-10 20:40:28', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:28', '2026-06-10 20:40:28', 4, 'M004', '铝合金外壳', 1.0000, '套', 1.50, NULL);
INSERT INTO `bom_info` VALUES (15, 'BOM003-001', '无线耳机BOM-驱动单元', 3, 'P003', '无线耳机', '1.0', '有效', '无线耳机驱动单元', '2025-06-10 20:40:44', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:44', '2026-06-10 20:40:44', 9, 'M009', '扬声器单元', 2.0000, '个', 1.00, NULL);
INSERT INTO `bom_info` VALUES (16, 'BOM003-002', '无线耳机BOM-电池', 3, 'P003', '无线耳机', '1.0', '有效', '无线耳机电池组件', '2025-06-10 20:40:44', NULL, '系统管理员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:44', '2026-06-10 20:40:44', 6, 'M006', '锂电池3000mAh', 1.0000, '个', 0.50, NULL);
INSERT INTO `bom_info` VALUES (17, 'BOM003-003', '无线耳机BOM-蓝牙模块', 3, 'P003', '无线耳机', '1.0', '有效', '无线耳机蓝牙模块', '2025-06-10 20:40:44', NULL, '系统管理员', '2025-06-11 10:52:42', NULL, NULL, 1, NULL, NULL, NULL, '标准BOM', '2025-06-10 20:40:44', '2026-06-10 20:40:44', 8, 'M008', '蓝牙5.0模块', 1.0000, '个', 1.00, NULL);

-- ----------------------------
-- Table structure for equipment_fault_log
-- ----------------------------
DROP TABLE IF EXISTS `equipment_fault_log`;
CREATE TABLE `equipment_fault_log`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '故障日志ID',
  `equipment_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '设备编码',
  `fault_description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '故障描述',
  `fault_time` datetime NOT NULL COMMENT '故障时间',
  `operator` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '操作员',
  `repair_description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '维修描述',
  `repair_time` datetime NULL DEFAULT NULL COMMENT '维修时间',
  `repair_person` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '维修人员',
  `status` int NULL DEFAULT 0 COMMENT '故障状态 (0:未处理, 1:处理中, 2:已修复)',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_equipment_code`(`equipment_code` ASC) USING BTREE,
  INDEX `idx_fault_time`(`fault_time` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '设备故障日志表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of equipment_fault_log
-- ----------------------------

-- ----------------------------
-- Table structure for equipment_info
-- ----------------------------
DROP TABLE IF EXISTS `equipment_info`;
CREATE TABLE `equipment_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '设备ID',
  `equipment_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '设备编码（唯一标识）',
  `equipment_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '设备名称',
  `equipment_type` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '设备类型名称',
  `equipment_type_id` int NOT NULL COMMENT '设备类型ID (1:加工设备, 2:装配设备, 3:检测设备, 4:包装设备, 5:运输设备)',
  `workshop_id` int NOT NULL COMMENT '所属车间ID',
  `location` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '设备位置',
  `status` int NULL DEFAULT 0 COMMENT '设备状态 (0:停止, 1:运行, 2:故障, 3:维护)',
  `efficiency` decimal(5, 2) NULL DEFAULT 0.00 COMMENT '运行效率百分比',
  `temperature` decimal(6, 2) NULL DEFAULT 0.00 COMMENT '温度(°C)',
  `pressure` decimal(6, 3) NULL DEFAULT 0.000 COMMENT '压力(MPa)',
  `speed` decimal(8, 2) NULL DEFAULT 0.00 COMMENT '转速(rpm)',
  `power` decimal(8, 2) NULL DEFAULT 0.00 COMMENT '功率(kW)',
  `vibration` decimal(6, 3) NULL DEFAULT 0.000 COMMENT '振动值',
  `last_maintenance` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '上次维护时间',
  `next_maintenance` datetime NOT NULL COMMENT '下次维护时间',
  `maintenance_cycle` int NULL DEFAULT 30 COMMENT '维护周期(天)',
  `operator` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '当前操作员',
  `operator_id` int NULL DEFAULT NULL COMMENT '操作员ID',
  `manufacturer` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '设备制造商',
  `model` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '设备型号',
  `purchase_date` date NULL DEFAULT NULL COMMENT '购买日期',
  `warranty_until` date NULL DEFAULT NULL COMMENT '保修期至',
  `value` decimal(18, 4) NULL DEFAULT NULL COMMENT '设备价值',
  `is_enabled` tinyint(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
  `remarks` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `create_by` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人',
  `update_by` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '更新人',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_equipment_code`(`equipment_code` ASC) USING BTREE,
  INDEX `idx_equipment_name`(`equipment_name` ASC) USING BTREE,
  INDEX `idx_equipment_type_id`(`equipment_type_id` ASC) USING BTREE,
  INDEX `idx_workshop_id`(`workshop_id` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_efficiency`(`efficiency` ASC) USING BTREE,
  INDEX `idx_next_maintenance`(`next_maintenance` ASC) USING BTREE,
  INDEX `idx_operator`(`operator` ASC) USING BTREE,
  INDEX `idx_is_enabled`(`is_enabled` ASC) USING BTREE,
  INDEX `idx_is_deleted`(`is_deleted` ASC) USING BTREE,
  INDEX `idx_equipment_workshop_status`(`workshop_id` ASC, `status` ASC, `is_deleted` ASC) USING BTREE,
  INDEX `idx_equipment_type_status`(`equipment_type_id` ASC, `status` ASC, `is_enabled` ASC) USING BTREE,
  INDEX `idx_equipment_maintenance`(`next_maintenance` ASC, `status` ASC) USING BTREE,
  INDEX `idx_equipment_efficiency`(`efficiency` ASC, `status` ASC) USING BTREE,
  CONSTRAINT `equipment_info_ibfk_1` FOREIGN KEY (`workshop_id`) REFERENCES `workshop_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '设备信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of equipment_info
-- ----------------------------
INSERT INTO `equipment_info` VALUES (1, 'EQ0001', 'CNC加工中心01', '加工设备', 1, 1, 'A区-01号位', 1, 85.50, 45.20, 2.350, 1800.00, 15.50, 0.025, '2025-05-15 08:00:00', '2025-06-14 08:00:00', 30, '张师傅', NULL, NULL, 'SINUMERIK 840D', '2023-01-15', '2026-01-15', 850000.0000, 1, '运行正常', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);
INSERT INTO `equipment_info` VALUES (2, 'EQ0002', 'CNC加工中心02', '加工设备', 1, 1, 'A区-02号位', 0, 0.00, 25.00, 0.000, 0.00, 0.00, 0.000, '2025-05-20 08:00:00', '2025-06-19 08:00:00', 30, '李师傅', NULL, NULL, 'SINUMERIK 828D', '2023-01-20', '2026-01-20', 850000.0000, 1, '停机待料', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);
INSERT INTO `equipment_info` VALUES (3, 'EQ0003', '装配线01', '装配设备', 2, 2, 'B区-装配线', 1, 92.30, 28.50, 0.500, 120.00, 8.20, 0.015, '2025-05-10 08:00:00', '2025-06-09 08:00:00', 30, '王师傅', NULL, NULL, 'FANUC R-2000iC', '2023-03-10', '2026-03-10', 450000.0000, 1, '高效运行', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);
INSERT INTO `equipment_info` VALUES (4, 'EQ0004', '装配线02', '装配设备', 2, 2, 'B区-装配线', 3, 0.00, 25.00, 0.000, 0.00, 0.00, 0.000, '2025-06-01 08:00:00', '2025-07-01 08:00:00', 30, '赵师傅', NULL, NULL, 'FANUC M-710iC', '2023-03-15', '2026-03-15', 450000.0000, 1, '计划维护中', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);
INSERT INTO `equipment_info` VALUES (5, 'EQ0005', '质检设备01', '检测设备', 3, 3, 'C区-质检室', 1, 88.70, 22.80, 0.100, 0.00, 2.10, 0.005, '2025-05-25 08:00:00', '2025-06-24 08:00:00', 30, '刘师傅', NULL, NULL, 'ABB IRB 6700', '2023-02-01', '2026-02-01', 320000.0000, 1, '精度良好', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);
INSERT INTO `equipment_info` VALUES (6, 'EQ0006', '包装机01', '包装设备', 4, 4, 'D区-包装线', 2, 65.20, 35.50, 1.200, 450.00, 5.80, 0.035, '2025-05-18 08:00:00', '2025-06-17 08:00:00', 30, '陈师傅', NULL, NULL, 'ABB FlexPicker', '2023-04-01', '2026-04-01', 280000.0000, 1, '故障待修', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);
INSERT INTO `equipment_info` VALUES (7, 'EQ0007', '输送带01', '运输设备', 5, 1, 'A区-输送线', 1, 95.80, 30.20, 0.800, 60.00, 3.20, 0.010, '2025-05-12 08:00:00', '2025-06-11 08:00:00', 30, '杨师傅', NULL, NULL, 'SANY STC250', '2023-05-01', '2026-05-01', 120000.0000, 1, '运行稳定', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);
INSERT INTO `equipment_info` VALUES (8, 'EQ0008', '输送带02', '运输设备', 5, 2, 'B区-输送线', 1, 90.40, 29.80, 0.750, 65.00, 3.50, 0.012, '2025-05-14 08:00:00', '2025-06-13 08:00:00', 30, '周师傅', NULL, NULL, 'SANY STC300', '2023-05-05', '2026-05-05', 120000.0000, 1, '运行正常', '2025-06-10 16:38:56', '2025-06-10 20:26:11', '系统', '系统', 0);

-- ----------------------------
-- Table structure for material_info
-- ----------------------------
DROP TABLE IF EXISTS `material_info`;
CREATE TABLE `material_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '物料ID',
  `material_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '物料编码',
  `material_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '物料名称',
  `material_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '物料类型',
  `category` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '物料分类',
  `specification` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '规格型号',
  `unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '计量单位',
  `standard_cost` decimal(18, 4) NULL DEFAULT NULL COMMENT '标准成本',
  `safety_stock` decimal(18, 2) NULL DEFAULT NULL COMMENT '安全库存',
  `min_stock` decimal(18, 2) NULL DEFAULT NULL COMMENT '最小库存',
  `max_stock` decimal(18, 2) NULL DEFAULT NULL COMMENT '最大库存',
  `stock_quantity` decimal(18, 2) NULL DEFAULT 0.00 COMMENT '当前库存数量',
  `supplier` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '供应商',
  `lead_time` int NULL DEFAULT NULL COMMENT '采购提前期（天）',
  `price` decimal(18, 4) NULL DEFAULT NULL COMMENT '价格',
  `status` tinyint(1) NOT NULL DEFAULT 1 COMMENT '状态：1-启用，0-禁用',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int NULL DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  `update_user_id` int NULL DEFAULT NULL COMMENT '最后修改人ID',
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '最后修改人姓名',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  `delete_time` datetime NULL DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int NULL DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '删除人姓名',
  `remark` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `version` int NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
  `warehouse_location` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_material_code`(`material_code` ASC) USING BTREE,
  INDEX `idx_material_type`(`material_type` ASC) USING BTREE,
  INDEX `idx_category`(`category` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_is_deleted`(`is_deleted` ASC) USING BTREE,
  INDEX `idx_material_type_status`(`material_type` ASC, `status` ASC, `is_deleted` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 16 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '物料信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of material_info
-- ----------------------------
INSERT INTO `material_info` VALUES (1, 'MAT001', '钢板A型', '原材料', NULL, '1000*2000*5mm', '张1', NULL, NULL, NULL, NULL, NULL, '', NULL, 0.0000, 1, '2025-06-10 16:38:56', NULL, '采购员', '2025-06-11 23:21:54', NULL, NULL, 0, NULL, NULL, NULL, NULL, 1, NULL);
INSERT INTO `material_info` VALUES (2, 'MAT002', '螺栓M8', '标准件', '紧固件', 'M8*25', '个', 0.5000, NULL, 500.00, 5000.00, NULL, NULL, 3, NULL, 1, '2025-06-10 16:38:56', NULL, '采购员', '2025-06-10 20:28:33', NULL, '采购员', 0, NULL, NULL, NULL, NULL, 1, NULL);
INSERT INTO `material_info` VALUES (3, 'MAT003', '电机220V', '电气元件', '动力设备', '220V/1.5KW', '台', 280.0000, NULL, 5.00, 50.00, NULL, NULL, 14, NULL, 1, '2025-06-10 16:38:56', NULL, '采购员', '2025-06-10 20:28:33', NULL, '采购员', 0, NULL, NULL, NULL, NULL, 1, NULL);
INSERT INTO `material_info` VALUES (4, 'PRD001', '产品A', '成品', '机械产品', 'A型机械设备', '台', 1500.0000, NULL, 2.00, 20.00, NULL, NULL, 0, NULL, 1, '2025-06-10 16:38:56', NULL, '采购员', '2025-06-10 20:28:33', NULL, '采购员', 0, NULL, NULL, NULL, NULL, 1, NULL);
INSERT INTO `material_info` VALUES (5, 'PRD002', '产品B', '成品', '机械产品', 'B型机械设备', '台', 2200.0000, NULL, 1.00, 15.00, NULL, NULL, 0, NULL, 1, '2025-06-10 16:38:56', NULL, '采购员', '2025-06-10 20:28:33', NULL, '采购员', 0, NULL, NULL, NULL, NULL, 1, NULL);
INSERT INTO `material_info` VALUES (6, 'M006', '锂电池3000mAh', '电子元件', NULL, '3.7V 3000mAh 锂聚合物电池', '个', NULL, 100.00, NULL, NULL, 800.00, '比亚迪电池', NULL, 25.5000, 1, '2025-06-10 20:28:51', NULL, '采购员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '智能手表专用电池', 1, 'A区-03-05');
INSERT INTO `material_info` VALUES (7, 'M007', '10.1寸LCD屏幕', '电子元件', NULL, '1920x1200分辨率 IPS屏幕', '片', NULL, 30.00, NULL, NULL, 200.00, '京东方科技', NULL, 180.0000, 1, '2025-06-10 20:28:51', NULL, '采购员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '平板电脑显示屏', 1, 'B区-02-03');
INSERT INTO `material_info` VALUES (8, 'M008', '蓝牙5.0模块', '电子元件', NULL, '低功耗蓝牙通信模块', '个', NULL, 50.00, NULL, NULL, 500.00, '联发科技', NULL, 15.8000, 1, '2025-06-10 20:28:51', NULL, '采购员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '无线连接模块', 1, 'A区-01-08');
INSERT INTO `material_info` VALUES (9, 'M009', '扬声器单元', '电子元件', NULL, '全频段高保真扬声器', '个', NULL, 40.00, NULL, NULL, 300.00, '哈曼卡顿', NULL, 45.0000, 1, '2025-06-10 20:28:51', NULL, '采购员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '音箱核心组件', 1, 'C区-01-02');
INSERT INTO `material_info` VALUES (10, 'M010', '硅胶表带111', '配件', '', '医用级硅胶材质表带', '条', NULL, 100.00, NULL, NULL, 1000.00, '富士康精密', NULL, 12.0000, 1, '2025-06-10 20:28:51', NULL, '采购员', '2025-06-11 23:26:31', NULL, NULL, 1, NULL, NULL, NULL, '智能手表配件', 1, 'D区-03-01');

-- ----------------------------
-- Table structure for process_route
-- ----------------------------
DROP TABLE IF EXISTS `process_route`;
CREATE TABLE `process_route`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '工艺路线ID',
  `route_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '工艺路线编码（唯一标识）',
  `route_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '工艺路线名称',
  `product_id` int NOT NULL COMMENT '产品ID',
  `version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'V1.0' COMMENT '版本号',
  `status` int NOT NULL DEFAULT 0 COMMENT '状态 (0:草稿, 1:启用, 2:停用)',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '描述',
  `create_user_id` int NOT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_user_id` int NULL DEFAULT NULL COMMENT '更新人ID',
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '更新人姓名',
  `update_time` datetime NULL DEFAULT NULL COMMENT '更新时间',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_route_code`(`route_code` ASC) USING BTREE,
  INDEX `idx_route_name`(`route_name` ASC) USING BTREE,
  INDEX `idx_product_id`(`product_id` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_version`(`version` ASC) USING BTREE,
  INDEX `idx_create_time`(`create_time` ASC) USING BTREE,
  INDEX `idx_is_deleted`(`is_deleted` ASC) USING BTREE,
  INDEX `idx_process_route_product_status`(`product_id` ASC, `status` ASC, `is_deleted` ASC) USING BTREE,
  INDEX `idx_process_route_status_time`(`status` ASC, `create_time` ASC) USING BTREE,
  CONSTRAINT `process_route_ibfk_1` FOREIGN KEY (`product_id`) REFERENCES `material_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '工艺路线表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of process_route
-- ----------------------------
INSERT INTO `process_route` VALUES (1, 'PR001', '智能手机主板生产工艺', 4, 'V1.0', 1, 'SMT贴片+回流焊接+功能测试的完整工艺流程', 1, '系统', '2025-06-10 16:38:56', NULL, NULL, NULL, 0);
INSERT INTO `process_route` VALUES (2, 'PR002', '锂电池组装工艺', 5, 'V2.1', 0, '电芯检测+组装焊接的电池生产工艺', 1, '系统', '2025-06-10 16:38:56', NULL, NULL, NULL, 0);
INSERT INTO `process_route` VALUES (3, 'PR003', '产品A标准工艺', 4, 'V1.2', 1, '产品A的标准生产工艺路线', 1, '系统', '2025-06-10 16:38:56', NULL, NULL, NULL, 0);
INSERT INTO `process_route` VALUES (4, 'PR004', '产品B精密工艺', 5, 'V1.0', 2, '产品B的精密加工工艺路线（已停用）', 1, '系统', '2025-06-10 16:38:56', NULL, NULL, NULL, 0);

-- ----------------------------
-- Table structure for process_step
-- ----------------------------
DROP TABLE IF EXISTS `process_step`;
CREATE TABLE `process_step`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '工艺步骤ID',
  `process_route_id` int NOT NULL COMMENT '工艺路线ID',
  `step_number` int NOT NULL COMMENT '步骤序号',
  `step_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '步骤名称',
  `step_type` int NOT NULL DEFAULT 1 COMMENT '步骤类型 (1:加工, 2:检验, 3:装配, 4:包装, 5:测试)',
  `workstation_id` int NOT NULL COMMENT '工作站ID',
  `standard_time` decimal(8, 2) NOT NULL DEFAULT 0.00 COMMENT '标准工时（分钟）',
  `setup_time` decimal(8, 2) NOT NULL DEFAULT 0.00 COMMENT '准备时间（分钟）',
  `wait_time` decimal(8, 2) NOT NULL DEFAULT 0.00 COMMENT '等待时间（分钟）',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '步骤描述',
  `operation_instructions` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '操作说明',
  `quality_requirements` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '质量要求',
  `safety_notes` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '安全注意事项',
  `required_skill_level` int NOT NULL DEFAULT 1 COMMENT '所需技能等级 (1-10)',
  `is_critical` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否关键步骤',
  `requires_inspection` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否需要检验',
  `status` int NOT NULL DEFAULT 1 COMMENT '状态 (0:停用, 1:启用)',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_time` datetime NULL DEFAULT NULL COMMENT '更新时间',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_process_route_id`(`process_route_id` ASC) USING BTREE,
  INDEX `idx_step_number`(`step_number` ASC) USING BTREE,
  INDEX `idx_step_name`(`step_name` ASC) USING BTREE,
  INDEX `idx_step_type`(`step_type` ASC) USING BTREE,
  INDEX `idx_workstation_id`(`workstation_id` ASC) USING BTREE,
  INDEX `idx_standard_time`(`standard_time` ASC) USING BTREE,
  INDEX `idx_is_critical`(`is_critical` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_is_deleted`(`is_deleted` ASC) USING BTREE,
  INDEX `idx_process_step_route_number`(`process_route_id` ASC, `step_number` ASC, `is_deleted` ASC) USING BTREE,
  INDEX `idx_process_step_workstation_type`(`workstation_id` ASC, `step_type` ASC, `status` ASC) USING BTREE,
  CONSTRAINT `process_step_ibfk_1` FOREIGN KEY (`process_route_id`) REFERENCES `process_route` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `process_step_ibfk_2` FOREIGN KEY (`workstation_id`) REFERENCES `workstation_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '工艺步骤表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of process_step
-- ----------------------------
INSERT INTO `process_step` VALUES (1, 1, 1, 'SMT贴片', 1, 1, 30.00, 5.00, 2.00, '表面贴装技术，将电子元件贴装到PCB上', '按照贴片程序进行自动贴片，注意元件方向', '贴片精度±0.05mm，无漏贴、错贴', '注意静电防护，佩戴防静电手套', 3, 1, 1, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (2, 1, 2, '回流焊接', 1, 2, 15.00, 3.00, 1.00, '高温焊接工艺，使焊膏熔化形成焊点', '设置正确的温度曲线，监控焊接质量', '焊点饱满，无虚焊、连焊现象', '高温作业，注意防烫伤', 4, 1, 1, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (3, 1, 3, '功能测试', 5, 3, 20.00, 2.00, 1.00, '电路功能检测，确保产品性能', '按照测试程序进行全功能测试', '所有功能正常，测试通过率≥99%', '注意用电安全', 2, 1, 1, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (4, 2, 1, '电芯检测', 2, 6, 10.00, 1.00, 0.50, '电芯质量检测，筛选合格电芯', '使用专用设备检测电芯容量和内阻', '容量偏差≤2%，内阻≤规定值', '注意电池安全，防止短路', 2, 1, 1, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (5, 2, 2, '组装焊接', 3, 4, 25.00, 5.00, 2.00, '电芯组装焊接，形成电池组', '按照工艺要求进行串并联焊接', '焊接牢固，绝缘良好', '焊接作业，注意通风', 4, 1, 1, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (6, 3, 1, '预处理', 1, 1, 8.00, 2.00, 1.00, '产品预处理工序', '清洁和预处理产品表面', '表面清洁，无污染', '使用化学品时注意防护', 2, 0, 0, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (7, 3, 2, '精密加工', 1, 2, 45.00, 10.00, 3.00, '精密加工工序', '按照图纸要求进行精密加工', '尺寸精度±0.01mm', '机械加工，注意安全操作', 5, 1, 1, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (8, 3, 3, '质量检验', 2, 6, 15.00, 1.00, 0.50, '产品质量检验', '全面检验产品质量', '符合质量标准要求', '检验设备操作安全', 3, 1, 1, 1, '2025-06-10 16:38:56', NULL, 0);
INSERT INTO `process_step` VALUES (9, 3, 4, '包装入库', 4, 7, 5.00, 1.00, 0.50, '产品包装入库', '按照包装标准进行包装', '包装完整，标识清楚', '搬运时注意轻拿轻放', 1, 0, 0, 1, '2025-06-10 16:38:56', NULL, 0);

-- ----------------------------
-- Table structure for product_info
-- ----------------------------
DROP TABLE IF EXISTS `product_info`;
CREATE TABLE `product_info`  (
  `id` int NOT NULL AUTO_INCREMENT,
  `product_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `product_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `product_type` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `specification` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '个',
  `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '有效',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL,
  `create_time` datetime NULL DEFAULT CURRENT_TIMESTAMP,
  `create_user_id` int NULL DEFAULT NULL,
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `update_time` datetime NULL DEFAULT NULL,
  `update_user_id` int NULL DEFAULT NULL,
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  `is_deleted` tinyint(1) NULL DEFAULT 0,
  `delete_time` datetime NULL DEFAULT NULL,
  `delete_user_id` int NULL DEFAULT NULL,
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `product_code`(`product_code` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of product_info
-- ----------------------------
INSERT INTO `product_info` VALUES (1, 'P001', '智能手机', '电子产品', '6.1英寸屏幕，128GB存储', '台', '有效', '高性能智能手机，支持5G网络', '2025-06-10 20:08:24', NULL, '系统管理员', '2025-06-10 20:25:05', NULL, '系统管理员', 0, NULL, NULL, NULL);
INSERT INTO `product_info` VALUES (2, 'P002', '笔记本电脑', '电子产品', '14英寸，8GB内存，256GB SSD', '台', '有效', '轻薄便携笔记本电脑，适合办公', '2025-06-10 20:08:24', NULL, '系统管理员', '2025-06-10 20:25:05', NULL, '系统管理员', 0, NULL, NULL, NULL);
INSERT INTO `product_info` VALUES (3, 'P003', '无线耳机', '电子产品', '蓝牙5.0，降噪功能', '副', '有效', '高品质无线蓝牙耳机，降噪功能', '2025-06-10 20:08:24', NULL, '系统管理员', '2025-06-10 20:25:05', NULL, '系统管理员', 0, NULL, NULL, NULL);
INSERT INTO `product_info` VALUES (4, 'P004', '智能手表', '电子产品', '1.4英寸AMOLED屏幕，GPS定位，心率监测', '块', '有效', NULL, '2025-06-10 20:24:55', NULL, '系统管理员', '2025-06-10 20:25:05', NULL, '系统管理员', 0, NULL, NULL, NULL);
INSERT INTO `product_info` VALUES (5, 'P005', '平板电脑', '电子产品', '10.1英寸屏幕，4GB内存，64GB存储', '台', '有效', NULL, '2025-06-10 20:24:55', NULL, '系统管理员', '2025-06-10 20:25:05', NULL, '系统管理员', 0, NULL, NULL, NULL);
INSERT INTO `product_info` VALUES (6, 'P006', '智能音箱', '电子产品', '360度环绕音效，语音控制，WiFi连接', '台', '有效', NULL, '2025-06-10 20:24:55', NULL, '系统管理员', '2025-06-10 20:25:05', NULL, '系统管理员', 0, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for production_order_info
-- ----------------------------
DROP TABLE IF EXISTS `production_order_info`;
CREATE TABLE `production_order_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '生产订单ID',
  `order_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '订单编号（唯一标识）',
  `material_id` int NULL DEFAULT 0 COMMENT '物料ID（关联物料表）',
  `product_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '产品编号',
  `product_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '产品名称',
  `planned_quantity` decimal(18, 4) NOT NULL COMMENT '计划生产数量',
  `actual_quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '实际完成数量',
  `unit` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '个' COMMENT '单位',
  `plan_start_time` datetime NOT NULL COMMENT '计划开始时间',
  `plan_end_time` datetime NOT NULL COMMENT '计划完成时间',
  `actual_start_time` datetime NULL DEFAULT NULL COMMENT '实际开始时间',
  `actual_end_time` datetime NULL DEFAULT NULL COMMENT '实际完成时间',
  `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '待开始' COMMENT '订单状态：待开始，进行中，已完成，已暂停，已取消',
  `priority` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '普通' COMMENT '优先级：普通、重要、紧急等',
  `workshop_id` int NULL DEFAULT 0 COMMENT '负责车间ID',
  `workshop_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '车间名称',
  `responsible_person` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '负责人',
  `customer_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '客户名称',
  `sales_order_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '销售订单号',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int NULL DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  `update_user_id` int NULL DEFAULT NULL COMMENT '最后修改人ID',
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '最后修改人姓名',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  `delete_time` datetime NULL DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int NULL DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '删除人姓名',
  `remark` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `version` int NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_order_no`(`order_no` ASC) USING BTREE,
  INDEX `idx_product_code`(`product_code` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_priority`(`priority` ASC) USING BTREE,
  INDEX `idx_workshop_id`(`workshop_id` ASC) USING BTREE,
  INDEX `idx_plan_start_time`(`plan_start_time` ASC) USING BTREE,
  INDEX `idx_customer_name`(`customer_name` ASC) USING BTREE,
  INDEX `idx_production_order_status_time`(`status` ASC, `plan_start_time` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '生产订单表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of production_order_info
-- ----------------------------
INSERT INTO `production_order_info` VALUES (2, 'PO202506090002', 5, 'PRD002', '产品B', 50.0000, 10.0000, '台', '2025-06-09 08:00:00', '2025-06-20 18:00:00', NULL, NULL, '进行中', '重要', 1, '生产车间A', '张三', '客户乙', 'SO202506090002', '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `production_order_info` VALUES (3, 'PO202506090003', 4, 'PRD001', '产品A', 80.0000, 80.0000, '台', '2025-06-01 08:00:00', '2025-06-08 18:00:00', NULL, NULL, '已完成', '普通', 2, '装配车间B', '李四', '客户丙', 'SO202506090003', '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `production_order_info` VALUES (4, 'PO202506100004', 0, 'P004', '智能手表', 200.0000, 0.0000, '块', '2025-06-12 08:00:00', '2025-06-18 18:00:00', NULL, NULL, '待开始', '普通', 2, '装配车间', '李工程师', '科技公司D', 'SO202506100004', '2025-06-10 20:25:22', NULL, '生产计划员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '智能手表批量生产订单', 1);
INSERT INTO `production_order_info` VALUES (5, 'PO202506100005', 0, 'P005', '平板电脑', 150.0000, 50.0000, '台', '2025-06-10 08:00:00', '2025-06-20 18:00:00', NULL, NULL, '进行中', '高', 1, '加工车间', '王工程师', '教育机构E', 'SO202506100005', '2025-06-10 20:25:22', NULL, '生产计划员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '教育用平板电脑订单', 1);
INSERT INTO `production_order_info` VALUES (6, 'PO202506100006', 0, 'P006', '智能音箱', 300.0000, 300.0000, '台', '2025-06-01 08:00:00', '2025-06-08 18:00:00', NULL, NULL, '已完成', '普通', 3, '包装车间', '赵工程师', '家居公司F', 'SO202506100006', '2025-06-10 20:25:22', NULL, '生产计划员', NULL, NULL, NULL, 0, NULL, NULL, NULL, '智能音箱大批量订单', 1);
INSERT INTO `production_order_info` VALUES (8, 'PO20250612001035', 0, '1', '1', 1.0000, 0.0000, '个', '2025-06-13 00:10:35', '2025-06-19 00:10:35', NULL, NULL, '待开始', '普通', 0, '', '', '', '', '2025-06-12 00:10:43', NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, '', 1);

-- ----------------------------
-- Table structure for wip_info
-- ----------------------------
DROP TABLE IF EXISTS `wip_info`;
CREATE TABLE `wip_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '在制品ID',
  `wip_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '在制品编号（唯一标识）',
  `batch_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '批次号',
  `work_order_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '工单号',
  `product_id` int NOT NULL COMMENT '产品ID',
  `product_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '产品编码',
  `product_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '产品名称',
  `workshop_id` int NOT NULL COMMENT '当前车间ID',
  `workstation_id` int NULL DEFAULT NULL COMMENT '当前工位ID',
  `workstation_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '当前工位名称',
  `quantity` int NOT NULL COMMENT '数量',
  `completed_quantity` int NULL DEFAULT 0 COMMENT '已完成数量',
  `status` int NULL DEFAULT 0 COMMENT '状态 (0:待开始, 1:生产中, 2:质检中, 3:暂停, 4:已完成)',
  `priority` int NULL DEFAULT 2 COMMENT '优先级 (1:低, 2:普通, 3:高, 4:紧急)',
  `start_time` datetime NOT NULL COMMENT '开始时间',
  `estimated_end_time` datetime NOT NULL COMMENT '预计完成时间',
  `actual_end_time` datetime NULL DEFAULT NULL COMMENT '实际完成时间',
  `unit_price` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '单价',
  `quality_grade` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT 'C' COMMENT '质量等级 (A:优秀, B:良好, C:合格, D:不合格)',
  `responsible_person` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '负责人',
  `remarks` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `create_by` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人',
  `update_by` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '更新人',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_wip_id`(`wip_id` ASC) USING BTREE,
  INDEX `idx_batch_number`(`batch_number` ASC) USING BTREE,
  INDEX `idx_work_order_number`(`work_order_number` ASC) USING BTREE,
  INDEX `idx_product_id`(`product_id` ASC) USING BTREE,
  INDEX `idx_product_code`(`product_code` ASC) USING BTREE,
  INDEX `idx_workshop_id`(`workshop_id` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_priority`(`priority` ASC) USING BTREE,
  INDEX `idx_start_time`(`start_time` ASC) USING BTREE,
  INDEX `idx_estimated_end_time`(`estimated_end_time` ASC) USING BTREE,
  INDEX `idx_responsible_person`(`responsible_person` ASC) USING BTREE,
  INDEX `idx_is_deleted`(`is_deleted` ASC) USING BTREE,
  INDEX `idx_wip_workshop_status`(`workshop_id` ASC, `status` ASC, `is_deleted` ASC) USING BTREE,
  INDEX `idx_wip_status_priority`(`status` ASC, `priority` ASC, `start_time` ASC) USING BTREE,
  INDEX `idx_wip_batch_status`(`batch_number` ASC, `status` ASC) USING BTREE,
  INDEX `idx_wip_workorder_status`(`work_order_number` ASC, `status` ASC) USING BTREE,
  CONSTRAINT `wip_info_ibfk_1` FOREIGN KEY (`workshop_id`) REFERENCES `workshop_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '在制品信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of wip_info
-- ----------------------------
INSERT INTO `wip_info` VALUES (1, 'WIP000001', 'B202506090001', 'WO202506090001', 4, 'PRD001', '产品A', 1, NULL, '工位A1', 100, 0, 0, 2, '2025-06-10 08:00:00', '2025-06-17 18:00:00', NULL, 1500.0000, 'C', '张三', '待开始生产', '2025-06-10 16:38:56', '2025-06-10 16:38:56', '系统', '系统', 0);
INSERT INTO `wip_info` VALUES (2, 'WIP000002', 'B202506090002', 'WO202506090002', 5, 'PRD002', '产品B', 1, NULL, '工位A2', 50, 10, 1, 3, '2025-06-09 08:00:00', '2025-06-20 18:00:00', NULL, 2200.0000, 'B', '张三', '生产中', '2025-06-10 16:38:56', '2025-06-10 16:38:56', '系统', '系统', 0);
INSERT INTO `wip_info` VALUES (3, 'WIP000003', 'B202506090003', 'WO202506090003', 4, 'PRD001', '产品A', 2, NULL, '工位B1', 80, 80, 4, 2, '2025-06-01 08:00:00', '2025-06-08 18:00:00', '2025-06-08 17:30:00', 1500.0000, 'A', '李四', '已完成', '2025-06-10 16:38:56', '2025-06-10 16:38:56', '系统', '系统', 0);
INSERT INTO `wip_info` VALUES (4, 'WIP000004', 'B202506090004', 'WO202506090004', 4, 'PRD001', '产品A', 3, NULL, '工位C1', 60, 30, 2, 2, '2025-06-11 08:00:00', '2025-06-18 18:00:00', NULL, 1500.0000, 'B', '王五', '质检中', '2025-06-10 16:38:56', '2025-06-10 16:38:56', '系统', '系统', 0);
INSERT INTO `wip_info` VALUES (5, 'WIP000005', 'B202506090005', 'WO202506090005', 5, 'PRD002', '产品B', 1, NULL, '工位A3', 40, 0, 3, 1, '2025-06-12 08:00:00', '2025-06-22 18:00:00', NULL, 2200.0000, 'C', '张三', '暂停维护', '2025-06-10 16:38:56', '2025-06-10 16:38:56', '系统', '系统', 0);

-- ----------------------------
-- Table structure for work_order_info
-- ----------------------------
DROP TABLE IF EXISTS `work_order_info`;
CREATE TABLE `work_order_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '工单ID',
  `work_order_id` int NULL DEFAULT 0 COMMENT '工单ID',
  `work_order_num` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '工单号',
  `work_order_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '工单类型',
  `product_id` int NULL DEFAULT 0 COMMENT '产品ID',
  `flow_id` int NULL DEFAULT 0 COMMENT '工艺流程ID',
  `bom_id` int NULL DEFAULT 0 COMMENT 'BOM ID',
  `planned_quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '计划数量',
  `input_quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '投入数量',
  `output_quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '产出数量',
  `scrap_quantity` decimal(18, 4) NULL DEFAULT 0.0000 COMMENT '报废数量',
  `work_order_status` int NULL DEFAULT 0 COMMENT '工单状态(0:未开始,1:进行中,2:已完成,3:已关闭)',
  `process_status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '工艺状态',
  `lock_status` int NULL DEFAULT 0 COMMENT '锁定状态(0:未锁定,1:已锁定)',
  `factory_id` int NULL DEFAULT 0 COMMENT '工厂ID',
  `hot_type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT 'Hot类型',
  `planned_start_time` datetime NULL DEFAULT NULL COMMENT '计划开始时间',
  `planned_due_date` datetime NULL DEFAULT NULL COMMENT '计划到期日',
  `production_start_time` datetime NULL DEFAULT NULL COMMENT '投产时间',
  `completion_time` datetime NULL DEFAULT NULL COMMENT '完成时间',
  `close_time` datetime NULL DEFAULT NULL COMMENT '关闭时间',
  `work_order_version` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '工单版本',
  `parent_work_order_version` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '父工单版本',
  `product_order_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '产品订单号',
  `product_order_version` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '产品订单版本',
  `sales_order_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '销售单号',
  `main_batch_no` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '主批次号',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '说明',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int NULL DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  `update_user_id` int NULL DEFAULT NULL COMMENT '最后修改人ID',
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '最后修改人姓名',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  `delete_time` datetime NULL DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int NULL DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '删除人姓名',
  `remark` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `version` int NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_work_order_num`(`work_order_num` ASC) USING BTREE,
  INDEX `idx_product_id`(`product_id` ASC) USING BTREE,
  INDEX `idx_work_order_status`(`work_order_status` ASC) USING BTREE,
  INDEX `idx_factory_id`(`factory_id` ASC) USING BTREE,
  INDEX `idx_planned_start_time`(`planned_start_time` ASC) USING BTREE,
  INDEX `idx_work_order_status_time`(`work_order_status` ASC, `planned_start_time` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '工单信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of work_order_info
-- ----------------------------
INSERT INTO `work_order_info` VALUES (1, 0, 'WO202506090001', '生产工单', 1, 0, 0, 100.0000, 0.0000, 0.0000, 0.0000, 0, NULL, 0, 1, NULL, '2025-06-10 08:00:00', '2025-06-17 18:00:00', NULL, NULL, NULL, NULL, NULL, 'PO202506090001', NULL, 'SO202506090001', 'B202506090001', '产品A生产工单', '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `work_order_info` VALUES (2, 0, 'WO202506090002', '生产工单', 2, 0, 0, 50.0000, 50.0000, 10.0000, 0.0000, 1, NULL, 0, 1, NULL, '2025-06-09 08:00:00', '2025-06-20 18:00:00', NULL, NULL, NULL, NULL, NULL, 'PO202506090002', NULL, 'SO202506090002', 'B202506090002', '产品B生产工单', '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `work_order_info` VALUES (3, 0, 'WO202506090003', '生产工单', 1, 0, 0, 80.0000, 80.0000, 80.0000, 0.0000, 2, NULL, 0, 1, NULL, '2025-06-01 08:00:00', '2025-06-08 18:00:00', NULL, NULL, NULL, NULL, NULL, 'PO202506090003', NULL, 'SO202506090003', 'B202506090003', '产品A生产工单', '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `work_order_info` VALUES (4, 0, 'WO202506110001', '生产工单', 0, 0, 0, 11.0000, 0.0000, 0.0000, 0.0000, 0, NULL, 0, 0, NULL, '2025-06-11 10:53:03', '2025-06-18 10:53:03', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, ' [取消原因: 111]', '2025-06-11 10:53:17', NULL, NULL, NULL, NULL, NULL, 1, '2025-06-11 10:54:47', 0, '系统', NULL, 1);

-- ----------------------------
-- Table structure for workshop_info
-- ----------------------------
DROP TABLE IF EXISTS `workshop_info`;
CREATE TABLE `workshop_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '车间ID',
  `workshop_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '车间编码（唯一标识）',
  `workshop_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '车间名称',
  `department` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '所属部门',
  `manager` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '车间负责人',
  `manager_id` int NULL DEFAULT NULL COMMENT '负责人ID (关联操作员/用户)',
  `phone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '联系电话',
  `location` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '车间位置/地址',
  `area` decimal(10, 2) NULL DEFAULT 0.00 COMMENT '车间面积（平方米）',
  `equipment_count` int NULL DEFAULT 0 COMMENT '设备数量',
  `employee_count` int NULL DEFAULT 0 COMMENT '员工数量',
  `workshop_type` int NULL DEFAULT 1 COMMENT '车间类型：1-生产车间，2-装配车间，3-包装车间，4-质检车间，5-仓储车间',
  `production_capacity` int NULL DEFAULT 0 COMMENT '生产能力（件/天）',
  `status` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '1' COMMENT '车间状态：0-停用，1-正常运行，2-维护中，3-故障停机',
  `work_shift` int NULL DEFAULT 1 COMMENT '工作班次：1-单班，2-两班，3-三班',
  `safety_level` int NULL DEFAULT 1 COMMENT '安全等级：1-一般，2-重要，3-关键',
  `environment_requirement` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '环境要求',
  `quality_standard` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '质量标准',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '描述信息',
  `equipment_list` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '设备列表 (逗号分隔的设备编码)',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int NULL DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  `update_user_id` int NULL DEFAULT NULL COMMENT '最后修改人ID',
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '最后修改人姓名',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  `delete_time` datetime NULL DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int NULL DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '删除人姓名',
  `remark` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `version` int NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_workshop_code`(`workshop_code` ASC) USING BTREE,
  INDEX `idx_workshop_name`(`workshop_name` ASC) USING BTREE,
  INDEX `idx_workshop_type`(`workshop_type` ASC) USING BTREE,
  INDEX `idx_manager_id`(`manager_id` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_workshop_status_type`(`status` ASC, `workshop_type` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '车间信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of workshop_info
-- ----------------------------
INSERT INTO `workshop_info` VALUES (1, 'WS001', '生产车间A', '生产部', '张三', NULL, NULL, '1号厂房', 500.00, 0, 0, 1, 1000, '1', 1, 1, NULL, NULL, NULL, NULL, '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `workshop_info` VALUES (2, 'WS002', '装配车间B', '生产部', '李四', NULL, NULL, '2号厂房', 300.00, 0, 0, 2, 800, '1', 1, 1, NULL, NULL, NULL, NULL, '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `workshop_info` VALUES (3, 'WS003', '包装车间C', '生产部', '王五', NULL, NULL, '3号厂房', 200.00, 0, 0, 3, 600, '1', 1, 1, NULL, NULL, NULL, NULL, '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `workshop_info` VALUES (4, 'WS004', '质检车间D', '质量部', '轩天帝', NULL, NULL, '4号厂房', 150.00, 0, 0, 4, 400, '1', 1, 1, NULL, NULL, NULL, NULL, '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);

-- ----------------------------
-- Table structure for workshop_operation_info
-- ----------------------------
DROP TABLE IF EXISTS `workshop_operation_info`;
CREATE TABLE `workshop_operation_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '作业ID',
  `operation_id` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '作业编号',
  `workshop_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '车间名称',
  `batch_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '批次号',
  `product_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '产品编码',
  `quantity` decimal(18, 4) NOT NULL COMMENT '数量',
  `status` int NULL DEFAULT 0 COMMENT '状态：0-待开始，1-进行中，2-已暂停，3-已完成，4-已停止',
  `status_text` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT '待开始' COMMENT '状态文本',
  `start_time` datetime NULL DEFAULT NULL COMMENT '开始时间',
  `progress` decimal(5, 2) NULL DEFAULT 0.00 COMMENT '进度(%)',
  `operator` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '操作员',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `create_user_id` int NULL DEFAULT NULL COMMENT '创建人ID',
  `create_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '创建人姓名',
  `update_time` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  `update_user_id` int NULL DEFAULT NULL COMMENT '最后修改人ID',
  `update_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '最后修改人姓名',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  `delete_time` datetime NULL DEFAULT NULL COMMENT '删除时间',
  `delete_user_id` int NULL DEFAULT NULL COMMENT '删除人ID',
  `delete_user_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '删除人姓名',
  `remark` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '备注',
  `version` int NOT NULL DEFAULT 1 COMMENT '版本号（用于乐观锁）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_operation_id`(`operation_id` ASC) USING BTREE,
  INDEX `idx_workshop_name`(`workshop_name` ASC) USING BTREE,
  INDEX `idx_batch_number`(`batch_number` ASC) USING BTREE,
  INDEX `idx_product_code`(`product_code` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_operator`(`operator` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '车间作业信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of workshop_operation_info
-- ----------------------------
INSERT INTO `workshop_operation_info` VALUES (1, 'OP000001', '生产车间A', 'B202506090001', 'PRD001', 100.0000, 0, '待开始', NULL, 0.00, '张三', '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);
INSERT INTO `workshop_operation_info` VALUES (2, 'OP000002', '生产车间A', 'B202506090002', 'PRD002', 50.0000, 1, '进行中', '2025-06-11 18:38:33', 20.00, '张三', '2025-06-10 16:38:56', NULL, '系统', '2025-06-11 18:38:33', NULL, '', 0, NULL, NULL, NULL, '', 1);
INSERT INTO `workshop_operation_info` VALUES (3, 'OP000003', '装配车间B', 'B202506090003', 'PRD001', 80.0000, 3, '已完成', '2025-06-01 08:00:00', 100.00, '李四', '2025-06-10 16:38:56', NULL, '系统', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1);

-- ----------------------------
-- Table structure for workstation_info
-- ----------------------------
DROP TABLE IF EXISTS `workstation_info`;
CREATE TABLE `workstation_info`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '工作站ID',
  `workstation_code` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '工作站编码',
  `workstation_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '工作站名称',
  `workshop_id` int NOT NULL COMMENT '所属车间ID',
  `workstation_type` int NOT NULL DEFAULT 1 COMMENT '工作站类型 (1:加工, 2:检验, 3:装配, 4:包装, 5:测试)',
  `capacity` int NOT NULL DEFAULT 1 COMMENT '产能（件/小时）',
  `status` int NOT NULL DEFAULT 1 COMMENT '状态 (0:停用, 1:启用, 2:维护)',
  `location` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL DEFAULT NULL COMMENT '位置',
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '描述',
  `is_enabled` tinyint(1) NOT NULL DEFAULT 1 COMMENT '是否启用',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  `update_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
  `is_deleted` tinyint(1) NOT NULL DEFAULT 0 COMMENT '是否删除（软删除标记）',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `uk_workstation_code`(`workstation_code` ASC) USING BTREE,
  INDEX `idx_workstation_name`(`workstation_name` ASC) USING BTREE,
  INDEX `idx_workshop_id`(`workshop_id` ASC) USING BTREE,
  INDEX `idx_workstation_type`(`workstation_type` ASC) USING BTREE,
  INDEX `idx_status`(`status` ASC) USING BTREE,
  INDEX `idx_is_enabled`(`is_enabled` ASC) USING BTREE,
  INDEX `idx_is_deleted`(`is_deleted` ASC) USING BTREE,
  INDEX `idx_workstation_workshop_type`(`workshop_id` ASC, `workstation_type` ASC, `is_enabled` ASC) USING BTREE,
  CONSTRAINT `workstation_info_ibfk_1` FOREIGN KEY (`workshop_id`) REFERENCES `workshop_info` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE = InnoDB AUTO_INCREMENT = 9 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci COMMENT = '工作站信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of workstation_info
-- ----------------------------
INSERT INTO `workstation_info` VALUES (1, 'WS001', 'SMT生产线', 1, 1, 50, 1, 'A区-01', 'SMT表面贴装生产线', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);
INSERT INTO `workstation_info` VALUES (2, 'WS002', '回流焊炉', 1, 1, 60, 1, 'A区-02', '高温回流焊接设备', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);
INSERT INTO `workstation_info` VALUES (3, 'WS003', '测试工位', 1, 5, 30, 1, 'A区-03', '电路功能测试工位', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);
INSERT INTO `workstation_info` VALUES (4, 'WS004', '装配线01', 2, 3, 40, 1, 'B区-01', '产品装配生产线', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);
INSERT INTO `workstation_info` VALUES (5, 'WS005', '装配线02', 2, 3, 40, 2, 'B区-02', '产品装配生产线（维护中）', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);
INSERT INTO `workstation_info` VALUES (6, 'WS006', '质检工位', 3, 2, 25, 1, 'C区-01', '产品质量检验工位', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);
INSERT INTO `workstation_info` VALUES (7, 'WS007', '包装线01', 4, 4, 80, 1, 'D区-01', '产品包装生产线', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);
INSERT INTO `workstation_info` VALUES (8, 'WS008', '包装线02', 4, 4, 80, 1, 'D区-02', '产品包装生产线', 1, '2025-06-10 16:38:56', '2025-06-10 16:38:56', 0);

SET FOREIGN_KEY_CHECKS = 1;
