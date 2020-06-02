using System;

namespace AppShapes.Core.Console
{
    public class ConfigureEnvironmentCommand
    {
        public virtual string Execute(string environment)
        {
            if (!string.IsNullOrWhiteSpace(environment))
            {
                Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", environment);
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
                return environment;
            }

            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")))
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);
                return Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            }

            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", environment);
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            }

            throw new InvalidOperationException("Environment must be set (either ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT).");
        }
    }
}