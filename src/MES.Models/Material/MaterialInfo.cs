using System;
using MES.Models.Base;

namespace MES.Models.Material
{
    /// <summary>
    /// 物料信息模型 - L成员负责完善
    /// </summary>
    public class MaterialInfo : BaseModel
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public string MaterialType { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 物料分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 标准成本
        /// </summary>
        public decimal? StandardCost { get; set; }

        /// <summary>
        /// 安全库存
        /// </summary>
        public decimal? SafetyStock { get; set; }

        /// <summary>
        /// 最小库存
        /// </summary>
        public decimal? MinStock { get; set; }

        /// <summary>
        /// 最大库存
        /// </summary>
        public decimal? MaxStock { get; set; }

        /// <summary>
        /// 当前库存数量
        /// </summary>
        public decimal? StockQuantity { get; set; }

        /// <summary>
        /// 采购提前期（天）
        /// </summary>
        public int? LeadTime { get; set; }

        /// <summary>
        /// 状态：1-启用，0-禁用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialInfo()
        {
            Status = true;
        }
    }
}
