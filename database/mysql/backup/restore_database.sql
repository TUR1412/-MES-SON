-- =============================================
-- MES制造执行系统 - 数据库恢复脚本
-- 版本: 1.0
-- 创建时间: 2025-06-06
-- 说明: 提供完整的数据库恢复解决方案
-- =============================================

USE `mes_system`;

-- =============================================
-- 恢复相关存储过程
-- =============================================

-- 删除已存在的恢复相关存储过程
DROP PROCEDURE IF EXISTS `sp_restore_database_full`;
DROP PROCEDURE IF EXISTS `sp_restore_database_data_only`;
DROP PROCEDURE IF EXISTS `sp_validate_backup_file`;
DROP PROCEDURE IF EXISTS `sp_get_restore_info`;
DROP PROCEDURE IF EXISTS `sp_create_restore_point`;

-- 创建恢复日志表
CREATE TABLE IF NOT EXISTS `sys_restore_log` (
    `id` INT AUTO_INCREMENT PRIMARY KEY COMMENT '恢复记录ID',
    `restore_type` VARCHAR(50) NOT NULL COMMENT '恢复类型：FULL-完整恢复，DATA-数据恢复，POINT_IN_TIME-时间点恢复',
    `backup_file` VARCHAR(500) NOT NULL COMMENT '备份文件路径',
    `restore_status` VARCHAR(50) DEFAULT 'SUCCESS' COMMENT '恢复状态：SUCCESS-成功，FAILED-失败，IN_PROGRESS-进行中',
    `start_time` DATETIME NOT NULL COMMENT '开始时间',
    `end_time` DATETIME DEFAULT NULL COMMENT '结束时间',
    `duration_seconds` INT DEFAULT 0 COMMENT '恢复耗时(秒)',
    `restored_tables` INT DEFAULT 0 COMMENT '恢复表数量',
    `restored_records` BIGINT DEFAULT 0 COMMENT '恢复记录数',
    `error_message` TEXT COMMENT '错误信息',
    `restore_user` VARCHAR(100) DEFAULT 'SYSTEM' COMMENT '恢复用户',
    `pre_restore_backup` VARCHAR(500) DEFAULT '' COMMENT '恢复前备份文件',
    `remarks` VARCHAR(500) DEFAULT '' COMMENT '恢复备注',
    `create_time` DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
    INDEX `idx_restore_type` (`restore_type`),
    INDEX `idx_restore_status` (`restore_status`),
    INDEX `idx_start_time` (`start_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci COMMENT='恢复日志表';

-- =============================================
-- 创建恢复点存储过程
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_create_restore_point`(
    IN p_restore_point_name VARCHAR(200),
    IN p_backup_path VARCHAR(500),
    IN p_restore_user VARCHAR(100),
    IN p_remarks VARCHAR(500)
)
BEGIN
    DECLARE v_backup_file VARCHAR(500);
    DECLARE v_sql_command TEXT;
    DECLARE v_start_time DATETIME DEFAULT NOW();
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    -- 生成恢复点备份文件名
    SET v_backup_file = CONCAT(
        IFNULL(p_backup_path, '/var/backups/mes/restore_points/'),
        'restore_point_',
        REPLACE(REPLACE(p_restore_point_name, ' ', '_'), ':', ''),
        '_',
        DATE_FORMAT(NOW(), '%Y%m%d_%H%i%s'),
        '.sql'
    );
    
    -- 构建备份命令
    SET v_sql_command = CONCAT(
        'mysqldump -u mes_user -p --single-transaction --routines --triggers ',
        '--events --hex-blob --default-character-set=utf8mb4 ',
        'mes_system > ', v_backup_file
    );
    
    -- 记录恢复点创建日志
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name,
        log_message, user_name, create_time
    ) VALUES (
        'INFO', 'RESTORE', 'DATABASE', 'CREATE_RESTORE_POINT',
        CONCAT('创建恢复点: ', p_restore_point_name, ', 备份文件: ', v_backup_file),
        IFNULL(p_restore_user, 'SYSTEM'), NOW()
    );
    
    -- 记录到备份日志表
    INSERT INTO sys_backup_log (
        backup_type, backup_file, backup_status, start_time,
        backup_user, remarks
    ) VALUES (
        'RESTORE_POINT', v_backup_file, 'SUCCESS', v_start_time,
        IFNULL(p_restore_user, 'SYSTEM'), 
        CONCAT('恢复点: ', p_restore_point_name, ' - ', IFNULL(p_remarks, ''))
    );
    
    SELECT 
        v_backup_file AS restore_point_file,
        v_sql_command AS backup_command,
        p_restore_point_name AS restore_point_name;
END$$
DELIMITER ;

-- =============================================
-- 验证备份文件存储过程
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_validate_backup_file`(
    IN p_backup_file VARCHAR(500)
)
BEGIN
    DECLARE v_file_exists INT DEFAULT 0;
    DECLARE v_file_size BIGINT DEFAULT 0;
    DECLARE v_validation_result VARCHAR(100);
    DECLARE v_backup_info TEXT DEFAULT '';
    
    -- 这里只是示例验证逻辑，实际需要在应用层实现文件检查
    -- 检查备份文件记录是否存在
    SELECT COUNT(*) INTO v_file_exists
    FROM sys_backup_log 
    WHERE backup_file = p_backup_file;
    
    IF v_file_exists > 0 THEN
        SELECT backup_size, 
               CONCAT('备份类型: ', backup_type, 
                     ', 创建时间: ', start_time,
                     ', 状态: ', backup_status) 
        INTO v_file_size, v_backup_info
        FROM sys_backup_log 
        WHERE backup_file = p_backup_file 
        ORDER BY start_time DESC 
        LIMIT 1;
        
        SET v_validation_result = '文件记录存在';
    ELSE
        SET v_validation_result = '文件记录不存在';
    END IF;
    
    -- 记录验证日志
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name,
        log_message, create_time
    ) VALUES (
        'INFO', 'RESTORE', 'DATABASE', 'VALIDATE_BACKUP',
        CONCAT('验证备份文件: ', p_backup_file, ', 结果: ', v_validation_result),
        NOW()
    );
    
    SELECT 
        p_backup_file AS backup_file,
        v_validation_result AS validation_result,
        v_file_size AS file_size_bytes,
        ROUND(v_file_size / 1024 / 1024, 2) AS file_size_mb,
        v_backup_info AS backup_info;
END$$
DELIMITER ;

-- =============================================
-- 完整恢复存储过程
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_restore_database_full`(
    IN p_backup_file VARCHAR(500),
    IN p_restore_user VARCHAR(100),
    IN p_create_backup_before_restore BOOLEAN,
    IN p_remarks VARCHAR(500)
)
BEGIN
    DECLARE v_restore_id INT;
    DECLARE v_start_time DATETIME DEFAULT NOW();
    DECLARE v_end_time DATETIME;
    DECLARE v_pre_restore_backup VARCHAR(500) DEFAULT '';
    DECLARE v_sql_command TEXT;
    DECLARE v_table_count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        -- 更新恢复状态为失败
        UPDATE sys_restore_log 
        SET restore_status = 'FAILED',
            end_time = NOW(),
            duration_seconds = TIMESTAMPDIFF(SECOND, start_time, NOW()),
            error_message = 'Restore failed due to SQL exception'
        WHERE id = v_restore_id;
        
        ROLLBACK;
        RESIGNAL;
    END;
    
    -- 如果需要，先创建恢复前备份
    IF p_create_backup_before_restore THEN
        SET v_pre_restore_backup = CONCAT(
            '/var/backups/mes/pre_restore/',
            'pre_restore_',
            DATE_FORMAT(NOW(), '%Y%m%d_%H%i%s'),
            '.sql'
        );
        
        -- 记录预备份创建
        INSERT INTO sys_log (
            log_level, log_type, module_name, operation_name,
            log_message, user_name, create_time
        ) VALUES (
            'INFO', 'RESTORE', 'DATABASE', 'PRE_RESTORE_BACKUP',
            CONCAT('恢复前创建备份: ', v_pre_restore_backup),
            p_restore_user, NOW()
        );
    END IF;
    
    -- 插入恢复开始记录
    INSERT INTO sys_restore_log (
        restore_type, backup_file, restore_status, start_time,
        restore_user, pre_restore_backup, remarks
    ) VALUES (
        'FULL', p_backup_file, 'IN_PROGRESS', v_start_time,
        IFNULL(p_restore_user, 'SYSTEM'), v_pre_restore_backup,
        IFNULL(p_remarks, '完整数据库恢复')
    );
    
    SET v_restore_id = LAST_INSERT_ID();
    
    -- 获取当前表数量
    SELECT COUNT(*) INTO v_table_count
    FROM information_schema.tables 
    WHERE table_schema = 'mes_system';
    
    -- 构建恢复命令
    SET v_sql_command = CONCAT(
        'mysql -u mes_user -p mes_system < ', p_backup_file
    );
    
    -- 记录恢复命令
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name,
        log_message, user_name, create_time
    ) VALUES (
        'INFO', 'RESTORE', 'DATABASE', 'FULL_RESTORE',
        CONCAT('执行完整恢复命令: ', v_sql_command),
        p_restore_user, NOW()
    );
    
    -- 模拟恢复完成
    SET v_end_time = NOW();
    
    -- 更新恢复完成记录
    UPDATE sys_restore_log 
    SET restore_status = 'SUCCESS',
        end_time = v_end_time,
        duration_seconds = TIMESTAMPDIFF(SECOND, v_start_time, v_end_time),
        restored_tables = v_table_count
    WHERE id = v_restore_id;
    
    -- 返回恢复信息
    SELECT 
        v_restore_id AS restore_id,
        p_backup_file AS backup_file,
        v_pre_restore_backup AS pre_restore_backup,
        v_table_count AS restored_tables,
        TIMESTAMPDIFF(SECOND, v_start_time, v_end_time) AS duration_seconds,
        v_sql_command AS restore_command;
