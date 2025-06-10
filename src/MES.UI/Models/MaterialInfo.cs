using System;

namespace MES.UI.Models
{
    /// <summary>
    /// 物料信息数据模型
    /// </summary>
    public class MaterialInfo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public string MaterialType { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 参考价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MaterialInfo()
        {
            MaterialCode = string.Empty;
            MaterialName = string.Empty;
            MaterialType = string.Empty;
            Unit = string.Empty;
            Specification = string.Empty;
            Supplier = string.Empty;
            Price = 0;
            Remark = string.Empty;
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsEnabled = true;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public MaterialInfo(string materialCode, string materialName, string materialType, string unit)
        {
            MaterialCode = materialCode ?? string.Empty;
            MaterialName = materialName ?? string.Empty;
            MaterialType = materialType ?? string.Empty;
            Unit = unit ?? string.Empty;
            Specification = string.Empty;
            Supplier = string.Empty;
            Price = 0;
            Remark = string.Empty;
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsEnabled = true;
        }

        /// <summary>
        /// 验证物料信息是否有效
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(MaterialCode) &&
                   !string.IsNullOrWhiteSpace(MaterialName) &&
                   !string.IsNullOrWhiteSpace(MaterialType) &&
                   !string.IsNullOrWhiteSpace(Unit);
        }

        /// <summary>
        /// 获取显示文本
        /// </summary>
        public string GetDisplayText()
        {
            return string.Format("{0} - {1}", MaterialCode, MaterialName);
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        public MaterialInfo Clone()
        {
            return new MaterialInfo
            {
                Id = this.Id,
                MaterialCode = this.MaterialCode,
                MaterialName = this.MaterialName,
                MaterialType = this.MaterialType,
                Unit = this.Unit,
                Specification = this.Specification,
                Supplier = this.Supplier,
                Price = this.Price,
                Remark = this.Remark,
                CreateTime = this.CreateTime,
                UpdateTime = this.UpdateTime,
                IsEnabled = this.IsEnabled
            };
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        public override string ToString()
        {
            return GetDisplayText();
        }
    }
}