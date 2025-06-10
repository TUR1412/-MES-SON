using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MES.DAL.Workshop;
using MES.Models.Workshop;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.Workshop
{
    /// <summary>
    /// 批次管理业务逻辑实现类
    /// 实现批次管理的核心业务逻辑
    /// </summary>
    public class BatchBLL : IBatchBLL
    {
        private readonly BatchDAL _batchDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchBLL()
        {
            _batchDAL = new BatchDAL();
        }

        /// <summary>
        /// 创建批次（为UI层提供的简化接口）
        /// </summary>
        /// <param name="batch">批次模型</param>
        /// <returns>操作是否成功</returns>
        public bool CreateBatch(BatchModel batch)
        {
            try
            {
                // 转换为BatchInfo
                var batchInfo = new BatchInfo
                {
                    BatchId = batch.BatchNo,
                    WorkOrderId = batch.WorkOrderNo,
                    ProductMaterialId = batch.ProductCode,
                    Quantity = (int)batch.BatchQuantity,
                    Status = 1, // 创建状态
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    IsDeleted = false
                };

                return AddBatch(batchInfo);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("创建批次异常：{0}", ex.Message), ex);
                throw new MESException("创建批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 添加批次信息
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>操作是否成功</returns>
        public bool AddBatch(BatchInfo batch)
        {
            try
            {
                // 验证输入参数
                if (batch == null)
                {
                    LogManager.Error("添加批次失败：批次信息不能为空");
                    return false;
                }

                // 业务规则验证
                string validationResult = ValidateBatch(batch);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error(string.Format("添加批次失败：{0}", validationResult));
                    return false;
                }

                // 检查批次编号是否已存在
                if (IsBatchIdExists(batch.BatchId))
                {
                    LogManager.Error(string.Format("添加批次失败：批次编号 {0} 已存在", batch.BatchId));
                    return false;
                }

                // 设置默认值
                batch.CreateTime = DateTime.Now;
                batch.UpdateTime = DateTime.Now;
                batch.IsDeleted = false;

                // 如果未设置状态，默认为创建状态
                if (batch.Status == 0)
                {
                    batch.Status = 1; // 创建状态
                }

                // 调用DAL层添加
                bool result = _batchDAL.Add(batch);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功添加批次：{0}", batch.BatchId));
                }
                else
                {
                    LogManager.Error(string.Format("添加批次失败：{0}", batch.BatchId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加批次异常：{0}", ex.Message), ex);
                throw new MESException("添加批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID删除批次信息（逻辑删除）
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <returns>操作是否成功</returns>
        public bool DeleteBatch(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("删除批次失败：ID无效");
                    return false;
                }

                // 检查批次是否存在
                var existingBatch = _batchDAL.GetById(id);
                if (existingBatch == null)
                {
                    LogManager.Error(string.Format("删除批次失败：ID为 {0} 的批次不存在", id));
                    return false;
                }

                // 检查批次是否可以删除（如果正在生产中则不能删除）
                if (existingBatch.Status == 3) // 生产中状态
                {
                    LogManager.Error(string.Format("删除批次失败：批次 {0} 正在生产中，不能删除", existingBatch.BatchId));
                    return false;
                }
                
                bool result = _batchDAL.Delete(id);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功删除批次：ID={0}, 批次编号={1}", id, existingBatch.BatchId));
                }
                else
                {
                    LogManager.Error(string.Format("删除批次失败：ID={0}", id));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除批次异常：{0}", ex.Message), ex);
                throw new MESException("删除批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新批次信息
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>操作是否成功</returns>
        public bool UpdateBatch(BatchInfo batch)
        {
            try
            {
                if (batch == null || batch.Id <= 0)
                {
                    LogManager.Error("更新批次失败：批次信息无效");
                    return false;
                }

                // 验证业务规则
                string validationResult = ValidateBatch(batch);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error(string.Format("更新批次失败：{0}", validationResult));
                    return false;
                }

                // 检查批次是否存在
                var existingBatch = _batchDAL.GetById(batch.Id);
                if (existingBatch == null)
                {
                    LogManager.Error(string.Format("更新批次失败：ID为 {0} 的批次不存在", batch.Id));
                    return false;
                }

                // 检查批次编号是否与其他批次冲突
                if (IsBatchIdExists(batch.BatchId, batch.Id))
                {
                    LogManager.Error(string.Format("更新批次失败：批次编号 {0} 已被其他批次使用", batch.BatchId));
                    return false;
                }

                // 更新时间
                batch.UpdateTime = DateTime.Now;

                bool result = _batchDAL.Update(batch);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功更新批次：{0}", batch.BatchId));
                }
                else
                {
                    LogManager.Error(string.Format("更新批次失败：{0}", batch.BatchId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新批次异常：{0}", ex.Message), ex);
                throw new MESException("更新批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID获取批次信息
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <returns>批次信息，未找到返回null</returns>
        public BatchInfo GetBatchById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("获取批次失败：ID无效");
                    return null;
                }

                return _batchDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取批次异常：{0}", ex.Message), ex);
                throw new MESException("获取批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据批次编号获取批次信息
        /// </summary>
        /// <param name="batchId">批次编号</param>
        /// <returns>批次信息，未找到返回null</returns>
        public BatchInfo GetBatchByBatchId(string batchId)
        {
            try
            {
                if (string.IsNullOrEmpty(batchId))
                {
                    LogManager.Error("获取批次失败：批次编号不能为空");
                    return null;
                }

                return _batchDAL.GetByBatchId(batchId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据批次编号获取批次异常：{0}", ex.Message), ex);
                throw new MESException("根据批次编号获取批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取所有批次列表
        /// </summary>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetAllBatches()
        {
            try
            {
                return _batchDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取所有批次异常：{0}", ex.Message), ex);
                throw new MESException("获取所有批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据工单ID获取批次列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetBatchesByWorkOrderId(string workOrderId)
        {
            try
            {
                if (string.IsNullOrEmpty(workOrderId))
                {
                    LogManager.Error("根据工单ID获取批次失败：工单ID不能为空");
                    return new List<BatchInfo>();
                }

                return _batchDAL.GetByWorkOrderId(workOrderId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工单ID获取批次异常：{0}", ex.Message), ex);
                throw new MESException("根据工单ID获取批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据状态获取批次列表
        /// </summary>
        /// <param name="status">批次状态</param>
        /// <returns>指定状态的批次列表</returns>
        public List<BatchInfo> GetBatchesByStatus(int status)
        {
            try
            {
                return _batchDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取批次异常：{0}", ex.Message), ex);
                throw new MESException("根据状态获取批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取可取消的批次列表（状态为待产或生产中的批次）
        /// </summary>
        /// <returns>可取消的批次数据表</returns>
        public DataTable GetCancellableBatches()
        {
            try
            {
                // 获取状态为待产(2)或生产中(3)的批次
                var cancellableBatches = new List<BatchInfo>();
                var waitingBatches = GetBatchesByStatus(2); // 待产状态
                var productionBatches = GetBatchesByStatus(3); // 生产中状态

                cancellableBatches.AddRange(waitingBatches);
                cancellableBatches.AddRange(productionBatches);

                // 转换为DataTable格式
                var dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("BatchNo", typeof(string));
                dataTable.Columns.Add("WorkOrderNo", typeof(string));
                dataTable.Columns.Add("ProductCode", typeof(string));
                dataTable.Columns.Add("BatchQuantity", typeof(decimal));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("CreatedBy", typeof(string));
                dataTable.Columns.Add("CreatedDate", typeof(DateTime));

                foreach (var batch in cancellableBatches)
                {
                    var row = dataTable.NewRow();
                    row["Id"] = batch.Id;
                    row["BatchNo"] = batch.BatchId;
                    row["WorkOrderNo"] = batch.WorkOrderId;
                    row["ProductCode"] = batch.ProductMaterialId;
                    row["BatchQuantity"] = batch.Quantity;
                    row["Status"] = GetStatusText(batch.Status);
                    row["CreatedBy"] = batch.CreateUserName ?? "系统";
                    row["CreatedDate"] = batch.CreateTime;
                    dataTable.Rows.Add(row);
                }

                LogManager.Info(string.Format("获取可取消批次列表完成，共 {0} 条记录", dataTable.Rows.Count));
                return dataTable;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取可取消批次列表异常：{0}", ex.Message), ex);
                throw new MESException("获取可取消批次列表时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取状态文本描述
        /// </summary>
        /// <param name="status">状态码</param>
        /// <returns>状态文本</returns>
        private string GetStatusText(int status)
        {
            switch (status)
            {
                case 1: return "已创建";
                case 2: return "待产";
                case 3: return "生产中";
                case 4: return "暂停";
                case 5: return "已完成";
                case 6: return "已取消";
                default: return "未知状态";
            }
        }

        /// <summary>
        /// 根据当前工站获取批次列表
        /// </summary>
        /// <param name="stationId">工站ID</param>
        /// <returns>批次列表</returns>
        public List<BatchInfo> GetBatchesByCurrentStation(string stationId)
        {
            try
            {
                if (string.IsNullOrEmpty(stationId))
                {
                    LogManager.Error("根据工站获取批次失败：工站ID不能为空");
                    return new List<BatchInfo>();
                }

                return _batchDAL.GetByCurrentStation(stationId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据工站获取批次异常：{0}", ex.Message), ex);
                throw new MESException("根据工站获取批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 分页获取批次列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的批次列表</returns>
        public List<BatchInfo> GetBatchesByPage(int pageIndex, int pageSize, out int totalCount)
        {
            try
            {
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    LogManager.Error("分页获取批次失败：页码和每页记录数必须大于0");
                    totalCount = 0;
                    return new List<BatchInfo>();
                }

                // 简化实现：从所有批次中分页
                var allBatches = GetAllBatches();
                totalCount = allBatches.Count;
                return allBatches.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("分页获取批次异常：{0}", ex.Message), ex);
                totalCount = 0;
                throw new MESException("分页获取批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据条件搜索批次
        /// </summary>
        /// <param name="keyword">搜索关键词（批次编号、工单ID等）</param>
        /// <returns>匹配的批次列表</returns>
        public List<BatchInfo> SearchBatches(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return GetAllBatches();
                }

                // 简化实现：从所有批次中搜索
                var allBatches = GetAllBatches();
                return allBatches.Where(b => b.BatchNumber.Contains(keyword) || b.ProductCode.Contains(keyword)).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索批次异常：{0}", ex.Message), ex);
                throw new MESException("搜索批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据多条件搜索批次
        /// </summary>
        /// <param name="keyword">关键词（批次号或产品名称）</param>
        /// <param name="status">状态</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>匹配的批次列表</returns>
        public List<BatchInfo> SearchBatches(string keyword = null, string status = null,
            DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                // 获取所有批次
                var allBatches = GetAllBatches();

                // 应用筛选条件
                var filteredBatches = allBatches.AsEnumerable();

                // 关键词筛选
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    filteredBatches = filteredBatches.Where(b =>
                        (b.BatchNumber != null && b.BatchNumber.Contains(keyword)) ||
                        (b.ProductName != null && b.ProductName.Contains(keyword)) ||
                        (b.ProductCode != null && b.ProductCode.Contains(keyword)) ||
                        (b.ProductionOrderNumber != null && b.ProductionOrderNumber.Contains(keyword)));
                }

                // 状态筛选
                if (!string.IsNullOrWhiteSpace(status) && status != "全部状态")
                {
                    filteredBatches = filteredBatches.Where(b => b.BatchStatus == status);
                }

                // 日期筛选
                if (startDate.HasValue)
                {
                    filteredBatches = filteredBatches.Where(b => b.CreateTime >= startDate.Value.Date);
                }

                if (endDate.HasValue)
                {
                    filteredBatches = filteredBatches.Where(b => b.CreateTime <= endDate.Value.Date.AddDays(1));
                }

                var result = filteredBatches.ToList();
                LogManager.Info(string.Format("搜索批次完成：关键词={0}, 状态={1}, 开始日期={2}, 结束日期={3}, 结果数量={4}",
                    keyword, status, startDate, endDate, result.Count));

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索批次异常：{0}", ex.Message), ex);
                throw new MESException("搜索批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取指定日期的最大序号
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>最大序号</returns>
        public int GetMaxSequenceForDate(DateTime date)
        {
            try
            {
                string dateStr = date.ToString("yyyyMMdd");
                string prefix = "BATCH" + dateStr;

                var allBatches = GetAllBatches();
                var todayBatches = allBatches.Where(b => b.BatchId.StartsWith(prefix)).ToList();

                if (todayBatches.Count == 0)
                {
                    return 0;
                }

                int maxSeq = 0;
                foreach (var batch in todayBatches)
                {
                    // 提取序号部分（最后4位）
                    if (batch.BatchId.Length >= prefix.Length + 4)
                    {
                        string seqStr = batch.BatchId.Substring(prefix.Length);
                        int seq;
                        if (int.TryParse(seqStr, out seq))
                        {
                            if (seq > maxSeq)
                            {
                                maxSeq = seq;
                            }
                        }
                    }
                }

                return maxSeq;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取日期最大序号异常：{0}", ex.Message), ex);
                return 0;
            }
        }

        /// <summary>
        /// 验证批次数据
        /// </summary>
        /// <param name="batch">批次信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        public string ValidateBatch(BatchInfo batch)
        {
            if (batch == null)
            {
                return "批次信息不能为空";
            }

            if (string.IsNullOrEmpty(batch.BatchId))
            {
                return "批次编号不能为空";
            }

            if (string.IsNullOrEmpty(batch.WorkOrderId))
            {
                return "工单ID不能为空";
            }

            if (string.IsNullOrEmpty(batch.ProductMaterialId))
            {
                return "产品物料ID不能为空";
            }

            if (batch.Quantity <= 0)
            {
                return "批次数量必须大于0";
            }

            return string.Empty;
        }

        /// <summary>
        /// 检查批次编号是否已存在
        /// </summary>
        /// <param name="batchId">批次编号</param>
        /// <param name="excludeId">排除的批次ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        public bool IsBatchIdExists(string batchId, int excludeId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(batchId))
                {
                    return false;
                }

                var existingBatch = _batchDAL.GetByBatchId(batchId);
                if (existingBatch == null)
                {
                    return false;
                }

                return excludeId == 0 || existingBatch.Id != excludeId;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("检查批次编号是否存在异常：{0}", ex.Message), ex);
                return false;
            }
        }

        /// <summary>
        /// 开始生产批次
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="stationId">开始生产的工站ID</param>
        /// <returns>操作是否成功</returns>
        public bool StartProduction(int id, string stationId)
        {
            try
            {
                var batch = _batchDAL.GetById(id);
                if (batch == null)
                {
                    LogManager.Error(string.Format("开始生产失败：ID为 {0} 的批次不存在", id));
                    return false;
                }

                if (batch.Status != 2) // 待产状态
                {
                    LogManager.Error(string.Format("开始生产失败：批次 {0} 状态不是待产状态", batch.BatchId));
                    return false;
                }

                batch.Status = 3; // 生产中状态
                batch.CurrentStationId = stationId;
                batch.ProductionStartTime = DateTime.Now;
                batch.UpdateTime = DateTime.Now;

                bool result = _batchDAL.Update(batch);

                if (result)
                {
                    LogManager.Info(string.Format("成功开始生产批次：{0}，工站：{1}", batch.BatchId, stationId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("开始生产批次异常：{0}", ex.Message), ex);
                throw new MESException("开始生产批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 完成生产批次
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <returns>操作是否成功</returns>
        public bool CompleteProduction(int id)
        {
            try
            {
                var batch = _batchDAL.GetById(id);
                if (batch == null)
                {
                    LogManager.Error(string.Format("完成生产失败：ID为 {0} 的批次不存在", id));
                    return false;
                }

                if (batch.Status != 3) // 生产中状态
                {
                    LogManager.Error(string.Format("完成生产失败：批次 {0} 状态不是生产中状态", batch.BatchId));
                    return false;
                }

                batch.Status = 5; // 完成状态
                batch.ProductionEndTime = DateTime.Now;
                batch.UpdateTime = DateTime.Now;

                bool result = _batchDAL.Update(batch);

                if (result)
                {
                    LogManager.Info(string.Format("成功完成生产批次：{0}", batch.BatchId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("完成生产批次异常：{0}", ex.Message), ex);
                throw new MESException("完成生产批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 取消批次
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="reason">取消原因</param>
        /// <returns>操作是否成功</returns>
        public bool CancelBatch(int id, string reason)
        {
            try
            {
                var batch = _batchDAL.GetById(id);
                if (batch == null)
                {
                    LogManager.Error(string.Format("取消批次失败：ID为 {0} 的批次不存在", id));
                    return false;
                }

                if (batch.Status == 5 || batch.Status == 6) // 已完成或已取消
                {
                    LogManager.Error(string.Format("取消批次失败：批次 {0} 已经完成或取消", batch.BatchId));
                    return false;
                }

                batch.Status = 6; // 取消状态
                batch.UpdateTime = DateTime.Now;

                bool result = _batchDAL.Update(batch);

                if (result)
                {
                    LogManager.Info(string.Format("成功取消批次：{0}，原因：{1}", batch.BatchId, reason));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("取消批次异常：{0}", ex.Message), ex);
                throw new MESException("取消批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据批次号取消批次
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="reason">取消原因</param>
        /// <returns>操作是否成功</returns>
        public bool CancelBatch(string batchNo, string reason)
        {
            try
            {
                if (string.IsNullOrEmpty(batchNo))
                {
                    LogManager.Error("取消批次失败：批次号不能为空");
                    return false;
                }

                var batch = _batchDAL.GetByBatchId(batchNo);
                if (batch == null)
                {
                    LogManager.Error(string.Format("取消批次失败：批次号为 {0} 的批次不存在", batchNo));
                    return false;
                }

                return CancelBatch(batch.Id, reason);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据批次号取消批次异常：{0}", ex.Message), ex);
                throw new MESException("根据批次号取消批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 转移批次到指定工站
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="targetStationId">目标工站ID</param>
        /// <returns>操作是否成功</returns>
        public bool TransferBatch(int id, string targetStationId)
        {
            try
            {
                var batch = _batchDAL.GetById(id);
                if (batch == null)
                {
                    LogManager.Error(string.Format("转移批次失败：ID为 {0} 的批次不存在", id));
                    return false;
                }

                if (batch.Status != 3) // 生产中状态
                {
                    LogManager.Error(string.Format("转移批次失败：批次 {0} 状态不是生产中状态", batch.BatchId));
                    return false;
                }

                string oldStationId = batch.CurrentStationId;
                batch.CurrentStationId = targetStationId;
                batch.UpdateTime = DateTime.Now;

                bool result = _batchDAL.Update(batch);

                if (result)
                {
                    LogManager.Info(string.Format("成功转移批次：{0}，从工站 {1} 转移到 {2}", batch.BatchId, oldStationId, targetStationId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("转移批次异常：{0}", ex.Message), ex);
                throw new MESException("转移批次时发生异常", ex);
            }
        }

        /// <summary>
        /// 设置批次载具
        /// </summary>
        /// <param name="id">批次ID</param>
        /// <param name="carrierId">载具ID</param>
        /// <returns>操作是否成功</returns>
        public bool SetBatchCarrier(int id, string carrierId)
        {
            try
            {
                var batch = _batchDAL.GetById(id);
                if (batch == null)
                {
                    LogManager.Error(string.Format("设置批次载具失败：ID为 {0} 的批次不存在", id));
                    return false;
                }

                batch.CarrierId = carrierId;
                batch.UpdateTime = DateTime.Now;

                bool result = _batchDAL.Update(batch);

                if (result)
                {
                    LogManager.Info(string.Format("成功设置批次 {0} 的载具：{1}", batch.BatchId, carrierId));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设置批次载具异常：{0}", ex.Message), ex);
                throw new MESException("设置批次载具时发生异常", ex);
            }
        }



        /// <summary>
        /// 获取批次统计信息
        /// </summary>
        /// <param name="batchId">批次ID</param>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, object> GetBatchStatistics(int batchId)
        {
            try
            {
                var batch = _batchDAL.GetById(batchId);
                if (batch == null)
                {
                    LogManager.Error(string.Format("获取批次统计信息失败：ID为 {0} 的批次不存在", batchId));
                    return new Dictionary<string, object>();
                }

                var statistics = new Dictionary<string, object>();
                statistics.Add("BatchId", batch.BatchId);
                statistics.Add("WorkOrderId", batch.WorkOrderId);
                statistics.Add("ProductMaterialId", batch.ProductMaterialId);
                statistics.Add("Quantity", batch.Quantity);
                statistics.Add("Status", batch.Status);
                statistics.Add("CurrentStationId", batch.CurrentStationId);
                statistics.Add("ProductionStartTime", batch.ProductionStartTime);
                statistics.Add("ProductionEndTime", batch.ProductionEndTime);
                statistics.Add("CarrierId", batch.CarrierId);
                statistics.Add("CreateTime", batch.CreateTime);
                statistics.Add("UpdateTime", batch.UpdateTime);

                // 计算生产时长
                if (batch.ProductionStartTime.HasValue)
                {
                    DateTime endTime = batch.ProductionEndTime.HasValue ? batch.ProductionEndTime.Value : DateTime.Now;
                    var duration = endTime - batch.ProductionStartTime.Value;
                    statistics.Add("ProductionDuration", duration.TotalHours);
                }

                return statistics;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取批次统计信息异常：{0}", ex.Message), ex);
                throw new MESException("获取批次统计信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 生成新的批次号（基于数据库序号）
        /// </summary>
        /// <returns>新的批次号</returns>
        public string GenerateNewBatchNumber()
        {
            try
            {
                // 获取今天的日期
                string dateStr = DateTime.Now.ToString("yyyyMMdd");

                // 从数据库获取今天已有的最大批次序号
                int maxSequence = _batchDAL.GetMaxBatchSequenceByDate(DateTime.Now.Date);

                // 生成新的序号
                int newSequence = maxSequence + 1;

                // 格式：BATCH + 日期 + 3位序号
                return string.Format("BATCH{0}{1:000}", dateStr, newSequence);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("生成批次号异常：{0}", ex.Message), ex);
                throw new MESException("生成批次号时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取批次状态文本
        /// </summary>
        /// <param name="status">状态值</param>
        /// <returns>状态文本</returns>
        private string GetBatchStatusText(int status)
        {
            switch (status)
            {
                case 1: return "待开始";
                case 2: return "进行中";
                case 3: return "已暂停";
                case 4: return "已完成";
                case 5: return "已取消";
                default: return "未知状态";
            }
        }
    }

    /// <summary>
    /// 批次模型（临时定义，应该在Models项目中）
    /// </summary>
    public class BatchModel
    {
        public string BatchNo { get; set; }
        public string WorkOrderNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal BatchQuantity { get; set; }
        public decimal CompletedQuantity { get; set; }
        public string Status { get; set; }
        public string ResponsiblePerson { get; set; }
        public DateTime PlanCompletionDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
