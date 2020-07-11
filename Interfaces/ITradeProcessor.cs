using System.IO;

namespace RateMyAgent.Trading.App.Interfaces
{
    public interface ITradeProcessor
    {
        void ProcessTrades(Stream stream);
        bool IsValid(int lineCount, string[] fields, out int tradeAmount, out decimal tradePrice);
    }
}