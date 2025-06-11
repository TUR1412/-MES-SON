using MES.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MES.Models.Material
{
    /// <summary>
    /// 工艺步骤类型枚举
    /// </summary>
    public enum ProcessStepType
    {
        /// <summary>
        /// 加工
        /// </summary>
        [Description("加工")]
        Processing = 1,

        /// <summary>
        /// 检验
        /// </summary>
        [Description("检验")]
        Inspection = 2,

        /// <summary>
        /// 装配
        /// </summary>
        [Description("装配")]
        Assembly = 3,

        /// <summary>
        /// 包装
        /// </summary>
        [Description("包装")]
        Packaging = 4,

        /// <summary>
        /// 测试
        /// </summary>
        [Description("测试")]
        Testing = 5
    }

    /// <summary>
    /// 工艺步骤状态枚举
    /// </summary>
    public enum ProcessStepStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Active = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Inactive = 0
    }

    /// <summary>
    /// 工艺步骤模型
    /// 定义工艺路线中的具体工艺步骤
    /// </summary>
    public class ProcessStep : BaseModel
    {
        /// <summary>
        /// 工艺步骤ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public int ProcessRouteId { get; set; }

        /// <summary>
        /// 步骤序号
        /// </summary>
        public int StepNumber { get; set; }

        /// <summary>
        /// 步骤名称
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 步骤类型
        /// </summary>
        public ProcessStepType StepType { get; set; }

        /// <summary>
        /// 步骤类型文本
        /// </summary>
        public string StepTypeText
        {
            get
            {
                switch (StepType)
                {
                    case ProcessStepType.Processing:
                        return "加工";
                    case ProcessStepType.Inspection:
                        return "检验";
                    case ProcessStepType.Assembly:
                        return "装配";
                    case ProcessStepType.Packaging:
                        return "包装";
                    case ProcessStepType.Testing:
                        return "测试";
                    default:
                        return "未知";
                }
            }
        }

        /// <summary>
        /// 工作站ID
        /// </summary>
        public int WorkstationId { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public string WorkstationName { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public string PortNumber { get; set; }

        /// <summary>
        /// 标准工时（分钟）
        /// </summary>
        public decimal StandardTime { get; set; }

        /// <summary>
        /// 准备时间（分钟）
        /// </summary>
        public decimal SetupTime { get; set; }

        /// <summary>
        /// 等待时间（分钟）
        /// </summary>
        public decimal WaitTime { get; set; }

        /// <summary>
        /// 步骤描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string OperationInstructions { get; set; }

        /// <summary>
        /// 质量要求
        /// </summary>
        public string QualityRequirements { get; set; }

        /// <summary>
        /// 安全注意事项
        /// </summary>
        public string SafetyNotes { get; set; }

        /// <summary>
        /// 所需技能等级
        /// </summary>
        public int RequiredSkillLevel { get; set; }

        /// <summary>
        /// 是否关键步骤
        /// </summary>
        public bool IsCritical { get; set; }

        /// <summary>
        /// 是否需要检验
        /// </summary>
        public bool RequiresInspection { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ProcessStepStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case ProcessStepStatus.Active:
                        return "启用";
                    case ProcessStepStatus.Inactive:
                        return "停用";
                    default:
                        return "未知";
                }
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessStep()
        {
            Status = ProcessStepStatus.Active;
            StepType = ProcessStepType.Processing;
            RequiredSkillLevel = 1;
            IsCritical = false;
            RequiresInspection = false;
        }

        /// <summary>
        /// 总时间（标准工时 + 准备时间 + 等待时间）
        /// </summary>
        public decimal TotalTime
        {
            get
            {
                return StandardTime + SetupTime + WaitTime;
            }
        }

        /// <summary>
        /// 验证工艺步骤数据
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (StepNumber <= 0)
            {
                result.AddError("步骤序号必须大于0");
            }

            if (string.IsNullOrWhiteSpace(StepName))
            {
                result.AddError("步骤名称不能为空");
            }

            if (WorkstationId <= 0)
            {
                result.AddError("必须选择工作站");
            }

            if (StandardTime < 0)
            {
                result.AddError("标准工时不能为负数");
            }

            if (SetupTime < 0)
            {
                result.AddError("准备时间不能为负数");
            }

            if (WaitTime < 0)
            {
                result.AddError("等待时间不能为负数");
            }

            if (RequiredSkillLevel < 1 || RequiredSkillLevel > 10)
            {
                result.AddError("技能等级必须在1-10之间");
            }

            return result;
        }

        /// <summary>
        /// 克隆工艺步骤（用于复制功能）
        /// </summary>
        /// <returns>克隆的工艺步骤</returns>
        public ProcessStep Clone()
        {
            return new ProcessStep
            {
                StepNumber = this.StepNumber,
                StepName = this.StepName,
                StepType = this.StepType,
                WorkstationId = this.WorkstationId,
                WorkstationName = this.WorkstationName,
                PortNumber = this.PortNumber,
                StandardTime = this.StandardTime,
                SetupTime = this.SetupTime,
                WaitTime = this.WaitTime,
                Description = this.Description,
                OperationInstructions = this.OperationInstructions,
                QualityRequirements = this.QualityRequirements,
                SafetyNotes = this.SafetyNotes,
                RequiredSkillLevel = this.RequiredSkillLevel,
                IsCritical = this.IsCritical,
                RequiresInspection = this.RequiresInspection,
                Status = this.Status,
                CreateTime = DateTime.Now,
                IsDeleted = false
            };
        }

        /// <summary>
        /// 获取步骤的完整描述
        /// </summary>
        /// <returns>完整描述</returns>
        public string GetFullDescription()
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(Description))
                parts.Add(string.Format("描述：{0}", Description));

            if (!string.IsNullOrWhiteSpace(OperationInstructions))
                parts.Add(string.Format("操作说明：{0}", OperationInstructions));

            if (!string.IsNullOrWhiteSpace(QualityRequirements))
                parts.Add(string.Format("质量要求：{0}", QualityRequirements));

            if (!string.IsNullOrWhiteSpace(SafetyNotes))
                parts.Add(string.Format("安全注意：{0}", SafetyNotes));

            return string.Join("\n", parts);
        }

        /// <summary>
        /// 获取步骤的关键信息摘要
        /// </summary>
        /// <returns>关键信息摘要</returns>
        public string GetSummary()
        {
            return string.Format("步骤{0}：{1} | 工作站：{2} | 标准工时：{3}分钟", StepNumber, StepName, WorkstationName, StandardTime);
        }
    }
}
