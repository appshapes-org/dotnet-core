using AppShapes.Core.Database;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Database
{
    public class DbContextBaseTests
    {
        [Fact]
        public void ConstructorMustInitializeDbContextOptionsWhenCalled()
        {
            Assert.NotNull(ReflectionHelper.GetField(typeof(DbContext), new FakeContext(), "_options"));
            Assert.NotNull(ReflectionHelper.GetField(typeof(DbContext), new FakeContext(new DbContextOptions<FakeContext>()), "_options"));
        }

        private class FakeContext : DbContextBase
        {
            public FakeContext()
            {
            }

            public FakeContext(DbContextOptions options) : base(options)
            {
            }
        }
    }
}