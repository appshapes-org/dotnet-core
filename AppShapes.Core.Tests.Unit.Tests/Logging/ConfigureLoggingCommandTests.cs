using System;
using System.Linq;
using AppShapes.Core.Logging;
using AppShapes.Core.Testing.Core;
using AppShapes.Logging.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Logging
{
    public class ConfigureLoggingCommandTests
    {
        [Fact]
        public void ExecuteMustAddConsoleLoggerWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ILoggerProvider>(new FakeLoggerProvider());
            new ConfigureLoggingCommand().Execute(services, new ConfigurationBuilder().Build(), _ => { });
            Assert.Contains(services, x => x.ImplementationType == typeof(ConsoleLoggerManager));
        }

        [Fact]
        public void ExecuteMustAddDebugLoggerWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ILoggerProvider>(new FakeLoggerProvider());
            new ConfigureLoggingCommand().Execute(services, new ConfigurationBuilder().Build(), _ => { });
            Assert.Contains(services, x => x.ImplementationType == typeof(DebugLoggerProvider));
        }

        [Fact]
        public void ExecuteMustAddLoggingConfigurationWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ILoggerProvider>(new FakeLoggerProvider());
            new ConfigureLoggingCommand().Execute(services, new ConfigurationBuilder().Build(), _ => { });
            Assert.Contains(services, x => x.ServiceType == typeof(ILoggerFactory));
        }

        [Fact]
        public void ExecuteMustCallLoggingOptionsWhenLoggingOptionsIsNotNull()
        {
            ServiceCollection services = new ServiceCollection();
            int loggingOptionsCalled = 0;
            new ConfigureLoggingCommand().Execute(services, new ConfigurationBuilder().Build(), _ => ++loggingOptionsCalled);
            Assert.Equal(1, loggingOptionsCalled);
        }

        [Fact]
        public void ExecuteMustClearProvidersWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ILoggerProvider>(new FakeLoggerProvider());
            Assert.Contains(services, x => x.ImplementationInstance?.GetType() == typeof(FakeLoggerProvider));
            new ConfigureLoggingCommand().Execute(services, new ConfigurationBuilder().Build(), _ => { });
            Assert.DoesNotContain(services, x => x.ImplementationInstance?.GetType() == typeof(FakeLoggerProvider));
        }

        [Fact]
        public void ExecuteMustNotCallLoggingOptionsWhenLoggingOptionsIsNull()
        {
            ServiceCollection services = new ServiceCollection();
            int loggingOptionsCalled = 0;
            new ConfigureLoggingCommand().Execute(services, new ConfigurationBuilder().Build());
            Assert.Equal(0, loggingOptionsCalled);
        }

        [Fact]
        public void GetLoggingConfigurationMustReturnLoggingConfigurationWhenConfigurationContainsLoggingConfigurationSection()
        {
            IConfiguration configuration = new StubConfigureLoggingCommand().InvokeGetLoggingConfiguration(new ConfigurationFactory().Create(("Logging:LogLevel:Default", "Information"), ("AppSettings:AutoMigrateDatabase", "false")));
            Assert.Equal("LogLevel", configuration.GetChildren().FirstOrDefault()?.Key);
        }

        private class FakeLoggerProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
            }
        }

        private class StubConfigureLoggingCommand : ConfigureLoggingCommand
        {
            public IConfiguration InvokeGetLoggingConfiguration(IConfiguration configuration)
            {
                return GetLoggingConfiguration(configuration);
            }

            protected override IConfiguration GetLoggingConfiguration(IConfiguration configuration)
            {
                return base.GetLoggingConfiguration(configuration);
            }
        }
    }
}