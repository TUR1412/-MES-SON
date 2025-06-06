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
    /// 物料信息数据访问类
    /// 提供物料相关的数据库操作功能
    /// </summary>
    public class MaterialDAL : BaseDAL<MaterialInfo>
    {
        #region 基类实现

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName => "material_info";

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey => "Id";

        /// <summary>
        /// 将DataRow转换为MaterialInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>MaterialInfo实体对象</returns>
        protected override MaterialInfo MapRowToEntity(DataRow row)
        {
            return new MaterialInfo
            {
                Id = Convert.ToInt32(row["id"]),
                MaterialCode = row["material_code"]?.ToString(),
                MaterialName = row["material_name"]?.ToString(),
                MaterialType = row["material_type"]?.ToString(),
                Category = row["category"]?.ToString(),
                Specification = row["specification"]?.ToString(),
                Unit = row["unit"]?.ToString(),
                StandardCost = Convert.ToDecimal(row["standard_cost"] ?? 0),
                SafetyStock = Convert.ToDecimal(row["safety_stock"] ?? 0),
                MinStock = Convert.ToDecimal(row["min_stock"] ?? 0),
                MaxStock = Convert.ToDecimal(row["max_stock"] ?? 0),
                StockQuantity = Convert.ToDecimal(row["stock_quantity"] ?? 0),
                Supplier = row["supplier"]?.ToString(),
                LeadTime = Convert.ToInt32(row["lead_time"] ?? 0),
                Status = row["status"]?.ToString(),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserName = row["create_user_name"]?.ToString(),
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                UpdateUserName = row["update_user_name"]?.ToString(),
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        #endregion

        #region 物料特有操作

        /// <summary>
        /// 根据物料编码获取物料信息
        /// </summary>
        /// <param name="materialCode">物料编码</param>
        /// <returns>物料信息，不存在时返回null</returns>
        public MaterialInfo GetByMaterialCode(string materialCode)
        {
            try
            {
                if (string.IsNullOrEmpty(materialCode))
                {
                    throw new ArgumentException("物料编码不能为空", nameof(materialCode));
                }

                var materials = GetByCondition("material_code = @materialCode", 
                    DatabaseHelper.CreateParameter("@materialCode", materialCode));
                
                return materials.Count > 0 ? materials[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据物料编码获取物料失败，物料编码: {materialCode}", ex);
                throw new MESException("获取物料信息失败", ex);
            }
        }

        /// <summary>
        /// 根据物料类别获取物料列表
        /// </summary>
        /// <param name="category">物料类别</param>
        /// <returns>物料列表</returns>
        public List<MaterialInfo> GetByCategory(string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category))
                {
                    return new List<MaterialInfo>();
                }

                return GetByCondition("category = @category", 
                    DatabaseHelper.CreateParameter("@category", category));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据类别获取物料列表失败，类别: {category}", ex);
                throw new MESException("获取物料列表失败", ex);
            }
        }

        /// <summary>
        /// 根据物料名称模糊查询
        /// </summary>
        /// <param name="materialName">物料名称关键字</param>
        /// <returns>物料列表</returns>
        public List<MaterialInfo> SearchByName(string materialName)
        {
            try
            {
                if (string.IsNullOrEmpty(materialName))
                {
                    return new List<MaterialInfo>();
                }

                return GetByCondition("material_name LIKE @materialName", 
                    DatabaseHelper.CreateParameter("@materialName", $"%{materialName}%"));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据名称搜索物料失败，关键字: {materialName}", ex);
                throw new MESException("搜索物料失败", ex);
            }
        }

        /// <summary>
        /// 获取所有物料类别
        /// </summary>
        /// <returns>类别列表</returns>
        public List<string> GetAllCategories()
        {
            try
            {
                string sql = "SELECT DISTINCT category FROM material_info WHERE is_deleted = 0 AND category IS NOT NULL ORDER BY category";
                var dataTable = DatabaseHelper.ExecuteQuery(sql);
                
                var categories = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    categories.Add(row["category"].ToString());
                }
                
                return categories;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取物料类别列表失败", ex);
                throw new MESException("获取物料类别失败", ex);
            }
        }

        /// <summary>
        /// 检查物料编码是否已存在
        /// </summary>
        /// <param name="materialCode">物料编码</param>
        /// <param name="excludeId">排除的ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        public bool IsCodeExists(string materialCode, int excludeId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(materialCode))
                {
                    return false;
                }

                string whereClause = "material_code = @materialCode";
                var parameters = new List<MySqlParameter>
                {
                    DatabaseHelper.CreateParameter("@materialCode", materialCode)
                };

                if (excludeId > 0)
                {
                    whereClause += " AND id != @excludeId";
                    parameters.Add(DatabaseHelper.CreateParameter("@excludeId", excludeId));
                }

                int count = GetCount(whereClause, parameters.ToArray());
                return count > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error($"检查物料编码是否存在失败，编码: {materialCode}", ex);
                throw new MESException("检查物料编码失败", ex);
            }
        }

        /// <summary>
        /// 更新物料库存
        /// </summary>
        /// <param name="materialId">物料ID</param>
        /// <param name="stockQuantity">库存数量</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateStock(int materialId, decimal stockQuantity)
        {
            try
            {
                string sql = "UPDATE material_info SET stock_quantity = @stockQuantity, update_time = @updateTime WHERE id = @materialId AND is_deleted = 0";
                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@stockQuantity", stockQuantity),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@materialId", materialId)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info($"物料库存更新成功，物料ID: {materialId}, 库存: {stockQuantity}");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新物料库存失败，物料ID: {materialId}", ex);
                throw new MESException("更新物料库存失败", ex);
            }
        }

        /// <summary>
        /// 检查物料编码是否已存在（包括逻辑删除的记录）
        /// </summary>
        /// <param name="materialCode">物料编码</param>
        /// <param name="excludeId">排除的ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        public bool ExistsByMaterialCode(string materialCode, int excludeId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(materialCode))
                {
                    return false;
                }

                string whereClause = "material_code = @materialCode";
                var parameters = new List<MySqlParameter>
        {
            DatabaseHelper.CreateParameter("@materialCode", materialCode)
        };

                if (excludeId > 0)
                {
                    whereClause += " AND id != @excludeId";
                    parameters.Add(DatabaseHelper.CreateParameter("@excludeId", excludeId));
                }

                // 使用 MySQL 的参数化查询
                int count = GetCount(whereClause, parameters.ToArray());
                return count > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error($"检查物料编码是否存在失败，编码: {materialCode}", ex);
                throw new MESException("检查物料编码失败", ex);
            }
        }

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, MySqlParameter[] parameters) BuildInsertSql(MaterialInfo entity)
        {
            string sql = @"INSERT INTO material_info
                          (material_code, material_name, material_type, category, specification, unit,
                           standard_cost, safety_stock, min_stock, max_stock, supplier, lead_time, status,
                           create_time, create_user_name, is_deleted)
                          VALUES
                          (@materialCode, @materialName, @materialType, @category, @specification, @unit,
                           @standardCost, @safetyStock, @minStock, @maxStock, @supplier, @leadTime, @status,
                           @createTime, @createUserName, @isDeleted)";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@materialCode", entity.MaterialCode),
                DatabaseHelper.CreateParameter("@materialName", entity.MaterialName),
                DatabaseHelper.CreateParameter("@materialType", entity.MaterialType),
                DatabaseHelper.CreateParameter("@category", entity.Category),
                DatabaseHelper.CreateParameter("@specification", entity.Specification),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@standardCost", entity.StandardCost),
                DatabaseHelper.CreateParameter("@safetyStock", entity.SafetyStock),
                DatabaseHelper.CreateParameter("@minStock", entity.MinStock),
                DatabaseHelper.CreateParameter("@maxStock", entity.MaxStock),
                DatabaseHelper.CreateParameter("@supplier", entity.Supplier),
                DatabaseHelper.CreateParameter("@leadTime", entity.LeadTime),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
            };

            return (sql, parameters);
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, MySqlParameter[] parameters) BuildUpdateSql(MaterialInfo entity)
        {
            string sql = @"UPDATE material_info SET
                          material_code = @materialCode, material_name = @materialName,
                          material_type = @materialType, category = @category, specification = @specification, unit = @unit,
                          standard_cost = @standardCost, safety_stock = @safetyStock,
                          min_stock = @minStock, max_stock = @maxStock, supplier = @supplier,
                          lead_time = @leadTime, status = @status,
                          update_time = @updateTime, update_user_name = @updateUserName
                          WHERE id = @id AND is_deleted = 0";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@materialCode", entity.MaterialCode),
                DatabaseHelper.CreateParameter("@materialName", entity.MaterialName),
                DatabaseHelper.CreateParameter("@materialType", entity.MaterialType),
                DatabaseHelper.CreateParameter("@category", entity.Category),
                DatabaseHelper.CreateParameter("@specification", entity.Specification),
                DatabaseHelper.CreateParameter("@unit", entity.Unit),
                DatabaseHelper.CreateParameter("@standardCost", entity.StandardCost),
                DatabaseHelper.CreateParameter("@safetyStock", entity.SafetyStock),
                DatabaseHelper.CreateParameter("@minStock", entity.MinStock),
                DatabaseHelper.CreateParameter("@maxStock", entity.MaxStock),
                DatabaseHelper.CreateParameter("@supplier", entity.Supplier),
                DatabaseHelper.CreateParameter("@leadTime", entity.LeadTime),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return (sql, parameters);
        }

        #endregion
    }
}
