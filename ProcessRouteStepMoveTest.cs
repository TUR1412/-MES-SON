using System;
using System.Collections.Generic;
using MES.BLL.Material;
using MES.Models.Material;
using MES.Common.Logging;

namespace MES.Tests
{
    /// <summary>
    /// 工艺步骤上下移动功能测试
    /// 用于验证修复后的上下移动功能是否正常工作
    /// </summary>
    public class ProcessRouteStepMoveTest
    {
        private readonly ProcessRouteBLL _processRouteBLL;

        public ProcessRouteStepMoveTest()
        {
            _processRouteBLL = new ProcessRouteBLL();
        }

        /// <summary>
        /// 执行完整的移动功能测试
        /// </summary>
        public void RunCompleteTest()
        {
            Console.WriteLine("=== 工艺步骤上下移动功能测试 ===");
            Console.WriteLine();

            try
            {
                // 1. 测试获取工艺路线列表
                TestGetProcessRoutes();

                // 2. 测试获取工艺步骤列表
                TestGetProcessSteps();

                // 3. 测试上移功能
                TestStepMoveUp();

                // 4. 测试下移功能
                TestStepMoveDown();

                // 5. 测试边界条件
                TestBoundaryConditions();

                Console.WriteLine("=== 所有测试完成 ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试过程中发生错误：{ex.Message}");
                LogManager.Error("工艺步骤移动测试失败", ex);
            }
        }

