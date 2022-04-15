using System;
using System.Threading;

namespace NamedPipeServerStream_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            while(true)
            {
                using (var clientStream = new System.IO.Pipes.NamedPipeClientStream("127.0.0.1", "NamedPipeServerStream_Test"))
                {
                    Console.WriteLine("before Connect()");
                    clientStream.Connect(1000);
                    clientStream.ReadMode = System.IO.Pipes.PipeTransmissionMode.Message;

                    do
                    {
                        byte[] bytes = new byte[4];
                        clientStream.Read(bytes, 0, 4);
                        int val = BitConverter.ToInt32(bytes, 0);
                        Console.WriteLine("NewID == " + val + "\r");
                    } while (!clientStream.IsMessageComplete);
                }

                Thread.Sleep(1);
            };
        }
    }
}
