using System.Collections.Generic;
using AppShapes.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Console
{
    public abstract class DatabaseProgram<T> : ProgramBase where T : DbContext
    {
        protected DatabaseProgram(IEnumerable<string> args) : base(args)
        {
        }

        protected abstract void ConfigureDatabase(DbContextOptionsBuilder arg1, string arg2);

        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            new ConfigureDatabaseCommand().Execute<T>(services, Configuration, ConfigureEntityFramework, ConfigureDatabase);
        }

        protected abstract void ConfigureEntityFramework(IServiceCollection services);

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            ConfigureDatabase(services);
        }

        protected override void Initialize()
        {
            base.Initialize();
            InitializeDatabase();
        }

        protected virtual void InitializeDatabase()
        {
            ILogger<InitializeDatabaseCommand> logger = Provider.GetRequiredService<ILogger<InitializeDatabaseCommand>>();
            new InitializeDatabaseCommand(logger, Configuration, Provider.CreateScope).Execute();
        }
    }
}