using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Service
{
    public class ConfigureHealthChecksCommand
    {
        public virtual IHealthChecksBuilder Execute(IServiceCollection services)
        {
            return services.AddHealthChecks();
        }
    }
}