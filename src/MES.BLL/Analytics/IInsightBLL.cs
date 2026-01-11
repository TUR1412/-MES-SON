// 运营洞察服务接口，聚合核心业务风险与概览指标。
using System;
using MES.Models.Analytics;

namespace MES.BLL.Analytics
{
    /// <summary>
    /// 运营洞察业务逻辑接口
    /// </summary>
    public interface IInsightBLL
    {
        /// <summary>
        /// 获取运营洞察快照
        /// </summary>
        /// <param name="referenceTime">参考时间，默认当前时间</param>
        /// <returns>运营洞察快照</returns>
        OperationalInsightSnapshot GetOperationalSnapshot(DateTime? referenceTime = null);
    }
}

