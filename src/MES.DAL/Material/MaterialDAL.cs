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
        protected override string TableName
        {
            get { return "material_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "Id"; }
        }

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
                MaterialCode = row["material_code"] != DBNull.Value ? row["material_code"].ToString() : null,
                MaterialName = row["material_name"] != DBNull.Value ? row["material_name"].ToString() : null,
                MaterialType = row["material_type"] != DBNull.Value ? row["material_type"].ToString() : null,
                Category = row["category"] != DBNull.Value ? row["category"].ToString() : null,
                Specification = row["specification"] != DBNull.Value ? row["specification"].ToString() : null,
                Unit = row["unit"] != DBNull.Value ? row["unit"].ToString() : null,
                StandardCost = row["standard_cost"] != DBNull.Value ? Convert.ToDecimal(row["standard_cost"]) : 0,
                SafetyStock = row["safety_stock"] != DBNull.Value ? Convert.ToDecimal(row["safety_stock"]) : 0,
                MinStock = row["min_stock"] != DBNull.Value ? Convert.ToDecimal(row["min_stock"]) : 0,
                MaxStock = row["max_stock"] != DBNull.Value ? Convert.ToDecimal(row["max_stock"]) : 0,
                StockQuantity = row["stock_quantity"] != DBNull.Value ? Convert.ToDecimal(row["stock_quantity"]) : 0,
                Supplier = row["supplier"] != DBNull.Value ? row["supplier"].ToString() : null,
                LeadTime = row["lead_time"] != DBNull.Value ? Convert.ToInt32(row["lead_time"]) : 0,
                Status = Convert.ToBoolean(row["status"]),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserName = row["create_user_name"] != DBNull.Value ? row["create_user_name"].ToString() : null,
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                UpdateUserName = row["update_user_name"] != DBNull.Value ? row["update_user_name"].ToString() : null,
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
                    throw new ArgumentException("物料编码不能为空", "materialCode");
                }

                var materials = GetByCondition("material_code = @materialCode", 
                    DatabaseHelper.CreateParameter("@materialCode", materialCode));
                
                return materials.Count > 0 ? materials[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据物料编码获取物料失败，物料编码: {0}", materialCode), ex);
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
                LogManager.Error(string.Format("根据类别获取物料列表失败，类别: {0}", category), ex);
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
                    DatabaseHelper.CreateParameter("@materialName", string.Format("%{0}%", materialName)));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据名称搜索物料失败，关键字: {0}", materialName), ex);
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
                LogManager.Error(string.Format("检查物料编码是否存在失败，编码: {0}", materialCode), ex);
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
                    LogManager.Info(string.Format("物料库存更新成功，物料ID: {0}, 库存: {1}", materialId, stockQuantity));
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新物料库存失败，物料ID: {0}", materialId), ex);
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
                LogManager.Error(string.Format("检查物料编码是否存在失败，编码: {0}", materialCode), ex);
                throw new MESException("检查物料编码失败", ex);
            }
        }

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected override bool BuildInsertSql(MaterialInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO material_info
                          (material_code, material_name, material_type, category, specification, unit,
                           standard_cost, safety_stock, min_stock, max_stock, supplier, lead_time, status,
                           create_time, create_user_name, is_deleted)
                          VALUES
                          (@materialCode, @materialName, @materialType, @category, @specification, @unit,
                           @standardCost, @safetyStock, @minStock, @maxStock, @supplier, @leadTime, @status,
                           @createTime, @createUserName, @isDeleted)";

            parameters = new[]
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

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <param name="sql">输出SQL语句</param>
        /// <param name="parameters">输出参数数组</param>
        /// <returns>操作是否成功</returns>
        protected override bool BuildUpdateSql(MaterialInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE material_info SET
                          material_code = @materialCode, material_name = @materialName,
                          material_type = @materialType, category = @category, specification = @specification, unit = @unit,
                          standard_cost = @standardCost, safety_stock = @safetyStock,
                          min_stock = @minStock, max_stock = @maxStock, supplier = @supplier,
                          lead_time = @leadTime, status = @status,
                          update_time = @updateTime, update_user_name = @updateUserName
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
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

            return true;
        }

        #endregion
    }
}
