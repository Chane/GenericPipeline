namespace Lexiconn.Pipeline
{
    using System.Collections.Generic;
    using Lexiconn.Pipeline.Interfaces;

    public class RuleChecker
    {
        private IList<IRule> rules;

        private IRepository repository;

        private ICheckableObject order;

        private string channelKey;

        public RuleChecker(ICheckableObject order)
        {
            this.IsRulesLoaded = false;
            this.ReportOfPassingRules = new List<string>();
            this.ReportOfFailingRules = new Dictionary<string, string>();
            this.order = order;
        }

        public RuleChecker(ICheckableObject order, string channelKey) : this(order)
        {
            this.channelKey = channelKey;
        }

        public RuleChecker(ICheckableObject order, IRepository repository) : this(order)
        {
            this.repository = repository;
            this.IsRepositoryLoaded = true;
        }

        public RuleChecker(ICheckableObject order, IRepository repository, string channelKey) : this(order, channelKey)
        {
            this.repository = repository;
            this.IsRepositoryLoaded = true;
        }

        public bool IsRulesLoaded { get; private set; }

        public bool IsRepositoryLoaded { get; private set; }

        public int CountOfRulesRan { get; private set; }

        public int CountOfRulesPassed { get; private set; }

        public int CountOfRulesFailed { get; private set; }

        public IList<string> ReportOfPassingRules { get; private set; }

        public IDictionary<string, string> ReportOfFailingRules { get; private set; }

        public ICheckResult RunRules()
        {
            this.ResetState();
            if (!this.IsRulesLoaded)
            {
                this.LoadRules();
            }

            foreach (var rule in this.rules)
            {
                this.CountOfRulesRan++;
                var validationResult = rule.ValidateRule(this.order);
                if (validationResult.InError)
                {
                    this.CountOfRulesFailed++;
                    this.ReportOfFailingRules.Add(rule.Name, validationResult.Message);
                }
                else
                {
                    this.CountOfRulesPassed++;
                    this.ReportOfPassingRules.Add(rule.Name);
                }
            }

            var result = this.CountOfRulesFailed == 0;
            return new CheckResult(result, this.CountOfRulesRan, this.CountOfRulesPassed, this.CountOfRulesFailed, this.ReportOfPassingRules, this.ReportOfFailingRules);
        }

        public void LoadOrder(ICheckableObject newOrder)
        {
            this.order = newOrder;
            this.ResetState();
        }

        public void LoadRules()
        {
            if (string.IsNullOrWhiteSpace(this.channelKey))
            {
                this.rules = this.repository.FetchAllRules();
            }
            else
            {
                this.rules = this.repository.FetchRulesByFilter(this.channelKey);
            }

            this.IsRulesLoaded = true;
        }

        private void ResetState()
        {
            this.CountOfRulesRan = 0;
            this.CountOfRulesFailed = 0;
            this.CountOfRulesPassed = 0;
            this.ReportOfPassingRules.Clear();
            this.ReportOfFailingRules.Clear();
        }
    }
}
