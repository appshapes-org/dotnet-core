using System;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Testing.Logging
{
    public class MockLoggerFactory : ILoggerFactory
    {
        public MockLoggerFactory(Func<string, ILogger> loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }

        public virtual void AddProvider(ILoggerProvider provider)
        {
        }

        public virtual ILogger CreateLogger(string categoryName)
        {
            return LoggerFactory == null ? new MockLogger() : LoggerFactory(categoryName);
        }

        public virtual void Dispose()
        {
        }

        private Func<string, ILogger> LoggerFactory { get; }
    }
}