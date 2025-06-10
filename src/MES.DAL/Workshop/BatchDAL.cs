using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Base;
using MES.Models.Workshop;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Workshop
{
    /// <summary>
    /// 批次信息数据访问类
    /// 提供批次相关的数据库操作功能
    /// </summary>
    public class BatchDAL : BaseDAL<BatchInfo>
    {
        #region 基类实现

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "batch_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "Id"; }
        }

        /// <summary>
        /// 将DataRow转换为BatchInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>BatchInfo实体对象</returns>
        protected override BatchInfo MapRowToEntity(DataRow row)
        {
            return new BatchInfo
            {
                Id = Convert.ToInt32(row["id"]),
                BatchNumber = row["batch_number"] != DBNull.Value ? row["batch_number"].ToString() : null,
                WorkshopId = row["workshop_id"] != DBNull.Value ? Convert.ToInt32(row["workshop_id"]) : 0,
                WorkshopName = row["workshop_name"] != DBNull.Value ? row["workshop_name"].ToString() : null,
                ProductionOrderId = row["production_order_id"] != DBNull.Value ? Convert.ToInt32(row["production_order_id"]) : 0,
                ProductionOrderNumber = row["production_order_number"] != DBNull.Value ? row["production_order_number"].ToString() : null,
                ProductCode = row["product_code"] != DBNull.Value ? row["product_code"].ToString() : null,
                ProductName = row["product_name"] != DBNull.Value ? row["product_name"].ToString() : null,
                PlannedQuantity = row["planned_quantity"] != DBNull.Value ? Convert.ToDecimal(row["planned_quantity"]) : 0,
                ActualQuantity = row["actual_quantity"] != DBNull.Value ? Convert.ToDecimal(row["actual_quantity"]) : 0,
                Unit = row["unit"] != DBNull.Value ? row["unit"].ToString() : "个",
                BatchStatus = row["batch_status"] != DBNull.Value ? row["batch_status"].ToString() : "待开始",
                ProductionStartTime = row["start_time"] != DBNull.Value ? Convert.ToDateTime(row["start_time"]) : (DateTime?)null,
                ProductionEndTime = row["end_time"] != DBNull.Value ? Convert.ToDateTime(row["end_time"]) : (DateTime?)null,
                OperatorId = row["operator_id"] != DBNull.Value ? Convert.ToInt32(row["operator_id"]) : 0,
                OperatorName = row["operator_name"] != DBNull.Value ? row["operator_name"].ToString() : null,
                Remarks = row["remarks"] != DBNull.Value ? row["remarks"].ToString() : null,
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserId = row["create_user_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["create_user_id"]) : null,
                CreateUserName = row["create_user_name"] != DBNull.Value ? row["create_user_name"].ToString() : null,
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                UpdateUserId = row["update_user_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["update_user_id"]) : null,
                UpdateUserName = row["update_user_name"] != DBNull.Value ? row["update_user_name"].ToString() : null,
                IsDeleted = Convert.ToBoolean(row["is_deleted"]),
                DeleteTime = row["delete_time"] != DBNull.Value ? Convert.ToDateTime(row["delete_time"]) : (DateTime?)null,
                DeleteUserId = row["delete_user_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["delete_user_id"]) : null,
                DeleteUserName = row["delete_user_name"] != DBNull.Value ? row["delete_user_name"].ToString() : null,
                Remark = row["remark"] != DBNull.Value ? row["remark"].ToString() : null,
                Version = row["version"] != DBNull.Value ? Convert.ToInt32(row["version"]) : 1
            };
        }

        #endregion

        #region 批次特有操作

        /// <summary>
        /// 根据批次编号获取批次信息
        /// </summary>
        /// <param name="batchNumber">批次编号</param>
        /// <returns>批次信息</returns>
        public BatchInfo GetByBatchNumber(string batchNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(batchNumber))
                {
                    throw new ArgumentException("批次编号不能为空", "batchNumber");
                }

                var batches = GetByCondition("batch_number = @batchNumber",
                    DatabaseHelper.CreateParameter("@batchNumber", batchNumber));

                return batches.Count > 0 ? batches[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据批次编号获取批次信息失败，批次编号: {0}", batchNumber), ex);
                throw new MESException("获取批次信息失败", ex);
            }
        }

        /// <summary>
        /// 根据批次编号获取批次信息 - 兼容方法
        /// </summary>
        /// <param name="batchId">批次编号</param>
        /// <returns>批次信息</returns>
        public BatchInfo GetByBatchId(string batchId)
        {
            return GetByBatchNumber(batchId);
        }

        /// <summary>
        /// 根据生产订单ID获取批次列表
        /// </summary>
        /// <param name="productionOrderId">生产订单ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByProductionOrderId(int productionOrderId)
        {
            try
            {
                if (productionOrderId <= 0)
                {
                    return new List<BatchInfo>();
                }

                return GetByCondition("production_order_id = @productionOrderId",
                    DatabaseHelper.CreateParameter("@productionOrderId", productionOrderId));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据生产订单ID获取批次列表失败，订单ID: {0}", productionOrderId), ex);
                throw new MESException("获取批次列表失败", ex);
            }
        }

        /// <summary>
        /// 根据车间ID获取批次列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByWorkshopId(int workshopId)
        {
            try
            {
                if (workshopId <= 0)
                {
                    return new List<BatchInfo>();
                }

                return GetByCondition("workshop_id = @workshopId",
                    DatabaseHelper.CreateParameter("@workshopId", workshopId));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间ID获取批次列表失败，车间ID: {0}", workshopId), ex);
                throw new MESException("获取批次列表失败", ex);
            }
        }

        /// <summary>
        /// 根据批次状态获取批次列表
        /// </summary>
        /// <param name="batchStatus">批次状态</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByBatchStatus(string batchStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(batchStatus))
                {
                    return new List<BatchInfo>();
                }

                return GetByCondition("batch_status = @batchStatus",
                    DatabaseHelper.CreateParameter("@batchStatus", batchStatus));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取批次列表失败，状态: {0}", batchStatus), ex);
                throw new MESException("获取批次列表失败", ex);
            }
        }

        /// <summary>
        /// 根据操作员获取批次列表
        /// </summary>
        /// <param name="operatorName">操作员姓名</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByOperator(string operatorName)
        {
            try
            {
                if (string.IsNullOrEmpty(operatorName))
                {
                    return new List<BatchInfo>();
                }

                return GetByCondition("operator_name = @operatorName",
                    DatabaseHelper.CreateParameter("@operatorName", operatorName));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据操作员获取批次列表失败，操作员: {0}", operatorName), ex);
                throw new MESException("获取批次列表失败", ex);
            }
        }

        /// <summary>
        /// 兼容方法 - 根据工单ID获取批次列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByWorkOrderId(string workOrderId)
        {
            // 这里可以根据实际需求实现，暂时返回空列表
            return new List<BatchInfo>();
        }

        /// <summary>
        /// 兼容方法 - 根据状态获取批次列表
        /// </summary>
        /// <param name="status">批次状态</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByStatus(int status)
        {
            // 将int状态转换为字符串状态
            string statusText = "";
            switch (status)
            {
                case 1: statusText = "待开始"; break;
                case 2: statusText = "进行中"; break;
                case 3: statusText = "已完成"; break;
                case 4: statusText = "已暂停"; break;
                case 5: statusText = "已取消"; break;
                default: statusText = "待开始"; break;
            }
            return GetByBatchStatus(statusText);
        }

        /// <summary>
        /// 兼容方法 - 根据当前工站获取批次列表
        /// </summary>
        /// <param name="stationId">工站ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByCurrentStation(string stationId)
        {
            // 这里可以根据实际需求实现，暂时返回空列表
            return new List<BatchInfo>();
        }

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">批次实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildInsertSql(BatchInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO batch_info
                          (batch_number, workshop_id, workshop_name, production_order_id, production_order_number,
                           product_code, product_name, planned_quantity, actual_quantity, unit,
                           batch_status, start_time, end_time, operator_id, operator_name, remarks,
                           create_time, create_user_id, create_user_name, is_deleted, remark, version)
                          VALUES
                          (@batchNumber, @workshopId, @workshopName, @productionOrderId, @productionOrderNumber,
                           @productCode, @productName, @plannedQuantity, @actualQuantity, @unit,
                           @batchStatus, @startTime, @endTime, @operatorId, @operatorName, @remarks,
                           @createTime, @createUserId, @createUserName, @isDeleted, @remark, @version)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@batchNumber", entity.BatchNumber),
                DatabaseHelper.CreateParameter("@workshopId", entity.WorkshopId),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@productionOrderId", entity.ProductionOrderId),
                DatabaseHelper.CreateParameter("@productionOrderNumber", entity.ProductionOrderNumber),
                DatabaseHelper.CreateParameter("@productCode", entity.ProductCode),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@plannedQuantity", entity.PlannedQuantity),
                DatabaseHelper.CreateParameter("@actualQuantity", entity.ActualQuantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@batchStatus", entity.BatchStatus),
                DatabaseHelper.CreateParameter("@startTime", entity.ProductionStartTime),
                DatabaseHelper.CreateParameter("@endTime", entity.ProductionEndTime),
                DatabaseHelper.CreateParameter("@operatorId", entity.OperatorId),
                DatabaseHelper.CreateParameter("@operatorName", entity.OperatorName),
                DatabaseHelper.CreateParameter("@remarks", entity.Remarks),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserId", entity.CreateUserId),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted),
                DatabaseHelper.CreateParameter("@remark", entity.Remark),
                DatabaseHelper.CreateParameter("@version", entity.Version)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">批次实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildUpdateSql(BatchInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE batch_info SET
                          batch_number = @batchNumber, workshop_id = @workshopId, workshop_name = @workshopName,
                          production_order_id = @productionOrderId, production_order_number = @productionOrderNumber,
                          product_code = @productCode, product_name = @productName,
                          planned_quantity = @plannedQuantity, actual_quantity = @actualQuantity, unit = @unit,
                          batch_status = @batchStatus, start_time = @startTime, end_time = @endTime,
                          operator_id = @operatorId, operator_name = @operatorName, remarks = @remarks,
                          update_time = @updateTime, update_user_id = @updateUserId, update_user_name = @updateUserName,
                          remark = @remark, version = @version
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@batchNumber", entity.BatchNumber),
                DatabaseHelper.CreateParameter("@workshopId", entity.WorkshopId),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@productionOrderId", entity.ProductionOrderId),
                DatabaseHelper.CreateParameter("@productionOrderNumber", entity.ProductionOrderNumber),
                DatabaseHelper.CreateParameter("@productCode", entity.ProductCode),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@plannedQuantity", entity.PlannedQuantity),
                DatabaseHelper.CreateParameter("@actualQuantity", entity.ActualQuantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@batchStatus", entity.BatchStatus),
                DatabaseHelper.CreateParameter("@startTime", entity.ProductionStartTime),
                DatabaseHelper.CreateParameter("@endTime", entity.ProductionEndTime),
                DatabaseHelper.CreateParameter("@operatorId", entity.OperatorId),
                DatabaseHelper.CreateParameter("@operatorName", entity.OperatorName),
                DatabaseHelper.CreateParameter("@remarks", entity.Remarks),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserId", entity.UpdateUserId),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@remark", entity.Remark),
                DatabaseHelper.CreateParameter("@version", entity.Version),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
        }

        #endregion

        #region 新增方法

        /// <summary>
        /// 获取可取消的批次列表（状态为待开始或进行中的批次）
        /// </summary>
        /// <returns>可取消的批次列表</returns>
        public List<BatchInfo> GetCancellableBatches()
        {
            try
            {
                // 获取状态为待开始或进行中的批次
                return GetByCondition("batch_status IN ('待开始', '进行中') AND is_deleted = 0 ORDER BY create_time DESC");
            }
            catch (Exception ex)
            {
                LogManager.Error("获取可取消批次列表失败", ex);
                throw new MESException("获取可取消批次列表失败", ex);
            }
        }

        /// <summary>
        /// 获取指定日期的最大批次序号
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>最大序号</returns>
        public int GetMaxBatchSequenceByDate(DateTime date)
        {
            try
            {
                string dateStr = date.ToString("yyyyMMdd");
                string sql = @"SELECT IFNULL(MAX(CAST(SUBSTRING(batch_number, 14, 3) AS UNSIGNED)), 0) as max_sequence
                              FROM batch_info
                              WHERE batch_number LIKE @pattern
                              AND is_deleted = 0";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@pattern", string.Format("BATCH{0}%", dateStr))
                };

                var result = DatabaseHelper.ExecuteScalar(sql, parameters);
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取日期 {0} 的最大批次序号失败", date.ToString("yyyy-MM-dd")), ex);
                throw new MESException("获取最大批次序号失败", ex);
            }
        }

        #endregion
    }
}
