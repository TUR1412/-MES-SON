using System;

namespace MES.Tests
{
    /// <summary>
    /// 测试运行器
    /// 用于验证MySQL架构迁移后的系统功能
    /// </summary>
    class TestRunner
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MES系统 - MySQL架构迁移验证测试");
            Console.WriteLine("=====================================");
            Console.WriteLine();

            try
            {
                // 数据库连接测试
                DatabaseConnectionTest.TestConnection();
                Console.WriteLine();

                // 物料表操作测试
                DatabaseConnectionTest.TestMaterialOperations();
                Console.WriteLine();

                Console.WriteLine("🎉 所有测试完成！系统已准备就绪！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 测试过程中发生错误: {ex.Message}");
                Console.WriteLine("请检查数据库配置和连接。");
            }

            Console.WriteLine();
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
