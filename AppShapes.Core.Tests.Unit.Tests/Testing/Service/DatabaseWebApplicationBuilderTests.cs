using AppShapes.Core.Testing.Infrastructure;
using AppShapes.Core.Testing.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class DatabaseWebApplicationBuilderTests
    {
        [Fact]
        public void ConfigureBootstrapMustCreateDatabase()
        {
            StubDatabaseWebApplicationBuilder builder = new StubDatabaseWebApplicationBuilder();
            IServiceCollection collection = new ServiceCollection().AddSingleton<FakeDbContext>();
            ServiceProvider provider = collection.BuildServiceProvider();
            builder.InvokeConfigureBootstrap(provider);
            FakeDbContext context = provider.GetRequiredService<FakeDbContext>();
            Assert.True((context.Database as DatabaseFacadeStub)?.Created);
        }

        [Fact]
        public void ConfigureServicesMustConfigureDatabaseWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            StubDatabaseWebApplicationBuilder builder = new StubDatabaseWebApplicationBuilder();
            builder.InvokeConfigureServices(services);
            Assert.Contains(services, x => x.ServiceType == typeof(FakeDbContext));
            ServiceProvider provider = services.BuildServiceProvider();
            provider.GetRequiredService<FakeDbContext>();
            DbContextOptions<FakeDbContext> options = builder.ContextOptions;
            Assert.Contains(options.Extensions, x => x is InMemoryOptionsExtension);
        }

        [Fact]
        public void InitializeDatabaseMustCallInitializeDatabaseActionWhenCalled()
        {
            int initializeDatabaseCalled = 0;
            StubDatabaseWebApplicationBuilder builder = new StubDatabaseWebApplicationBuilder();
            IServiceCollection collection = new ServiceCollection().AddSingleton<FakeDbContext>();
            builder.DependenciesSetter = collection.BuildServiceProvider();
            builder.InitializeDatabase(_ => { ++initializeDatabaseCalled; });
            Assert.Equal(1, initializeDatabaseCalled);
        }

        private class StubDatabaseWebApplicationBuilder : DatabaseWebApplicationBuilder<FakeStartup, FakeDbContext>
        {
            public ServiceProvider DependenciesSetter { set => Dependencies = value; }

            public void InvokeConfigureBootstrap(ServiceProvider provider)
            {
                ConfigureBootstrap(provider);
            }

            public void InvokeConfigureServices(IServiceCollection services)
            {
                ConfigureServices(services);
            }
        }
    }
}