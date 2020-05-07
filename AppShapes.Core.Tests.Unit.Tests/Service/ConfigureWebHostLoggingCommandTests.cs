using System;
using System.Linq;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Core;
using AppShapes.Logging.Console;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureWebHostLoggingCommandTests
    {
        [Fact]
        public void ExecuteMustAddLoggingConfigurationWhenCalled()
        {
            WebHostBuilder builder = new WebHostBuilder();
            builder.UseStartup<FakeStartup>();
            builder.UseConfiguration(new ConfigurationFactory().Create(("Logging:LogLevel:Default", "Information")));
            new ConfigureWebHostLoggingCommand().Execute(builder, b => Assert.NotNull(ReflectionHelper.GetProperty<ConfigurationSection>(b.Services.FirstOrDefault(x => x.ServiceType.Name == "LoggingConfiguration")?.ImplementationInstance, "Configuration")));
            builder.Build();
        }

        [Fact]
        public void ExecuteMustAddLoggingProvidersWhenCalled()
        {
            WebHostBuilder builder = new WebHostBuilder();
            builder.UseStartup<FakeStartup>();
            new ConfigureWebHostLoggingCommand().Execute(builder, b =>
            {
                Assert.Contains(b.Services, x => x.ImplementationType == typeof(ConsoleLoggerManager));
                Assert.Contains(b.Services, x => x.ImplementationType == typeof(DebugLoggerProvider));
                Assert.DoesNotContain(b.Services, x => x.ServiceType == typeof(ILoggerProvider) && x.ImplementationType != typeof(ConsoleLoggerManager) && x.ImplementationType != typeof(DebugLoggerProvider));
            });
            builder.Build();
        }

        [Fact]
        public void GetLoggingConfigurationMustReturnLoggingSectionWhenLoggingSectionDoesNotExist()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            StubConfigureWebHostLoggingCommand command = new StubConfigureWebHostLoggingCommand();
            Assert.Null(((IConfigurationSection) command.InvokeGetLoggingConfiguration(configuration)).Value);
        }

        [Fact]
        public void GetLoggingConfigurationMustReturnLoggingSectionWhenLoggingSectionExists()
        {
            IConfiguration configuration = new StubConfigureWebHostLoggingCommand().InvokeGetLoggingConfiguration(new ConfigurationFactory().Create(("Logging:LogLevel:Default", "Information")));
            Assert.Equal("Information", configuration["LogLevel:Default"]);
        }

        private class FakeStartup : IStartup
        {
            public void Configure(IApplicationBuilder app)
            {
            }

            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                return services.BuildServiceProvider();
            }
        }

        private class StubConfigureWebHostLoggingCommand : ConfigureWebHostLoggingCommand
        {
            public IConfiguration InvokeGetLoggingConfiguration(IConfiguration configuration)
            {
                return GetLoggingConfiguration(configuration);
            }
        }
    }
}