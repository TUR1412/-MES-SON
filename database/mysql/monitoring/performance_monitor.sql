-- =============================================
-- MES制造执行系统 - 数据库性能监控脚本
-- 版本: 1.0
-- 创建时间: 2025-06-06
-- 说明: 提供完整的数据库性能监控解决方案
-- =============================================

USE `mes_system`;

-- =============================================
-- 性能监控表
-- =============================================

-- 创建性能监控日志表
CREATE TABLE IF NOT EXISTS `sys_performance_log` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY COMMENT '监控记录ID',
    `monitor_type` VARCHAR(50) NOT NULL COMMENT '监控类型：CPU, MEMORY, DISK, CONNECTION, QUERY',
    `metric_name` VARCHAR(100) NOT NULL COMMENT '指标名称',
    `metric_value` DECIMAL(18,4) NOT NULL COMMENT '指标值',
    `metric_unit` VARCHAR(20) DEFAULT '' COMMENT '指标单位',
    `threshold_value` DECIMAL(18,4) DEFAULT 0 COMMENT '阈值',
    `alert_level` VARCHAR(20) DEFAULT 'INFO' COMMENT '告警级别：INFO, WARNING, ERROR, CRITICAL',
    `alert_message` TEXT COMMENT '告警信息',
    `server_name` VARCHAR(100) DEFAULT 'localhost' COMMENT '服务器名称',
    `database_name` VARCHAR(100) DEFAULT 'mes_system' COMMENT '数据库名称',
    `check_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '检查时间',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    INDEX `idx_monitor_type` (`monitor_type`),
    INDEX `idx_check_time` (`check_time`),
    INDEX `idx_alert_level` (`alert_level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='性能监控日志表';

