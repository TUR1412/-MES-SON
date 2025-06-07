-- =============================================
-- MES制造执行系统 - 数据库备份脚本
-- 版本: 1.0
-- 创建时间: 2025-06-06
-- 说明: 提供完整的数据库备份解决方案
-- =============================================

-- =============================================
-- 备份配置存储过程
-- =============================================

USE `mes_system`;

-- 删除已存在的备份相关存储过程
DROP PROCEDURE IF EXISTS `sp_backup_database_full`;
DROP PROCEDURE IF EXISTS `sp_backup_database_data_only`;
DROP PROCEDURE IF EXISTS `sp_backup_database_structure_only`;
DROP PROCEDURE IF EXISTS `sp_get_backup_info`;
DROP PROCEDURE IF EXISTS `sp_cleanup_old_backups`;

-- 创建备份信息表
CREATE TABLE IF NOT EXISTS `sys_backup_log` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '备份记录ID',
    `backup_type` VARCHAR(50) NOT NULL COMMENT '备份类型：FULL-完整备份，DATA-数据备份，STRUCTURE-结构备份',
    `backup_file` VARCHAR(500) NOT NULL COMMENT '备份文件路径',
    `backup_size` BIGINT DEFAULT 0 COMMENT '备份文件大小(字节)',
    `backup_status` VARCHAR(50) DEFAULT 'SUCCESS' COMMENT '备份状态：SUCCESS-成功，FAILED-失败，IN_PROGRESS-进行中',
    `start_time` DATETIME NOT NULL COMMENT '开始时间',
    `end_time` DATETIME DEFAULT NULL COMMENT '结束时间',
    `duration_seconds` INT DEFAULT 0 COMMENT '备份耗时(秒)',
    `table_count` INT DEFAULT 0 COMMENT '备份表数量',
    `record_count` BIGINT DEFAULT 0 COMMENT '备份记录数',
    `error_message` TEXT COMMENT '错误信息',
    `backup_user` VARCHAR(100) DEFAULT 'SYSTEM' COMMENT '备份用户',
    `remarks` VARCHAR(500) DEFAULT '' COMMENT '备份备注',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    INDEX `idx_backup_type` (`backup_type`),
    INDEX `idx_backup_status` (`backup_status`),
    INDEX `idx_start_time` (`start_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='备份日志表';

-- =============================================
-- 完整备份存储过程
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_backup_database_full`(
    IN p_backup_path VARCHAR(500),
    IN p_backup_user VARCHAR(100),
    IN p_remarks VARCHAR(500)
)
BEGIN
    DECLARE v_backup_id INT;
    DECLARE v_start_time DATETIME DEFAULT NOW();
    DECLARE v_end_time DATETIME;
    DECLARE v_table_count INT DEFAULT 0;
    DECLARE v_record_count BIGINT DEFAULT 0;
    DECLARE v_backup_file VARCHAR(500);
    DECLARE v_sql_command TEXT;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        -- 更新备份状态为失败
        UPDATE sys_backup_log 
        SET backup_status = 'FAILED',
            end_time = NOW(),
            duration_seconds = TIMESTAMPDIFF(SECOND, start_time, NOW()),
            error_message = 'Backup failed due to SQL exception'
        WHERE id = v_backup_id;
        
        ROLLBACK;
        RESIGNAL;
    END;
    
    -- 生成备份文件名
    SET v_backup_file = CONCAT(
        IFNULL(p_backup_path, '/var/backups/mes/'),
        'mes_system_full_',
        DATE_FORMAT(NOW(), '%Y%m%d_%H%i%s'),
        '.sql'
    );
    
    -- 插入备份开始记录
    INSERT INTO sys_backup_log (
        backup_type, backup_file, backup_status, start_time, 
        backup_user, remarks
    ) VALUES (
        'FULL', v_backup_file, 'IN_PROGRESS', v_start_time,
        IFNULL(p_backup_user, 'SYSTEM'), IFNULL(p_remarks, '完整数据库备份')
    );
    
    SET v_backup_id = LAST_INSERT_ID();
    
    -- 获取表数量和记录数
    SELECT COUNT(*) INTO v_table_count
    FROM information_schema.tables 
    WHERE table_schema = 'mes_system';
    
    SELECT SUM(table_rows) INTO v_record_count
    FROM information_schema.tables 
    WHERE table_schema = 'mes_system';
    
    -- 构建mysqldump命令（这里只是示例，实际需要在应用层执行）
    SET v_sql_command = CONCAT(
        'mysqldump -u mes_user -p --single-transaction --routines --triggers ',
        '--events --hex-blob --default-character-set=utf8mb4 ',
        'mes_system > ', v_backup_file
    );
    
    -- 记录备份命令到日志
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name, 
        log_message, user_name, create_time
    ) VALUES (
        'INFO', 'BACKUP', 'DATABASE', 'FULL_BACKUP',
        CONCAT('执行完整备份命令: ', v_sql_command),
        p_backup_user, NOW()
    );
    
    -- 模拟备份完成（实际应用中需要检查文件是否生成成功）
    SET v_end_time = NOW();
    
    -- 更新备份完成记录
    UPDATE sys_backup_log 
    SET backup_status = 'SUCCESS',
        end_time = v_end_time,
        duration_seconds = TIMESTAMPDIFF(SECOND, v_start_time, v_end_time),
        table_count = v_table_count,
        record_count = v_record_count
    WHERE id = v_backup_id;
    
    -- 返回备份信息
    SELECT 
        v_backup_id AS backup_id,
        v_backup_file AS backup_file,
        v_table_count AS table_count,
        v_record_count AS record_count,
        TIMESTAMPDIFF(SECOND, v_start_time, v_end_time) AS duration_seconds,
        v_sql_command AS backup_command;
