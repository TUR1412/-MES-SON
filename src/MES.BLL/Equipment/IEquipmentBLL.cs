using System;
using System.Collections.Generic;
using MES.Models.Equipment;

namespace MES.BLL.Equipment
{
    /// <summary>
    /// 设备管理业务逻辑接口
    /// 定义设备管理的核心业务操作
    /// </summary>
    public interface IEquipmentBLL
    {
        /// <summary>
        /// 添加设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>操作是否成功</returns>
        bool AddEquipment(EquipmentInfo equipment);

        /// <summary>
        /// 根据ID删除设备信息（逻辑删除）
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>操作是否成功</returns>
        bool DeleteEquipment(int id);

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>操作是否成功</returns>
        bool UpdateEquipment(EquipmentInfo equipment);

        /// <summary>
        /// 根据ID获取设备信息
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>设备信息，未找到返回null</returns>
        EquipmentInfo GetEquipmentById(int id);

        /// <summary>
        /// 根据设备编码获取设备信息
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns>设备信息，未找到返回null</returns>
        EquipmentInfo GetEquipmentByCode(string equipmentCode);

        /// <summary>
        /// 获取所有设备列表
        /// </summary>
        /// <returns>设备列表</returns>
        List<EquipmentInfo> GetAllEquipments();

        /// <summary>
        /// 根据车间ID获取设备列表
        /// </summary>
        /// <param name="workshopId">车间ID</param>
        /// <returns>设备列表</returns>
        List<EquipmentInfo> GetEquipmentsByWorkshopId(int workshopId);

        /// <summary>
        /// 根据状态获取设备列表
        /// </summary>
        /// <param name="status">设备状态</param>
        /// <returns>指定状态的设备列表</returns>
        List<EquipmentInfo> GetEquipmentsByStatus(int status);

        /// <summary>
        /// 分页获取设备列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的设备列表</returns>
        List<EquipmentInfo> GetEquipmentsByPage(int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 根据条件搜索设备
        /// </summary>
        /// <param name="keyword">搜索关键词（设备编码、名称等）</param>
        /// <returns>匹配的设备列表</returns>
        List<EquipmentInfo> SearchEquipments(string keyword);

        /// <summary>
        /// 获取需要维护的设备列表
        /// </summary>
        /// <returns>需要维护的设备列表</returns>
        List<EquipmentInfo> GetMaintenanceRequiredEquipments();

        /// <summary>
        /// 启用设备
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>操作是否成功</returns>
        bool EnableEquipment(int id);

        /// <summary>
        /// 停用设备
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="reason">停用原因</param>
        /// <returns>操作是否成功</returns>
        bool DisableEquipment(int id, string reason);

        /// <summary>
        /// 设备故障
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="faultDescription">故障描述</param>
        /// <returns>操作是否成功</returns>
        bool SetEquipmentFault(int id, string faultDescription);

        /// <summary>
        /// 设备维护
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="maintenanceDescription">维护描述</param>
        /// <returns>操作是否成功</returns>
        bool SetEquipmentMaintenance(int id, string maintenanceDescription);

        /// <summary>
        /// 完成设备维护
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <returns>操作是否成功</returns>
        bool CompleteEquipmentMaintenance(int id);

        /// <summary>
        /// 设置设备负责人
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="responsiblePersonId">负责人ID</param>
        /// <param name="responsiblePersonName">负责人姓名</param>
        /// <returns>操作是否成功</returns>
        bool SetEquipmentResponsiblePerson(int id, int responsiblePersonId, string responsiblePersonName);

        /// <summary>
        /// 转移设备到指定车间
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="targetWorkshopId">目标车间ID</param>
        /// <returns>操作是否成功</returns>
        bool TransferEquipment(int id, int targetWorkshopId);

        /// <summary>
        /// 验证设备数据
        /// </summary>
        /// <param name="equipment">设备信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        string ValidateEquipment(EquipmentInfo equipment);

        /// <summary>
        /// 检查设备编码是否已存在
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <param name="excludeId">排除的设备ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        bool IsEquipmentCodeExists(string equipmentCode, int excludeId = 0);

        /// <summary>
        /// 获取设备统计信息
        /// </summary>
        /// <param name="equipmentId">设备ID</param>
        /// <returns>统计信息字典</returns>
        Dictionary<string, object> GetEquipmentStatistics(int equipmentId);
    }
}
