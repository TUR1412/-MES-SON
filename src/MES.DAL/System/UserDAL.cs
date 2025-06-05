using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MES.Models.Base;
using MES.Models.System;
using MES.DAL.Base;
using MES.DAL.Core;
using MES.Common.Logging;
using MES.Common.Exceptions;

namespace MES.DAL.System
{
    /// <summary>
    /// 用户信息数据访问类
    /// 提供用户相关的数据库操作功能
    /// </summary>
    public class UserDAL : BaseDAL<UserInfo>
    {
        #region 基类实现

        /// <summary>
        /// 表名
        /// </summary>
        protected override string TableName => "sys_user";

        #endregion

        #region 用户特有操作

        /// <summary>
        /// 根据登录名获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>用户信息，不存在时返回null</returns>
        public UserInfo GetByLoginName(string loginName)
        {
            try
            {
                if (string.IsNullOrEmpty(loginName))
                {
                    throw new ArgumentException("登录名不能为空", nameof(loginName));
                }

                var users = GetByCondition("login_name = @loginName", 
                    DatabaseHelper.CreateParameter("@loginName", loginName));
                
                return users.Count > 0 ? users[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据登录名获取用户失败，登录名: {loginName}", ex);
                throw new MESException("获取用户信息失败", ex);
            }
        }

        /// <summary>
        /// 根据用户编码获取用户信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns>用户信息，不存在时返回null</returns>
        public UserInfo GetByUserCode(string userCode)
        {
            try
            {
                if (string.IsNullOrEmpty(userCode))
                {
                    throw new ArgumentException("用户编码不能为空", nameof(userCode));
                }

                var users = GetByCondition("user_code = @userCode", 
                    DatabaseHelper.CreateParameter("@userCode", userCode));
                
                return users.Count > 0 ? users[0] : null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据用户编码获取用户失败，用户编码: {userCode}", ex);
                throw new MESException("获取用户信息失败", ex);
            }
        }

        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="password">密码（已加密）</param>
        /// <returns>用户信息，验证失败返回null</returns>
        public UserInfo ValidateLogin(string loginName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
                {
                    return null;
                }

                var users = GetByCondition("login_name = @loginName AND password = @password", 
                    DatabaseHelper.CreateParameter("@loginName", loginName),
                    DatabaseHelper.CreateParameter("@password", password));
                
                if (users.Count > 0)
                {
                    LogManager.Info($"用户登录验证成功，登录名: {loginName}");
                    return users[0];
                }
                
                LogManager.Warning($"用户登录验证失败，登录名: {loginName}");
                return null;
            }
            catch (Exception ex)
            {
                LogManager.Error($"用户登录验证异常，登录名: {loginName}", ex);
                throw new MESException("用户登录验证失败", ex);
            }
        }

        /// <summary>
        /// 根据部门获取用户列表
        /// </summary>
        /// <param name="department">部门名称</param>
        /// <returns>用户列表</returns>
        public List<UserInfo> GetByDepartment(string department)
        {
            try
            {
                if (string.IsNullOrEmpty(department))
                {
                    return new List<UserInfo>();
                }

                return GetByCondition("department = @department", 
                    DatabaseHelper.CreateParameter("@department", department));
            }
            catch (Exception ex)
            {
                LogManager.Error($"根据部门获取用户列表失败，部门: {department}", ex);
                throw new MESException("获取部门用户列表失败", ex);
            }
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newPassword">新密码（已加密）</param>
        /// <returns>是否更新成功</returns>
        public bool UpdatePassword(int userId, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(newPassword))
                {
                    throw new ArgumentException("新密码不能为空", nameof(newPassword));
                }

                string sql = "UPDATE sys_user SET password = @password, update_time = @updateTime WHERE id = @userId AND is_deleted = 0";
                var parameters = new[]
                {
                    DatabaseHelper.CreateParameter("@password", newPassword),
                    DatabaseHelper.CreateParameter("@updateTime", DateTime.Now),
                    DatabaseHelper.CreateParameter("@userId", userId)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(sql, parameters);
                
                bool success = rowsAffected > 0;
                if (success)
                {
                    LogManager.Info($"用户密码更新成功，用户ID: {userId}");
                }
                
                return success;
            }
            catch (Exception ex)
            {
                LogManager.Error($"更新用户密码失败，用户ID: {userId}", ex);
                throw new MESException("更新用户密码失败", ex);
            }
        }

        #endregion

        #region SQL构建实现

        /// <summary>
        /// 构建INSERT SQL语句
        /// </summary>
        /// <param name="entity">用户实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, SqlParameter[] parameters) BuildInsertSql(UserInfo entity)
        {
            string sql = @"INSERT INTO sys_user 
                          (user_code, user_name, login_name, password, department, position, 
                           create_time, create_user_name, is_deleted) 
                          VALUES 
                          (@userCode, @userName, @loginName, @password, @department, @position, 
                           @createTime, @createUserName, @isDeleted)";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@userCode", entity.UserCode),
                DatabaseHelper.CreateParameter("@userName", entity.UserName),
                DatabaseHelper.CreateParameter("@loginName", entity.LoginName),
                DatabaseHelper.CreateParameter("@password", entity.Password),
                DatabaseHelper.CreateParameter("@department", entity.Department),
                DatabaseHelper.CreateParameter("@position", entity.Position),
                DatabaseHelper.CreateParameter("@createTime", entity.CreateTime),
                DatabaseHelper.CreateParameter("@createUserName", entity.CreateUserName),
                DatabaseHelper.CreateParameter("@isDeleted", entity.IsDeleted)
            };

            return (sql, parameters);
        }

        /// <summary>
        /// 构建UPDATE SQL语句
        /// </summary>
        /// <param name="entity">用户实体</param>
        /// <returns>SQL语句和参数</returns>
        protected override (string sql, SqlParameter[] parameters) BuildUpdateSql(UserInfo entity)
        {
            string sql = @"UPDATE sys_user SET 
                          user_code = @userCode, user_name = @userName, login_name = @loginName, 
                          department = @department, position = @position, 
                          update_time = @updateTime, update_user_name = @updateUserName 
                          WHERE id = @id AND is_deleted = 0";

            var parameters = new[]
            {
                DatabaseHelper.CreateParameter("@userCode", entity.UserCode),
                DatabaseHelper.CreateParameter("@userName", entity.UserName),
                DatabaseHelper.CreateParameter("@loginName", entity.LoginName),
                DatabaseHelper.CreateParameter("@department", entity.Department),
                DatabaseHelper.CreateParameter("@position", entity.Position),
                DatabaseHelper.CreateParameter("@updateTime", entity.UpdateTime),
                DatabaseHelper.CreateParameter("@updateUserName", entity.UpdateUserName),
                DatabaseHelper.CreateParameter("@id", entity.Id)
            };

            return (sql, parameters);
        }

        #endregion
    }
}
