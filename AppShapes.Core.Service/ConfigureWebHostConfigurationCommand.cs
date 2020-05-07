using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AppShapes.Core.Service
{
    public class ConfigureWebHostConfigurationCommand
    {
        public void Execute(IWebHostBuilder builder, Action<WebHostBuilderContext, IConfigurationBuilder> options)
        {
            builder.ConfigureAppConfiguration(options);
        }
    }
}