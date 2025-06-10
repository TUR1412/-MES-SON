namespace MES.Models.Workshop
{
    /// <summary>
    /// 设备状态分布统计
    /// </summary>
    public class EquipmentStatusDistribution
    {
        /// <summary>
        /// 状态值
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 百分比
        /// </summary>
        public decimal Percentage { get; set; }
    }
}
