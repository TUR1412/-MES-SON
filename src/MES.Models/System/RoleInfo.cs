using System;
using MES.Models.Base;

namespace MES.Models.System
{
    /// <summary>
    /// 角色信息模型
    /// 用于系统角色管理，定义用户角色和权限分组
    /// </summary>
    public class RoleInfo : BaseModel
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 角色状态：1-启用，0-禁用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 权限列表（JSON格式存储）
        /// </summary>
        public string Permissions { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleInfo()
        {
            RoleCode = string.Empty;
            RoleName = string.Empty;
            Description = string.Empty;
            Status = 1; // 默认启用
            Permissions = string.Empty;
            SortOrder = 0;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="description">角色描述</param>
        public RoleInfo(string roleCode, string roleName, string description = "")
        {
            RoleCode = roleCode ?? string.Empty;
            RoleName = roleName ?? string.Empty;
            Description = description ?? string.Empty;
            Status = 1;
            Permissions = string.Empty;
            SortOrder = 0;
        }

        /// <summary>
        /// 获取状态显示文本
        /// </summary>
        /// <returns>状态文本</returns>
        public string GetStatusText()
        {
            return Status == 1 ? "启用" : "禁用";
        }

        /// <summary>
        /// 检查角色是否启用
        /// </summary>
        /// <returns>是否启用</returns>
        public bool IsEnabled()
        {
            return Status == 1;
        }

        /// <summary>
        /// 验证角色信息是否有效
        /// </summary>
        /// <returns>验证结果</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(RoleCode) && !string.IsNullOrEmpty(RoleName);
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>角色信息字符串</returns>
        public override string ToString()
        {
            return $"{RoleCode} - {RoleName}";
        }
    }
}
