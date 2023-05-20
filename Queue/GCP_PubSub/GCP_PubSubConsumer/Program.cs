using Google.Cloud.PubSub.V1;
using Grpc.Core;

namespace GCP_PubSubConsumer
{
    internal class Program
    {
        private static string projectId = "projectID";
        private static async Task Main(string[] args)
        {
            Console.WriteLine(" Start GCP_PubSubConsumber");

            // 憑證
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Lab\PubSub\pubsub-credential.json");

            //await StreamPull();

            //await Pull();

            await PullFilter();
            Console.ReadKey();
        }

        private static async Task StreamPull()
        {
            SubscriptionName subscriptionName = new SubscriptionName(projectId, "Test-Topic-Sub03");
            SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);

            await subscriber.StartAsync(async (msg, cancellationToken) =>
            {
                Console.WriteLine($"MessageId {msg.MessageId}, Message: {msg.Data.ToStringUtf8()}, PublishTime {msg.PublishTime.ToDateTime()}");
                //Console.WriteLine($"MessageId {msg.MessageId}, Delay");
                //await Task.Delay(20000);
                //Console.WriteLine($"MessageId {msg.MessageId}, Delay End");
                // Return Reply.Ack to indicate this message has been handled.
                return await Task.FromResult(SubscriberClient.Reply.Ack);
            });
        }

        private static async Task Pull()
        {
            SubscriptionName subscriptionName = new SubscriptionName(projectId, "Test-Topic-Sub03");
            SubscriberServiceApiClient subscriberClient = SubscriberServiceApiClient.Create();
            int messageCount = 0;

            try
            {
                Console.WriteLine($"Start Pull, {DateTime.Now:yyyyMMdd HH:mm:ss.fff}");
                PullResponse response = await subscriberClient.PullAsync(subscriptionName, maxMessages: 10);
                Console.WriteLine($"End Pull, {DateTime.Now:yyyyMMdd HH:mm:ss.fff}");

                foreach (ReceivedMessage msg in response.ReceivedMessages)
                {
                    string text = System.Text.Encoding.UTF8.GetString(msg.Message.Data.ToArray());
                    Console.WriteLine($"MessageId: {msg.Message.MessageId}, Message: {text}, PublishTime: {msg.Message.PublishTime}");
                    Interlocked.Increment(ref messageCount);

                    subscriberClient.Acknowledge(subscriptionName, response.ReceivedMessages.Select(msg => msg.AckId));
                }
            }
            catch (RpcException ex) when (ex.Status.StatusCode == StatusCode.Unavailable)
            {
                // UNAVAILABLE due to too many concurrent pull requests pending for the given subscription.
            }
        }

        private static async Task PullFilter()
        {
            SubscriptionName subscriptionName = new SubscriptionName(projectId, "Test-Topic-SubFilter");
            SubscriberServiceApiClient subscriberClient = SubscriberServiceApiClient.Create();

            SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);

            await subscriber.StartAsync(async (msg, cancellationToken) =>
            {
                Console.WriteLine($"MessageId {msg.MessageId}, Message: {msg.Data.ToStringUtf8()}, PublishTime {msg.PublishTime.ToDateTime()}");
                //Console.WriteLine($"MessageId {msg.MessageId}, Delay");
                //await Task.Delay(20000);
                //Console.WriteLine($"MessageId {msg.MessageId}, Delay End");
                // Return Reply.Ack to indicate this message has been handled.
                return await Task.FromResult(SubscriberClient.Reply.Ack);
            });
        }
    }
}