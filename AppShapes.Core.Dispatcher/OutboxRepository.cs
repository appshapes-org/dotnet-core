using System;
using System.Collections.Generic;
using System.Linq;
using AppShapes.Core.Database;
using AppShapes.Core.Logging;
using AppShapes.Core.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace AppShapes.Core.Dispatcher
{
    public class OutboxRepository
    {
        private RetryPolicy itsRetryPolicy;

        public OutboxRepository(ILogger<OutboxRepository> logger, OutboxSettings settings, OutboxContext context)
        {
            Logger = logger;
            Settings = settings;
            Context = context;
        }

        public virtual void Remove(OutboxItem item)
        {
            Logger.Debug<OutboxRepository>($"Removing {item}");
            Context.Outbox.Remove(item);
            RetryPolicy.Execute(() => Context.SaveChanges());
        }

        public virtual List<OutboxItem> Retrieve()
        {
            Logger.Information<OutboxRepository>($"Remaining: {Context.Outbox.Count()}");
            List<OutboxItem> items = Context.Outbox.OrderBy(x => x.Timestamp).Take(Settings.OutboxTakeCount).ToList();
            Logger.Debug<OutboxRepository>($"Retrieved: {items.Count}");
            return items;
        }

        protected virtual RetryPolicy GetRetryPolicy()
        {
            IEnumerable<TimeSpan> sleepDurations = Settings.RetryPolicySleepDurationsInSeconds.Select(x => TimeSpan.FromSeconds(x));
            Logger.Debug<OutboxRepository>($"Creating {nameof(RetryPolicy)} (seconds): {string.Join(", ", Settings.RetryPolicySleepDurationsInSeconds)}");
            return Policy.Handle<DbUpdateException>().WaitAndRetry(sleepDurations, OnRetryPolicyRetry);
        }

        protected virtual void OnRetryPolicyRetry(Exception e, TimeSpan sleepDuration)
        {
            Logger.Warning<OutboxRepository>($"Waited {sleepDuration} second/s, now retrying");
        }

        private OutboxContext Context { get; }

        private ILogger<OutboxRepository> Logger { get; }

        private RetryPolicy RetryPolicy => itsRetryPolicy ??= GetRetryPolicy();

        private OutboxSettings Settings { get; }
    }
}