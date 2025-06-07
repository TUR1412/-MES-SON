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
        public int? LeadTime { get; set; }
        public bool Status { get; set; }
    }
}
