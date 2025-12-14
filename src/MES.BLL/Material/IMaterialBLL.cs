using MES.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.BLL.Material
{
    public interface IMaterialBLL
    {
        List<MaterialInfo> GetAllMaterials();
        MaterialInfo GetMaterialById(int id);
        MaterialInfo GetMaterialByCode(string materialCode);
        List<MaterialInfo> SearchMaterials(string keyword);
        List<MaterialInfo> GetMaterialsByPage(int pageIndex, int pageSize, out int totalCount);
        bool AddMaterial(MaterialInfo material);
        bool UpdateMaterial(MaterialInfo material);
        bool DeleteMaterial(int id);
    }
}
