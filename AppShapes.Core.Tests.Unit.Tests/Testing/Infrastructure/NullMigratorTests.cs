using System.Threading;
using AppShapes.Core.Testing.Infrastructure;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class NullMigratorTests
    {
        [Fact]
        public void GenerateScriptMustReturnEmptyScriptWhenCalled()
        {
            Assert.Empty(new NullMigrator().GenerateScript());
            Assert.Empty(new NullMigrator().GenerateScript("42"));
            Assert.Empty(new NullMigrator().GenerateScript("42", "43"));
            Assert.Empty(new NullMigrator().GenerateScript("42", "43", true));
        }

        [Fact]
        public void MigrateAsyncMustNotDoAnythingWhenCalled()
        {
            new NullMigrator().MigrateAsync();
            new NullMigrator().MigrateAsync("42");
            new NullMigrator().MigrateAsync("42", CancellationToken.None);
        }

        [Fact]
        public void MigrateMustNotDoAnythingWhenCalled()
        {
            new NullMigrator().Migrate();
            new NullMigrator().Migrate("42");
        }
    }
}