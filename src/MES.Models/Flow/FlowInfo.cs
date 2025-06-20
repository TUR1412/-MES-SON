﻿using System;
using MES.Models.Base;

namespace MES.Models.Flow
{
    /// <summary>
    /// 工艺流程
    /// </summary>
    public class FlowInfo : BaseModel
    {
        /// <summary>
        /// 工艺流程ID
        /// </summary>
        public int FlowId { get; set; }

        /// <summary>
        /// 工艺流程编号
        /// </summary>
        public string FlowNum { get; set; }

        /// <summary>
        /// 工艺流程名称
        /// </summary>
        public string FlowName { get; set; }

        /// <summary>
        /// 工艺流程版本（隐藏基类成员）
        /// </summary>
        public new string Version { get; set; }

        /// <summary>
        /// 工厂ID
        /// </summary>
        public int FortoryId { get; set; }

        /// <summary>
        /// 适用产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 状态(0:禁用,1:启用)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间（隐藏基类成员）
        /// </summary>
        public new DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间（隐藏基类成员）
        /// </summary>
        public new DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 磨片厚度(mm)
        /// </summary>
        public decimal GrindingThickness { get; set; }

        /// <summary>
        /// 封装形式
        /// </summary>
        public string PackageType { get; set; }
    }
}
