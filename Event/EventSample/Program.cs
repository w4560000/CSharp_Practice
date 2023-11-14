using System;
using EventSample.Sapmle3;

namespace EventSample
{
    /// <summary>
    /// 
    /// </summary>
    internal class Program
    {
        public static void Main()
        {
            //TestDelegate();

            //new Sample1().Run();

            new Sample2().Run();

            //new Sample3().Run();

            Console.Read();
        }

        public static void TestDelegate()
        {
            //1.普通委托
            var print1 = new Print(delegatemethod);
            print1("普通委託");

            //2.匿名委托
            Print print2 = delegate (string str) { Console.WriteLine(str); };
            print2("匿名委託");

            //3.lambda委托
            Print print3 = (string str) => Console.WriteLine(str);
            print3("lambda委託");
        }

        public delegate void Print(string str);

        private static void delegatemethod(string str)
        {
            Console.WriteLine(str);
        }
    }
}