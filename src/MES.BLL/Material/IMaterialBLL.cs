using MES.BLL.Material.DTO; // 引用DTO命名空间
using MES.Models.Analytics;
using System.Collections.Generic;

namespace MES.BLL.Material
{
    /// <summary>
    /// 物料业务逻辑层接口
    /// </summary>
    public interface IMaterialBLL
    {
        /// <summary>
        /// 获取所有物料的数据传输对象（DTO）列表
        /// </summary>
        /// <returns>物料DTO列表</returns>
        List<MaterialDto> GetAllMaterialDtos();

        /// <summary>
        /// 添加新的物料信息（通过DTO）
        /// </summary>
        /// <param name="materialDto">包含新物料信息的DTO对象</param>
        /// <returns>添加成功则返回true，否则返回false</returns>
        bool AddMaterial(MaterialDto materialDto);

        /// <summary>
        /// 更新现有的物料信息（通过DTO）
        /// </summary>
        /// <param name="materialDto">包含更新后物料信息的DTO对象</param>
        /// <returns>更新成功则返回true，否则返回false</returns>
        bool UpdateMaterial(MaterialDto materialDto);

        /// <summary>
        /// 根据ID删除物料（逻辑删除）
        /// </summary>
        /// <param name="id">要删除的物料ID</param>
        /// <returns>删除成功则返回true，否则返回false</returns>
        bool DeleteMaterial(int id);

        /// <summary>
        /// 获取物料库存告警摘要
        /// </summary>
        /// <param name="top">返回告警列表数量</param>
        /// <returns>库存告警摘要</returns>
        MaterialStockAlertSummary GetMaterialStockAlertSummary(int top = 5);
    }
}
