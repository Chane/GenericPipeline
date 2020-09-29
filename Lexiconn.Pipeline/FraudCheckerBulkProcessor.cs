namespace Lexiconn.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Lexiconn.Pipeline.Interfaces;

    public class FraudCheckerBulkProcessor
    {
        private IFraudRepository repository;

        public FraudCheckerBulkProcessor()
        {
            this.repository = new FraudRepository();
        }

        public FraudCheckerBulkProcessor(IFraudRepository specificRepository)
        {
            this.repository = specificRepository;
        }

        public IDictionary<string, ICheckResult> ProcessBatch(IList<IDummyOrderObject> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return null;
            }

            var results = new Dictionary<string, ICheckResult>();
            foreach (var order in orders)
            {
                var checker = new FraudChecker(order, this.repository);
                results.Add(order.OrderNumber, checker.RunRules());
            }

            return results;
        }
    }
}
