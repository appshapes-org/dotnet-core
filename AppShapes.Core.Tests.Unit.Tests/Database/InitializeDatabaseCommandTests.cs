using System;
using AppShapes.Core.Database;
using AppShapes.Core.Testing.Core;
using AppShapes.Core.Testing.Infrastructure;
using AppShapes.Core.Testing.Logging;
using AppShapes.Core.Testing.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Database
{
    public class InitializeDatabaseCommandTests
    {
        [Fact]
        public void ExecuteMustLogDatabaseMigrationWhenShouldMigrateDatabaseReturnsTrue()
        {
            MockLogger<InitializeDatabaseCommand> logger = new MockLogger<InitializeDatabaseCommand>();
            IConfiguration configuration = new ConfigurationFactory().Create(("AppSettings:AutoMigrateDatabase", "true"));
            new MockInitializeDatabaseCommand(logger, configuration, () => new MockServiceScope(new ServiceCollection().AddSingleton<DbContext>(new FakeDbContext()).BuildServiceProvider())).Execute();
            Assert.Contains(logger.Storage, x => x.StartsWith("Information: starting"));
            Assert.Contains(logger.Storage, x => x.StartsWith("Information: finished"));
        }

        [Fact]
        public void ExecuteMustMigrateDatabaseWhenShouldMigrateDatabaseReturnsTrue()
        {
            MockLogger<InitializeDatabaseCommand> logger = new MockLogger<InitializeDatabaseCommand>();
            IConfiguration configuration = new ConfigurationFactory().Create(("AppSettings:AutoMigrateDatabase", "true"));
            MockMigrator migrator = new MockMigrator();
            new MockInitializeDatabaseCommand(logger, configuration, () => new MockServiceScope(new ServiceCollection().AddSingleton<DbContext>(new FakeDbContext(new DbContextOptions<FakeDbContext>(), migrator)).BuildServiceProvider())).Execute();
            Assert.Equal(1, migrator.MigrateCalled);
        }

        [Fact]
        public void ExecuteMustNotMigrateDatabaseWhenShouldMigrateDatabaseReturnsFalse()
        {
            MockLogger<InitializeDatabaseCommand> logger = new MockLogger<InitializeDatabaseCommand>();
            IConfiguration configuration = new ConfigurationFactory().Create(("AppSettings:AutoMigrateDatabase", "false"));
            MockInitializeDatabaseCommand command = new MockInitializeDatabaseCommand(logger, configuration, null);
            command.Execute();
            Assert.Equal(0, command.MigrateDatabaseCalled);
        }

        [Fact]
        public void ShouldMigrateDatabaseMustLogAutoMigrateDatabaseSettingWhenCalled()
        {
            MockLogger<InitializeDatabaseCommand> logger = new MockLogger<InitializeDatabaseCommand>();
            IConfiguration configuration = new ConfigurationFactory().Create(("AppSettings:AutoMigrateDatabase", "false"));
            MockInitializeDatabaseCommand command = new MockInitializeDatabaseCommand(logger, configuration, null);
            command.Execute();
            Assert.Contains(logger.Storage, x => x.StartsWith("Debug: ShouldMigrateDatabase: False"));
        }

        private class MockInitializeDatabaseCommand : InitializeDatabaseCommand
        {
            public MockInitializeDatabaseCommand(ILogger<InitializeDatabaseCommand> logger, IConfiguration configuration, Func<IServiceScope> createScope) : base(logger, configuration, createScope)
            {
            }

            public int MigrateDatabaseCalled { get; private set; }

            protected override void MigrateDatabase()
            {
                ++MigrateDatabaseCalled;
                base.MigrateDatabase();
            }
        }
    }
}