using AppShapes.Core.Messaging;
using Microsoft.EntityFrameworkCore;

namespace AppShapes.Core.Database
{
    public class OutboxContext : DbContextBase
    {
        public OutboxContext()
        {
        }

        public OutboxContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<OutboxItem> Outbox { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OutboxItem>().HasIndex(u => u.Timestamp);
            builder.Entity<OutboxItem>().Property(x => x.Id).ValueGeneratedNever();
        }
    }
}