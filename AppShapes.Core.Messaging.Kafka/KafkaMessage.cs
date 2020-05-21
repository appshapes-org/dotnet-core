using Confluent.Kafka;

namespace AppShapes.Core.Messaging.Kafka
{
    public class KafkaMessage
    {
        public KafkaMessage(string topic, int partition, string key, string message)
        {
            TopicPartition = new TopicPartition(topic, partition);
            Message = new Message<string, string> {Key = key, Value = message};
        }

        public Message<string, string> Message { get; }

        public TopicPartition TopicPartition { get; }

        public override string ToString()
        {
            return $"{nameof(TopicPartition.Topic)}: {TopicPartition.Topic}, {nameof(TopicPartition.Partition)}: {TopicPartition.Partition}, {nameof(Message.Key)}: {Message.Key}, {nameof(Message.Value)}: {Message.Value}";
        }
    }
}