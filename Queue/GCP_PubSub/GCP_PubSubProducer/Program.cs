using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;

namespace GCP_PubSubProducer
{
    internal class Program
    {
        private static string projectId = "project-id";
        private static string topicId = "Test-Topic";
        private static async Task Main(string[] args)
        {
            Console.WriteLine(" Start GCP_PubSubProducer");

            // 憑證
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Lab\PubSub\pubsub-credential.json");

            await PublishOrderKey();

            Console.ReadKey();
        }

        private static async Task Publish()
        {
            TopicName topicName = new TopicName(projectId, topicId);
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

        }

        private static async Task PublishOrderKey()
        {
            // 設定要發布的主題
            TopicName topicName = new TopicName(projectId, topicId);

            var customSettings = new PublisherClient.Settings
            {
                EnableMessageOrdering = true
            };
            PublisherClient publisher = await new PublisherClientBuilder
            {
                TopicName = topicName,
                // Sending messages to the same region ensures they are received in order even when multiple publishers are used.
                //Endpoint = "us-east1-pubsub.googleapis.com:443",
                Settings = customSettings
            }.BuildAsync();

            // 發布訊息
            try
            {
                await Task.WhenAll(
                     Enumerable.Range(1, 5).Select(async item =>
                     {
                         string msg = $"{item}";
                         string messageId = await publisher.PublishAsync("1", msg);
                         Console.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff} messageId:{messageId}, Sent {msg}");
                     })
                    );
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"發布訊息到 Pub/Sub 時發生錯誤: {ex.Message}");
            }
        }
    }
}