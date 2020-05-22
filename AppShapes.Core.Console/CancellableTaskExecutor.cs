using System;
using System.Threading;
using System.Threading.Tasks;
using AppShapes.Core.Logging;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Console
{
    public class CancellableTaskExecutor : IDisposable
    {
        public CancellableTaskExecutor(ILogger<CancellableTaskExecutor> logger, Action<CancellationToken> command) : this(logger, command, new CancellationTokenSource())
        {
        }

        public CancellableTaskExecutor(ILogger<CancellableTaskExecutor> logger, Action<CancellationToken> command, CancellationTokenSource cancellationTokenSource)
        {
            Logger = logger;
            Command = command;
            CancellationTokenSource = cancellationTokenSource;
        }

        public void Dispose()
        {
            CancellationTokenSource.Dispose();
            IsDisposed = true;
        }

        public virtual void Run()
        {
            try
            {
                CheckCancellation();
                Execute();
            }
            catch (OperationCanceledException)
            {
                Logger.Trace<CancellableTaskExecutor>($"Handling {nameof(OperationCanceledException)}");
            }
            catch (AggregateException ex)
            {
                Logger.Trace<CancellableTaskExecutor>(ex, $"Handling {nameof(AggregateException)}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Unexpected exception: {ex.Message}");
            }
        }

        protected virtual void CancelExecution()
        {
            CancellationTokenSource.Cancel();
        }

        protected virtual void CheckCancellation()
        {
            CancellationTokenSource.Token.ThrowIfCancellationRequested();
        }

        protected virtual void Execute()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            CancellableTask = Task.Run(() => Command(CancellationTokenSource.Token), CancellationTokenSource.Token);
            CancellableTask.Wait(CancellationTokenSource.Token);
        }

        protected virtual void OnProcessExit(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;
            CancelExecution();
            WaitForCancellation();
        }

        protected virtual void WaitForCancellation()
        {
            try
            {
                Logger.Debug<CancellableTaskExecutor>($"{nameof(CancellableTask)} cancelling");
                CancellableTask.Wait();
                Logger.Debug<CancellableTaskExecutor>($"{nameof(CancellableTask)} cancelled");
            }
            catch (AggregateException)
            {
                Logger.Trace<CancellableTaskExecutor>($"Handling {nameof(AggregateException)} thrown from {nameof(CancellableTask)}");
            }
            catch (Exception ex)
            {
                Logger.Error<CancellableTaskExecutor>(ex, $"Handling unexpected error: {ex.Message}");
            }
        }

        private Task CancellableTask { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; }

        private Action<CancellationToken> Command { get; }

        private bool IsDisposed { get; set; }

        private ILogger<CancellableTaskExecutor> Logger { get; }
    }
}