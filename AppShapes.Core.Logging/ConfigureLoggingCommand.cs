using System;
using AppShapes.Logging.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Logging
{
    public class ConfigureLoggingCommand
    {
        public virtual void Execute(IServiceCollection services, IConfiguration configuration, Action<ILoggingBuilder> loggingOptions = null)
        {
            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(GetLoggingConfiguration(configuration));
                logging.AddConsoleLogger();
                logging.AddDebug();
                loggingOptions?.Invoke(logging);
            });
        }

        protected virtual IConfiguration GetLoggingConfiguration(IConfiguration configuration)
        {
            return configuration.GetSection("Logging");
        }
    }
}