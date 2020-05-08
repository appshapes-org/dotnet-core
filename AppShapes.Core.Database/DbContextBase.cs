using Microsoft.EntityFrameworkCore;

namespace AppShapes.Core.Database
{
    public abstract class DbContextBase : DbContext
    {
        protected DbContextBase()
        {
        }

        protected DbContextBase(DbContextOptions options) : base(options)
        {
        }
    }
}