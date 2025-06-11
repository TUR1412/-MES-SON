// --- START OF FILE ProcessPackageBLL.cs ---

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
    /// 工艺包业务逻辑层
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public class ProcessPackageBLL : BaseBLL
    {
        private readonly ProcessPackageDAL _processPackageDAL;
        private readonly ProcessFlowBLL _processFlowBLL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessPackageBLL()
        {
            _processPackageDAL = new ProcessPackageDAL();
            _processFlowBLL = new ProcessFlowBLL();
        }

        /// <summary>
        /// 获取所有工艺包列表
        /// </summary>
        /// <returns>工艺包列表</returns>
        public List<ProcessPackage> GetAllProcessPackages()
        {
            try
            {
                var dataTable = _processPackageDAL.GetAllProcessPackages();
                return ConvertDataTableToProcessPackageList(dataTable);
            }
            catch (Exception ex)
            {
                LogError("获取工艺包列表失败", ex);
                throw new BusinessException("获取工艺包列表失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 根据产品ID获取工艺包列表
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺包列表</returns>
        public List<ProcessPackage> GetProcessPackagesByProductId(int productId)
        {
            try
            {
                if (productId <= 0)
                {
                    throw new ArgumentException("产品ID必须大于0");
                }

                var dataTable = _processPackageDAL.GetProcessPackagesByProductId(productId);
                return ConvertDataTableToProcessPackageList(dataTable);
            }
            catch (Exception ex)
            {
                LogError(string.Format("获取产品ID为{0}的工艺包列表失败", productId), ex);
                throw new BusinessException("获取工艺包列表失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获取工艺包详情
        /// </summary>
        /// <param name="id">工艺包ID</param>
        /// <returns>工艺包详情</returns>
        public ProcessPackage GetProcessPackageById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("工艺包ID必须大于0");
                }

                var dataTable = _processPackageDAL.GetProcessPackageById(id);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }

                var processPackage = ConvertDataRowToProcessPackage(dataTable.Rows[0]);
                
                // 加载关联的工艺流程
                processPackage.Flows = _processFlowBLL.GetProcessFlowsByPackageId(id);

                return processPackage;
            }
            catch (Exception ex)
            {
                LogError(string.Format("获取工艺包详情失败，ID：{0}", id), ex);
                throw new BusinessException("获取工艺包详情失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加工艺包
        /// </summary>
        /// <param name="processPackage">工艺包信息</param>
        /// <returns>操作结果</returns>
        public OperationResult AddProcessPackage(ProcessPackage processPackage)
        {
            try
            {
                // 数据验证
                var validationResult = processPackage.Validate();
                if (!validationResult.IsValid)
                {
                    return OperationResult.Error(validationResult.ErrorMessage);
                }

                // 检查编码是否重复
                if (IsPackageCodeExists(processPackage.PackageCode))
                {
                    return OperationResult.Error("工艺包编码已存在");
                }

                // 设置创建信息
                processPackage.CreateTime = DateTime.Now;
                processPackage.IsDeleted = false;

                // 执行添加操作
                int newId = _processPackageDAL.AddProcessPackage(processPackage);
                if (newId > 0)
                {
                    processPackage.Id = newId;
                    return OperationResult.Success("工艺包添加成功", processPackage);
                }
                else
                {
                    return OperationResult.Error("工艺包添加失败");
                }
            }
            catch (Exception ex)
            {
                LogError("添加工艺包失败", ex);
                return OperationResult.Error("添加工艺包失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 更新工艺包
        /// </summary>
        /// <param name="processPackage">工艺包信息</param>
        /// <returns>操作结果</returns>
        public OperationResult UpdateProcessPackage(ProcessPackage processPackage)
        {
            try
            {
                // 数据验证
                var validationResult = processPackage.Validate();
                if (!validationResult.IsValid)
                {
                    return OperationResult.Error(validationResult.ErrorMessage);
                }

                // 检查编码是否重复（排除自身）
                if (IsPackageCodeExists(processPackage.PackageCode, processPackage.Id))
                {
                    return OperationResult.Error("工艺包编码已存在");
                }

                // 设置更新信息
                processPackage.UpdateTime = DateTime.Now;

                // 执行更新操作
                bool success = _processPackageDAL.UpdateProcessPackage(processPackage);
                if (success)
                {
                    return OperationResult.Success("工艺包更新成功", processPackage);
                }
                else
                {
                    return OperationResult.Error("工艺包更新失败");
                }
            }
            catch (Exception ex)
            {
                LogError("更新工艺包失败", ex);
                return OperationResult.Error("更新工艺包失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 物理删除工艺包
        /// </summary>
        /// <param name="id">工艺包ID</param>
        /// <returns>操作结果</returns>
        public OperationResult PhysicalDeleteProcessPackage(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult.Error("工艺包ID无效");
                }

                // 检查是否存在关联的工艺流程
                var flows = _processFlowBLL.GetProcessFlowsByPackageId(id);
                if (flows.Count > 0)
                {
                    return OperationResult.Error("该工艺包下存在工艺流程，无法删除");
                }

                // 执行物理删除
                bool success = _processPackageDAL.PhysicalDeleteProcessPackage(id);
                if (success)
                {
                    return OperationResult.Success("工艺包删除成功");
                }
                else
                {
                    return OperationResult.Error("工艺包删除失败");
                }
            }
            catch (Exception ex)
            {
                LogError(string.Format("删除工艺包失败，ID：{0}", id), ex);
                return OperationResult.Error("删除工艺包失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 检查工艺包编码是否存在
        /// </summary>
        /// <param name="packageCode">工艺包编码</param>
        /// <param name="excludeId">排除的ID</param>
        /// <returns>是否存在</returns>
        public bool IsPackageCodeExists(string packageCode, int excludeId = 0)
        {
            try
            {
                return _processPackageDAL.IsPackageCodeExists(packageCode, excludeId);
            }
            catch (Exception ex)
            {
                LogError("检查工艺包编码是否存在失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取工艺包统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public ProcessPackageStatistics GetProcessPackageStatistics()
        {
            try
            {
                var dataTable = _processPackageDAL.GetProcessPackageStatistics();
                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return new ProcessPackageStatistics
                    {
                        TotalCount = Convert.ToInt32(row["TotalCount"]),
                        ActiveCount = Convert.ToInt32(row["ActiveCount"]),
                        InactiveCount = Convert.ToInt32(row["InactiveCount"]),
                        DraftCount = Convert.ToInt32(row["DraftCount"]),
                        AverageFlowCount = Convert.ToDecimal(row["AverageFlowCount"]),
                        AverageTotalTime = Convert.ToDecimal(row["AverageTotalTime"])
                    };
                }
                return new ProcessPackageStatistics();
            }
            catch (Exception ex)
            {
                LogError("获取工艺包统计信息失败", ex);
                return new ProcessPackageStatistics();
            }
        }

        /// <summary>
        /// 将DataTable转换为工艺包列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>工艺包列表</returns>
        private List<ProcessPackage> ConvertDataTableToProcessPackageList(DataTable dataTable)
        {
            var list = new List<ProcessPackage>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(ConvertDataRowToProcessPackage(row));
            }
            return list;
        }

        /// <summary>
        /// 将DataRow转换为工艺包对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>工艺包对象</returns>
        private ProcessPackage ConvertDataRowToProcessPackage(DataRow row)
        {
            return new ProcessPackage
            {
                Id = Convert.ToInt32(row["id"]),
                PackageCode = row["package_code"].ToString(),
                PackageName = row["package_name"].ToString(),
                ProductId = Convert.ToInt32(row["product_id"]),
                ProductName = row.Table.Columns.Contains("product_name") ? row["product_name"].ToString() : "",
                Version = row["version"].ToString(),
                Status = (ProcessPackageStatus)Convert.ToInt32(row["status"]),
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

// --- END OF FILE ProcessPackageBLL.cs ---
