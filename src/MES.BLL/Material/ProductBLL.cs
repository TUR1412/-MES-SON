using System;
using System.Data;

namespace MES.BLL.Material
{
    /// <summary>
    /// 产品业务逻辑层
    /// </summary>
    public class ProductBLL
    {
        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns>产品数据表</returns>
        public global::System.Data.DataTable GetAllProducts()
        {
            try
            {
                global::System.Data.DataTable table = new global::System.Data.DataTable();
                table.Columns.Add("ProductCode", typeof(string));
                table.Columns.Add("ProductName", typeof(string));
                table.Columns.Add("ProductType", typeof(string));
                table.Columns.Add("Unit", typeof(string));
                
                // 添加示例数据
                table.Rows.Add("PA001", "产品A", "电子产品", "个");
                table.Rows.Add("PB001", "产品B", "机械产品", "台");
                table.Rows.Add("PC001", "产品C", "化工产品", "公斤");
                table.Rows.Add("PD001", "产品D", "纺织产品", "米");
                
                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取产品列表失败：{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// 根据产品编号获取产品信息
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>产品信息</returns>
        public ProductModel GetProductByCode(string productCode)
        {
            try
            {
                // 模拟数据
                switch (productCode)
                {
                    case "PA001":
                        return new ProductModel
                        {
                            ProductCode = "PA001",
                            ProductName = "产品A",
                            ProductType = "电子产品",
                            Unit = "个"
                        };
                    case "PB001":
                        return new ProductModel
                        {
                            ProductCode = "PB001",
                            ProductName = "产品B",
                            ProductType = "机械产品",
                            Unit = "台"
                        };
                    case "PC001":
                        return new ProductModel
                        {
                            ProductCode = "PC001",
                            ProductName = "产品C",
                            ProductType = "化工产品",
                            Unit = "公斤"
                        };
                    case "PD001":
                        return new ProductModel
                        {
                            ProductCode = "PD001",
                            ProductName = "产品D",
                            ProductType = "纺织产品",
                            Unit = "米"
                        };
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("获取产品信息失败：{0}", ex.Message), ex);
            }
        }
    }

    /// <summary>
    /// 产品模型（临时定义，应该在Models项目中）
    /// </summary>
    public class ProductModel
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string Unit { get; set; }
    }
}
