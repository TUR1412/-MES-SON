using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Workshop;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Workshop
{
    /// <summary>
    /// 设备数据访问层
    /// 提供设备信息的数据库操作功能
    /// </summary>
    public class EquipmentDAL
    {
        #region 基本CRUD操作

        /// <summary>
        /// 获取所有设备信息
        /// </summary>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetAllEquipments()
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name 
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.is_deleted = 0 AND e.is_enabled = 1
                    ORDER BY e.workshop_id, e.equipment_code";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                return ConvertDataTableToEquipmentList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有设备信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据ID获取设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>设备信息</returns>
        public EquipmentStatusInfo GetEquipmentById(int id)
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name 
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.id = @id AND e.is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                var equipmentList = ConvertDataTableToEquipmentList(dataTable);
                return equipmentList.Count > 0 ? equipmentList[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取设备信息失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据设备编码获取信息
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备信息</returns>
        public EquipmentStatusInfo GetEquipmentByCode(string equipmentCode)
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name 
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.equipment_code = @equipmentCode AND e.is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipmentCode)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                var equipmentList = ConvertDataTableToEquipmentList(dataTable);
                return equipmentList.Count > 0 ? equipmentList[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据编码获取设备信息失败：编码={0}", equipmentCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 添加设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否成功</returns>
        public bool AddEquipment(EquipmentStatusInfo equipment)
        {
            try
            {
                string sql = @"
                    INSERT INTO equipment_info (
                        equipment_code, equipment_name, equipment_type, equipment_type_id,
                        workshop_id, location, status, efficiency, temperature, pressure,
                        speed, power, vibration, last_maintenance, next_maintenance,
                        maintenance_cycle, operator, operator_id, manufacturer, model,
                        purchase_date, warranty_until, value, is_enabled, remarks,
                        create_time, update_time, create_by, update_by, is_deleted
                    ) VALUES (
                        @equipmentCode, @equipmentName, @equipmentType, @equipmentTypeId,
                        @workshopId, @location, @status, @efficiency, @temperature, @pressure,
                        @speed, @power, @vibration, @lastMaintenance, @nextMaintenance,
                        @maintenanceCycle, @operator, @operatorId, @manufacturer, @model,
                        @purchaseDate, @warrantyUntil, @value, @isEnabled, @remarks,
                        @createTime, @updateTime, @createBy, @updateBy, @isDeleted
                    )";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipment.EquipmentCode),
                    DatabaseHelper.CreateParameter("@equipmentName", equipment.EquipmentName),
                    DatabaseHelper.CreateParameter("@equipmentType", equipment.EquipmentType ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@equipmentTypeId", equipment.EquipmentTypeId),
                    DatabaseHelper.CreateParameter("@workshopId", equipment.WorkshopId),
                    DatabaseHelper.CreateParameter("@location", equipment.Location ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@status", equipment.Status),
                    DatabaseHelper.CreateParameter("@efficiency", equipment.Efficiency),
                    DatabaseHelper.CreateParameter("@temperature", equipment.Temperature),
                    DatabaseHelper.CreateParameter("@pressure", equipment.Pressure),
                    DatabaseHelper.CreateParameter("@speed", equipment.Speed),
                    DatabaseHelper.CreateParameter("@power", equipment.Power),
                    DatabaseHelper.CreateParameter("@vibration", equipment.Vibration),
                    DatabaseHelper.CreateParameter("@lastMaintenance", equipment.LastMaintenance),
                    DatabaseHelper.CreateParameter("@nextMaintenance", equipment.NextMaintenance),
                    DatabaseHelper.CreateParameter("@maintenanceCycle", equipment.MaintenanceCycle),
                    DatabaseHelper.CreateParameter("@operator", equipment.Operator ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@operatorId", equipment.OperatorId ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@manufacturer", equipment.Manufacturer ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@model", equipment.Model ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@purchaseDate", equipment.PurchaseDate ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@warrantyUntil", equipment.WarrantyUntil ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@value", equipment.Value ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@isEnabled", equipment.IsEnabled),
                    DatabaseHelper.CreateParameter("@remarks", equipment.Remarks ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@createTime", equipment.CreateTime),
                    DatabaseHelper.CreateParameter("@updateTime", equipment.UpdateTime),
                    DatabaseHelper.CreateParameter("@createBy", equipment.CreateBy ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateBy", equipment.UpdateBy ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@isDeleted", equipment.IsDeleted)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加设备信息失败：编码={0}", equipment?.EquipmentCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否成功</returns>
        public bool UpdateEquipment(EquipmentStatusInfo equipment)
        {
            try
            {
                string sql = @"
                    UPDATE equipment_info SET
                        equipment_code = @equipmentCode,
                        equipment_name = @equipmentName,
                        equipment_type = @equipmentType,
                        equipment_type_id = @equipmentTypeId,
                        workshop_id = @workshopId,
                        location = @location,
                        status = @status,
                        efficiency = @efficiency,
                        temperature = @temperature,
                        pressure = @pressure,
                        speed = @speed,
                        power = @power,
                        vibration = @vibration,
                        last_maintenance = @lastMaintenance,
                        next_maintenance = @nextMaintenance,
                        maintenance_cycle = @maintenanceCycle,
                        operator = @operator,
                        operator_id = @operatorId,
                        manufacturer = @manufacturer,
                        model = @model,
                        purchase_date = @purchaseDate,
                        warranty_until = @warrantyUntil,
                        value = @value,
                        is_enabled = @isEnabled,
                        remarks = @remarks,
                        update_time = @updateTime,
                        update_by = @updateBy
                    WHERE id = @id AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", equipment.Id),
                    DatabaseHelper.CreateParameter("@equipmentCode", equipment.EquipmentCode),
                    DatabaseHelper.CreateParameter("@equipmentName", equipment.EquipmentName),
                    DatabaseHelper.CreateParameter("@equipmentType", equipment.EquipmentType ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@equipmentTypeId", equipment.EquipmentTypeId),
                    DatabaseHelper.CreateParameter("@workshopId", equipment.WorkshopId),
                    DatabaseHelper.CreateParameter("@location", equipment.Location ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@status", equipment.Status),
                    DatabaseHelper.CreateParameter("@efficiency", equipment.Efficiency),
                    DatabaseHelper.CreateParameter("@temperature", equipment.Temperature),
                    DatabaseHelper.CreateParameter("@pressure", equipment.Pressure),
                    DatabaseHelper.CreateParameter("@speed", equipment.Speed),
                    DatabaseHelper.CreateParameter("@power", equipment.Power),
                    DatabaseHelper.CreateParameter("@vibration", equipment.Vibration),
                    DatabaseHelper.CreateParameter("@lastMaintenance", equipment.LastMaintenance),
                    DatabaseHelper.CreateParameter("@nextMaintenance", equipment.NextMaintenance),
                    DatabaseHelper.CreateParameter("@maintenanceCycle", equipment.MaintenanceCycle),
                    DatabaseHelper.CreateParameter("@operator", equipment.Operator ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@operatorId", equipment.OperatorId ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@manufacturer", equipment.Manufacturer ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@model", equipment.Model ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@purchaseDate", equipment.PurchaseDate ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@warrantyUntil", equipment.WarrantyUntil ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@value", equipment.Value ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@isEnabled", equipment.IsEnabled),
                    DatabaseHelper.CreateParameter("@remarks", equipment.Remarks ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateTime", equipment.UpdateTime),
                    DatabaseHelper.CreateParameter("@updateBy", equipment.UpdateBy ?? (object)DBNull.Value)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新设备信息失败：编码={0}", equipment?.EquipmentCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 删除设备信息（逻辑删除）
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteEquipment(int id)
        {
            try
            {
                string sql = @"
                    UPDATE equipment_info SET 
                        is_deleted = 1,
                        update_time = @updateTime
                    WHERE id = @id";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除设备信息失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        #endregion

        #region 查询和筛选

        /// <summary>
        /// 根据条件查询设备
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <param name="status">状态（可选）</param>
        /// <param name="equipmentType">设备类型（可选）</param>
        /// <param name="keyword">关键词（可选）</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> SearchEquipments(int? workshopId = null, int? status = null,
            int? equipmentType = null, string keyword = null)
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.is_deleted = 0 AND e.is_enabled = 1";

                var parameters = new List<MySqlParameter>();

                if (workshopId.HasValue && workshopId.Value > 0)
                {
                    sql += " AND e.workshop_id = @workshopId";
                    parameters.Add(DatabaseHelper.CreateParameter("@workshopId", workshopId.Value));
                }

                if (status.HasValue)
                {
                    sql += " AND e.status = @status";
                    parameters.Add(DatabaseHelper.CreateParameter("@status", status.Value));
                }

                if (equipmentType.HasValue && equipmentType.Value > 0)
                {
                    sql += " AND e.equipment_type_id = @equipmentType";
                    parameters.Add(DatabaseHelper.CreateParameter("@equipmentType", equipmentType.Value));
                }

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    sql += " AND (e.equipment_code LIKE @keyword OR e.equipment_name LIKE @keyword OR e.operator LIKE @keyword)";
                    parameters.Add(DatabaseHelper.CreateParameter("@keyword", string.Format("%{0}%", keyword)));
                }

                sql += " ORDER BY e.workshop_id, e.equipment_code";

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());
                return ConvertDataTableToEquipmentList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error("查询设备信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据车间获取设备列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsByWorkshop(int workshopId)
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.workshop_id = @workshopId AND e.is_deleted = 0 AND e.is_enabled = 1
                    ORDER BY e.equipment_code";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@workshopId", workshopId)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToEquipmentList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间获取设备信息失败：车间ID={0}", workshopId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据状态获取设备列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsByStatus(int status)
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.status = @status AND e.is_deleted = 0 AND e.is_enabled = 1
                    ORDER BY e.workshop_id, e.equipment_code";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@status", status)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToEquipmentList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取设备信息失败：状态={0}", status), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据设备类型获取设备列表
        /// </summary>
        /// <param name="equipmentType">设备类型</param>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsByType(int equipmentType)
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.equipment_type_id = @equipmentType AND e.is_deleted = 0 AND e.is_enabled = 1
                    ORDER BY e.workshop_id, e.equipment_code";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@equipmentType", equipmentType)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToEquipmentList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据类型获取设备信息失败：类型={0}", equipmentType), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取需要维护的设备列表
        /// </summary>
        /// <returns>设备信息列表</returns>
        public List<EquipmentStatusInfo> GetEquipmentsNeedMaintenance()
        {
            try
            {
                string sql = @"
                    SELECT e.*, ws.workshop_name
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.next_maintenance <= @currentDate AND e.is_deleted = 0 AND e.is_enabled = 1
                    ORDER BY e.next_maintenance, e.workshop_id, e.equipment_code";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@currentDate", DateTime.Now)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToEquipmentList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error("获取需要维护的设备信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        #endregion

        #region 设备控制和参数更新

        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        public bool UpdateEquipmentStatus(string equipmentCode, int newStatus, string operatorName = null)
        {
            try
            {
                string sql = @"
                    UPDATE equipment_info SET
                        status = @newStatus,
                        operator = @operatorName,
                        update_time = @updateTime,
                        update_by = @updateBy
                    WHERE equipment_code = @equipmentCode AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipmentCode),
                    DatabaseHelper.CreateParameter("@newStatus", newStatus),
                    DatabaseHelper.CreateParameter("@operatorName", operatorName ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@updateBy", operatorName ?? (object)DBNull.Value)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新设备状态失败：编码={0}", equipmentCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 更新维护时间
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="maintenanceTime">维护时间</param>
        /// <returns>是否成功</returns>
        public bool UpdateMaintenanceTime(string equipmentCode, DateTime maintenanceTime)
        {
            try
            {
                string sql = @"
                    UPDATE equipment_info SET
                        last_maintenance = @maintenanceTime,
                        next_maintenance = DATE_ADD(@maintenanceTime, INTERVAL maintenance_cycle DAY),
                        update_time = @updateTime
                    WHERE equipment_code = @equipmentCode AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipmentCode),
                    DatabaseHelper.CreateParameter("@maintenanceTime", maintenanceTime),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新维护时间失败：编码={0}", equipmentCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 记录故障信息
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="faultDescription">故障描述</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>是否成功</returns>
        public bool RecordFault(string equipmentCode, string faultDescription, string operatorName = null)
        {
            try
            {
                // 这里可以扩展为记录到故障日志表
                string sql = @"
                    INSERT INTO equipment_fault_log (
                        equipment_code, fault_description, fault_time, operator, create_time
                    ) VALUES (
                        @equipmentCode, @faultDescription, @faultTime, @operator, @createTime
                    )";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipmentCode),
                    DatabaseHelper.CreateParameter("@faultDescription", faultDescription ?? "设备故障"),
                    DatabaseHelper.CreateParameter("@faultTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@operator", operatorName ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@createTime", DateTime.Now)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("记录故障信息失败：编码={0}", equipmentCode), ex);
                // 故障记录失败不影响状态更新，只记录日志
                LogManager.Warning(string.Format("故障记录失败，但状态已更新：编码={0}, 故障描述={1}", equipmentCode, faultDescription));
                return true; // 返回true以免影响主流程
            }
        }

        /// <summary>
        /// 批量更新设备状态
        /// </summary>
        /// <param name="equipmentCodes">设备编码列表</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="operatorName">操作员</param>
        /// <returns>成功更新的数量</returns>
        public int BatchUpdateStatus(List<string> equipmentCodes, int newStatus, string operatorName = null)
        {
            try
            {
                int successCount = 0;
                foreach (var equipmentCode in equipmentCodes)
                {
                    if (UpdateEquipmentStatus(equipmentCode, newStatus, operatorName))
                    {
                        successCount++;
                    }
                }
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量更新设备状态失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 更新设备运行参数
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="efficiency">效率</param>
        /// <param name="temperature">温度</param>
        /// <param name="pressure">压力</param>
        /// <param name="speed">转速</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool UpdateEquipmentParameters(string equipmentCode, decimal? efficiency = null,
            decimal? temperature = null, decimal? pressure = null, decimal? speed = null,
            string updateBy = null)
        {
            try
            {
                var updateFields = new List<string>();
                var parameters = new List<MySqlParameter>
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipmentCode),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@updateBy", updateBy ?? (object)DBNull.Value)
                };

                if (efficiency.HasValue)
                {
                    updateFields.Add("efficiency = @efficiency");
                    parameters.Add(DatabaseHelper.CreateParameter("@efficiency", efficiency.Value));
                }

                if (temperature.HasValue)
                {
                    updateFields.Add("temperature = @temperature");
                    parameters.Add(DatabaseHelper.CreateParameter("@temperature", temperature.Value));
                }

                if (pressure.HasValue)
                {
                    updateFields.Add("pressure = @pressure");
                    parameters.Add(DatabaseHelper.CreateParameter("@pressure", pressure.Value));
                }

                if (speed.HasValue)
                {
                    updateFields.Add("speed = @speed");
                    parameters.Add(DatabaseHelper.CreateParameter("@speed", speed.Value));
                }

                if (updateFields.Count == 0)
                {
                    return true; // 没有需要更新的参数
                }

                string sql = string.Format(@"
                    UPDATE equipment_info SET
                        {0},
                        update_time = @updateTime,
                        update_by = @updateBy
                    WHERE equipment_code = @equipmentCode AND is_deleted = 0",
                    string.Join(", ", updateFields));

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray());
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新设备参数失败：编码={0}", equipmentCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取设备实时参数
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备参数</returns>
        public EquipmentParameters GetEquipmentParameters(string equipmentCode)
        {
            try
            {
                string sql = @"
                    SELECT equipment_code, efficiency, temperature, pressure, speed, power, vibration, update_time
                    FROM equipment_info
                    WHERE equipment_code = @equipmentCode AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipmentCode)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new EquipmentParameters
                    {
                        EquipmentCode = row["equipment_code"].ToString(),
                        Efficiency = Convert.ToDecimal(row["efficiency"]),
                        Temperature = Convert.ToDecimal(row["temperature"]),
                        Pressure = Convert.ToDecimal(row["pressure"]),
                        Speed = Convert.ToDecimal(row["speed"]),
                        Power = Convert.ToDecimal(row["power"]),
                        Vibration = Convert.ToDecimal(row["vibration"]),
                        UpdateTime = Convert.ToDateTime(row["update_time"])
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取设备参数失败：编码={0}", equipmentCode), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 批量更新设备参数（用于实时监控）
        /// </summary>
        /// <param name="parameterUpdates">参数更新列表</param>
        /// <returns>成功更新的数量</returns>
        public int BatchUpdateParameters(List<EquipmentParameterUpdate> parameterUpdates)
        {
            try
            {
                int successCount = 0;
                foreach (var update in parameterUpdates)
                {
                    if (UpdateEquipmentParameters(update.EquipmentCode, update.Efficiency,
                        update.Temperature, update.Pressure, update.Speed, update.UpdateBy))
                    {
                        successCount++;
                    }
                }
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量更新设备参数失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        #endregion

        #region 统计和验证

        /// <summary>
        /// 获取设备统计信息
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <returns>统计信息</returns>
        public EquipmentStatistics GetEquipmentStatistics(int? workshopId = null)
        {
            try
            {
                string sql = @"
                    SELECT
                        COUNT(*) as TotalCount,
                        SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) as RunningCount,
                        SUM(CASE WHEN status = 0 THEN 1 ELSE 0 END) as StoppedCount,
                        SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) as FaultCount,
                        SUM(CASE WHEN status = 3 THEN 1 ELSE 0 END) as MaintenanceCount,
                        AVG(efficiency) as AverageEfficiency,
                        SUM(IFNULL(value, 0)) as TotalValue,
                        SUM(CASE WHEN next_maintenance <= NOW() THEN 1 ELSE 0 END) as MaintenanceNeededCount
                    FROM equipment_info
                    WHERE is_deleted = 0 AND is_enabled = 1";

                var parameters = new List<MySqlParameter>();

                if (workshopId.HasValue && workshopId.Value > 0)
                {
                    sql += " AND workshop_id = @workshopId";
                    parameters.Add(DatabaseHelper.CreateParameter("@workshopId", workshopId.Value));
                }

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());

                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new EquipmentStatistics
                    {
                        TotalCount = Convert.ToInt32(row["TotalCount"]),
                        RunningCount = Convert.ToInt32(row["RunningCount"]),
                        StoppedCount = Convert.ToInt32(row["StoppedCount"]),
                        FaultCount = Convert.ToInt32(row["FaultCount"]),
                        MaintenanceCount = Convert.ToInt32(row["MaintenanceCount"]),
                        AverageEfficiency = row["AverageEfficiency"] != DBNull.Value ? Convert.ToDecimal(row["AverageEfficiency"]) : 0,
                        TotalValue = row["TotalValue"] != DBNull.Value ? Convert.ToDecimal(row["TotalValue"]) : 0,
                        MaintenanceNeededCount = Convert.ToInt32(row["MaintenanceNeededCount"])
                    };
                }

                return new EquipmentStatistics();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取设备统计信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取车间设备分布
        /// </summary>
        /// <returns>车间分布统计</returns>
        public List<WorkshopEquipmentDistribution> GetWorkshopEquipmentDistribution()
        {
            try
            {
                string sql = @"
                    SELECT
                        e.workshop_id as WorkshopId,
                        ws.workshop_name as WorkshopName,
                        COUNT(*) as EquipmentCount,
                        SUM(CASE WHEN e.status = 1 THEN 1 ELSE 0 END) as RunningCount,
                        AVG(e.efficiency) as AverageEfficiency
                    FROM equipment_info e
                    LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                    WHERE e.is_deleted = 0 AND e.is_enabled = 1
                    GROUP BY e.workshop_id, ws.workshop_name
                    ORDER BY EquipmentCount DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                var distribution = new List<WorkshopEquipmentDistribution>();

                foreach (DataRow row in dataTable.Rows)
                {
                    distribution.Add(new WorkshopEquipmentDistribution
                    {
                        WorkshopId = Convert.ToInt32(row["WorkshopId"]),
                        WorkshopName = row["WorkshopName"]?.ToString() ?? "未知车间",
                        EquipmentCount = Convert.ToInt32(row["EquipmentCount"]),
                        RunningCount = Convert.ToInt32(row["RunningCount"]),
                        AverageEfficiency = row["AverageEfficiency"] != DBNull.Value ? Convert.ToDecimal(row["AverageEfficiency"]) : 0
                    });
                }

                return distribution;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取车间设备分布失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取设备状态分布统计
        /// </summary>
        /// <returns>状态分布统计</returns>
        public List<EquipmentStatusDistribution> GetStatusDistribution()
        {
            try
            {
                string sql = @"
                    SELECT
                        status as Status,
                        COUNT(*) as Count,
                        (COUNT(*) * 100.0 / (SELECT COUNT(*) FROM equipment_info WHERE is_deleted = 0 AND is_enabled = 1)) as Percentage
                    FROM equipment_info
                    WHERE is_deleted = 0 AND is_enabled = 1
                    GROUP BY status
                    ORDER BY status";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                var distribution = new List<EquipmentStatusDistribution>();

                var statusNames = new[] { "停止", "运行", "故障", "维护" };

                foreach (DataRow row in dataTable.Rows)
                {
                    var status = Convert.ToInt32(row["Status"]);
                    distribution.Add(new EquipmentStatusDistribution
                    {
                        Status = status,
                        StatusName = status >= 0 && status < statusNames.Length ? statusNames[status] : "未知",
                        Count = Convert.ToInt32(row["Count"]),
                        Percentage = row["Percentage"] != DBNull.Value ? Convert.ToDecimal(row["Percentage"]) : 0
                    });
                }

                return distribution;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取设备状态分布失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取设备效率统计
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>效率统计</returns>
        public List<EquipmentEfficiencyReport> GetEfficiencyReport(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                string sql = @"
                    SELECT
                        equipment_code as EquipmentCode,
                        equipment_name as EquipmentName,
                        AVG(efficiency) as AverageEfficiency,
                        MAX(efficiency) as MaxEfficiency,
                        MIN(efficiency) as MinEfficiency,
                        24 as RunningHours,
                        CURDATE() as ReportDate
                    FROM equipment_info
                    WHERE is_deleted = 0 AND is_enabled = 1";

                var parameters = new List<MySqlParameter>();

                if (startDate.HasValue)
                {
                    sql += " AND update_time >= @startDate";
                    parameters.Add(DatabaseHelper.CreateParameter("@startDate", startDate.Value));
                }

                if (endDate.HasValue)
                {
                    sql += " AND update_time <= @endDate";
                    parameters.Add(DatabaseHelper.CreateParameter("@endDate", endDate.Value));
                }

                sql += " GROUP BY equipment_code, equipment_name ORDER BY AverageEfficiency DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());
                var report = new List<EquipmentEfficiencyReport>();

                foreach (DataRow row in dataTable.Rows)
                {
                    report.Add(new EquipmentEfficiencyReport
                    {
                        EquipmentCode = row["EquipmentCode"].ToString(),
                        EquipmentName = row["EquipmentName"].ToString(),
                        AverageEfficiency = row["AverageEfficiency"] != DBNull.Value ? Convert.ToDecimal(row["AverageEfficiency"]) : 0,
                        MaxEfficiency = row["MaxEfficiency"] != DBNull.Value ? Convert.ToDecimal(row["MaxEfficiency"]) : 0,
                        MinEfficiency = row["MinEfficiency"] != DBNull.Value ? Convert.ToDecimal(row["MinEfficiency"]) : 0,
                        RunningHours = Convert.ToInt32(row["RunningHours"]),
                        ReportDate = Convert.ToDateTime(row["ReportDate"])
                    });
                }

                return report;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取设备效率统计失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 验证设备编码是否唯一
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        public bool IsEquipmentCodeUnique(string equipmentCode, int excludeId = 0)
        {
            try
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM equipment_info
                    WHERE equipment_code = @equipmentCode AND is_deleted = 0";

                var parameters = new List<MySqlParameter>
                {
                    DatabaseHelper.CreateParameter("@equipmentCode", equipmentCode)
                };

                if (excludeId > 0)
                {
                    sql += " AND id != @excludeId";
                    parameters.Add(DatabaseHelper.CreateParameter("@excludeId", excludeId));
                }

                var result = DatabaseHelper.ExecuteScalar(sql, parameters.ToArray());
                return Convert.ToInt32(result) == 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证设备编码唯一性失败：编码={0}", equipmentCode), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证设备是否可以删除
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>是否可以删除</returns>
        public bool CanDeleteEquipment(int id)
        {
            try
            {
                // 检查设备状态，运行中和故障的不能删除
                string sql = @"
                    SELECT status
                    FROM equipment_info
                    WHERE id = @id AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id)
                };

                var result = DatabaseHelper.ExecuteScalar(sql, parameters);
                if (result != null)
                {
                    int status = Convert.ToInt32(result);
                    // 运行中(1)和故障(2)的设备不能删除
                    return status != 1 && status != 2;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证设备是否可删除失败：ID={0}", id), ex);
                return false;
            }
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 将DataTable转换为设备信息列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>设备信息列表</returns>
        private List<EquipmentStatusInfo> ConvertDataTableToEquipmentList(DataTable dataTable)
        {
            var equipmentList = new List<EquipmentStatusInfo>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return equipmentList;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var equipment = new EquipmentStatusInfo
                {
                    Id = Convert.ToInt32(row["id"]),
                    EquipmentCode = row["equipment_code"].ToString(),
                    EquipmentName = row["equipment_name"].ToString(),
                    EquipmentType = row["equipment_type"] != DBNull.Value ? row["equipment_type"].ToString() : null,
                    EquipmentTypeId = Convert.ToInt32(row["equipment_type_id"]),
                    WorkshopId = Convert.ToInt32(row["workshop_id"]),
                    WorkshopName = row["workshop_name"] != DBNull.Value ? row["workshop_name"].ToString() : "",
                    Location = row["location"] != DBNull.Value ? row["location"].ToString() : null,
                    Status = Convert.ToInt32(row["status"]),
                    Efficiency = Convert.ToDecimal(row["efficiency"]),
                    Temperature = Convert.ToDecimal(row["temperature"]),
                    Pressure = Convert.ToDecimal(row["pressure"]),
                    Speed = Convert.ToDecimal(row["speed"]),
                    Power = Convert.ToDecimal(row["power"]),
                    Vibration = Convert.ToDecimal(row["vibration"]),
                    LastMaintenance = Convert.ToDateTime(row["last_maintenance"]),
                    NextMaintenance = Convert.ToDateTime(row["next_maintenance"]),
                    MaintenanceCycle = Convert.ToInt32(row["maintenance_cycle"]),
                    Operator = row["operator"] != DBNull.Value ? row["operator"].ToString() : null,
                    OperatorId = row["operator_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["operator_id"]) : null,
                    Manufacturer = row["manufacturer"] != DBNull.Value ? row["manufacturer"].ToString() : null,
                    Model = row["model"] != DBNull.Value ? row["model"].ToString() : null,
                    PurchaseDate = row["purchase_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["purchase_date"]) : null,
                    WarrantyUntil = row["warranty_until"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["warranty_until"]) : null,
                    Value = row["value"] != DBNull.Value ? (decimal?)Convert.ToDecimal(row["value"]) : null,
                    IsEnabled = Convert.ToBoolean(row["is_enabled"]),
                    Remarks = row["remarks"] != DBNull.Value ? row["remarks"].ToString() : null,
                    CreateTime = Convert.ToDateTime(row["create_time"]),
                    UpdateTime = Convert.ToDateTime(row["update_time"]),
                    CreateBy = row["create_by"] != DBNull.Value ? row["create_by"].ToString() : null,
                    UpdateBy = row["update_by"] != DBNull.Value ? row["update_by"].ToString() : null,
                    IsDeleted = Convert.ToBoolean(row["is_deleted"])
                };

                equipmentList.Add(equipment);
            }

            return equipmentList;
        }

        #endregion
    }
}
