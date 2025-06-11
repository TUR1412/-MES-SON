using System;
using System.Collections.Generic;
using MES.Common.Exceptions;
using MES.Common.Logging;
using MES.DAL.Production;
using MES.Models.Production;

namespace MES.BLL.Material
{
    /// <summary>
    /// 产品业务逻辑层 (已升级)
    /// 遵循单一职责原则，实现IProductBLL接口，并提供所需方法
    /// </summary>
    public class ProductBLL : IProductBLL
    {
        private readonly ProductionDAL _productionDAL;

        public ProductBLL()
        {
            _productionDAL = new ProductionDAL();
        }

        /// <summary>
        /// 获取所有产品信息
        /// </summary>
        /// <returns>产品信息列表</returns>
        public List<ProductionInfo> GetAllProducts()
        {
            try
            {
                var products = _productionDAL.GetAll();
                LogManager.Info(string.Format("成功获取所有产品，共 {0} 条", products.Count));
                return products;
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有产品业务逻辑失败", ex);
                throw new MESException("获取产品列表失败", ex);
            }
        }

        /// <summary>
        /// ★★★ 新增方法：根据产品编码获取产品信息 ★★★
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <returns>产品信息</returns>
        public ProductionInfo GetProductByCode(string productCode)
        {
            try
            {
                if (string.IsNullOrEmpty(productCode))
                {
                    return null;
                }
                // 调用DAL中我们刚刚添加的方法
                return _productionDAL.GetByProductNum(productCode);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取产品信息失败，产品编码: {0}", productCode), ex);
                throw new MESException("获取产品信息失败", ex);
            }
        }
    }
}