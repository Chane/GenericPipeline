namespace PipelinePlay.UnitTests.RuleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PipelinePlay.Rules;

    [TestClass]
    public class BaseRuleTests
    {
        [TestMethod]
        public void TestBaseRule()
        {
            // Instantiate the abstract class as a mock to allow testing
            var name = "abc";
            var mock = new Mock<BaseRule>(name);

            Assert.AreEqual(name, mock.Object.Name);
        }
    }
}
