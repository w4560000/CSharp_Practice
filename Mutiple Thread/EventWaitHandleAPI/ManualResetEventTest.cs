using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventWaitHandleSample
{
    public class ManualResetEventTest
    {
        /// <summary>
        /// ManualResetEvent 建構子的 ture or false -> 是否有傳入Set的信號進去
        ///                        true  -> 代表現在是 Set()   狀態，  WaitOne() 無效果
        ///                        false -> 代表現在是 Reset() 狀態，  WaitOne() 有效果
        ///
        /// ManualResetEvent Set   -> 設定 WaitOne() 無效果，門永遠敞開，直到設定Reset之後門才關上
        ///                  Reset -> 設定 WaitOne() 有效果
        /// </summary>
        private static ManualResetEvent _ManualResetEvent_initialState_false = new ManualResetEvent(false);

        private static ManualResetEvent _ManualResetEvent_initialState_true = new ManualResetEvent(true);

        public void Run()
        {
            //ManualResetEvent_State_false_Test();
            //ManualResetEvent_State_true_Test();
            ManualResetEvent_MutlipleThread_Test();
        }

        private static void ManualResetEvent_State_false_Test()
        {
            Task t1 = new Task(Test_initialState_false);
            Console.WriteLine($"create ManualResetEvent_Test_initialState_false, {DateTime.Now}");
            t1.Start();

            Thread.Sleep(3000);
            _ManualResetEvent_initialState_false.Set();

            //Thread.Sleep(3000);
            //_ManualResetEvent_initialState_false.Set();
        }

        private static void ManualResetEvent_State_true_Test()
        {
            Task t1 = new Task(Test_initialState_true);
            Console.WriteLine($"create ManualResetEvent_State_true_Test, {DateTime.Now}");
            t1.Start();

            Thread.Sleep(3000);
            _ManualResetEvent_initialState_true.Set();

            Thread.Sleep(3000);
            _ManualResetEvent_initialState_true.Reset();
        }

        /// <summary>
        /// 測試 ManualResetEvent 在多執行緒阻塞的情況
        /// Set() -> 門永遠敞開，也就是說 若兩條執行緒都阻塞，Set 執行一次 兩條都會通，直到設定Reset後 的 WaitOne 才會阻塞
        /// </summary>
        private static void ManualResetEvent_MutlipleThread_Test()
        {
            Task t1 = new Task(() => ManualResetEvent_MutlipleThread_Test_void("t1"));
            Console.WriteLine($"create ManualResetEvent_MutlipleThread_Test_void t1, {DateTime.Now}");

            Task t2 = new Task(() => ManualResetEvent_MutlipleThread_Test_void("t2"));
            Console.WriteLine($"create ManualResetEvent_MutlipleThread_Test_void t2, {DateTime.Now}");

            t1.Start();
            t2.Start();

            Thread.Sleep(3000);
            _ManualResetEvent_initialState_false.Set();

            Thread.Sleep(1000);
            Console.WriteLine("觀察ManualResetEvent Set 設定一次 多條執行緒的WaitOne都會被允許通過");
        }

        private static void ManualResetEvent_MutlipleThread_Test_void(string thread)
        {
            _ManualResetEvent_initialState_false.WaitOne();
            Console.WriteLine($"{thread}, {DateTime.Now}");
        }

        /// <summary>
        /// 測試 ManualResetEvent 建構子帶入false時的效果
        /// </summary>
        private static void Test_initialState_false()
        {
            _ManualResetEvent_initialState_false.WaitOne();
            Console.WriteLine($"ManualResetEvent Test_initialState_false run, {DateTime.Now}");

            _ManualResetEvent_initialState_false.WaitOne();
            Console.WriteLine($"ManualResetEvent Test_initialState_false run2, {DateTime.Now}");

            Console.WriteLine("new ManualResetEvent(false) -> 代表初始狀態為Reset() -> WaitOne() 有效果.");
            Console.WriteLine("可以觀察到 ManualResetEvent 的 Set 之後，門就一直打開，WaitOne() 之後就無效果，除非在設定Reset()。");
        }

        /// <summary>
        /// 測試 ManualResetEvent 建構子帶入true時的效果
        /// </summary>
        private static void Test_initialState_true()
        {
            _ManualResetEvent_initialState_true.WaitOne();
            Console.WriteLine($"ManualResetEvent Test_initialState_true run, {DateTime.Now}");

            _ManualResetEvent_initialState_true.WaitOne();
            Console.WriteLine($"ManualResetEvent Test_initialState_true run2, {DateTime.Now}");

            Console.WriteLine("new ManualResetEvent(true) -> 代表初始狀態為Set() -> WaitOne() 無效果.");
        }
    }
}