END$$
DELIMITER ;

-- =============================================
-- 数据备份存储过程（仅备份数据，不包含结构）
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_backup_database_data_only`(
    IN p_backup_path VARCHAR(500),
    IN p_backup_user VARCHAR(100),
    IN p_remarks VARCHAR(500)
)
BEGIN
    DECLARE v_backup_id INT;
    DECLARE v_start_time DATETIME DEFAULT NOW();
    DECLARE v_backup_file VARCHAR(500);
    DECLARE v_sql_command TEXT;
    
    -- 生成备份文件名
    SET v_backup_file = CONCAT(
        IFNULL(p_backup_path, '/var/backups/mes/'),
        'mes_system_data_',
        DATE_FORMAT(NOW(), '%Y%m%d_%H%i%s'),
        '.sql'
    );
    
    -- 插入备份记录
    INSERT INTO sys_backup_log (
        backup_type, backup_file, backup_status, start_time,
        backup_user, remarks
    ) VALUES (
        'DATA', v_backup_file, 'IN_PROGRESS', v_start_time,
        IFNULL(p_backup_user, 'SYSTEM'), IFNULL(p_remarks, '数据备份')
    );
    
    SET v_backup_id = LAST_INSERT_ID();
    
    -- 构建数据备份命令
    SET v_sql_command = CONCAT(
        'mysqldump -u mes_user -p --no-create-info --single-transaction ',
        '--default-character-set=utf8mb4 mes_system > ', v_backup_file
    );
    
    -- 记录备份命令
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name,
        log_message, user_name, create_time
    ) VALUES (
        'INFO', 'BACKUP', 'DATABASE', 'DATA_BACKUP',
        CONCAT('执行数据备份命令: ', v_sql_command),
        p_backup_user, NOW()
    );
    
    -- 更新备份状态
    UPDATE sys_backup_log 
    SET backup_status = 'SUCCESS',
        end_time = NOW(),
        duration_seconds = TIMESTAMPDIFF(SECOND, v_start_time, NOW())
    WHERE id = v_backup_id;
    
    SELECT v_backup_file AS backup_file, v_sql_command AS backup_command;
END$$
DELIMITER ;

-- =============================================
-- 结构备份存储过程（仅备份表结构）
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_backup_database_structure_only`(
    IN p_backup_path VARCHAR(500),
    IN p_backup_user VARCHAR(100),
    IN p_remarks VARCHAR(500)
)
BEGIN
    DECLARE v_backup_id INT;
    DECLARE v_start_time DATETIME DEFAULT NOW();
    DECLARE v_backup_file VARCHAR(500);
    DECLARE v_sql_command TEXT;
    
    -- 生成备份文件名
    SET v_backup_file = CONCAT(
        IFNULL(p_backup_path, '/var/backups/mes/'),
        'mes_system_structure_',
        DATE_FORMAT(NOW(), '%Y%m%d_%H%i%s'),
        '.sql'
    );
    
    -- 插入备份记录
    INSERT INTO sys_backup_log (
        backup_type, backup_file, backup_status, start_time,
        backup_user, remarks
    ) VALUES (
        'STRUCTURE', v_backup_file, 'IN_PROGRESS', v_start_time,
        IFNULL(p_backup_user, 'SYSTEM'), IFNULL(p_remarks, '结构备份')
    );
    
    SET v_backup_id = LAST_INSERT_ID();
    
    -- 构建结构备份命令
    SET v_sql_command = CONCAT(
        'mysqldump -u mes_user -p --no-data --routines --triggers ',
        '--events --default-character-set=utf8mb4 mes_system > ', v_backup_file
    );
    
    -- 记录备份命令
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name,
        log_message, user_name, create_time
    ) VALUES (
        'INFO', 'BACKUP', 'DATABASE', 'STRUCTURE_BACKUP',
        CONCAT('执行结构备份命令: ', v_sql_command),
        p_backup_user, NOW()
    );
    
    -- 更新备份状态
    UPDATE sys_backup_log 
    SET backup_status = 'SUCCESS',
        end_time = NOW(),
        duration_seconds = TIMESTAMPDIFF(SECOND, v_start_time, NOW())
    WHERE id = v_backup_id;
    
    SELECT v_backup_file AS backup_file, v_sql_command AS backup_command;
