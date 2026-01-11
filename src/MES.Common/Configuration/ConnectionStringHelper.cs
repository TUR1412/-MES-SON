using System;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace MES.Common.Configuration
{
    /// <summary>
    /// 连接字符串工具：
    /// - 兼容 MySQL 8/9 默认认证 + 非 SSL 场景（AllowPublicKeyRetrieval）
    /// - 对外显示时脱敏（避免在 UI/日志中泄露真实密码）
    /// </summary>
    public static class ConnectionStringHelper
    {
        private const string AllowPublicKeyRetrievalKey = "AllowPublicKeyRetrieval";

        public static string EnsureAllowPublicKeyRetrieval(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return connectionString;
            }

            if (connectionString.IndexOf(AllowPublicKeyRetrievalKey, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return connectionString;
            }

            // 仅在显式禁用 SSL 时追加（避免无意改变其他安全策略）
            if (connectionString.IndexOf("SslMode=none", StringComparison.OrdinalIgnoreCase) < 0 &&
                connectionString.IndexOf("Ssl Mode=none", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return connectionString;
            }

            if (!connectionString.TrimEnd().EndsWith(";"))
            {
                connectionString += ";";
            }

            return connectionString + "AllowPublicKeyRetrieval=true;";
        }

        public static string MaskSecrets(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return string.Empty;
            }

            try
            {
                var builder = new MySqlConnectionStringBuilder(connectionString);
                if (!string.IsNullOrEmpty(builder.Password))
                {
                    builder.Password = "******";
                }

                return builder.ConnectionString;
            }
            catch
            {
                try
                {
                    return Regex.Replace(
                        connectionString,
                        @"(?i)(password|pwd)\\s*=\\s*([^;]*)",
                        m => m.Groups[1].Value + "=******");
                }
                catch
                {
                    return connectionString;
                }
            }
        }

        /// <summary>
        /// 在任意文本中脱敏敏感字段（例如日志/异常文本中的 Password/Pwd 片段）。
        /// 与 <see cref="MaskSecrets"/> 不同：该方法不会尝试“解析连接字符串”，仅做文本级替换，避免破坏上下文结构。
        /// </summary>
        public static string MaskSecretsInText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            try
            {
                return Regex.Replace(
                    text,
                    @"(?i)(password|pwd)\s*=\s*([^;\r\n]*)",
                    m => m.Groups[1].Value + "=******");
            }
            catch
            {
                return text;
            }
        }
    }
}

