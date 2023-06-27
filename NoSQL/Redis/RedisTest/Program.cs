using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace RedisTest
{
    public class RedisConnectionManager
    {
        private readonly object _lock = new object();
        private IConnectionMultiplexer _connectionMultiplexer;
        private readonly ConfigurationOptions _configurationOptions;
        private readonly RetryPolicy _retryPolicy;

        public RedisConnectionManager(ConfigurationOptions configurationOptions, RetryPolicy retryPolicy)
        {
            _configurationOptions = configurationOptions;
            _retryPolicy = retryPolicy;
        }

        public IConnectionMultiplexer GetConnection()
        {
            return _retryPolicy.Execute(() =>
            {
                if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
                {
                    lock (_lock)
                    {
                        _connectionMultiplexer?.Close();
                        _connectionMultiplexer = ConnectionMultiplexer.Connect(_configurationOptions);
                    }
                }

                return _connectionMultiplexer;
            });
        }

        public T? Get<T>(string key)
        {
            return _retryPolicy.Execute(() =>
            {
                try
                {
                    var value = _connectionMultiplexer.GetDatabase().StringGet(key);

                    if (value.IsNullOrEmpty)
                        return default;

                    return JsonConvert.DeserializeObject<T>(value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Get 發生錯誤, Error:{ex.Message}");
                    throw ex;
                }
            });
        }

        public void Update(string key, string data)
        {
            _retryPolicy.Execute(() =>
            {
                try
                {
                    _connectionMultiplexer.GetDatabase().StringSet(key, data);
                    Console.WriteLine("已更新");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Update 發生錯誤, Error:{ex.Message}");
                    throw ex;
                }
            });
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var retryPolicy = Policy.Handle<RedisConnectionException>()
                                        .Or<RedisTimeoutException>()
                                        .Retry(3, (exception, retryCount) =>
                                        {
                                            Console.WriteLine($"Redis connection failed. Retrying ({retryCount})...");
                                        });

                var configuration = new ConfigurationOptions()
                {
                    EndPoints = { "34.80.100.152:6379", "35.221.204.1:6379", "35.229.217.216:6379" },
                    AbortOnConnectFail = false,
                    ConnectTimeout = 10000,
                    SyncTimeout = 3000,
                    ConnectRetry = 5
                };

                var redisConnectionManager = new RedisConnectionManager(configuration, retryPolicy);
                var redisConnection = redisConnectionManager.GetConnection();

                while (true)
                {
                    Console.WriteLine($"是否已連接: {redisConnection.IsConnected}");
                    var value = redisConnectionManager.Get<string>("Key1");
                    var newValue = Convert.ToInt32(value) + 1;

                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 預計更新為 {newValue}");
                    redisConnectionManager.Update("Key1", newValue.ToString());
                    Console.WriteLine($"更新後確認 {redisConnectionManager.Get<string>("Key1")}\n");
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}