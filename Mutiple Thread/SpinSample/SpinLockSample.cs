using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SpinSample
{
    /// <summary>
    /// SpinLockSample1 SourceCode: https://isdaniel.github.io/high-concurrency-atomic-cas-algorithm/
    /// SpinLockSample2、SpinLockSample3 SourceCode: https://blog.csdn.net/weixin_41049188/article/details/100065921
    /// 
    /// 
    /// SpinLock 是 busy watting，等待的時候 不會釋放該 Thread，適用在短暫的 waitting 情境，若是長時間的 較不適合 會影響 CPU時間
    /// </summary>
    internal class SpinLockSample
    {
        public void Run()
        {
            SpinLockSample1();
            //SpinLockSample2();
            //SpinLockSample3();
            Console.ReadKey();
        }

        /// <summary>
        /// SpinLock 默认的方法，锁定和解锁
        /// </summary>
        private void SpinLockSample1()
        {
            int number = 100000;
            var sw = Stopwatch.StartNew();

            SpinLock sl = new SpinLock();

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10000; i++)
            {
                tasks.Add(
                    Task.Run(() =>
                    {
                        bool gotLock = false;

                        try
                        {
                            sl.Enter(ref gotLock);

                            number -= 10;
                        }
                        finally
                        {
                            // 只有当真的得到了那把锁，你才会放弃它
                            //if (gotLock) sl.Exit();
                        }
                    })
                );
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"number: {number}");
        }

        // Demonstrates:
        //     默认自旋锁构造函数(跟踪线程所有者)
        //      SpinLock.Enter(ref bool)
        //      SpinLock.Exit() throwing exception
        //      SpinLock.IsHeld
        //      SpinLock.IsHeldByCurrentThread
        //      SpinLock.IsThreadOwnerTrackingEnabled
        private void SpinLockSample2()
        {
            // 实例化一个自旋锁
            SpinLock sl = new SpinLock();

            // 这些 手动信号量 帮助排序下面的两个作业
            ManualResetEventSlim mre1 = new ManualResetEventSlim(false);
            ManualResetEventSlim mre2 = new ManualResetEventSlim(false);
            bool lockTaken = false;

            Task taskA = Task.Factory.StartNew(() =>
            {
                try
                {
                    sl.Enter(ref lockTaken);
                    Console.WriteLine("Task A: 進入自旋鎖");
                    mre1.Set(); // 信号任务B开始它的逻辑

                    //等待任务B完成其逻辑
                    /*
                     * (通常，您不会希望潜在地执行这样的操作
                        重量级的操作，同时持有自旋锁，但我们做到了
                        这里可以更有效地显示自旋锁属性
                        taskB)。
                     */
                    mre2.Wait();
                }
                finally
                {
                    //if (lockTaken) sl.Exit();
                }
            });

            Task taskB = Task.Factory.StartNew(() =>
            {
                mre1.Wait(); // 等待任务A给我发信号
                Console.WriteLine("Task B: sl.IsHeld = {0} (should be true)", sl.IsHeld);
                Console.WriteLine("Task B: sl.IsHeldByCurrentThread = {0} (should be false)", sl.IsHeldByCurrentThread);
                Console.WriteLine("Task B: sl.IsThreadOwnerTrackingEnabled = {0} (should be true)", sl.IsThreadOwnerTrackingEnabled);

                try
                {
                    sl.Exit(); // 該 Thread 沒有 SpinLock 的所有權，所以會噴 exception
                    Console.WriteLine("Task B: 释放 sl, should not have been able to!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Task B: sl.Exit resulted in exception, as expected: {0}", e.Message);
                }

                mre2.Set(); // 向任务A发送退出自旋锁的信号
            });

            // 等待任务完成并清理
            Task.WaitAll(taskA, taskB);
            mre1.Dispose();
            mre2.Dispose();
        }

        // Demonstrates:
        // 自旋锁构造函数(false)——未跟踪线程所有权
        private void SpinLockSample3()
        {
            // 若 enableThreadOwnerTracking = false時，則不限定當初 lock SpinLock 的 Thread 才能釋放
            SpinLock sl = new SpinLock(enableThreadOwnerTracking: false);

            // 用于与下面的任务同步 手动信号量
            ManualResetEventSlim mres = new ManualResetEventSlim(false);

            // 将验证下面的任务在单独的线程上运行
            Console.WriteLine("主線程ID = {0}", Thread.CurrentThread.ManagedThreadId);

            /*
             * 现在进入自旋锁。通常，不会想花那么多时间来持有自旋锁，但是我们在这里这样做的目的是演示一个非所有权跟踪自旋锁可以由一个不同于用于进入它的线程退出。
             */

            bool lockTaken = false;
            sl.Enter(ref lockTaken);

            // 创建一个单独的任务，从中退出自旋锁
            Task worker = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("工作線程id = {0} (should be different than main thread id)",
                    Thread.CurrentThread.ManagedThreadId);

                // 现在退出自旋锁
                try
                {
                    sl.Exit();
                    Console.WriteLine("worker任務:如預期一樣成功退出自旋鎖");
                }
                catch (Exception e)
                {
                    Console.WriteLine("worker任務:退出自旋鎖時發生意外故障: {0}", e.Message);
                }

                // 通知主线程继续
                mres.Set();
            });

            /*
             * wait()代替work . wait()，因为work . wait()可以内联worker任务，使其运行在同一个线程上。本例的目的是显示另一个线程可以退出在线程上创建的自旋锁(不跟踪线程)。
             */
            mres.Wait();

            // now Wait() on worker and clean up
            worker.Wait();
            mres.Dispose();
        }
    }
}