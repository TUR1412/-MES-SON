using System;
using System.Collections.Generic;
using MES.BLL.Quality;
using MES.DAL.Quality;
using MES.Models.Quality;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.Quality
{
    /// <summary>
    /// 质量检验业务逻辑类
    /// 提供质量检验管理的业务逻辑功能
    /// </summary>
    public class QualityInspectionBLL : IQualityInspectionBLL
    {
        private readonly QualityInspectionDAL _qualityInspectionDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualityInspectionBLL()
        {
            _qualityInspectionDAL = new QualityInspectionDAL();
        }

        /// <summary>
        /// 根据ID获取质量检验信息
        /// </summary>
        /// <param name="id">检验ID</param>
        /// <returns>检验信息</returns>
        public QualityInspectionInfo GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("检验ID必须大于0", "id");
                }

                return _qualityInspectionDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取质量检验信息失败，ID: {0}", id), ex);
                throw new MESException("获取质量检验信息失败", ex);
            }
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
                if (string.IsNullOrEmpty(inspectionNumber))
                {
                    throw new ArgumentException("检验单号不能为空", "inspectionNumber");
                }

                return _qualityInspectionDAL.GetByInspectionNumber(inspectionNumber);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据检验单号获取检验信息失败，单号: {0}", inspectionNumber), ex);
                throw new MESException("获取质量检验信息失败", ex);
            }
        }

        /// <summary>
        /// 获取所有质量检验列表
        /// </summary>
        /// <returns>检验列表</returns>
        public List<QualityInspectionInfo> GetAll()
        {
            try
            {
                return _qualityInspectionDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error("获取所有质量检验列表失败", ex);
                throw new MESException("获取质量检验列表失败", ex);
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
                if (productionOrderId <= 0)
                {
                    throw new ArgumentException("生产订单ID必须大于0", "productionOrderId");
                }

                return _qualityInspectionDAL.GetByProductionOrderId(productionOrderId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据生产订单ID获取检验列表失败，订单ID: {0}", productionOrderId), ex);
                throw new MESException("获取质量检验列表失败", ex);
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
                return _qualityInspectionDAL.GetByInspectionType(inspectionType);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据检验类型获取检验列表失败，类型: {0}", inspectionType), ex);
                throw new MESException("获取质量检验列表失败", ex);
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
                return _qualityInspectionDAL.GetByReviewStatus(reviewStatus);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据审核状态获取检验列表失败，状态: {0}", reviewStatus), ex);
                throw new MESException("获取质量检验列表失败", ex);
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
                if (string.IsNullOrEmpty(keyword))
                {
                    return GetAll();
                }

                return _qualityInspectionDAL.Search(keyword);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索质量检验记录失败，关键词: {0}", keyword), ex);
                throw new MESException("搜索质量检验记录失败", ex);
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
                if (startDate > endDate)
                {
                    throw new ArgumentException("开始日期不能大于结束日期");
                }

                return _qualityInspectionDAL.GetQualityStatistics(startDate, endDate);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取质量统计数据失败，时间范围: {0:yyyy-MM-dd} 到 {1:yyyy-MM-dd}", startDate, endDate), ex);
                throw new MESException("获取质量统计数据失败", ex);
            }
        }

        /// <summary>
        /// 添加质量检验记录
        /// </summary>
        /// <param name="inspection">检验信息</param>
        /// <returns>是否添加成功</returns>
        public bool Add(QualityInspectionInfo inspection)
        {
            try
            {
                if (inspection == null)
                {
                    throw new ArgumentNullException("inspection");
                }

                // 业务验证
                ValidateInspection(inspection);

                // 生成检验单号（如果为空）
                if (string.IsNullOrEmpty(inspection.InspectionNumber))
                {
                    inspection.InspectionNumber = GenerateInspectionNumber();
                }

                return _qualityInspectionDAL.Add(inspection);
            }
            catch (Exception ex)
            {
                LogManager.Error("添加质量检验记录失败", ex);
                throw new MESException("添加质量检验记录失败", ex);
            }
        }

        /// <summary>
        /// 更新质量检验记录
        /// </summary>
        /// <param name="inspection">检验信息</param>
        /// <returns>是否更新成功</returns>
        public bool Update(QualityInspectionInfo inspection)
        {
            try
            {
                if (inspection == null)
                {
                    throw new ArgumentNullException("inspection");
                }

                // 业务验证
                ValidateInspection(inspection);

                return _qualityInspectionDAL.Update(inspection);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新质量检验记录失败，ID: {0}", inspection != null ? inspection.Id.ToString() : "null"), ex);
                throw new MESException("更新质量检验记录失败", ex);
            }
        }

        /// <summary>
        /// 删除质量检验记录
        /// </summary>
        /// <param name="id">检验ID</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("检验ID必须大于0", "id");
                }

                return _qualityInspectionDAL.Delete(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除质量检验记录失败，ID: {0}", id), ex);
                throw new MESException("删除质量检验记录失败", ex);
            }
        }

        /// <summary>
        /// 验证检验信息
        /// </summary>
        /// <param name="inspection">检验信息</param>
        private void ValidateInspection(QualityInspectionInfo inspection)
        {
            if (string.IsNullOrEmpty(inspection.ProductCode))
            {
                throw new ArgumentException("产品编码不能为空");
            }

            if (inspection.InspectionQuantity <= 0)
            {
                throw new ArgumentException("检验数量必须大于0");
            }

            if (inspection.SampleQuantity <= 0)
            {
                throw new ArgumentException("抽样数量必须大于0");
            }

            if (inspection.SampleQuantity > inspection.InspectionQuantity)
            {
                throw new ArgumentException("抽样数量不能大于检验数量");
            }

            if (string.IsNullOrEmpty(inspection.InspectorName))
            {
                throw new ArgumentException("检验员姓名不能为空");
            }
        }

        /// <summary>
        /// 生成检验单号
        /// </summary>
        /// <returns>检验单号</returns>
        private string GenerateInspectionNumber()
        {
            return string.Format("QI{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), new Random().Next(100, 999));
        }
    }
}
