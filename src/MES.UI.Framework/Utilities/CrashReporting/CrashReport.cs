using System;

namespace MES.UI.Framework.Utilities.CrashReporting
{
    internal sealed class CrashReport
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; }
        public bool IsTerminating { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string EnvironmentInfo { get; set; }
        public string ExceptionText { get; set; }
    }
}

