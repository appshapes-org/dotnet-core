using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable 618

namespace AppShapes.Core.Testing.Service
{
    public class MockWebHost : IWebHost
    {
        public MockWebHost(IWebHost webHost)
        {
            WebHost = webHost;
        }

        public void Dispose()
        {
            WebHost?.Dispose();
            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        public IFeatureCollection ServerFeatures => WebHost.ServerFeatures;

        public IServiceProvider Services => WebHost.Services;

        public void Start()
        {
            ++StartCalled;
        }

        public Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Start();
            Services.GetRequiredService<IApplicationLifetime>().StopApplication();
            return Task.CompletedTask;
        }

        public int StartCalled { get; private set; }

        public Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        private IWebHost WebHost { get; }
    }
}