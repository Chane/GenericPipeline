namespace Lexiconn.Pipeline.Interfaces
{
    using System.Collections.Generic;

    public interface IFraudRepository
    {
        IList<IFraudRule> FetchAllRules();

        IList<IFraudRule> FetchRulesByChannel(string channel);
    }
}
