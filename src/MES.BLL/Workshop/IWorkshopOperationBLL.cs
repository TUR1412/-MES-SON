using System;
using System.Collections.Generic;
using MES.Models.Workshop;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 车间作业管理业务逻辑接口
    /// 定义车间作业管理的核心业务操作
    /// </summary>
    public interface IWorkshopOperationBLL
    {
        /// <summary>
        /// 获取所有车间作业信息
        /// </summary>
        /// <returns>车间作业信息列表</returns>
        List<WorkshopOperationInfo> GetAllOperations();

        /// <summary>
        /// 根据ID获取车间作业信息
        /// </summary>
        /// <param name="id">作业ID</param>
        /// <returns>车间作业信息，如果未找到则返回null</returns>
        WorkshopOperationInfo GetOperationById(int id);

        /// <summary>
        /// 根据作业编号获取车间作业信息
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>车间作业信息，如果未找到则返回null</returns>
        WorkshopOperationInfo GetOperationByOperationId(string operationId);

        /// <summary>
        /// 根据车间名称获取作业列表
        /// </summary>
        /// <param name="workshopName">车间名称</param>
        /// <returns>作业列表</returns>
        List<WorkshopOperationInfo> GetOperationsByWorkshop(string workshopName);

        /// <summary>
        /// 根据状态获取作业列表
        /// </summary>
        /// <param name="status">作业状态</param>
        /// <returns>指定状态的作业列表</returns>
        List<WorkshopOperationInfo> GetOperationsByStatus(int status);

        /// <summary>
        /// 添加车间作业信息
        /// </summary>
        /// <param name="operation">车间作业信息</param>
        /// <returns>操作是否成功</returns>
        bool AddOperation(WorkshopOperationInfo operation);

        /// <summary>
        /// 更新车间作业信息
        /// </summary>
        /// <param name="operation">车间作业信息</param>
        /// <returns>操作是否成功</returns>
        bool UpdateOperation(WorkshopOperationInfo operation);

        /// <summary>
        /// 删除车间作业信息（逻辑删除）
        /// </summary>
        /// <param name="id">作业ID</param>
        /// <returns>操作是否成功</returns>
        bool DeleteOperation(int id);

        /// <summary>
        /// 开始作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        bool StartOperation(string operationId);

        /// <summary>
        /// 暂停作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        bool PauseOperation(string operationId);

        /// <summary>
        /// 完成作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        bool CompleteOperation(string operationId);

        /// <summary>
        /// 停止作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        bool StopOperation(string operationId);
    }
}
