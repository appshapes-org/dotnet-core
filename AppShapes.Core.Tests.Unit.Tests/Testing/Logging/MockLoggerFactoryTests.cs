using System;
using AppShapes.Core.Testing.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Logging
{
    public class MockLoggerFactoryTests
    {
        [Fact]
        public void AddProviderMustDoNothingWhenCalled()
        {
            using MockLoggerFactory factory = new MockLoggerFactory(null);
            factory.AddProvider(null);
        }

        [Fact]
        public void CreateLoggerMustReturnILoggerWhenLoggerFactoryIsNotNull()
        {
            NamedLogger logger = new MockLoggerFactory(x => new NamedLogger {Name = x}).CreateLogger("42") as NamedLogger;
            Assert.NotNull(logger);
            Assert.Equal("42", logger.Name);
        }

        [Fact]
        public void CreateLoggerMustReturnMockLoggerWhenLoggerFactoryIsNull()
        {
            Assert.IsAssignableFrom<MockLogger>(new MockLoggerFactory(null).CreateLogger(null));
        }

        private class NamedLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return NullScope.Instance;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return false;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
            }

            public string Name { get; set; }
        }
    }
}