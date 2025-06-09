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
        /// 获取成品工单列表
        /// </summary>
        /// <returns>成品工单数据表</returns>
        public DataTable GetFinishedWorkOrders()
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("WorkOrderNo", typeof(string));
                table.Columns.Add("ProductName", typeof(string));
                table.Columns.Add("Status", typeof(string));
                
                // 添加示例数据
                table.Rows.Add("WO202506090001", "产品A", "已完成");
                table.Rows.Add("WO202506090002", "产品B", "已完成");
                table.Rows.Add("WO202506090003", "产品C", "已完成");
                
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

                // 模拟数据 - 实际应该从数据库查询状态为"待开始"或"进行中"的工单
                table.Rows.Add("WO202506090001", "生产工单", "钢制支架生产", "P001", 100, "个", "待开始", "张工", DateTime.Now.AddDays(-1));
                table.Rows.Add("WO202506090002", "生产工单", "铝合金外壳生产", "P002", 200, "个", "进行中", "李师傅", DateTime.Now.AddDays(-2));
                table.Rows.Add("WO202506090003", "生产工单", "精密齿轮生产", "P003", 50, "个", "待开始", "王工", DateTime.Now.AddHours(-6));
                table.Rows.Add("WO202506090004", "生产工单", "电机外壳生产", "P004", 150, "个", "待开始", "赵主管", DateTime.Now.AddHours(-12));
                table.Rows.Add("WO202506090005", "生产工单", "不锈钢管件生产", "P005", 300, "个", "进行中", "孙师傅", DateTime.Now.AddHours(-8));

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

                // 这里应该调用DAL层更新工单状态为"已取消"
                // 暂时返回true模拟成功
                return true;
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

                // 模拟数据 - 实际应该从数据库查询状态为"待提交"的工单
                table.Rows.Add("WO202506090006", "生产工单", "电机外壳生产", "P004", 150, "个", "待提交", "赵主管", DateTime.Now.AddDays(-1));
                table.Rows.Add("WO202506090007", "生产工单", "不锈钢管件生产", "P005", 300, "个", "待提交", "孙师傅", DateTime.Now.AddHours(-8));
                table.Rows.Add("WO202506090008", "生产工单", "塑料配件生产", "P006", 500, "个", "待提交", "钱工", DateTime.Now.AddHours(-4));

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

                // 这里应该调用DAL层更新工单状态为"已提交"
                // 暂时返回true模拟成功
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("提交工单失败：{0}", ex.Message), ex);
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
