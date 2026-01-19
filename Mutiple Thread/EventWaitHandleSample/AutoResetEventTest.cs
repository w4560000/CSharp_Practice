using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventWaitHandleSample
{
    /// <summary>
    /// MSDN https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.AutoResetEvent
    /// 
    /// 任一 Thread 通過 WaitOne()後，門自動上鎖 (一次只允許一個 Thread 執行)
    /// 
    /// todo 跨process AutoResetEvent 可設定名稱
    /// </summary>
    public class AutoResetEventTest
    {
        /// <summary>
        /// AutoResetEvent 建構子的 ture or false -> 是否有傳入Set的信號進去
        ///                        true  -> 代表現在是 Set()   狀態，  有信號 => 門敞開 其他 Thread 執行 WaitOne() 無效果
        ///                        false -> 代表現在是 Reset() 狀態，  無信號 => 門上鎖 其他 Thread 執行 WaitOne() 有效果
        ///
        /// AutoResetEvent WaitOne() => 若當下有信號 則無阻塞效果
        ///                             若當下無信號 則阻塞當前 Thread，且任一 Thread WaitOne()後，自動賦予狀態(門上鎖)
        /// </summary>
        private static AutoResetEvent _AutoResetEvent_initialState_false = new AutoResetEvent(false);

        private static AutoResetEvent _AutoResetEvent_initialState_true = new AutoResetEvent(true);

        public void Run()
        {
            //AutoResetEvent_State_false_Test();
            //AutoResetEvent_State_true_Test();
            AutoResetEvent_MutlipleThread_Test();
        }

        private static void AutoResetEvent_State_false_Test()
        {
            Task t1 = new Task(Test_initialState_false);
            Console.WriteLine($"create AutoResetEvent_Test_initialState_false, {DateTime.Now}");
            t1.Start();

            Thread.Sleep(3000);
            _AutoResetEvent_initialState_false.Set();

            Thread.Sleep(3000);
            _AutoResetEvent_initialState_false.Set();
        }

        private static void AutoResetEvent_State_true_Test()
        {
            Task t1 = new Task(Test_initialState_true);
            Console.WriteLine($"create AutoResetEvent_State_true_Test, {DateTime.Now}");
            t1.Start();

            Thread.Sleep(3000);
            _AutoResetEvent_initialState_true.Set();

            Thread.Sleep(3000);
            _AutoResetEvent_initialState_true.Reset();
        }

        /// <summary>
        /// 測試 AutoResetEvent 在多執行緒阻塞的情況
        /// Set() -> 設定開一次門，也就是說 若兩條執行緒都阻塞，Set 執行一次 只有一條會通，又馬上關閉，所以第二條還是不通
        /// </summary>
        private static void AutoResetEvent_MutlipleThread_Test()
        {
            Task t1 = new Task(() => AutoResetEvent_MutlipleThread_Test_void("t1"));
            Console.WriteLine($"create AutoResetEvent_MutlipleThread_Test_void t1, {DateTime.Now}");

            Task t2 = new Task(() => AutoResetEvent_MutlipleThread_Test_void("t2"));
            Console.WriteLine($"create AutoResetEvent_MutlipleThread_Test_void t2, {DateTime.Now}");

            t1.Start();
            t2.Start();

            Thread.Sleep(3000);
            _AutoResetEvent_initialState_false.Set();

            Thread.Sleep(3000);
            _AutoResetEvent_initialState_false.Set();

            Thread.Sleep(1000);
            Console.WriteLine("觀察AutoResetEvent Set 設定一次 只有一條thread會通過。");
        }

        private static void AutoResetEvent_MutlipleThread_Test_void(string thread)
        {
            _AutoResetEvent_initialState_false.WaitOne();
            Console.WriteLine($"{thread}, {DateTime.Now}");
        }

        /// <summary>
        /// 測試 AutoResetEvent 建構子帶入false時的效果
        /// </summary>
        private static void Test_initialState_false()
        {
            _AutoResetEvent_initialState_false.WaitOne();
            Console.WriteLine($"AutoResetEvent Test_initialState_false run, {DateTime.Now}");

            _AutoResetEvent_initialState_false.WaitOne();
            Console.WriteLine($"AutoResetEvent Test_initialState_false run2, {DateTime.Now}");

            Console.WriteLine("new AutoResetEvent(false) -> 代表初始狀態為Reset() -> WaitOne() 有效果.");
            Console.WriteLine("可以觀察到 AutoResetEvent 的 Set 只會開一次門，每次WaitOne之後 都需要再Set (開門一次) 才能繼續。");
        }

        /// <summary>
        /// 測試 AutoResetEvent 建構子帶入true時的效果
        /// </summary>
        private static void Test_initialState_true()
        {
            _AutoResetEvent_initialState_true.WaitOne();
            Console.WriteLine($"AutoResetEvent Test_initialState_true run, {DateTime.Now}");

            _AutoResetEvent_initialState_true.WaitOne();
            Console.WriteLine($"AutoResetEvent Test_initialState_true run2, {DateTime.Now}");

            Console.WriteLine("new AutoResetEvent(true) -> 代表初始狀態為Set() -> WaitOne() 無效果.");
        }
    }
}