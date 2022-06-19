using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeServerStream_Server
{
    class Program
    {
        private static volatile int _newId = 0;

        /// <summary>
        /// NamedPipeServerStream Server端
        /// 
        /// 建立管線
        /// 
        /// Client 透過 PipeName 來連線
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                NamedPipeServerStream stream = null;
                Console.WriteLine("start server in thread " + Thread.CurrentThread.ManagedThreadId);

                // maxNumberOfServerInstances = 允許建立相同PipeName的個數
                stream = new NamedPipeServerStream("NamedPipeServerStream_Test",
                     PipeDirection.InOut,
                     1,
                     PipeTransmissionMode.Message,
                     PipeOptions.None);

                while (true)
                {
                    Console.WriteLine("before WaitForConnection()");
                    stream.WaitForConnection();
                    Thread.Sleep(1000);

                    int newId = ++_newId;

                    byte[] bytes = BitConverter.GetBytes(newId);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    Console.WriteLine($"[{DateTime.Now:yyyyMMdd hhmmss}]Send newId: {newId}.");
                    stream.Disconnect();
                }
            });

            Console.Read();
        }
    }
}
