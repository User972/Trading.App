using System.Collections.Generic;
using System.IO;
using RateMyAgent.Trading.App.Interfaces;
using RateMyAgent.Trading.App.Model;
using Serilog;

namespace RateMyAgent.Trading.App.Services
{
    public class TradeProcessor : ITradeProcessor
    {
        private readonly IDataService _svc;
        private readonly ILogger _logger;
        private const float LotSize = 100000f;

        public TradeProcessor(IDataService svc, ILogger logger)
        {
            _svc = svc;
            _logger = logger;
        }
        public void ProcessTrades(Stream stream)
        {
            var trades = new List<TradeRecord>();
            var lineCount = 1;

            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(',');
                    if (!IsValid(lineCount, fields, out var tradeAmount, out var tradePrice))
                    {
                        continue;
                    }

                    var sourceCurrencyCode = fields[0].Substring(0, 3);
                    var destinationCurrencyCode = fields[0].Substring(3, 3);

                    // Calculate values
                    var trade = new TradeRecord
                    {
                        SourceCurrency = sourceCurrencyCode,
                        DestinationCurrency = destinationCurrencyCode,
                        Lots = tradeAmount / LotSize,
                        Price = tradePrice
                    };

                    trades.Add(trade);

                    lineCount++;
                }
                _logger.Information($"Total number of trades in lot is {trades.Count}");
            }
            _svc.Write(trades);

            _logger.Information($"INFO: {trades.Count} trades processed");
        }

        //TODO : Move ot a rules engine kind of setup
        public bool IsValid(int lineCount, string[] fields, out int tradeAmount , out decimal tradePrice)
        {
            tradeAmount = 0;
            tradePrice = 0;
            if (fields.Length != 3)
            {
                _logger.Warning($"WARN: Line {lineCount} malformed. Only {fields.Length} field(s) found.");
                return false;
            }

            if (fields[0].Length != 6)
            {
                _logger.Warning($"WARN: Trade currencies on line {lineCount} malformed: [{fields[0]}]");
                return false;
            }

            if (!int.TryParse(fields[1], out tradeAmount))
            {
                _logger.Warning($"WARN: Trade amount on line {lineCount} is not a valid integer: [{fields[1]}]");
                return false;
            }

            if (!decimal.TryParse(fields[2], out tradePrice))
            {
                _logger.Warning($"WARN: Trade price on line {lineCount} is not a valid decimal: [{fields[1]}]");
                return false;
            }

            if (tradePrice >= 0) 
                return true;

            _logger.Warning($"WARN: Trade price on line {lineCount} should not be negative: [{fields[1]}]");
            return false;

        }
    }
}