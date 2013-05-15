namespace PipelinePlay.Interfaces
{
    using System.Collections.Generic;

    public interface IFraudRepository
    {
        IList<IFraudRule> FetchAllRules();
        IList<IFraudRule> FetchRulesByChannel(string channel);
    }
}
