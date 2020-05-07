using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AppShapes.Core.Service
{
    public class ConfigurationBuilderFactory
    {
        public virtual IConfigurationBuilder Create(IConfiguration configuration, IWebHostEnvironment environment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(environment.ContentRootPath).AddJsonFile("appsettings.json", true, true).AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true).AddEnvironmentVariables();
            return configuration == null ? builder : builder.AddConfiguration(configuration);
        }
    }
}