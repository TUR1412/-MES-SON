// --- START OF FILE ProcessFlowBLL.cs ---

using MES.BLL.Base;
using MES.DAL.Material;
using MES.Models.Base;
using MES.Models.Material;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MES.BLL.Material
{
    /// <summary>
    /// 工艺流程业务逻辑层
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public class ProcessFlowBLL : BaseBLL
    {
        private readonly ProcessFlowDAL _processFlowDAL;
        private readonly ProcessRouteBLL _processRouteBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessFlowBLL()
        {
            _processFlowDAL = new ProcessFlowDAL();
            _processRouteBLL = new ProcessRouteBLL();
        }

        /// <summary>
        /// 获取所有工艺流程列表
        /// </summary>
        /// <returns>工艺流程列表</returns>
        public List<ProcessFlow> GetAllProcessFlows()
        {
            try
            {
                var dataTable = _processFlowDAL.GetAllProcessFlows();
                return ConvertDataTableToProcessFlowList(dataTable);
            }
            catch (Exception ex)
            {
                LogError("获取工艺流程列表失败", ex);
                throw new BusinessException("获取工艺流程列表失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 根据工艺包ID获取工艺流程列表
        /// </summary>
        /// <param name="packageId">工艺包ID</param>
        /// <returns>工艺流程列表</returns>
        public List<ProcessFlow> GetProcessFlowsByPackageId(int packageId)
        {
            try
            {
                if (packageId <= 0)
                {
                    throw new ArgumentException("工艺包ID必须大于0");
                }

                var dataTable = _processFlowDAL.GetProcessFlowsByPackageId(packageId);
                return ConvertDataTableToProcessFlowList(dataTable);
            }
            catch (Exception ex)
            {
                LogError(string.Format("获取工艺包ID为{0}的工艺流程列表失败", packageId), ex);
                throw new BusinessException("获取工艺流程列表失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获取工艺流程详情
        /// </summary>
        /// <param name="id">工艺流程ID</param>
        /// <returns>工艺流程详情</returns>
        public ProcessFlow GetProcessFlowById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("工艺流程ID必须大于0");
                }

                var dataTable = _processFlowDAL.GetProcessFlowById(id);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }

                var processFlow = ConvertDataRowToProcessFlow(dataTable.Rows[0]);
                
                // 加载关联的工艺路线
                processFlow.Routes = _processRouteBLL.GetProcessRoutesByFlowId(id);

                return processFlow;
            }
            catch (Exception ex)
            {
                LogError(string.Format("获取工艺流程详情失败，ID：{0}", id), ex);
                throw new BusinessException("获取工艺流程详情失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加工艺流程
        /// </summary>
        /// <param name="processFlow">工艺流程信息</param>
        /// <returns>操作结果</returns>
        public OperationResult AddProcessFlow(ProcessFlow processFlow)
        {
            try
            {
                // 数据验证
                var validationResult = processFlow.Validate();
                if (!validationResult.IsValid)
                {
                    return OperationResult.Error(validationResult.ErrorMessage);
                }

                // 检查编码是否重复
                if (IsFlowCodeExists(processFlow.FlowCode))
                {
                    return OperationResult.Error("工艺流程编码已存在");
                }

                // 设置创建信息
                processFlow.CreateTime = DateTime.Now;
                processFlow.IsDeleted = false;

                // 执行添加操作
                int newId = _processFlowDAL.AddProcessFlow(processFlow);
                if (newId > 0)
                {
                    processFlow.Id = newId;
                    return OperationResult.Success("工艺流程添加成功", processFlow);
                }
                else
                {
                    return OperationResult.Error("工艺流程添加失败");
                }
            }
            catch (Exception ex)
            {
                LogError("添加工艺流程失败", ex);
                return OperationResult.Error("添加工艺流程失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 更新工艺流程
        /// </summary>
        /// <param name="processFlow">工艺流程信息</param>
        /// <returns>操作结果</returns>
        public OperationResult UpdateProcessFlow(ProcessFlow processFlow)
        {
            try
            {
                // 数据验证
                var validationResult = processFlow.Validate();
                if (!validationResult.IsValid)
                {
                    return OperationResult.Error(validationResult.ErrorMessage);
                }

                // 检查编码是否重复（排除自身）
                if (IsFlowCodeExists(processFlow.FlowCode, processFlow.Id))
                {
                    return OperationResult.Error("工艺流程编码已存在");
                }

                // 设置更新信息
                processFlow.UpdateTime = DateTime.Now;

                // 执行更新操作
                bool success = _processFlowDAL.UpdateProcessFlow(processFlow);
                if (success)
                {
                    return OperationResult.Success("工艺流程更新成功", processFlow);
                }
                else
                {
                    return OperationResult.Error("工艺流程更新失败");
                }
            }
            catch (Exception ex)
            {
                LogError("更新工艺流程失败", ex);
                return OperationResult.Error("更新工艺流程失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 物理删除工艺流程
        /// </summary>
        /// <param name="id">工艺流程ID</param>
        /// <returns>操作结果</returns>
        public OperationResult PhysicalDeleteProcessFlow(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult.Error("工艺流程ID无效");
                }

                // 检查是否存在关联的工艺路线
                var routes = _processRouteBLL.GetProcessRoutesByFlowId(id);
                if (routes.Count > 0)
                {
                    return OperationResult.Error("该工艺流程下存在工艺路线，无法删除");
                }

                // 执行物理删除
                bool success = _processFlowDAL.PhysicalDeleteProcessFlow(id);
                if (success)
                {
                    return OperationResult.Success("工艺流程删除成功");
                }
                else
                {
                    return OperationResult.Error("工艺流程删除失败");
                }
            }
            catch (Exception ex)
            {
                LogError(string.Format("删除工艺流程失败，ID：{0}", id), ex);
                return OperationResult.Error("删除工艺流程失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 检查工艺流程编码是否存在
        /// </summary>
        /// <param name="flowCode">工艺流程编码</param>
        /// <param name="excludeId">排除的ID</param>
        /// <returns>是否存在</returns>
        public bool IsFlowCodeExists(string flowCode, int excludeId = 0)
        {
            try
            {
                return _processFlowDAL.IsFlowCodeExists(flowCode, excludeId);
            }
            catch (Exception ex)
            {
                LogError("检查工艺流程编码是否存在失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取工艺流程统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public ProcessFlowStatistics GetProcessFlowStatistics()
        {
            try
            {
                var dataTable = _processFlowDAL.GetProcessFlowStatistics();
                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new ProcessFlowStatistics
                    {
                        TotalCount = Convert.ToInt32(row["TotalCount"]),
                        ActiveCount = Convert.ToInt32(row["ActiveCount"]),
                        InactiveCount = Convert.ToInt32(row["InactiveCount"]),
                        DraftCount = Convert.ToInt32(row["DraftCount"]),
                        AverageRouteCount = Convert.ToDecimal(row["AverageRouteCount"]),
                        AverageTotalTime = Convert.ToDecimal(row["AverageTotalTime"])
                    };
                }
                return new ProcessFlowStatistics();
            }
            catch (Exception ex)
            {
                LogError("获取工艺流程统计信息失败", ex);
                return new ProcessFlowStatistics();
            }
        }

        /// <summary>
        /// 将DataTable转换为工艺流程列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>工艺流程列表</returns>
        private List<ProcessFlow> ConvertDataTableToProcessFlowList(DataTable dataTable)
        {
            var list = new List<ProcessFlow>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(ConvertDataRowToProcessFlow(row));
            }
            return list;
        }

        /// <summary>
        /// 将DataRow转换为工艺流程对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>工艺流程对象</returns>
        private ProcessFlow ConvertDataRowToProcessFlow(DataRow row)
        {
            return new ProcessFlow
            {
                Id = Convert.ToInt32(row["id"]),
                FlowCode = row["flow_code"].ToString(),
                FlowName = row["flow_name"].ToString(),
                ProcessPackageId = Convert.ToInt32(row["process_package_id"]),
                ProcessPackageName = row.Table.Columns.Contains("process_package_name") ? row["process_package_name"].ToString() : "",
                FlowNumber = Convert.ToInt32(row["flow_number"]),
                Version = row["version"].ToString(),
                Status = (ProcessFlowStatus)Convert.ToInt32(row["status"]),
                Description = row["description"].ToString(),
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserId = Convert.ToInt32(row["create_user_id"]),
                CreateUserName = row["create_user_name"].ToString(),
                UpdateTime = row["update_time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["update_time"]),
                UpdateUserId = row["update_user_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["update_user_id"]),
                UpdateUserName = row["update_user_name"].ToString(),
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }
    }
}

// --- END OF FILE ProcessFlowBLL.cs ---
