using AppShapes.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ConfigureDocumentationCommandTests
    {
        [Fact]
        public void ExecuteMustAddSwaggerAndCallSwaggerOptionsWhenCalled()
        {
            int optionsCalled = 0;
            ServiceCollection services = new ServiceCollection();
            new ConfigureDocumentationCommand().Execute(services, _ => ++optionsCalled);
            services.BuildServiceProvider().GetRequiredService<IConfigureOptions<SwaggerGenOptions>>().Configure(new SwaggerGenOptions());
            Assert.True(optionsCalled == 1);
        }
    }
}