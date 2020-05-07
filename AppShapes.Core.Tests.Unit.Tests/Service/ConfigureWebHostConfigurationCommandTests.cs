using System;
using System.Threading;
using System.Threading.Tasks;
using AppShapes.Core.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureWebHostConfigurationCommandTests
    {
        [Fact]
        public void ExecuteMustCallConfigureAppConfigurationWhenBuilderIsNotNull()
        {
            IWebHostBuilder builder = new FakeWebHostBuilder();
            int optionsCalled = 0;
            new ConfigureWebHostConfigurationCommand().Execute(builder, (c, b) => { ++optionsCalled; });
            builder.Build();
            Assert.Equal(1, optionsCalled);
        }

        private class FakeWebHostBuilder : IWebHostBuilder
        {
            public IWebHost Build()
            {
                ConfigureDelegate?.Invoke(new WebHostBuilderContext(), new ConfigurationBuilder());
                return new FakeWebHost();
            }

            public IWebHostBuilder ConfigureAppConfiguration(Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate)
            {
                ConfigureDelegate = configureDelegate;
                return this;
            }

            public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
            {
                return this;
            }

            public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
            {
                return this;
            }

            public string GetSetting(string key)
            {
                return null;
            }

            public IWebHostBuilder UseSetting(string key, string value)
            {
                return this;
            }

            private Action<WebHostBuilderContext, IConfigurationBuilder> ConfigureDelegate { get; set; }

            private class FakeWebHost : IWebHost
            {
                public void Dispose()
                {
                }

                public IFeatureCollection ServerFeatures { get; }

                public IServiceProvider Services { get; }

                public void Start()
                {
                }

                public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
                {
                    return Task.CompletedTask;
                }

                public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
                {
                    return Task.CompletedTask;
                }
            }
        }
    }
}