using MES.BLL.Material.DTO;
using MES.Common.Exceptions;
using MES.Common.Configuration;
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
        private static readonly object InMemoryLock = new object();
        private static readonly List<MaterialInfo> InMemoryMaterials = new List<MaterialInfo>();
        private static int InMemoryNextId = 1;

        private readonly MaterialDAL _materialDAL;
        private readonly bool _useInMemory;

        public MaterialBLL()
        {
            _materialDAL = new MaterialDAL();
            _useInMemory = ConfigManager.UseInMemoryData;

            if (_useInMemory)
            {
                EnsureInMemorySeedData();
            }
        }

        /// <summary>
        /// 获取所有物料信息
        /// </summary>
        /// <returns>物料信息列表</returns>
        public List<MaterialInfo> GetAllMaterials()
        {
            try
            {
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryMaterials.Where(m => !m.IsDeleted).Select(CloneMaterial).ToList();
                    }
                }

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
                if (string.IsNullOrWhiteSpace(materialName))
                    throw new ArgumentException("物料名称不能为空", "materialName");

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryMaterials
                            .Where(m => !m.IsDeleted && (m.MaterialName ?? string.Empty).IndexOf(materialName, StringComparison.OrdinalIgnoreCase) >= 0)
                            .Select(CloneMaterial)
                            .ToList();
                    }
                }

                return _materialDAL.SearchByName(materialName);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据名称搜索物料信息失败，名称: {0}", materialName), ex);
                throw new MESException(string.Format("搜索物料信息失败，名称: {0}", materialName), ex);
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
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var material = InMemoryMaterials.FirstOrDefault(m => !m.IsDeleted && m.Id == id);
                        return material != null ? CloneMaterial(material) : null;
                    }
                }

                return _materialDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据ID获取物料信息失败，ID: {0}", id), ex);
                throw new MESException(string.Format("获取物料信息失败，ID: {0}", id), ex);
            }
        }

        /// <summary>
        /// 根据物料编码获取物料信息
        /// </summary>
        /// <param name="materialCode">物料编码</param>
        /// <returns>物料信息，不存在时返回null</returns>
        public MaterialInfo GetMaterialByCode(string materialCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(materialCode))
                {
                    return null;
                }

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var material = InMemoryMaterials.FirstOrDefault(m =>
                            !m.IsDeleted &&
                            !string.IsNullOrEmpty(m.MaterialCode) &&
                            m.MaterialCode.Equals(materialCode, StringComparison.OrdinalIgnoreCase));

                        return material != null ? CloneMaterial(material) : null;
                    }
                }

                return _materialDAL.GetByMaterialCode(materialCode);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据物料编码获取物料信息失败，编码: {0}", materialCode), ex);
                throw new MESException(string.Format("获取物料信息失败，编码: {0}", materialCode), ex);
            }
        }

        /// <summary>
        /// 搜索物料（编码/名称/类型/规格/分类/供应商）
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>匹配的物料列表</returns>
        public List<MaterialInfo> SearchMaterials(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return GetAllMaterials();
                }

                var kw = keyword.Trim();

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryMaterials
                            .Where(m => !m.IsDeleted &&
                                       ((m.MaterialCode ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        (m.MaterialName ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        (m.MaterialType ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        (m.Specification ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        (m.Category ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        (m.Supplier ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0))
                            .Select(CloneMaterial)
                            .ToList();
                    }
                }

                var data = _materialDAL.GetAll();
                return data
                    .Where(m => m != null &&
                               ((m.MaterialCode ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                (m.MaterialName ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                (m.MaterialType ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                (m.Specification ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                (m.Category ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                (m.Supplier ?? string.Empty).IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索物料失败，关键字: {0}", keyword), ex);
                throw new MESException(string.Format("搜索物料失败，关键字: {0}", keyword), ex);
            }
        }

        /// <summary>
        /// 分页获取物料列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的物料列表</returns>
        public List<MaterialInfo> GetMaterialsByPage(int pageIndex, int pageSize, out int totalCount)
        {
            try
            {
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    totalCount = 0;
                    return new List<MaterialInfo>();
                }

                var all = GetAllMaterials();
                totalCount = all.Count;

                return all
                    .OrderBy(m => m.Id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("分页获取物料失败，pageIndex={0}, pageSize={1}", pageIndex, pageSize), ex);
                totalCount = 0;
                throw new MESException("分页获取物料失败", ex);
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
                    throw new ArgumentNullException("material");

                if (string.IsNullOrWhiteSpace(material.MaterialCode))
                    throw new MESException("物料编码不能为空");

                if (string.IsNullOrWhiteSpace(material.MaterialName))
                    throw new MESException("物料名称不能为空");

                // 2. 业务规则验证
                if (IsMaterialCodeExists(material.MaterialCode))
                    throw new MESException(string.Format("物料编码 {0} 已存在", material.MaterialCode));

                // 3. 数据完整性检查
                if (material.MinStock.HasValue && material.MaxStock.HasValue &&
                    material.MinStock > material.MaxStock)
                    throw new MESException("最小库存不能大于最大库存");

                // 4. 设置默认值
                material.CreateUserName = material.CreateUserName ?? "system";

                // 5. 执行添加操作
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var entity = CloneMaterial(material);
                        entity.Id = InMemoryNextId++;
                        entity.CreateTime = DateTime.Now;
                        entity.IsDeleted = false;
                        InMemoryMaterials.Add(entity);
                        return true;
                    }
                }

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
                if (material == null)
                    throw new ArgumentNullException("material");

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var existing = InMemoryMaterials.FirstOrDefault(m => !m.IsDeleted && m.Id == material.Id);
                        if (existing == null)
                        {
                            return false;
                        }

                        if (IsMaterialCodeExists(material.MaterialCode, material.Id))
                            throw new MESException(string.Format("物料编码 {0} 已存在", material.MaterialCode));

                        existing.MaterialCode = material.MaterialCode;
                        existing.MaterialName = material.MaterialName;
                        existing.MaterialType = material.MaterialType;
                        existing.Category = material.Category;
                        existing.Specification = material.Specification;
                        existing.Unit = material.Unit;
                        existing.Supplier = material.Supplier;
                        existing.StandardCost = material.StandardCost;
                        existing.SafetyStock = material.SafetyStock;
                        existing.MinStock = material.MinStock;
                        existing.MaxStock = material.MaxStock;
                        existing.StockQuantity = material.StockQuantity;
                        existing.LeadTime = material.LeadTime;
                        existing.Status = material.Status;
                        existing.UpdateTime = DateTime.Now;
                        existing.UpdateUserName = material.UpdateUserName;
                        return true;
                    }
                }

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
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var existing = InMemoryMaterials.FirstOrDefault(m => !m.IsDeleted && m.Id == id);
                        if (existing == null)
                        {
                            return false;
                        }

                        existing.SetDeleteInfo(0, "system");
                        return true;
                    }
                }

                return _materialDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除物料信息失败，ID: {0}", id), ex);
                throw new MESException(string.Format("删除物料信息失败，ID: {0}", id), ex);
            }
        }


        // 添加私有方法
        private MaterialDto ConvertToDto(MaterialInfo material)
        {
            return new MaterialDto
            {
                Id = material.Id,
                MaterialCode = material.MaterialCode,
                MaterialName = material.MaterialName,
                // ... 其他属性
            };
        }

        /// <summary>
        /// 获取所有物料的DTO对象列表
        /// </summary>
        /// <returns>物料DTO列表</returns>
        public List<MaterialDto> GetAllMaterialDtos()
        {
            return GetAllMaterials().Select(ConvertToDto).ToList();
        }

        /// <summary>
        /// 删除物料信息（逻辑删除）
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>删除是否成功</returns>
        public List<MaterialDto> SearchMaterialDtosByName(string name)
        {
            return SearchByName(name).Select(ConvertToDto).ToList();
        }

        private bool IsMaterialCodeExists(string materialCode, int excludeId = 0)
        {
            if (string.IsNullOrWhiteSpace(materialCode))
                return false;

            if (_useInMemory)
            {
                lock (InMemoryLock)
                {
                    return InMemoryMaterials.Any(m =>
                        !m.IsDeleted &&
                        m.MaterialCode != null &&
                        m.MaterialCode.Equals(materialCode, StringComparison.OrdinalIgnoreCase) &&
                        (excludeId <= 0 || m.Id != excludeId));
                }
            }

            return _materialDAL.ExistsByMaterialCode(materialCode, excludeId);
        }

        private static MaterialInfo CloneMaterial(MaterialInfo material)
        {
            if (material == null) return null;

            return new MaterialInfo
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
                CreateTime = material.CreateTime,
                CreateUserId = material.CreateUserId,
                CreateUserName = material.CreateUserName,
                UpdateTime = material.UpdateTime,
                UpdateUserId = material.UpdateUserId,
                UpdateUserName = material.UpdateUserName,
                IsDeleted = material.IsDeleted,
                DeleteTime = material.DeleteTime,
                DeleteUserId = material.DeleteUserId,
                DeleteUserName = material.DeleteUserName,
                Remark = material.Remark,
                Version = material.Version
            };
        }

        private static void EnsureInMemorySeedData()
        {
            lock (InMemoryLock)
            {
                if (InMemoryMaterials.Count > 0) return;

                var now = DateTime.Now;

                InMemoryMaterials.Add(new MaterialInfo
                {
                    Id = InMemoryNextId++,
                    MaterialCode = "M001",
                    MaterialName = "钢板",
                    MaterialType = "原材料",
                    Unit = "kg",
                    Category = "金属材料",
                    Supplier = "默认供应商",
                    StandardCost = 4.8m,
                    StockQuantity = 1000m,
                    Status = true,
                    CreateTime = now,
                    CreateUserName = "seed"
                });

                InMemoryMaterials.Add(new MaterialInfo
                {
                    Id = InMemoryNextId++,
                    MaterialCode = "M002",
                    MaterialName = "螺丝",
                    MaterialType = "辅料",
                    Unit = "个",
                    Category = "紧固件",
                    Supplier = "默认供应商",
                    StandardCost = 0.05m,
                    StockQuantity = 5000m,
                    Status = true,
                    CreateTime = now,
                    CreateUserName = "seed"
                });
            }
        }
    }
}
