using System.Threading.Channels;

namespace ChannelSample
{
    /// <summary>
    /// 生產者/消費者 通道
    /// CreateBounded = 有數據上限通道
    /// CreateUnbounded = 無數據上限通道
    /// 
    /// BoundedChannelOptions.FullMode
    /// - Wait (等待直到有空間)
    /// - DropWrite (丟棄當前寫入的數據)
    /// - DropNewest (丟棄最新寫入的數據) (把通道內最新的丟棄，塞入當前寫入的數據)
    /// - DropOldest (丟棄最舊的數據)
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await SingleReader();
            Console.WriteLine("Hello, World!");
        }

        private static async Task SingleReader()
        {
            //建立1
            var _channel = Channel.CreateBounded<int>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait, //等待寫完
                SingleReader = true //保證一次讀取一個
            });

            // Producer（寫很快）
            var producer = Task.Run(async () =>
            {
                for (int i = 1; i <= 3000; i++)
                {
                    await _channel.Writer.WriteAsync(i);
                    Console.WriteLine($"[WRITE ] {i}");
                    await Task.Delay(10); // 很快
                }

                _channel.Writer.Complete();
            });

            // Consumer（讀很慢）
            var consumer = Task.Run(async () =>
            {
                await foreach (var item in _channel.Reader.ReadAllAsync())
                {
                    Console.WriteLine($"        [READ  ] {item}");
                    await Task.Delay(50); // 很慢
                }
            });

            // Check（檢查）
            var Check = Task.Run(async () =>
            {
                while (true)
                {
                    var count = _channel.Reader.Count;
                    Console.WriteLine($"                [Check] 目前可讀取數量: {count}");

                    await Task.Delay(500);
                }
            });

            await Task.WhenAll(producer, consumer, Check);

            Console.WriteLine("Done");
        }
    }
}
