using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppShapes.Core.Logging;
using AppShapes.Core.Messaging;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Dispatcher
{
    public class OutboxDispatcher
    {
        public OutboxDispatcher(ILogger<OutboxDispatcher> logger, OutboxRepository repository, IMessageProducer producer)
        {
            Logger = logger;
            Repository = repository;
            Producer = producer;
        }

        public virtual async Task Execute(CancellationToken cancellationToken)
        {
            while (Retrieve(out List<OutboxItem> items))
                await Dispatch(items, cancellationToken);
        }

        protected virtual async Task Dispatch(List<OutboxItem> items, CancellationToken cancellationToken)
        {
            foreach (OutboxItem item in items)
                await Dispatch(item, cancellationToken);
        }

        protected virtual async Task Dispatch(OutboxItem item, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Producer.Produce(item);
            Remove(item);
            Logger.Information<OutboxDispatcher>($"Dispatched {item}");
        }

        protected virtual void Remove(OutboxItem item)
        {
            Repository.Remove(item);
        }

        protected virtual bool Retrieve(out List<OutboxItem> items)
        {
            items = Repository.Retrieve();
            return items.Count > 0;
        }

        private ILogger<OutboxDispatcher> Logger { get; }

        private IMessageProducer Producer { get; }

        private OutboxRepository Repository { get; }
    }
}