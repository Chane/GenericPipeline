namespace PipelinePlay.Rules
{
    using PipelinePlay.Interfaces;

    public abstract class BaseRule : IFraudRule
    {
        protected BaseRule(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public abstract IRuleValidationResult ValidateRule(IDummyOrderObject order);
    }
}
