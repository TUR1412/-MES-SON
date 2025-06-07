using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Base;
using MES.Models.Oper;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Oper
{
    /// <summary>
    /// 工站数据访问类
    /// 提供工站相关的数据库操作功能
    /// </summary>
    public class OperDAL : BaseDAL<OperInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "oper_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "OperId"; }
        }

        /// <summary>
        /// 将DataRow转换为OperInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>OperInfo实体对象</returns>
        protected override OperInfo MapRowToEntity(DataRow row)
        {
            return new OperInfo
            {
                OperId = Convert.ToInt32(row["oper_id"]),
                OperNum = row["oper_num"] != DBNull.Value ? row["oper_num"].ToString() : null,
                OperName = row["oper_name"] != DBNull.Value ? row["oper_name"].ToString() : null,
                FortoryId = Convert.ToInt32(row["fortory_id"]),
                FlowId = Convert.ToInt32(row["flow_id"]),
                Sequence = Convert.ToInt32(row["sequence"]),
                Version = row["version"] != DBNull.Value ? row["version"].ToString() : null,
                Status = Convert.ToInt32(row["status"]),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null
            };
        }

        /// <summary>
        /// 根据工站编号获取工站信息
        /// </summary>
        /// <param name="operNum">工站编号</param>
        /// <returns>工站信息</returns>
        public OperInfo GetByOperNum(string operNum)
        {
            try
            {
                if (string.IsNullOrEmpty(operNum))
                {
                    throw new ArgumentException("工站编号不能为空", "operNum");
                }

                var opers = GetByCondition("oper_num = @operNum",
                    DatabaseHelper.CreateParameter("@operNum", operNum));

                return opers.Count > 0 ? opers[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据工站编号获取工站信息失败，工站编号: {operNum}", ex);
                throw new MESException("获取工站信息失败", ex);
            }
        }

        /// <summary>
        /// 根据工艺流程ID获取工站列表
        /// </summary>
        /// <param name="flowId">工艺流程ID</param>
        /// <returns>工站列表</returns>
        public List<OperInfo> GetByFlowId(int flowId)
        {
            try
            {
                return GetByCondition("flow_id = @flowId ORDER BY sequence",
                    DatabaseHelper.CreateParameter("@flowId", flowId));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据工艺流程ID获取工站列表失败，工艺流程ID: {flowId}", ex);
                throw new MESException("获取工站列表失败", ex);
            }
        }

        /// <summary>
        /// 根据状态获取工站列表
        /// </summary>
        /// <param name="status">状态(0:禁用,1:启用)</param>
        /// <returns>工站列表</returns>
        public List<OperInfo> GetByStatus(int status)
        {
            try
            {
                return GetByCondition("status = @status",
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据状态获取工站列表失败，状态: {status}", ex);
                throw new MESException("获取工站列表失败", ex);
            }
        }

        /// <summary>
        /// 根据工艺流程ID和版本获取工站列表
        /// </summary>
        /// <param name="flowId">工艺流程ID</param>
        /// <param name="version">版本号</param>
        /// <returns>工站列表</returns>
        public List<OperInfo> GetByFlowIdAndVersion(int flowId, string version)
        {
            try
            {
                if (string.IsNullOrEmpty(version))
                {
                    throw new ArgumentException("版本号不能为空", "version");
                }

                return GetByCondition("flow_id = @flowId AND version = @version ORDER BY sequence",
                    DatabaseHelper.CreateParameter("@flowId", flowId),
                    DatabaseHelper.CreateParameter("@version", version));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据工艺流程ID和版本获取工站列表失败，工艺流程ID: {flowId}, 版本: {version}", ex);
                throw new MESException("获取工站列表失败", ex);
            }
        }

        /// <summary>
        /// 更新工站顺序
        /// </summary>
        /// <param name="operId">工站ID</param>
        /// <param name="newSequence">新顺序</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateOperSequence(int operId, int newSequence)
        {
            try
            {
                string sql = "UPDATE oper_info SET sequence = @sequence, update_time = @updateTime WHERE oper_id = @operId";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@sequence", newSequence),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@operId", operId)
                };

                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新工站顺序失败，工站ID: {operId}, 新顺序: {newSequence}", ex);
                throw new MESException("更新工站顺序失败", ex);
            }
        }

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">工站实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildInsertSql(OperInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO oper_info
                          (oper_num, oper_name, fortery_id, flow_id, sequence, 
                           version, status, create_time, update_time)
                          VALUES
                          (@operNum, @operName, @fortoryId, @flowId, @sequence, 
                           @version, @status, @createTime, @updateTime)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@operNum", entity.OperNum),
                DatabaseHelper.CreateParameter("@operName", entity.OperName),
                DatabaseHelper.CreateParameter("@fortoryId", entity.FortoryId),
                DatabaseHelper.CreateParameter("@flowId", entity.FlowId),
                DatabaseHelper.CreateParameter("@sequence", entity.Sequence),
                DatabaseHelper.CreateParameter("@version", entity.Version),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">工站实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildUpdateSql(OperInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE oper_info SET
                          oper_num = @operNum, 
                          oper_name = @operName,
                          fortery_id = @fortoryId,
                          flow_id = @flowId, 
                          sequence = @sequence,
                          version = @version, 
                          status = @status,
                          update_time = @updateTime
                          WHERE oper_id = @operId";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@operNum", entity.OperNum),
                DatabaseHelper.CreateParameter("@operName", entity.OperName),
                DatabaseHelper.CreateParameter("@fortoryId", entity.FortoryId),
                DatabaseHelper.CreateParameter("@flowId", entity.FlowId),
                DatabaseHelper.CreateParameter("@sequence", entity.Sequence),
                DatabaseHelper.CreateParameter("@version", entity.Version),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@operId", entity.OperId)
            };

            return true;
        }
    }
}