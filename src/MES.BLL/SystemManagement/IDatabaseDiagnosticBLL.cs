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
        /// 测试数据库连接并返回诊断详情
        /// </summary>
        /// <returns>诊断详情字符串</returns>
        string TestConnectionWithDetails();
    }
}

