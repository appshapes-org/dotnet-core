using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AppShapes.Core.Configuration
{
    public class ConfigureSettingsCommand
    {
        public virtual void Execute<T>(IServiceCollection services, IConfiguration configuration) where T : class, new()
        {
            services.Configure<T>(configuration.GetSection(nameof(T)));
            services.AddSingleton(x => x.GetRequiredService<IOptions<T>>().Value);
        }
    }
}