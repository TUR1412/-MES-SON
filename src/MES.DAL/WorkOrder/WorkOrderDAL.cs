using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.Models.Base;
using MES.Models.WorkOrder;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.WorkOrder
{
    /// <summary>
    /// 工单数据访问类
    /// 提供工单相关的数据库操作功能
    /// </summary>
    public class WorkOrderDAL : BaseDAL<WorkOrderInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "work_order_info"; }
        }

        /// <summary>
        /// 主键属性名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "WorkOrderId"; }
        }

        /// <summary>
        /// 将DataRow转换为WorkOrderInfo实体对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>WorkOrderInfo实体对象</returns>
        protected override WorkOrderInfo MapRowToEntity(DataRow row)
        {
            return new WorkOrderInfo
            {
                WorkOrderId = Convert.ToInt32(row["work_order_id"]),
                WorkOrderNum = row["work_order_num"] != DBNull.Value ? row["work_order_num"].ToString() : null,
                WorkOrderType = row["work_order_type"] != DBNull.Value ? row["work_order_type"].ToString() : null,
                ProductId = Convert.ToInt32(row["product_id"]),
                FlowId = Convert.ToInt32(row["flow_id"]),
                BOMId = Convert.ToInt32(row["bom_id"]),
                PlannedQuantity = Convert.ToDecimal(row["planned_quantity"]),
                InputQuantity = Convert.ToDecimal(row["input_quantity"]),
                OutputQuantity = Convert.ToDecimal(row["output_quantity"]),
                ScrapQuantity = Convert.ToDecimal(row["scrap_quantity"]),
                WorkOrderStatus = Convert.ToInt32(row["work_order_status"]),
                ProcessStatus = row["process_status"] != DBNull.Value ? row["process_status"].ToString() : null,
                LockStatus = Convert.ToInt32(row["lock_status"]),
                FactoryId = Convert.ToInt32(row["factory_id"]),
                HotType = row["hot_type"] != DBNull.Value ? row["hot_type"].ToString() : null,
                PlannedStartTime = row["planned_start_time"] != DBNull.Value ? Convert.ToDateTime(row["planned_start_time"]) : (DateTime?)null,
                PlannedDueDate = row["planned_due_date"] != DBNull.Value ? Convert.ToDateTime(row["planned_due_date"]) : (DateTime?)null,
                CreateTime = Convert.ToDateTime(row["create_time"]),
                ProductionStartTime = row["production_start_time"] != DBNull.Value ? Convert.ToDateTime(row["production_start_time"]) : (DateTime?)null,
                CompletionTime = row["completion_time"] != DBNull.Value ? Convert.ToDateTime(row["completion_time"]) : (DateTime?)null,
                CloseTime = row["close_time"] != DBNull.Value ? Convert.ToDateTime(row["close_time"]) : (DateTime?)null,
                WorkOrderVersion = row["work_order_version"] != DBNull.Value ? row["work_order_version"].ToString() : null,
                ParentWorkOrderVersion = row["parent_work_order_version"] != DBNull.Value ? row["parent_work_order_version"].ToString() : null,
                ProductOrderNo = row["product_order_no"] != DBNull.Value ? row["product_order_no"].ToString() : null,
                ProductOrderVersion = row["product_order_version"] != DBNull.Value ? row["product_order_version"].ToString() : null,
                SalesOrderNo = row["sales_order_no"] != DBNull.Value ? row["sales_order_no"].ToString() : null,
                MainBatchNo = row["main_batch_no"] != DBNull.Value ? row["main_batch_no"].ToString() : null,
                Description = row["description"] != DBNull.Value ? row["description"].ToString() : null
            };
        }

        /// <summary>
        /// 根据工单号获取工单信息
        /// </summary>
        /// <param name="workOrderNum">工单号</param>
        /// <returns>工单信息</returns>
        public WorkOrderInfo GetByWorkOrderNum(string workOrderNum)
        {
            try
            {
                if (string.IsNullOrEmpty(workOrderNum))
                {
                    throw new ArgumentException("工单号不能为空", "workOrderNum");
                }

                var workOrders = GetByCondition("work_order_num = @workOrderNum",
                    DatabaseHelper.CreateParameter("@workOrderNum", workOrderNum));

                return workOrders.Count > 0 ? workOrders[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据工单号获取工单信息失败，工单号: {workOrderNum}", ex);
                throw new MESException("获取工单信息失败", ex);
            }
        }

        /// <summary>
        /// 根据工单状态获取工单列表
        /// </summary>
        /// <param name="status">工单状态(0:未开始,1:进行中,2:已完成,3:已关闭)</param>
        /// <returns>工单列表</returns>
        public List<WorkOrderInfo> GetByStatus(int status)
        {
            try
            {
                return GetByCondition("work_order_status = @status",
                    DatabaseHelper.CreateParameter("@status", status));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据状态获取工单列表失败，状态: {status}", ex);
                throw new MESException("获取工单列表失败", ex);
            }
        }

        /// <summary>
        /// 根据产品ID获取工单列表
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工单列表</returns>
        public List<WorkOrderInfo> GetByProductId(int productId)
        {
            try
            {
                return GetByCondition("product_id = @productId",
                    DatabaseHelper.CreateParameter("@productId", productId));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据产品ID获取工单列表失败，产品ID: {productId}", ex);
                throw new MESException("获取工单列表失败", ex);
            }
        }

        /// <summary>
        /// 根据工艺流程ID获取工单列表
        /// </summary>
        /// <param name="flowId">工艺流程ID</param>
        /// <returns>工单列表</returns>
        public List<WorkOrderInfo> GetByFlowId(int flowId)
        {
            try
            {
                return GetByCondition("flow_id = @flowId",
                    DatabaseHelper.CreateParameter("@flowId", flowId));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据工艺流程ID获取工单列表失败，工艺流程ID: {flowId}", ex);
                throw new MESException("获取工单列表失败", ex);
            }
        }

        /// <summary>
        /// 根据工厂ID获取工单列表
        /// </summary>
        /// <param name="factoryId">工厂ID</param>
        /// <returns>工单列表</returns>
        public List<WorkOrderInfo> GetByFactoryId(int factoryId)
        {
            try
            {
                return GetByCondition("factory_id = @factoryId",
                    DatabaseHelper.CreateParameter("@factoryId", factoryId));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据工厂ID获取工单列表失败，工厂ID: {factoryId}", ex);
                throw new MESException("获取工单列表失败", ex);
            }
        }

        /// <summary>
        /// 根据销售单号获取工单列表
        /// </summary>
        /// <param name="salesOrderNo">销售单号</param>
        /// <returns>工单列表</returns>
        public List<WorkOrderInfo> GetBySalesOrderNo(string salesOrderNo)
        {
            try
            {
                if (string.IsNullOrEmpty(salesOrderNo))
                {
                    return new List<WorkOrderInfo>();
                }

                return GetByCondition("sales_order_no = @salesOrderNo",
                    DatabaseHelper.CreateParameter("@salesOrderNo", salesOrderNo));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据销售单号获取工单列表失败，销售单号: {salesOrderNo}", ex);
                throw new MESException("获取工单列表失败", ex);
            }
        }

        /// <summary>
        /// 更新工单状态
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="newStatus">新状态</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateWorkOrderStatus(int workOrderId, int newStatus)
        {
            try
            {
                string sql = @"UPDATE work_order_info SET 
                              work_order_status = @status, 
                              update_time = @updateTime
                              WHERE work_order_id = @workOrderId";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@status", newStatus),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@workOrderId", workOrderId)
                };

                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新工单状态失败，工单ID: {workOrderId}, 新状态: {newStatus}", ex);
                throw new MESException("更新工单状态失败", ex);
            }
        }

        /// <summary>
        /// 更新工单数量信息
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="inputQuantity">投入数量</param>
        /// <param name="outputQuantity">产出数量</param>
        /// <param name="scrapQuantity">报废数量</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateWorkOrderQuantities(int workOrderId, decimal inputQuantity, decimal outputQuantity, decimal scrapQuantity)
        {
            try
            {
                string sql = @"UPDATE work_order_info SET 
                              input_quantity = @inputQuantity, 
                              output_quantity = @outputQuantity,
                              scrap_quantity = @scrapQuantity,
                              update_time = @updateTime
                              WHERE work_order_id = @workOrderId";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@inputQuantity", inputQuantity),
                    DatabaseHelper.CreateParameter("@outputQuantity", outputQuantity),
                    DatabaseHelper.CreateParameter("@scrapQuantity", scrapQuantity),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@workOrderId", workOrderId)
                };

                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新工单数量信息失败，工单ID: {workOrderId}", ex);
                throw new MESException("更新工单数量信息失败", ex);
            }
        }
        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">工单实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildInsertSql(WorkOrderInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"INSERT INTO work_order_info
                          (work_order_num, work_order_type, product_id, flow_id, bom_id,
                           planned_quantity, input_quantity, output_quantity, scrap_quantity,
                           work_order_status, process_status, lock_status, factory_id, hot_type,
                           planned_start_time, planned_due_date, create_time, production_start_time,
                           completion_time, close_time, work_order_version, parent_work_order_version,
                           product_order_no, product_order_version, sales_order_no, main_batch_no, description)
                          VALUES
                          (@workOrderNum, @workOrderType, @productId, @flowId, @bomId,
                           @plannedQuantity, @inputQuantity, @outputQuantity, @scrapQuantity,
                           @workOrderStatus, @processStatus, @lockStatus, @factoryId, @hotType,
                           @plannedStartTime, @plannedDueDate, @createTime, @productionStartTime,
                           @completionTime, @closeTime, @workOrderVersion, @parentWorkOrderVersion,
                           @productOrderNo, @productOrderVersion, @salesOrderNo, @mainBatchNo, @description)";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@workOrderNum", entity.WorkOrderNum),
                DatabaseHelper.CreateParameter("@workOrderType", entity.WorkOrderType),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@flowId", entity.FlowId),
                DatabaseHelper.CreateParameter("@bomId", entity.BOMId),
                DatabaseHelper.CreateParameter("@plannedQuantity", entity.PlannedQuantity),
                DatabaseHelper.CreateParameter("@inputQuantity", entity.InputQuantity),
                DatabaseHelper.CreateParameter("@outputQuantity", entity.OutputQuantity),
                DatabaseHelper.CreateParameter("@scrapQuantity", entity.ScrapQuantity),
                DatabaseHelper.CreateParameter("@workOrderStatus", entity.WorkOrderStatus),
                DatabaseHelper.CreateParameter("@processStatus", entity.ProcessStatus),
                DatabaseHelper.CreateParameter("@lockStatus", entity.LockStatus),
                DatabaseHelper.CreateParameter("@factoryId", entity.FactoryId),
                DatabaseHelper.CreateParameter("@hotType", entity.HotType),
                DatabaseHelper.CreateParameter("@plannedStartTime", entity.PlannedStartTime),
                DatabaseHelper.CreateParameter("@plannedDueDate", entity.PlannedDueDate),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@productionStartTime", entity.ProductionStartTime),
                DatabaseHelper.CreateParameter("@completionTime", entity.CompletionTime),
                DatabaseHelper.CreateParameter("@closeTime", entity.CloseTime),
                DatabaseHelper.CreateParameter("@workOrderVersion", entity.WorkOrderVersion),
                DatabaseHelper.CreateParameter("@parentWorkOrderVersion", entity.ParentWorkOrderVersion),
                DatabaseHelper.CreateParameter("@productOrderNo", entity.ProductOrderNo),
                DatabaseHelper.CreateParameter("@productOrderVersion", entity.ProductOrderVersion),
                DatabaseHelper.CreateParameter("@salesOrderNo", entity.SalesOrderNo),
                DatabaseHelper.CreateParameter("@mainBatchNo", entity.MainBatchNo),
                DatabaseHelper.CreateParameter("@description", entity.Description)
            };

            return true;
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">工单实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override bool BuildUpdateSql(WorkOrderInfo entity, out string sql, out MySqlParameter[] parameters)
        {
            sql = @"UPDATE work_order_info SET
                          work_order_num = @workOrderNum, 
                          work_order_type = @workOrderType,
                          product_id = @productId, 
                          flow_id = @flowId,
                          bom_id = @bomId,
                          planned_quantity = @plannedQuantity,
                          input_quantity = @inputQuantity,
                          output_quantity = @outputQuantity,
                          scrap_quantity = @scrapQuantity,
                          work_order_status = @workOrderStatus,
                          process_status = @processStatus,
                          lock_status = @lockStatus,
                          factory_id = @factoryId,
                          hot_type = @hotType,
                          planned_start_time = @plannedStartTime,
                          planned_due_date = @plannedDueDate,
                          production_start_time = @productionStartTime,
                          completion_time = @completionTime,
                          close_time = @closeTime,
                          work_order_version = @workOrderVersion,
                          parent_work_order_version = @parentWorkOrderVersion,
                          product_order_no = @productOrderNo,
                          product_order_version = @productOrderVersion,
                          sales_order_no = @salesOrderNo,
                          main_batch_no = @mainBatchNo,
                          description = @description,
                          update_time = @updateTime
                          WHERE work_order_id = @workOrderId";

            parameters = new[]
            {
                DatabaseHelper.CreateParameter("@workOrderNum", entity.WorkOrderNum),
                DatabaseHelper.CreateParameter("@workOrderType", entity.WorkOrderType),
                DatabaseHelper.CreateParameter("@productId", entity.ProductId),
                DatabaseHelper.CreateParameter("@flowId", entity.FlowId),
                DatabaseHelper.CreateParameter("@bomId", entity.BOMId),
                DatabaseHelper.CreateParameter("@plannedQuantity", entity.PlannedQuantity),
                DatabaseHelper.CreateParameter("@inputQuantity", entity.InputQuantity),
                DatabaseHelper.CreateParameter("@outputQuantity", entity.OutputQuantity),
                DatabaseHelper.CreateParameter("@scrapQuantity", entity.ScrapQuantity),
                DatabaseHelper.CreateParameter("@workOrderStatus", entity.WorkOrderStatus),
                DatabaseHelper.CreateParameter("@processStatus", entity.ProcessStatus),
                DatabaseHelper.CreateParameter("@lockStatus", entity.LockStatus),
                DatabaseHelper.CreateParameter("@factoryId", entity.FactoryId),
                DatabaseHelper.CreateParameter("@hotType", entity.HotType),
                DatabaseHelper.CreateParameter("@plannedStartTime", entity.PlannedStartTime),
                DatabaseHelper.CreateParameter("@plannedDueDate", entity.PlannedDueDate),
                DatabaseHelper.CreateParameter("@productionStartTime", entity.ProductionStartTime),
                DatabaseHelper.CreateParameter("@completionTime", entity.CompletionTime),
                DatabaseHelper.CreateParameter("@closeTime", entity.CloseTime),
                DatabaseHelper.CreateParameter("@workOrderVersion", entity.WorkOrderVersion),
                DatabaseHelper.CreateParameter("@parentWorkOrderVersion", entity.ParentWorkOrderVersion),
                DatabaseHelper.CreateParameter("@productOrderNo", entity.ProductOrderNo),
                DatabaseHelper.CreateParameter("@productOrderVersion", entity.ProductOrderVersion),
                DatabaseHelper.CreateParameter("@salesOrderNo", entity.SalesOrderNo),
                DatabaseHelper.CreateParameter("@mainBatchNo", entity.MainBatchNo),
                DatabaseHelper.CreateParameter("@description", entity.Description),
                DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                DatabaseHelper.CreateParameter("@workOrderId", entity.WorkOrderId)
            };

            return true;
        }

        /// <summary>
        /// 获取可取消的工单列表（状态为待开始或进行中的工单）
        /// </summary>
        /// <returns>可取消的工单列表</returns>
        public List<WorkOrderInfo> GetCancellableWorkOrders()
        {
            try
            {
                // 获取状态为待开始(0)或进行中(1)的工单
                return GetByCondition("work_order_status IN (0, 1) ORDER BY create_time DESC");
            }
            catch (Exception ex)
            {
                LogManager.Error("获取可取消工单列表失败", ex);
                throw new MESException("获取可取消工单列表失败", ex);
            }
        }

        /// <summary>
        /// 获取可提交的工单列表（状态为待提交的工单）
        /// </summary>
        /// <returns>可提交的工单列表</returns>
        public List<WorkOrderInfo> GetSubmittableWorkOrders()
        {
            try
            {
                // 获取状态为待提交的工单（这里假设状态1为待提交）
                return GetByCondition("work_order_status = 1 AND process_status = 'READY_TO_SUBMIT' ORDER BY create_time DESC");
            }
            catch (Exception ex)
            {
                LogManager.Error("获取可提交工单列表失败", ex);
                throw new MESException("获取可提交工单列表失败", ex);
            }
        }

        /// <summary>
        /// 获取已完成的工单列表
        /// </summary>
        /// <returns>已完成的工单列表</returns>
        public List<WorkOrderInfo> GetFinishedWorkOrders()
        {
            try
            {
                // 获取状态为已完成(2)的工单
                return GetByCondition("work_order_status = 2 ORDER BY completion_time DESC");
            }
            catch (Exception ex)
            {
                LogManager.Error("获取已完成工单列表失败", ex);
                throw new MESException("获取已完成工单列表失败", ex);
            }
        }

        /// <summary>
        /// 取消工单
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <param name="cancelReason">取消原因</param>
        /// <returns>是否成功</returns>
        public bool CancelWorkOrder(string workOrderNo, string cancelReason)
        {
            try
            {
                string sql = @"UPDATE work_order_info SET
                              work_order_status = 3,
                              process_status = 'CANCELLED',
                              description = CONCAT(IFNULL(description, ''), ' [取消原因: ', @cancelReason, ']'),
                              update_time = @updateTime
                              WHERE work_order_num = @workOrderNo";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@cancelReason", cancelReason),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@workOrderNo", workOrderNo)
                };

                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("取消工单失败，工单号: {0}, 取消原因: {1}", workOrderNo, cancelReason), ex);
                throw new MESException("取消工单失败", ex);
            }
        }

        /// <summary>
        /// 提交工单
        /// </summary>
        /// <param name="workOrderNo">工单号</param>
        /// <param name="submitRemark">提交备注</param>
        /// <returns>是否成功</returns>
        public bool SubmitWorkOrder(string workOrderNo, string submitRemark)
        {
            try
            {
                string sql = @"UPDATE work_order_info SET
                              work_order_status = 2,
                              process_status = 'SUBMITTED',
                              completion_time = @completionTime,
                              description = CONCAT(IFNULL(description, ''), ' [提交备注: ', @submitRemark, ']'),
                              update_time = @updateTime
                              WHERE work_order_num = @workOrderNo";

                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@completionTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@submitRemark", submitRemark ?? ""),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@workOrderNo", workOrderNo)
                };

                return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("提交工单失败，工单号: {0}, 提交备注: {1}", workOrderNo, submitRemark), ex);
                throw new MESException("提交工单失败", ex);
            }
        }
    }
}