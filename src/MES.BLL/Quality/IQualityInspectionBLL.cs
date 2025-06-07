using System;
using System.Collections.Generic;
using MES.Models.Quality;

namespace MES.BLL.Quality
{
    /// <summary>
    /// 质量检验业务逻辑接口
    /// 定义质量检验管理的核心业务操作
    /// </summary>
    public interface IQualityInspectionBLL
    {
        /// <summary>
        /// 添加质量检验记录
        /// </summary>
        /// <param name="inspection">检验信息</param>
        /// <returns>操作是否成功</returns>
        bool AddInspection(QualityInspectionInfo inspection);

        /// <summary>
        /// 根据ID删除检验记录（逻辑删除）
        /// </summary>
        /// <param name="id">检验记录ID</param>
        /// <returns>操作是否成功</returns>
        bool DeleteInspection(int id);

        /// <summary>
        /// 更新检验记录信息
        /// </summary>
        /// <param name="inspection">检验信息</param>
        /// <returns>操作是否成功</returns>
        bool UpdateInspection(QualityInspectionInfo inspection);

        /// <summary>
        /// 根据ID获取检验记录信息
        /// </summary>
        /// <param name="id">检验记录ID</param>
        /// <returns>检验信息，未找到返回null</returns>
        QualityInspectionInfo GetInspectionById(int id);

        /// <summary>
        /// 根据检验单号获取检验记录信息
        /// </summary>
        /// <param name="inspectionNumber">检验单号</param>
        /// <returns>检验信息，未找到返回null</returns>
        QualityInspectionInfo GetInspectionByNumber(string inspectionNumber);

        /// <summary>
        /// 获取所有检验记录列表
        /// </summary>
        /// <returns>检验记录列表</returns>
        List<QualityInspectionInfo> GetAllInspections();

        /// <summary>
        /// 根据生产订单ID获取检验记录列表
        /// </summary>
        /// <param name="productionOrderId">生产订单ID</param>
        /// <returns>检验记录列表</returns>
        List<QualityInspectionInfo> GetInspectionsByProductionOrder(int productionOrderId);

        /// <summary>
        /// 根据检验类型获取检验记录列表
        /// </summary>
        /// <param name="inspectionType">检验类型</param>
        /// <returns>检验记录列表</returns>
        List<QualityInspectionInfo> GetInspectionsByType(int inspectionType);

        /// <summary>
        /// 根据检验结果获取检验记录列表
        /// </summary>
        /// <param name="inspectionResult">检验结果</param>
        /// <returns>检验记录列表</returns>
        List<QualityInspectionInfo> GetInspectionsByResult(int inspectionResult);

        /// <summary>
        /// 根据审核状态获取检验记录列表
        /// </summary>
        /// <param name="reviewStatus">审核状态</param>
        /// <returns>检验记录列表</returns>
        List<QualityInspectionInfo> GetInspectionsByReviewStatus(int reviewStatus);

        /// <summary>
        /// 分页获取检验记录列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的检验记录列表</returns>
        List<QualityInspectionInfo> GetInspectionsByPage(int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 根据条件搜索检验记录
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <returns>匹配的检验记录列表</returns>
        List<QualityInspectionInfo> SearchInspections(string keyword);

        /// <summary>
        /// 提交检验记录进行审核
        /// </summary>
        /// <param name="id">检验记录ID</param>
        /// <returns>操作是否成功</returns>
        bool SubmitForReview(int id);

        /// <summary>
        /// 审核检验记录
        /// </summary>
        /// <param name="id">检验记录ID</param>
        /// <param name="reviewerId">审核员ID</param>
        /// <param name="reviewerName">审核员姓名</param>
        /// <param name="reviewResult">审核结果：2-通过，3-不通过</param>
        /// <param name="reviewComments">审核意见</param>
        /// <returns>操作是否成功</returns>
        bool ReviewInspection(int id, int reviewerId, string reviewerName, int reviewResult, string reviewComments);

        /// <summary>
        /// 批量审核检验记录
        /// </summary>
        /// <param name="ids">检验记录ID列表</param>
        /// <param name="reviewerId">审核员ID</param>
        /// <param name="reviewerName">审核员姓名</param>
        /// <param name="reviewResult">审核结果</param>
        /// <param name="reviewComments">审核意见</param>
        /// <returns>操作是否成功</returns>
        bool BatchReviewInspections(List<int> ids, int reviewerId, string reviewerName, int reviewResult, string reviewComments);

        /// <summary>
        /// 生成检验单号
        /// </summary>
        /// <param name="inspectionType">检验类型</param>
        /// <returns>检验单号</returns>
        string GenerateInspectionNumber(int inspectionType);

        /// <summary>
        /// 计算合格率
        /// </summary>
        /// <param name="inspections">检验记录列表</param>
        /// <returns>合格率（百分比）</returns>
        decimal CalculateQualifiedRate(List<QualityInspectionInfo> inspections);

        /// <summary>
        /// 获取质量统计数据
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>质量统计数据</returns>
        Dictionary<string, object> GetQualityStatistics(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 获取检验类型统计
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>检验类型统计</returns>
        Dictionary<string, int> GetInspectionTypeStatistics(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 获取产品质量趋势数据
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>质量趋势数据</returns>
        List<Dictionary<string, object>> GetQualityTrendData(string productCode, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 获取不合格原因统计
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>不合格原因统计</returns>
        Dictionary<string, int> GetUnqualifiedReasonStatistics(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 验证检验数据
        /// </summary>
        /// <param name="inspection">检验信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        string ValidateInspection(QualityInspectionInfo inspection);

        /// <summary>
        /// 检查检验单号是否已存在
        /// </summary>
        /// <param name="inspectionNumber">检验单号</param>
        /// <param name="excludeId">排除的检验记录ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        bool IsInspectionNumberExists(string inspectionNumber, int excludeId = 0);

        /// <summary>
        /// 导出检验记录
        /// </summary>
        /// <param name="inspections">检验记录列表</param>
        /// <param name="filePath">导出文件路径</param>
        /// <returns>操作是否成功</returns>
        bool ExportInspections(List<QualityInspectionInfo> inspections, string filePath);

        /// <summary>
        /// 生成质量报告
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="reportType">报告类型</param>
        /// <returns>报告数据</returns>
        Dictionary<string, object> GenerateQualityReport(DateTime startDate, DateTime endDate, string reportType);

        /// <summary>
        /// 获取待审核检验记录数量
        /// </summary>
        /// <returns>待审核记录数量</returns>
        int GetPendingReviewCount();

        /// <summary>
        /// 获取今日检验统计
        /// </summary>
        /// <returns>今日检验统计</returns>
        Dictionary<string, object> GetTodayInspectionStatistics();
    }
}
