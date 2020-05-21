using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Messaging.Kafka
{
    public class KafkaProducerFactory
    {
        public KafkaProducerFactory(ILogger<KafkaProducer> logger, KafkaSettings settings)
        {
            Logger = logger;
            Settings = settings;
        }

        public virtual IMessageProducer Create()
        {
            return new KafkaProducer(Logger, Settings);
        }

        private ILogger<KafkaProducer> Logger { get; }

        private KafkaSettings Settings { get; }
    }
}