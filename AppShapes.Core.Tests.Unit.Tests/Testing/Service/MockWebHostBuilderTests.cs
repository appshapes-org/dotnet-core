using System;
using AppShapes.Core.Testing.Logging;
using AppShapes.Core.Testing.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class MockWebHostBuilderTests
    {
        [Fact]
        public void ConfigureAppConfigurationMustRegisterConfigureDelegateWhenCalled()
        {
            int configureServicesCalled = 0;
            new MockWebHostBuilder().ConfigureAppConfiguration((_, __) => { ++configureServicesCalled; }).Build();
            Assert.Equal(1, configureServicesCalled);
        }

        [Fact]
        public void ConfigureServicesMustConfigureMockLoggerFactoryWhenCalled()
        {
            MockWebHostBuilder builder = new MockWebHostBuilder();
            IWebHost host = builder.ConfigureServices((_, __) => { }).Build();
            Assert.IsAssignableFrom<MockLoggerFactory>(host.Services.GetRequiredService<ILoggerFactory>());
        }

        [Fact]
        public void ConfigureServicesMustRegisterConfigureServicesDelegateWhenCalled()
        {
            MockWebHostBuilder builder = new MockWebHostBuilder();
            int configureServicesCalled = 0;
            builder.ConfigureServices(_ => { ++configureServicesCalled; }).Build();
            Assert.Equal(1, configureServicesCalled);
        }

        [Fact]
        public void ConfigureServicesMustRegisterConfigureServicesDelegateWithWebHostBuilderContextWhenCalled()
        {
            MockWebHostBuilder builder = new MockWebHostBuilder();
            int configureServicesCalled = 0;
            builder.ConfigureServices((_, __) => { ++configureServicesCalled; }).Build();
            Assert.Equal(1, configureServicesCalled);
        }

        [Fact]
        public void ConfigureServicesMustSetMockLoggerWhenCalled()
        {
            MockWebHostBuilder builder = new MockWebHostBuilder();
            builder.ConfigureServices((_, __) => { });
            Assert.NotNull(builder.MockLogger);
        }

        [Fact]
        public void ConstructorMustSetWebHostBuilderToDefaultWebHostBuilderWhenWebHostBuilderIsNull()
        {
            Assert.IsAssignableFrom<FakeStartup>(new MockWebHostBuilder().Build().Services.GetService<IStartup>());
        }

        [Fact]
        public void ConstructorMustSetWebHostBuilderToWebHostBuilderWhenWebHostBuilderIsNotNull()
        {
            IWebHostBuilder builder = WebHost.CreateDefaultBuilder<NullStartup>(null).ConfigureLogging(x => x.ClearProviders());
            Assert.IsAssignableFrom<NullStartup>(new MockWebHostBuilder(builder).Build().Services.GetService<IStartup>());
        }

        [Fact]
        public void GetSettingMustReturnValueWhenKeyExists()
        {
            MockWebHostBuilder builder = new MockWebHostBuilder();
            builder.UseSetting("secret", "42");
            Assert.Equal("42", builder.GetSetting("secret"));
        }

        private class NullStartup : IStartup
        {
            public void Configure(IApplicationBuilder app)
            {
            }

            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                return services.BuildServiceProvider();
            }
        }
    }
}