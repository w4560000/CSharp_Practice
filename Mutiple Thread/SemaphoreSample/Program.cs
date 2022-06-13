using System;
using System.Threading;

namespace SemaphoreSample
{
    /// <summary>
    /// MSDN https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.semaphore
    /// 參考文章 https://www.uj5u.com/net/227011.html
    /// 
    /// Semaphore 可以透過 name 來跨應用程式取得 Semaphore 實體，SemaphoreSlim 則只限於單一應用程式
    /// </summary>
    internal class Program
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
        static Semaphore semaphore = new Semaphore(5, 5);

        static void Main(string[] args)
        {
            for (int i = 1; i <= 12; i++)
            {
                Thread td = new Thread(new ParameterizedThreadStart(TestFun));
                td.Start($"編號{i}");
            }
            Console.ReadKey();
        }
        public static void TestFun(object obj)
        {
            // 進洗手間 消耗一個廁所
            semaphore.WaitOne();
            Console.WriteLine(">>>>>" + obj.ToString() + "進洗手間：" + DateTime.Now.ToString());
            Thread.Sleep(2000);

            // 出洗手間 空出一個廁所
            Console.WriteLine(obj.ToString() + "出洗手間：" + DateTime.Now.ToString());
            semaphore.Release();
        }
    }
}
