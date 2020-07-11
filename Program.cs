using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using RateMyAgent.Trading.App.Interfaces;
using RateMyAgent.Trading.App.Services;
using RateMyAgent.Trading.App.Setup;
using Serilog;

namespace RateMyAgent.Trading.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            var container = IoCConfiguration.Configure(cb);
            var logger = container.Resolve<ILogger>();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            StaticResources.ConnectionString = configuration.GetConnectionString("DefaultConnection");
            
            Console.WriteLine("Please enter a valid file name to be processed for Trade data :");
            var fileName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                logger.Error("No file path provided.");
                Console.WriteLine("Please provide a file path next time.");
                Console.ReadKey();
                return;
            }
           
            if (!File.Exists(fileName))
            {
                logger.Error($"File {fileName} does not exist");
                
                Console.WriteLine("Next time, Please provide a file path which actually exists.");
                Console.ReadKey();
                return;
            }

            try
            {
                using (var fs = File.OpenRead(fileName))
                {
                    using (var scope = container.BeginLifetimeScope())
                    {
                        (scope.Resolve<ITradeProcessor>()).ProcessTrades(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception occurred : {ex}", ex);
            }
            finally
            {
                Console.WriteLine("Trade processor run finished. Thanks.");
                Console.ReadKey();
            }
        }
    }
}
