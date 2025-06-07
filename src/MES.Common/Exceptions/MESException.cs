using System;

namespace MES.Common.Exceptions
{
    /// <summary>
    /// MES系统自定义异常类
    /// </summary>
    public class MESException : Exception
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 错误模块
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MESException() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        public MESException(string message) : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        public MESException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <param name="message">错误消息</param>
        public MESException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <param name="message">错误消息</param>
        /// <param name="module">错误模块</param>
        public MESException(string errorCode, string message, string module) : base(message)
        {
            ErrorCode = errorCode;
            Module = module;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <param name="message">错误消息</param>
        /// <param name="module">错误模块</param>
        /// <param name="innerException">内部异常</param>
        public MESException(string errorCode, string message, string module, Exception innerException) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            Module = module;
        }

        /// <summary>
        /// 获取完整的错误信息
        /// </summary>
        /// <returns>完整的错误信息</returns>
        public override string ToString()
        {
            var result = base.ToString();
            
            if (!string.IsNullOrEmpty(ErrorCode))
            {
                result = string.Format("错误代码: {0}\n", ErrorCode) + result;
            }
            
            if (!string.IsNullOrEmpty(Module))
            {
                result = string.Format("错误模块: {0}\n", Module) + result;
            }
            
            return result;
        }
    }

    /// <summary>
    /// 数据访问异常
    /// </summary>
    public class DataAccessException : MESException
    {
        public DataAccessException(string message) : base("DAL001", message, "数据访问层")
        {
        }

        public DataAccessException(string message, Exception innerException) 
            : base("DAL001", message, "数据访问层", innerException)
        {
        }
    }

    /// <summary>
    /// 业务逻辑异常
    /// </summary>
    public class BusinessLogicException : MESException
    {
        public BusinessLogicException(string message) : base("BLL001", message, "业务逻辑层")
        {
        }

        public BusinessLogicException(string message, Exception innerException) 
            : base("BLL001", message, "业务逻辑层", innerException)
        {
        }
    }

    /// <summary>
    /// 验证异常
    /// </summary>
    public class ValidationException : MESException
    {
        public ValidationException(string message) : base("VAL001", message, "数据验证")
        {
        }

        public ValidationException(string fieldName, string message) 
            : base("VAL001", string.Format("字段 '{0}' 验证失败: {1}", fieldName, message), "数据验证")
        {
        }
    }
}
