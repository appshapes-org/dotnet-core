using AppShapes.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureHealthChecksCommandTests
    {
        [Fact]
        public void ExecuteMustAddHealthChecksWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            new ConfigureHealthChecksCommand().Execute(services);
            Assert.Contains(services, x => x.ServiceType == typeof(HealthCheckService));
        }
    }
}