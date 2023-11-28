using StackExchange.Redis;

namespace RedisTest
{
    /// <summary>
    /// Redis 功能測試
    /// </summary>
    public class FunctionTest
    {
        public void Main()
        {
            var configuration = new ConfigurationOptions()
            {
                EndPoints = { "localhost:6379" },
                AbortOnConnectFail = false,
                ConnectTimeout = 5000,
                SyncTimeout = 1000,
                ConnectRetry = 5,
                AllowAdmin = true
            };
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
            connectionMultiplexer.GetServer("localhost:6379").FlushAllDatabases();

            //StringTest(connectionMultiplexer);
            //HashTest(connectionMultiplexer);
            //ListTest(connectionMultiplexer);
            //SetTest(connectionMultiplexer);
            ZSetTest(connectionMultiplexer);
        }


        public void StringTest(IConnectionMultiplexer connectionMultiplexer)
        {
            connectionMultiplexer.GetDatabase().StringSet("Key1", "1");
            connectionMultiplexer.GetDatabase().StringSet("Key2", "2");

            var key1 = connectionMultiplexer.GetDatabase().StringGet("Key1");

            var keyList = connectionMultiplexer.GetServer("localhost:6379").Keys(pattern: "Key*").ToList();

            connectionMultiplexer.GetDatabase().KeyDelete("Key2");


            // 自動加1 (只能處理數字型別)
            connectionMultiplexer.GetDatabase().StringIncrement("Key1");

            // 減10
            connectionMultiplexer.GetDatabase().StringDecrement("Key1", 10);
        }

        public void HashTest(IConnectionMultiplexer connectionMultiplexer)
        {
            var hashEntrys1 = new List<HashEntry>
            {
                new HashEntry("T1", "1"),
                new HashEntry("T2", "2"),
            };

            connectionMultiplexer.GetDatabase().HashSet("Key1", hashEntrys1.ToArray());

            var hashEntrys2 = new List<HashEntry>
            {
                new HashEntry("T3", "3"),
                new HashEntry("T4", "4"),
                new HashEntry("T4", "5"),
            };

            // Hash 重複設定相同 Key，不會噴錯
            // 但key 裡面的欄位會疊加更新上去，若欄位有重複，則後寫的欄位會覆蓋舊欄位的值
            connectionMultiplexer.GetDatabase().HashSet("Key1", hashEntrys2.ToArray());

            connectionMultiplexer.GetDatabase().HashSet("Key2", hashEntrys2.ToArray());

            var key1_t1 = connectionMultiplexer.GetDatabase().HashGet("Key1", "T1");
            var key2_t3 = connectionMultiplexer.GetDatabase().HashGet("Key2", "T3");

            var key1 = connectionMultiplexer.GetDatabase().HashGetAll("Key1");

            connectionMultiplexer.GetDatabase().HashDelete("Key2", "T4");
        }

        public void ListTest(IConnectionMultiplexer connectionMultiplexer)
        {
            var list = new List<string>() { "1", "2", "3", "4", "5" };

            // ListRightPush = 依序寫入，第一筆為 1，第五筆為 5
            list.ForEach(x => connectionMultiplexer.GetDatabase().ListRightPush("Key1", x));

            // 指定 Index 可插入到 List 中
            connectionMultiplexer.GetDatabase().ListSetByIndex("Key1", 1, "插入");

            // ListLeftPush = 插隊，排到第一個
            connectionMultiplexer.GetDatabase().ListLeftPush("Key1", "後面123");

            // 從最後一筆開始取，取得 5，取完就被 Pop 出來
            //var key1Data = connectionMultiplexer.GetDatabase().ListRightPop("Key1");

            // 從第一筆開始取，取得 後面123，取完就被 Pop 出來
            //var key1Data2 = connectionMultiplexer.GetDatabase().ListLeftPop("Key1");

            // 取得陣列範圍 0~3 的資料，只單純取得數值，資料仍在
            var key1Range = connectionMultiplexer.GetDatabase().ListRange("Key1", 0, 3);
        }

        /// <summary>
        /// Set 裡不會有重複資料
        /// </summary>
        public void SetTest(IConnectionMultiplexer connectionMultiplexer)
        {
            connectionMultiplexer.GetDatabase().SetAdd("Key1", "1");
            connectionMultiplexer.GetDatabase().SetAdd("Key1", "1"); // 重複資料不會處理

            // 集合內資料是否存在
            var isContains = connectionMultiplexer.GetDatabase().SetContains("Key1", "1");

            // 集合長度
            var setLength = connectionMultiplexer.GetDatabase().SetLength("Key1");

            connectionMultiplexer.GetDatabase().SetAdd("Key2", "2");

            // 將 Key1 集合的 "1" 搬到 Key2 集合裡
            connectionMultiplexer.GetDatabase().SetMove("Key1", "Key2", "1");

            // 取得集合所有資料
            var temp = connectionMultiplexer.GetDatabase().SetMembers("Key2");

            // 從集合取最後一筆資料出來
            var pop = connectionMultiplexer.GetDatabase().SetPop("Key2");

            for (int i = 0; i < 10; i++)
                connectionMultiplexer.GetDatabase().SetAdd("TestKey", $"test_{i}");

            var setScanList = connectionMultiplexer.GetDatabase().SetScan("TestKey", pattern: "test_*").ToList();
        }

        /// <summary>
        /// ZSet 裡不會有重複資料
        /// Redis 透過分數為集合成員做排序
        /// </summary>
        public void ZSetTest(IConnectionMultiplexer connectionMultiplexer)
        {
            connectionMultiplexer.GetDatabase().SortedSetAdd("Key1", "1", 1);
            connectionMultiplexer.GetDatabase().SortedSetAdd("Key1", "2", 100);
            connectionMultiplexer.GetDatabase().SortedSetAdd("Key1", "3", 50);

            var rank1 = connectionMultiplexer.GetDatabase().SortedSetRangeByRank("Key1", order: Order.Ascending);
            var rank2 = connectionMultiplexer.GetDatabase().SortedSetRangeByRank("Key1", order: Order.Descending);
            var score = connectionMultiplexer.GetDatabase().SortedSetRangeByScore("Key1");

            var data = connectionMultiplexer.GetDatabase().SortedSetRangeByRankWithScores("Key1");
        }
    }
}