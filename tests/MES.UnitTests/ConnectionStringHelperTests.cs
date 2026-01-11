using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.Common.Configuration;

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
    }
}

