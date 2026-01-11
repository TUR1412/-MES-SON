using Microsoft.VisualStudio.TestTools.UnitTesting;
using MES.UI.Framework.Utilities.Search;

namespace MES.UnitTests
{
    [TestClass]
    public class CommandPaletteSearchTests
    {
        [TestMethod]
        public void Score_ExactMatch_ShouldBeHigh()
        {
            var scoreExact = CommandPaletteSearch.Score("系统配置", "主题切换", "system settings theme", "系统配置");
            var scoreKeyword = CommandPaletteSearch.Score("系统配置", "主题切换", "system settings theme", "settings");

            Assert.IsTrue(scoreExact > 0);
            Assert.IsTrue(scoreKeyword > 0);
            Assert.IsTrue(scoreExact > scoreKeyword);
        }

        [TestMethod]
        public void Score_MultiToken_AllTokensMustMatch()
        {
            var scoreOk = CommandPaletteSearch.Score("数据库诊断", "连接检测", "mysql diagnostic", "数据库 诊断");
            var scoreBad = CommandPaletteSearch.Score("数据库诊断", "连接检测", "mysql diagnostic", "数据库 不存在");

            Assert.IsTrue(scoreOk > 0);
            Assert.AreEqual(0, scoreBad);
        }

        [TestMethod]
        public void FuzzyScore_Unmatched_ReturnsMinusOne()
        {
            var score = CommandPaletteSearch.FuzzyScore("database diagnostic", "zzz");
            Assert.AreEqual(-1, score);
        }
    }
}
