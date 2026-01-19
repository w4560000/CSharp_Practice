using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventWaitHandleSample
{
    /// <summary>
    /// MSDN https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.manualreseteventslim?view=net-6.0
    /// ManualResetEventSlim 用法與 ManualResetEvent 類似, 但 ManualResetEventSlim 無法跨 process
    /// </summary>
    internal class ManualResetEventSlimTest
    {
        /// <summary>
        /// ManualResetEventSlim 建構子的 ture or false -> 是否有傳入Set的信號進去
        ///                              true  -> 代表現在是 Set()   狀態，  WaitOne() 無效果
        ///                              false -> 代表現在是 Reset() 狀態，  WaitOne() 有效果
        ///
        /// ManualResetEventSlim Set   -> 設定 WaitOne() 無效果，門永遠敞開，直到設定Reset之後門才關上
        ///                      Reset -> 設定 WaitOne() 有效果
        /// </summary>
        private static ManualResetEventSlim _ManualResetEventSlim_initialState_false = new ManualResetEventSlim(false);

        public void Run()
        {
            //ManualResetEventSlim_MutlipleThread_Test();

            ManualResetEventSlim_SpinCount_Test();
        }

        /// <summary>
        /// 測試 ManualResetEventSlim 在多執行緒阻塞的情況
        /// Set() -> 門永遠敞開，也就是說 若兩條執行緒都阻塞，Set 執行一次 兩條都會通，直到設定Reset後 的 Wait 才會阻塞
        /// </summary>
        private static void ManualResetEventSlim_MutlipleThread_Test()
        {
            Task t1 = new Task(() => ManualResetEventSlim_MutlipleThread_Test_void("t1"));
            Console.WriteLine($"create ManualResetEventSlim_MutlipleThread_Test_void t1, {DateTime.Now:yyyyMMdd hh:mm:ss.fff}");

            Task t2 = new Task(() => ManualResetEventSlim_MutlipleThread_Test_void("t2"));
            Console.WriteLine($"create ManualResetEventSlim_MutlipleThread_Test_void t2, {DateTime.Now:yyyyMMdd hh:mm:ss.fff}");

            t1.Start();
            t2.Start();

            Thread.Sleep(3000);
            _ManualResetEventSlim_initialState_false.Set();

            Thread.Sleep(1000);
            Console.WriteLine("觀察ManualResetEventSlim Set 設定一次 多條執行緒的Wait都會被允許通過");
        }

        /// <summary>
        /// 測試 ManualResetEventSlim SpinCount
        /// SpinCount 預設為 10
        ///
        /// 查看SourceCode 發現 SpinCount 不能 < 0 & > 2047
        /// 且在單核心時 SpinCount 為1
        ///
        /// 但並非是 SpinCount 次數內 每次都執行 SpinWait，還有一些算法 呼叫 Thread.Yield()、Thread.Sleep(1)、Thread.Sleep(0)
        /// https://referencesource.microsoft.com/#mscorlib/system/threading/ManualResetEventSlim.cs,542
        /// </summary>
        private static void ManualResetEventSlim_SpinCount_Test()
        {
            Console.WriteLine($"ManualResetEventSlim_SpinCount_Test Start, {DateTime.Now:yyyyMMdd hh:mm:ss.fff}");

            // Construct a ManualResetEventSlim with a SpinCount of 1000
            // Higher spincount => longer time the MRES will spin-wait before taking lock
            ManualResetEventSlim mres1 = new ManualResetEventSlim(false, 1000);
            ManualResetEventSlim mres2 = new ManualResetEventSlim(false, 1000);

            Task bgTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Task signalling both MRESes before, {DateTime.Now:yyyyMMdd hh:mm:ss.fff}");

                // Just wait a little
                Thread.Sleep(5000);

                mres1.Set();

                // Now signal both MRESes
                Console.WriteLine($"Task signalling both MRESes, {DateTime.Now:yyyyMMdd hh:mm:ss.fff}");

                Thread.Sleep(5000);
                mres2.Set();
            });

            // A common use of MRES.WaitHandle is to use MRES as a participant in
            // WaitHandle.WaitAll/WaitAny.  Note that accessing MRES.WaitHandle will
            // result in the unconditional inflation of the underlying ManualResetEvent.
            WaitHandle.WaitAll(new WaitHandle[] { mres1.WaitHandle, mres2.WaitHandle }); // 兩者都set後 (門打開) 才放行
            Console.WriteLine($"WaitHandle.WaitAll(mres1.WaitHandle, mres2.WaitHandle) completed, {DateTime.Now:yyyyMMdd hh:mm:ss.fff}");

            // Clean up
            bgTask.Wait();
            mres1.Dispose();
            mres2.Dispose();
        }

        private static void ManualResetEventSlim_MutlipleThread_Test_void(string thread)
        {
            _ManualResetEventSlim_initialState_false.Wait();
            Console.WriteLine($"{thread}, {DateTime.Now:yyyyMMdd hh:mm:ss.fff}");
        }
    }
}