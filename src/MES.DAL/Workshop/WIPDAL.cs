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
    /// 在制品数据访问层
    /// 提供在制品信息的数据库操作功能
    /// </summary>
    public class WIPDAL
    {
        #region 基本CRUD操作

        /// <summary>
        /// 获取所有在制品信息
        /// </summary>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetAllWIPs()
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name 
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.is_deleted = 0
                    ORDER BY w.create_time DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                return ConvertDataTableToWIPList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有在制品信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取在制品（未完成）
        /// </summary>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetActiveWIPs()
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.is_deleted = 0 AND w.status <> 4
                    ORDER BY w.create_time DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                return ConvertDataTableToWIPList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error("获取未完成在制品信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据ID获取在制品信息
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>在制品信息</returns>
        public WIPInfo GetWIPById(int id)
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name 
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.id = @id AND w.is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                var wipList = ConvertDataTableToWIPList(dataTable);
                return wipList.Count > 0 ? wipList[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取在制品信息失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据在制品编号获取信息
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <returns>在制品信息</returns>
        public WIPInfo GetWIPByWIPId(string wipId)
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name 
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.wip_id = @wipId AND w.is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@wipId", wipId)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                var wipList = ConvertDataTableToWIPList(dataTable);
                return wipList.Count > 0 ? wipList[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据编号获取在制品信息失败：编号={0}", wipId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 添加在制品信息
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        /// <returns>是否成功</returns>
        public bool AddWIP(WIPInfo wipInfo)
        {
            try
            {
                string sql = @"
                    INSERT INTO wip_info (
                        wip_id, batch_number, work_order_number, product_id, product_code, product_name,
                        workshop_id, workstation_id, workstation_name, quantity, completed_quantity,
                        status, priority, start_time, estimated_end_time, actual_end_time,
                        unit_price, quality_grade, responsible_person, remarks,
                        create_time, update_time, create_by, update_by, is_deleted
                    ) VALUES (
                        @wipId, @batchNumber, @workOrderNumber, @productId, @productCode, @productName,
                        @workshopId, @workstationId, @workstationName, @quantity, @completedQuantity,
                        @status, @priority, @startTime, @estimatedEndTime, @actualEndTime,
                        @unitPrice, @qualityGrade, @responsiblePerson, @remarks,
                        @createTime, @updateTime, @createBy, @updateBy, @isDeleted
                    )";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@wipId", wipInfo.WIPId),
                    DatabaseHelper.CreateParameter("@batchNumber", wipInfo.BatchNumber ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@workOrderNumber", wipInfo.WorkOrderNumber ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@productId", wipInfo.ProductId),
                    DatabaseHelper.CreateParameter("@productCode", wipInfo.ProductCode),
                    DatabaseHelper.CreateParameter("@productName", wipInfo.ProductName),
                    DatabaseHelper.CreateParameter("@workshopId", wipInfo.WorkshopId),
                    DatabaseHelper.CreateParameter("@workstationId", wipInfo.WorkstationId ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@workstationName", wipInfo.WorkstationName ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@quantity", wipInfo.Quantity),
                    DatabaseHelper.CreateParameter("@completedQuantity", wipInfo.CompletedQuantity),
                    DatabaseHelper.CreateParameter("@status", wipInfo.Status),
                    DatabaseHelper.CreateParameter("@priority", wipInfo.Priority),
                    DatabaseHelper.CreateParameter("@startTime", wipInfo.StartTime),
                    DatabaseHelper.CreateParameter("@estimatedEndTime", wipInfo.EstimatedEndTime),
                    DatabaseHelper.CreateParameter("@actualEndTime", wipInfo.ActualEndTime ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@unitPrice", wipInfo.UnitPrice),
                    DatabaseHelper.CreateParameter("@qualityGrade", wipInfo.QualityGrade ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@responsiblePerson", wipInfo.ResponsiblePerson ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@remarks", wipInfo.Remarks ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@createTime", wipInfo.CreateTime),
                    DatabaseHelper.CreateParameter("@updateTime", wipInfo.UpdateTime),
                    DatabaseHelper.CreateParameter("@createBy", wipInfo.CreateBy ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateBy", wipInfo.UpdateBy ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@isDeleted", wipInfo.IsDeleted)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加在制品信息失败：编号={0}", wipInfo?.WIPId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 更新在制品信息
        /// </summary>
        /// <param name="wipInfo">在制品信息</param>
        /// <returns>是否成功</returns>
        public bool UpdateWIP(WIPInfo wipInfo)
        {
            try
            {
                string sql = @"
                    UPDATE wip_info SET
                        wip_id = @wipId,
                        batch_number = @batchNumber,
                        work_order_number = @workOrderNumber,
                        product_id = @productId,
                        product_code = @productCode,
                        product_name = @productName,
                        workshop_id = @workshopId,
                        workstation_id = @workstationId,
                        workstation_name = @workstationName,
                        quantity = @quantity,
                        completed_quantity = @completedQuantity,
                        status = @status,
                        priority = @priority,
                        start_time = @startTime,
                        estimated_end_time = @estimatedEndTime,
                        actual_end_time = @actualEndTime,
                        unit_price = @unitPrice,
                        quality_grade = @qualityGrade,
                        responsible_person = @responsiblePerson,
                        remarks = @remarks,
                        update_time = @updateTime,
                        update_by = @updateBy
                    WHERE id = @id AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", wipInfo.Id),
                    DatabaseHelper.CreateParameter("@wipId", wipInfo.WIPId),
                    DatabaseHelper.CreateParameter("@batchNumber", wipInfo.BatchNumber ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@workOrderNumber", wipInfo.WorkOrderNumber ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@productId", wipInfo.ProductId),
                    DatabaseHelper.CreateParameter("@productCode", wipInfo.ProductCode),
                    DatabaseHelper.CreateParameter("@productName", wipInfo.ProductName),
                    DatabaseHelper.CreateParameter("@workshopId", wipInfo.WorkshopId),
                    DatabaseHelper.CreateParameter("@workstationId", wipInfo.WorkstationId ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@workstationName", wipInfo.WorkstationName ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@quantity", wipInfo.Quantity),
                    DatabaseHelper.CreateParameter("@completedQuantity", wipInfo.CompletedQuantity),
                    DatabaseHelper.CreateParameter("@status", wipInfo.Status),
                    DatabaseHelper.CreateParameter("@priority", wipInfo.Priority),
                    DatabaseHelper.CreateParameter("@startTime", wipInfo.StartTime),
                    DatabaseHelper.CreateParameter("@estimatedEndTime", wipInfo.EstimatedEndTime),
                    DatabaseHelper.CreateParameter("@actualEndTime", wipInfo.ActualEndTime ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@unitPrice", wipInfo.UnitPrice),
                    DatabaseHelper.CreateParameter("@qualityGrade", wipInfo.QualityGrade ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@responsiblePerson", wipInfo.ResponsiblePerson ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@remarks", wipInfo.Remarks ?? (object)DBNull.Value),
                    DatabaseHelper.CreateParameter("@updateTime", wipInfo.UpdateTime),
                    DatabaseHelper.CreateParameter("@updateBy", wipInfo.UpdateBy ?? (object)DBNull.Value)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新在制品信息失败：编号={0}", wipInfo?.WIPId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 删除在制品信息（逻辑删除）
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>是否成功</returns>
        public bool DeleteWIP(int id)
        {
            try
            {
                string sql = @"
                    UPDATE wip_info SET 
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
                LogManager.Error(string.Format("删除在制品信息失败：ID={0}", id), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        #endregion

        #region 查询和筛选

        /// <summary>
        /// 根据条件查询在制品
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <param name="status">状态（可选）</param>
        /// <param name="startDate">开始日期（可选）</param>
        /// <param name="endDate">结束日期（可选）</param>
        /// <param name="keyword">关键词（可选）</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> SearchWIPs(int? workshopId = null, int? status = null,
            DateTime? startDate = null, DateTime? endDate = null, string keyword = null)
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.is_deleted = 0";

                var parameters = new List<MySqlParameter>();

                if (workshopId.HasValue && workshopId.Value > 0)
                {
                    sql += " AND w.workshop_id = @workshopId";
                    parameters.Add(DatabaseHelper.CreateParameter("@workshopId", workshopId.Value));
                }

                if (status.HasValue)
                {
                    sql += " AND w.status = @status";
                    parameters.Add(DatabaseHelper.CreateParameter("@status", status.Value));
                }

                if (startDate.HasValue)
                {
                    sql += " AND w.start_time >= @startDate";
                    parameters.Add(DatabaseHelper.CreateParameter("@startDate", startDate.Value));
                }

                if (endDate.HasValue)
                {
                    sql += " AND w.start_time <= @endDate";
                    parameters.Add(DatabaseHelper.CreateParameter("@endDate", endDate.Value));
                }

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    sql += " AND (w.wip_id LIKE @keyword OR w.product_code LIKE @keyword OR w.product_name LIKE @keyword OR w.batch_number LIKE @keyword)";
                    parameters.Add(DatabaseHelper.CreateParameter("@keyword", string.Format("%{0}%", keyword)));
                }

                sql += " ORDER BY w.create_time DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());
                return ConvertDataTableToWIPList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error("查询在制品信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据批次号获取在制品列表
        /// </summary>
        /// <param name="batchNumber">批次号</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByBatchNumber(string batchNumber)
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.batch_number = @batchNumber AND w.is_deleted = 0
                    ORDER BY w.create_time DESC";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@batchNumber", batchNumber)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToWIPList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据批次号获取在制品信息失败：批次号={0}", batchNumber), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据工单号获取在制品列表
        /// </summary>
        /// <param name="workOrderNumber">工单号</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByWorkOrderNumber(string workOrderNumber)
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.work_order_number = @workOrderNumber AND w.is_deleted = 0
                    ORDER BY w.create_time DESC";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@workOrderNumber", workOrderNumber)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToWIPList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工单号获取在制品信息失败：工单号={0}", workOrderNumber), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据车间获取在制品列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByWorkshop(int workshopId)
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.workshop_id = @workshopId AND w.is_deleted = 0
                    ORDER BY w.create_time DESC";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@workshopId", workshopId)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToWIPList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间获取在制品信息失败：车间ID={0}", workshopId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 根据状态获取在制品列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>在制品信息列表</returns>
        public List<WIPInfo> GetWIPsByStatus(int status)
        {
            try
            {
                string sql = @"
                    SELECT w.*, ws.workshop_name
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.status = @status AND w.is_deleted = 0
                    ORDER BY w.create_time DESC";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@status", status)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                return ConvertDataTableToWIPList(dataTable);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取在制品信息失败：状态={0}", status), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        #endregion

        #region 状态管理和转移

        /// <summary>
        /// 更新在制品状态
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool UpdateWIPStatus(string wipId, int newStatus, string updateBy = null)
        {
            try
            {
                string sql = @"
                    UPDATE wip_info SET
                        status = @newStatus,
                        update_time = @updateTime,
                        update_by = @updateBy
                    WHERE wip_id = @wipId AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@wipId", wipId),
                    DatabaseHelper.CreateParameter("@newStatus", newStatus),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@updateBy", updateBy ?? (object)DBNull.Value)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新在制品状态失败：编号={0}", wipId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 批量更新在制品状态
        /// </summary>
        /// <param name="wipIds">在制品编号列表</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>成功更新的数量</returns>
        public int BatchUpdateWIPStatus(List<string> wipIds, int newStatus, string updateBy = null)
        {
            try
            {
                int successCount = 0;
                foreach (var wipId in wipIds)
                {
                    if (UpdateWIPStatus(wipId, newStatus, updateBy))
                    {
                        successCount++;
                    }
                }
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量更新在制品状态失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 更新在制品进度
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="completedQuantity">已完成数量</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool UpdateWIPProgress(string wipId, int completedQuantity, string updateBy = null)
        {
            try
            {
                string sql = @"
                    UPDATE wip_info SET
                        completed_quantity = @completedQuantity,
                        update_time = @updateTime,
                        update_by = @updateBy
                    WHERE wip_id = @wipId AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@wipId", wipId),
                    DatabaseHelper.CreateParameter("@completedQuantity", completedQuantity),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@updateBy", updateBy ?? (object)DBNull.Value)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新在制品进度失败：编号={0}", wipId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 转移在制品到新车间
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="newWorkshopId">新车间ID</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>是否成功</returns>
        public bool TransferWIPToWorkshop(string wipId, int newWorkshopId, string updateBy = null)
        {
            try
            {
                string sql = @"
                    UPDATE wip_info SET
                        workshop_id = @newWorkshopId,
                        update_time = @updateTime,
                        update_by = @updateBy
                    WHERE wip_id = @wipId AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@wipId", wipId),
                    DatabaseHelper.CreateParameter("@newWorkshopId", newWorkshopId),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@updateBy", updateBy ?? (object)DBNull.Value)
                };

                int result = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("转移在制品失败：编号={0}", wipId), ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 批量转移在制品
        /// </summary>
        /// <param name="wipIds">在制品编号列表</param>
        /// <param name="newWorkshopId">新车间ID</param>
        /// <param name="updateBy">更新人</param>
        /// <returns>成功转移的数量</returns>
        public int BatchTransferWIPs(List<string> wipIds, int newWorkshopId, string updateBy = null)
        {
            try
            {
                int successCount = 0;
                foreach (var wipId in wipIds)
                {
                    if (TransferWIPToWorkshop(wipId, newWorkshopId, updateBy))
                    {
                        successCount++;
                    }
                }
                return successCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("批量转移在制品失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        #endregion

        #region 统计和验证

        /// <summary>
        /// 获取在制品统计信息
        /// </summary>
        /// <param name="workshopId">车间ID（可选）</param>
        /// <param name="startDate">开始日期（可选）</param>
        /// <param name="endDate">结束日期（可选）</param>
        /// <returns>统计信息</returns>
        public Dictionary<string, object> GetWIPStatistics(int? workshopId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                string sql = @"
                    SELECT
                        COUNT(*) as TotalCount,
                        SUM(quantity * unit_price) as TotalValue,
                        SUM(CASE WHEN status = 0 THEN 1 ELSE 0 END) as PendingCount,
                        SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) as InProgressCount,
                        SUM(CASE WHEN status = 2 THEN 1 ELSE 0 END) as InQCCount,
                        SUM(CASE WHEN status = 3 THEN 1 ELSE 0 END) as PausedCount,
                        SUM(CASE WHEN status = 4 THEN 1 ELSE 0 END) as CompletedCount,
                        AVG(CASE WHEN quantity > 0 THEN (completed_quantity * 100.0 / quantity) ELSE 0 END) as AverageProgress
                    FROM wip_info
                    WHERE is_deleted = 0";

                var parameters = new List<MySqlParameter>();

                if (workshopId.HasValue && workshopId.Value > 0)
                {
                    sql += " AND workshop_id = @workshopId";
                    parameters.Add(DatabaseHelper.CreateParameter("@workshopId", workshopId.Value));
                }

                if (startDate.HasValue)
                {
                    sql += " AND start_time >= @startDate";
                    parameters.Add(DatabaseHelper.CreateParameter("@startDate", startDate.Value));
                }

                if (endDate.HasValue)
                {
                    sql += " AND start_time <= @endDate";
                    parameters.Add(DatabaseHelper.CreateParameter("@endDate", endDate.Value));
                }

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters.ToArray());

                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new Dictionary<string, object>
                    {
                        {"TotalCount", Convert.ToInt32(row["TotalCount"])},
                        {"TotalValue", row["TotalValue"] != DBNull.Value ? Convert.ToDecimal(row["TotalValue"]) : 0},
                        {"PendingCount", Convert.ToInt32(row["PendingCount"])},
                        {"InProgressCount", Convert.ToInt32(row["InProgressCount"])},
                        {"InQCCount", Convert.ToInt32(row["InQCCount"])},
                        {"PausedCount", Convert.ToInt32(row["PausedCount"])},
                        {"CompletedCount", Convert.ToInt32(row["CompletedCount"])},
                        {"AverageProgress", row["AverageProgress"] != DBNull.Value ? Convert.ToDecimal(row["AverageProgress"]) : 0}
                    };
                }

                return new Dictionary<string, object>();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取在制品统计信息失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取车间在制品分布
        /// </summary>
        /// <returns>车间分布统计</returns>
        public List<Dictionary<string, object>> GetWorkshopWIPDistribution()
        {
            try
            {
                string sql = @"
                    SELECT
                        w.workshop_id as WorkshopId,
                        ws.workshop_name as WorkshopName,
                        COUNT(*) as WIPCount,
                        SUM(w.quantity * w.unit_price) as TotalValue
                    FROM wip_info w
                    LEFT JOIN workshop_info ws ON w.workshop_id = ws.id
                    WHERE w.is_deleted = 0
                    GROUP BY w.workshop_id, ws.workshop_name
                    ORDER BY WIPCount DESC";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                var distribution = new List<Dictionary<string, object>>();

                foreach (DataRow row in dataTable.Rows)
                {
                    distribution.Add(new Dictionary<string, object>
                    {
                        {"WorkshopId", Convert.ToInt32(row["WorkshopId"])},
                        {"WorkshopName", row["WorkshopName"]?.ToString() ?? "未知车间"},
                        {"WIPCount", Convert.ToInt32(row["WIPCount"])},
                        {"TotalValue", row["TotalValue"] != DBNull.Value ? Convert.ToDecimal(row["TotalValue"]) : 0}
                    });
                }

                return distribution;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取车间在制品分布失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 获取状态分布统计
        /// </summary>
        /// <returns>状态分布统计</returns>
        public List<Dictionary<string, object>> GetStatusDistribution()
        {
            try
            {
                string sql = @"
                    SELECT
                        status as Status,
                        COUNT(*) as Count,
                        (COUNT(*) * 100.0 / (SELECT COUNT(*) FROM wip_info WHERE is_deleted = 0)) as Percentage
                    FROM wip_info
                    WHERE is_deleted = 0
                    GROUP BY status
                    ORDER BY status";

                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                var distribution = new List<Dictionary<string, object>>();

                var statusNames = new[] { "待开始", "生产中", "质检中", "暂停", "已完成" };

                foreach (DataRow row in dataTable.Rows)
                {
                    var status = Convert.ToInt32(row["Status"]);
                    distribution.Add(new Dictionary<string, object>
                    {
                        {"Status", status},
                        {"StatusName", status >= 0 && status < statusNames.Length ? statusNames[status] : "未知"},
                        {"Count", Convert.ToInt32(row["Count"])},
                        {"Percentage", row["Percentage"] != DBNull.Value ? Convert.ToDecimal(row["Percentage"]) : 0}
                    });
                }

                return distribution;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取状态分布统计失败", ex);
                throw new MESException("数据库操作失败", ex);
            }
        }

        /// <summary>
        /// 验证在制品编号是否唯一
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="excludeId">排除的ID（用于编辑时验证）</param>
        /// <returns>是否唯一</returns>
        public bool IsWIPIdUnique(string wipId, int excludeId = 0)
        {
            try
            {
                string sql = @"
                    SELECT COUNT(*)
                    FROM wip_info
                    WHERE wip_id = @wipId AND is_deleted = 0";

                var parameters = new List<MySqlParameter>
                {
                    DatabaseHelper.CreateParameter("@wipId", wipId)
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
                LogManager.Error(string.Format("验证在制品编号唯一性失败：编号={0}", wipId), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证在制品是否可以删除
        /// </summary>
        /// <param name="id">在制品ID</param>
        /// <returns>是否可以删除</returns>
        public bool CanDeleteWIP(int id)
        {
            try
            {
                // 检查在制品状态，生产中和质检中的不能删除
                string sql = @"
                    SELECT status
                    FROM wip_info
                    WHERE id = @id AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id)
                };

                var result = DatabaseHelper.ExecuteScalar(sql, parameters);
                if (result != null)
                {
                    int status = Convert.ToInt32(result);
                    // 生产中(1)和质检中(2)的在制品不能删除
                    return status != 1 && status != 2;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证在制品是否可删除失败：ID={0}", id), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证在制品是否可以转移
        /// </summary>
        /// <param name="wipId">在制品编号</param>
        /// <param name="targetWorkshopId">目标车间ID</param>
        /// <returns>是否可以转移</returns>
        public bool CanTransferWIP(string wipId, int targetWorkshopId)
        {
            try
            {
                // 检查在制品状态和当前车间
                string sql = @"
                    SELECT status, workshop_id
                    FROM wip_info
                    WHERE wip_id = @wipId AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@wipId", wipId)
                };

                var dataTable = DatabaseHelper.ExecuteQuery(sql, parameters);
                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    int status = Convert.ToInt32(row["status"]);
                    int currentWorkshopId = Convert.ToInt32(row["workshop_id"]);

                    // 已完成的在制品不能转移，且不能转移到相同车间
                    return status != 4 && currentWorkshopId != targetWorkshopId;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("验证在制品是否可转移失败：编号={0}", wipId), ex);
                return false;
            }
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 将DataTable转换为在制品信息列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>在制品信息列表</returns>
        private List<WIPInfo> ConvertDataTableToWIPList(DataTable dataTable)
        {
            var wipList = new List<WIPInfo>();

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return wipList;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var wip = new WIPInfo
                {
                    Id = Convert.ToInt32(row["id"]),
                    WIPId = row["wip_id"].ToString(),
                    BatchNumber = row["batch_number"] != DBNull.Value ? row["batch_number"].ToString() : null,
                    WorkOrderNumber = row["work_order_number"] != DBNull.Value ? row["work_order_number"].ToString() : null,
                    ProductId = Convert.ToInt32(row["product_id"]),
                    ProductCode = row["product_code"].ToString(),
                    ProductName = row["product_name"].ToString(),
                    WorkshopId = Convert.ToInt32(row["workshop_id"]),
                    WorkshopName = row["workshop_name"] != DBNull.Value ? row["workshop_name"].ToString() : "",
                    WorkstationId = row["workstation_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["workstation_id"]) : null,
                    WorkstationName = row["workstation_name"] != DBNull.Value ? row["workstation_name"].ToString() : null,
                    Quantity = Convert.ToInt32(row["quantity"]),
                    CompletedQuantity = Convert.ToInt32(row["completed_quantity"]),
                    Status = Convert.ToInt32(row["status"]),
                    Priority = Convert.ToInt32(row["priority"]),
                    StartTime = Convert.ToDateTime(row["start_time"]),
                    EstimatedEndTime = Convert.ToDateTime(row["estimated_end_time"]),
                    ActualEndTime = row["actual_end_time"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["actual_end_time"]) : null,
                    UnitPrice = Convert.ToDecimal(row["unit_price"]),
                    QualityGrade = row["quality_grade"] != DBNull.Value ? row["quality_grade"].ToString() : null,
                    ResponsiblePerson = row["responsible_person"] != DBNull.Value ? row["responsible_person"].ToString() : null,
                    Remarks = row["remarks"] != DBNull.Value ? row["remarks"].ToString() : null,
                    CreateTime = Convert.ToDateTime(row["create_time"]),
                    UpdateTime = Convert.ToDateTime(row["update_time"]),
                    CreateBy = row["create_by"] != DBNull.Value ? row["create_by"].ToString() : null,
                    UpdateBy = row["update_by"] != DBNull.Value ? row["update_by"].ToString() : null,
                    IsDeleted = Convert.ToBoolean(row["is_deleted"])
                };

                wipList.Add(wip);
            }

            return wipList;
        }

        #endregion
    }
}
