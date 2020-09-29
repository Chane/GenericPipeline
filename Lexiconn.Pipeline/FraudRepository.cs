namespace Lexiconn.Pipeline
{
    using System;
    using System.Collections.Generic;
    using Lexiconn.Pipeline.Interfaces;

    public class FraudRepository : IFraudRepository
    {
        public IList<IFraudRule> FetchAllRules()
        {
            throw new NotImplementedException();
        }

        public IList<IFraudRule> FetchRulesByChannel(string channel)
        {
            throw new NotImplementedException();
        }
    }
}
