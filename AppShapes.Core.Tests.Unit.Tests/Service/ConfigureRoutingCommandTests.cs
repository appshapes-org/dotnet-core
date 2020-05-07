using System.Linq;
using AppShapes.Core.Service;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureRoutingCommandTests
    {
        [Fact]
        public void ExecuteMustAddRoutingAndCallOptionsWhenOptionsIsNotNull()
        {
            int optionsCalled = 0;
            ServiceCollection services = new ServiceCollection();
            new ConfigureRoutingCommand().Execute(services, _ => ++optionsCalled);
            services.Remove(services.First(x => x.ServiceType == typeof(IConfigureOptions<RouteOptions>) && x.Lifetime == ServiceLifetime.Transient));
            services.BuildServiceProvider().GetRequiredService<IConfigureOptions<RouteOptions>>().Configure(new RouteOptions());
            Assert.True(optionsCalled == 1);
        }

        [Fact]
        public void ExecuteMustAddRoutingWhenOptionsIsNull()
        {
            ServiceCollection services = new ServiceCollection();
            new ConfigureRoutingCommand().Execute(services);
            Assert.NotNull(services.BuildServiceProvider().GetService<EndpointSelector>());
        }
    }
}