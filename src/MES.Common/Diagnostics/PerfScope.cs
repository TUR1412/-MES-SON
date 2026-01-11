using System;
using System.Diagnostics;
using MES.Common.Logging;

namespace MES.Common.Diagnostics
{
    /// <summary>
    /// 轻量性能埋点：通过 Stopwatch 记录耗时并输出到日志。
    /// 设计目标：零侵入、低开销、可渐进接入（不改变业务逻辑）。
    /// </summary>
    public sealed class PerfScope : IDisposable
    {
        private readonly string _name;
        private readonly Stopwatch _stopwatch;
        private Exception _error;
        private bool _disposed;

        private PerfScope(string name)
        {
            _name = string.IsNullOrWhiteSpace(name) ? "unnamed" : name.Trim();
            _stopwatch = Stopwatch.StartNew();
        }

        public static PerfScope Start(string name)
        {
            return new PerfScope(name);
        }

        public void Fail(Exception error)
        {
            _error = error;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                _stopwatch.Stop();
                var ms = _stopwatch.ElapsedMilliseconds;

                if (_error == null)
                {
                    LogManager.Info(string.Format("[perf] {0} ok ({1} ms)", _name, ms));
                }
                else
                {
                    LogManager.Warning(string.Format("[perf] {0} fail ({1} ms)", _name, ms), _error);
                }
            }
            catch
            {
                // ignore
            }
        }
    }
}

