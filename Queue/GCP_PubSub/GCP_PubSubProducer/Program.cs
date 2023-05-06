using Google.Cloud.PubSub.V1;

namespace GCP_PubSubProducer
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine(" Start GCP_PubSubProducer");

            // 憑證
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\Lab\PubSub\pubsub-credential.json");

            TopicName topicName = new TopicName("perfect-eon-383414", "Test-Topic");
            PublisherClient publisher = await PublisherClient.CreateAsync(topicName);

            await Task.WhenAll(
                 Enumerable.Range(1, 10).Select(async item =>
                 {
                     string msg = $"{item}";
                     string messageId = await publisher.PublishAsync(msg);
                     Console.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} messageId:{messageId}, Sent {msg}");
                 })
                );

            Console.ReadKey();
        }
    }
}