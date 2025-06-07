-- =============================================
-- MES制造执行系统 - 索引和存储过程创建脚本
-- 版本: 1.0
-- 创建时间: 2025-06-06
-- 说明: 创建性能优化索引和常用存储过程
-- =============================================

USE `mes_system`;

-- =============================================
-- 性能优化索引
-- =============================================

-- 复合索引优化查询性能
-- 物料表复合索引
CREATE INDEX `idx_material_type_status` ON `material` (`material_type`, `status`);
CREATE INDEX `idx_material_supplier_type` ON `material` (`supplier`, `material_type`);
CREATE INDEX `idx_material_stock_safety` ON `material` (`current_stock`, `safety_stock`);

-- BOM表复合索引
CREATE INDEX `idx_bom_parent_level` ON `bom` (`parent_material_id`, `level`);
CREATE INDEX `idx_bom_effective_status` ON `bom` (`effective_date`, `expire_date`, `status`);

-- 生产订单表复合索引
CREATE INDEX `idx_production_status_priority` ON `production_order` (`status`, `priority`);
CREATE INDEX `idx_production_time_range` ON `production_order` (`planned_start_time`, `planned_end_time`);
CREATE INDEX `idx_production_workshop_status` ON `production_order` (`workshop_id`, `status`);

-- 设备表复合索引
CREATE INDEX `idx_equipment_workshop_status` ON `equipment` (`workshop_id`, `status`);
CREATE INDEX `idx_equipment_maintenance_status` ON `equipment` (`next_maintenance_date`, `status`);

-- 质量检验表复合索引
CREATE INDEX `idx_inspection_type_result` ON `quality_inspection` (`inspection_type`, `inspection_result`);
CREATE INDEX `idx_inspection_time_type` ON `quality_inspection` (`inspection_time`, `inspection_type`);
CREATE INDEX `idx_inspection_product_time` ON `quality_inspection` (`product_code`, `inspection_time`);

-- 系统日志表分区索引（按月分区）
CREATE INDEX `idx_log_time_level` ON `sys_log` (`create_time`, `log_level`);
CREATE INDEX `idx_log_module_time` ON `sys_log` (`module_name`, `create_time`);

-- =============================================
-- 常用存储过程
-- =============================================

-- 删除已存在的存储过程
DROP PROCEDURE IF EXISTS `sp_get_material_stock_alert`;
DROP PROCEDURE IF EXISTS `sp_get_production_order_statistics`;
DROP PROCEDURE IF EXISTS `sp_get_equipment_maintenance_schedule`;
DROP PROCEDURE IF EXISTS `sp_get_quality_statistics`;
DROP PROCEDURE IF EXISTS `sp_cleanup_old_logs`;

-- 存储过程：获取库存预警物料
DELIMITER $$
CREATE PROCEDURE `sp_get_material_stock_alert`()
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    SELECT 
        m.id,
        m.material_code,
        m.material_name,
        m.material_type,
        m.current_stock,
        m.safety_stock,
        m.min_stock,
        CASE 
            WHEN m.current_stock <= m.min_stock THEN '严重不足'
            WHEN m.current_stock <= m.safety_stock THEN '库存不足'
            ELSE '正常'
        END AS stock_status,
        m.supplier,
        m.unit
    FROM material m
    WHERE m.is_deleted = 0 
        AND m.status = 1
        AND m.current_stock <= m.safety_stock
    ORDER BY 
        CASE 
            WHEN m.current_stock <= m.min_stock THEN 1
            WHEN m.current_stock <= m.safety_stock THEN 2
            ELSE 3
        END,
        m.current_stock ASC;
END$$
DELIMITER ;