-- 创建性能阈值配置表
CREATE TABLE IF NOT EXISTS `sys_performance_threshold` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '配置ID',
    `monitor_type` VARCHAR(50) NOT NULL COMMENT '监控类型',
    `metric_name` VARCHAR(100) NOT NULL COMMENT '指标名称',
    `warning_threshold` DECIMAL(18,4) NOT NULL COMMENT '警告阈值',
    `error_threshold` DECIMAL(18,4) NOT NULL COMMENT '错误阈值',
    `critical_threshold` DECIMAL(18,4) NOT NULL COMMENT '严重阈值',
    `metric_unit` VARCHAR(20) DEFAULT '' COMMENT '指标单位',
    `check_interval` INT DEFAULT 300 COMMENT '检查间隔(秒)',
    `enabled` TINYINT DEFAULT 1 COMMENT '是否启用：1-启用，0-禁用',
    `description` VARCHAR(500) DEFAULT '' COMMENT '描述',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    `update_time` DATETIME DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '更新时间',
    UNIQUE KEY `uk_monitor_metric` (`monitor_type`, `metric_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='性能阈值配置表';

-- =============================================
-- 插入默认阈值配置
-- =============================================

INSERT INTO `sys_performance_threshold` 
(`monitor_type`, `metric_name`, `warning_threshold`, `error_threshold`, `critical_threshold`, `metric_unit`, `description`) VALUES
('CONNECTION', 'active_connections', 150, 200, 250, 'count', '活跃连接数'),
('CONNECTION', 'connection_usage_rate', 70, 85, 95, 'percent', '连接使用率'),
('QUERY', 'slow_query_count', 10, 50, 100, 'count', '慢查询数量'),
('QUERY', 'avg_query_time', 2, 5, 10, 'seconds', '平均查询时间'),
('MEMORY', 'buffer_pool_usage', 80, 90, 95, 'percent', 'InnoDB缓冲池使用率'),
('DISK', 'disk_usage', 80, 90, 95, 'percent', '磁盘使用率'),
('DISK', 'table_size', 1000, 5000, 10000, 'MB', '单表大小'),
('PERFORMANCE', 'qps', 1000, 2000, 5000, 'queries/sec', '每秒查询数'),
('PERFORMANCE', 'tps', 500, 1000, 2000, 'transactions/sec', '每秒事务数');

-- =============================================
-- 性能监控存储过程
-- =============================================

-- 删除已存在的监控存储过程
DROP PROCEDURE IF EXISTS `sp_monitor_database_performance`;
DROP PROCEDURE IF EXISTS `sp_monitor_connection_status`;
DROP PROCEDURE IF EXISTS `sp_monitor_query_performance`;
DROP PROCEDURE IF EXISTS `sp_monitor_table_status`;
DROP PROCEDURE IF EXISTS `sp_generate_performance_report`;
DROP PROCEDURE IF EXISTS `sp_check_performance_alerts`;

-- 数据库整体性能监控
DELIMITER $$
CREATE PROCEDURE `sp_monitor_database_performance`()
BEGIN
    DECLARE v_qps DECIMAL(18,4) DEFAULT 0;
    DECLARE v_tps DECIMAL(18,4) DEFAULT 0;
    DECLARE v_buffer_pool_usage DECIMAL(18,4) DEFAULT 0;
    DECLARE v_slow_query_count INT DEFAULT 0;
    
    -- 计算QPS (每秒查询数)
    SELECT VARIABLE_VALUE INTO @queries_start
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Queries';
    
    -- 等待1秒
    DO SLEEP(1);
    
    SELECT VARIABLE_VALUE INTO @queries_end
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Queries';
    
    SET v_qps = @queries_end - @queries_start;
    
    -- 计算TPS (每秒事务数)
    SELECT VARIABLE_VALUE INTO @com_commit_start
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Com_commit';
    
    SELECT VARIABLE_VALUE INTO @com_rollback_start
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Com_rollback';
    
    DO SLEEP(1);
    
    SELECT VARIABLE_VALUE INTO @com_commit_end
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Com_commit';
    
    SELECT VARIABLE_VALUE INTO @com_rollback_end
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Com_rollback';
    
    SET v_tps = (@com_commit_end - @com_commit_start) + (@com_rollback_end - @com_rollback_start);
    
    -- 计算InnoDB缓冲池使用率
    SELECT 
        ROUND(
            (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Innodb_buffer_pool_pages_data') /
            (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Innodb_buffer_pool_pages_total') * 100, 2
        ) INTO v_buffer_pool_usage;
    
    -- 获取慢查询数量
    SELECT COUNT(*) INTO v_slow_query_count
    FROM mysql.slow_log 
    WHERE start_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR);
    
    -- 记录性能指标
    INSERT INTO sys_performance_log (monitor_type, metric_name, metric_value, metric_unit, alert_level)
    VALUES 
    ('PERFORMANCE', 'qps', v_qps, 'queries/sec', 
     CASE WHEN v_qps >= 5000 THEN 'CRITICAL' WHEN v_qps >= 2000 THEN 'ERROR' WHEN v_qps >= 1000 THEN 'WARNING' ELSE 'INFO' END),
    ('PERFORMANCE', 'tps', v_tps, 'transactions/sec',
     CASE WHEN v_tps >= 2000 THEN 'CRITICAL' WHEN v_tps >= 1000 THEN 'ERROR' WHEN v_tps >= 500 THEN 'WARNING' ELSE 'INFO' END),
    ('MEMORY', 'buffer_pool_usage', v_buffer_pool_usage, 'percent',
     CASE WHEN v_buffer_pool_usage >= 95 THEN 'CRITICAL' WHEN v_buffer_pool_usage >= 90 THEN 'ERROR' WHEN v_buffer_pool_usage >= 80 THEN 'WARNING' ELSE 'INFO' END),
    ('QUERY', 'slow_query_count', v_slow_query_count, 'count',
     CASE WHEN v_slow_query_count >= 100 THEN 'CRITICAL' WHEN v_slow_query_count >= 50 THEN 'ERROR' WHEN v_slow_query_count >= 10 THEN 'WARNING' ELSE 'INFO' END);
    
    -- 返回监控结果
    SELECT 
        'Database Performance' AS monitor_category,
        v_qps AS qps,
        v_tps AS tps,
        v_buffer_pool_usage AS buffer_pool_usage_percent,
        v_slow_query_count AS slow_query_count_last_hour;
END$$
DELIMITER ;

-- 连接状态监控
DELIMITER $$
CREATE PROCEDURE `sp_monitor_connection_status`()
BEGIN
    DECLARE v_active_connections INT DEFAULT 0;
    DECLARE v_max_connections INT DEFAULT 0;
    DECLARE v_connection_usage_rate DECIMAL(18,4) DEFAULT 0;
    DECLARE v_threads_running INT DEFAULT 0;
    
    -- 获取活跃连接数
    SELECT VARIABLE_VALUE INTO v_active_connections
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Threads_connected';
    
    -- 获取最大连接数
    SELECT VARIABLE_VALUE INTO v_max_connections
    FROM performance_schema.global_variables 
    WHERE VARIABLE_NAME = 'max_connections';
    
    -- 计算连接使用率
    SET v_connection_usage_rate = ROUND((v_active_connections / v_max_connections) * 100, 2);
    
    -- 获取运行中的线程数
    SELECT VARIABLE_VALUE INTO v_threads_running
    FROM performance_schema.global_status 
    WHERE VARIABLE_NAME = 'Threads_running';
    
    -- 记录连接监控指标
    INSERT INTO sys_performance_log (monitor_type, metric_name, metric_value, metric_unit, alert_level)
    VALUES 
    ('CONNECTION', 'active_connections', v_active_connections, 'count',
     CASE WHEN v_active_connections >= 250 THEN 'CRITICAL' WHEN v_active_connections >= 200 THEN 'ERROR' WHEN v_active_connections >= 150 THEN 'WARNING' ELSE 'INFO' END),
    ('CONNECTION', 'connection_usage_rate', v_connection_usage_rate, 'percent',
     CASE WHEN v_connection_usage_rate >= 95 THEN 'CRITICAL' WHEN v_connection_usage_rate >= 85 THEN 'ERROR' WHEN v_connection_usage_rate >= 70 THEN 'WARNING' ELSE 'INFO' END),
    ('CONNECTION', 'threads_running', v_threads_running, 'count',
     CASE WHEN v_threads_running >= 50 THEN 'ERROR' WHEN v_threads_running >= 30 THEN 'WARNING' ELSE 'INFO' END);
    
    -- 返回连接状态
    SELECT 
        'Connection Status' AS monitor_category,
        v_active_connections AS active_connections,
        v_max_connections AS max_connections,
        v_connection_usage_rate AS usage_rate_percent,
        v_threads_running AS threads_running;
END$$
DELIMITER ;

-- 查询性能监控
DELIMITER $$
CREATE PROCEDURE `sp_monitor_query_performance`()
BEGIN
    DECLARE v_avg_query_time DECIMAL(18,4) DEFAULT 0;
    DECLARE v_long_query_count INT DEFAULT 0;
    DECLARE v_select_count BIGINT DEFAULT 0;
    DECLARE v_insert_count BIGINT DEFAULT 0;
    DECLARE v_update_count BIGINT DEFAULT 0;
    DECLARE v_delete_count BIGINT DEFAULT 0;
    
    -- 计算平均查询时间（从慢查询日志）
    SELECT IFNULL(AVG(query_time), 0) INTO v_avg_query_time
    FROM mysql.slow_log 
    WHERE start_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR);
    
    -- 获取长时间运行的查询数量
    SELECT COUNT(*) INTO v_long_query_count
    FROM information_schema.processlist 
    WHERE command != 'Sleep' AND time > 10;
    
    -- 获取各类操作统计
    SELECT VARIABLE_VALUE INTO v_select_count FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Com_select';
    SELECT VARIABLE_VALUE INTO v_insert_count FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Com_insert';
    SELECT VARIABLE_VALUE INTO v_update_count FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Com_update';
    SELECT VARIABLE_VALUE INTO v_delete_count FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Com_delete';
    
    -- 记录查询性能指标
    INSERT INTO sys_performance_log (monitor_type, metric_name, metric_value, metric_unit, alert_level)
    VALUES 
    ('QUERY', 'avg_query_time', v_avg_query_time, 'seconds',
     CASE WHEN v_avg_query_time >= 10 THEN 'CRITICAL' WHEN v_avg_query_time >= 5 THEN 'ERROR' WHEN v_avg_query_time >= 2 THEN 'WARNING' ELSE 'INFO' END),
    ('QUERY', 'long_query_count', v_long_query_count, 'count',
     CASE WHEN v_long_query_count >= 20 THEN 'ERROR' WHEN v_long_query_count >= 10 THEN 'WARNING' ELSE 'INFO' END),
    ('QUERY', 'select_count', v_select_count, 'count', 'INFO'),
    ('QUERY', 'insert_count', v_insert_count, 'count', 'INFO'),
    ('QUERY', 'update_count', v_update_count, 'count', 'INFO'),
    ('QUERY', 'delete_count', v_delete_count, 'count', 'INFO');
    
    -- 返回查询性能
    SELECT 
        'Query Performance' AS monitor_category,
        v_avg_query_time AS avg_query_time_seconds,
        v_long_query_count AS long_running_queries,
        v_select_count AS total_selects,
        v_insert_count AS total_inserts,
        v_update_count AS total_updates,
        v_delete_count AS total_deletes;
END$$
DELIMITER ;

-- 表状态监控
DELIMITER $$
CREATE PROCEDURE `sp_monitor_table_status`()
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_table_name VARCHAR(100);
    DECLARE v_table_size_mb DECIMAL(18,4);
    DECLARE v_table_rows BIGINT;
    
    DECLARE table_cursor CURSOR FOR
        SELECT table_name, 
               ROUND((data_length + index_length) / 1024 / 1024, 2) AS size_mb,
               table_rows
        FROM information_schema.tables 
        WHERE table_schema = 'mes_system' 
        AND table_type = 'BASE TABLE'
        ORDER BY (data_length + index_length) DESC;
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
    
    OPEN table_cursor;
    
    read_loop: LOOP
        FETCH table_cursor INTO v_table_name, v_table_size_mb, v_table_rows;
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        -- 记录表大小监控
        INSERT INTO sys_performance_log (monitor_type, metric_name, metric_value, metric_unit, alert_level, alert_message)
        VALUES (
            'DISK', 
            CONCAT('table_size_', v_table_name), 
            v_table_size_mb, 
            'MB',
            CASE 
                WHEN v_table_size_mb >= 10000 THEN 'CRITICAL'
                WHEN v_table_size_mb >= 5000 THEN 'ERROR'
                WHEN v_table_size_mb >= 1000 THEN 'WARNING'
                ELSE 'INFO'
            END,
            CONCAT('表 ', v_table_name, ' 大小: ', v_table_size_mb, 'MB, 行数: ', v_table_rows)
        );
    END LOOP;
    
    CLOSE table_cursor;
    
    -- 返回表状态汇总
    SELECT 
        'Table Status' AS monitor_category,
        COUNT(*) AS total_tables,
        ROUND(SUM((data_length + index_length) / 1024 / 1024), 2) AS total_size_mb,
        SUM(table_rows) AS total_rows
    FROM information_schema.tables 
    WHERE table_schema = 'mes_system' 
    AND table_type = 'BASE TABLE';
END$$
DELIMITER ;

-- 性能告警检查
DELIMITER $$
CREATE PROCEDURE `sp_check_performance_alerts`()
BEGIN
    DECLARE v_alert_count INT DEFAULT 0;
    
    -- 统计最近1小时的告警数量
    SELECT COUNT(*) INTO v_alert_count
    FROM sys_performance_log 
    WHERE alert_level IN ('WARNING', 'ERROR', 'CRITICAL')
    AND check_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR);
    
    -- 返回告警汇总
    SELECT 
        v_alert_count AS total_alerts_last_hour,
        (SELECT COUNT(*) FROM sys_performance_log WHERE alert_level = 'CRITICAL' AND check_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR)) AS critical_alerts,
        (SELECT COUNT(*) FROM sys_performance_log WHERE alert_level = 'ERROR' AND check_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR)) AS error_alerts,
        (SELECT COUNT(*) FROM sys_performance_log WHERE alert_level = 'WARNING' AND check_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR)) AS warning_alerts;
    
    -- 返回最新的告警详情
    SELECT 
        monitor_type,
        metric_name,
        metric_value,
        metric_unit,
        alert_level,
        alert_message,
        check_time
    FROM sys_performance_log 
    WHERE alert_level IN ('WARNING', 'ERROR', 'CRITICAL')
    AND check_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR)
    ORDER BY check_time DESC, 
             CASE alert_level 
                WHEN 'CRITICAL' THEN 1 
                WHEN 'ERROR' THEN 2 
                WHEN 'WARNING' THEN 3 
                ELSE 4 
             END
    LIMIT 20;
END$$
DELIMITER ;

-- 生成性能报告
DELIMITER $$
CREATE PROCEDURE `sp_generate_performance_report`(
    IN p_start_time DATETIME,
    IN p_end_time DATETIME
)
BEGIN
    -- 性能指标汇总
    SELECT 
        monitor_type,
        metric_name,
        COUNT(*) AS check_count,
        ROUND(AVG(metric_value), 2) AS avg_value,
        ROUND(MIN(metric_value), 2) AS min_value,
        ROUND(MAX(metric_value), 2) AS max_value,
        metric_unit,
        SUM(CASE WHEN alert_level = 'CRITICAL' THEN 1 ELSE 0 END) AS critical_count,
        SUM(CASE WHEN alert_level = 'ERROR' THEN 1 ELSE 0 END) AS error_count,
        SUM(CASE WHEN alert_level = 'WARNING' THEN 1 ELSE 0 END) AS warning_count
    FROM sys_performance_log 
    WHERE check_time BETWEEN p_start_time AND p_end_time
    GROUP BY monitor_type, metric_name, metric_unit
    ORDER BY monitor_type, metric_name;
    
    -- 告警趋势
    SELECT 
        DATE(check_time) AS alert_date,
        HOUR(check_time) AS alert_hour,
        alert_level,
        COUNT(*) AS alert_count
    FROM sys_performance_log 
    WHERE check_time BETWEEN p_start_time AND p_end_time
    AND alert_level IN ('WARNING', 'ERROR', 'CRITICAL')
    GROUP BY DATE(check_time), HOUR(check_time), alert_level
    ORDER BY alert_date DESC, alert_hour DESC;
END$$
DELIMITER ;

-- =============================================
-- 性能监控视图
-- =============================================

-- 实时性能监控视图
CREATE OR REPLACE VIEW `v_performance_realtime` AS
SELECT 
    'Database Performance' AS category,
    (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Threads_connected') AS active_connections,
    (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME = 'Threads_running') AS threads_running,
    (SELECT COUNT(*) FROM information_schema.processlist WHERE command != 'Sleep') AS active_queries,
    (SELECT COUNT(*) FROM information_schema.processlist WHERE command != 'Sleep' AND time > 10) AS long_queries,
    NOW() AS check_time;

-- 性能告警汇总视图
CREATE OR REPLACE VIEW `v_performance_alerts_summary` AS
SELECT 
    monitor_type,
    alert_level,
    COUNT(*) AS alert_count,
    MAX(check_time) AS last_alert_time
FROM sys_performance_log 
WHERE check_time >= DATE_SUB(NOW(), INTERVAL 24 HOUR)
GROUP BY monitor_type, alert_level
ORDER BY monitor_type, 
         CASE alert_level 
            WHEN 'CRITICAL' THEN 1 
            WHEN 'ERROR' THEN 2 
            WHEN 'WARNING' THEN 3 
            ELSE 4 
         END;

-- =============================================
-- 监控脚本使用示例
-- =============================================

/*
-- 执行完整性能监控
CALL sp_monitor_database_performance();
CALL sp_monitor_connection_status();
CALL sp_monitor_query_performance();
CALL sp_monitor_table_status();

-- 检查性能告警
CALL sp_check_performance_alerts();

-- 生成性能报告（最近24小时）
CALL sp_generate_performance_report(DATE_SUB(NOW(), INTERVAL 24 HOUR), NOW());

-- 查看实时性能状态
SELECT * FROM v_performance_realtime;

-- 查看告警汇总
SELECT * FROM v_performance_alerts_summary;

-- 查看最近的性能日志
SELECT * FROM sys_performance_log 
WHERE check_time >= DATE_SUB(NOW(), INTERVAL 1 HOUR)
ORDER BY check_time DESC;
*/

-- 输出创建结果
SELECT 'MES系统数据库性能监控脚本创建完成' AS result;
SELECT '建议设置定时任务每5分钟执行一次监控' AS recommendation;
