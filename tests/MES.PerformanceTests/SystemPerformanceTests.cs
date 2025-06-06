using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

namespace MES.PerformanceTests
{
    /// <summary>
    /// MES系统性能测试
    /// 测试系统在高负载下的性能表现
    /// </summary>
    [TestClass]
    public class SystemPerformanceTests
    {
        private IMaterialBLL _materialBLL;
        private IProductionOrderBLL _productionOrderBLL;
        private IWorkshopBLL _workshopBLL;
        private IRoleBLL _roleBLL;

        /// <summary>
        /// 性能测试阈值配置
        /// </summary>
        private readonly Dictionary<string, double> _performanceThresholds = new Dictionary<string, double>
        {
            ["BatchQuery_MaxSeconds"] = 5.0,
            ["PagedQuery_MaxSeconds"] = 2.0,
            ["Search_MaxSeconds"] = 1.0,
            ["CRUD_MaxSeconds"] = 0.5,
            ["ConcurrentOperations_MaxSeconds"] = 10.0
        };

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

                LogManager.Info("性能测试初始化完成");
            }
            catch (Exception ex)
            {
                LogManager.Error($"性能测试初始化失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试批量查询性能
        /// </summary>
        [TestMethod]
        public void TestBatchQueryPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                LogManager.Info("开始批量查询性能测试");

                // 执行批量查询
                var materials = _materialBLL.GetAllMaterials();
                var workshops = _workshopBLL.GetAllWorkshops();
                var productionOrders = _productionOrderBLL.GetAllProductionOrders();
                var roles = _roleBLL.GetAllRoles();

                stopwatch.Stop();
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                LogManager.Info($"批量查询完成 - 耗时: {elapsedSeconds:F2}秒");
                LogManager.Info($"查询结果 - 物料: {materials.Count}, 车间: {workshops.Count}, 生产订单: {productionOrders.Count}, 角色: {roles.Count}");

                // 性能断言
                Assert.IsTrue(elapsedSeconds <= _performanceThresholds["BatchQuery_MaxSeconds"], 
                    $"批量查询性能不达标，耗时 {elapsedSeconds:F2}秒，超过阈值 {_performanceThresholds["BatchQuery_MaxSeconds"]}秒");

                LogManager.Info("批量查询性能测试通过");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogManager.Error($"批量查询性能测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试分页查询性能
        /// </summary>
        [TestMethod]
        public void TestPagedQueryPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                LogManager.Info("开始分页查询性能测试");

                int totalCount;
                var pageSize = 50;
                var results = new List<object>();

                // 测试多个模块的分页查询
                var pagedMaterials = _materialBLL.GetMaterialsByPage(1, pageSize, out totalCount);
                results.Add(new { Module = "Material", Count = pagedMaterials.Count, Total = totalCount });

                var pagedProductionOrders = _productionOrderBLL.GetProductionOrdersByPage(1, pageSize, out totalCount);
                results.Add(new { Module = "ProductionOrder", Count = pagedProductionOrders.Count, Total = totalCount });

                var pagedWorkshops = _workshopBLL.GetWorkshopsByPage(1, pageSize, out totalCount);
                results.Add(new { Module = "Workshop", Count = pagedWorkshops.Count, Total = totalCount });

                var pagedRoles = _roleBLL.GetRolesByPage(1, pageSize, out totalCount);
                results.Add(new { Module = "Role", Count = pagedRoles.Count, Total = totalCount });

                stopwatch.Stop();
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                LogManager.Info($"分页查询完成 - 耗时: {elapsedSeconds:F2}秒");
                foreach (var result in results)
                {
                    LogManager.Info($"分页结果 - {result}");
                }

                // 性能断言
                Assert.IsTrue(elapsedSeconds <= _performanceThresholds["PagedQuery_MaxSeconds"], 
                    $"分页查询性能不达标，耗时 {elapsedSeconds:F2}秒，超过阈值 {_performanceThresholds["PagedQuery_MaxSeconds"]}秒");

                LogManager.Info("分页查询性能测试通过");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogManager.Error($"分页查询性能测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试搜索功能性能
        /// </summary>
        [TestMethod]
        public void TestSearchPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                LogManager.Info("开始搜索功能性能测试");

                var searchKeywords = new[] { "测试", "原材料", "生产", "车间", "管理" };
                var searchResults = new List<object>();

                foreach (var keyword in searchKeywords)
                {
                    var materialResults = _materialBLL.SearchMaterials(keyword);
                    var productionResults = _productionOrderBLL.SearchProductionOrders(keyword);
                    var workshopResults = _workshopBLL.SearchWorkshops(keyword);
                    var roleResults = _roleBLL.SearchRoles(keyword);

                    searchResults.Add(new 
                    { 
                        Keyword = keyword,
                        Materials = materialResults.Count,
                        ProductionOrders = productionResults.Count,
                        Workshops = workshopResults.Count,
                        Roles = roleResults.Count
                    });
                }

                stopwatch.Stop();
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                LogManager.Info($"搜索功能测试完成 - 耗时: {elapsedSeconds:F2}秒");
                foreach (var result in searchResults)
                {
                    LogManager.Info($"搜索结果 - {result}");
                }

                // 性能断言
                Assert.IsTrue(elapsedSeconds <= _performanceThresholds["Search_MaxSeconds"], 
                    $"搜索功能性能不达标，耗时 {elapsedSeconds:F2}秒，超过阈值 {_performanceThresholds["Search_MaxSeconds"]}秒");

                LogManager.Info("搜索功能性能测试通过");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogManager.Error($"搜索功能性能测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试CRUD操作性能
        /// </summary>
        [TestMethod]
        public void TestCRUDPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                LogManager.Info("开始CRUD操作性能测试");

                var testMaterial = new MaterialInfo
                {
                    MaterialCode = $"PERF_TEST_{DateTime.Now:yyyyMMddHHmmss}",
                    MaterialName = "性能测试物料",
                    MaterialType = "原材料",
                    Unit = "个",
                    SafetyStock = 100
                };

                // Create
                var createResult = _materialBLL.AddMaterial(testMaterial);
                Assert.IsTrue(createResult, "创建操作失败");

                // Read
                var createdMaterial = _materialBLL.GetMaterialByCode(testMaterial.MaterialCode);
                Assert.IsNotNull(createdMaterial, "读取操作失败");

                // Update
                createdMaterial.MaterialName = "更新后的性能测试物料";
                var updateResult = _materialBLL.UpdateMaterial(createdMaterial);
                Assert.IsTrue(updateResult, "更新操作失败");

                // Delete
                var deleteResult = _materialBLL.DeleteMaterial(createdMaterial.Id);
                Assert.IsTrue(deleteResult, "删除操作失败");

                stopwatch.Stop();
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                LogManager.Info($"CRUD操作完成 - 耗时: {elapsedSeconds:F2}秒");

                // 性能断言
                Assert.IsTrue(elapsedSeconds <= _performanceThresholds["CRUD_MaxSeconds"], 
                    $"CRUD操作性能不达标，耗时 {elapsedSeconds:F2}秒，超过阈值 {_performanceThresholds["CRUD_MaxSeconds"]}秒");

                LogManager.Info("CRUD操作性能测试通过");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogManager.Error($"CRUD操作性能测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试并发操作性能
        /// </summary>
        [TestMethod]
        public void TestConcurrentOperationsPerformance()
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                LogManager.Info("开始并发操作性能测试");

                var concurrentTasks = new List<Task>();
                var taskCount = 10;
                var operationsPerTask = 5;

                // 创建并发任务
                for (int i = 0; i < taskCount; i++)
                {
                    var taskIndex = i;
                    var task = Task.Run(() => PerformConcurrentOperations(taskIndex, operationsPerTask));
                    concurrentTasks.Add(task);
                }

                // 等待所有任务完成
                Task.WaitAll(concurrentTasks.ToArray());

                stopwatch.Stop();
                var elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                LogManager.Info($"并发操作完成 - 耗时: {elapsedSeconds:F2}秒");
                LogManager.Info($"并发配置 - 任务数: {taskCount}, 每任务操作数: {operationsPerTask}");

                // 性能断言
                Assert.IsTrue(elapsedSeconds <= _performanceThresholds["ConcurrentOperations_MaxSeconds"], 
                    $"并发操作性能不达标，耗时 {elapsedSeconds:F2}秒，超过阈值 {_performanceThresholds["ConcurrentOperations_MaxSeconds"]}秒");

                LogManager.Info("并发操作性能测试通过");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogManager.Error($"并发操作性能测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试内存使用性能
        /// </summary>
        [TestMethod]
        public void TestMemoryUsagePerformance()
        {
            try
            {
                LogManager.Info("开始内存使用性能测试");

                // 获取初始内存使用量
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                
                var initialMemory = GC.GetTotalMemory(false);
                LogManager.Info($"初始内存使用: {initialMemory / 1024 / 1024:F2} MB");

                // 执行大量数据操作
                var materials = _materialBLL.GetAllMaterials();
                var workshops = _workshopBLL.GetAllWorkshops();
                var productionOrders = _productionOrderBLL.GetAllProductionOrders();
                var roles = _roleBLL.GetAllRoles();

                // 执行多次查询操作
                for (int i = 0; i < 10; i++)
                {
                    int totalCount;
                    _materialBLL.GetMaterialsByPage(1, 100, out totalCount);
                    _productionOrderBLL.GetProductionOrdersByPage(1, 100, out totalCount);
                }

                // 获取操作后内存使用量
                var afterOperationMemory = GC.GetTotalMemory(false);
                LogManager.Info($"操作后内存使用: {afterOperationMemory / 1024 / 1024:F2} MB");

                // 强制垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var finalMemory = GC.GetTotalMemory(false);
                LogManager.Info($"垃圾回收后内存使用: {finalMemory / 1024 / 1024:F2} MB");

                var memoryIncrease = (finalMemory - initialMemory) / 1024.0 / 1024.0;
                LogManager.Info($"内存增长: {memoryIncrease:F2} MB");

                // 内存使用断言（根据实际情况调整阈值）
                Assert.IsTrue(memoryIncrease < 50, $"内存使用增长过多: {memoryIncrease:F2} MB");

                LogManager.Info("内存使用性能测试通过");
            }
            catch (Exception ex)
            {
                LogManager.Error($"内存使用性能测试失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 执行并发操作
        /// </summary>
        private void PerformConcurrentOperations(int taskIndex, int operationCount)
        {
            try
            {
                for (int i = 0; i < operationCount; i++)
                {
                    var timestamp = DateTime.Now.Ticks;
                    var material = new MaterialInfo
                    {
                        MaterialCode = $"CONC_TEST_{taskIndex}_{i}_{timestamp}",
                        MaterialName = $"并发测试物料_{taskIndex}_{i}",
                        MaterialType = "原材料",
                        Unit = "个",
                        SafetyStock = 100
                    };

                    // 执行CRUD操作
                    var createResult = _materialBLL.AddMaterial(material);
                    if (createResult)
                    {
                        var createdMaterial = _materialBLL.GetMaterialByCode(material.MaterialCode);
                        if (createdMaterial != null)
                        {
                            createdMaterial.MaterialName += "_Updated";
                            _materialBLL.UpdateMaterial(createdMaterial);
                            _materialBLL.DeleteMaterial(createdMaterial.Id);
                        }
                    }
                }

                LogManager.Info($"任务 {taskIndex} 完成 {operationCount} 个并发操作");
            }
            catch (Exception ex)
            {
                LogManager.Error($"并发操作任务 {taskIndex} 失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 生成性能测试报告
        /// </summary>
        [TestMethod]
        public void GeneratePerformanceReport()
        {
            try
            {
                LogManager.Info("开始生成性能测试报告");

                var report = new Dictionary<string, object>
                {
                    ["TestTime"] = DateTime.Now,
                    ["PerformanceThresholds"] = _performanceThresholds,
                    ["SystemInfo"] = new
                    {
                        MachineName = Environment.MachineName,
                        ProcessorCount = Environment.ProcessorCount,
                        OSVersion = Environment.OSVersion.ToString(),
                        WorkingSet = Environment.WorkingSet / 1024 / 1024 // MB
                    }
                };

                LogManager.Info("性能测试报告:");
                LogManager.Info($"测试时间: {report["TestTime"]}");
                LogManager.Info($"系统信息: {report["SystemInfo"]}");
                LogManager.Info($"性能阈值: {string.Join(", ", _performanceThresholds.Select(kv => $"{kv.Key}={kv.Value}"))}");

                LogManager.Info("性能测试报告生成完成");
            }
            catch (Exception ex)
            {
                LogManager.Error($"生成性能测试报告失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 测试清理
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                // 清理测试数据
                var testMaterials = _materialBLL.SearchMaterials("PERF_TEST");
                foreach (var material in testMaterials)
                {
                    _materialBLL.DeleteMaterial(material.Id);
                }

                var concTestMaterials = _materialBLL.SearchMaterials("CONC_TEST");
                foreach (var material in concTestMaterials)
                {
                    _materialBLL.DeleteMaterial(material.Id);
                }

                LogManager.Info("性能测试清理完成");
            }
            catch (Exception ex)
            {
                LogManager.Error($"性能测试清理失败：{ex.Message}", ex);
            }
        }
    }
}
