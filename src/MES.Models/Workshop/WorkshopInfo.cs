using System;
using MES.Models.Base;

namespace MES.Models.Workshop
{
    /// <summary>
    /// 车间信息模型
    /// 用于管理车间的基本信息和状态
    /// </summary>
    public class WorkshopInfo : BaseModel
    {
        /// <summary>
        /// 车间编码（唯一标识）
        /// </summary>
        public string WorkshopCode { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 车间负责人
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 车间位置/地址
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 车间面积（平方米）
        /// </summary>
        public decimal Area { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public int EquipmentCount { get; set; }

        /// <summary>
        /// 员工数量
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// 车间类型
        /// 1-生产车间，2-装配车间，3-包装车间，4-质检车间，5-仓储车间
        /// </summary>
        public int WorkshopType { get; set; }

        /// <summary>
        /// 生产能力（件/天）
        /// </summary>
        public int ProductionCapacity { get; set; }

        /// <summary>
        /// 车间状态
        /// 0-停用，1-正常运行，2-维护中，3-故障停机
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 工作班次
        /// 1-单班，2-两班，3-三班
        /// </summary>
        public int WorkShift { get; set; }

        /// <summary>
        /// 安全等级
        /// 1-一般，2-重要，3-关键
        /// </summary>
        public int SafetyLevel { get; set; }

        /// <summary>
        /// 环境要求
        /// </summary>
        public string EnvironmentRequirement { get; set; }

        /// <summary>
        /// 质量标准
        /// </summary>
        public string QualityStandard { get; set; }



        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkshopInfo()
        {
            Status = "1"; // 默认状态为正常运行
            WorkshopType = 1; // 默认类型为生产车间
            WorkShift = 1; // 默认单班制
            SafetyLevel = 1; // 默认安全等级为一般
            EquipmentCount = 0; // 默认设备数量为0
            EmployeeCount = 0; // 默认员工数量为0
            ProductionCapacity = 0; // 默认生产能力为0
        }
    }
}
