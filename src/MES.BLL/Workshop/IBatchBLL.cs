using System;
using System.Collections.Generic;
using MES.Models.Workshop;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 批次管理业务逻辑接口
    /// 定义批次管理的核心业务操作
    /// </summary>
    public interface IBatchBLL
    {
        /// <summary>
        /// 添加批次信息
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>操作是否成功</returns>
        bool AddBatch(BatchInfo batch);

        /// <summary>
        /// 根据ID删除批次信息（逻辑删除）
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <returns>操作是否成功</returns>
        bool DeleteBatch(int id);

        /// <summary>
        /// 更新批次信息
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>操作是否成功</returns>
        bool UpdateBatch(BatchInfo batch);

        /// <summary>
        /// 根据ID获取批次信息
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <returns>批次信息，未找到返回null</returns>
        BatchInfo GetBatchById(int id);

        /// <summary>
        /// 根据批次编号获取批次信息
        /// </summary>
        /// <param name="batchId">批次编号</param>
        /// <returns>批次信息，未找到返回null</returns>
        BatchInfo GetBatchByBatchId(string batchId);

        /// <summary>
        /// 获取所有批次列表
        /// </summary>
        /// <returns>批次列表</returns>
        List<BatchInfo> GetAllBatches();

        /// <summary>
        /// 根据工单ID获取批次列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>批次列表</returns>
        List<BatchInfo> GetBatchesByWorkOrderId(string workOrderId);

        /// <summary>
        /// 根据状态获取批次列表
        /// </summary>
        /// <param name="status">批次状态</param>
        /// <returns>指定状态的批次列表</returns>
        List<BatchInfo> GetBatchesByStatus(int status);

        /// <summary>
        /// 根据当前工站获取批次列表
        /// </summary>
        /// <param name="stationId">工站ID</param>
        /// <returns>批次列表</returns>
        List<BatchInfo> GetBatchesByCurrentStation(string stationId);

        /// <summary>
        /// 分页获取批次列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的批次列表</returns>
        List<BatchInfo> GetBatchesByPage(int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 根据条件搜索批次
        /// </summary>
        /// <param name="keyword">搜索关键词（批次编号、工单ID等）</param>
        /// <returns>匹配的批次列表</returns>
        List<BatchInfo> SearchBatches(string keyword);

        /// <summary>
        /// 开始生产批次
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="stationId">开始生产的工站ID</param>
        /// <returns>操作是否成功</returns>
        bool StartProduction(int id, string stationId);

        /// <summary>
        /// 完成生产批次
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <returns>操作是否成功</returns>
        bool CompleteProduction(int id);

        /// <summary>
        /// 取消批次
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="reason">取消原因</param>
        /// <returns>操作是否成功</returns>
        bool CancelBatch(int id, string reason);

        /// <summary>
        /// 转移批次到指定工站
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="targetStationId">目标工站ID</param>
        /// <returns>操作是否成功</returns>
        bool TransferBatch(int id, string targetStationId);

        /// <summary>
        /// 设置批次载具
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="carrierId">载具ID</param>
        /// <returns>操作是否成功</returns>
        bool SetBatchCarrier(int id, string carrierId);

        /// <summary>
        /// 验证批次数据
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        string ValidateBatch(BatchInfo batch);

        /// <summary>
        /// 检查批次编号是否已存在
        /// </summary>
        /// <param name="batchId">批次编号</param>
        /// <param name="excludeId">排除的批次ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        bool IsBatchIdExists(string batchId, int excludeId = 0);

        /// <summary>
        /// 获取批次统计信息
        /// </summary>
        /// <param name="batchId">批次ID</param>
        /// <returns>统计信息字典</returns>
        Dictionary<string, object> GetBatchStatistics(int batchId);
    }
}
