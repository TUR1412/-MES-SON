using System;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI
{
    /// <summary>
    /// MES系统主程序入口
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // 启用应用程序视觉样式
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 初始化日志系统
                LogManager.Initialize();
                LogManager.Info("MES系统启动");

                // 显示简单消息框确认程序可以运行
                MessageBox.Show("MES制造执行系统启动成功！\n\n基础框架已就绪，可以开始开发各模块功能。",
                    "MES系统", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogManager.Info("MES系统正常退出");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"系统启动失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
