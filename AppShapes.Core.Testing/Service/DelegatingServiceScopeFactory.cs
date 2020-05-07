using System;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Testing.Service
{
    public class DelegatingServiceScopeFactory : IServiceScopeFactory
    {
        public DelegatingServiceScopeFactory(Func<IServiceScope> getServiceScope)
        {
            GetServiceScope = getServiceScope;
        }

        public IServiceScope CreateScope()
        {
            return GetServiceScope();
        }

        private Func<IServiceScope> GetServiceScope { get; }
    }
}