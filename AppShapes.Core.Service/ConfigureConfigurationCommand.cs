using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AppShapes.Core.Service
{
    public class ConfigureConfigurationCommand
    {
        public virtual void Execute(IServiceCollection services, IConfiguration configuration, Action<IServiceCollection, IConfiguration> configurationOptions = null)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<AppSettings>>().Value);
            configurationOptions?.Invoke(services, configuration);
        }
    }
}