using System;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 设备参数信息
    /// </summary>
    public class EquipmentParameters
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 运行效率
        /// </summary>
        public decimal Efficiency { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public decimal Temperature { get; set; }

        /// <summary>
        /// 压力
        /// </summary>
        public decimal Pressure { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public decimal Speed { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        public decimal Power { get; set; }

        /// <summary>
        /// 振动
        /// </summary>
        public decimal Vibration { get; set; }
    }
}
