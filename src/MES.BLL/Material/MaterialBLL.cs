using MES.BLL.Material.DTO;
using MES.Common.Exceptions;
using MES.Common.Logging;
using MES.DAL.Material;
using MES.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MES.BLL.Material
{
    public class MaterialBLL : IMaterialBLL
    {
        private readonly MaterialDAL _materialDAL;

        public MaterialBLL()
        {
            _materialDAL = new MaterialDAL();
        }

        #region 内部方法 (使用Model)

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

        public bool AddMaterial(MaterialInfo material)
        {
            try
            {
                // 参数验证
                if (material == null) throw new ArgumentNullException("material");
                if (string.IsNullOrWhiteSpace(material.MaterialCode)) throw new MESException("物料编码不能为空");
                if (string.IsNullOrWhiteSpace(material.MaterialName)) throw new MESException("物料名称不能为空");

                // 业务规则验证
                if (_materialDAL.ExistsByMaterialCode(material.MaterialCode))
                    throw new MESException(string.Format("物料编码 {0} 已存在", material.MaterialCode));

                if (material.MinStock.HasValue && material.MaxStock.HasValue && material.MinStock > material.MaxStock)
                    throw new MESException("最小库存不能大于最大库存");

                // 设置默认值
                material.CreateUserName = material.CreateUserName ?? "system";

                return _materialDAL.Add(material);
            }
            catch (Exception ex)
            {
                LogManager.Error("添加物料信息失败", ex);
                throw new MESException("添加物料信息失败", ex);
            }
        }

        public bool UpdateMaterial(MaterialInfo material)
        {
            try
            {
                // 业务逻辑验证
                if (material == null) return false;
                if (_materialDAL.GetById(material.Id) == null) throw new MESException("待更新的物料不存在");

                return _materialDAL.Update(material);
            }
            catch (Exception ex)
            {
                LogManager.Error("更新物料信息失败", ex);
                throw new MESException("更新物料信息失败", ex);
            }
        }

        #endregion

        #region 接口实现 (面向UI，使用DTO)

        public bool DeleteMaterial(int id)
        {
            try
            {
                return _materialDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除物料信息失败，ID: {0}", id), ex);
                throw new MESException(string.Format("删除物料信息失败，ID: {0}", id), ex);
            }
        }

        public List<MaterialDto> GetAllMaterialDtos()
        {
            var materialList = this.GetAllMaterials();
            // 使用完整的转换方法
            return materialList.Select(ConvertToDto).ToList();
        }

        public bool AddMaterial(MaterialDto materialDto)
        {
            var materialModel = ConvertToModel(materialDto);
            return this.AddMaterial(materialModel);
        }

        public bool UpdateMaterial(MaterialDto materialDto)
        {
            var materialModel = ConvertToModel(materialDto);
            return this.UpdateMaterial(materialModel);
        }

        #endregion

        #region 私有转换方法

        /// <summary>
        /// 将Model转换为DTO
        /// </summary>
        private MaterialDto ConvertToDto(MaterialInfo material)
        {
            if (material == null) return null;
            return new MaterialDto
            {
                Id = material.Id,
                MaterialCode = material.MaterialCode,
                MaterialName = material.MaterialName,
                MaterialType = material.MaterialType,
                Specification = material.Specification,
                Unit = material.Unit,
                Category = material.Category,
                Supplier = material.Supplier,
                StandardCost = material.StandardCost,
                SafetyStock = material.SafetyStock,
                MinStock = material.MinStock,
                MaxStock = material.MaxStock,
                StockQuantity = material.StockQuantity,
                LeadTime = material.LeadTime,
                Status = material.Status,
                // 根据业务逻辑，Price可能来自StandardCost
                Price = material.StandardCost.HasValue ? material.StandardCost.Value : 0,
                Remark = material.Remark
            };
        }

        /// <summary>
        /// 将DTO转换为Model
        /// </summary>
        private MaterialInfo ConvertToModel(MaterialDto dto)
        {
            if (dto == null) return null;

            // 如果是更新操作，最好先获取原实体，再用DTO覆盖，这里简化处理
            return new MaterialInfo
            {
                Id = dto.Id,
                MaterialCode = dto.MaterialCode,
                MaterialName = dto.MaterialName,
                MaterialType = dto.MaterialType,
                Specification = dto.Specification,
                Unit = dto.Unit,
                Category = dto.Category,
                Supplier = dto.Supplier,
                StandardCost = dto.StandardCost,
                SafetyStock = dto.SafetyStock,
                MinStock = dto.MinStock,
                MaxStock = dto.MaxStock,
                StockQuantity = dto.StockQuantity,
                LeadTime = dto.LeadTime,
                Status = dto.Status,
                Remark = dto.Remark
                // BaseModel中的字段（如CreateTime, UpdateTime）由DAL或BLL在操作时自动填充
            };
        }

        #endregion
    }
}