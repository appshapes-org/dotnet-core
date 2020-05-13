using System;
using System.Collections;
using AppShapes.Core.Testing.Service;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Service
{
    public class ServiceTestsBaseTests
    {
        [Fact]
        public void ConstructorMustNotThrowExceptionWhenRunningInContainer()
        {
            new FakeServiceTestsRunningInContainerIsTrue();
        }

        [Fact]
        public void ConstructorMustThrowExceptionWhenRunningInContainerIsFalse()
        {
            Assert.Equal("Use docker-compose to run system tests.", Assert.Throws<InvalidOperationException>(() => new FakeServiceTestsRunningInContainerIsFalse()).Message);
        }

        [Fact]
        public void ConstructorMustThrowExceptionWhenRunningInContainerIsNotSet()
        {
            Assert.Equal("Use docker-compose to run system tests.", Assert.Throws<InvalidOperationException>(() => new FakeServiceTestsRunningInContainerNotSet()).Message);
        }

        private class FakeServiceTestsRunningInContainerIsFalse : ServiceTestsBase
        {
            protected override IDictionary GetEnvironmentVariables()
            {
                IDictionary environment = base.GetEnvironmentVariables();
                environment["DOTNET_RUNNING_IN_CONTAINER"] = "false";
                return environment;
            }
        }

        private class FakeServiceTestsRunningInContainerIsTrue : ServiceTestsBase
        {
            protected override IDictionary GetEnvironmentVariables()
            {
                IDictionary environment = base.GetEnvironmentVariables();
                environment["DOTNET_RUNNING_IN_CONTAINER"] = "true";
                return environment;
            }
        }

        private class FakeServiceTestsRunningInContainerNotSet : ServiceTestsBase
        {
            protected override IDictionary GetEnvironmentVariables()
            {
                IDictionary environment = base.GetEnvironmentVariables();
                environment.Remove("DOTNET_RUNNING_IN_CONTAINER");
                return environment;
            }
        }
    }
}