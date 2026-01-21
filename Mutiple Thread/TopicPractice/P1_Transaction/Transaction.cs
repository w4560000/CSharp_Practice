using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicPractice.P1_Transaction
{
    /// <summary>
    /// https://columns.chicken-house.net/2018/03/25/interview01-transaction/
    /// </summary>
    public class Transaction
    {
        public void Run()
        {
            // skip DI, 建立指定的 account 實做機制
            AccountBase bank = new LockAccount();
            //AccountBase bank = new WithoutLockAccount(); // LockAccount 移除 Lock 版本
            //AccountBase bank = new TransactionAccount() { Name = "andrew" }; // 透過 DB Transaction機制來達成 (可處理多機、多執行續) 但瓶頸會在 DB
            //AccountBase bank = new DistributedLockAccount() { Name = "andrew" }; // 透過 Redis _redlock 完成 (分布式鎖，可處理多機、多執行續)


            long concurrent_threads = 3;
            long repeat_count = 1000;
            decimal origin_balance = bank.GetBalance();

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < concurrent_threads; i++)
            {
                Thread t = new Thread(() =>
                {
                    for (int j = 0; j < repeat_count; j++)
                        bank.ExecTransaction(1);
                });
                threads.Add(t);
            }

            Stopwatch timer = new Stopwatch();

            timer.Restart();
            foreach (Thread t in threads) t.Start();
            foreach (Thread t in threads) t.Join();


            decimal expected_balance = origin_balance + concurrent_threads * repeat_count;
            decimal actual_balance = bank.GetBalance();

            Console.WriteLine("Test Result for {1}: {0}!", (expected_balance == actual_balance) ? ("PASS") : ("FAIL"), bank.GetType().Name);
            Console.WriteLine($"- Expected Balance: {expected_balance}");
            Console.WriteLine($"- Actual Balance: {actual_balance}");
            Console.WriteLine($"- Performance: {concurrent_threads * repeat_count * 1000 / timer.ElapsedMilliseconds} trans/sec");
        }
    }
}
