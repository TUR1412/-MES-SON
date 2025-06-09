using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MES.Models.Material
{
    /// <summary>
    /// 工艺路线状态枚举
    /// </summary>
    public enum ProcessRouteStatus
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
    /// 工艺路线模型
    /// 定义产品的生产工艺流程路线
    /// </summary>
    public class ProcessRoute
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ProcessRouteStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case ProcessRouteStatus.Draft:
                        return "草稿";
                    case ProcessRouteStatus.Active:
                        return "启用";
                    case ProcessRouteStatus.Inactive:
                        return "停用";
                    default:
                        return "未知";
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
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新人ID
        /// </summary>
        public int? UpdateUserId { get; set; }

        /// <summary>
        /// 更新人姓名
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 工艺步骤列表
        /// </summary>
        public List<ProcessStep> Steps { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessRoute()
        {
            Steps = new List<ProcessStep>();
            CreateTime = DateTime.Now;
            IsDeleted = false;
            Status = ProcessRouteStatus.Draft;
            Version = "V1.0";
        }

        /// <summary>
        /// 获取总工时（分钟）
        /// </summary>
        public decimal TotalStandardTime
        {
            get
            {
                decimal total = 0;
                if (Steps != null)
                {
                    foreach (var step in Steps)
                    {
                        total += step.StandardTime;
                    }
                }
                return total;
            }
        }

        /// <summary>
        /// 获取工艺步骤数量
        /// </summary>
        public int StepCount
        {
            get
            {
                return Steps != null ? Steps.Count : 0;
            }
        }

        /// <summary>
        /// 验证工艺路线数据
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(RouteCode))
            {
                result.AddError("工艺路线编码不能为空");
            }

            if (string.IsNullOrWhiteSpace(RouteName))
            {
                result.AddError("工艺路线名称不能为空");
            }

            if (ProductId <= 0)
            {
                result.AddError("必须选择产品");
            }

            if (string.IsNullOrWhiteSpace(Version))
            {
                result.AddError("版本号不能为空");
            }

            if (Steps == null || Steps.Count == 0)
            {
                result.AddError("至少需要一个工艺步骤");
            }
            else
            {
                // 验证步骤序号的连续性
                for (int i = 0; i < Steps.Count; i++)
                {
                    if (Steps[i].StepNumber != i + 1)
                    {
                        result.AddError(string.Format("工艺步骤序号不连续，第{0}个步骤的序号应为{1}", i + 1, i + 1));
                        break;
                    }
                }

                // 验证每个步骤
                foreach (var step in Steps)
                {
                    var stepValidation = step.Validate();
                    if (!stepValidation.IsValid)
                    {
                        result.AddErrors(stepValidation.Errors);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 克隆工艺路线（用于复制功能）
        /// </summary>
        /// <returns>克隆的工艺路线</returns>
        public ProcessRoute Clone()
        {
            var cloned = new ProcessRoute
            {
                RouteCode = this.RouteCode + "_Copy",
                RouteName = this.RouteName + "_副本",
                ProductId = this.ProductId,
                ProductName = this.ProductName,
                Version = "V1.0",
                Status = ProcessRouteStatus.Draft,
                Description = this.Description,
                CreateTime = DateTime.Now,
                IsDeleted = false
            };

            // 克隆工艺步骤
            if (this.Steps != null)
            {
                foreach (var step in this.Steps)
                {
                    cloned.Steps.Add(step.Clone());
                }
            }

            return cloned;
        }
    }

    /// <summary>
    /// 验证结果类
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// 错误信息列表
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsValid
        {
            get { return Errors == null || Errors.Count == 0; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ValidationResult()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// 添加错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public void AddError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                Errors.Add(error);
            }
        }

        /// <summary>
        /// 添加多个错误信息
        /// </summary>
        /// <param name="errors">错误信息列表</param>
        public void AddErrors(List<string> errors)
        {
            if (errors != null)
            {
                Errors.AddRange(errors);
            }
        }

        /// <summary>
        /// 获取所有错误信息的字符串表示
        /// </summary>
        /// <returns>错误信息字符串</returns>
        public string GetErrorsString()
        {
            return string.Join("\n", Errors);
        }
    }
}
