using System;

namespace MES.Models.Base
{
    /// <summary>
    /// 基础模型类 - 所有实体模型的基类
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 最后修改人ID
        /// </summary>
        public int? UpdateUserId { get; set; }

        /// <summary>
        /// 最后修改人姓名
        /// </summary>
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 是否删除（软删除标记）
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }

        /// <summary>
        /// 删除人ID
        /// </summary>
        public int? DeleteUserId { get; set; }

        /// <summary>
        /// 删除人姓名
        /// </summary>
        public string DeleteUserName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 版本号（用于乐观锁）
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseModel()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
            Version = 1;
        }

        /// <summary>
        /// 设置创建信息
        /// </summary>
        /// <param name="userId">创建人ID</param>
        /// <param name="userName">创建人姓名</param>
        public virtual void SetCreateInfo(int userId, string userName)
        {
            CreateUserId = userId;
            CreateUserName = userName;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 设置更新信息
        /// </summary>
        /// <param name="userId">更新人ID</param>
        /// <param name="userName">更新人姓名</param>
        public virtual void SetUpdateInfo(int userId, string userName)
        {
            UpdateUserId = userId;
            UpdateUserName = userName;
            UpdateTime = DateTime.Now;
            Version++;
        }

        /// <summary>
        /// 设置删除信息（软删除）
        /// </summary>
        /// <param name="userId">删除人ID</param>
        /// <param name="userName">删除人姓名</param>
        public virtual void SetDeleteInfo(int userId, string userName)
        {
            DeleteUserId = userId;
            DeleteUserName = userName;
            DeleteTime = DateTime.Now;
            IsDeleted = true;
            Version++;
        }

        /// <summary>
        /// 恢复删除（取消软删除）
        /// </summary>
        public virtual void RestoreDelete()
        {
            IsDeleted = false;
            DeleteTime = null;
            DeleteUserId = null;
            DeleteUserName = null;
            Version++;
        }
    }
}
