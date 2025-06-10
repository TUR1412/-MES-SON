using System;
using MES.Models.Base;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 车间作业信息模型
    /// 用于管理车间生产作业的调度、监控和管理 - S成员负责
    /// </summary>
    public class WorkshopOperationInfo : BaseModel
    {
        /// <summary>
        /// 作业编号（唯一标识）
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 状态：0-待开始，1-进行中，2-已暂停，3-已完成，4-已停止
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 进度(%)
        /// </summary>
        public decimal Progress { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkshopOperationInfo()
        {
            Status = 0; // 默认状态为待开始
            StatusText = "待开始";
            Progress = 0;
            Quantity = 0;
        }
    }
}
