using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Base;
using MES.Models.Flow;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Flow
{
    /// <summary>
    /// 工艺流程数据访问类
    /// 提供工艺流程相关的数据库操作功能
    /// </summary>
    public class FlowDAL : BaseDAL<FlowInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "flow_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "FlowId"; }
        }

        /// <summary>
        /// 将DataRow转换为FlowInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>FlowInfo实体对象</returns>
        protected override FlowInfo MapRowToEntity(DataRow row)
        {
            return new FlowInfo
            {
                FlowId = Convert.ToInt32(row["flow_id"]),
                FlowNum = row["flow_num"] != DBNull.Value ? row["flow_num"].ToString() : null,
                FlowName = row["flow_name"] != DBNull.Value ? row["flow_name"].ToString() : null,
                Version = row["version"] != DBNull.Value ? row["version"].ToString() : null,
                ProductId = Convert.ToInt32(row["product_id"]),
                Status = Convert.ToInt32(row["status"]),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null
            };
        }

        /// <summary>
        /// 根据工艺流程编号获取工艺流程信息
        /// </summary>
        /// <param name="flowNum">工艺流程编号</param>
        /// <returns>工艺流程信息</returns>
        public FlowInfo GetByFlowNum(string flowNum)
        {
            try
            {
                if (string.IsNullOrEmpty(flowNum))
                {
                    throw new ArgumentException("工艺流程编号不能为空", "flowNum");
                }

                var flows = GetByCondition("flow_num = @flowNum",
                    DatabaseHelper.CreateParameter("@flowNum", flowNum));

                return flows.Count > 0 ? flows[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工艺流程编号获取工艺流程信息失败，工艺流程编号: {0}", flowNum), ex);
                throw new MESException("获取工艺流程信息失败", ex);
            }
        }

        /// <summary>
        /// 根据状态获取工艺流程列表
        /// </summary>
        /// <param name="status">状态(0:禁用,1:启用)</param>
        /// <returns>工艺流程列表</returns>
        public List<FlowInfo> GetByStatus(int status)
        {
            try
            {
                return GetByCondition("status = @status",
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取工艺流程列表失败，状态: {0}", status), ex);
                throw new MESException("获取工艺流程列表失败", ex);
            }
        }

        /// <summary>
        /// 根据产品ID获取工艺流程列表
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺流程列表</returns>
        public List<FlowInfo> GetByProductId(int productId)
        {
            try
            {
                return GetByCondition("product_id = @productId",
                    DatabaseHelper.CreateParameter("@productId", productId));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据产品ID获取工艺流程列表失败，产品ID: {0}", productId), ex);
                throw new MESException("获取工艺流程列表失败", ex);
            }
        }

        /// <summary>
        /// 根据工艺流程编号和版本获取工艺流程
        /// </summary>
        /// <param name="flowNum">工艺流程编号</param>
        /// <param name="version">版本号</param>
        /// <returns>工艺流程信息</returns>
        public FlowInfo GetByFlowNumAndVersion(string flowNum, string version)
        {
            try
            {
                if (string.IsNullOrEmpty(flowNum) || string.IsNullOrEmpty(version))
                {
                    throw new ArgumentException("工艺流程编号和版本不能为空");
                }

                var flows = GetByCondition("flow_num = @flowNum AND version = @version",
                    DatabaseHelper.CreateParameter("@flowNum", flowNum),
                    DatabaseHelper.CreateParameter("@version", version));

                return flows.Count > 0 ? flows[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工艺流程编号和版本获取工艺流程失败，编号: {0}, 版本: {1}", flowNum, version), ex);
                throw new MESException("获取工艺流程失败", ex);
            }
        }

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">工艺流程实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildInsertSql(FlowInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO flow_info
                          (flow_num, flow_name, version, product_id, 
                           status, create_time, update_time)
                          VALUES
                          (@flowNum, @flowName, @version, @productId, 
                           @status, @createTime, @updateTime)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@flowNum", entity.FlowNum),
                DatabaseHelper.CreateParameter("@flowName", entity.FlowName),
                DatabaseHelper.CreateParameter("@version", entity.Version),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">工艺流程实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildUpdateSql(FlowInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE flow_info SET
                          flow_num = @flowNum, 
                          flow_name = @flowName,
                          version = @version, 
                          product_id = @productId,
                          status = @status,
                          update_time = @updateTime
                          WHERE flow_id = @flowId";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@flowNum", entity.FlowNum),
                DatabaseHelper.CreateParameter("@flowName", entity.FlowName),
                DatabaseHelper.CreateParameter("@version", entity.Version),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@flowId", entity.FlowId)
            };

            return true;
        }
    }
}