-- 存储过程：获取生产订单统计
DELIMITER $$
CREATE PROCEDURE `sp_get_production_order_statistics`(
    IN p_start_date DATE,
    IN p_end_date DATE
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    SELECT 
        COUNT(*) AS total_orders,
        SUM(CASE WHEN status = '待开始' THEN 1 ELSE 0 END) AS pending_orders,
        SUM(CASE WHEN status = '进行中' THEN 1 ELSE 0 END) AS in_progress_orders,
        SUM(CASE WHEN status = '已完成' THEN 1 ELSE 0 END) AS completed_orders,
        SUM(CASE WHEN status = '已取消' THEN 1 ELSE 0 END) AS cancelled_orders,
        SUM(planned_quantity) AS total_planned_quantity,
        SUM(actual_quantity) AS total_actual_quantity,
        ROUND(
            CASE 
                WHEN SUM(planned_quantity) > 0 THEN 
                    (SUM(actual_quantity) / SUM(planned_quantity)) * 100
                ELSE 0 
            END, 2
        ) AS completion_rate,
        COUNT(DISTINCT workshop_id) AS workshops_involved
    FROM production_order
    WHERE is_deleted = 0
        AND planned_start_time >= p_start_date
        AND planned_start_time <= p_end_date;
END$$
DELIMITER ;

-- 存储过程：获取设备维护计划
DELIMITER $$
CREATE PROCEDURE `sp_get_equipment_maintenance_schedule`(
    IN p_days_ahead INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    SELECT 
        e.id,
        e.equipment_code,
        e.equipment_name,
        e.equipment_type,
        e.workshop_name,
        e.next_maintenance_date,
        e.maintenance_cycle,
        e.responsible_person_name,
        DATEDIFF(e.next_maintenance_date, CURDATE()) AS days_until_maintenance,
        CASE 
            WHEN e.next_maintenance_date < CURDATE() THEN '已逾期'
            WHEN e.next_maintenance_date = CURDATE() THEN '今日维护'
            WHEN DATEDIFF(e.next_maintenance_date, CURDATE()) <= 3 THEN '紧急'
            WHEN DATEDIFF(e.next_maintenance_date, CURDATE()) <= 7 THEN '即将到期'
            ELSE '正常'
        END AS maintenance_status
    FROM equipment e
    WHERE e.is_deleted = 0 
        AND e.status IN (1, 2) -- 正常或维护中
        AND e.next_maintenance_date IS NOT NULL
        AND e.next_maintenance_date <= DATE_ADD(CURDATE(), INTERVAL p_days_ahead DAY)
    ORDER BY e.next_maintenance_date ASC, e.equipment_code;
END$$
DELIMITER ;

-- 存储过程：获取质量统计
DELIMITER $$
CREATE PROCEDURE `sp_get_quality_statistics`(
    IN p_start_date DATE,
    IN p_end_date DATE,
    IN p_inspection_type INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    SELECT 
        COUNT(*) AS total_inspections,
        SUM(inspection_quantity) AS total_inspection_quantity,
        SUM(qualified_quantity) AS total_qualified_quantity,
        SUM(unqualified_quantity) AS total_unqualified_quantity,
        SUM(CASE WHEN inspection_result = 1 THEN 1 ELSE 0 END) AS qualified_count,
        SUM(CASE WHEN inspection_result = 2 THEN 1 ELSE 0 END) AS unqualified_count,
        SUM(CASE WHEN inspection_result = 3 THEN 1 ELSE 0 END) AS conditional_count,
        ROUND(
            CASE 
                WHEN COUNT(*) > 0 THEN 
                    (SUM(CASE WHEN inspection_result = 1 THEN 1 ELSE 0 END) / COUNT(*)) * 100
                ELSE 0 
            END, 2
        ) AS pass_rate,
        ROUND(
            CASE 
                WHEN SUM(inspection_quantity) > 0 THEN 
                    (SUM(qualified_quantity) / SUM(inspection_quantity)) * 100
                ELSE 0 
            END, 2
        ) AS quantity_pass_rate,
        COUNT(DISTINCT product_code) AS products_inspected,
        COUNT(DISTINCT inspector_id) AS inspectors_involved
    FROM quality_inspection
    WHERE is_deleted = 0
        AND inspection_time >= p_start_date
        AND inspection_time <= p_end_date
        AND (p_inspection_type = 0 OR inspection_type = p_inspection_type);
END$$
DELIMITER ;

-- 存储过程：清理旧日志
DELIMITER $$
CREATE PROCEDURE `sp_cleanup_old_logs`(
    IN p_retention_days INT
)
BEGIN
    DECLARE v_cutoff_date DATETIME;
    DECLARE v_deleted_count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    
    -- 计算截止日期
    SET v_cutoff_date = DATE_SUB(NOW(), INTERVAL p_retention_days DAY);
    
    -- 删除旧日志
    DELETE FROM sys_log 
    WHERE create_time < v_cutoff_date;
    
    -- 获取删除的记录数
    SET v_deleted_count = ROW_COUNT();
    
    -- 插入清理日志
    INSERT INTO sys_log (
        log_level, 
        log_type, 
        module_name, 
        operation_name, 
        log_message, 
        create_time
    ) VALUES (
        'INFO', 
        'SYSTEM', 
        'LOG_CLEANUP', 
        'AUTO_CLEANUP', 
        CONCAT('自动清理了 ', v_deleted_count, ' 条超过 ', p_retention_days, ' 天的日志记录'), 
        NOW()
    );
    
    COMMIT;
    
    SELECT v_deleted_count AS deleted_count, v_cutoff_date AS cutoff_date;
END$$
DELIMITER ;

-- =============================================
-- 创建视图
-- =============================================

-- 物料库存预警视图
CREATE OR REPLACE VIEW `v_material_stock_alert` AS
SELECT 
    m.id,
    m.material_code,
    m.material_name,
    m.material_type,
    m.current_stock,
    m.safety_stock,
    m.min_stock,
    m.unit,
    m.supplier,
    CASE 
        WHEN m.current_stock <= m.min_stock THEN '严重不足'
        WHEN m.current_stock <= m.safety_stock THEN '库存不足'
        ELSE '正常'
    END AS stock_status,
    CASE 
        WHEN m.current_stock <= m.min_stock THEN 1
        WHEN m.current_stock <= m.safety_stock THEN 2
        ELSE 3
    END AS alert_level
FROM material m
WHERE m.is_deleted = 0 AND m.status = 1;

-- 生产订单进度视图
CREATE OR REPLACE VIEW `v_production_order_progress` AS
SELECT 
    po.id,
    po.order_number,
    po.product_code,
    po.product_name,
    po.planned_quantity,
    po.actual_quantity,
    po.status,
    po.priority,
    po.planned_start_time,
    po.planned_end_time,
    po.actual_start_time,
    po.actual_end_time,
    po.workshop_name,
    CASE 
        WHEN po.planned_quantity > 0 THEN 
            ROUND((po.actual_quantity / po.planned_quantity) * 100, 2)
        ELSE 0 
    END AS completion_percentage,
    CASE 
        WHEN po.status = '已完成' THEN 
            DATEDIFF(po.actual_end_time, po.planned_end_time)
        WHEN po.status IN ('进行中', '已暂停') THEN 
            DATEDIFF(NOW(), po.planned_end_time)
        ELSE NULL
    END AS delay_days
FROM production_order po
WHERE po.is_deleted = 0;

-- 设备维护计划视图
CREATE OR REPLACE VIEW `v_equipment_maintenance_schedule` AS
SELECT 
    e.id,
    e.equipment_code,
    e.equipment_name,
    e.equipment_type,
    e.workshop_name,
    e.status,
    e.last_maintenance_date,
    e.next_maintenance_date,
    e.maintenance_cycle,
    e.responsible_person_name,
    DATEDIFF(e.next_maintenance_date, CURDATE()) AS days_until_maintenance,
    CASE 
        WHEN e.next_maintenance_date < CURDATE() THEN '已逾期'
        WHEN e.next_maintenance_date = CURDATE() THEN '今日维护'
        WHEN DATEDIFF(e.next_maintenance_date, CURDATE()) <= 3 THEN '紧急'
        WHEN DATEDIFF(e.next_maintenance_date, CURDATE()) <= 7 THEN '即将到期'
        ELSE '正常'
    END AS maintenance_status
FROM equipment e
WHERE e.is_deleted = 0 AND e.next_maintenance_date IS NOT NULL;

-- 质量检验汇总视图
CREATE OR REPLACE VIEW `v_quality_inspection_summary` AS
SELECT 
    qi.product_code,
    qi.product_name,
    qi.inspection_type,
    COUNT(*) AS total_inspections,
    SUM(qi.inspection_quantity) AS total_quantity,
    SUM(qi.qualified_quantity) AS qualified_quantity,
    SUM(qi.unqualified_quantity) AS unqualified_quantity,
    SUM(CASE WHEN qi.inspection_result = 1 THEN 1 ELSE 0 END) AS pass_count,
    ROUND(
        CASE 
            WHEN COUNT(*) > 0 THEN 
                (SUM(CASE WHEN qi.inspection_result = 1 THEN 1 ELSE 0 END) / COUNT(*)) * 100
            ELSE 0 
        END, 2
    ) AS pass_rate,
    DATE(qi.inspection_time) AS inspection_date
FROM quality_inspection qi
WHERE qi.is_deleted = 0
GROUP BY qi.product_code, qi.product_name, qi.inspection_type, DATE(qi.inspection_time);

-- =============================================
-- 创建触发器
-- =============================================

-- 物料库存更新触发器
DELIMITER $$
CREATE TRIGGER `tr_material_stock_update` 
AFTER UPDATE ON `material`
FOR EACH ROW
BEGIN
    IF OLD.current_stock != NEW.current_stock THEN
        INSERT INTO sys_log (
            log_level,
            log_type,
            module_name,
            operation_name,
            log_message,
            user_id,
            user_name,
            create_time
        ) VALUES (
            'INFO',
            'BUSINESS',
            'MATERIAL',
            'STOCK_UPDATE',
            CONCAT('物料 ', NEW.material_code, ' 库存从 ', OLD.current_stock, ' 更新为 ', NEW.current_stock),
            NEW.update_user_id,
            NEW.update_user_name,
            NOW()
        );
    END IF;
END$$
DELIMITER ;

-- 生产订单状态变更触发器
DELIMITER $$
CREATE TRIGGER `tr_production_order_status_change` 
AFTER UPDATE ON `production_order`
FOR EACH ROW
BEGIN
    IF OLD.status != NEW.status THEN
        INSERT INTO sys_log (
            log_level,
            log_type,
            module_name,
            operation_name,
            log_message,
            user_id,
            user_name,
            create_time
        ) VALUES (
            'INFO',
            'BUSINESS',
            'PRODUCTION',
            'STATUS_CHANGE',
            CONCAT('生产订单 ', NEW.order_number, ' 状态从 ', OLD.status, ' 变更为 ', NEW.status),
            NEW.update_user_id,
            NEW.update_user_name,
            NOW()
        );
    END IF;
END$$
DELIMITER ;

-- 输出创建结果
SELECT 'MES系统索引、存储过程、视图和触发器创建完成' AS result;
