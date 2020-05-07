using AppShapes.Core.Testing.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Logging
{
    public class NullScopeTests
    {
        [Fact]
        public void DisposeMustDoNothingWhenCalled()
        {
            NullScope scope = ReflectionHelper.InvokeConstructor<NullScope>();
            scope.Dispose();
            Assert.True(scope.IsDisposed);
        }
    }
}