using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppShapes.Core.Logging;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Core;
using AppShapes.Core.Testing.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class UseRoutingCommandTests
    {
        [Fact]
        public void ExecuteMustUseEndpointsWhenCalled()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().Build();
            ServiceCollection services = new ServiceCollection();
            new ConfigureControllersCommand().Create(services, options => new ConfigureFiltersCommand().Execute(options.Filters));
            new ConfigureCorsCommand().Execute(services, options => options.AddPolicy("AllowAnyPolicy", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
            new ConfigureRoutingCommand().Execute(services);
            new ConfigureConfigurationCommand().Execute(services, configuration);
            new ConfigureLoggingCommand().Execute(services, configuration);
            services.AddSingleton(new DiagnosticListener("Test"));
            IApplicationBuilder app = new ApplicationBuilder(services.BuildServiceProvider());
            new UseControllersCommand().Execute(app, "Test");
            new UseRoutingCommand().Execute(app, new FakeHostingEnvironment(), x => x.MapControllers());
            RequestDelegate requestDelegate = app.Build();
            List<object> middleware = new List<object>();
            while (requestDelegate != null)
            {
                middleware.Add(requestDelegate.Target);
                requestDelegate = ReflectionHelper.GetFieldOrDefault(requestDelegate.Target, "_next") as RequestDelegate;
            }

            Assert.Contains(middleware, x => x.GetType() == typeof(ResponseCachingMiddleware));
            Assert.Contains(middleware, x => x.GetType().Name == "EndpointRoutingMiddleware");
        }
    }
}