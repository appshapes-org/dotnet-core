using Microsoft.Extensions.Configuration;

namespace AppShapes.Core.Console
{
    public class ConfigureConfigurationBuilderCommand
    {
        public virtual IConfigurationBuilder Execute(ProgramOptions context)
        {
            return new ConfigurationBuilder().SetBasePath(context.BasePath).AddJsonFile("appsettings.json", true, true).AddJsonFile($"appsettings.{context.Environment}.json", true, true).AddEnvironmentVariables();
        }
    }
}