using System;
using System.Collections.Generic;
using MES.Models.System;

namespace MES.BLL.System
{
    /// <summary>
    /// 角色管理业务逻辑接口
    /// 定义角色管理的核心业务操作
    /// </summary>
    public interface IRoleBLL
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>操作是否成功</returns>
        bool AddRole(RoleInfo role);

        /// <summary>
        /// 根据ID删除角色（逻辑删除）
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作是否成功</returns>
        bool DeleteRole(int id);

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>操作是否成功</returns>
        bool UpdateRole(RoleInfo role);

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>角色信息，未找到返回null</returns>
        RoleInfo GetRoleById(int id);

        /// <summary>
        /// 根据角色编码获取角色信息
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>角色信息，未找到返回null</returns>
        RoleInfo GetRoleByCode(string roleCode);

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns>角色列表</returns>
        List<RoleInfo> GetAllRoles();

        /// <summary>
        /// 根据状态获取角色列表
        /// </summary>
        /// <param name="status">角色状态</param>
        /// <returns>指定状态的角色列表</returns>
        List<RoleInfo> GetRolesByStatus(int status);

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的角色列表</returns>
        List<RoleInfo> GetRolesByPage(int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 根据条件搜索角色
        /// </summary>
        /// <param name="keyword">搜索关键词（角色编码、名称等）</param>
        /// <returns>匹配的角色列表</returns>
        List<RoleInfo> SearchRoles(string keyword);

        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作是否成功</returns>
        bool EnableRole(int id);

        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作是否成功</returns>
        bool DisableRole(int id);

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="permissions">权限列表（JSON格式）</param>
        /// <returns>操作是否成功</returns>
        bool SetRolePermissions(int id, string permissions);

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>权限列表（JSON格式）</returns>
        string GetRolePermissions(int id);

        /// <summary>
        /// 检查角色是否有指定权限
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="permission">权限编码</param>
        /// <returns>是否有权限</returns>
        bool HasPermission(string roleCode, string permission);

        /// <summary>
        /// 验证角色数据
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        string ValidateRole(RoleInfo role);

        /// <summary>
        /// 检查角色编码是否已存在
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="excludeId">排除的角色ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        bool IsRoleCodeExists(string roleCode, int excludeId = 0);

        /// <summary>
        /// 获取角色统计信息
        /// </summary>
        /// <returns>统计信息字典</returns>
        Dictionary<string, object> GetRoleStatistics();

        /// <summary>
        /// 批量启用/禁用角色
        /// </summary>
        /// <param name="ids">角色ID列表</param>
        /// <param name="status">状态：1-启用，0-禁用</param>
        /// <returns>操作是否成功</returns>
        bool BatchUpdateRoleStatus(List<int> ids, int status);

        /// <summary>
        /// 复制角色
        /// </summary>
        /// <param name="sourceId">源角色ID</param>
        /// <param name="newRoleCode">新角色编码</param>
        /// <param name="newRoleName">新角色名称</param>
        /// <returns>操作是否成功</returns>
        bool CopyRole(int sourceId, string newRoleCode, string newRoleName);
    }
}
