using System;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AppShapes.Core.Service
{
    public class ConfigureDocumentationCommand
    {
        public virtual void Execute(IServiceCollection services, Action<SwaggerGenOptions> swaggerOptions)
        {
            services.AddSwaggerGen(swaggerOptions);
        }
    }
}