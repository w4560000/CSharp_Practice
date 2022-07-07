using Confluent.Kafka;
using System;
using System.Threading;

namespace KafkaConsumer
{
    internal class Program
    {
        private static readonly string topic = "test";
        private static readonly string groupId = "test_group1";
        private static readonly string bootstrapServers = "localhost:9092";

        private static void Main(string[] args)
        {
            Console.WriteLine(" Start KafkaConsumer");

            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();

                while (true)
                {
                    var consumer = consumerBuilder.Consume(cancelToken.Token);

                    Console.WriteLine($" Received {consumer.Message.Value}");
                }
            }
        }
    }
}