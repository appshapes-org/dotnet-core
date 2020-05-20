namespace AppShapes.Core.Messaging
{
    public interface IMessageProducerFactory
    {
        IMessageProducer Create();
    }
}