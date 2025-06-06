using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MES.Models.Base;
using MES.Models.Production;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Production
{
    /// <summary>
    /// 生产订单数据访问类
    /// 提供生产订单相关的数据库操作功能
    /// 注意：此类为H成员开发的模板，具体实现需要H成员根据业务需求完善
    /// </summary>
    public class ProductionOrderDAL : BaseDAL<ProductionOrderInfo>
    {
        #region 基类实现

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "production_order"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "Id"; }
        }

        /// <summary>
        /// 将DataRow转换为ProductionOrderInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>ProductionOrderInfo实体对象</returns>
        protected override ProductionOrderInfo MapRowToEntity(DataRow row)
        {
            return new ProductionOrderInfo
            {
                Id = Convert.ToInt32(row["id"]),
                OrderNumber = row["order_number"]?.ToString(),
                ProductCode = row["product_code"]?.ToString(),
                ProductName = row["product_name"]?.ToString(),
                PlannedQuantity = Convert.ToDecimal(row["planned_quantity"]),
                ActualQuantity = row["actual_quantity"] != DBNull.Value ? Convert.ToDecimal(row["actual_quantity"]) : (decimal?)null,
                Unit = row["unit"]?.ToString(),
                PlannedStartTime = Convert.ToDateTime(row["planned_start_time"]),
                PlannedEndTime = Convert.ToDateTime(row["planned_end_time"]),
                ActualStartTime = row["actual_start_time"] != DBNull.Value ? Convert.ToDateTime(row["actual_start_time"]) : (DateTime?)null,
                ActualEndTime = row["actual_end_time"] != DBNull.Value ? Convert.ToDateTime(row["actual_end_time"]) : (DateTime?)null,
                Status = row["status"]?.ToString(),
                Priority = row["priority"]?.ToString(),
                WorkshopId = row["workshop_id"] != DBNull.Value ? Convert.ToInt32(row["workshop_id"]) : (int?)null,
                WorkshopName = row["workshop_name"]?.ToString(),
                Customer = row["customer"]?.ToString(),
                SalesOrderNumber = row["sales_order_number"]?.ToString(),
                Remarks = row["remarks"]?.ToString(),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserName = row["create_user_name"]?.ToString(),
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        #endregion

        #region 生产订单特有操作

        /// <summary>
        /// 根据订单编号获取生产订单
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns>生产订单信息</returns>
        public ProductionOrderInfo GetByOrderNo(string orderNo)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNo))
                {
                    throw new ArgumentException("订单编号不能为空", nameof(orderNo));
                }

                var orders = GetByCondition("order_number = @orderNumber",
                    DatabaseHelper.CreateParameter("@orderNumber", orderNo));
                
                return orders.Count > 0 ? orders[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据订单编号获取生产订单失败，订单编号: {0}", orderNo), ex);
                throw new MESException("获取生产订单失败", ex);
            }
        }

        /// <summary>
        /// 根据订单状态获取生产订单列表
        /// </summary>
        /// <param name="status">订单状态</param>
        /// <returns>生产订单列表</returns>
        public List<ProductionOrderInfo> GetByStatus(string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    return new List<ProductionOrderInfo>();
                }

                return GetByCondition("status = @status", 
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取生产订单列表失败，状态: {0}", status), ex);
                throw new MESException("获取生产订单列表失败", ex);
            }
        }

        /// <summary>
        /// 根据订单编号获取生产订单 - BLL层兼容方法
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <returns>生产订单信息</returns>
        public ProductionOrderInfo GetByOrderNumber(string orderNumber)
        {
            return GetByOrderNo(orderNumber);
        }

        /// <summary>
        /// 根据产品编码获取生产订单列表
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>生产订单列表</returns>
        public List<ProductionOrderInfo> GetByProductCode(string productCode)
        {
            try
            {
                if (string.IsNullOrEmpty(productCode))
                {
                    return new List<ProductionOrderInfo>();
                }

                return GetByCondition("product_code = @productCode",
                    DatabaseHelper.CreateParameter("@productCode", productCode));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据产品编码获取生产订单列表失败，产品编码: {0}", productCode), ex);
                throw new MESException("获取生产订单列表失败", ex);
            }
        }

        /// <summary>
        /// 分页获取生产订单列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的生产订单列表</returns>
        public List<ProductionOrderInfo> GetByPage(int pageIndex, int pageSize, out int totalCount)
        {
            try
            {
                totalCount = 0;
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    return new List<ProductionOrderInfo>();
                }

                // 计算总记录数
                string countSql = string.Format("SELECT COUNT(*) FROM {0} WHERE is_deleted = 0", TableName);
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(countSql, connection))
                    {
                        totalCount = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

                // 分页查询
                int offset = (pageIndex - 1) * pageSize;
                string sql = string.Format("SELECT * FROM {0} WHERE is_deleted = 0 ORDER BY create_time DESC LIMIT @offset, @pageSize", TableName);

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@offset", offset),
                    DatabaseHelper.CreateParameter("@pageSize", pageSize)
                };

                return GetByCondition(sql.Replace(string.Format("SELECT * FROM {0} WHERE is_deleted = 0 ORDER BY create_time DESC LIMIT @offset, @pageSize", TableName),
                    "1=1 ORDER BY create_time DESC LIMIT @offset, @pageSize"), parameters);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("分页获取生产订单列表失败，页码: {0}, 每页记录数: {1}", pageIndex, pageSize), ex);
                totalCount = 0;
                throw new MESException("分页获取生产订单列表失败", ex);
            }
        }

        /// <summary>
        /// 根据关键词搜索生产订单
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns>匹配的生产订单列表</returns>
        public List<ProductionOrderInfo> Search(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return GetAll();
                }

                string condition = @"(order_number LIKE @keyword OR product_code LIKE @keyword
                                   OR product_name LIKE @keyword OR customer LIKE @keyword)";

                var parameter = DatabaseHelper.CreateParameter("@keyword", string.Format("%{0}%", keyword));

                return GetByCondition(condition, parameter);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索生产订单失败，关键词: {0}", keyword), ex);
                throw new MESException("搜索生产订单失败", ex);
            }
        }

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">生产订单实体</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected override bool BuildInsertSql(ProductionOrderInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO production_order
                          (order_number, product_code, product_name, planned_quantity, actual_quantity, unit,
                           planned_start_time, planned_end_time, actual_start_time, actual_end_time,
                           status, priority, workshop_id, workshop_name, customer, sales_order_number, remarks,
                           create_time, create_user_name, is_deleted)
                          VALUES
                          (@orderNumber, @productCode, @productName, @plannedQuantity, @actualQuantity, @unit,
                           @plannedStartTime, @plannedEndTime, @actualStartTime, @actualEndTime,
                           @status, @priority, @workshopId, @workshopName, @customer, @salesOrderNumber, @remarks,
                           @createTime, @createUserName, @isDeleted)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@orderNumber", entity.OrderNo),
                DatabaseHelper.CreateParameter("@productCode", entity.ProductCode),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@plannedQuantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@actualQuantity", entity.ActualQuantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@plannedStartTime", entity.PlanStartTime),
                DatabaseHelper.CreateParameter("@plannedEndTime", entity.PlanEndTime),
                DatabaseHelper.CreateParameter("@actualStartTime", entity.ActualStartTime),
                DatabaseHelper.CreateParameter("@actualEndTime", entity.ActualEndTime),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@priority", entity.Priority),
                DatabaseHelper.CreateParameter("@workshopId", entity.WorkshopId),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@customer", entity.CustomerName),
                DatabaseHelper.CreateParameter("@salesOrderNumber", entity.SalesOrderNumber),
                DatabaseHelper.CreateParameter("@remarks", entity.Remarks),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">生产订单实体</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected override bool BuildUpdateSql(ProductionOrderInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE production_order SET
                          order_number = @orderNumber, product_code = @productCode,
                          product_name = @productName, planned_quantity = @plannedQuantity, actual_quantity = @actualQuantity, unit = @unit,
                          planned_start_time = @plannedStartTime, planned_end_time = @plannedEndTime,
                          actual_start_time = @actualStartTime, actual_end_time = @actualEndTime,
                          status = @status, priority = @priority, workshop_id = @workshopId, workshop_name = @workshopName,
                          customer = @customer, sales_order_number = @salesOrderNumber, remarks = @remarks,
                          update_time = @updateTime, update_user_name = @updateUserName
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@orderNumber", entity.OrderNo),
                DatabaseHelper.CreateParameter("@productCode", entity.ProductCode),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@plannedQuantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@actualQuantity", entity.ActualQuantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@plannedStartTime", entity.PlanStartTime),
                DatabaseHelper.CreateParameter("@plannedEndTime", entity.PlanEndTime),
                DatabaseHelper.CreateParameter("@actualStartTime", entity.ActualStartTime),
                DatabaseHelper.CreateParameter("@actualEndTime", entity.ActualEndTime),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@priority", entity.Priority),
                DatabaseHelper.CreateParameter("@workshopId", entity.WorkshopId),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@customer", entity.CustomerName),
                DatabaseHelper.CreateParameter("@salesOrderNumber", entity.SalesOrderNumber),
                DatabaseHelper.CreateParameter("@remarks", entity.Remarks),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
        }

        #endregion
    }
}
