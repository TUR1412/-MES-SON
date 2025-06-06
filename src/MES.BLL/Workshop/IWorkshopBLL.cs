using System;
using System.Collections.Generic;
using MES.Models.Workshop;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 车间管理业务逻辑接口
    /// 定义车间管理的核心业务操作
    /// </summary>
    public interface IWorkshopBLL
    {
        /// <summary>
        /// 添加车间信息
        /// </summary>
        /// <param name="workshop">车间信息</param>
        /// <returns>操作是否成功</returns>
        bool AddWorkshop(WorkshopInfo workshop);

        /// <summary>
        /// 根据ID删除车间信息（逻辑删除）
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>操作是否成功</returns>
        bool DeleteWorkshop(int id);

        /// <summary>
        /// 更新车间信息
        /// </summary>
        /// <param name="workshop">车间信息</param>
        /// <returns>操作是否成功</returns>
        bool UpdateWorkshop(WorkshopInfo workshop);

        /// <summary>
        /// 根据ID获取车间信息
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>车间信息，未找到返回null</returns>
        WorkshopInfo GetWorkshopById(int id);

        /// <summary>
        /// 根据车间编码获取车间信息
        /// </summary>
        /// <param name="workshopCode">车间编码</param>
        /// <returns>车间信息，未找到返回null</returns>
        WorkshopInfo GetWorkshopByCode(string workshopCode);

        /// <summary>
        /// 获取所有车间列表
        /// </summary>
        /// <returns>车间列表</returns>
        List<WorkshopInfo> GetAllWorkshops();

        /// <summary>
        /// 根据状态获取车间列表
        /// </summary>
        /// <param name="status">车间状态</param>
        /// <returns>指定状态的车间列表</returns>
        List<WorkshopInfo> GetWorkshopsByStatus(string status);

        /// <summary>
        /// 根据车间类型获取车间列表
        /// </summary>
        /// <param name="workshopType">车间类型</param>
        /// <returns>指定类型的车间列表</returns>
        List<WorkshopInfo> GetWorkshopsByType(string workshopType);

        /// <summary>
        /// 分页获取车间列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的车间列表</returns>
        List<WorkshopInfo> GetWorkshopsByPage(int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 根据条件搜索车间
        /// </summary>
        /// <param name="keyword">搜索关键词（车间编码、名称等）</param>
        /// <returns>匹配的车间列表</returns>
        List<WorkshopInfo> SearchWorkshops(string keyword);

        /// <summary>
        /// 启用车间
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>操作是否成功</returns>
        bool EnableWorkshop(int id);

        /// <summary>
        /// 禁用车间
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <param name="reason">禁用原因</param>
        /// <returns>操作是否成功</returns>
        bool DisableWorkshop(int id, string reason);

        /// <summary>
        /// 设置车间负责人
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <param name="managerId">负责人ID</param>
        /// <returns>操作是否成功</returns>
        bool SetWorkshopManager(int id, int managerId);

        /// <summary>
        /// 更新车间产能信息
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <param name="capacity">产能</param>
        /// <returns>操作是否成功</returns>
        bool UpdateWorkshopCapacity(int id, int capacity);

        /// <summary>
        /// 获取车间当前工作负载
        /// </summary>
        /// <param name="id">车间ID</param>
        /// <returns>当前工作负载百分比</returns>
        decimal GetWorkshopWorkload(int id);

        /// <summary>
        /// 获取车间设备列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>设备列表</returns>
        List<string> GetWorkshopEquipments(int workshopId);

        /// <summary>
        /// 添加车间设备
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>操作是否成功</returns>
        bool AddWorkshopEquipment(int workshopId, string equipmentCode);

        /// <summary>
        /// 移除车间设备
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>操作是否成功</returns>
        bool RemoveWorkshopEquipment(int workshopId, string equipmentCode);

        /// <summary>
        /// 验证车间数据
        /// </summary>
        /// <param name="workshop">车间信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        string ValidateWorkshop(WorkshopInfo workshop);

        /// <summary>
        /// 检查车间编码是否已存在
        /// </summary>
        /// <param name="workshopCode">车间编码</param>
        /// <param name="excludeId">排除的车间ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        bool IsWorkshopCodeExists(string workshopCode, int excludeId = 0);

        /// <summary>
        /// 获取车间统计信息
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>统计信息字典</returns>
        Dictionary<string, object> GetWorkshopStatistics(int workshopId);
    }
}
