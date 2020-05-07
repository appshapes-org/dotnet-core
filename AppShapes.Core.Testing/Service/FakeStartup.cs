using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Testing.Service
{
    public class FakeStartup : IStartup
    {
        public void Configure(IApplicationBuilder app)
        {
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}