using System;
using MES.Models.Base;

namespace MES.Models.Material
{
    /// <summary>
    /// BOM物料清单模型 - L成员负责完善 (已由天帝修复)
    /// </summary>
    public class BOMInfo : BaseModel
    {
        /// <summary>
        /// BOM编码
        /// </summary>
        public string BOMCode { get; set; }

        /// <summary>
        /// BOM名称 (新增属性，对应数据库bom_name)
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 子物料ID
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// 子物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 子物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 损耗率(%)
        /// </summary>
        public decimal LossRate { get; set; }

        /// <summary>
        /// 替代料编码
        /// </summary>
        public string SubstituteMaterial { get; set; }

        /// <summary>
        /// 备注 (对应数据库description)
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// BOM版本 (对应数据库version)
        /// </summary>
        public string BOMVersion { get; set; }

        /// <summary>
        /// BOM类型：PRODUCTION-生产，ENGINEERING-工程
        /// </summary>
        public string BOMType { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 状态：true-启用，false-禁用 (对应数据库status)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BOMInfo()
        {
            BOMVersion = "1.0";
            BOMType = "PRODUCTION";
            EffectiveDate = DateTime.Now;
            Status = true;
            ProductCode = string.Empty;
            ProductName = string.Empty;
            MaterialCode = string.Empty;
            MaterialName = string.Empty;
            Unit = "个";
            LossRate = 0;
            SubstituteMaterial = string.Empty;
            Remarks = string.Empty;
            BomName = string.Empty;
        }

        /// <summary>
        /// 克隆对象 - C# 5.0兼容实现
        /// </summary>
        public BOMInfo Clone()
        {
            // 使用MemberwiseClone以简化克隆过程并自动处理新属性
            return (BOMInfo)this.MemberwiseClone();
        }
    }
}