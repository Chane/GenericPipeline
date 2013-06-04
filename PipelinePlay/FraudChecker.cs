namespace PipelinePlay
{
    using System.Collections.Generic;
    using PipelinePlay.Interfaces;

    public class FraudChecker
    {
        private IList<IFraudRule> rules;

        private IFraudRepository repository;

        private IDummyOrderObject order;

        private string channelKey;

        public FraudChecker(IDummyOrderObject order)
        {
            this.IsRulesLoaded = false;
            this.ReportOfPassingRules = new List<string>();
            this.ReportOfFailingRules = new Dictionary<string, string>();
            this.order = order;
        }

        public FraudChecker(IDummyOrderObject order, string channelKey) : this(order)
        {
            this.channelKey = channelKey;
        }

        public FraudChecker(IDummyOrderObject order, IFraudRepository repository) : this(order)
        {
            this.repository = repository;
            this.IsRepositoryLoaded = true;
        }

        public FraudChecker(IDummyOrderObject order, IFraudRepository repository, string channelKey) : this(order, channelKey)
        {
            this.repository = repository;
            this.IsRepositoryLoaded = true;
        }

        internal bool IsRulesLoaded { get; private set; }

        internal bool IsRepositoryLoaded { get; private set; }

        internal int CountOfRulesRan { get; private set; }

        internal int CountOfRulesPassed { get; private set; }

        internal int CountOfRulesFailed { get; private set; }

        internal IList<string> ReportOfPassingRules { get; private set; }

        internal IDictionary<string, string> ReportOfFailingRules { get; private set; }

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

        public void LoadOrder(IDummyOrderObject newOrder)
        {
            this.order = newOrder;
            this.ResetState();
        }

        internal void LoadRules()
        {
            if (!this.IsRepositoryLoaded)
            {
                this.LoadRepository();
            }

            if (string.IsNullOrWhiteSpace(this.channelKey))
            {
                this.rules = this.repository.FetchAllRules();
            }
            else
            {
                this.rules = this.repository.FetchRulesByChannel(this.channelKey);
            }

            this.IsRulesLoaded = true;
        }

        private void LoadRepository()
        {
            this.repository = new FraudRepository();
            this.IsRepositoryLoaded = true;
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
