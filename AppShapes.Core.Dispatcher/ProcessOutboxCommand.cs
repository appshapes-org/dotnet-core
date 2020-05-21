using System;
using System.Threading;
using System.Threading.Tasks;
using AppShapes.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace AppShapes.Core.Dispatcher
{
    public class ProcessOutboxCommand
    {
        private AsyncCircuitBreakerPolicy itsCircuitBreakerPolicy;
        private OutboxSettings itsSettings;

        public ProcessOutboxCommand(ILogger<ProcessOutboxCommand> logger, IServiceProvider provider)
        {
            Logger = logger;
            Provider = provider;
        }

        public virtual async Task Execute(CancellationToken cancellationToken)
        {
            try
            {
                Logger.Trace<OutboxProcessor>($"{nameof(CircuitBreakerPolicy.CircuitState)}: {CircuitBreakerPolicy.CircuitState}");
                await CircuitBreakerPolicy.ExecuteAsync(() => ExecuteDispatcher(cancellationToken));
            }
            catch (OperationCanceledException)
            {
                Logger.Debug<OutboxProcessor>($"{nameof(OperationCanceledException)} has signaled termination of outbox item processing.");
                throw;
            }
            catch (Exception e)
            {
                Logger.Warning<OutboxProcessor>(e);
            }
        }

        protected virtual async Task ExecuteDispatcher(CancellationToken cancellationToken)
        {
            using IServiceScope scope = Provider.CreateScope();
            OutboxDispatcher dispatcher = scope.ServiceProvider.GetRequiredService<OutboxDispatcher>();
            await dispatcher.Execute(cancellationToken);
        }

        protected virtual AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy()
        {
            PolicyBuilder builder = Policy.Handle<Exception>(ShouldHandleException);
            int exceptionsAllowedBeforeBreaking = Settings.CircuitBreakerExceptionsAllowedBeforeBreaking;
            TimeSpan durationOfBreak = TimeSpan.FromMinutes(Settings.CircuitBreakerDurationOfBreakInMinutes);
            return builder.CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, durationOfBreak, OnCircuitBreakerBreak, OnCircuitBreakerReset);
        }

        protected virtual OutboxSettings GetSettings()
        {
            return Provider.GetRequiredService<OutboxSettings>();
        }

        protected virtual void OnCircuitBreakerBreak(Exception e, TimeSpan durationOfBreak)
        {
            Logger.Warning<OutboxProcessor>($"{nameof(durationOfBreak)}: {durationOfBreak.TotalSeconds} (seconds)");
        }

        protected virtual void OnCircuitBreakerReset()
        {
            Logger.Information<OutboxProcessor>($"After {Settings.CircuitBreakerDurationOfBreakInMinutes}");
        }

        protected virtual bool ShouldHandleException(Exception e)
        {
            bool returnValue = !(e is OperationCanceledException);
            Logger.Debug<OutboxProcessor>($"{e.GetType().Name}: {returnValue}");
            return returnValue;
        }

        private AsyncCircuitBreakerPolicy CircuitBreakerPolicy => itsCircuitBreakerPolicy ??= GetCircuitBreakerPolicy();

        private ILogger<ProcessOutboxCommand> Logger { get; }

        private IServiceProvider Provider { get; }

        private OutboxSettings Settings => itsSettings ??= GetSettings();
    }
}