using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
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
                // 🚀 启用VS2022级别的详细调试 - 完全自动化！
                EnableFusionLogging();
                SetupAutomaticDebugCapture();

                // 启用应用程序视觉样式
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 初始化日志系统
                LogManager.Initialize();
                LogManager.Info("MES系统启动");

                // 测试调试输出
                Console.WriteLine("=== MES系统控制台输出测试 ===");
                Debug.WriteLine("=== MES系统调试输出测试 ===");
                Trace.WriteLine("=== MES系统跟踪输出测试 ===");

                // 设置全局异常处理
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                // 启动英雄联盟风格主窗体
                Application.Run(new MainFormLeague());

                LogManager.Info("MES系统正常退出");
            }
            catch (Exception ex)
            {
                LogManager.Error("系统启动失败", ex);
                MessageBox.Show(string.Format("系统启动失败：{0}", ex.Message), "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 🚀 启用Fusion日志 - VS2022级别的程序集绑定详情
        /// </summary>
        private static void EnableFusionLogging()
        {
            try
            {
                // 创建Fusion日志目录
                var fusionLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FusionLogs");
                if (!Directory.Exists(fusionLogPath))
                {
                    Directory.CreateDirectory(fusionLogPath);
                }

                // 启用Fusion日志注册表设置
                using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Fusion"))
                {
                    if (key != null)
                    {
                        key.SetValue("EnableLog", 1, RegistryValueKind.DWord);
                        key.SetValue("ForceLog", 1, RegistryValueKind.DWord);
                        key.SetValue("LogFailures", 1, RegistryValueKind.DWord);
                        key.SetValue("LogResourceBinds", 1, RegistryValueKind.DWord);
                        key.SetValue("LogPath", fusionLogPath, RegistryValueKind.String);
                    }
                }

                Console.WriteLine("✅ Fusion日志已启用，程序集绑定详情将记录到: " + fusionLogPath);
            }
            catch (Exception ex)
            {
                // Fusion日志启用失败不影响应用程序运行
                Console.WriteLine("⚠️ Fusion日志启用失败（可能需要管理员权限）: " + ex.Message);
            }
        }

        /// <summary>
        /// 🚀 设置自动化调试输出捕获 - 完全自动化！
        /// </summary>
        private static void SetupAutomaticDebugCapture()
        {
            try
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug_output.log");

                // 创建文件流（覆盖模式，避免混乱）
                var fileStream = new FileStream(logPath, FileMode.Create, FileAccess.Write, FileShare.Read);
                var streamWriter = new StreamWriter(fileStream);
                streamWriter.AutoFlush = true;

                // 重定向Console输出到文件
                Console.SetOut(streamWriter);
                Console.SetError(streamWriter);

                // 添加Debug监听器
                Debug.Listeners.Clear();
                Debug.Listeners.Add(new TextWriterTraceListener(streamWriter));
                Debug.AutoFlush = true;

                // 添加Trace监听器
                Trace.Listeners.Clear();
                Trace.Listeners.Add(new TextWriterTraceListener(streamWriter));
                Trace.AutoFlush = true;

                // 🚀 启用详细的.NET运行时调试信息
                AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
                {
                    streamWriter.WriteLine(string.Format("[程序集加载] {0}", args.LoadedAssembly.FullName));
                };

                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    streamWriter.WriteLine(string.Format("[程序集解析] 请求: {0}", args.Name));
                    return null;
                };

                AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
                {
                    streamWriter.WriteLine(string.Format("[首次异常] {0}: {1}", args.Exception.GetType().Name, args.Exception.Message));

                    // 🚀 VS2022级别的详细异常信息
                    streamWriter.WriteLine(string.Format("[异常类型] {0}", args.Exception.GetType().FullName));
                    streamWriter.WriteLine(string.Format("[异常源] {0}", args.Exception.Source ?? "未知"));
                    streamWriter.WriteLine(string.Format("[目标站点] {0}", args.Exception.TargetSite != null ? args.Exception.TargetSite.ToString() : "未知"));

                    if (args.Exception.StackTrace != null)
                    {
                        streamWriter.WriteLine("[完整堆栈跟踪]");
                        streamWriter.WriteLine(args.Exception.StackTrace);
                    }

                    // 递归显示内部异常
                    var innerEx = args.Exception.InnerException;
                    int level = 1;
                    while (innerEx != null)
                    {
                        streamWriter.WriteLine(string.Format("[内部异常 {0}] {1}: {2}", level, innerEx.GetType().Name, innerEx.Message));
                        if (innerEx.StackTrace != null)
                        {
                            streamWriter.WriteLine(string.Format("[内部异常 {0} 堆栈]", level));
                            streamWriter.WriteLine(innerEx.StackTrace);
                        }
                        innerEx = innerEx.InnerException;
                        level++;
                    }

                    streamWriter.WriteLine("".PadRight(80, '-'));
                };

                // 记录详细的启动信息
                var startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                streamWriter.WriteLine("=".PadRight(80, '='));
                streamWriter.WriteLine(string.Format("MES系统详细调试日志 - {0}", startTime));
                streamWriter.WriteLine("=".PadRight(80, '='));
                streamWriter.WriteLine(string.Format("[系统信息] OS: {0}", Environment.OSVersion));
                streamWriter.WriteLine(string.Format("[系统信息] .NET版本: {0}", Environment.Version));
                streamWriter.WriteLine(string.Format("[系统信息] 工作目录: {0}", Environment.CurrentDirectory));
                streamWriter.WriteLine(string.Format("[系统信息] 应用程序域: {0}", AppDomain.CurrentDomain.FriendlyName));
                streamWriter.WriteLine(string.Format("[系统信息] 进程ID: {0}", Process.GetCurrentProcess().Id));
                streamWriter.WriteLine("=".PadRight(80, '='));
                streamWriter.Flush();

                Console.WriteLine("✅ 自动化调试输出捕获已启动！");
                Debug.WriteLine("✅ Debug输出捕获已启动！");
                Trace.WriteLine("✅ Trace输出捕获已启动！");
            }
            catch (Exception ex)
            {
                // 如果自动捕获失败，至少要记录到默认日志
                MessageBox.Show(string.Format("调试输出捕获设置失败：{0}", ex.Message), "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogManager.Error("UI线程异常", e.Exception);
            MessageBox.Show(string.Format("系统发生错误：{0}", e.Exception.Message), "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 处理非UI线程异常
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                LogManager.Error("应用程序域异常", ex);
                MessageBox.Show(string.Format("系统发生严重错误：{0}", ex.Message), "严重错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
