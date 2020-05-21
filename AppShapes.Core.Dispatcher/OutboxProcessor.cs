using System;
using System.Threading;
using System.Threading.Tasks;
using AppShapes.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Dispatcher
{
    public class OutboxProcessor
    {
        private OutboxSettings itsSettings;

        public OutboxProcessor(ILogger<OutboxProcessor> logger, IServiceProvider provider)
        {
            Logger = logger;
            Provider = provider;
        }

        public virtual async Task Execute(CancellationToken cancellationToken)
        {
            cancellationToken.Register(OnCancellationTokenCancelled);
            while (ShouldProcessOutbox(cancellationToken))
            {
                await ProcessOutbox(cancellationToken);
                WaitForOutbox(Settings.WaitForOutboxInSeconds, cancellationToken);
            }
        }

        protected virtual OutboxSettings GetSettings()
        {
            return Provider.GetRequiredService<OutboxSettings>();
        }

        protected virtual void OnCancellationTokenCancelled()
        {
            Logger.Information<OutboxProcessor>($"Terminating outbox processing. Restart {nameof(OutboxProcessor)} to resume.");
        }

        protected virtual async Task ProcessOutbox(CancellationToken cancellationToken)
        {
            await new ProcessOutboxCommand(Provider.GetRequiredService<ILogger<ProcessOutboxCommand>>(), Provider).Execute(cancellationToken);
        }

        protected virtual bool ShouldProcessOutbox(CancellationToken cancellationToken)
        {
            return !cancellationToken.IsCancellationRequested;
        }

        protected virtual void WaitForOutbox(int waitInMilliseconds, CancellationToken cancellationToken)
        {
            Logger.Debug<OutboxProcessor>($"{nameof(waitInMilliseconds)}: {waitInMilliseconds}");
            cancellationToken.WaitHandle.WaitOne(waitInMilliseconds);
        }

        private ILogger<OutboxProcessor> Logger { get; }

        private IServiceProvider Provider { get; }

        private OutboxSettings Settings => itsSettings ??= GetSettings();
    }
}