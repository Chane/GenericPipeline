namespace PipelinePlay.UnitTests.Helpers
{
    using Moq;
    using PipelinePlay.Interfaces;

    internal static class RuleGenerators
    {
        internal static Mock<IFraudRule> FetchMockedRule()
        {
            return FetchMockedRule(true);
        }

        internal static Mock<IFraudRule> FetchMockedRule(string ruleName)
        {
            return FetchMockedRule(true, ruleName);
        }

        internal static Mock<IFraudRule> FetchMockedRule(bool ruleState)
        {
            return FetchMockedRule(ruleState, "abc");
        }

        internal static Mock<IFraudRule> FetchMockedRule(bool ruleState, string ruleName)
        {
            return FetchMockedRule(ruleState, ruleName, "default message");
        }

        internal static Mock<IFraudRule> FetchMockedRule(bool ruleState, string ruleName, string message)
        {
            var mockResult = new Mock<IRuleValidationResult>();
            mockResult.Setup(m => m.InError).Returns(ruleState);
            mockResult.Setup(m => m.Message).Returns(message);

            var mock = new Mock<IFraudRule>();
            mock.Setup(m => m.ValidateRule(It.IsAny<IDummyOrderObject>())).Returns(mockResult.Object);
            mock.Setup(m => m.Name).Returns(ruleName);
            return mock;
        }
    }
}
