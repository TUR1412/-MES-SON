using System;
using System.Collections.Generic;
using System.Linq;
using MES.DAL.Workshop;
using MES.Models.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 车间管理业务逻辑实现类
    /// 实现车间管理的核心业务逻辑
    /// </summary>
    public class WorkshopBLL : IWorkshopBLL
    {
        private readonly WorkshopDAL _workshopDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkshopBLL()
        {
            _workshopDAL = new WorkshopDAL();
        }

        /// <summary>
        /// 添加车间信息
        /// </summary>
        /// <param name="workshop">车间信息</param>
        /// <returns>操作是否成功</returns>
        public bool AddWorkshop(WorkshopInfo workshop)
        {
            try
            {
                // 验证输入参数
                if (workshop == null)
                {
                    LogManager.Error("添加车间失败：车间信息不能为空");
                    return false;
                }

                // 业务规则验证
                string validationResult = ValidateWorkshop(workshop);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error(string.Format("添加车间失败：{0}", validationResult));
                    return false;
                }

                // 检查车间编码是否已存在
                if (IsWorkshopCodeExists(workshop.WorkshopCode))
                {
                    LogManager.Error(string.Format("添加车间失败：车间编码 {0} 已存在", workshop.WorkshopCode));
                    return false;
                }

                // 设置默认值
                workshop.CreateTime = DateTime.Now;
                workshop.UpdateTime = DateTime.Now;
                workshop.IsDeleted = false;

                // 如果未设置状态，默认为启用
                if (string.IsNullOrEmpty(workshop.Status))
                {
                    workshop.Status = "1"; // 使用数字状态码
                }

                // 调用DAL层添加
                bool result = _workshopDAL.Add(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功添加车间：{0} - {1}", workshop.WorkshopCode, workshop.WorkshopName));
                }
                else
                {
                    LogManager.Error(string.Format("添加车间失败：{0}", workshop.WorkshopCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加车间异常：{0}", ex.Message), ex);
                throw new MESException("添加车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID删除车间信息（逻辑删除）
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>操作是否成功</returns>
        public bool DeleteWorkshop(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("删除车间失败：ID无效");
                    return false;
                }

                // 检查车间是否存在
                var existingWorkshop = _workshopDAL.GetById(id);
                if (existingWorkshop == null)
                {
                    LogManager.Error(string.Format("删除车间失败：ID为 {0} 的车间不存在", id));
                    return false;
                }

                // 检查车间是否有正在进行的生产任务
                // TODO: 这里需要与生产订单模块集成，检查是否有正在进行的生产任务
                
                bool result = _workshopDAL.Delete(id);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功删除车间：ID={0}, 车间编码={1}", id, existingWorkshop.WorkshopCode));
                }
                else
                {
                    LogManager.Error(string.Format("删除车间失败：ID={0}", id));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除车间异常：{0}", ex.Message), ex);
                throw new MESException("删除车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新车间信息
        /// </summary>
        /// <param name="workshop">车间信息</param>
        /// <returns>操作是否成功</returns>
        public bool UpdateWorkshop(WorkshopInfo workshop)
        {
            try
            {
                if (workshop == null || workshop.Id <= 0)
                {
                    LogManager.Error("更新车间失败：车间信息无效");
                    return false;
                }

                // 验证业务规则
                string validationResult = ValidateWorkshop(workshop);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error(string.Format("更新车间失败：{0}", validationResult));
                    return false;
                }

                // 检查车间是否存在
                var existingWorkshop = _workshopDAL.GetById(workshop.Id);
                if (existingWorkshop == null)
                {
                    LogManager.Error(string.Format("更新车间失败：ID为 {0} 的车间不存在", workshop.Id));
                    return false;
                }

                // 检查车间编码是否与其他车间冲突
                if (IsWorkshopCodeExists(workshop.WorkshopCode, workshop.Id))
                {
                    LogManager.Error(string.Format("更新车间失败：车间编码 {0} 已被其他车间使用", workshop.WorkshopCode));
                    return false;
                }

                // 更新时间
                workshop.UpdateTime = DateTime.Now;

                bool result = _workshopDAL.Update(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功更新车间：{0} - {1}", workshop.WorkshopCode, workshop.WorkshopName));
                }
                else
                {
                    LogManager.Error(string.Format("更新车间失败：{0}", workshop.WorkshopCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新车间异常：{0}", ex.Message), ex);
                throw new MESException("更新车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID获取车间信息
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>车间信息，未找到返回null</returns>
        public WorkshopInfo GetWorkshopById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("获取车间失败：ID无效");
                    return null;
                }

                return _workshopDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取车间异常：{0}", ex.Message), ex);
                throw new MESException("获取车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据车间编码获取车间信息
        /// </summary>
        /// <param name="workshopCode">车间编码</param>
        /// <returns>车间信息，未找到返回null</returns>
        public WorkshopInfo GetWorkshopByCode(string workshopCode)
        {
            try
            {
                if (string.IsNullOrEmpty(workshopCode))
                {
                    LogManager.Error("获取车间失败：车间编码不能为空");
                    return null;
                }

                return _workshopDAL.GetByWorkshopCode(workshopCode);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间编码获取车间异常：{0}", ex.Message), ex);
                throw new MESException("根据车间编码获取车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取所有车间列表
        /// </summary>
        /// <returns>车间列表</returns>
        public List<WorkshopInfo> GetAllWorkshops()
        {
            try
            {
                return _workshopDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取所有车间异常：{0}", ex.Message), ex);
                throw new MESException("获取所有车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据状态获取车间列表
        /// </summary>
        /// <param name="status">车间状态</param>
        /// <returns>指定状态的车间列表</returns>
        public List<WorkshopInfo> GetWorkshopsByStatus(string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    LogManager.Error("根据状态获取车间失败：状态不能为空");
                    return new List<WorkshopInfo>();
                }

                return _workshopDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取车间异常：{0}", ex.Message), ex);
                throw new MESException("根据状态获取车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据车间类型获取车间列表
        /// </summary>
        /// <param name="workshopType">车间类型</param>
        /// <returns>指定类型的车间列表</returns>
        public List<WorkshopInfo> GetWorkshopsByType(string workshopType)
        {
            try
            {
                if (string.IsNullOrEmpty(workshopType))
                {
                    LogManager.Error("根据类型获取车间失败：车间类型不能为空");
                    return new List<WorkshopInfo>();
                }

                return _workshopDAL.GetByWorkshopType(workshopType);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据类型获取车间异常：{0}", ex.Message), ex);
                throw new MESException("根据类型获取车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 分页获取车间列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的车间列表</returns>
        public List<WorkshopInfo> GetWorkshopsByPage(int pageIndex, int pageSize, out int totalCount)
        {
            try
            {
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    LogManager.Error("分页获取车间失败：页码和每页记录数必须大于0");
                    totalCount = 0;
                    return new List<WorkshopInfo>();
                }

                // 简化实现：从所有车间中分页
                var allWorkshops = GetAllWorkshops();
                totalCount = allWorkshops.Count;
                return allWorkshops.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("分页获取车间异常：{0}", ex.Message), ex);
                totalCount = 0;
                throw new MESException("分页获取车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据条件搜索车间
        /// </summary>
        /// <param name="keyword">搜索关键词（车间编码、名称等）</param>
        /// <returns>匹配的车间列表</returns>
        public List<WorkshopInfo> SearchWorkshops(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return GetAllWorkshops();
                }

                // 简化实现：从所有车间中搜索
                var allWorkshops = GetAllWorkshops();
                return allWorkshops.Where(w => w.WorkshopName.Contains(keyword) || w.WorkshopCode.Contains(keyword)).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索车间异常：{0}", ex.Message), ex);
                throw new MESException("搜索车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 启用车间
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>操作是否成功</returns>
        public bool EnableWorkshop(int id)
        {
            try
            {
                var workshop = _workshopDAL.GetById(id);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("启用车间失败：ID为 {0} 的车间不存在", id));
                    return false;
                }

                if (workshop.Status == "1")
                {
                    LogManager.Info(string.Format("车间 {0} 已经是启用状态", workshop.WorkshopCode));
                    return true;
                }

                workshop.Status = "1";
                workshop.UpdateTime = DateTime.Now;

                bool result = _workshopDAL.Update(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功启用车间：{0}", workshop.WorkshopCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("启用车间异常：{0}", ex.Message), ex);
                throw new MESException("启用车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 禁用车间
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <param name="reason">禁用原因</param>
        /// <returns>操作是否成功</returns>
        public bool DisableWorkshop(int id, string reason)
        {
            try
            {
                var workshop = _workshopDAL.GetById(id);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("禁用车间失败：ID为 {0} 的车间不存在", id));
                    return false;
                }

                if (workshop.Status == "0")
                {
                    LogManager.Info(string.Format("车间 {0} 已经是禁用状态", workshop.WorkshopCode));
                    return true;
                }

                workshop.Status = "0";
                workshop.Description = string.IsNullOrEmpty(workshop.Description) ? 
                    string.Format("禁用原因：{0}", reason) : string.Format("{0}；禁用原因：{1}", workshop.Description, reason);
                workshop.UpdateTime = DateTime.Now;

                bool result = _workshopDAL.Update(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功禁用车间：{0}，原因：{1}", workshop.WorkshopCode, reason));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("禁用车间异常：{0}", ex.Message), ex);
                throw new MESException("禁用车间时发生异常", ex);
            }
        }

        /// <summary>
        /// 设置车间负责人
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <param name="managerId">负责人ID</param>
        /// <returns>操作是否成功</returns>
        public bool SetWorkshopManager(int id, int managerId)
        {
            try
            {
                var workshop = _workshopDAL.GetById(id);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("设置车间负责人失败：ID为 {0} 的车间不存在", id));
                    return false;
                }

                // TODO: 这里需要验证managerId是否为有效的用户ID

                workshop.ManagerId = managerId;
                workshop.UpdateTime = DateTime.Now;

                bool result = _workshopDAL.Update(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功设置车间 {0} 的负责人：{1}", workshop.WorkshopCode, managerId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设置车间负责人异常：{0}", ex.Message), ex);
                throw new MESException("设置车间负责人时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新车间产能信息
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <param name="capacity">产能</param>
        /// <returns>操作是否成功</returns>
        public bool UpdateWorkshopCapacity(int id, int capacity)
        {
            try
            {
                if (capacity <= 0)
                {
                    LogManager.Error("更新车间产能失败：产能必须大于0");
                    return false;
                }

                var workshop = _workshopDAL.GetById(id);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("更新车间产能失败：ID为 {0} 的车间不存在", id));
                    return false;
                }

                workshop.ProductionCapacity = capacity;
                workshop.UpdateTime = DateTime.Now;

                bool result = _workshopDAL.Update(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功更新车间 {0} 的产能：{1}", workshop.WorkshopCode, capacity));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新车间产能异常：{0}", ex.Message), ex);
                throw new MESException("更新车间产能时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取车间当前工作负载
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>当前工作负载百分比</returns>
        public decimal GetWorkshopWorkload(int id)
        {
            try
            {
                var workshop = _workshopDAL.GetById(id);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("获取车间工作负载失败：ID为 {0} 的车间不存在", id));
                    return 0;
                }

                // TODO: 这里需要与生产订单模块集成，计算当前工作负载
                // 暂时返回模拟数据
                return 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取车间工作负载异常：{0}", ex.Message), ex);
                throw new MESException("获取车间工作负载时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取车间设备列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>设备列表</returns>
        public List<string> GetWorkshopEquipments(int workshopId)
        {
            try
            {
                var workshop = _workshopDAL.GetById(workshopId);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("获取车间设备失败：ID为 {0} 的车间不存在", workshopId));
                    return new List<string>();
                }

                // 解析设备列表字符串
                if (string.IsNullOrEmpty(workshop.EquipmentList))
                {
                    return new List<string>();
                }

                return workshop.EquipmentList.Split(',').Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取车间设备异常：{0}", ex.Message), ex);
                throw new MESException("获取车间设备时发生异常", ex);
            }
        }

        /// <summary>
        /// 添加车间设备
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>操作是否成功</returns>
        public bool AddWorkshopEquipment(int workshopId, string equipmentCode)
        {
            try
            {
                if (string.IsNullOrEmpty(equipmentCode))
                {
                    LogManager.Error("添加车间设备失败：设备编码不能为空");
                    return false;
                }

                var workshop = _workshopDAL.GetById(workshopId);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("添加车间设备失败：ID为 {0} 的车间不存在", workshopId));
                    return false;
                }

                var equipments = GetWorkshopEquipments(workshopId);
                if (equipments.Contains(equipmentCode))
                {
                    LogManager.Info(string.Format("设备 {0} 已存在于车间 {1} 中", equipmentCode, workshop.WorkshopCode));
                    return true;
                }

                equipments.Add(equipmentCode);
                workshop.EquipmentList = string.Join(",", equipments);
                workshop.UpdateTime = DateTime.Now;

                bool result = _workshopDAL.Update(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功为车间 {0} 添加设备：{1}", workshop.WorkshopCode, equipmentCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加车间设备异常：{0}", ex.Message), ex);
                throw new MESException("添加车间设备时发生异常", ex);
            }
        }

        /// <summary>
        /// 移除车间设备
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>操作是否成功</returns>
        public bool RemoveWorkshopEquipment(int workshopId, string equipmentCode)
        {
            try
            {
                if (string.IsNullOrEmpty(equipmentCode))
                {
                    LogManager.Error("移除车间设备失败：设备编码不能为空");
                    return false;
                }

                var workshop = _workshopDAL.GetById(workshopId);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("移除车间设备失败：ID为 {0} 的车间不存在", workshopId));
                    return false;
                }

                var equipments = GetWorkshopEquipments(workshopId);
                if (!equipments.Contains(equipmentCode))
                {
                    LogManager.Info(string.Format("设备 {0} 不存在于车间 {1} 中", equipmentCode, workshop.WorkshopCode));
                    return true;
                }

                equipments.Remove(equipmentCode);
                workshop.EquipmentList = string.Join(",", equipments);
                workshop.UpdateTime = DateTime.Now;

                bool result = _workshopDAL.Update(workshop);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功从车间 {0} 移除设备：{1}", workshop.WorkshopCode, equipmentCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("移除车间设备异常：{0}", ex.Message), ex);
                throw new MESException("移除车间设备时发生异常", ex);
            }
        }

        /// <summary>
        /// 验证车间数据
        /// </summary>
        /// <param name="workshop">车间信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        public string ValidateWorkshop(WorkshopInfo workshop)
        {
            if (workshop == null)
            {
                return "车间信息不能为空";
            }

            if (string.IsNullOrEmpty(workshop.WorkshopCode))
            {
                return "车间编码不能为空";
            }

            if (string.IsNullOrEmpty(workshop.WorkshopName))
            {
                return "车间名称不能为空";
            }

            if (workshop.ProductionCapacity <= 0)
            {
                return "车间产能必须大于0";
            }

            return string.Empty;
        }

        /// <summary>
        /// 检查车间编码是否已存在
        /// </summary>
        /// <param name="workshopCode">车间编码</param>
        /// <param name="excludeId">排除的车间ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        public bool IsWorkshopCodeExists(string workshopCode, int excludeId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(workshopCode))
                {
                    return false;
                }

                var existingWorkshop = _workshopDAL.GetByWorkshopCode(workshopCode);
                if (existingWorkshop == null)
                {
                    return false;
                }

                return excludeId == 0 || existingWorkshop.Id != excludeId;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("检查车间编码是否存在异常：{0}", ex.Message), ex);
                return false;
            }
        }

        /// <summary>
        /// 获取车间统计信息
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, object> GetWorkshopStatistics(int workshopId)
        {
            try
            {
                var workshop = _workshopDAL.GetById(workshopId);
                if (workshop == null)
                {
                    LogManager.Error(string.Format("获取车间统计信息失败：ID为 {0} 的车间不存在", workshopId));
                    return new Dictionary<string, object>();
                }

                var statistics = new Dictionary<string, object>();
                statistics.Add("WorkshopId", workshopId);
                statistics.Add("WorkshopCode", workshop.WorkshopCode);
                statistics.Add("WorkshopName", workshop.WorkshopName);
                statistics.Add("Status", workshop.Status);
                statistics.Add("ProductionCapacity", workshop.ProductionCapacity);
                statistics.Add("CurrentWorkload", GetWorkshopWorkload(workshopId));
                statistics.Add("EquipmentCount", GetWorkshopEquipments(workshopId).Count);
                statistics.Add("CreateTime", workshop.CreateTime);
                statistics.Add("UpdateTime", workshop.UpdateTime);

                // TODO: 添加更多统计信息，如生产订单数量、完成率等

                return statistics;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取车间统计信息异常：{0}", ex.Message), ex);
                throw new MESException("获取车间统计信息时发生异常", ex);
            }
        }
    }
}
