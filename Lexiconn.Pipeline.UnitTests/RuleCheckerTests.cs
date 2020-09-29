namespace Lexiconn.Pipeline.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Lexiconn.Pipeline.Interfaces;
    using Lexiconn.Pipeline.UnitTests.Helpers;

    [TestFixture]
    public class RuleCheckerTests
    {
        [Test]
        public void TestFraudCheckerLoadState()
        {
            var mockOrder = new Mock<ICheckableObject>();
            var checker = new RuleChecker(mockOrder.Object, Mock.Of<IRepository>());
            Assert.IsFalse(checker.IsRulesLoaded);
        }

        [Test]
        public void TestFraudCheckerCanLoadRepository()
        {
            var mockRepo = Mock.Of<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var checker = new RuleChecker(mockOrder.Object, mockRepo);

            Assert.IsTrue(checker.IsRepositoryLoaded);
            Assert.IsFalse(checker.IsRulesLoaded);
        }

        [Test]
        public void TestFraudCheckerCanRunRule()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule();
            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(1, checker.CountOfRulesRan);
        }

        [Test]
        public void TestFraudCheckerCanRunRuleViaReport()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule();
            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(1, report.RulesRan);
        }

        [Test]
        public void TestFraudCheckerCanRunRules()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule1 = RuleGenerators.FetchMockedRule("rule 1");
            var mockRule2 = RuleGenerators.FetchMockedRule("rule 2");
            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule1.Object, mockRule2.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(2, checker.CountOfRulesRan);
        }

        [Test]
        public void TestFraudCheckerCanRunRulesViaReport()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule1 = RuleGenerators.FetchMockedRule("rule 1");
            var mockRule2 = RuleGenerators.FetchMockedRule("rule 2");
            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule1.Object, mockRule2.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(2, report.RulesRan);
        }

        [Test]
        public void TestFraudCheckerCanPassRule()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(0, checker.CountOfRulesFailed);
            Assert.AreEqual(1, checker.CountOfRulesPassed);
        }

        [Test]
        public void TestFraudCheckerCanPassRuleViaReport()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(0, report.RulesFailed);
            Assert.AreEqual(1, report.RulesPassed);
            Assert.AreEqual(report.RulesFailed, checker.CountOfRulesFailed);
            Assert.AreEqual(report.RulesPassed, checker.CountOfRulesPassed);
        }

        [Test]
        public void TestFraudCheckerCanFailRule()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(1, checker.CountOfRulesFailed);
            Assert.AreEqual(0, checker.CountOfRulesPassed);
        }

        [Test]
        public void TestFraudCheckerCanFailRuleViaReport()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(1, report.RulesFailed);
            Assert.AreEqual(0, report.RulesPassed);
            Assert.AreEqual(report.RulesFailed, checker.CountOfRulesFailed);
            Assert.AreEqual(report.RulesPassed, checker.CountOfRulesPassed);
        }

        [Test]
        public void TestFraudCheckerCanPassAndFailRule()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRuleInError = RuleGenerators.FetchMockedRule(true);
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRuleInError.Object, mockRulePassing.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(1, checker.CountOfRulesFailed);
            Assert.AreEqual(1, checker.CountOfRulesPassed);
        }

        [Test]
        public void TestFraudCheckerCanPassAndFailRuleViaReport()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRuleInError = RuleGenerators.FetchMockedRule(true);
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRuleInError.Object, mockRulePassing.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(1, report.RulesFailed);
            Assert.AreEqual(1, report.RulesPassed);
            Assert.AreEqual(report.RulesFailed, checker.CountOfRulesFailed);
            Assert.AreEqual(report.RulesPassed, checker.CountOfRulesPassed);
        }

        [Test]
        public void TestFraudCheckerStoresPassingRuleNames()
        {
            var name = "ruleName";
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(false, name);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.IsTrue(checker.ReportOfPassingRules.Contains(name));
        }

        [Test]
        public void TestFraudCheckerStoresPassingRuleNamesViaReport()
        {
            var name = "ruleName";
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(false, name);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.IsTrue(report.ReportOfPassingRules.Contains(name));
        }

        [Test]
        public void TestFraudCheckerStoresFailingRuleNames()
        {
            var name = "ruleName";
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true, name);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.IsTrue(checker.ReportOfFailingRules.ContainsKey(name));
        }

        [Test]
        public void TestFraudCheckerStoresFailingRuleNamesViaReport()
        {
            var name = "ruleName";
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true, name);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.IsTrue(report.ReportOfFailingRules.ContainsKey(name));
        }

        [Test]
        public void TestFraudCheckerStoresFailingRuleMessages()
        {
            var name = "ruleName";
            var message = "rule failed";
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true, name, message);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(message, checker.ReportOfFailingRules[name]);
        }

        [Test]
        public void TestFraudCheckerStoresFailingRuleMessagesViaReport()
        {
            var name = "ruleName";
            var message = "rule failed";
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchMockedRule(true, name, message);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(message, report.ReportOfFailingRules[name]);
        }

        [Test]
        public void TestFraudCheckerUsesChannelKey()
        {
            var filter = "abc";
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();

            mockRepo.Setup(m => m.FetchRulesByFilter(filter)).Returns(new List<IRule>());
            mockRepo.Setup(m => m.FetchAllRules()).Throws(new NotImplementedException());

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object, filter);
            checker.LoadRules();

            Assert.IsTrue(checker.IsRulesLoaded);
        }

        [Test]
        public void TestFraudCheckerReturnsPassResult()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRulePassing.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();
            Assert.IsTrue(report.OverallResult);
        }

        [Test]
        public void TestFraudCheckerReturnsFailResult()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRuleFailing = RuleGenerators.FetchMockedRule(true);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRuleFailing.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();
            Assert.IsFalse(report.OverallResult);
        }

        [Test]
        public void TestReusability()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRuleFailing = RuleGenerators.FetchMockedRule(true);

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRuleFailing.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            var mockOrder2 = new Mock<ICheckableObject>();
            checker.LoadOrder(mockOrder2.Object);
            Assert.AreEqual(0, checker.CountOfRulesFailed);
            Assert.AreEqual(0, checker.CountOfRulesPassed);
            Assert.AreEqual(0, checker.CountOfRulesRan);
            Assert.AreEqual(0, checker.ReportOfFailingRules.Count);
            Assert.AreEqual(0, checker.ReportOfPassingRules.Count);
        }

        [Test]
        public void TestOrdersInSequence()
        {
            var mockRepo = new Mock<IRepository>();
            var orders = new List<ICheckableObject>();
            for (var i = 0; i < 6; i++)
            {
                orders.Add(new Mock<ICheckableObject>().Object);
            }

            var mockRuleFailing = RuleGenerators.FetchMockedRule(true);
            var mockRulePassing = RuleGenerators.FetchMockedRule(false);
            mockRepo
                .SetupSequence(m => m.FetchAllRules())
                .Returns(new List<IRule> { mockRuleFailing.Object })
                .Returns(new List<IRule> { mockRulePassing.Object })
                .Returns(new List<IRule> { mockRuleFailing.Object })
                .Returns(new List<IRule> { mockRulePassing.Object })
                .Returns(new List<IRule> { mockRuleFailing.Object })
                .Returns(new List<IRule> { mockRulePassing.Object });

            var passed = 0;
            var failed = 0;
            foreach (var order in orders)
            {
                var checker = new RuleChecker(order, mockRepo.Object);
                var report = checker.RunRules();
                if (report.OverallResult)
                {
                    passed++;
                }
                else
                {
                    failed++;
                }
            }

            Assert.AreEqual(3, passed);
            Assert.AreEqual(3, failed);
        }
    }
}
