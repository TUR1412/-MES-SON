using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Models.Quality;
using MES.Common.Logging;

namespace MES.DAL.Quality
{
    /// <summary>
    /// 质量检验信息数据访问类
    /// 提供质量检验管理的数据库操作功能
    /// </summary>
    public class QualityInspectionDAL : BaseDAL<QualityInspectionInfo>
    {
        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName
        {
            get { return "quality_inspection"; }
        }

        /// <summary>
        /// 主键字段名
        /// </summary>
        protected override string PrimaryKey
        {
            get { return "id"; }
        }

        /// <summary>
        /// 将DataRow转换为QualityInspectionInfo对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>质量检验信息对象</returns>
        protected override QualityInspectionInfo MapRowToEntity(DataRow row)
        {
            return new QualityInspectionInfo
            {
                Id = Convert.ToInt32(row["id"]),
                InspectionNumber = row["inspection_number"] != DBNull.Value ? row["inspection_number"].ToString() : string.Empty,
                ProductionOrderId = row["production_order_id"] != DBNull.Value ? Convert.ToInt32(row["production_order_id"]) : 0,
                ProductionOrderNumber = row["production_order_number"] != DBNull.Value ? row["production_order_number"].ToString() : string.Empty,
                ProductCode = row["product_code"] != DBNull.Value ? row["product_code"].ToString() : string.Empty,
                ProductName = row["product_name"] != DBNull.Value ? row["product_name"].ToString() : string.Empty,
                InspectionType = Convert.ToInt32(row["inspection_type"]),
                InspectionStage = row["inspection_stage"] != DBNull.Value ? row["inspection_stage"].ToString() : string.Empty,
                InspectionQuantity = row["inspection_quantity"] != DBNull.Value ? Convert.ToDecimal(row["inspection_quantity"]) : 0,
                SampleQuantity = row["sample_quantity"] != DBNull.Value ? Convert.ToDecimal(row["sample_quantity"]) : 0,
                QualifiedQuantity = row["qualified_quantity"] != DBNull.Value ? Convert.ToDecimal(row["qualified_quantity"]) : 0,
                UnqualifiedQuantity = row["unqualified_quantity"] != DBNull.Value ? Convert.ToDecimal(row["unqualified_quantity"]) : 0,
                InspectionResult = Convert.ToInt32(row["inspection_result"]),
                InspectionStandard = row["inspection_standard"] != DBNull.Value ? row["inspection_standard"].ToString() : string.Empty,
                InspectionItems = row["inspection_items"] != DBNull.Value ? row["inspection_items"].ToString() : string.Empty,
                InspectionData = row["inspection_data"] != DBNull.Value ? row["inspection_data"].ToString() : string.Empty,
                UnqualifiedReason = row["unqualified_reason"] != DBNull.Value ? row["unqualified_reason"].ToString() : string.Empty,
                TreatmentMeasure = row["treatment_measure"] != DBNull.Value ? row["treatment_measure"].ToString() : string.Empty,
                InspectorId = row["inspector_id"] != DBNull.Value ? Convert.ToInt32(row["inspector_id"]) : 0,
                InspectorName = row["inspector_name"] != DBNull.Value ? row["inspector_name"].ToString() : string.Empty,
                InspectionTime = Convert.ToDateTime(row["inspection_time"]),
                ReviewerId = row["reviewer_id"] != DBNull.Value ? Convert.ToInt32(row["reviewer_id"]) : 0,
                ReviewerName = row["reviewer_name"] != DBNull.Value ? row["reviewer_name"].ToString() : string.Empty,
                ReviewTime = row["review_time"] != DBNull.Value ? Convert.ToDateTime(row["review_time"]) : (DateTime?)null,
                ReviewStatus = Convert.ToInt32(row["review_status"]),
                ReviewComments = row["review_comments"] != DBNull.Value ? row["review_comments"].ToString() : string.Empty,
                CreateTime = Convert.ToDateTime(row["create_time"]),
                CreateUserId = row["create_user_id"] != DBNull.Value ? Convert.ToInt32(row["create_user_id"]) : 0,
                CreateUserName = row["create_user_name"] != DBNull.Value ? row["create_user_name"].ToString() : string.Empty,
                UpdateTime = row["update_time"] != DBNull.Value ? Convert.ToDateTime(row["update_time"]) : (DateTime?)null,
                UpdateUserId = row["update_user_id"] != DBNull.Value ? Convert.ToInt32(row["update_user_id"]) : 0,
                UpdateUserName = row["update_user_name"] != DBNull.Value ? row["update_user_name"].ToString() : string.Empty,
                IsDeleted = Convert.ToBoolean(row["is_deleted"])
            };
        }

