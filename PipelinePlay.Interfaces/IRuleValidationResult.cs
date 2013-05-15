namespace PipelinePlay.Interfaces
{
    public interface IRuleValidationResult
    {
        bool InError { get; set; }
        string Message { get; set; }
    }
}
