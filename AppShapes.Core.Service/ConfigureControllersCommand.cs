using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Service
{
    public class ConfigureControllersCommand
    {
        public virtual IMvcBuilder Create(IServiceCollection services, Action<MvcOptions> controllerOptions = null)
        {
            return controllerOptions == null ? services.AddControllers() : services.AddControllers(controllerOptions);
        }
    }
}