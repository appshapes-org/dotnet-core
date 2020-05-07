using System;
using AppShapes.Core.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class UseControllersCommandTests
    {
        [Fact]
        public void ExecuteMustUseRoutingWhenCalled()
        {
            ServiceCollection services = new ServiceCollection();
            new ConfigureRoutingCommand().Execute(services);
            IServiceProvider provider = services.BuildServiceProvider();
            IApplicationBuilder app = new ApplicationBuilder(provider);
            new UseControllersCommand().Execute(app, "Test");
            Assert.Contains(app.Properties, x => x.Key == "__EndpointRouteBuilder");
        }
    }
}