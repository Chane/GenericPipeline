namespace Lexiconn.Pipeline.UnitTests
{
    using System.Collections.Generic;
    using System.Globalization;
    using Moq;
    using NUnit.Framework;
    using Lexiconn.Pipeline.Interfaces;
    using Lexiconn.Pipeline.UnitTests.Helpers;

    [TestFixture]
    public class BulkRuleProcessorTests
    {
        [Test]
        public void Checkable_Objects_Can_Be_Processed_In_Batch()
        {
            var orders = new List<ICheckableObject>();
            for (var i = 0; i < 6; i++)
            {
                var order = new Mock<ICheckableObject>();
                order.Setup(s => s.Id).Returns(i.ToString(CultureInfo.InvariantCulture));
                orders.Add(order.Object);
            }

            var repo = new Mock<IRepository>();
            var mockRuleFailing = RuleGenerators.FetchMockedRule(true);
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);
            repo
                .SetupSequence(m => m.FetchAllRules())
                .Returns(new List<IRule> { mockRuleFailing.Object })
                .Returns(new List<IRule> { mockRulePassing.Object })
                .Returns(new List<IRule> { mockRuleFailing.Object })
                .Returns(new List<IRule> { mockRulePassing.Object })
                .Returns(new List<IRule> { mockRuleFailing.Object })
                .Returns(new List<IRule> { mockRulePassing.Object });

            var bulkProcessor = new BulkRuleProcessor(repo.Object);
            var report = bulkProcessor.ProcessBatch(orders);

            Assert.AreEqual(6, report.Count);
            Assert.IsFalse(report["0"].OverallResult);
            Assert.IsTrue(report["1"].OverallResult);
            Assert.IsFalse(report["2"].OverallResult);
            Assert.IsTrue(report["3"].OverallResult);
            Assert.IsFalse(report["4"].OverallResult);
            Assert.IsTrue(report["5"].OverallResult);
        }
    }
}