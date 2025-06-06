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
        protected override string TableName => "production_order";

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

                var orders = GetByCondition("order_no = @orderNo", 
                    DatabaseHelper.CreateParameter("@orderNo", orderNo));
                
                return orders.Count > 0 ? orders[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据订单编号获取生产订单失败，订单编号: {orderNo}", ex);
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
                LogManager.Error($"根据状态获取生产订单列表失败，状态: {status}", ex);
                throw new MESException("获取生产订单列表失败", ex);
            }
        }

        // TODO: H成员需要根据具体业务需求实现以下方法：
        // - GetByDateRange: 根据日期范围查询
        // - GetByMaterialId: 根据物料ID查询
        // - UpdateStatus: 更新订单状态
        // - GetProductionStatistics: 获取生产统计信息
        // - 其他业务相关方法

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// 注意：H成员需要根据实际表结构完善此方法
        /// </summary>
        /// <param name="entity">生产订单实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, MySqlParameter[] parameters) BuildInsertSql(ProductionOrderInfo entity)
        {
            // TODO: H成员需要根据实际的production_order表结构完善此SQL
            string sql = @"INSERT INTO production_order 
                          (order_no, material_id, quantity, status, 
                           create_time, create_user_name, is_deleted) 
                          VALUES 
                          (@orderNo, @materialId, @quantity, @status, 
                           @createTime, @createUserName, @isDeleted)";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@orderNo", entity.OrderNo),
                DatabaseHelper.CreateParameter("@materialId", entity.MaterialId),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
            };

            return (sql, parameters);
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// 注意：H成员需要根据实际表结构完善此方法
        /// </summary>
        /// <param name="entity">生产订单实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, MySqlParameter[] parameters) BuildUpdateSql(ProductionOrderInfo entity)
        {
            // TODO: H成员需要根据实际的production_order表结构完善此SQL
            string sql = @"UPDATE production_order SET 
                          order_no = @orderNo, material_id = @materialId, 
                          quantity = @quantity, status = @status, 
                          update_time = @updateTime, update_user_name = @updateUserName 
                          WHERE id = @id AND is_deleted = 0";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@orderNo", entity.OrderNo),
                DatabaseHelper.CreateParameter("@materialId", entity.MaterialId),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return (sql, parameters);
        }

        #endregion
    }

    // TODO: H成员需要在MES.Models.Production命名空间中创建ProductionOrderInfo模型类
}
