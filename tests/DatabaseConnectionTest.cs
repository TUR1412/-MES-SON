using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using MES.DAL.Core;
using MES.Common.Logging;

namespace MES.Tests
{
    /// <summary>
    /// 数据库连接测试类
    /// 验证MySQL架构迁移后的连接功能
    /// </summary>
    public class DatabaseConnectionTest
    {
        /// <summary>
        /// 测试数据库连接
        /// </summary>
        public static void TestConnection()
        {
            try
            {
                Console.WriteLine("=== MySQL数据库连接测试 ===");
                Console.WriteLine($"测试时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine();

                // 1. 测试连接字符串获取
                Console.WriteLine("1. 测试连接字符串获取...");
                string connectionString = DatabaseHelper.GetConnectionString();
                Console.WriteLine($"   连接字符串: {MaskConnectionString(connectionString)}");
                Console.WriteLine("   ✅ 连接字符串获取成功");
                Console.WriteLine();

                // 2. 测试数据库连接创建
                Console.WriteLine("2. 测试数据库连接创建...");
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    Console.WriteLine($"   连接类型: {connection.GetType().Name}");
                    Console.WriteLine("   ✅ MySQL连接对象创建成功");
                }
                Console.WriteLine();

                // 3. 测试数据库连接打开
                Console.WriteLine("3. 测试数据库连接打开...");
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    Console.WriteLine($"   连接状态: {connection.State}");
                    Console.WriteLine($"   服务器版本: {connection.ServerVersion}");
                    Console.WriteLine($"   数据库名: {connection.Database}");
                    Console.WriteLine("   ✅ 数据库连接打开成功");
                }
                Console.WriteLine();

                // 4. 测试基本查询
                Console.WriteLine("4. 测试基本查询...");
                var result = DatabaseHelper.ExecuteScalar("SELECT VERSION()");
                Console.WriteLine($"   MySQL版本: {result}");
                Console.WriteLine("   ✅ 基本查询执行成功");
                Console.WriteLine();

                // 5. 测试参数化查询
                Console.WriteLine("5. 测试参数化查询...");
                var param = DatabaseHelper.CreateParameter("@testValue", "测试");
                var paramResult = DatabaseHelper.ExecuteScalar("SELECT @testValue", param);
                Console.WriteLine($"   参数化查询结果: {paramResult}");
                Console.WriteLine("   ✅ 参数化查询执行成功");
                Console.WriteLine();

                Console.WriteLine("=== 所有测试通过！MySQL架构迁移成功！ ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 数据库连接测试失败: {ex.Message}");
                Console.WriteLine($"详细错误: {ex}");
                throw;
            }
        }

        /// <summary>
        /// 掩码连接字符串中的敏感信息
        /// </summary>
        private static string MaskConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "未配置";

            // 简单掩码密码部分
            return connectionString.Replace("Pwd=123456", "Pwd=******")
                                  .Replace("Password=123456", "Password=******");
        }

        /// <summary>
        /// 测试物料表基本操作
        /// </summary>
        public static void TestMaterialOperations()
        {
            try
            {
                Console.WriteLine("=== 物料表操作测试 ===");

                // 测试表是否存在
                var tableExists = DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'material_info'");
                
                Console.WriteLine($"物料表存在检查: {(Convert.ToInt32(tableExists) > 0 ? "✅ 存在" : "❌ 不存在")}");

                if (Convert.ToInt32(tableExists) > 0)
                {
                    // 测试表结构
                    var columnCount = DatabaseHelper.ExecuteScalar(
                        "SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = 'material_info'");
                    Console.WriteLine($"物料表字段数量: {columnCount}");

                    // 测试数据查询
                    var recordCount = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM material_info");
                    Console.WriteLine($"物料表记录数量: {recordCount}");
                }

                Console.WriteLine("✅ 物料表操作测试完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 物料表操作测试失败: {ex.Message}");
            }
        }
    }
}
