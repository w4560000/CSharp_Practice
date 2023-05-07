using Google.Cloud.PubSub.V1;

namespace GCP_PubSubProducer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine(" Start GCP_PubSubProducer");

            // 憑證
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Lab\PubSub\pubsub-credential.json");

            TopicName topicName = new TopicName("project-id", "Test-Topic");
            PublisherClient publisher = await PublisherClient.CreateAsync(topicName);

            Enumerable.Range(1, 10).ToList().ForEach(item =>
            {
                string msg = $"{item}";
                string messageId = publisher.PublishAsync(msg).GetAwaiter().GetResult();
                Console.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} MessageId:{messageId}, Sent {msg}");
            });


            //await Task.WhenAll(
            //     Enumerable.Range(1, 100).Select(async item =>
            //     {
            //         string msg = $"{item}";
            //         string messageId = await publisher.PublishAsync(msg);
            //         Console.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} messageId:{messageId}, Sent {msg}");
            //     })
            //    );

            Console.ReadKey();
        }
    }
}