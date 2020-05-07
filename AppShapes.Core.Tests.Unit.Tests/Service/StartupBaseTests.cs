using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AppShapes.Core.Service;
using AppShapes.Core.Testing.Core;
using AppShapes.Core.Testing.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Xunit;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class StartupBaseTests
    {
        [Fact]
        public void ConfigureMustConfigureCorsMiddlewareWithAllowAnyPolicyWhenEnvironmentIsNotProduction()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            IWebHostEnvironment environment = new FakeHostingEnvironment();
            StubStartup startup = new StubStartup(configuration, environment);
            startup.ConfigureServices(services);
            DiagnosticListener listener = new DiagnosticListener("Test");
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);
            services.AddSingleton(environment);
            IApplicationBuilder app = new ApplicationBuilder(services.BuildServiceProvider());
            startup.Configure(app, environment);
            List<Func<RequestDelegate, RequestDelegate>> delegates = ReflectionHelper.GetFieldOrDefault(app, "_components") as List<Func<RequestDelegate, RequestDelegate>>;
            Assert.NotNull(delegates);
            Func<RequestDelegate, RequestDelegate> cors = delegates.FirstOrDefault(x => (ReflectionHelper.GetFieldOrDefault(x.Target, "middleware") as Type)?.Name == "CorsMiddleware");
            Assert.NotNull(cors);
            object[] args = ReflectionHelper.GetFieldOrDefault(cors.Target, "args") as object[];
            Assert.NotNull(args);
            Assert.Contains(args, x => Equals(x, "AllowAnyPolicy"));
        }

        [Fact]
        public void ConfigureMustConfigureCorsMiddlewareWithAllowAnyPolicyWhenEnvironmentIsProduction()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            IWebHostEnvironment environment = new FakeHostingEnvironment {EnvironmentName = "Production"};
            StubStartup startup = new StubStartup(configuration, environment);
            startup.ConfigureServices(services);
            DiagnosticListener listener = new DiagnosticListener("Test");
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);
            services.AddSingleton(environment);
            IApplicationBuilder app = new ApplicationBuilder(services.BuildServiceProvider());
            startup.Configure(app, environment);
            List<Func<RequestDelegate, RequestDelegate>> delegates = ReflectionHelper.GetFieldOrDefault(app, "_components") as List<Func<RequestDelegate, RequestDelegate>>;
            Assert.NotNull(delegates);
            Func<RequestDelegate, RequestDelegate> cors = delegates.FirstOrDefault(x => (ReflectionHelper.GetFieldOrDefault(x.Target, "middleware") as Type)?.Name == "CorsMiddleware");
            Assert.NotNull(cors);
            object[] args = ReflectionHelper.GetFieldOrDefault(cors.Target, "args") as object[];
            Assert.NotNull(args);
            Assert.Contains(args, x => Equals(x, "AllowAnyPolicy"));
        }

        [Fact]
        public void ConfigureMustConfigureHealthCheckRouteWhenCalled()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            IWebHostEnvironment environment = new FakeHostingEnvironment();
            StubStartup startup = new StubStartup(configuration, environment);
            startup.ConfigureServices(services);
            DiagnosticListener listener = new DiagnosticListener("Test");
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);
            services.AddSingleton(environment);
            IApplicationBuilder app = new ApplicationBuilder(services.BuildServiceProvider());
            startup.Configure(app, environment);
            object property = app.Properties["__EndpointRouteBuilder"];
            Assert.NotNull(property);
            List<EndpointDataSource> dataSources = ReflectionHelper.GetProperty<List<EndpointDataSource>>(property, "DataSources");
            Assert.NotNull(dataSources.FirstOrDefault(x => x.Endpoints.Any(y => (y as RouteEndpoint)?.RoutePattern.RawText == "/health")));
        }

        [Fact]
        public void ConfigureMustConfigureHstsWhenEnvironmentIsProduction()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            IWebHostEnvironment environment = new FakeHostingEnvironment {EnvironmentName = "Production"};
            StubStartup startup = new StubStartup(configuration, environment);
            startup.ConfigureServices(services);
            DiagnosticListener listener = new DiagnosticListener("Test");
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);
            services.AddSingleton(environment);
            IApplicationBuilder app = new ApplicationBuilder(services.BuildServiceProvider());
            startup.Configure(app, environment);
            List<Func<RequestDelegate, RequestDelegate>> delegates = ReflectionHelper.GetFieldOrDefault(app, "_components") as List<Func<RequestDelegate, RequestDelegate>>;
            Assert.NotNull(delegates);
            Assert.NotNull(delegates.FirstOrDefault(x => (ReflectionHelper.GetFieldOrDefault(x.Target, "middleware") as Type)?.Name == "HstsMiddleware"));
        }

        [Fact]
        public void ConfigureMustConfigureSwaggerUIMiddlewareWhenCalled()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            IWebHostEnvironment environment = new FakeHostingEnvironment();
            StubStartup startup = new StubStartup(configuration, environment);
            startup.ConfigureServices(services);
            DiagnosticListener listener = new DiagnosticListener("Test");
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);
            services.AddSingleton(environment);
            IApplicationBuilder app = new ApplicationBuilder(services.BuildServiceProvider());
            startup.Configure(app, environment);
            List<Func<RequestDelegate, RequestDelegate>> delegates = ReflectionHelper.GetFieldOrDefault(app, "_components") as List<Func<RequestDelegate, RequestDelegate>>;
            Assert.NotNull(delegates);
            Func<RequestDelegate, RequestDelegate> requestDelegate = delegates.FirstOrDefault(x => ReflectionHelper.GetFieldOrDefault(x.Target, "middleware") as Type == typeof(SwaggerUIMiddleware));
            Assert.NotNull(requestDelegate);
            object[] args = ReflectionHelper.GetFieldOrDefault(requestDelegate.Target, "args") as object[];
            Assert.NotNull(args);
            SwaggerUIOptions options = args.FirstOrDefault(x => x is SwaggerUIOptions) as SwaggerUIOptions;
            Assert.NotNull(options);
            UrlDescriptor descriptor = options.ConfigObject.Urls.FirstOrDefault(y => y.Url == "/swagger/v1/swagger.json");
            Assert.NotNull(descriptor);
            Assert.Equal("testhost API V1", descriptor.Name);
        }

        [Fact]
        public void ConfigureMustNotConfigureHstsWhenEnvironmentIsNotProduction()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            IWebHostEnvironment environment = new FakeHostingEnvironment();
            StubStartup startup = new StubStartup(configuration, environment);
            startup.ConfigureServices(services);
            DiagnosticListener listener = new DiagnosticListener("Test");
            services.AddSingleton(listener);
            services.AddSingleton<DiagnosticSource>(listener);
            services.AddSingleton(environment);
            IApplicationBuilder app = new ApplicationBuilder(services.BuildServiceProvider());
            startup.Configure(app, environment);
            List<Func<RequestDelegate, RequestDelegate>> delegates = ReflectionHelper.GetFieldOrDefault(app, "_components") as List<Func<RequestDelegate, RequestDelegate>>;
            Assert.NotNull(delegates);
            Assert.Null(delegates.FirstOrDefault(x => (ReflectionHelper.GetFieldOrDefault(x.Target, "middleware") as Type)?.Name == "HstsMiddleware"));
        }

        [Fact]
        public void ConfigureServicesMustAddPolicyToCorsOptionsWhenCalled()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            new StubStartup(configuration, new FakeHostingEnvironment()).ConfigureServices(services);
            CorsOptions options = new CorsOptions();
            services.BuildServiceProvider().GetRequiredService<IConfigureOptions<CorsOptions>>().Configure(options);
            IDictionary policyMap = ReflectionHelper.GetProperty<IDictionary>(options, "PolicyMap");
            List<string> keys = new List<string>((IEnumerable<string>) policyMap.Keys);
            Assert.True(keys.Count == 1);
            Assert.Equal("AllowAnyPolicy", keys[0]);
        }

        [Fact]
        public void ConfigureServicesMustConfigureDocumentationWhenEnvironmentIsNotProduction()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            new StubStartup(configuration, new FakeHostingEnvironment()).ConfigureServices(services);
            SwaggerGenOptions options = new SwaggerGenOptions();
            services.BuildServiceProvider().GetRequiredService<IConfigureOptions<SwaggerGenOptions>>().Configure(options);
            Assert.Equal("testhost API", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Title);
            Assert.Equal("v1", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Version);
            OpenApiSecurityScheme scheme = options.SwaggerGeneratorOptions.SecuritySchemes["Bearer"];
            Assert.Equal("Authorization", scheme.Name);
            Assert.Equal("JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"", scheme.Description);
            Assert.Equal(ParameterLocation.Header, scheme.In);
            Assert.Equal(SecuritySchemeType.ApiKey, scheme.Type);
        }

        [Fact]
        public void ConfigureServicesMustConfigureRouteOptionsWhenCalled()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            new StubStartup(configuration, new FakeHostingEnvironment()).ConfigureServices(services);
            RouteOptions options = new RouteOptions();
            services.BuildServiceProvider().GetRequiredService<IConfigureOptions<RouteOptions>>().Configure(options);
            Assert.True(options.LowercaseUrls);
        }

        [Fact]
        public void ConfigureServicesMustNotConfigureDocumentationWhenEnvironmentIsProduction()
        {
            IConfiguration configuration = new ConfigurationFactory().Create();
            IServiceCollection services = new ServiceCollection();
            new StubStartup(configuration, new FakeHostingEnvironment {EnvironmentName = "Production"}).ConfigureServices(services);
            Assert.Null(services.BuildServiceProvider().GetService<IConfigureOptions<SwaggerGenOptions>>());
        }

        private class StubStartup : StartupBase
        {
            public StubStartup(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
            {
            }
        }
    }
}