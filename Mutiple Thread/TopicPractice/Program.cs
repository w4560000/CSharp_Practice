using TopicPractice.BlockQueue;
using TopicPractice.P1_Transaction;

namespace TopicPractice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // BlockQueueTest.Run();

            new Transaction().Run();
            Console.ReadKey();
        }
    }
}
