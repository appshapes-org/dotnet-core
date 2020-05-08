using AppShapes.Core.Database;
using AppShapes.Core.Messaging;
using AppShapes.Core.Testing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Database
{
    public class OutboxContextTests
    {
        [Fact]
        public void ConstructorMustInitializeOutboxWhenCalled()
        {
            Assert.NotEqual(default, new OutboxContext().Outbox);
            Assert.NotEqual(default, new OutboxContext(new DbContextOptions<OutboxContext>()).Outbox);
            DbSet<OutboxItem> outbox = new MockDbSet<OutboxItem>();
            Assert.Same(outbox, new OutboxContext {Outbox = outbox}.Outbox);
        }

        [Fact]
        public void OnModelCreatingMustCreateTimestampIndexWhenCalled()
        {
            using StubOutboxContext context = new StubOutboxContext();
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            context.InvokeOnModelCreating(builder);
            Assert.NotNull(new ModelHelper(builder.Model).GetIndex<OutboxItem>(nameof(OutboxItem.Timestamp)));
        }

        [Fact]
        public void OnModelCreatingMustSetOutboxItemIdToValueGeneratedNeverWhenCalled()
        {
            using StubOutboxContext context = new StubOutboxContext();
            ModelBuilder builder = new ModelBuilder(new ConventionSet());
            context.InvokeOnModelCreating(builder);
            Property property = new ModelHelper(builder.Model).GetProperty<OutboxItem>(nameof(OutboxItem.Id));
            Assert.Equal(ValueGenerated.Never, property.ValueGenerated);
        }

        private class StubOutboxContext : OutboxContext
        {
            public void InvokeOnModelCreating(ModelBuilder builder)
            {
                OnModelCreating(builder);
            }
        }
    }
}