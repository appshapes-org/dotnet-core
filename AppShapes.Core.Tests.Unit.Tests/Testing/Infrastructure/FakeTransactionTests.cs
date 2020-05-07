using System.Transactions;
using AppShapes.Core.Testing.Infrastructure;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class FakeTransactionTests
    {
        [Fact]
        public async void CommitAsyncMustSetStatusToCommittedWhenCalled()
        {
            FakeTransaction transaction = new FakeTransaction();
            await transaction.CommitAsync();
            Assert.Equal(TransactionStatus.Committed, transaction.Status);
        }

        [Fact]
        public void CommitMustSetStatusToCommittedWhenCalled()
        {
            FakeTransaction transaction = new FakeTransaction();
            transaction.Commit();
            Assert.Equal(TransactionStatus.Committed, transaction.Status);
        }

        [Fact]
        public async void DisposeAsyncMustDoNothingWhenCalled()
        {
            FakeTransaction transaction = new FakeTransaction();
            await transaction.DisposeAsync();
            Assert.True(transaction.IsDisposed);
        }

        [Fact]
        public void DisposeMustDisposeWhenCalled()
        {
            FakeTransaction transaction = new FakeTransaction();
            transaction.Dispose();
            Assert.True(transaction.IsDisposed);
        }

        [Fact]
        public void InitializerMustSetStatusToActiveWhenCalled()
        {
            Assert.Equal(TransactionStatus.Active, new FakeTransaction().Status);
        }

        [Fact]
        public async void RollbackAsyncMustSetStatusToAbortedWhenCalled()
        {
            FakeTransaction transaction = new FakeTransaction();
            await transaction.RollbackAsync();
            Assert.Equal(TransactionStatus.Aborted, transaction.Status);
        }

        [Fact]
        public void RollbackMustSetStatusToAbortedWhenCalled()
        {
            FakeTransaction transaction = new FakeTransaction();
            transaction.Rollback();
            Assert.Equal(TransactionStatus.Aborted, transaction.Status);
        }

        [Fact]
        public void TransactionIdMustSetTransactionIdToNewGuidWhenInitializerCalled()
        {
            Assert.NotEqual(default, new FakeTransaction().TransactionId);
        }
    }
}