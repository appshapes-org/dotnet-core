using System;
using System.Threading.Tasks;
using AppShapes.Core.Logging;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Messaging.Kafka
{
    public class KafkaProducer : IMessageProducer
    {
        private IProducer<string, string> itsProducer;

        public KafkaProducer(ILogger<KafkaProducer> logger, KafkaSettings settings)
        {
            Logger = logger;
            Settings = settings;
        }

        public void Dispose()
        {
            itsProducer?.Dispose();
        }

        public virtual async Task Produce(OutboxItem item)
        {
            string topic = GetTopic(item);
            string key = GetKey(item);
            string message = GetMessage(item);
            int partition = GetPartition(item.EntityId);
            await Produce(new KafkaMessage(topic, partition, key, message));
        }

        protected virtual string GetKey(OutboxItem item)
        {
            return $"{item.Entity}:{item.EntityId}:{item.Type}:{item.Id}";
        }

        protected virtual string GetMessage(OutboxItem item)
        {
            return item.Message;
        }

        protected virtual int GetPartition(Guid id)
        {
            return Math.Abs(BitConverter.ToInt32(id.ToByteArray(), 0) % Settings.Partitions);
        }

        protected virtual IProducer<string, string> GetProducer()
        {
            return new ProducerBuilder<string, string>(GetProducerConfig()).Build();
        }

        protected virtual ProducerConfig GetProducerConfig()
        {
            ProducerConfig config = new ProducerConfig {BootstrapServers = Settings.BootstrapServers, MessageTimeoutMs = Settings.MessageTimeoutMilliseconds, MessageSendMaxRetries = Settings.MessageSendMaxRetries};
            if (!Settings.UseEncryption || string.IsNullOrWhiteSpace(Settings.SslCaLocation))
                return config;
            config.SaslMechanism = SaslMechanism.Plain;
            config.SecurityProtocol = SecurityProtocol.SaslSsl;
            config.SslCaLocation = Settings.SslCaLocation;
            config.SaslUsername = Settings.SaslUsername;
            config.SaslPassword = Settings.SaslPassword;
            return config;
        }

        protected virtual string GetTopic(OutboxItem item)
        {
            return item.Context;
        }

        protected virtual async Task<DeliveryResult<string, string>> Produce(KafkaMessage kafkaMessage)
        {
            try
            {
                Logger.Debug<KafkaProducer>($"{kafkaMessage}");
                return await Producer.ProduceAsync(kafkaMessage.TopicPartition, kafkaMessage.Message);
            }
            catch (Exception e)
            {
                Logger.Error<KafkaProducer>(e, $"{e.Message}, {kafkaMessage}");
                throw;
            }
        }

        private ILogger<KafkaProducer> Logger { get; }

        private IProducer<string, string> Producer => itsProducer ??= GetProducer();

        private KafkaSettings Settings { get; }
    }
}