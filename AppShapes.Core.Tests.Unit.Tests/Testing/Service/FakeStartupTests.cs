using AppShapes.Core.Testing.Logging;
using AppShapes.Core.Testing.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class FakeStartupTests
    {
        [Fact]
        public void ConfigureMustDoNothingWhenCalled()
        {
            new FakeStartup().Configure(null);
            new FakeStartup().Configure(new ApplicationBuilder(new ServiceCollection().BuildServiceProvider()));
        }

        [Fact]
        public void ConfigureServicesMustBuildServiceProviderWhenCalled()
        {
            Assert.NotNull(new FakeStartup().ConfigureServices(new ServiceCollection().AddSingleton(new MockLogger())).GetService<MockLogger>());
        }
    }
}