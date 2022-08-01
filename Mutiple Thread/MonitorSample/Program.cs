using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = new MonitorTest();
            //for (int i = 1; i <= 5; i++)
            //{
            //    int index = i;
            //    Task.Run(() =>
            //    {
            //        a.func_monitor($"編號{index}");
            //    });
            //}

            for (int i = 1; i <= 5; i++)
            {
                int index = i;
                Task.Run(() =>
                {
                    a.func_lock($"編號{index}");
                });
            }

            Console.ReadKey();
        }
    }

    public class MonitorTest
    {
        private object _lock = new object();

        public void func_monitor(object o)
        {
            Monitor.Enter(this);
            Monitor.Pulse
            try
            {
                Console.WriteLine($"{DateTime.Now} start-" + o);
                Thread.Sleep(3000);
            }
            finally
            {
                Console.WriteLine($"{DateTime.Now} done-" + o);
                Monitor.Exit(this);
            }
        }

        public void func_lock(object o)
        {
            lock(_lock)
            {
                Console.WriteLine($"{DateTime.Now} start-" + o);
                Thread.Sleep(2000);
            }
            Console.WriteLine($"{DateTime.Now} done-" + o);
        }
    }
}
