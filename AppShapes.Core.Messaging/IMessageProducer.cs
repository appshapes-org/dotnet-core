using System;
using System.Threading.Tasks;

namespace AppShapes.Core.Messaging
{
    public interface IMessageProducer : IDisposable
    {
        Task Produce(OutboxItem item);
    }
}