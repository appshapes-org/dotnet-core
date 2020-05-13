using System;
using System.Collections;
using AppShapes.Core.Testing.Core;

namespace AppShapes.Core.Testing.Service
{
    public abstract class ServiceTestsBase
    {
        protected ServiceTestsBase()
        {
            Assertions.True(IsRunningInContainer(), "Use docker-compose to run system tests.");
        }

        protected virtual IDictionary GetEnvironmentVariables()
        {
            return Environment.GetEnvironmentVariables();
        }

        protected virtual bool IsRunningInContainer()
        {
            IDictionary environment = GetEnvironmentVariables();
            return environment.Contains("DOTNET_RUNNING_IN_CONTAINER") && bool.TryParse(environment["DOTNET_RUNNING_IN_CONTAINER"] as string, out bool result) && result;
        }
    }
}