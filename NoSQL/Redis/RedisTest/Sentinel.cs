using StackExchange.Redis;

public class Sentinel
{
    private readonly object _lock = new object();
    private IConnectionMultiplexer _masterConnectionMultiplexer;
    private IConnectionMultiplexer _replicaConnectionMultiplexer;
    private IConnectionMultiplexer _sentinelConnectionMultiplexer;


    public void Run()
    {
        InitSentinelConnection();
        ResetConnection();

        while (true)
        {
            try
            {
                var value = _replicaConnectionMultiplexer.GetDatabase().StringGet("Key1");
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Key1 = {value}");
                var newValue = Convert.ToInt32(value) + 1;
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Key1 預計更新為 {newValue}");
                _masterConnectionMultiplexer.GetDatabase().StringSet("Key1", newValue.ToString());
                Console.WriteLine($"更新後確認 Key1 = {_replicaConnectionMultiplexer.GetDatabase().StringGet("Key1")}\n");

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + $" Error {ex.Message}");
            }
        }

        Console.ReadLine();
    }

    private void InitSentinelConnection()
    {
        var sentinelConfig = new ConfigurationOptions
        {
            EndPoints = {
                        { "34.80.222.88:26379" }
                    },
            AbortOnConnectFail = false,
            ServiceName = "mymaster",
            TieBreaker = string.Empty,
            CommandMap = CommandMap.Sentinel,
            AllowAdmin = true,
            Ssl = false,
            ConnectTimeout = 1000,
            SyncTimeout = 1000,
            ConnectRetry = 5,
        };

        _sentinelConnectionMultiplexer = ConnectionMultiplexer.SentinelConnect(sentinelConfig);
        ISubscriber subscriber = _sentinelConnectionMultiplexer.GetSubscriber();

        subscriber.Subscribe("+switch-master", (channel, message) =>
        {
            Console.WriteLine($"Channel: {channel}, Message: {message}");
            ResetConnection();
        });

        Console.WriteLine("哨兵連線成功");
    }

    private void ResetConnection()
    {
        var endPoint = _sentinelConnectionMultiplexer.GetEndPoints().First();
        var server = _sentinelConnectionMultiplexer.GetServer(endPoint);

        var masterEndPoint = server.SentinelGetMasterAddressByName("mymaster");
        var masterConfiguration = new ConfigurationOptions()
        {
            AbortOnConnectFail = false,
            ConnectTimeout = 1000,
            SyncTimeout = 1000,
            ConnectRetry = 5,
        };
        masterConfiguration.EndPoints.Add(masterEndPoint);
        _masterConnectionMultiplexer = ConnectionMultiplexer.Connect(masterConfiguration);


        var replicaEndPoint = server.SentinelGetReplicaAddresses("mymaster").AsEnumerable();
        var replicaConfiguration = new ConfigurationOptions()
        {
            AbortOnConnectFail = false,
            ConnectTimeout = 1000,
            SyncTimeout = 1000,
            ConnectRetry = 5,
        };

        replicaEndPoint.ToList().ForEach(x => masterConfiguration.EndPoints.Add(x));
        _replicaConnectionMultiplexer = ConnectionMultiplexer.Connect(masterConfiguration);


        Console.WriteLine($"MasterEndPoint: {masterEndPoint}, ReplicaEndPoint: {string.Join(',', replicaEndPoint)}");
    }
}