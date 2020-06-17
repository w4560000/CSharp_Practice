using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitmqTest
{
    /// <summary>
    /// source code form https://www.cnblogs.com/Vincent-yuan/p/10934099.html
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("program start!");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection()) 
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                while (true)
                {
                    var message = Console.ReadLine();

                    //string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                                             routingKey: "hello",
                                                             basicProperties: null,
                                                             body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}