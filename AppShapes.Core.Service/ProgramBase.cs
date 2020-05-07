using System;
using AppShapes.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Service
{
    public abstract class ProgramBase
    {
        public virtual void Run(string[] args)
        {
            IWebHost host = GetWebHost(args);
            Initialize(host.Services);
            Run(host);
        }

        protected virtual void ConfigureConfiguration(IWebHostBuilder builder)
        {
            new ConfigureWebHostConfigurationCommand().Execute(builder, ConfigureConfigurationOptions);
        }

        protected virtual void ConfigureConfigurationOptions(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
        }

        protected virtual void ConfigureLogging(IWebHostBuilder builder)
        {
            new ConfigureWebHostLoggingCommand().Execute(builder, ConfigureLoggingOptions);
        }

        protected virtual void ConfigureLoggingOptions(ILoggingBuilder loggingBuilder)
        {
        }

        protected virtual IWebHostBuilder ConfigureWebHostBuilder(IWebHostBuilder builder)
        {
            ConfigureLogging(builder);
            ConfigureConfiguration(builder);
            return builder;
        }

        protected virtual IWebHost GetWebHost(string[] args)
        {
            IWebHostBuilder builder = GetWebHostBuilder(args);
            ConfigureWebHostBuilder(builder);
            return builder.Build();
        }

        protected abstract IWebHostBuilder GetWebHostBuilder(string[] args);

        protected virtual void Initialize(IServiceProvider provider)
        {
            InitializeLogging(provider);
        }

        protected virtual void InitializeLogging(IServiceProvider provider)
        {
            Logger = provider.GetRequiredService<ILogger<ProgramBase>>();
        }

        protected ILogger<ProgramBase> Logger { get; set; }

        protected virtual void Run(IWebHost host)
        {
            Logger.Information<ProgramBase>("starting");
            host.Run();
            Logger.Information<ProgramBase>("finished");
        }
    }
}