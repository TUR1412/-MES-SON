using System;
using MES.Models.Base;

namespace MES.Models.System
{
    /// <summary>
    /// 数据字典信息模型
    /// 用于系统数据字典管理，存储系统中的枚举值和配置项
    /// </summary>
    public class DictionaryInfo : BaseModel
    {
        /// <summary>
        /// 字典类型编码
        /// </summary>
        public string DictType { get; set; }

        /// <summary>
        /// 字典类型名称
        /// </summary>
        public string DictTypeName { get; set; }

        /// <summary>
        /// 字典项编码
        /// </summary>
        public string DictCode { get; set; }

        /// <summary>
        /// 字典项名称
        /// </summary>
        public string DictName { get; set; }

        /// <summary>
        /// 字典项值
        /// </summary>
        public string DictValue { get; set; }

        /// <summary>
        /// 父级字典ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 状态：1-启用，0-禁用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否系统内置：1-是，0-否
        /// </summary>
        public int IsSystem { get; set; }

        /// <summary>
        /// 字典描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 扩展属性1
        /// </summary>
        public string ExtendField1 { get; set; }

        /// <summary>
        /// 扩展属性2
        /// </summary>
        public string ExtendField2 { get; set; }

        /// <summary>
        /// 扩展属性3
        /// </summary>
        public string ExtendField3 { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DictionaryInfo()
        {
            DictType = string.Empty;
            DictTypeName = string.Empty;
            DictCode = string.Empty;
            DictName = string.Empty;
            DictValue = string.Empty;
            ParentId = 0;
            SortOrder = 0;
            Status = 1; // 默认启用
            IsSystem = 0; // 默认非系统内置
            Description = string.Empty;
            ExtendField1 = string.Empty;
            ExtendField2 = string.Empty;
            ExtendField3 = string.Empty;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="dictType">字典类型编码</param>
        /// <param name="dictCode">字典项编码</param>
        /// <param name="dictName">字典项名称</param>
        /// <param name="dictValue">字典项值</param>
        public DictionaryInfo(string dictType, string dictCode, string dictName, string dictValue = "")
        {
            DictType = dictType ?? string.Empty;
            DictTypeName = string.Empty;
            DictCode = dictCode ?? string.Empty;
            DictName = dictName ?? string.Empty;
            DictValue = string.IsNullOrEmpty(dictValue) ? dictCode : dictValue;
            ParentId = 0;
            SortOrder = 0;
            Status = 1;
            IsSystem = 0;
            Description = string.Empty;
            ExtendField1 = string.Empty;
            ExtendField2 = string.Empty;
            ExtendField3 = string.Empty;
        }

        /// <summary>
        /// 获取状态显示文本
        /// </summary>
        /// <returns>状态文本</returns>
        public string GetStatusText()
        {
            return Status == 1 ? "启用" : "禁用";
        }

        /// <summary>
        /// 检查是否启用
        /// </summary>
        /// <returns>是否启用</returns>
        public bool IsEnabled()
        {
            return Status == 1;
        }

        /// <summary>
        /// 检查是否为系统内置
        /// </summary>
        /// <returns>是否为系统内置</returns>
        public bool IsSystemBuiltIn()
        {
            return IsSystem == 1;
        }

        /// <summary>
        /// 检查是否为顶级字典项
        /// </summary>
        /// <returns>是否为顶级字典项</returns>
        public bool IsTopLevel()
        {
            return ParentId == 0;
        }

        /// <summary>
        /// 验证字典信息是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(DictType) && 
                   !string.IsNullOrEmpty(DictCode) && 
                   !string.IsNullOrEmpty(DictName);
        }

        /// <summary>
        /// 获取完整的字典路径
        /// </summary>
        /// <returns>字典路径</returns>
        public string GetFullPath()
        {
            return string.Format("{0}.{1}", DictType, DictCode);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字典信息字符串</returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}: {2}", DictType, DictCode, DictName);
        }
    }

    /// <summary>
    /// 常用字典类型常量
    /// </summary>
    public static class DictTypes
    {
        /// <summary>
        /// 用户状态
        /// </summary>
        public const string USER_STATUS = "USER_STATUS";

        /// <summary>
        /// 物料类型
        /// </summary>
        public const string MATERIAL_TYPE = "MATERIAL_TYPE";

        /// <summary>
        /// 生产订单状态
        /// </summary>
        public const string PRODUCTION_ORDER_STATUS = "PRODUCTION_ORDER_STATUS";

        /// <summary>
        /// 车间状态
        /// </summary>
        public const string WORKSHOP_STATUS = "WORKSHOP_STATUS";

        /// <summary>
        /// 设备状态
        /// </summary>
        public const string EQUIPMENT_STATUS = "EQUIPMENT_STATUS";

        /// <summary>
        /// 质量检验类型
        /// </summary>
        public const string INSPECTION_TYPE = "INSPECTION_TYPE";

        /// <summary>
        /// 质量检验结果
        /// </summary>
        public const string INSPECTION_RESULT = "INSPECTION_RESULT";

        /// <summary>
        /// 计量单位
        /// </summary>
        public const string UNIT = "UNIT";

        /// <summary>
        /// 优先级
        /// </summary>
        public const string PRIORITY = "PRIORITY";

        /// <summary>
        /// 紧急程度
        /// </summary>
        public const string URGENCY = "URGENCY";
    }
}