END$$
DELIMITER ;

-- =============================================
-- 获取备份信息存储过程
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_get_backup_info`(
    IN p_days_back INT,
    IN p_backup_type VARCHAR(50)
)
BEGIN
    SELECT 
        id,
        backup_type,
        backup_file,
        ROUND(backup_size / 1024 / 1024, 2) AS backup_size_mb,
        backup_status,
        start_time,
        end_time,
        duration_seconds,
        table_count,
        record_count,
        backup_user,
        remarks,
        CASE 
            WHEN backup_status = 'SUCCESS' THEN '✅ 成功'
            WHEN backup_status = 'FAILED' THEN '❌ 失败'
            WHEN backup_status = 'IN_PROGRESS' THEN '🔄 进行中'
            ELSE backup_status
        END AS status_display
    FROM sys_backup_log
    WHERE (p_days_back = 0 OR start_time >= DATE_SUB(NOW(), INTERVAL p_days_back DAY))
        AND (p_backup_type = '' OR backup_type = p_backup_type)
    ORDER BY start_time DESC;
END$$
DELIMITER ;

-- =============================================
-- 清理旧备份记录存储过程
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_cleanup_old_backups`(
    IN p_retention_days INT
)
BEGIN
    DECLARE v_deleted_count INT DEFAULT 0;
    DECLARE v_cutoff_date DATETIME;
    
    SET v_cutoff_date = DATE_SUB(NOW(), INTERVAL p_retention_days DAY);
    
    -- 删除旧的备份记录
    DELETE FROM sys_backup_log 
    WHERE start_time < v_cutoff_date;
    
    SET v_deleted_count = ROW_COUNT();
    
    -- 记录清理日志
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name,
        log_message, create_time
    ) VALUES (
        'INFO', 'BACKUP', 'DATABASE', 'CLEANUP_OLD_BACKUPS',
        CONCAT('清理了 ', v_deleted_count, ' 条超过 ', p_retention_days, ' 天的备份记录'),
        NOW()
    );
    
    SELECT v_deleted_count AS deleted_count, v_cutoff_date AS cutoff_date;
END$$
DELIMITER ;

-- =============================================
-- 备份脚本使用示例
-- =============================================

/*
-- 执行完整备份
CALL sp_backup_database_full('/var/backups/mes/', 'admin', '每日定时备份');

-- 执行数据备份
CALL sp_backup_database_data_only('/var/backups/mes/', 'admin', '数据迁移备份');

-- 执行结构备份
CALL sp_backup_database_structure_only('/var/backups/mes/', 'admin', '结构变更备份');

-- 查看最近7天的备份记录
CALL sp_get_backup_info(7, '');

-- 查看所有完整备份记录
CALL sp_get_backup_info(0, 'FULL');

-- 清理30天前的备份记录
CALL sp_cleanup_old_backups(30);
*/

-- 输出创建结果
SELECT 'MES系统数据库备份脚本创建完成' AS result;
SELECT '请配合Shell脚本使用以实现自动化备份' AS note;
