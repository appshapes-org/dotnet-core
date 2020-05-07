using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppShapes.Core.Testing.Infrastructure
{
    public class FakeTransaction : IDbContextTransaction
    {
        public void Commit()
        {
            Status = TransactionStatus.Committed;
        }

        public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Commit();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return new ValueTask(Task.CompletedTask);
        }

        public void Rollback()
        {
            Status = TransactionStatus.Aborted;
        }

        public Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Rollback();
            return Task.CompletedTask;
        }

        public TransactionStatus Status { get; set; } = TransactionStatus.Active;

        public Guid TransactionId { get; } = Guid.NewGuid();
    }
}