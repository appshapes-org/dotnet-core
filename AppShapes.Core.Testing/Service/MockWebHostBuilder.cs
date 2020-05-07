using System;
using AppShapes.Core.Testing.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Testing.Service
{
    public class MockWebHostBuilder : IWebHostBuilder
    {
        public MockWebHostBuilder(IWebHostBuilder builder = null)
        {
            WebHostBuilder = builder ?? GetWebHostBuilder();
        }

        public IWebHost Build()
        {
            return WebHostBuilder.Build();
        }

        public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            WebHostBuilder.ConfigureAppConfiguration(configureDelegate);
            return this;
        }

        public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
        {
            WebHostBuilder.ConfigureServices(configureServices);
            MockLogger = new MockLogger();
            WebHostBuilder.ConfigureServices(x => x.Replace(new ServiceDescriptor(typeof(ILoggerFactory), new MockLoggerFactory(_ => MockLogger))));
            return this;
        }

        public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            WebHostBuilder.ConfigureServices(configureServices);
            return this;
        }

        public string GetSetting(string key)
        {
            return WebHostBuilder.GetSetting(key);
        }

        public MockLogger MockLogger { get; private set; }

        public IWebHostBuilder UseSetting(string key, string value)
        {
            WebHostBuilder.UseSetting(key, value);
            return this;
        }

        protected virtual IWebHostBuilder GetWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder().UseStartup<FakeStartup>().ConfigureLogging((_, x) => x.ClearProviders());
        }

        private IWebHostBuilder WebHostBuilder { get; }
    }
}