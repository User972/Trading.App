using System;
using Autofac;
using RateMyAgent.Trading.App.Interfaces;
using RateMyAgent.Trading.App.Services;
using Serilog;

namespace RateMyAgent.Trading.App.Setup
{
    public class IoCConfiguration
    {
        public static IContainer Configure(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<TradeProcessor>().As<ITradeProcessor>();
        
            containerBuilder.RegisterType<DataService>().As<IDataService>();
            //TODO : Move definition values to config file
            containerBuilder.Register<ILogger>((c, p) => new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File( $"Logs/Log-{DateTime.Now.Ticks}.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger()).SingleInstance();
            return containerBuilder.Build();
        }
    }
}