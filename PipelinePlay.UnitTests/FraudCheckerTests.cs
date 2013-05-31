namespace PipelinePlay.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PipelinePlay.Interfaces;
    using PipelinePlay.UnitTests.Helpers;

    [TestClass]
    public class FraudCheckerTests
    {
        [TestMethod]
        public void TestFraudCheckerLoadState()
        {
            var mockOrder = new Mock<IDummyOrderObject>();
            var checker = new FraudChecker(mockOrder.Object);
            Assert.IsFalse(checker.IsRulesLoaded);
        }

        [TestMethod]
        public void TestFraudCheckerCanLoadRepository()
        {
            var mockRepo = Mock.Of<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var checker = new FraudChecker(mockOrder.Object, mockRepo);

            Assert.IsTrue(checker.IsRepositoryLoaded);
            Assert.IsFalse(checker.IsRulesLoaded);
        }

        [TestMethod]
        public void TestFraudCheckerCanRunRule()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRule = RuleGenerators.FetchMockedRule();
            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule>{mockRule.Object});

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(1, checker.CountOfRulesRan);
        }

        [TestMethod]
        public void TestFraudCheckerCanRunRules()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRule1 = RuleGenerators.FetchMockedRule("rule 1");
            var mockRule2 = RuleGenerators.FetchMockedRule("rule 2");
            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRule1.Object, mockRule2.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(2, checker.CountOfRulesRan);
        }

        [TestMethod]
        public void TestFraudCheckerCanPassRule()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRule = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRule.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(0, checker.CountOfRulesFailed);
            Assert.AreEqual(1, checker.CountOfRulesPassed);
        }

        [TestMethod]
        public void TestFraudCheckerCanFailRule()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRule.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(1, checker.CountOfRulesFailed);
            Assert.AreEqual(0, checker.CountOfRulesPassed);
        }

        [TestMethod]
        public void TestFraudCheckerCanPassAndFailRule()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRuleInError = RuleGenerators.FetchMockedRule(true);
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRuleInError.Object, mockRulePassing.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(1, checker.CountOfRulesFailed);
            Assert.AreEqual(1, checker.CountOfRulesPassed);
        }

        [TestMethod]
        public void TestFraudCheckerStoresPassingRuleNames()
        {
            var name = "ruleName";
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRule = RuleGenerators.FetchMockedRule(false, name);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRule.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.IsTrue(checker.ReportOfPassingRules.Contains(name));
        }

        [TestMethod]
        public void TestFraudCheckerStoresFailingRuleNames()
        {
            var name = "ruleName";
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true, name);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRule.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.IsTrue(checker.ReportOfFailingRules.ContainsKey(name));
        }

        [TestMethod]
        public void TestFraudCheckerStoresFailingRuleMessages()
        {
            var name = "ruleName";
            var message = "rule failed";
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true, name, message);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRule.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(message, checker.ReportOfFailingRules[name]);
        }

        [TestMethod]
        public void TestFraudCheckerUsesChannelKey()
        {
            var channelKey = "abc";
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();

            mockRepo.Setup(m => m.FetchRulesByChannel(channelKey)).Returns(new List<IFraudRule>());
            mockRepo.Setup(m => m.FetchAllRules()).Throws(new NotImplementedException());

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object, channelKey);
            checker.LoadRules();

            Assert.IsTrue(checker.IsRulesLoaded);
        }

        [TestMethod]
        public void TestFraudCheckerReturnsPassResult()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRulePassing.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            Assert.IsTrue(checker.RunRules());
        }

        [TestMethod]
        public void TestFraudCheckerReturnsFailResult()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRuleFailing = RuleGenerators.FetchMockedRule(true);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRuleFailing.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            Assert.IsFalse(checker.RunRules());
        }

        [TestMethod]
        public void TestReusability()
        {
            var mockRepo = new Mock<IFraudRepository>();
            var mockOrder = new Mock<IDummyOrderObject>();
            var mockRuleFailing = RuleGenerators.FetchMockedRule(true);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IFraudRule> { mockRuleFailing.Object });

            var checker = new FraudChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();
        
            var mockOrder2 = new Mock<IDummyOrderObject>();
            checker.LoadOrder(mockOrder2.Object);
            Assert.AreEqual(0, checker.CountOfRulesFailed);
            Assert.AreEqual(0, checker.CountOfRulesPassed);
            Assert.AreEqual(0, checker.CountOfRulesRan);
            Assert.AreEqual(0, checker.ReportOfFailingRules.Count);
            Assert.AreEqual(0, checker.ReportOfPassingRules.Count);
        }
    }
}
