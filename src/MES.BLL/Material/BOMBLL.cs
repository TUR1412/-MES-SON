using MES.Common.Exceptions;
using MES.Common.Logging;
using MES.Common.Configuration;
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
        private static readonly object InMemoryLock = new object();
        private static readonly List<BOMInfo> InMemoryBoms = new List<BOMInfo>();
        private static int InMemoryNextId = 1;

        private readonly BOMDAL _bomDAL;
        private readonly bool _useInMemory;

        public BOMBLL()
        {
            _bomDAL = new BOMDAL();
            _useInMemory = ConfigManager.UseInMemoryData;

            if (_useInMemory)
            {
                EnsureInMemorySeedData();
            }
        }

        /// <summary>
        /// 获取所有BOM信息
        /// </summary>
        /// <returns>BOM信息列表</returns>
        public List<BOMInfo> GetAllBOMs()
        {
            try
            {
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryBoms.Where(b => !b.IsDeleted).Select(CloneBom).ToList();
                    }
                }

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
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var bom = InMemoryBoms.FirstOrDefault(b => !b.IsDeleted && b.Id == id);
                        return bom != null ? CloneBom(bom) : null;
                    }
                }

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
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        return InMemoryBoms.Where(b => !b.IsDeleted && b.ProductId == productId).Select(CloneBom).ToList();
                    }
                }

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
                if (bom == null)
                    throw new ArgumentNullException("bom");

                if (string.IsNullOrWhiteSpace(bom.BOMCode))
                    throw new MESException("BOM编码不能为空");

                if (bom.Quantity <= 0)
                    throw new MESException("BOM用量必须大于0");

                // 业务唯一性：BOM编码
                if (IsBomCodeExists(bom.BOMCode))
                    throw new MESException(string.Format("BOM编码 {0} 已存在", bom.BOMCode));

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var entity = CloneBom(bom);
                        entity.Id = InMemoryNextId++;
                        entity.CreateTime = DateTime.Now;
                        entity.IsDeleted = false;
                        InMemoryBoms.Add(entity);
                        return true;
                    }
                }

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
                if (bom == null)
                    throw new ArgumentNullException("bom");

                if (string.IsNullOrWhiteSpace(bom.BOMCode))
                    throw new MESException("BOM编码不能为空");

                if (bom.Quantity <= 0)
                    throw new MESException("BOM用量必须大于0");

                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var existing = InMemoryBoms.FirstOrDefault(b => !b.IsDeleted && b.Id == bom.Id);
                        if (existing == null) return false;

                        if (IsBomCodeExists(bom.BOMCode, bom.Id))
                            throw new MESException(string.Format("BOM编码 {0} 已存在", bom.BOMCode));

                        existing.BOMCode = bom.BOMCode;
                        existing.ProductId = bom.ProductId;
                        existing.ProductCode = bom.ProductCode;
                        existing.ProductName = bom.ProductName;
                        existing.MaterialId = bom.MaterialId;
                        existing.MaterialCode = bom.MaterialCode;
                        existing.MaterialName = bom.MaterialName;
                        existing.Quantity = bom.Quantity;
                        existing.Unit = bom.Unit;
                        existing.LossRate = bom.LossRate;
                        existing.SubstituteMaterial = bom.SubstituteMaterial;
                        existing.Remarks = bom.Remarks;
                        existing.BOMVersion = bom.BOMVersion;
                        existing.BOMType = bom.BOMType;
                        existing.EffectiveDate = bom.EffectiveDate;
                        existing.ExpireDate = bom.ExpireDate;
                        existing.Status = bom.Status;
                        existing.UpdateTime = DateTime.Now;
                        existing.UpdateUserName = bom.UpdateUserName;
                        return true;
                    }
                }

                if (IsBomCodeExists(bom.BOMCode, bom.Id))
                    throw new MESException(string.Format("BOM编码 {0} 已存在", bom.BOMCode));

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
                if (_useInMemory)
                {
                    lock (InMemoryLock)
                    {
                        var existing = InMemoryBoms.FirstOrDefault(b => !b.IsDeleted && b.Id == id);
                        if (existing == null) return false;

                        existing.SetDeleteInfo(0, "system");
                        return true;
                    }
                }

                return _bomDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除BOM信息失败，ID: {0}", id), ex);
                throw new MESException(string.Format("删除BOM信息失败，ID: {0}", id), ex);
            }
        }

        private bool IsBomCodeExists(string bomCode, int excludeId = 0)
        {
            if (string.IsNullOrWhiteSpace(bomCode))
                return false;

            if (_useInMemory)
            {
                lock (InMemoryLock)
                {
                    return InMemoryBoms.Any(b =>
                        !b.IsDeleted &&
                        !string.IsNullOrEmpty(b.BOMCode) &&
                        b.BOMCode.Equals(bomCode, StringComparison.OrdinalIgnoreCase) &&
                        (excludeId <= 0 || b.Id != excludeId));
                }
            }

            return _bomDAL.ExistsByBomCode(bomCode, excludeId);
        }

        private static BOMInfo CloneBom(BOMInfo bom)
        {
            if (bom == null) return null;

            return new BOMInfo
            {
                Id = bom.Id,
                BOMCode = bom.BOMCode,
                ProductId = bom.ProductId,
                ProductCode = bom.ProductCode,
                ProductName = bom.ProductName,
                MaterialId = bom.MaterialId,
                MaterialCode = bom.MaterialCode,
                MaterialName = bom.MaterialName,
                Quantity = bom.Quantity,
                Unit = bom.Unit,
                LossRate = bom.LossRate,
                SubstituteMaterial = bom.SubstituteMaterial,
                Remarks = bom.Remarks,
                BOMVersion = bom.BOMVersion,
                BOMType = bom.BOMType,
                EffectiveDate = bom.EffectiveDate,
                ExpireDate = bom.ExpireDate,
                Status = bom.Status,
                CreateTime = bom.CreateTime,
                CreateUserId = bom.CreateUserId,
                CreateUserName = bom.CreateUserName,
                UpdateTime = bom.UpdateTime,
                UpdateUserId = bom.UpdateUserId,
                UpdateUserName = bom.UpdateUserName,
                IsDeleted = bom.IsDeleted,
                DeleteTime = bom.DeleteTime,
                DeleteUserId = bom.DeleteUserId,
                DeleteUserName = bom.DeleteUserName,
                Remark = bom.Remark,
                Version = bom.Version
            };
        }

        private static void EnsureInMemorySeedData()
        {
            lock (InMemoryLock)
            {
                if (InMemoryBoms.Count > 0) return;

                var now = DateTime.Now;

                InMemoryBoms.Add(new BOMInfo
                {
                    Id = InMemoryNextId++,
                    BOMCode = "BOM-001",
                    ProductId = 1,
                    ProductCode = "P001",
                    ProductName = "示例产品",
                    MaterialId = 2,
                    MaterialCode = "M002",
                    MaterialName = "螺丝",
                    Quantity = 4m,
                    Unit = "个",
                    LossRate = 0m,
                    BOMVersion = "1.0",
                    BOMType = "PRODUCTION",
                    EffectiveDate = now,
                    Status = true,
                    Remarks = "种子数据",
                    CreateTime = now,
                    CreateUserName = "seed"
                });
            }
        }
    }
}
