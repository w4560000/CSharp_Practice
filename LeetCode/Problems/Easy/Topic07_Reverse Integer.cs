using LeetCode.Interface;
using System;
using System.Linq;

namespace LeetCode.Problems
{
    public class Topic07_Reverse_Integer : IEntry
    {
        public void Main()
        {
            Console.WriteLine(Reverse(-123));
        }

        public int Reverse(int x)
        {
            int num = 0;

            while (x != 0)
            {
                int a = x % 10;
                num = a + num * 10;
                x /= 10;
            }
            return (num > int.MaxValue || num < int.MinValue) ? 0 : Convert.ToInt32(num);
        }

        public int Reverse一行解(int x)
        {
            // ex x = 123

            // Aggregate : 
            // loop1: 0 * 10 + 3
            // loop2: 3 * 10 + 2
            // loop3: 32 * 10 + 1

            // result = 321
            return Math.Abs(x).ToString().Reverse().Aggregate(0, (x, next) => x * 10 + next - '0') * Math.Sign(x);
        }
    }
}