        /// <summary>
        /// 获取插入SQL语句
        /// </summary>
        /// <returns>插入SQL</returns>
        protected override string GetInsertSql()
        {
            return @"INSERT INTO quality_inspection 
                    (inspection_number, production_order_id, production_order_number, product_code, product_name,
                     inspection_type, inspection_stage, inspection_quantity, sample_quantity, qualified_quantity, unqualified_quantity,
                     inspection_result, inspection_standard, inspection_items, inspection_data, unqualified_reason, treatment_measure,
                     inspector_id, inspector_name, inspection_time, reviewer_id, reviewer_name, review_time, review_status, review_comments,
                     create_time, create_user_id, create_user_name, is_deleted) 
                    VALUES 
                    (@InspectionNumber, @ProductionOrderId, @ProductionOrderNumber, @ProductCode, @ProductName,
                     @InspectionType, @InspectionStage, @InspectionQuantity, @SampleQuantity, @QualifiedQuantity, @UnqualifiedQuantity,
                     @InspectionResult, @InspectionStandard, @InspectionItems, @InspectionData, @UnqualifiedReason, @TreatmentMeasure,
                     @InspectorId, @InspectorName, @InspectionTime, @ReviewerId, @ReviewerName, @ReviewTime, @ReviewStatus, @ReviewComments,
                     @CreateTime, @CreateUserId, @CreateUserName, @IsDeleted)";
        }

        /// <summary>
        /// 获取更新SQL语句
        /// </summary>
        /// <returns>更新SQL</returns>
        protected override string GetUpdateSql()
        {
            return @"UPDATE quality_inspection SET 
                    inspection_number = @InspectionNumber,
                    production_order_id = @ProductionOrderId,
                    production_order_number = @ProductionOrderNumber,
                    product_code = @ProductCode,
                    product_name = @ProductName,
                    inspection_type = @InspectionType,
                    inspection_stage = @InspectionStage,
                    inspection_quantity = @InspectionQuantity,
                    sample_quantity = @SampleQuantity,
                    qualified_quantity = @QualifiedQuantity,
                    unqualified_quantity = @UnqualifiedQuantity,
                    inspection_result = @InspectionResult,
                    inspection_standard = @InspectionStandard,
                    inspection_items = @InspectionItems,
                    inspection_data = @InspectionData,
                    unqualified_reason = @UnqualifiedReason,
                    treatment_measure = @TreatmentMeasure,
                    inspector_id = @InspectorId,
                    inspector_name = @InspectorName,
                    inspection_time = @InspectionTime,
                    reviewer_id = @ReviewerId,
                    reviewer_name = @ReviewerName,
                    review_time = @ReviewTime,
                    review_status = @ReviewStatus,
                    review_comments = @ReviewComments,
                    update_time = @UpdateTime, 
                    update_user_id = @UpdateUserId, 
                    update_user_name = @UpdateUserName 
                    WHERE id = @Id";
        }

