using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MES.Models.WorkOrder;
using MES.DAL.WorkOrder;

namespace MES.BLL.WorkOrder
{
    /// <summary>
    /// 工单业务逻辑层
    /// </summary>
    public class WorkOrderBLL
    {
        private readonly WorkOrderDAL workOrderDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkOrderBLL()
        {
            workOrderDAL = new WorkOrderDAL();
        }

        /// <summary>
        /// 创建工单
        /// </summary>
        /// <param name="workOrder">工单信息</param>
        /// <returns>是否成功</returns>
        public bool CreateWorkOrder(WorkOrderModel workOrder)
        {
            try
            {
                // 这里应该调用DAL层保存工单
                // 暂时返回true模拟成功
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("创建工单失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 获取所有工单信息
        /// </summary>
        /// <returns>工单信息列表</returns>
        public List<WorkOrderInfo> GetAllWorkOrders()
        {
            try
            {
                return workOrderDAL.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取工单列表失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 获取成品工单列表
        /// </summary>
        /// <returns>成品工单数据表</returns>
        public DataTable GetFinishedWorkOrders()
        {
            try
            {
                // 从数据库获取真实的已完成工单数据
                var workOrders = workOrderDAL.GetFinishedWorkOrders();

                DataTable table = new DataTable();
                table.Columns.Add("WorkOrderNo", typeof(string));
                table.Columns.Add("ProductName", typeof(string));
                table.Columns.Add("Status", typeof(string));

                // 将真实数据填充到DataTable中
                foreach (var workOrder in workOrders)
                {
                    table.Rows.Add(
                        workOrder.WorkOrderNum,
                        workOrder.ProductName ?? "",
                        GetWorkOrderStatusText(workOrder.WorkOrderStatus)
                    );
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取成品工单列表失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 根据日期获取最大序号
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>最大序号</returns>
        public int GetMaxSequenceByDate(DateTime date)
        {
            try
            {
                // 这里应该查询数据库获取当日最大序号
                // 暂时返回随机数模拟
                Random random = new Random();
                return random.Next(1, 100);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取最大序号失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 创建工单BOM
        /// </summary>
        /// <param name="bomDetail">BOM明细</param>
        /// <returns>是否成功</returns>
        public bool CreateWorkOrderBOM(WorkOrderBOMModel bomDetail)
        {
            try
            {
                // 这里应该调用DAL层保存BOM明细
                // 暂时返回true模拟成功
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("创建工单BOM失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 获取可取消的工单列表
        /// </summary>
        /// <returns>可取消的工单数据表</returns>
        public DataTable GetCancellableWorkOrders()
        {
            try
            {
                // 从数据库获取真实的可取消工单数据
                var workOrders = workOrderDAL.GetCancellableWorkOrders();

                DataTable table = new DataTable();
                table.Columns.Add("WorkOrderNo", typeof(string));
                table.Columns.Add("WorkOrderType", typeof(string));
                table.Columns.Add("WorkOrderDesc", typeof(string));
                table.Columns.Add("ProductCode", typeof(string));
                table.Columns.Add("PlanQuantity", typeof(decimal));
                table.Columns.Add("Unit", typeof(string));
                table.Columns.Add("Status", typeof(string));
                table.Columns.Add("CreatedBy", typeof(string));
                table.Columns.Add("CreatedDate", typeof(DateTime));

                // 将真实数据填充到DataTable中
                foreach (var workOrder in workOrders)
                {
                    table.Rows.Add(
                        workOrder.WorkOrderNum,
                        workOrder.WorkOrderType,
                        workOrder.Description ?? "",
                        workOrder.ProductCode ?? "",
                        workOrder.PlannedQuantity,
                        "个", // 默认单位
                        GetWorkOrderStatusText(workOrder.WorkOrderStatus),
                        "系统", // 默认创建人
                        workOrder.CreateTime
                    );
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取可取消工单列表失败：{0}", ex.Message), ex);
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
                if (string.IsNullOrEmpty(workOrderNo))
                {
                    throw new ArgumentException("工单号不能为空");
                }

                if (string.IsNullOrEmpty(cancelReason))
                {
                    throw new ArgumentException("取消原因不能为空");
                }

                // 调用DAL层真实更新工单状态为"已取消"
                return workOrderDAL.CancelWorkOrder(workOrderNo, cancelReason);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("取消工单失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 获取可提交的工单列表
        /// </summary>
        /// <returns>可提交的工单数据表</returns>
        public DataTable GetSubmittableWorkOrders()
        {
            try
            {
                // 从数据库获取真实的可提交工单数据
                var workOrders = workOrderDAL.GetSubmittableWorkOrders();

                DataTable table = new DataTable();
                table.Columns.Add("WorkOrderNo", typeof(string));
                table.Columns.Add("WorkOrderType", typeof(string));
                table.Columns.Add("WorkOrderDesc", typeof(string));
                table.Columns.Add("ProductCode", typeof(string));
                table.Columns.Add("PlanQuantity", typeof(decimal));
                table.Columns.Add("Unit", typeof(string));
                table.Columns.Add("Status", typeof(string));
                table.Columns.Add("CreatedBy", typeof(string));
                table.Columns.Add("CreatedDate", typeof(DateTime));

                // 将真实数据填充到DataTable中
                foreach (var workOrder in workOrders)
                {
                    table.Rows.Add(
                        workOrder.WorkOrderNum,
                        workOrder.WorkOrderType,
                        workOrder.Description ?? "",
                        workOrder.ProductCode ?? "",
                        workOrder.PlannedQuantity,
                        "个", // 默认单位
                        GetWorkOrderStatusText(workOrder.WorkOrderStatus),
                        "系统", // 默认创建人
                        workOrder.CreateTime
                    );
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取可提交工单列表失败：{0}", ex.Message), ex);
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
                if (string.IsNullOrEmpty(workOrderNo))
                {
                    throw new ArgumentException("工单号不能为空");
                }

                // 调用DAL层真实更新工单状态为"已提交"
                return workOrderDAL.SubmitWorkOrder(workOrderNo, submitRemark);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("提交工单失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 搜索工单
        /// </summary>
        /// <param name="keyword">关键词（工单号或产品名称）</param>
        /// <param name="status">状态</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>工单信息列表</returns>
        public List<WorkOrderInfo> SearchWorkOrders(string keyword = null, string status = null,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // 获取所有工单
                var allWorkOrders = GetAllWorkOrders();

                // 应用筛选条件
                var filteredWorkOrders = allWorkOrders.AsEnumerable();

                // 关键词筛选
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    filteredWorkOrders = filteredWorkOrders.Where(w =>
                        (w.WorkOrderNum != null && w.WorkOrderNum.Contains(keyword)) ||
                        (w.ProductName != null && w.ProductName.Contains(keyword)) ||
                        (w.ProductCode != null && w.ProductCode.Contains(keyword)));
                }

                // 状态筛选
                if (!string.IsNullOrWhiteSpace(status) && status != "全部状态")
                {
                    int statusValue = GetStatusValueFromText(status);
                    filteredWorkOrders = filteredWorkOrders.Where(w => w.WorkOrderStatus == statusValue);
                }

                // 日期筛选
                if (startDate.HasValue)
                {
                    filteredWorkOrders = filteredWorkOrders.Where(w => w.CreateTime >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    filteredWorkOrders = filteredWorkOrders.Where(w => w.CreateTime <= endDate.Value.AddDays(1));
                }

                return filteredWorkOrders.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("搜索工单失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 从状态文本获取状态值
        /// </summary>
        /// <param name="statusText">状态文本</param>
        /// <returns>状态值</returns>
        private int GetStatusValueFromText(string statusText)
        {
            switch (statusText)
            {
                case "待开始": return 0;
                case "进行中": return 1;
                case "已完成": return 2;
                case "已关闭": return 3;
                default: return -1;
            }
        }

        /// <summary>
        /// 获取工单状态文本
        /// </summary>
        /// <param name="status">状态值</param>
        /// <returns>状态文本</returns>
        private string GetWorkOrderStatusText(int status)
        {
            switch (status)
            {
                case 0: return "待开始";
                case 1: return "进行中";
                case 2: return "已完成";
                case 3: return "已关闭";
                default: return "未知状态";
            }
        }
    }

    /// <summary>
    /// 工单模型（临时定义，应该在Models项目中）
    /// </summary>
    public class WorkOrderModel
    {
        public string WorkOrderNo { get; set; }
        public string WorkOrderType { get; set; }
        public string WorkOrderDesc { get; set; }
        public string ProductCode { get; set; }
        public string FinishedWorkOrderNo { get; set; }
        public string BOMCode { get; set; }
        public string BOMVersion { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public decimal PlanQuantity { get; set; }
        public string Unit { get; set; }
        public string ProductType { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// 工单BOM模型（临时定义，应该在Models项目中）
    /// </summary>
    public class WorkOrderBOMModel
    {
        public string WorkOrderNo { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Specification { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string Unit { get; set; }
        public decimal StockQuantity { get; set; }
        public string Status { get; set; }
    }
}
