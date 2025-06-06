using System;
using System.Collections.Generic;
using MES.BLL.Equipment;
using MES.DAL.Equipment;
using MES.Models.Equipment;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.Equipment
{
    /// <summary>
    /// 设备业务逻辑类
    /// 提供设备管理的业务逻辑功能
    /// </summary>
    public class EquipmentBLL : IEquipmentBLL
    {
        private readonly EquipmentDAL _equipmentDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentBLL()
        {
            _equipmentDAL = new EquipmentDAL();
        }

        /// <summary>
        /// 根据ID获取设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>设备信息</returns>
        public EquipmentInfo GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("设备ID必须大于0", nameof(id));
                }

                return _equipmentDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取设备信息失败，ID: {0}", id), ex);
                throw new MESException("获取设备信息失败", ex);
            }
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
                if (string.IsNullOrEmpty(equipmentCode))
                {
                    throw new ArgumentException("设备编码不能为空", nameof(equipmentCode));
                }

                return _equipmentDAL.GetByEquipmentCode(equipmentCode);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据设备编码获取设备信息失败，编码: {0}", equipmentCode), ex);
                throw new MESException("获取设备信息失败", ex);
            }
        }

        /// <summary>
        /// 获取所有设备列表
        /// </summary>
        /// <returns>设备列表</returns>
        public List<EquipmentInfo> GetAll()
        {
            try
            {
                return _equipmentDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有设备列表失败", ex);
                throw new MESException("获取设备列表失败", ex);
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
                if (workshopId <= 0)
                {
                    throw new ArgumentException("车间ID必须大于0", nameof(workshopId));
                }

                return _equipmentDAL.GetByWorkshopId(workshopId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间ID获取设备列表失败，车间ID: {0}", workshopId), ex);
                throw new MESException("获取设备列表失败", ex);
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
                return _equipmentDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取设备列表失败，状态: {0}", status), ex);
                throw new MESException("获取设备列表失败", ex);
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
                return _equipmentDAL.GetMaintenanceRequired();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取需要维护的设备列表失败", ex);
                throw new MESException("获取维护设备列表失败", ex);
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
                if (string.IsNullOrEmpty(keyword))
                {
                    return GetAll();
                }

                return _equipmentDAL.Search(keyword);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索设备失败，关键词: {0}", keyword), ex);
                throw new MESException("搜索设备失败", ex);
            }
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否添加成功</returns>
        public bool Add(EquipmentInfo equipment)
        {
            try
            {
                if (equipment == null)
                {
                    throw new ArgumentNullException(nameof(equipment));
                }

                // 业务验证
                ValidateEquipment(equipment);

                // 检查设备编码是否已存在
                if (_equipmentDAL.IsEquipmentCodeExists(equipment.EquipmentCode))
                {
                    throw new MESException(string.Format("设备编码 {0} 已存在", equipment.EquipmentCode));
                }

                return _equipmentDAL.Add(equipment);
            }
            catch (Exception ex)
            {
                LogManager.Error("添加设备失败", ex);
                throw new MESException("添加设备失败", ex);
            }
        }

        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>是否更新成功</returns>
        public bool Update(EquipmentInfo equipment)
        {
            try
            {
                if (equipment == null)
                {
                    throw new ArgumentNullException(nameof(equipment));
                }

                // 业务验证
                ValidateEquipment(equipment);

                // 检查设备编码是否已存在（排除当前设备）
                if (_equipmentDAL.IsEquipmentCodeExists(equipment.EquipmentCode, equipment.Id))
                {
                    throw new MESException(string.Format("设备编码 {0} 已存在", equipment.EquipmentCode));
                }

                return _equipmentDAL.Update(equipment);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新设备失败，ID: {0}", equipment != null ? equipment.Id.ToString() : "null"), ex);
                throw new MESException("更新设备失败", ex);
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("设备ID必须大于0", nameof(id));
                }

                return _equipmentDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除设备失败，ID: {0}", id), ex);
                throw new MESException("删除设备失败", ex);
            }
        }

        /// <summary>
        /// 验证设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        private void ValidateEquipment(EquipmentInfo equipment)
        {
            if (string.IsNullOrEmpty(equipment.EquipmentCode))
            {
                throw new ArgumentException("设备编码不能为空");
            }

            if (string.IsNullOrEmpty(equipment.EquipmentName))
            {
                throw new ArgumentException("设备名称不能为空");
            }

            if (equipment.EquipmentCode.Length > 50)
            {
                throw new ArgumentException("设备编码长度不能超过50个字符");
            }

            if (equipment.EquipmentName.Length > 100)
            {
                throw new ArgumentException("设备名称长度不能超过100个字符");
            }
        }
    }
}
