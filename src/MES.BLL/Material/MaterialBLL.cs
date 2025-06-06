using MES.BLL.Material.DTO;
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
    public class MaterialBLL : IMaterialBLL
    {
        private readonly MaterialDAL _materialDAL;

        public MaterialBLL()
        {
            _materialDAL = new MaterialDAL();
        }

        /// <summary>
        /// 获取所有物料信息
        /// </summary>
        /// <returns>物料信息列表</returns>
        public List<MaterialInfo> GetAllMaterials()
        {
            try
            {
                return _materialDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有物料信息失败", ex);
                throw new MESException("获取物料信息失败", ex);
            }
        }

        public List<MaterialInfo> SearchByName(string materialName)
        {
            try
            {
                return _materialDAL.SearchByName(materialName);
            }
            catch (Exception ex)
            {
                LogManager.Error($"获取name信息失败，name: {ex}", ex);
                throw new MESException($"获取name信息失败，name: {ex}", ex);
            }

        }

        /// <summary>
        /// 根据ID获取物料信息
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>物料信息，如果未找到则返回null</returns>
        public MaterialInfo GetMaterialById(int id)
        {
            try
            {
                return _materialDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据ID获取物料信息失败，ID: {id}", ex);
                throw new MESException($"获取物料信息失败，ID: {id}", ex);
            }
        }

        /// <summary>
        /// 添加新的物料信息
        /// </summary>
        /// <param name="material">物料信息对象</param>
        /// <returns>添加是否成功</returns>
        public bool AddMaterial(MaterialInfo material)
        {
            try
            {
                // 1. 参数验证
                if (material == null)
                    throw new ArgumentNullException(nameof(material));

                if (string.IsNullOrWhiteSpace(material.MaterialCode))
                    throw new MESException("物料编码不能为空");

                if (string.IsNullOrWhiteSpace(material.MaterialName))
                    throw new MESException("物料名称不能为空");

                // 2. 业务规则验证
                if (_materialDAL.ExistsByMaterialCode(material.MaterialCode))
                    throw new MESException($"物料编码 {material.MaterialCode} 已存在");

                // 3. 数据完整性检查
                if (material.MinStock.HasValue && material.MaxStock.HasValue &&
                    material.MinStock > material.MaxStock)
                    throw new MESException("最小库存不能大于最大库存");

                // 4. 设置默认值
                material.CreateUserName = material.CreateUserName ?? "system";

                // 5. 执行添加操作
                return _materialDAL.Add(material);
            }
            catch (Exception ex)
            {
                LogManager.Error("添加物料信息失败", ex);
                throw new MESException("添加物料信息失败", ex);
            }
        }

        /// <summary>
        /// 更新物料信息
        /// </summary>
        /// <param name="material">物料信息对象</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateMaterial(MaterialInfo material)
        {
            try
            {
                // 这里可以添加业务逻辑验证
                return _materialDAL.Update(material);
            }
            catch (Exception ex)
            {
                LogManager.Error("更新物料信息失败", ex);
                throw new MESException("更新物料信息失败", ex);
            }
        }

        /// <summary>
        /// 删除物料信息（逻辑删除）
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteMaterial(int id)
        {
            try
            {
                return _materialDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error($"删除物料信息失败，ID: {id}", ex);
                throw new MESException($"删除物料信息失败，ID: {id}", ex);
            }
        }

        /// <summary>
        /// 更新物料信息
        /// </summary>
        /// <param name="material">物料信息对象</param>
        /// <returns>更新是否成功</returns>
        public List<MaterialDto> GetAllMaterialDtos()
        {
            return GetAllMaterials().Select(m => new MaterialDto
            {
                Id = m.Id,
                MaterialCode = m.MaterialCode,
                MaterialName = m.MaterialName,
                MaterialType = m.MaterialType,
                Specification = m.Specification,
                Unit = m.Unit,
                Category = m.Category,
                Supplier = m.Supplier,
                StandardCost = m.StandardCost,
                SafetyStock = m.SafetyStock,
                MinStock = m.MinStock,
                MaxStock = m.MaxStock,
                LeadTime = m.LeadTime,
                Status = m.Status
            }).ToList();
        }

        /// <summary>
        /// 删除物料信息（逻辑删除）
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>删除是否成功</returns>
        public List<MaterialDto> SearchMaterialDtosByName(string name)
        {
            return SearchByName(name)
                .Select(m => new MaterialDto
                {
                    Id = m.Id,
                    MaterialCode = m.MaterialCode,
                    MaterialName = m.MaterialName,
                    MaterialType = m.MaterialType,
                    Specification = m.Specification,
                    Unit = m.Unit,
                    Category = m.Category,
                    Supplier = m.Supplier,
                    StandardCost = m.StandardCost,
                    SafetyStock = m.SafetyStock,
                    MinStock = m.MinStock,
                    MaxStock = m.MaxStock,
                    LeadTime = m.LeadTime,
                    Status = m.Status
                }).ToList();
        }
    }
}
