using System;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Material;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;
using System.Collections.Generic;

namespace MES.DAL.Material
{
    /// <summary>
    /// BOM物料清单数据访问类
    /// 提供BOM相关的数据库操作功能
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
            get { return "id"; } // 数据库主键是 id
        }

        /// <summary>
        /// 将DataRow转换为BOMInfo实体对象 (已由天帝完全修复)
        /// </summary>
        protected override BOMInfo MapRowToEntity(DataRow row)
        {
            var bom = new BOMInfo();

            // 安全地从DataRow获取值
            bom.Id = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
            bom.BOMCode = row.Table.Columns.Contains("bom_code") && row["bom_code"] != DBNull.Value ? row["bom_code"].ToString() : string.Empty;
            bom.BomName = row.Table.Columns.Contains("bom_name") && row["bom_name"] != DBNull.Value ? row["bom_name"].ToString() : string.Empty;

            bom.ProductId = row.Table.Columns.Contains("product_id") && row["product_id"] != DBNull.Value ? Convert.ToInt32(row["product_id"]) : 0;
            bom.ProductCode = row.Table.Columns.Contains("product_code") && row["product_code"] != DBNull.Value ? row["product_code"].ToString() : string.Empty;
            bom.ProductName = row.Table.Columns.Contains("product_name") && row["product_name"] != DBNull.Value ? row["product_name"].ToString() : string.Empty;

            bom.MaterialId = row.Table.Columns.Contains("material_id") && row["material_id"] != DBNull.Value ? Convert.ToInt32(row["material_id"]) : 0;
            bom.MaterialCode = row.Table.Columns.Contains("material_code") && row["material_code"] != DBNull.Value ? row["material_code"].ToString() : string.Empty;
            bom.MaterialName = row.Table.Columns.Contains("material_name") && row["material_name"] != DBNull.Value ? row["material_name"].ToString() : string.Empty;

            bom.Quantity = row.Table.Columns.Contains("quantity") && row["quantity"] != DBNull.Value ? Convert.ToDecimal(row["quantity"]) : 0;
            bom.Unit = row.Table.Columns.Contains("unit") && row["unit"] != DBNull.Value ? row["unit"].ToString() : "个";
            bom.LossRate = row.Table.Columns.Contains("loss_rate") && row["loss_rate"] != DBNull.Value ? Convert.ToDecimal(row["loss_rate"]) : 0;
            bom.SubstituteMaterial = row.Table.Columns.Contains("substitute_material") && row["substitute_material"] != DBNull.Value ? row["substitute_material"].ToString() : string.Empty;

            // 核心修正：修正命名不匹配的映射
            bom.BOMVersion = row.Table.Columns.Contains("version") && row["version"] != DBNull.Value ? row["version"].ToString() : "1.0";
            bom.Remarks = row.Table.Columns.Contains("description") && row["description"] != DBNull.Value ? row["description"].ToString() : string.Empty;

            bom.BOMType = row.Table.Columns.Contains("bom_type") && row["bom_type"] != DBNull.Value ? row["bom_type"].ToString() : string.Empty;
            bom.EffectiveDate = row.Table.Columns.Contains("effective_date") && row["effective_date"] != DBNull.Value ? Convert.ToDateTime(row["effective_date"]) : DateTime.MinValue;
            bom.ExpireDate = row.Table.Columns.Contains("expire_date") && row["expire_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["expire_date"]) : null;

            // 核心修正：处理字符串状态到布尔值的转换
            if (row.Table.Columns.Contains("status") && row["status"] != DBNull.Value)
            {
                string statusStr = row["status"].ToString().ToLower();
                bom.Status = (statusStr == "有效" || statusStr == "1" || statusStr == "true");
            }
            else
            {
                bom.Status = false;
            }

            // BaseModel 字段
            bom.CreateTime = row.Table.Columns.Contains("create_time") && row["create_time"] != DBNull.Value ? Convert.ToDateTime(row["create_time"]) : DateTime.MinValue;
            bom.CreateUserId = row.Table.Columns.Contains("create_user_id") && row["create_user_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["create_user_id"]) : null;
            bom.CreateUserName = row.Table.Columns.Contains("create_user_name") && row["create_user_name"] != DBNull.Value ? row["create_user_name"].ToString() : string.Empty;
            bom.UpdateTime = row.Table.Columns.Contains("update_time") && row["update_time"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["update_time"]) : null;
            bom.UpdateUserId = row.Table.Columns.Contains("update_user_id") && row["update_user_id"] != DBNull.Value ? (int?)Convert.ToInt32(row["update_user_id"]) : null;
            bom.UpdateUserName = row.Table.Columns.Contains("update_user_name") && row["update_user_name"] != DBNull.Value ? row["update_user_name"].ToString() : string.Empty;
            bom.IsDeleted = row.Table.Columns.Contains("is_deleted") && row["is_deleted"] != DBNull.Value ? Convert.ToBoolean(Convert.ToInt32(row["is_deleted"])) : false;

            return bom;
        }

        #endregion

        #region BOM特有操作 (无需修改)

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

        #endregion

        #region SQL构建实现

        protected override bool BuildInsertSql(BOMInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO bom_info
                          (bom_code, bom_name, product_id, product_code, product_name, material_id, material_code, material_name, 
                           quantity, unit, loss_rate, substitute_material, version, bom_type, 
                           effective_date, expire_date, status, description, 
                           create_time, create_user_id, create_user_name, is_deleted)
                          VALUES
                          (@bom_code, @bom_name, @product_id, @product_code, @product_name, @material_id, @material_code, @material_name, 
                           @quantity, @unit, @loss_rate, @substitute_material, @version, @bom_type,
                           @effective_date, @expire_date, @status, @description, 
                           @create_time, @create_user_id, @create_user_name, @is_deleted)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@bom_code", entity.BOMCode),
                DatabaseHelper.CreateParameter("@bom_name", entity.BomName),
                DatabaseHelper.CreateParameter("@product_id", entity.ProductId),
                DatabaseHelper.CreateParameter("@product_code", entity.ProductCode),
                DatabaseHelper.CreateParameter("@product_name", entity.ProductName),
                DatabaseHelper.CreateParameter("@material_id", entity.MaterialId),
                DatabaseHelper.CreateParameter("@material_code", entity.MaterialCode),
                DatabaseHelper.CreateParameter("@material_name", entity.MaterialName),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@loss_rate", entity.LossRate),
                DatabaseHelper.CreateParameter("@substitute_material", entity.SubstituteMaterial),
                DatabaseHelper.CreateParameter("@version", entity.BOMVersion),
                DatabaseHelper.CreateParameter("@bom_type", entity.BOMType),
                DatabaseHelper.CreateParameter("@effective_date", entity.EffectiveDate),
                DatabaseHelper.CreateParameter("@expire_date", entity.ExpireDate),
                DatabaseHelper.CreateParameter("@status", entity.Status ? "有效" : "无效"), // 转换回数据库的字符串格式
                DatabaseHelper.CreateParameter("@description", entity.Remarks),
                DatabaseHelper.CreateParameter("@create_time", entity.CreateTime),
                DatabaseHelper.CreateParameter("@create_user_id", entity.CreateUserId),
                DatabaseHelper.CreateParameter("@create_user_name", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@is_deleted", entity.IsDeleted)
            };

            return true;
        }

        protected override bool BuildUpdateSql(BOMInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE bom_info SET
                          bom_code = @bom_code,
                          bom_name = @bom_name,
                          product_id = @product_id,
                          product_code = @product_code,
                          product_name = @product_name,
                          material_id = @material_id,
                          material_code = @material_code,
                          material_name = @material_name,
                          quantity = @quantity,
                          unit = @unit,
                          loss_rate = @loss_rate,
                          substitute_material = @substitute_material,
                          version = @version,
                          bom_type = @bom_type,
                          effective_date = @effective_date,
                          expire_date = @expire_date,
                          status = @status,
                          description = @description,
                          update_time = @update_time,
                          update_user_id = @update_user_id,
                          update_user_name = @update_user_name
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@bom_code", entity.BOMCode),
                DatabaseHelper.CreateParameter("@bom_name", entity.BomName),
                DatabaseHelper.CreateParameter("@product_id", entity.ProductId),
                DatabaseHelper.CreateParameter("@product_code", entity.ProductCode),
                DatabaseHelper.CreateParameter("@product_name", entity.ProductName),
                DatabaseHelper.CreateParameter("@material_id", entity.MaterialId),
                DatabaseHelper.CreateParameter("@material_code", entity.MaterialCode),
                DatabaseHelper.CreateParameter("@material_name", entity.MaterialName),
                DatabaseHelper.CreateParameter("@quantity", entity.Quantity),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@loss_rate", entity.LossRate),
                DatabaseHelper.CreateParameter("@substitute_material", entity.SubstituteMaterial),
                DatabaseHelper.CreateParameter("@version", entity.BOMVersion),
                DatabaseHelper.CreateParameter("@bom_type", entity.BOMType),
                DatabaseHelper.CreateParameter("@effective_date", entity.EffectiveDate),
                DatabaseHelper.CreateParameter("@expire_date", entity.ExpireDate),
                DatabaseHelper.CreateParameter("@status", entity.Status ? "有效" : "无效"), // 转换回数据库的字符串格式
                DatabaseHelper.CreateParameter("@description", entity.Remarks),
                DatabaseHelper.CreateParameter("@update_time", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@update_user_id", entity.UpdateUserId),
                DatabaseHelper.CreateParameter("@update_user_name", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
        }
        #endregion

        #region 物理删除方法

        /// <summary>
        /// 物理删除BOM（真实删除，不可恢复）
        /// </summary>
        /// <param name="id">BOM ID</param>
        /// <returns>是否删除成功</returns>
        public bool PhysicalDelete(int id)
        {
            try
            {
                string sql = "DELETE FROM bom_info WHERE id = @id";
                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@id", id)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);

                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info(string.Format("物理删除BOMInfo成功，ID: {0}", id));
                }

                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("物理删除BOMInfo失败，ID: {0}", id), ex);
                throw new MESException("物理删除BOM失败", ex);
            }
        }

        #endregion
    }
}