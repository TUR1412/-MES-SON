using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using MES.Common.Configuration;
using MES.Common.Logging;

namespace MES.UnitTests
{
    [TestClass]
    public class ConnectionStringHelperTests
    {
        [TestMethod]
        public void EnsureAllowPublicKeyRetrieval_OnlyAddsWhenSslModeNone()
        {
            var input = "Server=127.0.0.1;Port=3306;Database=mes;User Id=root;Password=123;SslMode=None;";
            var output = ConnectionStringHelper.EnsureAllowPublicKeyRetrieval(input);

            Assert.IsTrue(output.IndexOf("AllowPublicKeyRetrieval", System.StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [TestMethod]
        public void EnsureAllowPublicKeyRetrieval_DoesNotDuplicate()
        {
            var input = "Server=127.0.0.1;Database=mes;User Id=root;Password=123;SslMode=None;AllowPublicKeyRetrieval=true;";
            var output = ConnectionStringHelper.EnsureAllowPublicKeyRetrieval(input);

            // 不应出现两次关键字
            var first = output.IndexOf("AllowPublicKeyRetrieval", System.StringComparison.OrdinalIgnoreCase);
            var last = output.LastIndexOf("AllowPublicKeyRetrieval", System.StringComparison.OrdinalIgnoreCase);
            Assert.AreEqual(first, last);
        }

        [TestMethod]
        public void MaskSecrets_ShouldHidePassword()
        {
            var input = "Server=127.0.0.1;Database=mes;User Id=root;Password=123456;SslMode=None;";
            var output = ConnectionStringHelper.MaskSecrets(input);

            Assert.IsTrue(output.IndexOf("123456", System.StringComparison.OrdinalIgnoreCase) < 0);
            Assert.IsTrue(output.IndexOf("******", System.StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [TestMethod]
        public void MaskSecretsInText_ShouldHidePasswordAssignments()
        {
            var input = "prefix Password=123; suffix" + Environment.NewLine + "another password = abc; end";
            var output = ConnectionStringHelper.MaskSecretsInText(input);

            Assert.IsTrue(output.IndexOf("123", System.StringComparison.OrdinalIgnoreCase) < 0);
            Assert.IsTrue(output.IndexOf("abc", System.StringComparison.OrdinalIgnoreCase) < 0);
            Assert.IsTrue(output.IndexOf("******", System.StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.IsTrue(output.IndexOf("prefix", System.StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.IsTrue(output.IndexOf("suffix", System.StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [TestMethod]
        public void CrashReportWriter_ShouldIncludeMaskedLogTail()
        {
            LogManager.Initialize();

            var todayLog = LogManager.GetTodayLogFilePath();
            var dir = Path.GetDirectoryName(todayLog);
            if (!string.IsNullOrWhiteSpace(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(todayLog, "hello Password=123;" + Environment.NewLine + "world", Encoding.UTF8);

            var ex = new Exception("boom Password=123;");

            var frameworkAssembly = typeof(MES.UI.Framework.Utilities.CrashReporting.GlobalExceptionBoundary).Assembly;
            var type = frameworkAssembly.GetType("MES.UI.Framework.Utilities.CrashReporting.CrashReportWriter");
            Assert.IsNotNull(type);

            var method = type.GetMethod("Write", BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(method);

            var path = method.Invoke(null, new object[] { ex, "UnitTest", false }) as string;
            Assert.IsFalse(string.IsNullOrWhiteSpace(path));
            Assert.IsTrue(File.Exists(path));

            var report = File.ReadAllText(path, Encoding.UTF8);
            Assert.IsTrue(report.IndexOf("Recent Log Tail", System.StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.IsTrue(report.IndexOf("Password=******", System.StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.IsTrue(report.IndexOf("Password=123", System.StringComparison.OrdinalIgnoreCase) < 0);
            Assert.IsTrue(report.IndexOf("hello", System.StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}

