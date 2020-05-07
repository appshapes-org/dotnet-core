using System;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Service
{
    public class ConfigureCorsCommand
    {
        public virtual void Execute(IServiceCollection services, Action<CorsOptions> corsOptions)
        {
            services.AddCors(corsOptions);
        }
    }
}