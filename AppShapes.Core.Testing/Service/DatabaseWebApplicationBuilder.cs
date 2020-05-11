using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Testing.Service
{
    public class DatabaseWebApplicationBuilder<TStartup, TContext> : WebApplicationBuilder<TStartup> where TStartup : class where TContext : DbContext
    {
        public DbContextOptions<TContext> ContextOptions { get; private set; }

        public virtual void InitializeDatabase(Action<TContext> initializeDatabase)
        {
            initializeDatabase(Dependencies.GetRequiredService<TContext>());
        }

        protected override void ConfigureBootstrap(ServiceProvider provider)
        {
            base.ConfigureBootstrap(provider);
            CreateDatabase(provider);
        }

        protected virtual void ConfigureDatabase(DbContextOptionsBuilder options, ServiceProvider serviceProvider)
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
            options.UseInternalServiceProvider(serviceProvider);
            ContextOptions = (DbContextOptions<TContext>) options.Options;
        }

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            ServiceProvider serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            services.AddDbContext<TContext>(options => ConfigureDatabase(options, serviceProvider));
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            ConfigureDatabase(services);
        }

        protected virtual void CreateDatabase(ServiceProvider dependencies)
        {
            using IServiceScope scope = dependencies.CreateScope();
            DbContext context = scope.ServiceProvider.GetRequiredService<TContext>();
            context.Database.EnsureCreated();
        }
    }
}