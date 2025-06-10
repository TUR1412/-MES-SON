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
    /// 车间作业信息数据访问类
    /// 提供车间作业相关的数据库操作功能
    /// </summary>
    public class WorkshopOperationDAL : BaseDAL<WorkshopOperationInfo>
    {
        #region 基类实现

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "workshop_operation_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "Id"; }
        }

        /// <summary>
        /// 将DataRow转换为实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>实体对象</returns>
        protected override WorkshopOperationInfo MapRowToEntity(DataRow row)
        {
            return new WorkshopOperationInfo
            {
                Id = Convert.ToInt32(row["id"]),
                OperationId = row["operation_id"].ToString(),
                WorkshopName = row["workshop_name"].ToString(),
                BatchNumber = row["batch_number"].ToString(),
                ProductCode = row["product_code"].ToString(),
                Quantity = Convert.ToDecimal(row["quantity"]),
                Status = Convert.ToInt32(row["status"]),
                StatusText = row["status_text"].ToString(),
                StartTime = row["start_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["start_time"]),
                Progress = Convert.ToDecimal(row["progress"]),
                Operator = row["operator"].ToString(),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserId = row["create_user_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["create_user_id"]),
                CreateUserName = row["create_user_name"].ToString(),
                UpdateTime = row["update_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["update_time"]),
                UpdateUserId = row["update_user_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["update_user_id"]),
                UpdateUserName = row["update_user_name"].ToString(),
                IsDeleted = Convert.ToBoolean(row["is_deleted"]),
                DeleteTime = row["delete_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["delete_time"]),
                DeleteUserId = row["delete_user_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["delete_user_id"]),
                DeleteUserName = row["delete_user_name"].ToString(),
                Remark = row["remark"].ToString(),
                Version = Convert.ToInt32(row["version"])
            };
        }

        #endregion

        #region 车间作业特有操作

        /// <summary>
        /// 根据作业编号获取车间作业信息
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>车间作业信息</returns>
        public WorkshopOperationInfo GetByOperationId(string operationId)
        {
            try
            {
                if (string.IsNullOrEmpty(operationId))
                {
                    throw new ArgumentException("作业编号不能为空", "operationId");
                }

                var operations = GetByCondition("operation_id = @operationId", 
                    DatabaseHelper.CreateParameter("@operationId", operationId));
                
                return operations.Count > 0 ? operations[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据作业编号获取车间作业信息失败，作业编号: {0}", operationId), ex);
                throw new MESException("获取车间作业信息失败", ex);
            }
        }

        /// <summary>
        /// 根据车间名称获取作业列表
        /// </summary>
        /// <param name="workshopName">车间名称</param>
        /// <returns>作业列表</returns>
        public List<WorkshopOperationInfo> GetByWorkshopName(string workshopName)
        {
            try
            {
                if (string.IsNullOrEmpty(workshopName))
                {
                    throw new ArgumentException("车间名称不能为空", "workshopName");
                }

                return GetByCondition("workshop_name = @workshopName ORDER BY create_time DESC", 
                    DatabaseHelper.CreateParameter("@workshopName", workshopName));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间名称获取作业列表失败，车间名称: {0}", workshopName), ex);
                throw new MESException("获取作业列表失败", ex);
            }
        }

        /// <summary>
        /// 根据状态获取作业列表
        /// </summary>
        /// <param name="status">作业状态</param>
        /// <returns>作业列表</returns>
        public List<WorkshopOperationInfo> GetByStatus(int status)
        {
            try
            {
                return GetByCondition("status = @status ORDER BY create_time DESC", 
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取作业列表失败，状态: {0}", status), ex);
                throw new MESException("获取作业列表失败", ex);
            }
        }

        /// <summary>
        /// 根据批次号获取作业列表
        /// </summary>
        /// <param name="batchNumber">批次号</param>
        /// <returns>作业列表</returns>
        public List<WorkshopOperationInfo> GetByBatchNumber(string batchNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(batchNumber))
                {
                    throw new ArgumentException("批次号不能为空", "batchNumber");
                }

                return GetByCondition("batch_number = @batchNumber ORDER BY create_time DESC", 
                    DatabaseHelper.CreateParameter("@batchNumber", batchNumber));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据批次号获取作业列表失败，批次号: {0}", batchNumber), ex);
                throw new MESException("获取作业列表失败", ex);
            }
        }

        /// <summary>
        /// 根据产品编码获取作业列表
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>作业列表</returns>
        public List<WorkshopOperationInfo> GetByProductCode(string productCode)
        {
            try
            {
                if (string.IsNullOrEmpty(productCode))
                {
                    throw new ArgumentException("产品编码不能为空", "productCode");
                }

                return GetByCondition("product_code = @productCode ORDER BY create_time DESC", 
                    DatabaseHelper.CreateParameter("@productCode", productCode));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据产品编码获取作业列表失败，产品编码: {0}", productCode), ex);
                throw new MESException("获取作业列表失败", ex);
            }
        }

        /// <summary>
        /// 根据操作员获取作业列表
        /// </summary>
        /// <param name="operatorName">操作员</param>
        /// <returns>作业列表</returns>
        public List<WorkshopOperationInfo> GetByOperator(string operatorName)
        {
            try
            {
                if (string.IsNullOrEmpty(operatorName))
                {
                    throw new ArgumentException("操作员不能为空", "operatorName");
                }

                return GetByCondition("operator = @operator ORDER BY create_time DESC", 
                    DatabaseHelper.CreateParameter("@operator", operatorName));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据操作员获取作业列表失败，操作员: {0}", operatorName), ex);
                throw new MESException("获取作业列表失败", ex);
            }
        }

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建插入SQL语句
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>是否构建成功</returns>
        protected override bool BuildInsertSql(WorkshopOperationInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO workshop_operation_info 
                    (operation_id, workshop_name, batch_number, product_code, quantity, 
                     status, status_text, start_time, progress, operator, 
                     create_time, create_user_id, create_user_name, is_deleted, remark, version) 
                    VALUES 
                    (@operation_id, @workshop_name, @batch_number, @product_code, @quantity, 
                     @status, @status_text, @start_time, @progress, @operator, 
                     @create_time, @create_user_id, @create_user_name, @is_deleted, @remark, @version)";

            parameters = new MySqlParameter[]
            {
                DatabaseHelper.CreateParameter("@operation_id", entity.OperationId),
                DatabaseHelper.CreateParameter("@workshop_name", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@batch_number", entity.BatchNumber),
                DatabaseHelper.CreateParameter("@product_code", entity.ProductCode),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@status_text", entity.StatusText),
                DatabaseHelper.CreateParameter("@start_time", entity.StartTime),
                DatabaseHelper.CreateParameter("@progress", entity.Progress),
                DatabaseHelper.CreateParameter("@operator", entity.Operator),
                DatabaseHelper.CreateParameter("@create_time", entity.CreateTime),
                DatabaseHelper.CreateParameter("@create_user_id", entity.CreateUserId),
                DatabaseHelper.CreateParameter("@create_user_name", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@is_deleted", entity.IsDeleted),
                DatabaseHelper.CreateParameter("@remark", entity.Remark),
                DatabaseHelper.CreateParameter("@version", entity.Version)
            };

            return true;
        }

        /// <summary>
        /// 构建更新SQL语句
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>是否构建成功</returns>
        protected override bool BuildUpdateSql(WorkshopOperationInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE workshop_operation_info SET 
                    operation_id = @operation_id, workshop_name = @workshop_name, 
                    batch_number = @batch_number, product_code = @product_code, 
                    quantity = @quantity, status = @status, status_text = @status_text, 
                    start_time = @start_time, progress = @progress, operator = @operator, 
                    update_time = @update_time, update_user_id = @update_user_id, 
                    update_user_name = @update_user_name, remark = @remark, version = @version 
                    WHERE id = @id";

            parameters = new MySqlParameter[]
            {
                DatabaseHelper.CreateParameter("@operation_id", entity.OperationId),
                DatabaseHelper.CreateParameter("@workshop_name", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@batch_number", entity.BatchNumber),
                DatabaseHelper.CreateParameter("@product_code", entity.ProductCode),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@status_text", entity.StatusText),
                DatabaseHelper.CreateParameter("@start_time", entity.StartTime),
                DatabaseHelper.CreateParameter("@progress", entity.Progress),
                DatabaseHelper.CreateParameter("@operator", entity.Operator),
                DatabaseHelper.CreateParameter("@update_time", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@update_user_id", entity.UpdateUserId),
                DatabaseHelper.CreateParameter("@update_user_name", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@remark", entity.Remark),
                DatabaseHelper.CreateParameter("@version", entity.Version),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
        }

        #endregion
    }
}
