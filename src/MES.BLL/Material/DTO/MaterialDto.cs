using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.BLL.Material.DTO
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialType { get; set; }
        public string Specification { get; set; }
        public string Unit { get; set; }
        public string Category { get; set; }
        public string Supplier { get; set; }
        public decimal? StandardCost { get; set; }
        public decimal? SafetyStock { get; set; }
        public decimal? MinStock { get; set; }
        public decimal? MaxStock { get; set; }
        public decimal? StockQuantity { get; set; } // 新增：库存数量
        public int? LeadTime { get; set; }
        public bool Status { get; set; }

        // --- 新增的字段，以匹配UI显示需求 ---
        public decimal Price { get; set; } // 价格/成本，根据您的业务可能是StandardCost
        public string Remark { get; set; } // 备注

        /// <summary>
        /// 创建此对象的浅表副本。
        /// </summary>
        /// <returns>一个新的 MaterialDto 对象，其值与当前对象相同。</returns>
        public MaterialDto Clone()
        {
            // 在类的内部，可以合法地调用受保护的 MemberwiseClone 方法。
            return (MaterialDto)this.MemberwiseClone();
        }
    }
}