using AppShapes.Core.Testing.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Logging
{
    public class MockLoggerTests
    {
        [Fact]
        public void BeginScopeMustReturnNullScopeWhenCalled()
        {
            Assert.Same(NullScope.Instance, new MockLogger().BeginScope<string>(null));
        }

        [Fact]
        public void IsEnabledFunctionMustReturnValueWhenSet()
        {
            static bool IsEnabled(LogLevel _)
            {
                return false;
            }

            MockLogger logger = new MockLogger {IsEnabledFunction = IsEnabled};
            Assert.Equal(IsEnabled, logger.IsEnabledFunction);
        }

        [Theory]
        [InlineData(LogLevel.None)]
        [InlineData(LogLevel.Critical)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Error)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Warning)]
        public void IsEnabledMustReturnTrueWhenIsEnabledFunctionHasNotBeenSet(LogLevel level)
        {
            Assert.True(new MockLogger().IsEnabled(level));
        }

        [Fact]
        public void LogMustAddLogEventToStorageWhenCalled()
        {
            MockLogger logger = new MockLogger<MockLoggerTests>();
            logger.Log(LogLevel.Information, 0, "42", null, null);
            Assert.Contains(logger.Storage, x => x == "Information: 42");
        }
    }
}