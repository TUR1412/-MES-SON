using System;
using System.Collections.Generic;
using System.Data;
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
    /// 产品数据访问类
    /// 提供产品相关的数据库操作功能
    /// </summary>
    public class ProductionDAL : BaseDAL<ProductionInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "product_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "ProductId"; }
        }

        /// <summary>
        /// 将DataRow转换为ProductionInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>ProductionInfo实体对象</returns>
        protected override ProductionInfo MapRowToEntity(DataRow row)
        {
            return new ProductionInfo
            {
                ProductId = Convert.ToInt32(row["product_id"]),
                ProductNum = row["product_num"] != DBNull.Value ? row["product_num"].ToString() : null,
                ProductName = row["product_name"] != DBNull.Value ? row["product_name"].ToString() : null,
                ProductType = row["product_type"] != DBNull.Value ? row["product_type"].ToString() : null,
                DetailType = row["detail_type"] != DBNull.Value ? row["detail_type"].ToString() : null,
                PackageType = row["package_type"] != DBNull.Value ? row["package_type"].ToString() : null,
                Unit = row["unit"] != DBNull.Value ? row["unit"].ToString() : null,
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : null,
                Status = Convert.ToInt32(row["status"]),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null
            };
        }

        /// <summary>
        /// 根据产品编号获取产品信息
        /// </summary>
        /// <param name="productNum">产品编号</param>
        /// <returns>产品信息</returns>
        public ProductionInfo GetByProductNum(string productNum)
        {
            try
            {
                if (string.IsNullOrEmpty(productNum))
                {
                    throw new ArgumentException("产品编号不能为空", "productNum");
                }

                var products = GetByCondition("product_num = @productNum",
                    DatabaseHelper.CreateParameter("@productNum", productNum));

                return products.Count > 0 ? products[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据产品编号获取产品信息失败，产品编号: {productNum}", ex);
                throw new MESException("获取产品信息失败", ex);
            }
        }

        /// <summary>
        /// 根据产品类型获取产品列表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>产品列表</returns>
        public List<ProductionInfo> GetByProductType(string productType)
        {
            try
            {
                if (string.IsNullOrEmpty(productType))
                {
                    return new List<ProductionInfo>();
                }

                return GetByCondition("product_type = @productType",
                    DatabaseHelper.CreateParameter("@productType", productType));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据产品类型获取产品列表失败，产品类型: {productType}", ex);
                throw new MESException("获取产品列表失败", ex);
            }
        }

        /// <summary>
        /// 根据状态获取产品列表
        /// </summary>
        /// <param name="status">状态(0:禁用,1:启用)</param>
        /// <returns>产品列表</returns>
        public List<ProductionInfo> GetByStatus(int status)
        {
            try
            {
                return GetByCondition("status = @status",
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据状态获取产品列表失败，状态: {status}", ex);
                throw new MESException("获取产品列表失败", ex);
            }
        }

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">产品实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildInsertSql(ProductionInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO product_info
                  (product_num, product_name, product_type, detail_type, 
                   package_type, unit, description, status, create_time, update_time)
                  VALUES
                  (@productNum, @productName, @productType, @detailType, 
                   @packageType, @unit, @description, @status, @createTime, @updateTime)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@productNum", entity.ProductNum),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@productType", entity.ProductType),
                DatabaseHelper.CreateParameter("@detailType", entity.DetailType),
                DatabaseHelper.CreateParameter("@packageType", entity.PackageType),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">产品实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildUpdateSql(ProductionInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE product_info SET
                  product_num = @productNum, 
                  product_name = @productName,
                  product_type = @productType, 
                  detail_type = @detailType,
                  package_type = @packageType,
                  unit = @unit,
                  description = @description,
                  status = @status,
                  update_time = @updateTime
                  WHERE product_id = @productId";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@productNum", entity.ProductNum),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@productType", entity.ProductType),
                DatabaseHelper.CreateParameter("@detailType", entity.DetailType),
                DatabaseHelper.CreateParameter("@packageType", entity.PackageType),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId)
            };

            return true;
        }

        /// <summary>
        /// 更新产品状态
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="newStatus">新状态</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateStatus(int productId, int newStatus)
        {
            try
            {
                string sql = @"UPDATE product_info SET 
                              status = @status, 
                              update_time = @updateTime
                              WHERE product_id = @productId";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@status", newStatus),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@productId", productId)
                };

                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新产品状态失败，产品ID: {productId}, 新状态: {newStatus}", ex);
                throw new MESException("更新产品状态失败", ex);
            }
        }
    }
}