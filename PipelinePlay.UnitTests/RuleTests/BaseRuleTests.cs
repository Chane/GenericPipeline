namespace PipelinePlay.UnitTests.RuleTests
{
    using Moq;
    using NUnit.Framework;
    using PipelinePlay.Rules;

    [TestFixture]
    public class BaseRuleTests
    {
        [Test]
        public void TestBaseRule()
        {
            // Instantiate the abstract class as a mock to allow testing
            var name = "abc";
            var mock = new Mock<BaseRule>(name);

            Assert.AreEqual(name, mock.Object.Name);
        }
    }
}
