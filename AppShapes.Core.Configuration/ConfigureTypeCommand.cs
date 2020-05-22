using System;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Configuration
{
    public class ConfigureTypeCommand
    {
        public virtual void Execute<T>(IServiceCollection services, string assemblyQualifiedName, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.Add(new ServiceDescriptor(typeof(T), Type.GetType(assemblyQualifiedName, true), lifetime));
        }
    }
}