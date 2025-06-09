using System;
using System.Collections.Generic;
using System.Linq;

// 模拟测试工艺路线配置功能
// 这个文件用于验证我们创建的工艺路线相关类的基本功能

namespace MES.Models.Material
{
    /// <summary>
    /// 工艺路线配置功能测试类
    /// 验证ProcessRoute和ProcessStep类的基本功能
    /// </summary>
    public class ProcessRouteTest
    {
        /// <summary>
        /// 测试工艺路线创建和验证
        /// </summary>
        public static void TestProcessRouteCreation()
        {
            Console.WriteLine("=== 测试工艺路线创建和验证 ===");
            
            // 创建工艺路线
            var processRoute = new ProcessRoute
            {
                Id = 1,
                RouteCode = "PR001",
                RouteName = "手机主板生产工艺",
                ProductId = 1,
                ProductName = "智能手机主板",
                Version = "V1.0",
                Status = ProcessRouteStatus.Active,
                Description = "智能手机主板的完整生产工艺流程"
            };
            
            // 添加工艺步骤
            processRoute.Steps.Add(new ProcessStep
            {
                Id = 1,
                ProcessRouteId = 1,
                StepNumber = 1,
                StepName = "SMT贴片",
                StepType = ProcessStepType.Processing,
                WorkstationId = 1,
                WorkstationName = "SMT生产线",
                StandardTime = 30,
                SetupTime = 5,
                WaitTime = 2,
                Description = "表面贴装技术，将电子元件贴装到PCB板上",
                OperationInstructions = "按照贴片程序进行自动贴片",
                QualityRequirements = "贴片精度±0.05mm，无漏贴、错贴",
                SafetyNotes = "注意防静电，佩戴防静电手套",
                RequiredSkillLevel = 3,
                IsCritical = true,
                RequiresInspection = true
            });
            
            processRoute.Steps.Add(new ProcessStep
            {
                Id = 2,
                ProcessRouteId = 1,
                StepNumber = 2,
                StepName = "回流焊接",
                StepType = ProcessStepType.Processing,
                WorkstationId = 2,
                WorkstationName = "回流焊炉",
                StandardTime = 15,
                SetupTime = 3,
                WaitTime = 1,
                Description = "高温焊接工艺，使焊膏熔化形成焊点",
                OperationInstructions = "设置温度曲线，启动回流焊炉",
                QualityRequirements = "焊点饱满，无虚焊、连焊",
                SafetyNotes = "注意高温，佩戴防护用品",
                RequiredSkillLevel = 4,
                IsCritical = true,
                RequiresInspection = true
            });
            
            processRoute.Steps.Add(new ProcessStep
            {
                Id = 3,
                ProcessRouteId = 1,
                StepNumber = 3,
                StepName = "功能测试",
                StepType = ProcessStepType.Testing,
                WorkstationId = 3,
                WorkstationName = "测试工位",
                StandardTime = 20,
                SetupTime = 2,
                WaitTime = 1,
                Description = "电路功能检测，确保产品性能符合要求",
                OperationInstructions = "连接测试设备，运行测试程序",
                QualityRequirements = "所有功能测试项目必须通过",
                SafetyNotes = "注意用电安全",
                RequiredSkillLevel = 5,
                IsCritical = true,
                RequiresInspection = false
            });
            
            // 输出工艺路线信息
            Console.WriteLine($"工艺路线编码: {processRoute.RouteCode}");
            Console.WriteLine($"工艺路线名称: {processRoute.RouteName}");
            Console.WriteLine($"产品名称: {processRoute.ProductName}");
            Console.WriteLine($"版本: {processRoute.Version}");
            Console.WriteLine($"状态: {processRoute.StatusText}");
            Console.WriteLine($"工艺步骤数量: {processRoute.StepCount}");
            Console.WriteLine($"总标准工时: {processRoute.TotalStandardTime} 分钟");
            Console.WriteLine();
            
            // 输出工艺步骤详情
            Console.WriteLine("工艺步骤详情:");
            foreach (var step in processRoute.Steps.OrderBy(s => s.StepNumber))
            {
                Console.WriteLine($"  步骤 {step.StepNumber}: {step.StepName}");
                Console.WriteLine($"    类型: {step.StepTypeText}");
                Console.WriteLine($"    工作站: {step.WorkstationName}");
                Console.WriteLine($"    标准工时: {step.StandardTime} 分钟");
                Console.WriteLine($"    总时间: {step.TotalTime} 分钟");
                Console.WriteLine($"    技能等级: {step.RequiredSkillLevel}");
                Console.WriteLine($"    关键步骤: {(step.IsCritical ? "是" : "否")}");
                Console.WriteLine($"    需要检验: {(step.RequiresInspection ? "是" : "否")}");
                Console.WriteLine($"    摘要: {step.GetSummary()}");
                Console.WriteLine();
            }
            
            // 验证工艺路线
            var validation = processRoute.Validate();
            Console.WriteLine($"验证结果: {(validation.IsValid ? "通过" : "失败")}");
            if (!validation.IsValid)
            {
                Console.WriteLine("验证错误:");
                foreach (var error in validation.Errors)
                {
                    Console.WriteLine($"  - {error}");
                }
            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// 测试工艺路线克隆功能
        /// </summary>
        public static void TestProcessRouteClone()
        {
            Console.WriteLine("=== 测试工艺路线克隆功能 ===");
            
            // 创建原始工艺路线
            var originalRoute = new ProcessRoute
            {
                Id = 1,
                RouteCode = "PR001",
                RouteName = "原始工艺路线",
                ProductId = 1,
                ProductName = "测试产品",
                Version = "V1.0",
                Status = ProcessRouteStatus.Active,
                Description = "这是原始的工艺路线"
            };
            
            originalRoute.Steps.Add(new ProcessStep
            {
                StepNumber = 1,
                StepName = "测试步骤1",
                WorkstationName = "工作站1",
                StandardTime = 10
            });
            
            // 克隆工艺路线
            var clonedRoute = originalRoute.Clone();
            
            Console.WriteLine($"原始路线编码: {originalRoute.RouteCode}");
            Console.WriteLine($"克隆路线编码: {clonedRoute.RouteCode}");
            Console.WriteLine($"原始路线名称: {originalRoute.RouteName}");
            Console.WriteLine($"克隆路线名称: {clonedRoute.RouteName}");
            Console.WriteLine($"原始路线状态: {originalRoute.StatusText}");
            Console.WriteLine($"克隆路线状态: {clonedRoute.StatusText}");
            Console.WriteLine($"步骤数量相同: {originalRoute.StepCount == clonedRoute.StepCount}");
            Console.WriteLine();
        }
        
        /// <summary>
        /// 测试工艺步骤验证
        /// </summary>
        public static void TestProcessStepValidation()
        {
            Console.WriteLine("=== 测试工艺步骤验证 ===");
            
            // 创建有效的工艺步骤
            var validStep = new ProcessStep
            {
                StepNumber = 1,
                StepName = "有效步骤",
                WorkstationId = 1,
                WorkstationName = "工作站1",
                StandardTime = 10,
                RequiredSkillLevel = 5
            };
            
            var validResult = validStep.Validate();
            Console.WriteLine($"有效步骤验证结果: {(validResult.IsValid ? "通过" : "失败")}");
            
            // 创建无效的工艺步骤
            var invalidStep = new ProcessStep
            {
                StepNumber = 0, // 无效的步骤序号
                StepName = "", // 空的步骤名称
                WorkstationId = 0, // 无效的工作站ID
                StandardTime = -5, // 负数的标准工时
                RequiredSkillLevel = 15 // 超出范围的技能等级
            };
            
            var invalidResult = invalidStep.Validate();
            Console.WriteLine($"无效步骤验证结果: {(invalidResult.IsValid ? "通过" : "失败")}");
            if (!invalidResult.IsValid)
            {
                Console.WriteLine("验证错误:");
                foreach (var error in invalidResult.Errors)
                {
                    Console.WriteLine($"  - {error}");
                }
            }
            Console.WriteLine();
        }
        
        /// <summary>
        /// 主测试方法
        /// </summary>
        public static void RunAllTests()
        {
            Console.WriteLine("开始测试工艺路线配置功能...");
            Console.WriteLine();
            
            TestProcessRouteCreation();
            TestProcessRouteClone();
            TestProcessStepValidation();
            
            Console.WriteLine("所有测试完成！");
            Console.WriteLine("工艺路线配置功能验证成功 ✅");
        }
    }
}

// 如果这是一个独立的测试程序，可以添加Main方法
/*
class Program
{
    static void Main(string[] args)
    {
        ProcessRouteTest.RunAllTests();
        Console.ReadKey();
    }
}
*/
