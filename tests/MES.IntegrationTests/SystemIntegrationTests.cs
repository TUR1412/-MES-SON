using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.BLL.Material;
using MES.BLL.Production;
using MES.BLL.Workshop;
using MES.BLL.System;
using MES.Models.Material;
using MES.Models.Production;
using MES.Models.Workshop;
using MES.Models.System;
using MES.Common.Logging;

namespace MES.IntegrationTests
{
    /// <summary>
    /// MES系统集成测试
    /// 测试各模块间的协作和数据一致性
    /// </summary>
    [TestClass]
    public class SystemIntegrationTests
    {
        private IMaterialBLL _materialBLL;
        private IProductionOrderBLL _productionOrderBLL;
        private IWorkshopBLL _workshopBLL;
        private IRoleBLL _roleBLL;

        /// <summary>
        /// 测试初始化
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            try
            {
                _materialBLL = new MaterialBLL();
                _productionOrderBLL = new ProductionOrderBLL();
                _workshopBLL = new WorkshopBLL();
                _roleBLL = new RoleBLL();

                LogManager.Info("集成测试初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error($"集成测试初始化失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试完整的生产流程集成
        /// 物料 → 车间 → 生产订单 → 执行
        /// </summary>
        [TestMethod]
        public void TestCompleteProductionWorkflow()
        {
            try
            {
                LogManager.Info("开始测试完整生产流程集成");

                // 1. 创建测试物料
                var material = CreateTestMaterial();
                Assert.IsNotNull(material, "物料创建失败");
                LogManager.Info($"创建测试物料：{material.MaterialCode}");

                // 2. 创建测试车间
                var workshop = CreateTestWorkshop();
                Assert.IsNotNull(workshop, "车间创建失败");
                LogManager.Info($"创建测试车间：{workshop.WorkshopCode}");

                // 3. 创建生产订单
                var productionOrder = CreateTestProductionOrder(material.MaterialCode, workshop.Id);
                Assert.IsNotNull(productionOrder, "生产订单创建失败");
                LogManager.Info($"创建测试生产订单：{productionOrder.OrderNumber}");

                // 4. 验证数据关联性
                ValidateDataIntegrity(material, workshop, productionOrder);

                // 5. 测试生产流程状态变更
                TestProductionStatusFlow(productionOrder.Id);

                LogManager.Info("完整生产流程集成测试通过");
            }
            catch (Exception ex)
            {
                LogManager.Error($"完整生产流程集成测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试权限系统集成
        /// </summary>
        [TestMethod]
        public void TestPermissionSystemIntegration()
        {
            try
            {
                LogManager.Info("开始测试权限系统集成");

                // 1. 创建测试角色
                var role = CreateTestRole();
                Assert.IsNotNull(role, "角色创建失败");

                // 2. 测试权限验证
                var hasPermission = _roleBLL.HasPermission(role.RoleCode, "MATERIAL_VIEW");
                LogManager.Info($"权限验证结果：{hasPermission}");

                // 3. 测试角色状态管理
                var enableResult = _roleBLL.EnableRole(role.Id);
                Assert.IsTrue(enableResult, "角色启用失败");

                var disableResult = _roleBLL.DisableRole(role.Id);
                Assert.IsTrue(disableResult, "角色禁用失败");

                LogManager.Info("权限系统集成测试通过");
            }
            catch (Exception ex)
            {
                LogManager.Error($"权限系统集成测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试数据一致性
        /// </summary>
        [TestMethod]
        public void TestDataConsistency()
        {
            try
            {
                LogManager.Info("开始测试数据一致性");

                // 1. 获取所有模块的数据统计
                var materialCount = _materialBLL.GetAllMaterials().Count;
                var workshopCount = _workshopBLL.GetAllWorkshops().Count;
                var productionOrderCount = _productionOrderBLL.GetAllProductionOrders().Count;
                var roleCount = _roleBLL.GetAllRoles().Count;

                LogManager.Info($"数据统计 - 物料:{materialCount}, 车间:{workshopCount}, 生产订单:{productionOrderCount}, 角色:{roleCount}");

                // 2. 验证关联数据的一致性
                var productionOrders = _productionOrderBLL.GetAllProductionOrders();
                foreach (var order in productionOrders.Take(5)) // 测试前5个订单
                {
                    // 验证物料是否存在
                    if (!string.IsNullOrEmpty(order.ProductCode))
                    {
                        var material = _materialBLL.GetMaterialByCode(order.ProductCode);
                        if (material == null)
                        {
                            LogManager.Warning($"生产订单 {order.OrderNumber} 引用的物料 {order.ProductCode} 不存在");
                        }
                    }
                }

                LogManager.Info("数据一致性测试通过");
            }
            catch (Exception ex)
            {
                LogManager.Error($"数据一致性测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试系统性能
        /// </summary>
        [TestMethod]
        public void TestSystemPerformance()
        {
            try
            {
                LogManager.Info("开始测试系统性能");

                var startTime = DateTime.Now;

                // 1. 批量数据查询性能测试
                var materials = _materialBLL.GetAllMaterials();
                var workshops = _workshopBLL.GetAllWorkshops();
                var productionOrders = _productionOrderBLL.GetAllProductionOrders();
                var roles = _roleBLL.GetAllRoles();

                var queryTime = DateTime.Now - startTime;
                LogManager.Info($"批量查询耗时：{queryTime.TotalMilliseconds}ms");

                // 2. 分页查询性能测试
                startTime = DateTime.Now;
                int totalCount;
                var pagedMaterials = _materialBLL.GetMaterialsByPage(1, 50, out totalCount);
                var pagedProductionOrders = _productionOrderBLL.GetProductionOrdersByPage(1, 50, out totalCount);

                var pagedQueryTime = DateTime.Now - startTime;
                LogManager.Info($"分页查询耗时：{pagedQueryTime.TotalMilliseconds}ms");

                // 3. 搜索功能性能测试
                startTime = DateTime.Now;
                var searchResults = _materialBLL.SearchMaterials("测试");
                var searchTime = DateTime.Now - startTime;
                LogManager.Info($"搜索功能耗时：{searchTime.TotalMilliseconds}ms");

                // 性能断言（根据实际情况调整阈值）
                Assert.IsTrue(queryTime.TotalSeconds < 10, "批量查询性能不达标");
                Assert.IsTrue(pagedQueryTime.TotalSeconds < 5, "分页查询性能不达标");
                Assert.IsTrue(searchTime.TotalSeconds < 3, "搜索功能性能不达标");

                LogManager.Info("系统性能测试通过");
            }
            catch (Exception ex)
            {
                LogManager.Error($"系统性能测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试异常处理
        /// </summary>
        [TestMethod]
        public void TestExceptionHandling()
        {
            try
            {
                LogManager.Info("开始测试异常处理");

                // 1. 测试无效数据处理
                var invalidMaterial = new MaterialInfo
                {
                    MaterialCode = "", // 无效的空编码
                    MaterialName = "测试物料"
                };

                var result = _materialBLL.AddMaterial(invalidMaterial);
                Assert.IsFalse(result, "应该拒绝无效数据");

                // 2. 测试重复数据处理
                var existingMaterials = _materialBLL.GetAllMaterials();
                if (existingMaterials.Any())
                {
                    var duplicateMaterial = new MaterialInfo
                    {
                        MaterialCode = existingMaterials.First().MaterialCode,
                        MaterialName = "重复测试物料"
                    };

                    var duplicateResult = _materialBLL.AddMaterial(duplicateMaterial);
                    Assert.IsFalse(duplicateResult, "应该拒绝重复数据");
                }

                // 3. 测试不存在记录的操作
                var deleteResult = _materialBLL.DeleteMaterial(-1);
                Assert.IsFalse(deleteResult, "删除不存在的记录应该失败");

                LogManager.Info("异常处理测试通过");
            }
            catch (Exception ex)
            {
                LogManager.Error($"异常处理测试失败：{ex.Message}", ex);
                throw;
            }
        }

        #region 辅助方法

        /// <summary>
        /// 创建测试物料
        /// </summary>
        private MaterialInfo CreateTestMaterial()
        {
            var material = new MaterialInfo
            {
                MaterialCode = $"TEST_MAT_{DateTime.Now:yyyyMMddHHmmss}",
                MaterialName = "集成测试物料",
                MaterialType = "原材料",
                Unit = "个",
                SafetyStock = 100,
                Description = "用于集成测试的物料"
            };

            var success = _materialBLL.AddMaterial(material);
            return success ? _materialBLL.GetMaterialByCode(material.MaterialCode) : null;
        }

        /// <summary>
        /// 创建测试车间
        /// </summary>
        private WorkshopInfo CreateTestWorkshop()
        {
            var workshop = new WorkshopInfo
            {
                WorkshopCode = $"TEST_WS_{DateTime.Now:yyyyMMddHHmmss}",
                WorkshopName = "集成测试车间",
                WorkshopType = "生产车间",
                Capacity = 1000,
                Description = "用于集成测试的车间"
            };

            var success = _workshopBLL.AddWorkshop(workshop);
            return success ? _workshopBLL.GetWorkshopByCode(workshop.WorkshopCode) : null;
        }

        /// <summary>
        /// 创建测试生产订单
        /// </summary>
        private ProductionOrderInfo CreateTestProductionOrder(string productCode, int workshopId)
        {
            var order = new ProductionOrderInfo
            {
                OrderNumber = $"TEST_PO_{DateTime.Now:yyyyMMddHHmmss}",
                ProductCode = productCode,
                ProductName = "集成测试产品",
                PlannedQuantity = 100,
                PlannedStartTime = DateTime.Now.AddDays(1),
                PlannedEndTime = DateTime.Now.AddDays(7),
                Priority = "普通",
                Status = "待开始"
            };

            var success = _productionOrderBLL.AddProductionOrder(order);
            return success ? _productionOrderBLL.GetProductionOrderByNumber(order.OrderNumber) : null;
        }

        /// <summary>
        /// 创建测试角色
        /// </summary>
        private RoleInfo CreateTestRole()
        {
            var role = new RoleInfo
            {
                RoleCode = $"TEST_ROLE_{DateTime.Now:yyyyMMddHHmmss}",
                RoleName = "集成测试角色",
                Description = "用于集成测试的角色",
                Status = 1,
                Permissions = "MATERIAL_VIEW,PRODUCTION_VIEW"
            };

            var success = _roleBLL.AddRole(role);
            return success ? _roleBLL.GetRoleByCode(role.RoleCode) : null;
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        private void ValidateDataIntegrity(MaterialInfo material, WorkshopInfo workshop, ProductionOrderInfo productionOrder)
        {
            Assert.IsNotNull(material, "物料数据为空");
            Assert.IsNotNull(workshop, "车间数据为空");
            Assert.IsNotNull(productionOrder, "生产订单数据为空");

            Assert.AreEqual(material.MaterialCode, productionOrder.ProductCode, "生产订单产品编码与物料编码不匹配");
            
            LogManager.Info("数据完整性验证通过");
        }

        /// <summary>
        /// 测试生产流程状态变更
        /// </summary>
        private void TestProductionStatusFlow(int productionOrderId)
        {
            // 启动生产订单
            var startResult = _productionOrderBLL.StartProductionOrder(productionOrderId);
            Assert.IsTrue(startResult, "启动生产订单失败");

            // 验证状态变更
            var order = _productionOrderBLL.GetProductionOrderById(productionOrderId);
            Assert.AreEqual("进行中", order.Status, "生产订单状态未正确更新");

            // 完成生产订单
            var completeResult = _productionOrderBLL.CompleteProductionOrder(productionOrderId, 95);
            Assert.IsTrue(completeResult, "完成生产订单失败");

            // 验证最终状态
            order = _productionOrderBLL.GetProductionOrderById(productionOrderId);
            Assert.AreEqual("已完成", order.Status, "生产订单最终状态不正确");
            Assert.AreEqual(95, order.ActualQuantity, "实际完成数量不正确");

            LogManager.Info("生产流程状态变更测试通过");
        }

        #endregion

        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                LogManager.Info("集成测试清理完成");
            }
            catch (Exception ex)
            {
                LogManager.Error($"集成测试清理失败：{ex.Message}", ex);
            }
        }
    }
}
