using System;
using System.Collections.Generic;
using System.Threading;

namespace MutexSample
{
    /// <summary>
    /// Source Code : https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.mutex?view=netcore-3.1
    /// </summary>
    internal class ThreadTest
    {
        public ThreadTest(bool initiallyOwned)
        {
            mut = new Mutex(initiallyOwned);
        }

        public Mutex mut;

        private const int numIterations = 1;
        private const int numThreads = 3;
        private List<string> temp = new List<string>();

        public void StartThreads()
        {
            // Create the threads that will use the protected resource.
            for (int i = 0; i < numThreads; i++)
            {
                Thread newThread = new Thread(new ThreadStart(ThreadProc));
                newThread.Name = String.Format("Thread{0}", i + 1);
                newThread.Start();
            }

            // cross thread 鎖、解鎖 (測試 由不同thread鎖、再由不同thread開鎖) 會發生異常
            // Mutex 的鎖、解鎖 限定為同一thread
            //for (int i = 1; i <= 12; i++)
            //{
            //    Thread td = new Thread(new ParameterizedThreadStart(Test1));
            //    td.Start($"編號{i}");
            //}

            //for (int i = 13; i <= 20; i++)
            //{
            //    Thread td = new Thread(new ParameterizedThreadStart(Test2));
            //    td.Start($"編號{i}");
            //}
        }

        private void ThreadProc()
        {
            for (int i = 0; i < numIterations; i++)
            {
                while (true)
                {
                    if (temp.Contains(Thread.CurrentThread.Name))
                    {
                        Console.WriteLine(DateTime.Now.ToString() + " {0} done", Thread.CurrentThread.Name);
                        return;
                    }

                    UseResource();
                }
            }
        }

        // This method represents a resource that must be synchronized
        // so that only one thread at a time can enter.
        private void UseResource()
        {
            // Wait until it is safe to enter, and do not enter if the request times out.
            Console.WriteLine(DateTime.Now.ToString() + " {0} is requesting the mutex", Thread.CurrentThread.Name);
            if (mut.WaitOne(1000))
            {
                Console.WriteLine(DateTime.Now.ToString() + " {0} has entered the protected area",
                    Thread.CurrentThread.Name);

                // Place code to access non-reentrant resources here.

                // Simulate some work.
                Thread.Sleep(5000);

                Console.WriteLine(DateTime.Now.ToString() + " {0} is leaving the protected area",
                    Thread.CurrentThread.Name);

                // Release the Mutex.
                mut.ReleaseMutex();
                Console.WriteLine(DateTime.Now.ToString() + " {0} has released the mutex",
                                  Thread.CurrentThread.Name);

                temp.Add(Thread.CurrentThread.Name);
            }
            //else
            //{
            //    Console.WriteLine(DateTime.Now.ToString() + " {0} will not acquire the mutex",
            //                      Thread.CurrentThread.Name);
            //}
        }

        public void Test1(object obj)
        {
            // 進洗手間 消耗一個廁所
            mut.WaitOne();
            Console.WriteLine(">>>>>" + obj.ToString() + "鎖起來：" + DateTime.Now.ToString());
        }

        public void Test2(object obj)
        {
            // 進洗手間 消耗一個廁所
            mut.ReleaseMutex(); // 錯誤!!
            Console.WriteLine(obj.ToString() + "開鎖：" + DateTime.Now.ToString());
        }

        ~ThreadTest()
        {
            mut.Dispose();
        }
    }
}