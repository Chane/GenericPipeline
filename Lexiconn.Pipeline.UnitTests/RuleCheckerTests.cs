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
        public void RuleChecker_Does_Not_Load_Rules_On_Initialisation()
        {
            var mockOrder = new Mock<ICheckableObject>();
            var checker = new RuleChecker(mockOrder.Object, Mock.Of<IRepository>());
            Assert.IsFalse(checker.IsRulesLoaded);
        }

        [Test]
        public void RuleChecker_Loads_Repository_On_Initialisation()
        {
            var mockRepo = Mock.Of<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var checker = new RuleChecker(mockOrder.Object, mockRepo);

            Assert.IsTrue(checker.IsRepositoryLoaded);
            Assert.IsFalse(checker.IsRulesLoaded);
        }

        [Test]
        public void RuleChecker_Will_Run_Rules_From_Repository()
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
        public void RuleChecker_Will_Return_A_Report_With_Number_Of_RulesRan()
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
        public void RuleChecker_Will_Run_Multiple_Rules()
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
        public void RuleChecker_Will_Return_A_Report_With_Number_Of_RulesRan_For_Multiple_Rules()
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
        public void Rule_Checker_Will_Count_Rules_Passed()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchPassingRule();

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(0, checker.CountOfRulesFailed);
            Assert.AreEqual(1, checker.CountOfRulesPassed);
        }

        [Test]
        public void Rule_Checker_Will_Count_Rules_Failed()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchPassingRule();

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            checker.RunRules();

            Assert.AreEqual(1, checker.CountOfRulesFailed);
            Assert.AreEqual(0, checker.CountOfRulesPassed);
        }

        [Test]
        public void RuleChecker_Report_Has_Number_Of_Passes()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchFailingRule();

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(0, report.RulesFailed);
            Assert.AreEqual(1, report.RulesPassed);
            Assert.AreEqual(report.RulesFailed, checker.CountOfRulesFailed);
            Assert.AreEqual(report.RulesPassed, checker.CountOfRulesPassed);
        }

        [Test]
        public void RuleChecker_Report_Has_Number_Of_Fails()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchFailingRule();

            mockRepo.Setup(m => m.FetchAllRules()).Returns(new List<IRule> { mockRule.Object });

            var checker = new RuleChecker(mockOrder.Object, mockRepo.Object);
            var report = checker.RunRules();

            Assert.AreEqual(1, report.RulesFailed);
            Assert.AreEqual(0, report.RulesPassed);
            Assert.AreEqual(report.RulesFailed, checker.CountOfRulesFailed);
            Assert.AreEqual(report.RulesPassed, checker.CountOfRulesPassed);
        }

        [Test]
        public void TestFraudCheckerCanFailRule()
        {
            var mockRepo = new Mock<IRepository>();
            var mockOrder = new Mock<ICheckableObject>();
            var mockRule = RuleGenerators.FetchFailingRule();

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
            var mockRule = RuleGenerators.FetchFailingRule();

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
            var mockRuleInError = RuleGenerators.FetchFailingRule();
            var mockRulePassing = RuleGenerators.FetchPassingRule();

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
            var mockRuleInError = RuleGenerators.FetchFailingRule();
            var mockRulePassing = RuleGenerators.FetchPassingRule();

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