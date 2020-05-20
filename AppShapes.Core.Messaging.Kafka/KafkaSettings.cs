namespace AppShapes.Core.Messaging.Kafka
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }

        public int MessageSendMaxRetries { get; set; } = 3;

        public int MessageTimeoutMilliseconds { get; set; } = 5000;

        public int Partitions { get; set; } = 1;

        public string SaslPassword { get; set; }

        public string SaslUsername { get; set; }

        public string SslCaLocation { get; set; }

        public bool UseEncryption { get; set; }
    }
}