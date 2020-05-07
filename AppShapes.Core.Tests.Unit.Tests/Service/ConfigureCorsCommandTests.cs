using AppShapes.Core.Service;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureCorsCommandTests
    {
        [Fact]
        public void ExecuteMustAddCorsAndCallCorsOptionsWhenCalled()
        {
            int corsOptionsCalled = 0;
            ServiceCollection services = new ServiceCollection();
            new ConfigureCorsCommand().Execute(services, _ => ++corsOptionsCalled);
            services.BuildServiceProvider().GetRequiredService<IConfigureOptions<CorsOptions>>().Configure(new CorsOptions());
            Assert.True(corsOptionsCalled == 1);
        }
    }
}