using System;

namespace InterlockedSample
{
    internal class Program
    {
        /// <summary>
        /// Source Code: https://blog.csdn.net/ylq1045/article/details/108983401
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // 搶紅包測試
            //new RedEnvelopeTest().RunTest();

            new InterlockedTest().RunTest();
            Console.ReadLine();
        }
    }
}