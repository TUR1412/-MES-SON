using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Material;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Material
{
    /// <summary>
    /// BOM物料清单数据访问类
    /// 提供BOM相关的数据库操作功能
    /// 注意：此类需要L成员根据实际BOM表结构进行完善
    /// </summary>
    public class BOMDAL : BaseDAL<BOMInfo>
    {
        #region 基类实现

        protected override string TableName
        {
            get { return "bom_info"; }
        }

        protected override string PrimaryKey
        {
            get { return "Id"; }
        }

        /// <summary>
        /// 将DataRow转换为BOMInfo实体对象 (已修复)
        /// </summary>
        protected override BOMInfo MapRowToEntity(DataRow row)
        {
            // 安全地获取每一列的值，如果列不存在或值为DBNull，则使用默认值
            return new BOMInfo
            {
                Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0,
                BOMCode = row.Table.Columns.Contains("bom_code") && row["bom_code"] != DBNull.Value ? row["bom_code"].ToString() : string.Empty,
                ProductId = row.Table.Columns.Contains("product_id") && row["product_id"] != DBNull.Value ? Convert.ToInt32(row["product_id"]) : 0,
                ProductCode = row.Table.Columns.Contains("product_code") && row["product_code"] != DBNull.Value ? row["product_code"].ToString() : string.Empty,
                ProductName = row.Table.Columns.Contains("product_name") && row["product_name"] != DBNull.Value ? row["product_name"].ToString() : string.Empty,
                MaterialId = row.Table.Columns.Contains("material_id") && row["material_id"] != DBNull.Value ? Convert.ToInt32(row["material_id"]) : 0,
                MaterialCode = row.Table.Columns.Contains("material_code") && row["material_code"] != DBNull.Value ? row["material_code"].ToString() : string.Empty,
                MaterialName = row.Table.Columns.Contains("material_name") && row["material_name"] != DBNull.Value ? row["material_name"].ToString() : string.Empty,
                Quantity = row.Table.Columns.Contains("quantity") && row["quantity"] != DBNull.Value ? Convert.ToDecimal(row["quantity"]) : 0,
                Unit = row.Table.Columns.Contains("unit") && row["unit"] != DBNull.Value ? row["unit"].ToString() : "个",
                LossRate = row.Table.Columns.Contains("loss_rate") && row["loss_rate"] != DBNull.Value ? Convert.ToDecimal(row["loss_rate"]) : 0,
                SubstituteMaterial = row.Table.Columns.Contains("substitute_material") && row["substitute_material"] != DBNull.Value ? row["substitute_material"].ToString() : string.Empty,
                BOMVersion = row.Table.Columns.Contains("bom_version") && row["bom_version"] != DBNull.Value ? row["bom_version"].ToString() : "1.0",
                BOMType = row.Table.Columns.Contains("bom_type") && row["bom_type"] != DBNull.Value ? row["bom_type"].ToString() : "PRODUCTION",
                EffectiveDate = row.Table.Columns.Contains("effective_date") && row["effective_date"] != DBNull.Value ? Convert.ToDateTime(row["effective_date"]) : DateTime.Now,
                ExpireDate = row.Table.Columns.Contains("expire_date") && row["expire_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["expire_date"]) : null,
                Status = row.Table.Columns.Contains("status") && row["status"] != DBNull.Value ? Convert.ToBoolean(row["status"]) : false,
                Remarks = row.Table.Columns.Contains("remarks") && row["remarks"] != DBNull.Value ? row["remarks"].ToString() : string.Empty, // 修正：使用 "remarks"
                CreateTime = row.Table.Columns.Contains("create_time") && row["create_time"] != DBNull.Value ? Convert.ToDateTime(row["create_time"]) : DateTime.MinValue,
                UpdateTime = row.Table.Columns.Contains("update_time") && row["update_time"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["update_time"]) : null,
                IsDeleted = row.Table.Columns.Contains("is_deleted") && row["is_deleted"] != DBNull.Value ? Convert.ToBoolean(row["is_deleted"]) : false
            };
        }

        #endregion

        #region BOM特有操作

        /// <summary>
        /// 根据产品ID获取BOM列表
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>BOM列表</returns>
        public List<BOMInfo> GetByProductId(int productId)
        {
            try
            {
                return GetByCondition("product_id = @productId",
                    DatabaseHelper.CreateParameter("@productId", productId));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据产品ID获取BOM失败，产品ID: {0}", productId), ex);
                throw new MESException("获取BOM列表失败", ex);
            }
        }

        /// <summary>
        /// 根据BOM编码获取BOM信息
        /// </summary>
        /// <param name="bomCode">BOM编码</param>
        /// <returns>BOM信息</returns>
        public BOMInfo GetByBOMCode(string bomCode)
        {
            try
            {
                if (string.IsNullOrEmpty(bomCode))
                {
                    throw new ArgumentException("BOM编码不能为空", "bomCode");
                }

                var boms = GetByCondition("bom_code = @bomCode",
                    DatabaseHelper.CreateParameter("@bomCode", bomCode));

                return boms.Count > 0 ? boms[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据BOM编码获取BOM失败，BOM编码: {0}", bomCode), ex);
                throw new MESException("获取BOM信息失败", ex);
            }
        }

        // TODO: L成员需要根据实际业务需求实现BOM详细查询方法

        // TODO: L成员需要根据实际业务需求实现其他BOM操作方法

        #endregion

        #region SQL构建实现 (已修复)

        /// <summary>
        /// 构建INSERT SQL语句 (已修复)
        /// </summary>
        protected override bool BuildInsertSql(BOMInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO bom_info
                          (bom_code, product_id, product_code, product_name, material_id, material_code, material_name, 
                           quantity, unit, loss_rate, substitute_material, bom_version, bom_type, 
                           effective_date, expire_date, status, remarks, 
                           create_time, create_user_name, is_deleted, version)
                          VALUES
                          (@bomCode, @productId, @productCode, @productName, @materialId, @materialCode, @materialName, 
                           @quantity, @unit, @lossRate, @substituteMaterial, @bomVersion, @bomType,
                           @effectiveDate, @expireDate, @status, @remarks, 
                           @createTime, @createUserName, @isDeleted, @version)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@bomCode", entity.BOMCode),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@productCode", entity.ProductCode),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@materialId", entity.MaterialId),
                DatabaseHelper.CreateParameter("@materialCode", entity.MaterialCode),
                DatabaseHelper.CreateParameter("@materialName", entity.MaterialName),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@lossRate", entity.LossRate),
                DatabaseHelper.CreateParameter("@substituteMaterial", entity.SubstituteMaterial),
                DatabaseHelper.CreateParameter("@bomVersion", entity.BOMVersion),
                DatabaseHelper.CreateParameter("@bomType", entity.BOMType),
                DatabaseHelper.CreateParameter("@effectiveDate", entity.EffectiveDate),
                DatabaseHelper.CreateParameter("@expireDate", entity.ExpireDate),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@remarks", entity.Remarks),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted),
                DatabaseHelper.CreateParameter("@version", entity.Version)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句 (已修复)
        /// </summary>
        protected override bool BuildUpdateSql(BOMInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE bom_info SET
                          bom_code = @bomCode,
                          product_id = @productId,
                          product_code = @productCode,
                          product_name = @productName,
                          material_id = @materialId,
                          material_code = @materialCode,
                          material_name = @materialName,
                          quantity = @quantity,
                          unit = @unit,
                          loss_rate = @lossRate,
                          substitute_material = @substituteMaterial,
                          bom_version = @bomVersion,
                          bom_type = @bomType,
                          effective_date = @effectiveDate,
                          expire_date = @expireDate,
                          status = @status,
                          remarks = @remarks,
                          update_time = @updateTime,
                          update_user_name = @updateUserName,
                          version = @version
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@bomCode", entity.BOMCode),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@productCode", entity.ProductCode),
                DatabaseHelper.CreateParameter("@productName", entity.ProductName),
                DatabaseHelper.CreateParameter("@materialId", entity.MaterialId),
                DatabaseHelper.CreateParameter("@materialCode", entity.MaterialCode),
                DatabaseHelper.CreateParameter("@materialName", entity.MaterialName),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@lossRate", entity.LossRate),
                DatabaseHelper.CreateParameter("@substituteMaterial", entity.SubstituteMaterial),
                DatabaseHelper.CreateParameter("@bomVersion", entity.BOMVersion),
                DatabaseHelper.CreateParameter("@bomType", entity.BOMType),
                DatabaseHelper.CreateParameter("@effectiveDate", entity.EffectiveDate),
                DatabaseHelper.CreateParameter("@expireDate", entity.ExpireDate),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@remarks", entity.Remarks),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@version", entity.Version),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
        }

        #endregion
    }
}
