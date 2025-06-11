using MES.Models.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.BLL.Material
{
    /// <summary>
    /// 产品业务逻辑接口
    /// </summary>
    public interface IProductBLL
    {
        /// <summary>
        /// 获取所有产品信息
        /// </summary>
        /// <returns>产品信息列表</returns>
        List<ProductionInfo> GetAllProducts();
    }
}