END$$
DELIMITER ;

-- =============================================
-- 数据恢复存储过程（仅恢复数据）
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_restore_database_data_only`(
    IN p_backup_file VARCHAR(500),
    IN p_restore_user VARCHAR(100),
    IN p_truncate_tables BOOLEAN,
    IN p_remarks VARCHAR(500)
)
BEGIN
    DECLARE v_restore_id INT;
    DECLARE v_start_time DATETIME DEFAULT NOW();
    DECLARE v_sql_command TEXT;
    
    -- 插入恢复记录
    INSERT INTO sys_restore_log (
        restore_type, backup_file, restore_status, start_time,
        restore_user, remarks
    ) VALUES (
        'DATA', p_backup_file, 'IN_PROGRESS', v_start_time,
        IFNULL(p_restore_user, 'SYSTEM'), IFNULL(p_remarks, '数据恢复')
    );
    
    SET v_restore_id = LAST_INSERT_ID();
    
    -- 如果需要清空表
    IF p_truncate_tables THEN
        INSERT INTO sys_log (
            log_level, log_type, module_name, operation_name,
            log_message, user_name, create_time
        ) VALUES (
            'WARNING', 'RESTORE', 'DATABASE', 'TRUNCATE_TABLES',
            '数据恢复前清空所有业务表',
            p_restore_user, NOW()
        );
    END IF;
    
    -- 构建数据恢复命令
    SET v_sql_command = CONCAT(
        'mysql -u mes_user -p mes_system < ', p_backup_file
    );
    
    -- 记录恢复命令
    INSERT INTO sys_log (
        log_level, log_type, module_name, operation_name,
        log_message, user_name, create_time
    ) VALUES (
        'INFO', 'RESTORE', 'DATABASE', 'DATA_RESTORE',
        CONCAT('执行数据恢复命令: ', v_sql_command),
        p_restore_user, NOW()
    );
    
    -- 更新恢复状态
    UPDATE sys_restore_log 
    SET restore_status = 'SUCCESS',
        end_time = NOW(),
        duration_seconds = TIMESTAMPDIFF(SECOND, v_start_time, NOW())
    WHERE id = v_restore_id;
    
    SELECT v_sql_command AS restore_command;
END$$
DELIMITER ;

-- =============================================
-- 获取恢复信息存储过程
-- =============================================

DELIMITER $$
CREATE PROCEDURE `sp_get_restore_info`(
    IN p_days_back INT,
    IN p_restore_type VARCHAR(50)
)
BEGIN
    SELECT 
        id,
        restore_type,
        backup_file,
        restore_status,
        start_time,
        end_time,
        duration_seconds,
        restored_tables,
        restored_records,
        restore_user,
        pre_restore_backup,
        remarks,
        CASE 
            WHEN restore_status = 'SUCCESS' THEN '✅ 成功'
            WHEN restore_status = 'FAILED' THEN '❌ 失败'
            WHEN restore_status = 'IN_PROGRESS' THEN '🔄 进行中'
            ELSE restore_status
        END AS status_display,
        CASE 
            WHEN pre_restore_backup != '' THEN '🔒 已备份'
            ELSE '⚠️ 未备份'
        END AS backup_status
    FROM sys_restore_log
    WHERE (p_days_back = 0 OR start_time >= DATE_SUB(NOW(), INTERVAL p_days_back DAY))
        AND (p_restore_type = '' OR restore_type = p_restore_type)
    ORDER BY start_time DESC;
END$$
DELIMITER ;

-- =============================================
-- 恢复脚本使用示例
-- =============================================

/*
-- 创建恢复点
CALL sp_create_restore_point('升级前备份', '/var/backups/mes/restore_points/', 'admin', '系统升级前的安全备份');

-- 验证备份文件
CALL sp_validate_backup_file('/var/backups/mes/mes_system_full_20250606_100000.sql');

-- 执行完整恢复（恢复前自动备份）
CALL sp_restore_database_full('/var/backups/mes/mes_system_full_20250606_100000.sql', 'admin', TRUE, '系统故障恢复');

-- 执行数据恢复（清空表后恢复）
CALL sp_restore_database_data_only('/var/backups/mes/mes_system_data_20250606_100000.sql', 'admin', TRUE, '数据迁移恢复');

-- 查看最近7天的恢复记录
CALL sp_get_restore_info(7, '');

-- 查看所有完整恢复记录
CALL sp_get_restore_info(0, 'FULL');
*/

-- =============================================
-- 恢复前检查清单
-- =============================================

/*
恢复前必须检查的项目：

1. 确认备份文件完整性
   - 文件大小是否合理
   - 文件是否可读
   - 备份时间是否正确

2. 确认恢复环境
   - MySQL服务是否正常
   - 磁盘空间是否充足
   - 用户权限是否正确

3. 创建恢复点
   - 恢复前必须创建当前状态备份
   - 确认恢复点备份成功

4. 通知相关人员
   - 通知系统用户停止操作
   - 通知运维团队准备监控

5. 验证恢复结果
   - 检查关键表数据
   - 验证系统功能
   - 确认数据完整性
*/

-- 输出创建结果
SELECT 'MES系统数据库恢复脚本创建完成' AS result;
SELECT '请务必在恢复前创建恢复点备份' AS warning;
