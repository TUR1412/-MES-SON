namespace MES.BLL.SystemManagement
{
    /// <summary>
    /// 数据库诊断业务接口（提供给 UI 使用）
    /// </summary>
    public interface IDatabaseDiagnosticBLL
    {
        /// <summary>
        /// 测试数据库是否可连接
        /// </summary>
        /// <returns>可连接返回 true，否则返回 false</returns>
        bool TestConnection();

        /// <summary>
        /// 测试指定连接字符串是否可连接（用于 UI“配置/诊断”场景）
        /// </summary>
        bool TestConnection(string connectionString);

        /// <summary>
        /// 测试数据库连接并返回诊断详情
        /// </summary>
        /// <returns>诊断详情字符串</returns>
        string TestConnectionWithDetails();

        /// <summary>
        /// 测试指定连接字符串并返回诊断详情（默认脱敏）
        /// </summary>
        string TestConnectionWithDetails(string connectionString);
    }
}

