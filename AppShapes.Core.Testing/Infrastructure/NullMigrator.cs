using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppShapes.Core.Testing.Infrastructure
{
    public class NullMigrator : IMigrator
    {
        public virtual string GenerateScript(string fromMigration = null, string toMigration = null, bool idempotent = false)
        {
            return string.Empty;
        }

        public virtual void Migrate(string targetMigration = null)
        {
        }

        public virtual Task MigrateAsync(string targetMigration = null, CancellationToken cancellationToken = new CancellationToken())
        {
            Migrate();
            return Task.CompletedTask;
        }
    }
}