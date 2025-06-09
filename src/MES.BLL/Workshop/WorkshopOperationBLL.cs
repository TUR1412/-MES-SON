using System;
using System.Collections.Generic;
using MES.DAL.Workshop;
using MES.Models.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 车间作业管理业务逻辑实现类
    /// 实现车间作业管理的核心业务逻辑
    /// </summary>
    public class WorkshopOperationBLL : IWorkshopOperationBLL
    {
        private readonly WorkshopOperationDAL _workshopOperationDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkshopOperationBLL()
        {
            _workshopOperationDAL = new WorkshopOperationDAL();
        }

        /// <summary>
        /// 获取所有车间作业信息
        /// </summary>
        /// <returns>车间作业信息列表</returns>
        public List<WorkshopOperationInfo> GetAllOperations()
        {
            try
            {
                return _workshopOperationDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有车间作业信息失败", ex);
                throw new MESException("获取车间作业信息失败", ex);
            }
        }

        /// <summary>
        /// 根据ID获取车间作业信息
        /// </summary>
        /// <param name="id">作业ID</param>
        /// <returns>车间作业信息，如果未找到则返回null</returns>
        public WorkshopOperationInfo GetOperationById(int id)
        {
            try
            {
                return _workshopOperationDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取车间作业信息失败，ID: {0}", id), ex);
                throw new MESException(string.Format("获取车间作业信息失败，ID: {0}", id), ex);
            }
        }

        /// <summary>
        /// 根据作业编号获取车间作业信息
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>车间作业信息，如果未找到则返回null</returns>
        public WorkshopOperationInfo GetOperationByOperationId(string operationId)
        {
            try
            {
                if (string.IsNullOrEmpty(operationId))
                {
                    LogManager.Error("获取车间作业失败：作业编号不能为空");
                    return null;
                }

                return _workshopOperationDAL.GetByOperationId(operationId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据作业编号获取车间作业异常：{0}", ex.Message), ex);
                throw new MESException("根据作业编号获取车间作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据车间名称获取作业列表
        /// </summary>
        /// <param name="workshopName">车间名称</param>
        /// <returns>作业列表</returns>
        public List<WorkshopOperationInfo> GetOperationsByWorkshop(string workshopName)
        {
            try
            {
                if (string.IsNullOrEmpty(workshopName))
                {
                    LogManager.Error("根据车间获取作业失败：车间名称不能为空");
                    return new List<WorkshopOperationInfo>();
                }

                return _workshopOperationDAL.GetByWorkshopName(workshopName);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据车间获取作业异常：{0}", ex.Message), ex);
                throw new MESException("根据车间获取作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据状态获取作业列表
        /// </summary>
        /// <param name="status">作业状态</param>
        /// <returns>指定状态的作业列表</returns>
        public List<WorkshopOperationInfo> GetOperationsByStatus(int status)
        {
            try
            {
                return _workshopOperationDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取作业异常：{0}", ex.Message), ex);
                throw new MESException("根据状态获取作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 添加车间作业信息
        /// </summary>
        /// <param name="operation">车间作业信息</param>
        /// <returns>操作是否成功</returns>
        public bool AddOperation(WorkshopOperationInfo operation)
        {
            try
            {
                if (operation == null)
                {
                    LogManager.Error("添加车间作业失败：作业信息不能为空");
                    return false;
                }

                // 设置默认值
                operation.CreateTime = DateTime.Now;
                operation.UpdateTime = DateTime.Now;
                operation.IsDeleted = false;

                return _workshopOperationDAL.Add(operation);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加车间作业异常：{0}", ex.Message), ex);
                throw new MESException("添加车间作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新车间作业信息
        /// </summary>
        /// <param name="operation">车间作业信息</param>
        /// <returns>操作是否成功</returns>
        public bool UpdateOperation(WorkshopOperationInfo operation)
        {
            try
            {
                if (operation == null || operation.Id <= 0)
                {
                    LogManager.Error("更新车间作业失败：作业信息无效");
                    return false;
                }

                operation.UpdateTime = DateTime.Now;
                return _workshopOperationDAL.Update(operation);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新车间作业异常：{0}", ex.Message), ex);
                throw new MESException("更新车间作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 删除车间作业信息（逻辑删除）
        /// </summary>
        /// <param name="id">作业ID</param>
        /// <returns>操作是否成功</returns>
        public bool DeleteOperation(int id)
        {
            try
            {
                return _workshopOperationDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除车间作业异常：{0}", ex.Message), ex);
                throw new MESException("删除车间作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 开始作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        public bool StartOperation(string operationId)
        {
            try
            {
                var operation = GetOperationByOperationId(operationId);
                if (operation == null)
                {
                    LogManager.Error(string.Format("开始作业失败：作业编号 {0} 不存在", operationId));
                    return false;
                }

                if (operation.Status != 0 && operation.Status != 2) // 待开始或已暂停
                {
                    LogManager.Error(string.Format("开始作业失败：作业 {0} 状态不允许开始", operationId));
                    return false;
                }

                operation.Status = 1; // 进行中
                operation.StatusText = "进行中";
                operation.StartTime = DateTime.Now;
                operation.UpdateTime = DateTime.Now;

                bool result = UpdateOperation(operation);
                if (result)
                {
                    LogManager.Info(string.Format("成功开始作业：{0}", operationId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("开始作业异常：{0}", ex.Message), ex);
                throw new MESException("开始作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 暂停作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        public bool PauseOperation(string operationId)
        {
            try
            {
                var operation = GetOperationByOperationId(operationId);
                if (operation == null)
                {
                    LogManager.Error(string.Format("暂停作业失败：作业编号 {0} 不存在", operationId));
                    return false;
                }

                if (operation.Status != 1) // 进行中
                {
                    LogManager.Error(string.Format("暂停作业失败：作业 {0} 状态不允许暂停", operationId));
                    return false;
                }

                operation.Status = 2; // 已暂停
                operation.StatusText = "已暂停";
                operation.UpdateTime = DateTime.Now;

                bool result = UpdateOperation(operation);
                if (result)
                {
                    LogManager.Info(string.Format("成功暂停作业：{0}", operationId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("暂停作业异常：{0}", ex.Message), ex);
                throw new MESException("暂停作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 完成作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        public bool CompleteOperation(string operationId)
        {
            try
            {
                var operation = GetOperationByOperationId(operationId);
                if (operation == null)
                {
                    LogManager.Error(string.Format("完成作业失败：作业编号 {0} 不存在", operationId));
                    return false;
                }

                if (operation.Status != 1) // 进行中
                {
                    LogManager.Error(string.Format("完成作业失败：作业 {0} 状态不允许完成", operationId));
                    return false;
                }

                operation.Status = 3; // 已完成
                operation.StatusText = "已完成";
                operation.Progress = 100;
                operation.UpdateTime = DateTime.Now;

                bool result = UpdateOperation(operation);
                if (result)
                {
                    LogManager.Info(string.Format("成功完成作业：{0}", operationId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("完成作业异常：{0}", ex.Message), ex);
                throw new MESException("完成作业时发生异常", ex);
            }
        }

        /// <summary>
        /// 停止作业
        /// </summary>
        /// <param name="operationId">作业编号</param>
        /// <returns>操作是否成功</returns>
        public bool StopOperation(string operationId)
        {
            try
            {
                var operation = GetOperationByOperationId(operationId);
                if (operation == null)
                {
                    LogManager.Error(string.Format("停止作业失败：作业编号 {0} 不存在", operationId));
                    return false;
                }

                if (operation.Status != 1 && operation.Status != 2) // 进行中或已暂停
                {
                    LogManager.Error(string.Format("停止作业失败：作业 {0} 状态不允许停止", operationId));
                    return false;
                }

                operation.Status = 4; // 已停止
                operation.StatusText = "已停止";
                operation.UpdateTime = DateTime.Now;

                bool result = UpdateOperation(operation);
                if (result)
                {
                    LogManager.Info(string.Format("成功停止作业：{0}", operationId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("停止作业异常：{0}", ex.Message), ex);
                throw new MESException("停止作业时发生异常", ex);
            }
        }
    }
}
