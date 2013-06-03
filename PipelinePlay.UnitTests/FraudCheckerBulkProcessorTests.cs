namespace PipelinePlay.UnitTests
{
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PipelinePlay.Interfaces;
    using PipelinePlay.UnitTests.Helpers;

    [TestClass]
    public class FraudCheckerBulkProcessorTests
    {
        [TestMethod]
        public void IntegrationTestProcessBatch()
        {
            var orders = new List<IDummyOrderObject>();
            for (var i = 0; i < 6; i++)
            {
                var order = new Mock<IDummyOrderObject>();
                order.Setup(s => s.OrderNumber).Returns(i.ToString(CultureInfo.InvariantCulture));
                orders.Add(order.Object);
            }

            var repo = new Mock<IFraudRepository>();
            var mockRuleFailing = RuleGenerators.FetchMockedRule(true);
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);
            repo
                .SetupSequence(m => m.FetchAllRules())
                .Returns(new List<IFraudRule> { mockRuleFailing.Object })
                .Returns(new List<IFraudRule> { mockRulePassing.Object })
                .Returns(new List<IFraudRule> { mockRuleFailing.Object })
                .Returns(new List<IFraudRule> { mockRulePassing.Object })
                .Returns(new List<IFraudRule> { mockRuleFailing.Object })
                .Returns(new List<IFraudRule> { mockRulePassing.Object });

            var bulkProcessor = new FraudCheckerBulkProcessor(repo.Object);
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