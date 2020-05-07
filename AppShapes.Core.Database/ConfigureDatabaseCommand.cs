using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Database
{
    public class ConfigureDatabaseCommand
    {
        public virtual void Execute<T>(IServiceCollection services, IConfiguration configuration, Action<IServiceCollection> configureEntityFramework, Action<DbContextOptionsBuilder, string> configureDatabase) where T : DbContext
        {
            configureEntityFramework(services);
            services.AddDbContext<T>(builder => configureDatabase(builder, GetConnectionString(configuration)));
            services.AddScoped<DbContext>(p => p.GetRequiredService<T>());
        }

        protected virtual string GetConnectionString(IConfiguration configuration)
        {
            return configuration.GetConnectionString("DatabaseConnection");
        }
    }
}