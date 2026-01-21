using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicPractice.BlockQueue
{
    /// <summary>
    /// https://columns.chicken-house.net/2008/10/18/producer-vs-consumer-blockqueue-implementation/
    /// 
    /// 5個 Producer => 各生產 30 個item => 共 150 個
    /// 10個 Consumer => 
    /// </summary>
    public class BlockQueueTest
    {
        public static BlockQueue<string> queue = new BlockQueue<string>(10);

        public static void Run()
        {
            List<Thread> ps = new List<Thread>();
            List<Thread> cs = new List<Thread>();
            for (int index = 0; index < 5; index++)
            {
                Thread t = new Thread(Producer);
                ps.Add(t);
                t.Start();
            }
            for (int index = 0; index < 10; index++)
            {
                Thread t = new Thread(Consumer);
                cs.Add(t);
                t.Start();
            }
            foreach (Thread t in ps)
            {
                t.Join();
            }
            WriteLine("Producer shutdown. ");
            queue.Shutdown();
            foreach (Thread t in cs)
            {
                t.Join();
            }
        }

        public static long sn = 0;

        public static void Producer()
        {
            for (int count = 0; count < 30; count++)
            {
                RandomWait();
                string item = string.Format("item:{0} ", Interlocked.Increment(ref sn));
                WriteLine("Produce Item: {0} ", item);
                queue.EnQueue(item);
            }
            WriteLine("Producer Exit ");
        }

        public static void Consumer()
        {
            try
            {
                while (true)
                {
                    RandomWait();
                    string item = queue.DeQueue();
                    WriteLine("Cust Item: {0} ", item);
                }
            }
            catch
            {
                WriteLine("Consumer Exit ");
            }
        }

        private static void RandomWait()
        {
            Random rnd = new Random();
            Thread.Sleep((int)(rnd.NextDouble() * 300));
        }

        private static void WriteLine(string patterns, params object[] arguments)
        {
            Console.WriteLine(string.Format("[#{0:D02}]  ", Thread.CurrentThread.ManagedThreadId) + patterns, arguments);
        }
    }
}