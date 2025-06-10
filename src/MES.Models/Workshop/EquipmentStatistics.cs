using System;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 设备统计信息
    /// </summary>
    public class EquipmentStatistics
    {
        /// <summary>
        /// 总设备数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 运行中设备数量
        /// </summary>
        public int RunningCount { get; set; }

        /// <summary>
        /// 停机设备数量
        /// </summary>
        public int StoppedCount { get; set; }

        /// <summary>
        /// 故障设备数量
        /// </summary>
        public int FaultCount { get; set; }

        /// <summary>
        /// 维护中设备数量
        /// </summary>
        public int MaintenanceCount { get; set; }

        /// <summary>
        /// 平均运行效率
        /// </summary>
        public decimal AverageEfficiency { get; set; }

        /// <summary>
        /// 平均利用率
        /// </summary>
        public decimal AverageUtilization { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime StatisticsTime { get; set; }

        /// <summary>
        /// 设备总价值
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// 需要维护的设备数量
        /// </summary>
        public int MaintenanceNeededCount { get; set; }
    }
}
