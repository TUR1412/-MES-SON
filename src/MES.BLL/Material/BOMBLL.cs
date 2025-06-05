using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MES.Common.Exceptions;
using MES.Common.Logging;
using MES.DAL;
using MES.DAL.Material;
using MES.Models.Material;

namespace MES.BLL.Material
{
    public class BOMBLL
    {
        private readonly BOMDAL _bomDAL;

        public BOMBLL()
        {
            _bomDAL = new BOMDAL();
        }

        /// <summary>
        /// 获取所有BOM信息
        /// </summary>
        /// <returns>BOM信息列表</returns>
        public List<BOMInfo> GetAllBOMs()
        {
            try
            {
                return _bomDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有BOM信息失败", ex);
                throw new MESException("获取BOM信息失败", ex);
            }
        }

        /// <summary>
        /// 根据ID获取BOM信息
        /// </summary>
        /// <param name="id">BOM ID</param>
        /// <returns>BOM信息，如果未找到则返回null</returns>
        public BOMInfo GetBOMById(int id)
        {
            try
            {
                return _bomDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据ID获取BOM信息失败，ID: {id}", ex);
                throw new MESException($"获取BOM信息失败，ID: {id}", ex);
            }
        }

        /// <summary>
        /// 根据产品ID获取BOM列表
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>BOM列表</returns>
        public List<BOMInfo> GetBOMsByProductId(int productId)
        {
            try
            {
                return _bomDAL.GetByProductId(productId);
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据产品ID获取BOM列表失败，产品ID: {productId}", ex);
                throw new MESException($"获取BOM列表失败，产品ID: {productId}", ex);
            }
        }

        /// <summary>
        /// 添加新的BOM信息
        /// </summary>
        /// <param name="bom">BOM信息对象</param>
        /// <returns>添加是否成功</returns>
        public bool AddBOM(BOMInfo bom)
        {
            try
            {
                // 这里可以添加业务逻辑验证
                return _bomDAL.Add(bom);
            }
            catch (Exception ex)
            {
                LogManager.Error("添加BOM信息失败", ex);
                throw new MESException("添加BOM信息失败", ex);
            }
        }

        /// <summary>
        /// 更新BOM信息
        /// </summary>
        /// <param name="bom">BOM信息对象</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateBOM(BOMInfo bom)
        {
            try
            {
                // 这里可以添加业务逻辑验证
                return _bomDAL.Update(bom);
            }
            catch (Exception ex)
            {
                LogManager.Error("更新BOM信息失败", ex);
                throw new MESException("更新BOM信息失败", ex);
            }
        }

        /// <summary>
        /// 删除BOM信息（逻辑删除）
        /// </summary>
        /// <param name="id">BOM ID</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteBOM(int id)
        {
            try
            {
                return _bomDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error($"删除BOM信息失败，ID: {id}", ex);
                throw new MESException($"删除BOM信息失败，ID: {id}", ex);
            }
        }
    }
}
