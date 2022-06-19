using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MutexSample
{
    internal class ProcessTest
    {
        public void StartProcess()
        {
            string SHARED_MUTEX_NAME = "MutexSample_ProcessTest";
            int pid = Process.GetCurrentProcess().Id;

            // initiallyOwned true = 給予
            using (Mutex mtx = new Mutex(false, SHARED_MUTEX_NAME))
            {
                while (true)
                {
                    Console.WriteLine($"Press any key to let process {pid} acquire the {SHARED_MUTEX_NAME} mutex.");
                    Console.ReadKey();

                    while (!mtx.WaitOne(1000))
                    {
                        Console.WriteLine($"Process {pid} is waiting for the {SHARED_MUTEX_NAME} mutex...");
                    }

                    Console.WriteLine($"Process {pid} has acquired the {SHARED_MUTEX_NAME} mutex. Press any key to release it.");
                    Console.ReadKey();

                    mtx.ReleaseMutex();
                    Console.WriteLine($"Process {pid} released the {SHARED_MUTEX_NAME} mutex.");
                }
            }
        }
    }
}
