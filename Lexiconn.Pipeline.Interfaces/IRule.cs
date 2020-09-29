namespace Lexiconn.Pipeline.Interfaces
{
    public interface IRule
    {
        string Name { get; set; }

        IRuleValidationResult ValidateRule(ICheckableObject order);
    }
}