        /// <summary>
        /// 设置插入参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected override void SetInsertParameters(MySqlCommand cmd, QualityInspectionInfo entity)
        {
            cmd.Parameters.AddWithValue("@InspectionNumber", entity.InspectionNumber);
            cmd.Parameters.AddWithValue("@ProductionOrderId", entity.ProductionOrderId > 0 ? (object)entity.ProductionOrderId : DBNull.Value);
            cmd.Parameters.AddWithValue("@ProductionOrderNumber", entity.ProductionOrderNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("@ProductCode", entity.ProductCode);
            cmd.Parameters.AddWithValue("@ProductName", entity.ProductName ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionType", entity.InspectionType);
            cmd.Parameters.AddWithValue("@InspectionStage", entity.InspectionStage ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionQuantity", entity.InspectionQuantity);
            cmd.Parameters.AddWithValue("@SampleQuantity", entity.SampleQuantity);
            cmd.Parameters.AddWithValue("@QualifiedQuantity", entity.QualifiedQuantity);
            cmd.Parameters.AddWithValue("@UnqualifiedQuantity", entity.UnqualifiedQuantity);
            cmd.Parameters.AddWithValue("@InspectionResult", entity.InspectionResult);
            cmd.Parameters.AddWithValue("@InspectionStandard", entity.InspectionStandard ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionItems", entity.InspectionItems ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionData", entity.InspectionData ?? string.Empty);
            cmd.Parameters.AddWithValue("@UnqualifiedReason", entity.UnqualifiedReason ?? string.Empty);
            cmd.Parameters.AddWithValue("@TreatmentMeasure", entity.TreatmentMeasure ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectorId", entity.InspectorId);
            cmd.Parameters.AddWithValue("@InspectorName", entity.InspectorName ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionTime", entity.InspectionTime);
            cmd.Parameters.AddWithValue("@ReviewerId", entity.ReviewerId > 0 ? (object)entity.ReviewerId : DBNull.Value);
            cmd.Parameters.AddWithValue("@ReviewerName", entity.ReviewerName ?? string.Empty);
            cmd.Parameters.AddWithValue("@ReviewTime", entity.ReviewTime.HasValue ? (object)entity.ReviewTime.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@ReviewStatus", entity.ReviewStatus);
            cmd.Parameters.AddWithValue("@ReviewComments", entity.ReviewComments ?? string.Empty);
            cmd.Parameters.AddWithValue("@CreateTime", entity.CreateTime);
            cmd.Parameters.AddWithValue("@CreateUserId", entity.CreateUserId);
            cmd.Parameters.AddWithValue("@CreateUserName", entity.CreateUserName ?? string.Empty);
            cmd.Parameters.AddWithValue("@IsDeleted", entity.IsDeleted);
        }

        /// <summary>
        /// 设置更新参数
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="entity">实体对象</param>
        protected override void SetUpdateParameters(MySqlCommand cmd, QualityInspectionInfo entity)
        {
            cmd.Parameters.AddWithValue("@Id", entity.Id);
            cmd.Parameters.AddWithValue("@InspectionNumber", entity.InspectionNumber);
            cmd.Parameters.AddWithValue("@ProductionOrderId", entity.ProductionOrderId > 0 ? (object)entity.ProductionOrderId : DBNull.Value);
            cmd.Parameters.AddWithValue("@ProductionOrderNumber", entity.ProductionOrderNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("@ProductCode", entity.ProductCode);
            cmd.Parameters.AddWithValue("@ProductName", entity.ProductName ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionType", entity.InspectionType);
            cmd.Parameters.AddWithValue("@InspectionStage", entity.InspectionStage ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionQuantity", entity.InspectionQuantity);
            cmd.Parameters.AddWithValue("@SampleQuantity", entity.SampleQuantity);
            cmd.Parameters.AddWithValue("@QualifiedQuantity", entity.QualifiedQuantity);
            cmd.Parameters.AddWithValue("@UnqualifiedQuantity", entity.UnqualifiedQuantity);
            cmd.Parameters.AddWithValue("@InspectionResult", entity.InspectionResult);
            cmd.Parameters.AddWithValue("@InspectionStandard", entity.InspectionStandard ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionItems", entity.InspectionItems ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionData", entity.InspectionData ?? string.Empty);
            cmd.Parameters.AddWithValue("@UnqualifiedReason", entity.UnqualifiedReason ?? string.Empty);
            cmd.Parameters.AddWithValue("@TreatmentMeasure", entity.TreatmentMeasure ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectorId", entity.InspectorId);
            cmd.Parameters.AddWithValue("@InspectorName", entity.InspectorName ?? string.Empty);
            cmd.Parameters.AddWithValue("@InspectionTime", entity.InspectionTime);
            cmd.Parameters.AddWithValue("@ReviewerId", entity.ReviewerId > 0 ? (object)entity.ReviewerId : DBNull.Value);
            cmd.Parameters.AddWithValue("@ReviewerName", entity.ReviewerName ?? string.Empty);
            cmd.Parameters.AddWithValue("@ReviewTime", entity.ReviewTime.HasValue ? (object)entity.ReviewTime.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@ReviewStatus", entity.ReviewStatus);
            cmd.Parameters.AddWithValue("@ReviewComments", entity.ReviewComments ?? string.Empty);
            cmd.Parameters.AddWithValue("@UpdateTime", entity.UpdateTime);
            cmd.Parameters.AddWithValue("@UpdateUserId", entity.UpdateUserId);
            cmd.Parameters.AddWithValue("@UpdateUserName", entity.UpdateUserName ?? string.Empty);
        }

        /// <summary>
        /// 根据检验单号获取检验信息
        /// </summary>
        /// <param name="inspectionNumber">检验单号</param>
        /// <returns>检验信息</returns>
        public QualityInspectionInfo GetByInspectionNumber(string inspectionNumber)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} WHERE inspection_number = @InspectionNumber AND is_deleted = 0", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@InspectionNumber", inspectionNumber);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                return MapRowToEntity(dataTable.Rows[0]);
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据检验单号获取检验信息失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 根据生产订单ID获取检验列表
        /// </summary>
        /// <param name="productionOrderId">生产订单ID</param>
        /// <returns>检验列表</returns>
        public List<QualityInspectionInfo> GetByProductionOrderId(int productionOrderId)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} WHERE production_order_id = @ProductionOrderId AND is_deleted = 0 ORDER BY inspection_time DESC", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProductionOrderId", productionOrderId);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var inspections = new List<QualityInspectionInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                inspections.Add(MapRowToEntity(row));
                            }

                            return inspections;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据生产订单ID获取检验列表失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 根据检验类型获取检验列表
        /// </summary>
        /// <param name="inspectionType">检验类型</param>
        /// <returns>检验列表</returns>
        public List<QualityInspectionInfo> GetByInspectionType(int inspectionType)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} WHERE inspection_type = @InspectionType AND is_deleted = 0 ORDER BY inspection_time DESC", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@InspectionType", inspectionType);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var inspections = new List<QualityInspectionInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                inspections.Add(MapRowToEntity(row));
                            }

                            return inspections;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据检验类型获取检验列表失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 根据审核状态获取检验列表
        /// </summary>
        /// <param name="reviewStatus">审核状态</param>
        /// <returns>检验列表</returns>
        public List<QualityInspectionInfo> GetByReviewStatus(int reviewStatus)
        {
            try
            {
                string sql = string.Format("SELECT * FROM {0} WHERE review_status = @ReviewStatus AND is_deleted = 0 ORDER BY inspection_time DESC", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@ReviewStatus", reviewStatus);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var inspections = new List<QualityInspectionInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                inspections.Add(MapRowToEntity(row));
                            }

                            return inspections;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据审核状态获取检验列表失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 搜索质量检验记录
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <returns>检验列表</returns>
        public List<QualityInspectionInfo> Search(string keyword)
        {
            try
            {
                string sql = string.Format(@"SELECT * FROM {0}
                               WHERE (inspection_number LIKE @Keyword OR production_order_number LIKE @Keyword
                                      OR product_code LIKE @Keyword OR product_name LIKE @Keyword
                                      OR inspector_name LIKE @Keyword OR reviewer_name LIKE @Keyword)
                               AND is_deleted = 0
                               ORDER BY inspection_time DESC", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", string.Format("%{0}%", keyword));

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            var inspections = new List<QualityInspectionInfo>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                inspections.Add(MapRowToEntity(row));
                            }

                            return inspections;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索质量检验记录失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 获取质量统计数据
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>统计数据</returns>
        public Dictionary<string, object> GetQualityStatistics(DateTime startDate, DateTime endDate)
        {
            try
            {
                string sql = string.Format(@"SELECT
                               COUNT(*) as TotalCount,
                               SUM(CASE WHEN inspection_result = 1 THEN 1 ELSE 0 END) as QualifiedCount,
                               SUM(CASE WHEN inspection_result = 2 THEN 1 ELSE 0 END) as UnqualifiedCount,
                               SUM(CASE WHEN inspection_result = 3 THEN 1 ELSE 0 END) as ConcessionalCount,
                               SUM(inspection_quantity) as TotalInspectionQuantity,
                               SUM(qualified_quantity) as TotalQualifiedQuantity,
                               SUM(unqualified_quantity) as TotalUnqualifiedQuantity
                               FROM {0}
                               WHERE inspection_time BETWEEN @StartDate AND @EndDate
                               AND is_deleted = 0", TableName);
                
                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var totalCount = Convert.ToInt32(reader["TotalCount"]);
                                var qualifiedCount = Convert.ToInt32(reader["QualifiedCount"]);
                                var totalInspectionQuantity = Convert.ToDecimal(reader["TotalInspectionQuantity"]);
                                var totalQualifiedQuantity = Convert.ToDecimal(reader["TotalQualifiedQuantity"]);

                                var result = new Dictionary<string, object>();
                                result.Add("TotalCount", totalCount);
                                result.Add("QualifiedCount", qualifiedCount);
                                result.Add("UnqualifiedCount", Convert.ToInt32(reader["UnqualifiedCount"]));
                                result.Add("ConcessionalCount", Convert.ToInt32(reader["ConcessionalCount"]));
                                result.Add("QualifiedRate", totalCount > 0 ? Math.Round((decimal)qualifiedCount / totalCount * 100, 2) : 0);
                                result.Add("TotalInspectionQuantity", totalInspectionQuantity);
                                result.Add("TotalQualifiedQuantity", totalQualifiedQuantity);
                                result.Add("TotalUnqualifiedQuantity", Convert.ToDecimal(reader["TotalUnqualifiedQuantity"]));
                                result.Add("QuantityQualifiedRate", totalInspectionQuantity > 0 ? Math.Round(totalQualifiedQuantity / totalInspectionQuantity * 100, 2) : 0);
                                return result;
                            }
                        }
                    }
                }
                
                return new Dictionary<string, object>();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取质量统计数据失败：{0}", ex.Message), ex);
                throw;
            }
        }

        /// <summary>
        /// 获取不合格原因统计（按数量降序）
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="top">返回数量</param>
        /// <returns>原因与数量的映射</returns>
        public Dictionary<string, int> GetDefectReasonStatistics(DateTime startDate, DateTime endDate, int top)
        {
            try
            {
                if (top <= 0) top = 5;

                string sql = string.Format(@"SELECT
                               IFNULL(unqualified_reason, '未填写') as defect_reason,
                               COUNT(1) as defect_count
                               FROM {0}
                               WHERE inspection_time BETWEEN @StartDate AND @EndDate
                               AND inspection_result = 2
                               AND is_deleted = 0
                               GROUP BY defect_reason
                               ORDER BY defect_count DESC
                               LIMIT @Top", TableName);

                using (var connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new MySqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@Top", top);

                        using (var reader = cmd.ExecuteReader())
                        {
                            var result = new Dictionary<string, int>();
                            while (reader.Read())
                            {
                                var reason = reader["defect_reason"] != DBNull.Value
                                    ? reader["defect_reason"].ToString()
                                    : "未填写";
                                var count = Convert.ToInt32(reader["defect_count"]);
                                result[reason] = count;
                            }
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取缺陷原因统计失败：{0}", ex.Message), ex);
                throw;
            }
        }
    }
}
