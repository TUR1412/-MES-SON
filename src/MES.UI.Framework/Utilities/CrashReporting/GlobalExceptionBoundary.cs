using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MES.Common.Logging;

namespace MES.UI.Framework.Utilities.CrashReporting
{
    public static class GlobalExceptionBoundary
    {
        private static readonly object _lockObject = new object();
        private static bool _isRegistered = false;
        private static bool _isHandling = false;

        public static void Register()
        {
            if (_isRegistered) return;

            lock (_lockObject)
            {
                if (_isRegistered) return;

                try
                {
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                    Application.ThreadException += (s, e) =>
                    {
                        Handle(e != null ? e.Exception : null, "UI线程异常", false);
                    };

                    AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    {
                        var ex = e != null ? e.ExceptionObject as Exception : null;
                        Handle(ex, "应用程序域异常", e != null && e.IsTerminating);
                    };

                    TaskScheduler.UnobservedTaskException += (s, e) =>
                    {
                        Handle(e != null ? e.Exception : null, "未观察到的任务异常", false);
                        try
                        {
                            if (e != null) e.SetObserved();
                        }
                        catch
                        {
                            // ignore
                        }
                    };

                    _isRegistered = true;
                }
                catch (Exception ex)
                {
                    try { LogManager.Error("注册全局异常边界失败", ex); } catch { }
                }
            }
        }

        public static void Handle(Exception exception, string source, bool isTerminating)
        {
            if (exception == null) return;

            lock (_lockObject)
            {
                if (_isHandling) return;
                _isHandling = true;
            }

            try
            {
                try { LogManager.Fatal(source ?? "未处理异常", exception); } catch { }

                var reportPath = CrashReportWriter.Write(exception, source, isTerminating);

                try
                {
                    using (var dialog = new CrashReportDialog(source, exception, reportPath, isTerminating))
                    {
                        // 如果有活动窗体，优先作为 owner，避免弹窗跑到后台
                        var owner = GetBestOwnerWindow();
                        if (owner != null)
                        {
                            dialog.ShowDialog(owner);
                        }
                        else
                        {
                            dialog.ShowDialog();
                        }
                    }
                }
                catch
                {
                    try
                    {
                        var message = string.Format("系统发生错误：{0}", exception.Message);
                        MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch
                    {
                        // ignore
                    }
                }

                if (isTerminating)
                {
                    try { Environment.Exit(1); } catch { }
                }
            }
            finally
            {
                lock (_lockObject)
                {
                    _isHandling = false;
                }
            }
        }

        private static IWin32Window GetBestOwnerWindow()
        {
            try
            {
                if (Form.ActiveForm != null) return Form.ActiveForm;
            }
            catch
            {
                // ignore
            }

            try
            {
                foreach (Form open in Application.OpenForms)
                {
                    if (open == null) continue;
                    if (open.Visible && open.WindowState != FormWindowState.Minimized)
                    {
                        return open;
                    }
                }
            }
            catch
            {
                // ignore
            }

            return null;
        }
    }
}

