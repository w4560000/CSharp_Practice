using System;
using System.Diagnostics;
using System.Threading;

namespace SemaphoreSample
{
    /// <summary>
    /// 測試 Semaphore cross process
    /// </summary>
    internal class ProcessTest
    {
        public void StartProcess()
        {
            string SHARED_SemaphoreNAME = "SemaphoreSample_ProcessTest";

            int pid = Process.GetCurrentProcess().Id;

            // initialCount = 空位初始值
            // maximumCount = 空位最大值
            // 在此情境 initialCount、maximumCount 都為 1，模擬
            using (Semaphore semaphoreA = new Semaphore(1, 1, SHARED_SemaphoreNAME))
            {
                while (true)
                {
                    Console.WriteLine($"Press any key to let process {pid} acquire the {SHARED_SemaphoreNAME} semaphore.");
                    Console.ReadKey();

                    while (!semaphoreA.WaitOne(1000))
                    {
                        Console.WriteLine($"Process {pid} is waiting for the {SHARED_SemaphoreNAME} semaphore...");
                    }

                    Console.WriteLine($"Process {pid} has acquired the {SHARED_SemaphoreNAME} semaphore. Press any key to release it.");
                    Console.ReadKey();

                    semaphoreA.Release();
                    Console.WriteLine($"Process {pid} released the {SHARED_SemaphoreNAME} semaphore.");
                }
            }
        }
    }
}