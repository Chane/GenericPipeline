namespace PipelinePlay
{
    using System;
    using System.Collections.Generic;
    using PipelinePlay.Interfaces;

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
