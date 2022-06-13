using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Action action1 = () => Console.WriteLine($"Task={Task.CurrentId}, Thread={Thread.CurrentThread.ManagedThreadId}");
            #region new Task

            // 1. (Action action)
            Task t1 = new Task(action1);

            // 2. (Action action)
            var a = new CancellationTokenSource();
            CancellationToken b = a.Token;

            #endregion

            t1.Start();

            Console.WriteLine($"Main Thread= {Thread.CurrentThread.ManagedThreadId}");

            Console.ReadKey();
        }
    }
}
