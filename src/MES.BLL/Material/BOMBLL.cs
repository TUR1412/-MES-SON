using MES.Common.Exceptions;
using MES.Common.Logging;
using MES.DAL.Material;
using MES.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MES.BLL.Material
{
    public class BOMBLL : IBOMBLL
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
                LogManager.Error(string.Format("根据ID获取BOM信息失败，ID: {0}", id), ex);
                throw new MESException(string.Format("获取BOM信息失败，ID: {0}", id), ex);
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
                LogManager.Error(string.Format("根据产品ID获取BOM列表失败，产品ID: {0}", productId), ex);
                throw new MESException(string.Format("获取BOM列表失败，产品ID: {0}", productId), ex);
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
                LogManager.Error(string.Format("删除BOM信息失败，ID: {0}", id), ex);
                throw new MESException(string.Format("删除BOM信息失败，ID: {0}", id), ex);
            }
        }

        /// <summary>
        /// 根据产品编号获取默认BOM
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>默认BOM信息</returns>
        public BOMModel GetDefaultBOMByProduct(string productCode)
        {
            try
            {
                // 模拟返回默认BOM
                return new BOMModel
                {
                    BOMCode = string.Format("BOM_{0}", productCode),
                    Version = "V1.0",
                    ProductCode = productCode,
                    Status = "有效"
                };
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据产品编号获取默认BOM失败，产品编号: {0}", productCode), ex);
                throw new MESException(string.Format("获取默认BOM失败，产品编号: {0}", productCode), ex);
            }
        }

        /// <summary>
        /// 获取BOM明细
        /// </summary>
        /// <param name="bomCode">BOM编号</param>
        /// <param name="bomVersion">BOM版本</param>
        /// <returns>BOM明细数据表</returns>
        public global::System.Data.DataTable GetBOMDetails(string bomCode, string bomVersion)
        {
            try
            {
                var table = new global::System.Data.DataTable();
                table.Columns.Add("MaterialCode", typeof(string));
                table.Columns.Add("MaterialName", typeof(string));
                table.Columns.Add("Specification", typeof(string));
                table.Columns.Add("RequiredQuantity", typeof(decimal));
                table.Columns.Add("Unit", typeof(string));

                // 添加示例数据
                table.Rows.Add("MAT001", "电阻", "1K欧", 10, "个");
                table.Rows.Add("MAT002", "电容", "100uF", 5, "个");
                table.Rows.Add("MAT003", "芯片", "STM32", 1, "个");
                table.Rows.Add("MAT004", "PCB板", "FR4", 1, "片");
                table.Rows.Add("MAT005", "外壳", "塑料", 1, "个");

                return table;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取BOM明细失败，BOM编号: {0}, 版本: {1}", bomCode, bomVersion), ex);
                throw new MESException(string.Format("获取BOM明细失败，BOM编号: {0}, 版本: {1}", bomCode, bomVersion), ex);
            }
        }
    }

    /// <summary>
    /// BOM模型（临时定义，应该在Models项目中）
    /// </summary>
    public class BOMModel
    {
        public string BOMCode { get; set; }
        public string Version { get; set; }
        public string ProductCode { get; set; }
        public string Status { get; set; }
    }
}
