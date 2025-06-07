using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Base;
using MES.Models.Factory;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Factory
{
    /// <summary>
    /// 工厂信息数据访问类
    /// 提供工厂相关的数据库操作功能
    /// </summary>
    public class FactoryDAL : BaseDAL<FactoryInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "factory_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "FactoryId"; }
        }

        /// <summary>
        /// 将DataRow转换为FactoryInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>FactoryInfo实体对象</returns>
        protected override FactoryInfo MapRowToEntity(DataRow row)
        {
            return new FactoryInfo
            {
                FortoryId = Convert.ToInt32(row["factory_id"]),
                FactoryNum = row["factory_num"] != DBNull.Value ? row["factory_num"].ToString() : null,
                FactoryName = row["factory_name"] != DBNull.Value ? row["factory_name"].ToString() : null,
                Address = row["address"] != DBNull.Value ? row["address"].ToString() : null,
                ContactPerson = row["contact_person"] != DBNull.Value ? row["contact_person"].ToString() : null,
                ContactPhone = row["contact_phone"] != DBNull.Value ? row["contact_phone"].ToString() : null,
                Status = Convert.ToInt32(row["status"]),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null
            };
        }

        /// <summary>
        /// 根据工厂编号获取工厂信息
        /// </summary>
        /// <param name="factoryNum">工厂编号</param>
        /// <returns>工厂信息</returns>
        public FactoryInfo GetByFactoryNum(string factoryNum)
        {
            try
            {
                if (string.IsNullOrEmpty(factoryNum))
                {
                    throw new ArgumentException("工厂编号不能为空", "factoryNum");
                }

                var factories = GetByCondition("factory_num = @factoryNum",
                    DatabaseHelper.CreateParameter("@factoryNum", factoryNum));

                return factories.Count > 0 ? factories[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工厂编号获取工厂信息失败，工厂编号: {0}", factoryNum), ex);
                throw new MESException("获取工厂信息失败", ex);
            }
        }

        /// <summary>
        /// 根据状态获取工厂列表
        /// </summary>
        /// <param name="status">状态(0:禁用,1:启用)</param>
        /// <returns>工厂列表</returns>
        public List<FactoryInfo> GetByStatus(int status)
        {
            try
            {
                return GetByCondition("status = @status",
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取工厂列表失败，状态: {0}", status), ex);
                throw new MESException("获取工厂列表失败", ex);
            }
        }

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">工厂实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildInsertSql(FactoryInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO factory_info
                          (factory_num, factory_name, address, contact_person, 
                           contact_phone, status, create_time, update_time)
                          VALUES
                          (@factoryNum, @factoryName, @address, @contactPerson, 
                           @contactPhone, @status, @createTime, @updateTime)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@factoryNum", entity.FactoryNum),
                DatabaseHelper.CreateParameter("@factoryName", entity.FactoryName),
                DatabaseHelper.CreateParameter("@address", entity.Address),
                DatabaseHelper.CreateParameter("@contactPerson", entity.ContactPerson),
                DatabaseHelper.CreateParameter("@contactPhone", entity.ContactPhone),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">工厂实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildUpdateSql(FactoryInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE factory_info SET
                          factory_num = @factoryNum, 
                          factory_name = @factoryName,
                          address = @address, 
                          contact_person = @contactPerson,
                          contact_phone = @contactPhone, 
                          status = @status,
                          update_time = @updateTime
                          WHERE factory_id = @factoryId";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@factoryNum", entity.FactoryNum),
                DatabaseHelper.CreateParameter("@factoryName", entity.FactoryName),
                DatabaseHelper.CreateParameter("@address", entity.Address),
                DatabaseHelper.CreateParameter("@contactPerson", entity.ContactPerson),
                DatabaseHelper.CreateParameter("@contactPhone", entity.ContactPhone),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@factoryId", entity.FortoryId)
            };

            return true;
        }
    }
}