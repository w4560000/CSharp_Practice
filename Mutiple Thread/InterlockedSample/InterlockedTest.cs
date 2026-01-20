using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockedSample
{
    internal class InterlockedTest
    {
        public static int Balance_Add = 0;
        public static int Balance_CompareExchange = 0;
        public static int Balance_Decrement = 1000;
        public static int Balance_Increment = 0;
        public static int Balance_Exchange = 0;

        public void RunTest()
        {
            //TestAdd();
            //TestCompareExchange1();
            //TestCompareExchange2();
            //TestDecrement();
            //TestIncrement();
            //TestExchange();
            TestSpinLock();
        }

        public void TestAdd()
        {
            void Add()
            {
                int a = Interlocked.Add(ref Balance_Add, 10);
                //int a = Balance_Add = Balance_Add + 10; // 沒Lock住 在多執行續下 會計算錯誤
                Console.WriteLine($"Balance_Add: {Balance_Add}, a: {a}");
            }

            List<Task> taskCollection = new List<Task>();

            for (int i = 0; i < 1000; i++)
                taskCollection.Add(Task.Factory.StartNew(() => Add()));

            //等待所有任務完成
            Task.WaitAll(taskCollection.ToArray());
            Console.WriteLine($"Total Balance_Add: {Balance_Add}");
        }

        /// <summary>
        /// CompareExchange(ref int location1, int value, int comparand)
        ///
        /// 比較 location1 、comparand 是否相等
        /// 若相等 location1 改為 value
        ///
        /// 回傳 location1 原始值
        /// </summary>
        public void TestCompareExchange1()
        {
            void CompareExchange()
            {
                // 若 執行完 CompareExchange 回傳的 原始 Balance_CompareExchange 跟 Balance_CompareExchange 不符
                // 代表 Balance_CompareExchange 已被其他Thread 加過了，
                Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId} ,Before Balance_CompareExchange: {Balance_CompareExchange}");
                int temp = Balance_CompareExchange;
                while (Interlocked.CompareExchange(ref Balance_CompareExchange, Balance_CompareExchange + 1, Balance_CompareExchange) == Balance_CompareExchange)
                {
                    Console.WriteLine($"@@@ ThreadID:{Thread.CurrentThread.ManagedThreadId} ,temp: {temp} Balance_CompareExchange: {Balance_CompareExchange}");
                }
                Console.WriteLine($"ThreadID:{Thread.CurrentThread.ManagedThreadId} END ,temp: {temp} Balance_CompareExchange: {Balance_CompareExchange}");
            }
            List<Task> taskCollection = new List<Task>();

            for (int i = 0; i < 1000; i++)
                taskCollection.Add(Task.Factory.StartNew(() => CompareExchange()));

            //等待所有任務完成
            Task.WaitAll(taskCollection.ToArray());
            Console.WriteLine($"Total Balance_CompareExchange: {Balance_CompareExchange}");
        }

        public void TestCompareExchange2()
        {
            int location1 = 1;
            int value = 2;
            int comparand = 3;

            Console.WriteLine("運行前：");
            Console.WriteLine($" location1 = {location1}    |   value = {value} |   comparand = {comparand}");

            Console.WriteLine("當 location1 != comparand 時");
            int result = Interlocked.CompareExchange(ref location1, value, comparand);
            Console.WriteLine($" location1 = {location1} | value = {value} |  comparand = {comparand} |  location1 改變前的值  {result}");

            // 當 location1 == comparand 時 => location1 = value, result = location1原始值
            Console.WriteLine("當 location1 == comparand 時");
            comparand = 1;
            result = Interlocked.CompareExchange(ref location1, value, comparand);
            Console.WriteLine($" location1 = {location1} | value = {value} |  comparand = {comparand} |  location1 改變前的值  {result}");
        }

        /// <summary>
        /// Decrement = 減一
        /// </summary>
        public void TestDecrement()
        {
            void Decrement()
            {
                int a = Interlocked.Decrement(ref Balance_Decrement);
                Console.WriteLine($"Balance_Decrement: {Balance_Decrement}, a: {a}");
            }

            List<Task> taskCollection = new List<Task>();

            for (int i = 0; i < 1000; i++)
                taskCollection.Add(Task.Factory.StartNew(() => Decrement()));

            //等待所有任務完成
            Task.WaitAll(taskCollection.ToArray());
            Console.WriteLine($"Total Balance_Decrement: {Balance_Decrement}");
        }

        /// <summary>
        /// Increment = 加一
        /// </summary>
        public void TestIncrement()
        {
            void Increment()
            {
                int a = Interlocked.Increment(ref Balance_Increment);
                Console.WriteLine($"Balance_Increment: {Balance_Increment}, a: {a}");
            }

            List<Task> taskCollection = new List<Task>();

            for (int i = 0; i < 1000; i++)
                taskCollection.Add(Task.Factory.StartNew(() => Increment()));

            //等待所有任務完成
            Task.WaitAll(taskCollection.ToArray());
            Console.WriteLine($"Total Balance_Increment: {Balance_Increment}");
        }

        /// <summary>
        /// Exchange(ref int location1, int value)
        /// 
        /// location1 改為 value
        /// 
        /// 回傳 location1 原始值
        /// </summary>
        public void TestExchange()
        {
            int lockedFlag = 0;
            void Exchange()
            {
                // 只允許 lockedFlag 為 0 時 進入
                // Thread 進入時 lockedFlag 改為 1
                // Thread 離開時 lockedFlag 復原回 0
                while (Interlocked.Exchange(ref lockedFlag, 1) != 0)
                {
                    Thread.Sleep(20);
                }

                Balance_Exchange++;
                Console.WriteLine($"Balance_Exchange: {Balance_Exchange}");
                Interlocked.Exchange(ref lockedFlag, 0);
            }

            List<Task> taskCollection = new List<Task>();

            for (int i = 0; i < 1000; i++)
                taskCollection.Add(Task.Factory.StartNew(() => Exchange()));

            //等待所有任務完成
            Task.WaitAll(taskCollection.ToArray());
            Console.WriteLine($"Total Balance_Exchange: {Balance_Exchange}");
        }

        /// <summary>
        /// 註記 SpinLock 在區塊中間 不要使用 await 其他方法，可能造成死鎖
        /// </summary>
        public void TestSpinLock()
        {
            var spinLock = new SpinLock();
            void Exchange()
            {
                bool lockTaken = false;
                try
                {
                    spinLock.Enter(ref lockTaken);
                    Balance_Exchange++;
                    Console.WriteLine($"Balance_Exchange: {Balance_Exchange}");
                }
                finally
                {
                    if (lockTaken)
                        spinLock.Exit();
                }
            }

            List<Task> taskCollection = new List<Task>();

            for (int i = 0; i < 1000; i++)
                taskCollection.Add(Task.Factory.StartNew(() => Exchange()));

            //等待所有任務完成
            Task.WaitAll(taskCollection.ToArray());
            Console.WriteLine($"Total Balance_Exchange: {Balance_Exchange}");
        }
    }
}