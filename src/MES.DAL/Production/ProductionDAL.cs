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
using System.Linq;

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

        /// ★★★ 修正：主键数据库字段名 ★★★
        /// </summary>
        protected override string PrimaryKeyField
        {
            get { return "id"; } // 使用数据库中的真实字段名 'id'
        }

        /// <summary>
        /// ★★★ 修正：主键模型属性名 ★★★
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
            var entity = new ProductionInfo
            {
                Id = Convert.ToInt32(row["id"]),
                ProductId = Convert.ToInt32(row["id"]),
                ProductNum = row["product_code"]?.ToString(),
                ProductName = row["product_name"]?.ToString(),
                ProductType = row["product_type"]?.ToString(),
                // ★★★ 核心修正：数据库是 'specification'，模型是 'DetailType' ★★★
                DetailType = row["specification"]?.ToString(),
                // PackageType 在数据库中不存在，需要从模型中移除或在数据库中添加
                // PackageType = row["package_type"]?.ToString(), 
                Unit = row["unit"]?.ToString(),
                Description = row["description"]?.ToString(),
                // 数据库中 status 是 varchar，模型是 int，需要转换
                Status = (row["status"]?.ToString() == "有效" ? 1 : 0),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                UpdateTime = row.Table.Columns.Contains("update_time") && row["update_time"] != DBNull.Value
                           ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null
            };
            return entity;
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
                if (string.IsNullOrEmpty(productNum)) return null;
                var products = GetByCondition("product_code = @productCode", DatabaseHelper.CreateParameter("@productCode", productNum));
                return products.FirstOrDefault();
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
                  (product_code, product_name, product_type, specification, unit, description, status, create_time, update_time)
                  VALUES
                  (@productCode, @productName, @productType, @specification, @unit, @description, @status, @createTime, @updateTime)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@productCode", entity.ProductNum),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@productType", entity.ProductType),
                DatabaseHelper.CreateParameter("@specification", entity.DetailType), // 映射到specification
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@status", entity.Status == 1 ? "有效" : "禁用"), // 映射到varchar
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
                  product_code = @productCode,
                  product_name = @productName,
                  product_type = @productType, 
                  specification = @specification,
                  unit = @unit,
                  description = @description,
                  status = @status,
                  update_time = @updateTime
                  WHERE id = @id"; // 使用正确的 'id'

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@productCode", entity.ProductNum),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@productType", entity.ProductType),
                DatabaseHelper.CreateParameter("@specification", entity.DetailType),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                DatabaseHelper.CreateParameter("@id", entity.Id) // 使用正确的 entity.Id
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
                              WHERE id = @id"; // 使用正确的 'id'

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@status", newStatus),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@id", productId)
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