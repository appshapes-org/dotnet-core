using AppShapes.Core.Database;
using AppShapes.Core.Testing.Core;
using AppShapes.Core.Testing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Database
{
    public class ConfigureDatabaseCommandTests
    {
        [Fact]
        public void ExecuteMustCallConfigureDatabaseWhenCalled()
        {
            int configureDatabaseCalled = 0;
            ServiceCollection services = new ServiceCollection();
            new ConfigureDatabaseCommand().Execute<FakeDbContext>(services, new ConfigurationFactory().Create(("ConnectionStrings:DatabaseConnection", "Test")), collection => { }, (_, connectionString) =>
            {
                ++configureDatabaseCalled;
                Assert.Equal("Test", connectionString);
            });
            services.BuildServiceProvider().GetRequiredService<FakeDbContext>();
            Assert.Equal(1, configureDatabaseCalled);
        }

        [Fact]
        public void ExecuteMustCallConfigureEntityFrameworkWhenCalled()
        {
            IServiceCollection services = new ServiceCollection();
            int configureEntityFrameworkCalled = 0;
            new ConfigureDatabaseCommand().Execute<FakeDbContext>(services, new ConfigurationFactory().Create(("ConnectionStrings:DatabaseConnection", "Test")), collection => ++configureEntityFrameworkCalled, (_, __) => { });
            Assert.Equal(1, configureEntityFrameworkCalled);
        }

        [Fact]
        public void ExecuteMustConfigureDbContextsWhenCalled()
        {
            IServiceCollection services = new ServiceCollection();
            new ConfigureDatabaseCommand().Execute<FakeDbContext>(services, new ConfigurationFactory().Create(("ConnectionStrings:DatabaseConnection", "Test")), collection => { }, (_, __) => { });
            ServiceProvider provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetService<FakeDbContext>());
            Assert.NotNull(provider.GetService<DbContext>());
        }
    }
}