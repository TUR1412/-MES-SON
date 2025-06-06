using System;
using System.Collections.Generic;
using System.Linq;
using MES.BLL.System;
using MES.DAL.System;
using MES.Models.System;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.BLL.System
{
    /// <summary>
    /// 角色管理业务逻辑实现类
    /// 实现角色管理的核心业务逻辑
    /// </summary>
    public class RoleBLL : IRoleBLL
    {
        private readonly RoleDAL _roleDAL;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleBLL()
        {
            _roleDAL = new RoleDAL();
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>操作是否成功</returns>
        public bool AddRole(RoleInfo role)
        {
            try
            {
                // 验证输入参数
                if (role == null)
                {
                    LogManager.Error("添加角色失败：角色信息不能为空");
                    return false;
                }

                // 业务规则验证
                string validationResult = ValidateRole(role);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error(string.Format("添加角色失败：{0}", validationResult));
                    return false;
                }

                // 检查角色编码是否已存在
                if (IsRoleCodeExists(role.RoleCode))
                {
                    LogManager.Error(string.Format("添加角色失败：角色编码 {0} 已存在", role.RoleCode));
                    return false;
                }

                // 设置默认值
                role.CreateTime = DateTime.Now;
                role.UpdateTime = DateTime.Now;
                role.IsDeleted = false;

                // 如果未设置状态，默认为启用
                if (role.Status == 0)
                {
                    role.Status = 1;
                }

                // 调用DAL层添加
                bool result = _roleDAL.Add(role);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功添加角色：{0} - {1}", role.RoleCode, role.RoleName));
                }
                else
                {
                    LogManager.Error(string.Format("添加角色失败：{0}", role.RoleCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("添加角色异常：{0}", ex.Message), ex);
                throw new MESException("添加角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID删除角色（逻辑删除）
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作是否成功</returns>
        public bool DeleteRole(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("删除角色失败：ID无效");
                    return false;
                }

                // 检查角色是否存在
                var existingRole = _roleDAL.GetById(id);
                if (existingRole == null)
                {
                    LogManager.Error(string.Format("删除角色失败：ID为 {0} 的角色不存在", id));
                    return false;
                }

                // 检查是否为系统内置角色
                if (existingRole.RoleCode == "ADMIN" || existingRole.RoleCode == "SYSTEM")
                {
                    LogManager.Error(string.Format("删除角色失败：系统内置角色 {0} 不能删除", existingRole.RoleCode));
                    return false;
                }

                // TODO: 检查角色是否被用户使用，如果有用户使用该角色，不能删除

                bool result = _roleDAL.Delete(id);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功删除角色：ID={0}, 角色编码={1}", id, existingRole.RoleCode));
                }
                else
                {
                    LogManager.Error(string.Format("删除角色失败：ID={0}", id));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("删除角色异常：{0}", ex.Message), ex);
                throw new MESException("删除角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>操作是否成功</returns>
        public bool UpdateRole(RoleInfo role)
        {
            try
            {
                if (role == null || role.Id <= 0)
                {
                    LogManager.Error("更新角色失败：角色信息无效");
                    return false;
                }

                // 验证业务规则
                string validationResult = ValidateRole(role);
                if (!string.IsNullOrEmpty(validationResult))
                {
                    LogManager.Error(string.Format("更新角色失败：{0}", validationResult));
                    return false;
                }

                // 检查角色是否存在
                var existingRole = _roleDAL.GetById(role.Id);
                if (existingRole == null)
                {
                    LogManager.Error(string.Format("更新角色失败：ID为 {0} 的角色不存在", role.Id));
                    return false;
                }

                // 检查角色编码是否与其他角色冲突
                if (IsRoleCodeExists(role.RoleCode, role.Id))
                {
                    LogManager.Error(string.Format("更新角色失败：角色编码 {0} 已被其他角色使用", role.RoleCode));
                    return false;
                }

                // 更新时间
                role.UpdateTime = DateTime.Now;

                bool result = _roleDAL.Update(role);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功更新角色：{0} - {1}", role.RoleCode, role.RoleName));
                }
                else
                {
                    LogManager.Error(string.Format("更新角色失败：{0}", role.RoleCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("更新角色异常：{0}", ex.Message), ex);
                throw new MESException("更新角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据ID获取角色信息
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>角色信息，未找到返回null</returns>
        public RoleInfo GetRoleById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    LogManager.Error("获取角色失败：ID无效");
                    return null;
                }

                return _roleDAL.GetById(id);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取角色异常：{0}", ex.Message), ex);
                throw new MESException("获取角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据角色编码获取角色信息
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns>角色信息，未找到返回null</returns>
        public RoleInfo GetRoleByCode(string roleCode)
        {
            try
            {
                if (string.IsNullOrEmpty(roleCode))
                {
                    LogManager.Error("获取角色失败：角色编码不能为空");
                    return null;
                }

                return _roleDAL.GetByRoleCode(roleCode);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据角色编码获取角色异常：{0}", ex.Message), ex);
                throw new MESException("根据角色编码获取角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns>角色列表</returns>
        public List<RoleInfo> GetAllRoles()
        {
            try
            {
                return _roleDAL.GetAll();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取所有角色异常：{0}", ex.Message), ex);
                throw new MESException("获取所有角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据状态获取角色列表
        /// </summary>
        /// <param name="status">角色状态</param>
        /// <returns>指定状态的角色列表</returns>
        public List<RoleInfo> GetRolesByStatus(int status)
        {
            try
            {
                return _roleDAL.GetByStatus(status);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("根据状态获取角色异常：{0}", ex.Message), ex);
                throw new MESException("根据状态获取角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="pageIndex">页码（从1开始）</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>分页的角色列表</returns>
        public List<RoleInfo> GetRolesByPage(int pageIndex, int pageSize, out int totalCount)
        {
            try
            {
                if (pageIndex <= 0 || pageSize <= 0)
                {
                    LogManager.Error("分页获取角色失败：页码和每页记录数必须大于0");
                    totalCount = 0;
                    return new List<RoleInfo>();
                }

                // 简化实现：从所有角色中分页
                var allRoles = GetAllRoles();
                totalCount = allRoles.Count;
                return allRoles.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("分页获取角色异常：{0}", ex.Message), ex);
                totalCount = 0;
                throw new MESException("分页获取角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 根据条件搜索角色
        /// </summary>
        /// <param name="keyword">搜索关键词（角色编码、名称等）</param>
        /// <returns>匹配的角色列表</returns>
        public List<RoleInfo> SearchRoles(string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return GetAllRoles();
                }

                return _roleDAL.Search(keyword);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("搜索角色异常：{0}", ex.Message), ex);
                throw new MESException("搜索角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作是否成功</returns>
        public bool EnableRole(int id)
        {
            try
            {
                var role = _roleDAL.GetById(id);
                if (role == null)
                {
                    LogManager.Error(string.Format("启用角色失败：ID为 {0} 的角色不存在", id));
                    return false;
                }

                if (role.Status == 1)
                {
                    LogManager.Info(string.Format("角色 {0} 已经是启用状态", role.RoleCode));
                    return true;
                }

                role.Status = 1;
                role.UpdateTime = DateTime.Now;

                bool result = _roleDAL.Update(role);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功启用角色：{0}", role.RoleCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("启用角色异常：{0}", ex.Message), ex);
                throw new MESException("启用角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作是否成功</returns>
        public bool DisableRole(int id)
        {
            try
            {
                var role = _roleDAL.GetById(id);
                if (role == null)
                {
                    LogManager.Error(string.Format("禁用角色失败：ID为 {0} 的角色不存在", id));
                    return false;
                }

                // 检查是否为系统管理员角色
                if (role.RoleCode == "ADMIN")
                {
                    LogManager.Error("禁用角色失败：系统管理员角色不能禁用");
                    return false;
                }

                if (role.Status == 0)
                {
                    LogManager.Info(string.Format("角色 {0} 已经是禁用状态", role.RoleCode));
                    return true;
                }

                role.Status = 0;
                role.UpdateTime = DateTime.Now;

                bool result = _roleDAL.Update(role);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功禁用角色：{0}", role.RoleCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("禁用角色异常：{0}", ex.Message), ex);
                throw new MESException("禁用角色时发生异常", ex);
            }
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="permissions">权限列表（JSON格式）</param>
        /// <returns>操作是否成功</returns>
        public bool SetRolePermissions(int id, string permissions)
        {
            try
            {
                var role = _roleDAL.GetById(id);
                if (role == null)
                {
                    LogManager.Error(string.Format("设置角色权限失败：ID为 {0} 的角色不存在", id));
                    return false;
                }

                role.Permissions = permissions ?? string.Empty;
                role.UpdateTime = DateTime.Now;

                bool result = _roleDAL.Update(role);
                
                if (result)
                {
                    LogManager.Info(string.Format("成功设置角色 {0} 的权限", role.RoleCode));
                }

                return result;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("设置角色权限异常：{0}", ex.Message), ex);
                throw new MESException("设置角色权限时发生异常", ex);
            }
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>权限列表（JSON格式）</returns>
        public string GetRolePermissions(int id)
        {
            try
            {
                var role = _roleDAL.GetById(id);
                if (role == null)
                {
                    LogManager.Error(string.Format("获取角色权限失败：ID为 {0} 的角色不存在", id));
                    return string.Empty;
                }

                return role.Permissions ?? string.Empty;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取角色权限异常：{0}", ex.Message), ex);
                throw new MESException("获取角色权限时发生异常", ex);
            }
        }

        /// <summary>
        /// 检查角色是否有指定权限
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="permission">权限编码</param>
        /// <returns>是否有权限</returns>
        public bool HasPermission(string roleCode, string permission)
        {
            try
            {
                if (string.IsNullOrEmpty(roleCode) || string.IsNullOrEmpty(permission))
                {
                    return false;
                }

                // 系统管理员拥有所有权限
                if (roleCode == "ADMIN")
                {
                    return true;
                }

                var role = _roleDAL.GetByRoleCode(roleCode);
                if (role == null || role.Status != 1)
                {
                    return false;
                }

                // TODO: 解析JSON权限列表，检查是否包含指定权限
                // 这里需要根据实际的权限存储格式来实现
                return !string.IsNullOrEmpty(role.Permissions) && role.Permissions.Contains(permission);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("检查角色权限异常：{0}", ex.Message), ex);
                return false;
            }
        }

        /// <summary>
        /// 验证角色数据
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns>验证结果消息，验证通过返回空字符串</returns>
        public string ValidateRole(RoleInfo role)
        {
            if (role == null)
            {
                return "角色信息不能为空";
            }

            if (string.IsNullOrEmpty(role.RoleCode))
            {
                return "角色编码不能为空";
            }

            if (string.IsNullOrEmpty(role.RoleName))
            {
                return "角色名称不能为空";
            }

            if (role.RoleCode.Length > 50)
            {
                return "角色编码长度不能超过50个字符";
            }

            if (role.RoleName.Length > 100)
            {
                return "角色名称长度不能超过100个字符";
            }

            return string.Empty;
        }

        /// <summary>
        /// 检查角色编码是否已存在
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <param name="excludeId">排除的角色ID（用于更新时检查）</param>
        /// <returns>是否已存在</returns>
        public bool IsRoleCodeExists(string roleCode, int excludeId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(roleCode))
                {
                    return false;
                }

                return _roleDAL.IsRoleCodeExists(roleCode, excludeId);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("检查角色编码是否存在异常：{0}", ex.Message), ex);
                return false;
            }
        }

        /// <summary>
        /// 获取角色统计信息
        /// </summary>
        /// <returns>统计信息字典</returns>
        public Dictionary<string, object> GetRoleStatistics()
        {
            try
            {
                var allRoles = _roleDAL.GetAll();
                var statistics = new Dictionary<string, object>();
                statistics.Add("TotalCount", allRoles.Count);
                statistics.Add("EnabledCount", allRoles.Count(r => r.Status == 1));
                statistics.Add("DisabledCount", allRoles.Count(r => r.Status == 0));
                statistics.Add("SystemRoleCount", allRoles.Count(r => r.RoleCode == "ADMIN" || r.RoleCode == "SYSTEM"));
                statistics.Add("CustomRoleCount", allRoles.Count(r => r.RoleCode != "ADMIN" && r.RoleCode != "SYSTEM"));

                return statistics;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("获取角色统计信息异常：{0}", ex.Message), ex);
                throw new MESException("获取角色统计信息时发生异常", ex);
            }
        }

        /// <summary>
        /// 批量启用/禁用角色
        /// </summary>
        /// <param name="ids">角色ID列表</param>
        /// <param name="status">状态：1-启用，0-禁用</param>
        /// <returns>操作是否成功</returns>
        public bool BatchUpdateRoleStatus(List<int> ids, int status)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    LogManager.Error("批量更新角色状态失败：角色ID列表不能为空");
                    return false;
                }

                int successCount = 0;
                foreach (var id in ids)
                {
                    if (status == 1 ? EnableRole(id) : DisableRole(id))
                    {
                        successCount++;
                    }
                }

                LogManager.Info(string.Format("批量更新角色状态完成：成功 {0}/{1}", successCount, ids.Count));
                return successCount == ids.Count;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("批量更新角色状态异常：{0}", ex.Message), ex);
                throw new MESException("批量更新角色状态时发生异常", ex);
            }
        }

        /// <summary>
        /// 复制角色
        /// </summary>
        /// <param name="sourceId">源角色ID</param>
        /// <param name="newRoleCode">新角色编码</param>
        /// <param name="newRoleName">新角色名称</param>
        /// <returns>操作是否成功</returns>
        public bool CopyRole(int sourceId, string newRoleCode, string newRoleName)
        {
            try
            {
                var sourceRole = _roleDAL.GetById(sourceId);
                if (sourceRole == null)
                {
                    LogManager.Error(string.Format("复制角色失败：源角色ID {0} 不存在", sourceId));
                    return false;
                }

                if (IsRoleCodeExists(newRoleCode))
                {
                    LogManager.Error(string.Format("复制角色失败：新角色编码 {0} 已存在", newRoleCode));
                    return false;
                }

                var newRole = new RoleInfo
                {
                    RoleCode = newRoleCode,
                    RoleName = newRoleName,
                    Description = string.Format("复制自 {0}", sourceRole.RoleName),
                    Status = sourceRole.Status,
                    Permissions = sourceRole.Permissions,
                    SortOrder = sourceRole.SortOrder
                };

                return AddRole(newRole);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("复制角色异常：{0}", ex.Message), ex);
                throw new MESException("复制角色时发生异常", ex);
            }
        }
    }
}
