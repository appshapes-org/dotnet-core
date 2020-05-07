using System;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Logging;
using AppShapes.Core.Testing.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ProgramBaseTests
    {
        [Fact]
        public void GetWebHostMustConfigureConfigurationWhenCalled()
        {
            MockProgram program = new MockProgram();
            IWebHost host = program.InvokeGetWebHost(new string[] { });
            Assert.NotNull(host.Services.GetService<IConfiguration>());
        }

        [Fact]
        public void GetWebHostMustConfigureLoggingWhenCalled()
        {
            MockProgram program = new MockProgram();
            IWebHost host = program.InvokeGetWebHost(new string[] { });
            Assert.NotNull(host.Services.GetService<ILogger<ProgramBase>>());
        }

        [Fact]
        public void InitializeMustInitializeLoggingWhenCalled()
        {
            MockProgram program = new MockProgram();
            IWebHost host = program.InvokeGetWebHost(new string[] { });
            program.InvokeInitialize(host.Services);
            Assert.NotNull(program.LoggerGetter);
        }

        [Fact]
        public void RunMustRunWebHostWhenCalled()
        {
            MockProgram program = new MockProgram();
            program.Run(new string[] { });
            Assert.Contains(program.LoggerGetter.Storage, x => x.StartsWith("Information: starting"));
            Assert.Contains(program.LoggerGetter.Storage, x => x.StartsWith("Information: finished"));
        }

        private class MockProgram : ProgramBase
        {
            public MockProgram(IWebHostBuilder webHostBuilder = null)
            {
                WebHostBuilder = webHostBuilder ?? new MockWebHostBuilder();
            }

            public IWebHost InvokeGetWebHost(string[] args)
            {
                return GetWebHost(args);
            }

            public void InvokeInitialize(IServiceProvider provider)
            {
                Initialize(provider);
            }

            public MockLogger LoggerGetter => (MockLogger) ReflectionHelper.GetField(Logger, "_logger");

            protected override IWebHost GetWebHost(string[] args)
            {
                return new MockWebHost(base.GetWebHost(args));
            }

            protected override IWebHostBuilder GetWebHostBuilder(string[] args)
            {
                return WebHostBuilder;
            }

            private IWebHostBuilder WebHostBuilder { get; }
        }
    }
}