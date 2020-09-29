namespace Lexiconn.Pipeline.Interfaces
{
    public interface IRuleValidationResult
    {
        bool InError { get; set; }

        string Message { get; set; }
    }
}