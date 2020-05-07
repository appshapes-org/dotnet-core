using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using AppShapes.Core.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Logging
{
    public class LoggingContextTests
    {
        [Fact]
        public void ActivityIdMustReturnActivityIdWhenActivityIdIsSet()
        {
            Activity activity = new Activity(nameof(ActivityIdMustReturnActivityIdWhenActivityIdIsSet)).Start();
            using (new Disposable(() => activity.Stop()))
            {
                Assert.Equal(activity.Id, new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test").ActivityId);
            }
        }

        [Fact]
        public void ApplicationNameMustReturnApplicationNameWhenCalled()
        {
            Assert.Equal(AppDomain.CurrentDomain.FriendlyName, new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test").ApplicationName);
        }

        [Fact]
        public void EnvironmentNameMustReturnEmptyWhenEnvironmentNameIsNotSet()
        {
            Assert.Equal(string.Empty, new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test").EnvironmentName);
        }

        [Fact]
        public void ExceptionMustReturnExceptionWhenExceptionIsSet()
        {
            Assert.Equal("42", new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test") {Exception = "42"}.Exception);
        }

        [Fact]
        public void LoggerNameMustReturnLoggerNameWhenLoggerNameIsSet()
        {
            Assert.Equal(nameof(LoggingContextTests), new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, nameof(LoggerNameMustReturnLoggerNameWhenLoggerNameIsSet)).LoggerName);
        }

        [Fact]
        public void LogLevelMustReturnLogLevelWhenLogLevelIsSet()
        {
            Assert.Equal(LogLevel.Information, new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test").LogLevel);
        }

        [Fact]
        public void MemberNameMustReturnMemberNameWhenMemberNameIsSet()
        {
            Assert.Equal("Test", new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test").MemberName);
        }

        [Fact]
        public void MessageMustReturnMessageWhenMessageIsSet()
        {
            Assert.Equal("42", new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test") {Message = "42"}.Message);
        }

        [Fact]
        public void OrganizationIdMustReturnOrganizationIdWhenOrganizationIdIsSet()
        {
            Activity activity = new Activity(nameof(OrganizationIdMustReturnOrganizationIdWhenOrganizationIdIsSet)).Start();
            using (new Disposable(() => activity.Stop()))
            {
                activity.AddBaggage("OrganizationId", $"{42}");
                Assert.Equal("42", new LoggingContext(nameof(LoggingContextTests), LogLevel.Information, "Test").OrganizationId);
            }
        }

        [Fact]
        public void ThreadNameMustReturnThreadIdWhenThreadNameIsNotSet()
        {
            Assert.Equal($"{Thread.CurrentThread.ManagedThreadId}", new LoggingContext("Test", LogLevel.Information, nameof(ThreadNameMustReturnThreadIdWhenThreadNameIsNotSet)).ThreadName);
        }

        [Fact]
        public void TimestampMustBeSet()
        {
            Assert.NotEqual(default, new LoggingContext("Test", LogLevel.Information, nameof(TimestampMustBeSet)).Timestamp);
        }

        [Fact]
        public void ToStringMustSerializeLoggingContextToJsonWhenCalled()
        {
            Activity activity = new Activity(nameof(ToStringMustSerializeLoggingContextToJsonWhenCalled)).Start();
            activity.AddBaggage("UserId", $"{42}");
            activity.AddBaggage("OrganizationId", $"{42}");
            using (new Disposable(() => activity.Stop()))
            {
                LoggingContext expected = new LoggingContext("Test", LogLevel.Information, nameof(ToStringMustSerializeLoggingContextToJsonWhenCalled)) {Exception = $"{new Exception("42")}", Message = "42"};
                string value = $"{expected}";
                LoggingContext actual = JsonSerializer.Deserialize<LoggingContext>(value, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
                Assert.Equal(expected.ActivityId, actual.ActivityId);
                Assert.Equal(expected.ApplicationName, actual.ApplicationName);
                Assert.Equal(expected.EnvironmentName, actual.EnvironmentName);
                Assert.Equal(expected.Exception, actual.Exception);
                Assert.Equal(expected.LoggerName, actual.LoggerName);
                Assert.Equal(expected.LogLevel, actual.LogLevel);
                Assert.Equal(expected.MemberName, actual.MemberName);
                Assert.Equal(expected.Message, actual.Message);
                Assert.Equal(expected.OrganizationId, actual.OrganizationId);
                Assert.Equal(expected.ThreadName, actual.ThreadName);
                Assert.Equal(expected.Timestamp, actual.Timestamp);
            }
        }

        private class Disposable : IDisposable
        {
            public Disposable(Action disposeAction)
            {
                DisposeAction = disposeAction;
            }

            public void Dispose()
            {
                DisposeAction();
            }

            private Action DisposeAction { get; }
        }
    }
}