using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string url = "http://localhost:2000/Account/Test";

            List<string> result = new List<string>();

            var task = Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    result.Add(await Post(url));
                }
            });

            Task.WaitAll(task);

            Console.WriteLine($"mobileCount:{result.Where(x => x.Contains("MOBILEWEB")).Count()}, webCount:{result.Where(x => x.Contains("PCWEB")).Count()}");
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="uri">uri</param>
        /// <returns>response</returns>
        public static async Task<string> Post(string url)
        {
            using HttpClient client = new HttpClient();
            //string response = client.GetAsync(url).GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult();

            HttpResponseMessage httpResponseMessage = await client.GetAsync(url);
            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            string result = JsonConvert.DeserializeObject<dynamic>(response).result;

            return result;
        }
    }
}