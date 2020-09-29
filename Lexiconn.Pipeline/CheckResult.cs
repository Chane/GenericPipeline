namespace Lexiconn.Pipeline
{
    using System.Collections.Generic;
    using Lexiconn.Pipeline.Interfaces;

    public class CheckResult : ICheckResult
    {
        public CheckResult(bool overallResult, int rulesRan, int rulesPassed, int rulesFailed, IList<string> passingReport, IDictionary<string, string> failingReport)
        {
            this.OverallResult = overallResult;
            this.RulesRan = rulesRan;
            this.RulesPassed = rulesPassed;
            this.RulesFailed = rulesFailed;
            this.ReportOfFailingRules = failingReport;
            this.ReportOfPassingRules = passingReport;
        }

        public bool OverallResult { get; private set; }

        public int RulesRan { get; private set; }

        public int RulesPassed { get; private set; }

        public int RulesFailed { get; private set; }

        public IList<string> ReportOfPassingRules { get; private set; }

        public IDictionary<string, string> ReportOfFailingRules { get; private set; }
    }
}