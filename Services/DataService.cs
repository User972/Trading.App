using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RateMyAgent.Trading.App.Interfaces;
using RateMyAgent.Trading.App.Model;
using Serilog;

namespace RateMyAgent.Trading.App.Services
{
    public class DataService : IDataService
    {
        private readonly ILogger _logger;

        public DataService(ILogger logger)
        {
            _logger = logger;
        }
        public void Write(IEnumerable<TradeRecord> trades)
        {
            using (var connection = new SqlConnection(StaticResources.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var trade in trades)
                        {
                            var command = connection.CreateCommand();
                            command.Transaction = transaction;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "dbo.insert_trade";
                            command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                            command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                            command.Parameters.AddWithValue("@lots", trade.Lots);
                            command.Parameters.AddWithValue("@price", trade.Price);

                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        _logger.Error("DataService Error : {e}", e);
                        transaction.Rollback();
                    }
                    finally { connection.Close(); }
                }
            }
        }
    }
}