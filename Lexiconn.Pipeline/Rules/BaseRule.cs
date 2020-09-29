namespace Lexiconn.Pipeline.Rules
{
    using Lexiconn.Pipeline.Interfaces;

    public abstract class BaseRule : IRule
    {
        protected BaseRule(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public abstract IRuleValidationResult ValidateRule(ICheckableObject item);
    }
}