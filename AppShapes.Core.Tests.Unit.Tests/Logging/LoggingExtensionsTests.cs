using System;
using AppShapes.Core.Logging;
using AppShapes.Core.Testing.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Logging
{
    public class LoggingExtensionsTests
    {
        [Fact]
        public void DebugMustLogDebugWhenCalled()
        {
            MockLogger logger = new MockLogger();
            logger.Debug<LoggingExtensionsTests>("42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains(logger.Storage, x => x.StartsWith($"Debug: 42 ({{\"activityId\":\"\",\"applicationName\":\"{new RuntimeContext().ApplicationName}\",\"environmentName\":\"\",\"loggerName\":\"{typeof(LoggingExtensionsTests).FullName}\",\"logLevel\":1,\"memberName\":\"{nameof(DebugMustLogDebugWhenCalled)}\",\"message\":\"42\",\"organizationId\":\"\""));
            Assert.Contains("threadName", logger.Storage[0]);
            Assert.Contains("timestamp", logger.Storage[0]);
            Assert.Contains("userId", logger.Storage[0]);
        }

        [Fact]
        public void DebugWithExceptionMustLogDebugWhenCalled()
        {
            MockLogger logger = new MockLogger();
            Exception exception = new Exception("Test");
            logger.Debug<LoggingExtensionsTests>(exception, "42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains("System.Exception: Test", logger.Storage[0]);
        }

        [Fact]
        public void ErrorMustLogErrorWhenCalled()
        {
            MockLogger logger = new MockLogger();
            logger.Error<LoggingExtensionsTests>("42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains(logger.Storage, x => x.StartsWith($"Error: 42 ({{\"activityId\":\"\",\"applicationName\":\"{new RuntimeContext().ApplicationName}\",\"environmentName\":\"\",\"loggerName\":\"{typeof(LoggingExtensionsTests).FullName}\",\"logLevel\":4,\"memberName\":\"{nameof(ErrorMustLogErrorWhenCalled)}\",\"message\":\"42\",\"organizationId\":\"\""));
            Assert.Contains("threadName", logger.Storage[0]);
            Assert.Contains("timestamp", logger.Storage[0]);
            Assert.Contains("userId", logger.Storage[0]);
        }

        [Fact]
        public void ErrorWithExceptionMustLogErrorWhenCalled()
        {
            MockLogger logger = new MockLogger();
            Exception exception = new Exception("Test");
            logger.Error<LoggingExtensionsTests>(exception, "42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains("System.Exception: Test", logger.Storage[0]);
        }

        [Fact]
        public void InformationMustLogInformationWhenCalled()
        {
            MockLogger logger = new MockLogger();
            logger.Information<LoggingExtensionsTests>("42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains(logger.Storage, x => x.StartsWith($"Information: 42 ({{\"activityId\":\"\",\"applicationName\":\"{new RuntimeContext().ApplicationName}\",\"environmentName\":\"\",\"loggerName\":\"{typeof(LoggingExtensionsTests).FullName}\",\"logLevel\":2,\"memberName\":\"{nameof(InformationMustLogInformationWhenCalled)}\",\"message\":\"42\",\"organizationId\":\"\""));
            Assert.Contains("threadName", logger.Storage[0]);
            Assert.Contains("timestamp", logger.Storage[0]);
            Assert.Contains("userId", logger.Storage[0]);
        }

        [Fact]
        public void InformationWithExceptionMustLogInformationWhenCalled()
        {
            MockLogger logger = new MockLogger();
            Exception exception = new Exception("Test");
            logger.Information<LoggingExtensionsTests>(exception, "42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains("System.Exception: Test", logger.Storage[0]);
        }

        [Fact]
        public void TraceMustLogTraceWhenCalled()
        {
            MockLogger logger = new MockLogger();
            logger.Trace<LoggingExtensionsTests>("42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains(logger.Storage, x => x.StartsWith($"Trace: 42 ({{\"activityId\":\"\",\"applicationName\":\"{new RuntimeContext().ApplicationName}\",\"environmentName\":\"\",\"loggerName\":\"{typeof(LoggingExtensionsTests).FullName}\",\"logLevel\":0,\"memberName\":\"{nameof(TraceMustLogTraceWhenCalled)}\",\"message\":\"42\",\"organizationId\":\"\""));
            Assert.Contains("threadName", logger.Storage[0]);
            Assert.Contains("timestamp", logger.Storage[0]);
            Assert.Contains("userId", logger.Storage[0]);
        }

        [Fact]
        public void TraceWithExceptionMustLogTraceWhenCalled()
        {
            MockLogger logger = new MockLogger();
            Exception exception = new Exception("Test");
            logger.Trace<LoggingExtensionsTests>(exception, "42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains("System.Exception: Test", logger.Storage[0]);
        }

        [Fact]
        public void WarningMustLogWarningWhenCalled()
        {
            MockLogger logger = new MockLogger();
            logger.Warning<LoggingExtensionsTests>("42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains(logger.Storage, x => x.StartsWith($"Warning: 42 ({{\"activityId\":\"\",\"applicationName\":\"{new RuntimeContext().ApplicationName}\",\"environmentName\":\"\",\"loggerName\":\"{typeof(LoggingExtensionsTests).FullName}\",\"logLevel\":3,\"memberName\":\"{nameof(WarningMustLogWarningWhenCalled)}\",\"message\":\"42\",\"organizationId\":\"\""));
            Assert.Contains("threadName", logger.Storage[0]);
            Assert.Contains("timestamp", logger.Storage[0]);
            Assert.Contains("userId", logger.Storage[0]);
        }

        [Fact]
        public void WarningWithExceptionMustLogWarningWhenCalled()
        {
            MockLogger logger = new MockLogger();
            Exception exception = new Exception("Test");
            logger.Warning<LoggingExtensionsTests>(exception, "42");
            Assert.True(logger.Storage.Count == 1);
            Assert.Contains("System.Exception: Test", logger.Storage[0]);
        }
    }
}