using System;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace DatabaseConnectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MES数据库连接测试 ===");
            
            try
            {
                // 测试连接字符串
                string connectionString = "Server=localhost;Database=mes_db;Uid=root;Pwd=Qwe.123;CharSet=utf8mb4;SslMode=none;";
                Console.WriteLine("连接字符串: " + connectionString);
                
                using (var connection = new MySqlConnection(connectionString))
                {
                    Console.WriteLine("正在连接数据库...");
                    connection.Open();
                    Console.WriteLine("数据库连接成功！");
                    
                    // 测试查询物料数据
                    string sql = "SELECT COUNT(*) FROM material_info WHERE is_deleted = 0";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        var count = command.ExecuteScalar();
                        Console.WriteLine("物料数据数量: " + count);
                    }
                    
                    // 测试查询工单数据
                    sql = "SELECT COUNT(*) FROM work_order_info WHERE is_deleted = 0";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        var count = command.ExecuteScalar();
                        Console.WriteLine("工单数据数量: " + count);
                    }
                    
                    // 测试查询车间数据
                    sql = "SELECT COUNT(*) FROM workshop_info WHERE is_deleted = 0";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        var count = command.ExecuteScalar();
                        Console.WriteLine("车间数据数量: " + count);
                    }

                    // 测试查询设备数据（这是问题窗体）
                    Console.WriteLine("\n=== 测试设备数据查询 ===");
                    sql = @"
                        SELECT e.*, ws.workshop_name
                        FROM equipment_info e
                        LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                        WHERE e.is_deleted = 0 AND e.is_enabled = 1
                        ORDER BY e.workshop_id, e.equipment_code";

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                count++;
                                var id = reader["id"];
                                var code = reader["equipment_code"];
                                var name = reader["equipment_name"];
                                var nextMaintenance = reader["next_maintenance"];

                                Console.WriteLine("设备 " + count + ": " + code + " - " + name);
                                Console.WriteLine("  下次维护: " + nextMaintenance);
                            }
                            Console.WriteLine("设备数据查询成功！共 " + count + " 条记录");
                        }
                    }

                    // 测试查询批次数据（这是问题窗体）
                    Console.WriteLine("\n=== 测试批次数据查询 ===");
                    sql = "SELECT * FROM batch_info WHERE is_deleted = 0 ORDER BY id";
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                count++;
                                var batchNumber = reader["batch_number"];
                                var status = reader["batch_status"];
                                Console.WriteLine("批次 " + count + ": " + batchNumber + " - " + status);
                            }
                            Console.WriteLine("批次数据查询成功！共 " + count + " 条记录");
                        }
                    }

                    // 测试模拟EquipmentDAL的ConvertDataTableToEquipmentList方法
                    Console.WriteLine("\n=== 测试设备数据转换 ===");
                    sql = @"
                        SELECT e.*, ws.workshop_name
                        FROM equipment_info e
                        LEFT JOIN workshop_info ws ON e.workshop_id = ws.id
                        WHERE e.is_deleted = 0 AND e.is_enabled = 1
                        ORDER BY e.workshop_id, e.equipment_code";

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            Console.WriteLine("DataTable填充成功，行数: " + dataTable.Rows.Count);

                            // 模拟ConvertDataTableToEquipmentList的数据转换过程
                            foreach (DataRow row in dataTable.Rows)
                            {
                                try
                                {
                                    var id = Convert.ToInt32(row["id"]);
                                    var code = row["equipment_code"].ToString();
                                    var name = row["equipment_name"].ToString();
                                    var status = Convert.ToInt32(row["status"]);
                                    var efficiency = Convert.ToDecimal(row["efficiency"]);
                                    var temperature = Convert.ToDecimal(row["temperature"]);
                                    var nextMaintenance = Convert.ToDateTime(row["next_maintenance"]);

                                    Console.WriteLine("设备转换成功: " + code + " - " + name);
                                }
                                catch (Exception convEx)
                                {
                                    Console.WriteLine("❌ 设备数据转换失败: " + convEx.Message);
                                    Console.WriteLine("   详细错误: " + convEx.ToString());

                                    // 输出问题行的详细信息
                                    Console.WriteLine("   问题行数据:");
                                    for (int i = 0; i < dataTable.Columns.Count; i++)
                                    {
                                        var columnName = dataTable.Columns[i].ColumnName;
                                        var value = row[i];
                                        var valueType = value != null ? value.GetType().Name : "NULL";
                                        Console.WriteLine("     " + columnName + ": " + value + " (" + valueType + ")");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                
                Console.WriteLine("数据库连接测试完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine("数据库连接失败: " + ex.Message);
                Console.WriteLine("详细错误: " + ex.ToString());
            }
            
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
