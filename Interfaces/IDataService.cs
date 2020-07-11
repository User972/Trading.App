using System.Collections.Generic;
using RateMyAgent.Trading.App.Model;

namespace RateMyAgent.Trading.App.Interfaces
{
    public interface IDataService
    {
        void Write(IEnumerable<TradeRecord> trades);
    }
}