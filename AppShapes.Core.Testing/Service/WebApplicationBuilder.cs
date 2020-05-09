using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AppShapes.Core.Testing.Service
{
    public class WebApplicationBuilder<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public Func<string[], IWebHostBuilder> CreateWebHostBuilderAction { get; set; } = a => WebHost.CreateDefaultBuilder<TStartup>(a).ConfigureLogging((_, x) => x.ClearProviders());

        public ServiceProvider Dependencies { get; private set; }

        protected virtual void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddProvider(NullLoggerProvider.Instance);
            });
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureLogging(services);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ConfigureServices(services);
                Dependencies = services.BuildServiceProvider();
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            try
            {
                return base.CreateWebHostBuilder();
            }
            catch
            {
                return CreateWebHostBuilderAction(new string[] { }).UseEnvironment(Environments.Development);
            }
        }
    }
}