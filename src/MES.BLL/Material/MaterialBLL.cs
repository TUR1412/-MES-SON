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
            // 对于新增操作，直接将DTO转换为新Model即可
            var materialModel = ConvertToModel(materialDto);
            return this.AddMaterial(materialModel);
        }

        /// <summary>
        /// 更新现有的物料信息（通过DTO）
        /// </summary>
        public bool UpdateMaterial(MaterialDto materialDto)
        {
            try
            {
                // --- 核心修复点 1: 先从数据库获取原始实体 ---
                var existingMaterial = _materialDAL.GetById(materialDto.Id);
                if (existingMaterial == null)
                {
                    // 如果找不到要更新的物料，直接抛出异常，防止后续操作出错
                    throw new MESException(string.Format("ID为 {0} 的物料不存在，无法更新。", materialDto.Id));
                }

                // --- 核心修复点 2: 将DTO的数据更新到已存在的实体上 ---
                var updatedMaterial = MapDtoToModel(materialDto, existingMaterial);

                // 调用内部的UpdateMaterial方法，传入的是一个完整的、最新的实体
                return this.UpdateMaterial(updatedMaterial);
            }
            catch (Exception ex)
            {
                // 捕获所有可能的异常（包括上面手动抛出的），记录日志并向上层抛出通用错误
                LogManager.Error(string.Format("更新物料信息失败，ID: {0}", materialDto.Id), ex);
                throw new MESException("更新物料信息失败", ex);
            }
        }

        /// <summary>
        /// 根据物料编码获取物料信息
        /// </summary>
        public MaterialDto GetMaterialByCode(string materialCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(materialCode))
                    return null;

                var materials = _materialDAL.GetAll();
                var material = materials.FirstOrDefault(m => m.MaterialCode == materialCode);

                return material != null ? ConvertToDto(material) : null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据编码获取物料信息失败，编码: {0}", materialCode), ex);
                throw new MESException("获取物料信息失败", ex);
            }
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

                // --- 核心修复点: 直接从 Model.Price 获取值 ---
                Price = material.Price,
                Remark = material.Remark
            };
        }

        /// <summary>
        /// 将DTO转换为Model
        /// </summary>
        private MaterialInfo ConvertToModel(MaterialDto dto)
        {

            if (dto == null) return null;
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
                Price = dto.Price,
                Remark = dto.Remark
            };
        }

        /// <summary>
        /// (新增方法) 将DTO的数据映射（覆盖）到已存在的Model上（用于更新操作）
        /// </summary>
        /// <param name="dto">包含新数据的DTO</param>
        /// <param name="model">从数据库获取的原始Model</param>
        /// <returns>已更新的Model</returns>
        private MaterialInfo MapDtoToModel(MaterialDto dto, MaterialInfo model)
        {
            // 将DTO中的值赋给从数据库取出的实体
            // 这样可以保留model中原有的、DTO中不存在的字段值（如CreateTime, CreateUserName等）
            model.MaterialCode = dto.MaterialCode;
            model.MaterialName = dto.MaterialName;
            model.MaterialType = dto.MaterialType;
            model.Specification = dto.Specification;
            model.Unit = dto.Unit;
            model.Category = dto.Category;
            model.Supplier = dto.Supplier;
            model.StandardCost = dto.StandardCost;
            model.SafetyStock = dto.SafetyStock;
            model.MinStock = dto.MinStock;
            model.MaxStock = dto.MaxStock;
            // 注意：库存量(StockQuantity)通常不由编辑界面直接修改，所以这里不赋值
            // model.StockQuantity = dto.StockQuantity; 
            model.LeadTime = dto.LeadTime;
            model.Status = dto.Status;
            model.Price = dto.Price;
            model.Remark = dto.Remark;

            // UpdateTime 和 UpdateUserName 等字段将在 BaseDAL.Update() 方法中自动设置

            return model;
        }

        #endregion
    }
}