using Google.Cloud.PubSub.V1;
using System.Text.Json;

namespace GCP_PubSubConsumber
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(" Start GCP_PubSubConsumber");

            // 憑證
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"D:\Lab\PubSub\pubsub-credential.json");

            SubscriptionName subscriptionName = new SubscriptionName("perfect-eon-383414", "Test-Topic-Sub01");
            SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);

            await subscriber.StartAsync((msg, cancellationToken) =>
            {
                string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
                string guid = Guid.NewGuid().ToString();
                Console.WriteLine($"{guid} Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
                Console.WriteLine($"{guid} Text: '{msg.Data.ToStringUtf8()}'");
                Console.WriteLine("\n");

                // Return Reply.Ack to indicate this message has been handled.
                return Task.FromResult(SubscriberClient.Reply.Ack);
            });

            Console.ReadKey();
        }
    }
}