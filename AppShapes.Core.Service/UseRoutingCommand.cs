using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;

namespace AppShapes.Core.Service
{
    public class UseRoutingCommand
    {
        public virtual void Execute(IApplicationBuilder app, IWebHostEnvironment environment, Action<IEndpointRouteBuilder> routingOptions)
        {
            app.UseEndpoints(routingOptions);
        }
    }
}