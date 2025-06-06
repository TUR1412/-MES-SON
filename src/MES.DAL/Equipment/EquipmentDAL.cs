using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Models.Equipment;
using MES.Common.Logging;

namespace MES.DAL.Equipment
{
    /// <summary>
    /// 设备信息数据访问类
    /// 提供设备管理的数据库操作功能
    /// </summary>
    public class EquipmentDAL : BaseDAL<EquipmentInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "equipment"; }
        }

        /// <summary>
        /// 主键字段名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "id"; }
        }

        /// <summary>
        /// 将DataRow转换为EquipmentInfo对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>设备信息对象</returns>
        protected override EquipmentInfo MapRowToEntity(DataRow row)
        {
            return new EquipmentInfo
            {
                Id = Convert.ToInt32(row["id"]),
                EquipmentCode = row["equipment_code"] != DBNull.Value ? row["equipment_code"].ToString() : string.Empty,
                EquipmentName = row["equipment_name"] != DBNull.Value ? row["equipment_name"].ToString() : string.Empty,
                EquipmentType = row["equipment_type"] != DBNull.Value ? row["equipment_type"].ToString() : string.Empty,
                WorkshopId = row["workshop_id"] != DBNull.Value ? Convert.ToInt32(row["workshop_id"]) : 0,
                WorkshopName = row["workshop_name"] != DBNull.Value ? row["workshop_name"].ToString() : string.Empty,
                Status = Convert.ToInt32(row["status"]),
                Specification = row["specification"] != DBNull.Value ? row["specification"].ToString() : string.Empty,
                Manufacturer = row["manufacturer"] != DBNull.Value ? row["manufacturer"].ToString() : string.Empty,
                Model = row["model"] != DBNull.Value ? row["model"].ToString() : string.Empty,
                PurchaseDate = row["purchase_date"] != DBNull.Value ? Convert.ToDateTime(row["purchase_date"]) : (DateTime?)null,
                InstallDate = row["install_date"] != DBNull.Value ? Convert.ToDateTime(row["install_date"]) : (DateTime?)null,
                EnableDate = row["enable_date"] != DBNull.Value ? Convert.ToDateTime(row["enable_date"]) : (DateTime?)null,
                LastMaintenanceDate = row["last_maintenance_date"] != DBNull.Value ? Convert.ToDateTime(row["last_maintenance_date"]) : (DateTime?)null,
                NextMaintenanceDate = row["next_maintenance_date"] != DBNull.Value ? Convert.ToDateTime(row["next_maintenance_date"]) : (DateTime?)null,
                MaintenanceCycle = row["maintenance_cycle"] != DBNull.Value ? Convert.ToInt32(row["maintenance_cycle"]) : 30,
                Location = row["location"] != DBNull.Value ? row["location"].ToString() : string.Empty,
                ResponsiblePersonId = row["responsible_person_id"] != DBNull.Value ? Convert.ToInt32(row["responsible_person_id"]) : 0,
                ResponsiblePersonName = row["responsible_person_name"] != DBNull.Value ? row["responsible_person_name"].ToString() : string.Empty,
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : string.Empty,
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserId = row["create_user_id"] != DBNull.Value ? Convert.ToInt32(row["create_user_id"]) : 0,
                CreateUserName = row["create_user_name"] != DBNull.Value ? row["create_user_name"].ToString() : string.Empty,
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                UpdateUserId = row["update_user_id"] != DBNull.Value ? Convert.ToInt32(row["update_user_id"]) : 0,
                UpdateUserName = row["update_user_name"] != DBNull.Value ? row["update_user_name"].ToString() : string.Empty,
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        /// <summary>
        /// 获取插入SQL语句
        /// </summary>
        /// <returns>插入SQL</returns>
        protected override string GetInsertSql()
        {
            return @"INSERT INTO equipment 
                    (equipment_code, equipment_name, equipment_type, workshop_id, status, 
                     specification, manufacturer, model, purchase_date, install_date, enable_date,
                     last_maintenance_date, next_maintenance_date, maintenance_cycle, location,
                     responsible_person_id, responsible_person_name, description,
                     create_time, create_user_id, create_user_name, is_deleted) 
                    VALUES 
                    (@EquipmentCode, @EquipmentName, @EquipmentType, @WorkshopId, @Status,
                     @Specification, @Manufacturer, @Model, @PurchaseDate, @InstallDate, @EnableDate,
                     @LastMaintenanceDate, @NextMaintenanceDate, @MaintenanceCycle, @Location,
                     @ResponsiblePersonId, @ResponsiblePersonName, @Description,
                     @CreateTime, @CreateUserId, @CreateUserName, @IsDeleted)";
        }

        /// <summary>
        /// 获取更新SQL语句
        /// </summary>
        /// <returns>更新SQL</returns>
        protected override string GetUpdateSql()
        {
            return @"UPDATE equipment SET 
                    equipment_code = @EquipmentCode, 
                    equipment_name = @EquipmentName, 
                    equipment_type = @EquipmentType, 
                    workshop_id = @WorkshopId, 
                    status = @Status,
                    specification = @Specification,
                    manufacturer = @Manufacturer,
                    model = @Model,
                    purchase_date = @PurchaseDate,
                    install_date = @InstallDate,
                    enable_date = @EnableDate,
                    last_maintenance_date = @LastMaintenanceDate,
                    next_maintenance_date = @NextMaintenanceDate,
                    maintenance_cycle = @MaintenanceCycle,
                    location = @Location,
                    responsible_person_id = @ResponsiblePersonId,
                    responsible_person_name = @ResponsiblePersonName,
                    description = @Description,
                    update_time = @UpdateTime, 
                    update_user_id = @UpdateUserId, 
                    update_user_name = @UpdateUserName 
                    WHERE id = @Id";
        }

        /// <summary>
        /// 设置插入参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected override void SetInsertParameters(MySqlCommand cmd, EquipmentInfo entity)
        {
            cmd.Parameters.AddWithValue("@EquipmentCode", entity.EquipmentCode);
            cmd.Parameters.AddWithValue("@EquipmentName", entity.EquipmentName);
            cmd.Parameters.AddWithValue("@EquipmentType", entity.EquipmentType ?? string.Empty);
            cmd.Parameters.AddWithValue("@WorkshopId", entity.WorkshopId > 0 ? (object)entity.WorkshopId : DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", entity.Status);
            cmd.Parameters.AddWithValue("@Specification", entity.Specification ?? string.Empty);
            cmd.Parameters.AddWithValue("@Manufacturer", entity.Manufacturer ?? string.Empty);
            cmd.Parameters.AddWithValue("@Model", entity.Model ?? string.Empty);
            cmd.Parameters.AddWithValue("@PurchaseDate", entity.PurchaseDate.HasValue ? (object)entity.PurchaseDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@InstallDate", entity.InstallDate.HasValue ? (object)entity.InstallDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@EnableDate", entity.EnableDate.HasValue ? (object)entity.EnableDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@LastMaintenanceDate", entity.LastMaintenanceDate.HasValue ? (object)entity.LastMaintenanceDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@NextMaintenanceDate", entity.NextMaintenanceDate.HasValue ? (object)entity.NextMaintenanceDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@MaintenanceCycle", entity.MaintenanceCycle);
            cmd.Parameters.AddWithValue("@Location", entity.Location ?? string.Empty);
            cmd.Parameters.AddWithValue("@ResponsiblePersonId", entity.ResponsiblePersonId > 0 ? (object)entity.ResponsiblePersonId : DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponsiblePersonName", entity.ResponsiblePersonName ?? string.Empty);
            cmd.Parameters.AddWithValue("@Description", entity.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@CreateTime", entity.CreateTime);
            cmd.Parameters.AddWithValue("@CreateUserId", entity.CreateUserId);
            cmd.Parameters.AddWithValue("@CreateUserName", entity.CreateUserName ?? string.Empty);
            cmd.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted);
        }

        /// <summary>
        /// 设置更新参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected override void SetUpdateParameters(MySqlCommand cmd, EquipmentInfo entity)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@EquipmentCode", entity.EquipmentCode);
            cmd.Parameters.AddWithValue("@EquipmentName", entity.EquipmentName);
            cmd.Parameters.AddWithValue("@EquipmentType", entity.EquipmentType ?? string.Empty);
            cmd.Parameters.AddWithValue("@WorkshopId", entity.WorkshopId > 0 ? (object)entity.WorkshopId : DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", entity.Status);
            cmd.Parameters.AddWithValue("@Specification", entity.Specification ?? string.Empty);
            cmd.Parameters.AddWithValue("@Manufacturer", entity.Manufacturer ?? string.Empty);
            cmd.Parameters.AddWithValue("@Model", entity.Model ?? string.Empty);
            cmd.Parameters.AddWithValue("@PurchaseDate", entity.PurchaseDate.HasValue ? (object)entity.PurchaseDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@InstallDate", entity.InstallDate.HasValue ? (object)entity.InstallDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@EnableDate", entity.EnableDate.HasValue ? (object)entity.EnableDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@LastMaintenanceDate", entity.LastMaintenanceDate.HasValue ? (object)entity.LastMaintenanceDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@NextMaintenanceDate", entity.NextMaintenanceDate.HasValue ? (object)entity.NextMaintenanceDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@MaintenanceCycle", entity.MaintenanceCycle);
            cmd.Parameters.AddWithValue("@Location", entity.Location ?? string.Empty);
            cmd.Parameters.AddWithValue("@ResponsiblePersonId", entity.ResponsiblePersonId > 0 ? (object)entity.ResponsiblePersonId : DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponsiblePersonName", entity.ResponsiblePersonName ?? string.Empty);
            cmd.Parameters.AddWithValue("@Description", entity.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@UpdateTime", entity.UpdateTime);
            cmd.Parameters.AddWithValue("@UpdateUserId", entity.UpdateUserId);
            cmd.Parameters.AddWithValue("@UpdateUserName", entity.UpdateUserName ?? string.Empty);
        }

        /// <summary>
        /// 根据设备编码获取设备信息
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备信息</returns>
        public EquipmentInfo GetByEquipmentCode(string equipmentCode)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} WHERE equipment_code = @EquipmentCode AND is_deleted = 0", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@EquipmentCode", equipmentCode);
                        
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            
                            if (dataTable.Rows.Count > 0)
                            {
                                return MapRowToEntity(dataTable.Rows[0]);
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据设备编码获取设备信息失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 根据车间ID获取设备列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>设备列表</returns>
        public List<EquipmentInfo> GetByWorkshopId(int workshopId)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} WHERE workshop_id = @WorkshopId AND is_deleted = 0 ORDER BY equipment_code", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@WorkshopId", workshopId);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var equipments = new List<EquipmentInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                equipments.Add(MapRowToEntity(row));
                            }

                            return equipments;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间ID获取设备列表失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 根据状态获取设备列表
        /// </summary>
        /// <param name="status">设备状态</param>
        /// <returns>设备列表</returns>
        public List<EquipmentInfo> GetByStatus(int status)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} WHERE status = @Status AND is_deleted = 0 ORDER BY equipment_code", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var equipments = new List<EquipmentInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                equipments.Add(MapRowToEntity(row));
                            }

                            return equipments;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取设备列表失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 获取需要维护的设备列表
        /// </summary>
        /// <returns>需要维护的设备列表</returns>
        public List<EquipmentInfo> GetMaintenanceRequired()
        {
            try
            {
                string sql = string.Format(@"SELECT * FROM {0}
                               WHERE next_maintenance_date <= @CurrentDate
                               AND status = 1
                               AND is_deleted = 0
                               ORDER BY next_maintenance_date", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var equipments = new List<EquipmentInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                equipments.Add(MapRowToEntity(row));
                            }

                            return equipments;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取需要维护的设备列表失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 搜索设备
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>设备列表</returns>
        public List<EquipmentInfo> Search(string keyword)
        {
            try
            {
                string sql = string.Format(@"SELECT * FROM {0}
                               WHERE (equipment_code LIKE @Keyword OR equipment_name LIKE @Keyword
                                      OR equipment_type LIKE @Keyword OR manufacturer LIKE @Keyword
                                      OR model LIKE @Keyword OR location LIKE @Keyword)
                               AND is_deleted = 0
                               ORDER BY equipment_code", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", string.Format("%{0}%", keyword));

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var equipments = new List<EquipmentInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                equipments.Add(MapRowToEntity(row));
                            }

                            return equipments;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索设备失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 检查设备编码是否存在
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="excludeId">排除的ID</param>
        /// <returns>是否存在</returns>
        public bool IsEquipmentCodeExists(string equipmentCode, int excludeId = 0)
        {
            try
            {
                string sql = string.Format("SELECT COUNT(1) FROM {0} WHERE equipment_code = @EquipmentCode AND is_deleted = 0", TableName);
                if (excludeId > 0)
                {
                    sql += " AND id != @ExcludeId";
                }
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@EquipmentCode", equipmentCode);
                        if (excludeId > 0)
                        {
                            cmd.Parameters.AddWithValue("@ExcludeId", excludeId);
                        }

                        var count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("检查设备编码是否存在失败：{0}", ex.Message), ex);
                throw;
            }
        }
    }
}
