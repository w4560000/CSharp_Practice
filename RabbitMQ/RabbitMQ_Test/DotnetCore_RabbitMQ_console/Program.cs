using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DotnetCore_RabbitMQ_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("program start!");
            RabbitMQConfig.InitReceiver(null);

            IModel publisherModel = RabbitMQConfig.GetPublisherModel();
            //publisherModel.ConfirmSelect();
            string input = string.Empty;

            while (true)
            {
                input = Console.ReadLine();

                var sendBytes = Encoding.UTF8.GetBytes(input);

                publisherModel.BasicPublish("leo", "hello", null, sendBytes);

                //if(publisherModel.WaitForConfirms())
                    Console.WriteLine($"成功發送訊息。");

            }
        }

        /// <summary>
        /// 收到MQ訊息後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine($"接收訊息: {msg}");
        }
    }
}
