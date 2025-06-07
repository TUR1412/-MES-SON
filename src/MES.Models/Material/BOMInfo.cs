using System;
using MES.Models.Base;

namespace MES.Models.Material
{
    /// <summary>
    /// BOM物料清单模型 - L成员负责完善
    /// </summary>
    public class BOMInfo : BaseModel
    {
        /// <summary>
        /// BOM编码
        /// </summary>
        public string BOMCode { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// BOM版本
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
        /// 状态：1-启用，0-禁用
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
        }
    }
}
