using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Diagnostics;

namespace MongoDBConsole
{
    /// <summary>
    /// 參考
    /// https://ithelp.ithome.com.tw/articles/10186504
    /// </summary>
    internal class Program
    {
        private static MongoClient _mongoClient = new MongoClient("mongodb://10.10.10.10:27017");
        private static IMongoDatabase _database = _mongoClient.GetDatabase("SocialNetworkDB");
        private static IMongoCollection<Post> _postTable = _database.GetCollection<Post>("Post");

        private static void Main(string[] args)
        {
            try
            {
                // 塞假資料
                //InitFakeData();

                // 吐2萬筆資料測試
                // SearchPostByLee();

                var a1 = new Post();
                var a = _postTable.Find(f => f.Like == 660).ToList();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 10萬筆假資料 Author = Lee 的有2萬筆，搜尋耗時 約6~8秒
        /// SocialNetworkDB   13.85 MiB
        /// 第1次搜尋耗時:7288 ms
        /// 第2次搜尋耗時:6676 ms
        /// 第3次搜尋耗時:6320 ms
        /// 第4次搜尋耗時:6466 ms
        /// 第5次搜尋耗時:7470 ms
        /// 第6次搜尋耗時:7597 ms
        /// 第7次搜尋耗時:6913 ms
        /// 第8次搜尋耗時:6981 ms
        /// 第9次搜尋耗時:7392 ms
        /// 第10次搜尋耗時:7653 ms
        /// 總耗時:70758 ms
        /// 平均耗時:7075 ms
        /// </summary>
        private void SearchPostByLee()
        {
            Stopwatch sw = new Stopwatch();
            long total = 0;
            for (int i = 1; i <= 10; i++)
            {
                sw.Restart();
                var post = _postTable.Find(f => f.Author == "Lee").ToList();
                Console.WriteLine($"第{i}次搜尋耗時:{sw.ElapsedMilliseconds} ms");
                total += sw.ElapsedMilliseconds;
            }

            Console.WriteLine($"總耗時:{total} ms");
            Console.WriteLine($"平均耗時:{total / 10} ms");
        }

        private static void InitFakeData()
        {
            List<string> sentences = new List<string>(){
                  "hello world c#",
                  "hello world c++",
                  "hello world c",
                  "hello world java",
                  "hello world go"
            };

            List<string> users = new List<string>() { "Leo", "James", "Jason", "Lee", "Curry" };

            Post RandomPost()
            {
                return new Post()
                {
                    Text = sentences[new Random().Next(sentences.Count)],
                    Date = DateTime.Now,
                    Author = users[new Random().Next(users.Count)],
                    Like = new Random().Next(1000),
                    Messages = RandomMessages()
                };
            }

            List<Message> RandomMessages()
            {
                var messages = new List<Message>();

                for (int i = 0; i < new Random().Next(1, 50); i++)
                {
                    messages.Add(new Message()
                    {
                        MsgId = i,
                        Author = users[new Random().Next(users.Count)],
                        Msg = sentences[new Random().Next(sentences.Count)],
                        Date = DateTime.Now
                    });
                }

                return messages;
            }

            List<Post> fakeData = new List<Post>();
            for (int i = 0; i < 1000000; i++)
                fakeData.Add(RandomPost());

            _postTable.InsertMany(fakeData);
        }
    }

    public class Post
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public int Like { get; set; }
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        public int MsgId { get; set; }
        public string Author { get; set; }
        public string Msg { get; set; }
        public DateTime Date { get; set; }
    }
}