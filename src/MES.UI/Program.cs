using System;
using System.Windows.Forms;
using MES.Common.Logging;
using MES.UI.Forms;

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

                // 设置全局异常处理
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                // 启动主窗体
                Application.Run(new MainForm());

                LogManager.Info("MES系统正常退出");
            }
            catch (Exception ex)
            {
                LogManager.Error("系统启动失败", ex);
                MessageBox.Show($"系统启动失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogManager.Error("UI线程异常", e.Exception);
            MessageBox.Show($"系统发生错误：{e.Exception.Message}", "错误", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 处理非UI线程异常
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                LogManager.Error("应用程序域异常", ex);
                MessageBox.Show($"系统发生严重错误：{ex.Message}", "严重错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
