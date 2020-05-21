using AppShapes.Core.Configuration;
using AppShapes.Core.Database;
using AppShapes.Core.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppShapes.Core.Dispatcher
{
    public class ConfigureDispatcherCommand
    {
        public virtual void Execute<T>(IServiceCollection services, IConfiguration configuration) where T : OutboxContext
        {
            new ConfigureSettingsCommand().Execute<OutboxSettings>(services, configuration);
            new ConfigureSettingsCommand().Execute<DispatcherSettings>(services, configuration);
            new ConfigureTypeCommand().Execute<IMessageProducerFactory>(services, configuration.GetSection(nameof(DispatcherSettings))[nameof(DispatcherSettings.MessageProducerFactoryType)], ServiceLifetime.Singleton);
            services.AddSingleton(x => x.GetRequiredService<IMessageProducerFactory>().Create());
            services.AddScoped<OutboxDispatcher>();
            services.AddScoped<OutboxRepository>();
            services.AddScoped<OutboxContext>(x => x.GetRequiredService<T>());
        }
    }
}