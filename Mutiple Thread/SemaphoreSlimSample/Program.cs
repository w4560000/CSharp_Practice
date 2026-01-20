using System;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreSlimSample
{
    /// <summary>
    /// SemaphoreSlim 提供了非同步的 WaitAsync 方法、CancellationToken 參數
    /// 不能跨Process，但較輕量
    /// </summary>
    internal class Program
    {
        // 目前洗手間共有五間廁所
        // new SemaphoreSlim(5, 5) = 一開始五個空位置, 最多五個位置

        // 假設目前有 12 個人要上廁所
        // 第一波 因為有五個空廁所 => 五個人都可以上
        // 第二波 兩秒過後 => 前五個人都上完廁所 => 再補上五個人上廁所
        // 第三波 兩秒過後 => 剩餘兩人可以上廁所

        //  >>>>>編號5進洗手間：2022/6/13 下午 02:43:49
        //  >>>>>編號2進洗手間：2022/6/13 下午 02:43:49
        //  >>>>>編號3進洗手間：2022/6/13 下午 02:43:49
        //  >>>>>編號7進洗手間：2022/6/13 下午 02:43:49
        //  >>>>>編號8進洗手間：2022/6/13 下午 02:43:49
        //  編號7出洗手間：2022/6/13 下午 02:43:51
        //  編號3出洗手間：2022/6/13 下午 02:43:51
        //  編號5出洗手間：2022/6/13 下午 02:43:51
        //  編號8出洗手間：2022/6/13 下午 02:43:51
        //  編號2出洗手間：2022/6/13 下午 02:43:51
        //  >>>>>編號6進洗手間：2022/6/13 下午 02:43:51
        //  >>>>>編號1進洗手間：2022/6/13 下午 02:43:51
        //  >>>>>編號10進洗手間：2022/6/13 下午 02:43:51
        //  >>>>>編號9進洗手間：2022/6/13 下午 02:43:51
        //  >>>>>編號4進洗手間：2022/6/13 下午 02:43:51
        //  編號1出洗手間：2022/6/13 下午 02:43:53
        //  編號4出洗手間：2022/6/13 下午 02:43:53
        //  編號10出洗手間：2022/6/13 下午 02:43:53
        //  編號6出洗手間：2022/6/13 下午 02:43:53
        //  編號9出洗手間：2022/6/13 下午 02:43:53
        //  >>>>>編號11進洗手間：2022/6/13 下午 02:43:53
        //  >>>>>編號12進洗手間：2022/6/13 下午 02:43:53
        //  編號12出洗手間：2022/6/13 下午 02:43:55
        //  編號11出洗手間：2022/6/13 下午 02:43:55
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(5, 5);

        private static void Main(string[] args)
        {
            // 可傳入CancellationToken，當CancellationToken 狀態為取消時，semaphoreSlim wait時 會拋出異常
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Token.Register(() => Console.WriteLine($"Task Canceled"));
            for (int i = 1; i <= 12; i++)
            {
                int index = i;
                Task.Run(async () =>
                {
                    await TestFun($"編號{index}", cancellationTokenSource.Token);
                });
            }

            Console.ReadKey();
        }

        public static async Task TestFun(string name, CancellationToken cancellationToken)
        {
            try
            {
                // 進洗手間 消耗一個廁所
                await semaphoreSlim.WaitAsync(cancellationToken);
                Console.WriteLine(">>>>>" + name + "進洗手間：" + DateTime.Now.ToString());
                Thread.Sleep(3000);

                // 出洗手間 空出一個廁所
                Console.WriteLine(name + "出洗手間：" + DateTime.Now.ToString());
                semaphoreSlim.Release();
            }
            catch (Exception ex)
            {
                Console.WriteLine(name + ex.Message);
            }
        }
    }
}