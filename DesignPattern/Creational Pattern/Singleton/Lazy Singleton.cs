using System;
using System.Threading;

namespace DesignPattern.Lazy_Singleton
{
    /// <summary>
    /// 使用.net 4.0 新增的Lazy<T>類別來實現
    /// 可以避免當其他static member被使用，使得instance被建立起來
    /// 
    /// thread safe
    /// </summary>
    public class Lazy_Singleton : IExecute
    {
        public void main()
        {
            Console.WriteLine(LazySingletonSample.StaticTestString);

            Console.WriteLine($"process start ， {DateTime.Now}");

            Thread.Sleep(2000);

            Console.WriteLine(LazySingletonSample.Instance.TestString);
        }
    }

    public class LazySingletonSample
    {
        public LazySingletonSample()
        {
            Console.WriteLine($"LazySingletonSample created ， {DateTime.Now}");
        }

        private static readonly Lazy<LazySingletonSample> lazyInstance = new Lazy<LazySingletonSample>();

        public static LazySingletonSample Instance => lazyInstance.Value;

        public string TestString { get; set; } = "Test";

        public static string StaticTestString { get; set; } = "StaticTest";
    }
}