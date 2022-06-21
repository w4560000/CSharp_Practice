using System;
using System.Threading;

namespace EventWaitHandleSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 測試 AutoResetEvent
            //new AutoResetEventTest().Run();

            // 測試 ManualResetEvent
            //new ManualResetEventTest().Run();

            // 測試 ManualResetEventSlim
            new ManualResetEventSlimTest().Run();
            Console.ReadKey();
        }
    }
}
