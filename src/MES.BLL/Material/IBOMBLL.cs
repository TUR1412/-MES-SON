using MES.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.BLL.Material
{
    public interface IBOMBLL
    {
        List<BOMInfo> GetAllBOMs();
        BOMInfo GetBOMById(int id);
        List<BOMInfo> GetBOMsByProductId(int productId);
        bool AddBOM(BOMInfo bom);
        bool UpdateBOM(BOMInfo bom);
        bool DeleteBOM(int id);
    }
}
