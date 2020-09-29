namespace Lexiconn.Pipeline
{
    using System.Collections.Generic;
    using Lexiconn.Pipeline.Interfaces;

    public class BulkRuleProcessor
    {
        private IRepository repository;

        public BulkRuleProcessor(IRepository specificRepository)
        {
            this.repository = specificRepository;
        }

        public IDictionary<string, ICheckResult> ProcessBatch(IList<ICheckableObject> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return null;
            }

            var results = new Dictionary<string, ICheckResult>();
            foreach (var order in orders)
            {
                var checker = new RuleChecker(order, this.repository);
                results.Add(order.Id, checker.RunRules());
            }

            return results;
        }
    }
}
