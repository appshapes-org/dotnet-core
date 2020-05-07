using System;
using AppShapes.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Database
{
    public class InitializeDatabaseCommand
    {
        public InitializeDatabaseCommand(ILogger<InitializeDatabaseCommand> logger, IConfiguration configuration, Func<IServiceScope> createScope)
        {
            Logger = logger;
            Configuration = configuration;
            CreateScope = createScope;
        }

        public virtual void Execute()
        {
            if (ShouldMigrateDatabase())
                MigrateDatabase();
        }

        protected virtual DbContext GetDatabaseContext(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<DbContext>();
        }

        protected virtual void MigrateDatabase()
        {
            Logger.Information<InitializeDatabaseCommand>("starting");
            using (IServiceScope scope = CreateScope())
            {
                DbContext context = GetDatabaseContext(scope);
                context.Database.Migrate();
            }

            Logger.Information<InitializeDatabaseCommand>("finished");
        }

        protected virtual bool ShouldMigrateDatabase()
        {
            bool shouldMigrateDatabase = Convert.ToBoolean(Configuration["AppSettings:AutoMigrateDatabase"]);
            Logger.Debug<InitializeDatabaseCommand>($"{nameof(ShouldMigrateDatabase)}: {shouldMigrateDatabase}");
            return shouldMigrateDatabase;
        }

        private IConfiguration Configuration { get; }

        private Func<IServiceScope> CreateScope { get; }

        private ILogger<InitializeDatabaseCommand> Logger { get; }
    }
}