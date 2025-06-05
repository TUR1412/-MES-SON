using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName => "bom_info";

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
                LogManager.Error($"根据产品ID获取BOM失败，产品ID: {productId}", ex);
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
                    throw new ArgumentException("BOM编码不能为空", nameof(bomCode));
                }

                var boms = GetByCondition("bom_code = @bomCode",
                    DatabaseHelper.CreateParameter("@bomCode", bomCode));

                return boms.Count > 0 ? boms[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据BOM编码获取BOM失败，BOM编码: {bomCode}", ex);
                throw new MESException("获取BOM信息失败", ex);
            }
        }

        // TODO: L成员需要根据实际业务需求实现BOM详细查询方法

        // TODO: L成员需要根据实际业务需求实现其他BOM操作方法

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">BOM实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, SqlParameter[] parameters) BuildInsertSql(BOMInfo entity)
        {
            string sql = @"INSERT INTO bom_info
                          (bom_code, product_id, bom_version, bom_type, effective_date, expire_date, status,
                           create_time, create_user_name, is_deleted)
                          VALUES
                          (@bomCode, @productId, @bomVersion, @bomType, @effectiveDate, @expireDate, @status,
                           @createTime, @createUserName, @isDeleted)";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@bomCode", entity.BOMCode),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@bomVersion", entity.BOMVersion),
                DatabaseHelper.CreateParameter("@bomType", entity.BOMType),
                DatabaseHelper.CreateParameter("@effectiveDate", entity.EffectiveDate),
                DatabaseHelper.CreateParameter("@expireDate", entity.ExpireDate),
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
        /// <param name="entity">BOM实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, SqlParameter[] parameters) BuildUpdateSql(BOMInfo entity)
        {
            string sql = @"UPDATE bom_info SET
                          bom_code = @bomCode, product_id = @productId, bom_version = @bomVersion,
                          bom_type = @bomType, effective_date = @effectiveDate, expire_date = @expireDate,
                          status = @status, update_time = @updateTime, update_user_name = @updateUserName
                          WHERE id = @id AND is_deleted = 0";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@bomCode", entity.BOMCode),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@bomVersion", entity.BOMVersion),
                DatabaseHelper.CreateParameter("@bomType", entity.BOMType),
                DatabaseHelper.CreateParameter("@effectiveDate", entity.EffectiveDate),
                DatabaseHelper.CreateParameter("@expireDate", entity.ExpireDate),
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
