using System;

namespace SemaphoreSample
{
    /// <summary>
    /// MSDN https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.semaphore
    /// 參考文章 https://www.uj5u.com/net/227011.html
    ///
    /// Semaphore 可以透過 name 來跨應用程式取得 Semaphore 實體，SemaphoreSlim 則只限於單一應用程式
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // test cross thread
            new ThreadTest().StartThread();

            // test cross process
            //new ProcessTest().StartProcess();
            Console.ReadKey();
        }
    }
}