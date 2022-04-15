using System;

namespace EventWaitHandleSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 測試 AutoResetEvent
            new AutoResetEventTest().Run();

            // 測試 ManualResetEvent
            //new ManualResetEventTest().Run();

            Console.ReadKey();
        }
    }
}