using System;
using AppShapes.Core.Logging;
using AppShapes.Core.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class UseProductionControllersCommandTests
    {
        [Fact]
        public void ExecuteMustUseHstsWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            new ConfigureRoutingCommand().Execute(services);
            new ConfigureCorsCommand().Execute(services, options => { });
            new ConfigureLoggingCommand().Execute(services, new ConfigurationBuilder().Build());
            IServiceProvider provider = services.BuildServiceProvider();
            IApplicationBuilder app = new ApplicationBuilder(provider);
            new UseProductionControllersCommand().Execute(app, "Test");
        }
    }
}