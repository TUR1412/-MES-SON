using System;
using MES.Models.Base;

namespace MES.Models.System
{
    /// <summary>
    /// 用户信息模型
    /// </summary>
    public class UserInfo : BaseModel
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码（加密后）
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 状态：1-启用，0-禁用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserInfo()
        {
            Status = true;
        }
    }
}
