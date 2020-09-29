namespace Lexiconn.Pipeline.Interfaces
{
    using System.Collections.Generic;

    public interface IRepository
    {
        IList<IRule> FetchAllRules();

        IList<IRule> FetchRulesByFilter(string filter);
    }
}