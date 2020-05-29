using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace CancellationTokenSourceAPI
{
    internal class Program
    {
        private enum switchTest
        {
            Construct,
            CreateLinkedTokenSource,
            Cancel,
            Cancel_Register,
            CancelAfter,
            Dispose
        }

        private static void Main(string[] args)
        {
            ProgramExtension.EnterMethod(Type.GetType(MethodBase.GetCurrentMethod().DeclaringType.FullName), switchTest.Dispose);
        }

        /// <summary>
        /// 測試建立 CancellationTokenSource 的三種多載建構子
        /// </summary>
        public static void Construct()
        {
            // CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            // CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(1);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            cancellationTokenSource.Token.Register(() => Console.WriteLine("工作已取消"));

            Task t1 = new Task(() => Console.WriteLine("Task Running~"), cancellationTokenSource.Token);
            t1.Start();

            Console.WriteLine($"cancellationTokenSource.IsCancellationRequested = {cancellationTokenSource.IsCancellationRequested}");
            Console.WriteLine($"cancellationTokenSource.Token.IsCancellationRequested = {cancellationTokenSource.Token.IsCancellationRequested}");
        }

        /// <summary>
        /// 建立有連結的CancellationTokenSource
        /// 當用來建立的任一CancellationTokenSource被Cancel掉時，該LinkedTokenSource也同樣被Cancel掉
        /// 
        /// 測試呼叫 CreateLinkedTokenSource
        /// </summary>
        public static void CreateLinkedTokenSource()
        {
            CancellationTokenSource notCancellationTokenSource = new CancellationTokenSource();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            CancellationTokenSource test = CancellationTokenSource.CreateLinkedTokenSource(notCancellationTokenSource.Token, cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            Console.WriteLine($"notCancellationTokenSource類別是否被Cancel了 = {notCancellationTokenSource.IsCancellationRequested}");
            Console.WriteLine($"cancellationTokenSource類別是否被Cancel了 = {cancellationTokenSource.IsCancellationRequested}");
            Console.WriteLine($"該CancellationTokenSource類別是否被Cancel了 = {test.IsCancellationRequested}");
        }

        /// <summary>
        /// cancel掉Task
        /// 
        /// 測試呼叫 Cancel()
        /// </summary>
        public static void Cancel()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Task have canceled!{cancellationTokenSource.IsCancellationRequested}");

                        return;
                    }
                    else
                        Console.WriteLine($"Task Running!{cancellationTokenSource.IsCancellationRequested}");
                }
            }, cancellationTokenSource.Token);

            Thread.Sleep(5000);
            cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// 透過cancellationTokenSource設定Task被cancel掉所要執行的delegate
        /// 
        /// 測試呼叫 Cancel() & Cancel(bool throwOnFirstException)
        /// 呼叫 Cancel() 會執行 Cancel(false)
        /// 該boolean參數 用來判斷當有Register被Cancel掉要執行的動作噴Exception時
        /// 若為是，代表直接噴Exception，不管其餘動作是否執行完畢。
        /// 若為否，則會執行完其餘Register的動作之後才會噴Exception。
        /// </summary>
        public static void Cancel_Register()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            // 註冊的動作是LIFO，最晚註冊的開始執行
            cancellationTokenSource.Token.Register(() => Console.WriteLine("Task Canceled1"));
            cancellationTokenSource.Token.Register(() => Console.WriteLine("Task Canceled2"));
            cancellationTokenSource.Token.Register(() => Console.WriteLine("Task Canceled3"));
            cancellationTokenSource.Token.Register(() => Console.WriteLine("Task Canceled4"));
            cancellationTokenSource.Token.Register(() => Console.WriteLine("Task Canceled5"));
            cancellationTokenSource.Token.Register(() => throw new Exception("Task Canceled"));

            try
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        Console.WriteLine($"Task Running!{cancellationTokenSource.IsCancellationRequested}");
                    }
                }, cancellationTokenSource.Token);

                Thread.Sleep(5000);
                cancellationTokenSource.Cancel();
            }
            catch(Exception ex)
            {

            }
            
        }

        /// <summary>
        /// 透過cancellationTokenSource設定Task幾秒後會cancel掉
        /// 
        /// 測試呼叫 CancelAfter(int millisecondsDelay) & CancelAfter(TimeSpan delay)
        /// millisecondsDelay 與 delay，代表多久要取消該Task
        /// </summary>
        public static void CancelAfter()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    Console.WriteLine($"Task Running!{cancellationTokenSource.IsCancellationRequested}");
                }
            }, cancellationTokenSource.Token);

            //cancellationTokenSource.CancelAfter(3000);
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// 測試呼叫 Dispose
        /// 當CancellationTokenSource被Dispose掉，代表先前設定的Register和Cancel Timer都被清除
        /// </summary>
        public static void Dispose()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    Console.WriteLine($"Task Running!{cancellationTokenSource.IsCancellationRequested}");
                }
            }, cancellationTokenSource.Token);

            cancellationTokenSource.Token.Register(() => Console.WriteLine("Task Canceled"));

            // 2秒後Cancel掉Task
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

            // 1秒後dispose掉CancellationTokenSource，清除掉CancelAfter的timer和Register動作
            Thread.Sleep(1000);
            cancellationTokenSource.Dispose();

            // 被dispose掉後就無法再調用，會噴Exception
            //cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));
        }
    }
}