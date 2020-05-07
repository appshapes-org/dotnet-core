using System.Collections.Generic;
using System.Linq;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigurationBuilderFactoryTests
    {
        [Fact]
        public void CreateMustReturnConfigurationBuilderWhenConfigurationIsNotNull()
        {
            IConfiguration memoryConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new[] {new KeyValuePair<string, string>("Test", "42")}).Build();
            IConfigurationRoot configuration = new ConfigurationBuilderFactory().Create(memoryConfiguration, new FakeHostingEnvironment()).Build();
            JsonConfigurationProvider provider = configuration.Providers.FirstOrDefault(x => (x as JsonConfigurationProvider)?.Source.Path == "appsettings.json") as JsonConfigurationProvider;
            Assert.NotNull(provider);
            Assert.True(provider.Source.ReloadOnChange);
            Assert.Contains(configuration.Providers, x => (x as JsonConfigurationProvider)?.Source.Path == "appsettings.Test.json");
            Assert.Contains(configuration.Providers, x => x is EnvironmentVariablesConfigurationProvider);
            Assert.Contains(configuration.Providers, x => x is ChainedConfigurationProvider);
        }

        [Fact]
        public void CreateMustReturnConfigurationBuilderWhenConfigurationIsNull()
        {
            IConfigurationRoot configuration = new ConfigurationBuilderFactory().Create(null, new FakeHostingEnvironment()).Build();
            Assert.Contains(configuration.Providers, x => (x as JsonConfigurationProvider)?.Source.Path == "appsettings.json");
            Assert.Contains(configuration.Providers, x => (x as JsonConfigurationProvider)?.Source.Path == "appsettings.Test.json");
            Assert.Contains(configuration.Providers, x => x is EnvironmentVariablesConfigurationProvider);
        }
    }
}