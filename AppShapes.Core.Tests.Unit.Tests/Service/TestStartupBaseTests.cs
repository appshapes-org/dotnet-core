using System;
using AppShapes.Core.Testing.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class TestStartupBaseTests
    {
        [Fact]
        public void ConfigureServicesMustAddControllersWhenCalled()
        {
            IServiceProvider provider = new TestStartup().ConfigureServices(new ServiceCollection());
            Assert.NotNull(provider.GetService<IConfigureOptions<MvcOptions>>());
        }

        [Fact]
        public void ConfigureServicesMustSetRoutingOptionsToLowercaseUrlsWhenCalled()
        {
            IServiceProvider provider = new TestStartup().ConfigureServices(new ServiceCollection());
            Assert.True(provider.GetRequiredService<IOptions<RouteOptions>>().Value.LowercaseUrls);
        }

        private class TestStartup : TestStartupBase
        {
        }
    }
}