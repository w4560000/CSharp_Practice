using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace RedisTest
{
    
    internal class Program
    {
        private static void Main(string[] args)
        {
            // 透過哨兵模式 取得 Master、Replica EndPoint
            // new Sentinel().Run();

            // 故障轉移測試
            // new FailoverTest().Run();

            Console.ReadKey();
        }
    }
}