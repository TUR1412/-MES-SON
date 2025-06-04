namespace MES.Common.Logging
{
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试级别 - 详细的调试信息
        /// </summary>
        Debug = 0,

        /// <summary>
        /// 信息级别 - 一般的信息性消息
        /// </summary>
        Info = 1,

        /// <summary>
        /// 警告级别 - 警告信息，不影响程序运行
        /// </summary>
        Warning = 2,

        /// <summary>
        /// 错误级别 - 错误信息，可能影响功能
        /// </summary>
        Error = 3,

        /// <summary>
        /// 致命级别 - 严重错误，可能导致程序崩溃
        /// </summary>
        Fatal = 4
    }
}
