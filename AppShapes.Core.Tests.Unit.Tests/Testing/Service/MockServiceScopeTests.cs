using System;
using AppShapes.Core.Testing.Service;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class MockServiceScopeTests
    {
        [Fact]
        public void DisposeMustDisposeWhenCalled()
        {
            int disposeCalled = 0;
            new MockServiceScope(null, () => { ++disposeCalled; }).Dispose();
            Assert.Equal(1, disposeCalled);
        }

        [Fact]
        public void ServiceProviderMustReturnServiceProviderWhenSet()
        {
            IServiceProvider provider = new ServiceCollection().BuildServiceProvider();
            MockServiceScope scope = new MockServiceScope(provider);
            Assert.Same(provider, scope.ServiceProvider);
        }
    }
}