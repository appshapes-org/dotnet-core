using System;
using AppShapes.Core.Testing.Service;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class MockWebHostTests
    {
        [Fact]
        public void DisposeMustDisposeWebHostWhenWebHostIsNotNull()
        {
            MockWebHost host = new MockWebHost(null);
            new MockWebHost(host).Dispose();
            Assert.True(host.IsDisposed);
        }

        [Fact]
        public void DisposeMustNotDisposeWebHostWhenWebHostIsNull()
        {
            MockWebHost host = new MockWebHost(null);
            host.Dispose();
            Assert.Null(ReflectionHelper.GetProperty(host, "WebHost"));
        }

        [Fact]
        public void ServerFeaturesMustReturnWebHostServerFeaturesWhenCalled()
        {
            Assert.NotEmpty(new MockWebHost(new MockWebHostBuilder().Build()).ServerFeatures);
        }

        [Fact]
        public void ServicesMustReturnWebHostServicesWhenCalled()
        {
            IServiceProvider provider = new MockWebHost(new MockWebHostBuilder().Build()).Services;
            Assert.NotNull(provider.GetService(typeof(IConfiguration)));
        }

        [Fact]
        public async void StartMustStartWebHostWhenCalled()
        {
            MockWebHost host = new MockWebHost(new MockWebHostBuilder().Build());
            await host.StartAsync();
            Assert.True(host.StartCalled > 0);
        }
    }
}