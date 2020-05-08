using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppShapes.Core.Testing.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppShapes.Core.Testing.Infrastructure
{
    public class FakeDbContext : DbContext
    {
        public FakeDbContext() : this(new DbContextOptions<FakeDbContext>())
        {
        }

        public FakeDbContext(DbContextOptions options, IMigrator migrator = null) : base(options)
        {
            Dictionary<Type, object> services = new Dictionary<Type, object> {{typeof(IMigrator), migrator ?? new NullMigrator()}};
            Database = new DatabaseFacadeStub(this, new DelegatingServiceProvider(type => services[type], () => { }));
        }

        public override DatabaseFacade Database { get; }

        public override int SaveChanges()
        {
            return 0;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(SaveChanges());
        }
    }
}