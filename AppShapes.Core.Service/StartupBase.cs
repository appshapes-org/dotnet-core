using System;
using AppShapes.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AppShapes.Core.Service
{
    public abstract class StartupBase
    {
        protected StartupBase(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment.EnvironmentName;
            Configuration = GetConfigurationBuilder(configuration, environment).Build();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            UseMiddleware(app, environment);
            UseDocumentation(app);
            UseControllers(app, environment);
            UseSecurity(app, environment);
            UseRouting(app, environment);
            UseBootstrap(app, environment);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureControllers(services);
            ConfigureCors(services);
            ConfigureRouting(services);
            ConfigureDocumentation(services);
            ConfigureServices(services, Configuration);
            ConfigureServiceProvider(services);
        }

        protected IConfiguration Configuration { get; }

        protected virtual void ConfigureConfigurationOptions(IServiceCollection arg1, IConfiguration arg2)
        {
        }

        protected virtual IMvcBuilder ConfigureControllers(IServiceCollection services)
        {
            return new ConfigureControllersCommand().Create(services, ConfigureControllersOptions);
        }

        protected virtual void ConfigureControllersFilters(FilterCollection filters)
        {
            new ConfigureFiltersCommand().Execute(filters);
        }

        protected virtual void ConfigureControllersOptions(MvcOptions options)
        {
            ConfigureControllersFilters(options.Filters);
        }

        protected virtual void ConfigureCors(IServiceCollection services)
        {
            new ConfigureCorsCommand().Execute(services, ConfigureCorsOptions);
        }

        protected virtual void ConfigureCorsOptions(CorsOptions options)
        {
            options.AddPolicy("AllowAnyPolicy", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
        }

        protected virtual void ConfigureDocumentation(IServiceCollection services)
        {
            if (!IsProductionEnvironment)
                new ConfigureDocumentationCommand().Execute(services, ConfigureSwaggerOptions);
        }

        protected virtual void ConfigureLoggingOptions(ILoggingBuilder logging)
        {
        }

        protected virtual void ConfigureRouting(IServiceCollection services)
        {
            new ConfigureRoutingCommand().Execute(services, ConfigureRoutingOptions);
        }

        protected virtual void ConfigureRoutingOptions(RouteOptions options)
        {
            options.LowercaseUrls = true;
        }

        protected virtual void ConfigureServiceProvider(IServiceCollection services)
        {
            Dependencies = services.BuildServiceProvider();
        }

        protected virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            new ConfigureConfigurationCommand().Execute(services, configuration, ConfigureConfigurationOptions);
            new ConfigureLoggingCommand().Execute(services, configuration, ConfigureLoggingOptions);
            new ConfigureHealthChecksCommand().Execute(services);
        }

        protected virtual void ConfigureSwaggerOptions(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo {Title = $"{AppDomain.CurrentDomain.FriendlyName} API", Version = "v1"});
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"", Name = "Authorization", In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey});
        }

        protected virtual IServiceProvider Dependencies { get; private set; }

        protected string Environment { get; }

        protected virtual IConfigurationBuilder GetConfigurationBuilder(IConfiguration configuration, IWebHostEnvironment environment)
        {
            return new ConfigurationBuilderFactory().Create(configuration, environment);
        }

        protected virtual bool IsProductionEnvironment => string.Equals(Environment, Environments.Production, StringComparison.OrdinalIgnoreCase);

        protected virtual void UseBootstrap(IApplicationBuilder app, IWebHostEnvironment environment)
        {
        }

        protected virtual void UseControllers(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (IsProductionEnvironment)
                new UseProductionControllersCommand().Execute(app, "AllowAnyPolicy");
            else
                new UseControllersCommand().Execute(app, "AllowAnyPolicy");
        }

        protected virtual void UseDocumentation(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppDomain.CurrentDomain.FriendlyName} API V1"); });
            app.UseDeveloperExceptionPage();
        }

        protected virtual void UseMiddleware(IApplicationBuilder app, IWebHostEnvironment environment)
        {
        }

        protected virtual void UseRouting(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            new UseRoutingCommand().Execute(app, environment, UseRoutingOptions);
        }

        protected virtual void UseRoutingOptions(IEndpointRouteBuilder builder)
        {
            builder.MapControllers();
            builder.MapHealthChecks("/health");
        }

        /// <summary>
        ///     <code>app.UseAuthorization();</code>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="environment"></param>
        protected virtual void UseSecurity(IApplicationBuilder app, IWebHostEnvironment environment)
        {
        }
    }
}