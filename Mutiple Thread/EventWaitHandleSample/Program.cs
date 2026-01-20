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

            Console.WriteLine("請輸入 wait or unlock");
            var input = Console.ReadLine();
            var isUnlock = input == "unlock";
            // 測試 AutoResetEvent 跨Process
            new AutoResetEventTest().Run(isUnlock);

            // 測試 ManualResetEvent
            //new ManualResetEventTest().Run();

            // 測試 ManualResetEventSlim
            //new ManualResetEventSlimTest().Run();
            Console.ReadKey();
        }
    }
}
