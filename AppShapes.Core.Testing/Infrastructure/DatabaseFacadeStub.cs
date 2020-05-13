using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppShapes.Core.Testing.Infrastructure
{
    public class DatabaseFacadeStub : DatabaseFacade, IInfrastructure<IServiceProvider>
    {
        public DatabaseFacadeStub(DbContext context, IServiceProvider provider) : base(context)
        {
            Provider = provider;
        }

        public override IDbContextTransaction BeginTransaction()
        {
            FakeTransaction transaction = new FakeTransaction();
            Transactions.Push(transaction);
            return transaction;
        }

        public override void CommitTransaction()
        {
            Transactions.Pop().Status = TransactionStatus.Committed;
        }

        public bool Created { get; private set; }

        public override bool EnsureCreated()
        {
            Created = true;
            return Created;
        }

        public override void RollbackTransaction()
        {
            Transactions.Pop().Status = TransactionStatus.Aborted;
        }

        IServiceProvider IInfrastructure<IServiceProvider>.Instance => Provider;

        private IServiceProvider Provider { get; }

        private Stack<FakeTransaction> Transactions { get; } = new Stack<FakeTransaction>();
    }
}