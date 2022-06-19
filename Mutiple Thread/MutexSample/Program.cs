using System;

namespace MutexSample
{
    /// <summary>
    /// Mutex(互斥鎖) CSDN https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.mutex?view=net-6.0
    ///
    /// 用以限定 Multipe Thread 、Multipe Process 存取臨界區段(Critical section)
    /// 臨界區段(Critical section) = WaitOne ~ ReleaseMutex
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // test cross thread initiallyOwned = true
            //var threadTestnew = new ThreadTest(true);
            //threadTestnew.StartThreads();

            //Thread.Sleep(3000);
            //Console.WriteLine("Program 開鎖");
            //threadTestnew.mut.ReleaseMutex();

            // test cross thread initiallyOwned = false
            new ThreadTest(false).StartThreads();

            // test cross process
            //new ProcessTest().StartProcess();

            Console.Read();
        }
    }
}