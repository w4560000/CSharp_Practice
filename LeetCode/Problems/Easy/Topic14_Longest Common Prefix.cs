using LeetCode.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeetCode.Problems.Easy
{
    public class Topic14_Longest_Common_Prefix : IEntry
    {
        public void Main()
        {
            Console.WriteLine(LongestCommonPrefix(new string[] { "flower", "flow", "flight" }));
        }

        public string LongestCommonPrefix(string[] strs)
        {
            if (strs.Length == 0) return "";
            string prefix = strs[0];
            for (int i = 1; i < strs.Length; i++)
            {
                while (strs[i].IndexOf(prefix) != 0)
                {
                    prefix = prefix[0..^1];
                    if (string.IsNullOrEmpty(prefix)) return "";
                }
            }
            return prefix;
        }
    }
}
