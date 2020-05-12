using System;
using AppShapes.Core.Testing.Core;

namespace AppShapes.Core.Testing.Service
{
    public abstract class ServiceTestsBase
    {
        protected ServiceTestsBase()
        {
            Assertions.True(bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out bool result) && result, "Use docker-compose to run system tests.");
        }
    }
}