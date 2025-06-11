// --- START OF FILE ProcessPackage.cs ---

using MES.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MES.Models.Material
{
    /// <summary>
    /// 工艺包状态枚举
    /// </summary>
    public enum ProcessPackageStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Active = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Inactive = 2
    }

    /// <summary>
    /// 工艺包模型
    /// 定义产品的工艺包信息，包含多个工艺流程
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public class ProcessPackage : BaseModel
    {
        /// <summary>
        /// 工艺包编码
        /// </summary>
        public string PackageCode { get; set; }

        /// <summary>
        /// 工艺包名称
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称 (从关联查询获取)
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public new string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ProcessPackageStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case ProcessPackageStatus.Draft: return "草稿";
                    case ProcessPackageStatus.Active: return "启用";
                    case ProcessPackageStatus.Inactive: return "停用";
                    default: return "未知";
                }
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 工艺流程列表
        /// </summary>
        public List<ProcessFlow> Flows { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessPackage()
        {
            Flows = new List<ProcessFlow>();
            Status = ProcessPackageStatus.Draft;
            Version = "V1.0";
        }

        /// <summary>
        /// 工艺流程数量
        /// </summary>
        public int FlowCount
        {
            get
            {
                return Flows != null ? Flows.Count : 0;
            }
        }

        /// <summary>
        /// 总工艺路线数量
        /// </summary>
        public int TotalRouteCount
        {
            get
            {
                if (Flows == null) return 0;
                int count = 0;
                foreach (var flow in Flows)
                {
                    count += flow.RouteCount;
                }
                return count;
            }
        }

        /// <summary>
        /// 总工艺步骤数量
        /// </summary>
        public int TotalStepCount
        {
            get
            {
                if (Flows == null) return 0;
                int count = 0;
                foreach (var flow in Flows)
                {
                    count += flow.TotalStepCount;
                }
                return count;
            }
        }

        /// <summary>
        /// 总标准工时
        /// </summary>
        public decimal TotalStandardTime
        {
            get
            {
                if (Flows == null) return 0;
                decimal total = 0;
                foreach (var flow in Flows)
                {
                    total += flow.TotalStandardTime;
                }
                return total;
            }
        }

        /// <summary>
        /// 验证工艺包数据
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(PackageCode))
            {
                result.AddError("工艺包编码不能为空");
            }

            if (string.IsNullOrWhiteSpace(PackageName))
            {
                result.AddError("工艺包名称不能为空");
            }

            if (ProductId <= 0)
            {
                result.AddError("必须关联一个产品");
            }

            if (string.IsNullOrWhiteSpace(Version))
            {
                result.AddError("版本号不能为空");
            }

            return result;
        }

        /// <summary>
        /// 克隆工艺包（用于复制功能）
        /// 严格遵循C# 5.0语法规范
        /// </summary>
        /// <returns>克隆的工艺包</returns>
        public ProcessPackage Clone()
        {
            var cloned = new ProcessPackage
            {
                PackageCode = this.PackageCode,
                PackageName = this.PackageName,
                ProductId = this.ProductId,
                ProductName = this.ProductName,
                Version = this.Version,
                Status = ProcessPackageStatus.Draft, // 复制的包默认为草稿状态
                Description = this.Description,
                CreateTime = DateTime.Now,
                IsDeleted = false
            };

            // 克隆工艺流程列表
            if (this.Flows != null)
            {
                cloned.Flows = new List<ProcessFlow>();
                foreach (var flow in this.Flows)
                {
                    var clonedFlow = flow.Clone();
                    clonedFlow.ProcessPackageId = 0; // 重置关联ID
                    cloned.Flows.Add(clonedFlow);
                }
            }

            return cloned;
        }

        /// <summary>
        /// 获取工艺包的显示文本
        /// </summary>
        /// <returns>显示文本</returns>
        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", PackageCode, PackageName, StatusText);
        }
    }

    /// <summary>
    /// 工艺包统计信息
    /// </summary>
    public class ProcessPackageStatistics
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 启用数量
        /// </summary>
        public int ActiveCount { get; set; }

        /// <summary>
        /// 停用数量
        /// </summary>
        public int InactiveCount { get; set; }

        /// <summary>
        /// 草稿数量
        /// </summary>
        public int DraftCount { get; set; }

        /// <summary>
        /// 平均工艺流程数量
        /// </summary>
        public decimal AverageFlowCount { get; set; }

        /// <summary>
        /// 平均总工时
        /// </summary>
        public decimal AverageTotalTime { get; set; }
    }

    /// <summary>
    /// 产品工艺包分布
    /// </summary>
    public class ProductPackageDistribution
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工艺包数量
        /// </summary>
        public int PackageCount { get; set; }

        /// <summary>
        /// 启用的工艺包数量
        /// </summary>
        public int ActivePackageCount { get; set; }
    }
}

// --- END OF FILE ProcessPackage.cs ---
