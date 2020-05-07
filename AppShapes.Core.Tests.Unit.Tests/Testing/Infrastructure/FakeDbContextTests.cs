using System;
using AppShapes.Core.Testing.Infrastructure;
using AppShapes.Core.Testing.Service;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class FakeDbContextTests
    {
        [Fact]
        public void DatabaseInternalServiceProviderMustReturnNullMigratorWhenCalled()
        {
            DatabaseFacadeStub database = Assert.IsAssignableFrom<DatabaseFacadeStub>(new FakeDbContext().Database);
            IServiceProvider provider = ReflectionHelper.GetProperty<IServiceProvider>(database, "Provider");
            Assert.IsAssignableFrom<NullMigrator>(provider.GetRequiredService<IMigrator>());
        }

        [Fact]
        public void DatabaseMustReturnDatabaseFacadeStubWhenCalled()
        {
            Assert.IsAssignableFrom<DatabaseFacadeStub>(new FakeDbContext().Database);
        }

        [Fact]
        public void DatabaseProviderMustDisposeWhenDisposeCalled()
        {
            DatabaseFacadeStub database = (DatabaseFacadeStub) new FakeDbContext().Database;
            DelegatingServiceProvider provider = ReflectionHelper.GetProperty<DelegatingServiceProvider>(database, "Provider");
            provider.Dispose();
            Assert.True(ReflectionHelper.GetProperty<bool>(provider, "IsDisposed"));
        }

        [Fact]
        public async void SaveChangesAsyncMustReturnZeroWhenCalled()
        {
            Assert.Equal(0, await new FakeDbContext().SaveChangesAsync());
        }

        [Fact]
        public void SaveChangesMustReturnZeroWhenCalled()
        {
            Assert.Equal(0, new FakeDbContext().SaveChanges());
        }
    }
}