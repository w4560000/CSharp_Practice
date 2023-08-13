using Newtonsoft.Json;
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
                //var configString = "35.194.230.192:6379";
                //ConfigurationOptions options = ConfigurationOptions.Parse(configString);
                //var conn = ConnectionMultiplexer.Connect(options);
                //var a = conn.GetDatabase().StringGetAsync("test3").Result;

                //CSRedisClient client = new CSRedisClient("35.194.230.192:6379");
                //var a = client.Get("Key1");
                //return;

                //var retryPolicy = Policy.Handle<RedisConnectionException>()
                //                        .Or<RedisTimeoutException>()
                //                        .Retry(3, (exception, retryCount) =>
                //                        {
                //                            Console.WriteLine($"Redis connection failed. Retrying ({retryCount})...");
                //                        });

                var configuration = new ConfigurationOptions()
                {
                    EndPoints = {
                        { "35.194.230.192:6379" },
                        //{ "34.81.254.51:6379" },
                        //{ "35.185.134.146:6379" },
                        //{ "34.81.74.195:6379" },
                        //{ "34.81.147.95:6379" },
                        //{ "35.201.133.198:6379" }
                    },
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000,
                    SyncTimeout = 3000,
                    ConnectRetry = 3,
                };

                using (TextWriter log = File.CreateText("D:\\redis_log.txt"))
                {
                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Connect Start");
                    var redisConnectionManager = ConnectionMultiplexer.Connect(configuration, log);
                    Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " Connect End");

                    try
                    {
                        var key1 = redisConnectionManager.GetDatabase().StringGet("Key1");
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + $" Key1:{key1}" + "\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + $" GetKey Error {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + $" Error {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}