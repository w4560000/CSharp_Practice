using System;
using System.Threading;
using CommonClassLibary;

namespace DesignPattern.Singleton
{
    /// <summary>
    /// 定義:
    /// 單例對象的類必須保證只有一個實例存在。許多時候整個系統只需要擁有一個的全局對象，這樣有利於我們協調系統整體的行為。
    /// 
    /// 該類別的Construct設為Private，可避免被直接new出instance
    /// 該類別設定sealed，可避免被繼承，若被繼承，子類別有可能會被誤用到該Singleton的功能。
    /// thread safe
    /// 
    /// 缺點:
    /// 若該類別有其他static member被引用，則instance會被建立起來
    /// </summary>
    public class Singleton : IExecute
    {
        public void Main()
        {
            string a = SingletonSample.StaticTestString;

            Console.WriteLine($"process start ， {DateTime.Now}");

            Thread.Sleep(2000);

            Console.WriteLine(SingletonSample.Instance.TestString);
        }
    }

    public sealed class SingletonSample
    {
        private SingletonSample()
        {
            Console.WriteLine($"SingletonSample class init  ， {DateTime.Now}");
        }

        public static SingletonSample Instance { get; } = new SingletonSample();

        public string TestString { get; set; } = "Test";

        public static string StaticTestString { get; set; } = "StaticTest";
    }
}