using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MES.Models.Base;
using MES.Models.Workshop;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.Workshop
{
    /// <summary>
    /// 车间信息数据访问类
    /// 提供车间相关的数据库操作功能
    /// 注意：此类为S成员开发的模板，具体实现需要S成员根据业务需求完善
    /// </summary>
    public class WorkshopDAL : BaseDAL<WorkshopInfo>
    {
        #region 基类实现

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName => "workshop_info";

        #endregion

        #region 车间特有操作

        /// <summary>
        /// 根据车间编码获取车间信息
        /// </summary>
        /// <param name="workshopCode">车间编码</param>
        /// <returns>车间信息</returns>
        public WorkshopInfo GetByWorkshopCode(string workshopCode)
        {
            try
            {
                if (string.IsNullOrEmpty(workshopCode))
                {
                    throw new ArgumentException("车间编码不能为空", nameof(workshopCode));
                }

                var workshops = GetByCondition("workshop_code = @workshopCode", 
                    DatabaseHelper.CreateParameter("@workshopCode", workshopCode));
                
                return workshops.Count > 0 ? workshops[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据车间编码获取车间信息失败，车间编码: {workshopCode}", ex);
                throw new MESException("获取车间信息失败", ex);
            }
        }

        /// <summary>
        /// 根据车间状态获取车间列表
        /// </summary>
        /// <param name="status">车间状态</param>
        /// <returns>车间列表</returns>
        public List<WorkshopInfo> GetByStatus(bool status)
        {
            try
            {
                return GetByCondition("status = @status",
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据状态获取车间列表失败，状态: {status}", ex);
                throw new MESException("获取车间列表失败", ex);
            }
        }

        /// <summary>
        /// 根据车间类型获取车间列表
        /// </summary>
        /// <param name="workshopType">车间类型</param>
        /// <returns>车间列表</returns>
        public List<WorkshopInfo> GetByWorkshopType(string workshopType)
        {
            try
            {
                if (string.IsNullOrEmpty(workshopType))
                {
                    return new List<WorkshopInfo>();
                }

                return GetByCondition("workshop_type = @workshopType",
                    DatabaseHelper.CreateParameter("@workshopType", workshopType));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据类型获取车间列表失败，类型: {workshopType}", ex);
                throw new MESException("获取车间列表失败", ex);
            }
        }

        // TODO: S成员需要根据具体业务需求实现以下方法：
        // - GetByDepartment: 根据部门查询车间
        // - GetWorkshopCapacity: 获取车间产能信息
        // - GetEquipmentList: 获取车间设备列表
        // - UpdateWorkshopStatus: 更新车间状态
        // - GetWorkshopStatistics: 获取车间统计信息
        // - 其他业务相关方法

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// 注意：S成员需要根据实际表结构完善此方法
        /// </summary>
        /// <param name="entity">车间实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, MySqlParameter[] parameters) BuildInsertSql(WorkshopInfo entity)
        {
            string sql = @"INSERT INTO workshop_info
                          (workshop_code, workshop_name, manager_id, capacity, status,
                           location_id, workshop_type, department, description, equipment_list,
                           create_time, create_user_name, is_deleted)
                          VALUES
                          (@workshopCode, @workshopName, @managerId, @capacity, @status,
                           @locationId, @workshopType, @department, @description, @equipmentList,
                           @createTime, @createUserName, @isDeleted)";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@workshopCode", entity.WorkshopCode),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@managerId", entity.ManagerId),
                DatabaseHelper.CreateParameter("@capacity", entity.Capacity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@locationId", entity.LocationId),
                DatabaseHelper.CreateParameter("@workshopType", entity.WorkshopType),
                DatabaseHelper.CreateParameter("@department", entity.Department),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@equipmentList", entity.EquipmentList),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
            };

            return (sql, parameters);
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// 注意：S成员需要根据实际表结构完善此方法
        /// </summary>
        /// <param name="entity">车间实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, MySqlParameter[] parameters) BuildUpdateSql(WorkshopInfo entity)
        {
            string sql = @"UPDATE workshop_info SET
                          workshop_code = @workshopCode, workshop_name = @workshopName,
                          manager_id = @managerId, capacity = @capacity, status = @status,
                          location_id = @locationId, workshop_type = @workshopType,
                          department = @department, description = @description,
                          equipment_list = @equipmentList,
                          update_time = @updateTime, update_user_name = @updateUserName
                          WHERE id = @id AND is_deleted = 0";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@workshopCode", entity.WorkshopCode),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@managerId", entity.ManagerId),
                DatabaseHelper.CreateParameter("@capacity", entity.Capacity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@locationId", entity.LocationId),
                DatabaseHelper.CreateParameter("@workshopType", entity.WorkshopType),
                DatabaseHelper.CreateParameter("@department", entity.Department),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@equipmentList", entity.EquipmentList),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return (sql, parameters);
        }

        #endregion
    }

    // TODO: S成员需要在MES.Models.Workshop命名空间中创建WorkshopInfo模型类
}
