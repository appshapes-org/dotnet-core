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
        public virtual Func<string[], IWebHostBuilder> CreateWebHostBuilderAction { get; set; } = args =>
        {
            IWebHostBuilder builder = WebHost.CreateDefaultBuilder<TStartup>(args);
            builder.ConfigureLogging((_, x) => x.ClearProviders());
            builder.UseEnvironment(Environments.Development);
            return builder;
        };

        public ServiceProvider Dependencies { get; protected set; }

        protected virtual void ConfigureBootstrap(ServiceProvider provider)
        {
        }

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
                ConfigureBootstrap(Dependencies);
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
                return CreateWebHostBuilderAction(new string[] { });
            }
        }
    }
}