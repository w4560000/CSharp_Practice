using System;
using System.Diagnostics;
using System.Threading;

namespace SpinSample
{
    /// <summary>
    /// SpinWait MSDN: https://docs.microsoft.com/zh-tw/dotnet/standard/threading/spinwait?redirectedfrom=MSDN
    /// SourceCode: https://blog.csdn.net/xxdddail/article/details/16982351
    /// 博客園文章SpinWait說明: https://www.cnblogs.com/majiang/p/7889584.html
    /// 
    /// 當 Thread 需要空轉 等待其他 Thread 處理完後 再進行處理時，我們常常會使用 Thread.Sleep()
    /// 但 Thread.Sleep 是先釋放當前 Thread，讓它去處理其他程序，等時間到再喚醒
    /// 但頻繁切換 Thread 是有 CPU 效能損耗的(暫停Thread、，且喚醒時間也不一定準
    /// 比如執行 ThreadSleepInThread() 呼叫 1000次 Thread.Sleep(10) 正常情況 1000*10 = 10秒，但實際上完成時間到了15秒
    /// 
    /// 當有空轉 Thread需求時，可改為使用 SpinWait
    /// 
    /// 
    /// 輸出
    /// No Sleep Consume Time:00:00:00.0000126
    /// SpinWait Consume Time:00:00:00.0004005
    /// Thread Sleep Consume Time:00:00:15.5115966
    /// </summary>
    internal class SpinWaitSample
    {
        private int _count1 = 1000;
        private int _count2 = 1000;
        private int _count3 = 1000;
        private int _timeout_ms = 10;

        public void Run()
        {
            NoSleep();
            ThreadSleepInThread();
            SpinWaitInThread();
        }

        private void NoSleep()
        {
            Thread thread = new Thread(() =>
            {
                var sw = Stopwatch.StartNew();
                while(_count1 > 0)
                {
                    _count1--;
                }
                Console.WriteLine("No Sleep Consume Time:{0}", sw.Elapsed.ToString());
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void ThreadSleepInThread()
        {
            Thread thread = new Thread(() =>
            {
                var sw = Stopwatch.StartNew();
                while (_count2 > 0)
                {
                    Console.WriteLine("1 Thread ID: " + Thread.CurrentThread.ManagedThreadId);

                    Thread.Sleep(_timeout_ms);

                    Console.WriteLine("2 Thread ID: " + Thread.CurrentThread.ManagedThreadId);
                    _count2--;
                }
                Console.WriteLine("Thread Sleep Consume Time:{0}", sw.Elapsed.ToString());
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void SpinWaitInThread()
        {
            Thread thread = new Thread(() =>
            {
                var sw = Stopwatch.StartNew();
                while (_count3 > 0)
                {
                    Thread.SpinWait(_timeout_ms);
                    //SpinWait.SpinUntil(() => true, _timeout_ms);

                    _count3--;
                }
                Console.WriteLine("SpinWait Consume Time:{0}", sw.Elapsed.ToString());
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}