        /// <summary>
        /// 测试获取工艺路线列表
        /// </summary>
        private void TestGetProcessRoutes()
        {
            Console.WriteLine("1. 测试获取工艺路线列表...");
            try
            {
                var routes = _processRouteBLL.GetAllProcessRoutes();
                Console.WriteLine($"   获取到 {routes.Count} 条工艺路线");
                
                if (routes.Count > 0)
                {
                    var firstRoute = routes[0];
                    Console.WriteLine($"   第一条路线：{firstRoute.RouteCode} - {firstRoute.RouteName}");
                }
                Console.WriteLine("   ✓ 获取工艺路线列表成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ 获取工艺路线列表失败：{ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 测试获取工艺步骤列表
        /// </summary>
        private void TestGetProcessSteps()
        {
            Console.WriteLine("2. 测试获取工艺步骤列表...");
            try
            {
                var routes = _processRouteBLL.GetAllProcessRoutes();
                if (routes.Count > 0)
                {
                    var firstRoute = routes[0];
                    var steps = _processRouteBLL.GetProcessSteps(firstRoute.Id);
                    Console.WriteLine($"   路线 {firstRoute.RouteCode} 有 {steps.Count} 个工艺步骤");
                    
                    for (int i = 0; i < Math.Min(steps.Count, 3); i++)
                    {
                        var step = steps[i];
                        Console.WriteLine($"   步骤 {step.StepNumber}: {step.StepName}");
                    }
                    Console.WriteLine("   ✓ 获取工艺步骤列表成功");
                }
                else
                {
                    Console.WriteLine("   ⚠ 没有工艺路线可测试");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ 获取工艺步骤列表失败：{ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 测试步骤上移功能
        /// </summary>
        private void TestStepMoveUp()
        {
            Console.WriteLine("3. 测试步骤上移功能...");
            try
            {
                var routes = _processRouteBLL.GetAllProcessRoutes();
                if (routes.Count > 0)
                {
                    var firstRoute = routes[0];
                    var steps = _processRouteBLL.GetProcessSteps(firstRoute.Id);
                    
                    if (steps.Count >= 2)
                    {
                        var secondStep = steps[1]; // 选择第二个步骤进行上移
                        Console.WriteLine($"   尝试上移步骤：{secondStep.StepName} (序号: {secondStep.StepNumber})");
                        
                        bool result = _processRouteBLL.MoveProcessStep(firstRoute.Id, secondStep.Id, true);
                        if (result)
                        {
                            Console.WriteLine("   ✓ 步骤上移成功");
                            
                            // 验证移动结果
                            var updatedSteps = _processRouteBLL.GetProcessSteps(firstRoute.Id);
                            var movedStep = updatedSteps.Find(s => s.Id == secondStep.Id);
                            Console.WriteLine($"   移动后序号：{movedStep.StepNumber}");
                        }
                        else
                        {
                            Console.WriteLine("   ✗ 步骤上移失败");
                        }
                    }
                    else
                    {
                        Console.WriteLine("   ⚠ 工艺步骤数量不足，无法测试上移");
                    }
                }
                else
                {
                    Console.WriteLine("   ⚠ 没有工艺路线可测试");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ 测试步骤上移失败：{ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 测试步骤下移功能
        /// </summary>
        private void TestStepMoveDown()
        {
            Console.WriteLine("4. 测试步骤下移功能...");
            try
            {
                var routes = _processRouteBLL.GetAllProcessRoutes();
                if (routes.Count > 0)
                {
                    var firstRoute = routes[0];
                    var steps = _processRouteBLL.GetProcessSteps(firstRoute.Id);
                    
                    if (steps.Count >= 2)
                    {
                        var firstStep = steps[0]; // 选择第一个步骤进行下移
                        Console.WriteLine($"   尝试下移步骤：{firstStep.StepName} (序号: {firstStep.StepNumber})");
                        
                        bool result = _processRouteBLL.MoveProcessStep(firstRoute.Id, firstStep.Id, false);
                        if (result)
                        {
                            Console.WriteLine("   ✓ 步骤下移成功");
                            
                            // 验证移动结果
                            var updatedSteps = _processRouteBLL.GetProcessSteps(firstRoute.Id);
                            var movedStep = updatedSteps.Find(s => s.Id == firstStep.Id);
                            Console.WriteLine($"   移动后序号：{movedStep.StepNumber}");
                        }
                        else
                        {
                            Console.WriteLine("   ✗ 步骤下移失败");
                        }
                    }
                    else
                    {
                        Console.WriteLine("   ⚠ 工艺步骤数量不足，无法测试下移");
                    }
                }
                else
                {
                    Console.WriteLine("   ⚠ 没有工艺路线可测试");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ 测试步骤下移失败：{ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 测试边界条件
        /// </summary>
        private void TestBoundaryConditions()
        {
            Console.WriteLine("5. 测试边界条件...");
            try
            {
                var routes = _processRouteBLL.GetAllProcessRoutes();
                if (routes.Count > 0)
                {
                    var firstRoute = routes[0];
                    var steps = _processRouteBLL.GetProcessSteps(firstRoute.Id);
                    
                    if (steps.Count > 0)
                    {
                        // 测试第一个步骤上移（应该返回true但不实际移动）
                        var firstStep = steps[0];
                        Console.WriteLine($"   测试第一个步骤上移：{firstStep.StepName}");
                        bool result1 = _processRouteBLL.MoveProcessStep(firstRoute.Id, firstStep.Id, true);
                        Console.WriteLine($"   结果：{(result1 ? "成功（边界处理正确）" : "失败")}");
                        
                        // 测试最后一个步骤下移（应该返回true但不实际移动）
                        var lastStep = steps[steps.Count - 1];
                        Console.WriteLine($"   测试最后一个步骤下移：{lastStep.StepName}");
                        bool result2 = _processRouteBLL.MoveProcessStep(firstRoute.Id, lastStep.Id, false);
                        Console.WriteLine($"   结果：{(result2 ? "成功（边界处理正确）" : "失败")}");
                        
                        Console.WriteLine("   ✓ 边界条件测试完成");
                    }
                    else
                    {
                        Console.WriteLine("   ⚠ 没有工艺步骤可测试");
                    }
                }
                else
                {
                    Console.WriteLine("   ⚠ 没有工艺路线可测试");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ 测试边界条件失败：{ex.Message}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 程序入口点
        /// </summary>
        public static void Main(string[] args)
        {
            var test = new ProcessRouteStepMoveTest();
            test.RunCompleteTest();
            
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
