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
        bool AddMaterial(MaterialInfo material);
        bool UpdateMaterial(MaterialInfo material);
        bool DeleteMaterial(int id);
    }
}
