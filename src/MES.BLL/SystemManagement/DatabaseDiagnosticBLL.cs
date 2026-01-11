using MES.DAL.Core;

namespace MES.BLL.SystemManagement
{
    /// <summary>
    /// 数据库诊断业务实现：封装 DAL 的连接检测能力，避免 UI 直接依赖 DAL。
    /// </summary>
    public class DatabaseDiagnosticBLL : IDatabaseDiagnosticBLL
    {
        public bool TestConnection()
        {
            return DatabaseHelper.TestConnection();
        }

        public bool TestConnection(string connectionString)
        {
            return DatabaseHelper.TestConnection(connectionString);
        }

        public string TestConnectionWithDetails()
        {
            return DatabaseHelper.TestConnectionWithDetails();
        }

        public string TestConnectionWithDetails(string connectionString)
        {
            return DatabaseHelper.TestConnectionWithDetails(connectionString);
        }
    }
}

