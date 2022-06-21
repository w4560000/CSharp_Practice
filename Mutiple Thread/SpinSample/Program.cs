using System;
using System.Diagnostics;
using System.Threading;

namespace SpinSample
{
    /// <summary>
    /// 
    /// </summary>
    internal class Program
    {

        private static void Main(string[] args)
        {
            new SpinWaitSample().Run();
            Console.ReadLine();
        }
    }
}