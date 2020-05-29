using LeetCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode.Problems.Easy
{
    public class Topic13_Roman_to_Integer : IEntry
    {
        public void Main()
        {
            Console.WriteLine(RomanToInt("MCMXCIV"));
        }

        public int RomanToInt(string s)
        {
            Dictionary<char, int> dictionary = new Dictionary<char, int>()
            {
                {'I', 1},
                {'V', 5},
                {'X', 10},
                {'L', 50},
                {'C', 100},
                {'D', 500},
                {'M', 1000}
            };

            int number = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (i + 1 < s.Length && dictionary[s[i]] < dictionary[s[i + 1]])
                {
                    number -= dictionary[s[i]];
                }
                else
                {
                    number += dictionary[s[i]];
                }
            }
            return number;
        }
    }
}