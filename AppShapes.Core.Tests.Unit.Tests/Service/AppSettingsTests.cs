using AppShapes.Core.Service;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class AppSettingsTests
    {
        [Fact]
        public void AutoMigrateDatabaseMustReturnValueWhenSet()
        {
            Assert.True(new AppSettings {AutoMigrateDatabase = true}.AutoMigrateDatabase);
        }
    }
}