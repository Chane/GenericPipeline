namespace PipelinePlay.Interfaces
{
    using System.Collections.Generic;

    public interface ICheckResult
    {
        bool OverallResult { get; }
        int RulesRan { get; }
        int RulesPassed { get; }
        int RulesFailed { get; }
        IList<string> ReportOfPassingRules { get; }
        IDictionary<string, string> ReportOfFailingRules { get; }
    }
}
