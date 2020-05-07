using AppShapes.Core.Testing.Service;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class DelegatingServiceProviderTests
    {
        [Fact]
        public void DisposeMustDisposeWhenCalled()
        {
            DelegatingServiceProvider provider = new DelegatingServiceProvider(null, () => { });
            Assert.False(provider.IsDisposed);
            provider.Dispose();
            Assert.True(provider.IsDisposed);
        }

        [Fact]
        public void GetServiceMustGetServiceWhenCalled()
        {
            Assert.Equal("42", new DelegatingServiceProvider(t => "42", () => { }).GetService(typeof(string)));
        }
    }
}