using AppShapes.Core.Service;
using AppShapes.Core.Testing.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureConfigurationCommandTests
    {
        [Fact]
        public void ExecuteMustConfigureAppSettingsAndConfigurationOptionsWhenConfigurationOptionsIsNotNull()
        {
            IServiceCollection services = new ServiceCollection();
            new ConfigureConfigurationCommand().Execute(services, new ConfigurationFactory().Create(("AppSettings:AutoMigrateDatabase", "true")), (s, _) => { s.AddSingleton(new FakeService()); });
            Assert.True(services.BuildServiceProvider().GetRequiredService<AppSettings>().AutoMigrateDatabase);
            Assert.NotNull(services.BuildServiceProvider().GetService<FakeService>());
        }

        [Fact]
        public void ExecuteMustConfigureAppSettingsWhenConfigurationOptionsIsNull()
        {
            IServiceCollection services = new ServiceCollection();
            new ConfigureConfigurationCommand().Execute(services, new ConfigurationFactory().Create(("AppSettings:AutoMigrateDatabase", "true")), (_, __) => { });
            Assert.True(services.BuildServiceProvider().GetRequiredService<AppSettings>().AutoMigrateDatabase);
        }

        private class FakeService
        {
        }
    }
}