using System;
using AppShapes.Logging.Console;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Service
{
    public class ConfigureWebHostLoggingCommand
    {
        public virtual void Execute(IWebHostBuilder builder, Action<ILoggingBuilder> loggingOptions = null)
        {
            builder.ConfigureLogging((context, loggingBuilder) =>
            {
                ConfigureLogging(context, loggingBuilder);
                loggingOptions?.Invoke(loggingBuilder);
            });
        }

        protected virtual void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConfiguration(GetLoggingConfiguration(context.Configuration));
            logging.AddConsoleLogger();
            logging.AddDebug();
        }

        protected virtual IConfiguration GetLoggingConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("Logging");
        }
    }
}