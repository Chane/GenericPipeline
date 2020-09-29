namespace Lexiconn.Pipeline
{
    using System.Collections.Generic;
    using Lexiconn.Pipeline.Interfaces;

    public class BulkRuleProcessor
    {
        private IRepository repository;

        public BulkRuleProcessor(IRepository repository)
        {
            this.repository = repository;
        }

        public IDictionary<string, ICheckResult> ProcessBatch(IList<ICheckableObject> batch)
        {
            if (batch == null || batch.Count == 0)
            {
                return null;
            }

            var results = new Dictionary<string, ICheckResult>();
            foreach (var item in batch)
            {
                var checker = new RuleChecker(item, this.repository);
                results.Add(item.Id, checker.RunRules());
            }

            return results;
        }
    }
}