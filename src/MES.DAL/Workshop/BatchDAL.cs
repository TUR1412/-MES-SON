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
                BatchId = row["batch_id"] != DBNull.Value ? row["batch_id"].ToString() : null,
                WorkOrderId = row["work_order_id"] != DBNull.Value ? row["work_order_id"].ToString() : null,
                ProductMaterialId = row["product_material_id"] != DBNull.Value ? row["product_material_id"].ToString() : null,
                Quantity = Convert.ToDecimal(row["quantity"]),
                Status = Convert.ToInt32(row["status"]),
                CurrentStationId = row["current_station_id"] != DBNull.Value ? row["current_station_id"].ToString() : null,
                ProductionStartTime = row["production_start_time"] != DBNull.Value ? Convert.ToDateTime(row["production_start_time"]) : (DateTime?)null,
                ProductionEndTime = row["production_end_time"] != DBNull.Value ? Convert.ToDateTime(row["production_end_time"]) : (DateTime?)null,
                CarrierId = row["carrier_id"] != DBNull.Value ? row["carrier_id"].ToString() : null,
                CreateTime = Convert.ToDateTime(row["create_time"]),
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        #endregion

        #region 批次特有操作

        /// <summary>
        /// 根据批次编号获取批次信息
        /// </summary>
        /// <param name="batchId">批次编号</param>
        /// <returns>批次信息</returns>
        public BatchInfo GetByBatchId(string batchId)
        {
            try
            {
                if (string.IsNullOrEmpty(batchId))
                {
                    throw new ArgumentException("批次编号不能为空", "batchId");
                }

                var batches = GetByCondition("batch_id = @batchId", 
                    DatabaseHelper.CreateParameter("@batchId", batchId));
                
                return batches.Count > 0 ? batches[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据批次编号获取批次信息失败，批次编号: {0}", batchId), ex);
                throw new MESException("获取批次信息失败", ex);
            }
        }

        /// <summary>
        /// 根据工单ID获取批次列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByWorkOrderId(string workOrderId)
        {
            try
            {
                if (string.IsNullOrEmpty(workOrderId))
                {
                    return new List<BatchInfo>();
                }

                return GetByCondition("work_order_id = @workOrderId", 
                    DatabaseHelper.CreateParameter("@workOrderId", workOrderId));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工单ID获取批次列表失败，工单ID: {0}", workOrderId), ex);
                throw new MESException("获取批次列表失败", ex);
            }
        }

        /// <summary>
        /// 根据批次状态获取批次列表
        /// </summary>
        /// <param name="status">批次状态</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByStatus(int status)
        {
            try
            {
                return GetByCondition("status = @status", 
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取批次列表失败，状态: {0}", status), ex);
                throw new MESException("获取批次列表失败", ex);
            }
        }

        /// <summary>
        /// 根据当前工站获取批次列表
        /// </summary>
        /// <param name="stationId">工站ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetByCurrentStation(string stationId)
        {
            try
            {
                if (string.IsNullOrEmpty(stationId))
                {
                    return new List<BatchInfo>();
                }

                return GetByCondition("current_station_id = @stationId", 
                    DatabaseHelper.CreateParameter("@stationId", stationId));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工站获取批次列表失败，工站ID: {0}", stationId), ex);
                throw new MESException("获取批次列表失败", ex);
            }
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
                          (batch_id, work_order_id, product_material_id, quantity, status,
                           current_station_id, production_start_time, production_end_time,
                           carrier_id, create_time, update_time, is_deleted)
                          VALUES
                          (@batchId, @workOrderId, @productMaterialId, @quantity, @status,
                           @currentStationId, @productionStartTime, @productionEndTime,
                           @carrierId, @createTime, @updateTime, @isDeleted)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@batchId", entity.BatchId),
                DatabaseHelper.CreateParameter("@workOrderId", entity.WorkOrderId),
                DatabaseHelper.CreateParameter("@productMaterialId", entity.ProductMaterialId),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@currentStationId", entity.CurrentStationId),
                DatabaseHelper.CreateParameter("@productionStartTime", entity.ProductionStartTime),
                DatabaseHelper.CreateParameter("@productionEndTime", entity.ProductionEndTime),
                DatabaseHelper.CreateParameter("@carrierId", entity.CarrierId),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
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
                          batch_id = @batchId, work_order_id = @workOrderId,
                          product_material_id = @productMaterialId, quantity = @quantity,
                          status = @status, current_station_id = @currentStationId,
                          production_start_time = @productionStartTime,
                          production_end_time = @productionEndTime, carrier_id = @carrierId,
                          update_time = @updateTime
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@batchId", entity.BatchId),
                DatabaseHelper.CreateParameter("@workOrderId", entity.WorkOrderId),
                DatabaseHelper.CreateParameter("@productMaterialId", entity.ProductMaterialId),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@currentStationId", entity.CurrentStationId),
                DatabaseHelper.CreateParameter("@productionStartTime", entity.ProductionStartTime),
                DatabaseHelper.CreateParameter("@productionEndTime", entity.ProductionEndTime),
                DatabaseHelper.CreateParameter("@carrierId", entity.CarrierId),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
        }

        #endregion
    }
}
