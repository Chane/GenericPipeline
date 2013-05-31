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

        public bool IsRulesLoaded { get; private set; }

        public bool IsRepositoryLoaded { get; private set; }

        public int CountOfRulesRan { get; private set; }

        public int CountOfRulesPassed { get; private set; }

        public int CountOfRulesFailed { get; private set; }

        public IList<string> ReportOfPassingRules { get; private set; }

        public IDictionary<string, string> ReportOfFailingRules { get; private set; }

        public void LoadRules()
        {
            if (!this.IsRepositoryLoaded)
            {
                LoadRepository();
                
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

        public bool RunRules()
        {
            this.ResetState();
            if (!this.IsRulesLoaded)
            {
                LoadRules();
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

            return this.CountOfRulesFailed == 0;
        }

        public void LoadOrder(IDummyOrderObject newOrder)
        {
            this.order = newOrder;
            this.ResetState();
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
