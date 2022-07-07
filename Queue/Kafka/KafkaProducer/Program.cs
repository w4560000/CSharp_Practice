using Confluent.Kafka;
using System;
using System.Net;
using System.Threading.Tasks;

namespace KafkaProducer
{
    internal class Program
    {
        private static readonly string bootstrapServers = "localhost:9092";
        private static readonly string topic = "test";

        private static void Main(string[] args)
        {
            Console.WriteLine(" Start KafkaProducer");
            Console.WriteLine(" Type Message...");

            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                while (true)
                {
                    var message = Console.ReadLine();

                    // 不指定 Partition
                    producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

                    // 指定 Partition
                    //TopicPartition topicPartition = new TopicPartition(topic, 0);
                    //producer.ProduceAsync(topicPartition, new Message<Null, string> { Value = message });

                    Console.WriteLine(" Sent {0}", message);
                }
            };
        }
    }
}