namespace Lexiconn.Pipeline.Interfaces
{
    public interface IFraudRule
    {
        string Name { get; set; }

        IRuleValidationResult ValidateRule(IDummyOrderObject order);
    }
}
