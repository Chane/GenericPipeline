namespace Lexiconn.Pipeline.UnitTests.Helpers
{
    using Moq;
    using Lexiconn.Pipeline.Interfaces;

    internal static class RuleGenerators
    {
        internal static Mock<IRule> FetchFailingRule()
        {
            return FetchMockedRule(true);
        }

        internal static Mock<IRule> FetchPassingRule()
        {
            return FetchMockedRule(false);
        }

        internal static Mock<IRule> FetchMockedRule()
        {
            return FetchMockedRule(true);
        }

        internal static Mock<IRule> FetchMockedRule(string ruleName)
        {
            return FetchMockedRule(true, ruleName);
        }

        internal static Mock<IRule> FetchMockedRule(bool ruleState)
        {
            return FetchMockedRule(ruleState, "abc");
        }

        internal static Mock<IRule> FetchMockedRule(bool ruleState, string ruleName)
        {
            return FetchMockedRule(ruleState, ruleName, "default message");
        }

        internal static Mock<IRule> FetchMockedRule(bool ruleState, string ruleName, string message)
        {
            var mockResult = new Mock<IRuleValidationResult>();
            mockResult.Setup(m => m.InError).Returns(ruleState);
            mockResult.Setup(m => m.Message).Returns(message);

            var mock = new Mock<IRule>();
            mock.Setup(m => m.ValidateRule(It.IsAny<ICheckableObject>())).Returns(mockResult.Object);
            mock.Setup(m => m.Name).Returns(ruleName);
            return mock;
        }
    }
}