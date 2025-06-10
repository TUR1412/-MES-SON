using System;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 设备效率报告
    /// </summary>
    public class EquipmentEfficiencyReport
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 运行效率
        /// </summary>
        public decimal Efficiency { get; set; }

        /// <summary>
        /// 利用率
        /// </summary>
        public decimal Utilization { get; set; }

        /// <summary>
        /// 运行时间（小时）
        /// </summary>
        public decimal RunningHours { get; set; }

        /// <summary>
        /// 停机时间（小时）
        /// </summary>
        public decimal DowntimeHours { get; set; }

        /// <summary>
        /// 故障次数
        /// </summary>
        public int FaultCount { get; set; }

        /// <summary>
        /// 统计开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 统计结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 平均效率
        /// </summary>
        public decimal AverageEfficiency { get; set; }

        /// <summary>
        /// 最大效率
        /// </summary>
        public decimal MaxEfficiency { get; set; }

        /// <summary>
        /// 最小效率
        /// </summary>
        public decimal MinEfficiency { get; set; }

        /// <summary>
        /// 报告日期
        /// </summary>
        public DateTime ReportDate { get; set; }
    }
}
