using LeetCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeetCode.Problems.Easy
{
    /// <summary>
    /// https://leetcode.com/problems/longest-common-prefix/
    /// </summary>
    public class Topic14_Longest_Common_Prefix : IEntry
    {
        public void Main()
        {
            Console.WriteLine(LongestCommonPrefixV3(new string[] { "flow", "flower", "flight" }));
        }

        public string LongestCommonPrefixV1(string[] strs)
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

        public string LongestCommonPrefixV2(string[] strs)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            var result = "";
            var length = strs.Min(str => str.Length);

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < strs.Length; j++)
                {
                    if (!dict.ContainsKey(j))
                        dict[j] = strs[j][i].ToString();
                    else
                        dict[j] += strs[j][i].ToString();
                }

                result = dict[0];
                if (dict.GroupBy(g => g.Value).Count() == 1)
                {
                    if (strs.Length == 1 || length == 1)
                        return result;

                    continue;
                }
                else
                    return result[..^1];
            }

            return result;
        }

        /// <summary>
        /// https://leetcode.com/problems/longest-common-prefix/solutions/2972955/c-using-string-with-min-length/
        /// </summary>
        public string LongestCommonPrefixV3(string[] strs)
        {
            int minLength = strs.Min(e => e.Length);
            string res = strs.First(e => e.Length == minLength);

            foreach (string str in strs)
            {
                for (int j = 0; j < minLength; j++)
                {
                    if (str[j] != res[j])
                    {
                        minLength = j; break;
                        if (minLength == 0) return "";
                    }
                }
            }

            return res.Substring(0, minLength);
        }
    }
}