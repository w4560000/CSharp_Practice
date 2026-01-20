using System;
using System.Threading;

namespace SemaphoreSample
{
    // 目前洗手間共有五間廁所
    // new Semaphore(5, 5) = 一開始五個空位置, 最多五個位置

    // 假設目前有 12 個人要上廁所
    // 第一波 因為有五個空廁所 => 五個人都可以上
    // 第二波 兩秒過後 => 前五個人都上完廁所 => 再補上五個人上廁所
    // 第三波 兩秒過後 => 剩餘兩人可以上廁所

    // output:
    //  >>>>>編號4進洗手間：2022/5/16 下午 02:12:33
    //  >>>>>編號8進洗手間：2022/5/16 下午 02:12:33
    //  >>>>>編號10進洗手間：2022/5/16 下午 02:12:33
    //  >>>>>編號11進洗手間：2022/5/16 下午 02:12:33
    //  >>>>>編號12進洗手間：2022/5/16 下午 02:12:33
    //  編號8出洗手間：2022/5/16 下午 02:12:35
    //  編號4出洗手間：2022/5/16 下午 02:12:35
    //  編號10出洗手間：2022/5/16 下午 02:12:35
    //  編號11出洗手間：2022/5/16 下午 02:12:35
    //  編號12出洗手間：2022/5/16 下午 02:12:35
    //  >>>>>編號2進洗手間：2022/5/16 下午 02:12:35
    //  >>>>>編號1進洗手間：2022/5/16 下午 02:12:35
    //  >>>>>編號3進洗手間：2022/5/16 下午 02:12:35
    //  >>>>>編號5進洗手間：2022/5/16 下午 02:12:35
    //  >>>>>編號6進洗手間：2022/5/16 下午 02:12:35
    //  編號3出洗手間：2022/5/16 下午 02:12:37
    //  編號2出洗手間：2022/5/16 下午 02:12:37
    //  編號1出洗手間：2022/5/16 下午 02:12:37
    //  編號5出洗手間：2022/5/16 下午 02:12:37
    //  編號6出洗手間：2022/5/16 下午 02:12:37
    //  >>>>>編號9進洗手間：2022/5/16 下午 02:12:38
    //  >>>>>編號7進洗手間：2022/5/16 下午 02:12:38
    //  編號9出洗手間：2022/5/16 下午 02:12:40
    //  編號7出洗手間：2022/5/16 下午 02:12:40
    internal class ThreadTest
    {
        private Semaphore semaphore = new Semaphore(2, 10);

        public void StartThread()
        {
            for (int i = 1; i <= 12; i++)
            {
                Thread td = new Thread(new ParameterizedThreadStart(TestFun));
                td.Start($"編號{i}");
            }

            // cross thread 鎖、解鎖 (測試 由不同thread鎖、再由不同thread開鎖) 測試正常
            // Semaphore 的鎖、解鎖 可由不同thread 執行
            //for (int i = 1; i <= 12; i++)
            //{
            //    Thread td = new Thread(new ParameterizedThreadStart(Test1));
            //    td.Start($"編號{i}");
            //}

            //for (int i = 13; i <= 20; i++)
            //{
            //    Thread td = new Thread(new ParameterizedThreadStart(Test2));
            //    td.Start($"編號{i}");
            //}
        }

        public void TestFun(object obj)
        {
            // 進洗手間 消耗一個廁所
            semaphore.WaitOne();
            Console.WriteLine(">>>>>" + obj.ToString() + "進洗手間：" + DateTime.Now.ToString());
            Thread.Sleep(2000);

            // 出洗手間 空出一個廁所
            Console.WriteLine(obj.ToString() + "出洗手間：" + DateTime.Now.ToString());
            semaphore.Release();
        }

        public void Test1(object obj)
        {
            // 進洗手間 消耗一個廁所
            semaphore.WaitOne();
            Console.WriteLine(">>>>>" + obj.ToString() + "鎖起來：" + DateTime.Now.ToString());
        }

        public void Test2(object obj)
        {
            // 進洗手間 消耗一個廁所
            semaphore.Release();
            Console.WriteLine(obj.ToString() + "開鎖：" + DateTime.Now.ToString());
        }
    }
}