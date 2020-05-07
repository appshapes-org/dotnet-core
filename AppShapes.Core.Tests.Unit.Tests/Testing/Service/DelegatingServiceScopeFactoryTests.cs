using AppShapes.Core.Testing.Service;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class DelegatingServiceScopeFactoryTests
    {
        [Fact]
        public void CreateScopeMustReturnServiceScopeWhenCalled()
        {
            IServiceScope scope = new MockServiceScope(null);
            Assert.Same(scope, new DelegatingServiceScopeFactory(() => scope).CreateScope());
        }
    }
}