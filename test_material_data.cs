using System;
using System.Collections.Generic;
using MES.BLL.Material;
using MES.Models.Material;
using MES.Common.Logging;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("开始测试物料数据加载...");
            
            var materialBLL = new MaterialBLL();
            var materials = materialBLL.GetAllMaterials();
            
            Console.WriteLine(string.Format("成功加载物料数据，共 {0} 条记录", materials.Count));
            
            foreach (var material in materials)
            {
                Console.WriteLine(string.Format("物料: {0} - {1}", material.MaterialCode, material.MaterialName));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(string.Format("测试失败: {0}", ex.Message));
            Console.WriteLine(string.Format("详细错误: {0}", ex.ToString()));
        }
        
        Console.WriteLine("按任意键退出...");
        Console.ReadKey();
    }
}