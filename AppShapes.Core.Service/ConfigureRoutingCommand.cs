using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Service
{
    public class ConfigureRoutingCommand
    {
        public virtual void Execute(IServiceCollection services, Action<RouteOptions> routingOptions = null)
        {
            if (routingOptions == null)
                services.AddRouting();
            else
                services.AddRouting(routingOptions);
            services.AddResponseCaching();
        }
    }
}