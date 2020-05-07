using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Testing.Logging
{
    public class MockLogger : ILogger
    {
        public MockLogger()
        {
            IsEnabledFunction = _ => true;
        }

        public virtual IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public virtual bool IsEnabled(LogLevel logLevel)
        {
            return IsEnabledFunction(logLevel);
        }

        public virtual Func<LogLevel, bool> IsEnabledFunction { get; set; }

        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Storage.Add($"{logLevel}: {state}");
        }

        public List<string> Storage { get; } = new List<string>();
    }

    public class MockLogger<T> : MockLogger, ILogger<T>
    {
    }
}