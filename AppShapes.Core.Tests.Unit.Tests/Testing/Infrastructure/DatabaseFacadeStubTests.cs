using System;
using System.Transactions;
using AppShapes.Core.Testing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class DatabaseFacadeStubTests
    {
        [Fact]
        public void BeginTransactionMustReturnFakeTransactionWhenCalled()
        {
            DatabaseFacadeStub database = new DatabaseFacadeStub(new FakeDbContext(), null);
            Assert.IsAssignableFrom<FakeTransaction>(database.BeginTransaction());
        }

        [Fact]
        public void CommitTransactionMustCommitTransactionWhenCalled()
        {
            DatabaseFacadeStub database = new DatabaseFacadeStub(new FakeDbContext(), null);
            FakeTransaction transaction = (FakeTransaction) database.BeginTransaction();
            Assert.Equal(TransactionStatus.Active, transaction.Status);
            database.CommitTransaction();
            Assert.Equal(TransactionStatus.Committed, transaction.Status);
        }

        [Fact]
        public void InstanceMustReturnServiceProviderWhenCalled()
        {
            ServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            DatabaseFacadeStub database = new DatabaseFacadeStub(new FakeDbContext(), provider);
            Assert.Same(provider, ((IInfrastructure<IServiceProvider>) database).Instance);
        }

        [Fact]
        public void RollbackTransactionMustRollbackTransactionWhenCalled()
        {
            DatabaseFacadeStub database = new DatabaseFacadeStub(new FakeDbContext(), null);
            FakeTransaction transaction = (FakeTransaction) database.BeginTransaction();
            Assert.Equal(TransactionStatus.Active, transaction.Status);
            database.RollbackTransaction();
            Assert.Equal(TransactionStatus.Aborted, transaction.Status);
        }
    }
}