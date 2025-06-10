namespace MES.Models.Workshop
{
    /// <summary>
    /// 车间设备分布统计
    /// </summary>
    public class WorkshopEquipmentDistribution
    {
        /// <summary>
        /// 车间ID
        /// </summary>
        public int WorkshopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public int EquipmentCount { get; set; }

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
        /// 平均效率
        /// </summary>
        public decimal AverageEfficiency { get; set; }
    }
}
