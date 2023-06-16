﻿using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisTest
{
    internal class Program
    {
        static IConnectionMultiplexer conn = ConnectionMultiplexer.Connect(new ConfigurationOptions()
        {
            EndPoints = { "104.199.218.114:6379" },
            AbortOnConnectFail = false
        });

        static void Main(string[] args)
        {
            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    conn.GetDatabase().StringSet($"Key{i}", i.ToString());

                    Console.WriteLine($"已更新 Key{i} 為 {i}");

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }

        private static T? Get<T>(string key)
        {
            var value = conn.GetDatabase().StringGet(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonConvert.DeserializeObject<T>(value);
        }

        private static void Update(string key, string data)
        {
            conn.GetDatabase().StringSet(key, data);
        }


        private static void Delete(string key)
        {
            conn.GetDatabase().KeyDelete(key);
        }
    }
}