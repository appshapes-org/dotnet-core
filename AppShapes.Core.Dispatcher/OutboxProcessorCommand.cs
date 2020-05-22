using System;
using System.Threading;
using AppShapes.Core.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Dispatcher
{
    public class OutboxProcessorCommand
    {
        public OutboxProcessorCommand(IServiceProvider provider)
        {
            Provider = provider;
        }

        public virtual void Execute()
        {
            ILogger<CancellableTaskExecutor> logger = Provider.GetRequiredService<ILogger<CancellableTaskExecutor>>();
            using CancellableTaskExecutor executor = new CancellableTaskExecutor(logger, ExecuteOutboxProcessor);
            executor.Run();
        }

        protected virtual void ExecuteOutboxProcessor(CancellationToken cancellationToken)
        {
            ILogger<OutboxProcessor> logger = Provider.GetRequiredService<ILogger<OutboxProcessor>>();
            new OutboxProcessor(logger, Provider).Execute(cancellationToken).Wait(cancellationToken);
        }

        private IServiceProvider Provider { get; }
    }
}