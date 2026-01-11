using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.Common.IO;

namespace MES.UnitTests
{
    [TestClass]
    public class TextFileTailReaderTests
    {
        [TestMethod]
        public void ReadTailText_SmallFile_ReturnsAllLines()
        {
            var path = Path.Combine(Path.GetTempPath(), "mes_tail_" + Guid.NewGuid().ToString("N") + ".log");
            try
            {
                File.WriteAllText(path, "a\r\nb\r\nc\r\n", System.Text.Encoding.UTF8);

                var text = TextFileTailReader.ReadTailText(path, 200);
                Assert.IsTrue(text.IndexOf("a", StringComparison.OrdinalIgnoreCase) >= 0);
                Assert.IsTrue(text.IndexOf("b", StringComparison.OrdinalIgnoreCase) >= 0);
                Assert.IsTrue(text.IndexOf("c", StringComparison.OrdinalIgnoreCase) >= 0);
            }
            finally
            {
                try { if (File.Exists(path)) File.Delete(path); } catch { }
            }
        }

        [TestMethod]
        public void ReadTailText_LargeFile_ReturnsLastLines()
        {
            var path = Path.Combine(Path.GetTempPath(), "mes_tail_" + Guid.NewGuid().ToString("N") + ".log");
            try
            {
                var sb = new System.Text.StringBuilder();
                for (int i = 1; i <= 120; i++)
                {
                    sb.AppendLine(string.Format("第{0}行 - line-{0:000}", i));
                }

                File.WriteAllText(path, sb.ToString(), System.Text.Encoding.UTF8);

                var text = TextFileTailReader.ReadTailText(path, 10, 2048);
                Assert.IsTrue(text.IndexOf("第120行", StringComparison.OrdinalIgnoreCase) >= 0);
                Assert.IsTrue(text.IndexOf("第111行", StringComparison.OrdinalIgnoreCase) >= 0);
                Assert.IsTrue(text.IndexOf("第110行", StringComparison.OrdinalIgnoreCase) < 0);
            }
            finally
            {
                try { if (File.Exists(path)) File.Delete(path); } catch { }
            }
        }
    }
}

