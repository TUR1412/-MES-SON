// --- START OF FILE ProcessFlow.cs ---

using MES.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MES.Models.Material
{
    /// <summary>
    /// 工艺流程状态枚举
    /// </summary>
    public enum ProcessFlowStatus
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
    /// 工艺流程模型
    /// 定义工艺包中的具体流程，包含多个工艺路线
    /// 严格遵循C# 5.0语法规范
    /// </summary>
    public class ProcessFlow : BaseModel
    {
        /// <summary>
        /// 工艺流程编码
        /// </summary>
        public string FlowCode { get; set; }

        /// <summary>
        /// 工艺流程名称
        /// </summary>
        public string FlowName { get; set; }

        /// <summary>
        /// 工艺包ID
        /// </summary>
        public int ProcessPackageId { get; set; }

        /// <summary>
        /// 工艺包名称 (从关联查询获取)
        /// </summary>
        public string ProcessPackageName { get; set; }

        /// <summary>
        /// 流程序号
        /// </summary>
        public int FlowNumber { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public new string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ProcessFlowStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case ProcessFlowStatus.Draft: return "草稿";
                    case ProcessFlowStatus.Active: return "启用";
                    case ProcessFlowStatus.Inactive: return "停用";
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
        /// 工艺路线列表
        /// </summary>
        public List<ProcessRoute> Routes { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessFlow()
        {
            Routes = new List<ProcessRoute>();
            Status = ProcessFlowStatus.Draft;
            Version = "V1.0";
            FlowNumber = 1;
        }

        /// <summary>
        /// 工艺路线数量
        /// </summary>
        public int RouteCount
        {
            get
            {
                return Routes != null ? Routes.Count : 0;
            }
        }

        /// <summary>
        /// 总工艺步骤数量
        /// </summary>
        public int TotalStepCount
        {
            get
            {
                if (Routes == null) return 0;
                int count = 0;
                foreach (var route in Routes)
                {
                    count += route.StepCount;
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
                if (Routes == null) return 0;
                decimal total = 0;
                foreach (var route in Routes)
                {
                    total += route.TotalStandardTime;
                }
                return total;
            }
        }

        /// <summary>
        /// 验证工艺流程数据
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(FlowCode))
            {
                result.AddError("工艺流程编码不能为空");
            }

            if (string.IsNullOrWhiteSpace(FlowName))
            {
                result.AddError("工艺流程名称不能为空");
            }

            if (ProcessPackageId <= 0)
            {
                result.AddError("必须关联一个工艺包");
            }

            if (FlowNumber <= 0)
            {
                result.AddError("流程序号必须大于0");
            }

            if (string.IsNullOrWhiteSpace(Version))
            {
                result.AddError("版本号不能为空");
            }

            return result;
        }

        /// <summary>
        /// 克隆工艺流程（用于复制功能）
        /// 严格遵循C# 5.0语法规范
        /// </summary>
        /// <returns>克隆的工艺流程</returns>
        public ProcessFlow Clone()
        {
            var cloned = new ProcessFlow
            {
                FlowCode = this.FlowCode,
                FlowName = this.FlowName,
                ProcessPackageId = this.ProcessPackageId,
                ProcessPackageName = this.ProcessPackageName,
                FlowNumber = this.FlowNumber,
                Version = this.Version,
                Status = ProcessFlowStatus.Draft, // 复制的流程默认为草稿状态
                Description = this.Description,
                CreateTime = DateTime.Now,
                IsDeleted = false
            };

            // 克隆工艺路线列表
            if (this.Routes != null)
            {
                cloned.Routes = new List<ProcessRoute>();
                foreach (var route in this.Routes)
                {
                    var clonedRoute = route.Clone();
                    clonedRoute.ProcessFlowId = 0; // 重置关联ID
                    cloned.Routes.Add(clonedRoute);
                }
            }

            return cloned;
        }

        /// <summary>
        /// 获取工艺流程的显示文本
        /// </summary>
        /// <returns>显示文本</returns>
        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", FlowCode, FlowName, StatusText);
        }
    }

    /// <summary>
    /// 工艺流程统计信息
    /// </summary>
    public class ProcessFlowStatistics
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
        /// 平均工艺路线数量
        /// </summary>
        public decimal AverageRouteCount { get; set; }

        /// <summary>
        /// 平均总工时
        /// </summary>
        public decimal AverageTotalTime { get; set; }
    }

    /// <summary>
    /// 工艺包流程分布
    /// </summary>
    public class PackageFlowDistribution
    {
        /// <summary>
        /// 工艺包ID
        /// </summary>
        public int ProcessPackageId { get; set; }

        /// <summary>
        /// 工艺包名称
        /// </summary>
        public string ProcessPackageName { get; set; }

        /// <summary>
        /// 工艺流程数量
        /// </summary>
        public int FlowCount { get; set; }

        /// <summary>
        /// 启用的工艺流程数量
        /// </summary>
        public int ActiveFlowCount { get; set; }
    }
}

// --- END OF FILE ProcessFlow.cs ---
