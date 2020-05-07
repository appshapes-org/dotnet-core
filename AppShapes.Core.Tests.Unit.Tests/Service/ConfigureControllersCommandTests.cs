using AppShapes.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureControllersCommandTests
    {
        [Fact]
        public void CreateMustAddControllersAndCallControllerOptionsWhenControllerOptionsIsNotNull()
        {
            int controllerOptionsCalled = 0;
            IServiceCollection services = new ServiceCollection();
            new ConfigureControllersCommand().Create(services, _ => ++controllerOptionsCalled);
            services.BuildServiceProvider().GetRequiredService<IConfigureOptions<MvcOptions>>().Configure(new MvcOptions());
            Assert.True(controllerOptionsCalled == 1);
        }

        [Fact]
        public void CreateMustAddControllersWhenControllerOptionsIsNull()
        {
            IServiceCollection services = new ServiceCollection();
            Assert.Empty(services);
            new ConfigureControllersCommand().Create(services);
            Assert.NotEmpty(services);
        }
    }
}