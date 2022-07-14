using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InterlockedSample
{
    /// <summary>
    /// Source Code: https://blog.csdn.net/ylq1045/article/details/108983401
    /// </summary>
    internal class RedEnvelopeTest
    {
        public void RunTest()
        {
            int currentIndex = 0;//當前索引
            int totalAmount = 10000;//紅包總金額【10000】
            int count = 10;//搶紅包人數
            Console.WriteLine($"...紅包總金額【{totalAmount}】,搶紅包人數【{count}】,開始模擬搶紅包...");
            List<Task> taskCollection = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                taskCollection.Add(Task.Factory.StartNew(() =>
                {
                    GrabGift(totalAmount, count, ref currentIndex);
                }));
            }
            //等待所有任務完成
            Task.WaitAll(taskCollection.ToArray());
        }

        /// <summary>
        /// 用于排他锁,确保在多线程调用接口时，不会同时调用
        /// </summary>
        private static int lockedFlag = 0;

        /// <summary>
        /// 每个人获取红包的最小值，每个人的收到的红包不能小于这个值
        /// </summary>
        private const int giftMin = 100;//随机礼物的最小值,要分割的红包的最小值

        /// <summary>
        /// 将已抢红包放入列表
        /// </summary>
        private static List<int> list = new List<int>();

        /// <summary>
        /// 抢红包
        /// 限定 lockedFlag 為 0 時 才能搶紅包 (一次允許一個 Thread 搶紅包)
        /// </summary>
        /// <param name="totalAmount">红包金额总数</param>
        /// <param name="count">要抢红包的总人数</param>
        /// <param name="currentIndex">当前抢红包的人员索引，从0开始</param>
        private static void GrabGift(int totalAmount, int count, ref int currentIndex)
        {
            // 添加锁:如果初始值不为零，就一直等待
            // lockedFlag 初始值為0 ,在此迴圈判斷為 0 ,可繼續執行並賦值為1 ,而其他 Thread 因迴圈判斷為 1 Lock 在此迴圈中

            // Interlocked.Exchange(ref lockedFlag, 1) => 將 lockedFlag 賦值為1，並回傳 lockedFlag 原始值
            // ex: lockedFlag 初始值 = 0
            //     Interlocked.Exchange(ref lockedFlag, 1) 回傳 0，而 lockedFlag 變為 1
            while (Interlocked.Exchange(ref lockedFlag, 1) != 0)
            {
                Thread.Sleep(20);
            }

            //前面（N-1）个随机处理,为了保证每个人至少抢一元钱【giftMin】。因此随机数的最大值为{总数-已经随机后的总和-(count - i) * giftMin}
            int sum = 0;
            int currentAmount = 0;
            Random random = new Random(Guid.NewGuid().GetHashCode());
            if (currentIndex < count - 1)
            {
                Thread.Sleep(random.Next(100, 300 + currentIndex));
                sum = list.Sum();
                int maxValue = totalAmount - sum - (count - currentIndex) * giftMin;
                if (giftMin > maxValue)
                {
                    //释放锁
                    Interlocked.Exchange(ref lockedFlag, 0);
                    throw new Exception($"搶紅包算法邏輯錯誤，請合理規劃：每人獲得紅包的最小值【{giftMin}】，當前可搶紅包金額【{maxValue}】");
                }
                currentAmount = random.Next(giftMin, maxValue);
                Console.WriteLine($"當前第【{currentIndex + 1}】人,搶到紅包【{currentAmount}】");
                list.Add(currentAmount);
                currentIndex++;
            }
            else
            {
                //最后一个:直接是剩余的红包金额
                sum = list.Sum();
                currentAmount = totalAmount - sum;
                Console.WriteLine($"當前第【{currentIndex + 1}】人,搶到紅包【{currentAmount}】");
                list.Add(currentAmount);
            }
            //释放锁，将初始值置0
            //lockedFlag 賦值為0 (讓其他 Thread 搶紅包)
            Interlocked.Exchange(ref lockedFlag, 0);
        }
    }
}