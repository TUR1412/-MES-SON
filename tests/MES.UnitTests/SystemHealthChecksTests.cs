using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.UI.Forms.SystemManagement;

namespace MES.UnitTests
{
    [TestClass]
    public class SystemHealthChecksTests
    {
        private sealed class CustomProbe : IHealthCheckProbe
        {
            public void Collect(List<HealthCheckResult> list, HealthCheckOptions options, HealthCheckContext context)
            {
                list.Add(new HealthCheckResult
                {
                    Item = "custom_probe",
                    Status = "✓",
                    Detail = "ok",
                    Severity = HealthCheckSeverity.Ok
                });
            }
        }

        [TestMethod]
        public void RenderText_ShouldContainHeaderAndItems()
        {
            var results = new List<HealthCheckResult>
            {
                new HealthCheckResult
                {
                    Item = "X",
                    Status = "✓",
                    Detail = "Y",
                    Severity = HealthCheckSeverity.Ok
                }
            };

            var text = SystemHealthChecks.RenderText(results);

            Assert.IsFalse(string.IsNullOrWhiteSpace(text));
            Assert.IsTrue(text.IndexOf("MES System Health Check") >= 0);
            Assert.IsTrue(text.IndexOf("X") >= 0);
        }

        [TestMethod]
        public void CollectWithProbes_ShouldReturnBasicItems()
        {
            var options = new HealthCheckOptions
            {
                IncludeDatabaseConnectivity = false,
                IncludeRecentCrashIndicator = false
            };

            var results = SystemHealthChecks.CollectWithProbes(options, null);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Any(r => r != null && r.Item == "应用版本"));
        }

        [TestMethod]
        public void CollectWithProbes_AdditionalProbe_ShouldBeIncluded()
        {
            var options = new HealthCheckOptions
            {
                IncludeDatabaseConnectivity = false,
                IncludeRecentCrashIndicator = false
            };

            var results = SystemHealthChecks.CollectWithProbes(options, new[] { new CustomProbe() });

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any(r => r != null && r.Item == "custom_probe"));
        }
    }
}
