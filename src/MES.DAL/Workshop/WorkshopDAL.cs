using System;
using System.Collections.Generic;
using System.Data;
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
        protected override string TableName
        {
            get { return "workshop_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "Id"; }
        }

        /// <summary>
        /// 将DataRow转换为WorkshopInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>WorkshopInfo实体对象</returns>
        protected override WorkshopInfo MapRowToEntity(DataRow row)
        {
            return new WorkshopInfo
            {
                Id = Convert.ToInt32(row["id"]),
                WorkshopCode = row["workshop_code"]?.ToString(),
                WorkshopName = row["workshop_name"]?.ToString(),
                Department = row["department"]?.ToString(),
                Manager = row["manager"]?.ToString(),
                ManagerId = row["manager_id"] != DBNull.Value ? Convert.ToInt32(row["manager_id"]) : (int?)null,
                Phone = row["phone"]?.ToString(),
                Location = row["location"]?.ToString(),
                Area = row["area"] != DBNull.Value ? Convert.ToDecimal(row["area"]) : (decimal?)null,
                EquipmentCount = row["equipment_count"] != DBNull.Value ? Convert.ToInt32(row["equipment_count"]) : (int?)null,
                EmployeeCount = row["employee_count"] != DBNull.Value ? Convert.ToInt32(row["employee_count"]) : (int?)null,
                WorkshopType = row["workshop_type"]?.ToString(),
                ProductionCapacity = row["production_capacity"] != DBNull.Value ? Convert.ToDecimal(row["production_capacity"]) : (decimal?)null,
                Status = row["status"]?.ToString(),
                WorkShift = row["work_shift"]?.ToString(),
                SafetyLevel = row["safety_level"]?.ToString(),
                EnvironmentRequirement = row["environment_requirement"]?.ToString(),
                QualityStandard = row["quality_standard"]?.ToString(),
                Description = row["description"]?.ToString(),
                EquipmentList = row["equipment_list"]?.ToString(),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

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
                LogManager.Error(string.Format("根据车间编码获取车间信息失败，车间编码: {0}", workshopCode), ex);
                throw new MESException("获取车间信息失败", ex);
            }
        }

        /// <summary>
        /// 根据车间状态获取车间列表
        /// </summary>
        /// <param name="status">车间状态</param>
        /// <returns>车间列表</returns>
        public List<WorkshopInfo> GetByStatus(string status)
        {
            try
            {
                return GetByCondition("status = @status",
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取车间列表失败，状态: {0}", status), ex);
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
                LogManager.Error(string.Format("根据类型获取车间列表失败，类型: {0}", workshopType), ex);
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
        protected override bool BuildInsertSql(WorkshopInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO workshop_info
                          (workshop_code, workshop_name, department, manager, manager_id, phone,
                           location, area, equipment_count, employee_count, workshop_type,
                           production_capacity, status, work_shift, safety_level,
                           environment_requirement, quality_standard, description, equipment_list,
                           create_time, update_time, is_deleted)
                          VALUES
                          (@workshopCode, @workshopName, @department, @manager, @managerId, @phone,
                           @location, @area, @equipmentCount, @employeeCount, @workshopType,
                           @productionCapacity, @status, @workShift, @safetyLevel,
                           @environmentRequirement, @qualityStandard, @description, @equipmentList,
                           @createTime, @updateTime, @isDeleted)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@workshopCode", entity.WorkshopCode),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@department", entity.Department),
                DatabaseHelper.CreateParameter("@manager", entity.Manager),
                DatabaseHelper.CreateParameter("@managerId", entity.ManagerId),
                DatabaseHelper.CreateParameter("@phone", entity.Phone),
                DatabaseHelper.CreateParameter("@location", entity.Location),
                DatabaseHelper.CreateParameter("@area", entity.Area),
                DatabaseHelper.CreateParameter("@equipmentCount", entity.EquipmentCount),
                DatabaseHelper.CreateParameter("@employeeCount", entity.EmployeeCount),
                DatabaseHelper.CreateParameter("@workshopType", entity.WorkshopType),
                DatabaseHelper.CreateParameter("@productionCapacity", entity.ProductionCapacity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@workShift", entity.WorkShift),
                DatabaseHelper.CreateParameter("@safetyLevel", entity.SafetyLevel),
                DatabaseHelper.CreateParameter("@environmentRequirement", entity.EnvironmentRequirement),
                DatabaseHelper.CreateParameter("@qualityStandard", entity.QualityStandard),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@equipmentList", entity.EquipmentList),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// 注意：S成员需要根据实际表结构完善此方法
        /// </summary>
        /// <param name="entity">车间实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildUpdateSql(WorkshopInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE workshop_info SET
                          workshop_code = @workshopCode, workshop_name = @workshopName,
                          department = @department, manager = @manager, manager_id = @managerId,
                          phone = @phone, location = @location, area = @area,
                          equipment_count = @equipmentCount, employee_count = @employeeCount,
                          workshop_type = @workshopType, production_capacity = @productionCapacity,
                          status = @status, work_shift = @workShift, safety_level = @safetyLevel,
                          environment_requirement = @environmentRequirement, quality_standard = @qualityStandard,
                          description = @description, equipment_list = @equipmentList,
                          update_time = @updateTime
                          WHERE id = @id AND is_deleted = 0";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@workshopCode", entity.WorkshopCode),
                DatabaseHelper.CreateParameter("@workshopName", entity.WorkshopName),
                DatabaseHelper.CreateParameter("@department", entity.Department),
                DatabaseHelper.CreateParameter("@manager", entity.Manager),
                DatabaseHelper.CreateParameter("@managerId", entity.ManagerId),
                DatabaseHelper.CreateParameter("@phone", entity.Phone),
                DatabaseHelper.CreateParameter("@location", entity.Location),
                DatabaseHelper.CreateParameter("@area", entity.Area),
                DatabaseHelper.CreateParameter("@equipmentCount", entity.EquipmentCount),
                DatabaseHelper.CreateParameter("@employeeCount", entity.EmployeeCount),
                DatabaseHelper.CreateParameter("@workshopType", entity.WorkshopType),
                DatabaseHelper.CreateParameter("@productionCapacity", entity.ProductionCapacity),
                DatabaseHelper.CreateParameter("@status", entity.Status),
                DatabaseHelper.CreateParameter("@workShift", entity.WorkShift),
                DatabaseHelper.CreateParameter("@safetyLevel", entity.SafetyLevel),
                DatabaseHelper.CreateParameter("@environmentRequirement", entity.EnvironmentRequirement),
                DatabaseHelper.CreateParameter("@qualityStandard", entity.QualityStandard),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@equipmentList", entity.EquipmentList),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return true;
        }

        #endregion
    }

    // TODO: S成员需要在MES.Models.Workshop命名空间中创建WorkshopInfo模型类
}
